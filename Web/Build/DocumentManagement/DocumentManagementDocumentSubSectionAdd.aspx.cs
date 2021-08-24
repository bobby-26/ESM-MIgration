using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;

public partial class DocumentManagementDocumentSubSectionAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        PhoenixToolbar toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("Add", "ADD", ToolBarDirection.Right);
        MenuLocation.MenuList = toolbarmain.Show();

        if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != string.Empty)
            ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();
        else
            ViewState["DOCUMENTID"] = "";

        if (Request.QueryString["SECTIONID"] != null && Request.QueryString["SECTIONID"].ToString() != string.Empty)
            ViewState["SECTIONID"] = Request.QueryString["SECTIONID"].ToString();
        else
            ViewState["SECTIONID"] = "";
    }

    protected void MenuLocation_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (General.GetNullableGuid(ViewState["DOCUMENTID"].ToString()) == null)
            {
                lblMessage.Text = "Document is not selected.";
                return;
            }
            if (General.GetNullableGuid(ViewState["SECTIONID"].ToString()) == null)
            {
                lblMessage.Text = "Section is not selected.";
                return;
            }
            if (General.GetNullableString(txtSectionNameAdd.Text) == null)
            {
                lblMessage.Text = "Name is required.";
                return;
            }

            if (ViewState["DOCUMENTID"] != null && ViewState["SECTIONID"] != null)
            {
                if (CommandName.ToUpper().Equals("ADD"))
                {
                    Guid? newsectionid = Guid.Empty;

                    PhoenixDocumentManagementDocumentSection.DocumentSectionInsert(
                                  PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                , txtSectionNameAdd.Text
                                , null
                                , new Guid(ViewState["SECTIONID"].ToString())
                                , new Guid(ViewState["DOCUMENTID"].ToString())
                                , chkActiveYNAdd.Checked == true ? 1 : 0
                                , null
                                , txtRevisionnumberAdd.Text
                                , ref newsectionid
                                );

                    Reset();

                    lblMessage.Text = "Sub Section is added.";
                    lblMessage.ForeColor = System.Drawing.Color.Blue;
                    lblMessage.Visible = true;

                    //String script = String.Format("SetParentHiddenKey('" + newsectionid.ToString() + "'); javascript:fnReloadList('code1');");
                    //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                    string scriptRefreshDontClose = "";
                    scriptRefreshDontClose += "<script language='javaScript' id='View'>" + "\n";
                    scriptRefreshDontClose += "fnReloadList('VesselOtherCompany');";
                    scriptRefreshDontClose += "</script>" + "\n";

                    //Page.ClientScript.RegisterStartupScript(typeof(Page), "View", scriptRefreshDontClose, false);                   
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "View", scriptRefreshDontClose, false);

                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "* " + ex.Message;
        }
    }

    protected void Reset()
    {
        txtSectionNameAdd.Text = "";
        //txtSectionNumberAdd.Text = "";
        chkActiveYNAdd.Checked = true;
    }
}
