using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using System.Web.UI.DataVisualization.Charting;
using SouthNests.Phoenix.Common;
using System.Configuration;
using System.Drawing;
using iTextSharp.text.pdf;
using System.IO;
using System.Web;
using iTextSharp.text;
using Telerik.Web.UI;

public partial class VesselPositionChart : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarvoyagelist = new PhoenixToolbar();
            toolbarvoyagelist.AddFontAwesomeButton("../VesselPosition/VesselPositionChart.aspx", "Search", "<i class=\"fas fa-search\"></i>", "SEARCH");
            toolbarvoyagelist.AddFontAwesomeButton("../VesselPosition/VesselPositionChart.aspx", "Clear Filter", "<i class=\"fas fa-eraser\"></i>", "RESET");

            MenuVoyageList.AccessRights = this.ViewState;
            MenuVoyageList.MenuList = toolbarvoyagelist.Show();

            if (!IsPostBack)
            {
               // cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden;");
                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
                {
                    UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    if (PhoenixSecurityContext.CurrentSecurityContext.UserType.ToUpper() != "OWNER")
                        UcVessel.Enabled = false;
                } 
            btnslip.Visible=false;
            btnconsanalysis.Visible=false;

            btnMainEngineanlysis.Visible=false;
            btnScavepressureanalysis.Visible=false;
            btnspeedanalysis.Visible = false;
            }
            btnslip.Visible = false;
            btnconsanalysis.Visible = false;

            btnMainEngineanlysis.Visible = false;
            btnScavepressureanalysis.Visible = false;
            btnspeedanalysis.Visible = false;

            RadScriptManager.GetCurrent(this).RegisterPostBackControl(btnslip);
            RadScriptManager.GetCurrent(this).RegisterPostBackControl(btnconsanalysis);
            RadScriptManager.GetCurrent(this).RegisterPostBackControl(btnMainEngineanlysis);
            RadScriptManager.GetCurrent(this).RegisterPostBackControl(btnScavepressureanalysis);
            RadScriptManager.GetCurrent(this).RegisterPostBackControl(btnspeedanalysis);
            RadScriptManager.GetCurrent(this).RegisterPostBackControl(btnPowerVSMEConsumption);
            RadScriptManager.GetCurrent(this).RegisterPostBackControl(btnSludgeTank);
            RadScriptManager.GetCurrent(this).RegisterPostBackControl(btnFreshWaterProd);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

   
    protected void VoyageList_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("SEARCH"))
        {
            BindData();
        }

        if (CommandName.ToUpper().Equals("RESET"))
        {
            ClearFilter();
            BindData();
        }
    }
    private void BindData()
    {
        try
        {
        int? vesselid = null;

        if (PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0)
            vesselid = PhoenixSecurityContext.CurrentSecurityContext.VesselID;
        else
            vesselid = General.GetNullableInteger(UcVessel.SelectedVessel == "Dummy" ? "" : UcVessel.SelectedValue.ToString());
        if (vesselid == null)
        {
            ucError.ErrorMessage = "Vessel is required.";
            ucError.Visible = true;
            return;
        }
        DataTable dt = new DataTable();
        DataSet ds = PhoenixVesselPositionNoonReport.GetNoonReportChartData(General.GetNullableInteger(UcVessel.SelectedValue.ToString()),
            General.GetNullableDateTime( FromDate.Text),
            General.GetNullableDateTime( ToDate.Text),
             General.GetNullableString(ddlreporttype.SelectedValue.ToString().Trim()),
             General.GetNullableString(ddlVesselStatus.SelectedValue.ToString().Trim()));
        dt = ds.Tables[0];
            // SLIP
            if (dt.Rows.Count > 0)
            {
                int interval = 1;
                if (ddlreporttype.SelectedValue.ToString().ToUpper().Equals("WEEKLY") || ddlreporttype.SelectedValue.ToString().ToUpper().Equals("MONTHLY"))
                    interval = 1;
                if (ddlreporttype.SelectedValue.ToString().ToUpper().Equals("QUARTELY"))
                    interval = 3;
                if (ddlreporttype.SelectedValue.ToString().ToUpper().Equals("HALF YEARLY"))
                    interval = 6;


                PhoenixCommonChart pcc = new PhoenixCommonChart(ChartSlip, UcVessel.SelectedVesselName + "\nMain Engine RPM Analysis");
                pcc.ChartType = SeriesChartType.Line;
                pcc.YSeries("RPM", new YAxisColumn("FLDMERPM", "RPM"));
                pcc.XSeries("Date", interval, "FLDDATE");
                pcc.Enable3D = false;
                pcc.Show(dt);
                pcc.LegendDocking = Docking.Bottom;


                PhoenixCommonChart ConsAnalysis = new PhoenixCommonChart(ConsumtionAnalysis, UcVessel.SelectedVesselName + "\nFO/DO/FW Consumption Analysis");
                ConsAnalysis.ChartType = SeriesChartType.Line;
                ConsAnalysis.YSeries("MT", new YAxisColumn("FLDHFOOILCONSUMPTIONQTY", "HFO"), new YAxisColumn("FLDMDOOILCONSUMPTIONQTY", "MDO"), new YAxisColumn("FLDDOMILCONSUMPTIONQTY", "FW"));
                ConsAnalysis.XSeries("Date", interval, "FLDDATE");
                ConsAnalysis.Enable3D = false;
                ConsAnalysis.Show(dt);
                ConsAnalysis.LegendDocking = Docking.Bottom;

                PhoenixCommonChart MainEngineTemp = new PhoenixCommonChart(MainEngileTempAnalysis, UcVessel.SelectedVesselName + "\nMain Engine Exh. Temp Analysis");
                MainEngineTemp.ChartType = SeriesChartType.Line;
                MainEngineTemp.YSeries("Temperature", new YAxisColumn("FLDXMAXEXHTEMP", "Xmaxext1"), new YAxisColumn("FLDMAXEXHTEMP", "Xmaxext1"), new YAxisColumn("FLDXMINEXHTEMP", "Xminext1"));
                MainEngineTemp.XSeries("Date", interval, "FLDDATE");
                MainEngineTemp.Enable3D = false;
                MainEngineTemp.Show(dt);
                MainEngineTemp.LegendDocking = Docking.Bottom;

                PhoenixCommonChart MeScaveAnalysis = new PhoenixCommonChart(MainEngineScavePressureAnalysys, UcVessel.SelectedVesselName + "\nMain Engine Scav Pressure Analysis");
                MeScaveAnalysis.ChartType = SeriesChartType.Line;
                MeScaveAnalysis.YSeries("Scav Pressure", new YAxisColumn("FLDSCAVAIRPRESS", "Scav Pressure"));
                MeScaveAnalysis.XSeries("Date", interval, "FLDDATE");
                MeScaveAnalysis.Enable3D = false;
                MeScaveAnalysis.Show(dt);
                MeScaveAnalysis.LegendDocking = Docking.Bottom;


                PhoenixCommonChart SpeedAnalyse = new PhoenixCommonChart(SpeedAnalysis, UcVessel.SelectedVesselName + "\nSpeed Analysis");
                SpeedAnalyse.ChartType = SeriesChartType.Line;
                SpeedAnalyse.YSeries("Speed", new YAxisColumn("FLDOBSSPEED", "XSpeed"), new YAxisColumn("FLDEMLOGSPEED", "@LogSpeed"), new YAxisColumn("FLDVOYAGEORDERSPEED", "XcpSpeed"));
                SpeedAnalyse.XSeries("Date", interval, "FLDDATE");
                SpeedAnalyse.Enable3D = false;
                SpeedAnalyse.Show(dt);
                SpeedAnalyse.LegendDocking = Docking.Bottom;

                PhoenixCommonChart PowervsMEConsumpiton = new PhoenixCommonChart(PowerVSMEConsumption, UcVessel.SelectedVesselName + "\nPower vs CLO Cons/Day");
                PowervsMEConsumpiton.ChartType = SeriesChartType.Line;
                PowervsMEConsumpiton.YSeriesWithSecondary("Power/Cons", new YAxisColumnWithSecondary("FLDPOWER", "Power", "0"), new YAxisColumnWithSecondary("FLDMECYLINDEROILATY", "CLO Cons/Day", "1"));
                PowervsMEConsumpiton.XSeries("Date", interval, "FLDDATE");
                PowervsMEConsumpiton.Enable3D = false;
                PowervsMEConsumpiton.Show(dt);
                PowervsMEConsumpiton.LegendDocking = Docking.Bottom;

                PhoenixCommonChart SludgeTankanylysis = new PhoenixCommonChart(SludgeTank, UcVessel.SelectedVesselName + "\nSludge Volume Analysis (cu.m)");
                SludgeTankanylysis.ChartType = SeriesChartType.Line;
                SludgeTankanylysis.YSeries("Sludge Volume", new YAxisColumn("FLDSLUDGETANKROB", "Sludge Volume"));
                SludgeTankanylysis.XSeries("Date", interval, "FLDDATE");
                SludgeTankanylysis.Enable3D = false;
                SludgeTankanylysis.Show(dt);
                SludgeTankanylysis.LegendDocking = Docking.Bottom;

                PhoenixCommonChart FreshWaterProdduced = new PhoenixCommonChart(FreshWaterProd, UcVessel.SelectedVesselName + "\nFresh Water Produced");
                FreshWaterProdduced.ChartType = SeriesChartType.Line;
                FreshWaterProdduced.YSeries("Fresh Water(MT)", new YAxisColumn("FLDMFWPRODUCED", "Fresh Water"));
                FreshWaterProdduced.XSeries("Date", interval, "FLDDATE");
                FreshWaterProdduced.Enable3D = false;
                FreshWaterProdduced.Show(dt);
                FreshWaterProdduced.LegendDocking = Docking.Bottom;

                btnslip.Visible = true;
                btnconsanalysis.Visible = true;
                btnMainEngineanlysis.Visible = true;
                btnScavepressureanalysis.Visible = true;
                btnspeedanalysis.Visible = true;
                btnSludgeTank.Visible = true;
                btnFreshWaterProd.Visible = true;
            }
            else
            {
                btnslip.Visible = false;
                btnconsanalysis.Visible = false;
                btnMainEngineanlysis.Visible = false;
                btnScavepressureanalysis.Visible = false;
                btnspeedanalysis.Visible = false;
                btnSludgeTank.Visible = false;
                btnFreshWaterProd.Visible = false;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void ClearFilter()
    {
        UcVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID != 0 ? PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString() : "";
        FromDate.Text = "";
        ToDate.Text = "";
        ddlreporttype.SelectedIndex = 0;
        ddlVesselStatus.SelectedIndex = 0;

        btnslip.Visible = false;
        btnconsanalysis.Visible = false;

        btnMainEngineanlysis.Visible = false;
        btnScavepressureanalysis.Visible = false;
        btnspeedanalysis.Visible = false;
    }
    protected void ChartToPdf(Chart chart)
    {
        Document Doc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
        Doc.SetPageSize(iTextSharp.text.PageSize.A4.Rotate());
        PdfWriter.GetInstance(Doc, Response.OutputStream);
        Doc.Open();
        MemoryStream memoryStream = new MemoryStream();


        chart.SaveImage(memoryStream, ChartImageFormat.Png);
        iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(memoryStream.GetBuffer());
        img.ScalePercent(75f);
        memoryStream = new MemoryStream();

        var image = iTextSharp.text.Image.GetInstance(Server.MapPath("../../" + Session["images"] + "/esmlogo4_small.png"));
        
        Doc.Add(image);

        Doc.Add(img);

        Doc.Close();

        
        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=Chart.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Write(Doc);
        Response.End();
    }
    protected void btnslip_Click(object sender, EventArgs e)
    {
        //ChartToPdf(ChartSlip);
        BindData();

        ChartToPdf(ChartSlip);
       // 
    }
    protected void btnconsanalysis_Click(object sender, EventArgs e)
    {
        BindData();
        ChartToPdf(ConsumtionAnalysis);
    }
    protected void btnMainEngineanlysis_Click(object sender, EventArgs e)
    {
        BindData();
        ChartToPdf(MainEngileTempAnalysis);
    }
    protected void btnScavepressureanalysis_Click(object sender, EventArgs e)
    {
        BindData();
        ChartToPdf(MainEngineScavePressureAnalysys);
    }
    protected void btnspeedanalysis_Click(object sender, EventArgs e)
    {
        BindData();
        ChartToPdf(SpeedAnalysis);
    }
    protected void btnPowerVSMEConsumption_Click(object sender, EventArgs e)
    {
        BindData();
        ChartToPdf(PowerVSMEConsumption);
    }
    protected void btnSludgeTank_Click(object sender, EventArgs e)
    {
        BindData();
        ChartToPdf(SludgeTank);
    }
    protected void FreshWaterProd_Click(object sender, EventArgs e)
    {
        BindData();
        ChartToPdf(FreshWaterProd);
    }
}
