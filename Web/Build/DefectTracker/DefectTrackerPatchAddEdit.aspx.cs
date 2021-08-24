using System;
using System.IO;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DefectTracker;

public partial class DefectTrackerPatchAddEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbaredit = new PhoenixToolbar();
            toolbaredit.AddButton("Add/Edit", "ADDEDIT");
            toolbaredit.AddButton("Vessel List", "VESSELLIST");
            MenuPatchAdd.AccessRights = this.ViewState;
            MenuPatchAdd.MenuList = toolbaredit.Show();
            MenuPatchAdd.SelectedMenuIndex = 0;
            MenuPatchAdd.Visible = false;

            if (!IsPostBack)
            {
                ddlPatchProject.DataSource = PhoenixPatchTracker.PatchProjectList();
                ddlPatchProject.DataBind();

                if (Request.QueryString["projectdtkey"] != null)
                {
                    ViewState["PATCHPROJECTDTKEY"] = Request.QueryString["projectdtkey"].ToString();
                    ddlPatchProject.SelectedValue = Request.QueryString["projectdtkey"].ToString();
                }

                if (Request.QueryString["dtkey"] != null)
                {
                    MenuPatchAdd.Visible = true;
                    ViewState["PATCHDTKEY"] = Request.QueryString["dtkey"].ToString();
                }

                if (Request.QueryString["message"] != null)
                {
                    MenuPatchAdd.Visible = true;
                    ViewState["MESSAGEBODY"] = Request.QueryString["message"].ToString();
                }

                PhoenixToolbar toolbarsave = new PhoenixToolbar();
                toolbarsave.AddButton("Save", "SAVE");
                MenuPatchSave.AccessRights = this.ViewState;
                MenuPatchSave.MenuList = toolbarsave.Show();

                if (!IsPostBack)
                {
                    cblVessels.DataSource = PhoenixDefectTracker.ListVessel();
                    cblVessels.DataBind();
                    txtLinkFile.Text = "";
                }

                if (ViewState["PATCHDTKEY"] != null)
                    BindData();
                if (ViewState["PATCHDTKEY"] != null)
                    BindAttachments();

                BindProject();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdSelect_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in cblVessels.Items)
        {
            if (ddlCriteria.SelectedValue.Equals("LIKE"))
            {
                if (item.Text.ToUpper().Contains(txtFilter.Text.ToUpper().Trim()))
                {
                    item.Selected = true;
                }
            }

            if (ddlCriteria.SelectedValue.Equals("START"))
            {
                if (item.Text.ToUpper().StartsWith(txtFilter.Text.ToUpper().Trim()))
                {
                    item.Selected = true;
                }
            }

            if (ddlCriteria.SelectedValue.Equals("END"))
            {
                if (item.Text.ToUpper().EndsWith(txtFilter.Text.ToUpper().Trim()))
                {
                    item.Selected = true;
                }
            }
        }
    }


    protected void cmdClear_Click(object sender, EventArgs e)
    {
        foreach (ListItem item in cblVessels.Items)
        {
            if (ddlCriteria.SelectedValue.Equals("LIKE"))
            {
                if (item.Text.ToUpper().Contains(txtFilter.Text.ToUpper().Trim()))
                {
                    item.Selected = false;
                }
            }

            if (ddlCriteria.SelectedValue.Equals("START"))
            {
                if (item.Text.ToUpper().StartsWith(txtFilter.Text.ToUpper().Trim()))
                {
                    item.Selected = false;
                }
            }

            if (ddlCriteria.SelectedValue.Equals("END"))
            {
                if (item.Text.ToUpper().EndsWith(txtFilter.Text.ToUpper().Trim()))
                {
                    item.Selected = false;
                }
            }
        }
    }

    protected void cmdUpdateLink_Click(object sender, EventArgs e)
    {
        if (ViewState["PATCHDTKEY"] == null)
        {
            ucError.ErrorMessage = "Create a patch and then upload the attachments";
            ucError.Visible = true;
            return;
        }

        string vessellist = General.ReadCheckBoxList(cblVessels);
        string[] arr = vessellist.Split(',');
        if (arr.Length <= 1)
        {
            if (arr[0].Length == 0)
            {
                ucError.ErrorMessage = "You have to select the vessels to which the file will be sent.";
                ucError.Visible = true;
                return;
            }
        }

        try
        {
            string linkfilename = "";
            string linkfilepath = "";
            if (IsValidURL(txtLinkFile.Text, ref linkfilepath, ref linkfilename))
            {
                PhoenixPatchTracker.PatchAttachmentFileInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                     General.GetNullableGuid(ViewState["PATCHDTKEY"].ToString()),
                     linkfilename,
                     linkfilepath,
                     2); // 2 - Link 1 - File
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

        BindData();
    }

    protected void cmdUpload_Click(object sender, EventArgs e)
    {
        if (ViewState["PATCHDTKEY"] == null)
        {
            ucError.ErrorMessage = "Create a patch and then upload the attachments";
            ucError.Visible = true;
            return;
        }

        string vessellist = General.ReadCheckBoxList(cblVessels);
        string[] arr = vessellist.Split(',');
        if (arr.Length <= 1)
        {
            if (arr[0].Length == 0)
            {
                ucError.ErrorMessage = "You have to select the vessels to which the file will be sent.";
                ucError.Visible = true;
                return;
            }
        }


        HttpFileCollection postedFiles = Request.Files;
        if (Request.Files["filPatchAttachment"].ContentLength > 0)
        {
            string origpath = HttpContext.Current.Request.MapPath("~/");
            string path = origpath + "Attachments\\";
            for (int i = 0; i < postedFiles.Count; i++)
            {
                HttpPostedFile postedFile = postedFiles[i];

                try
                {
                    string targetpath = PhoenixPatchTracker.PatchValidate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableGuid(ViewState["PATCHDTKEY"].ToString()),
                        postedFile.FileName);

                    if (targetpath.Length > 0)
                        path = path + "\\" + targetpath + "\\";
                }
                catch (Exception ex)
                {
                    ucError.ErrorMessage = ex.Message;
                    ucError.Visible = true;
                    return;
                }

                if (postedFile.ContentLength > 0)
                {
                    postedFile.SaveAs(path + postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\') + 1));
                    string filename = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('\\') + 1);
                    PhoenixPatchTracker.PatchAttachmentFileInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableGuid(ViewState["PATCHDTKEY"].ToString()),
                        filename,
                        path,
                        1); // 2 - Link 1 - File

                    BindAttachments();
                }
            }
        }
    }

    private void BindProject()
    {
        if (ViewState["PATCHPROJECTDTKEY"] == null)
            return;
        if (ViewState["PATCHPROJECTDTKEY"].ToString() == "")
            return;
        DataTable dt = PhoenixPatchTracker.PatchProjectEdit(new Guid(ViewState["PATCHPROJECTDTKEY"].ToString()));

        if (dt.Rows.Count > 0)
        {
            lblCatalog.Text = dt.Rows[0]["FLDCATALOGNUMBER"].ToString();
            lblTitle.Text = dt.Rows[0]["FLDTITLE"].ToString();
            txtCallNumber.Text = dt.Rows[0]["FLDCALLNUMBER"].ToString();
            ucCallDate.Text = dt.Rows[0]["FLDCALLDATE"].ToString();
        }
    }

    private void BindData()
    {
        DataSet ds = PhoenixDefectTracker.PatchEdit(new Guid(ViewState["PATCHDTKEY"].ToString()));

        DataTable dt = ds.Tables[0];

        if (dt.Rows.Count > 0)
        {
            txtSubject.Text = dt.Rows[0]["FLDSUBJECT"].ToString();
            txtCreatedby.SelectedTeamMember = dt.Rows[0]["FLDPATCHCREATEDBY"].ToString();
            General.BindCheckBoxList(cblVessels, dt.Rows[0]["FLDVESSELLIST"].ToString());
            if (!dt.Rows[0]["FLDPATCHPROJECTDTKEY"].ToString().Equals(""))
                ddlPatchProject.SelectedValue = General.GetNullableGuid(dt.Rows[0]["FLDPATCHPROJECTDTKEY"].ToString()).ToString();
            ViewState["PATCHPROJECTDTKEY"] = dt.Rows[0]["FLDPATCHPROJECTDTKEY"].ToString();
            txtMessage.Text = dt.Rows[0]["FLDBODY"].ToString();
        }
        DataTable dt1 = ds.Tables[1];
        if (dt1.Rows.Count > 0)
        {
            gvPatchAttachment.DataSource = dt1;
            gvPatchAttachment.DataBind();
        }
    }

    private void BindAttachments()
    {
        DataSet ds = PhoenixDefectTracker.PatchEdit(new Guid(ViewState["PATCHDTKEY"].ToString()));

        DataTable dt = ds.Tables[1];
        if (dt.Rows.Count >= 0)
        {
            gvPatchAttachment.DataSource = dt;
            gvPatchAttachment.DataBind();
        }
    }


    protected void MenuPatchAdd_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("MAILEDIT"))
        {
            Response.Redirect("DefectTrackerEditPatch.aspx?dtkey=" + ViewState["PATCHDTKEY"].ToString() + "&projectdtkey=" + ViewState["PATCHPROJECTDTKEY"].ToString());
        }


        if (dce.CommandName.ToUpper().Equals("VESSELLIST"))
        {
            Response.Redirect("DefectTrackerPatchVesselList.aspx?dtkey=" + ViewState["PATCHDTKEY"].ToString() + "&projectdtkey=" + ViewState["PATCHPROJECTDTKEY"].ToString());
        }
    }

    protected void MenuPatchSave_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            DataListCommandEventArgs dce = (DataListCommandEventArgs)e;


            string vessellist = General.ReadCheckBoxList(cblVessels);
            string[] arr = vessellist.Split(',');
            if (arr.Length <= 1)
            {
                if (arr[0].Length == 0)
                {
                    ucError.ErrorMessage = "You have to select the vessels to which the file will be sent.";
                    ucError.Visible = true;
                    return;
                }
            }

            if (dce.CommandName.ToUpper().Equals("SAVE"))
            {
                HttpFileCollection postedFiles = Request.Files;

                if (!IsValidValues(txtSubject.Text, txtCreatedby.SelectedTeamMemberName))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["PATCHDTKEY"] != null)
                {
                    PhoenixPatchTracker.PatchAttachmentUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                new Guid(ViewState["PATCHDTKEY"].ToString()),
                                                                txtSubject.Text,
                                                                txtCreatedby.SelectedTeamMemberName,
                                                                vessellist,
                                                                General.GetNullableGuid(ddlPatchProject.SelectedValue),
                                                                txtMessage.Text
                                                               );
                    ucStatus.Text = "Patch information updated";

                    Response.Redirect("DefectTrackerPatchAddEdit.aspx?dtkey=" + ViewState["PATCHDTKEY"].ToString());
                }
                else
                {
                    Guid? patchdtkey = null;

                    PhoenixPatchTracker.PatchAttachmentInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                General.GetNullableGuid((ViewState["PATCHPROJECTDTKEY"] == null) ? null : ViewState["PATCHPROJECTDTKEY"].ToString()),
                                                                txtSubject.Text,
                                                                txtCreatedby.SelectedTeamMemberName,
                                                                vessellist,
                                                                ref patchdtkey,
                                                                txtMessage.Text
                                                                );
                    ViewState["PATCHDTKEY"] = patchdtkey.ToString();
                                     
                }
                BindData();
                ucStatus.Text = "Patch Created";
                String script = String.Format("javascript:fnReloadList('code1');");
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                 
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvPatchAttachment_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblFileName = (Label)e.Row.FindControl("lblFileName");
            Label lblFilePath = (Label)e.Row.FindControl("lblFilePath");
            HyperLink lnk = (HyperLink)e.Row.FindControl("lnkfilename");

            string path = "";
            string origpath = HttpContext.Current.Request.MapPath("~/");

            if (lblFilePath.Text.Contains("http://") || lblFilePath.Text.Contains("ftp://"))
            {
                path = lblFilePath.Text;
                lnk.NavigateUrl = path.Trim('/') + "/" + lblFileName.Text;
            }
            else
            {
                if (lblFileName.Text.ToUpper().Contains("DATAEXTRACT"))
                {
                    path = origpath + "Attachments\\Patch\\DataExtract";
                }
                else if (lblFileName.Text.ToUpper().Contains("HOTFIX"))
                {
                    path = origpath + "Attachments\\Patch\\HotFixes";
                }
                else if (lblFileName.Text.ToUpper().Contains("PATCH"))
                {
                    path = origpath + "Attachments\\Patch\\Patch";
                }
                else
                    path = origpath + "Attachments\\Patch\\ServicePack";
                lnk.NavigateUrl = "DefectTrackerMailAttachment.ashx?path=" + path + "\\" + lblFileName.Text;
            }

        }
    }

    protected void gvPatchAttachment_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
    }

    protected void gvPatchAttachment_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            Label lbl = (Label)_gridView.Rows[nCurrentRow].FindControl("lblDTKey");

            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixDefectTracker.PatchFileDelete(new Guid(lbl.Text));
                BindData();
                BindAttachments();
            }

            if (e.CommandName.ToUpper().Equals("EDITFILE"))
            {
                if (lbl != null)
                {
                    DataTable dt = PhoenixDefectTracker.PatchFileEdit(General.GetNullableGuid(lbl.Text));
                    string filepath = dt.Rows[0]["FLDFILEPATH"].ToString().TrimEnd('/').TrimEnd('\\');

                    if (filepath.Contains("/"))
                        txtLinkFile.Text = filepath + "/" + dt.Rows[0]["FLDFILENAME"].ToString();
                    if (filepath.Contains("\\"))
                        txtLinkFile.Text = filepath + "\\" + dt.Rows[0]["FLDFILENAME"].ToString();

                    cblVessels.ClearSelection();
                    General.BindCheckBoxList(cblVessels, dt.Rows[0]["FLDVESSELLIST"].ToString());
                }
                BindAttachments();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private bool IsValidValues(string subject, string createdby)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (subject == "")
            ucError.ErrorMessage = "Subject is required";

        if ((createdby == "") || (createdby == "Dummy"))
            ucError.ErrorMessage = "Created by is required";

        return (!ucError.IsError);
    }

    private bool IsValidURL(string url, ref string path, ref string filename)
    {
        if (url.Trim().Equals(""))
            return false;

        int fileindex = url.LastIndexOf("/");
        if (fileindex < 0)
            fileindex = url.LastIndexOf("\\");

        fileindex = fileindex + 1;
        filename = url.Substring(fileindex);
        path = url.Substring(0, fileindex);

        return true;
    }

}
