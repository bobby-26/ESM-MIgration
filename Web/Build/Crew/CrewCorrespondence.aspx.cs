using System;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class CrewCorrespondence : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewCorrespondence.aspx?"+ Request.QueryString, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCorrespondence')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('CI','','" + Session["sitepath"] + "/Crew/CrewCorrespondenceFilter.aspx?iframIgnore=True'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");            
            MenuCrewCorrespondence.AccessRights = this.ViewState;
            MenuCrewCorrespondence.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();          
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);          
            MenuCorrespondence.AccessRights = this.ViewState;
            MenuCorrespondence.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none");
                RemoveEditorToolBarIcons(); // remove the unwanted icons in editor

                string strEmployeeId = string.Empty;
                if (string.IsNullOrEmpty(Filter.CurrentCrewSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                    strEmployeeId = string.Empty;
                else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                    strEmployeeId = Request.QueryString["empid"];
                else if (!string.IsNullOrEmpty(Filter.CurrentCrewSelection))
                    strEmployeeId = Filter.CurrentCrewSelection;

                ViewState["EMPID"] = strEmployeeId;
                DataTable dt = PhoenixCrewAddress.ListEmployeeAddress(int.Parse(strEmployeeId));
                ViewState["EMPEMAIL"] = dt.Rows.Count > 0 ? dt.Rows[0]["FLDEMAIL"].ToString() : string.Empty;

                if (General.GetNullableInteger(ViewState["EMPID"].ToString()) == null)
                {
                    ucError.ErrorMessage = "Select a Employee from Query Activity";
                    ucError.Visible = true;
                    return;
                }
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = -1;
                ViewState["CORRESPONDENCEID"] = string.Empty;
                ViewState["mailsessionid"] = string.Empty;
                SetEmployeePrimaryDetails();
                trBCC.Visible = false;
                trCC.Visible = false;
                trAtt.Visible = false;
                trFrom.Visible = false;
                trTO.Visible = false;
                txtTO.CssClass = "input";
                txtDate.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);

                txtSubject.CssClass = "readonlytextbox";
                txtSubject.ReadOnly = true;
                ddlCorrespondenceType.Enabled = false;
                
                gvCorrespondence.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                
            }       
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void RemoveEditorToolBarIcons()
    {
        txtBody.EnsureToolsFileLoaded();
        RemoveButton("ImageManager");
        RemoveButton("DocumentManager");
        RemoveButton("FlashManager");
        RemoveButton("MediaManager");
        RemoveButton("TemplateManager");
        RemoveButton("XhtmlValidator");
        RemoveButton("InsertSnippet");
        RemoveButton("ModuleManager");
        RemoveButton("ImageMapDialog");
        RemoveButton("AboutDialog");
        RemoveButton("InsertFormElement");

        txtBody.EnsureToolsFileLoaded();
        txtBody.Modules.Remove("RadEditorHtmlInspector");
        txtBody.Modules.Remove("RadEditorNodeInspector");
        txtBody.Modules.Remove("RadEditorDomInspector");
        txtBody.Modules.Remove("RadEditorStatistics");

    }

    protected void RemoveButton(string name)
    {
        foreach (EditorToolGroup group in txtBody.Tools)
        {
            EditorTool tool = group.FindTool(name);
            if (tool != null)
            {
                group.Tools.Remove(tool);
            }
        }

    }


    protected void CrewCorrespondence_TabStripCommand(object sender, EventArgs e)
    {
        try
        {

            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                BindData();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void Correspondence_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidCorrespondence(txtSubject.Text, ddlCorrespondenceType.SelectedQuick))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["CORRESPONDENCEID"].ToString() == string.Empty)
                {
                    PhoenixCrewCorrespondence.InsertCorrespondence(int.Parse(ViewState["EMPID"].ToString()), int.Parse(ddlCorrespondenceType.SelectedQuick)
                                                                    , txtFrom.Text, txtTO.Text, txtSubject.Text, txtBody.Content, 1);
                }
                else
                {
                    //PhoenixCrewCorrespondence.UpdateCorrespondence(int.Parse(ViewState["CORRESPONDENCEID"].ToString()), int.Parse(ViewState["EMPID"].ToString()), int.Parse(ddlCorrespondenceType.SelectedQuick)
                    //                                                    , txtFrom.Text, txtTO.Text, txtSubject.Text, txtBody.Content, 1);

                    ucError.ErrorMessage = "Cannot Update the Existing Record. Add New Entry";
                    ucError.Visible = true;
                    return;

                }              
                BindData();
                gvCorrespondence.Rebind();             
            }
                
            else if (CommandName.ToUpper().Equals("SEND"))
            {
                if (!IsValidCorrespondence(txtSubject.Text, ddlCorrespondenceType.SelectedQuick))
                {
                    ucError.Visible = true;
                    return;
                }
                string dtkey = string.Empty;                
                string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/EmailAttachments/" + ViewState["mailsessionid"].ToString();
                DataSet ds = PhoenixCrewCorrespondence.InsertCorrespondence(int.Parse(ViewState["EMPID"].ToString()), int.Parse(ddlCorrespondenceType.SelectedQuick)
                                                                , txtFrom.Text, txtTO.Text, txtSubject.Text, txtBody.Content, null);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    dtkey = ds.Tables[0].Rows[0][0].ToString();
                    if (Directory.Exists(path))
                    {
                        DirectoryInfo di = new DirectoryInfo(path);
                        FileInfo[] fi = di.GetFiles();
                        foreach (FileInfo f in fi)
                        {

                            string tempkey = PhoenixCommonFileAttachment.GenerateDTKey();
                            PhoenixCommonFileAttachment.InsertAttachment(new Guid(dtkey), f.Name, "Crew/" + tempkey + f.Extension, f.Length
                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, PhoenixCrewAttachmentType.CORRESPONDENCE.ToString(), new Guid(tempkey));

                            string desgpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
                            desgpath = desgpath + "/Crew/" + tempkey + f.Extension;
                            
                            f.CopyTo(desgpath, true);

                        }
                    }
                }
                PhoenixMail.SendMail(txtTO.Text, txtCC.Text, txtBCC.Text, txtSubject.Text, txtBody.Content
                               , true, System.Net.Mail.MailPriority.Normal, ViewState["mailsessionid"].ToString(), General.GetNullableInteger(ViewState["EMPID"].ToString()));
              
                BindData();
                gvCorrespondence.Rebind();
            }
            if (!CommandName.ToUpper().Equals("NEWMAIL"))
            {
                ViewState["CORRESPONDENCEID"] = string.Empty;
                ResetFields();
                txtDate.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                trBCC.Visible = false;
                trCC.Visible = false;
                trAtt.Visible = false;
                trFrom.Visible = false;
                trTO.Visible = false;
                txtTO.CssClass = "input";

                if (CommandName.ToUpper().Equals("DISCARD"))
                {
                    if (lstAttachments.Items.Count > 0)
                    {
                        string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/EmailAttachments/" + ViewState["mailsessionid"].ToString();
                        if (Directory.Exists(path))
                            Directory.Delete(path, true);
                        lstAttachments.Items.Clear();
                    }
                }
                ViewState["mailsessionid"] = string.Empty;

                //PhoenixToolbar toolbar = new PhoenixToolbar();
                //toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                //toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
                //MenuCorrespondence.MenuList = toolbar.Show();

                txtSubject.CssClass = "input_mandatory";
                txtSubject.ReadOnly = false;
                ddlCorrespondenceType.Enabled = true;

                txtBody.EditModes = EditModes.All;
            }

          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditCorrespondence(string correspondenceid, string employeeid)
    {
        try
        {

            DataTable dt = PhoenixCrewCorrespondence.EditCorrespondence(General.GetNullableInteger(correspondenceid).Value, General.GetNullableInteger(employeeid));

            if (dt.Rows.Count > 0)
            {
                ViewState["CORRESPONDENCEID"] = dt.Rows[0]["FLDCORRESPONDENCEID"].ToString();
                //PhoenixToolbar toolbar = new PhoenixToolbar();
                //toolbar.AddButton("New", "NEW", ToolBarDirection.Right);                            
                //MenuCorrespondence.MenuList = toolbar.Show();
                txtFrom.Text = dt.Rows[0]["FLDFROM"].ToString();
                txtTO.Text = dt.Rows[0]["FLDTO"].ToString();
                txtSubject.Text = dt.Rows[0]["FLDSUBJECT"].ToString();
                txtBody.Content = "";
                txtBody.Content = "<br/>" + "<b>" + "From: " + "</b>" + dt.Rows[0]["FLDFROM"].ToString() + "<br/>" + "<b>" + "To: " + "</b>" + dt.Rows[0]["FLDTO"].ToString() + "<br/>";
                string CC = dt.Rows[0]["FLDCC"].ToString();
                if (CC != null && CC != "")
                    txtBody.Content += "<b>" + "Cc: " + "</b>" + dt.Rows[0]["FLDCC"].ToString();
                txtBody.Content += "<br/>" + "<br/>" + dt.Rows[0]["FLDBODY"].ToString();

                txtDate.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDCREATEDDATE"].ToString()));
                ddlCorrespondenceType.SelectedQuick = "";
                ddlCorrespondenceType.SelectedQuick = dt.Rows[0]["FLDCORRESPONDENCETYPE"].ToString();
                ViewState["DTKEY"] = dt.Rows[0]["FLDDTKEY"].ToString();

                txtBody.EditModes = EditModes.Preview;

            }

            txtSubject.CssClass = "readonlytextbox";
            txtSubject.ReadOnly = true;
            ddlCorrespondenceType.Enabled = false;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDCREATEDDATE", "FLDSUBJECT", "FLDCORRESPONDENCETYPENAME", "FLDCREATEDBYNAME", "FLDTYPEOF" };
            string[] alCaptions = { "Created Date", "Subject", "Correspondence Type", "Created By", "Type Of" };
            string sortexpression;
            int? sortdirection = null;

            sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
                iRowCount = 10;
            else
                iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
            NameValueCollection nvc = Filter.CurrentCorrespondenceFilter;
            DataTable dt = new DataTable();
            if (Filter.CurrentCorrespondenceFilter != null)
            {
                nvc["filter"] = "0";
                dt = PhoenixCrewCorrespondence.SearchCorrespondence(Convert.ToInt32(ViewState["EMPID"].ToString()),
                                                                                               sortexpression, sortdirection,
                                                                                         (int)ViewState["PAGENUMBER"],
                                                                                         General.ShowRecords(null),
                                                                                         ref iRowCount,
                                                                                         ref iTotalPageCount,
                                                                                         General.GetNullableString(nvc.Get("txtSubject").ToString()),
                                                                                         General.GetNullableInteger(nvc.Get("ddlCorrespondenceType").ToString()));


            }
            else
            {
                dt = PhoenixCrewCorrespondence.SearchCorrespondence(Convert.ToInt32(ViewState["EMPID"].ToString()),
                                                                                   sortexpression, sortdirection,
                                                                             (int)ViewState["PAGENUMBER"],
                                                                             General.ShowRecords(null),
                                                                             ref iRowCount,
                                                                             ref iTotalPageCount,
                                                                             null, null);
            }

            General.ShowExcel("Crew Correspondence", dt, alColumns, alCaptions, sortdirection, sortexpression);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCorrespondence_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCorrespondence.CurrentPageIndex + 1;
        BindData();
    }

    
 

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCREATEDDATE", "FLDSUBJECT", "FLDCORRESPONDENCETYPENAME", "FLDCREATEDBYNAME", "FLDTYPEOF" };
            string[] alCaptions = { "Created Date", "Subject", "Correspondence Type", "Created By", "Type Of" };

            NameValueCollection nvc = Filter.CurrentCorrespondenceFilter;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 1;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = new DataTable();
            if (Filter.CurrentCorrespondenceFilter != null)
            {
                nvc["filter"] = "0";
                dt = PhoenixCrewCorrespondence.SearchCorrespondence(Convert.ToInt32(ViewState["EMPID"].ToString())
                                                                    , sortexpression
                                                                    , sortdirection
                                                                    , Int32.Parse(ViewState["PAGENUMBER"].ToString())
                                                                    , gvCorrespondence.PageSize
                                                                    , ref iRowCount
                                                                    , ref iTotalPageCount
                                                                    , General.GetNullableString(nvc.Get("txtSubject").ToString())
                                                                    , General.GetNullableInteger(nvc.Get("ddlCorrespondenceType").ToString()));
                
            }
            else
            {
                dt = PhoenixCrewCorrespondence.SearchCorrespondence(Convert.ToInt32(ViewState["EMPID"].ToString()),
                                                                             sortexpression, sortdirection,
                                                                             Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                                                                             gvCorrespondence.PageSize,
                                                                             ref iRowCount,
                                                                             ref iTotalPageCount,
                                                                             null, null);
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            ds.AcceptChanges();

            General.SetPrintOptions("gvCorrespondence", "Crew Correspondence", alCaptions, alColumns, ds);
           
            gvCorrespondence.DataSource = dt;
            gvCorrespondence.VirtualItemCount = iRowCount;            
            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void gvCorrespondence_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName)) db.Visible = false;
            }
            
            LinkButton cme = (LinkButton)e.Item.FindControl("cmdEdit");
            if (cme != null) if (!SessionUtil.CanAccess(this.ViewState, cme.CommandName)) cme.Visible = false;

            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            string strlock = ((RadLabel)e.Item.FindControl("lblLockYN")).Text;

            if (att != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }

                att.Attributes.Add("onclick", "javascript:parent.openNewWindow('CI','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                        + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.CORRESPONDENCE + (strlock == "1" ? "&U=N" : string.Empty)  + "&cmdname=CORRESUPLOAD'); return false;");
            }
 
            LinkButton chkMail = (LinkButton)e.Item.FindControl("lnkCorrespondence");
            if (drv["FLDTYPE"].ToString() != "1")
            {
                chkMail.Attributes["target"] = "filterandsearch";
                chkMail.Attributes["href"] = "CrewPersonalGeneral.aspx?email=true&cid=" + drv["FLDCORRESPONDENCEID"].ToString();
            }
            
            LinkButton cmdLock = (LinkButton)e.Item.FindControl("cmdLock");
           
            if (cmdLock != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdLock.CommandName)) cmdLock.Visible = false;

                if (drv["FLDLOCKEDYN"].ToString() != "0")
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-unlock\"></i></span>";
                    cmdLock.Controls.Add(html);
                }

                cmdLock.Attributes.Add("onclick", "javascript:openNewWindow('CI','','" + Session["sitepath"] + "/Crew/CrewCorrespondenceLockAndUnlock.aspx?id=" + drv["FLDCORRESPONDENCEID"].ToString() + " &empid="
                        + ViewState["EMPID"].ToString() + "','medium'); return false;");

                //   toolbar.AddImageLink("javascript:parent.Openpopup('CI','','../Crew/CrewCorrespondenceLockAndUnlock.aspx?id=" + dt.Rows[0]["FLDCORRESPONDENCEID"].ToString() + "&empid=" + ViewState["EMPID"].ToString() + "','medium'); return false;", dt.Rows[0]["FLDLOCKEDYN"].ToString() == "0" ? "Lock" : "UnLock", "", dt.Rows[0]["FLDLOCKEDYN"].ToString() == "0" ? "LOCK" : "UNLOCK");           
            }            
        }
    }

    protected void gvCorrespondence_ItemCommand(object sender, GridCommandEventArgs e)
    {
        GridEditableItem editedItem = e.Item as GridEditableItem;

        if (e.CommandName.ToUpper() == "EMAIL")
        {
            string Correspondenceid = ((RadLabel)editedItem.FindControl("lblCorrespondenceId")).Text;          
            if (Correspondenceid != "")            
            {
                string empid = ((RadLabel)editedItem.FindControl("lblEmployeeId")).Text;
                Response.Redirect("..\\Crew\\CrewCorrespondenceEmail.aspx?email=true&cid=" + Correspondenceid+ "&empid="+empid, false);
            }
        }
        if (e.CommandName.ToUpper() == "LOCKUNLOCK")
        {
          
        }
        if (e.CommandName.ToUpper() == "ROWCLICK" || e.CommandName.ToUpper() == "SELECT")
        {
            string Correspondenceid = ((RadLabel)editedItem.FindControl("lblCorrespondenceId")).Text;
            string empid = ((RadLabel)editedItem.FindControl("lblEmployeeId")).Text;

            EditCorrespondence(Correspondenceid, empid);

        }
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvCorrespondence_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            string id = ((RadLabel)e.Item.FindControl("lblCorrespondenceId")).Text;

            PhoenixCrewCorrespondence.DeleteCorrespondence(Convert.ToInt32(id), int.Parse(ViewState["EMPID"].ToString()));
            BindData();
            gvCorrespondence.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvCorrespondence_EditCommand(object sender, GridCommandEventArgs e)
    {
        try
        {   
            string id = ((RadLabel)e.Item.FindControl("lblCorrespondenceId")).Text;
            string empid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
            

            EditCorrespondence(id, empid);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    

    private void ResetFields()
    {
        try
        {
            ViewState["CORRESPONDENCEID"] = string.Empty;
            txtFrom.Text = string.Empty;
            txtTO.Text = string.Empty;
            txtSubject.Text = string.Empty;
            txtBody.Content = string.Empty;
            ddlCorrespondenceType.SelectedQuick = string.Empty;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    public bool IsValidCorrespondence(string subject, string type)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int resultInt;

        if (string.IsNullOrEmpty(subject))
            ucError.ErrorMessage = "Subject is required.";

        if (!int.TryParse(type, out resultInt))
            ucError.ErrorMessage = "Correspondence Type is required.";

        if (txtFrom.Text.Trim() != string.Empty && !General.IsvalidEmail(txtFrom.Text))
            ucError.ErrorMessage = "Please enter valid From E-Mail Address";

        if ((txtTO.Text.Trim() != string.Empty || txtTO.CssClass == "input_mandatory") && !General.IsvalidEmail(txtTO.Text))
            ucError.ErrorMessage = "Please enter valid To E-Mail Address";

        return (!ucError.IsError);

    }

    private void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(ViewState["EMPID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            string correspondenceid = (ViewState["CORRESPONDENCEID"] == null) ? null : (ViewState["CORRESPONDENCEID"].ToString());
            string strEmployeeId = (ViewState["EMPID"] == null) ? null : (ViewState["EMPID"].ToString());


            if (Session["AttachFiles"] != null)
            {
                lstAttachments.Items.Clear();
                DataTable dt = (DataTable)Session["AttachFiles"];
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    lstAttachments.Items.Add(new ListItem(dt.Rows[i]["Text"].ToString(), dt.Rows[i]["Value"].ToString()));
                }
                Session["AttachFiles"] = null;
            }
            else if (!trFrom.Visible && Session["corres"] != null && Session["corres"].ToString() == "1")
            {
                EditCorrespondence(correspondenceid, strEmployeeId);
            }
            else
                ResetFields();

            BindData();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void gvCorrespondence_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GridDataItem item = (GridDataItem)gvCorrespondence.SelectedItems[0]; //get selected row
    }
    
}
