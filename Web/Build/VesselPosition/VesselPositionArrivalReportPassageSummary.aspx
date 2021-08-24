<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionArrivalReportPassageSummary.aspx.cs" Inherits="VesselPositionArrivalReportPassageSummary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Voyage" Src="~/UserControls/UserControlVoyage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Direction" Src="~/UserControls/UserControlDircection.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Arrival Report..</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <style>
            .table {
                border-collapse: collapse;
            }

                .table td, th {
                    border: 1px solid black;
                }

            .accordian_voluntary {
                background-color: blue;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="panel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
        <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="ArrivalSummary" TabStrip="true" runat="server" OnTabStripCommand="ArrivalSummary_TabStripCommand"></eluc:TabStrip>
        <eluc:TabStrip ID="ArrivalSave" runat="server" OnTabStripCommand="ArrivalSave_TabStripCommand"></eluc:TabStrip>

        <telerik:RadAjaxPanel runat="server" ID="panel1">
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td style="width: 15%;">
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td style="width: 25%;">
                            <telerik:RadTextBox runat="server" ID="txtVessel" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVoyage" runat="server" Text="Voyage"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="txtVoyage" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFromPort" runat="server" Text="From Port"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="txtFromPort" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblToPort" runat="server" Text="To Port"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="txtPort" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCOSP" runat="server" Text="COSP"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Date ID="txtCOSPDate" runat="server" CssClass="readonlytextbox" Enabled="false" DatePicker="true" />
                            <telerik:RadTimePicker ID="txtCOSPDateTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                                DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                            </telerik:RadTimePicker>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblEOSP" runat="server" Text="EOSP"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Date ID="txtEOSPDate" runat="server" CssClass="readonlytextbox" Enabled="false" DatePicker="true" />
                            <telerik:RadTimePicker ID="txtEOSPDatetime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                                DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                            </telerik:RadTimePicker>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblBallastorLaden" runat="server" Text="Ballast/Laden"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="txtBallastLaden" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFullSpeedTotal" runat="server" Text="Full Speed Total"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Number ID="txtFullSpeed" runat="server" CssClass="readonlytextbox" MaxLength="6" Enabled="false" Width="80px" />
                            &nbsp;
                            <telerik:RadLabel ID="lblFullSpeedhrs" runat="server" Text="hrs"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblSuptRemarks" runat="server" Text="Supt Remarks"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblReducedSpeedTotal" runat="server" Text="Reduced Speed Total"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Number ID="txtReducedSpeed" runat="server" CssClass="readonlytextbox" MaxLength="5" Enabled="false" Width="80px" />
                            &nbsp;
                            <telerik:RadLabel ID="lblReducedSpeedhrs" runat="server" Text="hrs"></telerik:RadLabel>
                        </td>
                        <td rowspan="10">
                            <telerik:RadTextBox ID="txtSuptRemarks" runat="server" CssClass="input" TextMode="MultiLine" Resize="Both" Rows="10" Width="500px" Height="190px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblStopped" runat="server" Text="Stopped"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Number ID="txtStopped" runat="server" CssClass="readonlytextbox" MaxLength="5" Width="80px" Enabled="false" />
                            &nbsp;
                            <telerik:RadLabel ID="lblStoppedhrs" runat="server" Text="hrs"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblManoeuvering" runat="server" Text="Manoeuvering"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Number ID="txtManoevering" runat="server" CssClass="readonlytextbox" MaxLength="5" Width="80px" Enabled="false" />
                            &nbsp;
                            <telerik:RadLabel ID="lblManoeveringhrs" runat="server" Text="hrs"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTotalDistanceObserved" runat="server" Text="Total Distance Observed"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Number ID="txtDistanceObserved" runat="server" CssClass="readonlytextbox" MaxLength="6" Width="80px" Enabled="false" />
                            &nbsp;
                            <telerik:RadLabel ID="lblDistanceObservednm" runat="server" Text="nm"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTotalEngineDistance" runat="server" Text="Total Engine Distance"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Number ID="txtEngineDistance" runat="server" CssClass="readonlytextbox" MaxLength="6" Width="80px" Enabled="false" />
                            &nbsp;
                            <telerik:RadLabel ID="lblEngineDistancenm" runat="server" Text="nm"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTotalManoeuveringDistance" runat="server" Text="Total Manoeuvering Distance"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Number ID="txtManoeveringDist" runat="server" CssClass="readonlytextbox" MaxLength="6" Width="80px" Enabled="false" />
                            &nbsp;
                            <telerik:RadLabel ID="lblManoeveringDistnm" runat="server" Text="nm"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAvgSpeed" runat="server" Text="Avg Speed"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Number ID="txtSpeed" runat="server" CssClass="readonlytextbox" Width="80px" Enabled="false" MaxLength="6" />
                            &nbsp;
                            <telerik:RadLabel ID="lblSpeedkts" runat="server" Text="kts"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAvgSlip" runat="server" Text="Avg Slip"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Number ID="txtAvgSlip" runat="server" CssClass="readonlytextbox" MaxLength="6" Width="80px" Enabled="false" />
                            &nbsp;
                            <telerik:RadLabel ID="lblAvgSlipPercentage" runat="server" Text="%"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAvgRPM" runat="server" Text="Avg RPM"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Number ID="txtAvgRPM" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                IsPositive="true" MaxLength="9" Width="80px" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblEEOI" runat="server" Text="EEOI"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Number ID="txtEEOI" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                IsPositive="true" MaxLength="9" Width="80px" Enabled="false" />
                            &nbsp;
                            <telerik:RadLabel ID="lblEEOIUnit" runat="server" Text="g-CO2/T-nm"></telerik:RadLabel>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <b>
                    <telerik:RadLabel ID="lbl" runat="server" Text="Voyage Summary"></telerik:RadLabel>
                </b>
                <table class="rfdTable rfdOptionList" style="width: 100%;">
                    <tr>
                        <th style="width: 400px">
                            <telerik:RadLabel ID="lblPrticularsheader" Text="" runat="server"></telerik:RadLabel>
                        </th>
                        <th>
                            <telerik:RadLabel ID="lblbadweatherheader" Text="Bad Weather" runat="server"></telerik:RadLabel>
                        </th>
                        <th>
                            <telerik:RadLabel ID="lblgoodweatherheader" Text="Good Weather" runat="server"></telerik:RadLabel>
                        </th>
                        <th>
                            <telerik:RadLabel ID="lbloverallweatherheader" Text="Overall " runat="server"></telerik:RadLabel>
                        </th>
                    </tr>
                    <tr>
                        <td style="width: 25%">
                            <telerik:RadLabel ID="lblDistanceSailed" runat="server" Text="Distance Sailed (nm)"></telerik:RadLabel>
                        </td>
                        <td align="right" style="width: 25%">
                            <telerik:RadLabel ID="lblDistanceSailedBad" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right" style="width: 25%">
                            <telerik:RadLabel ID="lblDistanceSailedGood" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right" style="width: 25%">
                            <telerik:RadLabel ID="lblDistanceSailedOverall" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTimeEnroute" runat="server" Text="Time En Route (hr)"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblTimeEnrouteBad" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblTimeEnrouteGood" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblTimeEnrouteOverall" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAvgSpeedkts" runat="server" Text="Average Speed (kts)"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblAvgSpeedktsBad" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblAvgSpeedktsGood" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblAvgSpeedktsOverall" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPredictedTime" runat="server" Text="Predicted Time (As per CP) (hr)"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblPredictedTimeBad" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblPredictedTimeGood" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblPredictedTimeOverall" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="OverallLossorGain" runat="server" Text="Time (Loss)/Gain in Good WX (hr)"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="OverallLossorGainBad" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="OverallLossorGainGood" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="OverallLossorGainAll" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTotalMEFOCinVoyage" runat="server" Text="Actual Good WX ME FOC in Voy (mt)"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblTotalMEFOCinVoyageBad" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblTotalMEFOCinVoyageGood" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblTotalMEFOCinVoyageOverall" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPredictedFOCinVoyage" runat="server" Text="Predicted ME FOC in Voy (mt)"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblPredictedFOCinVoyageBad" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblPredictedFOCinVoyageGood" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblPredictedFOCinVoyageOverall" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblLossorGaininFOC" runat="server" Text="Good WX (Loss) or Gain in FOC (mt)"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblLossorGaininFOCBad" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblLossorGaininFOCGood" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblLossorGaininFOCOverall" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAvgRPMHeader" runat="server" Text="Avg. RPM"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblAvgRPMBad" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblAvgRPMGood" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblAvgRPMOverall" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAvgBHP" runat="server" Text="Average BHP"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblAvgBHPBad" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblAvgBHPGood" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblAvgBHPOverall" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <b>
                    <telerik:RadLabel ID="Literal1" runat="server" Text="Time Analysis Based in Good Weather"></telerik:RadLabel>
                </b>
                <table class="rfdTable rfdOptionList" style="width: 50%;">
                    <tr>
                        <td style="width: 25%;">
                            <telerik:RadLabel ID="lblCPSpeed" runat="server" Text="CP Speed"></telerik:RadLabel>
                        </td>
                        <td style="width: 25%;" align="right">
                            <telerik:RadLabel ID="lblCPSpeedValue" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPredictedTimeHeader" runat="server" Text="Predicted Time"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblPredictedTimeValue" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTimelossorgainingoodweather" runat="server" Text="Time (Loss)/Gain in Good WX (hr)"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblTimelossorgainingoodweatherValue" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOverallTimelossorgain" runat="server" Text="Overall Time (loss)/gain"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblOverallTimelossorgainValue" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <b>
                    <telerik:RadLabel ID="lblBunkerAnalysis" runat="server" Text="Bunker Analysis"></telerik:RadLabel>
                </b>
                <table class="rfdTable rfdOptionList" style="width: 75%;">
                    <tr class="rfdTable rfdOptionList">
                        <th align="right" style="width: 25%">
                            <telerik:RadLabel ID="lblUnit" Text="" runat="server"></telerik:RadLabel>
                        </th>
                        <th style="width: 25%">
                            <telerik:RadLabel ID="lblHFO" Text="HFO" runat="server"></telerik:RadLabel>
                        </th>
                        <th style="width: 25%">
                            <telerik:RadLabel ID="lblMDO" Text="MDO" runat="server"></telerik:RadLabel>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDailyCPAllowance" runat="server" Text="Daily CP Allowance"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblDailyCPAllowanceHFO" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblDailyCPAllowanceMDO" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblActuaGoodWXConsumption" runat="server" Text="Actual Good WX Consumption"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblActuaGoodWXConsumptionHFO" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblActuaGoodWXConsumptionMDO" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblGoodWXAvgDailyConsumption" runat="server" Text="Good WX Avg. Daily Consumption"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblGoodWXAvgDailyConsumptionHFO" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblGoodWXAvgDailyConsumptionMDO" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOverallAvgDailyConsumption" runat="server" Text="Overall Avg. Daily Consumption"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblOverallAvgDailyConsumptionHOF" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblOverallAvgDailyConsumptionMDO" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAllowedGoodWXConsumption" runat="server" Text="Allowed Good WX Consumption"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblAllowedGoodWXConsumptionHFO" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblAllowedGoodWXConsumptionMDO" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblGoodWXoverorunderconsumption" runat="server" Text="Good WX (over)/under consumption"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblGoodWXoverorunderconsumptionHFO" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblGoodWXoverorunderconsumptionMDO" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOveralloverorunderConsumption" runat="server" Text="Overall (over)/under Consumption"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblOveralloverorunderConsumptionHFO" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblOveralloverorunderConsumptionMDO" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table class="rfdTable rfdOptionList" style="width: 100%;">
                    <tr>
                        <th colspan="1" style="width: 25%"></th>
                        <th colspan="4" align="center">
                            <telerik:RadLabel ID="lblROB" runat="server" Text="ROB"></telerik:RadLabel>
                        </th>
                        <th colspan="4"></th>
                    </tr>
                    <tr>
                        <th colspan="1" align="right">
                            <telerik:RadLabel ID="lblROBUnit" runat="server" Text=""></telerik:RadLabel>
                        </th>
                        <th colspan="2" align="center" style="width: 25%">
                            <telerik:RadLabel ID="lblDepROB" runat="server" Text="Departure (COSP)"></telerik:RadLabel>
                        </th>
                        <th colspan="2" align="center" style="width: 25%">
                            <telerik:RadLabel ID="lblArrROB" runat="server" Text="Arrival (EOSP)"></telerik:RadLabel>
                        </th>
                        <th colspan="4" align="center">
                            <telerik:RadLabel ID="lblConsumed" runat="server" Text="Consumed"></telerik:RadLabel>
                        </th>
                    </tr>
                    <tr>
                        <th>
                            <telerik:RadLabel ID="lblFromToHdr" runat="server" Text="From - To"></telerik:RadLabel>
                        </th>
                        <th>
                            <telerik:RadLabel ID="lblDepHFOHdr" runat="server" Text="HFO"></telerik:RadLabel>
                        </th>
                        <th>
                            <telerik:RadLabel ID="lblDepMDOHdr" runat="server" Text="MDO"></telerik:RadLabel>
                        </th>
                        <th>
                            <telerik:RadLabel ID="lblArrHFOHdr" runat="server" Text="HFO"></telerik:RadLabel>
                        </th>
                        <th>
                            <telerik:RadLabel ID="lblArrMDOHdr" runat="server" Text="MDO"></telerik:RadLabel>
                        </th>
                        <th>
                            <telerik:RadLabel ID="lblConsHFOHdr" runat="server" Text="HFO"></telerik:RadLabel>
                        </th>
                        <th>
                            <telerik:RadLabel ID="lblConsMDOHdr" runat="server" Text="MDO"></telerik:RadLabel>
                        </th>
                        <th>
                            <telerik:RadLabel ID="lblConsMEHdr" runat="server" Text="ME"></telerik:RadLabel>
                        </th>
                        <th>
                            <telerik:RadLabel ID="lblConsAEHdr" runat="server" Text="AE"></telerik:RadLabel>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFromTo" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblDepHFO" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblDepMDO" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblArrHFO" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblArrMDO" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblConsHFO" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblConsMDO" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblConsME" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblConsAE" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <telerik:RadLabel ID="lblTotal" runat="server" Text="<b>Total</b>"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblConsHFOValue" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblConsMDOValue" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblConsMEValue" runat="server"></telerik:RadLabel>
                        </td>
                        <td align="right">
                            <telerik:RadLabel ID="lblConsAEValue" runat="server"></telerik:RadLabel>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td colspan="4">
                            <b>
                                <telerik:RadLabel ID="lblConsumption" runat="server" Text="Consumption"></telerik:RadLabel>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="gvConsumption" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" OnItemDataBound="gvConsumption_ItemDataBound" OnNeedDataSource="gvConsumption_NeedDataSource"
                                AllowSorting="false" ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" DataKeyNames="FLDOILTYPECODE">
                                    <HeaderStyle Width="102px" />
                                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Item">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblOilType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="ROB @ COSP">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblROBCSOP" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBCOSP") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblVesselArrivalID" runat="server" Width="120px" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELARRIVALID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblOilConsumptionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="ROB @ EOSP">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblROBFWE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBFWE") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn Visible="false">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblgvBunkered" runat="server" Text="Bunkered"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblBunkered" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUNKERED") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Total Cons">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblTotalCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCONS") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Avg Cons/day">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblAvgConsumptionItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGCONSUMPTIONQTY") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Action">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                </table>
                <br />
                <br />
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td colspan="4">
                            <b>
                                <telerik:RadLabel ID="lblFreshWater" runat="server" Text="Fresh Water"></telerik:RadLabel>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <telerik:RadGrid ID="gvFW" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                OnItemDataBound="gvFW_ItemDataBound" OnNeedDataSource="gvFW_NeedDataSource"
                                Width="100%" CellPadding="3" ShowFooter="false" ShowHeader="true" EnableViewState="false"
                                EnableHeaderContextMenu="true" GroupingEnabled="false">
                                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" DataKeyNames="FLDOILCONSUMPTIONID">
                                    <HeaderStyle Width="102px" />
                                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Fresh Water">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblOilType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="ROB on Prev Arr">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblROBonPrevArr" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBONARR") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Received">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblReceived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUNKERED") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Cons in Port">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblConsinPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSINPORT") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="ROB on Dep">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblROBonDep" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBONDEP") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Produced">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblProduced" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCED") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="ROB on Arr">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblROBonArr" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Cons at Sea">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblConsatSea" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSATSEA") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblOilConsumptionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn Visible="false" HeaderText="Cons at Arr">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblConsatArr" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSATARR") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn Visible="false" HeaderText="Cons at Dep">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblConsatDep" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSATDEP") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Action">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
