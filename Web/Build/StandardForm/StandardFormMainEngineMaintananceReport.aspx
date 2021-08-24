<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormMainEngineMaintananceReport.aspx.cs" Inherits="StandardForm_StandardFormMainEngineMaintananceReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Main Engine Maintanance Report</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
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
        .verticaltext
        {
            writing-mode: tb-rl;
            filter: flipv fliph; 
            width: 15px;
            height: 70px;
        }
        .verticaltext1
        {
            writing-mode: tb-rl;
            filter: flipv fliph;
            width: 70px;
            height: 70px;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function cmdPrint_Click()
        {            
            document.getElementById('cmdPrint').style.visibility = "hidden";
            window.print();
        }
    </script>
</telerik:RadCodeBlock></head>
<body>
    <form id="FrmMainEngineMaintananceReport" runat="server">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Me Inspection Through Scavenge Ports"
                ShowMenu="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="StandardForm_TabStripCommand" />
            <asp:Button ID="cmdHiddenConfirm" runat="server" Text="cmdHiddenConfirm" OnClick="StandardFormConfirm_TabStripCommand" />
       </div>        
         <asp:UpdatePanel runat="server" ID="pnlStandardForm">
            <ContentTemplate>
            <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server" visible="false" />
    <table width="95%">
    <tr>
     <td colspan="5" align="left">
                            <b><asp:Literal ID="lblExecutiveOffshore" runat="server" Text="EXECUTIVE OFFSHORE"></asp:Literal></b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="lblFileRef1512" runat="server" Text="File Ref – 151.2"></asp:Literal>
                        </td>
                        <td align="right" colspan="2">
                            <asp:Literal ID="lblE18" runat="server" Text="E18"></asp:Literal>
                        </td>
                    
    </tr>
    <tr>
                        <td colspan="5" align="left">
                            
                        </td>
                        <td align="left">
                            <asp:Literal ID="lblCE20" runat="server" Text="C/E 20"></asp:Literal>
                        </td >
                        <td align="right" colspan="2">
                            <asp:Literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Literal ID="lbl813Rev1" runat="server" Text="(8/13 Rev 1)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <h2>
                                <asp:Literal ID="lblMainEngineMaintenanceRecordPort" runat="server" Text="MAIN ENGINE MAINTENANCE RECORD PORT"></asp:Literal>
                            </h2>
                        </td>
                    </tr>
                    <tr>
                    <td  align="left">
                    <asp:Literal ID="lblMVMT" runat="server" Text="M.V. / M.T."></asp:Literal>
                    </td>
                     <td colspan="3" align="left">
                   <asp:TextBox ID="txtMVMT" runat="server"   CssClass="input"></asp:TextBox>
                    </td>
                    
                     <td colspan="3"  align="right">
                    <asp:Literal ID="lblDate" runat="server" Text="DATE"></asp:Literal>
                    </td>
                     <td colspan="3" align="left">
                   <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                
                    </td>
                    
                    </tr>
                    <tr>
                    <td  align="left">
                   <asp:Literal ID="lblMainEngine" runat="server" Text="MAIN ENGINE:"></asp:Literal>
                    </td>
                     <td align="left">
                   <asp:TextBox ID="txtMainEngine" runat="server"   CssClass="input"></asp:TextBox>
                    </td>
                    
                     <td  align="left">
                  <asp:Literal ID="lblPortSTBD" runat="server" Text="PORT/STBD"></asp:Literal>
                    </td>
                     <td align="left">
                   <asp:TextBox ID="txtPortorStbd" runat="server"   CssClass="input"></asp:TextBox>
                    </td>
                    
                     <td colspan="3"  align="right">
                    <asp:Literal ID="lblType" runat="server" Text="TYPE"></asp:Literal>
                    </td>
                     <td colspan="3" align="left">
                   <asp:TextBox ID="txtType" runat="server"   CssClass="input"></asp:TextBox>
                    </td>
                    
                    </tr>
                    <tr>
                    <td colspan="4">
                    <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                    <tr>
                    <td colspan="2">
                    <asp:Literal ID="lblMainEngineRunningHrsThisMonth" runat="server" Text="MAIN ENGINE RUNNING HRS THIS MONTH"></asp:Literal>
                    </td>
                    <td>
                   
                     <eluc:Number ID="txtMainEngineRunningHrs" runat="server"   CssClass="input" />
                    </td>
                    </tr>
                    <tr>
                    <td colspan="2">
                    <asp:Literal ID="lblMainEngineTotalRunningHours" runat="server" Text="MAIN ENGINE TOTAL RUNNING HOURS"></asp:Literal>
                    </td>
                    <td>
                    <eluc:Number ID="txtMainEngineTotalRunningHrs" runat="server"   CssClass="input" />
                    </td>
                    </tr>
                    </table>
                    </td>
                    </tr>
                   
                    </table>
                    <br />
                    <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                    <tr>
                    <td colspan="2">
                    <asp:Literal ID="lblOverHaulActivityHrs" runat="server" Text="Overhaul activity/Hrs as <br/> per maker's manual STANDARD HRS."></asp:Literal>             </td>
                    
                    <td>
                    <asp:literal ID="lblStatus" runat="server" Text="STATUS"></asp:literal>
                    </td>
                    <td>
                    <asp:literal ID="lblUnit1A" runat="server" Text="UNIT 1A"></asp:literal>
                    </td>
                    <td>
                    <asp:Literal ID="lblUnit2A" runat="server" Text="UNIT 2A"></asp:Literal>
                    </td>
                    <td>
                    <asp:Literal ID="lblUnit3A" runat="server" Text="UNIT 3A"></asp:Literal>
                    </td>
                    <td>
                    <asp:Literal ID="lblUnit4A" runat="server" Text="UNIT 4A"></asp:Literal>
                    </td>
                    <td>
                    <asp:Literal ID="lblUnit5A" runat="server" Text="UNIT 5A"></asp:Literal>
                    </td>
                    <td>
                    <asp:Literal ID="lblUnit6A" runat="server" Text="UNIT 6A"></asp:Literal>
                    </td>
                    <td>
                    <asp:Literal ID="lblUnit1B" runat="server" Text="UNIT 1B"></asp:Literal>
                    </td>
                    
                 
                    <td>
                    <asp:Literal ID="lblUnit2B" runat="server" Text="UNIT 2B"></asp:Literal>
                    </td>
                    <td>
                    <asp:Literal ID="lblUnit3B" runat="server" Text="UNIT 3B"></asp:Literal>
                    </td>
                    <td>
                    <asp:Literal ID="lblUnit4B" runat="server" Text="UNIT 4B"></asp:Literal>
                    </td>
                    <td>
                    <asp:Literal ID="lblUnit5B" runat="server" Text="UNIT 5B"></asp:Literal>
                    </td>
                    <td>
                    <asp:Literal ID="lblUnit6B" runat="server" Text="UNIT 6B"></asp:Literal>
                    </td>
                                    
                    </tr>
                    <tr>
                    <td >
                    <asp:Literal ID="lblLastDecarb" runat="server" Text="LAST DECARB."></asp:Literal></td><td></td>
                    
                    <td>
                   <asp:Literal ID="lblDate1" runat="server" Text="DATE"></asp:Literal>
                    </td>
                    <td>
                    <eluc:Date ID="txtLDDateU1A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtLDDateU2A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                   <eluc:Date ID="txtLDDateU3A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtLDDateU4A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtLDDateU5A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtLDDateU6A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtLDDateU1B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    
                 
                    <td>
                    <eluc:Date ID="txtLDDateU2B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtLDDateU3B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtLDDateU4B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtLDDateU5B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtLDDateU6B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                                    
                    </tr>
                    <tr>
                    <td >
                    <asp:Literal ID="lblDecarbHrsAsPerManual" runat="server" Text="Decarb Hrs as per manual:"></asp:Literal>
                    </td>
                   <td>
                   <eluc:Number ID="txtDecarbHrsAsPerManual" runat="server" CssClass="input"/>
                   </td> 
                    <td>
                    <asp:LIteral ID="lblHoursSince" runat="server" Text="HOURS SINCE"></asp:LIteral>
                    </td>
                    <td>
                    <eluc:Number ID="txtLDHrsU1A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtLDHrsU2A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtLDHrsU3A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtLDHrsU4A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtLDHrsU5A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtLDHrsU6A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtLDHrsU1B" runat="server"   CssClass="input"/>
                    </td>
                    
                 
                    <td>
                    <eluc:Number ID="txtLDHrsU2B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtLDHrsU3B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtLDHrsU4B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtLDHrsU5B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtLDHrsU6B" runat="server"   CssClass="input" />
                    </td>
                                    
                    </tr>
                    
                    
                    <tr>
                    <td >
                    <asp:Literal ID="lblExhValueOhl" runat="server" Text="EXH. VALVE O'HL"></asp:Literal></td>
                    <td></td>
                    <td>
                    <asp:Literal ID="lblDate2" runat="server" Text="DATE"></asp:Literal>
                    </td>
                    <td>
                    <eluc:Date ID="txtEVODateU1A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtEVODateU2A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                   <eluc:Date ID="txtEVODateU3A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtEVODateU4A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtEVODateU5A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtEVODateU6A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtEVODateU1B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    
                 
                    <td>
                    <eluc:Date ID="txtEVODateU2B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtEVODateU3B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtEVODateU4B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtEVODateU5B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtEVODateU6B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                                    
                    </tr>
                    <tr>
                    <td >
                    <asp:Literal ID="lblOhaulHrsasPerManual" runat="server" Text="Ohaul Hrs as per manual:"></asp:Literal>
                    </td>
                   <td>
                   <eluc:Number ID="txtOhaulHrsAsPerManual" runat="server" CssClass="input"/>
                   </td> 
                    <td>
                    <asp:literal ID="lblHoursSince1" runat="server" Text="HOURS SINCE"></asp:literal>
                    </td>
                    <td>
                    <eluc:Number ID="txtEVOOHrsU1A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtEVOOHrsU2A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtEVOOHrsU3A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtEVOOHrsU4A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtEVOOHrsU5A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtEVOOHrsU6A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtEVOOHrsU1B" runat="server"   CssClass="input"/>
                    </td>
                    
                 
                    <td>
                    <eluc:Number ID="txtEVOOHrsU2B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtEVOOHrsU3B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtEVOOHrsU4B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtEVOOHrsU5B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtEVOOHrsU6B" runat="server"   CssClass="input"/>
                    </td>
                                    
                    </tr>
                    
                    
                    <tr>
                    <td >
                   <asp:Literal ID="lblFuelInjectorsLastPrTest" runat="server" Text="FUEL INJECTOR'S (last pr.test)"></asp:Literal></td>
                    <td></td>
                    <td>
                    <asp:Literal ID="lblDate3" runat="server" Text="DATE"></asp:Literal>
                    </td>
                    <td>
                    <eluc:Date ID="txtFuelInjectorDateU1A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFuelInjectorDateU2A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                   <eluc:Date ID="txtFuelInjectorDateU3A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFuelInjectorDateU4A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFuelInjectorDateU5A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFuelInjectorDateU6A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFuelInjectorDateU1B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    
                 
                    <td>
                    <eluc:Date ID="txtFuelInjectorDateU2B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFuelInjectorDateU3B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFuelInjectorDateU4B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFuelInjectorDateU5B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFuelInjectorDateU6B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                                    
                    </tr>
                    <tr>
                    <td >
                    <asp:Literal ID="lblOhaulHrsAsPerManual1" runat="server" Text="Ohaul Hrs as per manual:"></asp:Literal>
                    </td>
                   <td>
                  <eluc:Number ID="txtFuelOhaulHrsAsPerManual" runat="server" CssClass="input"/>
                   </td> 
                    <td>
                    <asp:Literal ID="lblHoursSince12" runat="server" Text="HOURS SINCE"></asp:Literal>
                    </td>
                    <td>
                    <eluc:Number ID="txtFuelOHrsU1A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtFuelOHrsU2A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                  <eluc:Number ID="txtFuelOHrsU3A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtFuelOHrsU4A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtFuelOHrsU5A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtFuelOHrsU6A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtFuelOHrsU1B" runat="server"   CssClass="input" />
                    </td>
                    
                 
                    <td>
                    <eluc:Number ID="txtFuelOHrsU2B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtFuelOHrsU3B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtFuelOHrsU4B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtFuelOHrsU5B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtFuelOHrsU6B" runat="server"   CssClass="input"/>
                    </td>
                                    
                    </tr>
                    
                    
                    <tr>
                    <td>
                    <asp:Literal ID="lblstAirValueOhl" runat="server" Text="ST. AIR Valve O'HL"></asp:Literal></td>
                    <td></td>
                    <td>
                    <asp:literal ID="lblDate4" runat="server" Text="DATE"></asp:literal>
                    </td>
                    <td>
                    <eluc:Date ID="txtSTAVDateU1A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSTAVDateU2A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                   <eluc:Date ID="txtSTAVDateU3A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSTAVDateU4A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSTAVDateU5A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSTAVDateU6A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSTAVDateU1B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    
                 
                    <td>
                    <eluc:Date ID="txtSTAVDateU2B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSTAVDateU3B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSTAVDateU4B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSTAVDateU5B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSTAVDateU6B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                                    
                    </tr>
                    <tr>
                    <td >
                    <asp:Literal ID="lblOhaulHrsAsPerManual2" runat="server" Text="Ohaul Hrs as per manual:"></asp:Literal>
                    </td>
                   <td>
                   <eluc:Number ID="txtSTAVOHrsAsPerManual" runat="server" CssClass="input"/>
                   </td> 
                    <td>
                    <asp:Literal ID="lblHoursSince2" runat="server" Text="HOURS SINCE"></asp:Literal>
                    </td>
                    <td>
                    <eluc:Number ID="txtSTAVHrsU1A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtSTAVHrsU2A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtSTAVHrsU3A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtSTAVHrsU4A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtSTAVHrsU5A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtSTAVHrsU6A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtSTAVHrsU1B" runat="server"   CssClass="input"/>
                    </td>
                    
                 
                    <td>
                    <eluc:Number ID="txtSTAVHrsU2B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtSTAVHrsU3B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtSTAVHrsU4B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtSTAVHrsU5B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtSTAVHrsU6B" runat="server"   CssClass="input"/>
                    </td>
                                    
                    </tr>
                    
                    
                    <tr>
                    <td >
                    <asp:Literal ID="lblIndicatorValve" runat="server" Text="INDICATOR VALVE"></asp:Literal></td>
                    <td></td>
                    <td>
                    <asp:Literal ID="lblDate5" runat="server" Text="DATE"></asp:Literal>
                    </td>
                    <td>
                    <eluc:Date ID="txtIVDateU1A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtIVDateU2A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                   <eluc:Date ID="txtIVDateU3A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtIVDateU4A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtIVDateU5A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtIVDateU6A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtIVDateU1B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    
                 
                    <td>
                    <eluc:Date ID="txtIVDateU2B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtIVDateU3B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtIVDateU4B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtIVDateU5B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtIVDateU6B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                                    
                    </tr>
                    <tr>
                    <td >
                    <asp:Literal ID="lblOhaulHrsAsPerManual3" runat="server" Text="Ohaul Hrs as per manual:"></asp:Literal>
                    </td>
                   <td>
                  <eluc:Number ID="txtIVOHrsAsPerManual" runat="server" CssClass="input"/>
                   </td> 
                    <td>
                    <asp:Literal ID="lblHoursSince3" runat="server" Text="HOURS SINCE"></asp:Literal>                    </td>
                    <td>
                   <eluc:Number ID="txtIVOHrsU1A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtIVOHrsU2A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtIVOHrsU3A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtIVOHrsU4A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtIVOHrsU5A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtIVOHrsU6A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtIVOHrsU1B" runat="server"   CssClass="input"/>
                    </td>
                    
                 
                    <td>
                    <eluc:Number ID="txtIVOHrsU2B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtIVOHrsU3B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtIVOHrsU4B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtIVOHrsU5B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtIVOHrsU6B" runat="server"   CssClass="input"/>
                    </td>
                                    
                    </tr>
                    
                    <tr>
                    <td >
                    <asp:Literal ID="lblSafetyValve" runat="server" Text="SAFETY VALVE"></asp:Literal></td>
                    <td></td>
                    <td>
                    <asp:Literal ID="lblDate6" runat="server" Text="DATE"></asp:Literal>
                    </td>
                    <td>
                    <eluc:Date ID="txtSVDateU1A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSVDateU2A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                   <eluc:Date ID="txtSVDateU3A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSVDateU4A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSVDateU5A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSVDateU6A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSVDateU1B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    
                 
                    <td>
                    <eluc:Date ID="txtSVDateU2B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSVDateU3B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSVDateU4B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSVDateU5B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtSVDateU6B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                                    
                    </tr>
                    <tr>
                    <td >
                    <asp:Literal ID="lblOhaulHrsAsPerManual4" runat="server" Text="Ohaul Hrs as per manual:"></asp:Literal>
                    </td>
                   <td>
                   <eluc:Number ID="txtSVOHrsAsPerManual" runat="server" CssClass="input"/>
                   </td> 
                    <td>
                    <asp:Literal ID="lblHoursSince4" runat="server" Text="HOURS SINCE"></asp:Literal>
                    </td>
                    <td>
                    <eluc:Number ID="txtSVOHrsU1A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtSVOHrsU2A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtSVOHrsU3A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtSVOHrsU4A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtSVOHrsU5A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtSVOHrsU6A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtSVOHrsU1B" runat="server"   CssClass="input"/>
                    </td>
                    
                 
                    <td>
                    <eluc:Number ID="txtSVOHrsU2B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtSVOHrsU3B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtSVOHrsU4B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtSVOHrsU5B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtSVOHrsU6B" runat="server"   CssClass="input"/>
                    </td>
                                    
                    </tr>
                    
                    
                    <tr>
                    <td >
                    <asp:Literal ID="lblFuelPumpOverHaul" runat="server" Text="FUEL PUMP OVERHAUL"></asp:Literal></td>
                    <td></td>
                    <td>
                    <asp:literal ID="lblDate7" runat="server" Text="DATE"></asp:literal>
                    </td>
                    <td>
                    <eluc:Date ID="txtFPODateU1A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFPODateU2A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                   <eluc:Date ID="txtFPODateU3A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFPODateU4A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFPODateU5A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFPODateU6A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFPODateU1B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    
                 
                    <td>
                    <eluc:Date ID="txtFPODateU2B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFPODateU3B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFPODateU4B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFPODateU5B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtFPODateU6B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                                    
                    </tr>
                    <tr>
                    <td >
                    <asp:Literal ID="lblOhaulHrsAsPerManual5" runat="server" Text="Ohaul Hrs as per manual:"></asp:Literal>
                    </td>
                   <td>
                   <eluc:Number ID="txtFPOHrsAsPerManual" runat="server" CssClass="input"/>
                   </td> 
                    <td>
                    <asp:Literal ID="lblHoursSince5" runat="server" Text="HOURS SINCE"></asp:Literal>
                    </td>
                    <td>
                    <eluc:Number ID="txtFPOHrsU1A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtFPOHrsU2A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtFPOHrsU3A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtFPOHrsU4A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtFPOHrsU5A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtFPOHrsU6A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtFPOHrsU1B" runat="server"   CssClass="input"/>
                    </td>
                    
                 
                    <td>
                    <eluc:Number ID="txtFPOHrsU2B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtFPOHrsU3B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtFPOHrsU4B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtFPOHrsU5B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtFPOHrsU6B" runat="server"   CssClass="input"/>
                    </td>
                                    
                    </tr>
                    
                    
                    
                    <tr>
                    <td >
                    <asp:Literal ID="lblCrankCaseInspection" runat="server" Text="CRANK CASE INSPECTION"></asp:Literal></td>
                    <td></td>
                    <td>
                    <asp:Literal ID="lblDate8" runat="server" Text="DATE"></asp:Literal>
                    </td>
                    <td>
                    <eluc:Date ID="txtCCIDateU1A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCCIDateU2A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                   <eluc:Date ID="txtCCIDateU3A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCCIDateU4A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCCIDateU5A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCCIDateU6A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCCIDateU1B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    
                 
                    <td>
                    <eluc:Date ID="txtCCIDateU2B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCCIDateU3B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCCIDateU4B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCCIDateU5B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCCIDateU6B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                                    
                    </tr>
                    <tr>
                    <td >
                    <asp:Literal ID="lblDueHrsTimeAsPerManual" runat="server" Text="Due Hrs/time as per manual:"></asp:Literal>
                    </td>
                   <td>
                  <eluc:Number ID="txtCCIDueHrsAsPerManual" runat="server" CssClass="input"/>
                   </td> 
                    <td>
                    <asp:Literal ID="lblHoursSince6" runat="server" Text="HOURS SINCE"></asp:Literal>
                    </td>
                    <td>
                    <eluc:Number ID="txtCCIDueHrsU1A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtCCIDueHrsU2A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtCCIDueHrsU3A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtCCIDueHrsU4A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtCCIDueHrsU5A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtCCIDueHrsU6A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtCCIDueHrsU1B" runat="server"   CssClass="input"/>
                    </td>
                    
                 
                    <td>
                    <eluc:Number ID="txtCCIDueHrsU2B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtCCIDueHrsU3B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtCCIDueHrsU4B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtCCIDueHrsU5B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtCCIDueHrsU6B" runat="server"   CssClass="input"/>
                    </td>
                                    
                    </tr>
                    
                    
                    <tr>
                    <td>
                    <asp:Literal ID="lblcrankShaftDeflection" runat="server" Text="CRANKSHAFT DEFLECTION"></asp:Literal></td>
                    <td></td>
                    <td>
                   <asp:Literal ID="lblDate9" runat="server" Text="DATE"></asp:Literal>
                    </td>
                    <td>
                    <eluc:Date ID="txtCDDateU1A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCDDateU2A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                   <eluc:Date ID="txtCDDateU3A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCDDateU4A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCDDateU5A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCDDateU6A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCDDateU1B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    
                 
                    <td>
                    <eluc:Date ID="txtCDDateU2B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCDDateU3B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCDDateU4B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCDDateU5B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtCDDateU6B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                                    
                    </tr>
                    <tr>
                    <td >
                   <asp:Literal ID="lbl3Month4000Hrs" runat="server" Text="3 month / 4000 hrs"></asp:Literal>
                    </td>
                    <td></td>
                    
                    <td>
                    <asp:Literal ID="lblHoursSince7" runat="server" Text="HOURS SINCE"></asp:Literal>
                    </td>
                    <td>
                    <eluc:Number ID="txt3monthor4000HrsU1A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txt3monthor4000HrsU2A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txt3monthor4000HrsU3A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txt3monthor4000HrsU4A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txt3monthor4000HrsU5A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txt3monthor4000HrsU6A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txt3monthor4000HrsU1B" runat="server"   CssClass="input"/>
                    </td>
                    
                 
                    <td>
                   <eluc:Number ID="txt3monthor4000HrsU2B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txt3monthor4000HrsU3B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txt3monthor4000HrsU4B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txt3monthor4000HrsU5B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txt3monthor4000HrsU6B" runat="server"   CssClass="input"/>
                    </td>
                                    
                    </tr>
                    
                    
                    <tr>
                    <td >
                    <asp:Literal ID="lblAlarmsTripsTryOut" runat="server" Text="ALARMS/TRIPS TRY OUT"></asp:Literal></td><td></td>
                    
                    <td>
                    <asp:Literal ID="lblDate10" runat="server" Text="DATE"></asp:Literal>
                    </td>
                    <td>
                    <eluc:Date ID="txtAlarmorTripDateU1A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtAlarmorTripDateU2A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                   <eluc:Date ID="txtAlarmorTripDateU3A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtAlarmorTripDateU4A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtAlarmorTripDateU5A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtAlarmorTripDateU6A" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtAlarmorTripDateU1B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    
                 
                    <td>
                    <eluc:Date ID="txtAlarmorTripDateU2B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtAlarmorTripDateU3B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtAlarmorTripDateU4B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtAlarmorTripDateU5B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                    <eluc:Date ID="txtAlarmorTripDateU6B" runat="server"  CssClass="input" DatePicker="true" />
                    </td>
                                    
                    </tr>
                    <tr>
                    <td >
                   <asp:Literal ID="lblMonthly" runat="server" Text="(MONTHLY)"></asp:Literal>
                    </td>
                    <td></td>
                    <td>
                    <asp:Literal ID="lblHoursSince8" runat="server" Text="HOURS SINCE"></asp:Literal>
                    </td>
                    <td>
                    <eluc:Number ID="txtMonthlyHrsU1A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtMonthlyHrsU2A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                   <eluc:Number ID="txtMonthlyHrsU3A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtMonthlyHrsU4A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtMonthlyHrsU5A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtMonthlyHrsU6A" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtMonthlyHrsU1B" runat="server"   CssClass="input"/>
                    </td>
                    
                 
                    <td>
                    <eluc:Number ID="txtMonthlyHrsU2B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtMonthlyHrsU3B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtMonthlyHrsU4B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtMonthlyHrsU5B" runat="server"   CssClass="input"/>
                    </td>
                    <td>
                    <eluc:Number ID="txtMonthlyHrsU6B" runat="server"   CssClass="input"/>
                    </td>
                                    
                    </tr>
                    </table>
                    <br />  
                    <table class="Sftblclass" cellpadding="1" cellspacing="1">
           <tr>
                    <td colspan="8">
                    <asp:Literal ID="lblAirCooler" runat="server" Text="AIR COOLER"></asp:Literal>
                    </td>
                    
                    </tr>      
                    <tr>
                   
                   
                    
                    
                    <td colspan="2"><asp:Literal ID="lblLastCleaned" runat="server" Text="LAST CLEANED"></asp:Literal></td>
                     <td><asp:Literal ID="lblDate11" runat="server" Text="DATE"></asp:Literal></td>
                     <td><eluc:Date ID="txtAirCoolerLastCleanedDateU1A" runat="server"  CssClass="input" DatePicker="true" /></td>
                     <td><eluc:Date ID="txtAirCoolerLastCleanedDateU2A" runat="server"  CssClass="input" DatePicker="true" /></td>
                     <td><asp:Literal ID="lblSWSide" runat="server" Text="SW SIDE"></asp:Literal> </td>
                     <td> <eluc:Date ID="txtSWSide1" runat="server"  CssClass="input" DatePicker="true" /></td>
                     <td> <eluc:Date ID="txtSWSide2" runat="server"  CssClass="input" DatePicker="true" /></td>
                    </tr>
                    
                    <tr>
                    <td><asp:Literal ID="lblDueHrsAsPerManual" runat="server" text="Due Hrs as per manual:"></asp:Literal></td>
                     <td><eluc:Number ID="txtAirCoolerLastCleanedHrsAsPerManual" runat="server"   CssClass="input"/></td>
                     <td><asp:Literal ID="lblHoursSince9" runat="server" Text="HOURS SINCE"></asp:Literal></td>
                     <td><eluc:Number ID="txtAirCoolerLAstCleanedHrs1" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtAirCoolerLAstCleanedHrs2" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtAirCoolerLAstCleanedHrs3" runat="server"   CssClass="input"/></td>
                     <td><eluc:Number ID="txtAirCoolerLAstCleanedHrs4" runat="server"   CssClass="input"/> </td>
                     <td> <eluc:Number ID="txtAirCoolerLAstCleanedHrs5" runat="server"   CssClass="input"/></td>
                    </tr>
                    </table>
                 
                    <br />
                    <table class="Sftblclass" cellpadding="1" cellspacing="1">
                    
                    <tr >
                    <td colspan="5">
                    <asp:Literal ID="lblTurboCharger" runat="server" Text="TURBOCHARGER"></asp:Literal>
                    </td>
                    
                    </tr>
                                       
                    <tr>
                    <td colspan="2">
                    <asp:Literal ID="lblInspectedCleaningExh" runat="server" Text="INSPECT/CLEANING EXH. INLET GRID"></asp:Literal>
                    </td>
                    
                    <td>
                    <asp:literal ID="lblDate12" runat="server" Text="DATE"></asp:literal>
                    </td>
                   
                    <td><eluc:Date ID="txtTurboChangerInspectDateU1A" runat="server" DatePicker="true"  CssClass="input"  /></td>
                  
                    
                    <td><eluc:Date ID="txtTurboChangerInspectDateU2A" runat="server" DatePicker="true"  CssClass="input" /></td>
                    
                    </tr>
                    
                    
                    <tr>
                    <td>
                    <asp:literal ID="lblDueHrsAsPerManual1" runat="server" Text="Due Hrs as per manual:"></asp:literal>
                    </td>
                    
                    <td><eluc:Number ID="txtTurboChangerInspectDueHrsAsPerManual" runat="server"   CssClass="input"/></td>
                  
                    <td>
                    <asp:Literal ID="lblHoursSince10" runat="server" Text="HOURS SINCE"></asp:Literal>
                    </td>
                    
                    <td><eluc:Number ID="txtTurboChangerInspectDueHrsU1A" runat="server"   CssClass="input"/></td>
                    
                    
                    <td><eluc:Number ID="txtTurboChangerInspectDueHrsU2A" runat="server"   CssClass="input"/></td>
                    
                    </tr>
                    <tr>
                    <td colspan="2">
                    <asp:Literal ID="lblOverHaulCleaning" runat="server" Text="OVERHAUL & CLEANING"></asp:Literal>
                    </td>
                   
                    <td>
                    <asp:Literal ID="lblDate13" runat="server" Text="DATE"></asp:Literal>
                    </td>
                   
                    <td><eluc:Date ID="txtOverHaulDateU1A" runat="server" CssClass="input" DatePicker="true" /></td>
                   
                    
                    <td><eluc:Date ID="txtOverHaulDateU2A" runat="server"  CssClass="input" DatePicker="true" /></td>
                    
                    </tr>
                    <tr>
                    <td>
                    <asp:Literal ID="lblDueHrsAsPerManual2" runat="server" Text="Due Hrs as per manual:"></asp:Literal>
                    </td>
                     
                    <td><eluc:Number ID="txtOverHaulHrsAsPerManual" runat="server"   CssClass="input"/></td>
                   
                    <td>
                    <asp:Literal ID="lblHoursSince11" runat="server" Text="HOURS SINCE"></asp:Literal>
                    </td>
                    
                    <td><eluc:Number ID="txtOverHaulHrsU1A" runat="server"   CssClass="input"/></td>
                   
                    
                    <td><eluc:Number ID="txtOverHaulHrsU2A" runat="server"   CssClass="input"/></td>
                    
                    </tr>
                    </table>
                    <br />  
                    <table class="Sftblclass" cellpadding="1" cellspacing="1">
                    <tr>
                    <td colspan="5">
                <asp:Literal ID="lblGovernor" runat="server" Text="GOVERNOR"></asp:Literal>
                    </td>
                    
                    </tr>
                    
                    
                    
                    
                    <tr>
                  
                    <td colspan="2">
                    <asp:Literal ID="lblGovernorLastOverHauled" runat="server" Text="GOVERNOR LAST OVERHAULED"></asp:Literal>
                    </td>
                  
                    <td>
                    <asp:Literal ID="lblDate14" runat="server" Text="DATE"></asp:Literal>
                    </td>
                   
                    <td><eluc:Date ID="txtGovernorLastOverHauledDateU1A" runat="server" DatePicker="true"  CssClass="input" /></td>
                    
                    
                    <td><eluc:Date ID="txtGovernorLastOverHauledDateU2A" runat="server" DatePicker="true"  CssClass="input" /></td>
                    
                    </tr>
                    
                    
                    <tr>
                    <td>
                    <asp:Literal ID="lblDueHrsAsPerManual3" runat="server" Text="Due Hrs as per manual:"></asp:Literal>
                    </td>
                    
                    <td><eluc:Number ID="txtGovernorLastOverHauledHrsAsPerManual" runat="server"   CssClass="input"/></td>
                   
                    <td>
                    <asp:Literal ID="lblHoursSInce13" runat="server" text="HOURS SINCE"></asp:Literal>
                    </td>
                    
                    <td><eluc:Number ID="txtGovernorLastOverHauledHrsU1A" runat="server"   CssClass="input"/></td>
                   
                    
                    <td><eluc:Number ID="txtGovernorLastOverHauledHrsU2A" runat="server"   CssClass="input"/></td>
                    
                    </tr>
                    <tr>
                    <td colspan="2">
                    <asp:Literal ID="lblGovernorLastOilRenewed" runat="Server" Text="GOVERNOR LAST OIL RENEWED"></asp:Literal>
                    </td>
                   
                    <td>
                   <asp:Literal ID="lblDate15" runat="server" Text="DATE"></asp:Literal>
                    </td>
                   
                    <td><eluc:Date ID="txtGovernorLastOilRenewedDateU1A" runat="server" DatePicker="true"  CssClass="input" /></td>
                    
                    
                    <td><eluc:Date ID="txtGovernorLastOilRenewedDateU2A" runat="server" DatePicker="true"   CssClass="input" /></td>
                    
                    </tr>
                    <tr>
                    <td>
                    <asp:Literal ID="lblDueHrsAsperManual4" runat="server" Text="Due Hrs as per manual:"></asp:Literal>
                    </td>
                     
                    <td><eluc:Number ID="txtGovernorLastOilRenewedHrsAsPerManual" runat="server"   CssClass="input"/></td>
                   
                    <td>
                    <asp:Literal ID="lblHoursSince14" runat="Server" Text="HOURS SINCE"></asp:Literal>
                    </td>
                    
                    <td><eluc:Number ID="txtGovernorLastOilRenewedHrsU1A" runat="server"   CssClass="input"/></td>
                   
                    
                    <td><eluc:Number ID="txtGovernorLastOilRenewedHrsU2A" runat="server"   CssClass="input"/></td>
                    
                    </tr>
                   
                    </table>
              
                    <table width="100%" >
    <tr>
    <td colspan="4"> <asp:TextBox ID="txtNameOfSecondEngineer" runat="server"   CssClass="input"></asp:TextBox></td>
    <td colspan="4"> <asp:TextBox ID="txtNameOfChiefEngineer" runat="server"   CssClass="input"></asp:TextBox></td>
    </tr>
    <br /><br />
    <tr></tr>
     <tr><td colspan="4"><asp:Literal ID="lblSecondEngineer" runat="server" Text="Second Engineer"></asp:Literal></td><td colspan="4"><asp:Literal ID="lblChiefEngineer" runat="server" Text="Chief engineer"></asp:Literal></td> </tr></table>
    </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
