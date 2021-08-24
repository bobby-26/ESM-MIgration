using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class CrewOffshoreReliever : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {

        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshoreReliever.aspx" + (Request.QueryString.ToString() != string.Empty ? "?" + Request.QueryString.ToString() : string.Empty), "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbarsub.Show();

            if (!IsPostBack)
            {
                ViewState["rankid"] = "";
                ViewState["vesselid"] = "";
                ViewState["vsltype"] = "";
                ViewState["reliefdate"] = "";
                ViewState["offsignerid"] = "";
                ViewState["charterer"] = "";

                if (Request.QueryString["p"] != null && Request.QueryString["p"].ToString() != string.Empty)
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["p"]);
                else
                    ViewState["PAGENUMBER"] = 1;

                if (Request.QueryString["hidemenu"] != null && Request.QueryString["hidemenu"].ToString() != "")
                    CrewQueryMenu.Title = "";//.ShowMenu = "false";

                if (Request.QueryString["rankid"] != null && Request.QueryString["rankid"].ToString() != "")
                    ViewState["rankid"] = Request.QueryString["rankid"].ToString();

                if (Request.QueryString["vesselid"] != null && Request.QueryString["vesselid"].ToString() != "")
                    ViewState["vesselid"] = Request.QueryString["vesselid"].ToString();

                if (Request.QueryString["vsltype"] != null && Request.QueryString["vsltype"].ToString() != "")
                    ViewState["vsltype"] = Request.QueryString["vsltype"].ToString();

                if (Request.QueryString["reliefdate"] != null && Request.QueryString["reliefdate"].ToString() != "")
                    ViewState["reliefdate"] = Request.QueryString["reliefdate"].ToString();

                if (Request.QueryString["offsignerid"] != null && Request.QueryString["offsignerid"].ToString() != "")
                    ViewState["offsignerid"] = Request.QueryString["offsignerid"].ToString();

                SetVesselInfo();

                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;

                BindTrainingMatrix();
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
            //  BindData();
            CreateTabs();
            CrewRelieverTabs.SelectedMenuIndex = 0;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void SetVesselInfo()
    {
        ViewState["vsltype"] = "";
        ViewState["charterer"] = "";

        if (General.GetNullableInteger(ViewState["vesselid"].ToString()) != null && ViewState["vesselid"].ToString() != "")
        {
            DataSet ds = PhoenixRegistersVessel.EditVessel(int.Parse(ViewState["vesselid"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["vsltype"] = ds.Tables[0].Rows[0]["FLDTYPE"].ToString();
                ViewState["charterer"] = ds.Tables[0].Rows[0]["FLDCHARTERER"].ToString();
            }
        }
    }

    protected void BindTrainingMatrix()
    {
        if (ViewState["vsltype"] != null && ViewState["vsltype"].ToString() != ""
            && ViewState["rankid"] != null && ViewState["rankid"].ToString() != ""
            && ViewState["charterer"] != null && ViewState["charterer"].ToString() != "")
        {
            ddlTrainingMatrix.Items.Clear();
            ddlTrainingMatrix.DataSource = PhoenixCrewOffshoreTrainingMatrix.CrewOffshoreTrainingMatrixList(
                                                General.GetNullableInteger(ViewState["vsltype"].ToString()),
                                                General.GetNullableInteger(ViewState["rankid"].ToString()),
                                                General.GetNullableInteger(ViewState["charterer"].ToString()));
        }
        ddlTrainingMatrix.DataTextField = "FLDMATRIXNAME";
        ddlTrainingMatrix.DataValueField = "FLDMATRIXID";
        ddlTrainingMatrix.DataBind();
        ddlTrainingMatrix.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
        //if (!IsPostBack)
        //{
        //    if (ddlTrainingMatrix.Items.Count > 0)
        //        ddlTrainingMatrix.SelectedIndex = 1;
        //}
    }

    private void CreateTabs()
    {
        PhoenixToolbar toolbarmain = new PhoenixToolbar();
        toolbarmain.AddButton("Reliever", "RELIEVER", ToolBarDirection.Right);
        toolbarmain.AddButton("Relief Plan", "RELIEFPLAN", ToolBarDirection.Right);


        CrewRelieverTabs.AccessRights = this.ViewState;
        CrewRelieverTabs.MenuList = toolbarmain.Show();
        CrewRelieverTabs.SelectedMenuIndex = 1;
    }

    protected void CrewRelieverTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("RELIEFPLAN"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshoreReliefPlan.aspx" + (Request.QueryString.ToString() != string.Empty ? ("?" + Request.QueryString.ToString()) : string.Empty), false);
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }



    protected void CrewQueryMenu_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("SEARCH"))
            {
                Response.Redirect("CrewOffshoreAvailabilitySearch.aspx", true);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentOffshoreAvailabilitySearch = null;
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewSearch_Sorting(object sender, GridViewSortEventArgs se)
    {
        GridView _gridView = (GridView)sender;
        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO", "FLDLASTVESSELNAME", "FLDPRESENTVESSELNAME", "FLDLASTSIGNOFFDATE", "FLDSTATUS", "FLDDOA" };
        string[] alCaptions = { "Name", "Rank", "File No", "Last Vessel", "Present Vessel", "Last Sign-Off Date", "Status", "DOA" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataTable dt = PhoenixCrewOffshoreCrewChange.CrewOffshoreRelieverSearch(null, null
                , General.GetNullableInteger(ViewState["vesselid"].ToString())
                , General.GetNullableInteger(ViewState["rankid"].ToString())
                , General.GetNullableDateTime(ViewState["reliefdate"].ToString())
                , General.GetNullableInteger(ddlTrainingMatrix.SelectedValue)
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"], iRowCount
                , ref iRowCount, ref iTotalPageCount
                , (Request.QueryString["pl"] != null ? Request.QueryString["pl"] : string.Empty)
                , General.GetNullableInteger(ViewState["offsignerid"].ToString()));

        General.ShowExcel("Reliever", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            DataTable dt = PhoenixCrewOffshoreCrewChange.CrewOffshoreRelieverSearch(null, null
                , General.GetNullableInteger(ViewState["vesselid"].ToString())
                , General.GetNullableInteger(ViewState["rankid"].ToString())
                , General.GetNullableDateTime(ViewState["reliefdate"].ToString())
                , General.GetNullableInteger(ddlTrainingMatrix.SelectedValue)
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                , ref iRowCount, ref iTotalPageCount
                , (Request.QueryString["pl"] != null ? Request.QueryString["pl"] : string.Empty)
                , General.GetNullableInteger(ViewState["offsignerid"].ToString()));
            gvCrewSearch.DataSource = dt;
            gvCrewSearch.VirtualItemCount = iRowCount;


            ViewState["ROWCOUNT"] = iRowCount;
            ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void gvCrewSearch_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvCrewSearch_ItemCommand(object sender, GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }

    }

    protected void gvCrewSearch_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton ac = (LinkButton)e.Item.FindControl("imgActivity");
            if (ac != null)
            {
                ac.Attributes.Add("onclick", "parent.openNewWindow('codehelpactivity', '', '" + Session["sitepath"] + "/Crew/CrewActivityGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&sg=0&ds=0');return false;");
            }
            LinkButton imgSuitability = (LinkButton)e.Item.FindControl("imgSuitableCheck");
            if (imgSuitability != null)
            {
                if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                    imgSuitability.Attributes.Add("onclick", "parent.openNewWindow('codehelpsuitability', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid="
                        + drv["FLDEMPLOYEEID"].ToString()
                        + "&personalmaster=true&vesselid=" + ViewState["vesselid"].ToString()
                        + "&rankid=" + ViewState["rankid"].ToString()
                        + "&reliefdate=" + ViewState["reliefdate"].ToString()
                        + "&trainingmatrixid=" + General.GetNullableInteger(ddlTrainingMatrix.SelectedValue)
                        + "&offsignerid=" + ViewState["offsignerid"] + "&popup=1');return false;");
                else if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) == null)
                    imgSuitability.Attributes.Add("onclick", "parent.openNewWindow('codehelpsuitability', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid="
                        + drv["FLDEMPLOYEEID"].ToString()
                        + "&newapplicant=true&vesselid=" + ViewState["vesselid"].ToString()
                        + "&rankid=" + ViewState["rankid"].ToString()
                        + "&reliefdate=" + ViewState["reliefdate"].ToString()
                        + "&trainingmatrixid=" + General.GetNullableInteger(ddlTrainingMatrix.SelectedValue)
                        + "&offsignerid=" + ViewState["offsignerid"] + "&popup=1');return false;");
            }

            LinkButton lnkEployeeName = (LinkButton)e.Item.FindControl("lnkEployeeName");
            Label lblEmployeeid = (Label)e.Item.FindControl("lblEmployeeid");
            if (lnkEployeeName != null)
            {
                //lnkEployeeName.Attributes.Add("onclick", "parent.Openpopup('PDForm', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + lblEmployeeid.Text + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
                if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEployeeName.Attributes.Add("onclick", "javascript:parent.openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore" + "'); return true;");
                else if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) == null)
                    lnkEployeeName.Attributes.Add("onclick", "javascript:parent.openNewWindow('CrewPage','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore" + "'); return true;");
            }
            Label lblPlannedVessel = (Label)e.Item.FindControl("lblPlannedVessel");
            UserControlToolTip ucToolTipPlannedVessel = (UserControlToolTip)e.Item.FindControl("ucToolTipPlannedVessel");
            lblPlannedVessel.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipPlannedVessel.ToolTip + "', 'visible');");
            lblPlannedVessel.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipPlannedVessel.ToolTip + "', 'hidden');");

            Label lblEmpName = (Label)e.Item.FindControl("lblEmpName");
            if (drv["FLDVIEWPARTICULARSYN"] != null && drv["FLDVIEWPARTICULARSYN"].ToString().Equals("1"))
            {
                lblEmpName.Visible = false;
                lnkEployeeName.Visible = true;
            }
            else
            {
                lblEmpName.Visible = true;
                lnkEployeeName.Visible = false;
            }

            if (lnkEployeeName != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, lnkEployeeName.CommandName)) lnkEployeeName.Visible = false;
            }
            if (ac != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ac.CommandName)) ac.Visible = false;
            }
            if (imgSuitability != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgSuitability.CommandName)) imgSuitability.Visible = false;
            }
        }
    }

    protected void gvCrewSearch_SortCommand(object sender, GridSortCommandEventArgs e)
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
