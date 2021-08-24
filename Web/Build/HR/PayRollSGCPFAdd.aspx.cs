using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class HR_PayRollSGCPFAdd : PhoenixBasePage
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
                PhoenixPayRollSingapore.PayrollSingaporeCPFInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableInteger(Request.QueryString["payrollid"])
                    , General.GetNullableInteger(radtbminimumage.Text)
                    , General.GetNullableInteger(radtblmaximumage.Text)
                    , General.GetNullableDecimal(radminimumtw.Text)
                    , General.GetNullableDecimal(radmaximumtw.Text)
                    , General.GetNullableDecimal(rademployerow.Text)
                    ,General.GetNullableDecimal(rademployeraw.Text)
                    ,General.GetNullableDecimal(rademployeeow.Text)
                    ,General.GetNullableDecimal(rademployeeaw.Text)
                    ,General.GetNullableDecimal(rademployeecorrectionfactor.Text)
                    ,General.GetNullableDecimal(rademployerowlimit.Text)
                    ,General.GetNullableDecimal(rademployeeowlimit.Text));
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
        if (General.GetNullableInteger(radtbminimumage.Text) == null)
        {
            ucError.ErrorMessage = "Minimum Age ";
        }
        if (General.GetNullableInteger(radtblmaximumage.Text) == null)
        {
            ucError.ErrorMessage = "Maximum Age ";
        }
        if (General.GetNullableInteger(radminimumtw.Text) == null)
        {
            ucError.ErrorMessage = "Minimum Total Wage ";
        }
        
        if (General.GetNullableInteger(radtbminimumage.Text) != null & General.GetNullableInteger(radtblmaximumage.Text) != null)
        {
            if(!(General.GetNullableInteger(radtblmaximumage.Text) > General.GetNullableInteger(radtbminimumage.Text)))
            {
                ucError.ErrorMessage = "Maximum age should be greater than minimum age.";
            }
        }
        if (General.GetNullableInteger(radminimumtw.Text) != null & General.GetNullableInteger(radmaximumtw.Text) != null)
        {
            if (!(General.GetNullableInteger(radmaximumtw.Text) > General.GetNullableInteger(radminimumtw.Text)))
            {
                ucError.ErrorMessage = "Maximum total wage should be greater than minimum total wage.";
            }
        }
        return ucError.IsError;
    }
}