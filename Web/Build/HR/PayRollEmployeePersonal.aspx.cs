using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PayRoll_PayRollEmployeePersonal : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    int Id = 0;
  //  Guid Id = Guid.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        if (string.IsNullOrWhiteSpace(Request.QueryString["Id"]) == false)
        {
            Id = int.Parse(Request.QueryString["Id"]);
        }
        ShowToolBar();

        if (IsPostBack == false)
        {
            ifMoreInfo.Src = "../HR/PayRollEmployeePersonalDetail.aspx?Id=" + Id;
        }
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Personal", "PERSONAL");
        toolbarmain.AddButton("Income", "INCOME");
        toolbarmain.AddButton("Standard Deduction", "STANDARDDEDUCTION");
        toolbarmain.AddButton("Deduction", "DEDUCTION");
        toolbarmain.AddButton("Tax", "TAX");
        toolbarmain.AddButton("Summary", "SUMMARY");
        toolbarmain.AddButton("Back", "BACK", ToolBarDirection.Right);
        EmployeeMenu.AccessRights = this.ViewState;
        EmployeeMenu.MenuList = toolbarmain.Show();
        EmployeeMenu.SelectedMenuIndex = 0;
    }

    protected void EmployeeMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("PERSONAL"))
            {
                EmployeeMenu.SelectedMenuIndex = 0;
                ifMoreInfo.Src = "../HR/PayRollEmployeePersonalDetail.aspx?Id=" + Id;
            }

            if (CommandName.ToUpper().Equals("INCOME"))
            {
                EmployeeMenu.SelectedMenuIndex = 1;
                ifMoreInfo.Src = "../HR/PayRollEmployeeIncome.aspx?Id=" + Id;
            }

            if (CommandName.ToUpper().Equals("STANDARDDEDUCTION"))
            {
                EmployeeMenu.SelectedMenuIndex = 2;
                ifMoreInfo.Src = "../HR/PayRollEmployeeStandardDeduction.aspx?Id=" + Id;
            }

            if (CommandName.ToUpper().Equals("DEDUCTION"))
            {
                EmployeeMenu.SelectedMenuIndex = 3;
                ifMoreInfo.Src = "../HR/PayRollEmployeeDeduction.aspx?Id=" + Id;
            }

            if (CommandName.ToUpper().Equals("TAX"))
            {
                EmployeeMenu.SelectedMenuIndex = 4;
                ifMoreInfo.Src = "../HR/PayRollEmployeeTax.aspx?Id=" + Id; 
            }
            if (CommandName.ToUpper().Equals("SUMMARY"))
            {
                EmployeeMenu.SelectedMenuIndex = 5;
                ifMoreInfo.Src = "../HR/PayRollSummaryIndia.aspx?Id=" + Id;
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../HR/PayRollGeneral.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}