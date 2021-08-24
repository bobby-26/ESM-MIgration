using System;
using System.Data;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Common;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportDoctorVisit : PhoenixBasePage
{
    
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT",ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportDoctorVisit.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportDoctorVisit.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
                ViewState["TTO"] = PhoenixCommonRegisters.GetHardCode(1, 53, "TTO");
                ucDate1.Text = DateTime.Now.ToShortDateString();
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
                NameValueCollection nvc = Filter.CurrentSignOnFilter;
                if (nvc != null)
                {
                    ucManager.SelectedAddress = nvc.Get("ucManager");
                    ucDate.Text = nvc.Get("ucDate");
                    ucDate1.Text = nvc.Get("ucDate1");
                    ucZone.SelectedZoneValue = nvc.Get("ucZone");

                    if (nvc.Get("ucPrincipal").Equals("Dummy"))
                    {
                        ucPrincipal.SelectedAddress = "";
                    }
                    else
                    {
                        ucPrincipal.SelectedAddress = nvc.Get("ucPrincipal");
                    }
                    if (nvc.Get("ucVesselType").Equals("Dummy"))
                    {
                        ucVesselType.SelectedVesselTypeValue = "";
                    }
                    else
                    {
                        ucVesselType.SelectedVesselTypeValue = nvc.Get("ucVesselType");
                    }

                    if (nvc.Get("ucPool").Equals("Dummy"))
                    {
                        ucPool.SelectedPoolValue = "";
                    }
                    else
                    {
                        ucPool.SelectedPoolValue = nvc.Get("ucPool").ToString();
                    }
                    if (nvc.Get("ucRank").Equals("Dummy"))
                    {
                        ucRank.SelectedValue = "";
                    }
                    else
                    {
                        ucRank.SelectedRankValue = nvc.Get("ucRank").ToString();
                    }
                    if (nvc.Get("ucZone").Equals("Dummy"))
                    {
                        ucZone.SelectedZoneValue = "";
                    }
                    else
                    {
                        ucZone.SelectedZoneValue = nvc.Get("ucZone").ToString();
                    }
                }
            }
            
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void Rebind()
    {
        gvCrew.SelectedIndexes.Clear();
        gvCrew.EditIndexes.Clear();
        gvCrew.DataSource = null;
        gvCrew.Rebind();
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
                ViewState["SHOWREPORT"] = null;
                ucDate.Text = null;
                ucDate1.Text = DateTime.Now.ToShortDateString();
                ucZone.selectedlist = "";
                ucVesselType.SelectedVesseltype = "";
                ucPool.SelectedPoolValue = "";
                ucManager.SelectedAddress = "";
                ucPrincipal.SelectedAddress = "";
                ucRank.SelectedRankValue = "";
              
                
                Filter.CurrentSignOnFilter = null;

                Rebind();

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
                   
                    ViewState["PAGENUMBER"] = 1;
                    Rebind();

                }
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
        string[] alColumns = { "FLDROW", "FLDVESSELNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDDOCTORVISITDATE", "FLDREFERENCENO", "FLDILLNESSDESCRIPTION" };
        string[] alCaptions = { "Sl.No","Vessel", "File No","Name", "Rank","Doctor Visit Date", "Reference Number", "Remarks" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        if (ucDate.Text == null)
        {
            ds = PhoenixCrewReportDoctorVisit.CrewDoctorVisitorReport(
                   (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                   (ucPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPool.SelectedPool),
                   (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                   (ucRank.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucRank.selectedlist),
                   (ucManager.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucManager.SelectedAddress),
                   (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrincipal.SelectedAddress),
                   General.GetNullableDateTime(null),
                   General.GetNullableDateTime(null),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixCrewReportDoctorVisit.CrewDoctorVisitorReport(
                   (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                   (ucPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPool.SelectedPool),
                   (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                   (ucRank.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucRank.selectedlist),
                   (ucManager.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucManager.SelectedAddress),
                   (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrincipal.SelectedAddress),
                   General.GetNullableDateTime(ucDate.Text),
                   General.GetNullableDateTime(ucDate1.Text),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount);
        }
       
        string fromdates = ucDate.Text;
        string todatess = ucDate1.Text;
        Response.AddHeader("Content-Disposition", "attachment; filename=Doctor_Visit_Report.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td colspan='2'><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("</tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
       
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>Doctor Visit Report</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>From:" + fromdates + "  To:" + todatess + "</center></h5></td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td align='left'>");
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
        ViewState["SHOWREPORT"] = 1;
        ViewState["PAGENUMBER"] = 1;
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROW", "FLDVESSELNAME", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDDOCTORVISITDATE", "FLDREFERENCENO", "FLDILLNESSDESCRIPTION" };
        string[] alCaptions = { "Sl.No", "Vessel", "File No", "Name", "Rank", "Doctor Visit Date", "Reference Number", "Remarks" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        DataSet ds = new DataSet();
        if (ucDate.Text==null)
        {
            ds = PhoenixCrewReportDoctorVisit.CrewDoctorVisitorReport(
                   (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                   (ucPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPool.SelectedPool),
                   (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                   (ucRank.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucRank.selectedlist),
                   (ucManager.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucManager.SelectedAddress),
                   (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrincipal.SelectedAddress),
                   General.GetNullableDateTime(null),
                   General.GetNullableDateTime(null),
                   sortexpression, sortdirection,
                   Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                   gvCrew.PageSize,
                   ref iRowCount,
                   ref iTotalPageCount);
        }
        else
        {
            ds = PhoenixCrewReportDoctorVisit.CrewDoctorVisitorReport(
                   (ucZone.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucZone.selectedlist),
                   (ucPool.SelectedPool.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPool.SelectedPool),
                   (ucVesselType.SelectedVesseltype.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype),
                   (ucRank.selectedlist.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucRank.selectedlist),
                   (ucManager.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableInteger(ucManager.SelectedAddress),
                   (ucPrincipal.SelectedAddress.ToUpper()) == "DUMMY" ? null : General.GetNullableString(ucPrincipal.SelectedAddress),
                   General.GetNullableDateTime(ucDate.Text),
                   General.GetNullableDateTime(ucDate1.Text),
                   sortexpression, sortdirection,
                   Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                   gvCrew.PageSize,
                   ref iRowCount,
                   ref iTotalPageCount);
        }


        General.SetPrintOptions("gvCrew", "Doctor Visit Report", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lbl = (RadLabel)e.Item.FindControl("lblPNIMedicalCaseId");
            HtmlImage img = (HtmlImage)e.Item.FindControl("imgRemarks");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("ucToolTipAddress");
            img.Attributes.Add("onmouseover", "showTooltip(ev, '" + uct.ToolTip + "', 'visible');");
            img.Attributes.Add("onmouseout", "showTooltip(ev, '" + uct.ToolTip + "', 'hidden');");
            img.Attributes.Add("onclick", "parent.Openpopup('MoreInfo','','../Crew/CrewMoreInfo.aspx?id=" + lbl.Text + "&type=MEDICALCASE','xlarge')");
            RadLabel lblR = (RadLabel)e.Item.FindControl("lblRemarks");
            if (string.IsNullOrEmpty(lblR.Text.Trim()))
            {
                uct.Visible = false;
                img.Src = Session["images"] + "/no-remarks.png";
            }
        }
       
    }

    public bool IsValidFilter( string fromdate, string todate)
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
    protected void gvCrew_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrew.CurrentPageIndex + 1;

        ShowReport();
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


}
