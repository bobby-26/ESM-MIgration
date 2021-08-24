using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;
using System.Collections;
using Telerik.Web.UI;
using SouthNests.Phoenix.Inspection;
using SouthNests.Phoenix.Integration;
using SouthNests.Phoenix.Common;


public partial class DocumentManagement_DocumentManagementDocumentSectionAdd : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarmain = new PhoenixToolbar();

            toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
            MenuDocumentSectionGeneral.MenuList = toolbarmain.Show();

            if (!IsPostBack)
            {
                ViewState["STATUS"] = string.Empty;
                ViewState["DOCUMENTID"] = string.Empty;
                ViewState["SECTIONID"] = string.Empty;

                if (Request.QueryString["DOCUMENTID"] != null && Request.QueryString["DOCUMENTID"].ToString() != string.Empty)
                    ViewState["DOCUMENTID"] = Request.QueryString["DOCUMENTID"].ToString();

                if (Request.QueryString["SECTIONID"] != null && Request.QueryString["SECTIONID"].ToString() != string.Empty)
                    ViewState["SECTIONID"] = Request.QueryString["SECTIONID"].ToString();

                BindSection();

            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void BindSection()
    {
        if (ViewState["SECTIONID"].ToString() != string.Empty && ViewState["SECTIONID"] != null)
        {
            DataSet ds = PhoenixDocumentManagementDocumentSection.DocumentSectionEdit(new Guid(ViewState["SECTIONID"].ToString()));
            if (ds.Tables.Count > 0)
            {
                txtSectionName.Text = ds.Tables[0].Rows[0]["FLDSECTIONNAME"].ToString();
                txtSectionNumber.Text = ds.Tables[0].Rows[0]["FLDSECTIONNUMBER"].ToString();

                if (ds.Tables[0].Rows[0]["FLDACTIVEYN"].ToString() == "1")
                {
                    chkActiveYN.Checked = true;
                }
                else
                {
                    chkActiveYN.Checked = false;
                }
                if (ds.Tables[0].Rows[0]["FLDREVISIONSTATUS"].ToString() == "")
                {
                    ViewState["STATUS"] = null;
                    trstatus.Visible = false;
                }
                else
                {
                    ddlRevisionStatus.SelectedValue = ds.Tables[0].Rows[0]["FLDREVISIONSTATUS"].ToString();
                    ViewState["STATUS"] = ds.Tables[0].Rows[0]["FLDREVISIONSTATUS"].ToString();
                }
            }
        }
        else
        {
            trstatus.Visible = false;
        }
    }


    protected void MenuDocumentSectionGeneral_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SAVE"))
        {
            if (!IsValidDocument())
            {
                ucError.Visible = true;
                return;
            }

            if (ViewState["SECTIONID"].ToString() == string.Empty)
            {
                int active = (chkActiveYN.Checked == true) ? 1 : 0;

                PhoenixDocumentManagementDocumentSection.DocumentSectionInsert(
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableString(txtSectionName.Text)
                    , null
                    , null
                    , new Guid(ViewState["DOCUMENTID"].ToString())
                    , active
                    , General.GetNullableString(txtSectionNumber.Text)
                    , null);
            }
            else
            {
                int active = (chkActiveYN.Checked == true) ? 1 : 0;

                PhoenixDocumentManagementDocumentSection.DocumentSectionUpdate(
                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableString(txtSectionName.Text)
                    , new Guid(ViewState["DOCUMENTID"].ToString())
                    , new Guid(ViewState["SECTIONID"].ToString())
                    , active
                    , General.GetNullableString(txtSectionNumber.Text)
                    , General.GetNullableInteger(null)
                    );

            }
            ucStatus.Text = "Section is added.";
            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
    private bool IsValidDocument()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(ViewState["DOCUMENTID"].ToString()) == null)
            ucError.ErrorMessage = "Document is not selected.";

        if (General.GetNullableString(txtSectionNumber.Text) == null)
            ucError.ErrorMessage = "Section is required.";

        if (General.GetNullableString(txtSectionName.Text) == null)
            ucError.ErrorMessage = "Name is required.";

        return (!ucError.IsError);
    }

    protected void ddlRevisionStatus_SelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        ViewState["STATUS"] = ddlRevisionStatus.SelectedValue;
    }
}