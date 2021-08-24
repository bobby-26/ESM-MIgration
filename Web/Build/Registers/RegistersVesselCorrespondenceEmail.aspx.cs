using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using System.IO;
using System.Web;
using Telerik.Web.UI;
public partial class RegistersVesselCorrespondenceEmail : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Discard", "DISCARD", ToolBarDirection.Right);
            toolbar.AddButton("Send", "SEND", ToolBarDirection.Right);
            MenuVesselCorrespondenceMail.AccessRights = this.ViewState;
            MenuVesselCorrespondenceMail.MenuList = toolbar.Show();

            if (General.GetNullableInteger(Filter.CurrentVesselMasterFilter) == null)
            {
                ucError.ErrorMessage = "Select a Vessel and Continue";
                ucError.Visible = true;
                return;
            }

            if (!IsPostBack)
            {
                DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(Filter.CurrentVesselMasterFilter));

                if (ds.Tables[0].Rows.Count > 0)
                    txtVesselName.Text = ds.Tables[0].Rows[0]["FLDVESSELNAME"].ToString();

                ViewState["CURRENTINDEX"] = -1;
                ViewState["CORRESPONDENCEID"] = Request.QueryString["cid"].ToString();
                ViewState["mailsessionid"] = string.Empty;

                txtDate.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);

                EditCorrespondence(ViewState["CORRESPONDENCEID"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuVesselCorrespondenceMail_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SEND"))
            {
                if (!IsValidCorrespondence(txtSubject.Text, ddlCorrespondenceType.SelectedQuick))
                {
                    ucError.Visible = true;
                    return;
                }
                string dtkey = string.Empty;
                string path = Server.MapPath("~/Attachments/EmailAttachments/" + ViewState["mailsessionid"].ToString()).ToString();

                DataSet ds = PhoenixRegistersVesselCorrespondence.InsertCorrespondence(
                    int.Parse(Filter.CurrentVesselMasterFilter)
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
                                , PhoenixSecurityContext.CurrentSecurityContext.VesselID, null, PhoenixCrewAttachmentType.CORRESPONDENCE.ToString(), new Guid(tempkey));
                            string desgpath = HttpContext.Current.Request.MapPath("~/");
                            desgpath = desgpath + "Attachments/Crew/" + tempkey + f.Extension;
                            f.CopyTo(desgpath, true);

                        }
                    }
                }

                PhoenixMail.SendMail(txtTO.Text, txtCC.Text, txtCC.Text, txtSubject.Text, txtBody.Content
                               , true, System.Net.Mail.MailPriority.Normal, ViewState["mailsessionid"].ToString()
                               , General.GetNullableInteger(Filter.CurrentVesselMasterFilter));
            }
            else
            {
                ResetFields();
            }
            txtBody.EditModes = EditModes.All;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditCorrespondence(string correspondenceid)
    {
        DataTable dt = PhoenixRegistersVesselCorrespondence.EditCorrespondence(General.GetNullableInteger(correspondenceid).Value);

        if (dt.Rows.Count > 0)
        {
            ViewState["CORRESPONDENCEID"] = dt.Rows[0]["FLDCORRESPONDENCEID"].ToString();
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Send", "SEND");
            toolbar.AddButton("Discard", "DISCARD");
            toolbar.AddImageLink("javascript:parent.Openpopup('CI','','../Registers/RegistersVesselCorrespondenceLockAndUnlock.aspx?id="
                + dt.Rows[0]["FLDCORRESPONDENCEID"].ToString() + "&vesselid=" + Filter.CurrentVesselMasterFilter
                + "','medium'); return false;"
                , dt.Rows[0]["FLDLOCKEDYN"].ToString() == "0" ? "Lock" : "UnLock", "", dt.Rows[0]["FLDLOCKEDYN"].ToString() == "0" ? "LOCK" : "UNLOCK");

            MenuVesselCorrespondenceMail.MenuList = toolbar.Show();
            txtFrom.Text = dt.Rows[0]["FLDFROM"].ToString();
            txtTO.Text = dt.Rows[0]["FLDTO"].ToString();
            txtSubject.Text = dt.Rows[0]["FLDSUBJECT"].ToString();
            txtBody.Content = dt.Rows[0]["FLDBODY"].ToString();
            txtDate.Text = String.Format("{0:dd/MMM/yyyy}", General.GetNullableDateTime(dt.Rows[0]["FLDCREATEDDATE"].ToString()));
            ddlCorrespondenceType.SelectedQuick = dt.Rows[0]["FLDCORRESPONDENCETYPE"].ToString();
            ViewState["DTKEY"] = dt.Rows[0]["FLDDTKEY"].ToString();
            txtBody.EditModes = EditModes.Preview;
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
}
