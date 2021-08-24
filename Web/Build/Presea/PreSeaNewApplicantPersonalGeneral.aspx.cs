using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.PreSea;

public partial class PreSeaNewApplicantPersonalGeneral : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Personal", "PERSONAL");
        toolbarmain.AddButton("History ", "HISTORY");
        toolbarmain.AddButton("Notes", "NOTES");
        if (Request.QueryString["back"] != null)
        {
            toolbarmain.AddButton("Back", "LIST");
        }
        PreSeaMenu.AccessRights = this.ViewState;
        PreSeaMenu.MenuList = toolbarmain.Show();

        ViewState["ACCESS"] = "1";

        PhoenixToolbar toolbarsub = new PhoenixToolbar();

        if (Request.QueryString["familyid"] != "" && Request.QueryString["familyid"] != null)
        {
            toolbarsub.AddButton("Personal", "SUBPERSONAL");
            if (ViewState["ACCESS"].ToString() == "1")
            {
                toolbarsub.AddButton("Address ", "ADDRESS");
                toolbarsub.AddButton("Family/Nok", "FAMILY/NOK");
            }
            toolbarsub.AddButton("Bank Acct.", "OTHERS");
            toolbarsub.AddButton("Bank Acct.", "BANKACCT");
            PreSeaMenuGeneral.AccessRights = this.ViewState;
            PreSeaMenuGeneral.MenuList = toolbarsub.Show();
            PreSeaMenuGeneral.SelectedMenuIndex = 2;
            ifMoreInfo.Attributes["src"] = "../PreSea/PreSeaNewApplicantFamilyNok.aspx?familyid=" + Request.QueryString["familyid"];
        }
        else if (Request.QueryString["documents"] != null)
        {
            toolbarsub.AddButton("Personal", "SUBPERSONAL");
            if (ViewState["ACCESS"].ToString() == "1")
            {
                toolbarsub.AddButton("Address ", "ADDRESS");
                toolbarsub.AddButton("Family/Nok", "FAMILY/NOK");
            }
            toolbarsub.AddButton("Academic", "ACADEMIC");
            toolbarsub.AddButton("Bank Acct.", "OTHERS");
            toolbarsub.AddButton("Bank Acct.", "BANKACCT");
            PreSeaMenuGeneral.AccessRights = this.ViewState;
            PreSeaMenuGeneral.MenuList = toolbarsub.Show();
            PreSeaMenu.SelectedMenuIndex = 0;
            PreSeaMenuGeneral.SelectedMenuIndex = 3;
        }
        else
        {

            toolbarsub.AddButton("Personal", "SUBPERSONAL");
            if (ViewState["ACCESS"].ToString() == "1")
            {
                toolbarsub.AddButton("Address ", "ADDRESS");
                toolbarsub.AddButton("Family/Nok", "FAMILY/NOK");
            }
            toolbarsub.AddButton("Others", "OTHERS");
            toolbarsub.AddButton("Bank Acct.", "BANKACCT");
            PreSeaMenuGeneral.AccessRights = this.ViewState;
            PreSeaMenuGeneral.MenuList = toolbarsub.Show();
            PreSeaMenu.SelectedMenuIndex = 0;
            PreSeaMenuGeneral.SelectedMenuIndex = 0;
            if (!string.IsNullOrEmpty(Request.QueryString["empid"]))
            {
                Filter.CurrentPreSeaNewApplicantSelection = Request.QueryString["empid"];
                Title1.ShowMenu = "false";
            }
            if (!Page.IsPostBack)
            {
                ifMoreInfo.Attributes["src"] = "../PreSea/PreSeaNewApplicantPersonal.aspx";
            }
        }
    }
    protected void PreSeaMenu_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (Filter.CurrentPreSeaNewApplicantSelection == null || dce.CommandName.ToUpper().Equals("PERSONAL"))
        {
            if (Filter.CurrentPreSeaNewApplicantSelection == null && Request.QueryString["t"] == null)
            {
                ucError.ErrorMessage = "  Please Select a Person .";
                ucError.Visible = true;
                PreSeaMenu.SelectedMenuIndex = 0;
            }
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Personal", "SUBPERSONAL");
            if (ViewState["ACCESS"].ToString() == "1")
            {
                toolbarsub.AddButton("Address ", "ADDRESS");
                toolbarsub.AddButton("Family/Nok", "FAMILY/NOK");
            }
            toolbarsub.AddButton("Others", "OTHERS");
            toolbarsub.AddButton("Bank Acct.", "BANKACCT");
            PreSeaMenuGeneral.AccessRights = this.ViewState;
            PreSeaMenuGeneral.MenuList = toolbarsub.Show();
            PreSeaMenuGeneral.SelectedMenuIndex = 0;
            ifMoreInfo.Attributes["src"] = "PreSeaNewApplicantPersonal.aspx";
        }
        if (Filter.CurrentPreSeaNewApplicantSelection == null)
        {
            ucError.ErrorMessage = " Please Select a Employee ";
            ucError.Visible = true;
            Response.Redirect("../PreSea/PreSeaNewApplicantPersonalGeneral.aspx");
        }
        else if (dce.CommandName.ToUpper().Equals("LIST"))
        {
            Response.Redirect("PreSeaNewApplicantQueryActivity.aspx?p=" + Request.QueryString["p"], false);
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
        else if (dce.CommandName.ToUpper().Equals("NOTES"))
        {
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("General Remarks", "GENERALREMARKS");
            PreSeaMenuGeneral.AccessRights = this.ViewState;
            PreSeaMenuGeneral.MenuList = toolbarsub.Show();
            Session["USERID"] = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
            PhoenixCommonDiscussion objdiscussion = new PhoenixCommonDiscussion();
            objdiscussion.dtkey = GetCurrentEmployeeDTkey();
            objdiscussion.userid = Convert.ToInt32(Session["USERID"]);
            objdiscussion.title = PhoenixPreSeaConstants.REMARKSTITLE;
            objdiscussion.type = PhoenixPreSeaConstants.REMARKS;
            PhoenixCommonDiscussion.SetCurrentContext = objdiscussion;
            PreSeaMenuGeneral.SelectedMenuIndex = 0;
            ifMoreInfo.Attributes["src"] = "../PreSea/PreSeaNewApplicantRemarks.aspx";
        }
        else if (dce.CommandName.ToUpper().Equals("ACTIVITYLOG"))
        {
            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddButton("Activity Log", "SUBACTIVITYLOG");
            PreSeaMenuGeneral.AccessRights = this.ViewState;
            PreSeaMenuGeneral.MenuList = toolbarsub.Show();
            PreSeaMenuGeneral.SelectedMenuIndex = 0;
            ifMoreInfo.Attributes["src"] = "../PreSea/PreSeaNewApplicantActivityLog.aspx";
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
