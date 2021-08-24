using System;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using SouthNests.Phoenix.ShippingKPI;
using System.Web.UI;
using System.Data;

public partial class Dashboard_DashboardSKOVMSAEdit : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        PhoenixToolbar toolbargrid = new PhoenixToolbar();

        toolbargrid.AddButton("Update", "Update", ToolBarDirection.Right);
        Tabstripspiaddmenu.MenuList = toolbargrid.Show();
        if (!Page.IsPostBack)
        {
            ViewState["TMSAID"] = General.GetNullableGuid(Request.QueryString["tmsaid"]);

            DataTable dt = PhoemixDashboardSKOVMSA.TMSAElementEdit(General.GetNullableGuid(ViewState["TMSAID"].ToString()));

            radtbidentry.Text = dt.Rows[0]["FLDTMSACODE"].ToString();
            radtbshortcodeentry.Text = dt.Rows[0]["FLDTMSASHORTCODE"].ToString();
            radtbdescriptionentry.Text = dt.Rows[0]["FLDTMSADESCRIPTION"].ToString();

        }
    }

    protected void Tabstripspiaddmenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("UPDATE"))
            {
                int rowusercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
                string id = General.GetNullableString(radtbidentry.Text);
                string shortcode = General.GetNullableString(radtbshortcodeentry.Text);
                string description = General.GetNullableString(radtbdescriptionentry.Text);
                Guid? Tmsaid = General.GetNullableGuid(ViewState["TMSAID"].ToString());
                if (!IsValidTMSAElementDetails(id, shortcode, description))
                {
                    ucError.Visible = true;
                    return;
                }

                PhoemixDashboardSKOVMSA.TMSAElementUpdate(rowusercode, id, shortcode, description,Tmsaid);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(),
                                 "BookMarkScript", "fnReloadList('codehelp1', 'ifMoreInfo', null);", true);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    private bool IsValidTMSAElementDetails(string id, string shortcode, string Description)
    {
        ucError.HeaderMessage = "Provide the following required information";

        if (id == null)
        {
            ucError.ErrorMessage = "TMSA Element ID.";
        }
        if (shortcode == null)
        {
            ucError.ErrorMessage = "TMSA Element Short Code.";
        }
        if (Description == null)
        {
            ucError.ErrorMessage = "TMSA Element Description.";
        }

        return (!ucError.IsError);
    }
}