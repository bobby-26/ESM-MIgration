using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Data;
using System.IO;
using SouthNests.Phoenix.Accounts;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;

public partial class Accounts_AccountsDiamondSpecificReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            DiamondSpecific.Title = "Diamond Specific Report";
            DiamondSpecific.AccessRights = this.ViewState;
            DiamondSpecific.MenuList = toolbargrid.Show();
            cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            if (!IsPostBack)
            {
                ucOwner.SelectedAddress = "757";                
            }
            ucOwner.Enabled = false;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

  
    protected void DiamondSpecific_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        try
        {
            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "ExportUACC('" + 33 + "','" + 193 + "','" + 572 + "','" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "','UACC');", true);

                PhoenixAccountsDiamondReport2XL.Export2XLDiamond(General.GetNullableInteger(ucFinancialYear.SelectedValue), General.GetNullableInteger(ucOwner.SelectedAddress), 0);
                //Export2XLDiamond(General.GetNullableInteger(ucFinancialYear.SelectedValue), General.GetNullableInteger(ucOwner.SelectedAddress), 0);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
