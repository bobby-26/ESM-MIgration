using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Collections;
using System.Text;
using Telerik.Web.UI;

public partial class VesselPositionArrivalReportPassageSummary : PhoenixBasePage
{
    DataSet dsDeatil = new DataSet();
    DataSet dsFWconsumption = new DataSet();
    DataSet dsOilconsumption = new DataSet();
    DataSet dsVoyage = new DataSet();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);

            cmdHiddenPick.Attributes.Add("style", "display:none;");
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");

            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddButton("List", "ARRIVALREPORT");
            toolbar.AddButton("Noon to EOSP", "EOSP");
            toolbar.AddButton("Passage Summary", "SUMMARY");
            toolbar.AddButton("MRV Summary  ", "MRVSUMMARY");

            ArrivalSummary.AccessRights = this.ViewState;
            ArrivalSummary.MenuList = toolbar.Show();

            ArrivalSummary.SelectedMenuIndex = 2;

            toolbar = new PhoenixToolbar();

            BindVesselId();

            toolbar.AddButton("Export Excel", "EXCEL", ToolBarDirection.Right);
            toolbar.AddButton("Export PDF", "PDF", ToolBarDirection.Right);
            toolbar.AddImageLink("javascript:return showPickList('spnPickListStore', 'codehelp1', '', '../VesselPosition/VesselPositionVPRSChartCPDeviation.aspx?vesselid="
                           + ViewState["VESSELID"] + "&vesselname=" + txtVessel.Text + "&cosp=" + txtCOSPDate.Text + "&eosp=" + txtEOSPDate.Text + "'); return false;", "Chart", "", "SPEED", ToolBarDirection.Right);
            toolbar.AddImageLink("javascript:return showPickList('spnPickListStore', 'codehelp1', '', '../VesselPosition/VesselPositionArrivalReportPassageSummaryBreakup.aspx?vesselid="
                           + ViewState["VESSELID"] + "'); return false;", "Break Up", "", "BREAKUP", ToolBarDirection.Right);
            if (PhoenixSecurityContext.CurrentSecurityContext.InstallCode == 0)
                toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
            ArrivalSave.AccessRights = this.ViewState;
            ArrivalSave.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                VesselArrivalEdit();

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void BindVesselId()
    {
        if (Session["VESSELARRIVALID"] != null)
        {
            DataSet ds = PhoenixVesselPositionArrivalReport.EditArrivalReport(General.GetNullableGuid(Session["VESSELARRIVALID"].ToString()));
            dsDeatil = ds;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();

                    txtVessel.Text = dr["FLDVESSELNAME"].ToString();
                }
            }
        }

        DataSet dsSummary = PhoenixVesselPositionArrivalOilConsumption.ListVoyageSummary(
            General.GetNullableInteger(ViewState["VESSELID"].ToString())
            , General.GetNullableGuid(Session["VESSELARRIVALID"] == null ? "" : Session["VESSELARRIVALID"].ToString()));

        dsVoyage = dsSummary;
        if (dsSummary.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsSummary.Tables[0].Rows[0];

            txtCOSPDate.Text = General.GetDateTimeToString(dr["FLDCOSP"].ToString());
            txtEOSPDate.Text = General.GetDateTimeToString(dr["FLDEOSP"].ToString());
        }
    }
    protected void VesselArrivalEdit()
    {
        if (Session["VESSELARRIVALID"] != null)
        {
            DataSet ds = PhoenixVesselPositionArrivalReport.EditArrivalReport(General.GetNullableGuid(Session["VESSELARRIVALID"].ToString()));
            dsDeatil = ds;
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DataRow dr = ds.Tables[0].Rows[0];

                    ViewState["VESSELID"] = dr["FLDVESSELID"].ToString();

                    txtVessel.Text = dr["FLDVESSELNAME"].ToString();
                    txtVoyage.Text = dr["FLDVOYAGENO"].ToString();
                    txtPort.Text = dr["FLDTOSEAPORTNAME"].ToString();
                    txtFromPort.Text = dr["FLDFROMSEAPORTNAME"].ToString();

                    txtManoevering.Text = dr["FLDMANOEVERING"].ToString();
                    txtManoeveringDist.Text = dr["FLDMANOEVERINGDIST"].ToString();
                    txtAvgRPM.Text = dr["FLDAVGRPM"].ToString();
                    txtSpeed.Text = dr["FLDAVGSPEED"].ToString();
                    txtAvgSlip.Text = dr["FLDAVGSLIP"].ToString();
                    txtFullSpeed.Text = dr["FLDFULLSPEED"].ToString();
                    txtReducedSpeed.Text = dr["FLDREDUCEDSPEED"].ToString();
                    txtStopped.Text = dr["FLDPSSTOPPED"].ToString();
                    txtDistanceObserved.Text = dr["FLDDISTOBSERVED"].ToString();
                    txtEngineDistance.Text = dr["FLDENGINEDISTANCE"].ToString();
                    txtEEOI.Text = dr["FLDPSEEOI"].ToString();
                    txtSuptRemarks.Text = dr["FLDSUPTREMARKS"].ToString();
                }
            }

            DataSet dsSummary = PhoenixVesselPositionArrivalOilConsumption.ListVoyageSummary(
            General.GetNullableInteger(ViewState["VESSELID"].ToString())
            , General.GetNullableGuid(Session["VESSELARRIVALID"] == null ? "" : Session["VESSELARRIVALID"].ToString()));
            dsVoyage = dsSummary;
            if (dsSummary.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dsSummary.Tables[0].Rows[0];

                txtBallastLaden.Text = dr["FLDBALLASTLADEN"].ToString();

                txtCOSPDate.Text = General.GetDateTimeToString(dr["FLDCOSP"].ToString());

                if (General.GetNullableDateTime(dr["FLDCOSP"].ToString()) != null)
                    txtCOSPDateTime.SelectedDate = Convert.ToDateTime(dr["FLDCOSP"].ToString());

                txtEOSPDate.Text = General.GetDateTimeToString(dr["FLDEOSP"].ToString());

                if (General.GetNullableDateTime(dr["FLDEOSP"].ToString()) != null)
                    txtEOSPDatetime.SelectedDate = Convert.ToDateTime(dr["FLDEOSP"].ToString());

                lblDistanceSailedBad.Text = dr["FLDBADWEATHERDIST"].ToString();
                lblDistanceSailedGood.Text = dr["FLDGOODWEATHERDIST"].ToString();
                lblDistanceSailedOverall.Text = dr["FLDOVERALLDIST"].ToString();

                lblTimeEnrouteBad.Text = dr["FLDBADENROUTE"].ToString();
                lblTimeEnrouteGood.Text = dr["FLDGOODENROUTE"].ToString();
                lblTimeEnrouteOverall.Text = dr["FLDOVERALLENROUTE"].ToString();

                lblAvgSpeedktsBad.Text = dr["FLDBADSPEED"].ToString();
                lblAvgSpeedktsGood.Text = dr["FLDGOODSPEED"].ToString();
                lblAvgSpeedktsOverall.Text = dr["FLDOVERALLSPEED"].ToString();

                lblPredictedTimeBad.Text = dr["FLDPREDICTEDTIMEBAD"].ToString();
                lblPredictedTimeGood.Text = dr["FLDPREDICTEDTIMEGOOD"].ToString();
                lblPredictedTimeOverall.Text = dr["FLDPREDICTEDTIMEOVERALL"].ToString();

                if (General.GetNullableDecimal(dr["FLDBADWEATHERLOSSORGAIN"].ToString()) < 0)
                {
                    OverallLossorGainBad.Text = "(" + -1 * General.GetNullableDecimal(dr["FLDBADWEATHERLOSSORGAIN"].ToString()) + ")";
                    OverallLossorGainBad.Attributes.Add("style", "color:red;");
                }

                else
                    OverallLossorGainBad.Text = dr["FLDBADWEATHERLOSSORGAIN"].ToString();

                if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) < 0)
                {
                    OverallLossorGainGood.Text = "(" + -1 * General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + ")";
                    OverallLossorGainGood.Attributes.Add("style", "color:red;");
                }

                else
                    OverallLossorGainGood.Text = dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString();

                if (General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN"].ToString()) < 0)
                {
                    OverallLossorGainAll.Text = "(" + -1 * General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN"].ToString()) + ")";
                    OverallLossorGainAll.Attributes.Add("style", "color:red;");
                }
                else
                    OverallLossorGainAll.Text = dr["FLDOVERALLTIMELOSSORGAIN"].ToString();

                //lblTotalMEFOCinVoyageBad.Text = dr["FLDFOCBAD"].ToString();
                lblTotalMEFOCinVoyageGood.Text = dr["FLDFOCGOOD"].ToString();
                //lblTotalMEFOCinVoyageOverall.Text = dr["FLDFOCOVERALL"].ToString();

                //lblPredictedFOCinVoyageBad.Text = dr["FLDPREDICTFOCBAD"].ToString();
                lblPredictedFOCinVoyageGood.Text = dr["FLDPREDICTFOCGOOD"].ToString();
                //lblPredictedFOCinVoyageOverall.Text = dr["FLDPREDICTFOCOVERALL"].ToString();

                //if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCBAD"].ToString()) < 0)
                //{
                //    lblLossorGaininFOCBad.Text = "(" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCBAD"].ToString()) + ")";
                //    lblLossorGaininFOCBad.Attributes.Add("style", "color:red;");
                //}
                //else
                //    lblLossorGaininFOCBad.Text = dr["FLDLOSSORGAINFOCBAD"].ToString();


                if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) < 0)
                {
                    lblLossorGaininFOCGood.Text = "(" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) + ")";
                    lblLossorGaininFOCGood.Attributes.Add("style", "color:red;");
                }
                else
                    lblLossorGaininFOCGood.Text = dr["FLDLOSSORGAINFOCGOOD"].ToString();

                //if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCOVERALL"].ToString()) < 0)
                //{
                //    lblLossorGaininFOCOverall.Text = "("+-1*General.GetNullableDecimal(dr["FLDLOSSORGAINFOCOVERALL"].ToString())+")";
                //    lblLossorGaininFOCOverall.Attributes.Add("style", "color:red;");
                //}
                //else
                //    lblLossorGaininFOCOverall.Text = dr["FLDLOSSORGAINFOCOVERALL"].ToString();

                lblAvgRPMBad.Text = dr["FLDAVGRPMBAD"].ToString();
                lblAvgRPMGood.Text = dr["FLDAVGRPMGOOD"].ToString();
                lblAvgRPMOverall.Text = dr["FLDAVGRPMOVERALL"].ToString();

                txtAvgRPM.Text = dr["FLDAVGRPMOVERALL"].ToString();

                lblAvgBHPBad.Text = dr["FLDAVGBHPBAD"].ToString();
                lblAvgBHPGood.Text = dr["FLDAVGBHPGOOD"].ToString();
                lblAvgBHPOverall.Text = dr["FLDAVGBHPOVERALL"].ToString();

                lblCPSpeedValue.Text = dr["FLDCHARTERPARTYSPEED"].ToString();
                lblPredictedTimeValue.Text = dr["FLDPREDICTEDTIMEGOOD"].ToString();

                if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) < 0)
                {
                    lblTimelossorgainingoodweatherValue.Text = "(" + -1 * General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + ")";
                    lblTimelossorgainingoodweatherValue.Attributes.Add("style", "color:red;");
                }
                else
                    lblTimelossorgainingoodweatherValue.Text = dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString();

                if (General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) < 0)
                {
                    lblOverallTimelossorgainValue.Text = "(" + -1 * General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) + ")";
                    lblOverallTimelossorgainValue.Attributes.Add("style", "color:red;");
                }
                else
                    lblOverallTimelossorgainValue.Text = dr["FLDOVERALLTIMELOSSORGAIN2"].ToString(); // check

                lblDailyCPAllowanceHFO.Text = dr["FLDCPHFO"].ToString();
                lblDailyCPAllowanceMDO.Text = dr["FLDCPMDO"].ToString();

                lblActuaGoodWXConsumptionHFO.Text = dr["FLDACTHFOCONSGOOD"].ToString();
                lblActuaGoodWXConsumptionMDO.Text = dr["FLDACTMDOCONSGOOD"].ToString();

                lblGoodWXAvgDailyConsumptionHFO.Text = dr["FLDAVGHFOCONSGOOD"].ToString();
                lblGoodWXAvgDailyConsumptionMDO.Text = dr["FLDAVGMDOCONSGOOD"].ToString();

                lblOverallAvgDailyConsumptionHOF.Text = dr["FLDAVGHFOCONSOVERALL"].ToString();
                lblOverallAvgDailyConsumptionMDO.Text = dr["FLDAVGMDOCONSOVERALL"].ToString();

                lblAllowedGoodWXConsumptionHFO.Text = dr["FLDALLOWEDGOODWXHFOCONS"].ToString();
                lblAllowedGoodWXConsumptionMDO.Text = dr["FLDALLOWEDGOODWXMDOCONS"].ToString();

                if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) < 0)
                {
                    lblGoodWXoverorunderconsumptionHFO.Text = "(" + -1 * General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) + ")";
                    lblGoodWXoverorunderconsumptionHFO.Attributes.Add("style", "color:red;");
                }
                else
                    lblGoodWXoverorunderconsumptionHFO.Text = dr["FLDGOODWXUNDEROVERHFOCONS"].ToString();

                if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) < 0)
                {
                    lblGoodWXoverorunderconsumptionMDO.Text = "(" + -1 * General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) + ")";
                    lblGoodWXoverorunderconsumptionMDO.Attributes.Add("style", "color:red;");
                }
                else
                    lblGoodWXoverorunderconsumptionMDO.Text = dr["FLDGOODWXUNDEROVERMDOCONS"].ToString();

                if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) < 0)
                {
                    lblOveralloverorunderConsumptionHFO.Text = "(" + -1 * General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) + ")";
                    lblOveralloverorunderConsumptionHFO.Attributes.Add("style", "color:red;");
                }
                else
                    lblOveralloverorunderConsumptionHFO.Text = dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString();

                if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) < 0)
                {
                    lblOveralloverorunderConsumptionMDO.Text = "(" + -1 * General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) + ")";
                    lblOveralloverorunderConsumptionMDO.Attributes.Add("style", "color:red;");
                }
                else
                    lblOveralloverorunderConsumptionMDO.Text = dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString();

                lblFromTo.Text = "<b>Voy : </b>" + dr["FLDFROMPORT"].ToString() + " - " + dr["FLDTOPORT"].ToString();
                lblDepHFO.Text = dr["FLDHFOROBONDEP"].ToString();
                lblDepMDO.Text = dr["FLDMDOROBONDEP"].ToString();
                lblArrHFO.Text = dr["FLDHFOROBONARR"].ToString();
                lblArrMDO.Text = dr["FLDMDOROBONARR"].ToString();
                lblConsHFO.Text = dr["FLDHFOCONS"].ToString();
                lblConsMDO.Text = dr["FLDMDOCONS"].ToString();
                lblConsME.Text = dr["FLDMEOVERALLCONS"].ToString();
                lblConsAE.Text = dr["FLDAEOVERALLCONS"].ToString();

                lblConsHFOValue.Text = dr["FLDHFOCONS"].ToString();
                lblConsMDOValue.Text = dr["FLDMDOCONS"].ToString();
                lblConsMEValue.Text = dr["FLDMEOVERALLCONS"].ToString();
                lblConsAEValue.Text = dr["FLDAEOVERALLCONS"].ToString();

            }
        }
    }

    protected void ArrivalSave_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (Session["VESSELARRIVALID"] != null && Session["VESSELARRIVALID"].ToString() != "")
                {
                    UpdateArrivalReport();
                }

                VesselArrivalEdit();
                gvConsumptionRebind();
                gvFWRebind();
            }
            if (CommandName.ToUpper().Equals("PDF"))
            {
                ConvertToPdf(PrepareHtmlDoc());
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ConvertToExcel(PrepareHtmlDocVoyageSummary());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        VesselArrivalEdit();
    }

    protected void Reset()
    {
        txtSpeed.Text = "";
        txtAvgRPM.Text = "";
        txtAvgSlip.Text = "";
    }

    protected void ArrivalSummary_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EOSP"))
            {
                Response.Redirect("../VesselPosition/VesselPositionArrivalReportEdit.aspx", true);
            }
            if (CommandName.ToUpper().Equals("MRVSUMMARY"))
            {
                Response.Redirect("../VesselPosition/VesselPositionArrivalReportMRVSummary.aspx");
            }
            if (CommandName.ToUpper().Equals("ARRIVALREPORT"))
            {
                if (Filter.CurrentNoonReportLaunchFrom != null && Filter.CurrentNoonReportLaunchFrom == "ST")
                    Response.Redirect("VesselPositionReports.aspx", false);
                else
                    Response.Redirect("VesselPositionArrivalReport.aspx", false);
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
            return;
        }
    }
    private void UpdateArrivalReport()
    {
        if (Session["VESSELARRIVALID"] != null)
        {
            PhoenixVesselPositionArrivalReport.UpdateArrivalReportInPassageSummary(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode
                , General.GetNullableGuid(Session["VESSELARRIVALID"].ToString())
                , General.GetNullableInteger(ViewState["VESSELID"].ToString())
                , txtSuptRemarks.Text);

            ucStatus.Text = "Passage Summary updated.";
        }
    }


    protected void gvConsumption_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (e.Item is GridEditableItem)
            {
                LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                {
                    cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/VesselPosition/VesselPositionArrivalReportPassageSummaryOilBreakup.aspx?vesselid="
                           + drv["FLDVESSELID"].ToString() + "&oiltype=" + drv["FLDOILTYPECODE"].ToString() + "'); return false;");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private void ShowNoRecordsFound(DataTable dt, GridView gv)
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
        gv.Rows[0].Attributes["onclick"] = "";
    }

    private bool IsValidArrivalReport(string port, string voyageid, string arrivaldate)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(voyageid) == null)
            ucError.ErrorMessage = "Voyage is required.";

        if (General.GetNullableInteger(port) == null)
            ucError.ErrorMessage = "Port is required.";

        if (General.GetNullableDateTime(arrivaldate) == null)
            ucError.ErrorMessage = "Arrival Date is required.";

        return (!ucError.IsError);
    }

    protected void cmdHiddenPick_Click(object sender, EventArgs e)
    {
        gvConsumptionRebind();
    }
    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    // Fresh Water..

    protected void gvFW_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (e.Item is GridEditableItem)
            {
                LinkButton cmdEdit = (LinkButton)e.Item.FindControl("cmdEdit");
                if (cmdEdit != null)
                {
                    cmdEdit.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/VesselPosition/VesselPositionArrivalReportPassageSummaryWaterBreakup.aspx?vesselid="
                           + drv["FLDVESSELID"].ToString() + "&oiltype=" + drv["FLDOILTYPECODE"].ToString() + "'); return false;");
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
    private string PrepareHtmlDoc()
    {
        VesselArrivalEdit();

        StringBuilder DsHtmlcontent = new StringBuilder();
        if (dsDeatil.Tables[0].Rows.Count > 0)
        {
            DataRow dr1 = dsDeatil.Tables[0].Rows[0];
            DataRow dr2 = dsVoyage.Tables[0].Rows[0];

            DsHtmlcontent.Append("<html><table><tr><td align=\"center\"><b>ARRIVAL REPORT PASSAGE SUMMARY</b></td></tr></table><br />");

            DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='3' cellspacing='0' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\" >Vessel</td><td colspan=\"4\">" + dr1["FLDVESSELNAME"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Voyage </td><td colspan=\"4\">" + dr1["FLDVOYAGENO"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">From Port </td><td colspan=\"4\">" + dr1["FLDFROMSEAPORTNAME"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">To Port </td><td colspan=\"4\">" + dr1["FLDTOSEAPORTNAME"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Full Speed Total</td><td colspan=\"4\">" + dr1["FLDFULLSPEED"].ToString() + "   hrs</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Reduced Speed Total </td><td colspan=\"4\">" + dr1["FLDREDUCEDSPEED"].ToString() + "   hrs</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Stopped</td><td colspan=\"4\">" + dr1["FLDPSSTOPPED"].ToString() + "   hrs</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Manoeuvering </td><td colspan=\"4\">" + dr1["FLDMANOEVERING"].ToString() + "   hrs</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Total Distance Observed </td><td colspan=\"4\">" + dr1["FLDDISTOBSERVED"].ToString() + "   nm</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Total Engine Distance</td><td colspan=\"4\">" + dr1["FLDENGINEDISTANCE"].ToString() + "   nm</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Total Manoeuvering Distance</td><td colspan=\"4\">" + dr1["FLDMANOEVERINGDIST"].ToString() + "   nm</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Avg Speed</td><td colspan=\"4\">" + dr1["FLDAVGSPEED"].ToString() + "   kts</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Avg Slip</td><td colspan=\"4\">" + dr1["FLDAVGSLIP"].ToString() + "   %</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Avg RPM </td><td colspan=\"4\">" + dr2["FLDAVGRPMOVERALL"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">EEOI</td><td colspan=\"4\">" + dr1["FLDPSEEOI"].ToString() + "   g-CO2/T-nm</td></tr>");
            DsHtmlcontent.Append("</table>");


            if (dsFWconsumption.Tables[1].Rows.Count > 0)
            {
                DataTable t1 = new DataTable();
                t1 = dsFWconsumption.Tables[1];
                DsHtmlcontent.Append("<br /><br /><br /><br /><br /><br /><br />");

                DsHtmlcontent.Append("<div class = 'mystyle' align='left'>");
                DsHtmlcontent.Append("<font size='9px' face='Helvetica'>");
                DsHtmlcontent.Append("<table ID='tbl1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
                DsHtmlcontent.Append("<font color='white' size=14px ><tr><td height='9' align='center'><b>Fresh Water</b></td></tr></font>");
                DsHtmlcontent.Append("</table>");

                DsHtmlcontent.Append("<table ID=\"tbl2\" border ='0.5'  opacity='0.5' cellpadding=\"7\" cellspacing='0' style='border:red 1px solid'>");
                DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' colspan='2'><b>Fresh Water</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>ROB on Prev Arr</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Received</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Cons in Port</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>ROB on Dep</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Produced</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>ROB on Arr</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Cons at Sea</b></td></tr>");

                if (t1.Rows.Count > 0)
                {
                    foreach (DataRow dr in t1.Rows)
                    {
                        DsHtmlcontent.Append("<tr>");//colspan='2'
                        DsHtmlcontent.Append("<td colspan='2' >" + dr["FLDOILTYPENAME"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDROBONARR"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDBUNKERED"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCONSINPORT"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDROBONDEP"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDPRODUCED"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDROB"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCONSATSEA"].ToString() + "</td>");
                        DsHtmlcontent.Append("</tr>");//colspan='2'
                    }
                }

                DsHtmlcontent.Append("</table>");
            }

            if (dsOilconsumption.Tables[0].Rows.Count > 0)
            {
                DataTable t1 = new DataTable();
                t1 = dsOilconsumption.Tables[0];
                DsHtmlcontent.Append("<br />");

                DsHtmlcontent.Append("<div class = 'mystyle' align='left'>");
                DsHtmlcontent.Append("<font size='9px' face='Helvetica'>");
                DsHtmlcontent.Append("<table ID='tbl1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
                DsHtmlcontent.Append("<font color='white' size=14px ><tr><td height='9' align='center'><b>Consumption</b></td></tr></font>");
                DsHtmlcontent.Append("</table>");

                DsHtmlcontent.Append("<table ID=\"tbl2\" border ='0.5'  opacity='0.5' cellpadding=\"7\" cellspacing='0' style='border:red 1px solid'>");
                DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' colspan='2'><b>Item</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>ROB @ COSP</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>ROB @ FWE</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Total Cons</b></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Avg Cons/day</b></td></tr>");

                if (t1.Rows.Count > 0)
                {
                    foreach (DataRow dr in t1.Rows)
                    {
                        DsHtmlcontent.Append("<tr>");//colspan='2'
                        DsHtmlcontent.Append("<td colspan='2' >" + dr["FLDOILTYPENAME"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDROBCOSP"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDROBFWE"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDTOTALCONS"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAVGCONSUMPTIONQTY"].ToString() + "</td>"); ;
                        DsHtmlcontent.Append("</tr>");//colspan='2'
                    }
                }

                DsHtmlcontent.Append("</table>");
            }

            DsHtmlcontent.Append("</html>");
        }

        return DsHtmlcontent.ToString();
    }

    private string PrepareHtmlDocVoyageSummary()
    {
        VesselArrivalEdit();

        StringBuilder DsHtmlcontent = new StringBuilder();

        string fmt = "#,##0.00;(#,##0.00)";

        if (dsVoyage.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsVoyage.Tables[0].Rows[0];


            DsHtmlcontent.Append("<html><table><tr><td align=\"center\"><b>ARRIVAL REPORT PASSAGE SUMMARY</b></td></tr></table><br />");

            DsHtmlcontent.Append("<table ID='tbl1' border='1' cellpadding='3' cellspacing='0' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Vessel</td><td colspan=\"4\">" + dr["FLDVESSELNAME"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Voyage Number</td><td colspan=\"4\" align=\"Left\">" + dr["FLDVOYAGENUMBER"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Company</td><td colspan=\"4\">" + dr["FLDCOMPANY"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">From Port </td><td colspan=\"4\">" + dr["FLDFROMPORT"] + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">To Port </td><td colspan=\"4\">" + dr["FLDTOPORT"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Ballast/Laden</td><td colspan=\"4\">" + dr["FLDBALLASTLADEN"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">COSP </td><td colspan=\"4\" align=\"Left\">" + DateTime.Parse(dr["FLDCOSP"].ToString()).ToString("dd/MM/yyyy hh:mm") + "&nbsp;</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">EOSP</td><td colspan=\"4\" align=\"Left\">" + DateTime.Parse(dr["FLDEOSP"].ToString()).ToString("dd/MM/yyyy hh:mm") + "&nbsp;</td></tr>");
            DsHtmlcontent.Append("</table>");

            DsHtmlcontent.Append("<br />");


            DsHtmlcontent.Append("<table><tr><td align=\"center\"><b>Voyage Summary</b></td></tr></table><br />");

            DsHtmlcontent.Append("<table ID=\"tbl2\" border ='1'  opacity='0.5' cellpadding=\"7\" cellspacing='0'>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b></b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Bad Weather</b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Good Weather</b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>Overall</b></td>");
            DsHtmlcontent.Append("</tr>");

            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Distance Sailed (nm)</b></td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDBADWEATHERDIST"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDGOODWEATHERDIST"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDOVERALLDIST"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Time En Route (hr)</b></td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDBADENROUTE"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDGOODENROUTE"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDOVERALLENROUTE"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Average Speed (kts)</b></td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDBADSPEED"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDGOODSPEED"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDOVERALLSPEED"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Predicted Time (As per CP) (hr)</b></td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDPREDICTEDTIMEBAD"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDPREDICTEDTIMEGOOD"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDPREDICTEDTIMEOVERALL"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Time (Loss)/Gain in Good WX (hr)</b></td>");

            if (General.GetNullableDecimal(dr["FLDBADWEATHERLOSSORGAIN"].ToString()) < 0)
            {
                DsHtmlcontent.Append("<td align=\"Right\" style='color:Red;'>&nbsp;(" + (-1 * Decimal.Parse(dr["FLDBADWEATHERLOSSORGAIN"].ToString())).ToString(fmt) + ")</td>");
            }
            else
                DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDBADWEATHERLOSSORGAIN"].ToString() + "</td>");

            if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) < 0)
            {
                DsHtmlcontent.Append("<td align=\"Right\" style='color:Red;'>&nbsp;(" + (-1 * Decimal.Parse(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString())).ToString(fmt) + ")</td>");
            }
            else
                DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString() + "</td>");

            if (General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN"].ToString()) < 0)
            {
                DsHtmlcontent.Append("<td align=\"Right\" style='color:Red;'>&nbsp;(" + (-1 * Decimal.Parse(dr["FLDOVERALLTIMELOSSORGAIN"].ToString())).ToString(fmt) + ")</td>");
            }
            else
                DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDOVERALLTIMELOSSORGAIN"].ToString() + "</td>");

            //DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDOVERALLTIMELOSSORGAIN"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Actual Good WX ME FOC in Voy (mt)</b></td>");
            //DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDFOCBAD"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">  </td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDFOCGOOD"].ToString() + "</td>");
            //DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDFOCOVERALL"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\"> </td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Predicted ME FOC in Voy (mt)</b></td>");
            //DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDPREDICTFOCBAD"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\"> </td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDPREDICTFOCGOOD"].ToString() + "</td>");
            //DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDPREDICTFOCOVERALL"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\"> </td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Good WX (Loss) or Gain in FOC (mt)</b></td>");

            if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCBAD"].ToString()) < 0)
            {
                //DsHtmlcontent.Append("<td align=\"Right\" style='color:Red;'>&nbsp;(" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCBAD"].ToString()) + ")</td>");
                DsHtmlcontent.Append("<td align=\"Right\" style='color:Red;'> </td>");
            }
            else
                DsHtmlcontent.Append("<td align=\"Right\"> </td>");
            //DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLOSSORGAINFOCBAD"].ToString() + "</td>");

            if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) < 0)
            {
                DsHtmlcontent.Append("<td align=\"Right\" style='color:Red;'>&nbsp;(" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCGOOD"].ToString()) + ")</td>");
            }
            else
                DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLOSSORGAINFOCGOOD"].ToString() + "</td>");

            DsHtmlcontent.Append("<td align=\"Right\"> </td>");
            //if (General.GetNullableDecimal(dr["FLDLOSSORGAINFOCOVERALL"].ToString()) < 0)
            //{
            //    DsHtmlcontent.Append("<td align=\"Right\" style='color:Red;'>&nbsp;(" + -1 * General.GetNullableDecimal(dr["FLDLOSSORGAINFOCOVERALL"].ToString()) + ")</td>");
            //}
            //else
            //    DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDLOSSORGAINFOCOVERALL"].ToString() + "</td>");

            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Avg. RPM</b></td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAVGRPMBAD"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAVGRPMGOOD"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAVGRPMOVERALL"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");

            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Avg. BHP</b></td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAVGBHPBAD"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAVGBHPGOOD"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAVGBHPOVERALL"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");

            DsHtmlcontent.Append("</table>");

            DsHtmlcontent.Append("<br />");

            DsHtmlcontent.Append("<table><tr><td align=\"center\"><b>Time Analysis Based in Good Weather</b></td></tr></table><br />");

            DsHtmlcontent.Append("<table ID=\"tbl3\" border ='1'  opacity='0.5' cellpadding=\"7\" cellspacing='0'>");

            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>CP Speed</b></td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCHARTERPARTYSPEED"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Predicted Time</b></td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDPREDICTEDTIMEGOOD"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Time (Loss)/Gain in Good WX (hr)</b></td>");

            if (General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) < 0)
            {
                DsHtmlcontent.Append("<td align=\"Right\" style='color:Red;'>&nbsp;(" + -1 * General.GetNullableDecimal(dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString()) + ")</td>");
            }
            else
                DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDTIMELOSSORGAININGOODWEATHER"].ToString() + "</td>");

            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Overall Time (loss)/gain</b></td>");

            if (General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) < 0)
            {
                DsHtmlcontent.Append("<td align=\"Right\" style='color:Red;'>&nbsp;(" + -1 * General.GetNullableDecimal(dr["FLDOVERALLTIMELOSSORGAIN2"].ToString()) + ")</td>");
            }
            else
                DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDOVERALLTIMELOSSORGAIN2"].ToString() + "</td>");

            DsHtmlcontent.Append("</tr>");


            DsHtmlcontent.Append("</table>");


            DsHtmlcontent.Append("<br />");

            DsHtmlcontent.Append("<table><tr><td align=\"center\"><b>Bunker Analysis</b></td></tr></table><br />");

            DsHtmlcontent.Append("<table ID=\"tbl4\" border ='1'  opacity='0.5' cellpadding=\"7\" cellspacing='0'>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b></b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>HFO</b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1'><b>MDO</b></td>");
            DsHtmlcontent.Append("</tr>");

            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Daily CP Allowance</b></td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCPHFO"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCPMDO"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Actual Good WX Consumption</b></td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDACTHFOCONSGOOD"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDACTMDOCONSGOOD"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Good WX Avg. Daily Consumption</b></td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAVGHFOCONSGOOD"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAVGMDOCONSGOOD"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Overall Avg. Daily Consumption </b></td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAVGHFOCONSOVERALL"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAVGMDOCONSOVERALL"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Allowed Good WX Consumption</b></td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDALLOWEDGOODWXHFOCONS"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDALLOWEDGOODWXMDOCONS"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Good WX (over)/under consumption</b></td>");

            if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) < 0)
            {
                DsHtmlcontent.Append("<td align=\"Right\" style='color:Red;'>&nbsp;(" + -1 * General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERHFOCONS"].ToString()) + ")</td>");
            }
            else
                DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDGOODWXUNDEROVERHFOCONS"].ToString() + "</td>");

            if (General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) < 0)
            {
                DsHtmlcontent.Append("<td align=\"Right\" style='color:Red;'>&nbsp;(" + -1 * General.GetNullableDecimal(dr["FLDGOODWXUNDEROVERMDOCONS"].ToString()) + ")</td>");
            }
            else
                DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDGOODWXUNDEROVERMDOCONS"].ToString() + "</td>");

            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" bgcolor='#f1f1f1'><b>Overall (over)/under Consumption</b></td>");

            if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) < 0)
            {
                DsHtmlcontent.Append("<td align=\"Right\" style='color:Red;'>&nbsp;(" + -1 * General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString()) + ")</td>");
            }
            else
                DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDOVERALLWXUNDEROVERHFOCONS"].ToString() + "</td>");

            if (General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) < 0)
            {
                DsHtmlcontent.Append("<td align=\"Right\" style='color:Red;'>&nbsp;(" + -1 * General.GetNullableDecimal(dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString()) + ")</td>");
            }
            else
                DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDOVERALLWXUNDEROVERMDOCONS"].ToString() + "</td>");

            DsHtmlcontent.Append("</tr>");

            DsHtmlcontent.Append("</table>");

            DsHtmlcontent.Append("<br /><br />");

            DsHtmlcontent.Append("<table ID=\"tbl4\" border ='1'  opacity='0.5' cellpadding=\"7\" cellspacing='0'>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' colspan='1'><b></b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1' colspan='4' align=\"Center\"><b>ROB</b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1' colspan='4'><b></b></td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' colspan='1'><b></b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1' colspan='2' align=\"Center\"><b>Departure (COSP)</b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1' colspan='2' align=\"Center\"><b>Arrival (EOSP)</b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1' colspan='4' align=\"Center\"><b>Consumed</b></td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1'><b>From - To</b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1' align=\"Center\"><b>HFO</b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1' align=\"Center\"><b>MDO</b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1' align=\"Center\"><b>HFO</b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1' align=\"Center\"><b>MDO</b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1' align=\"Center\"><b>HFO</b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1' align=\"Center\"><b>MDO</b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1' align=\"Center\"><b>ME</b></td>");
            DsHtmlcontent.Append("<td bgcolor='#f1f1f1' align=\"Center\"><b>AE</b></td>");
            DsHtmlcontent.Append("</tr>");

            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\"> Voy : " + dr["FLDFROMPORT"].ToString() + " - " + dr["FLDTOPORT"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDHFOROBONDEP"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMDOROBONDEP"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDHFOROBONARR"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMDOROBONARR"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDHFOCONS"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMDOCONS"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMEOVERALLCONS"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAEOVERALLCONS"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");
            DsHtmlcontent.Append("<tr>");
            DsHtmlcontent.Append("<td align=\"Left\" colspan='5' bgcolor='#f1f1f1'><b>Total</b></td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDHFOCONS"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMDOCONS"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDMEOVERALLCONS"].ToString() + "</td>");
            DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDAEOVERALLCONS"].ToString() + "</td>");
            DsHtmlcontent.Append("</tr>");

            DsHtmlcontent.Append("</table>");

            DsHtmlcontent.Append("</html>");
        }

        return DsHtmlcontent.ToString();
    }
    public void ConvertToPdf(string HTMLString)
    {
        try
        {
            if (HTMLString != "")
            {
                using (var ms = new MemoryStream())
                {
                    iTextSharp.text.Document document = new iTextSharp.text.Document(new iTextSharp.text.Rectangle(595f, 842f));
                    document.SetMargins(36f, 36f, 36f, 0f);
                    document.SetPageSize(iTextSharp.text.PageSize.LEGAL.Rotate());
                    string filefullpath = "PassageSummary" + ".pdf";
                    PdfWriter.GetInstance(document, ms);
                    document.Open();

                    StyleSheet styles = new StyleSheet();
                    styles.LoadStyle(".headertable td", "background-color", "Blue");
                    ArrayList htmlarraylist = (ArrayList)iTextSharp.text.html.simpleparser.HTMLWorker.ParseToList(new StringReader(HTMLString), styles);

                    for (int k = 0; k < htmlarraylist.Count; k++)
                    {
                        document.Add((iTextSharp.text.IElement)htmlarraylist[k]);

                    }
                    document.Close();
                    Response.Buffer = true;
                    var bytes = ms.ToArray();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", "attachment; filename=" + filefullpath);
                    Response.OutputStream.Write(bytes, 0, bytes.Length);
                    Response.Flush();
                    Response.End();
                }
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void ConvertToExcel(string HTMLString)
    {
        try
        {
            if (HTMLString != "")
            {
                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=" + txtVessel.Text + "_" + txtVoyage.Text + "_PassageSummary.xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                Response.Output.Write(HTMLString);
                Response.Flush();
                Response.End();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvConsumption_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        DataSet ds = PhoenixVesselPositionArrivalOilConsumption.ListArrivalOilConsumption(
            General.GetNullableInteger(ViewState["VESSELID"].ToString())
            , General.GetNullableGuid(Session["VESSELARRIVALID"] == null ? "" : Session["VESSELARRIVALID"].ToString()));

        dsOilconsumption = ds;
        gvConsumption.DataSource = ds;

    }
    protected void gvConsumptionRebind()
    {
        gvConsumption.SelectedIndexes.Clear();
        gvConsumption.EditIndexes.Clear();
        gvConsumption.DataSource = null;
        gvConsumption.Rebind();
    }

    protected void gvFW_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        ds = PhoenixVesselPositionArrivalOilConsumption.ListArrivalOilConsumption(
            General.GetNullableInteger(ViewState["VESSELID"].ToString())
            , General.GetNullableGuid(Session["VESSELARRIVALID"] == null ? "" : Session["VESSELARRIVALID"].ToString()));

        dsFWconsumption = ds;
        gvFW.DataSource = ds.Tables[1];

    }
    protected void gvFWRebind()
    {
        gvConsumption.SelectedIndexes.Clear();
        gvConsumption.EditIndexes.Clear();
        gvConsumption.DataSource = null;
        gvConsumption.Rebind();
    }
}
