<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormAuxiliaryEngineDecarbonization.aspx.cs"
    Inherits="StandardFormAuxiliaryEngineDecarbonization" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Decarbonization of Aux Engine</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>

    <script language="javascript" type="text/javascript">
        function cmdPrint_Click() {
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
            width: 94%;
        }
    </style>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRecordofNox" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 1650px">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Decarbonization of Aux Engine" ShowMenu="false">
            </eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="StandardForm_TabStripCommand" />
            <asp:Button ID="cmdHiddenConfirm" runat="server" Text="cmdHiddenConfirm" OnClick="StandardFormConfirm_TabStripCommand" />
        </div>
        <asp:UpdatePanel runat="server" ID="pnlStandardForm">
            <ContentTemplate>
                <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server"
                    visible="false" />
                <table style="width: 1650px" cellspacing="0px">
                    <tr>
                        <td colspan="12" align="left">
                            <b>
                                <asp:Literal ID="lblCompanyName" runat="server" Text="EXECUTIVE SHIP MANAGEMENT PTE LTD"></asp:Literal></b>
                        </td>
                        <td align="left">
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblE7" runat="server" Text="E7"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="12" align="left">
                            <b>
                                <asp:Literal ID="lblSingapore" runat="server" Text="SINGAPORE"></asp:Literal></b>
                        </td>
                        <td align="left">
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPage1of1" runat="server" Text="Page 1 of 1"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="14" align="right">
                            <asp:Literal ID="lbl0714Rev1" runat="server" Text="(07/14 Rev 1)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="14" align="center">
                            <h1>
                                <asp:Literal ID="lblReportOnDecarbonisationOfAuxiliaryEngines" runat="server" Text="REPORT ON DECARBONISATION OF AUXILIARY ENGINES"></asp:Literal>
                            </h1>
                        </td>
                    </tr>
                    <tr>
                        <td style="border-style: solid none solid solid; border-width: 1px; border-color: #000000;">
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td style="border-top-style: solid; border-bottom-style: solid; border-width: 1px;
                            border-color: #000000">
                            <asp:TextBox ID="txtVesselName" runat="server" CssClass="input" Enabled="false"></asp:TextBox>
                        </td>
                        <td style="border-style: solid none solid solid; border-width: 1px; border-color: #000000">
                            <asp:Literal ID="lblAuxEngineNo" runat="server" Text="Aux. Engine No."></asp:Literal>
                        </td>
                        <td style="border-top-style: solid; border-bottom-style: solid; border-width: 1px;
                            border-color: #000000">
                            <asp:TextBox ID="txtAuxEngine" runat="server" CssClass="input" Enabled="false"></asp:TextBox>
                        </td>
                        <td style="border-style: solid none solid solid; border-width: 1px; border-color: #000000">
                            <asp:Literal ID="lblLocation" runat="server" Text="Location :"></asp:Literal>
                        </td>
                        <td style="border-style: solid none solid none; border-width: 1px; border-color: #000000">
                            <asp:TextBox ID="txtLocation" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td style="border-style: solid none solid solid; border-width: 1px; border-color: #000000">
                            <asp:Literal ID="lblDateOHaulCommenced" runat="server" Text="Date O’haul<br />Commenced"></asp:Literal>
                        </td>
                        <td style="border-top-style: solid; border-bottom-style: solid; border-width: 1px;
                            border-color: #000000">
                            <eluc:Date ID="txtDate1" runat="server" CssClass="input" DatePicker="true" />
                        </td>
                        <td style="border-top-style: solid; border-bottom-style: solid; border-left-style: solid;
                            border-width: 1px; border-color: #000000">
                            <asp:Literal ID="lblDateOHaulCompleted" runat="server" Text="Date O’haul<br />Completed"></asp:Literal>
                        </td>
                        <td style="border-top-style: solid; border-bottom-style: solid; border-width: 1px;
                            border-color: #000000">
                            <eluc:Date ID="txtDate2" runat="server" CssClass="input" DatePicker="true" />
                        </td>
                        <td style="border-style: solid none solid solid; border-width: 1px; border-color: #000000">
                            <asp:Literal ID="lblDateLastOverHaul" runat="server" Text="Date Last<br />Overhaul"></asp:Literal>
                        </td>
                        <td style="border-top-style: solid; border-bottom-style: solid; border-width: 1px;
                            border-color: #000000">
                            <eluc:Date ID="txtDate3" runat="server" CssClass="input" DatePicker="true" />
                        </td>
                        <td style="border-style: solid; border-width: 1px; border-color: #000000">
                            <asp:Literal ID="lblTotalOprHrsSinceInstallation" runat="server" Text="Total Opr. Hrs.<br />Since Installation"></asp:Literal>
                        </td>
                        <td style="border-style: solid solid solid none; border-width: 1px; border-color: #000000;">
                            <asp:TextBox ID="txtTotOpr" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="border-left-style: solid; border-width: 1px; border-color: #000000">
                            <asp:CheckBox ID="chChromePlated" runat="server" Text="Chrome Plated" /><br />
                            <asp:CheckBox ID="chOrdinary" runat="server" Text="Ordinary" />
                        </td>
                        <td colspan="7" align="center">
                            <h3>
                                <asp:Literal ID="lbLCalibrationofCylinderLiners" runat="server" Text="CALIBRATION OF CYLINDER LINERS"></asp:Literal></h3>
                        </td>
                        <td colspan="2" style="border-left-style: solid; border-width: 1px; border-color: #000000">
                            <asp:Literal ID="lblUnitsNumberedFrom" runat="server" Text="Units Numbered from"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBox ID="chFord" runat="server" Text="Ford" /><br />
                            <asp:CheckBox ID="chAft" runat="server" Text="Aft" />
                        </td>
                        <td style="border-left-style: solid; border-width: 1px; border-color: #000000">
                            <asp:Literal ID="lblOprHrsSinceLastOhaul" runat="server" Text="Opr. Hrs. Since<br />Last O’haul"></asp:Literal>
                        </td>
                        <td style="border-right-style: solid; border-left-style: solid; border-width: 1px;
                            border-color: #000000">
                            <asp:TextBox ID="txtOprHaul" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="14">
                            <table class="Sftblclass" cellpadding="1px" cellspacing="1px" width="100%">
                                <tr>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblunitNo" runat="server" Text="Unit<br /> No."></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblDateLinerInstalled" runat="server" Text="Date Liner<br />Installed"></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblPostNo" runat="server" Text="Post<br />No."></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblNewliner" runat="server" Text="New Liner"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblLastOhaul" runat="server" Text="Last O’haul"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblPresent" runat="server" Text="Present"></asp:Literal>
                                    </td>
                                    <td colspan="3" align="center">
                                        <asp:Literal ID="lblWearSinceNew" runat="server" Text="Wear Since New"></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblUnitNumber" runat="server" Text="Unit<br />No."></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblDateLiner" runat="server" Text="Date Liner<br />Installed"></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblPostNo1" runat="server" Text="Post<br />No."></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblNewLiner11" runat="server" Text="New Liner"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblLastOhaul1" runat="server" Text="Last O’haul"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblPresent1" runat="server" Text="Present"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblWearSinceNew2" runat="server" Text="Wear Since New"></asp:Literal>
                                    </td>
                                    <td rowspan="2" align="center">
                                        <asp:Literal ID="lblLoCoolerTubeNestCleaned" runat="server" Text="L. O. Cooler Tube<br />Nest Cleaned"></asp:Literal>
                                    </td>
                                    <td rowspan="2">
                                        <asp:RadioButtonList runat="server" ID="rbtnCoolerTube" RepeatDirection="Horizontal"
                                            Width="100%">
                                            <asp:ListItem Value="0" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="No"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblPS" runat="server" Text="P/S"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbLFA" runat="server" Text="F/A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblPS4" runat="server" Text="P/S"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblFA1" runat="server" Text="F/A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblPS5" runat="server" Text="P/S"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblFA2" runat="server" Text="F/A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblMax" runat="server" Text="Max"></asp:Literal>
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:Literal ID="lblRate1000Hrs" runat="server" Text="Rate / 1000 Hrs"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblPs1" runat="server" Text="P/S"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblFA3" runat="server" Text="F/A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblPs2" runat="server" Text="P/S"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblFA4" runat="Server" Text="F/A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblPS3" runat="server" Text="P/S"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblFA5" runat="server" Text="F/A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblMax1" runat="server" Text="Max"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblRate1000Hrs1" runat="server" Text="Rate / 1000 Hrs"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" rowspan="4">
                                        <asp:Literal ID="lbl1" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td rowspan="4">
                                        <asp:TextBox ID="txtLinearps1" runat="server" CssClass="input" Height="82px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl11" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearps2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLine1arFA2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtLinearFA7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center" rowspan="4">
                                        <asp:Literal ID="lb4" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td rowspan="4">
                                        <asp:TextBox ID="txtLinearFA448" runat="server" CssClass="input" Height="82px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblone" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA12" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA13" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA14" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA15" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA16" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA17" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA18" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblLOCoolerDate" runat="server" Text="L. O. Cooler Date<br />Tube Nest Last Cleaned"></asp:Literal>
                                    </td>
                                    <td rowspan="2">
                                        <eluc:Date ID="txtLastCleanedDate" CssClass="input" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl2" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA21" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA22" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA23" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA24" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA25" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA26" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA27" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtLinearFA28" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl22" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA31" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA32" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA33" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA34" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA35" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA36" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA37" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA38" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl3" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA41" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA42" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA43" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA44" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA45" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA46" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA47" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtLinearFA248" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl33" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinhearFA51" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLineharFA52" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLineahrFA53" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLineaurFA54" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLineaurFA55" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLineharFA56" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLineharFA57" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinhearFA58" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblConditionOfLOCoolerEndCovers" runat="server" Text="Condition of<br />L.O.Cooler End Covers"></asp:Literal>
                                    </td>
                                    <td rowspan="2">
                                        <asp:TextBox ID="txtConditionCoolerCovers" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl4" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearF3A61" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA62" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA63" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA64" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA65" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA66" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA67" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtLinearFA68" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl41" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA69" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA610" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA611" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA612" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA613" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA614" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA615" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA616" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" rowspan="4">
                                        <asp:Literal ID="lbltwo" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td rowspan="4">
                                        <asp:TextBox ID="txtLinearFA61" runat="server" CssClass="input" Height="82px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl111" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA71" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA72" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA73" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA74" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA75" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA76" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA77" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtLinearFA478" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center" rowspan="4">
                                        <asp:Literal ID="lbl5" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td rowspan="4">
                                        <asp:TextBox ID="txtLinearFA578" runat="server" CssClass="input" Height="82px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl12" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA81" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA82" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA83" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA84" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA85" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA86" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA87" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLinearFA88" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblLOCoolerZineAnodesReplaced" runat="server" Text="L. O. Cooler Zine<br />Anodes Replaced"></asp:Literal>
                                    </td>
                                    <td rowspan="2">
                                        <asp:RadioButtonList runat="server" ID="rbtnCoolerZine" RepeatDirection="Horizontal"
                                            Width="100%">
                                            <asp:ListItem Value="0" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="No"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblTwo2" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtZine8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblTwo21" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine12" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine13" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine14" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine15" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine16" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine17" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine18" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl331" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine21" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine22" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine23" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine25" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine24" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine26" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine27" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtZine28" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblThree" runat="Server" Text="3"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine31" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine32" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine33" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine34" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine35" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine36" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine37" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtZine38" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblTurboChargerOverhauled" runat="server" Text="Turbo-Charger<br />Overhauled"></asp:Literal>
                                    </td>
                                    <td rowspan="2">
                                        <asp:RadioButtonList runat="server" ID="rbtnTurboCharger" RepeatDirection="Horizontal"
                                            Width="100%">
                                            <asp:ListItem Value="0" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="No"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblFour" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtTurboOverHaul8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblfour4" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul12" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul13" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul14" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul15" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul16" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul17" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul18" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" rowspan="4">
                                        <asp:Literal ID="lblthree3" runat="server" Text="3&nbsp;"></asp:Literal>
                                    </td>
                                    <td rowspan="4">
                                        <asp:TextBox ID="txtTurboOverHaul20" runat="server" CssClass="input" Height="82px"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblone1" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul21" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul22" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul23" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul24" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul25" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul26" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul27" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtTurboOverHaul28" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center" rowspan="4">
                                        <asp:Literal ID="lblSix" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td rowspan="4">
                                        <asp:TextBox ID="txtTurboOverHaul29" runat="server" CssClass="input" Height="82px"
                                            TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblOne11" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul31" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul32" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul33" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul34" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul35" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul36" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul37" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul38" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblTurboChargerDateLastOhauld" runat="server" Text="Turbo-Charger Date<br />Last O’hauled"></asp:Literal>
                                    </td>
                                    <td rowspan="2">
                                        <eluc:Date ID="Date2" CssClass="input" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl2two" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul41" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul42" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul43" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul44" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul45" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul46" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul47" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtTurboOverHaul48" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lbl221" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul51" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul52" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul53" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul54" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul55" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul56" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul57" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul58" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblThree31" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul61" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul62" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul63" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul64" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul65" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul66" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul67" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtTurboOverHaul68" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblThree33" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul71" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul72" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul73" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul74" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul75" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul76" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul77" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTurboOverHaul78" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblTurboChargerDateBearingsRenewed" runat="server" Text="Turbo-Charger Date<br />Bearings Renewed"></asp:Literal>
                                    </td>
                                    <td rowspan="2">
                                        <eluc:Date ID="txtTurboChargerRDate" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblFour41" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingsRenewed1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingsRenewed2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingsRenewed3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingsRenewed4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingsRenewed5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingsRenewed6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingsRenewed7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtBearingsRenewed8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblfour44" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingsRenewed41" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingsRenewed42" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingsRenewed43" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingsRenewed44" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingsRenewed45" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingsRenewed46" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingsRenewed47" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingsRenewed48" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="23" align="center">
                                        <h3>
                                            <asp:Literal ID="lblPistonsCylinderHeadsFuelPumps" runat="server" Text="PISTONS, CYLINDER HEADS & FUEL PUMPS">.</asp:Literal>
                                        </h3>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblTurboCharger" runat="server" Text="Turbo-Charger<br />Date Rotor Renewed"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtTurboRotorRenewe" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" rowspan="3">
                                        <asp:Literal ID="lblUnitNo2" runat="server" Text="Unit<br />No."></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="3">
                                        <asp:Literal ID="lblPistonDateInstalled" runat="server" Text="Piston<br />Date<br />Installed"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center" rowspan="3">
                                        <asp:Literal ID="lblPresentConditionOfPistonGroovesETC" runat="server" Text="Present Condition<br />of Piston Grooves Etc."></asp:Literal>
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:Literal ID="lblGudegonPin" runat="server" Text="Gudgeon-Pin<br />Clearance"></asp:Literal>
                                    </td>
                                    <td align="center" colspan="4">
                                        <asp:Literal ID="lblCrankPinBrg" runat="server" Text="Crank-Pin Brg.<br />Clearance"></asp:Literal>
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:Literal ID="lblComprPrKgsCm2" runat="server" Text="Compr. Pr.<br />Kgs/cm2"></asp:Literal>
                                    </td>
                                    <td align="center" colspan="2">
                                        <asp:Literal ID="lblCylHeadVvsDateRenewed" runat="server" Text="Cyl. Head V/Vs.<br />Date Renewed"></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="3" colspan="2">
                                        <asp:Literal ID="lblCylHeadDate" runat="server" Text="Cyl. Head Date<br />Re-Cond.nd"></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="3" colspan="2">
                                        <asp:Literal ID="lblFuelPumpOhauled" runat="server" Text="Fuel Pump O'hauled<br />Yes/No"></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="3" colspan="2">
                                        <asp:Literal ID="lblFuelPumpDateLastOhauled" runat="server" Text="Fuel Pump Date<br />Last O’hauled"></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="3" colspan="3">
                                        <asp:Literal ID="lblPartsRenewedOnCylHeads" runat="server" Text="Parts Renewed on Cyl. Heads<br />Fuel Pumps & Valve Gear Etc."></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="3">
                                        <asp:Literal ID="lblTurboChargerDate" runat="server" Text="Turbo-Charger Date<br />Nozzle Ring Renewed"></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="3">
                                        <eluc:Date ID="txtTurboRingRenewe" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblNew" runat="server" Text="New"></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblNow" runat="server" Text="Now"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNew1" runat="server" Text="New"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNew2" runat="server" Text="New"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNow1" runat="server" Text="Now"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNow2" runat="server" Text="Now"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblBefore" runat="server" Text="Before"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblAfter" runat="server" Text="After"></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblInlet" runat="server" Text="Inlet"></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblExht" runat="server" Text="Exht."></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblF" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblA" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblF1" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblA1" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblOhaul1" runat="server" Text="O’haul"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblOhaul2" runat="server" Text="O’haul"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl113" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNew1" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtNOw1" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNewA1" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtNowA1" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOhsaul" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOHaul1" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOHaul2" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOHaul3" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOHaul4" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOHaul5" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOHaul6" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOHaul7" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtOHaul8" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtOHaul9" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtOHaul10" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtOHaul11" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblAirCoolerAir" runat="server" Text="Air Cooler (Air<br />& S. W. Sides) Cleaned"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList runat="server" ID="RadioAirCooler" RepeatDirection="Horizontal"
                                            Width="100%">
                                            <asp:ListItem Value="0" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="No"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl222" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned1" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned2" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned3" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned4" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned5" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned6" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned7" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned8" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned9" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned10" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned11" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned12" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned13" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned14" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned15" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtAirCoolerCleaned16" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblAirCoolerDateLastClearned" runat="server" Text="Air Cooler Date<br />Last Clearned"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtAirCoolerLastCleandate" CssClass="input" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lb34" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned31" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned32" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned33" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned34" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned35" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned36" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned37" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned38" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned39" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned310" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned311" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned312" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned313" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned314" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned315" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtAirCoolerCleaned316" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblAirCoolerDateReplaced" runat="server" Text="Air Cooler Date<br />Replaced"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtAirCoolerReplaced" CssClass="input" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl42" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned41" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned42" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned43" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned44" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned45" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned46" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned47" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned48" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned49" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned410" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned411" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned412" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned413" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned414" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned415" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtAirCoolerCleaned416" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblConditionOfAirCoolerEndCovers" runat="server" Text="Condition of Air Cooler<br />End Covers"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtAirCoolerEndCover" CssClass="input" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl52" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned51" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned52" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned53" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned54" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned55" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned56" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned57" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned58" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned59" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned510" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned511" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned512" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned513" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned514" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned515" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtAirCoolerCleaned516" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblAirCoolerZineAnodesRenewed" runat="server" Text="Air Cooler Zine<br />Anodes Renewed"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList runat="server" ID="rbtnAirCoolerZine" RepeatDirection="Horizontal"
                                            Width="100%">
                                            <asp:ListItem Value="0" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="No"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl6" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned61" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned62" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned63" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned64" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned65" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned66" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned67" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned68" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned69" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned610" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned611" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAirCoolerCleaned612" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned613" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned614" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtAirCoolerCleaned615" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txtAirCoolerCleaned616" CssClass="input" runat="server"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblGovernorOverhauled" runat="server" Text="Governor<br />Overhauled"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:RadioButtonList runat="server" ID="rbtnGovernorOverhauled" RepeatDirection="Horizontal"
                                            Width="100%">
                                            <asp:ListItem Value="0" Text="Yes"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="No"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chChromePlated1" runat="server" Text="Chrome Plated" /><br />
                                        <asp:CheckBox ID="chOrdinary1" runat="server" Text="Ordinary" />
                                    </td>
                                    <td colspan="21" align="center">
                                        <h3>
                                            <asp:Literal ID="lblPistonsRings" runat="server" Text="PISTONS RINGS"></asp:Literal></h3>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblGovernorDate" runat="server" Text="Governor Date<br />Last Overhauled"></asp:Literal>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtLastOverhaulDateGovernor" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="14">
                            <table width="100%" cellpadding="1px" cellspacing="1px" class="Sftblclass">
                                <tr>
                                    <td align="center" rowspan="2">
                                        <asp:Literal ID="lblRingNo" runat="server" Text="Ring<br />No."></asp:Literal>
                                    </td>
                                    <td align="center" colspan="4">
                                        <asp:Literal ID="lblUnitNo1" runat="server" Text="Unit No. 1"></asp:Literal>
                                    </td>
                                    <td align="center" colspan="4">
                                        <asp:Literal ID="lblUnitNo22" runat="server" Text="Unit No. 2"></asp:Literal>
                                    </td>
                                    <td align="center" colspan="4">
                                        <asp:Literal ID="lblUnitNo3" runat="server" Text="Unit No. 3"></asp:Literal>
                                    </td>
                                    <td align="center" colspan="4">
                                        <asp:Literal ID="lblUnitNo4" runat="server" Text="Unit No. 4"></asp:Literal>
                                    </td>
                                    <td align="center" colspan="4">
                                        <asp:Literal ID="lblUnitNo5" runat="server" Text="Unit No. 5"></asp:Literal>
                                    </td>
                                    <td align="center" colspan="4">
                                        <asp:Literal ID="lblUnitNo6" runat="server" Text="Unit No. 6"></asp:Literal>
                                    </td>
                                    <td align="center" rowspan="2" width="15%">
                                        <asp:Literal ID="lblRemarksOnConditionOfCrankShaftCamShaft" runat="server" Text="Remarks on Condition of Crank-Shaft, Cam-Shaft,<br />Driving Gears & Driven Pump Etc."></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblButtCir" runat="server" Text="Butt<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNewCir" runat="server" Text="New<br />Cir."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblButtCir1" runat="server" Text="Butt<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblAxialCir" runat="server" Text="Axial<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblButtCir11" runat="server" Text="Butt<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNewCir1" runat="server" Text="New<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblButtCir2" runat="server" Text="Butt<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblAxialCir1" runat="server" Text="Axial<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblButtCir3" runat="server" Text="Butt<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNewCir14" runat="server" Text="New<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblButtCir4" runat="server" Text="Butt<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblAxialCir2" runat="server" Text="Axial<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblButtCir5" runat="server" Text="Butt<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNewCir11" runat="server" Text="New<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblButtCir6" runat="server" Text="Butt<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblAxialCir3" runat="server" Text="Axial<br />Clr."></asp:Literal>>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblButtCir7" runat="server" Text="Butt<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNewCir12" runat="server" Text="New<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblButtCir8" runat="server" Text="Butt<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblAxialCir5" runat="server" Text="Axial<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblButtCir9" runat="server" Text="Butt<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNewCir13" runat="server" Text="New<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblButtCir10" runat="server" Text="Butt<br />Clr."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblAxialCir4" runat="server" Text="Axial<br />Clr."></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblone12" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft12" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft13" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft14" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft15" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft16" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft17" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft18" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft19" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft120" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft121" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft122" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft123" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft124" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft125" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbltwo22" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft21" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft22" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft23" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft24" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft25" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft26" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft27" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft28" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft29" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft210" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft211" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft212" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft213" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft214" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft215" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft216" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft217" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft218" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft219" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft220" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft221" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft222" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft223" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft224" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft225" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lbl333" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft31" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft32" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft34" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft33" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft35" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft36" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft37" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft38" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft39" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft310" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft311" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft312" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft313" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft314" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft315" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft316" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft317" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft318" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft319" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft320" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft321" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft322" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft323" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft324" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft325" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblfour42" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft41" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft42" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft43" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft44" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft45" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft46" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft47" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft48" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft49" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft410" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft411" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft412" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft413" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft414" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft415" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft416" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft417" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft418" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft419" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft420" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft421" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft422" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft423" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft424" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft425" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblfive55" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft51" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft52" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft53" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft54" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft55" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft56" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft57" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft58" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft59" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft510" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft511" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft512" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft513" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft514" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft515" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft516" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft517" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft518" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft519" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft520" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft521" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft522" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft523" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft524" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft525" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <asp:Literal ID="lblsix6" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft61" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft62" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft63" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft64" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft65" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft66" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft68" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft67" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft69" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft610" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft611" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft612" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft613" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft614" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft615" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft616" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft617" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft618" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft619" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft620" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft621" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft622" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft623" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft624" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtConditionCrankShaft625" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="9">
                                        <asp:Literal ID="lblBearingNumberedFrom" runat="server" Text="&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        Bearings Numbered From&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
                                        <asp:CheckBox ID="chFordBearing" runat="server" Text="Ford" /><br />
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                        <asp:CheckBox ID="chAftBearing" runat="server" Text="Aft" />
                                    </td>
                                    <td colspan="16" align="center">
                                        <h3>
                                            <asp:Literal ID="lblMainBearings" runat="server" Text="MAIN BEARINGS"></asp:Literal></h3>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="5">
                                        <asp:Literal ID="lblBearingNo" runat="server" Text="Bearing No."></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblno1" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblNo2" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblNo3" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblNo4" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblNo5" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblNo6" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblNo7" runat="server" Text="7"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblNo8" runat="server" Text="8"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="5">
                                        <asp:Literal ID="lblDateInstalled" runat="server" Text="Date Installed"></asp:Literal>
                                    </td>
                                    <td colspan="2" align="center">
                                        <eluc:Date ID="txtInstalledDate1" CssClass="input" runat="server" />
                                    </td>
                                    <td colspan="2" align="center">
                                        <eluc:Date ID="txtInstalledDate2" CssClass="input" runat="server" />
                                    </td>
                                    <td colspan="2" align="center">
                                        <eluc:Date ID="txtInstalledDate3" CssClass="input" runat="server" />
                                    </td>
                                    <td colspan="2" align="center">
                                        <eluc:Date ID="txtInstalledDate4" CssClass="input" runat="server" />
                                    </td>
                                    <td colspan="2" align="center">
                                        <eluc:Date ID="txtInstalledDate5" CssClass="input" runat="server" />
                                    </td>
                                    <td colspan="2" align="center">
                                        <eluc:Date ID="txtInstalledDate6" CssClass="input" runat="server" />
                                    </td>
                                    <td colspan="2" align="center">
                                        <eluc:Date ID="txtInstalledDate7" CssClass="input" runat="server" />
                                    </td>
                                    <td colspan="2" align="center">
                                        <eluc:Date ID="txtInstalledDate8" CssClass="input" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="5">
                                        <asp:TextBox ID="txtInstalled" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNew9" runat="server" Text="New"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNow11" runat="server" Text="Now"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNew10" runat="server" Text="New"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNow22" runat="server" Text="Now"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNew3" runat="server" Text="New"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNow3" runat="server" Text="Now"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNew4" runat="server" Text="New"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNow4" runat="server" Text="Now"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNew5" runat="server" Text="New"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNow5" runat="server" Text="Now"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNew6" runat="server" Text="New"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNow6" runat="server" Text="Now"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNew7" runat="server" Text="New"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNow7" runat="server" Text="Now"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNew8" runat="server" Text="New"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblNow8" runat="server" Text="Now"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="5">
                                        <asp:Literal ID="lblBridgeGaugeReading" runat="server" Text="Bridge Gauge<br />Reading"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGaugeReading1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGaugeReading2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGaugeReading3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGaugeReading4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGaugeReading5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGaugeReading6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGaugeReading7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGaugeReading8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGaugeReading9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGaugeReading10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGaugeReading11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGaugeReading12" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGaugeReading13" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGaugeReading14" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGaugeReading15" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtGaugeReading16" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="center" rowspan="2">
                                        <asp:Literal ID="lblBearingClearance" runat="server" Text="Bearing Clearance"></asp:Literal>
                                    </td>
                                    <td colspan="2">
                                        <asp:Literal ID="lblF2" runat="server" Text="F"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingClearence1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingClearence2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingClearence3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingClearence4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingClearence5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingClearence6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingClearence7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingClearence8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingClearence9" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingClearence10" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingClearence11" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingClearence12" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingClearence13" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingClearence14" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingClearence15" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBearingClearence16" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Literal ID="lbl1A" runat="server" Text="A"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtANew1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtANow1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtANew2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtANow2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtANew3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtANow3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtANew4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtANow4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtANew5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtANow5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtANew6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtANow6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtANew7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtANow7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtANew8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtANow8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblNote" runat="server" Text="Note :"></asp:Literal>
                        </td>
                        <td colspan="13">
                            <asp:Literal ID="lblClearanceOvalityAllowedTobeMentionedWhereapplicable" runat="server"
                                Text="1.&nbsp;&nbsp;Max clearance, ovality allowed, to be mentioned where applicable."></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="7">
                            <asp:Literal ID="lblDuringEveryDecarbonisation" runat="server" Text="2.&nbsp;&nbsp;During every decarbonisation at least two main bearings to be inspected.<br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;At subsequent decarbonisations other bearings
                            to be inspected in order to cover all the main bearings in three decarbonisations."></asp:Literal>
                        </td>
                        <td colspan="6">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" align="right">
                            <asp:TextBox ID="txtNameOfChiefEngineer" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7" align="right">
                            <asp:Literal ID="lblChiefEngineer" runat="server" Text="CHIEF ENGINEER"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:Image ID="ImgCrankpin" runat="server" ImageUrl="~/StandardForm/Images/CrankpinBearingClearance.jpg"
                                Width="300px" Height="300px" />
                        </td>
                        <td colspan="4">
                            <table style="width: 100%;" class="Sftblclass">
                                <tr>
                                    <td colspan="10" align="center" style="background-color: #CCFFFF">
                                        <asp:Literal ID="lblCrankPinJournals" runat="server" Text="CRANKPIN JOURNALS"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5" align="center" style="background-color: #CCFFFF;">
                                        <asp:Literal ID="lblOriginalDiameter" runat="server" Text="ORIGINAL DIAMETER"></asp:Literal>
                                    </td>
                                    <td colspan="5" align="left">
                                        <asp:TextBox ID="txtOriginalDiameter" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="10">
                                        <asp:TextBox ID="txtOriginalDiameter1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="background-color: #CCFFFF">
                                    <td colspan="2" align="center">
                                        <asp:Literal ID="lblNo" runat="server" Text="No."></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblValueOne" runat="server" Text="1"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblValueTwo" runat="server" Text="2"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblValueThree" runat="server" Text="3"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblValueFour" runat="server" Text="4"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblValueFive" runat="server" Text="5"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblValueSix" runat="server" Text="6"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblValueSeven" runat="server" Text="7"></asp:Literal>
                                    </td>
                                    <td align="center">
                                        <asp:Literal ID="lblValueEight" runat="server" Text="8"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" align="center" style="background-color: #CCFFFF">
                                        <asp:Literal ID="lblI" runat="server" Text="I"></asp:Literal>
                                    </td>
                                    <td style="background-color: #CCFFFF" align="center">
                                        <asp:Literal ID="lblV" runat="server" Text="&nbsp;V&nbsp;"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIV1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIV2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIV3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIV4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIV5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIV6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIV7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIV8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="background-color: #CCFFFF">
                                        <asp:Literal ID="lblH" runat="server" Text="&nbsp;H&nbsp;"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIH1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIH2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIH3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIH4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIH5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIH6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIH7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIH8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" align="center" style="background-color: #CCFFFF">
                                        <asp:Literal ID="lblII" runat="server" Text="II"></asp:Literal>
                                    </td>
                                    <td style="background-color: #CCFFFF" align="center">
                                        <asp:Literal ID="lblV2" runat="server" Text="&nbsp;V&nbsp;"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIV1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIV2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIV3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIV4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIV5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIV6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIV7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIV8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="background-color: #CCFFFF">
                                        <asp:Literal ID="lblH2" runat="server" Text="&nbsp;H&nbsp;"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIH1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIH2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIH3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIH4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIH5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIH6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIH7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtIIH8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td rowspan="2" align="center" style="background-color: #CCFFFF;">
                                        <asp:Literal ID="lblBear" runat="server" Text="BEAR.<br />CLEARANCE"></asp:Literal>
                                    </td>
                                    <td align="center" style="background-color: #CCFFFF">
                                        <asp:Literal ID="lblF3" runat="server" Text="&nbsp;F&nbsp;"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBEF1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBEF2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBEF3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBEF4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBEF5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBEF6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBEF7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBEF8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="background-color: #CCFFFF">
                                        <asp:Literal ID="lblA2" runat="server" Text="&nbsp;A&nbsp;"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBEA1" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBEA2" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBEA3" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBEA4" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBEA5" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBEA6" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBEA7" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBEA8" runat="server" CssClass="input"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
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
