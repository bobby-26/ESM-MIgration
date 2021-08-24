using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Common;
using SouthNests.Phoenix.Registers;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class CrewOffshore_CrewoffshoreOperationalCompanyExp : PhoenixBasePage
{
    string empid = string.Empty;
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);
        empid = Request.QueryString["empid"].ToString();

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../CrewOffshore/CrewoffshoreOperationalCompanyExp.aspx?empid=" + empid, "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewCompanyExperience')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        if (!IsPostBack)
        {
            PhoenixCrewOffshoreCrewList.EmployeeExperienceupdate(General.GetNullableInteger(Request.QueryString["empid"].ToString()));
            if (Request.QueryString["empid"] != null)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

            }
            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            MenuCrewOtherExperienceList.AccessRights = this.ViewState;
            MenuCrewOtherExperienceList.MenuList = toolbar1.Show();
            gvCrewCompanyExperience.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }
        //EditCrewOperationalExperience();
        SetEmployeePrimaryDetails();
        MenuCrewCompanyExperience.AccessRights = this.ViewState;
        MenuCrewCompanyExperience.MenuList = toolbar.Show();
        // MenuCrewCompanyExperience.SetTrigger(pnlExpList);
    }

    protected void CrewCompanyExperience_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }
    protected void ShowExcel()
    {
        string[] alColumns = {"FLDVESSELNAME", "FLDRANKNAME","FLDSIGNONDATE", "FLDDRYDOCKYN","FLDOVID", "FLDOSVISCOUTN", "FLDCLASSOFROV","FLDTYPESOFANCHORSHANDLED",
                                "FLDFSIPSCYN","FLDFMEAYN", "FLDDPANNUALSYN","FLDNEWDELIVERYTAKEOVERYN","FLDHEAVYLIFTCARGOESYN","FLDDKDMUDYN","FLDMETHANOLYN","FLDGLYCOLYN"
                                ,"FLDGRAPPLINGANCHORSYN","FLDLENGTHCHAINHANDLEDYN","FLDMAXANCHORHANDLINGYN" };
        string[] alCaptions = { "Vessel","Rank","Sign On","Dry Dockings Attended","No of OVID	","No of OSVIS","Class of ROV","Types of Anchors Handled",
                                  "FSI/PSC","FMEA ","DP Annuals","New Delivery / Take over","Heavy Lift / Project cargoes","DKD Mud","Methanol","Glycol",
                                  "Experience in Grappling for Anchors","Experience in Chain Handled"," Maximum Anchor Handling Depth" };
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string sortexpression;
        int? sortdirection = null;
        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixNewApplicantOtherExperience.ListEmployeeOperationalExperienceds(Convert.ToInt32(empid), sortexpression, sortdirection
           , 1
           , General.ShowRecords(null)
           , ref iRowCount
           , ref iTotalPageCount);

        General.ShowExcel("Crew Operational Experience", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(empid));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void EditCrewOperationalExperience()
    {
        string[] alColumns = {"FLDVESSELNAME", "FLDRANKNAME","FLDSIGNONDATE", "FLDDRYDOCKYN","FLDOVID", "FLDOSVISCOUTN", "FLDCLASSOFROV","FLDTYPESOFANCHORSHANDLED",
                                "FLDFSIPSCYN","FLDFMEAYN", "FLDDPANNUALSYN","FLDNEWDELIVERYTAKEOVERYN","FLDHEAVYLIFTCARGOESYN","FLDDKDMUDYN","FLDMETHANOLYN","FLDGLYCOLYN"
                                ,"FLDGRAPPLINGANCHORSYN","FLDLENGTHCHAINHANDLEDYN","FLDMAXANCHORHANDLINGYN" };
        string[] alCaptions = { "Vessel","Rank","Sign On","Dry Dockings Attended","No of OVID	","No of OSVIS","Class of ROV","Types of Anchors Handled",
                                  "FSI/PSC","FMEA ","DP Annuals","New Delivery / Take over","Heavy Lift / Project cargoes","DKD Mud","Methanol","Glycol",
                                  "Experience in Grappling for Anchors","Experience in Chain Handled"," Maximum Anchor Handling Depth" };

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        DataSet ds = PhoenixNewApplicantOtherExperience.ListEmployeeOperationalExperienceds(Convert.ToInt32(empid), sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"]
            , gvCrewCompanyExperience.PageSize
            , ref iRowCount
            , ref iTotalPageCount);

        General.SetPrintOptions("gvCrewCompanyExperience", "Crew Experience", alCaptions, alColumns, ds);
        DataTable dt = ds.Tables[0];
        gvCrewCompanyExperience.DataSource = dt;
        gvCrewCompanyExperience.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;

    }
    protected void gvCrewCompanyExperience_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        EditCrewOperationalExperience();
    }

    protected void gvCrewCompanyExperience_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {

        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewCompanyExperience.CurrentPageIndex + 1;
            EditCrewOperationalExperience();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewCompanyExperience_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                RadLabel lblROV = (RadLabel)e.Item.FindControl("lblROV");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucROV");
                if (lblROV != null)
                {
                    uct.Position = ToolTipPosition.TopCenter;
                    uct.TargetControlId = lblROV.ClientID;
                    //lblROV.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    //lblROV.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }
                RadLabel lblTypesofAnchors = (RadLabel)e.Item.FindControl("lblTypesofAnchors");
                UserControlToolTip ucTypesofAnchors = (UserControlToolTip)e.Item.FindControl("ucTypesofAnchors");
                if (lblTypesofAnchors != null)
                {
                    uct.Position = ToolTipPosition.TopCenter;
                    uct.TargetControlId = lblTypesofAnchors.ClientID;
                    //lblTypesofAnchors.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucTypesofAnchors.ToolTip + "', 'visible');");
                    //lblTypesofAnchors.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucTypesofAnchors.ToolTip + "', 'hidden');");
                }
            }
        }

    }

    protected void gvCrewCompanyExperience_SortCommand(object sender, GridSortCommandEventArgs e)
    {
        ViewState["SORTEXPRESSION"] = e.SortExpression;
        switch (e.NewSortOrder)
        {
            case GridSortOrder.Ascending:
                ViewState["SORTDIRECTION"] = "0";
                break;
            case GridSortOrder.Descending:
                ViewState["SORTDIRECTION"] = "1";
                break;
        }
    }

    protected void gvCrewCompanyExperience_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }
}
