<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionDepartureReportEdit.aspx.cs"
    Inherits="VesselPositionDepartureReportEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Voyage" Src="~/UserControls/UserControlVoyage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Cargo" Src="~/UserControls/UserControlCargo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PortActivity" Src="~/UserControls/UserControlPortActivity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnVoyagePort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="../UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="../UserControls/UserControlDecimal.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <title>Departure Report</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function rowClick(sender, eventArgs) {
                sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
            }
        </script>
    </telerik:RadCodeBlock>
     <Style>
        .RadGrid .rgHeader, .RadGrid th.rgResizeCol,.RadGrid .rgRow td,.RadGrid .rgAltRow td {
            padding-left: 2px !important;
            padding-right: 2px !important;
        }
    </Style>
</head>
<body>
    <form id="frmDepartureReport" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="panel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuDRSubTab" runat="server" TabStrip="true" OnTabStripCommand="MenuDRSubTab_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />

            <eluc:TabStrip ID="ProjectBilling" runat="server" OnTabStripCommand="ProjectBilling_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblAlertSenttoOFC" runat="server" Text="Report Sent to Office" Visible="false" Font-Bold="False" Font-Size="Large"
                            Font-Underline="True" ForeColor="Red" BorderColor="Red">
                        </telerik:RadLabel>
                        <br />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%;">
                        <telerik:RadLabel ID="lblmVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td style="width: 25%;">
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true" Width="180px"
                            AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmVoyageNo" runat="server" Text="Voyage No"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <span id="spnPickListVoyage">
                            <telerik:RadTextBox ID="txtVoyageName" runat="server" Width="180px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowVoyage" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" Visible="false"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtVoyageId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <table>
                            <tr>
                                <td>
                                    <eluc:Multiport ID="ucDeparturePort" runat="server" CssClass="input_mandatory" Width="180px"
                                        OnTextChangedEvent="ucDeparturePort_Changed" />
                                </td>
                                <td>
                                    <b>
                                        <telerik:RadCheckBox runat="server" ID="chkOffPortLimits" Text="Off Port Limits" />
                                    </b></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmNameofBerth" runat="server" Text="Name of Berth/Location"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtBirthName" CssClass="input" MaxLength="200" Width="180px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmLatitude" runat="server" Text="Latitude"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Latitude ID="ucLatitude" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmLongitude" runat="server" Text="Longitude"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Longitude ID="ucLongitude" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShipMeanTime" runat="server" Text="Ship Mean Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <b>
                            <telerik:RadLabel ID="ltrlUTC" runat="server" Text="UTC"></telerik:RadLabel>
                        </b>
                        <telerik:RadComboBox runat="server" ID="txtShipMeanTimeSymbol" CssClass="dropdown_mandatory" Width="50px">
                            <Items>
                                <telerik:RadComboBoxItem Text="+" Value="+" Selected="True"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="-" Value="-"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                        <eluc:Decimal runat="server" ID="txtShipMeanTime" CssClass="input_mandatory" Mask="99.9" Width="40px"></eluc:Decimal>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <b>
                            <telerik:RadLabel ID="ltrlLT" runat="server" Text="LT"></telerik:RadLabel>
                        </b>
                        <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="200" height="0" />
                        <b>
                            <telerik:RadLabel ID="ltrlUTCDate" runat="server" Text="UTC"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmDepartureDate" runat="server" Text="Departure Date"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date ID="txtDate" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtDepTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCDate" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmPilotonBoard" runat="server" Text="Pilot on Board (POB)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtPOB" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtPOBTime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCPOB" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCPOBTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmStandbyEngines" runat="server" Text="Standby Engines (SBE)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtSBE" CssClass="input_mandatory" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtSBETime" runat="server" Width="80px" CssClass="input_mandatory"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCSBE" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCSBETime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmLLC" runat="server" Text="All Gone and Clear (LLC)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtLLC" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtLLCTime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCLLC" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCLLCTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmAAW" runat="server" Text="Anchor Aweigh (AAW)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtAAW" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtAAWTime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCAAW" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCAAWTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmDLOSP" runat="server" Text="Dropping of Last Outward Sea Pilot (DLOSP)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtDLOSP" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtDLOSPTime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCDLOSP" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCDLOSPTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRFA" runat="server" Text="Ring Full Away (RFA)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtRFA" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtRFATime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCRFA" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCRFATime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmCOSP" runat="server" Text="Commencement of Sea Passage (COSP)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtCOSP" CssClass="input_mandatory" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtCOSPTime" runat="server" Width="80px" CssClass="input_mandatory"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCCOSP" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCCOSPTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmBallastLaden" runat="server" Text="Ballast/Laden"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rbtnBallastLaden" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Ballast" Value="0"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="Laden" Value="1"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmStartNewVoyage" runat="server" Text="Start New Voyage"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <span id="spnNewVoyage">
                            <telerik:RadTextBox ID="txtNewVoyageName" runat="server" Width="180px" CssClass="input"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" AlternateText=".." ID="imgShowNewVoyage" ToolTip="Voyage">
                             <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtNewVoyageId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbloffhiredelay" runat="server" Text="Off Hire/Delay"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtoffhiredelay" runat="server" Width="80px" CssClass="input" DecimalPlace="2"
                            IsPositive="true" MaxLength="9" />
                        &nbsp;
                                <telerik:RadLabel ID="lbloffhiredelayhrs" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmManoeuvering" runat="server" Text="Manoeuvering"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtManoevering" runat="server" Width="80px" CssClass="readonlytextbox"
                            DecimalPlace="2" IsPositive="true" MaxLength="9" Enabled="false" />
                        &nbsp;
                                <telerik:RadLabel ID="lblmManoeuveringhrs" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmManoeuveringdistance" runat="server" Text="Manoeuvering distance"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtHarbourSteamingDist" runat="server" CssClass="input" Width="80px"
                            DecimalPlace="2" IsPositive="true" MaxLength="9" />
                        &nbsp;
                                <telerik:RadLabel ID="lblmManoeuveringdistancenm" runat="server" Text="nm"></telerik:RadLabel>


                        <telerik:RadLabel ID="lblEMLogcounteratSBE" runat="server" Text="EM Log Counter at SBE" Visible="false"></telerik:RadLabel>
                        <eluc:Number ID="txtEMLogcounteratSBE" runat="server" CssClass="input" Width="80px" Visible="false"
                            DecimalPlace="2" IsPositive="true" MaxLength="15" />

                        <telerik:RadLabel ID="lblEMLogcounteratCOSP" runat="server" Text="EM Log Counter at COSP" Visible="false"></telerik:RadLabel>
                        <eluc:Number ID="txtEMLogcounteratCOSP" runat="server" CssClass="input" Width="80px" Visible="false"
                            DecimalPlace="2" IsPositive="true" MaxLength="15" />
                        &nbsp;
                                 <telerik:RadCheckBox runat="server" ID="chkCounterDefective" AutoPostBack="True" Visible="false"
                                     OnCheckedChanged="chkCounterDefective_CheckedChanged" Text=" counter defective / Reset" />

                        <telerik:RadLabel ID="lblEMLogManoeuveringDistance" runat="server" Text="EM Log Manoeuvering Distance" Visible="false"></telerik:RadLabel>
                        <eluc:Number ID="txtEMLogManoeuveringDistance" runat="server" CssClass="input" Width="80px" Visible="false"
                            DecimalPlace="2" IsPositive="true" MaxLength="9" />
                        &nbsp;
                                <telerik:RadLabel ID="Literal3" runat="server" Text="nm" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmNextPort" runat="server" Text="Next Port"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Multiport ID="ucNextPort" runat="server" CssClass="input" Width="180px" OnTextChangedEvent="ucNextPort_Changed" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmNextPortoperation" runat="server" Text="Next Port operation"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:PortActivity ID="txtNextPortOperation" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmDistancetoGo" runat="server" Text="Distance to Go"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtDistance" runat="server" CssClass="input" Width="80px" DecimalPlace="2"
                            IsPositive="true" MaxLength="9" />
                        &nbsp;
                                <telerik:RadLabel ID="lblmDistancetoGonm" runat="server" Text="nm"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmETA" runat="server" Text="ETA"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date ID="txtETA" runat="server" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtTimeOfETA" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmQuantityofSludgeLanded" runat="server" Text="Quantity of Sludge Landed"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtSludgeLandedQty" runat="server" CssClass="input" DecimalPlace="2"
                            Width="80px" IsPositive="true" MaxLength="9" />
                        &nbsp;
                                <telerik:RadLabel ID="lblmQuantityofSludgeLandedcum" runat="server" Text="cu.m"></telerik:RadLabel>
                        &nbsp;&nbsp;
                                <eluc:Date runat="server" ID="txtSludgeLanded" CssClass="input" DatePicker="true" />
                        <telerik:RadTextBox ID="txtSludgeLandedTime" runat="server" CssClass="input" Width="50px"
                            Visible="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmBilgeWaterLandedQty" runat="server" Text="Quantity of Bilge Water Landed"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtBilgeWaterLandedQty" runat="server" CssClass="input" DecimalPlace="2"
                            Width="80px" IsPositive="true" MaxLength="9" />
                        &nbsp;
                                <telerik:RadLabel ID="lblmBilgeWaterLandedQtycum" runat="server" Text="cu.m"></telerik:RadLabel>
                        &nbsp;&nbsp;
                                <eluc:Date runat="server" ID="txtBilgeWaterLanded" CssClass="input" DatePicker="true" />
                        <telerik:RadTextBox ID="txtBilgeWaterLandedTime" runat="server" CssClass="input" Width="50px"
                            Visible="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmGarbageLandedQty" runat="server" Text="Quantity of Garbage Landed"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtGarbageLandedQty" runat="server" CssClass="input" DecimalPlace="2"
                            Width="80px" IsPositive="true" MaxLength="9" />
                        &nbsp;
                                <telerik:RadLabel ID="lblmGarbageLandedQtycum" runat="server" Text="cu.m"></telerik:RadLabel>
                        &nbsp;&nbsp;
                                <eluc:Date runat="server" ID="txtGarbageLanded" CssClass="input" DatePicker="true" />
                        <telerik:RadTextBox ID="txtGarbageLandedTime" runat="server" CssClass="input" Width="50px"
                            Visible="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmLOSampleLanded" runat="server" Text="LO Sample Landed?"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadRadioButtonList runat="server" ID="rbLOSampleLanded" AppendDataBoundItems="true"
                                        Direction="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbLOSampleLanded_OnSelectedIndexChanged">
                                        <Items>
                                            <telerik:ButtonListItem Text="Yes" Value="1"></telerik:ButtonListItem>
                                            <telerik:ButtonListItem Text="No" Value="0" Selected="True"></telerik:ButtonListItem>
                                        </Items>
                                    </telerik:RadRadioButtonList>
                                </td>
                                <td>
                                    <eluc:Date runat="server" ID="txtLOSampleLanded" CssClass="input" DatePicker="true"
                                        Enabled="false" />
                                    <telerik:RadTextBox ID="txtLOSampleLandedTime" runat="server" CssClass="input" Width="50px"
                                        Visible="false">
                                    </telerik:RadTextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" Width="375px" Height="70px" Resize="Both"
                            TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td colspan="2" style="width: 20%">
                        <b>
                            <telerik:RadLabel ID="VoyagePlanHead" runat="server" Text="Voyage Planning"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 20%">
                        <telerik:RadLabel ID="VoyagePlanInstruction" runat="server" Text="To Improve Energy Efficiency, the following factors have been <br/> considered, prior to this voyage:"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblOptimumSpeed" runat="server" Text="Optimum Speed"></telerik:RadLabel>
                    </td>
                    <td style="width: 80%">
                        <telerik:RadCheckBox runat="server" ID="ChkOptimumSpeed" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblOptimumTrim" runat="server" Text="Optimum Trim"></telerik:RadLabel>
                    </td>
                    <td style="width: 80%">
                        <telerik:RadCheckBox runat="server" ID="chkOptimumTrim" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblMostEfficientRoute" runat="server" Text="Most Efficient Route"></telerik:RadLabel>
                    </td>
                    <td style="width: 80%">
                        <telerik:RadCheckBox runat="server" ID="chkMostEfficientRoute" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblCargoStowage" runat="server" Text="Cargo Stowage"></telerik:RadLabel>
                    </td>
                    <td style="width: 80%">
                        <telerik:RadCheckBox runat="server" ID="chkCargoStowage" />
                    </td>
                </tr>
            </table>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td colspan="4">
                        <b>
                            <telerik:RadLabel ID="lblmServicesinPort" runat="server" Text="Services in Port"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadGrid ID="gvServices" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                            Width="100%" CellPadding="2" OnItemDataBound="gvServices_ItemDataBound" ShowFooter="false" OnNeedDataSource="gvServices_NeedDataSource"
                            ShowHeader="true" EnableViewState="false" AllowSorting="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDDEPARTURESERVICEID" CommandItemDisplay="Top">
                                <HeaderStyle Width="102px" />
                                <NoRecordsTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />

                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Service Type">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblServiceType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERVICETYPE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Qty">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblQty" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblDepServiceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTURESERVICEID") %>'></telerik:RadLabel>
                                            <eluc:Number ID="txtQtyEdit" runat="server" CssClass="input txtNumber" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>'
                                                IsPositive="true" IsInteger="true" MaxLength="9" />
                                            <telerik:RadLabel ID="lblServiceIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERVICETYPEID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblDepServiceIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTURESERVICEID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblVesselDepartureID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELDEPARTUREID") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Unit">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblUnit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblUnitEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                            <telerik:RadLabel runat="server" ID="lblUnitId" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNIT") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Est Cost">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblEstCost" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOST") %>'></telerik:RadLabel>
                                            <eluc:Number ID="txtEstCostEdit" runat="server" CssClass="input txtNumber" DecimalPlace="2"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOST") %>' IsPositive="true"
                                                MaxLength="9" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Currency">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCurrency" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                            <eluc:Currency ID="ucCurrency" AppendDataBoundItems="true" runat="server" CurrencyList='<%# PhoenixRegistersCurrency.ListActiveCurrency(1, true) %>' SelectedCurrency='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCY") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Service Co">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblServiceComp" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSERVICECOMPANY").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDSERVICECOMPANY").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDSERVICECOMPANY").ToString() %>'></telerik:RadLabel>
                                            <eluc:Tooltip ID="ucServiceCompTT" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERVICECOMPANY") %>' />
                                            <telerik:RadTextBox runat="server" ID="txtServiceCompEdit" CssClass="input" Resize="Vertical" Width="99%" 
                                                TextMode="MultiLine" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERVICECOMPANY") %>'>
                                            </telerik:RadTextBox>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Remarks">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblRemarks" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'></telerik:RadLabel>
                                            <eluc:Tooltip ID="ucRemarksTT" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
                                            <telerik:RadTextBox runat="server" ID="txtRemarksEdit" CssClass="input"  Width="99%" Resize="Vertical"
                                                TextMode="MultiLine" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'>
                                            </telerik:RadTextBox>
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
                <tr>
                    <td>
                        <telerik:RadGrid ID="gvServices1" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                            OnItemDataBound="gvServices1_ItemDataBound" Width="100%" CellPadding="2" ShowFooter="false" OnNeedDataSource="gvServices1_NeedDataSource"
                            ShowHeader="true" EnableViewState="false" AllowSorting="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDDEPARTURESERVICEID" CommandItemDisplay="Top">
                                <HeaderStyle Width="102px" />
                                <NoRecordsTemplate>
                                    <table width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />

                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Service Type">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblServiceType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERVICETYPE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Qty" Visible="false">
                                        <ItemStyle Wrap="false" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblQty" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblDepServiceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTURESERVICEID") %>'></telerik:RadLabel>
                                            <eluc:Number ID="txtQtyEdit" runat="server" CssClass="input txtNumber" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>'
                                                IsPositive="true" IsInteger="true" MaxLength="9" />
                                            <telerik:RadLabel ID="lblServiceIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERVICETYPEID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblDepServiceIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTURESERVICEID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblVesselDepartureID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELDEPARTUREID") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Remarks">
                                        <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadTextBox runat="server" ID="txtRemarksEdit" CssClass="input" Width="99%" Resize="Vertical" TextMode="MultiLine"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'>
                                            </telerik:RadTextBox>
                                            <telerik:RadLabel ID="lblRemarks" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'></telerik:RadLabel>
                                            <eluc:Tooltip ID="ucRemarksTT" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />

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
            <table>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblmFreshWater" runat="server" Text="Fresh Water"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid ID="gvFW" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" ShowFooter="false" EnableHeaderContextMenu="true" GroupingEnabled="false"
                ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvFW_NeedDataSource" AllowSorting="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDOILCONSUMPTIONID" CommandItemDisplay="Top">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Fresh Water">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOilType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Previous ROB">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblROBOnArrival" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUSROB") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOilConsumptionIdItem" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblNoonOilConsumptionIdItem" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOONOILCONSUMPTIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblROBOnArrivalEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUSROB") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOilTypeCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOilConsumptionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblNoonOilConsumptionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOONOILCONSUMPTIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselDepartureID" runat="server" Width="120px" Visible="false"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELDEPARTUREID") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Received">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReceived" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUNKEREDQTY") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtReceivedEdit" runat="server" CssClass="input txtNumber" DecimalPlace="2"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUNKEREDQTY") %>' IsPositive="true"
                                    MaxLength="9" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ROB on Dep">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblROBOnDepartureItem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBONDEP") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtROBOnDepartureEdit" runat="server" CssClass="input txtNumber"
                                    DecimalPlace="2" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBONDEP") %>'
                                    IsPositive="true" MaxLength="9" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Consumption">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConsumptionInPortItem" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSUMEDINPORT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblConsumptionInPortEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSUMEDINPORT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMEReCounter" runat="server" Text="ME Rev Counter @COSP"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtMERevCounter" runat="server" CssClass="input" Width="80px"
                            DecimalPlace="2" IsPositive="true" MaxLength="15" />
                        &nbsp;
                                <asp:CheckBox runat="server" ID="chkRevCounterDefective" Text=" counter defective / Reset" />
                    </td>
                </tr>
            </table>
            <br />
<%--            <telerik:RadDockZone ID="RadDockZone8" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="100%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock8" runat="server" Title="<b>Consumption</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>--%>
                        <h2> <telerik:RadLabel ID="RadLabel1" runat="server" Text="Consumption"></telerik:RadLabel> </h2>
                        <table cellpadding="1" cellspacing="1" width="100%">
                            <tr style="color: Blue">
                                <td colspan="4">
                                    <telerik:RadLabel ID="lblmFuelOilConsumption" runat="server" Text="* Fuel Oil Consumption in mT"></telerik:RadLabel>
                                    <br />
                                    <telerik:RadLabel ID="lblmLubOilConsumption" runat="server" Text="* Lub Oil Consumption in Ltr"></telerik:RadLabel>
                                </td>
                            </tr>
                            </table>
                                    <telerik:RadGrid ID="gvConsumption" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="2" OnItemCommand="gvConsumption_RowCommand" OnItemDataBound="gvConsumption_ItemDataBound"
                                        AllowSorting="true" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvConsumption_NeedDataSource"
                                        EnableHeaderContextMenu="true" GroupingEnabled="false" OnPreRender="gvConsumption_PreRender" OnUpdateCommand="gvConsumption_RowUpdating">
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" DataKeyNames="FLDOILCONSUMPTIONID" CommandItemDisplay="Top">
                                            <HeaderStyle Width="102px" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                                            <ColumnGroups>
                                                <telerik:GridColumnGroup HeaderText="IN PORT" Name="IN PORT" HeaderStyle-HorizontalAlign="Center">
                                                </telerik:GridColumnGroup>
                                            </ColumnGroups>
                                            <ColumnGroups>
                                                <telerik:GridColumnGroup HeaderText="AT HARBOUR" Name="AT HORBOUR" HeaderStyle-HorizontalAlign="Center">
                                                </telerik:GridColumnGroup>
                                            </ColumnGroups>
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Item">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOilTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOILTYPE")%>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lbloilconsumptiononlaterdateyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOILCONSUMPTIONONLATERDATEYN")%>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblOilTypeName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOILTYPENAME")%>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDOILTYPENAME")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Previous ROB" HeaderStyle-Width="60px">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblROBOnArrival" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUSROB") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblOilConsumptionIdItem" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <telerik:RadLabel ID="lblROBOnArrivalEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUSROB") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblOilTypeCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPE") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblOilConsumptionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblVesselDepartureID" runat="server" Width="120px" Visible="false"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELDEPARTUREID") %>'>
                                                        </telerik:RadLabel>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="meinport" HeaderText="M/E" ColumnGroupName="IN PORT" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblInPortME" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTME")%>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <table cellpadding="0">
                                                            <tr>
                                                                <td>M/E</td>
                                                                <td>
                                                                    <eluc:Number ID="ucInPortMEEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="60px"
                                                                        DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTME")%>' />
                                                                </td>
                                                                <td>A/E</td>
                                                                <td>
                                                                    <eluc:Number ID="ucInPortAEEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="60px"
                                                                        DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTAE")%>' />
                                                                </td>
                                                                <td>BLR</td>
                                                                <td>
                                                                    <eluc:Number ID="ucInPortBLREdit" runat="server" CssClass="gridinput" MaxLength="6" Width="60px"
                                                                        DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTBLR")%>' />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>IGG</td>
                                                                <td>
                                                                    <eluc:Number ID="ucInPortIGGEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="60px"
                                                                        DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTIGG")%>' />
                                                                </td>
                                                                <td>C/ENG</td>
                                                                <td>
                                                                    <eluc:Number ID="ucInPortCARGOENGEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="60px"
                                                                        DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTCARGOENG")%>' />
                                                                </td>
                                                                <td>C/HTG</td>
                                                                <td>
                                                                    <eluc:Number ID="ucAtSeaCARGOHEATINdit" runat="server" CssClass="gridinput" MaxLength="5" Width="60px"
                                                                        DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTCTHG")%>' />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>TK CLNG</td>
                                                                <td>
                                                                    <eluc:Number ID="ucAtSeaTKCLNGEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="60px"
                                                                        DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTTKCLNG")%>' />
                                                                </td>
                                                                <td>OTH</td>
                                                                <td>
                                                                    <eluc:Number ID="ucInPortOTHEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="60px"
                                                                        DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTOTH")%>' />
                                                                </td>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="aeinport" HeaderText="A/E" ColumnGroupName="IN PORT" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblInPortAE" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTAE")%>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="blrinport" HeaderText="BLR" ColumnGroupName="IN PORT" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblInPortBLR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTBLR")%>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="igginport" HeaderText="IGG" ColumnGroupName="IN PORT" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblInPortIGG" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTIGG")%>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="cenginport" HeaderText="C/ENG" ColumnGroupName="IN PORT" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblInPortCARGOENG" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTCARGOENG")%>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="cthginport" HeaderText="C/HTG" ColumnGroupName="IN PORT" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAtSeaCARGOHEATIN" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTCTHG")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="tkclnginport" HeaderText="TK CLNG" ColumnGroupName="IN PORT" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAtSeaTKCLNG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTTKCLNG")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="othinport" HeaderText="OTH" ColumnGroupName="IN PORT" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblInPortOTH" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTOTH")%>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="bunker" HeaderText="Bunker Qty" HeaderStyle-Width="50px" HeaderStyle-Wrap="true" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblBunkeredItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUNKEREDQTY") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>Bunkered</td>
                                                                <td>
                                                                    <eluc:Number ID="txtBunkeredEdit" runat="server" CssClass="input txtNumber" DecimalPlace="2"
                                                                        Width="75px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUNKEREDQTY") %>'
                                                                        MaxLength="9" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>Sulphur %</td>
                                                                <td>
                                                                    <eluc:Number ID="txtSulphurPercentageEdit" runat="server" CssClass="input txtNumber" DecimalPlace="4"
                                                                        Width="75px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSULPHURPERCENTAGE") %>'
                                                                        IsPositive="true" MaxLength="18" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="sulphur" HeaderText="Sulphur <br /> %" HeaderStyle-Width="50px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblSulphurPercentage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSULPHURPERCENTAGE") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>


                                                <telerik:GridTemplateColumn UniqueName="robonsbe" HeaderText="ROB <br /> @ <br /> (SBE)" HeaderStyle-Width="60px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblROBOnDepartureItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBSBE") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn Visible="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblAtSeaMEHeader" Visible="true" Text=" M/E " runat="server">
                                                        </telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAtSeaME" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAME")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:Number ID="ucAtSeaMEEdit" runat="server" CssClass="gridinput" MaxLength="9"
                                                            Width="40px" DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAME")%>' />
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn Visible="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblAtSeaAEHeader" Visible="true" Text=" A/E " runat="server">
                                                        </telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAtSeaAE" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAAE")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:Number ID="ucAtSeaAEEdit" runat="server" CssClass="gridinput" MaxLength="9"
                                                            Width="40px" DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAAE")%>' />
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn Visible="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblAtSeaBLRHeader" Visible="true" Text=" BLR " runat="server">
                                                        </telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAtSeaBLR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEABLR")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:Number ID="ucAtSeaBLREdit" runat="server" CssClass="gridinput" MaxLength="9"
                                                            Width="40px" DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEABLR")%>' />
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn Visible="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblAtSeaIGGHeader" Visible="true" Text="IGG" runat="server">
                                                        </telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAtSeaIGG" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAIGG")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:Number ID="ucAtSeaIGGEdit" runat="server" CssClass="gridinput" MaxLength="9"
                                                            Width="40px" DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAIGG")%>' />
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn Visible="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblAtSeaCARGOENGHeader" Visible="true" Text="C/ENG" runat="server">
                                                        </telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAtSeaCARGOENG" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEACARGOENG")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:Number ID="ucAtSeaCARGOENGEdit" runat="server" CssClass="gridinput" MaxLength="9"
                                                            Width="40px" DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEACARGOENG")%>' />
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn Visible="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <HeaderTemplate>
                                                        <telerik:RadLabel ID="lblAtSeaOTHHeader" Visible="true" Text=" OTH " runat="server">
                                                        </telerik:RadLabel>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAtSeaOTH" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAOTH")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:Number ID="ucAtSeaOTHEdit" runat="server" CssClass="gridinput" MaxLength="9"
                                                            Width="40px" DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAOTH")%>' />
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="meatharbour" HeaderText="M/E" ColumnGroupName="AT HORBOUR" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAtHourbourME" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURME")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <table cellpadding="0">
                                                            <tr>
                                                                <td>M/E</td>
                                                                <td>
                                                                    <eluc:Number ID="ucAtHourbourMEEdit" runat="server" CssClass="gridinput" MaxLength="9" Width="60px"
                                                                        DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURME")%>' />
                                                                </td>
                                                                <td>A/E</td>
                                                                <td>
                                                                    <eluc:Number ID="ucAtHourbourAEEdit" runat="server" CssClass="gridinput" MaxLength="9" Width="60px"
                                                                        DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURAE")%>' />
                                                                </td>
                                                                <td>BLR</td>
                                                                <td>
                                                                    <eluc:Number ID="ucAtHourbourBLREdit" runat="server" CssClass="gridinput" MaxLength="9" Width="60px"
                                                                        DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURBLR")%>' />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>IGG</td>
                                                                <td>
                                                                    <eluc:Number ID="ucAtHourbourIGGEdit" runat="server" CssClass="gridinput" MaxLength="9" Width="60px"
                                                                        DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURIGG")%>' />
                                                                </td>
                                                                <td>C/ENG</td>
                                                                <td>
                                                                    <eluc:Number ID="ucAtHourbourCARGOENGEdit" runat="server" CssClass="gridinput" MaxLength="9" Width="60px"
                                                                        DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURCARGOENG")%>' />
                                                                </td>
                                                                <td>C/HTG</td>
                                                                <td>
                                                                    <eluc:Number ID="ucAtHourbourCARGOHEATINdit" runat="server" CssClass="gridinput" MaxLength="5" Width="60px"
                                                                        DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURCTHG")%>' />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>TK CLNG</td>
                                                                <td>
                                                                    <eluc:Number ID="ucAtHourbourTKCLNGEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="60px"
                                                                        DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURTKCLNG")%>' />
                                                                </td>
                                                                <td>OTH</td>
                                                                <td>
                                                                    <eluc:Number ID="ucAtHourbourOTHEdit" runat="server" CssClass="gridinput" MaxLength="9" Width="60px"
                                                                        DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOUROTH")%>' />
                                                                </td>
                                                                <td></td>
                                                                <td></td>
                                                            </tr>
                                                        </table>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="aeatharbour" HeaderText="A/E" ColumnGroupName="AT HORBOUR" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAtHourbourAE" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURAE")%>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="blratharbour" HeaderText="BLR" ColumnGroupName="AT HORBOUR" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAtHourbourBLR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURBLR")%>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="iggatharbour" HeaderText="IGG" ColumnGroupName="AT HORBOUR" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAtHourbourIGG" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURIGG")%>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="cngatharbour" HeaderText="C/ENG" ColumnGroupName="AT HORBOUR" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAtHourbourCARGOENG" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURCARGOENG")%>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="chtgatharbour" HeaderText="C/HTG" ColumnGroupName="AT HORBOUR" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAtHourbourCARGOHEATIN" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURCTHG")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="tkclngatharbour" HeaderText="TK CLNG" ColumnGroupName="AT HORBOUR" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAtHourbourTKCLNG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURTKCLNG")%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="othatharbour" HeaderText="OTH" ColumnGroupName="AT HORBOUR" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAtHourbourOTH" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOUROTH")%>'>
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="robondeaprture" HeaderText="ROB <br /> @ <br /> (COSP)" HeaderStyle-Width="60px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblROBOnDepCOSP" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBCOSP") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Bunkering" CommandName="BUNKER" ID="cmdBunkerAdd" ToolTip="BDN">
                                                            <span class="icon"><i class="fas fa-gas-pump"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="De-Bunker" CommandName="EDIT" ID="DeBunker" ToolTip="De-Bunker" CommandArgument="cmdDeBunker">
                                                            <span class="icon"><i class="fas fa-debunker"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave" ToolTip="Save">
                                                            <span class="icon"><i class="fas fa-save"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                                                <span class="icon"><i class="fas fa-times-circle"></i></span>
                                                        </asp:LinkButton>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" ScrollHeight="" UseStaticHeaders="true" SaveScrollPosition="true" EnableColumnClientFreeze="true" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                            <ClientEvents OnRowClick="rowClick" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
<%--                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>--%>
            <br />
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
