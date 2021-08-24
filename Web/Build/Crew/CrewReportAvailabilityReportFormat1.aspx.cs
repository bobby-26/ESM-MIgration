using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Registers;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using System.Text;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewAvailabilityReport : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            MenuReportsFilter.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Format 2", "FORMAT2", ToolBarDirection.Right);
            toolbar.AddButton("Format 1", "FORMAT1",ToolBarDirection.Right);
            
            MenuRecStatTabs.AccessRights = this.ViewState;
            MenuRecStatTabs.MenuList = toolbar.Show();
            MenuRecStatTabs.SelectedMenuIndex = 1;


            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportAvailabilityReportFormat1.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportAvailabilityReportFormat1.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {

                ucNationality.NationalityList = PhoenixRegistersCountry.ListNationality();
                ucPool.PoolList = PhoenixRegistersMiscellaneousPoolMaster.ListMiscellaneousPoolMaster();
                ucZone.ZoneList = PhoenixRegistersMiscellaneousZoneMaster.ListMiscellaneousZoneMaster();
                ucRank.RankList = PhoenixRegistersRank.ListRank();
                ddlVesselType.VesselTypeList = PhoenixRegistersVesselType.ListVesselType(0);
                DataSet ds = PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                           , 54, 1, string.Empty);
                ds.Tables[0].Merge(PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                   , 53, 1, string.Empty).Tables[0]);
                lstStatus.AddressList = ds;

                lstStatus.DataBind();

                ViewState["PAGENUMBER"] = 1;

                ucDate1.Text = DateTime.Now.ToShortDateString();
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            }

        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }


    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucDate.Text, ucDate1.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                else
                {
                    NameValueCollection criteria = new NameValueCollection();
                    criteria.Clear();
                    criteria.Add("ucRank", (ucRank.selectedlist) == "," ? null : ucRank.selectedlist);
                    criteria.Add("ucNationality", (ucNationality.SelectedList) == "Dummy" ? null : ucNationality.SelectedList);
                    criteria.Add("ucZone", (ucZone.selectedlist) == "Dummy" ? null : ucZone.selectedlist);
                    criteria.Add("ucPool", (ucPool.SelectedPool) == "," ? null : ucPool.SelectedPool);
                    criteria.Add("ucName", txtName.Text);
                    criteria.Add("ucDate", ucDate.Text);
                    criteria.Add("ucDate1", ucDate1.Text);
                    criteria.Add("ucPrincipal", ucPrinicpal.SelectedAddress);
                    criteria.Add("newappln", (chkNewApp.Checked==true) ? "1" : "0");
                    criteria.Add("inactive", (chkInactive.Checked==true) ? "1" : "0");
                    criteria.Add("vesseltype", (ddlVesselType.SelectedVesseltype) == "Dummy" ? null : ddlVesselType.SelectedVesseltype);
                    criteria.Add("includepastexp", (chkIncludepastexp.Checked==true) ? "1" : "0");
                    criteria.Add("ucStatus", lstStatus.SelectedList);
                    criteria.Add("chkFollowup", ((chkFollowup.Checked == true) ? PhoenixSecurityContext.CurrentSecurityContext.UserCode.ToString() : ""));
                    criteria.Add("ucSeafarerRequirement", ucSeafarerRequirement.SelectedQuick);

                    Filter.CurrentAvailabilityFilter = criteria;
                }
                ShowReport();
                gvCrew.Rebind();
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

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }

            if (CommandName.ToUpper().Equals("CLEAR"))
            {
                ucRank.SelectedRankValue = "";
                ucNationality.SelectedNationalityValue = "";
                ucZone.SelectedZoneValue = "";
                ucPool.SelectedPoolValue = "";
                txtName.Text = "";
                ucDate.Text = "";
                ucPrinicpal.SelectedAddress = "";
                ucDate1.Text = DateTime.Now.ToShortDateString();
                chkInactive.Checked = false;
                chkNewApp.Checked = false;
                ddlVesselType.SelectedVesseltype = "";
                chkIncludepastexp.Checked = false;
                ucPrinicpal.SelectedAddress = "";
                ddlVesselType.SelectedVesseltype = "";
                Filter.CurrentAvailabilityFilter = null;
                chkFollowup.Checked = false;
                ucSeafarerRequirement.SelectedQuick = "";
                lstStatus.SelectedValue = "";
                ShowReport();
                gvCrew.Rebind();
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
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDMANNINGCOMPANY", "FLDLASTVESSEL", "FLDLASTSIGNOFF", "FLDDOA", "FLDLASTCONTACT", "FLDLASTCONTACTDATE", "FLDLEAVEREMARKS", "FLDSTATUS", "FLDSEAFARERREQUIREMENT" };
        string[] alCaptions = { "S.No.", "File No", "Name", "Rank", "Zone", "Last Sign Off Vessel", "Last Sign Off Date", "DOA", "Last Contact By", "Last Contact On", "Last Remarks", "Status", "Requirement" };
        string[] FilterColumns = { "FLDSELECTEDFROMDATE", "FLDSELECTEDTODATE", "FLDSELECTEDPRINCIPAL", "FLDSELECTEDNATIONALITY", "FLDSELECTEDZONE", "FLDSELECTEDPOOL", "FLDSELECTEDRANK", "FLDSEARCHNAME", "FLDNEWAPPLICANT", "FLDINACTIVE" };
        string[] FilterCaptions = { "From Date", "To Date", "Principal", "Nationality", "Zone", "Pool", "Rank", "Name", "New Applicant", "Inactive" };

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        NameValueCollection nvc = Filter.CurrentAvailabilityFilter;
        if (nvc == null)
        {
            ds = PhoenixCrewAvailabilityReport.CrewAvailabilityReport(
                       General.GetNullableString(null),
                       General.GetNullableString(null),
                       General.GetNullableString(null),
                       General.GetNullableString(null),
                       General.GetNullableString(null),
                       General.GetNullableDateTime(null),
                       General.GetNullableDateTime(null),
                       General.GetNullableString(null),
                       General.GetNullableInteger(null),
                       General.GetNullableInteger(null),
                       Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                       General.ShowRecords(null),
                       ref iRowCount,
                       ref iTotalPageCount,
                       null,
                       null,
                       null, null, null, null);
        }
        else
        {
            ds = PhoenixCrewAvailabilityReport.CrewAvailabilityReport(
                           General.GetNullableString(nvc.Get("ucRank")),
                           General.GetNullableString(nvc.Get("ucNationality")),
                           General.GetNullableString(nvc.Get("ucZone")),
                           General.GetNullableString(nvc.Get("ucPool")),
                           General.GetNullableString(nvc.Get("ucName")),
                           DateTime.Parse(nvc.Get("ucDate")),
                           DateTime.Parse(nvc.Get("ucDate1")),
                           General.GetNullableString(nvc.Get("ucPrincipal")),
                           General.GetNullableInteger(nvc.Get("newappln")),
                           General.GetNullableInteger(nvc.Get("inactive")),
                           Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                           iRowCount,
                           ref iRowCount,
                           ref iTotalPageCount,
                           General.GetNullableString(nvc.Get("vesseltype")),
                           General.GetNullableInteger(nvc.Get("includepastexp")),
                           General.GetNullableInteger(nvc.Get("pvtype")),
                           General.GetNullableString(nvc.Get("ucStatus")),
                           General.GetNullableInteger(nvc.Get("chkFollowup")),
                           General.GetNullableInteger(nvc.Get("ucSeafarerRequirement"))
                           );
        }

        Response.AddHeader("Content-Disposition", "attachment; filename=CrewAvailabilityReportFormat1.xls");
        Response.ContentType = "application/x-excel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>Crew Availability Report</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");
            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }
    private void ShowReport()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;


        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDMANNINGCOMPANY", "FLDLASTVESSEL", "FLDLASTSIGNOFF", "FLDDOA", "FLDLASTCONTACT", "FLDLASTCONTACTDATE", "FLDLEAVEREMARKS", "FLDSTATUS", "FLDSEAFARERREQUIREMENT" };
        string[] alCaptions = { "S.No.", "File No", "Name", "Rank", "Zone", "Last Sign Off Vessel", "Last Sign Off Date", "DOA", "Last Contact By", "Last Contact On", "Last Remarks", "Status", "Requirement" };
        NameValueCollection nvc = Filter.CurrentAvailabilityFilter;
        DataSet ds = new DataSet();
        if (nvc == null)
        {
            ds = PhoenixCrewAvailabilityReport.CrewAvailabilityReport(
                       General.GetNullableString(null),
                       General.GetNullableString(null),
                       General.GetNullableString(null),
                       General.GetNullableString(null),
                       General.GetNullableString(null),
                       General.GetNullableDateTime(null),
                       General.GetNullableDateTime(null),
                       General.GetNullableString(null),
                       General.GetNullableInteger(null),
                       General.GetNullableInteger(null),
                       Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                       gvCrew.PageSize,
                       ref iRowCount,
                       ref iTotalPageCount,
                       General.GetNullableString(null),
                       General.GetNullableInteger(null),
                       General.GetNullableInteger(null),
                       General.GetNullableString(null),
                       null,
                       null);
        }
        else
        {
            ds = PhoenixCrewAvailabilityReport.CrewAvailabilityReport(
                       General.GetNullableString(nvc.Get("ucRank")),
                       General.GetNullableString(nvc.Get("ucNationality")),
                       General.GetNullableString(nvc.Get("ucZone")),
                       General.GetNullableString(nvc.Get("ucPool")),
                       General.GetNullableString(nvc.Get("ucName")),
                       General.GetNullableDateTime(nvc.Get("ucDate")),
                       General.GetNullableDateTime(nvc.Get("ucDate1")),
                       General.GetNullableString(nvc.Get("ucPrincipal")),
                       General.GetNullableInteger(nvc.Get("newappln")),
                       General.GetNullableInteger(nvc.Get("inactive")),
                       Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                       gvCrew.PageSize,
                       ref iRowCount,
                       ref iTotalPageCount,
                       General.GetNullableString(nvc.Get("vesseltype")),
                       General.GetNullableInteger(nvc.Get("includepastexp")),
                       General.GetNullableInteger(nvc.Get("pvtype")),
                       General.GetNullableString(nvc.Get("ucStatus")),
                       General.GetNullableInteger(nvc.Get("chkFollowup")),
                       General.GetNullableInteger(nvc.Get("ucSeafarerRequirement")));
        }



        General.SetPrintOptions("gvCrew", "Crew Availability Report", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;
        
        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void MenuRecStatTabs_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FORMAT1"))
            {
                Response.Redirect("../Crew/CrewReportAvailabilityReportFormat2.aspx", true);

            }
            else if (CommandName.ToUpper().Equals("FORMAT2"))
            {
                Response.Redirect("../Crew/CrewReportAvailabilityReportFormat2.aspx", true);

            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }
    protected void gvCrew_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {

            if (e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
   
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
   
                RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
                LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
                RadLabel lblStatus = (RadLabel)e.Item.FindControl("lblStatus");
                if (!lblStatus.Text.Trim().Equals("NEW"))
                    lbr.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewPage','','CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
                else
                    lbr.Attributes.Add("onclick", "javascript:parent.Openpopup('CrewPage','','CrewNewApplicantPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");


                LinkButton imgSuitableCheck = (LinkButton)e.Item.FindControl("imgSuitableCheck");
                imgSuitableCheck.Attributes.Add("onclick", "parent.Openpopup('codehelp', '', 'CrewSuitabilityCheck.aspx?empid=" + empid.Text + "&personalmaster=true');return false;");

                RadLabel lbtn = (RadLabel)e.Item.FindControl("lblRemarks");
                UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipLeaveRemarks");
                lbtn.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
                lbtn.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            }
    }

    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
    }

    protected void gvCrew_SortCommand(object sender, GridSortCommandEventArgs e)
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
    public bool IsValidFilter(string fromdate, string todate)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }
        return (!ucError.IsError);
    }
    
    protected void gvCrew_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    public bool IsValidFilter(string fromdate, string todate, string format)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information";

        if (string.IsNullOrEmpty(fromdate))
        {
            ucError.ErrorMessage = "From Date is required";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (string.IsNullOrEmpty(todate))
        {
            ucError.ErrorMessage = "To Date is required";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'";
        }

        return (!ucError.IsError);
    }

}
