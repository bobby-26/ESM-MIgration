using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.CrewReports;
using SouthNests.Phoenix.Framework;
using System.Collections.Specialized;
using Telerik.Web.UI;
using System.Web;

public partial class Crew_CrewReportShortHandManning : PhoenixBasePage
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
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportShortHandManning.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar1.AddFontAwesomeButton("javascript:CallPrint('gvCrew')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar1.AddFontAwesomeButton("../Crew/CrewReportShortHandManning.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "CLEAR");
            MenuShowExcel.AccessRights = this.ViewState;
            MenuShowExcel.MenuList = toolbar1.Show();
            
            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
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
                ucVesselType.SelectedVesseltype = "";
                ucManager.SelectedAddress = "";
                ucPrincipal.SelectedAddress = "";
                ucDate.Text = "";
                ucDate1.Text = DateTime.Now.ToShortDateString();
                rblManningScale.SelectedValue = "2";
                lstVessel.SelectedVessel = "";
                lstRank.selectedlist = "";
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
    protected void ReportsFilter_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SHOWREPORT"))
            {
                if (!IsValidFilter(ucDate.Text, ucDate1.Text, lstVessel.SelectedVessel))
                {

                    ucError.Visible = true;
                    return;
                  
                }
                else
                {
                    ShowReport();
                    gvCrew.Rebind();
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
        string[] alColumns = { "FLDROW", "FLDVESSELNAME", "FLDRANKNAME", "FLDSIGNOFFPORT", "FLDFROMDATE", "FLDSIGNONPORT", "FLDTODATE", "FLDDISPENSATIONAPPLIED" };
        string[] alCaptions = { "S.No.", "Vessel", "Rank", "Sign Off Port", "From Date", "Sign On Port", "To Date", "Dispn applied" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());
        ds = PhoenixCrewShortofManning.CrewShortofManning(
                       General.GetNullableDateTime(ucDate.Text),
                       General.GetNullableDateTime(ucDate1.Text),
                       ucVesselType.SelectedVesseltype,
                       General.GetNullableInteger(ucManager.SelectedAddress),
                       General.GetNullableInteger(ucPrincipal.SelectedAddress),
                       int.Parse(rblManningScale.SelectedValue),
                        lstVessel.SelectedVessel,
                       lstRank.selectedlist,
                       Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                       iRowCount,
                       ref iRowCount,
                       ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=CrewShortHandManning.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>" + HttpContext.Current.Session["companyname"] + "</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "'><h5><center>Crew Shorthand Manning</center></h5></td></tr>");
        Response.Write("<tr><td style='font-family:Arial; font-size:10px;' colspan='" + (alColumns.Length - 2).ToString() + "' align='left'>Date:" + date + "</td></tr>");
        Response.Write("<tr><td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
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
        ViewState["SHOWREPORT"] = 1;

        int iRowCount = 0;
        int iTotalPageCount = 0;

        string[] alColumns = { "FLDROW", "FLDVESSELNAME", "FLDRANKNAME", "FLDSIGNOFFPORT", "FLDFROMDATE", "FLDSIGNONPORT", "FLDTODATE", "FLDDISPENSATIONAPPLIED" };
        string[] alCaptions = { "S.No.", "Vessel", "Rank", "Sign Off Port", "From Date", "Sign On Port", "To Date", "Dispn applied" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        ds = PhoenixCrewShortofManning.CrewShortofManning(
                       General.GetNullableDateTime(ucDate.Text),
                       General.GetNullableDateTime(ucDate1.Text),
                       ucVesselType.SelectedVesseltype,
                       General.GetNullableInteger(ucManager.SelectedAddress),
                       General.GetNullableInteger(ucPrincipal.SelectedAddress),
                       int.Parse(rblManningScale.SelectedValue),
                       lstVessel.SelectedVessel,
                       lstRank.selectedlist,
                       Int32.Parse(ViewState["PAGENUMBER"].ToString()),
                       gvCrew.PageSize,
                       ref iRowCount,
                       ref iTotalPageCount);


        General.SetPrintOptions("gvCrew", "Crew Shorthand Manning", alCaptions, alColumns, ds);

        gvCrew.DataSource = ds;
        gvCrew.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
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
    protected void gvCrew_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            //RadLabel empid = (RadLabel)e.Item.FindControl("lblEmpNo");
            //LinkButton lbr = (LinkButton)e.Item.FindControl("lnkName");
            //lbr.Attributes.Add("onclick", "javascript:Openpopup('CrewPage','','CrewPersonalGeneral.aspx?empid=" + empid.Text + "'); return false;");

        }
    }
    public bool IsValidFilter(string fromdate, string todate, string vessellist)
    {
        DateTime resultdate;
        ucError.HeaderMessage = "Please provide the following required information.";

        if (General.GetNullableDateTime(fromdate).Equals(null))
        {
            ucError.ErrorMessage = "From Date is required.";
        }
        else if (DateTime.TryParse(fromdate, out resultdate) && DateTime.Compare(resultdate, DateTime.Now) > 0)
        {
            ucError.ErrorMessage = "From Date should be earlier than current date.";
        }

        if (General.GetNullableDateTime(todate).Equals(null))
        {
            ucError.ErrorMessage = "To Date is required.";
        }
        else if (!string.IsNullOrEmpty(fromdate)
            && DateTime.TryParse(todate, out resultdate) && DateTime.Compare(resultdate, DateTime.Parse(fromdate)) < 0)
        {
            ucError.ErrorMessage = "To Date should be later than 'From Date'.";
        }
        if (string.IsNullOrEmpty(vessellist) || vessellist == "Dummy")
            ucError.ErrorMessage = "Vessel  is required.";

        return (!ucError.IsError);
    }
}
