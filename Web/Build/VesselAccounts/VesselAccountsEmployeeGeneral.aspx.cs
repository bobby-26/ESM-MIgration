using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using System.Data;
using Telerik.Web.UI;

public partial class VesselAccountsEmployeeGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
        {
            Filter.CurrentVesselCrewSelection = Request.QueryString["empid"];
        }
        Filter.CurrentCrewSelection = Filter.CurrentVesselCrewSelection;
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Personal", "PERSONAL", CreatePersonalSubTab(string.Empty, 0, string.Empty));
        toolbarmain.AddButton("History", "HISTORY", CreateHistorySubTab(string.Empty, 0, string.Empty));
        toolbarmain.AddButton("Crew List ", "CREWLIST");
        CrewMenu.AccessRights = this.ViewState;
        CrewMenu.MenuList = toolbarmain.Show();
        CrewMenu.SelectedMenuIndex = 0;

        //PhoenixToolbar toolbarsub = new PhoenixToolbar();
        //toolbarsub.AddButton("Personal", "PERSONAL");
        //toolbarsub.AddButton("Family/Nok", "NOK");
        //toolbarsub.AddButton("Bank Acct.", "BANKACCT");
        //CrewMenuGeneral.AccessRights = this.ViewState;
        //CrewMenuGeneral.MenuList = toolbarsub.Show();
        //CrewMenuGeneral.SelectedMenuIndex = 0;
        if (!Page.IsPostBack)
        {

            ifMoreInfo.Attributes["src"] = "../VesselAccounts/VesselAccountsEmployeePersonal.aspx";
        }
    }
    private PhoenixToolbar CreatePersonalSubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Personal", "PERSONAL");
        toolbarsub.AddButton("Family/Nok", "NOK");
        toolbarsub.AddButton("Bank Acct.", "BANKACCT");
        if (Cmd == "PERSONAL")
        {
            if (SessionUtil.CanAccess(this.ViewState, "PERSONAL"))
                Page = "../VesselAccounts/VesselAccountsEmployeePersonal.aspx";
        }
        else if (Cmd == "NOK")
        {
            if (SessionUtil.CanAccess(this.ViewState, "NOK"))
                Page = "../VesselAccounts/VesselAccountsEmployeeFamily.aspx";
        }
        else if (Cmd == "BANKACCT")
        {
            if (SessionUtil.CanAccess(this.ViewState, "BANKACCT"))
                Page = "../Crew/CrewBankAccount.aspx";
        }
        ifMoreInfo.Attributes["src"] = Page;
        return toolbarsub;
    }
    private PhoenixToolbar CreateHistorySubTab(string Page, int MenuIndex, string Cmd)
    {
        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Licence", "LICENCE");

        if (Cmd == "HISTORY")
        {
            if (SessionUtil.CanAccess(this.ViewState, "HISTORY"))
                Page = "../VesselAccounts/VesselAccountsEmployeeLicence.aspx?empid=" + Request.QueryString["empid"];
        }
        else if (Cmd == "LICENCE")
        {
            if (SessionUtil.CanAccess(this.ViewState, "LICENCE"))
                Page = "../VesselAccounts/VesselAccountsEmployeeLicence.aspx?empid=" + Request.QueryString["empid"];
        }

        ifMoreInfo.Attributes["src"] = Page;
        return toolbarsub;
    }
    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("CREWLIST"))
        {
            Response.Redirect("VesselAccountsEmployeeQuery.aspx", false);
        }
        else if (Filter.CurrentCrewSelection == null || CommandName.ToUpper().Equals("PERSONAL"))
        {
            if (string.IsNullOrEmpty(Filter.CurrentVesselCrewSelection))
            {
                ucError.ErrorMessage = "Please Select a Employee.";
                ucError.Visible = true;
                //CrewMenu.SelectedMenuIndex = 0;
            }
            //PhoenixToolbar toolbarsub = new PhoenixToolbar();
            //toolbarsub.AddButton("Personal", "PERSONAL");
            //toolbarsub.AddButton("Family/Nok", "NOK");
            //toolbarsub.AddButton("Bank Acct.", "BANKACCT");
            //CrewMenuGeneral.AccessRights = this.ViewState;
            //CrewMenuGeneral.MenuList = toolbarsub.Show();
            //CrewMenuGeneral.SelectedMenuIndex = 0;
            ifMoreInfo.Attributes["src"] = "../VesselAccounts/VesselAccountsEmployeePersonal.aspx";
        }
        else if (CommandName.ToUpper().Equals("NOK"))
        {
            ifMoreInfo.Attributes["src"] = "../VesselAccounts/VesselAccountsEmployeeFamily.aspx";
        }
        else if (CommandName.ToUpper().Equals("BANKACCT"))
        {
            ifMoreInfo.Attributes["src"] = "../Crew/CrewBankAccount.aspx";
        }
        else if (CommandName.ToUpper().Equals("HISTORY") || CommandName.ToUpper().Equals("LICENCE"))
        {

            ifMoreInfo.Attributes["src"] = "../VesselAccounts/VesselAccountsEmployeeLicence.aspx?empid=" + Request.QueryString["empid"];
        }

    }

    //protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    //{
    //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
    //    if (string.IsNullOrEmpty(Filter.CurrentVesselCrewSelection) || dce.CommandName.ToUpper().Equals("PERSONAL"))
    //    {
    //        if (string.IsNullOrEmpty(Filter.CurrentVesselCrewSelection))
    //        {
    //            ucError.ErrorMessage = "Please Select a Employee ";
    //            ucError.Visible = true;
    //            CrewMenu.SelectedMenuIndex = 0;
    //            CrewMenuGeneral.SelectedMenuIndex = 0;
    //        }
    //        ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsEmployeePersonal.aspx";
    //    }
    //    else if (dce.CommandName.ToUpper().Equals("LICENCE"))
    //    {
    //        ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsEmployeeLicence.aspx";
    //    }
    //    //else if (dce.CommandName.ToUpper().Equals("APPRAISAL"))
    //    //{
    //    //    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewAppraisal.aspx?vslid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
    //    //}
    //    else if (dce.CommandName.ToUpper().Equals("NOK"))
    //    {
    //        ViewState["SETCURRENTNAVIGATIONTAB"] = "../VesselAccounts/VesselAccountsEmployeeFamily.aspx";
    //    }
    //    else if (dce.CommandName.ToUpper().Equals("BANKACCT"))
    //    {
    //        ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewBankAccount.aspx";
    //    }
    //    ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
    //}
}
