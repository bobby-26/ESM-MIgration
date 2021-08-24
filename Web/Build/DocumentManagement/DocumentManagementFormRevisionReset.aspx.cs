using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.DocumentManagement;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class DocumentManagementFormRevisionReset : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        confirm.Attributes.Add("style", "display:none;");
        if (!IsPostBack)
        {
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Reset", "RESET", ToolBarDirection.Right);

            MenuDocument.AccessRights = this.ViewState;
            MenuDocument.MenuList = toolbar.Show();

            if (Request.QueryString["formid"] != null && Request.QueryString["formid"].ToString() != "")
                ViewState["formid"] = Request.QueryString["formid"].ToString();
            else
                ViewState["formid"] = "";

            FormEdit();

            //BindCategory();
        }
    }

    //protected void BindCategory()
    //{
    //    ddlCategory.DataSource = PhoenixDocumentManagementCategory.ListDocumentCategory();
    //    ddlCategory.DataTextField = "FLDCATEGORYNAME";
    //    ddlCategory.DataValueField = "FLDCATEGORYID";
    //    ddlCategory.DataBind();

    //    ddlCategory.Items.Insert(0, new ListItem("--Select--", "Dummy"));
    //}

    protected void FormEdit()
    {
        DataSet ds = PhoenixDocumentManagementForm.FormEdit(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , new Guid(ViewState["formid"].ToString()));
        if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            txtFormNo.Text = ds.Tables[0].Rows[0]["FLDFORMNO"].ToString();
            txtFormName.Text = ds.Tables[0].Rows[0]["FLDCAPTION"].ToString();
        }
    }
    protected void MenuDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("RESET"))
            {
                RadWindowManager1.RadConfirm("Do you want to reset the revisions.?", "confirm", 320, 150, null, "Confirm");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void confirm_Click(object sender, EventArgs e)
    {
        try
        {

            PhoenixDocumentManagementForm.FormRevisionReset(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                ViewState["formid"].ToString(),
                General.GetNullableString(null),
                General.GetNullableDateTime(txtRevisionDate.Text),
                General.GetNullableString(ucRevisionNo.Text),
                null);

            ucStatus.Text = "Revisions have been reset.";
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null, true);", true);

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidReset()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableDateTime(txtRevisionDate.Text) == null)
            ucError.ErrorMessage = "Revision date is required.";
        else
        {
            if (DateTime.Parse(txtRevisionDate.Text) > DateTime.Today)
                ucError.ErrorMessage = "Revision date should not be the future date.";
        }

        if (General.GetNullableInteger(ucRevisionNo.Text) == null)
            ucError.ErrorMessage = "Revision number is required.";

        return (!ucError.IsError);
    }
}
