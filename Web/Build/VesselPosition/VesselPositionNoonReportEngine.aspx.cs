using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.VesselPosition;
using SouthNests.Phoenix.Registers;
using System.Text;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Collections;
using Telerik.Web.UI;

public partial class VesselPositionNoonReportEngine : PhoenixBasePage
{
    DataSet dsDeatil = new DataSet();
    DataSet dsoilcounsumption = new DataSet();
    DataSet dsOtheroilcounsumption = new DataSet();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            PhoenixToolbar toolbarsubtap = new PhoenixToolbar();
            toolbarsubtap.AddButton("List", "NOONREPORTLIST", ToolBarDirection.Left);
            toolbarsubtap.AddButton("Deck Dept", "DECK",ToolBarDirection.Left);
            toolbarsubtap.AddButton("Engine Dept", "ENGINE", ToolBarDirection.Left);
            MenuNRSubTab.AccessRights = this.ViewState;
            MenuNRSubTab.MenuList = toolbarsubtap.Show();
            MenuNRSubTab.SelectedMenuIndex = 2;

            if (!IsPostBack)
            {
                ViewState["VesselStatus"] = "";
                
                ViewState["REPORTSTATUS"] = "";
                ViewState["PREVIOUSREPORTSTATUS"] = "";
                ViewState["DEBUNKER"] = 0;
                ucSludgeTankROBAlert.Position = ToolTipPosition.TopCenter;
                ucSludgeTankROBAlert.TargetControlId = lblmSludgeTankROB.ClientID;

                BindData();
                SetFieldRange();
            }

            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbarNoonReporttap = new PhoenixToolbar();
            toolbarNoonReporttap.AddButton("Export PDF", "PDF", ToolBarDirection.Right);
            if (ViewState["REPORTSTATUS"] != null && ViewState["REPORTSTATUS"].ToString() != "1")
            {
                toolbarNoonReporttap.AddButton("Copy", "COPY", ToolBarDirection.Right);
                toolbarNoonReporttap.AddButton("Save", "SAVE", ToolBarDirection.Right);
            }
            
            MenuNewSaveTabStrip.AccessRights = this.ViewState;
            MenuNewSaveTabStrip.MenuList = toolbarNoonReporttap.Show();
            ScriptManager.GetCurrent(this).RegisterPostBackControl(MenuNewSaveTabStrip);
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void Reset()
    {
        if (ViewState["VESSELID"].ToString() != "")
        {
            Guid? noonreportid = null;

            if (Session["NOONREPORTID"] != null)
                noonreportid = General.GetNullableGuid(Session["NOONREPORTID"].ToString());

            DataSet ds = PhoenixVesselPositionNoonReport.ResetNoonReport(int.Parse(ViewState["VESSELID"].ToString()), noonreportid);

            if (ds.Tables[0].Rows.Count > 0)
            {
                DataTable dt = ds.Tables[0];

                ViewState["OLDNOONREPORTID"] = dt.Rows[0]["FLDNOONREPORTID"].ToString();

               // txtEngineDistance.Text = dt.Rows[0]["FLDENGINEDISTANCE"].ToString();
                txtSwellTemp.Text = dt.Rows[0]["FLDSWTEMP"].ToString();
                txtERExhTemp.Text = dt.Rows[0]["FLDERTEMP"].ToString();
                txtRemarksCE.Text = dt.Rows[0]["FLDREMARKSCE"].ToString();
                txtGovernorSetting.Text = dt.Rows[0]["FLDGOVERNORSETTING"].ToString();
                txtSpeedSetting.Text = dt.Rows[0]["FLDSPEEDSETTING"].ToString();
                txtFOInletTemp.Text = dt.Rows[0]["FLDFOINLETTEMP"].ToString();
                txtSWPress.Text = dt.Rows[0]["FLDSWPRESS"].ToString();
                txtFuelOilPress.Text = dt.Rows[0]["FLDFUELOILPRESS"].ToString();
                txtBilgeROB.Text = dt.Rows[0]["FLDBILGETANKROB"].ToString();
                txtSludgeROB.Text = dt.Rows[0]["FLDSLUDGETANKROB"].ToString();
                txtMERPM.Text = dt.Rows[0]["FLDMERPM"].ToString();
                txtBoilerWaterChlorides.Text = dt.Rows[0]["FLDBOILERCHLORIDES"].ToString();
                txtHFOTankCleaning.Text = dt.Rows[0]["FLDHFOTANKCLEANING"].ToString();
                txtGeneratorLoadAE1.Text = string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE1"]));
                txtGeneratorLoadAE2.Text = string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE2"]));
                txtGeneratorLoadAE3.Text = string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE3"]));
                txtGeneratorLoadAE4.Text = string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE4"]));
                txtHFOCargoHeating.Text = dt.Rows[0]["FLDHFOCARGOHEATING"].ToString();

                txtFOCatFines.Text = dt.Rows[0]["FLDFOCATFINES"].ToString();
                txthfopfr1.Text = dt.Rows[0]["FLDHFOPFR1HRS"].ToString();
                txthfopfr2.Text = dt.Rows[0]["FLDHFOPFR2HRS"].ToString();
                txtdopfr.Text = dt.Rows[0]["FLDDOPFRHRS"].ToString();
                txtmelopfr.Text = dt.Rows[0]["FLDMELOPFRHRS"].ToString();
                txtaelopfr.Text = dt.Rows[0]["FLDAELOPFRHRS"].ToString();
                txtMEFOAUTOBACKWASHFILTER.Text = dt.Rows[0]["FLDMEFOAUTOBACKWASHFILTERCOUNTER"].ToString();
                txtMELOAUTOBACKWASHFILTER.Text = dt.Rows[0]["FLDMELOAUTOBACKWASHFILTERCOUNTER"].ToString();
                txtNOOFOPERATIONSMEFOAUTOBACKWASHFILTER.Text = dt.Rows[0]["NOOFMEFOOPERATIONS"].ToString();
                txtNOOFOPERATIONSMELOAUTOBACKWASHFILTER.Text = dt.Rows[0]["NOOFMELOOPERATIONS"].ToString();
                txtGeneratorLoadAE1OPHrs.Text = dt.Rows[0]["FLDGENERATORLOADAE1OPHRS"].ToString();
                txtGeneratorLoadAE2OPHrs.Text = dt.Rows[0]["FLDGENERATORLOADAE2OPHRS"].ToString();
                txtGeneratorLoadAE3OPHrs.Text = dt.Rows[0]["FLDGENERATORLOADAE3OPHRS"].ToString();
                txtGeneratorLoadAE4OPHrs.Text = dt.Rows[0]["FLDGENERATORLOADAE4OPHRS"].ToString();
                txtScavAirTemp.Text = dt.Rows[0]["FLDSCAVAIRTEMP"].ToString();
                txtMinExhTemp.Text = dt.Rows[0]["FLDMINEXHTEMP"].ToString();
                txtMaxExhTemp.Text = dt.Rows[0]["FLDMAXEXHTEMP"].ToString();
                txtBHP.Text = dt.Rows[0]["FLDBHP"].ToString();
                txtTCRPMInboard.Text = dt.Rows[0]["FLDTCRPMINBOARD"].ToString();
                txtTCRPMOutboard.Text = dt.Rows[0]["FLDTCRPMOUTBOARD"].ToString();
                txtExhTCInboardBefore.Text = dt.Rows[0]["FLDEXHGASTEMPTCINBOARDBEFORE"].ToString();
                txtExhTCInboardAfter.Text = dt.Rows[0]["FLDEXHGASTEMPTCINBOARDAFTER"].ToString();
                txtExhTCOutboardBefore.Text = dt.Rows[0]["FLDEXHGASTEMPTCOUTBOARDBEFORE"].ToString();
                txtExhTCOutboardAfter.Text = dt.Rows[0]["FLDEXHGASTEMPTCOUTBOARDAFTER"].ToString();
                txtScavAirPress.Text = dt.Rows[0]["FLDSCAVAIRPRESS"].ToString();
                rbtnPowerUnit.SelectedValue = General.GetNullableString(dt.Rows[0]["FLDPOWEROUTPUTUNIT"].ToString()) == null ? "BHP" : dt.Rows[0]["FLDPOWEROUTPUTUNIT"].ToString();
                txtcalculatedBHP.Text = dt.Rows[0]["FLDBHPCALCULATEDVALUE"].ToString();
                txtEarthFaultMonitor400.Text = dt.Rows[0]["FLDEARTHFAULTMONITOR400"].ToString();
                txtEarthFaultMonitor230or110.Text = dt.Rows[0]["FLDEARTHFAULTMONITOR230OR110"].ToString();
            }
            if (ViewState["VesselStatus"].ToString().Equals("INPORT") || ViewState["VesselStatus"].ToString().Equals("ATANCHOR"))
            {
                txtEngineDistance.Text = "";
                txtEngineDistance.CssClass = "readonlytextbox";
                txtEngineDistance.Enabled = false;

                txtSlip.Text = "";
                txtSlip.CssClass = "readonlytextbox";
                txtSlip.Enabled = false;

                //txtSWPress.CssClass = "readonlytextbox";
                //txtSWPress.Text = "";
                //txtSWPress.Enabled = false;

                txtMERPM.Text = "";
                txtMERPM.CssClass = "readonlytextbox";
                txtMERPM.Enabled = false;

                txtGovernorSetting.Text = "";
                txtGovernorSetting.CssClass = "readonlytextbox";
                txtGovernorSetting.Enabled = false;

                txtSpeedSetting.Text = "";
                txtSpeedSetting.CssClass = "readonlytextbox";
                txtSpeedSetting.Enabled = false;

                txtFOInletTemp.Text = "";
                txtFOInletTemp.CssClass = "readonlytextbox";
                txtFOInletTemp.Enabled = false;

                txtFuelOilPress.Text = "";
                txtFuelOilPress.CssClass = "readonlytextbox";
                txtFuelOilPress.Enabled = false;

                //txtERExhTemp.Text = "";
                //txtERExhTemp.CssClass = "readonlytextbox";
                //txtERExhTemp.Enabled = false;

                //txtSwellTemp.Text = "";
                //txtSwellTemp.CssClass = "readonlytextbox";
                //txtSwellTemp.Enabled = false;

                txtBHP.Text = "";
                txtBHP.CssClass = "readonlytextbox";
                txtBHP.Enabled = false;

                txtMaxExhTemp.Text = "";
                txtMaxExhTemp.CssClass = "readonlytextbox";
                txtMaxExhTemp.Enabled = false;

                txtMinExhTemp.Text = "";
                txtMinExhTemp.CssClass = "readonlytextbox";
                txtMinExhTemp.Enabled = false;

                txtScavAirTemp.Text = "";
                txtScavAirTemp.CssClass = "readonlytextbox";
                txtScavAirTemp.Enabled = false;

                txtScavAirPress.Text = "";
                txtScavAirPress.CssClass = "readonlytextbox";
                txtScavAirPress.Enabled = false;

                txtTCRPMInboard.Text = "";
                txtTCRPMInboard.CssClass = "readonlytextbox";
                txtTCRPMInboard.Enabled = false;

                txtTCRPMOutboard.Text = "";
                txtTCRPMOutboard.CssClass = "readonlytextbox";
                txtTCRPMOutboard.Enabled = false;

                txtExhTCInboardBefore.Text = "";
                txtExhTCInboardBefore.CssClass = "readonlytextbox";
                txtExhTCInboardBefore.Enabled = false;

                txtExhTCInboardAfter.Text = "";
                txtExhTCInboardAfter.CssClass = "readonlytextbox";
                txtExhTCInboardAfter.Enabled = false;

                txtExhTCOutboardBefore.Text = "";
                txtExhTCOutboardBefore.CssClass = "readonlytextbox";
                txtExhTCOutboardBefore.Enabled = false;

                txtExhTCOutboardAfter.Text = "";
                txtExhTCOutboardAfter.CssClass = "readonlytextbox";
                txtExhTCOutboardAfter.Enabled = false;

                //txtFOCatFines.Text = "";
                //txtFOCatFines.CssClass = "readonlytextbox";
                //txtFOCatFines.Enabled = false;

                //txtGeneratorLoadAE1.Text = "";
                //txtGeneratorLoadAE1.CssClass = "readonlytextbox";
                //txtGeneratorLoadAE1.Enabled = false;

                //txtGeneratorLoadAE2.Text = "";
                //txtGeneratorLoadAE2.CssClass = "readonlytextbox";
                //txtGeneratorLoadAE2.Enabled = false;

                //txtGeneratorLoadAE3.Text = "";
                //txtGeneratorLoadAE3.CssClass = "readonlytextbox";
                //txtGeneratorLoadAE3.Enabled = false;

                //txtGeneratorLoadAE4.Text = "";
                //txtGeneratorLoadAE4.CssClass = "readonlytextbox";
                //txtGeneratorLoadAE4.Enabled = false;

                //txtGeneratorLoadAE1OPHrs.Text = "";
                //txtGeneratorLoadAE1OPHrs.CssClass = "readonlytextbox";
                //txtGeneratorLoadAE1OPHrs.Enabled = false;

                //txtGeneratorLoadAE2OPHrs.Text = "";
                //txtGeneratorLoadAE2OPHrs.CssClass = "readonlytextbox";
                //txtGeneratorLoadAE2OPHrs.Enabled = false;

                //txtGeneratorLoadAE3OPHrs.Text = "";
                //txtGeneratorLoadAE3OPHrs.CssClass = "readonlytextbox";
                //txtGeneratorLoadAE3OPHrs.Enabled = false;

                //txtGeneratorLoadAE4OPHrs.Text = "";
                //txtGeneratorLoadAE4OPHrs.CssClass = "readonlytextbox";
                //txtGeneratorLoadAE4OPHrs.Enabled = false;

                //txthfopfr1.Text = "";
                //txthfopfr1.CssClass = "readonlytextbox";
                //txthfopfr1.Enabled = false;

                //txthfopfr2.Text = "";
                //txthfopfr2.CssClass = "readonlytextbox";
                //txthfopfr2.Enabled = false;

                //txtdopfr.Text = "";
                //txtdopfr.CssClass = "readonlytextbox";
                //txtdopfr.Enabled = false;

                //txtmelopfr.Text = "";
                //txtmelopfr.CssClass = "readonlytextbox";
                //txtmelopfr.Enabled = false;

                //txtaelopfr.Text = "";
                //txtaelopfr.CssClass = "readonlytextbox";
                //txtaelopfr.Enabled = false;

                //txtMEFOAUTOBACKWASHFILTER.Text = "";
                //txtMEFOAUTOBACKWASHFILTER.CssClass = "readonlytextbox";
                //txtMEFOAUTOBACKWASHFILTER.Enabled = false;

                //txtNOOFOPERATIONSMEFOAUTOBACKWASHFILTER.Text = "";
                //txtNOOFOPERATIONSMEFOAUTOBACKWASHFILTER.CssClass = "readonlytextbox";
                //txtNOOFOPERATIONSMEFOAUTOBACKWASHFILTER.Enabled = false;

                //txtMELOAUTOBACKWASHFILTER.Text = "";
                //txtMELOAUTOBACKWASHFILTER.CssClass = "readonlytextbox";
                //txtMELOAUTOBACKWASHFILTER.Enabled = false;

                //txtNOOFOPERATIONSMELOAUTOBACKWASHFILTER.Text = "";
                //txtNOOFOPERATIONSMELOAUTOBACKWASHFILTER.CssClass = "readonlytextbox";
                //txtNOOFOPERATIONSMELOAUTOBACKWASHFILTER.Enabled = false;

            }
        }
    }
    protected void gvConsumption_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        int? iVesselId = null;

        if (ViewState["VESSELID"].ToString() != "")
        {
            iVesselId = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        }
        else
            iVesselId = 0;

        string noonreportid = "";

        noonreportid = Session["NOONREPORTID"] == null ? (ViewState["OLDNOONREPORTID"] != null ? ViewState["OLDNOONREPORTID"].ToString() : "") : Session["NOONREPORTID"].ToString();

        if (ViewState["COPY"] != null)
            ds = PhoenixVesselPositionNoonReportOilConsumption.ListOilConsumptionReset(iVesselId, General.GetNullableGuid(noonreportid));
        else
            ds = PhoenixVesselPositionNoonReportOilConsumption.ListOilConsumption(iVesselId, General.GetNullableGuid(noonreportid), "NOON");
        dsoilcounsumption = ds;
        gvConsumption.DataSource = ds;
    }
    protected void gvConsumptionRebind()
    {
        gvConsumption.SelectedIndexes.Clear();
        gvConsumption.EditIndexes.Clear();
        gvConsumption.DataSource = null;
        gvConsumption.Rebind();
    }

    protected void NewSaveTap_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        try
        {
            if (CommandName.ToUpper().Equals("COPY"))
            {
                ViewState["COPY"] = "copy"; 
                Reset();
               // BindOilConsumption();
               // BindOtherOilConsumption();
            }

            if (CommandName.ToUpper().Equals("SAVE"))
            {
                if (ViewState["COPY"] != null)
                {
                    //if (gvOtherOilCons.EditIndex >= 0)
                    //{
                    //    ucError.ErrorMessage = "Please Cancel the Edit Mode in the Fresh Water grid to Save the Copied Details";
                    //    ucError.Visible = true;
                    //    return;
                    //}
                    //if (gvConsumption.EditIndex >= 0)
                    //{
                    //    ucError.ErrorMessage = "Please Cancel the Edit Mode in the Consumption grid to Save the Copied Details";
                    //    ucError.Visible = true;
                    //    return;
                    //}
                }

                UpdateNoonReport();
                InsertOtherOilConsumption();


                if (Session["NOONREPORTID"] != null)
                {
                    BindData();
                    SetFieldRange();
                }

                if (ViewState["COPY"] != null)
                {
                    //OilConsUpdate();
                   // OtherOilConsUpdate();
                }

                ViewState["COPY"] = null;
                gvConsumptionRebind();
                gvOtherOilConsRebind();
            }
            if (CommandName.ToUpper().Equals("PDF"))
            {
                ConvertToPdf(PrepareHtmlDoc());
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void UpdateNoonReport()
    {
        string lastlandsludgetime = txtLastLandSludgeTime.Text.Trim().Equals("__:__") ? string.Empty : txtLastLandSludgeTime.Text;
        string BilgeLandingTime = txtBilgeLandingTime.Text.Trim().Equals("__:__") ? string.Empty : txtBilgeLandingTime.Text;

        PhoenixVesselPositionNoonReport.UpdateNoonReportEngineDept(
            PhoenixSecurityContext.CurrentSecurityContext.UserCode,
            new Guid(Session["NOONREPORTID"].ToString()),
            int.Parse(ViewState["VESSELID"].ToString()),
            General.GetNullableDecimal(txtSwellTemp.Text),
            General.GetNullableDecimal(txtEngineDistance.Text),
            null, //CalculateSlip(txtEngineDistance.Text, ViewState["DistObserved"].ToString()),
            General.GetNullableDecimal(txtERExhTemp.Text),
            General.GetNullableDecimal(""),
            General.GetNullableInteger(""),
            txtRemarksCE.Text, General.GetNullableDecimal(txtGovernorSetting.Text),
            General.GetNullableDecimal(txtSpeedSetting.Text),
            General.GetNullableDecimal(txtFOInletTemp.Text), General.GetNullableDecimal(txtSWPress.Text),
            General.GetNullableDecimal(txtScavAirPress.Text), General.GetNullableDecimal(txtFuelOilPress.Text),
            General.GetNullableDecimal(""), General.GetNullableDecimal(""),
            General.GetNullableDecimal(""), General.GetNullableDecimal(""),
            General.GetNullableDecimal(""), General.GetNullableDecimal(""),
            General.GetNullableDecimal(""), General.GetNullableDecimal(""),
            General.GetNullableDecimal(txtBoilerWaterChlorides.Text), General.GetNullableDecimal(txtBilgeROB.Text),
            General.GetNullableDecimal(txtSludgeROB.Text), General.GetNullableDateTime(txtLastLandSludge.Text + " " + lastlandsludgetime),
            General.GetNullableInteger(txtLastLandingDays.Text), General.GetNullableDecimal(txtTCRPMInboard.Text),
            General.GetNullableDecimal(txtTCRPMOutboard.Text), General.GetNullableDecimal(txtExhTCInboardBefore.Text),
            General.GetNullableDecimal(txtExhTCInboardAfter.Text), General.GetNullableDecimal(txtExhTCOutboardBefore.Text),
            General.GetNullableDecimal(txtExhTCOutboardAfter.Text), General.GetNullableDecimal(""),
            General.GetNullableDecimal(txtMERPM.Text),
            General.GetNullableDateTime(txtBilgeLanding.Text + " " + BilgeLandingTime),
            General.GetNullableInteger(txtBilgeLandingDays.Text),
            General.GetNullableDecimal(txtHFOTankCleaning.Text),
            General.GetNullableDecimal(txtHFOCargoHeating.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE1.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE2.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE3.Text),
            "NOON", General.GetNullableDecimal(txtFOCatFines.Text),
            General.GetNullableDecimal(txthfopfr1.Text),
            General.GetNullableDecimal(txthfopfr2.Text),
            General.GetNullableDecimal(txtdopfr.Text),
            General.GetNullableDecimal(txtmelopfr.Text),
            General.GetNullableDecimal(txtaelopfr.Text),
            General.GetNullableInteger(txtMEFOAUTOBACKWASHFILTER.Text),
            General.GetNullableInteger(txtMELOAUTOBACKWASHFILTER.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE4.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE1OPHrs.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE2OPHrs.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE3OPHrs.Text),
            General.GetNullableDecimal(txtGeneratorLoadAE4OPHrs.Text),
            General.GetNullableDecimal(txtBHP.Text),
            General.GetNullableDecimal(txtMaxExhTemp.Text), 
            General.GetNullableDecimal(txtMinExhTemp.Text),
            General.GetNullableDecimal(txtScavAirTemp.Text),
            General.GetNullableDecimal(txtrevcounter.Text),
            chkRevCounterDefective.Checked==true? 1 : 0,
            General.GetNullableDecimal(txtSulphur.Text),
            General.GetNullableDecimal(txtDensity.Text),
            chkFOCounterDefective.Checked==true? 1 : 0,
            chkLOCounterDefective.Checked==true? 1 : 0,
            General.GetNullableDecimal(txtNOOFOPERATIONSMEFOAUTOBACKWASHFILTER.Text),
            General.GetNullableDecimal(txtNOOFOPERATIONSMELOAUTOBACKWASHFILTER.Text),
            General.GetNullableString(rbtnPowerUnit.SelectedValue.ToString()),
            General.GetNullableDecimal(txtEarthFaultMonitor400.Text),
            General.GetNullableDecimal(txtEarthFaultMonitor230or110.Text)
          );

        ucStatus.Text = "Noon Report updated.";
    }

    protected void MenuNRSubTab_TabStripCommand(object sender, EventArgs e)
    {
        RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
        string CommandName = ((RadToolBarButton)dce.Item).CommandName;

        if (CommandName.ToUpper().Equals("DECK"))
        {
            Response.Redirect("VesselPositionNoonReport.aspx", false);
        }
        if (CommandName.ToUpper().Equals("NOONREPORTLIST"))
        {
            if (Filter.CurrentNoonReportLaunchFrom != null && Filter.CurrentNoonReportLaunchFrom == "ST")
                Response.Redirect("VesselPositionReports.aspx", false);
            else
                Response.Redirect("VesselPositionNoonReportList.aspx", false);
        }
    }

    private void BindData()
    {
        string noonreportid = Session["NOONREPORTID"].ToString();
        DataSet ds = PhoenixVesselPositionNoonReport.EditNoonReport(General.GetNullableGuid(noonreportid));
        dsDeatil = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            DataTable dt = ds.Tables[0];
            ViewState["VESSELID"] = dt.Rows[0]["FLDVESSELID"].ToString();
            ViewState["VOYAGEID"] = dt.Rows[0]["FLDVOYAGEID"].ToString();
            ViewState["REPORTSTATUS"] =  dt.Rows[0]["FLDCONFIRMEDYN"].ToString();

            txtEngineDistance.Text = dt.Rows[0]["FLDENGINEDISTANCE"].ToString();
            txtSwellTemp.Text = dt.Rows[0]["FLDSWTEMP"].ToString();
            txtERExhTemp.Text = dt.Rows[0]["FLDERTEMP"].ToString();
            txtSlip.Text = string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDSLIP"]));
            txtRemarksCE.Text = dt.Rows[0]["FLDREMARKSCE"].ToString();

            txtGovernorSetting.Text = dt.Rows[0]["FLDGOVERNORSETTING"].ToString();
            txtSpeedSetting.Text = dt.Rows[0]["FLDSPEEDSETTING"].ToString();
            txtFOInletTemp.Text = dt.Rows[0]["FLDFOINLETTEMP"].ToString();
            txtSWPress.Text = dt.Rows[0]["FLDSWPRESS"].ToString();
            txtFuelOilPress.Text = dt.Rows[0]["FLDFUELOILPRESS"].ToString();
            txtBoilerWaterChlorides.Text = dt.Rows[0]["FLDBOILERCHLORIDES"].ToString();
            txtBilgeROB.Text = dt.Rows[0]["FLDBILGETANKROB"].ToString();
            txtSludgeROB.Text = dt.Rows[0]["FLDSLUDGETANKROB"].ToString();
            txtLastLandSludge.Text = dt.Rows[0]["FLDLASTLANDINGSLUDGE"].ToString();
            txtLastLandingDays.Text = dt.Rows[0]["FLDDAYSFROMLASTLANDING"].ToString();
            txtLastLandSludgeTime.Text = string.Format("{0:HH:mm}", dt.Rows[0]["FLDLASTLANDINGSLUDGE"]);

            txtMERPM.Text = dt.Rows[0]["FLDMERPM"].ToString();
            txtBilgeLandingDays.Text = dt.Rows[0]["FLDDAYSLASTLANDINGBILGE"].ToString();
            txtHFOTankCleaning.Text = dt.Rows[0]["FLDHFOTANKCLEANING"].ToString();
            txtBilgeLanding.Text = General.GetDateTimeToString(dt.Rows[0]["FLDLASTLANDINGBILGE"].ToString());
            txtLastLandSludgeTime.Text = String.Format("{0:HH:mm}", dt.Rows[0]["FLDLASTLANDINGSLUDGE"]);
            txtBilgeLandingTime.Text = String.Format("{0:HH:mm}", dt.Rows[0]["FLDLASTLANDINGBILGE"]);

            txtGeneratorLoadAE1.Text= string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE1"]));
            txtGeneratorLoadAE2.Text= string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE2"]));
            txtGeneratorLoadAE3.Text= string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE3"]));
            txtGeneratorLoadAE4.Text= string.Format(String.Format("{0:#####.00}", dt.Rows[0]["FLDGENERATORLOADAE4"]));
            txtHFOCargoHeating.Text = dt.Rows[0]["FLDHFOCARGOHEATING"].ToString();
            txtFOCatFines.Text =dt.Rows[0]["FLDFOCATFINES"].ToString() ;
            txthfopfr1.Text = dt.Rows[0]["FLDHFOPFR1HRS"].ToString();
            txthfopfr2.Text = dt.Rows[0]["FLDHFOPFR2HRS"].ToString();
            txtdopfr.Text = dt.Rows[0]["FLDDOPFRHRS"].ToString();
            txtmelopfr.Text = dt.Rows[0]["FLDMELOPFRHRS"].ToString();
            txtaelopfr.Text = dt.Rows[0]["FLDAELOPFRHRS"].ToString();
            txtMEFOAUTOBACKWASHFILTER.Text = dt.Rows[0]["FLDMEFOAUTOBACKWASHFILTERCOUNTER"].ToString();
            txtMELOAUTOBACKWASHFILTER.Text = dt.Rows[0]["FLDMELOAUTOBACKWASHFILTERCOUNTER"].ToString();
            txtNOOFOPERATIONSMEFOAUTOBACKWASHFILTER.Text = dt.Rows[0]["NOOFMEFOOPERATIONS"].ToString();
            txtNOOFOPERATIONSMELOAUTOBACKWASHFILTER.Text = dt.Rows[0]["NOOFMELOOPERATIONS"].ToString();
            chkFOCounterDefective.Checked = dt.Rows[0]["FLDMEFOAUTOBWFILTERHRDEFECTIVEYN"].ToString().Equals("1") ? true : false;
            chkLOCounterDefective.Checked = dt.Rows[0]["FLDMELOAUTOBWFILTERHRDEFECTIVEYN"].ToString().Equals("1") ? true : false;
            txtGeneratorLoadAE1OPHrs.Text = dt.Rows[0]["FLDGENERATORLOADAE1OPHRS"].ToString();
            txtGeneratorLoadAE2OPHrs.Text = dt.Rows[0]["FLDGENERATORLOADAE2OPHRS"].ToString();
            txtGeneratorLoadAE3OPHrs.Text = dt.Rows[0]["FLDGENERATORLOADAE3OPHRS"].ToString();
            txtGeneratorLoadAE4OPHrs.Text = dt.Rows[0]["FLDGENERATORLOADAE4OPHRS"].ToString();
            txtScavAirTemp.Text = dt.Rows[0]["FLDSCAVAIRTEMP"].ToString();
            txtMinExhTemp.Text = dt.Rows[0]["FLDMINEXHTEMP"].ToString();
            txtMaxExhTemp.Text = dt.Rows[0]["FLDMAXEXHTEMP"].ToString();
            txtBHP.Text = dt.Rows[0]["FLDBHP"].ToString();
            txtTCRPMInboard.Text = dt.Rows[0]["FLDTCRPMINBOARD"].ToString();
            txtTCRPMOutboard.Text = dt.Rows[0]["FLDTCRPMOUTBOARD"].ToString();
            txtExhTCInboardBefore.Text = dt.Rows[0]["FLDEXHGASTEMPTCINBOARDBEFORE"].ToString();
            txtExhTCInboardAfter.Text = dt.Rows[0]["FLDEXHGASTEMPTCINBOARDAFTER"].ToString();
            txtExhTCOutboardBefore.Text = dt.Rows[0]["FLDEXHGASTEMPTCOUTBOARDBEFORE"].ToString();
            txtExhTCOutboardAfter.Text = dt.Rows[0]["FLDEXHGASTEMPTCOUTBOARDAFTER"].ToString();
            txtScavAirPress.Text = dt.Rows[0]["FLDSCAVAIRPRESS"].ToString();
            txtrevcounter.Text = dt.Rows[0]["FLDMEREVCOUNTER"].ToString();
            ViewState["VesselStatus"] = dt.Rows[0]["FLDVESSELSTATUS"].ToString();

            txtFOConsPerday.Text = dt.Rows[0]["FLDHFOOILCONSUMPTIONQTY"].ToString();
            txtDOConsPerday.Text = dt.Rows[0]["FLDMDOOILCONSUMPTIONQTY"].ToString();
            chkRevCounterDefective.Checked = dt.Rows[0]["FLDMEREVCOUNTERDEFECTIVE"].ToString().Equals("1") ? true : false;
            txtDensity.Text = dt.Rows[0]["FLDDENSITY"].ToString();
            txtSulphur.Text = dt.Rows[0]["FLDSULPHURPERCENT"].ToString();

            rbtnPowerUnit.SelectedValue = General.GetNullableString(dt.Rows[0]["FLDPOWEROUTPUTUNIT"].ToString()) == null ? "BHP" : dt.Rows[0]["FLDPOWEROUTPUTUNIT"].ToString();
            txtcalculatedBHP.Text = dt.Rows[0]["FLDBHPCALCULATEDVALUE"].ToString();
            txtEarthFaultMonitor400.Text = dt.Rows[0]["FLDEARTHFAULTMONITOR400"].ToString();
            txtEarthFaultMonitor230or110.Text = dt.Rows[0]["FLDEARTHFAULTMONITOR230OR110"].ToString();
            ViewState["PREVIOUSREPORTSTATUS"] = dt.Rows[0]["FLDPREVIOUSREPORTTYPE"].ToString();
            ViewState["REPORTSTATUS"] = dt.Rows[0]["FLDVESSELSTATUS"].ToString();
            if (General.GetNullableDecimal(dt.Rows[0]["FLDHFOOILCONSUMPTIONQTY"].ToString()) != null && (General.GetNullableDecimal(dt.Rows[0]["FLDHFOOILCONSUMPTIONQTY"].ToString()) > General.GetNullableDecimal(dt.Rows[0]["FLDMCRFOCONS"].ToString())))
            {
                lblAlert.Visible = true;
            }
            else
            {
                lblAlert.Visible = false;
            }

            if (chkRevCounterDefective.Checked == true)
            {
                txtMERPM.CssClass = "input";
                txtMERPM.Enabled = true;

            }
            else
            {
                txtMERPM.CssClass = "readonlytextbox";
                txtMERPM.Enabled = false;
            }


            if (dt.Rows[0]["FLDCONFIRMEDYN"].ToString() == "1")
            {
               // MenuNewSaveTabStrip.Visible = false;
                lblAlertSenttoOFC.Visible = true;
            }

            if (dt.Rows[0]["FLDVESSELSTATUS"].ToString().Equals("INPORT") || dt.Rows[0]["FLDVESSELSTATUS"].ToString().Equals("ATANCHOR"))
            {
                txtEngineDistance.Text = "";
                txtEngineDistance.CssClass = "readonlytextbox";
                txtEngineDistance.Enabled = false;

                txtSlip.Text = "";
                txtSlip.CssClass = "readonlytextbox";
                txtSlip.Enabled = false;

                txtMERPM.Text = "";
                txtMERPM.CssClass = "readonlytextbox";
                txtMERPM.Enabled = false;

                //txtSWPress.CssClass = "readonlytextbox";
                //txtSWPress.Text = "";
                //txtSWPress.Enabled = false;

                txtMERPM.Text = "";
                txtMERPM.CssClass = "readonlytextbox";
                txtMERPM.Enabled = false;

                txtGovernorSetting.Text = "";
                txtGovernorSetting.CssClass = "readonlytextbox";
                txtGovernorSetting.Enabled = false;

                txtSpeedSetting.Text = "";
                txtSpeedSetting.CssClass = "readonlytextbox";
                txtSpeedSetting.Enabled = false;

                txtFOInletTemp.Text = "";
                txtFOInletTemp.CssClass = "readonlytextbox";
                txtFOInletTemp.Enabled = false;

                txtFuelOilPress.Text = "";
                txtFuelOilPress.CssClass = "readonlytextbox";
                txtFuelOilPress.Enabled = false;

                //txtERExhTemp.Text = "";
                //txtERExhTemp.CssClass = "readonlytextbox";
                //txtERExhTemp.Enabled = false;

                //txtSwellTemp.Text = "";
                //txtSwellTemp.CssClass = "readonlytextbox";
                //txtSwellTemp.Enabled = false;

                txtBHP.Text = "";
                txtBHP.CssClass = "readonlytextbox";
                txtBHP.Enabled = false;

                txtMaxExhTemp.Text = "";
                txtMaxExhTemp.CssClass = "readonlytextbox";
                txtMaxExhTemp.Enabled = false;

                txtMinExhTemp.Text = "";
                txtMinExhTemp.CssClass = "readonlytextbox";
                txtMinExhTemp.Enabled = false;

                txtScavAirTemp.Text = "";
                txtScavAirTemp.CssClass = "readonlytextbox";
                txtScavAirTemp.Enabled = false;

                txtScavAirPress.Text = "";
                txtScavAirPress.CssClass = "readonlytextbox";
                txtScavAirPress.Enabled = false;

                txtTCRPMInboard.Text = "";
                txtTCRPMInboard.CssClass = "readonlytextbox";
                txtTCRPMInboard.Enabled = false;

                txtTCRPMOutboard.Text = "";
                txtTCRPMOutboard.CssClass = "readonlytextbox";
                txtTCRPMOutboard.Enabled = false;

                txtExhTCInboardBefore.Text = "";
                txtExhTCInboardBefore.CssClass = "readonlytextbox";
                txtExhTCInboardBefore.Enabled = false;

                txtExhTCInboardAfter.Text = "";
                txtExhTCInboardAfter.CssClass = "readonlytextbox";
                txtExhTCInboardAfter.Enabled = false;

                txtExhTCOutboardBefore.Text = "";
                txtExhTCOutboardBefore.CssClass = "readonlytextbox";
                txtExhTCOutboardBefore.Enabled = false;

                txtExhTCOutboardAfter.Text = "";
                txtExhTCOutboardAfter.CssClass = "readonlytextbox";
                txtExhTCOutboardAfter.Enabled = false;

            }
        }
    }

    protected void gvConsumption_RowCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (Session["NOONREPORTID"] == null)
                {
                    ucError.ErrorMessage = "Please click the save button for the 'Noon Report' and then update the Consumption details.";
                    ucError.Visible = true;
                    return;
                }

                if (ViewState["COPY"] != null)
                {
                    ucError.ErrorMessage = "Please save the copied details first.";
                    ucError.Visible = true;
                    return;
                }

                if (((RadLabel)e.Item.FindControl("lbloilconsumptiononlaterdateyn")).Text == "1")
                {
                    ReCalculateROB(e);
                }
                else
                {
                    decimal? bunkerqty = General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtBunkeredEdit")).Text);
                    if (bunkerqty != null && bunkerqty > 0 && ViewState["DEBUNKER"].ToString() == "1")
                    {
                        bunkerqty = -1 * bunkerqty;
                    }
                    PhoenixVesselPositionNoonReportOilConsumption.InsertOilConsumption(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(ViewState["VESSELID"].ToString()),
                        new Guid(Session["NOONREPORTID"].ToString()),
                        new Guid(((RadLabel)e.Item.FindControl("lblOilTypeid")).Text),
                        "NOON",
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaMEEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaAEEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaBLREdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaIGGEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaCARGOENGEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaOTHEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourMEEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourAEEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourBLREdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourIGGEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourCARGOENGEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourOTHEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortMEEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortAEEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortBLREdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortIGGEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortCARGOENGEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucInPortOTHEdit")).Text),
                        null,
                        null,
                        1, // ROBATNOON YN
                        0, // ROB @ EOSP AND ROB @ FWE YN
                        0, // ROB @ EOSP AND ROB @ FWE CALCULATION YN
                        0, // RECALCULATE ROB
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaCARGOHEATINdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtSeaTKCLNGEdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourCARGOHEATINdit")).Text),
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("ucAtHourbourTKCLNGEdit")).Text),
                        null,
                        null,
                        bunkerqty,
                        General.GetNullableDecimal(((UserControlMaskNumber)e.Item.FindControl("txtSulphurPercentageEdit")).Text)
                        );

                    PhoenixVesselPositionNoonReport.InsertESIRegister(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                        General.GetNullableGuid(Session["NOONREPORTID"].ToString()));

                    PhoenixVesselPositionNoonReport.UpdateESIRegister(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                        General.GetNullableGuid(Session["NOONREPORTID"].ToString()));
                    ViewState["DEBUNKER"] = 0;
                    BindData();
                }
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                if (ViewState["COPY"] != null)
                {
                    ucError.ErrorMessage = "Please save the copied details first.";
                    ucError.Visible = true;
                    return;
                }

                if (Session["NOONREPORTID"] != null)
                {
                    PhoenixVesselPositionNoonReportOilConsumption.DeleteOilConsumption(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        General.GetNullableGuid(Session["NOONREPORTID"].ToString()),
                        General.GetNullableInteger(ViewState["VESSELID"].ToString()),
                        General.GetNullableGuid(((RadLabel)e.Item.FindControl("lblOilTypeid")).Text));

                    RadLabel OilConsumptionId = ((RadLabel)e.Item.FindControl("lblOilConsumptionIdItem"));
                    if (OilConsumptionId.Text != "")
                        PhoenixVesselPositionNoonReportOilConsumption.DeleteBunkerOilConsumption(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode
                        , General.GetNullableGuid((OilConsumptionId.Text)), "NOON");

                    PhoenixVesselPositionNoonReport.InsertESIRegister(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                        General.GetNullableGuid(Session["NOONREPORTID"].ToString()));

                    PhoenixVesselPositionNoonReport.UpdateESIRegister(
                        PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                        int.Parse(ViewState["VESSELID"].ToString()),
                        General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                        General.GetNullableGuid(Session["NOONREPORTID"].ToString()));
                }
            }
            else if (e.CommandName.ToUpper().Equals("EDIT"))
            {
                string ss = e.CommandArgument.ToString();
                if (ss == "cmdDeBunker")
                    ViewState["DEBUNKER"] = 1;
            }
            gvConsumptionRebind();
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void gvConsumption_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        try
        {
            LinkButton edit = (LinkButton)e.Item.FindControl("cmdEdit");

            if (edit != null)
                edit.Visible = SessionUtil.CanAccess(this.ViewState, edit.CommandName);

            DataRowView drv = (DataRowView)e.Item.DataItem;

            if (e.Item is GridEditableItem)
            {
                LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
                    if (db != null) db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");

                LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
                if (sb != null)
                {
                    RadLabel ldyn = (RadLabel)e.Item.FindControl("lbloilconsumptiononlaterdateyn");
                    if (ldyn != null)
                    {
                        if(ldyn.Text == "1")
                            sb.Attributes.Add("onclick", "return fnConfirmDelete(event, 'Please confirm you want to proceed ?'); return false;");
                    }
                }

                LinkButton cmdBunkerAdd = (LinkButton)e.Item.FindControl("cmdBunkerAdd");
                UserControlMaskNumber txtSulphurPercentageEdit = (UserControlMaskNumber)e.Item.FindControl("txtSulphurPercentageEdit");

                if (txtSulphurPercentageEdit != null)
                {
                    if (drv["FLDFUELOIL"].ToString() == "1") txtSulphurPercentageEdit.Enabled = true;
                    else txtSulphurPercentageEdit.Enabled = false;
                }

                if (cmdBunkerAdd != null)
                {
                    RadLabel lblOilConsumptionIdItem = (RadLabel)e.Item.FindControl("lblOilConsumptionIdItem");
                    if (lblOilConsumptionIdItem != null)
                    {
                        if (General.GetNullableDecimal(drv["FLDBUNKEREDQTY"].ToString()) != null && drv["FLDFUELOIL"].ToString() == "1" && drv["FLDBUNKEREDQTY"].ToString() != "0.00")
                        {
                            cmdBunkerAdd.Visible = SessionUtil.CanAccess(this.ViewState, cmdBunkerAdd.CommandName);
                            cmdBunkerAdd.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','"+Session["sitepath"]+ "/VesselPosition/VesselPositionBunkerReceipt.aspx?consumptionid="
                            + lblOilConsumptionIdItem.Text + "&oiltype=" + drv["FLDOILTYPECODE"].ToString() + "'); return true;");
                        }
                        else
                            cmdBunkerAdd.Visible = false;
                    }
                }

                UserControlMaskNumber ucAtSeaMEEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaMEEdit");
                UserControlMaskNumber ucAtSeaAEEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaAEEdit");
                UserControlMaskNumber ucAtSeaBLREdit = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaBLREdit");
                UserControlMaskNumber ucAtSeaIGGEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaIGGEdit");
                UserControlMaskNumber ucAtSeaCARGOENGEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaCARGOENGEdit");
                UserControlMaskNumber ucAtSeaCARGOHEATINdit = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaCARGOHEATINdit");
                UserControlMaskNumber ucAtSeaTKCLNGEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaTKCLNGEdit");
                UserControlMaskNumber ucAtSeaOTHEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtSeaOTHEdit");

                UserControlMaskNumber ucAtHourbourMEEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtHourbourMEEdit");
                UserControlMaskNumber ucAtHourbourAEEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtHourbourAEEdit");
                UserControlMaskNumber ucAtHourbourBLREdit = (UserControlMaskNumber)e.Item.FindControl("ucAtHourbourBLREdit");
                UserControlMaskNumber ucAtHourbourIGGEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtHourbourIGGEdit");
                UserControlMaskNumber ucAtHourbourCARGOENGEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtHourbourCARGOENGEdit");
                UserControlMaskNumber ucInPortMEEdit = (UserControlMaskNumber)e.Item.FindControl("ucInPortMEEdit");
                UserControlMaskNumber ucInPortAEEdit = (UserControlMaskNumber)e.Item.FindControl("ucInPortAEEdit");
                UserControlMaskNumber ucInPortBLREdit = (UserControlMaskNumber)e.Item.FindControl("ucInPortBLREdit");
                UserControlMaskNumber ucInPortIGGEdit = (UserControlMaskNumber)e.Item.FindControl("ucInPortIGGEdit");
                UserControlMaskNumber ucInPortCARGOENGEdit = (UserControlMaskNumber)e.Item.FindControl("ucInPortCARGOENGEdit");
                UserControlMaskNumber ucAtHourbourCARGOHEATINdit = (UserControlMaskNumber)e.Item.FindControl("ucAtHourbourCARGOHEATINdit");
                UserControlMaskNumber ucAtHourbourTKCLNGEdit = (UserControlMaskNumber)e.Item.FindControl("ucAtHourbourTKCLNGEdit");

                if (ucAtSeaMEEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) if (drv["FLDSHORTNAME"].ToString().ToUpper().Equals("AECC")) ucAtSeaMEEdit.Enabled = false; else ucAtSeaMEEdit.Enabled = true;
                if (ucAtSeaAEEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) if (!drv["FLDSHORTNAME"].ToString().ToUpper().Equals("AECC")) ucAtSeaAEEdit.Enabled = false; else ucAtSeaAEEdit.Enabled = true;
                if (ucAtSeaBLREdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtSeaBLREdit.Enabled = false; else ucAtSeaBLREdit.Enabled = true;
                if (ucAtSeaIGGEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtSeaIGGEdit.Enabled = false; else ucAtSeaIGGEdit.Enabled = true;
                if (ucAtSeaCARGOENGEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtSeaCARGOENGEdit.Enabled = false; else ucAtSeaCARGOENGEdit.Enabled = true;
                if (ucAtSeaCARGOHEATINdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtSeaCARGOHEATINdit.Enabled = false; else ucAtSeaCARGOHEATINdit.Enabled = true;
                if (ucAtSeaTKCLNGEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtSeaTKCLNGEdit.Enabled = false; else ucAtSeaTKCLNGEdit.Enabled = true;
                if (ucAtHourbourMEEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) if (drv["FLDSHORTNAME"].ToString().ToUpper().Equals("AECC")) ucAtHourbourMEEdit.Enabled = false; else ucAtHourbourMEEdit.Enabled = true;
                if (ucAtHourbourAEEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) if (!drv["FLDSHORTNAME"].ToString().ToUpper().Equals("AECC")) ucAtHourbourAEEdit.Enabled = false; else ucAtHourbourAEEdit.Enabled = true;
                if (ucAtHourbourBLREdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtHourbourBLREdit.Enabled = false; else ucAtHourbourBLREdit.Enabled = true;
                if (ucAtHourbourIGGEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtHourbourIGGEdit.Enabled = false; else ucAtHourbourIGGEdit.Enabled = true;
                if (ucAtSeaCARGOENGEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtHourbourCARGOENGEdit.Enabled = false; else ucAtHourbourCARGOENGEdit.Enabled = true;
                if (ucInPortMEEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) if (drv["FLDSHORTNAME"].ToString().ToUpper().Equals("AECC")) ucInPortMEEdit.Enabled = false; else ucInPortMEEdit.Enabled = true;
                if (ucInPortAEEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) if (!drv["FLDSHORTNAME"].ToString().ToUpper().Equals("AECC")) ucInPortAEEdit.Enabled = false; else ucInPortAEEdit.Enabled = true;
                if (ucInPortBLREdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucInPortBLREdit.Enabled = false; else ucInPortBLREdit.Enabled = true;
                if (ucInPortIGGEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucInPortIGGEdit.Enabled = false; else ucInPortIGGEdit.Enabled = true;
                if (ucInPortCARGOENGEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucInPortCARGOENGEdit.Enabled = false; else ucInPortCARGOENGEdit.Enabled = true;
                if (ucAtHourbourCARGOHEATINdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtHourbourCARGOHEATINdit.Enabled = false; else ucAtHourbourCARGOHEATINdit.Enabled = true;
                if (ucAtHourbourTKCLNGEdit != null) if (drv["FLDFUELOIL"].ToString() == "0" && !drv["FLDSHORTNAME"].ToString().ToUpper().Equals("OTH")) ucAtHourbourTKCLNGEdit.Enabled = false; else ucAtHourbourTKCLNGEdit.Enabled = true; 

                if (ViewState["PREVIOUSREPORTSTATUS"].ToString().ToUpper().Equals("ARRIVAL") && ViewState["REPORTSTATUS"].ToString().ToUpper().Equals("DRIFTING"))
                {
                    if (ucAtSeaMEEdit != null) ucAtSeaMEEdit.Enabled = false;
                    if (ucAtSeaAEEdit != null) ucAtSeaAEEdit.Enabled = false;
                    if (ucAtSeaBLREdit != null) ucAtSeaBLREdit.Enabled = false;
                    if (ucAtSeaIGGEdit != null) ucAtSeaIGGEdit.Enabled = false;
                    if (ucAtSeaCARGOENGEdit != null) ucAtSeaCARGOENGEdit.Enabled = false;
                    if (ucAtSeaCARGOHEATINdit != null) ucAtSeaCARGOHEATINdit.Enabled = false;
                    if (ucAtSeaTKCLNGEdit != null) ucAtSeaTKCLNGEdit.Enabled = false;
                    if (ucAtSeaOTHEdit != null) ucAtSeaOTHEdit.Enabled = false;
                }

                
            }

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

   
 
    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        //BindData();        
    }

    private decimal? CalculateLogSpeed(string fullspeed, string fullspeeddistance, string reducedspeed, string reduceedspeeddistance)
    {
        decimal? logspeed = 0;
        decimal? fs = General.GetNullableDecimal(fullspeed);
        decimal? rs = General.GetNullableDecimal(reducedspeed);
        decimal? fsdist = General.GetNullableDecimal(fullspeeddistance);
        decimal? rsdist = General.GetNullableDecimal(reduceedspeeddistance);

        decimal? distance = (fsdist == null ? 0 : fsdist) + (rsdist == null ? 0 : rsdist);
        decimal? speed = (fs == null ? 0 : fs) + (rs == null ? 0 : rs);

        if (speed > 0)
            logspeed = distance / speed;

        return logspeed;
    }

    private decimal? CalculateDistanceObserved(string fullspeeddistance, string reduceedspeeddistance)
    {
        decimal? fsdist = General.GetNullableDecimal(fullspeeddistance);
        decimal? rsdist = General.GetNullableDecimal(reduceedspeeddistance);

        decimal? distance = (fsdist == null ? 0 : fsdist) + (rsdist == null ? 0 : rsdist);

        return distance;
    }

    private decimal? CalculateSlip(string enginedist, string distobserved)
    {
        decimal? engdist = General.GetNullableDecimal(enginedist);
        decimal? distobs = General.GetNullableDecimal(distobserved);

        decimal? slip = 0;

        if (engdist != null && engdist != 0)
            slip = ((engdist - (distobs == null ? 0 : distobs)) / engdist) * 100;

        return slip;
    }

    // bug id: 9826 - Noon Report Field range configuration..

    private void SetFieldRange()
    {
        DataSet ds = PhoenixRegistersNoonReportRangeConfig.ListNoonReportRangeConfig(
            General.GetNullableInteger(ViewState["VESSELID"].ToString()),
            1);

        if (ds.Tables.Count > 0)
        {
            decimal? minvalue = null;
            decimal? maxvalue = null;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                //minvalue = General.GetNullableDecimal(dr["FLDMINVALUE"].ToString());
                //maxvalue = General.GetNullableDecimal(dr["FLDMAXVALUE"].ToString());
                minvalue = General.GetNullableDecimal(dr["FLDALERTLEVEL"].ToString());
                maxvalue = General.GetNullableDecimal(dr["FLDMAXALERTLEVEL"].ToString());

                switch (dr["FLDCOLUMNNAME"].ToString())
                {
                    case "FLDSLIP":
                        {
                            if (General.GetNullableDecimal(txtSlip.Text) < minvalue || General.GetNullableDecimal(txtSlip.Text) > maxvalue)
                                txtSlip.CssClass = "maxhighlight";
                            else
                                txtSlip.CssClass = "input";
                            break;
                        }
                    case "FLDMAXEXHTEMP":
                        {
                            if (General.GetNullableDecimal(txtMaxExhTemp.Text) < minvalue || General.GetNullableDecimal(txtMaxExhTemp.Text) > maxvalue)
                                txtMaxExhTemp.CssClass = "maxhighlight";
                            else
                                txtMaxExhTemp.CssClass = "input";
                            break;
                        }
                    case "FLDMINEXHTEMP":
                        {
                            if (General.GetNullableDecimal(txtMinExhTemp.Text) < minvalue || General.GetNullableDecimal(txtMinExhTemp.Text) > maxvalue)
                                txtMinExhTemp.CssClass = "maxhighlight";
                            else
                                txtMinExhTemp.CssClass = "input";
                            break;
                        }
                    case "FLDEXHGASTEMPTCINBOARDBEFORE":
                        {
                            if (General.GetNullableDecimal(txtExhTCInboardBefore.Text) < minvalue || General.GetNullableDecimal(txtExhTCInboardBefore.Text) > maxvalue)
                                txtExhTCInboardBefore.CssClass = "maxhighlight";
                            else
                                txtExhTCInboardBefore.CssClass = "input";

                            if (General.GetNullableDecimal(txtExhTCOutboardBefore.Text) < minvalue || General.GetNullableDecimal(txtExhTCOutboardBefore.Text) > maxvalue)
                                txtExhTCOutboardBefore.CssClass = "maxhighlight";
                            else
                                txtExhTCOutboardBefore.CssClass = "input";
                            break;
                        }
                    case "FLDEXHGASTEMPTCOUTBOARDBEFORE":
                        {
                            if (General.GetNullableDecimal(txtExhTCInboardAfter.Text) < minvalue || General.GetNullableDecimal(txtExhTCInboardAfter.Text) > maxvalue)
                                txtExhTCInboardAfter.CssClass = "maxhighlight";
                            else
                                txtExhTCInboardAfter.CssClass = "input";

                            if (General.GetNullableDecimal(txtExhTCOutboardAfter.Text) < minvalue || General.GetNullableDecimal(txtExhTCOutboardAfter.Text) > maxvalue)
                                txtExhTCOutboardAfter.CssClass = "maxhighlight";
                            else
                                txtExhTCOutboardAfter.CssClass = "input";
                            break;
                        }
                    case "FLDFOCATFINES":
                        {
                            if (General.GetNullableDecimal(txtFOCatFines.Text) < minvalue || General.GetNullableDecimal(txtFOCatFines.Text) > maxvalue)
                                txtFOCatFines.CssClass = "maxhighlight";
                            else
                                txtFOCatFines.CssClass = "input";
                            break;
                        }
                    case "FLDBOILERCHLORIDES":
                        {
                            if (General.GetNullableDecimal(txtBoilerWaterChlorides.Text) < minvalue || General.GetNullableDecimal(txtBoilerWaterChlorides.Text) > maxvalue)
                                txtBoilerWaterChlorides.CssClass = "maxhighlight";
                            else
                                txtBoilerWaterChlorides.CssClass = "input";
                            break;
                        }
                    case "FLDMEFOAUTOBACKWASHFILTERCOUNTER":
                        {
                            if (General.GetNullableDecimal(txtNOOFOPERATIONSMEFOAUTOBACKWASHFILTER.Text) < minvalue || General.GetNullableDecimal(txtNOOFOPERATIONSMEFOAUTOBACKWASHFILTER.Text) > maxvalue)
                                txtNOOFOPERATIONSMEFOAUTOBACKWASHFILTER.CssClass = "maxhighlight";
                            else
                                txtNOOFOPERATIONSMEFOAUTOBACKWASHFILTER.CssClass = "input";
                            break;
                        }
                    case "FLDMELOAUTOBACKWASHFILTERCOUNTER":
                        {
                            if (General.GetNullableDecimal(txtNOOFOPERATIONSMELOAUTOBACKWASHFILTER.Text) < minvalue || General.GetNullableDecimal(txtNOOFOPERATIONSMELOAUTOBACKWASHFILTER.Text) > maxvalue)
                                txtNOOFOPERATIONSMELOAUTOBACKWASHFILTER.CssClass = "maxhighlight";
                            else
                                txtNOOFOPERATIONSMELOAUTOBACKWASHFILTER.CssClass = "input";
                            break;
                        }
                    case "FLDAUXENGINEHROFOPERATION":
                        {
                            decimal? totalAuxHoursofOperatoin = null;
                            totalAuxHoursofOperatoin = (General.GetNullableDecimal(txtGeneratorLoadAE1OPHrs.Text) != null ? General.GetNullableDecimal(txtGeneratorLoadAE1OPHrs.Text) : 0) +
                                (General.GetNullableDecimal(txtGeneratorLoadAE2OPHrs.Text) != null ? General.GetNullableDecimal(txtGeneratorLoadAE2OPHrs.Text) : 0) +
                                (General.GetNullableDecimal(txtGeneratorLoadAE3OPHrs.Text) != null ? General.GetNullableDecimal(txtGeneratorLoadAE3OPHrs.Text) : 0) +
                                (General.GetNullableDecimal(txtGeneratorLoadAE4OPHrs.Text) != null ? General.GetNullableDecimal(txtGeneratorLoadAE4OPHrs.Text) : 0);

                            if (totalAuxHoursofOperatoin < minvalue || totalAuxHoursofOperatoin > maxvalue)
                            {
                                if (General.GetNullableDecimal(txtGeneratorLoadAE1OPHrs.Text) != null)
                                    txtGeneratorLoadAE1OPHrs.CssClass = "maxhighlight";
                                
                                if (General.GetNullableDecimal(txtGeneratorLoadAE2OPHrs.Text) != null)
                                    txtGeneratorLoadAE2OPHrs.CssClass = "maxhighlight";
                                if (General.GetNullableDecimal(txtGeneratorLoadAE3OPHrs.Text) != null)
                                    txtGeneratorLoadAE3OPHrs.CssClass = "maxhighlight";
                                if (General.GetNullableDecimal(txtGeneratorLoadAE4OPHrs.Text) != null)
                                    txtGeneratorLoadAE4OPHrs.CssClass = "maxhighlight";
                            }
                            else
                            {
                                txtGeneratorLoadAE1OPHrs.CssClass = "input";
                                txtGeneratorLoadAE2OPHrs.CssClass = "input";
                                txtGeneratorLoadAE3OPHrs.CssClass = "input";
                                txtGeneratorLoadAE4OPHrs.CssClass = "input";
                            }
                            break;
                        }
                    case "FLDERTEMP":
                        {
                            if (General.GetNullableDecimal(txtERExhTemp.Text) < minvalue || General.GetNullableDecimal(txtERExhTemp.Text) > maxvalue)
                                txtERExhTemp.CssClass = "maxhighlight";
                            else
                                txtERExhTemp.CssClass = "input";
                            break;
                        }
                    case "FLDENGINEDISTANCE":
                        {
                            if (General.GetNullableDecimal(txtEngineDistance.Text) < minvalue || General.GetNullableDecimal(txtEngineDistance.Text) > maxvalue)
                                txtEngineDistance.CssClass = "maxhighlight";
                            else
                                txtEngineDistance.CssClass = "input";
                            break;
                        }
                    case "FLDSWTEMP":
                        {
                            if (General.GetNullableDecimal(txtSwellTemp.Text) < minvalue || General.GetNullableDecimal(txtSwellTemp.Text) > maxvalue)
                                txtSwellTemp.CssClass = "maxhighlight";
                            //else
                            //    txtSwellTemp.CssClass = "input";
                            break;
                        }
                }
            }
        }
    }

    // Other oil consumption..

    protected void gvOtherOilCons_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataSet ds = new DataSet();

        int? iVesselId = null;

        if (ViewState["VESSELID"].ToString() != "")
        {
            iVesselId = General.GetNullableInteger(ViewState["VESSELID"].ToString());
        }
        else
            iVesselId = 0;

        string noonreportid = "";

        noonreportid = Session["NOONREPORTID"] == null ? (ViewState["OLDNOONREPORTID"] != null ? ViewState["OLDNOONREPORTID"].ToString() : "") : Session["NOONREPORTID"].ToString();

        ds = PhoenixVesselPositionNoonReportOilConsumption.ListOtherOilConsumption(iVesselId, General.GetNullableGuid(noonreportid));
        dsOtheroilcounsumption = ds;
        gvOtherOilCons.DataSource = ds;
    }
    protected void gvOtherOilConsRebind()
    {
        gvOtherOilCons.SelectedIndexes.Clear();
        gvOtherOilCons.EditIndexes.Clear();
        gvOtherOilCons.DataSource = null;
        gvOtherOilCons.Rebind();
    }

    private void InsertOtherOilConsumption()
    {
        foreach (GridDataItem gvr in gvOtherOilCons.Items)
        {

            PhoenixVesselPositionNoonReportOilConsumption.InsertOtherOilConsumption(
               PhoenixSecurityContext.CurrentSecurityContext.UserCode,
               int.Parse(ViewState["VESSELID"].ToString()),
               new Guid(Session["NOONREPORTID"].ToString()),
               new Guid(((RadLabel)gvr.FindControl("lblOilTypeCodeEdit")).Text),
               "NOON",
               General.GetNullableDecimal(((RadLabel)gvr.FindControl("lblConsumptionEdit")).Text),
               General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtROBEdit")).Text),
               General.GetNullableDecimal(((UserControlMaskNumber)gvr.FindControl("txtProducedEdit")).Text),
               General.GetNullableGuid(((RadLabel)gvr.FindControl("lblOilConsumptionIdEdit")).Text));
            
        }
        gvOtherOilConsRebind();
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    private void SetKeyDownScroll(object sender, GridViewRowEventArgs e)
    {
        int nextRow = 0;
        GridView _gridView = (GridView)sender;

        if (e.Row.RowType == DataControlRowType.DataRow
            && (e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Alternate)
            || e.Row.RowState == (DataControlRowState.Edit | DataControlRowState.Selected | DataControlRowState.Normal)))
        {
            int nRows = ((DataSet)_gridView.DataSource).Tables[0].Rows.Count - 1;

            String script = "var keyValue = SelectSibling(event); ";
            script += " if(keyValue == 38) {";  //Up Arrow
            nextRow = (e.Row.RowIndex == 0) ? nRows : e.Row.RowIndex - 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";

            script += " if(keyValue == 40) {";  //Down Arrow
            nextRow = (e.Row.RowIndex == nRows) ? 0 : e.Row.RowIndex + 1;

            script += Page.ClientScript.GetPostBackEventReference(_gridView, "Edit$" + (nextRow).ToString(), false);
            script += "}";
            script += " if(keyValue != 40 && keyValue != 38) { eleminscroll.focus(); return true; } return false;";
            e.Row.Attributes["onkeydown"] = script;
        }

    }

    private void ReCalculateROB(GridCommandEventArgs cRow)
    {
        try
        {
            decimal? bunkerqty = General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("txtBunkeredEdit")).Text);
            if (bunkerqty != null && bunkerqty > 0 && ViewState["DEBUNKER"].ToString() == "1")
            {
                bunkerqty = -1 * bunkerqty;
            }

            PhoenixVesselPositionNoonReportOilConsumption.InsertOilConsumption(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["VESSELID"].ToString()),
                new Guid(Session["NOONREPORTID"].ToString()),
                new Guid(((RadLabel)cRow.Item.FindControl("lblOilTypeid")).Text),
                "NOON",
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaMEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaAEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaBLREdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaIGGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaCARGOENGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaOTHEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourMEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourAEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourBLREdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourIGGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourCARGOENGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourOTHEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucInPortMEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucInPortAEEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucInPortBLREdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucInPortIGGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucInPortCARGOENGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucInPortOTHEdit")).Text),
                null,
                null,
                1, // ROBATNOON YN
                0, // ROB @ EOSP AND ROB @ FWE YN
                0, // ROB @ EOSP AND ROB @ FWE CALCULATION YN
                1, // RECALCULATE ROB
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaCARGOHEATINdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtSeaTKCLNGEdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourCARGOHEATINdit")).Text),
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("ucAtHourbourTKCLNGEdit")).Text),
                null,
                null,
                bunkerqty,
                General.GetNullableDecimal(((UserControlMaskNumber)cRow.Item.FindControl("txtSulphurPercentageEdit")).Text)
                );

            PhoenixVesselPositionNoonReport.InsertESIRegister(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["VESSELID"].ToString()),
                General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                General.GetNullableGuid(Session["NOONREPORTID"].ToString()));

            PhoenixVesselPositionNoonReport.UpdateESIRegister(
                PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                int.Parse(ViewState["VESSELID"].ToString()),
                General.GetNullableGuid(ViewState["VOYAGEID"].ToString()),
                General.GetNullableGuid(Session["NOONREPORTID"].ToString()));

            ucStatus.Text = "ROB Recalculated";
            ViewState["DEBUNKER"] = 0;
            gvConsumptionRebind();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    protected void chkRevCounterDefective_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkRevCounterDefective.Checked == true)
        {
            txtMERPM.CssClass = "input";
            txtMERPM.Enabled = true;

        }
        else
        {
            txtMERPM.CssClass = "readonlytextbox";
            txtMERPM.Enabled = false;
        }
    }

    private string PrepareHtmlDoc()
    {
        BindData();
        gvConsumptionRebind();
        gvOtherOilConsRebind();

        StringBuilder DsHtmlcontent = new StringBuilder();

        if (dsDeatil.Tables[0].Rows.Count > 0)
        {
            DataRow dr1 = dsDeatil.Tables[0].Rows[0];

            string counterdefective = dr1["FLDMEREVCOUNTERDEFECTIVE"].ToString().Equals("1") ? "Yes" : "No";

            DsHtmlcontent.Append("<html><table><tr><td align=\"center\"><b>NOON REPORT ENGINE DEPARTMENT DETAIL</b></td></tr></table><br />");

            DsHtmlcontent.Append("<table ID='tbl1' border='0.5' cellpadding='3' cellspacing='0' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\" >Vessel</td><td colspan=\"4\">" + dr1["FLDVESSELNAME"].ToString() + "</td><tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Engine Distance </td><td colspan=\"4\">" + dr1["FLDENGINEDISTANCE"].ToString() + "   nm</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Slip</td><td colspan=\"4\">" + string.Format(String.Format("{0:#####.00}", dr1["FLDSLIP"])) + "   %</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">ER Temp</td><td colspan=\"4\">" + dr1["FLDERTEMP"].ToString() + "   ° C</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">SW Temp</td><td colspan=\"4\">" + dr1["FLDSWTEMP"].ToString() + "   ° C</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">SW Press</td><td colspan=\"4\">" + dr1["FLDSWPRESS"].ToString() + "   bar</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"6\"><b>Main Engine</b></td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">M/E Rev Counter</td><td colspan=\"2\">" + dr1["FLDMEREVCOUNTER"].ToString() + "</td><td colspan=\"2\"><b>Counter Defective : </b>" + counterdefective + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Average RPM</td><td colspan=\"4\">" + dr1["FLDMERPM"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Power Output</td><td colspan=\"4\">" + dr1["FLDBHP"].ToString() + "   bhp</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Governor Setting / Fuel rack</td><td colspan=\"4\">" + dr1["FLDGOVERNORSETTING"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Speed Setting</td><td colspan=\"4\">" + dr1["FLDSPEEDSETTING"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Exh Temp Max</td><td colspan=\"4\">" + dr1["FLDMAXEXHTEMP"].ToString() + "   ° C</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Exh Temp Min</td><td colspan=\"4\">" + dr1["FLDMINEXHTEMP"].ToString() + "   ° C</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Scav Air Temp</td><td colspan=\"4\">" + dr1["FLDSCAVAIRTEMP"].ToString() + "   ° C</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">FO Inlet Temp</td><td colspan=\"4\">" + dr1["FLDFOINLETTEMP"].ToString() + "   ° C</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Scav Air Press</td><td colspan=\"4\">" + dr1["FLDSCAVAIRPRESS"].ToString() + "    bar</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Fuel Oil Press</td><td colspan=\"4\">" + dr1["FLDFUELOILPRESS"].ToString() + "    bar</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">T/C1 RPM</td><td colspan=\"4\">" + dr1["FLDTCRPMINBOARD"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">T/C2 RPM</td><td colspan=\"4\">" + dr1["FLDTCRPMOUTBOARD"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">T/C1 Exh Gas Temp In</td><td colspan=\"4\">" + dr1["FLDEXHGASTEMPTCINBOARDBEFORE"].ToString() + "   ° C</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">T/C1 Exh Gas Temp Out</td><td colspan=\"4\">" + dr1["FLDEXHGASTEMPTCINBOARDAFTER"].ToString() + "   ° C</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">T/C2 Exh Gas Temp In</td><td colspan=\"4\">" + dr1["FLDEXHGASTEMPTCOUTBOARDBEFORE"].ToString() + "   ° C</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">T/C2 Exh Gas Temp Out</td><td colspan=\"4\">" + dr1["FLDEXHGASTEMPTCOUTBOARDAFTER"].ToString() + "   ° C</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">FO Cat Fines</td><td colspan=\"4\">" + dr1["FLDFOCATFINES"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\"><b>Aux Engine</b></td><td colspan=\"2\"></td><td colspan=\"2\"> Hours Of Operation</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">A/E No 1. Generator Load</td><td colspan=\"2\">" + string.Format(String.Format("{0:#####.00}", dr1["FLDGENERATORLOADAE1"].ToString())) + "    kw</td><td colspan=\"2\">" + dr1["FLDGENERATORLOADAE1OPHRS"].ToString() + "   hrs</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">A/E No 2. Generator Load </td><td colspan=\"2\">" + string.Format(String.Format("{0:#####.00}", dr1["FLDGENERATORLOADAE2"].ToString())) + "    kw</td><td colspan=\"2\">" + dr1["FLDGENERATORLOADAE2OPHRS"].ToString() + "   hrs</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">A/E No 3. Generator Load</td><td colspan=\"2\">" + string.Format(String.Format("{0:#####.00}", dr1["FLDGENERATORLOADAE3"].ToString())) + "    kw</td><td colspan=\"2\">" + dr1["FLDGENERATORLOADAE3OPHRS"].ToString() + "   hrs</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">A/E No 4. Generator Load</td><td colspan=\"2\">" + string.Format(String.Format("{0:#####.00}", dr1["FLDGENERATORLOADAE4"].ToString())) + "    kw</td><td colspan=\"2\">" + dr1["FLDGENERATORLOADAE4OPHRS"].ToString() + "   hrs</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\"><b>Purifier</b></td><td colspan=\"4\">Hours Of Operation</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">HFO PFR 1</td><td colspan=\"4\">" + dr1["FLDHFOPFR1HRS"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">HFO PFR 2</td><td colspan=\"4\">" + dr1["FLDHFOPFR2HRS"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">DO PFR</td><td colspan=\"4\">" + dr1["FLDDOPFRHRS"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">ME LO PFR</td><td colspan=\"4\">" + dr1["FLDMELOPFRHRS"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">AE LO PFR</td><td colspan=\"4\">" + dr1["FLDAELOPFRHRS"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\"><b>Fine Filter</b></td><td  colspan=\"2\">Counter</td><td  colspan=\"2\">No Of Operations</td>></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">ME FO AUTO BACK WASH FILTER</td><td  colspan=\"2\">" + dr1["FLDMEFOAUTOBACKWASHFILTERCOUNTER"].ToString() + "</td><td  colspan=\"2\">" + dr1["NOOFMEFOOPERATIONS"].ToString() + "</td>></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">ME LO AUTO BACK WASH FILTER</td><td  colspan=\"2\">" + dr1["FLDMELOAUTOBACKWASHFILTERCOUNTER"].ToString() + "</td><td  colspan=\"2\">" + dr1["NOOFMELOOPERATIONS"].ToString() + "</td>></tr>");
            DsHtmlcontent.Append("</table>");


            if (dsOtheroilcounsumption.Tables[0].Rows.Count > 0)
            {
                DataTable t1 = new DataTable();
                t1 = dsOtheroilcounsumption.Tables[0];
                DsHtmlcontent.Append("<br /><br /><br />");

                DsHtmlcontent.Append("<div class = 'mystyle' align='left'>");
                DsHtmlcontent.Append("<font size='9px' face='Helvetica'>");
                DsHtmlcontent.Append("<table ID='tbl1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
                DsHtmlcontent.Append("<font color='white' size=14px ><tr><td height='9' align='center'><b>Fresh Water</b></td></tr></font>");
                DsHtmlcontent.Append("</table>");

                DsHtmlcontent.Append("<table ID=\"tbl2\" border ='0.5'  opacity='0.5' cellpadding=\"7\" cellspacing='0' style='border:red 1px solid'>");
                DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' colspan='2'>Fresh Water</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Previous ROB</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Produced</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>ROB</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>Consumption</td></tr>");

                if (t1.Rows.Count > 0)
                {
                    foreach (DataRow dr in t1.Rows)
                    {
                        DsHtmlcontent.Append("<tr>");//colspan='2'
                        DsHtmlcontent.Append("<td colspan='2' >" + dr["FLDOILTYPENAME"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDPREVIOUSROB"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDPRODUCED"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDROB"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDCONSUMPTIONQTY"].ToString() + "</td>");
                        DsHtmlcontent.Append("</tr>");//colspan='2'
                    }
                }

                DsHtmlcontent.Append("</table>");
            }

            
            DsHtmlcontent.Append("<br /><table ID='tbl3' border='0.5' cellpadding='3' cellspacing='0' >");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Boiler water Chlorides</td><td colspan=\"4\">" + dr1["FLDBOILERCHLORIDES"].ToString() + "   ppm</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"6\"><b>Bilge and Sludge</b></td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Bilge Tank ROB</td><td colspan=\"4\">" + dr1["FLDBILGETANKROB"].ToString() + "   cu.m</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Sludge Tank ROB</td><td colspan=\"4\">" + dr1["FLDSLUDGETANKROB"].ToString() + "   cu.m</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Last landing of Bilge Water</td><td colspan=\"2\">" + dr1["FLDLASTLANDINGBILGE"].ToString() + "</td><td colspan=\"2\"><b> Days : </b>" + dr1["FLDDAYSLASTLANDINGBILGE"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Last landing of Bilge Water</td><td colspan=\"2\">" + dr1["FLDLASTLANDINGSLUDGE"].ToString() + "</td><td colspan=\"2\"> <b>Days : </b>" + dr1["FLDDAYSFROMLASTLANDING"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' width=\"300px\" colspan=\"2\">Chief Engineer Remarks</td><td colspan=\"4\">" + dr1["FLDREMARKSCE"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("</table><br /><br />");

            DsHtmlcontent.Append("<div class = 'mystyle' align='left'>");
            DsHtmlcontent.Append("<font size='9px' face='Helvetica'>");
            DsHtmlcontent.Append("<table ID='tbl1' bgcolor='#93a3b9' opacity ='0.5' cellpadding='7' cellspacing='0'>");
            DsHtmlcontent.Append("<font color='white' size=14px ><tr><td height='9' align='center'><b>Consumption</b></td></tr></font>");
            DsHtmlcontent.Append("</table>");

            DsHtmlcontent.Append("<table ID='tbl4' border='0.5' cellpadding='3' cellspacing='0'>");
            DsHtmlcontent.Append("<tr><td>* Fuel Oil Consumption in mT </td><td>FO Cons Rate (mt/day)</td><td>" + dr1["FLDHFOOILCONSUMPTIONQTY"].ToString() + "</td><td>Density @ 15˚C</td><td>" + dr1["FLDDENSITY"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("<tr><td>* Lub Oil Consumption in Ltr </td><td>DO Cons Rate (mt/day)</td><td>" + dr1["FLDMDOOILCONSUMPTIONQTY"].ToString() + "</td><td>Sulphur Countent %</td><td>" + dr1["FLDSULPHURPERCENT"].ToString() + "</td></tr>");
            DsHtmlcontent.Append("</table>");

            if (dsoilcounsumption.Tables[0].Rows.Count > 0)
            {
                DataTable t1 = new DataTable();
                t1 = dsoilcounsumption.Tables[0];
                DsHtmlcontent.Append("<br /><br /><br /><br /><br />");

                DsHtmlcontent.Append("<table ID=\"tbl3\" border ='0.5'  opacity='0.5' cellpadding=\"7\" cellspacing='0' style='border:red 1px solid'>");
                DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' colspan='2'>Oil Type</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' align=\"Center\" colspan='2'>Previous ROB</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' align=\"Center\" colspan='8'>AT SEA</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' align=\"Center\" colspan='8'>IN PORT</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' align=\"Center\" >Bunkered</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' align=\"Center\" >Sulphur %</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' align=\"Center\">Total</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' align=\"Center\" colspan='2'>ROB at Noon</td></tr>");

                DsHtmlcontent.Append("<tr><td bgcolor='#f1f1f1' colspan='2'></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' colspan='2'></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' >M/E</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' >A/E</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' >BLR</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' >IGG</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' >C/ENG</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' >C/HTG</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' >TK CLNG</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>OTH</td>");

                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' >M/E</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' >A/E</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' >BLR</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' >IGG</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' >C/ENG</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' >C/HTG</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' >TK CLNG</td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'>OTH</td>");

                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1'></td>");
                DsHtmlcontent.Append("<td bgcolor='#f1f1f1' colspan='2'></td></tr>");

                if (t1.Rows.Count > 0)
                {
                    foreach (DataRow dr in t1.Rows)
                    {
                        DsHtmlcontent.Append("<tr>");//colspan='2'
                        DsHtmlcontent.Append("<td colspan='2' >" + dr["FLDOILTYPENAME"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\" colspan='2'>" + dr["FLDPREVIOUSROB"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["ATSEAME"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["ATSEAAE"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["ATSEABLR"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["ATSEAIGG"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["ATSEACARGOENG"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["ATSEACTHG"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["ATSEATKCLNG"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["ATSEAOTH"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["INPORTME"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["INPORTAE"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["INPORTBLR"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["INPORTIGG"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["INPORTCARGOENG"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["INPORTCTHG"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["INPORTTKCLNG"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["INPORTOTH"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDBUNKEREDQTY"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDSULPHURPERCENTAGE"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\">" + dr["FLDTOTALCONSUMPTION"].ToString() + "</td>");
                        DsHtmlcontent.Append("<td align=\"Right\" colspan='2'>" + dr["FLDROBATNOON"].ToString() + "</td>");
                        DsHtmlcontent.Append("</tr>");//colspan='2'
                    }
                }

                DsHtmlcontent.Append("</table>");
            }

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
                    string filefullpath = "EngineDepartment" + ".pdf";
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

    protected void chkFOCounterDefective_OnCheckedChanged(object sender, EventArgs e)
    {
        if (chkFOCounterDefective.Checked == true)
        {
            txtNOOFOPERATIONSMEFOAUTOBACKWASHFILTER.CssClass = "input";
            txtNOOFOPERATIONSMEFOAUTOBACKWASHFILTER.Enabled = true;
        }
        else
        {
            txtNOOFOPERATIONSMEFOAUTOBACKWASHFILTER.CssClass = "readonlytextbox";
            txtNOOFOPERATIONSMEFOAUTOBACKWASHFILTER.Enabled = false;
        }
    }

    protected void chkLOCounterDefective_OnCheckedChanged(object sender, EventArgs e)
    {

        if (chkLOCounterDefective.Checked == true)
        {
            txtNOOFOPERATIONSMELOAUTOBACKWASHFILTER.CssClass = "input";
            txtNOOFOPERATIONSMELOAUTOBACKWASHFILTER.Enabled = true;
        }
        else
        {
            txtNOOFOPERATIONSMELOAUTOBACKWASHFILTER.CssClass = "readonlytextbox";
            txtNOOFOPERATIONSMELOAUTOBACKWASHFILTER.Enabled = false;
        }
    }

    protected void gvConsumption_PreRender(object sender, EventArgs e)
    {
        if (gvConsumption.EditItems.Count > 0)
        {
            foreach (GridDataItem item in gvConsumption.Items)
            {
                if (item.Edit)
                {
                    item["meatsea"].ColumnSpan = 8;
                    item["aeatsea"].Visible = false;
                    item["blratsea"].Visible = false;
                    item["iggatsea"].Visible = false;
                    item["cengatsea"].Visible = false;
                    item["chtgatsea"].Visible = false;
                    item["tkclngatsea"].Visible = false;
                    item["othatsea"].Visible = false;

                    item["meinport"].ColumnSpan = 8;
                    item["aeinport"].Visible = false;
                    item["blrinport"].Visible = false;
                    item["igginport"].Visible = false;
                    item["cenginport"].Visible = false;
                    item["chtginport"].Visible = false;
                    item["tkclnginport"].Visible = false;
                    item["othinport"].Visible = false;

                    item["bunker"].ColumnSpan = 3;
                    item["sulphur"].Visible = false;
                    item["total"].Visible = false;
                }
            }
        }
    }
}
