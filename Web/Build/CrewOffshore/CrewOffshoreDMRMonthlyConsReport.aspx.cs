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

public partial class CrewOffshoreDMRMonthlyConsReport : PhoenixBasePage 
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarvoyagetap = new PhoenixToolbar();

            toolbarvoyagetap.AddButton("List", "MONTHLYREPORTLIST");
            toolbarvoyagetap.AddButton("Status", "MONTHLYREPORT");
            toolbarvoyagetap.AddButton("Engine", "CONSUMPTION");
            toolbarvoyagetap.AddButton("DP Summary", "DP");



            MenuReportTap.AccessRights = this.ViewState;
            MenuReportTap.MenuList = toolbarvoyagetap.Show();
            MenuReportTap.SelectedMenuIndex = 2;

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
            DataSet ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportByVessel(General.GetNullableInteger(ucVessel.SelectedVessel), General.GetNullableInteger(ddlMonth.SelectedValue), General.GetNullableInteger(ddlYear.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
                ViewState["MONTH"] = ds.Tables[0].Rows[0]["FLDMONTH"].ToString();
                ViewState["YEAR"] = ds.Tables[0].Rows[0]["FLDYEAR"].ToString();
                Session["MONTHLYREPORTID"] = ds.Tables[0].Rows[0]["FLDMONTHLYREPORTID"].ToString();
            }
            BindEngineSummary();
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
        BindEngineSummary();
    }

    protected void ReportTapp_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("MONTHLYREPORTLIST"))
        {
            Response.Redirect("CrewOffshoreDMRMonthlyReportList.aspx", false);
        }
        if (CommandName.ToUpper().Equals("MONTHLYREPORT"))
        {
            Response.Redirect("CrewOffshoreDMRMonthlyReport.aspx", false);
        }
        if (CommandName.ToUpper().Equals("DP"))
        {
            Response.Redirect("CrewOffshoreDMRMonthlyDPOperations.aspx", false);
        }
    }

    //protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    //{
    //    DataListCommandEventArgs dce = (DataListCommandEventArgs)e;

    //    try
    //    {
    //        if (dce.CommandName.ToUpper().Equals("SAVE"))
    //        {
    //            if (!IsValidData())
    //            {
    //                ucError.Visible = true;
    //                return;
    //            }

    //            Guid? monthlyreportid = null;

    //            PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportInsert(int.Parse(ViewState["VESSELID"].ToString()), int.Parse(ViewState["MONTH"].ToString()), int.Parse(ViewState["YEAR"].ToString()), ref monthlyreportid);

    //            Session["MONTHLYREPORTID"] = monthlyreportid;
    //            ucStatus.Text = "Monthly Report Created";

    //            if (Session["MONTHLYREPORTID"] != null)
    //            {
    //                EditMonthlyReport();
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucError.ErrorMessage = ex.Message;
    //        ucError.Visible = true;
    //    }
    //}

    private void EditMonthlyReport()
    {
        DataSet ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportEdit(new Guid(Session["MONTHLYREPORTID"].ToString()));
        if (ds.Tables[0].Rows.Count > 0)
        {
            ViewState["VESSELID"] = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ucVessel.SelectedVessel = ds.Tables[0].Rows[0]["FLDVESSELID"].ToString();
            ddlYear.SelectedValue = ds.Tables[0].Rows[0]["FLDYEAR"].ToString();
            ddlMonth.SelectedValue = ds.Tables[0].Rows[0]["FLDMONTH"].ToString();
            ddlYear.Enabled = false;
            ddlMonth.Enabled = false;
            ViewState["MONTH"] = ds.Tables[0].Rows[0]["FLDMONTH"].ToString();
            ViewState["YEAR"] = ds.Tables[0].Rows[0]["FLDYEAR"].ToString();

        }
    }

    private bool IsValidData()
    {
        ucError.HeaderMessage = "Please provide the following required information";



        return (!ucError.IsError);
    }

    private void BindEngineSummary()
    {
        try
        {
            DataSet ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportEngineSummaryList(General.GetNullableInteger(ucVessel.SelectedVessel)
                , General.GetNullableInteger(ddlMonth.SelectedValue)
                , General.GetNullableInteger(ddlYear.SelectedValue)
                , General.GetNullableGuid(Session["MONTHLYREPORTID"] == null ? "" : Session["MONTHLYREPORTID"].ToString()));

            gvOperationalSummary.DataSource = ds;
            gvOperationalSummary.DataBind();
          
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvOperationalSummary_ItemDataBound(Object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        DataRowView drv = (DataRowView)e.Row.DataItem;
    //        Label lblEngineTypeId = ((Label)e.Row.FindControl("lblEngineTypeId"));
    //        Label lblEngineName = ((Label)e.Row.FindControl("lblEngineName"));

    //        if (lblEngineTypeId != null && lblEngineName != null) 
    //        {
    //            DisplayFuelConsumptionRateChart(lblEngineTypeId.Text, lblEngineName.Text);
    //        }
    //    }        
    //}

    //protected void gvOperationalSummary_RowCommand(object sender, GridViewCommandEventArgs e)
    //{
    //    try
    //    {
    //        GridView _gridView = (GridView)sender;
    //        int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

    //        if (e.CommandName.ToUpper().Equals("EDIT"))
    //        {
    //            ViewState["ENGINETYPEID"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEngineTypeId")).Text;
    //            ViewState["ENGINENAME"] = ((Label)_gridView.Rows[nCurrentRow].FindControl("lblEngineName")).Text;
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

    private void DisplayFuelConsumptionRateChart(string engineTypeId, string engineName)
    {
        try
        {
            if (engineTypeId != "")
            {
                DataSet ds = PhoenixCrewOffshoreDMRMonthlyReport.MonthlyReportEngineOilConsumptionRateList(int.Parse(ViewState["VESSELID"].ToString()), int.Parse(ViewState["MONTH"].ToString()), int.Parse(ViewState["YEAR"].ToString()), int.Parse(engineTypeId));
                DataTable dt = ds.Tables[0];

                if (engineTypeId == "1")
                {
                    double width = 0;
                    double max = double.Parse(ds.Tables[0].Rows[0]["FLDMAX"].ToString());
                    double min = double.Parse(ds.Tables[0].Rows[0]["FLDMIN"].ToString());
                    string maxCaption = "Max " + ds.Tables[0].Rows[0]["FLDMAX"].ToString() + " ltrs/hr";
                    string minCaption = "Min " + ds.Tables[0].Rows[0]["FLDMIN"].ToString() + " ltrs/hr";
                    PhoenixCommonChart pcc = new PhoenixCommonChart(ChartFuelConsRateME1, engineName + " FO Cons ltrs/hr");
                    pcc.ChartType = SeriesChartType.Line;
                    pcc.YSeries("", new YAxisColumn("FLDFUELOILCONSUMPTIONRATE", "Litres / hour"));
                    pcc.XSeries("", 1, "FLDREPORTDATE");
                    pcc.Enable3D = false;
                    width = (max * 1.5) / 100;
                    //if (max <= 100)
                    //    width = 1.5;
                    //if (max > 100 && max <= 200)
                    //    width = 2.7;
                    //if (max > 200)
                    //    width = 3.6;
                    pcc.Show(dt, min, max, minCaption, maxCaption, width, System.Drawing.Color.Red);
                }

                if (engineTypeId == "2")
                {
                    double width = 0;
                    double max = double.Parse(ds.Tables[0].Rows[0]["FLDMAX"].ToString());
                    double min = double.Parse(ds.Tables[0].Rows[0]["FLDMIN"].ToString());
                    string maxCaption = "Max " + ds.Tables[0].Rows[0]["FLDMAX"].ToString() + " ltrs/hr";
                    string minCaption = "Min " + ds.Tables[0].Rows[0]["FLDMIN"].ToString() + " ltrs/hr";
                    PhoenixCommonChart pcc2 = new PhoenixCommonChart(ChartFuelConsRateME2, engineName + " FO Cons ltrs/hr");
                    pcc2.ChartType = SeriesChartType.Line;
                    pcc2.YSeries("", new YAxisColumn("FLDFUELOILCONSUMPTIONRATE", "Litres / hour"));
                    pcc2.XSeries("", 1, "FLDREPORTDATE");
                    pcc2.Enable3D = false;
                    width = (max * 1.5) / 100;
                    //if (max <= 100)
                    //    width = 1.5;
                    //if (max > 100 && max <= 200)
                    //    width = 2.7;
                    //if (max > 200)
                    //    width = 3.6;
                    pcc2.Show(dt, min, max, minCaption, maxCaption, width, System.Drawing.Color.Red);
                }

                if (engineTypeId == "3")
                {
                    double width = 0;
                    double max = double.Parse(ds.Tables[0].Rows[0]["FLDMAX"].ToString());
                    double min = double.Parse(ds.Tables[0].Rows[0]["FLDMIN"].ToString());
                    string maxCaption = "Max " + ds.Tables[0].Rows[0]["FLDMAX"].ToString() + " ltrs/hr";
                    string minCaption = "Min " + ds.Tables[0].Rows[0]["FLDMIN"].ToString() + " ltrs/hr";
                    PhoenixCommonChart pcc3 = new PhoenixCommonChart(ChartFuelConsRateAE1, engineName + " FO Cons ltrs/hr");
                    pcc3.ChartType = SeriesChartType.Line;
                    pcc3.YSeries("", new YAxisColumn("FLDFUELOILCONSUMPTIONRATE", "Litres / hour"));
                    pcc3.XSeries("", 1, "FLDREPORTDATE");
                    pcc3.Enable3D = false;
                    width = (max * 1.5) / 100;
                    //if (max <= 100)
                    //    width = 1.5;
                    //if (max > 100 && max <= 200)
                    //    width = 2.7;
                    //if (max > 200)
                    //    width = 3.6;
                    pcc3.Show(dt, min, max, minCaption, maxCaption, width, System.Drawing.Color.Red);
                }

                if (engineTypeId == "4")
                {
                    double width = 0;
                    double max = double.Parse(ds.Tables[0].Rows[0]["FLDMAX"].ToString());
                    double min = double.Parse(ds.Tables[0].Rows[0]["FLDMIN"].ToString());
                    string maxCaption = "Max " + ds.Tables[0].Rows[0]["FLDMAX"].ToString() + " ltrs/hr";
                    string minCaption = "Min " + ds.Tables[0].Rows[0]["FLDMIN"].ToString() + " ltrs/hr";
                    PhoenixCommonChart pcc4 = new PhoenixCommonChart(ChartFuelConsRateAE2, engineName + " FO Cons ltrs/hr");
                    pcc4.ChartType = SeriesChartType.Line;
                    pcc4.YSeries("", new YAxisColumn("FLDFUELOILCONSUMPTIONRATE", "Litres / hour"));
                    pcc4.XSeries("", 1, "FLDREPORTDATE");
                    pcc4.Enable3D = false;
                    width = (max * 1.5) / 100;
                    //if (max <= 100)
                    //    width = 1.5;
                    //if (max > 100 && max <= 200)
                    //    width = 2.7;
                    //if (max > 200)
                    //    width = 3.6;
                    pcc4.Show(dt, min, max, minCaption, maxCaption, width, System.Drawing.Color.Red);
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ucVessel_OnTextChangedEvent(object sender, EventArgs e)
    {
        ViewState["MONTH"] = ddlMonth.SelectedValue;
        ViewState["YEAR"] = ddlYear.SelectedValue;
        ViewState["VESSELID"] = ucVessel.SelectedVessel;
    }

    protected void btnExportME1_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartFuelConsRateME1);
    }

    protected void btnExportME2_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartFuelConsRateME2);
    }

    protected void btnExportAE1_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartFuelConsRateAE1);
    }

    protected void btnExportAE2_Click(object sender, EventArgs e)
    {
        ChartToPdf(ChartFuelConsRateAE2);
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

    protected void gvOperationalSummary_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        BindEngineSummary();
    }

    protected void gvOperationalSummary_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            ////GridView _gridView = (GridView)sender;
            ////int nCurrentRow = Int32.Parse(e.CommandArgument.ToString());

            //if (e.CommandName.ToUpper().Equals("EDIT"))
            //{
            //    ViewState["ENGINETYPEID"] = ((RadLabel)e.Item.FindControl("lblEngineTypeId")).Text;
            //    ViewState["ENGINENAME"] = ((RadLabel)e.Item.FindControl("lblEngineName")).Text;
            //    DisplayFuelConsumptionRateChart(ViewState["ENGINETYPEID"].ToString(), ViewState["ENGINENAME"].ToString());
            //}
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvOperationalSummary_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {

            RadLabel lblEngineTypeId = ((RadLabel)e.Item.FindControl("lblEngineTypeId"));
            RadLabel lblEngineName = ((RadLabel)e.Item.FindControl("lblEngineName"));

            if (lblEngineTypeId != null && lblEngineName != null)
            {
                DisplayFuelConsumptionRateChart(lblEngineTypeId.Text, lblEngineName.Text);
            }
        }
    }
}
