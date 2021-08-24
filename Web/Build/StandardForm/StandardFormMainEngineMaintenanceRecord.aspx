<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormMainEngineMaintenanceRecord.aspx.cs"
    Inherits="StandardFormMainEngineMaintenanceRecord" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Main Engine Maintenance Record</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <form id="frmMainEngineMaintenance" runat="server" autocomplete="off">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Main Engine Maintenance" ShowMenu="false">
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
                            <b><asp:Literal ID="lblCompanyName" runat="server" Text="EXECUTIVE SHIP MANAGEMENT PTE LTD"></asp:Literal></b>
                        </td>
                        <td align="left">
                          
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblE18" runat="server" Text="E18"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left">
                            <%--<b><asp:Literal ID="lblSingapore" runat="server" Text="SINGAPORE"></asp:Literal></b>--%>
                        </td>
                        <td align="left">
                           
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Literal ID="lbl303Rev0" runat="server" Text="(07/14 Rev 1)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <h3>
                                <asp:Literal ID="lblMainEngineMaintenanecRecord" runat="server" Text="MAIN ENGINE MAINTENANCE RECORD"></asp:Literal>
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name :"></asp:Literal>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtvesselName" runat="server" Enabled="false"  CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblDate" runat="server" Text="Date :"></asp:Literal>
                        </td>
                        <td colspan="3">
                            <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblMainEngine" runat="server" Text="Main Engine"></asp:Literal>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtMainEngine" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblType" runat="server" Text="Type :"></asp:Literal>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtEngineTye" runat="server" Enabled="false"  CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <table width="90%" class="Sftblclass" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td>
                                       <asp:Literal ID="lblMainEngineRunningHrsThisMonth" runat="server" Text="MAIN ENGINE RUNNING HRS THIS MONTH."></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtMainEngineRunningHrsMonth" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblMainEngineTotalRunningHours" runat="server" Text="MAIN ENGINE TOTAL RUNNING HOURS:"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtMainEngineRunningHrs" runat="server" CssClass="input" />
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
                        <td colspan="8">
                            <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHRSSinceStatndardHrs" runat="server" Text="HR'S SINCE /<br />STANDARD HRS."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblStatus" runat="server" Text="STATUS"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblUnit1" runat="server" Text="UNIT #1"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblUnit2" runat="server" Text="UNIT #2"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblUnit3" runat="server" Text="UNIT #3"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblUnit4" runat="server" Text="UNIT #4"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblUnit5" runat="server" Text="UNIT #5"></asp:Literal>
                                    </td>
                                    <td align="center">
                                       <asp:Literal ID="lblUnit6" runat="server" Text="UNIT #6"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Literal ID="lblLastDecarb" runat="server" Text="LAST DECARB.<br />( HRS.)"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDate1" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtLastDecarbUnit1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtLastDecarbUnit2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtLastDecarbUnit3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtLastDecarbUnit4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtLastDecarbUnit5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtLastDecarbUnit6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSuince" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtHourLastDecarb1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtHourLastDecarb2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtHourLastDecarb3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtHourLastDecarb4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtHourLastDecarb5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtHourLastDecarb6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Literal ID="lblPistonOHD" runat="server" Text="PISTON O'HD<br />( HRS.)"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbLDate19" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtPistonDate1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtPistonDate2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtPistonDate3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtPistonDate4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtPistonDate5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtPistonDate6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSince" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPistonHours1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPistonHours2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPistonHours3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPistonHours4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPistonHours5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPistonHours6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Literal ID="lblEXHValueOHL" runat="server" Text="EXH. VALVE O'HL<br />( HRS.)"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDate2" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtValveDate1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtValveDate2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtValveDate3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtValveDate4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtValveDate5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtValveDate6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSince1" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtValveHour1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtValveHour2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtValveHour3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtValveHour4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtValveHour5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtValveHour6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="4">
                                        <asp:Literal ID="lblFuelInjectors" runat="server" Text="FUEL INJECTOR'S (last pr.test)<br />(&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;HRS. CHECK COND.)<br />(&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;HRS. CHECK COND.)<br />(&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;HRS. CHECK COND.)"></asp:Literal>       </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDate3" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtFInjectorDate1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtFInjectorDate2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtFInjectorDate3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtFInjectorDate4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtFInjectorDate5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtFInjectorDate6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSince2" runat="Server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtFInjectorHour1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtFInjectorHour2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtFInjectorHour3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtFInjectorHour4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtFInjectorHour5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtFInjectorHour6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSince3" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtFInjectorHour21" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtFInjectorHour22" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtFInjectorHour23" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtFInjectorHour24" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtFInjectorHour25" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtFInjectorHour26" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSince5" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="Number30" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="Number31" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="Number32" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="Number33" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="Number34" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="Number35" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Literal ID="lblSTAirVVOhl" runat="server" Text="ST. AIR VV O'HL<br />(HRS.)"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDate4" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtAirDate1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtAirDate2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtAirDate3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtAirDate4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtAirDate5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtAirDate6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                       <asp:Literal ID="lblHoursSince6" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAirHour1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAirHour2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAirHour3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAirHour4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAirHour5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAirHour6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Literal ID="lblIndicatorValueHrs" runat="server" Text="INDICATOR VALVE<br />(HRS.)"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDate5" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtIndicatorDate1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtIndicatorDate2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtIndicatorDate3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtIndicatorDate4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtIndicatorDate5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtIndicatorDate6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSince7" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtIndicatorHour1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtIndicatorHour2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtIndicatorHour3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtIndicatorHour4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtIndicatorHour5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtIndicatorHour6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="3">
                                        <asp:Literal ID="lblSafetyValue" runat="server" Text="SAFETY VALVE<br />(HRS.)<br />(HRS.)"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDate15" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtSafetyDate1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtSafetyDate2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtSafetyDate3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtSafetyDate4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtSafetyDate5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtSafetyDate6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSince8" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtSafetyHour1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtSafetyHour2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtSafetyHour3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtSafetyHour4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtSafetyHour5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtSafetyHour6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSince4" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtSafetyHour21" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtSafetyHourt22" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtSafetyHoutr22" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtSafetyHour23" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtSafetyHour24" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtSafetyHour25" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="3">
                                        <asp:Literal ID="lblFuelPumpOHD" runat="server" Text="FUEL PUMP O'HD<br />OVERHAULE BASED ON OBSERVATION<br />(RENEW PLUNGER/BARREL :HRS.)<br />"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDate6" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtPumpDate1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtPumpDate2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtPumpDate3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtPumpDate4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtPumpDate5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtPumpDate6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSince9" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPumpHour1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPumpHour2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPumpHour3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPumpHour4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPumpHour5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPumpHour6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSince10" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPumpHour21" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPumpHour22" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPumpHour23" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPumpHour24" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPumpHour25" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtPumpHour26" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Literal ID="lblCrankCaseCamCaseInspection" runat="server" Text="CRANK CASE / CAM CASE INSPECTION<br />(HRS.)"></asp:Literal>
                                    </td>
                                    <td align="center">
                                       <asp:Literal ID="lblDate7" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtCrankDate1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtCrankDate2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtCrankDate3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtCrankDate4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtCrankDate5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtCrankDate6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSince11" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtCrankHour1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtCrankHour2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtCrankHour3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtCrankHour4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtCrankHour5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtCrankHour6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                       <asp:Literal ID="lblCrankshaftDeflection" runat="server" Text="CRANKSHAFT DEFLECTION<br />(ONCE IN 3 MONTHS)"></asp:Literal>
                                    </td>
                                    <td align="center">
                                       <asp:Literal ID="lblDate8" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtCrankDeflDate1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtCrankDeflDate2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtCrankDeflDate3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtCrankDeflDate4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtCrankDeflDate5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtCrankDeflDate6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSince12" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtCrankDeflHour1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtCrankDeflHour2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtCrankDeflHour3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtCrankDeflHour4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtCrankDeflHour5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtCrankDeflHour6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                       <asp:Literal ID="lblScavengeCleaningInspection" runat="server" Text="SCAVENGE CLEANING/INSPECTION<br />(HRS.)"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbLDate9" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtScavengeDate1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtScavengeDate2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtScavengeDate3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtScavengeDate4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtScavengeDate5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtScavengeDate6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                       <asp:Literal ID="lblHoursSince13" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtScavengeHour1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtScavengeHour2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtScavengeHour3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtScavengeHour4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtScavengeHour5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtScavengeHour6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Literal ID="lblAlarmsTripsTryOut" runat="server" Text="ALARMS/TRIPS TRY OUT<br />(MONTHLY)"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDate21" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtAlarmDate1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtAlarmDate2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtAlarmDate3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtAlarmDate4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtAlarmDate5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtAlarmDate6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSince14" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAlarmHour1" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAlarmHour2" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAlarmHour3" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAlarmHour4" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAlarmHour5" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAlarmHour6" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Literal ID="lblAirCooler" runat="server" Text="AIR COOLER"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td rowspan="2">
                                       <asp:literal ID="lblLastCleaning" runat="server" Text="LAST CLEANED<br />(HRS)."></asp:literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDate10" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td rowspan="2" align="center">
                                        <asp:Literal ID="lbLAIRSide" runat="server" Text="AIR SIDE<br />"></asp:Literal>
                                        <br />
                                    </td>
                                    <td colspan="2" rowspan="2">
                                        <asp:TextBox ID="txtAirSide" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td rowspan="2" align="center">
                                        <asp:Literal ID="lblSwSide" runat="server" Text="SW SIDE<br />"></asp:Literal>
                                        <br />
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtSwSide" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                       <asp:Literal ID="lblHoursSince15" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirHour" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Literal ID="lblTurboCharger" runat="server" Text="TURBOCHARGER"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td rowspan="2">
                                        <asp:Literal ID="lblInspectionCleaningExh" runat="server" Text="INSPECTION/CLEANING EXH. INLET GRID<br />( ) CHECK COND."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDate11" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtInspectionDate" runat="server" CssClass="input" />
                                    </td>
                                    <td rowspan="2">
                                        <asp:Literal ID="lblAirFilter" runat="server" Text="AIR FILTER<br />( HRS)"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDate12" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtAirFilter" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSince16" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAirFilterHour" runat="server" CssClass="input" />
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblHRS" runat="server" Text="HR'S"></asp:Literal>
                                        <td>
                                            <eluc:Number ID="txtAirFilterHrs" runat="server" CssClass="input" />
                                        </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                       <asp:Literal ID="lblOverHaulCleaning" runat="server" Text="OVERHAUL & CLEANING.<br />( ) CHECK COND."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDate13" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtOverhaulDate" runat="server" CssClass="input" />
                                    </td>
                                    <td rowspan="2">
                                        <asp:Literal ID="lblTurbineSideClean" runat="server" Text="TURBINE SIDE CLEAN<br />( HRS)"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDate14" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtTurbineDate" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSince17" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtTurbineHour" runat="server" CssClass="input" />
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblHrs1" runat="server" Text="HR'S"></asp:Literal>
                                        <td>
                                            <eluc:Number ID="txtTurbineHRS" runat="server" CssClass="input" />
                                        </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Literal ID="lblGovernor" runat="server" Text="GOVERNOR"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td rowspan="2">
                                        <asp:Literal ID="lblGovernorLastOverhauled" runat="server" Text="GOVERNOR LAST OVERHAULED<br />( ) CHECK COND."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDate20" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtGovernorDate" runat="server" CssClass="input" />
                                    </td>
                                    <td rowspan="2">
                                       <asp:Literal ID="lblLastInspected" runat="server" Text="LAST INSPECTED"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDate16" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtLastInspectedDate" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblHoursSince18" runat="server" Text="HOURS SINCE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtLastHour" runat="server" CssClass="input" />
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblHrs2" runat="server" Text="HR'S"></asp:Literal>
                                        <td>
                                            <eluc:Number ID="Number100" runat="server" CssClass="input" />
                                        </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                       <asp:Literal ID="lblGovernorLastOilRenewed" runat="server" Text="GOVERNOR LAST OIL RENEWED"></asp:Literal>
                                    </td>
                                    <td align="center">
                                      <asp:Literal ID="lblDate17" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtOilRenewedDate" runat="server" CssClass="input" />
                                    </td>
                                    <td rowspan="2">
                                       <asp:Literal ID="lblLastTightened" runat="server" Text="LAST TIGHTENED<br />"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblDate18" runat="server" Text="DATE"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtTightenedDate" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:literal ID="lblHoursSince19" runat="server" Text="HOURS SINCE"></asp:literal>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtTightenedHour" runat="server" CssClass="input" />
                                    </td>
                                    <td align="center">
                                       <asp:literal ID="lblHrs3" runat="server" Text="HR'S"></asp:literal>
                                        <td>
                                            <eluc:Number ID="txtTightenedHRS" runat="server" CssClass="input" />
                                        </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:TextBox ID="txtSecondEngineer" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td colspan="3" align="center">
                            <asp:TextBox ID="txtChiefEngineer" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <asp:Literal ID="lblSecondEngineer" runat="server" Text="Second Engineer"></asp:Literal>
                        </td>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td colspan="3" align="center">
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
