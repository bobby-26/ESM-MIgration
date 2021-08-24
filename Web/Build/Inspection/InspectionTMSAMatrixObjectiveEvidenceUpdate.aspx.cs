using System;
using System.Data;
using System.Web;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using Telerik.Web.UI;


public partial class Inspection_InspectionTMSAMatrixObjectiveEvidenceUpdate : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);
        MenuOfficeRemarks.AccessRights = this.ViewState;
        MenuOfficeRemarks.MenuList = toolbarmain.Show();

        if (!IsPostBack)
        {
            ViewState["CATEGORYID"] = null;
            ViewState["FLDCONTENTID"] = null;
            ViewState["INSPECTIONID"] = null;

            if (Request.QueryString["categoryid"] != null)
                ViewState["CATEGORYID"] = Request.QueryString["categoryid"].ToString();

            if (Request.QueryString["contentid"] != null)
                ViewState["FLDCONTENTID"] = Request.QueryString["contentid"].ToString();
            else
                ViewState["FLDCONTENTID"] = null;

            if (Request.QueryString["inspectionid"] != null)
                ViewState["INSPECTIONID"] = Request.QueryString["inspectionid"].ToString();
            else
                ViewState["INSPECTIONID"] = null;

            BindComments();
        }

    }

    protected void MenuOfficeRemarks_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            //if (General.GetNullableString(txtOfficeRemarks.Text) == null)
            //{
            //    lblMessage.Text = "Office comments is required.";
            //    return;
            //}

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixInspectionTMSAMatrix.UpdateTMSAMatrixObjectiveEvidenceUpdate(
                                                     General.GetNullableGuid(ViewState["FLDCONTENTID"].ToString()),
                                                     General.GetNullableGuid(ViewState["CATEGORYID"].ToString()),
                                                     General.GetNullableString(txtOfficeRemarks.Text)
                                                     );
            }
            ucStatus.Text = "Objective Evidence updated";
            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindComments()
    {
        if (ViewState["FLDCONTENTID"] != null)
        {
            DataTable dt = new DataTable();
            dt = PhoenixInspectionTMSAMatrix.EditTMSAMatrixContent(General.GetNullableGuid(ViewState["CATEGORYID"].ToString()),
                    General.GetNullableGuid(ViewState["FLDCONTENTID"].ToString()));
            if (dt.Rows.Count > 0)
            {
                txtOfficeRemarks.Text = dt.Rows[0]["FLDOBJECTIVEEVIDENCE"].ToString();
            }
        }
    }
}