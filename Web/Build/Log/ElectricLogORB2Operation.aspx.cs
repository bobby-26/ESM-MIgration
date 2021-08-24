using SouthNests.Phoenix.Elog;
using SouthNests.Phoenix.Framework;
using System;
using System.Data;

public partial class Log_ElectricLogORB2Operation : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        DataTable dt = PhoenixMarbolLogORB2.ORB2LogIsFaliureODME(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        if (dt.Rows.Count > 0)
        {
            if (dt.Rows[0]["FLDFAILURE"].ToString() == "1")
            {
                btnFailureOfOilDischarge.Enabled = false;
            }
            else
                btnAfterRepairOilDischarge.Enabled = false;
        }
        buttononclick();
    }

    private void buttononclick()
    {
        btnLoadingCargo.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2CargoLoading.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnInternalTransferOfOilCargo.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2InternalCargoTransfer.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnUnloadingOilCargo.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2UnloadingCargo.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnCrudeOilWashing.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2CrudeOilWashing.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnBallastingCargoTanks.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2BallastingCargoTanks.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnCleaningCargo.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2CleaningCargo.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnDischargeOfDirtyBallast.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2DischargeDirtyBallast.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnDischargeOfWater.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2DischargeOfWater.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnCollectionResidues.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2CollectionResidues.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnDischargeOfCleanBallast.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2DischargeOfCleaningBallast.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));

        btnFailureOfOilDischarge.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2FailureOfOilDischarge.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnAccidentalDischarge.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2AccidentalDischarge.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnAdditionalOperationProcedure.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2AdditionalProcedure.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnAfterRepairOilDischarge.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2AfterRepairOilDischarge.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnMissedOperation.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2MissedOperation.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));

        btnBallastDedicatedClean.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2BallastingDedicatedClean.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnDischargeOfBallastDedicated.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2DischargeBallastDedicatedClean.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnLoadingofBallast.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2LoadingBallastWater.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnReallocationBallast.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2ReallocationBallastWater.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
        btnBallastReceptionFacility.Attributes.Add("onclick", string.Format("javascript:parent.openNewWindow('Log','','{0}/Log/ElectricLogORB2BallastWaterDischargeReception.aspx', null, null, null, null, null, {{'closeAlert': true}});", Session["sitepath"]));
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {

    }
}