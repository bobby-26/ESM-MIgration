using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class HR_PayRollSGETFAdd : PhoenixBasePage
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
                PhoenixPayRollSingapore.PayrollSingaporeETFInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                    , General.GetNullableString(radtbshortcode.Text)
                    , General.GetNullableString(radtbname.Text)
                    , General.GetNullableString(radtbdescription.Text)
                    , General.GetNullableInteger(ddlhard.SelectedHard)
                  );
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
        if (General.GetNullableString(radtbname.Text) == null)
        {
            ucError.ErrorMessage = "Name of the Self Help Group fund. ";
        }
        if (General.GetNullableString(radtbshortcode.Text) == null)
        {
            ucError.ErrorMessage = "Short code. ";
        }
        if (General.GetNullableInteger(ddlhard.SelectedHard) == null)
        {
            ucError.ErrorMessage = "Race of Employee ";
        }
       
        return ucError.IsError;
    }

}