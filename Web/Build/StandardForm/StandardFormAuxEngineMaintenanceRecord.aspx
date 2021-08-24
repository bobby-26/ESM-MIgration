<%@ Page Language="C#" AutoEventWireup="true" Inherits="StandardFormAuxEngineMaintenanceRecord"
    CodeFile="StandardFormAuxEngineMaintenanceRecord.aspx.cs" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>E19 Aux. Engine Maintenance record 8-07 Rev 1s</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
     <script language="javascript" type="text/javascript">
        function cmdPrint_Click()
        {            
            document.getElementById('cmdPrint').style.visibility = "hidden";
            window.print();
        }
    </script>
    <style type="text/css">
        .Sftblclass
        {
            border-collapse: collapse;
        }
        .Sftblclass tr td
        {
            border: 1px solid black;
        }
        .Sftblclass tr td input
        {
            width: 96%;
        }
    </style>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAuxEngine" runat="server" autocomplete="off">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
         <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="AUXILIARY ENGINE MAINTENANCE RECORD" ShowMenu="false">
            </eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="StandardForm_TabStripCommand" />
            <asp:Button ID="cmdHiddenConfirm" runat="server" Text="cmdHiddenConfirm" OnClick="StandardFormConfirm_TabStripCommand" />
        </div>                 
        <asp:UpdatePanel runat="server" ID="pnlStandardForm">
            <ContentTemplate>
              <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server" visible="false" />
                <table width="95%">
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblCompanyName" runat="server" Text="EXECUTIVE SHIP MANAGEMENT"></asp:Literal></b>
                        </td>
                        <td align="center">
                           
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblE19" runat="server" Text="E19"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td align="center">
                           
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" colspan="3">
                            <asp:Literal ID="lbl0714Rev" runat="server" Text="(07/14 Rev 1)"></asp:Literal>
                        </td>
                    </tr>
                </table>
                <table width="95%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td align="center" colspan="4">
                            <h3>
                                <asp:Literal ID="lblAuxiliaryEngineMaintenanceRecord" runat="server" Text="AUXILIARY ENGINE MAINTENANCE RECORD"></asp:Literal></h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVesselName" runat="server" Enabled="false"  CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblMake" runat="server" Text="Make"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMake" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblType" runat="server" Text="Type"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVesselType" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <table width="95%" class="Sftblclass" cellpadding="1" cellspacing="1">
                    <tr>
                        <td width="20%" align="center">
                            <asp:Literal ID="lblAuxiliaryEnginesRunningHours" runat="server" Text="AUXILIARY ENGINES RUNNING HOURS/STANDARD HOURS"></asp:Literal>
                        </td>
                        <td align="center" width="5%">
                            <asp:Literal ID="lblDetails" runat="server" Text="DETAILS"></asp:Literal>
                        </td>
                        <td align="center" colspan="2">
                           <asp:Literal ID="lblAuxiliaryEngineNo1" runat="server" Text="AUXILIARY ENGINE NO.1"></asp:Literal>
                        </td>
                        <td align="center" colspan="2">
                            <asp:Literal ID="lblauxiliaryEngineNO2" runat="server" Text="AUXILIARY ENGINE NO.2"></asp:Literal>
                        </td>
                        <td align="center" colspan="2">
                            <asp:Literal ID="lblauxiliaryEngineNO3" runat="server" Text="AUXILIARY ENGINE NO.3"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblRHrsDuringTheMonth" runat="server" Text="R/Hrs.DURING THE MONTH."></asp:Literal>
                        </td>
                        <td align="center">
                            -
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtDuration1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtDuration2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtDuration3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblTotalRunningHours" runat="server" Text="TOTAL RUNNING HOURS"></asp:Literal>
                        </td>
                        <td align="center">
                            -
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtTotalRunhour1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtTotalRunhour2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtTotalRunhour3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                           <asp:Literal ID="lblLastDecarbonisation" runat="server" Text="LAST DECARBONISATION (HRS)"></asp:Literal>
                        </td>
                        <td align="center">
                           <asp:Literal ID="lblDate1" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtDecarbonisationDate1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtDecarbonisationDate2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtDecarbonisationDate3" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblHRSSince" runat="server" Text="HRS SINCE"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtDecarbonisationHrs1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtDecarbonisationHrs2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtDecarbonisationHrs3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <asp:Literal ID="lblCylinderHeadOverhaul" runat="server" Text="CYLINDER HEAD OVERHAUL (HRS)"></asp:Literal>
                        </td>
                        <td align="center">
                           <asp:Literal ID="lblDate2" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtOverhaulDate1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtOverhaulDate2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtOverhaulDate3" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblHRSSince1" runat="server" Text="HRS SINCE"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtOverhaulHrs1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtOverhaulHrs2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtOverhaulHrs3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                           <asp:Literal ID="lbl250HourlyRoutines" runat="server" Text="250 HOURLY ROUTINES (FILTER CLEANING ETC.) AIR/FUEL/LUBE."></asp:Literal>
                        </td>
                        <td align="center">
                           <asp:Literal ID="lblDate3" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtRoutineDate1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtRoutineDate2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtRoutineDate3" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblHrsSince2" runat="server" Text="HRS SINCE"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtRoutineHrs1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtRoutineHrs2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtRoutineHrs3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <asp:Literal ID="lbl500HrsRoutine" runat="server" Text="500 HRS ROUTINE."></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblDate4" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtFiveDate1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtFiveDate2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtFiveDate3" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                           <asp:Literal ID="lblHRSSince6" runat="server" Text="HRS SINCE"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtFiveHrs1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtFiveHrs2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtFiveHrs3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <asp:Literal ID="lbl1000HrsRoutine" runat="server" Text="1000 HRS ROUTINE."></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblDate13" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtThousandDate1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtThousandDate2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtThousandDate3" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:literal ID="lblHrsSince4" runat="server" Text="HRS SINCE"></asp:literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtThousandHrs1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtThousandHrs2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtThousandHrs3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <asp:Literal ID="lblFuelPumpTimingChecK" runat="server" Text="FUEL PUMP TIMING CHECK (HRS)"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblDate5" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtTimingcheckDate1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtTimingcheckDate2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtTimingcheckDate3" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                           <asp:Literal ID="lblHrsSince5" runat="server" Text="HRS SINCE"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtTimingcheckHrs1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtTimingcheckHrs2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtTimingcheckHrs3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <asp:Literal ID="lblFuelPumpOverHaul" runat="server" Text="FUEL PUMP OVERHAUL (HRS)"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblDate14" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtFPoverhaulDate1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtFPoverhaulDate2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtFPoverhaulDate3" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblHrsSince7" runat="server" Text="HRS SINCE"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtFPoverhaulHrs1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtFPoverhaulHrs2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtFPoverhaulHrs3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <asp:Literal ID="lblCrankshaftDeflection" runat="server" Text="CRANKSHAFT DEFLECTION (ONCE IN THREE MONTHS)"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblDate15" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtDeflectionDate1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtDeflectionDate2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtDeflectionDate3" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblHrsSince8" runat="server" Text="HRS SINCE"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtDeflectionHrs1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtDeflectionHrs2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtDeflectionHrs3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <asp:Literal ID="lblCrankCaseOilRenewel" runat="server" Text="CRANK CASE OIL RENEWEL (HRS)"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblDate6" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtOilDate1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtOilDate2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtOilDate3" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                           <asp:Literal ID="lblHrsSince9" runat="server" Text="HRS SINCE"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtOilHrs1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtOilHrs2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtOilHrs3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                           <asp:Literal ID="lblTurboChargerDisassembly" runat="server" Text="TURBOCHARGER DISASSEMBLY & CLEANING (HRS)"></asp:Literal>                        </td>
                        <td align="center">
                           <asp:Literal ID="lblDate7" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtDisassemblyDate1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtDisassemblyDate2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtDisassemblyDate3" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblHrsSince11" runat="server" Text="HRS SINCE"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtDisassemblyHrs1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtDisassemblyHrs2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtDisassemblyHrs3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                           <asp:Literal ID="lblTurboChargeBearingRenewal" runat="server" Text="TURBOCHARGER BEARINGS RENEWAL (HRS)"></asp:Literal>
                        </td>
                        <td align="center">
                           <asp:Literal ID="lblDate16" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtTurboBearingDate1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtTurboBearingDate2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtTurboBearingDate3" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                           <asp:Literal ID="lblHrsSince12" runat="server" Text="HRS SINCE"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtTurboBearingHrs1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtTurboBearingHrs2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtTurboBearingHrs3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <asp:Literal ID="lblAIRIncoolerCleaningHrs" runat="server" Text="AIR INTERCOOLER CLEANING (HRS)"></asp:Literal>
                        </td>
                        <td align="center">
                           <asp:LIteral ID="lblDate9" runat="server" Text="Date"></asp:LIteral>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtAirCoolerDate1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtAirCoolerDate2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtAirCoolerDate3" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblHrsSince13" runat="server" Text="HRS SINCE"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtAirCoolerHrs1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtAirCoolerHrs2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtAirCoolerHrs3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <asp:Literal ID="lblLOCoolerCleaning" runat="server" Text="LO COOLER CLEANING (HRS)"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblDate17" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtLoCoolerDate1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtLoCoolerDate2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtLoCoolerDate3" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblHRSSince14" runat="server" Text="HRS SINCE"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtLoCoolerHrs1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtLoCoolerHrs2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtLoCoolerHrs3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                          <asp:Literal ID="lblBIGENDBEARINGBOLTRENEWAL" runat="server" Text="BIG END BEARING BOLT RENEWAL (HRS)"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblDate11" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtBoltDate1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtBoltDate2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtBoltDate3" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblHRSSINCE15" Runat="server" Text="HRS SINCE"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtBoltHrs1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtBoltHrs2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtBoltHrs3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="2">
                            <asp:Literal ID="lblGovernorOverhaul" runat="server" Text="GOVERNOR OVERHAUL (HRS)"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblDate12" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtGovernorDate1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtGovernorDate2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtGovernorDate3" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td align="center">
                            <asp:Literal ID="lblHRSSINCE16" runat="server" Text="HRS SINCE"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtGovernorHrs1" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtGovernorHrs2" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                        <td colspan="2">
                            <eluc:Number ID="txtGovernorHrs3" runat="server" CssClass="input" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAlarmsArmsTripsTrial" runat="server" Text="ALARMS/TRIPS TRIAL (ONCE MONTHLY)"></asp:Literal>
                        </td>
                        <td align="center">
                            <asp:Literal ID="lblDate18" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtalarmDate1" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtalarmDate2" runat="server" CssClass="input" />
                        </td>
                        <td colspan="2">
                            <eluc:Date ID="txtalarmDate3" runat="server" CssClass="input" />
                        </td>
                    </tr>
                    <tr align="center">
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Literal ID="lblUnitNos" runat="server" Text="UNIT NOS"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lbl1" runat="server" Text="1"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lbl2" runat="server" Text="2"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lbl3" runat="server" Text="3"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lbl4" runat="server" Text="4"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lbl5" runat="server" Text="5"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lbl6" runat="server" Text="6"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="3">
                            <asp:Literal ID="lblMainBearingRenewalDateHrsSince" runat="server" Text="MAIN BEARING RENEWAL DATE/HRS SINCE"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblAE1" runat="server" Text="AE1"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe11" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe12" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe13" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe14" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe15" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe16" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAE2" runat="server" Text="AE2"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe21" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe22" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe23" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe24" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe25" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe26" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAE3" runat="server" Text="AE3"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe31" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe32" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe33" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe34" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe35" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMainAe36" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="3">
                            <asp:Literal ID="lblBigEndBearingRenewalDateHrsSince" runat="server" Text="BIG END BEARING RENEWAL DATE/HRS SINCE"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblAE11" runat="server" Text="AE1"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe11" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe12" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe13" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe14" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe15" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe16" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAE21" runat="server" Text="AE2"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe21" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe22" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe23" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe24" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe25" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe26" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAE31" runat="server" Text="AE3"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe31" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe32" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe33" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe34" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe35" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBigAe36" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td rowspan="3">
                            <asp:Literal ID="lblConnectingRodRenewalDateHrsSince" runat="server" Text="CONNECTING ROD RENEWAL DATE/HRS SINCE"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblAE12" runat="server" Text="AE1"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe11" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe12" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe13" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe14" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe15" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe16" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAE22" runat="server" Text="AE2"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe21" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe22" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe23" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe24" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe25" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe26" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAE32" runat="server" Text="AE3"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe31" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe32" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe33" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe34" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe35" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRodAe36" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td align="center" colspan="2">
                            <asp:TextBox ID="txtSecondEngineer" runat="server" CssClass="gridinput"></asp:TextBox><br />
                            <asp:Literal ID="lblSecondEngineer" runat="server" Text="Second Engineer"></asp:Literal>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td align="center" colspan="2">
                            <asp:TextBox ID="txtChiefEngineer" runat="server" CssClass="gridinput"></asp:TextBox><br />
                            <asp:Literal ID="lblChiefEngineer" runat="server" Text="Chief Engineer"></asp:Literal>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
