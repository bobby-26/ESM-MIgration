using System;
using System.Collections.Specialized;
using System.Data;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Dashboard;
using System.Web.UI.DataVisualization.Charting;

public partial class DashboardChart : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                ViewState["MEASURE"] = "";
                ViewState["TYPE"] = "";

                if (Request.QueryString["type"] != null)
                    ViewState["TYPE"] = Request.QueryString["type"].ToString();

                if (Request.QueryString["measure"] != null)
                    ViewState["MEASURE"] = Request.QueryString["measure"].ToString();

                if (ViewState["TYPE"].ToString() == "1")
                    DisplayVesselMeasureChart();
                else
                    DisplayRankMeasureChart();

                NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
                lblMeasureTitle.Text = (nvc != null ? (nvc.Get("MeasureName") != null ? General.GetNullableString(nvc.Get("MeasureName")) : null) : "");
            } 
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DisplayVesselMeasureChart()
    {
        NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
        Guid MeasureId = new Guid();
        DataSet ds = PhoenixDashboardTechnical.DashboardMeasureChart(ViewState["MEASURE"].ToString()
            , new Guid(nvc != null ? (nvc.Get("MeasureId") != null ? (nvc.Get("MeasureId").ToString() != "" ? nvc.Get("MeasureId").ToString() : MeasureId.ToString()) : MeasureId.ToString()) : MeasureId.ToString())
            , nvc != null ? (nvc.Get("VesselList") != null ? General.GetNullableString(nvc.Get("VesselList")) : null) : null);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            PhoenixCommonChart pcc = new PhoenixCommonChart(ChartMeasure, "");
            pcc.HideLegend();
            pcc.ChartType = SeriesChartType.Column;
            pcc.YSeries("", new YAxisColumn("FLDMEASURE", nvc.Get("MeasureName") != null ? nvc.Get("MeasureName").ToString() : ""));
            pcc.XSeries("", 1, "FLDVESSELNAME");
            pcc.Enable3D = false;
            pcc.Show(dt);
            pcc.LegendDocking = Docking.Bottom;

            if (ds.Tables[0].Rows.Count > 5)
                ChartMeasure.Width = 800 + ((ds.Tables[0].Rows.Count - 5) * 10);
        }
    }

    private void DisplayRankMeasureChart()
    {
        NameValueCollection nvc = Filter.CurrentDashboardLastSelection;
        Guid MeasureId = new Guid();
        DataSet ds = PhoenixDashboardCrew.DashboardMeasureChart(ViewState["MEASURE"].ToString()
            , new Guid(nvc != null ? (nvc.Get("MeasureId") != null ? (nvc.Get("MeasureId").ToString() != "" ? nvc.Get("MeasureId").ToString() : MeasureId.ToString()) : MeasureId.ToString()) : MeasureId.ToString())
            , nvc != null ? (nvc.Get("RankList") != null ? General.GetNullableString(nvc.Get("RankList")) : null) : null);
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];

            PhoenixCommonChart pcc = new PhoenixCommonChart(ChartMeasure, "");
            pcc.HideLegend();
            pcc.ChartType = SeriesChartType.Column;
            pcc.YSeries("", new YAxisColumn("FLDMEASURE", nvc.Get("MeasureName") != null ? nvc.Get("MeasureName").ToString() : ""));
            pcc.XSeries("", 1, "FLDRANKNAME");
            pcc.Enable3D = false;
            pcc.Show(dt);
            pcc.LegendDocking = Docking.Bottom;

            if (ds.Tables[0].Rows.Count > 5)
                ChartMeasure.Width = 800 + ((ds.Tables[0].Rows.Count - 5) * 10);
        }
    }
}
