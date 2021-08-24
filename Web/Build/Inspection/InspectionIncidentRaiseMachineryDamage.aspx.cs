using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using System.Collections.Specialized;
using Telerik.Web.UI;
public partial class InspectionIncidentRaiseMachineryDamage : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Raise", "RAISE", ToolBarDirection.Right);
        MenuLocation.AccessRights = this.ViewState;
        MenuLocation.MenuList = toolbar.Show();

        if (!IsPostBack)
        {            
            if (Request.QueryString["INCIDENTID"] != null && Request.QueryString["INCIDENTID"].ToString() != "")
                ViewState["INCIDENTID"] = Request.QueryString["INCIDENTID"].ToString();
            else
                ViewState["INCIDENTID"] = "";

            ViewState["COMPANYID"] = "";

            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;

            if (nvc != null && nvc.Get("QMS") != null && nvc.Get("QMS") != "")
            {
                ViewState["COMPANYID"] = nvc.Get("QMS");
            }

            ucCategory.TypeId = "4";
            ucCategory.CategoryList = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(General.GetNullableInteger("4"));
            ucCategory.DataBind();
        }

    }

    protected void MenuLocation_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("RAISE"))
            {
                string strMsg = "";

                if (General.GetNullableGuid(ucCategory.SelectedCategory) == null)
                    strMsg = strMsg + "* Category is required." + System.Environment.NewLine;

                if (General.GetNullableGuid(ucSubcategory.SelectedSubCategory) == null)
                    strMsg = strMsg + "* Sub Category is required.";

                if (General.GetNullableString(strMsg) != null)
                {
                    lblMessage.Text = strMsg;
                    lblMessage.ForeColor = System.Drawing.Color.Red;
                    lblMessage.Visible = true;
                    return;
                }

                if (ViewState["COMPANYID"].ToString() != "")
                {
                    PhoenixInspectionIncident.RaiseMachineryDamage(new Guid(Filter.CurrentIncidentID),General.GetNullableInteger(ViewState["COMPANYID"].ToString()),General.GetNullableGuid(ucCategory.SelectedCategory),General.GetNullableGuid(ucSubcategory.SelectedSubCategory));

                    lblMessage.Text = "Machinery / Damage Failure is raised.";
                    lblMessage.ForeColor = System.Drawing.Color.Blue;
                    lblMessage.Visible = true;

                    String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
                }
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = ex.Message;
            lblMessage.ForeColor = System.Drawing.Color.Red;
            lblMessage.Visible = true;
            return;
        }
    }

    private bool IsValidDetails()
    {
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableGuid(ucCategory.SelectedCategory) == null)
            ucError.ErrorMessage = "Category is required.";

        if (General.GetNullableGuid(ucSubcategory.SelectedSubCategory) == null)
            ucError.ErrorMessage = "Sub Category is required.";

        return (!ucError.IsError);
    }

    protected void ucCategory_Changed(object sender, EventArgs e)
    {
        ucSubcategory.SelectedSubCategory = "";
        ucSubcategory.CategoryId = ucCategory.SelectedCategory;
        ucSubcategory.SubCategoryList = PhoenixInspectionIncidentNearMissSubCategory.ListIncidentNearMissSubCategory(General.GetNullableGuid(ucCategory.SelectedCategory));
        ucSubcategory.DataBind();
    }
}
