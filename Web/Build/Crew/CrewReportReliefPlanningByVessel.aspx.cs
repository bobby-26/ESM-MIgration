using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web;
public partial class Crew_CrewReportReliefPlanningByVessel : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Format2", "FORMAT2", ToolBarDirection.Right);
            toolbar.AddButton("Format1", "FORMAT1", ToolBarDirection.Right);

            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();
            MenuReportsFilter.SelectedMenuIndex = 1;

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportReliefPlanningByVessel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrewReliefPlanning')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportReliefPlanningByVessel.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();


            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar3 = new PhoenixToolbar();
            toolbar3.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);

            ShowReports.AccessRights = this.ViewState;
            ShowReports.MenuList = toolbar3.Show();

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                gvCrewReliefPlanning.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);

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
        gvCrewReliefPlanning.SelectedIndexes.Clear();
        gvCrewReliefPlanning.EditIndexes.Clear();
        gvCrewReliefPlanning.DataSource = null;
        gvCrewReliefPlanning.Rebind();

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
                ucPrincipal.SelectedAddress = "";
                ucVesselType.SelectedVesseltype = "";
                ucVessel.SelectedVessel = "";
                ucRank.SelectedRankValue = "";
                ucFromDate.Text = "";
                ucToDate.Text = "";
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

            if (CommandName.ToUpper().Equals("FORMAT1"))
            {
                Response.Redirect("../Crew/CrewReportReliefPlanningByVessel.aspx", true);
                MenuReportsFilter.SelectedMenuIndex = 0;
            }
            else
            {
                Response.Redirect("../Crew/CrewReportReliefPlanningByVessel2.aspx", true);
                MenuReportsFilter.SelectedMenuIndex = 1;
            }

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void MenuShowReport_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SHOWREPORT"))
        {
            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidDates(ucFromDate.Text, ucToDate.Text))
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
    }

    private bool IsValidDates(string fromdate, string todate)
    {
        DateTime resultdate;

        ucError.HeaderMessage = "Please provide the following information";

        if (General.GetNullableDateTime(fromdate) == null)
            ucError.ErrorMessage = "From date required";
        if (General.GetNullableDateTime(todate) == null)
            ucError.ErrorMessage = "To date required";
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(todate)) > 0)
            ucError.ErrorMessage = "To date should be later than or equal to From date";

        return (!ucError.IsError);
    }
    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDVESSELNAME", "FLDOFFSIGNERRANK", "FLDOFFSIGNERNAME", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDRELIEVERNAME", "FLDDOA", "FLDLEAVECOMPLETIONDATE", "FLDREMARKS" };
        string[] alCaptions = { "Vessel Name", "Rank", "Onboard", "Sign On Date", "Relief Date", "Reliever", "DOA", "EOL(Including activities)", "Remarks" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
        ucVessel.SelectedVessel = ucVessel.SelectedVessel.ToString().Contains("Dummy,") ? ucVessel.SelectedVessel.ToString().Replace("Dummy,", "") : ucVessel.SelectedVessel;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;

        ds = PhoenixCrewReportReliefPlanningFormat.ReliefPlanningReportFormat1(
                    (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                    (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                    (ucVessel.SelectedVessel.ToString()) == "Dummy" ? null : General.GetNullableString(ucVessel.SelectedVessel),
                    General.GetNullableString(ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                    General.GetNullableDateTime(ucFromDate.Text),
                    General.GetNullableDateTime(ucToDate.Text),
                    sortexpression, sortdirection,
                    Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                    iRowCount,
                    ref iRowCount,
                    ref iTotalPageCount,
                    General.GetNullableString(ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool),
                    General.GetNullableString(ucZone.SelectedZoneValue) == "," ? null : General.GetNullableString(ucZone.SelectedZoneValue));

        string strVesselName = "";

        Response.AddHeader("Content-Disposition", "attachment; filename=ReliefPlanning.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Relief Planning</center></h5></td></tr>");
        //Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>From:" + fromdates + "To:" + todatess + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' align='right'>Date:</td><td style='font-family:Arial; font-size:10px;'>" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 1; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            if (strVesselName != dr[alColumns[0]].ToString())
            {
                Response.Write("<tr>");
                Response.Write("<td colspan=8>");
                Response.Write(dr[alColumns[0]]);
                Response.Write("</td>");
                Response.Write("</tr>");
            }

            strVesselName = dr[alColumns[0]].ToString();

            Response.Write("<tr>");
            for (int i = 1; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;'>");
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
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = {  "FLDOFFSIGNERRANK", "FLDOFFSIGNERNAME", "FLDSIGNONDATE", "FLDRELIEFDUEDATE", "FLDRELIEVERNAME", "FLDDOA", "FLDLEAVECOMPLETIONDATE", "FLDREMARKS" };
        string[] alCaptions = {  "Rank", "Onboard", "Sign On Date", "Relief Date", "Reliever", "DOA", "EOL(Including activities)", "Remarks" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
        ucVessel.SelectedVessel = ucVessel.SelectedVessel.ToString().Contains("Dummy,") ? ucVessel.SelectedVessel.ToString().Replace("Dummy,", "") : ucVessel.SelectedVessel;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;

        ds = PhoenixCrewReportReliefPlanningFormat.ReliefPlanningReportFormat1(
                            (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                            (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                            (ucVessel.SelectedVessel.ToString()) == "Dummy" ? null : General.GetNullableString(ucVessel.SelectedVessel),
                            General.GetNullableString(ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                            General.GetNullableDateTime(ucFromDate.Text),
                            General.GetNullableDateTime(ucToDate.Text),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                            gvCrewReliefPlanning.PageSize,
                            ref iRowCount,
                            ref iTotalPageCount,
                            General.GetNullableString(ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool),
                            General.GetNullableString(ucZone.SelectedZoneValue) == "," ? null : General.GetNullableString(ucZone.SelectedZoneValue));

        General.SetPrintOptions("gvCrewReliefPlanning", "Relief Planning", alCaptions, alColumns, ds);

        gvCrewReliefPlanning.DataSource = ds;
        gvCrewReliefPlanning.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void gvCrewReliefPlanning_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel empid = (RadLabel)e.Item.FindControl("lblOffsignerID");
            LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            lbr.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

            RadLabel relieverid = (RadLabel)e.Item.FindControl("lblOnsignerID");
            RadLabel reliever = (RadLabel)e.Item.FindControl("lblRelieverName");
            LinkButton relivername = (LinkButton)e.Item.FindControl("lnkRelieverName");
            if (relieverid.Text.Equals(""))
            {
                reliever.Visible = true;
                relivername.Visible = false;
            }
            relivername.Attributes.Add("onclick", "javascript:openNewWindow('chml','','" + Session["sitepath"] + "/Crew/CrewPersonalGeneral.aspx?empid=" + relieverid.Text + "'); return false;");
        }

    }

    protected void gvCrewReliefPlanning_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvCrewReliefPlanning_ItemCommand(object sender, GridCommandEventArgs e)
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

    protected void gvCrewReliefPlanning_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewReliefPlanning.CurrentPageIndex + 1;

        ShowReport();
    }
    public class GridDecorator
    {
        public static void MergeRows(GridView gridView)
        {
            int flag = 1;

            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                string currentVesselId = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblVesselId")).Text;
                string previousVesselId = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblVesselId")).Text;

                if (currentVesselId != previousVesselId)
                {
                    GridViewRow rownew = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Insert);
                    //create Cell and it will be added to row
                    //int cellcnt = gridView.Columns.Count;
                    TableCell cell = null;
                    for (int i = 0; i <= 1; i++)
                    {
                        cell = new TableCell();
                        rownew.Cells.Add(cell);
                    }
                    rownew.Cells[0].Text = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblVesselName")).Text;
                    rownew.Cells[0].Attributes.Add("style", "text-align:left; font-weight:bold; border-right-color:#ffffff;");
                    rownew.Cells[0].ColumnSpan = 7;
                    //add the row to Gridview and AddAT(Index of where it will be appear
                    gridView.Controls[0].Controls.AddAt(row.RowIndex + 2, rownew);

                    flag = rowIndex;
                }
            }
            if (flag == 0)
            {
                GridViewRow rownew = new GridViewRow(-1, -1, DataControlRowType.DataRow, DataControlRowState.Insert);
                TableCell cell = null;
                for (int i = 0; i <= 1; i++)
                {
                    cell = new TableCell();
                    rownew.Cells.Add(cell);
                }
                rownew.Cells[0].Text = ((RadLabel)gridView.Rows[0].FindControl("lblVesselName")).Text;
                rownew.Cells[0].Attributes.Add("style", "text-align:left; font-weight:bold; border-right-color:#ffffff;");
                rownew.Cells[0].ColumnSpan = 7;
                gridView.Controls[0].Controls.AddAt(1, rownew);
            }
        }
    }


}
