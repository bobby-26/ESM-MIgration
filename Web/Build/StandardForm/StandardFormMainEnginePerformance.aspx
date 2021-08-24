<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormMainEnginePerformance.aspx.cs"
    Inherits="StandardFormMainEnginePerformance" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Main Engine Performance</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
        .Sftblclass tr td input[type=text]
        {
            width: 92%;
        }
        .verticaltext
        {
            writing-mode: tb-rl;
            filter: flipv fliph;
            width: 50px;
            height: 70px;
        }
        .verticaltext1
        {
            writing-mode: tb-rl;
            filter: flipv fliph;
            width: 15px;
            height: 40px;
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
            <eluc:Title runat="server" ID="Title1" Text="Main Engine Performance" ShowMenu="false">
            </eluc:Title>
             <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="StandardForm_TabStripCommand" />
            <asp:Button ID="cmdHiddenConfirm" runat="server" Text="cmdHiddenConfirm" OnClick="StandardFormConfirm_TabStripCommand" />
        </div>
        
        <asp:UpdatePanel runat="server" ID="pnlStandardForm">
            <ContentTemplate>
            <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server" visible="false" />
                <table width="95%">
                    <tr>
                        <td align="left">
                            <b><asp:Literal ID="lblCompanyName" runat="server" Text="EXECUTIVE SHIP MANAGEMENT PTE LTD"></asp:Literal></b>
                        </td>
                        <td align="left">
                            <asp:Literal ID="lblFileRef" runat="server" Text="File Ref - 151.2"></asp:Literal>
                        </td>
                        <td align="right">
                            <asp:literal ID="lblEB" runat="server" Text="E8"></asp:literal>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <b><asp:literal ID="lblsingapore" runat="server" Text="SINGAPORE"></asp:literal></b>
                        </td>
                        <td align="left">
                            <asp:literal ID="lbLCE20" runat="server" Text="C/E 20"></asp:literal>
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="right">
                            <asp:Literal ID="lbl303Rev0" runat="server" Text="(3/03 Rev 0)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" align="center">
                            <h3>
                                <asp:literal ID="lblMainEnginePerformance" runat="server" Text="MAIN ENGINE PERFORMANCE"></asp:literal>
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <table width="100%" cellspacing="0px" style="margin-bottom: 0px">
                                <tr>
                                    <td style="border-style: solid none solid solid; border-width: 1px; border-color: #000000">
                                        <asp:Literal ID="lblVessel" runat="server" Text="VESSEL"></asp:Literal>
                                    </td>
                                    <td colspan="2" style="border-style: solid none solid none; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtVesselName" runat="server" CssClass="input" Enabled="false" ></asp:TextBox>
                                    </td>
                                    <td colspan="2" style="border-style: solid none solid solid; border-width: 1px; border-color: #000000">
                                        <asp:literal ID="lblEngineType" runat="server" Text="ENGINE TYPE:"></asp:literal>
                                    </td>
                                    <td style="border-style: solid none solid none; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtEngineType" runat="server" CssClass="input" Enabled="false" ></asp:TextBox>
                                    </td>
                                    <td style="border-style: solid none solid solid; border-width: 1px; border-color: #000000">
                                        <u><asp:Literal ID="lblEngineOutput" runat="server" Text="ENGINE OUTPUT:"></asp:Literal></u>
                                    </td>
                                    <td colspan="2" align="center" style="border: 1px solid #000000;">
                                        <asp:Literal ID="lblCheckedBy" runat="server" Text="Checked by:"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" style="border-style: none none solid solid; border-width: 1px; border-color: #000000">
                                        <asp:Literal ID="lblYardNo" runat="server" Text="YARD NO."></asp:Literal>
                                    </td>
                                    <td rowspan="2" style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtYardNo" runat="server" CssClass="input" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td style="border-style: none none solid solid; border-width: 1px; border-color: #000000">
                                        <asp:Literal ID="lblBuilder" runat="server" Text="BUILDER:"></asp:Literal>
                                    </td>
                                    <td colspan="3" style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtBuilder" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td rowspan="2" style="border-style: none none solid solid; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtBuilder1" runat="server" CssClass="input" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td colspan="2" align="center" style="border-style: none solid solid solid; border-width: 1px;
                                        border-color: #000000">
                                        <asp:Literal ID="lblCE" runat="server" Text="C/E"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-style: none none solid solid; border-width: 1px; border-color: #000000">
                                        <asp:Literal ID="lblBuildYear" runat="server" Text="BUILD YEAR:"></asp:Literal>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtBuildYear" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:Literal ID="lblEngineNR" runat="server" Text="ENGINE Nr:"></asp:Literal>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtEngineNo" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td style="border-style: none none solid solid; border-width: 1px; border-color: #000000">
                                        <asp:literal ID="lblDate" runat="server" Text="DATE"></asp:literal>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000;
                                        border-right-style: solid;">
                                        <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center" style="border-style: none solid solid solid; border-width: 1px;
                                        border-color: #000000">
                                        <asp:Literal ID="lblTurboCharger" runat="server" Text="TURBOCHARGER"></asp:Literal>
                                    </td>
                                    <td style="border-bottom-style: solid; border-left-style: solid; border-width: 1px;
                                        border-color: #000000">
                                        <asp:literal ID="lblSerialNo" runat="server" Text="Serial No.:"></asp:literal>
                                    </td>
                                    <td colspan="6" style="border-style: none solid solid solid; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtSerialNo" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom-style: solid; border-left-style: solid; border-width: 1px;
                                        border-color: #000000">
                                        <asp:Literal ID="lblMake" runat="server" Text="MAKE:"></asp:Literal>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtMake" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td rowspan="4" style="border-style: none solid solid solid; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtMake1" runat="server" CssClass="input" TextMode="MultiLine" Height="76px"></asp:TextBox>
                                    </td>
                                    <td colspan="3" style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:Literal ID="lblCylinderBoreStoke" runat="server" Text="CYLINDER (BORE x STROKE) :"></asp:Literal>
                                    </td>
                                    <td colspan="3" style="border-bottom-style: solid; border-right-style: solid; border-width: 1px;
                                        border-color: #000000">
                                        <asp:TextBox ID="txtCylinder" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom-style: solid; border-left-style: solid; border-width: 1px;
                                        border-color: #000000">
                                        <asp:Literal ID="lblType" runat="server" Text="Type"></asp:Literal>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtType1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:Literal ID="lblGovernor" runat="server" Text="GOVERNOR :"></asp:Literal>
                                    </td>
                                    <td colspan="2" style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtGovernor2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:Literal ID="lbltype2" runat="server" Text="TYPE:"></asp:Literal>
                                    </td>
                                    <td colspan="2" style="border-bottom-style: solid; border-width: 1px; border-color: #000000;
                                        border-right-style: solid;">
                                        <asp:TextBox ID="txtType" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom-style: solid; border-left-style: solid; border-width: 1px;
                                        border-color: #000000">
                                        <asp:Literal ID="lblMaxRPM" runat="server" Text="Max RPM"></asp:Literal>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtmaxRim" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2" style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                       <asp:Literal ID="lblTCSpecification" runat="server" Text="TC SPECIFICATION:"></asp:Literal>
                                    </td>
                                    <td colspan="4" style="border-bottom-style: solid; border-width: 1px; border-color: #000000;
                                        border-right-style: solid;">
                                        <asp:TextBox ID="txtTcSpecification" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom-style: solid; border-left-style: solid; border-width: 1px;
                                        border-color: #000000">
                                        <asp:literal ID="lblMaxTemp" runat="server" Text="Max Temp 0c. 580"></asp:literal>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtMaxTemp" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:literal ID="lblPropellerPitch" runat="server" Text="PROPELLER PITCH:"></asp:literal>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px">
                                        <asp:TextBox ID="txtPropeller" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000;
                                        border-left-style: solid;">
                                       <asp:Literal ID="lblVLDisplacement" runat="server" Text="V/L DISPLACEMENT:"></asp:Literal>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtDisplacement" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000;
                                        border-left-style: solid;">
                                        <asp:literal ID="lblCargo" runat="server" Text="CARGO % "></asp:literal>
                                    </td>
                                    <td style="border-right-style: solid; border-bottom-style: solid; border-width: 1px;
                                        border-color: #000000">
                                        <asp:TextBox ID="txtCargo" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-style: none none solid solid; border-width: 1px; border-color: #000000">
                                        <asp:Literal ID="lblLubeOilSystem" runat="server" Text="LUBE. OIL SYSTEM:"></asp:Literal>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:Literal ID="lblInternalSystem" runat="server" Text="INTERNAL SYSTEM"></asp:Literal>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:CheckBox ID="chInternationalSystem" runat="server" Text="INTERNAL SYSTEM" />
                                    </td>
                                    <td colspan="2" style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:CheckBox ID="chMESystem" runat="server" Text="EXTERNAL FROM M.E. SYSTEM" />
                                    </td>
                                    <td colspan="3" style="border-bottom-style: solid; border-right-style: solid; border-width: 1px;
                                        border-color: #000000">
                                        <asp:CheckBox ID="chGravityTank" runat="server" Text="EXTERNAL FROM GRAVITY TANK" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom-style: solid; border-left-style: solid; border-width: 1px;
                                        border-color: #000000">
                                        <asp:Literal ID="lblFuelOilViscosity" runat="server" Text="FUEL OIL VISCOSITY:"></asp:Literal>
                                    </td>
                                    <td colspan="3" style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtFuelViscosity" runat="server" CssClass="input"></asp:TextBox>
                                        <asp:Literal ID="lblat" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        at &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; 50 &degC"></asp:Literal>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtFuelViscosity1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000;
                                        border-left-style: solid;">
                                        <asp:TextBox ID="txtFuelViscosity2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center" style="border-bottom-style: solid; border-width: 1px; border-color: #000000;
                                        border-left-style: solid;">
                                        <asp:Literal ID="lblBrand" runat="server" Text="BRAND"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center" style="border-bottom-style: solid; border-right-style: solid;
                                        border-width: 1px; border-color: #000000; border-left-style: solid;">
                                       <asp:Literal ID="lblType1" runat="server" Text="TYPE"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom-style: solid; border-left-style: ridge; border-width: 1px;
                                        border-color: #000000">
                                       <asp:Literal ID="lblBunkerStation" runat="server" Text="BUNKER STATION:"></asp:Literal>
                                    </td>
                                    <td colspan="4" style="border-bottom-style: solid; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtBunkerStation" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000;
                                        border-left-style: solid;">
                                        <asp:Literal ID="lblCylinderOil" runat="server" Text="CYLINDER OIL"></asp:Literal>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000;
                                        border-left-style: solid;">
                                        <asp:TextBox ID="txtCyliderOil" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2" style="border-bottom-style: solid; border-right-style: solid; border-width: 1px;
                                        border-color: #000000; border-left-style: solid;">
                                        <asp:TextBox ID="txtCylinderOil1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="border-bottom-style: solid; border-left-style: solid; border-width: 1px;
                                        border-color: #000000">
                                        <asp:Literal ID="lblOilBrandIFO" runat="server" Text="OIL BRAND:IFO"></asp:Literal>
                                    </td>
                                    <td align="center" colspan="3" style="border-bottom-style: solid; border-width: 1px;
                                        border-color: #000000">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;CST &nbsp;&nbsp;&nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:literal ID="lblHeatValueMJKG" runat="server" Text="HEAT VALUE (MJ/kG):"></asp:literal>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000;
                                        border-left-style: none;">
                                        <asp:TextBox ID="txtOilBrand" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td style="border-width: 1px; border-color: #000000; border-left-style: solid; border-bottom-style: solid;">
                                       <asp:Literal ID="lblCIRCOIl" runat="server" Text="CIRC. OIL"></asp:Literal>
                                    </td>
                                    <td style="border-bottom-style: solid; border-width: 1px; border-color: #000000;
                                        border-left-style: solid;">
                                        <asp:TextBox ID="txtCirOil1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2" style="border-bottom-style: solid; border-right-style: solid; border-width: 1px;
                                        border-color: #000000; border-left-style: solid;">
                                        <asp:TextBox ID="txtCirOil2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="border-bottom-style: none; border-left-style: solid; border-width: 1px;
                                        border-color: #000000">
                                        <asp:Literal ID="lblDensity" runat="server" Text="DENSITY @ 150C"></asp:Literal>
                                    </td>
                                    <td style="border-bottom-style: none; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtDensity" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td style="border-bottom-style: none; border-width: 1px; border-color: #000000; border-left-style: solid;">
                                        <asp:Literal ID="lblsulphur" runat="server" Text="SULPHUR: % :"></asp:Literal>
                                    </td>
                                    <td style="border-bottom-style: none; border-width: 1px; border-color: #000000">
                                        <asp:TextBox ID="txtDensity1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td style="border-bottom-style: none; border-width: 1px; border-color: #000000; border-left-style: solid;">
                                       <asp:Literal ID="lblTurboOil" runat="server" Text="TURBO OIL"></asp:Literal>
                                    </td>
                                    <td style="border-bottom-style: none; border-width: 1px; border-color: #000000; border-left-style: solid;">
                                        <asp:TextBox ID="txtDensity2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2" style="border-color: #000000; border-width: 1px; border-bottom-style: none;
                                        border-right-style: solid; border-left-style: solid;">
                                        <asp:TextBox ID="txtDensity3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="9">
                                        <table class="Sftblclass" cellpadding="1px" cellspacing="1px" width="100%">
                                            <tr>
                                                <td align="center">
                                                    <asp:literal ID="lblDatetwo" runat="server" Text="Date"></asp:literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblDraughtforem" runat="server" Text="Draught fore m"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblTotalRunningHrs" runat="server" Text="Total Running Hrs"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblRPM" runat="server" Text="RPM"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:literal ID="lblwindms" runat="server" Text="Wind m/s"></asp:literal>
                                                </td>
                                                <td align="center">
                                                    <asp:literal ID="lblDirect" runat="server" Text="Direct"></asp:literal>
                                                </td>
                                                <td align="center" colspan="2">
                                                    <asp:literal ID="lblPiBar" runat="server" Text="Pi bar"></asp:literal>
                                                </td>
                                                <td align="center" colspan="2">
                                                    <asp:literal ID="lblPmaxBar" runat="server" Text="Pmax bar"></asp:literal>
                                                </td>
                                                <td align="center" colspan="2">
                                                    <asp:literal ID="lblPcompBar" runat="server" Text="Pcomp bar"></asp:literal>
                                                </td>
                                                <td align="center" colspan="2">
                                                    <asp:Literal runat="server" Text="FUEL P/P<br /> INDEX" ID="lblFuel"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <eluc:Date ID="txtDate1" runat="server" CssClass="input" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDraught" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <eluc:Number ID="txtTotalRunningHours1" runat="server" CssClass="input" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPRM" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtWind" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDirect" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblpibar1" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblpibar2" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblPmaxbar1" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                   <asp:Literal ID="lblPmaxbar2" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblPcompBar1" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblPcompbar2" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblFuelppIndex1" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblFuelPPIndex2" runat="server" Text="2"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Literal ID="lblHour" runat="server" Text="Hour"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblDraughtaftm" runat="server" Text="Draught aft m"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblSpeedSettingBar" runat="server" Text="Speed<br />setting bar"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                   <asp:Literal ID="lblKW" runat="server" Text="kW (1st estimation)"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblwaveheightm" runat="server" Text="Wave height m"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtWageHeightM" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtHour1" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtDraaught2" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtSpeed" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtKW" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtWaveHeight" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtSE" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtSE21" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtSE2" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtDate2" runat="server" CssClass="input" ></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDraugh1t" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <eluc:Number ID="txtTotalRunningHours2" runat="server" CssClass="input" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSpeed2" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtKW2" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSEe22" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblPibar3" runat="server" Text="3"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblPibar4" runat="server" Text="4"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblPmaxBar3" runat="server" Text="3"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                   <asp:Literal ID="lblPimaxbar4" runat="server" Text="4"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblPcompbar3" runat="server" Text="3"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblPCompbar4" runat="server" Text="4"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblFuelPPIndex3" runat="server" Text="3"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                   <asp:Literal ID="lblFuelPPIndex44" runat="server" Text="4"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Literal ID="lblLoad" runat="server" Text="Load %"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                   <asp:Literal ID="lblLogKnote" runat="server" Text="Log Knots"></asp:Literal>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:Literal ID="lblGovernorIndex" runat="server" Text="Governor<br />Index"></asp:Literal>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:Literal ID="lblKw1" runat="server" Text="kW (after corr.for<br />LCV/density"></asp:Literal>
                                                </td>
                                                <td colspan="2" rowspan="2">
                                                    <asp:TextBox ID="txtKWLCVDensity" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:TextBox ID="txtLoad" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:TextBox ID="txtKnots" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:TextBox ID="txtGovernor" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:TextBox ID="txtDensityw" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:TextBox ID="txtgKW" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:TextBox ID="txtSeE21" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:TextBox ID="txttSE42" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:TextBox ID="txtSe23" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txteSe31" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtSer32" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" rowspan="2">
                                                   <asp:Literal ID="lblBarom" runat="server" Text="Barom.<br />Millibar"></asp:Literal>
                                                </td>
                                                <td rowspan="2" align="center">
                                                    <asp:Literal ID="lblObsKnots" runat="server" Text="Obs.<br />Knot's"></asp:Literal>
                                                </td>
                                                <td align="center" rowspan="3">
                                                    <asp:TextBox ID="txtObsKnots" runat="server" CssClass="input" TextMode="MultiLine"
                                                        Height="48px"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:TextBox ID="txtObsKnots1" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtObsKnots2" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                   <asp:Literal ID="lblPibar5" runat="server" Text="5"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblPibar6" runat="server" Text="6"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                   <asp:Literal ID="lblPimax5" runat="server" Text="5"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblPimax6" runat="server" Text="6"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblPcompbar5" runat="server" Text="5"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblPcompbar6" runat="server" Text="6"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblFuelPPIndex5" runat="server" Text="5"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblFuelPPIndex6" runat="server" Text="6"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtLoad1" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtKnots1" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:TextBox ID="txtHknots" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:TextBox ID="txtLoasds" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:TextBox ID="txtFFSa" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:TextBox ID="txtAfds" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:TextBox ID="txtSae41" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:TextBox ID="txtse43" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:TextBox ID="txtse42" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:TextBox ID="txtSe41" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtKw31" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtse311" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtse32" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td colspan="2">
                                                    <asp:TextBox ID="txtse33" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="5">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <b><asp:Literal ID="lblAverage" runat="server" Text="Average:"></asp:Literal></b>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAverage1" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAverge2" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAverage3" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAverage4" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="9">
                                        <table class="Sftblclass" cellpadding="1px" cellspacing="1px" width="100%">
                                            <tr>
                                                <td colspan="3" rowspan="3" align="center">
                                                    <asp:Literal ID="lblPmaxadjustmentNoofShims" runat="server" Text="Pmax<br />adjustment<br />no. of shims"></asp:Literal>
                                                </td>
                                                <td colspan="5" align="center">
                                                    <asp:Literal ID="lblexhaustGasTemp" runat="server" Text="Exhaust gas temp. &deg C"></asp:Literal>
                                                </td>
                                                <td colspan="2" align="center">
                                                    <asp:literal ID="lblExhPress" runat="server" Text="Exh. Press."></asp:literal>
                                                </td>
                                                <td align="center" rowspan="3">
                                                   <asp:Literal ID="lblTurboCharger1" runat="server" Text="Turbo-<br />
                                                    Charger<br />
                                                    RPM"></asp:Literal>
                                                </td>
                                                <td align="center" colspan="3">
                                                    <asp:Literal ID="lblScavAirPr" runat="server" Text="Scav. Air Pr."></asp:Literal>
                                                </td>
                                                <td colspan="3" align="center">
                                                   <asp:Literal ID="lblScavAirTempDeg" runat="server" Text="Scav. Air temp. &deg C"></asp:Literal>
                                                </td>
                                                <td align="center" rowspan="3">
                                                    <div class="verticaltext">
                                                        <asp:Literal ID="lblAuxiliaryBlower" runat="server" Text="Auxiliary<br />Blower"></asp:Literal>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3" align="center" rowspan="2">
                                                    <asp:Literal ID="lblexhaustvalue" runat="server" Text="Exhaust valve"></asp:Literal>
                                                </td>
                                                <td colspan="2" align="center">
                                                    <asp:Literal ID="lblturbine" runat="server" Text="Turbine"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:literal ID="lblReceiver" runat="server" Text="Receiver"></asp:literal>
                                                </td>
                                                <td><center>
                                                   <asp:Literal ID="lblTurbineOutlet" runat="server" Text="Turbine outlet"></asp:Literal></center>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    &Delta
                                                    <br />
                                                    <asp:Literal ID="lblPFIlter" runat="server" Text="P filter<br />mmWC"></asp:Literal>
                                                </td>
                                                <td align="center" rowspan="2">
                                                   <<asp:Literal ID="lblrPCooler" runat="server" Text="r<br />P cooler<br />mmWC"></asp:Literal>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:Literal ID="lblReceiver2" runat="server" Text="Receiver"></asp:Literal>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:Literal ID="lblInlet" runat="server" Text="Inlet<br />Blower"></asp:Literal>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:literal ID="lblBefore" runat="server" Text="Before<br />Cooler"></asp:literal>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:Literal ID="lblAfterCooler" runat="server" Text="After<br />Coole<br />r"></asp:Literal>
                                                    
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Literal ID="lblIn" runat="server" Text="In"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblOut" runat="server" Text="Out"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblmmHG" runat="server" Text="mmHg"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:literal ID="lblmmWC" runat="server" Text="mm WC"></asp:literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Literal ID="lbl15" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl25" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl3" runat="server" Text="3"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl16" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl26" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                   <asp:Literal ID="lbl35" runat="server" Text="3"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblin1" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblout1" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td rowspan="4">
                                                    <asp:TextBox ID="txtExhaustvalve1" runat="server" CssClass="input" Height="86px"
                                                        TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblTurbineoutlet1" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblTurboChargerRPM1" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblDelta1" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl1PCooler2" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:TextBox ID="txtExhaustvalve2" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblInletBlower1" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblBeforeCooler1" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblAfterCOoler5" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center" rowspan="3">
                                                    <asp:Literal ID="lblOn" runat="server" Text="On"></asp:Literal>
                                                    <br />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="3">
                                                    <asp:Literal ID="lblAllOriginal" runat="server" Text="(ALL ORIGINAL)"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAllOriginal" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAllOriginal1" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAllOriginal2" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAllOriginal3" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAllOriginal4" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAllOriginal5" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExhaustvalve11" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExhaustvalve21" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExhaustvalve31" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExhaustvalve41" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExhaustvalve51" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExhaustvalve61" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Literal ID="lbl4" runat="server" Text="4"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl54" runat="server" Text="5"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl65" runat="server" Text="6"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl45" runat="server" Text="4"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl55" runat="server" Text="5"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl64" runat="server" Text="6"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblin2" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblout2" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblTurbineoutlet2" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblTurboCharge2" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblDelta2" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblPCooler2" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExhaustvalve111" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblInletBlower2" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbBeforeCoolerl2" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblAfterCooler2" runat="server" Text="2"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="3">
                                                    <asp:Literal ID="lblAlloriginal1" runat="server" Text="(ALL ORIGINAL)"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAllOriginals21" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAllOriginals22" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAllOriginals23" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAllOriginals24" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAllOriginals25" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAllOriginals26" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAllOriginals27" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAllOriginals28" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAllOriginals29" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblmmHg1" runat="server" Text="mm Hg"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtmmHg1" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtmmHg2" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtmmHg3" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td rowspan="3" align="center">
                                                    <asp:Literal ID="lblOff" runat="server" Text="Off"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:Literal ID="lblAverage1" runat="server" Text="Average"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAverage" CssClass="input" runat="server"></asp:TextBox>
                                                </td>
                                                <td colspan="2" align="center">
                                                    <asp:Literal ID="lblAverage2" runat="server" Text="Average"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtmmHg1Average" CssClass="input" runat="server"></asp:TextBox>
                                                </td>
                                                <td colspan="7" rowspan="2">
                                                    <asp:Literal ID="lblRemarks" runat="server" Text="Remark's"></asp:Literal><br />
                                                    <asp:TextBox ID="txtRemark" CssClass="input" runat="server"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblc" runat="server" Text="&deg C"></asp:Literal>
                                                </td>
                                                <td colspan="3" rowspan="2">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="6">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRemark1" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="9">
                                        <table class="Sftblclass" cellpadding="1px" cellspacing="1px" width="100%">
                                            <tr>
                                                <td colspan="2" align="center">
                                                    <asp:literal ID="lblSpecialPoints" Text="SPECIAL POINTS" runat="server"></asp:literal>
                                                </td>
                                                <td colspan="7" align="center">
                                                    <asp:Literal ID="lblCoolingWaterTemp" runat="server" Text="COOLING WATER TEMP. &deg C"></asp:Literal>
                                                </td>
                                                <td align="center" colspan="7">
                                                   <asp:Literal ID="lblLubricatingOil" runat="server" Text="LUBRICATING OIL"></asp:Literal>
                                                </td>
                                                <td align="center" rowspan="2">
                                                   <asp:Literal ID="lblFuelOilPrBar" runat="server" Text="FUEL OIL<br />PR. BAR"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center" rowspan="2">
                                                    <asp:Literal ID="lblManTCPatSpiral" runat="server" Text="MAN TC p at spiral<br />
                                                    housing outer dia. Mm<br />
                                                    Hg"></asp:Literal>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:Literal ID="lblManTCbbc" runat="server" Text="MAN TC BBC<br />
                                                    TC r p<br />
                                                    inner & outer<br />
                                                    dia. Mm WC"></asp:Literal>
                                                </td>
                                                <td align="center" colspan="2">
                                                    <asp:Literal ID="lblAIR" runat="server" Text="AIR<br />
                                                    COOLER"></asp:Literal>
                                                </td>
                                                <td align="center" colspan="4">
                                                    <asp:Literal ID="lblMainEngine" runat="server" Text="MAIN ENGINE"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblTurbine1" runat="server" Text="TURBINE"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:literal ID="lblPressBar" runat="server" Text="Press.<br />BAR"></asp:literal>
                                                </td>
                                                <td align="center" colspan="6">
                                                    <asp:Literal ID="lblTemperature" runat="server" Text="TEMPERATURE &deg C"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <div class="verticaltext1">
                                                        <asp:Literal ID="lblINLET1" runat="server" Text="INLET"></asp:Literal>
                                                    </div>
                                                </td>
                                                <td align="center">
                                                    <div class="verticaltext1">
                                                        <asp:Literal ID="lblOUTLET" runat="server" Text="OUTLET"></asp:Literal>
                                                    </div>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblINLET2" runat="server" Text="INLET"></asp:Literal>
                                                </td>
                                                <td align="center" colspan="3">
                                                    <asp:Literal ID="lblOutletCylinder" runat="server" Text="OUTLET (CYLINDER'S)"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblOutlet1" runat="server" Text="OUTLET"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                   <asp:Literal ID="lblsysOil" runat="server" Text="SYS.<br />OIL"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblENginEInlet" runat="server" Text="ENGIN<br />E<br />INLET"></asp:Literal>
                                                </td>
                                                <td align="center" colspan="3">
                                                    <asp:Literal ID="lblOutletPistons" runat="server" Text="OUTLET PISTON'S"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:literal ID="lblManTC" runat="server" Text="MAN<br />
                                                    TC<br />
                                                    INLET"></asp:literal>
                                                </td>
                                                <td align="center">
                                                    <asp:literal ID="lblMan" runat="server" Text="MAN<br />
                                                    TC<br />
                                                    OUTLET"></asp:literal>
                                                </td>
                                                <td align="center">
                                                    <asp:literal ID="lblBefore1" runat="server" Text="BEFORE<br />
                                                    FILTER"></asp:literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Literal ID="lbl127" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl128" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl129" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl130" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:TextBox ID="txtc" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl1Outlet1" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:literal ID="lbOutlet2" runat="server" Text="2"></asp:literal>
                                                </td>
                                                <td align="center">
                                                   <asp:Literal ID="lblOutlet3" runat="server" Text="3"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl1Turbine1" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtManTC" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td rowspan="4">
                                                    <asp:TextBox ID="txtManBBC" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lboutletprisonl" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:literal ID="lblOutletPrison2" runat="server" Text="2"></asp:literal>
                                                </td>
                                                <td align="center">
                                                   <asp:Literal ID="lblOutletPrison3" runat="server" Text="3"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl134" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl135" runat="server" Text="1"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtManTC1" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtManTC21" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtManBBC21" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td rowspan="2" align="center">
                                                    <asp:TextBox ID="txtAirCoolerInlet" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td rowspan="2" align="center">
                                                    <asp:TextBox ID="txtAirCoolerOutlet" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTCInlet" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTCOutlet" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTCfitter1" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtTCfitter2" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:Literal ID="lblEXHVV" runat="server" Text="EXH<br />
                                                    V/V"></asp:Literal>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExeV1" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExeV2" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExeV3" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExeV4" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtExeV5" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:Literal ID="lblAfterFilter" runat="server" Text="AFTER<br />
                                                    FILTER"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                   <asp:Literal ID="lbl217" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl218" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td align="center" rowspan="3">
                                                   <asp:Literal ID="lblsw" runat="server" Text="S.W.<br />
                                                    &deg C"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblOutlet4" runat="server" Text="4"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblOutlet5" runat="server" Text="5"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblOUtlet6" runat="server" Text="6"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblTurbine2" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblOutletPrision4" runat="server" Text="4"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblOutletPrision5" runat="server" Text="5"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblOutletPrision6" runat="server" Text="6"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblManTcin2" runat="server" Text="2"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lblMantcout2" runat="server" Text="2"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtAFilter1" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAFilter2" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td rowspan="5" colspan="2">
                                                    <asp:TextBox ID="txtAFilter3" runat="server" CssClass="input" Height="128px" TextMode="MultiLine"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAFilter4" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAFilter5" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAFilter6" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAFilter7" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAFilter8" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAFilter9" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAFilter10" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAFilter11" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAFilter12" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAFilter14" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAFilter13" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Literal ID="lbl39" runat="server" Text="3"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl38" runat="server" Text="3"></asp:Literal>
                                                </td>
                                                <td align="center" rowspan="4" colspan="2">
                                                    <asp:Literal ID="lblAverage3" runat="server" Text="Average"></asp:Literal>
                                                </td>
                                                <td rowspan="4">
                                                    <asp:TextBox ID="txtss" runat="server" CssClass="input" TextMode="MultiLine" Height="113px"></asp:TextBox>
                                                </td>
                                                <td rowspan="4">
                                                    <asp:TextBox ID="txtFilteAverage" runat="server" CssClass="input" TextMode="MultiLine"
                                                        Height="113px"></asp:TextBox>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:Literal ID="lblTCH" runat="server" Text="T/Ch"></asp:Literal>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:Literal ID="lblThrustBrg" runat="server" Text="Thrust<br />
                                                    Brg."></asp:Literal>
                                                </td>
                                                <td align="center" rowspan="4" colspan="2">
                                                    <asp:Literal ID="lblAverage4" runat="server" Text="Average"></asp:Literal>
                                                </td>
                                                <td rowspan="4">
                                                    <asp:TextBox ID="txtThrust" runat="server" CssClass="input" TextMode="MultiLine"
                                                        Height="113px"></asp:TextBox>
                                                </td>
                                                <td rowspan="4" colspan="2">
                                                    <asp:TextBox ID="txtThrustAverage" runat="server" CssClass="input" TextMode="MultiLine"
                                                        Height="113px"></asp:TextBox>
                                                </td>
                                                <td align="center" rowspan="2">
                                                    <asp:Literal ID="lblTempDEGEngineInlet" runat="server" Text="TEMP. &deg C<br />
                                                    ENGINE<br />
                                                    INLET"></asp:Literal>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtTemp" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtEngine" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td rowspan="3">
                                                    <asp:TextBox ID="txtEngineInlet" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Literal ID="lbl49" runat="server" Text="4"></asp:Literal>
                                                </td>
                                                <td align="center">
                                                    <asp:Literal ID="lbl48" runat="server" Text="4"></asp:Literal>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:TextBox ID="txtTemp1" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:TextBox ID="txtEngine1" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td rowspan="2">
                                                    <asp:TextBox ID="txtEngineInlet1" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtInlet11" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtOutlet11" runat="server" CssClass="input"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Literal ID="lblCylinderLubricatorLevel" runat="server" Text="CYLINDER LUBRICATOR LEVER SETTING IS AT , L =&nbsp;&nbsp;"></asp:Literal><asp:TextBox ID="txtleverstting"
                                runat="server" CssClass="input"></asp:TextBox>
                            <asp:Literal ID="lblCylinder" runat="server" Text=", CYLINDER LUBE. OIL CONSUMPTION ABT. L/D @ RPM =&nbsp;&nbsp;"></asp:Literal>
                            <asp:TextBox ID="txtOilConsumptiomn" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Literal ID="lblFOCONS" runat="server" Text="FO CONS/DAY ="></asp:Literal>
                            <asp:TextBox ID="txtFoDays" runat="server" CssClass="input"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                           <asp:Literal ID="lblSPCYLOILCONS" runat="server" Text="SP. CYL OIL CONS ="></asp:Literal>
                            <asp:TextBox ID="txtOilCons" runat="server" CssClass="input"></asp:TextBox>
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
