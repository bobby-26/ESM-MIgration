using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOffshore;
using System.Data;

public partial class CrewOffshoreVesselEmployeeGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
        {
            Filter.CurrentVesselCrewSelection = Request.QueryString["empid"];
            Title1.ShowMenu = "false";
        }        
        Filter.CurrentCrewSelection = Filter.CurrentVesselCrewSelection;

        if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
           Filter.CurrentCrewOffsoreVesselSelection = Request.QueryString["vesselid"].ToString();

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
        {
            Filter.CurrentCrewOffsoreVesselSelection = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        }
        if (Request.QueryString["signonId"] != null && Request.QueryString["signonId"].ToString() != "")
            Filter.CurrentCrewOffsoreVesselEmpSignOnId = Request.QueryString["signonId"].ToString();
        else
            Filter.CurrentCrewOffsoreVesselEmpSignOnId = null;
             
        if (!Page.IsPostBack)
        {
           
            PhoenixToolbar toolbarmain = new PhoenixToolbar();
            toolbarmain.AddButton("Personal", "PERSONAL");
            toolbarmain.AddButton("History ", "HISTORY");
            //toolbarmain.AddButton("Crew List ", "CREWLIST");
            CrewMenu.AccessRights = this.ViewState;
            CrewMenu.MenuList = toolbarmain.Show();
            CrewMenu.SelectedMenuIndex = 0;

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Personal", "PERSONAL");
            toolbarsub.AddButton("Family/Nok", "NOK");
            toolbarsub.AddButton("Bank Acct.", "BANKACCT");
            CrewMenuGeneral.AccessRights = this.ViewState;
            CrewMenuGeneral.MenuList = toolbarsub.Show();
            CrewMenuGeneral.SelectedMenuIndex = 0;
            ifMoreInfo.Attributes["src"] = "../CrewOffshore/CrewOffshoreVesselEmployeePersonal.aspx?vesselid=" + Filter.CurrentCrewOffsoreVesselSelection + "&signonId=" + Filter.CurrentCrewOffsoreVesselEmpSignOnId;
        }
    }

    protected void CrewMenu_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        //if (dce.CommandName.ToUpper().Equals("CREWLIST"))
        //{
        //    Response.Redirect("VesselAccountsEmployeeQuery.aspx", false);
        //}
        if (Filter.CurrentCrewSelection == null || dce.CommandName.ToUpper().Equals("PERSONAL"))
        {
            if (string.IsNullOrEmpty(Filter.CurrentVesselCrewSelection))
            {
                ucError.ErrorMessage = "Please Select a Employee.";
                ucError.Visible = true;
                CrewMenu.SelectedMenuIndex = 0;
            }
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Personal", "PERSONAL");
            toolbarsub.AddButton("Family/Nok", "NOK");
            toolbarsub.AddButton("Bank Acct.", "BANKACCT");
            CrewMenuGeneral.AccessRights = this.ViewState;
            CrewMenuGeneral.MenuList = toolbarsub.Show();
            CrewMenuGeneral.SelectedMenuIndex = 0;
            ifMoreInfo.Attributes["src"] = "../CrewOffshore/CrewOffshoreVesselEmployeePersonal.aspx?vesselid=" + Filter.CurrentCrewOffsoreVesselSelection + "&signonId=" + Filter.CurrentCrewOffsoreVesselEmpSignOnId;
        }
        else if (dce.CommandName.ToUpper().Equals("HISTORY"))
        {
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Licence", "LICENCE");

            //string canview = "1";
            DataTable dt = PhoenixCrewSignOnOff.CrewSignOnEdit(General.GetNullableInteger(Filter.CurrentCrewSelection));

            //PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(Filter.CurrentCrewSelection));

            //DataSet ds = PhoenixCrewAppraisal.AppraisalSecurityEdit(int.Parse(dt.Rows[0]["FLDSIGNONRANKID"].ToString()), General.GetNullableInteger(Filter.CurrentAppraisalSelection));

            //if (ds.Tables[0].Rows.Count > 0)
            //{
            //    canview = ds.Tables[0].Rows[0]["FLDCANVIEW"].ToString();
            //}
            //if (canview.Equals("1"))
            //{
            //    toolbarsub.AddButton("Appraisal", "APPRAISAL");
            //}
            CrewMenuGeneral.AccessRights = this.ViewState;
            CrewMenuGeneral.MenuList = toolbarsub.Show();
            CrewMenuGeneral.SelectedMenuIndex = 0;
            ifMoreInfo.Attributes["src"] = "../CrewOffshore/CrewOffshoreVesselEmployeeLicence.aspx?empid=" + Request.QueryString["empid"] + "&vesselid=" + Filter.CurrentCrewOffsoreVesselSelection + "&signonId=" + Filter.CurrentCrewOffsoreVesselEmpSignOnId;
        }

    }

    protected void CrewMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (string.IsNullOrEmpty(Filter.CurrentVesselCrewSelection) || dce.CommandName.ToUpper().Equals("PERSONAL"))
        {
            if (string.IsNullOrEmpty(Filter.CurrentVesselCrewSelection))
            {
                ucError.ErrorMessage = "Please Select a Employee ";
                ucError.Visible = true;
                CrewMenu.SelectedMenuIndex = 0;
                CrewMenuGeneral.SelectedMenuIndex = 0;
            }
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../CrewOffshore/CrewOffshoreVesselEmployeePersonal.aspx?vesselid=" + Filter.CurrentCrewOffsoreVesselSelection + "&signonId=" + Filter.CurrentCrewOffsoreVesselEmpSignOnId;
        }
        else if (dce.CommandName.ToUpper().Equals("LICENCE"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../CrewOffshore/CrewOffshoreVesselEmployeeLicence.aspx?vesselid=" + Filter.CurrentCrewOffsoreVesselSelection + "&signonId=" + Filter.CurrentCrewOffsoreVesselEmpSignOnId;
        }
        //else if (dce.CommandName.ToUpper().Equals("APPRAISAL"))
        //{
        //    ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewAppraisal.aspx?vslid=" + PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
        //}
        else if (dce.CommandName.ToUpper().Equals("NOK"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../CrewOffshore/CrewOffshoreVesselEmployeeFamily.aspx?vesselid=" + Filter.CurrentCrewOffsoreVesselSelection + "&signonId=" + Filter.CurrentCrewOffsoreVesselEmpSignOnId;
        }
        else if (dce.CommandName.ToUpper().Equals("BANKACCT"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../Crew/CrewBankAccount.aspx?vesselid=" + Filter.CurrentCrewOffsoreVesselSelection + "&signonId=" + Filter.CurrentCrewOffsoreVesselEmpSignOnId;
        }
        ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();
    }
}
