using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.IO;
using System.Web;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;


public partial class CrewNewApplicantCorrespondence : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddFontAwesomeButton("../Crew/CrewNewApplicantCorrespondence.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbargrid.AddFontAwesomeButton("javascript:CallPrint('gvCorrespondence')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbargrid.AddFontAwesomeButton("javascript:openNewWindow('CI','','" + Session["sitepath"] + "/Crew/CrewCorrespondenceFilter.aspx'); return false;", "Filter", "<i class=\"fas fa-search\"></i>", "FIND");
            MenuCrewCorrespondence.AccessRights = this.ViewState;
            MenuCrewCorrespondence.MenuList = toolbargrid.Show();

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            toolbar.AddButton("New", "NEW", ToolBarDirection.Right);


            MenuCorrespondence.AccessRights = this.ViewState;
            MenuCorrespondence.MenuList = toolbar.Show();
            if (!IsPostBack)
            {
                cmdHiddenSubmit.Attributes.Add("style", "display:none;");
                RemoveEditorToolBarIcons(); // remove the unwanted icons in editor

                string strEmployeeId = string.Empty;
                if (string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection) && string.IsNullOrEmpty(Request.QueryString["empid"]))
                    strEmployeeId = string.Empty;
                else if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
                    strEmployeeId = Request.QueryString["empid"];
                else if (!string.IsNullOrEmpty(Filter.CurrentNewApplicantSelection))
                    strEmployeeId = Filter.CurrentNewApplicantSelection;

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
            //BindData();

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
                                                                    , txtFrom.Text, txtTO.Text, txtCC.Text, txtSubject.Text, txtBody.Content, 1);
                }
                else
                {
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
                    gvCorrespondence.Rebind();
                    return;
                }
                string dtkey = string.Empty;              
                string path = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath + "/EmailAttachments/" + ViewState["mailsessionid"].ToString();
                DataSet ds = PhoenixCrewCorrespondence.InsertCorrespondence(int.Parse(ViewState["EMPID"].ToString()), int.Parse(ddlCorrespondenceType.SelectedQuick)
                                                                , txtFrom.Text, txtTO.Text, txtCC.Text, txtSubject.Text, txtBody.Content, null);
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
                            //string desgpath = HttpContext.Current.Request.MapPath("~/");
                            //desgpath = desgpath + "Attachments/Crew/" + tempkey + f.Extension;

                            string desgpath = PhoenixGeneralSettings.CurrentGeneralSetting.AttachmentPath;
                            desgpath = desgpath + "/Crew/" + tempkey + f.Extension;

                            f.CopyTo(desgpath, true);

                        }
                    }
                }
                PhoenixMail.SendMail(txtTO.Text, txtCC.Text, txtCC.Text, txtSubject.Text, txtBody.Content
                               , true, System.Net.Mail.MailPriority.Normal, ViewState["mailsessionid"].ToString(), General.GetNullableInteger(ViewState["EMPID"].ToString()));

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

                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar.AddButton("New", "NEW", ToolBarDirection.Right);

                MenuCorrespondence.MenuList = toolbar.Show();

                txtSubject.CssClass = "input_mandatory";
                txtSubject.ReadOnly = false;
                ddlCorrespondenceType.Enabled = true;

                txtBody.Visible = true;
                txtBody.EditModes = EditModes.All;
                ddlEmailTemplate.Enabled = true;
                gvCorrespondence.Rebind();
            }
            //BindData();
            //gvCorrespondence.Rebind();
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

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;
            string[] alColumns = { "FLDCREATEDDATE", "FLDSUBJECT", "FLDCORRESPONDENCETYPENAME", "FLDCREATEDBYNAME", "FLDTYPEOF" };
            string[] alCaptions = { "Date", "Subject", "Type", "User", "Corres/Email" };
            NameValueCollection nvc = Filter.CurrentCorrespondenceFilter;
            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 1;
            DataTable dt = new DataTable();
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
            if (Filter.CurrentCorrespondenceFilter != null)
            {
                nvc["filter"] = "0";
                dt = PhoenixCrewCorrespondence.SearchCorrespondence(Convert.ToInt32(ViewState["EMPID"].ToString()), sortexpression, sortdirection,
                                                                                         (int)ViewState["PAGENUMBER"],
                                                                                         gvCorrespondence.PageSize,
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
                                                                              gvCorrespondence.PageSize,
                                                                             ref iRowCount,
                                                                             ref iTotalPageCount,
                                                                             null, null);
            }

            DataSet ds = new DataSet();
            ds.Tables.Add(dt.Copy());
            General.SetPrintOptions("gvCorrespondence", "Correspondence", alCaptions, alColumns, ds);

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
    
    private void ResetFields()
    {
        try
        {

            txtFrom.Text = string.Empty;
            txtTO.Text = string.Empty;
            txtSubject.Text = string.Empty;
            txtBody.Content = string.Empty;
            ddlCorrespondenceType.SelectedQuick = string.Empty;
            ddlEmailTemplate.SelectedTemplate = string.Empty;
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
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(ViewState["EMPID"].ToString()));

            if (dt.Rows.Count > 0)
            {
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
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
            gvCorrespondence.Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
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
                gvCorrespondence.Rebind();
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
    protected void ShowExcel()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string[] alColumns = { "FLDCREATEDDATE", "FLDSUBJECT", "FLDCORRESPONDENCETYPENAME", "FLDCREATEDBYNAME", "FLDTYPEOF" };
            string[] alCaptions = { "Date", "Subject", "Type", "User", "Corres/Email" };

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
                dt = PhoenixCrewCorrespondence.SearchCorrespondence(Convert.ToInt32(ViewState["EMPID"].ToString()), sortexpression, sortdirection,
                                                                                         (int)ViewState["PAGENUMBER"],
                                                                                         iRowCount,
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
                                                                             iRowCount,
                                                                             ref iRowCount,
                                                                             ref iTotalPageCount,
                                                                             null, null);
            }

            Response.AddHeader("Content-Disposition", "attachment; filename=NewApplicantCorrespondence.xls");
            Response.ContentType = "application/vnd.msexcel";
            Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
            Response.Write("<tr>");
            Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
            Response.Write("<td><h3>Correspondence</h3></td>");
            Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
            Response.Write("</tr>");
            Response.Write("</TABLE>");
            Response.Write("<br />");
            Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
            Response.Write("<tr>");
            for (int i = 0; i < alCaptions.Length; i++)
            {
                Response.Write("<td width='20%'>");
                Response.Write("<b>" + alCaptions[i] + "</b>");
                Response.Write("</td>");
            }
            Response.Write("</tr>");
            foreach (DataRow dr in dt.Rows)
            {
                Response.Write("<tr>");
                for (int i = 0; i < alColumns.Length; i++)
                {
                    Response.Write("<td>");
                    Response.Write(dr[alColumns[i]].GetType().Equals(typeof(DateTime)) ? General.GetDateTimeToString(dr[alColumns[i]].ToString()) : dr[alColumns[i]]);
                    Response.Write("</td>");
                }
                Response.Write("</tr>");
            }
            Response.Write("</TABLE>");
            Response.End();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ddlEmailTemplate_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetEmailTemplate(General.GetNullableInteger(ddlEmailTemplate.SelectedTemplate));
    }
    protected void SetEmailTemplate(int? iTempldateId)
    {
        try
        {
            if (iTempldateId.HasValue)
            {
                DataSet ds = PhoenixCommonEmailTemplate.ListEmailTemplate(null, iTempldateId);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    txtBody.Content = ds.Tables[0].Rows[0]["FLDTEMPLATE"].ToString();
                    txtSubject.Text = ds.Tables[0].Rows[0]["FLDSUBJECT"].ToString();
                }
                DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(ViewState["EMPID"].ToString()));
                if (dt.Rows.Count > 0)
                {
                    txtBody.Content = txtBody.Content.Replace("#FIRSTNAME#", dt.Rows[0]["FLDFIRSTNAME"].ToString()).Replace("#MIDDLENAME#", dt.Rows[0]["FLDMIDDLENAME"].ToString())
                        .Replace("#LASTNAME#", dt.Rows[0]["FLDLASTNAME"].ToString()).Replace("#PORTALLINK#", Session["sitepath"] + "/Portal/PortalSeafarerDefault.aspx");
                    txtSubject.Text = txtSubject.Text + " " + dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void gvCorrespondence_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCorrespondence.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCorrespondence_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper() == "EMAIL")
        {
            string Correspondenceid = ((RadLabel)e.Item.FindControl("lblCorrespondenceId")).Text;
            if (Correspondenceid != "")
            {
                Response.Redirect("..\\Crew\\CrewNewApplicantCorrespondenceEmail.aspx?email=true&cid=" + Correspondenceid, false);
            }
        }
        if (e.CommandName.ToUpper() == "ROWCLICK" || e.CommandName.ToUpper() == "SELECT")
        {
            string Correspondenceid = ((RadLabel)e.Item.FindControl("lblCorrespondenceId")).Text;
            string empid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;

            EditCorrespondence(Correspondenceid, empid);

        }


    }

    protected void gvCorrespondence_EditCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            string id = ((RadLabel)e.Item.FindControl("lblCorrespondenceId")).Text;
            string empid = ((RadLabel)e.Item.FindControl("lblEmployeeId")).Text;
            e.Item.Selected = true;
            EditCorrespondence(id, empid);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCorrespondence_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
            if (db != null) db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            LinkButton att = (LinkButton)e.Item.FindControl("cmdAtt");

            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");

            if (att != null)
            {
                string strlock = ((RadLabel)e.Item.FindControl("lblLockYN")).Text;

                if (!SessionUtil.CanAccess(this.ViewState, att.CommandName)) att.Visible = false;

                if (lblIsAtt.Text == string.Empty)
                {
                    HtmlGenericControl html = new HtmlGenericControl();
                    html.InnerHtml = "<span class=\"icon\" style=\"color:gray\"><i class=\"fas fa-paperclip-na\"></i></span>";
                    att.Controls.Add(html);
                }
                att.Attributes.Add("onclick", "javascript:openNewWindow('CI','','" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey=" + lblDTKey.Text + "&mod="
                              + PhoenixModule.CREW + "&type=" + PhoenixCrewAttachmentType.CORRESPONDENCE + (strlock == "1" ? "&U=N" : string.Empty) + "&cmdname=NCORRESUPLOAD'); return false;");
            }
          
            LinkButton chkMail = (LinkButton)e.Item.FindControl("lnkCorrespondence");

            if (drv["FLDTYPE"].ToString() != "1")
            {
                chkMail.Attributes["target"] = "filterandsearch";
                chkMail.Attributes["href"] = "CrewNewApplicantPersonalGeneral.aspx?email=true&cid=" + drv["FLDCORRESPONDENCEID"].ToString();
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


            }

        }

    }

    protected void gvCorrespondence_DeleteCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            string id = ((RadLabel)e.Item.FindControl("lblCorrespondenceId")).Text;

            PhoenixCrewCorrespondence.DeleteCorrespondence(Convert.ToInt32(id), int.Parse(ViewState["EMPID"].ToString()));
            gvCorrespondence.Rebind();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvCorrespondence_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}