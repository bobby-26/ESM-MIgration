using SouthNests.Phoenix.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

public partial class PayRoll_PayRollEmployeePersonalSingapore : System.Web.UI.Page
{
    int usercode = 0;
    int vesselId = 0;
    int Id = 0;
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
            ifMoreInfo.Src = "../HR/PayRollEmployeePersonalDetailSingapore.aspx?Id=" + Id;
        }
    }

    private void ShowToolBar()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Personal", "PERSONAL");
        toolbarmain.AddButton("Income", "INCOME");
        toolbarmain.AddButton("Deduction", "DEDUCTION");
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
                ifMoreInfo.Src = "../HR/PayRollEmployeePersonalDetailSingapore.aspx?Id=" + Id;
            }

            if (CommandName.ToUpper().Equals("INCOME"))
            {
                EmployeeMenu.SelectedMenuIndex = 1;
                ifMoreInfo.Src = "../HR/PayRollPFSalarySingapore.aspx?Id=" + Id;
            }

         

            if (CommandName.ToUpper().Equals("DEDUCTION"))
            {
                EmployeeMenu.SelectedMenuIndex = 2;
                ifMoreInfo.Src = "../HR/PayRollEmployeeDeductionSingapore.aspx?Id=" + Id;
            }


            if (CommandName.ToUpper().Equals("SUMMARY"))
            {
                EmployeeMenu.SelectedMenuIndex = 3;
                ifMoreInfo.Src = "../HR/PayRollSummarySingapore.aspx?Id=" + Id;
            }
            if (CommandName.ToUpper().Equals("BACK"))
            {
                Response.Redirect("../HR/PayRollGeneralSingapore.aspx");
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}