using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
public partial class CrewOffshorePlannerGridDetails : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            //toolbar.AddImageButton("../CrewOffshore/CrewOffshorePlannerGridDetails.aspx", "Export to Excel", "icon_xls.png", "EXCEL");
            toolbar.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePlannerGridDetails.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            //toolbar.AddImageButton("../CrewOffshore/CrewOffshorePlannerGridDetails.aspx", "Clear Filter", "clear-filter.png", "CLEAR");
            MenuExcel.AccessRights = this.ViewState;
            MenuExcel.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["rankid"] = "";
                ViewState["vesseltypeid"] = "";
                ViewState["date"] = "";

                if (Request.QueryString["rankid"] != null)
                    ViewState["rankid"] = Request.QueryString["rankid"].ToString();
                if (Request.QueryString["vesseltypeid"] != null)
                    ViewState["vesseltypeid"] = Request.QueryString["vesseltypeid"].ToString();
                if (Request.QueryString["date"] != null)
                    ViewState["date"] = Request.QueryString["date"].ToString();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                
               

                DateTime now = DateTime.Now;
                DateTime lastonemonth = now.AddDays(-30);
                DateTime nexttwomonths = now.AddDays(60);
                txtDOAFrom.Text = lastonemonth.ToString("dd/MM/yyyy");
                txtDOATo.Text = nexttwomonths.ToString("dd/MM/yyyy");
            }
            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Back", "PLANNERGRID",ToolBarDirection.Right);

            MenuPlannerGrid.AccessRights = this.ViewState;
            MenuPlannerGrid.MenuList = toolbar.Show();
           // BindReliefPlan();
            BindAvailability();
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void MenuPlannerGrid_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("PLANNERGRID"))
            {
                Response.Redirect("../CrewOffshore/CrewOffshorePlannerGrid.aspx?rankid=" + ViewState["rankid"].ToString() + "&vesseltypeid=" + ViewState["vesseltypeid"].ToString() + "&date=" + ViewState["date"].ToString() );
            }
           
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void CrewShowExcel_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;


            if (CommandName.ToUpper().Equals("FIND"))
            {
                BindAvailability();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    private void BindReliefPlan()
    {
        DataTable dt = PhoenixCrewOffshorePlanner.CrewOffshorePlannerDetails(General.GetNullableInteger(ViewState["rankid"].ToString())
            , General.GetNullableInteger(ViewState["vesseltypeid"].ToString())
            , General.GetNullableDateTime(ViewState["date"].ToString()));

        if (dt.Rows.Count > 0)
        {
            gvCrewPlan.DataSource = dt;
          

            txtVesselType.Text = dt.Rows[0]["FLDVESSELTYPENAME"].ToString();
            txtRank.Text = dt.Rows[0]["FLDRANKNAME"].ToString();
            txtDate.Text = General.GetDateTimeToString(dt.Rows[0]["FLDDATE"].ToString());
        }
        else
        {
            gvCrewPlan.DataSource = dt;
        }
    }
    public void BindAvailability()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        try
        {

            DataSet ds = PhoenixCrewOffshorePlanner.CrewOffshorePlannerAvailability(General.GetNullableInteger(ViewState["rankid"].ToString())
                , General.GetNullableInteger(ViewState["vesseltypeid"].ToString())
                , sortexpression, sortdirection
                , (int)ViewState["PAGENUMBER"], gvCrewSearch.PageSize
                , ref iRowCount, ref iTotalPageCount
                , General.GetNullableInteger(chkShowAll.Checked ? "1" : "0")
                , General.GetNullableDateTime(txtDOAFrom.Text)
                , General.GetNullableDateTime(txtDOATo.Text)
                );
            gvCrewSearch.DataSource = ds.Tables[0];
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


    protected void gvCrewPlan_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            GridView HeaderGrid = (GridView)sender;
            GridViewRow HeaderGridRow = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Insert);

            TableCell HeaderCell;
            HeaderCell = new TableCell();
            HeaderCell.Text = "";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 1;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Due for Sign Off ";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 4;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Approved for Vessel";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);

            HeaderCell = new TableCell();
            HeaderCell.Text = "Proposed";
            HeaderCell.HorizontalAlign = HorizontalAlign.Center;
            HeaderCell.ColumnSpan = 3;
            HeaderGridRow.Cells.Add(HeaderCell);

            gvCrewPlan.Controls[0].Controls.AddAt(0, HeaderGridRow);
        }
    }

    //protected void gvCrewSearch_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {

    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());
    //        Label lblEmployeeid = (Label)_gridView.Rows[nCurrentRow].FindControl("lblEmployeeid");

    //        if (e.CommandName.ToUpper().Equals("SUITABILITYCHECK"))
    //        {
    //            Response.Redirect("../Crew/CrewActivityGeneral.aspx?empid=" + lblEmployeeid.Text + "&sg=0&ds=0&launchedfrom=offshore');return false;");
    //            //ac.Attributes.Add("onclick", "parent.Openpopup('codehelpactivity', '', '../Crew/CrewActivityGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&sg=0&ds=0&launchedfrom=offshore');return false;");
    //        }
    //        //else if (e.CommandName.ToUpper().Equals("ACTIVITY"))
    //        //{
    //        //    Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentRevisionList.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"]);
    //        //}
    //        //else if (e.CommandName.ToUpper().Equals("PDFORM"))
    //        //{
    //        //    Response.Redirect("../DocumentManagement/DocumentManagementAdminDocumentRevisionList.aspx?DOCUMENTID=" + ViewState["DOCUMENTID"]);
    //        //}
    //    }

    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;

    //    }
    //}

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDNATIONALITY", "FLDSTATUSNAME", "FLDPLANNEDVESSELNAME",
                                 "FLDLASTVESSELNAME", "FLDVESSELTYPENAME", "FLDLASTSIGNOFFDATE", "FLDDAILYRATEUSD", "FLDFLDDPALLOWANCEUSD", "FLDDOA", "FLDLASTCONTACTDATE" };
        string[] alCaptions = { "File No", "Name", "Rank", "Nationality", "Status", "Planned Vessel", "Last Vessel", "Last Vessel Type",
                                  "Last Sign-Off Date", "Last drawn salary/day(USD)", "DP Allowance/day (USD)", "DOA", "Last Contact" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        DataSet ds = PhoenixCrewOffshorePlanner.CrewOffshorePlannerAvailability(General.GetNullableInteger(ViewState["rankid"].ToString())
                 , General.GetNullableInteger(ViewState["vesseltypeid"].ToString())
                 //, General.GetNullableDateTime(ViewState["date"].ToString())
                 , sortexpression, sortdirection
                 , (int)ViewState["PAGENUMBER"], General.ShowRecords(null)
                 , ref iRowCount, ref iTotalPageCount
                 , General.GetNullableInteger(chkShowAll.Checked ? "1" : "0")
                 , General.GetNullableDateTime(txtDOAFrom.ToString())
                , General.GetNullableDateTime(txtDOATo.ToString())
                 );

        General.ShowExcel("Reliever", ds.Tables[0], alColumns, alCaptions, sortdirection, sortexpression);
    }

    protected void gvCrewPlan_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindReliefPlan();
    }

    protected void gvCrewSearch_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewSearch.CurrentPageIndex + 1;
            BindAvailability();
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

            LinkButton lnkName = (LinkButton)e.Item.FindControl("lnkName");
            RadLabel lblEmpName = (RadLabel)e.Item.FindControl("lblEmpName");
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            RadLabel lblRankId = (RadLabel)e.Item.FindControl("lblRankId");

            if (lnkName != null) lnkName.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "');return false;");
            RadLabel lblPlannedVessel = (RadLabel)e.Item.FindControl("lblPlannedVessel");
            UserControlToolTip ucToolTipPlannedVessel = (UserControlToolTip)e.Item.FindControl("ucToolTipPlannedVessel");
            lblPlannedVessel.Attributes.Add("onmouseover", "showTooltip(ev, '" + ucToolTipPlannedVessel.ToolTip + "', 'visible');");
            lblPlannedVessel.Attributes.Add("onmouseout", "showTooltip(ev, '" + ucToolTipPlannedVessel.ToolTip + "', 'hidden');");

            LinkButton ac = (LinkButton)e.Item.FindControl("imgActivity");
            if (ac != null)
            {
                ac.Attributes.Add("onclick", "parent.openNewWindow('codehelpactivity', '', '"+Session["sitepath"]+"/Crew/CrewActivityGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&sg=0&ds=0&launchedfrom=offshore');return false;");
            }
            LinkButton imgSuitability = (LinkButton)e.Item.FindControl("imgSuitableCheck");
            if (imgSuitability != null)
            {
                if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                    imgSuitability.Attributes.Add("onclick", "parent.openNewWindow('codehelpsuitability', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&personalmaster=true&popup=1');return false;");
                else if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) == null)
                    imgSuitability.Attributes.Add("onclick", "parent.Openpopup('codehelpsuitability', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&newapplicant=true&popup=1');return false;");
            }

            LinkButton pd = (LinkButton)e.Item.FindControl("cmdPDForm");
            if (pd != null)
                pd.Attributes.Add("onclick", "openNewWindow('CrewPage', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=EMPLOYEECV&employeeid=" + drv["FLDEMPLOYEEID"].ToString() + "&rankid=" + lblRankId.Text + "');return false;");

            if (ac != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ac.CommandName)) ac.Visible = false;
            }
            if (imgSuitability != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgSuitability.CommandName)) imgSuitability.Visible = false;
            }
            if (pd != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, pd.CommandName)) pd.Visible = false;
            }

            if (drv["FLDVIEWPARTICULARSYN"] != null && drv["FLDVIEWPARTICULARSYN"].ToString().Equals("1"))
            {
                lblEmpName.Visible = false;
                lnkName.Visible = true;
            }
            else
            {
                lblEmpName.Visible = true;
                lnkName.Visible = false;
            }
        }

    }

    protected void gvCrewPlan_ItemCommand(object sender, GridCommandEventArgs e)
    {

    }

    protected void gvCrewPlan_ItemDataBound(object sender, GridItemEventArgs e)
    {
      
        if (e.Item is GridDataItem)
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            LinkButton lnkOffSigner = (LinkButton)e.Item.FindControl("lnkOffSigner");
            LinkButton lnkOnSigner = (LinkButton)e.Item.FindControl("lnkOnSigner");
            LinkButton lnkRecommendedEmployee = (LinkButton)e.Item.FindControl("lnkRecommendedEmployee");
            RadLabel lblOffsignerName = (RadLabel)e.Item.FindControl("lblOffsignerName");
            RadLabel lblOnSignerName = (RadLabel)e.Item.FindControl("lblOnSignerName");
            RadLabel lblRecommendedName = (RadLabel)e.Item.FindControl("lblRecommendedName");

            if (lnkOffSigner != null) lnkOffSigner.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '"+Session["sitepath"]+"/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + drv["FLDOFFSIGNER"].ToString() + "');return false;");
            if (lnkOnSigner != null) lnkOnSigner.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + drv["FLDONSIGNER"].ToString() + "');return false;");
            if (lnkRecommendedEmployee != null) lnkRecommendedEmployee.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + drv["FLDREC"].ToString() + "');return false;");


            if (drv["FLDOFFSIGNERVIEWPARTICULARSYN"] != null && drv["FLDOFFSIGNERVIEWPARTICULARSYN"].ToString().Equals("1"))
            {
                lblOffsignerName.Visible = false;
                lnkOffSigner.Visible = true;
            }
            else
            {
                lblOffsignerName.Visible = true;
                lnkOffSigner.Visible = false;
            }

            if (drv["FLDONSIGNERVIEWPARTICULARSYN"] != null && drv["FLDONSIGNERVIEWPARTICULARSYN"].ToString().Equals("1"))
            {
                lblOnSignerName.Visible = false;
                lnkOnSigner.Visible = true;
            }
            else
            {
                lblOnSignerName.Visible = true;
                lnkOnSigner.Visible = false;
            }

            if (drv["FLDRECOMMENDEDPARTICULARSYN"] != null && drv["FLDRECOMMENDEDPARTICULARSYN"].ToString().Equals("1"))
            {
                lblRecommendedName.Visible = false;
                lnkRecommendedEmployee.Visible = true;
            }
            else
            {
                lblRecommendedName.Visible = true;
                lnkRecommendedEmployee.Visible = false;
            }
        }
    }
}
