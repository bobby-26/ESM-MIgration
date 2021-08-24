using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;

public partial class Log_ElectricLogOperation : PhoenixBasePage
{
    int usercode = 0;
    int vesselId = 0;
    protected void Page_Load(object sender, EventArgs e)
    {
        usercode = PhoenixSecurityContext.CurrentSecurityContext.UserCode;
        vesselId = PhoenixSecurityContext.CurrentSecurityContext.VesselID;

        cmdHiddenSubmit.Attributes.Add("style", "display:none");
        PhoenixToolbar toolbar = new PhoenixToolbar();
        toolbar.AddButton("Save", "SAVE", ToolBarDirection.Right);
        if (!IsPostBack)
        {
            setButtonClick();
            isFailureLogFiled();
        }
    }

    private void isFailureLogFiled()
    {
        if (isFailureClosed(vesselId))
        {
            btnfailure.Enabled = false;
            btnRectification.Enabled = true;
        } else
        {
            btnfailure.Enabled = true;
            btnRectification.Enabled = false;
        }
    }

    private bool isFailureClosed(int VesselId)
    {
        DataTable dt = PhoenixElog.IsFaliureClosed(usercode, VesselId);
        if (dt.Rows.Count > 0)
        {
            DataRow row = dt.Rows[0];
            if (Convert.ToBoolean(row["FLDISFAILURE"]))
            {
                return true;
            }
        }
        return false;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
    public void BindData()
    {
        try
        {

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    public void setButtonClick()
    {
        btnWeeklyEntries.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogWeeklyEntries.aspx', null, null, null, null, null, {{'closeAlert': true }});", Session["sitepath"]));
        btnSludgeTransfer.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogSludgeTransfer.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnEvpSludgeTank.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogEvaporationFromSludge.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnIncineration.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogIncineration.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnMnlCollectionIOPPTank.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogOilSludgeToIOPP.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnShoreSludgeDisposal.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogShoreSludgeDisposal.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnSludgefromERtoDeckCargo.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogSludgeFromERToDeck.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnWaterDrained.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogWaterDrainedFromSludge.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnSludgeBurning.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogSludgeBurningInBoiler.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnBilgeWell.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogBilgeWell.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btninternalbilge.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogInternalBilge.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnOWSOperation.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogOWSOperation.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnbilgetosludge.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogBilgetoSludge.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnBilgeWaterfrmEr.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogBilgewaterERToDeck.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnfailure.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogOilFilteringEquiment.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnRectification.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogRectificationOWSOCM.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnAccidentalDischarge.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogAccidentalDischarge.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnbunkering.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogBunkeringFuel.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnbunkerniglube.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogBunkeringLube.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btncargobilge.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogCargoBilgeHoldingTank.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        
        btndebunering.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogDebunkeringFuelOil.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnsealing.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogSealingOverboardValve.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnunsealing.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogUnSealingOverboardValve.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        
        btnBilgeShoreDisposal.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogBilgeShoreDisposal.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnMissedentry.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogMissedOperationalEntry.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnEvaporationFromBilge.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogEvaporationFromBilge.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnMiscellaneousEntry.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogMiscellaneousEntry.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnQuarterlyTesting.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogQuarterlyTestingOWS.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnBilgeMainenaneSepearator.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogMaintenance15BilgeSeparator.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnMainenanceIncinerator.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogMaintenanceOfIncinerator.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnDisposingSludge.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogDisposingSludgeCleaning.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnCleaningBilge.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogCleaningBilgeTank.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));

        btnAutomaticPumpingBilge.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogAutomaticPumpingBilge.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnAutomaticTransferBilge.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogAutomaticTransferBilge.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));


        //btnSludgeTransfer.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogSludgeTransfer.aspx', null, null, null, null, null, {{'closeAlert': true, 'closeAlertMessage': 'Are you sure ? '}});", Session["sitepath"]));

    }

    protected void OnClientItemClicked(object sender, EventArgs e)
    {

    }
}