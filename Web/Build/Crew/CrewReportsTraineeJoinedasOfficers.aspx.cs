using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportsTraineeJoinedasOfficers : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("Show Report", "SHOWREPORT", ToolBarDirection.Right);
            MenuReportsFilter.AccessRights = this.ViewState;
            MenuReportsFilter.MenuList = toolbar.Show();

            PhoenixToolbar toolbar1 = new PhoenixToolbar();
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsTraineeJoinedasOfficers.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportsTraineeJoinedasOfficers.aspx", "Clear Filter", "<i class=\"fa fa-ban fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["SHOWREPORT"] = null;
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
                ucVessel.SelectedVessel = "";
                ucBatch.SelectedList = "";
                ucPrincipal.SelectedAddress = "";
                ucVesselType.SelectedVesseltype = "";
                ucRank.SelectedRankValue = "";
                ucFromDate.Text = "";
                ucToDate.Text = DateTime.Now.ToShortDateString();
                ucSignOnFromDate.Text = "";
                ucSignOnToDate.Text = "";
                ucPool.SelectedPoolValue = "";
                chkactiveyn.Checked = false;
                ddlSelectFrom.SelectedIndex = 0;
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
                if (!IsValidFilter(ucFromDate.Text, ucToDate.Text))
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
        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDBATCH", "FLDDATEOFJOINING", "FLDFIRSTOFFICERJOINDATE", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE", "FLDACTIVE" };
        string[] alCaptions = { "S.No.", "File No", "Name", "Rank", "Batch", "1st Join Date", "Dt joined as Off", "Joined Vessel", "Signon Date", "Last Vessel", " SignOff Date", "Status" };

        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixCrewTraineeJoinedOfficers.CrewTraineeJoinedOfficers(
                       (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                       (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                        (ucBatch.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucBatch.SelectedList.ToString()),
                       (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                       (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                       General.GetNullableDateTime(ucFromDate.Text),
                       General.GetNullableDateTime(ucToDate.Text),
                       sortexpression, sortdirection,
                       Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                       iRowCount,
                       ref iRowCount,
                       ref iTotalPageCount,
                       General.GetNullableInteger(ddlSelectFrom.SelectedValue),
                       General.GetNullableDateTime(ucSignOnFromDate.Text),
                       General.GetNullableDateTime(ucSignOnToDate.Text),
                       General.GetNullableString(ucPool.SelectedPool),
                       chkactiveyn.Checked == true ? 1 : 0);

        Response.AddHeader("Content-Disposition", "attachment; filename=TraineeJoinedAsOfficers.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "'><h5><center>Trainee Joined As Officers</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length).ToString() + "' align='left'>Date:" + date + "</td></tr>");
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
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td style='font-family:Arial; font-size:10px;' align='left'>");
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

        string[] alColumns = { "FLDROW", "FLDFILENO", "FLDEMPLOYEENAME", "FLDRANKCODE", "FLDBATCH", "FLDDATEOFJOINING", "FLDFIRSTOFFICERJOINDATE", "FLDVESSELNAME", "FLDSIGNONDATE", "FLDLASTVESSELNAME", "FLDLASTSIGNOFFDATE", "FLDACTIVE" };
        string[] alCaptions = { "S.No.", "File No", "Name", "Rank", "Batch", "1st Join Date", "Dt joined as Off", "Joined Vessel", "Signon Date", "Last Vessel", " SignOff Date", "Status" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = PhoenixCrewTraineeJoinedOfficers.CrewTraineeJoinedOfficers(
                       (ucPrincipal.SelectedAddress.ToString()) == "Dummy" ? null : General.GetNullableString(ucPrincipal.SelectedAddress.ToString()),
                       (ucVesselType.SelectedVesseltype.ToString()) == "Dummy" ? null : General.GetNullableString(ucVesselType.SelectedVesseltype.ToString()),
                       (ucBatch.SelectedList.ToString()) == "Dummy" ? null : General.GetNullableString(ucBatch.SelectedList.ToString()),
                       (ucVessel.SelectedVessel.ToString()) == "," ? null : General.GetNullableString(ucVessel.SelectedVessel.ToString()),
                       (ucRank.selectedlist) == "," ? null : General.GetNullableString(ucRank.selectedlist),
                       General.GetNullableDateTime(ucFromDate.Text),
                       General.GetNullableDateTime(ucToDate.Text),
                       sortexpression, sortdirection,
                       Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                       gvCrew.PageSize,
                       ref iRowCount,
                       ref iTotalPageCount,
                       General.GetNullableInteger(ddlSelectFrom.SelectedValue),
                       General.GetNullableDateTime(ucSignOnFromDate.Text),
                       General.GetNullableDateTime(ucSignOnToDate.Text),
                       General.GetNullableString(ucPool.SelectedPool),
                       chkactiveyn.Checked == true ? 1 : 0);


        General.SetPrintOptions("gvCrew", "Trainee Joined As Officers", alCaptions, alColumns, ds);

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

        if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date";
        }

        if (!string.IsNullOrEmpty(fromdate)
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
