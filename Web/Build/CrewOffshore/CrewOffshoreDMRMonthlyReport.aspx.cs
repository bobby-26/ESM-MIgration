using System;
using System.Data;
using System.IO;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SouthNests.Phoenix.CrewOffshore;
using SouthNests.Phoenix.Framework;
using Telerik.Web.UI;
using System.Web.UI.DataVisualization;
public partial class CrewOffshoreDMRMonthlyReport : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarReporttap = new PhoenixToolbar();
            toolbarReporttap.AddButton("Save", "SAVE",ToolBarDirection.Right);
            MenuNewSaveTabStrip.AccessRights = this.ViewState;
            MenuNewSaveTabStrip.MenuList = toolbarReporttap.Show();

            PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();
            toolbarvoyagetap.AddButton("List", "MONTHLYREPORTLIST");
            toolbarvoyagetap.AddButton("Status", "MONTHLYREPORT");
            toolbarvoyagetap.AddButton("Engine", "CONSUMPTION");
            toolbarvoyagetap.AddButton("DP Summary", "DP");
            

            MenuReportTap.AccessRights = this.ViewState;
            MenuReportTap.MenuList = toolbarvoyagetap.Show();
            MenuReportTap.SelectedMenuIndex = 1;

            if (Request.QueryString["Source"] != null && Request.QueryString["Source"].ToString() == "midnightreport")
            {
                MenuNewSaveTabStrip.Visible = false;
                MenuReportTap.Visible = false;
            }

            if (!IsPostBack)
            {
                ddlYear.Items.Clear();
                ddlYear.Items.Add(new DropDownListItem("--Select--", ""));
                for (int i = 2005; i <= DateTime.Now.Year; i++)
                {
                    ddlYear.Items.Add((new DropDownListItem(i.ToString(), i.ToString())));
                }
               

                ddlYear.SelectedValue = DateTime.Now.Year.ToString();
                ddlMonth.SelectedValue = DateTime.Now.Month.ToString();

                ViewState["YEAR"] = DateTime.Now.Year;
                ViewState["MONTH"] = DateTime.Now.Month;
                ViewState["TaskShortCode"] = "";

                if (PhoenixSecurityContext.CurrentSecurityContext.VesselID > 0)
                {
                    ViewState["VESSELID"] = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.SelectedVessel = PhoenixSecurityContext.CurrentSecurityContext.VesselID.ToString();
                    ucVessel.Enabled = false;
                }
                else
                {
                    ViewState["VESSELID"] = "";
                    ucVessel.Enabled = true;
                }

                if (Session["MONTHLYREPORTID"] != null)
                {
                    EditMonthlyReport();
                }
               

            }

            MenuNewSaveTabStrip.Visible = Session["MONTHLYREPORTID"] == null ? true : false;

            DataSet ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportByVessel(General.GetNullableInteger(ucVessel.SelectedVessel), General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                ViewState["MONTH"] = ds.Tables[0].Rows[0]["FLDMONTH"].ToString();
                ViewState["YEAR"] = ds.Tables[0].Rows[0]["FLDYEAR"].ToString();
                Session["MONTHLYREPORTID"] = ds.Tables[0].Rows[0]["FLDMONTHLYREPORTID"].ToString();
            }
             DisplayChart();
            gvOperationalSummary.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
            BindOperationalSummary();
            //ChartsClear();
            //DisplayChart();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }

    }

    protected void ddlMonth_TextChangedEvent(object sender, EventArgs e)
    {
        ViewState["MONTH"] = ddlMonth.SelectedValue;
        ViewState["YEAR"] = ddlYear.SelectedValue;
        ViewState["VESSELID"] = ucVessel.SelectedVessel;
        //BindOperationalSummary();
        //DisplayChart();
        gvOperationalSummary.Rebind();
    }

    protected void ReportTapp_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;
        if (CommandName.ToUpper().Equals("MONTHLYREPORTLIST"))
        {
            Response.Redirect("CrewOffshoreDMRMonthlyReportList.aspx", false);
        }
        if (CommandName.ToUpper().Equals("CONSUMPTION"))
        {
            Response.Redirect("CrewOffshoreDMRMonthlyConsReport.aspx", false);
        }
        if (CommandName.ToUpper().Equals("DP"))
        {
            Response.Redirect("CrewOffshoreDMRMonthlyDPOperations.aspx", false);
        }
    }

    protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (!IsValidData())
                {
                    ucError.Visible = true;
                    return;
                }

                Guid? monthlyreportid = null;

                PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportInsert(int.Parse(ViewState["VESSELID"].ToString()), int.Parse(ViewState["MONTH"].ToString()), int.Parse(ViewState["YEAR"].ToString()), ref monthlyreportid);
                
                Session["MONTHLYREPORTID"] = monthlyreportid;
                ucStatus.Text = "Monthly Report Created";

                if (Session["MONTHLYREPORTID"] != null)
                {
                    EditMonthlyReport();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void EditMonthlyReport()
    {
        DataSet ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportEdit(new Guid(Session["MONTHLYREPORTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ucVessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ddlYear.SelectedValue = ds.Tables[0].Rows[0]["FLDYEAR"].ToString();
            ddlMonth.SelectedValue = ds.Tables[0].Rows[0]["FLDMONTH"].ToString();            
            ViewState["MONTH"] = ds.Tables[0].Rows[0]["FLDMONTH"].ToString();
            ddlYear.Enabled = PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? false : true;
            ddlMonth.Enabled = PhoenixSecurityContext.CurrentSecurityContext.InstallCode > 0 ? false : true;
        }
    }

    private bool IsValidData()
    {
        ucError.HeaderMessage = "Please provide the following required information";

        

        return (!ucError.IsError);
    }

    private void BindOperationalSummary()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportOperationalSummaryList(General.GetNullableInteger(ucVessel.SelectedVessel), General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue), General.GetNullableGuid(Session["MONTHLYREPORTID"] == null ? "" : Session["MONTHLYREPORTID"].ToString()));
           

            if (ds.Tables[0].Rows.Count > 0)
            {
                gvOperationalSummary.DataSource = ds;
               // gvOperationalSummary.DataBind();
            }
            else
            {
                gvOperationalSummary.DataSource = ds;
               // gvOperationalSummary.DataBind();
               // btnExport.Visible = false;
            }

            gvOperationalSummary.VirtualItemCount = ds.Tables[0].Rows.Count;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOperationalSummary_ItemDataBound(Object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            Label lblOperationalTaskid = ((Label)e.Row.FindControl("lblOperationalTaskid"));
            Label lblOperationalTaskName = ((Label)e.Row.FindControl("lblOperationalTaskName"));
            Label lblShortName = ((Label)e.Row.FindControl("lblShortName"));
            DisplayFuelConsumptionRateChart(General.GetNullableGuid(lblOperationalTaskid.Text), lblOperationalTaskName.Text, lblShortName.Text);
        }
        if (e.Row.RowType == DataControlRowType.Footer)
        {
            Label lblTotalTimeDuration = (Label)e.Row.FindControl("lblTotalTimeDuration");
            Label lblTotalFuelConsumption = (Label)e.Row.FindControl("lblTotalFuelConsumption");
            Label lblTotalConsumptionRate = (Label)e.Row.FindControl("lblTotalConsumptionRate");
            DataSet ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportOperationalSummaryList(int.Parse(ViewState["VESSELID"].ToString()), int.Parse(ViewState["MONTH"].ToString()), int.Parse(ViewState["YEAR"].ToString()), General.GetNullableGuid(Session["MONTHLYREPORTID"] == null ? "" : Session["MONTHLYREPORTID"].ToString()));
            if (ds.Tables[1].Rows.Count > 0)
            {
                if(lblTotalTimeDuration != null)
                    lblTotalTimeDuration.Text = ds.Tables[1].Rows[0]["FLDTIMEDURATION"].ToString();
                if (lblTotalFuelConsumption != null)
                    lblTotalFuelConsumption.Text = ds.Tables[1].Rows[0]["FLDFUELOILCONSUMPTION"].ToString();
                if (lblTotalConsumptionRate != null)
                    lblTotalConsumptionRate.Text = ds.Tables[1].Rows[0]["FLDCONSRATE"].ToString();
            }
        }
    }

    //protected void gvOperationalSummary_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("EDIT"))
    //        {
    //            ViewState["TASKID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOperationalTaskid")).Text;
    //            ViewState["TASKNAME"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblOperationalTaskName")).Text;
    //            tblChart.Visible = false;
    //            ChartFuelConsRate.Visible = true;
    //            btnFuelCons.Visible = true;
    //            ViewState["TaskShortCode"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblShortName")).Text;
    //            DisplayFuelConsumptionRateChart();
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    protected void gvOperationalSummary_RowEditing(object sender, GridViewEditEventArgs de)
    {
        GridView _gridView = (GridView)sender;
        _gridView.SelectedIndex = de.NewEditIndex;
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
    {
        try
        {
            dt.Rows.Add(dt.NewRow());
            gv.DataSource = dt;
            gv.DataBind();

            int colcount = gv.Columns.Count;
            gv.Rows[0].Cells.Clear();
            gv.Rows[0].Cells.Add(new TableCell());
            gv.Rows[0].Cells[0].ColumnSpan = colcount;
            gv.Rows[0].Cells[0].HorizontalAlign = HorizontalAlign.Center;
            gv.Rows[0].Cells[0].ForeColor = System.Drawing.Color.Red;
            gv.Rows[0].Cells[0].Font.Bold = true;
            gv.Rows[0].Cells[0].Text = "NO RECORDS FOUND";
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void DisplayChart()
    {

        DataSet ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportOperationalSummaryList(int.Parse(ViewState["VESSELID"].ToString()), int.Parse(ViewState["MONTH"].ToString()), int.Parse(ViewState["YEAR"].ToString()), General.GetNullableGuid(Session["MONTHLYREPORTID"] == null ? "" : Session["MONTHLYREPORTID"].ToString()));
        DataTable dt = ds.Tables[2];

        if (dt.Rows.Count > 0)
        {
            PhoenixCommonChart pcc = new PhoenixCommonChart(ChartMonthlyActivityPercentage, "Monthly Activity Percentages");
            pcc.ChartType = SeriesChartType.Pie;
            pcc.YSeries("", new YAxisColumn("FLDTIMEDURATIONPERCENTAGE", ""));
            pcc.XSeries("", 1, "FLDTASKNAME");
            pcc.Enable3D = true;
            pcc.Show(dt);
            pcc.ShowLabelInPie = false;
            ChartMonthlyActivityPercentage.Visible = true;
        }
        else
        {
            ChartMonthlyActivityPercentage.Visible = false;
        }
    }

    private void DisplayFuelConsumptionRateChart()
    {
        if (ViewState["TASKID"].ToString() != "")
        {
            DataSet ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportFuelConsumptionRateList(int.Parse(ViewState["VESSELID"].ToString()), int.Parse(ViewState["MONTH"].ToString()), int.Parse(ViewState["YEAR"].ToString()), new Guid(ViewState["TASKID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                double width = 0;
                double max = double.Parse(ds.Tables[0].Rows[0]["FLDMAX"].ToString());
                double min = double.Parse(ds.Tables[0].Rows[0]["FLDMIN"].ToString());
                string maxCaption = "Max " + ds.Tables[0].Rows[0]["FLDMAX"].ToString() + " ltrs/hr";
                string minCaption = "Min " + ds.Tables[0].Rows[0]["FLDMIN"].ToString() + " ltrs/hr";

                PhoenixCommonChart pcc = new PhoenixCommonChart(ChartFuelConsRate, ViewState["TASKNAME"].ToString(), " Fuel Consumption Rate");
                pcc.ChartType = SeriesChartType.Line;
                pcc.YSeries("", new YAxisColumn("FLDFUELOILCONSUMPTIONRATE", "Litres / hour"));
                pcc.XSeries("", 1, "FLDREPORTDATE");
                pcc.Enable3D = false;
                width = (max * 1.5) / 100;
                pcc.Show(dt, min, max, minCaption, maxCaption, width, System.Drawing.Color.Red);
            }
        }
    }

    private void DisplayFuelConsumptionRateChart(Guid? taskId, string taskName, string taskShortCode)
    {
        Chart ChartDp = new Chart();

        if (taskId != null)
        {
            if (taskShortCode == "PORT")
            {
                ChartDp = ChartPORT;
                btnPORT.Visible = true;
            }

            if (taskShortCode == "SSES")
            {
                ChartDp = ChartSSES;
                btnSSES.Visible = true;
            }

            if (taskShortCode == "MAN")
            {
                ChartDp = ChartMAN;
                btnMAN.Visible = true;
            }

            if (taskShortCode == "SS")
            {
                ChartDp = ChartSS;
                btnSS.Visible = true;
            }

            if (taskShortCode == "STBY")
            {
                ChartDp = ChartSTBY;
                btnSTBY.Visible = true;
            }

            if (taskShortCode == "CAR-DP")
            {
                ChartDp = ChartCARDP;
                btnCARDP.Visible = true;
            }

            if (taskShortCode == "CAR")
            {
                ChartDp = ChartCAR;
                btnCAR.Visible = true;
            }


            if (taskShortCode == "AHD")
            {
                ChartDp = ChartAHD;
                btnAHD.Visible = true;
            }


            if (taskShortCode == "ROV")
            {
                ChartDp = ChartROV;
                btnROV.Visible = true;
            }


            if (taskShortCode == "BRD")
            {
                ChartDp = ChartBRD;
                btnBRD.Visible = true;
            }


            if (taskShortCode == "DIVE")
            {
                ChartDp = ChartDIVE;
                btnDIVE.Visible = true;
            }

            if (taskShortCode == "TOW")
            {
                ChartDp = ChartTOW;
                btnTOW.Visible = true;
            }

            if (taskShortCode == "OTOW")
            {
                ChartDp = ChartOTOW;
                btnOTOW.Visible = true;
            }

            tblChart.Visible = true;

            DataSet ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportFuelConsumptionRateList(int.Parse(ViewState["VESSELID"].ToString()), int.Parse(ViewState["MONTH"].ToString()), int.Parse(ViewState["YEAR"].ToString()), new Guid(taskId.ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                double width = 0;
                double max = double.Parse(ds.Tables[0].Rows[0]["FLDMAX"].ToString());
                double min = double.Parse(ds.Tables[0].Rows[0]["FLDMIN"].ToString());
                string maxCaption = "Max " + ds.Tables[0].Rows[0]["FLDMAX"].ToString() + " ltrs/hr";
                string minCaption = "Min " + ds.Tables[0].Rows[0]["FLDMIN"].ToString() + " ltrs/hr";

                PhoenixCommonChart pcc = new PhoenixCommonChart(ChartDp, taskName, " Fuel Consumption Rate");
                pcc.ChartType = SeriesChartType.Line;
                pcc.YSeries("", new YAxisColumn("FLDFUELOILCONSUMPTIONRATE", "Litres / hour"));
                pcc.XSeries("", 1, "FLDREPORTDATE");
                pcc.Enable3D = false;
                width = (max * 1.5) / 100;
                pcc.Show(dt, min, max, minCaption, maxCaption, width, System.Drawing.Color.Red);
            }
        }
    }

    protected void ucVessel_OnTextChangedEvent(object sender, EventArgs e)
    {
        ViewState["MONTH"] = ddlMonth.SelectedValue;
        ViewState["YEAR"] = ddlYear.SelectedValue;
        ViewState["VESSELID"] = ucVessel.SelectedVessel;
        gvOperationalSummary.Rebind();
    }

    protected void btnExport_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartMonthlyActivityPercentage);
    }

    protected void btnFuelCons_Click(object sender, EventArgs e)
    {
        Chart ChartDp = new Chart();

        if (ViewState["TaskShortCode"].ToString() == "PORT")
            ChartDp = ChartPORT;
        if (ViewState["TaskShortCode"].ToString() == "SSES")
            ChartDp = ChartSSES;
        if (ViewState["TaskShortCode"].ToString() == "MAN")
            ChartDp = ChartMAN;
        if (ViewState["TaskShortCode"].ToString() == "SS")
            ChartDp = ChartSS;
        if (ViewState["TaskShortCode"].ToString() == "STBY")
            ChartDp = ChartSTBY;
        if (ViewState["TaskShortCode"].ToString() == "CAR-DP")
            ChartDp = ChartCARDP;
        if (ViewState["TaskShortCode"].ToString() == "CAR")
            ChartDp = ChartCAR;
        if (ViewState["TaskShortCode"].ToString() == "AHD")
            ChartDp = ChartAHD;
        if (ViewState["TaskShortCode"].ToString() == "ROV")
            ChartDp = ChartROV;
        if (ViewState["TaskShortCode"].ToString() == "BRD")
            ChartDp = ChartBRD;
        if (ViewState["TaskShortCode"].ToString() == "DIVE")
            ChartDp = ChartDIVE;
        if (ViewState["TaskShortCode"].ToString() == "TOW")
            ChartDp = ChartTOW;
        if (ViewState["TaskShortCode"].ToString() == "OTOW")
            ChartDp = ChartOTOW;
        ChartToPdf(ChartDp);
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
        Doc.Add(img);

        Doc.Close();

        Response.ContentType = "application/pdf";
        Response.AddHeader("content-disposition", "attachment;filename=Chart.pdf");
        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Write(Doc);
        Response.End();
    }

    protected void btnPORT_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartPORT);
    }

    protected void btnSSES_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartSSES);
    }
    protected void btnMAN_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartMAN);
    }
    protected void btnSS_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartSS);
    }
    protected void btnSTBY_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartSTBY);
    }
    protected void btnCARDP_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartCARDP);
    }
    protected void btnCAR_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartCAR);
    }
    protected void btnAHD_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartAHD);
    }
    protected void btnROV_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartROV);
    }
    protected void btnBRD_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartBRD);
    }
    protected void btnDIVE_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartDIVE);
    }
    protected void btnTOW_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartTOW);
    }
    protected void btnOTOW_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartOTOW);
    }


    protected void gvOperationalSummary_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindOperationalSummary();
    }

    protected void gvOperationalSummary_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
          //  GridView _gridView = (GridView)sender;
            int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                ViewState["TASKID"] = ((RadLabel)e.Item.FindControl("lblOperationalTaskid")).Text;
                ViewState["TASKNAME"] = ((RadLabel)e.Item.FindControl("lblOperationalTaskName")).Text;
                tblChart.Visible = false;
                ChartFuelConsRate.Visible = true;
                btnFuelCons.Visible = true;
                ViewState["TaskShortCode"] = ((RadLabel)e.Item.FindControl("lblShortName")).Text;
                DisplayFuelConsumptionRateChart();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
