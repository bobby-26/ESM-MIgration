using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;

public partial class CrewOffshorePendingReview : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbarsub = new PhoenixToolbar();
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePendingReview.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePendingReviewSearch.aspx", "Filter", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbarsub.AddFontAwesomeButton("../CrewOffshore/CrewOffshorePendingReview.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            CrewQueryMenu.AccessRights = this.ViewState;
            CrewQueryMenu.MenuList = toolbarsub.Show();
            cmdHiddenSubmit.Style.Add("display", "none");
            if (!IsPostBack)
            {
                if (Request.QueryString["p"] != null && Request.QueryString["p"].ToString() != string.Empty)
                    ViewState["PAGENUMBER"] = int.Parse(Request.QueryString["p"]);
                else
                    ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CURRENTINDEX"] = 1;
                gvCrewSearch.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }
           // BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
   

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();
        gvCrewSearch.Rebind();

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
                Response.Redirect("CrewOffshorePendingReviewSearch.aspx", true);
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentOffshorePendingReviewSearch = null;
                BindData();
                gvCrewSearch.Rebind();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string bl = string.Empty;
        string RE = string.Empty;

        string[] alColumns = { "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDFILENO", "FLDSTATUS", "FLDLASTVESSELNAME",
                                  "FLDLASTSIGNOFFDATE", "FLDDAILYRATEUSD", "FLDDPALLOWANCEUSD", "FLDDOA", "FLDLASTCONTACTDATE" };
        string[] alCaptions = { "Name", "Rank", "File No", "Status" , "Last Vessel",
                                  "Last Sign-Off Date", "Last drawn salary/day", "DP Allowance/day", "DOA", "Last Contact" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        NameValueCollection nvc = Filter.CurrentOffshorePendingReviewSearch;
        
        if (nvc != null)
        {
            bl = nvc.Get("chkDOAGivenYN") == "1" ? "1" : "0";
            RE = nvc.Get("chkReEmploymentNotGivenYN") == "1" ? "1" : "0";
        }
        DataTable dt = PhoenixCrewOffshoreEmployee.CrewOffshorePendingReviewSearch(nvc != null ? nvc["txtName"] : ""
                                                                  , nvc != null ? nvc["txtFileNo"] : ""
                                                                  , General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : "")
                                                                  , General.GetNullableInteger(bl.ToString())
                                                                   , General.GetNullableInteger(RE.ToString())
                                                                   , sortexpression, sortdirection
                                                                   , 1, iRowCount
                                                                   , ref iRowCount, ref iTotalPageCount
                                                               );

        General.ShowExcel("Pending Review", dt, alColumns, alCaptions, sortdirection, sortexpression);
    }

    public void BindData()
    {

        int iRowCount = 0;
        int iTotalPageCount = 0;
        string bl = string.Empty;
        string RE = string.Empty;
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        try
        {
            NameValueCollection nvc = Filter.CurrentOffshorePendingReviewSearch;

            if (nvc != null)
            {
                bl = nvc.Get("chkDOAGivenYN") == "1" ? "1" : "0";
                RE= nvc.Get("chkReEmploymentNotGivenYN") == "1" ? "1" : "0";
            }

            DataTable dt = PhoenixCrewOffshoreEmployee.CrewOffshorePendingReviewSearch(nvc != null ? nvc["txtName"] : ""
                                                                  , nvc != null ? nvc["txtFileNo"] : ""
                                                                  , General.GetNullableInteger(nvc != null ? nvc["ddlRank"] : "")
                                                                  , General.GetNullableInteger(bl.ToString())
                                                                   , General.GetNullableInteger(RE.ToString())
                                                                  //, General.GetNullableInteger(nvc != null ? nvc["chkDOAGivenYN"] : "")
                                                                  , sortexpression, sortdirection
                                                                  , (int)ViewState["PAGENUMBER"], gvCrewSearch.PageSize
                                                                  , ref iRowCount, ref iTotalPageCount
                                                              );
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

    protected void gvCrewSearch_ItemCommand(object sender, Telerik.Web.UI.GridCommandEventArgs e)
    {
        if (e.CommandName == "Page")
        {
            ViewState["PAGENUMBER"] = null;
        }
    }

    protected void gvCrewSearch_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        
        if (e.Item is GridDataItem)
        {

            DataRowView drv = (DataRowView)e.Item.DataItem;
            LinkButton ac = (LinkButton)e.Item.FindControl("imgActivity");
            LinkButton ass = (LinkButton)e.Item.FindControl("cmdAssess");
            if (ass != null)
            {
                ass.Attributes.Add("onclick", "parent.openNewWindow('Filter', '', '"+Session["sitepath"]+"/Crew/CrewAssessReEmployment.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&PendingReviewid=" + drv["FLDEMPLOYEEPENDINGREVIEWID"].ToString() + "&sg=0&ds=0&launchedfrom=offshore');return false;");
            }
            if (ac != null)
            {
                ac.Attributes.Add("onclick", "parent.openNewWindow('codehelpactivity', '', '" + Session["sitepath"] + "/Crew/CrewActivityGeneral.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&sg=0&ds=0&launchedfrom=offshore');return false;");
            }
            LinkButton imgSuitability = (LinkButton)e.Item.FindControl("imgSuitableCheck");
            if (imgSuitability != null)
            {
                if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                    imgSuitability.Attributes.Add("onclick", "parent.openNewWindow('codehelpsuitability', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&personalmaster=true&popup=1');return false;");
                else if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) == null)
                    imgSuitability.Attributes.Add("onclick", "parent.openNewWindow('codehelpsuitability', '', '" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreSuitabilityCheck.aspx?empid=" + drv["FLDEMPLOYEEID"].ToString() + "&newapplicant=true&popup=1');return false;");
            }

            LinkButton lnkEployeeName = (LinkButton)e.Item.FindControl("lnkEployeeName");
            RadLabel lblEmployeeid = (RadLabel)e.Item.FindControl("lblEmployeeid");
            RadLabel lblRankId = (RadLabel)e.Item.FindControl("lblRankId");
            RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
            if (lnkEployeeName != null)
            {
                if (drv["FLDEMPLOYEECODE"] != null && General.GetNullableString(drv["FLDEMPLOYEECODE"].ToString()) != null)
                    lnkEployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshorePersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                else
                    lnkEployeeName.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/CrewOffshore/CrewOffshoreNewApplicantPersonalGeneral.aspx?empid=" + lblEmployeeid.Text + "&launchedfrom=offshore'); return false;");
                //lnkEployeeName.Attributes.Add("onclick", "Openpopup('PDForm', '', '../Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=PDFORM&empid=" + lblEmployeeid.Text + "&rowusercode=" + PhoenixSecurityContext.CurrentSecurityContext.UserCode + "&showmenu=0');return false;");
            }

            LinkButton pd = (LinkButton)e.Item.FindControl("cmdPDForm");
            if (pd != null)
                pd.Attributes.Add("onclick", "openNewWindow('CrewPage', '', '" + Session["sitepath"] + "/Reports/ReportsViewWithSubReport.aspx?applicationcode=4&reportcode=EMPLOYEECV&employeeid=" + lblEmployeeid.Text + "&rankid=" + lblRankId.Text + "');return false;");


            RadLabel lblName = (RadLabel)e.Item.FindControl("lblName");
            if (drv["FLDVIEWPARTICULARSYN"] != null && drv["FLDVIEWPARTICULARSYN"].ToString().Equals("1"))
            {
                lblName.Visible = false;
                lnkEployeeName.Visible = true;
            }
            else
            {
                lblName.Visible = true;
                lnkEployeeName.Visible = false;
            }

            if (ac != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, ac.CommandName)) ac.Visible = false;
            }
            if (imgSuitability != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, imgSuitability.CommandName)) imgSuitability.Visible = false;
            }
            if (lnkEployeeName != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, lnkEployeeName.CommandName)) lnkEployeeName.Visible = false;
            }
            if (pd != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, pd.CommandName)) pd.Visible = false;
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
