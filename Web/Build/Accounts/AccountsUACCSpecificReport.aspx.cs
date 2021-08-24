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

public partial class Accounts_AccountsUACCSpecificReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            if (!IsPostBack)
            {
                ucOwner.SelectedAddress = "3817";
                ucOwner_Onchange(null, null);
            }
            PhoenixToolbar toolbargrid = new PhoenixToolbar();
            toolbargrid.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            UACCSpecific.AccessRights = this.ViewState;
            UACCSpecific.MenuList = toolbargrid.Show();
            //   cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            ucOwner.Enabled = false;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    
    protected void UACCSpecific_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (ddlvessel.SelectedValue == "")
                { 
                    ucError.ErrorMessage = "Vessel is required.";
                    ucError.Visible = true;
                    return;
                }
                if (ucFinancialYear.SelectedValue == "Dummy")
                { 
                    ucError.ErrorMessage = "Financial Year is required.";
                    ucError.Visible = true;
                    return;
                }

                //ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "hide", "ExportUACC('" + 33 + "','" + 193 + "','" + 572 + "','" + PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() + "','UACC');", true);
                string headyear = ucFinancialYear.SelectedText;
                PhoenixAccountsUACCReport2XL.Export2XLUACC(General.GetNullableInteger(ucFinancialYear.SelectedValue), General.GetNullableInteger(ucOwner.SelectedAddress), 0, General.GetNullableInteger(ddlvessel.SelectedValue), headyear);
                //Export2XLUACC(General.GetNullableInteger(ucFinancialYear.SelectedValue), General.GetNullableInteger(ucOwner.SelectedAddress), 0, General.GetNullableInteger(ddlvessel.SelectedValue), headyear);


            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    private static string GetExcelColumnName(int columnNumber)
    {
        int dividend = columnNumber;
        string columnName = String.Empty;
        int modulo;

        while (dividend > 0)
        {
            modulo = (dividend - 1) % 26;
            columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
            dividend = (int)((dividend - modulo) / 26);
        }

        return columnName;
    }
    protected void ucOwner_Onchange(object sender, EventArgs e)
    {
        DataSet dsveaaelname = null;
        dsveaaelname = PhoenixCommonBudgetGroupAllocation.OwnerVessellist(3817);
        ddlvessel.DataSource = dsveaaelname;
        ddlvessel.DataTextField = "FLDVESSELNAME";
        ddlvessel.DataValueField = "FLDVESSELID";
        ddlvessel.DataBind();
        ddlvessel.Items.Insert(0, new DropDownListItem("--Select--", ""));
    }
}
