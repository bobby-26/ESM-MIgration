using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class HR_PayRollSGSDLAdd : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        ShowToolBar();
    }
    public void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain = new PhoenixToolbar();

        toolbarmain.AddButton("Save", "SAVE", ToolBarDirection.Right);


        gvTabStrip.MenuList = toolbarmain.Show();
    }

    protected void gvTabStrip_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (IsValidInput())
            {
                ucError.Visible = true;
                return;
            }



            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixPayRollSingapore.PayrollSingaporeSDLInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableInteger(Request.QueryString["payrollid"])
                    , General.GetNullableDecimal(rademployercontribution.Text)
                    , General.GetNullableDecimal(radminimum.Text)
                    , General.GetNullableDecimal(radmaximum.Text));
            }



            String script = String.Format("javascript:fnReloadList('codehelp1','ifMoreInfo',null);");
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "script", script, true);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public bool IsValidInput()
    {
        ucError.HeaderMessage = "Please provide the following ";
        if (General.GetNullableDecimal(rademployercontribution.Text) == null)
        {
            ucError.ErrorMessage = "Employer contribution percentage of Total Wage ";
        }
        if (General.GetNullableDecimal(radminimum.Text) == null)
        {
            ucError.ErrorMessage = "Minimum Contribution towards SDL  ";
        }
        if (General.GetNullableDecimal(radmaximum.Text) == null)
        {
            ucError.ErrorMessage = "Maximum contribution towards SDL ";
        }

        if (General.GetNullableInteger(radminimum.Text) != null & General.GetNullableInteger(radmaximum.Text) != null)
        {
            if (!(General.GetNullableInteger(radmaximum.Text) > General.GetNullableInteger(radminimum.Text)))
            {
                ucError.ErrorMessage = "Maximum contribution should be greater than minimum contribution.";
            }
        }
       
        return ucError.IsError;
    }
}