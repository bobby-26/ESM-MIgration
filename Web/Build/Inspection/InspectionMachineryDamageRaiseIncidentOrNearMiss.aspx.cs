using System;
using System.Data;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;
public partial class InspectionMachineryDamageRaiseIncidentOrNearMiss : PhoenixBasePage
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
            if (Request.QueryString["INCIDENTORNEARMISS"] != null && Request.QueryString["INCIDENTORNEARMISS"].ToString() != "")
                ViewState["INCIDENTORNEARMISS"] = Request.QueryString["INCIDENTORNEARMISS"].ToString();
            else
                ViewState["INCIDENTORNEARMISS"] = "";

            if (Request.QueryString["MACHINERYDAMAGEID"] != null && Request.QueryString["MACHINERYDAMAGEID"].ToString() != "")
                ViewState["MACHINERYDAMAGEID"] = Request.QueryString["MACHINERYDAMAGEID"].ToString();
            else
                ViewState["MACHINERYDAMAGEID"] = "";

            if (Request.QueryString["VESSELID"] != null && Request.QueryString["VESSELID"].ToString() != "")
                ViewState["VESSELID"] = Request.QueryString["VESSELID"].ToString();
            else
                ViewState["VESSELID"] = "";

            ucCategory.TypeId = ViewState["INCIDENTORNEARMISS"].ToString() == "2" ? "1" : "2";
            ucCategory.CategoryList = PhoenixInspectionIncidentNearMissCategory.ListIncidentNearMissCategory(General.GetNullableInteger(ViewState["INCIDENTORNEARMISS"].ToString()));
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

                PhoenixInspectionMachineryDamage.MachineryDamageStatusUpdate(
                                                                          PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                                                                          General.GetNullableGuid(ViewState["MACHINERYDAMAGEID"].ToString()),
                                                                          General.GetNullableInteger(ViewState["INCIDENTORNEARMISS"].ToString()),
                                                                          null,
                                                                          General.GetNullableGuid(ucCategory.SelectedCategory),
                                                                          General.GetNullableGuid(ucSubcategory.SelectedSubCategory)
                                                                          );

                lblMessage.Text = "Incident / Near miss has been raised successfully.";
                lblMessage.ForeColor = System.Drawing.Color.Blue;
                lblMessage.Visible = true;

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "refreshScript", "javascript:fnReloadList('codehelp1',null, true);", true);
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
