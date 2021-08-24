using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewManagement;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Common;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class CrewOffshoreEmployeeExperience : PhoenixBasePage
{
    //protected override void Render(HtmlTextWriter writer)
    //{
    //    foreach (GridViewRow r in gvCrewCompanyExperience.Rows)
    //    {
    //        if (r.RowType == DataControlRowType.DataRow)
    //        {
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$lnkDoubleClick");
    //            Page.ClientScript.RegisterForEventValidation
    //                    (r.UniqueID + "$lnkDoubleClickEdit");
    //        }
    //    }
    //    base.Render(writer);
    //}

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionUtil.PageAccessRights(this.ViewState);

        if (!IsPostBack)
        {
            ViewState["PAGENUMBER"] = 1;
            ViewState["SORTEXPRESSION"] = null;
            ViewState["SORTDIRECTION"] = null;
            ViewState["CURRENTINDEX"] = 1;

            if (Request.QueryString["newapplicant"] != null)
            {
                ViewState["empid"] = Filter.CurrentNewApplicantSelection;
                ViewState["type"] = "n";
            }
            else
            {
                ViewState["empid"] = Filter.CurrentCrewSelection;
                ViewState["type"] = "p";
            }
            PhoenixCrewOffshoreCrewList.EmployeeExperienceupdate(General.GetNullableInteger(ViewState["empid"].ToString()));
            SetEmployeePrimaryDetails();

            gvCrewCompanyExperience.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
        }

        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreEmployeeExperience.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
        toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrewCompanyExperience')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");

        if (ViewState["type"].ToString() == "n")
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewNewApplicantCompanyExperienceList.aspx?empid=" + ViewState["empid"].ToString() + "')", "Add Company Experience", "<i class=\"fa fa-plus-circle\"></i>", "ADDCREWCOMPANYEXPERIENCE");
        else
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewCompanyExperienceList.aspx?type=" + ViewState["type"].ToString() + "&empid=" + ViewState["empid"].ToString() + "')", "Add Company Experience", "<i class=\"fa fa-plus-circle\"></i>", "ADDCREWCOMPANYEXPERIENCE");

        if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
        {
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/CrewOffshore/CrewoffshoreOtherCompanyExperienceAdd.aspx?type=" + ViewState["type"].ToString() + "&empid=" + ViewState["empid"].ToString() + "')", "Add Other Experience", "<i class=\"fa fa-plus-circle\"></i>", "ADDCREWOTHEREXPERIENCE");
        }
        else
        {
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Crew/CrewOtherExperienceList.aspx?type=" + ViewState["type"].ToString() + "&empid=" + ViewState["empid"].ToString() + "')", "Add Other Experience", "<i class=\"fa fa-plus-circle\"></i>", "ADDCREWOTHEREXPERIENCE");
        }


        MenuCrewCompanyExperience.AccessRights = this.ViewState;
        MenuCrewCompanyExperience.MenuList = toolbar.Show();
        //MenuCrewCompanyExperience.SetTrigger(pnlCrewCompanyExperienceEntry);

        PhoenixToolbar toolbarsub = new PhoenixToolbar();
        toolbarsub.AddButton("Back", "BACK");
        CrewQuery.AccessRights = this.ViewState;
        //CrewQuery.MenuList = toolbarsub.Show();

        cmdHiddenSubmit.Attributes.Add("style", "display:none;");

        PhoenixToolbar toolbar1 = new PhoenixToolbar();
        CrewQuery.AccessRights = this.ViewState;
        CrewQuery.MenuList = toolbar1.Show();

        //BindData();
        //SetPageNavigator();
    }

    protected void CrewQuery_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

        if (dce.CommandName.ToUpper().Equals("BACK"))
        {
            Response.Redirect("CrewOffshoreCrewList.aspx", true);
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROWNUMBER","FLDMANNINGCOMPANY","FLDVESSELNAME", "FLDRANKCODE", "FLDVESSELTYPENAME","FLDDPCLASS", "FLDDPMAKEANDMODEL", "FLDVESSELGT","FLDENGINENAME", "FLDENGINETYPEMODEL",
                                "FLDBHP","FLDPROPULSIONNAME", "FLDVOLTAGE","FLDSUPPLYCOUNT","FLDANCHORHANDLING","FLDTOWINGCOUTN","FLDDIVESUPPORTCOUNT","FLDROVCOUNT","FLDFLOTELCOUNT","FLDINSTALLATIONSERVED",
                                "FLDOPERATINGAREA","FLDCHARTERER","FLDFROMDATE", "FLDTODATE", "FLDDURATION", "FLDSIGNOFFREASONNAME" };
        string[] alCaptions = { "S.No.","Name of Company","Name of Vessel","Rank","	VesselType","DP 1/2 /3","DP Make and Model","GRT","	Make","Model",
                                  "BHP"," Propulsion"," Operating Voltage","Supply","	Anchor Handling","	Towing","	Dive Support","	ROV","	Flotel","	Type of Installation Served","	Operating Area",
                                  "Charterer","From Date","To Date","Duration","Sign Off Reason" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewOffshoreCrewList.EmployeeExperienceSearch(
            Int32.Parse(ViewState["empid"].ToString())
            , sortexpression, sortdirection
            , 1
            , General.ShowRecords(null)
            , ref iRowCount
            , ref iTotalPageCount);
        General.ShowExcel("Crew Experience", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);

    }

    protected void CrewCompanyExperience_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("FIND"))
        {
            ViewState["PAGENUMBER"] = 1;
            BindData();
            gvCrewCompanyExperience.Rebind();
        }
        if (CommandName.ToUpper().Equals("EXCEL"))
        {
            ShowExcel();
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDROWNUMBER","FLDMANNINGCOMPANY","FLDVESSELNAME", "FLDRANKCODE", "FLDVESSELTYPENAME","FLDDPCLASS", "FLDDPMAKEANDMODEL", "FLDVESSELGT","FLDENGINENAME", "FLDENGINETYPEMODEL",
                                "FLDBHP","FLDPROPULSIONNAME", "FLDVOLTAGE","FLDSUPPLYCOUNT","FLDANCHORHANDLING","FLDTOWINGCOUTN","FLDDIVESUPPORTCOUNT","FLDROVCOUNT","FLDFLOTELCOUNT","FLDINSTALLATIONSERVED",
                                "FLDOPERATINGAREA","FLDCHARTERER","FLDFROMDATE", "FLDTODATE", "FLDDURATION", "FLDSIGNOFFREASONNAME" };
        string[] alCaptions = { "S.No.","Name of Company","Name of Vessel","Rank","	VesselType","DP 1/2 /3","DP Make and Model","GRT","	Make","Model",
                                  "BHP"," Propulsion"," Operating Voltage","Supply","	Anchor Handling","	Towing","	Dive Support","	ROV","	Flotel","	Type of Installation Served","	Operating Area",
                                  "Charterer","From Date","To Date","Duration","Sign Off Reason" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewOffshoreCrewList.EmployeeExperienceSearch(
            Int32.Parse(ViewState["empid"].ToString())
            , sortexpression, sortdirection
            , (int)ViewState["PAGENUMBER"]
            , gvCrewCompanyExperience.PageSize
            , ref iRowCount
            , ref iTotalPageCount);
        General.SetPrintOptions("gvCrewCompanyExperience", "Crew Experience", alCaptions, alColumns, ds);
        gvCrewCompanyExperience.DataSource = ds;
        gvCrewCompanyExperience.VirtualItemCount = iRowCount;


        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    public void SetEmployeePrimaryDetails()
    {
        try
        {
            DataTable dt = PhoenixCrewManagement.EmployeeList(General.GetNullableInteger(ViewState["empid"].ToString()));

            if (dt.Rows.Count > 0)
            {
                txtEmployeeNumber.Text = dt.Rows[0]["FLDEMPLOYEECODE"].ToString();
                txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
                txtLastName.Text = dt.Rows[0]["FLDLASTNAME"].ToString();
                txtFirstName.Text = dt.Rows[0]["FLDFIRSTNAME"].ToString();
                txtMiddleName.Text = dt.Rows[0]["FLDMIDDLENAME"].ToString();
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
            , companyexperienceid, Int32.Parse(ViewState["empid"].ToString()));
    }





    protected void cmdSearch_Click(object sender, EventArgs e)
    {
        BindData();

    }


    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvCrewCompanyExperience_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewCompanyExperience.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewCompanyExperience_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }

    protected void gvCrewCompanyExperience_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            if (!e.Item.Equals(DataControlRowState.Alternate | DataControlRowState.Edit) && !e.Item.Equals(DataControlRowState.Edit))
            {
                RadLabel l = (RadLabel)e.Item.FindControl("lblCrewCompanyExperienceId");
                LinkButton lb = (LinkButton)e.Item.FindControl("lnkRank");
                RadLabel lf = (RadLabel)e.Item.FindControl("lblExperienceFlag");

                if (lf.Text == "1")
                {
                    if (ViewState["type"].ToString() == "n")
                        lb.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewNewApplicantCompanyExperienceList.aspx?empid=" + ViewState["empid"].ToString() + "&CrewCompanyExperienceId=" + l.Text + "');return false;");
                    else
                        lb.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewCompanyExperienceList.aspx?type=" + ViewState["type"].ToString() + "&empid=" + ViewState["empid"].ToString() + "&CrewCompanyExperienceId=" + l.Text + "');return false;");
                }
                else
                {
                    if (PhoenixSecurityContext.CurrentSecurityContext.DatabaseCode == "OFFSHORE")
                    {

                        lb.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewoffshoreOtherCompanyExperienceAdd.aspx?type=" + ViewState["type"].ToString() + "&empid=" + ViewState["empid"].ToString() + "&CrewOtherExperienceId=" + l.Text + "');return false;");
                    }
                    else
                    {
                        lb.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Crew/CrewOtherExperienceList.aspx?type=" + ViewState["type"].ToString() + "&empid=" + ViewState["empid"].ToString() + "&CrewOtherExperienceId=" + l.Text + "');return false;");
                    }
                }
                //lb.Enabled = SessionUtil.CanAccess(this.ViewState, "EDIT");
                RadLabel lbtn = (RadLabel)e.Item.FindControl("lblManningCompanyName");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucManningCompanyTT");
                if (lbtn != null)
                {
                    uct.Position = ToolTipPosition.TopCenter;
                    uct.TargetControlId = lbtn.ClientID;
                    //lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                    //lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
                }
                RadLabel lbinstallation = (RadLabel)e.Item.FindControl("lblInstallationServed");
                UserControlToolTip ucinstallation = (UserControlToolTip)e.Item.FindControl("ucInstallationServed");
                if (lbinstallation != null)
                {
                    ucinstallation.Position = ToolTipPosition.TopCenter;
                    ucinstallation.TargetControlId = lbinstallation.ClientID;
                    //lbinstallation.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucinstallation.ToolTip + "', 'visible');");
                    //lbinstallation.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucinstallation.ToolTip + "', 'hidden');");
                }
                RadLabel lblOperatingArea = (RadLabel)e.Item.FindControl("lblOperatingArea");
                UserControlToolTip ucOperatingArea = (UserControlToolTip)e.Item.FindControl("ucOperatingArea");
                if (lblOperatingArea != null)
                {
                    ucOperatingArea.Position = ToolTipPosition.TopCenter;
                    ucOperatingArea.TargetControlId = lblOperatingArea.ClientID;
                    //lblOperatingArea.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucOperatingArea.ToolTip + "', 'visible');");
                    //lblOperatingArea.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucOperatingArea.ToolTip + "', 'hidden');");
                }
                RadLabel lblCharterer = (RadLabel)e.Item.FindControl("lblCharterer");
                UserControlToolTip ucCharterer = (UserControlToolTip)e.Item.FindControl("ucCharterer");
                if (lblCharterer != null)
                {
                    ucCharterer.Position = ToolTipPosition.TopCenter;
                    ucCharterer.TargetControlId = lblCharterer.ClientID;
                    //lblCharterer.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucCharterer.ToolTip + "', 'visible');");
                    //lblCharterer.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucCharterer.ToolTip + "', 'hidden');");
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
}
