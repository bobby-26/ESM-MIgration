using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Text;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Common;
using System.Collections;
using Telerik.Web.UI;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Integration;
using SouthNests.Phoenix.Dashboard;
using System.IO;

public partial class DocumentManagement_DocumentManagementFormListDetailsUpdateNew : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuCommentsEdit.AccessRights = this.ViewState;
        MenuCommentsEdit.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            btnShowCategory.Attributes.Add("onclick", "return showPickList('spnPickListCategory', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListDocumentCategory.aspx', true); ");
            btnShowComponents.Attributes.Add("onclick", "return showPickList('spnPickListComponent', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListGlobalComponent.aspx', true); ");
            if (Request.QueryString["FORMID"] != null && Request.QueryString["FORMID"].ToString() != string.Empty)
            {
                ViewState["FORMID"] = Request.QueryString["FORMID"].ToString();

                BindJobs(txtComponentId.Text);
            }
            else
                ViewState["FORMID"] = "";
            if (Request.QueryString["pageno"] != null)
                ViewState["pageno"] = Request.QueryString["pageno"].ToString();

            BindGroupfunctionList();
            FormEdit();

        }

        //imgJob.Attributes.Add("onclick", "return showPickList('spnPickListJob', 'codehelp1', '', '" + Session["sitepath"] + "/Common/CommonPickListJob.aspx', true);");


    }

    private void BindGroupfunctionList()
    {
        DataSet ds = new DataSet();
        ds = PhoenixInspectionRiskAssessmentJobHazardExtn.FunctionGroupMappingList();
        ddlgroupfunction.DataSource = ds;
        ddlgroupfunction.DataBind();
    }
    protected void BindFormRA()
    {
        DataSet dss = PhoenixDocumentManagementQuestion.MappedRAList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["FORMID"].ToString()));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0 && dss.Tables[0].Columns.Count > 1)
        {
            tblRA.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                CheckBox cbRA = new CheckBox();
                cbRA.ID = dr["FLDRAID"].ToString();
                cbRA.Text = "";
                cbRA.Checked = true;
                cbRA.Enabled = false;
                HyperLink hlreport = new HyperLink();
                hlreport.Text = dr["FLDNAME"].ToString();
                hlreport.ID = "hlreportra" + number.ToString();
                hlreport.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                hlreport.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'SSRSReports/SsrsReportsViewWithSubReport.aspx?applicationcode=9&reportcode=RAPROCESSNEW&showmenu=0&showexcel=NO&processid=" + dr["FLDRAID"].ToString() + "');return false;");
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tc.Controls.Add(cbRA);
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hlreport);
                tr.Cells.Add(tc);
                tblRA.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblRA.Rows.Add(tr);
                number = number + 1;
            }
            dvRA.Visible = true;
        }
        else
            dvRA.Visible = true;
    }
    private void BindJobs(string Componentid)
    {

        cbJobs.DataSource = PhoenixDocumentManagementQuestion.ComponentJobList(General.GetNullableGuid(Componentid));
        cbJobs.DataTextField = "FLDJOBTITLE";
        cbJobs.DataValueField = "FLDGLOBALCOMPONENTJOBID";
        cbJobs.DataBind();
    }
    protected void BindFormProcedures()
    {
        DataSet dss = PhoenixDocumentManagementQuestion.DocumentList(new Guid(ViewState["FORMID"].ToString()));

        int number = 1;
        if (dss.Tables[0].Rows.Count > 0 && dss.Tables[0].Columns.Count > 1)
        {
            tblPROCEDURE.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                CheckBox cbDocument = new CheckBox();
                cbDocument.ID = dr["FLDSECTIONID"].ToString();
                cbDocument.Text = "";
                cbDocument.Checked = true;
                cbDocument.Enabled = false;
                HyperLink hl = new HyperLink();
                hl.Text = dr["FLDNAME"].ToString();
                hl.ID = "hlink" + number.ToString();
                hl.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                hl.Attributes.Add("onclick", "openNewWindow('codehelp2', '','" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDocumentSectionNewContentView.aspx?callfrom=documenttree&SECTIONID=" + dr["FLDSECTIONID"].ToString() + "');return false;");
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tc.Controls.Add(cbDocument);
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hl);
                tr.Cells.Add(tc);
                tblPROCEDURE.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblPROCEDURE.Rows.Add(tr);
                number = number + 1;
            }
            dvPROCEDURE.Visible = true;
        }
        else
            dvPROCEDURE.Visible = true;
    }
    protected void BindFormJHA()
    {
        DataSet dss = PhoenixDocumentManagementQuestion.MappedJHAList(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["FORMID"].ToString()));
        int number = 1;
        if (dss.Tables[0].Rows.Count > 0 && dss.Tables[0].Columns.Count > 1)
        {
            tblJHA.Rows.Clear();
            foreach (DataRow dr in dss.Tables[0].Rows)
            {
                CheckBox cbreport = new CheckBox();
                cbreport.ID = dr["FLDJHAID"].ToString();
                cbreport.Text = "";
                cbreport.Checked = true;
                cbreport.Enabled = false;
                HyperLink hlreport = new HyperLink();
                hlreport.Text = dr["FLDNAME"].ToString();
                hlreport.ID = "reportlink" + number.ToString();
                hlreport.Attributes.Add("style", "text-decoration:underline; cursor: pointer;color:Blue;");
                hlreport.Attributes.Add("onclick", "openNewWindow('codehelp1', '', 'SSRSReports/SsrsReportsViewWithSubReport.aspx?applicationcode=9&reportcode=JOBHAZARDNEW&showmenu=0&showexcel=NO&showword=NO&jobhazardid=" + dr["FLDJHAID"].ToString() + "');return false;");
                HtmlTableRow tr = new HtmlTableRow();
                HtmlTableCell tc = new HtmlTableCell();
                tc.Controls.Add(cbreport);
                tr.Cells.Add(tc);
                tc = new HtmlTableCell();
                tc.Controls.Add(hlreport);
                tr.Cells.Add(tc);
                tblJHA.Rows.Add(tr);
                tc = new HtmlTableCell();
                tc.InnerHtml = "<br/>";
                tr.Cells.Add(tc);
                tblJHA.Rows.Add(tr);
                number = number + 1;
            }
            dvJHA.Visible = true;
        }
        else
            dvJHA.Visible = true;
    }
    protected void FormEdit()
    {
        if (General.GetNullableGuid(ViewState["FORMID"].ToString()) != null)
        {
            DataSet ds = PhoenixDocumentManagementForm.FormEdit(PhoenixSecurityContext.CurrentSecurityContext.UserCode, new Guid(ViewState["FORMID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[0].Rows[0];
                txtFormNo.Text = dr["FLDFORMNO"].ToString();
                txtCategoryid.Text = dr["FLDCATEGORYID"].ToString();
                txtCategory.Text = dr["FLDCATEGORY"].ToString();
                txtFormName.Text = dr["FLDCAPTION"].ToString();
                imgPhoto.Attributes.Add("src", dr["FLDIMAGE"].ToString());
                if (dr["FLDDISPLAYINFMS"] != null)
                {
                    if (dr["FLDDISPLAYINFMS"].ToString() == "1")
                        radchckfmsyn.Checked = true;
                }
                if (dr["FLDTYPE"] != DBNull.Value)
                {
                    rListAdd.SelectedValue = dr["FLDTYPE"].ToString();
                }
                if (dr["FLDACTIVEYN"].ToString() == "1")
                    chkActiveYN.Checked = true;
                else
                    chkActiveYN.Checked = false;
                txtRemarks.Text = dr["FLDPURPOSE"].ToString();
                if (dr["FLDCOMPANYID"] != null && dr["FLDCOMPANYID"].ToString() != "")
                    ucCompany.SelectedCompany = dr["FLDCOMPANYID"].ToString();
                if (dr["FLDSHIPDEPARTMENT"] != null && dr["FLDSHIPDEPARTMENT"].ToString() != "")
                    ucGroupEdit.SelectedHard = dr["FLDSHIPDEPARTMENT"].ToString();
                if (dr["FLDOFFICEDEPARTMENT"] != null && dr["FLDOFFICEDEPARTMENT"].ToString() != "")
                    ucOfficeDept.SelectedValue = int.Parse(dr["FLDOFFICEDEPARTMENT"].ToString());
                txtTimeInterval.Text = dr["FLDTIMEINTERVAL"].ToString();
                if (dr["FLDCOUNTRYPORT"] != null && dr["FLDCOUNTRYPORT"].ToString() != "")
                    ucPort.SelectedValue = dr["FLDCOUNTRYPORT"].ToString();
                if (dr["FLDEQUIPMENTMAKER"] != null && dr["FLDEQUIPMENTMAKER"].ToString() != "")
                    ucEquipment.SelectedValue = dr["FLDEQUIPMENTMAKER"].ToString();

                BindFormJHA();
                txtComponentId.Text = dr["FLDPMSCOMPONENT"].ToString();
                BindFormProcedures();
                BindFormRA();
                //txtJobId.Text = dr["FLDJOB"].ToString();
                //txtJobCode.Text = dr["FLDJOBCODE"].ToString();
                //txtJobName.Text = dr["FLDJOBTITLE"].ToString();

                foreach (RadComboBoxItem item in ddlgroupfunction.Items)
                {
                    item.Checked = false;
                    if (dr["FLDFUNCTION"].ToString().Contains("," + item.Value + ","))
                    {
                        item.Checked = true;
                    }
                }
                txtComponentCode.Text = dr["FLDCOMPONENTNUMBER"].ToString();
                txtComponentName.Text = dr["FLDCOMPONENTNAME"].ToString();
                BindJobs(txtComponentId.Text);
                General.BindCheckBoxList(cbJobs, dr["FLDJOBLIST"].ToString());
            }
        }
    }

    protected void MenuCommentsEdit_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {

                string result = "";


                foreach (UploadedFile postedFile in RadUpload1.UploadedFiles)
                {
                    if (!Object.Equals(postedFile, null))
                    {
                        if (postedFile.ContentLength > 0)
                        {

                            if (postedFile.ContentLength > (60 * 1024))
                            {
                                ucError.ErrorMessage = "Uploaded Photo size cannot exceed 60kb.";
                                ucError.Visible = true;
                                return;
                            }

                            using (MemoryStream stream = new MemoryStream())
                            {
                                string path = HttpContext.Current.Server.MapPath("..\\Attachments\\TEMP\\") + postedFile.FileName;
                                postedFile.SaveAs(path);

                                FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                                byte[] bites = new byte[fs.Length];
                                fs.Read(bites, 0, bites.Length);

                                string base64ImageRepresentation = Convert.ToBase64String(bites);

                                result = String.Format("data:image/{0};base64,{1}", "png", base64ImageRepresentation);
                            }

                        }
                    }
                }

                string function = "";
                foreach (RadComboBoxItem item in ddlgroupfunction.Items)
                {
                    if (item.Checked == true)
                        function = function + item.Value + ",";

                }
                int displayinfms = 0;
                if (radchckfmsyn.Checked == true)
                {
                    displayinfms = 1;
                }
                PhoenixDocumentManagementForm.FormDetailsUpdate(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                 , General.GetNullableString(txtFormNo.Text)
                                                 , General.GetNullableString(txtFormName.Text)
                                                 , General.GetNullableGuid(txtCategoryid.Text)
                                                 , chkActiveYN.Checked == true ? 1 : 0
                                                 , General.GetNullableString(txtRemarks.Text)
                                                 , General.GetNullableGuid(ViewState["FORMID"].ToString())
                                                 , General.GetNullableInteger(rListAdd.SelectedValue)
                                                 , General.GetNullableInteger(ucCompany.SelectedCompany)
                                                 , General.GetNullableInteger(ucGroupEdit.SelectedHard)
                                                 , General.GetNullableInteger(ucOfficeDept.SelectedDepartment)
                                                 , null
                                                 , General.GetNullableInteger(ucPort.SelectedValue)
                                                 , General.GetNullableInteger(ucEquipment.SelectedValue)
                                                 , General.GetNullableGuid(txtComponentId.Text)
                                                 , null
                                                 , General.GetNullableString(txtTimeInterval.Text)
                                                 , displayinfms
                                                 , null
                                                 , function
                                                 , General.GetNullableGuid(null)
                                                 , General.ReadCheckBoxList(cbJobs)
                                                 , General.GetNullableString(result));

                FormEdit();
                ucStatus.Text = "Saved successfully.";
            }

            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementFormListNew.aspx?FORMID=" + ViewState["FORMID"] + "&pageno=" + ViewState["pageno"].ToString());
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void cmdJobClear_Click(object sender, ImageClickEventArgs e)
    {
        //txtJobCode.Text = "";
        //txtJobName.Text = "";
        //txtJobId.Text = "";
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindJobs(txtComponentId.Text);
    }

    protected void txtComponentId_TextChanged(object sender, EventArgs e)
    {
        BindJobs(txtComponentId.Text);
    }
}