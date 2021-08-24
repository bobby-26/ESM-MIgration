using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.IO;
using System.Web;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;
public partial class RegistersVesselCorrespondence : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            if (!IsPostBack)
            {
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar.AddButton("New Mail", "NEWMAIL", ToolBarDirection.Right);
                toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
                MenuCorrespondence.AccessRights = this.ViewState;
                MenuCorrespondence.MenuList = toolbar.Show();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = -1;
                ViewState["CORRESPONDENCEID"] = string.Empty;
                ViewState["mailsessionid"] = string.Empty;
                RemoveEditorToolBarIcons();

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
                if (!IsValidCorrespondence(txtSubject.Text, ddlCorrespondenceType.SelectedQuick, ddlVessel.SelectedVessel))
                {
                    ucError.Visible = true;
                    return;
                }
                if (ViewState["CORRESPONDENCEID"].ToString() == string.Empty)
                {
                    PhoenixRegistersVesselCorrespondence.InsertCorrespondence(General.GetNullableInteger(ddlVessel.SelectedVessel), int.Parse(ddlCorrespondenceType.SelectedQuick)
                                                                    , txtFrom.Text, txtTO.Text, txtSubject.Text, txtBody.Content);
                    ResetFields();
                    Rebind();
                    return;
                }
                else
                {
                    PhoenixRegistersVesselCorrespondence.UpdateCorrespondence(int.Parse(ViewState["CORRESPONDENCEID"].ToString()), General.GetNullableInteger(ddlVessel.SelectedVessel), int.Parse(ddlCorrespondenceType.SelectedQuick)
                                                                        , txtFrom.Text, txtTO.Text, txtSubject.Text, txtBody.Content);
                    ucStatus.Text = "Correspondence Information Updated";
                }
                Rebind();
            }
            else if (CommandName.ToUpper().Equals("NEWMAIL"))
            {
                if (!IsValidVessel(ddlVessel.SelectedVessel))
                {
                    ucError.Visible = true;
                    return;
                }
                DataSet ds = PhoenixRegistersVesselCommunicationDetails.EditCommunicationDetails(General.GetNullableInteger(ddlVessel.SelectedVessel));

                trCC.Visible = true;
                trBCC.Visible = true;
                trAtt.Visible = true;
                trFrom.Visible = true;
                trTO.Visible = true;
                txtTO.CssClass = "input_mandatory";
                if (ds.Tables[0].Rows.Count > 0)
                {
                    txtTO.Text = ds.Tables[0].Rows[0]["FLDEMAIL"].ToString();
                }
                txtSubject.Enabled = true;
                txtSubject.CssClass = "input_mandatory";
                txtSubject.ReadOnly = false;
                ddlCorrespondenceType.Enabled = true;
                ViewState["CORRESPONDENCEID"] = string.Empty;
                //ResetFields();
                txtDate.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);


                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Discard", "DISCARD", ToolBarDirection.Right);
                toolbar.AddButton("Send", "SEND", ToolBarDirection.Right);
                MenuCorrespondence.AccessRights = this.ViewState;
                MenuCorrespondence.MenuList = toolbar.Show();

                Rebind();

                if (lstAttachments.Items.Count > 0)
                {
                    string path = Server.MapPath("~/Attachments/EmailAttachments/" + ViewState["mailsessionid"].ToString()).ToString();
                    if (Directory.Exists(path))
                        Directory.Delete(path, true);
                    lstAttachments.Items.Clear();
                }

                ViewState["mailsessionid"] = System.DateTime.Now.Day.ToString() + "_"
                    + System.DateTime.Now.Month.ToString() + "_"
                    + System.DateTime.Now.Year.ToString() + "_"
                    + System.DateTime.Now.Hour.ToString() + "_"
                    + System.DateTime.Now.Minute.ToString() + "_"
                    + System.DateTime.Now.Second.ToString() + "_"
                    + System.DateTime.Now.Millisecond.ToString();

                lnkAttachment.OnClientClick = "openNewWindow('MailAttachment', '', '" + Session["sitepath"] + "/Options/OptionsAttachment.aspx?mailsessionid=" + ViewState["mailsessionid"] + "', 'xdata');";
                lstAttachments.Attributes["onkeypress"] = "javascript:DeleteFiles(event,'" + ViewState["mailsessionid"] + "');";
            }

            else if (CommandName.ToUpper().Equals("NEW"))
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

                ViewState["mailsessionid"] = string.Empty;

                MenuCorrespondence.Visible = true;
                PhoenixToolbar toolbar = new PhoenixToolbar();
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
                toolbar.AddButton("New Mail", "NEWMAIL", ToolBarDirection.Right);
                toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
                MenuCorrespondence.MenuList = toolbar.Show();

                txtSubject.CssClass = "input_mandatory";
                txtSubject.ReadOnly = false;
                ddlCorrespondenceType.Enabled = true;
                txtBody.EditModes = EditModes.All;
            }
            else
            {
                MenuCommon_TabStripCommand(sender, e);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditCorrespondence(string correspondenceid)
    {
        if (General.GetNullableInteger(correspondenceid) != null)
        {
            DataTable dt = PhoenixRegistersVesselCorrespondence.EditCorrespondence(int.Parse(correspondenceid));

            if (dt.Rows.Count > 0)
            {
                ViewState["CORRESPONDENCEID"] = dt.Rows[0]["FLDCORRESPONDENCEID"].ToString();
                PhoenixToolbar toolbar = new PhoenixToolbar();
                
                toolbar.AddButton("New", "NEW", ToolBarDirection.Right);
                
                MenuCorrespondence.AccessRights = this.ViewState;
                MenuCorrespondence.MenuList = toolbar.Show();

                txtFrom.Text = dt.Rows[0]["FLDFROM"].ToString();
                txtTO.Text = dt.Rows[0]["FLDTO"].ToString();
                txtSubject.Text = dt.Rows[0]["FLDSUBJECT"].ToString();
                txtBody.Content = dt.Rows[0]["FLDBODY"].ToString();
                txtDate.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDCREATEDDATE"].ToString()));
                ddlCorrespondenceType.SelectedQuick = dt.Rows[0]["FLDCORRESPONDENCETYPE"].ToString();
                ViewState["DTKEY"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        txtSubject.CssClass = "readonlytextbox";
        txtSubject.ReadOnly = true;
        ddlCorrespondenceType.Enabled = false;
        txtBody.EditModes = EditModes.Preview;
    }

    private void BindData()
    {
        try
        {
            int iRowCount = 0;
            int iTotalPageCount = 0;

            string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
            int? sortdirection = 1;
            if (ViewState["SORTDIRECTION"] != null)
                sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

            DataTable dt = PhoenixRegistersVesselCorrespondence.SearchCorrespondence(
                General.GetNullableInteger(ddlVessel.SelectedVessel)
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"]
                , gvCorrespondence.PageSize
                , ref iRowCount
                , ref iTotalPageCount);

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
        txtFrom.Text = string.Empty;
        txtTO.Text = string.Empty;
        txtSubject.Text = string.Empty;
        txtBody.Content = string.Empty;
        ddlCorrespondenceType.SelectedQuick = string.Empty;
    }

    public bool IsValidCorrespondence(string subject, string type, string vessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";
        int resultInt;

        if (ddlVessel.SelectedVessel.Equals("") || ddlVessel.SelectedVessel.Equals("Dummy"))
            ucError.ErrorMessage = "Vessel is required.";

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

    public bool IsValidVessel(string Vessel)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (ddlVessel.SelectedVessel.Equals("") || ddlVessel.SelectedVessel.Equals("Dummy"))
            ucError.ErrorMessage = "Vessel is required.";

        return (!ucError.IsError);
    }
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
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
        else if (!trFrom.Visible)
            EditCorrespondence(ViewState["CORRESPONDENCEID"].ToString());
    }

    protected void gvCorrespondence_ItemDataBound(object sender, GridItemEventArgs e)
    {
        ImageButton del = (ImageButton)e.Item.FindControl("cmdDelete");
        if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

        ImageButton atta = (ImageButton)e.Item.FindControl("cmdAtt");
        if (atta != null) atta.Visible = SessionUtil.CanAccess(this.ViewState, atta.CommandName);

        //ImageButton mail = (ImageButton)e.Row.FindControl("cmdMail");
        //if (mail != null) mail.Visible = SessionUtil.CanAccess(this.ViewState, mail.CommandName);

        if (e.Item is GridDataItem)
        {
            //if (!e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Edit)
            //   && !e.Row.RowState.Equals(DataControlRowState.Selected | DataControlRowState.Edit) && !e.Row.RowState.Equals(DataControlRowState.Alternate | DataControlRowState.Selected | DataControlRowState.Edit))
            //{
            DataRowView drv = (DataRowView)e.Item.DataItem;
            RadLabel lblDTKey = (RadLabel)e.Item.FindControl("lblDTKey");
            ImageButton att = (ImageButton)e.Item.FindControl("cmdAtt");
            RadLabel lblIsAtt = (RadLabel)e.Item.FindControl("lblIsAtt");
            if (lblIsAtt.Text == string.Empty) att.ImageUrl = Session["images"] + "/no-attachment.png";

            string strlock = ((RadLabel)e.Item.FindControl("lblLockYN")).Text;
            att.Attributes.Add("onclick", "openNewWindow('CI','', '" + Session["sitepath"] + "/Common/CommonFileAttachment.aspx?dtkey="
                + lblDTKey.Text + "&mod="
                + PhoenixModule.REGISTERS + (strlock == "1" ? "&U=N" : string.Empty) + "'); return false;");

            HtmlAnchor mail = (HtmlAnchor)e.Item.FindControl("cmdMail");

            mail.Attributes.Add("onclick", "openNewWindow('CI','', '" + Session["sitepath"] + "/Registers/RegistersVesselCorrespondenceEmail.aspx?cid="
 + drv["FLDCORRESPONDENCEID"].ToString() + "'); return false;");

            //}
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

                cmdLock.Attributes.Add("onclick", "javascript:openNewWindow('CI','','" + Session["sitepath"] + "/Registers/RegistersVesselCorrespondenceLockAndUnlock.aspx?id=" + drv["FLDCORRESPONDENCEID"].ToString() + " &vesselid="
                        + ddlVessel.SelectedVessel + "','medium'); return false;");

                //   toolbar.AddImageLink("javascript:parent.Openpopup('CI','','../Crew/CrewCorrespondenceLockAndUnlock.aspx?id=" + dt.Rows[0]["FLDCORRESPONDENCEID"].ToString() + "&empid=" + ViewState["EMPID"].ToString() + "','medium'); return false;", dt.Rows[0]["FLDLOCKEDYN"].ToString() == "0" ? "Lock" : "UnLock", "", dt.Rows[0]["FLDLOCKEDYN"].ToString() == "0" ? "LOCK" : "UNLOCK");           
            }
        }
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        // Get the LinkButton control in the first cell
        //LinkButton _doubleClickButton = (LinkButton)e.Row.Cells[0].Controls[0];
        // Get the javascript which is assigned to this LinkButton
        //string _jsDouble = ClientScript.GetPostBackClientHyperlink(_doubleClickButton, "");
        // Add this javascript to the onclick Attribute of the row
        //e.Row.Attributes["onclick"] = _jsDouble;
        //}
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

    protected void gvCorrespondence_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridDataItem item = (GridDataItem)gvCorrespondence.SelectedItems[0];
        int nCurrentRow = item.ItemIndex;

        string id = ((RadLabel)item.FindControl("lblCorrespondenceId")).Text;
        EditCorrespondence(id);

        txtSubject.Text = "RE : " + txtSubject.Text;
        trCC.Visible = true;
        trBCC.Visible = true;
        trAtt.Visible = true;
        trFrom.Visible = true;
        trTO.Visible = true;
        txtTO.CssClass = "input_mandatory";

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Send", "SEND", ToolBarDirection.Right);
        toolbar.AddButton("Discard", "DISCARD", ToolBarDirection.Right);
        MenuCorrespondence.AccessRights = this.ViewState;
        MenuCorrespondence.MenuList = toolbar.Show();

        if (lstAttachments.Items.Count > 0)
        {
            string path = Server.MapPath("~/Attachments/EmailAttachments/" + ViewState["mailsessionid"].ToString()).ToString();
            if (Directory.Exists(path))
                Directory.Delete(path, true);
            lstAttachments.Items.Clear();
        }

        ViewState["mailsessionid"] = System.DateTime.Now.Day.ToString() + "_"
            + System.DateTime.Now.Month.ToString() + "_"
            + System.DateTime.Now.Year.ToString() + "_"
            + System.DateTime.Now.Hour.ToString() + "_"
            + System.DateTime.Now.Minute.ToString() + "_"
            + System.DateTime.Now.Second.ToString() + "_"
            + System.DateTime.Now.Millisecond.ToString();

        lnkAttachment.OnClientClick = "javascript:parent.Openpopup('MailAttachment', '', '../Options/OptionsAttachment.aspx?mailsessionid="
            + ViewState["mailsessionid"] + "', 'xdata');";
        lstAttachments.Attributes["onkeypress"] = "javascript:DeleteFiles(event,'" + ViewState["mailsessionid"] + "');";

        DataTable dt = PhoenixCommonFileAttachment.AttachmentList(new Guid(ViewState["DTKEY"].ToString()));

        if (dt.Rows.Count > 0)
        {
            string dirpath = Server.MapPath("~/Attachments/EmailAttachments/" + ViewState["mailsessionid"].ToString()).ToString();
            if (!Directory.Exists(dirpath))
                Directory.CreateDirectory(dirpath);
            string filepath = string.Empty;
            lstAttachments.Items.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                filepath = Server.MapPath("~/Attachments/" + dt.Rows[i]["FLDFILEPATH"].ToString()).ToString();
                if (File.Exists(filepath))
                {
                    FileInfo fi = new FileInfo(filepath);
                    fi.CopyTo(dirpath + "/"
                        + dt.Rows[i]["FLDFILENAME"].ToString().Substring((dt.Rows[i]["FLDFILENAME"].ToString().LastIndexOf("\\") != -1) ? dt.Rows[i]["FLDFILENAME"].ToString().LastIndexOf("\\") : 0)
                        , true);
                    lstAttachments.Items.Add(new ListItem(dt.Rows[i]["FLDFILENAME"].ToString()));
                }
            }
        }
    }

    protected void gvCorrespondence_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
        if (e.CommandName.ToUpper().Equals("EDIT"))
        {
            string id = ((RadLabel)e.Item.FindControl("lblCorrespondenceId")).Text;
            EditCorrespondence(id);
        }
        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            ViewState["ID"] = ((RadLabel)e.Item.FindControl("lblCorrespondenceId")).Text;
            RadWindowManager1.RadConfirm("Are you sure you want to Delete?", "DeleteRecord", 320, 150, null, "Confirm");
            return;
        }
        if (e.CommandName.ToUpper() == "LOCKUNLOCK")
        {

        }
    }
    protected void Rebind()
    {
        gvCorrespondence.SelectedIndexes.Clear();
        gvCorrespondence.EditIndexes.Clear();
        gvCorrespondence.DataSource = null;
        gvCorrespondence.Rebind();
    }

    protected void ddlVessel_TextChangedEvent(object sender, EventArgs e)
    {
        Rebind();
    }
    protected void MenuCommon_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("NEW"))
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

            ViewState["mailsessionid"] = string.Empty;

            txtSubject.CssClass = "input_mandatory";
            txtSubject.ReadOnly = false;
            ddlCorrespondenceType.Enabled = true;
            txtBody.EditModes = EditModes.All;
        }
        if (CommandName.ToUpper().Equals("SEND"))
        {
            if (!IsValidCorrespondence(txtSubject.Text, ddlCorrespondenceType.SelectedQuick, ddlVessel.SelectedVessel))
            {
                ucError.Visible = true;
                return;
            }
            string dtkey = string.Empty;
            string path = Server.MapPath("~/Attachments/EmailAttachments/" + ViewState["mailsessionid"].ToString()).ToString();

            DataSet ds = PhoenixRegistersVesselCorrespondence.InsertCorrespondence(
                General.GetNullableInteger(ddlVessel.SelectedVessel)
                , int.Parse(ddlCorrespondenceType.SelectedQuick)
                , txtFrom.Text, txtTO.Text, txtSubject.Text, txtBody.Content);

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
                            , ddlVessel.SelectedValue, null, PhoenixCrewAttachmentType.CORRESPONDENCE.ToString(), new Guid(tempkey));
                        string desgpath = HttpContext.Current.Request.MapPath("~/");
                        desgpath = desgpath + "Attachments/Crew/" + tempkey + f.Extension;
                        f.CopyTo(desgpath, true);
                    }
                }
            }

            PhoenixMail.SendMail(txtTO.Text, txtCC.Text, txtCC.Text, txtSubject.Text, txtBody.Content
                           , true, System.Net.Mail.MailPriority.Normal, ViewState["mailsessionid"].ToString()
                           , General.GetNullableInteger(ddlVessel.SelectedVessel));

            Rebind();
        }
        if (CommandName.ToUpper().Equals("DISCARD"))
        {
            if (lstAttachments.Items.Count > 0)
            {
                string path = Server.MapPath("~/Attachments/EmailAttachments/" + ViewState["mailsessionid"].ToString()).ToString();
                if (Directory.Exists(path))
                    Directory.Delete(path, true);
                lstAttachments.Items.Clear();
            }
        }
    }
    protected void ucConfirmDelete_Click(object sender, EventArgs e)
    {
        try
        {
            PhoenixRegistersVesselCorrespondence.DeleteCorrespondence(Convert.ToInt32(ViewState["ID"]), General.GetNullableInteger(ddlVessel.SelectedVessel));
            Rebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
