using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewOffshore;
using Telerik.Web.UI;
public partial class CrewNewApplicantCompanyExperience : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../Crew/CrewNewApplicantCompanyExperience.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewCompanyExperience')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
        toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewNewApplicantCompanyExperienceList.aspx?empid=" + Filter.CurrentNewApplicantSelection + "'); return false;", "Add", "<i class=\"fas fa-plus\"></i>", "ADDCREWCOMPANYEXPERIENCE");
        MenuCrewCompanyExperience.AccessRights = this.ViewState;
        MenuCrewCompanyExperience.MenuList = toolbar.Show();

        PhoenixToolbar toolbar1 = new PhoenixToolbar();

        TabStrip1.AccessRights = this.ViewState;
        TabStrip1.MenuList = toolbar1.Show();

        if (!IsPostBack)
        {
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            SetEmployeePrimaryDetails();
            gvCrewCompanyExperience.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

        }

        //BindData();
        //SetPageNavigator();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = new string[] { };
        string[] alCaptions = new string[] { };

        DataSet ds = new DataSet();
        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
        {
            alColumns = new string[]{ "FLDVESSELNAME", "FLDVESSELTYPENAME", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDVESSELDWT", "FLDVESSELGT","FLDVESSELTEU", "FLDENGINETYPEMODEL",
                                     "FLDFROMDATE", "FLDTODATE", "FLDDURATION", "FLDGAP", "FLDMANNINGCOMPANY", "FLDDAILYRATEUSD", "FLDDPALLOWANCE","FLDPRINCIPALNAME" , "FLDSIGNOFFREASONNAME", "FLDICEEXPYN" };
            alCaptions = new string[]{ "Vessel", "Vessel Type","Employee", "Rank", "DWT", "GRT","TEU", "Engine Type / Model", "Sign On Date", "Sign Off Date", "Duration",
                                      "Gap", "Employer", "Last drawn salary/day(USD)", "Owner", "SignOff Reason", "Ice Experience" };
        }
        else
        {
            alColumns = new string[]{ "FLDVESSELNAME", "FLDVESSELTYPENAME", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDVESSELDWT", "FLDVESSELGT","FLDVESSELTEU", "FLDENGINETYPEMODEL",
                                     "FLDFROMDATE", "FLDTODATE", "FLDDURATION", "FLDGAP", "FLDMANNINGCOMPANY", "FLDPRINCIPALNAME", "FLDSIGNOFFREASONNAME", "FLDICEEXPYN" };
            alCaptions = new string[]{ "Vessel", "Vessel Type","Employee", "Rank", "DWT", "GRT","TEU", "Engine Type / Model", "From", "To", "Duration",
                                      "Gap", "Employer","Owner" , "SignOff Reason", "Ice Experience" };
        }
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewCompanyExperience.CrewCompanyExperienceSearch(
            Int32.Parse(Filter.CurrentNewApplicantSelection.ToString())
            , sortexpression, sortdirection
            , 1
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount);
        General.ShowExcel("Company Experience", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void CrewCompanyExperience_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
        gvCrewCompanyExperience.Rebind();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = new string[] { };
        string[] alCaptions = new string[] { };

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
        {
            alColumns = new string[]{ "FLDVESSELNAME", "FLDVESSELTYPENAME", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDVESSELDWT", "FLDVESSELGT","FLDVESSELTEU", "FLDENGINETYPEMODEL",
                                     "FLDFROMDATE", "FLDTODATE", "FLDDURATION", "FLDGAP", "FLDMANNINGCOMPANY", "FLDDAILYRATEUSD", "FLDDPALLOWANCE", "FLDPRINCIPALNAME" , "FLDSIGNOFFREASONNAME", "FLDICEEXPYN" };
            alCaptions = new string[]{ "Vessel", "Vessel Type", "Employee" , "Rank", "DWT", "GRT","TEU", "Engine Type / Model", "Sign On Date", "Sign Off Date", "Duration",
                                      "Gap", "Employer", "Last drawn salary/day(USD)", "Owner", "SignOff Reason", "Ice Experience" };
        }
        else
        {
            alColumns = new string[]{ "FLDVESSELNAME", "FLDVESSELTYPENAME", "FLDEMPLOYEENAME", "FLDRANKNAME", "FLDVESSELDWT", "FLDVESSELGT","FLDVESSELTEU", "FLDENGINETYPEMODEL",
                                     "FLDFROMDATE", "FLDTODATE", "FLDDURATION", "FLDGAP", "FLDMANNINGCOMPANY","FLDPRINCIPALNAME" , "FLDSIGNOFFREASONNAME", "FLDICEEXPYN" };
            alCaptions = new string[]{ "Vessel", "Vessel Type", "Employee", "Rank", "DWT", "GRT","TEU", "Engine Type / Model", "From", "To", "Duration",
                                      "Gap", "Employer", "Owner", "SignOff Reason", "Ice Experience" };
        }
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewCompanyExperience.CrewCompanyExperienceSearch(
            Int32.Parse(Filter.CurrentNewApplicantSelection.ToString())
            , sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"]
            , gvCrewCompanyExperience.PageSize
            , ref iRowCount
            , ref iTotalPageCount);
        General.SetPrintOptions("gvCrewCompanyExperience", "Company Experience", alCaptions, alColumns, ds);


        gvCrewCompanyExperience.DataSource = ds;
        gvCrewCompanyExperience.VirtualItemCount = iRowCount;

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
        {
            gvCrewCompanyExperience.Columns[11].Visible = true;
            gvCrewCompanyExperience.Columns[12].Visible = true;
        }
        else
        {
            gvCrewCompanyExperience.Columns[11].Visible = false;
            gvCrewCompanyExperience.Columns[12].Visible = false;
        }

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixNewApplicantManagement.NewApplicantList(General.GetNullableInteger(Filter.CurrentNewApplicantSelection));
            if (dt.Rows.Count > 0)
            {
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtRank.Text = dt.Rows[0]["FLDAPPLIEDRANK"].ToString();
                ViewState["attachmentcode"] = dt.Rows[0]["FLDDTKEY"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void DeleteCrewCompanyExperience(int companyexperienceid)
    {
        PhoenixCrewCompanyExperience.DeleteCrewCompanyExperience(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode
            , companyexperienceid, Int32.Parse(Filter.CurrentNewApplicantSelection.ToString()));
    }
    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }
    protected void gvCrewCompanyExperience_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event)");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            RadLabel l = (RadLabel)e.Item.FindControl("lblCrewCompanyExperienceId");

            LinkButton lb = (LinkButton)e.Item.FindControl("lnkRank");
            lb.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/Crew/CrewNewApplicantCompanyExperienceList.aspx?empid=" + Filter.CurrentNewApplicantSelection + "&CrewCompanyExperienceId=" + l.Text + "');return false;");

            LinkButton cmdMove = (LinkButton)e.Item.FindControl("cmdMove");
            cmdMove.Attributes.Add("onclick", "return fnConfirmDelete(event,'Are you sure to move the company experience to other experience?')");
            RadLabel lblVessel = (RadLabel)e.Item.FindControl("lblVessel");
            if (string.IsNullOrEmpty(lblVessel.Text))
            {
                if (cmdMove != null) cmdMove.Visible = true;
            }
            else
            {
                if (cmdMove != null) cmdMove.Visible = false;
            }

            if (cmdMove != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, cmdMove.CommandName))
                    cmdMove.Visible = false;
            }
        }

        if (e.Item is GridHeaderItem)
        {
            GridHeaderItem item = e.Item as GridHeaderItem;

            if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode.ToUpper().Equals("OFFSHORE"))
            {
                item["Fromdate"].Text = "Sign On Date";
                item["Todate"].Text = "Sign Off Date";
            }
        }
    }
    protected void gvCrewCompanyExperience_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewCompanyExperience.CurrentPageIndex + 1;
        BindData();
    }
    protected void gvCrewCompanyExperience_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName.ToUpper().Equals("SORT")) return;

        if (e.CommandName.ToUpper().Equals("DELETE"))
        {
            DeleteCrewCompanyExperience(Int32.Parse(((RadLabel)e.Item.FindControl("lblCrewCompanyExperienceId")).Text));
        }
        else if (e.CommandName.ToUpper().Equals("MOVETOOTHEREXPERIENCE"))
        {
            PhoenixCrewOffshoreEmployee.MoveCompanyExperience(General.GetNullableInteger(((RadLabel)e.Item.FindControl("lblCrewCompanyExperienceId")).Text));
        }
        else
        {
            gvCrewCompanyExperience.EditIndexes.Clear();
            gvCrewCompanyExperience.SelectedIndexes.Clear();
            BindData();
            gvCrewCompanyExperience.Rebind();
        }
    }
}
