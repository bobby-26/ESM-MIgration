using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web;

public partial class CrewReportsStatusofCrewChange : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Crew/CrewReportsStatusofCrewChange.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Crew/CrewReportsStatusofCrewChange.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ucToDate.Text = DateTime.Now.ToShortDateString();
                gvCrew.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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

    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucFromDate.Text, ucToDate.Text))
                {
                    ucError.Visible = true;
                    return;
                }
                Rebind();
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
                ucFromDate.Text = "";
                ucRank.selectedlist = "";
                ucVessel.SelectedVessel = "";
                ucToDate.Text = DateTime.Now.ToShortDateString();
                ucPrinicpal.SelectedAddress = "";
                rblPrincipalVesselType.Items[0].Selected = false;
                rblPrincipalVesselType.Items[1].Selected = false;
                ddlVesselType.SelectedVesseltype = "";
                chkIncludepastexp.Checked = false;
                ucPrinicpal.Enabled = false;
                ddlVesselType.Enabled = false;
                chkIncludepastexp.Enabled = false;
                Rebind();
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
        string[] alColumns = { "FLDROW","FLDEMPLOYEENAME","FLDRANKCODE", "FLDVESSELNAME",   "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDSERVICEMONTHS",
                                 "FLDREASON", "FLDSEAPORTNAME", "FLDTOTALEXPENSEOFFSIGNER", "FLDTOTALEXPENSEONSIGNER", "FLDTOBERECOVERED" };
        string[] alCaptions = { "S.No.", "Name","Rank", "Vessel",  "Date of Sign On", "Date of Sign Off", "Service Period", "Reason of Disembarkation",
                                  "Changed Location", "Total expenses for Off-Signer", "Total Expenses for On-Signer", "To be recovered to Owner" };
        string[] FilterColumns = { "FLDSELECTEDFROMDATE", "FLDSELECTEDTODATE", "FLDSELECTEDPRICIPAL", "FLDSELECTEDVESSELTYPE", "FLDSELECTEDRANK", "FLDSELECTEDVESSEL" };
        string[] FilterCaptions = { "From Date", "To Date", "Principal", "Vessel Type", "Rank", "Vessel" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixCrewChangeStatusReport.CrewChangeStatusReport(General.GetNullableString(ucVessel.SelectedVessel),
                            General.GetNullableString(ucRank.selectedlist),
                            General.GetNullableDateTime(ucFromDate.Text),
                            General.GetNullableDateTime(ucToDate.Text),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            iRowCount,
                            ref iRowCount,
                            ref iTotalPageCount,
                            General.GetNullableInteger(ucPrinicpal.SelectedAddress),
                            General.GetNullableString(ddlVesselType.SelectedVesseltype),
                            General.GetNullableInteger(chkIncludepastexp.Checked == true ? "1" : "0"),
                            General.GetNullableInteger(rblPrincipalVesselType.Items[0].Selected ? "1" : rblPrincipalVesselType.Items[1].Selected ? "2" : null));

        Response.AddHeader("Content-Disposition", "attachment; filename=StatusOfCrewChange.xls");
        Response.ContentType = "application/x-excel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>Status of Crew Change</center></h5></td></tr>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        General.ShowFilterCriteriaInExcel(ds, FilterCaptions, FilterColumns);
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

        string[] alColumns = { "FLDROW","FLDEMPLOYEENAME","FLDRANKCODE", "FLDVESSELNAME",   "FLDSIGNONDATE", "FLDSIGNOFFDATE", "FLDSERVICEMONTHS",
                                 "FLDREASON", "FLDSEAPORTNAME", "FLDTOTALEXPENSEOFFSIGNER", "FLDTOTALEXPENSEONSIGNER", "FLDTOBERECOVERED" };
        string[] alCaptions = { "S.No.", "Name","Rank", "Vessel",  "Date of Sign On", "Date of Sign Off", "Service Period", "Reason of Disembarkation",
                                  "Changed Location", "Total expenses for Off-Signer", "Total Expenses for On-Signer", "To be recovered to Owner" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixCrewChangeStatusReport.CrewChangeStatusReport(General.GetNullableString(ucVessel.SelectedVessel),
                            General.GetNullableString(ucRank.selectedlist),
                            General.GetNullableDateTime(ucFromDate.Text),
                            General.GetNullableDateTime(ucToDate.Text),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            gvCrew.PageSize,
                            ref iRowCount,
                            ref iTotalPageCount,
                            General.GetNullableInteger(ucPrinicpal.SelectedAddress),
                            General.GetNullableString(ddlVesselType.SelectedVesseltype),
                            General.GetNullableInteger(chkIncludepastexp.Checked == true ? "1" : "0"),
                            General.GetNullableInteger(rblPrincipalVesselType.Items[0].Selected ? "1" : rblPrincipalVesselType.Items[1].Selected ? "2" : null));

        General.SetPrintOptions("gvCrew", "Status of Crew Change", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");
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
    protected void PrincipalVesselTypeClick(object sender, EventArgs e)
    {
        if (rblPrincipalVesselType.SelectedValue == "1")
        {
            ddlVesselType.Enabled = false;
            chkIncludepastexp.Enabled = false;
            chkIncludepastexp.Checked = false;
            ddlVesselType.SelectedVesseltype = "";
            ucPrinicpal.Enabled = true;
            ucPrinicpal.SelectedAddress = "";
        }
        else if (rblPrincipalVesselType.SelectedValue == "2")
        {
            ddlVesselType.Enabled = true;
            chkIncludepastexp.Enabled = true;
            chkIncludepastexp.Checked = false;
            ddlVesselType.SelectedVesseltype = "";
            ucPrinicpal.Enabled = false;
            ucPrinicpal.SelectedAddress = "";
        }
    }
}
