<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormBearingMeasuring.aspx.cs"
    Inherits="StandardFormBearingMeasuring" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Record of Nox Verification</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <form id="frmRecordofNox" runat="server" autocomplete="off">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Record Of Nox Verification" ShowMenu="false">
            </eluc:Title>
             <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="StandardForm_TabStripCommand" />
            <asp:Button ID="cmdHiddenConfirm" runat="server" Text="cmdHiddenConfirm" OnClick="StandardFormConfirm_TabStripCommand" />
        </div>
       
        <asp:UpdatePanel runat="server" ID="pnlStandardForm">
            <ContentTemplate>
             <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server" visible="false" />
                <table width="95%">
                    <tr>   
                        <td colspan="6" align="left">
                            <b><asp:Literal ID="lblExecutiveShipManagement" runat="server" Text="EXECUTIVE SHIP MANAGEMENT PTE LTD"></asp:Literal></b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="lblFIleRef" runat="server" Text="File Ref- Marpol Annex VI"></asp:Literal>
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblE12" runat="server" Text="E12"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left">
                            <b><asp:Literal ID="lblSingapore" runat="server" Text="SINGAPORE"></asp:Literal></b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="lblCE23" runat="server" Text="C/E 23"></asp:Literal>
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Literal ID="lbl605Rev1" runat="server" Text="(6/05 Rev 1)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <h3 style="margin-bottom: 0px">
                                <asp:Literal ID="lblBearingMeasureReport" runat="server" Text="BEARING MEASURING REPORT"></asp:Literal><br />
                            </h3>
                            <b><asp:Literal ID="lblParts" runat="server" Text="PARTS A, B, C & D."></asp:Literal></b>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtvesselName" runat="server" CssClass="input" Enabled="false" ></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblPort" runat="server" Text="Port"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPort" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td colspan="2">
                           <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblEngineType" runat="server" Text="Engine Type"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtEngineTye" runat="server" CssClass="input" Enabled="false" ></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblMainEngineAuxEngineNo" runat="server" Text="Main Engine / Aux. Engine No"></asp:Literal>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtMainEngineAuxEngine" runat="server" CssClass="input" Enabled="false" ></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblDateLastReportPartA" runat="server" Text="Date Last Report: Part A"></asp:Literal>
                        </td>
                        <td>
                           <asp:TextBox ID="txtPartAReport" runat="server"  CssClass="input"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtCalender1" PopupPosition="BottomRight" runat="server"  TargetControlID="txtPartAReport">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td>
                           <<asp:Literal ID="lblPartB" runat="server" Text="Part B"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPartBReport" runat="server"  CssClass="input"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtCalender2" PopupPosition="BottomRight" runat="server"  TargetControlID="txtPartBReport">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:Literal ID="lblPartC" runat="server" Text="Part C"></asp:Literal>
                        </td>
                        <td>
                           <asp:TextBox ID="txtPartCReport" runat="server"  CssClass="input"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtCalender3" PopupPosition="BottomRight" runat="server"  TargetControlID="txtPartCReport">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                        <td>
                            <asp:Literal ID="lblPartD" runat="server" Text="Part D"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPartDReport" runat="server"  CssClass="input"></asp:TextBox>
                            <ajaxToolkit:CalendarExtender ID="txtCalender4" PopupPosition="BottomRight" runat="server"  TargetControlID="txtPartDReport">
                            </ajaxToolkit:CalendarExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblDraftFwd" runat="server" Text="Draft: Fwd"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDraftfwd" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblAft" runat="server" Text="Aft"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtAft" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblTrim" runat="server" Text="Trim"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTrim" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <h3 style="margin-bottom: 0px">
                                <u><asp:Literal ID="lblPartA" runat="server" Text="PART ‘A’"></asp:Literal></u>
                            </h3>
                        </td>
                        <td colspan="3">
                            <asp:Literal ID="lblUnitofMeasurements" runat="server" Text="Unit of measurements = 1/100 mm"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Literal ID="lblBridgeGaugeReadingsMaxReading" runat="server" Text="Bridge Gauge Readings Max Reading:"></asp:Literal>
                        </td>
                        <td colspan="6">
                            <asp:TextBox ID="txtGaugeMaxReading" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td align="center">
                                       <asp:Literal ID="lblBRGNO" runat="server" Text="BRG No."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl11" runat="server" Text="11"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl10" runat="server" Text="10"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl9" runat="server" Text="9"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl8" runat="server" Text="8"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl7" runat="server" Text="7"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl6" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl5" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl4" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl3" runat="server" Text="3"></asp:Literal>
                                        
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl2" runat="server" Text="2"></asp:Literal>
                                        
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl1" runat="server" Text="1"></asp:Literal>
                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:literal ID="lblOriginalBridgeGaugeReadings" runat="server" Text="Original<br />Bridge Gauge<br />Readings<br />"></asp:literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOriginalReadings1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOriginalReadings2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOriginalReadings3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOriginalReadings4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOriginalReadings5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOriginalReadings6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOriginalReadings7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOriginalReadings8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOriginalReadings9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOriginalReadings10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOriginalReadings11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                       <asp:Literal ID="lblNewReadings" runat="server" Text="New Readings"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNewReadings1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNewReadings2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNewReadings3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNewReadings4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNewReadings5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNewReadings6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNewReadings7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNewReadings8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNewReadings9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNewReadings10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewReadings11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblWearDown" runat="server" Text="Wear Down"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWearDown1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWearDown2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWearDown3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWearDown4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWearDown5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWearDown6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWearDown7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWearDown8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWearDown9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtWearDown10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtWearDown11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <h3 style="margin-bottom: 0px">
                                <u><asp:Literal ID="lblPartBB" runat="server" Text="PART ‘B’"></asp:Literal></u>
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Literal ID="lblMainBearingClearancesTakenOff" runat="server" Text="Main Bearing Clearances, Taken off"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBox ID="chLead" runat="server" Text="Leads" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chFeelerGauge" runat="server" Text="Feeler Gauge" />
                        </td>
                        <td>
                            <asp:Literal ID="lblNormal" runat="server" Text="Normal"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNormal" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblMax" runat="server" Text="Max"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMax" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Literal ID="lblBRGNO1" runat="server" Text="BRG No."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblBRGNO11" runat="server" Text="11"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblBRGNO10" runat="server" Text="10"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblBRGNO9" runat="server" Text="9"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblBRGNO8" runat="server" Text="8"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblBRGNO7" runat="server" Text="7"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblBRGNO6" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblBRGNO5" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblBRGNO4" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblBRGNO3" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblBRGNO2" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblBRGNOone" runat="server" Text="1"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" align="center">
                                       <asp:Literal ID="lblOriginalClearances" runat="server" Text="Original<br />Clearances"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblF" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartBF2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartBF3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartBF4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartBF5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartBF6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartBF7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartBF8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartBF9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartBF1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartBF10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartBF11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblA" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartAF1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartAF2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartAF3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartAF4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartAF5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartAF6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartAF7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartAF8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartAF9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartAF10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtClearencePartAF11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" align="center">
                                        <asp:Literal ID="lblNewReadings1" runat="server" Text="New<br />Readings"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblFF" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedF1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedF2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedF3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedF4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedF5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedF6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedF7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedF8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedF9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedF10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedF11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblAA" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedA1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedA2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedA3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedA4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedA5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedA6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedA7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedA8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedA9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedA10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:TextBox ID="txtNewRedA11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <h3 style="margin-bottom: 0px">
                                <u><asp:Literal ID="lblPartC3" runat="server" Text="PART ‘C’"></asp:Literal></u>
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Literal ID="lblCrossHeadBearingClearancesTakenOff" runat="server" Text="I. Crosshead Bearing Clearances, Taken off"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBox ID="chCrossheadLead" runat="server" Text="Leads" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chCrossheadFeeler" runat="server" Text="Feeler Gauge" />
                        </td>
                        <td>
                           <asp:Literal ID="lblNormalA" runat="server" Text="Normal"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCrossHeadNormal" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblMax1" runat="server" Text="Max"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCrossHeadMax" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td colspan="2" rowspan="2" align="center">
                                       <asp:Literal ID="lblUnitNo" runat="server" Text="Unit No."></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl9A" runat="server" Text="9"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl8A" runat="server" Text="8"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl7A" runat="server" Text="7"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl6A" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl5A" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl4A" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl3A" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl2A" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl1A" runat="server" Text="1"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblA1" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblF1" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblA2" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblF2" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblA3" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblF3" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td align="center">
                                       <asp:Literal ID="lblA4" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblF4" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblA5" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblF5" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblA6" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblF6" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblA7" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblF7" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblA8" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblF8" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblA9" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblF9" runat="server" Text="F"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" align="center">
                                        <asp:literal ID="lblOriginal" runat="server" Text="Original<br />Clearances"></asp:literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblF11" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC12" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC13" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC14" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC15" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC16" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC17" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartC18" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:literal ID="lblA11" runat="server" Text="A"></asp:literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox19" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox20" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox21" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox22" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox23" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox24" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox25" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox26" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox27" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox28" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox29" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox30" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox31" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox32" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox33" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox34" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox35" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox36" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" align="center">
                                       <asp:Literal ID="lblNewReadingsC" runat="server" TexT="New<br />Readings"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblF12" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF12" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF13" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF14" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF15" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF16" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF17" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCF18" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblA12" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA12" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA13" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA14" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA15" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA16" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA17" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtClearencePartCA18" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Literal ID="lblIICrossHeadGuide" runat="server" Text="II. Crosshead Guide / Shoe"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCrossheadShoe1" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCrossheadShoe2" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblNormalC" runat="server" Text="Normal"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCrossheadShoe1Normal" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblMaxC" runat="server" Text="Max"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCrossheadShoeMax" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td colspan="2" align="center">
                                        <asp:literal ID="lblUnitNo2" runat="server" Text="Unit No."></asp:literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl9C" runat="server" Text="9"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl8C" runat="server" Text="8"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl7C" runat="server" Text="7"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl6C" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl5C" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl4C" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl3C" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl2C" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lbl1C" runat="server" Text="1"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" align="center">
                                        <asp:Literal ID="lblOriginalClearancesC" runat="server" Text="Original<br />Clearances"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:literal ID="lblFplusA" runat="server" Text="F+A"></asp:literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA12" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA13" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA14" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA15" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA16" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA17" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoeFA18" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblPplusS" runat="server" Text="P+S"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS12" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS13" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS14" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS154" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS16" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS17" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS18" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoePS19" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" align="center">
                                        <asp:Literal ID="lblNewReadings11" runat="server" Text="New<br />   Readings"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblFplusANew" runat="server" Text="F+A"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA12" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA13" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA14" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA15" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA16" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA17" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2FA18" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblPlusSNew" runat="server" Text="P+S"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS12" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS134" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS14" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS15" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS16" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS17" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrossheadShoe2PS18" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <h3 style="margin-bottom: 0px">
                                <u><asp:Literal ID="lblPartDD" runat="server" Text="PART ‘D’"></asp:Literal></u>
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Literal ID="lblCrankpinBearingTakenOff" runat="server" Text="Crankpin Bearing, Taken off"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBox ID="chCrankpinLead" runat="server" Text="Leads" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chCrankpinFeeler" runat="server" Text="Feeler Gauge" />
                        </td>
                        <td>
                            <asp:Literal ID="lblNormal1" runat="server" Text="Normal"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCrankpin1Normal" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblMaxD" runat="server" Text="Max"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCrankpinMax" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Literal ID="lblUnitNo4" runat="server" Text="Unit No."></asp:Literal>
                                    </td>
                                    <td align="center">
                                       <asp:Literal ID="lbl9D" runat="server" Text="9"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl8D" runat="server" Text="8"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl7D" runat="server" Text="7"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl6D" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl5D" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl4D" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl3D" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl2D" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl1D" runat="server" Text="1"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" align="center">
                                        <asp:Literal ID="lblOriginal1" runat="server" Text="Original<br />Clearances"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDF" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin1F1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin1F2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin1F3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin1F4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin1F5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin1F6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin1F7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin1F8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin1F9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                       <asp:Literal ID="lblDA" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinA1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinA2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinA3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinA4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinA5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinA6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinA8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinA9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinA10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" align="center">
                                        <asp:Literal ID="lblNewReadingsD" runat="server" Text="New<br />Readings"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblFD" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinF1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinF2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinF3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinF4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinF5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinF6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinF7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinF8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpinF9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:literal ID="lblAD" runat="server" Text="A"></asp:literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin2A1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin2A2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin2A3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin2A4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin2A5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin2A6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin2A7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin2A8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCrankpin2A9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtChiefEngineer" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            &nbsp;
                        </td>
                        <td>
                           <asp:Literal ID="lblChiefEngineer" runat="server" Text="Chief Engineer"></asp:Literal>
                        </td>
                    </tr>
                </table>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
