using SouthNests.Phoenix.Framework;
using System;

public partial class Log_ElectricLogCRBOperation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        buttononclick();
    }

    private void buttononclick()
    {
        btnLoadingCargo.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogCRBCargoLoading.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnInternalTransferOfOilCargo.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogCRBInternalCargoTransfer.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnUnloadingOilCargo.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogCRBUnloadingCargo.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnMandatoryPreWash.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogCRBMandatoryPrewash.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnCleaningCargoTank.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogCRBCleaningCargo.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnDischargeSea.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogCRBDischargeOfWater.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnBallastingCargoTank.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogCRBBallastingCargoTanks.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnDischargeBallastWater.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogCRBDischargeOfCleaningBallast.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnAccidentalDischarge.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogCRBAccidentalDischarge.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnControlAuthorized.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogCRBControlAuhorizedSurveyors.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnAdditionalOperational.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogCRBMissedOperation.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
}