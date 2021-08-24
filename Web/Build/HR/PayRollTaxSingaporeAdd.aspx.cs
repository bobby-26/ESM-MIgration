using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PayRoll;
using System;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PayRoll_PayRollTaxSingapore : PhoenixBasePage
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


            if (IsValidReport())
            {
                ucError.Visible = true;
                return;
            }



            if (CommandName.ToUpper().Equals("SAVE"))
            {
                PhoenixPayRollSingapore.TaxSingaporeInsert(PhoenixSecurityContext.CurrentSecurityContext.UserCode,radtbdescription.Text, General.GetNullableDateTime(ucfromdate.Text), General.GetNullableDateTime(uctodate.Text), General.GetNullableInteger(radddcurrency.SelectedCurrency),General.GetNullableString(radtbchanges.Text));
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

    private bool IsValidReport()
    {
        ucError.HeaderMessage = "Please provide the following ";
        if (string.IsNullOrWhiteSpace(ucfromdate.Text))
        {
            ucError.ErrorMessage = "From date ";
        }
        if (General.GetNullableDateTime(ucfromdate.Text)!= null && General.GetNullableDateTime(uctodate.Text) != null)
        {
            if(!(General.GetNullableDateTime(uctodate.Text) > General.GetNullableDateTime(ucfromdate.Text)))
                ucError.ErrorMessage = "To date should be greater than from date. ";
        }

        if (string.IsNullOrWhiteSpace(radddcurrency.SelectedCurrency))
        {
            ucError.ErrorMessage = "Currency ";
        }

        if (string.IsNullOrWhiteSpace(radtbdescription.Text))
        {
            ucError.ErrorMessage = "Description ";
        }


        return ucError.IsError;
    }
}