<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormAEPerformanceReport.aspx.cs"
    Inherits="StandardFormAEPerformanceReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>AE Performance Report</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAePerformane" runat="server" autocomplete="off">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="AE Performance Report" ShowMenu="false">
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
                            <b><asp:Literal ID="lblCompanyName" runat="server" Text="EXECUTIVE SHIP MANAGEMENT"></asp:Literal></b>
                        </td>
                        <td align="left">
                          
                        </td>
                        <td align="right">
                            <asp:literal ID="lblE21" runat ="server" Text="E21"></asp:literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left">
                          <%--  <b><asp:literal ID="lblSingapore" runat="server" Text="SINGAPORE"></asp:literal></b>--%>
                        </td>
                        <td align="left">
                         
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Literal ID="lbl0714Rev1" runat="server" Text="(07/14 Rev 1)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <h2>
                               <asp:Literal ID="lblAEPerformanceReport" runat="server" Text="AE Performance Report"></asp:Literal>
                            </h2>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtvesselName" runat="server" Enabled="false"  CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:literal ID="lblAE" runat="server" Text="A/E"></asp:literal>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtAuxEngine" runat="server" Enabled="false"  CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblTypeofEngine" runat="server" Text="Type of Engine"></asp:Literal>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtEngineTye" runat="server" Enabled="false"  CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lbltypeofTC" runat="server" Text="Type of T/C"></asp:Literal>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtTypeofTC" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td>
                           <asp:Literal ID="lblPs" runat="server" Text="Ps"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPs" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblRPM" runat="server" Text="R.P.M"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txthRpm1" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblKW" runat="server" Text="KW"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKW" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <table width="100%" class="Sftblclass" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td colspan="3">
                                        <asp:Literal ID="lblDate" Runat="server" Text="Date"></asp:Literal>
                                    </td>
                                    <td>
                                     <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                                    </td>
                                    <td rowspan="3" align="center">
                                        <div class="verticaltext">
                                           <asp:Literal ID="lblTemp" runat="server" Text="Temp"></asp:Literal>
                                        </div>
                                    </td>
                                    <td rowspan="2">
                                       <asp:Literal ID="lblExhGasBeforeTC" runat="server" Text="Exh. Gas<br />Before T/C"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblOne" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGas1" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="3" align="center">
                                        <div class="verticaltext">
                                            <asp:Literal ID="lblTemp1" runat="server" Text="Temp"></asp:Literal>
                                        </div>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblSeaWater" runat="server" Text="Sea Water"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSeaWater" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="lbl2" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGas2" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblEngineRoom" runat="server" Text="Engine Room"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEngineRoom" runat="server" CssClass="input" />
                                    </td>
                                    <td colspan="2">
                                       <asp:Literal ID="lblAfterTc" runat="server" Text="After T/C"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAfterTC" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblAtomsAir" runat="server" Text="Atoms Air"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAtomAir" runat="server" CssClass="input" />
                                    </td>
                                    <td rowspan="6" align="center" colspan="2">
                                        <asp:Literal ID="lblPMAXPCOMP" runat="server" Text="PMAX/PCOMP"></asp:Literal>
                                    </td>
                                    <td>
                                       <asp:Literal ID="lbl1" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPMax" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                       <asp:Literal ID="lblLoadKWLoad" runat="server" Text="Load – KW / % Load"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLoadkw" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                       <asp:Literal ID="lbltwo" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLoadkw1" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                       <asp:Literal ID="lblLoadAMP" runat="server" Text="Load – AMP"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLoadAmp" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="lbl3" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLoadAmp1" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Literal ID="lblRPM1" runat="server" Text="R.P.M."></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRpm" runat="server" CssClass="input" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="lbl4" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRpm1" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="11" align="center">
                                        <div class="verticaltext">
                                            <asp:Literal ID="lblPressure" runat="server" Text="Pressure"></asp:Literal>
                                        </div>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblScavAir" runat="server" Text="Scav. Air"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtScavAir" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lbl5" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtScaAir1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:literal ID="lblAirCirDiffP" runat="server" Text="Air Clr. Diff. P."></asp:literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCir" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lbl6" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCir1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblCool" runat="server" Text="Cool F/W"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCoolFW" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center" rowspan="6" colspan="2">
                                        <asp:literal ID="lblIndicateofFOPumpRack" runat="server" Text="Indicate of<br />F.O. Pump Rack"></asp:literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblFopumprackone" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPumpRack" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblFOVVCoolFW" runat="server" Text="F.O. v/v Cool F/W"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVCool" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lbl22" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtVCool1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblRockerArmLo" runat="server" Text="Rocker Arm L.O."></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRockerArm1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblthree" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRockerArm2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblFOInlet" runat="server" Text="F.O. Inlet"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInlet1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblfour" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInlet4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lblLOInlet" runat="server" Text="L.O. Inlet"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLoInlet" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblfive" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLOinlet5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" colspan="2">
                                        <asp:Literal ID="lblDifferPressofLOFilter" runat="server" Text="Differ Press of<br />L.O. Filter"></asp:Literal>
                                    </td>
                                    <td rowspan="2">
                                        <asp:TextBox ID="txtPress" runat="server" CssClass="input"></asp:TextBox><br />
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblsix" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPress6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Literal ID="lblFOConsumpMTDay" runat="server" Text="F.O. Consump MT/Day"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConsump" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" colspan="2">
                                        <asp:Literal ID="lblLOInletofTC" runat="server" Text="L.O. Inlet of T/C<br />(After Filter)"></asp:Literal>
                                    </td>
                                    <td rowspan="2">
                                        <asp:TextBox ID="txtinlet" runat="server" CssClass="input"></asp:TextBox><br />
                                    </td>
                                    <td colspan="3">
                                        <asp:Literal ID="lblLOConsumpLDay" runat="server" Text="L.O. Consump L/Day"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLoConsump" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:Literal ID="lblTotalWorkingHrs" runat="server" Text="Total Working Hrs"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotHours" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="18" align="center">
                                        <div class="verticaltext">
                                            <asp:Literal ID="lblTemperature" runat="server" Text="Temperature"></asp:Literal>
                                        </div>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblFOInlet1" runat="server" Text="F.O. Inlet"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFOInlet" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td rowspan="7" align="center">
                                        <div class="verticaltext1">
                                            <asp:Literal ID="lblHrsSinceLast" runat="server" Text="Hrs. since Last<br />Overhaul"></asp:Literal>
                                        </div>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblPiston" runat="server" Text="Piston"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPiston" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2">
                                        <asp:Literal ID="lblLOCir" runat="server" Text="L.O.<br />Clr."></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblIn" runat="server" Text="In"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInLo" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblFOInjector" runat="server" Text="F.O. Injector"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFoInjector" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblOut" runat="server" Text="Out"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOut" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblFOInjectorPump" runat="server" Text="F.O. Injector Pump"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInjectorPump" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblJCFW" runat="server" Text="J.C.F.W"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblIn1" runat="server" Text="In"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtInJCFW" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblSucValue" runat="server" Text="Suc. Valve"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSucValve" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="6" align="center">
                                        <div class="verticaltext1">
                                           <asp:Literal ID="lblCoolJacketOutlet" runat="server" Text=" Cool&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;FW<br />Jacket<br />Outlet"></asp:Literal>
                                        </div>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lbl11" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCoolJacket1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblExhValue" runat="server" Text="Exh. Valve"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtExhValve" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lbltwo2" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtExhValve2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblTCCleaning" runat="server" Text="T/C Cleaning"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCleaning1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <asp:Literal ID="lbl33" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCleaning12" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                       <asp:Literal ID="lblTcRenewBrg" runat="server" Text="T/C Renew Brg."></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRenewBrg" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lbl44" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRenew4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td rowspan="5" colspan="2" align="center">
                                        <div class="verticaltext1">
                                            <asp:Literal ID="lblDateofLastOverHaul" runat="server" Text="Date of Last<br />Overhaul"></asp:Literal>
                                        </div>
                                    </td>
                                    <td>
                                       <asp:Literal ID="lblLOFilter" runat="server" Text="L.O. Filter (s)"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLoFilters" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lbl55" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLoFilter5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblLOFilter1" runat="server" Text="L.O. Filter (d)"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLoFilterd" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <asp:Literal ID="lblsix6" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLoFilter6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                       <asp:Literal ID="lblTCLOFilter" runat="server" Text="T/C LO Filter"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTCLOFilter" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                       <asp:Literal ID="lblScavAir1" runat="server" Text="Scav. Air"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtScavAir2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td rowspan="2">
                                        <asp:literal ID="lblFOFilter" runat="server" Text="F.O. Filter<br />(Engine – Side)"></asp:literal>
                                    </td>
                                    <td rowspan="2">
                                        <asp:TextBox ID="txtFoFilter" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="6">
                                        <asp:Literal ID="lblEXHGasAftCyl" runat="server" Text="Exh.<br />Gas<br />AFT/<br />Cyl."></asp:Literal>
                                    </td>
                                    <td>
                                       <asp:Literal ID="lblone1" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtExhGas1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <asp:Literal ID="lbl2two" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtExGas1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:Literal ID="lblGradeofFOCst" runat="server" Text="Grade of F.O. cst."></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGradeOfFO1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lbl3three" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGradeOfFO2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtGradeOfFO3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGradeOfFO4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <asp:Literal ID="lbl4four" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGradeOfF41" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtGradeOfF42" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGradeOfF43" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                      <asp:Literal ID="lbl5five" runat="server" Text=" 5"></asp:Literal>                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGradeOfF51" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtGradeOfF53" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGradeOfF52" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                       <asp:Literal ID="lbl6six" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGradeOfF61" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtGradeOfF62" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGradeOfF63" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Literal ID="lblNoter" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;Note : If no indicator cocks are fitted the rest of the
                            parameters other than Pmax and Pcomp to be<br />&nbsp;&nbsp;&nbsp;&nbsp;recorded at the maximum load possible."></asp:Literal>
                        </td>
                    </tr>
                     <tr>
                     <td colspan="7" align="right"> <asp:TextBox ID="txtNameOfChiefEngineer" runat="server"   CssClass="input"></asp:TextBox></td>
                    </tr>
                    <tr>
                         <td colspan="7" align="right"><asp:Literal ID="lblChiefEngineer" runat="server" Text="CHIEF ENGINEER"></asp:Literal></td>
                    </tr>
                </table>
                <br />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
