using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web;
public partial class CrewReportReliefPlanningByVessel2 : PhoenixBasePage
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
            MenuReportsFilter.SelectedMenuIndex = 0;


            PhoenixToolbar toolbar2 = new PhoenixToolbar();
            toolbar2.AddFontAwesomeButton("../Crew/CrewReportReliefPlanningByVessel2.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel1");
            toolbar2.AddFontAwesomeButton("javascript:CallPrint('gvCrewReliefPlanningF2')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT1");
            toolbar2.AddFontAwesomeButton("../Crew/CrewReportReliefPlanningByVessel2.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel1.AccessRights = this.ViewState;
            MenuShowExcel1.MenuList = toolbar2.Show();

            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar3 = new PhoenixToolbar();
            toolbar3.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);

            ShowReports.AccessRights = this.ViewState;
            ShowReports.MenuList = toolbar3.Show();

            if (!IsPostBack)
            {


                ViewState["PAGENUMBER2"] = 1;
                ViewState["SORTEXPRESSION2"] = null;
                ViewState["SORTDIRECTION2"] = null;

                gvCrewReliefPlanningF2.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
        gvCrewReliefPlanningF2.SelectedIndexes.Clear();
        gvCrewReliefPlanningF2.EditIndexes.Clear();
        gvCrewReliefPlanningF2.DataSource = null;
        gvCrewReliefPlanningF2.Rebind();
    }

    

    protected void CrewShowExcel1_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL1"))
            {
                ShowExcelF2();
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


    protected void ShowExcelF2()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string date = DateTime.Now.ToShortDateString();

        DataSet ds = new DataSet();
        string[] alColumns = { "FLDOFFSIGNERNAME", "FLDOFFSIGNERRANK", "FLDVESSELNAME", "FLDOFFSIGNERTOURCOUNT", "FLDRELIEFDUEDATE", "FLDRELIEVERNAME", "FLDDOA", "FLDLEAVECOMPLETIONDATE", "FLDRANKDECIMALEXPERIENCE", "FLDPRPLDECIMALEXPERIENCE", "FLDRELIVERTOURCOUNT" };
        string[] alCaptions = { "Name", "Rank", "Vessel", "Tour", "Relief Date", "Reliever", "DOA", "COL", "Time In Rank", "Time In BP", "Tour" };

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

        ds = PhoenixCrewReportReliefPlanningFormat.ReliefPlanningReportFormat2(
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
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td style='font-family:Arial; font-size:10px;' width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");

        string strVesselName = "";
        string strRank = "";
        string strName = "";
        string strtbl;

        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            strtbl = "<tr>";

            for (int i = 0; i < alColumns.Length; i++)
            {
                if (i == 0)
                {
                    if (strVesselName == dr[alColumns[0]].ToString())
                    {
                        strtbl += "<td style='font-family:Arial; font-size:10px;'>";
                        strtbl += "</td>";
                    }
                    else
                    {
                        strtbl += "<td style='font-family:Arial; font-size:10px;'>";
                        strtbl += dr[alColumns[0]];
                        strtbl += "</td>";
                    }
                }
                else if (i == 1 || i == 2 || i == 3 || i == 6 || i == 7)
                {
                    if (strRank == dr[alColumns[1]].ToString() && strName == dr[alColumns[2]].ToString())
                    {
                        strtbl += "<td style='font-family:Arial; font-size:10px;'>";
                        strtbl += "</td>";
                    }
                    else
                    {
                        strtbl += "<td style='font-family:Arial; font-size:10px;'>";
                        strtbl += dr[alColumns[i]];
                        strtbl += "</td>";
                    }
                }
                else
                {
                    strtbl += "<td style='font-family:Arial; font-size:10px;'>";
                    strtbl += dr[alColumns[i]];
                    strtbl += "</td>";
                }
            }
            strtbl += "</tr>";

            strVesselName = dr[alColumns[0]].ToString();
            strRank = dr[alColumns[1]].ToString();
            strName = dr[alColumns[2]].ToString();

            Response.Write(strtbl);
        }

        Response.Write("</TABLE>");
        Response.End();
    }








    private void ShowReportFormat2()
    {
        DataSet ds = new DataSet();
        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDOFFSIGNERNAME", "FLDOFFSIGNERRANK", "FLDVESSELNAME", "FLDOFFSIGNERTOURCOUNT", "FLDRELIEFDUEDATE", "FLDRELIEVERNAME", "FLDDOA", "FLDLEAVECOMPLETIONDATE", "FLDRANKDECIMALEXPERIENCE", "FLDPRPLDECIMALEXPERIENCE", "FLDRELIVERTOURCOUNT" };
        string[] alCaptions = { "Name", "Rank", "Vessel", "Tour", "Relief Date", "Reliever", "DOA", "COL", "Time In Rank", "Time In BP", "Tour" };

        string sortexpression = (ViewState["SORTEXPRESSION2"] == null) ? null : (ViewState["SORTEXPRESSION2"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION2"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION2"].ToString());

        ucVesselType.SelectedVesseltype = ucVesselType.SelectedVesseltype.ToString().Contains("Dummy,") ? ucVesselType.SelectedVesseltype.ToString().Replace("Dummy,", "") : ucVesselType.SelectedVesseltype;
        ucPool.SelectedPool = ucPool.SelectedPool.ToString().Contains("Dummy,") ? ucPool.SelectedPool.ToString().Replace("Dummy,", "") : ucPool.SelectedPool;
        ucRank.selectedlist = ucRank.selectedlist.ToString().Contains("Dummy,") ? ucRank.selectedlist.ToString().Replace("Dummy,", "") : ucRank.selectedlist;
        ucVessel.SelectedVessel = ucVessel.SelectedVessel.ToString().Contains("Dummy,") ? ucVessel.SelectedVessel.ToString().Replace("Dummy,", "") : ucVessel.SelectedVessel;
        ucZone.selectedlist = ucZone.selectedlist.ToString().Contains("Dummy,") ? ucZone.selectedlist.ToString().Replace("Dummy,", "") : ucZone.selectedlist;

        ds = PhoenixCrewReportReliefPlanningFormat.ReliefPlanningReportFormat2(
                            (ucPrincipal.SelectedAddress.ToString()) == "" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                            (ucVesselType.SelectedVesseltype.ToString()) == "" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                            (ucVessel.SelectedVessel.ToString()) == "" ? null : General.GetNullableString(ucVessel.SelectedVessel),
                            General.GetNullableString(ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                            General.GetNullableDateTime(ucFromDate.Text),
                            General.GetNullableDateTime(ucToDate.Text),
                            sortexpression, sortdirection,
                            Int32.Parse(ViewState["PAGENUMBER2"].ToString()),
                            gvCrewReliefPlanningF2.PageSize,
                            ref iRowCount,
                            ref iTotalPageCount,
                            General.GetNullableString(ucPool.SelectedPool) == "," ? null : General.GetNullableString(ucPool.SelectedPool),
                            General.GetNullableString(ucZone.SelectedZoneValue) == "," ? null : General.GetNullableString(ucZone.SelectedZoneValue));

        General.SetPrintOptions("gvCrewReliefPlanningF2", "Relief Planning Format 2", alCaptions, alColumns, ds);

        gvCrewReliefPlanningF2.DataSource = ds;
        gvCrewReliefPlanningF2.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT2"] = iRowCount;
        ViewState["TOTALPAGECOUNT2"] = iTotalPageCount;
    }

    protected void gvCrewReliefPlanningF2_ItemDataBound(object sender, GridItemEventArgs e)
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
    protected void gvCrewReliefPlanningF2_SortCommand(object sender, GridSortCommandEventArgs e)
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

    protected void gvCrewReliefPlanningF2_ItemCommand(object sender, GridCommandEventArgs e)
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
    protected void gvCrewReliefPlanningF2_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvCrewReliefPlanningF2.CurrentPageIndex + 1;

        ShowReportFormat2();
    }
    public class GridDecorator2
    {
        public static void MergeRows2(GridView gridView)
        {
            //int newrow = gridView.Rows.Count - 2; 

            for (int rowIndex = gridView.Rows.Count - 2; rowIndex >= 0; rowIndex--)
            {
                GridViewRow row = gridView.Rows[rowIndex];
                GridViewRow previousRow = gridView.Rows[rowIndex + 1];

                string CurrentVesselId = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblVesselId")).Text;
                string PreviousVesselId = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblVesselId")).Text;

                string CurrentRankId = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblRankId")).Text;
                string PreviousRankId = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblRankId")).Text;

                string CurrentOffsignerID = ((RadLabel)gridView.Rows[rowIndex].FindControl("lblOffsignerID")).Text;
                string PreviousOffsignerID = ((RadLabel)gridView.Rows[rowIndex + 1].FindControl("lblOffsignerID")).Text;


                if (CurrentVesselId == PreviousVesselId)
                {
                    //Vessel
                    row.Cells[0].RowSpan = previousRow.Cells[0].RowSpan < 2 ? 2 :
                                           previousRow.Cells[0].RowSpan + 1;

                    previousRow.Cells[0].Visible = false;
                }

                if (CurrentRankId == PreviousRankId && CurrentOffsignerID == PreviousOffsignerID)
                {
                    //Rank
                    row.Cells[1].RowSpan = previousRow.Cells[1].RowSpan < 2 ? 2 :
                                           previousRow.Cells[1].RowSpan + 1;

                    previousRow.Cells[1].Visible = false;

                    //Name
                    row.Cells[2].RowSpan = previousRow.Cells[2].RowSpan < 2 ? 2 :
                                          previousRow.Cells[2].RowSpan + 1;

                    previousRow.Cells[2].Visible = false;

                    //Tour
                    row.Cells[3].RowSpan = previousRow.Cells[3].RowSpan < 2 ? 2 :
                                          previousRow.Cells[3].RowSpan + 1;

                    previousRow.Cells[3].Visible = false;

                    //DOA
                    row.Cells[6].RowSpan = previousRow.Cells[6].RowSpan < 2 ? 2 :
                                          previousRow.Cells[6].RowSpan + 1;

                    previousRow.Cells[6].Visible = false;

                    //Tour
                    row.Cells[7].RowSpan = previousRow.Cells[7].RowSpan < 2 ? 2 :
                                          previousRow.Cells[7].RowSpan + 1;

                    previousRow.Cells[7].Visible = false;
                }
            }
        }
    }
}
