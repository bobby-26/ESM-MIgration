using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class Presea_PreSeaCandidateBioData : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
        {
            Filter.CurrentPreSeaNewApplicantSelection = Request.QueryString["empid"];
            Title1.ShowMenu = "false";
        }

        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Personal", "PERSONAL");
        toolbarmain.AddButton("History ", "HISTORY");
        toolbarmain.AddButton("Bio Data", "BIODATA");

        PreSeaMenu.AccessRights = this.ViewState;
        PreSeaMenu.MenuList = toolbarmain.Show();

        PhoenixToolbar toolbarsub = new PhoenixToolbar();

        toolbarsub.AddButton("Personal", "SUBPERSONAL");
        toolbarsub.AddButton("Address ", "ADDRESS");
        toolbarsub.AddButton("Family/Nok", "FAMILY/NOK");

        PreSeaMenuGeneral.AccessRights = this.ViewState;
        PreSeaMenuGeneral.MenuList = toolbarsub.Show();

        if (!Page.IsPostBack)
        {
            PreSeaMenu.SelectedMenuIndex = 0;
            PreSeaMenuGeneral.SelectedMenuIndex = 0;
            ifMoreInfo.Attributes["src"] = "../PreSea/PreSeaNewApplicantPersonal.aspx";

            }
        }

    protected void PreSeaMenu_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        PreSeaMenuGeneral.Visible = true;
        

        if (dce.CommandName.ToUpper().Equals("PERSONAL"))
        {
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Personal", "SUBPERSONAL");
            toolbarsub.AddButton("Address ", "ADDRESS");
            toolbarsub.AddButton("Family/Nok", "FAMILY/NOK");
           
            PreSeaMenuGeneral.AccessRights = this.ViewState;
            PreSeaMenuGeneral.MenuList = toolbarsub.Show();
            PreSeaMenuGeneral.SelectedMenuIndex = 0;
            ifMoreInfo.Attributes["src"] = "PreSeaNewApplicantPersonal.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("HISTORY"))
        {
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Academics", "ACADEMICS");

            DataTable dt = PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantList(General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection));

            PreSeaMenuGeneral.AccessRights = this.ViewState;
            PreSeaMenuGeneral.MenuList = toolbarsub.Show();
            PreSeaMenuGeneral.SelectedMenuIndex = 0;
            ifMoreInfo.Attributes["src"] = "PreSeaNewApplicantAcademicQualification.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("BIODATA"))
        {
            PreSeaMenuGeneral.Visible = false;

            ifMoreInfo.Attributes["src"] = "../Reports/ReportsView.aspx?applicationcode=10&reportcode=CANDIDATEBIODATA&employeeid=" + Filter.CurrentPreSeaNewApplicantSelection;
        }
    }

    protected void PreSeaMenuGeneral_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if ((Filter.CurrentPreSeaNewApplicantSelection == null && Request.QueryString["t"] == null) || (Filter.CurrentPreSeaNewApplicantSelection == null && dce.CommandName.ToUpper() != "PERSONAL"))
        {
            ucError.ErrorMessage = " Please Select a Employee ";
            ucError.Visible = true;
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PreSea/PreSeaNewApplicantPersonal.aspx";
            PreSeaMenu.SelectedMenuIndex = 0;
            PreSeaMenuGeneral.SelectedMenuIndex = 0;
        }
        else if (dce.CommandName.ToUpper().Equals("SUBPERSONAL"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PreSea/PreSeaNewApplicantPersonal.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("ADDRESS"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PreSea/PreSeaNewApplicantPersonalAddress.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("FAMILY/NOK"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PreSea/PreSeaNewApplicantFamilyNok.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("ACADEMICS"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PreSea/PreSeaNewApplicantAcademicQualification.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("BANKACCT"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PreSea/PreSeaBankAccount.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("OTHERS"))
        {
            ViewState["SETCURRENTNAVIGATIONTAB"] = "../PreSea/PreSeaNewApplicantOthers.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("GENERALREMARKS"))
        {

            Session["USERID"] = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
            PhoenixCommonDiscussion objdiscussion = new PhoenixCommonDiscussion();
            objdiscussion.dtkey = GetCurrentEmployeeDTkey();
            objdiscussion.userid = Convert.ToInt32(Session["USERID"]);
            objdiscussion.title = PhoenixPreSeaConstants.REMARKSTITLE;

            objdiscussion.type = PhoenixPreSeaConstants.REMARKS;
            PhoenixCommonDiscussion.SetCurrentContext = objdiscussion;

            ViewState["SETCURRENTNAVIGATIONTAB"] = "PreSeaNewApplicantRemarks.aspx";
        }
        ifMoreInfo.Attributes["src"] = ViewState["SETCURRENTNAVIGATIONTAB"].ToString();

    }

    private Guid GetCurrentEmployeeDTkey()
    {
        Guid dtkey = Guid.Empty;
        try
        {

            DataTable dt = PhoenixPreSeaNewApplicantPersonal.PreSeaNewApplicantList(General.GetNullableInteger(Filter.CurrentPreSeaNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {
                dtkey = new Guid(dt.Rows[0]["FLDDTKEY"].ToString());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
        return dtkey;
    }
}
