using System;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Registers;
using System.Web.UI.DataVisualization.Charting;

public partial class CrewOffshoreDMRMonthlyReportChart : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();
            toolbarvoyagetap.AddButton("List", "MONTHLYREPORTLIST");
            toolbarvoyagetap.AddButton("Back", "MONTHLYREPORT");
            MenuReportTap.AccessRights = this.ViewState;
            MenuReportTap.MenuList = toolbarvoyagetap.Show();

            if (!IsPostBack)
            {
                if (Request.QueryString["year"] != null)
                    ViewState["YEAR"] = Request.QueryString["year"].ToString();
                else
                    ViewState["YEAR"] = DateTime.Now.Year;

                if (Request.QueryString["month"] != null)
                    ViewState["MONTH"] = Request.QueryString["month"].ToString();
                else
                    ViewState["MONTH"] = DateTime.Now.Month;

                if (Request.QueryString["taskid"] != null)
                    ViewState["TASKID"] = Request.QueryString["taskid"].ToString();
                else
                    ViewState["TASKID"] = "";

                if (Request.QueryString["taskname"] != null)
                    ViewState["TASKNAME"] = Request.QueryString["taskname"].ToString();
                else
                    ViewState["TASKNAME"] = "";
               
                 
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                else
                    ViewState["VESSELID"] = "0";

                if (Session["MONTHLYREPORTID"] != null)
                    EditMonthlyReport();
            }
            DisplayChart();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void ReportTapp_TabStripCommand(object sender, EventArgs e)
    {
        DataListCommandEventArgs dce = (DataListCommandEventArgs)e;
        if (dce.CommandName.ToUpper().Equals("MONTHLYREPORTLIST"))
        {
            Response.Redirect("CrewOffshoreDMRMonthlyReportList.aspx", false);
        }
        if (dce.CommandName.ToUpper().Equals("MONTHLYREPORT"))
        {
            Response.Redirect("CrewOffshoreDMRMonthlyReport.aspx", false);
        }
    }

    private void EditMonthlyReport()
    {
        DataSet ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportEdit(new Guid(Session["MONTHLYREPORTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
        }
    }

    private void DisplayChart()
    {
        if (ViewState["TASKID"].ToString() != "")
        {
            DataSet ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportFuelConsumptionRateList(int.Parse(ViewState["VESSELID"].ToString()), int.Parse(ViewState["MONTH"].ToString()), int.Parse(ViewState["YEAR"].ToString()), new Guid(ViewState["TASKID"].ToString()));
            DataTable dt = ds.Tables[0];

            PhoenixCommonChart pcc = new PhoenixCommonChart(ChartFuelConsRate, ViewState["TASKNAME"].ToString() + " Fuel Consumption Rate");
            pcc.ChartType = SeriesChartType.Column;
            pcc.YSeries("", new YAxisColumn("FLDFUELOILCONSUMPTIONRATE", "Litres / hour"));
            pcc.XSeries("", 1, "FLDREPORTDATE");
            pcc.Enable3D = false;
            pcc.Show(dt);
        }
    }
}
