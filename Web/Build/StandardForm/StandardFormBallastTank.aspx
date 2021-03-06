<%@ Page Language="C#" AutoEventWireup="true" CodeFile="StandardFormBallastTank.aspx.cs"
    Inherits="StandardFormBallastTank" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Mask" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>14 D16 Ballast Tank Inspection Report</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    </style>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmBallastTank" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Ballast Tank Inspection" ShowMenu="false">
            </eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="StandardForm_TabStripCommand" />
            <asp:Button ID="cmdHiddenConfirm" runat="server" Text="cmdHiddenConfirm" OnClick="StandardFormConfirm_TabStripCommand" />
        </div>
        <asp:UpdatePanel runat="server" ID="pnlStandardForm">
            <ContentTemplate>
                <input type="button" id="cmdPrint" value="Print" onclick="cmdPrint_Click();" runat="server"
                    visible="false" />
                <table width="95%" cellspacing="0px" cellpadding="0px">
                    <tr>
                        <td colspan="6" align="left">
                            <b>
                                <asp:Literal ID="lblCompanyName" runat="server" Text="EXECUTIVE SHIP MANAGEMENT"></asp:Literal></b>
                        </td>
                        <td align="left">
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPB102" runat="server" Text="PB10"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" align="left">
                            &nbsp;
                        </td>
                        <td align="left">
                        </td>
                        <td align="right">
                            <asp:Literal ID="lblPage1of2" runat="server" Text="Page 1 of 2"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="right">
                            <asp:Literal ID="lbl0714Rev1" runat="server" Text="(07/14 Rev 1)"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <h3>
                                <asp:Literal ID="lblBallastTankInspectionReport" runat="server" Text="Ballast Tank / Fresh Water Tank / Void space Inspection Report"></asp:Literal>
                            </h3>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" align="center">
                            <b>
                                <asp:Literal ID="lblTobeSenttotheOfficeinHardCopy" runat="server" Text="(Uploaded in PHOENIX along with Q 25 - Tank Inspection Photo Template and send an e. mail to the Tech. Suptd on the same day)"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel :"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtVesselName" runat="server" Enabled="false" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Literal ID="lblTank" runat="server" Text="Tank :"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTank" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:Literal ID="lblDate" runat="server" Text="Date :"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <b>
                                <asp:Literal ID="lblInspectionIntervalis12Months" runat="server" Text="-Inspection interval is 12 months .For vessel's greater than 10 years old and for
                                vessels carrying heated cargo consistently the interval between inspections is 6 months."></asp:Literal>
                            </b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblA" runat="server" Text="A)"></asp:Literal>
                        </td>
                        <td colspan="7">
                            <asp:Literal ID="lblPhotographs" runat="server" Text="Photographs:"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="5">
                            <asp:Literal ID="lblAreAttachedinaSeparateSheet" runat="server" Text="-Are attached in a separate sheet."></asp:Literal>
                        </td>
                        <td colspan="2">
                            <asp:RadioButton ID="chYes" runat="server" Text="Yes" GroupName="a" />
                            &nbsp;&nbsp;&nbsp;
                            <asp:RadioButton ID="chNo" runat="server" Text="No" GroupName="a" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="5">
                            <asp:Literal ID="lblNumberofPhotographAttachedalongWith" runat="server" Text="-Number of photograph attached along with"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Mask ID="txtPhotoNos" runat="server" CssClass="input" />
                        </td>
                        <td>
                            <asp:Literal ID="lblNos" runat="server" Text="Nos"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblB" runat="server" Text="B)"></asp:Literal>
                        </td>
                        <td colspan="7">
                            <asp:Literal ID="lblIndicateonSketchDamagedArea" runat="server" Text="Indicate on sketch damaged area coating integrity by percentage with reference to tank coating breakdown extent"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="7">
                            <asp:Literal ID="lbldiagramRefertoChapter" runat="server" Text="diagram.<b><u>Refer to Chapter 12-14 of 'ABS Guidance Notes on the Inspection,Maintanance and Application of Marine</u></b>"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="7">
                            <asp:Literal ID="lblCoatingwhichisavailabein" runat="server" Text="<b><u>Coating' which is availabe in the Phoenix Document Management System under 'Class circulars-ABS Publications'.</u></b>"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            <asp:Literal ID="lblDamageCoding" runat="server" Text="Damage coding"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td rowspan="6" colspan="3" style="border: 1px solid #000000">
                            <asp:Literal ID="lblDeckHead" runat="server" Text="&nbsp;&nbsp; Deck Head"></asp:Literal><br />
                            &nbsp;&nbsp;<asp:TextBox ID="txtDeckHead" runat="server" TextMode="MultiLine" CssClass="input"
                                Height="150px" Width="215px"></asp:TextBox>
                        </td>
                        <td colspan="3" rowspan="6">
                            <table width="80%" style="border: 1px solid #000000; text-align: center" align="right">
                                <th>
                                    <asp:Literal ID="lblTickasapprpriate" runat="server" Text="Tick as</br> Appropriate"></asp:Literal>
                                </th>
                                <th>
                                    <asp:Literal ID="lblMudAccumulation" runat="server" Text="Mud Accumulation"></asp:Literal>
                                </th>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chSnegl" runat="server" Text="" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblNegligible" runat="server" Text="Negligible < 1cm"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chSlight" runat="server" Text="" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblSlight" runat="server" Text="Slight- 1 - 2 cm"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chModerate" runat="server" Text="" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblbModerate" runat="server" Text="Moderate-2 - 5 cm"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chHeavy" runat="server" Text="" />
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblHeavy" runat="server" Text="Heavy >5 cm"></asp:Literal>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtScale" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblScale" runat="server" Text="Scale"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtPitting" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblPitting" runat="server" Text="Pitting"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtRust" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblRust" runat="server" Text="Rust"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td rowspan="9" colspan="3" style="border-style: none solid none solid; border-width: 1px;
                            border-color: #000000">
                            &nbsp;&nbsp;T.B<br />
                            &nbsp;&nbsp;<asp:TextBox ID="txtTranverseBulkheads" runat="server" TextMode="MultiLine"
                                CssClass="input" Height="150px" Width="215px"></asp:TextBox>
                        </td>
                        <td colspan="3" rowspan="9">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtBuckling" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblBuckling" runat="server" Text="Buckling"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtFractures" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblFractures" runat="server" Text="Fractures"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtCoatingDamage" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblCoatingDamage" runat="server" Text="Coating Damage"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtDeposits" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblDeposits" runat="server" Text="Deposits"></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="border-style: solid none solid solid; border-width: 1px; border-color: #000000;">
                            <asp:Literal ID="lblCB" runat="server" Text="&nbsp;&nbsp;C.B"></asp:Literal><br />
                            &nbsp;&nbsp;<asp:TextBox ID="txtCrossBeams" runat="server" TextMode="MultiLine" CssClass="input"
                                Height="150px" Width="215px"></asp:TextBox>
                        </td>
                        <td colspan="3" style="border: 1px solid #000000;">
                            <asp:Literal ID="lblBS" runat="server" Text="&nbsp;&nbsp;B.S"></asp:Literal><br />
                            &nbsp;&nbsp;<asp:TextBox ID="txtBottomShell" runat="server" TextMode="MultiLine"
                                CssClass="input" Height="150px" Width="215px"></asp:TextBox>
                        </td>
                        <td colspan="3" style="border-style: solid solid solid none; border-width: 1px; border-color: #000000;">
                            <asp:Literal ID="lblSS" runat="server" Text="&nbsp;&nbsp;S.S &nbsp;&nbsp;"></asp:Literal><asp:TextBox
                                ID="txtSdieShell" runat="server" TextMode="MultiLine" CssClass="input" Height="150px"
                                Width="215px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            &nbsp;
                        </td>
                        <td colspan="3" style="border-style: none solid solid solid; border-width: 1px; border-color: #000000">
                            <asp:Literal ID="lblLH" runat="server" Text="&nbsp;&nbsp;L.H"></asp:Literal><br />
                            &nbsp;&nbsp;<asp:TextBox ID="txtLongitudinalBulkHead" runat="server" TextMode="MultiLine"
                                CssClass="input" Height="150px" Width="215px"></asp:TextBox>
                        </td>
                        <td colspan="3">
                            <table width="90%" cellpadding="0px" cellspacing="0px" style="border: 1px solid #000000;
                                margin-left: 15px; height: -13px;">
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblCondition" runat="server" Text="&nbsp;&nbsp;Condition"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblNotA" runat="server" Text="&nbsp;&nbsp;GOOD"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblSAT" runat="server" Text="&nbsp;&nbsp;SAT"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblRA" runat="server" Text="&nbsp;&nbsp;POOR*"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:Literal ID="Literal1" runat="server" Text="&nbsp;&nbsp;NA"></asp:Literal>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <hr style="border-style: none none solid none; border-width: 1px; border-color: #000000;
                                            height: -13px; width: 100%; color: #000000;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblStrikerPlate" runat="server" Text="&nbsp;&nbsp;Striker Plate"></asp:Literal>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="strikerplate1" Text="" runat="server" GroupName="strikerplate" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                        <asp:RadioButton ID="strikerplate2" Text="" runat="server" GroupName="strikerplate" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="strikerplate3" Text="" runat="server" GroupName="strikerplate" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="strikerplate4" Text="" runat="server" GroupName="strikerplate" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblLadders" runat="server" Text="&nbsp;&nbsp;Ladders"></asp:Literal>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnLadders1" Text="" runat="server" GroupName="Ladders" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnLadders2" Text="" runat="server" GroupName="Ladders" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnLadders3" Text="" runat="server" GroupName="Ladders" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbtnLadders4" Text="" runat="server" GroupName="Ladders" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblSoundingPipe" runat="server" Text="&nbsp;&nbsp;Sounding Pipe"></asp:Literal>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnSoundingPipe1" Text="" runat="server" GroupName="SoundingPipe" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnSoundingPipe2" Text="" runat="server" GroupName="SoundingPipe" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnSoundingPipe3" Text="" runat="server" GroupName="SoundingPipe" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbtnSoundingPipe4" Text="" runat="server" GroupName="SoundingPipe" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblLevelGauge" runat="server" Text="&nbsp;&nbsp;Level Gauge"></asp:Literal>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnLevelGauge1" Text="" runat="server" GroupName="LevelGauge" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnLevelGauge2" Text="" runat="server" GroupName="LevelGauge" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnLevelGauge3" Text="" runat="server" GroupName="LevelGauge" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbtnLevelGauge4" Text="" runat="server" GroupName="LevelGauge" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblBallastPump" runat="server" Text="&nbsp;&nbsp;Ballast Pump"></asp:Literal>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnBallastPump1" Text="" runat="server" GroupName="BallastPump" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnBallastPump2" Text="" runat="server" GroupName="BallastPump" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnBallastPump3" Text="" runat="server" GroupName="BallastPump" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbtnBallastPump4" Text="" runat="server" GroupName="BallastPump" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblSuctionwell" runat="server" Text="&nbsp;&nbsp;Suction well /<br>&nbsp; bell mouth"></asp:Literal>
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnSuctionwell" Text="" runat="server" GroupName="Suctionwell" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnSuctionwell2" Text="" runat="server" GroupName="Suctionwell" />
                                    </td>
                                    <td>
                                        &nbsp;&nbsp;&nbsp;
                                        <asp:RadioButton ID="rbtnSuctionwell3" Text="" runat="server" GroupName="Suctionwell" />
                                    </td>
                                    <td>
                                        <asp:RadioButton ID="rbtnSuctionwell4" Text="" runat="server" GroupName="Suctionwell" />
                                    </td>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <asp:Literal ID="lblIfconditionispoor" runat="server" Text="* If condition is poor, then details to  be recorded  in the  comments"></asp:Literal>
                        </td>
                    </tr>
                </table>
                </td> </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblcommentsarea" Text="Comments" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                <td colspan="10">
                    <asp:TextBox ID="txtcomments" runat="server" Text="" TextMode="MultiLine" CssClass="input"
                        Height="75px" Width="750px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td colspan="8">
                        <br />
                        <br />
                    </td>
                </tr>
                
                <tr>
                    <td colspan="6" align="left">
                        <b>
                            <asp:Literal ID="lblExecutiveShipManagement1" runat="server" Text="EXECUTIVE SHIP MANAGEMENT"></asp:Literal></b>
                    </td>
                    <td align="left">
                    </td>
                    <td align="right">
                        <asp:Literal ID="lblPB10" runat="server" Text="PB10"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" align="left">
                        &nbsp;
                    </td>
                    <td align="left">
                    </td>
                    <td align="right">
                        <asp:Literal ID="lblPage12" runat="server" Text="Page 2 of 2"></asp:Literal>
                    </td>
                </tr>
                <tr>
                    <td colspan="8" align="right">
                        <asp:Literal ID="lbL0714Rev" runat="server" Text="(07/14 Rev 1)"></asp:Literal>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblVessel1" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtVessel1" runat="server" CssClass="input" Enabled="false"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblDate1" runat="server" Text="Date"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Date ID="txtDate1" runat="server" CssClass="input" DatePicker="true" />
                        <td>
                            <asp:Literal ID="lblTank1" runat="server" Text="Tank"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTank1" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <table width="100%" style="border: 1px solid #000000">
                            <tr>
                                <td align="center">
                                    <h3>
                                        <asp:Literal ID="lblTankInspectionReport" runat="server" Text="TANK/VOID SPACE INSPECTION REPORT"></asp:Literal>
                                    </h3>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="8" align="center">
                                    <table cellpadding="1px" cellspacing="1px" class="Sftblclass" width="98%">
                                        <tr>
                                            <td align="center" rowspan="2">
                                                <div>
                                                    <asp:Literal ID="lblAsur" runat="server" Text="A.SUR"></asp:Literal>
                                                </div>
                                            </td>
                                            <td align="center" rowspan="2">
                                                <asp:Literal ID="lblStructure" runat="server" Text="STRUCTURE"></asp:Literal>
                                            </td>
                                            <td colspan="2" align="center">
                                                <asp:Literal ID="lblScale1" runat="server" Text="SCALE"></asp:Literal>
                                            </td>
                                            <td colspan="3" align="center">
                                                <asp:Literal ID="lblPitting1" runat="server" Text="PITTTING"></asp:Literal>
                                            </td>
                                            <td align="center" rowspan="2">
                                                <div>
                                                    <asp:Literal ID="lblVL" runat="server" Text="V.L"></asp:Literal>
                                                </div>
                                            </td>
                                            <td align="center" rowspan="2">
                                                <div>
                                                    <asp:Literal ID="lblUT" runat="server" Text="U.T"></asp:Literal>
                                                </div>
                                            </td>
                                            <td align="center" rowspan="2">
                                                <div>
                                                    <asp:Literal ID="lblUR" runat="server" Text="U.R"></asp:Literal>
                                                </div>
                                            </td>
                                            <td align="center" rowspan="2" width="5%">
                                                &nbsp;
                                            </td>
                                            <td align="center" rowspan="2">
                                                <div>
                                                    <asp:Literal ID="lblFs" runat="server" Text="F.S"></asp:Literal>
                                                </div>
                                            </td>
                                            <td align="center" rowspan="2">
                                                <div>
                                                    <asp:Literal ID="lblBG" runat="server" Text="B.G"></asp:Literal>
                                                </div>
                                            </td>
                                            <td align="center" rowspan="2">
                                                <div>
                                                    <asp:Literal ID="lblWs" runat="server" Text="W.S"></asp:Literal>
                                                </div>
                                            </td>
                                            <td align="center" rowspan="2">
                                                <div>
                                                    <asp:Literal ID="lblCF" runat="server" Text="%<br />C.F"></asp:Literal>
                                                </div>
                                            </td>
                                            <td align="center" rowspan="2">
                                                <div>
                                                    <asp:Literal ID="lblAR" runat="server" Text="%<br />A.R"></asp:Literal>
                                                </div>
                                            </td>
                                            <td align="center" rowspan="2">
                                                <div>
                                                    <asp:Literal ID="lblSK" runat="server" Text="S.K"></asp:Literal>
                                                </div>
                                            </td>
                                            <td align="center" rowspan="2" width="20%">
                                                <asp:Literal ID="lblComments" runat="server" Text="COMMENTS"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <div>
                                                    <asp:Literal ID="lblTKmm" runat="server" Text="T.K<br /> mm"></asp:Literal>
                                                </div>
                                            </td>
                                            <td align="center">
                                                <div>
                                                    <asp:Literal ID="lblTe" runat="server" Text="T.E"></asp:Literal>
                                                </div>
                                            </td>
                                            <td align="center">
                                                <div>
                                                    <asp:Literal ID="lblDPmm" runat="server" Text="D.P<br />mm"></asp:Literal>
                                                </div>
                                            </td>
                                            <td align="center">
                                                <div>
                                                    <asp:Literal ID="lblDmmm" runat="server" Text="D.M<br />mm"></asp:Literal>
                                                </div>
                                            </td>
                                            <td align="center">
                                                <div>
                                                    <asp:Literal ID="lblIn" runat="server" Text="I.N<br />%"></asp:Literal>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" rowspan="3">
                                                <div>
                                                    <asp:Literal ID="lblSH" runat="server" Text="S.S<br />"></asp:Literal>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Literal ID="lblPlating" runat="server" Text="PLATING"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideShell1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideShell2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideShell3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideShell4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideShell5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideShell6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideShell7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideShell8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideShell9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideShell10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideShell11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideShell12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideShell13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideShell14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideShell15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideShell16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblLongitudinal" runat="server" Text="LONGITUDINAL"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideLongitunal1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideLongitunal2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideLongitunal3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideLongitunal4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideLongitunal5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideLongitunal6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideLongitunal7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideLongitunal8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideLongitunal9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideLongitunal10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideLongitunal11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideLongitunal12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideLongitunal13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideLongitunal14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideLongitunal15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideLongitunal16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblWebFrames" runat="server" Text="WEB FRAMES"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideFrames1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideFrames2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideFrames3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideFrames4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideFrames5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideFrames6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideFrames7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideFrames8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideFrames9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideFrames10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideFrames11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideFrames12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideFrames13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideFrames14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideFrames15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSideFrames16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" rowspan="2">
                                                <div>
                                                    <asp:Literal ID="lblCB1" runat="server" Text="C.B"></asp:Literal>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Literal ID="lblPlating1" runat="server" Text="PLATING"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCrossPlating1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCrossPlating2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCrossPlating3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCrossPlating4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCrossPlating5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCrossPlating6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCrossPlating7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCrossPlating8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCrossPlating9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCrossPlating10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCrossPlating11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCrossPlating12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCrossPlating13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCrossPlating14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCrossPlating15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCrossPlating16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblStiffeners" runat="server" Text="STIFFENERS"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStiffencers1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStiffencers2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStiffencers3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStiffencers4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStiffencers5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStiffencers6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStiffencers7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStiffencers8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStiffencers9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStiffencers10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStiffencers11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStiffencers12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStiffencers13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStiffencers14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStiffencers15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtStiffencers16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" rowspan="5">
                                                <div>
                                                    <asp:Literal ID="lblBs1" runat="server" Text="B.S"></asp:Literal>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Literal ID="lblPlating2" runat="server" Text="PLATING"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomPlating1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomPlating2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomPlating3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomPlating4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomPlating5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomPlating6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomPlating7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomPlating8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomPlating9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomPlating10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomPlating11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomPlating12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomPlating13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomPlating14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomPlating15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomPlating16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblLongitudinals" runat="server" Text="LONGITUDINALS"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomLongitudinal1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomLongitudinal2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomLongitudinal3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomLongitudinal4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomLongitudinal5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomLongitudinal6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomLongitudinal7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomLongitudinal8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomLongitudinal9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomLongitudinal10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomLongitudinal11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomLongitudinal12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomLongitudinal13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomLongitudinal14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomLongitudinal15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBottomLongitudinal16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblForeAftGirders" runat="server" Text="FORE & AFT GIRDERS"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirders1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirders2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirders3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirders4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirders5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirders6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirders7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirders8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirders9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirders10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirders11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirders12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirders13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirders14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirders15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirders16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblWebFrame" runat="server" Text="WEB FRAMES"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirdersFrames1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirdersFrames2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirdersFrames3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirdersFrames4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirdersFrames5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirdersFrames6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirdersFrames7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirdersFrames8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirdersFrames9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirdersFrames10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirdersFrames11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirdersFrames12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirdersFrames13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirdersFrames14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirdersFrames15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtGirdersFrames16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblBellMouths" runat="server" Text="BELLMOUTHS"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBell1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBell2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBell3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBell4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBell5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBell6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBell7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBell8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBell9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBell10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBell11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBell12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBell13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBell14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBell15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBell16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" rowspan="4">
                                                <div>
                                                    <asp:Literal ID="lblLB" runat="server" Text="L.B"></asp:Literal>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Literal ID="lblPlating4" runat="server" Text="PLATING"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellPlating1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellPlating2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellPlating3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellPlating4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellPlating5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellPlating6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellPlating7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellPlating8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellPlating9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellPlating10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellPlating11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellPlating12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellPlating13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellPlating14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellPlating15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellPlating16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblLongitudinal1" runat="server" Text="LONGITUDINALS"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellLongitudinal1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellLongitudinal2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellLongitudinal3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellLongitudinal4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellLongitudinal5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellLongitudinal6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellLongitudinal7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellLongitudinal8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellLongitudinal9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellLongitudinal10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellLongitudinal11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellLongitudinal12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellLongitudinal13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellLongitudinal14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellLongitudinal15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellLongitudinal16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblWebFrames1" runat="server" Text="WEB FRAMES"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellFrames1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellFrames2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellFrames3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellFrames4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellFrames5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellFrames6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellFrames7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellFrames8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellFrames9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellFrames10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellFrames11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellFrames12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellFrames13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellFrames14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellFrames15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtBellFrames16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblOpenings" runat="server" Text="OPENINGS"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOpenings1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOpenings2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOpenings3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOpenings4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOpenings5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOpenings6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOpenings7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOpenings8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOpenings9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOpenings10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOpenings11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOpenings12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOpenings13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOpenings14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOpenings15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtOpenings16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" rowspan="4">
                                                <div>
                                                    <asp:Literal ID="lblDH" runat="server" Text="D.H"></asp:Literal>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Literal ID="lblPlating5" runat="server" Text="PLATING"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckPlate1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckPlate2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckPlate3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckPlate4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckPlate5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckPlate6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckPlate7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckPlate8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckPlate9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckPlate10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckPlate11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckPlate12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckPlate13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckPlate14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckPlate15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckPlate16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblLongitudinals1" runat="server" Text="LONGITUDINALS"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckLongitudinal1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckLongitudinal2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckLongitudinal3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckLongitudinal4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckLongitudinal5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckLongitudinal6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckLongitudinal7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckLongitudinal8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckLongitudinal9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckLongitudinal10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckLongitudinal11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckLongitudinal13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckLongitudinal12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckLongitudinal14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckLongitudinal15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckLongitudinal16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblForeandAFtGirders" runat="server" Text="FORE & AFT GIRDERS"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFore1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFore2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFore3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFore4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFore5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFore6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFore7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFore8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFore9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFore10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFore11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFore12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFore13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFore14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFore15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFore16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblWebFrames3" runat="server" Text="WEB FRAMES"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFrames1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFrames2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFrames3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFrames4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFrames5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFrames6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFrames7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFrames8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFrames9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFrames10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFrames11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFrames12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFrames13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFrames14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFrames15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDeckFrames16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="center" rowspan="4">
                                                <div>
                                                    <asp:Literal ID="lblTB" runat="server" Text="T.B"></asp:Literal>
                                                </div>
                                            </td>
                                            <td>
                                                <asp:Literal ID="lblPlating6" runat="server" Text="PLATING"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseBulk1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseBulk2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseBulk4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseBulk3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseBulk5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseBulk6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseBulk7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseBulk8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseBulk9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseBulk10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseBulk11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseBulk12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseBulk13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseBulk14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseBulk15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseBulk16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblStiffenrs" runat="server" Text="STIFFENERS"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseStiffeners1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseStiffeners2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseStiffeners3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseStiffeners4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseStiffeners5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseStiffeners6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseStiffeners7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseStiffeners8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseStiffeners9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseStiffeners10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseStiffeners11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseStiffeners12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseStiffeners13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseStiffeners14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseStiffeners15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseStiffeners16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblWebFrames4" runat="server" Text="WEB FRAMES"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWeb1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWeb2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWeb3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWeb4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWeb5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWeb6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWeb7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWeb8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWeb9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWeb10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWeb11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWeb12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWeb13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWeb14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWeb15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWeb16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Literal ID="lblStringers" runat="server" Text="STRINGERS"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWebStingers1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWebStingers2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWebStingers3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWebStingers4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWebStingers5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWebStingers6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWebStingers7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWebStingers8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWebStingers9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWebStingers10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWebStingers11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWebStingers12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWebStingers13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWebStingers14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWebStingers15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransverseWebStingers16" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:Literal ID="lblPipelines" runat="server" Text="PIPELINES"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPipeLines1" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPipeLines2" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPipeLines3" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPipeLines4" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPipeLines5" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPipeLines6" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPipeLines7" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPipeLines8" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPipeLines9" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPipeLines10" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPipeLines11" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPipeLines12" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPipeLines13" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPipeLines14" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPipeLines15" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPipeLines16" runat="server" CssClass="input"></asp:TextBox>
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
                                <td colspan="8" align="center">
                                    <table width="98%">
                                        <tr>
                                            <td colspan="2" align="left">
                                                <b>
                                                    <asp:Literal ID="lblSymbols" runat="server" Text="SYMBOLS"></asp:Literal></b>
                                            </td>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                            <td colspan="5" align="left">
                                                <b>
                                                    <asp:Literal ID="lblAbbreviationsForScale" runat="server" Text="ABBREVIATIONS FOR SCALE"></asp:Literal></b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Literal ID="lblSelect" runat="server" Text="Y"></asp:Literal>
                                            </td>
                                            <td align="left">
                                                <asp:Literal ID="lblInspectedAndSatisfactory" runat="server" Text="INSPECTED AND SATISFACTORY"></asp:Literal>
                                            </td>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                            <td colspan="2" align="left">
                                                <asp:Literal ID="lblHTHardTightScale" runat="server" Text="H.T. - HARD TIGHT SCALE"></asp:Literal>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="2">
                                                <asp:Literal ID="lblInspectionCarriedOutBy" runat="server" Text="Inspection carried out by:"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Image runat="server" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ID="cmdCancel">
                                                </asp:Image>
                                            </td>
                                            <td align="left">
                                                <asp:Literal ID="lblNotInspected" runat="server" Text="NOT INSPECTED"></asp:Literal>
                                            </td>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                            <td colspan="5" align="left">
                                                <asp:Literal ID="lblCSCalcareousScale" runat="server" Text="C.S. - CALCAREOUS SCALE"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left">
                                                <asp:Literal ID="lblNA" runat="server" Text="NA"></asp:Literal>
                                            </td>
                                            <td align="left">
                                                <asp:Literal ID="lblNotApplicable" runat="server" Text="NOT APPLICABLE"></asp:Literal>
                                            </td>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                            <td colspan="2" align="left">
                                                <asp:Literal ID="lblLsLooseScale" runat="server" Text="L.S. - LOOSE SCALE"></asp:Literal>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtInspectionBy" runat="server" CssClass="input"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                &nbsp;
                                            </td>
                                            <td colspan="5" align="left">
                                                <asp:Literal ID="lblBsBlisterScale" runat="server" Text="B.S. - BLISTER SCALE"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <caption>
                                                <br />
                                            </caption>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <b>
                                                    <asp:Literal ID="lblAbbreviations" runat="server" Text="ABBREVIATIONS"></asp:Literal></b>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblASURAreaSurveyed" runat="server" Text="A.SUR. - AREA SURVEYED"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblTkThickness" runat="server" Text="T.K- THICKNESS"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblTEType" runat="server" Text="T.E- TYPE"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblDPDepth" runat="server" Text="D.P- DEPTH"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblDMDiameter" runat="server" Text="D.M- DIAMETER"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblINIntensity" runat="server" Text="I.N- INTENSITY"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblVLVisibleThicknessLoss" runat="server" Text="V.L- VISIBLE THICKNESS LOSS"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblUTUltrasonicReadingTaken" runat="server" Text="U.T- ULTRASONIC READING TAKEN"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblURUltrasonicReadingRequired" runat="server" Text="U.R- ULTRASONIC READING REQUIRED"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblFSFractures" runat="server" Text="F.S- FRACTURES"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblBGBuckling" runat="server" Text="B.G- BUCKLING"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblWSWeldSeams" runat="server" Text="W.S- WELD SEAMS"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblCFCoatingFailure" runat="server" Text="C.F- COATING FAILURE"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblARANodesRemaining" runat="server" Text="A.R- ANODES REMAINING"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblSKSeeSketchno" runat="server" Text="S.K- SEE SKETCHNO"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblssSideShell" runat="server" Text="S.S- SIDE SHELL"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblCBCrossBeams" runat="server" Text="C.B- CROSS BEAMS"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblBsBottomShell" runat="server" Text="B.S- BOTTOM SHELL"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblLHLongitudinalBulkHead" runat="server" Text="L.H- LONGITUDINAL BULKHEAD"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblDHDeckHead" runat="server" Text="D.H- DECKHEAD"></asp:Literal>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="9" align="left">
                                                <asp:Literal ID="lblTBTransverseBulkHead" runat="server" Text="T.B- TRANSVERSE BULKHEAD"></asp:Literal>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
