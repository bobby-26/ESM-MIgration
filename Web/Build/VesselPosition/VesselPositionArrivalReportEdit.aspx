<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionArrivalReportEdit.aspx.cs" Inherits="VesselPositionArrivalReportEdit" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnVoyagePort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Voyage" Src="~/UserControls/UserControlVoyage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Direction" Src="~/UserControls/UserControlDircection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="../UserControls/UserControlDecimal.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Arrival Report..</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
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
    <form id="form1" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="panel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="panel1" Height="98%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuARSubTab" runat="server" TabStrip="true" OnTabStripCommand="MenuARSubTab_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="ArrivalSave" runat="server" OnTabStripCommand="ArrivalSave_TabStripCommand"></eluc:TabStrip>
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
                    <td style="width: 31%;">
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true"
                            AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucVessel_Changed" Width="180px" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmVoyage" runat="server" Text="Voyage"></telerik:RadLabel>
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
                                    <eluc:Multiport runat="server" ID="ddlVoyagePort" Width="180px" CssClass="dropdown_mandatory" />
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
                        <telerik:RadLabel ID="lblmNameofBerthAnchorage" runat="server" Text="Name of Berth / Anchorage"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtBerthName" CssClass="input" Width="180px"></telerik:RadTextBox>
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
                            <telerik:RadLabel ID="ltrlLT" runat="server" Text="<b>LT</b>"></telerik:RadLabel>
                        <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="200" height="0" />
                            <telerik:RadLabel ID="ltrlUTClable" runat="server" Text="<b>UTC</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmArrivalDate" runat="server" Text="Arrival Date"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date ID="txtArrivalDate" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtArrivalTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
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
                        <telerik:RadLabel ID="lblmEOSP" runat="server" Text="End of Sea Passage (EOSP)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date ID="txtESOP" runat="server" CssClass="input_mandatory" OnTextChangedEvent="OnTextChange" AutoPostBack="true" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtESOPTime" runat="server" Width="80px" CssClass="input_mandatory"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCESOP" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCESOPTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmPOB" runat="server" Text="Pilot on Board (POB)"></telerik:RadLabel>
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
                        <telerik:RadLabel ID="lblmFLA" runat="server" Text="First Line Ashore (FLA)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtFLA" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtFLATime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCFLA" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCFLATime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFSW" runat="server" Text="First Shackle in water (FSW)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtFSW" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtFSWTime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCFSW" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCFSWTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmLGA" runat="server" Text="Let Go Anchor (LGA)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtLGA" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtLGATime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCLGA" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCLGATime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmFWE" runat="server" Text="Finished with Engine (FWE)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtFWE" CssClass="input_mandatory" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtFWETime" runat="server" Width="80px" CssClass="input_mandatory"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCFWE" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCFWETime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmDOP" runat="server" Text="Pilot Away (DOP)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtDOP" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtDOPTime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCDOP" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCDOPTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmNORT" runat="server" Text="NOR Tendered (NORT)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtNORT" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtNORTTime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCNORT" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCNORTTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmNORA" runat="server" Text="NOR Accepted (NORA)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtNORA" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtNORATime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCNORA" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCNORATime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmFPG" runat="server" Text="Free Pratique Granted (FPG)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtFPG" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtFPGTime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCFPG" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCFPGTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmETB" runat="server" Text="ETB"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date ID="txtETB" runat="server" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtETBHours" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCETB" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCETBHours" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmETD" runat="server" Text="ETD"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date ID="txtETD" runat="server" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtETDHours" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <eluc:Date ID="txtUTCETD" runat="server" CssClass="readonlytextbox" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtUTCETDHours" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmBallastLaden" runat="server" Text="Ballast/Laden"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadRadioButtonList ID="rbtnBallastLaden" runat="server" AutoPostBack="True" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Ballast" Value="0"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="Laden" Value="1"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStartNewVoyage" runat="server" Text="Start New Voyage"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <span id="spnNewVoyage">
                            <telerik:RadTextBox ID="txtNewVoyageName" runat="server" Width="180px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                            <asp:LinkButton runat="server" AlternateText=".." ID="imgShowNewVoyage" ToolTip="Voyage">
                             <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtNewVoyageId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmManoeuvering" runat="server" Text="Manoeuvering"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtManoevering" runat="server" Width="80px" CssClass="readonlytextbox" DecimalPlace="2"
                            IsPositive="true" MaxLength="9" ReadOnly="true" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmManoeveringhrs" runat="server" Text="hrs"></telerik:RadLabel>
                        <eluc:Number ID="txtManoeveringDist" runat="server" Width="80px" CssClass="input" DecimalPlace="2"
                            IsPositive="true" MaxLength="9" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmManoeveringnm" runat="server" Text="nm"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMErevcounterFWE" runat="server" Text="ME Rev Counter @FWE"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtMErevcounterFWE" runat="server" CssClass="input" Width="80px"
                            DecimalPlace="2" IsPositive="true" MaxLength="15" />

                        <telerik:RadLabel ID="lblEMLogcounteratFWE" runat="server" Text="EM Log Counter at FWE" Visible="false"></telerik:RadLabel>
                        <eluc:Number ID="txtEMLogcounteratFWE" runat="server" CssClass="input" Width="80px" Visible="false"
                            DecimalPlace="2" IsPositive="true" MaxLength="15" />

                        <telerik:RadLabel ID="lblEMLogManoeuveringDistance" runat="server" Text="EM Log Manoeuvering Distance" Visible="false"></telerik:RadLabel>
                        <eluc:Number ID="txtEMLogManoeuveringDistance" runat="server" CssClass="readonlytextbox" Enabled="false" Width="80px" Visible="false"
                            DecimalPlace="2" IsPositive="true" MaxLength="9" />
                        <telerik:RadLabel ID="Literal3" runat="server" Text="nm" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                        <br />
                        <u><b>
                            <telerik:RadLabel ID="lblmFromNoontoEOSP" runat="server" Text="From Noon to EOSP"></telerik:RadLabel>
                        </b></u>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmFullSpeed" runat="server" Text="Full Speed"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtFullSpeed" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmFullSpeedhrs" runat="server" Text="hrs"></telerik:RadLabel>
                        &nbsp;
                                <eluc:Number ID="txtFSDistance" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmFullSpeednm" runat="server" Text="nm"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmReducedSpeed" runat="server" Text="Reduced Speed"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtReducedSpeed" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmReducedSpeedhrs" runat="server" Text="hrs"></telerik:RadLabel>
                        &nbsp;
                                <eluc:Number ID="txtRSDistance" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmReducedSpeednm" runat="server" Text="nm"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmStopped" runat="server" Text="Stopped"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtStopped" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmStoppedhrs" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmDistanceObserved" runat="server" Text="Distance Observed"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtDistanceObserved" runat="server" CssClass="readonlytextbox" MaxLength="9"
                            Width="80px" Enabled="false" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmDistanceObservednm" runat="server" Text="nm"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmLogSpeed" runat="server" Text="Obs Speed"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtLogSpeed" runat="server" CssClass="readonlytextbox" MaxLength="9" Enabled="false" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmLogSpeedkts" runat="server" Text="kts"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNoonSpeed" runat="server" Text="EM Log Speed"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtNoonSpeed" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblNoonSpeedkts" runat="server" Text="kts"></telerik:RadLabel>

                        <telerik:RadLabel ID="lblEMLogCounteratEOSP" runat="server" Visible="false" Text="EM Log Counter at EOSP"></telerik:RadLabel>

                        <eluc:Number ID="txtEMLogCounteratEOSP" runat="server" CssClass="input" Width="80px" Visible="false"
                            DecimalPlace="2" IsPositive="true" MaxLength="15" />
                        <telerik:RadCheckBox runat="server" ID="chkCounterDefective" AutoPostBack="True" Visible="false"
                            OnCheckedChanged="chkCounterDefective_CheckedChanged" Text=" counter defective / Reset" />

                        <telerik:RadLabel ID="lblEMLogDistance" runat="server" Text="EM Log Distance" Visible="false"></telerik:RadLabel>
                        <eluc:Number ID="txtEMLogDistance" runat="server" CssClass="readonlytextbox" Enabled="false" Width="80px" Visible="false"
                            DecimalPlace="2" IsPositive="true" MaxLength="9" />
                        <telerik:RadLabel ID="Literal4" runat="server" Text="nm" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmVoyageOrderSpeed" runat="server" Text="Voyage Order Speed"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number runat="server" ID="txtVOSpeed" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmVoyageOrderSpeedkts" runat="server" Text="kts"></telerik:RadLabel>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmVoyageOrderCons" runat="server" Text="Voyage Order Cons"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number runat="server" ID="txtVOCons" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmVoyageOrderConsmt" runat="server" Text="mt"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmCourse" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtCourse" MaxLength="9" Width="80px" IsInteger="true" IsPositive="true" runat="server" CssClass="input" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmCourseT" runat="server" Text="T"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmDraftF" runat="server" Text="Draft F"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtDraftF" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmDraftFm" runat="server" Text="m"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmDraftA" runat="server" Text="Draft A"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtDraftA" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmDraftAm" runat="server" Text="m"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmWindDirection" runat="server" Text="Wind Direction"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Direction ID="ucWindDirection" runat="server" AppendDataBoundItems="true" CssClass="input" Width="120px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmWindForce" runat="server" Text="Wind Force"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtWindForce" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmSeaHeight" runat="server" Text="Sea Height"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtSeaHeight" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmSeaHeightm" runat="server" Text="m"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmSeaDirection" runat="server" Text="Sea Direction"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Direction ID="ucSeaDirection" runat="server" AppendDataBoundItems="true" CssClass="input" Width="120px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmSwell" runat="server" Text="Swell"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtSwell" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmSwellm" runat="server" Text="m"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmSetandDriftofCurrent" runat="server" Text="Set and Drift of Current"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtCurrentSpeed" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmSetandDriftofCurrentkts" runat="server" Text="kts"></telerik:RadLabel>
                        &nbsp;
                                <eluc:Direction ID="ucCurrentDirection" runat="server" AppendDataBoundItems="true" CssClass="input" Width="120px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAirTemp" runat="server" Text="Air Temp"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtAirTemp" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp; &deg;
                            <telerik:RadLabel ID="lblAirTempC" runat="server" Text="C"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmIcingonDeck" runat="server" Text="Icing on Deck?"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox runat="server" ID="chkIceDeck" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmDWTDisplacement" runat="server" Text="DWT/Displacement"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtDWT" runat="server" IsInteger="true" IsPositive="true" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmDWTDisplacementmt" runat="server" Text="mt"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmEntryintoECA" runat="server" Text="Entry into ECA"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox runat="server" ID="ChkECAyn" OnCheckedChanged="ChkECAyn_OnCheckedChanged" AutoPostBack="true" />
                        &nbsp;
                                <eluc:Date ID="txtECAEntryDate" runat="server" CssClass="input" DatePicker="true" Enabled="false" />
                        &nbsp;
                            <telerik:RadTimePicker ID="txtTimeofECAEntry" runat="server" Width="80px" CssClass="input" Enabled="false"
                                DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                            </telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblECAlatitude" runat="server" Text="Latitude"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Latitude ID="ucECAlatitude" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblECALongitude" runat="server" Text="Longitude"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Longitude ID="ucECALongitude" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmFuelusedinECA" runat="server" Text="Fuel used in ECA"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadComboBox ID="ddlECAOilType" runat="server" CssClass="input" DataValueField="FLDOILTYPECODE" DataTextField="FLDOILTYPENAME" Enabled="false">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFuelCOTime" runat="server" Text="Fuel C/O Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucFuelCOTime" runat="server" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtFuelCOTime" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <telerik:RadLabel ID="ltrltxtFuelCOTime" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" TextMode="MultiLine" Resize="Both" Rows="5" Width="300px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmEngineDistance" runat="server" Text="Engine Distance"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtEngineDistance" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmEngineDistancenm" runat="server" Text="nm"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmSlip" runat="server" Text="Slip"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtAvgSlip" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                            IsPositive="false" MaxLength="9" Width="80px" Enabled="false" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmSlipPercentage" runat="server" Text="%"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmERTemp" runat="server" Text="ER Temp"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtERExhTemp" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp; &deg;
                            <telerik:RadLabel ID="lblmERTempC" runat="server" Text="C"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmSWTemp" runat="server" Text="SW Temp"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtSwellTemp" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp; &deg;
                            <telerik:RadLabel ID="lblmSWTempC" runat="server" Text="C"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <h3><u>
                            <telerik:RadLabel ID="lblmMainEngine" runat="server" Text="Main Engine"></telerik:RadLabel>
                        </u></h3>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMEReCounter" runat="server" Text="ME Rev Counter @EOSP"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtMERevCounter" runat="server" CssClass="input" Width="80px"
                            DecimalPlace="2" IsPositive="true" MaxLength="15" />
                        &nbsp;
                                <telerik:RadCheckBox runat="server" ID="chkRevCounterDefective" AutoPostBack="True" OnCheckedChanged="chkRevCounterDefective_OnCheckedChanged" Text=" counter defective / Reset" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmRPM" runat="server" Text="Average RPM"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number runat="server" ID="txtMERPM" Enabled="false" CssClass="readonlytextbox" MaxLength="9" IsPositive="true" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmGovernorSetting" runat="server" Text="Governor Setting / Fuel rack"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number runat="server" ID="txtGovernorSetting" CssClass="input" MaxLength="9" DecimalPlace="2"
                            IsPositive="true" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmSpeedSetting" runat="server" Text="Speed Setting"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number runat="server" ID="txtSpeedSetting" CssClass="input" MaxLength="9" IsInteger="true"
                            IsPositive="true" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmFOInletTemp" runat="server" Text="FO Inlet Temp"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number runat="server" ID="txtFOInletTemp" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />
                        &nbsp; &deg;
                            <telerik:RadLabel ID="lblmFOInletTempC" runat="server" Text="C"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmFuelOilPress" runat="server" Text="Fuel Oil Press"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number runat="server" ID="txtFuelOilPress" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmFuelOilPressbar" runat="server" Text="bar"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <h3><u>
                            <telerik:RadLabel ID="lblmAuxEngine" runat="server" Text="Aux Engine"></telerik:RadLabel>
                        </u></h3>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOperatinghrs" runat="server" Text="Hours Of Operation"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmGeneralLoadAE1" runat="server" Text="A/E No 1. Generator Load"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtGeneralLoadAE1" runat="server" MaxLength="9" DecimalPlace="2" CssClass="input" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmGeneralLoadAE1kw" runat="server" Text="kw"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtGeneratorLoadAE1OPHrs" runat="server" CssClass="input"
                            Width="80px" MaxLength="9" />
                        <telerik:RadLabel ID="lblGeneratorLoadAE1hrs" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmGeneralLoadAE2" runat="server" Text="A/E No 2. Generator Load"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtGeneralLoadAE2" runat="server" MaxLength="9" DecimalPlace="2" CssClass="input" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmGeneralLoadAE2kw" runat="server" Text="kw"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtGeneratorLoadAE2OPHrs" runat="server" CssClass="input"
                            Width="80px" MaxLength="9" />
                        <telerik:RadLabel ID="lblGeneratorLoadAE2hrs" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmGeneralLoadAE3" runat="server" Text="A/E No 3. Generator Load"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtGeneralLoadAE3" runat="server" MaxLength="9" DecimalPlace="2" CssClass="input" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmGeneralLoadAE3kw" runat="server" Text="kw"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtGeneratorLoadAE3OPHrs" runat="server" CssClass="input"
                            Width="80px" MaxLength="9" />
                        <telerik:RadLabel ID="lblGeneratorLoadAE3hrs" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAENo4GeneratorLoad" runat="server" Text="A/E No 4. Generator Load"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtGeneratorLoadAE4" runat="server" MaxLength="9" DecimalPlace="2" CssClass="input" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblAENo4GeneratorLoadkw" runat="server" Text="kw"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtGeneratorLoadAE4OPHrs" runat="server" CssClass="input"
                            Width="80px" MaxLength="9" />
                        <telerik:RadLabel ID="lblGeneratorLoadAE4hrs" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <br />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td colspan="4">
                        <h3><u>
                            <telerik:RadLabel ID="lblFreshWater" runat="server" Text="Fresh Water"></telerik:RadLabel>
                        </u></h3>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div id="divGrid" style="position: relative; z-index: 0">
                            <telerik:RadGrid ID="gvOtherOilCons" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" OnNeedDataSource="gvOtherOilCons_NeedDataSource"
                                ShowFooter="false" ShowHeader="true"
                                EnableViewState="false">
                                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" DataKeyNames="FLDOILTYPECODE">
                                    <HeaderStyle Width="102px" />
                                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Fresh Water">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblOilTypeCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblOilType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Previous ROB">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblPreviousROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUSROB") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Produced">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblProduced" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCED") %>'></telerik:RadLabel>
                                                <eluc:Number ID="txtProducedEdit" runat="server" CssClass="input txtNumber" DecimalPlace="2"
                                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCED") %>' IsPositive="true"
                                                    MaxLength="9" />
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="ROB">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblROB" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB") %>'></telerik:RadLabel>
                                                <eluc:Number ID="txtROBEdit" runat="server" CssClass="input txtNumber" DecimalPlace="2"
                                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB") %>' IsPositive="true"
                                                    MaxLength="9" />
                                            </ItemTemplate>

                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Consumption">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblConsumption" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSUMPTIONQTY") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblOilConsumptionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></telerik:RadLabel>

                                                <telerik:RadLabel ID="lblConsumptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSUMPTIONQTY") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblOilConsumptionIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblOilTypeCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></telerik:RadLabel>

                                            </ItemTemplate>
                                            <EditItemTemplate>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </div>

                    </td>
                </tr>
            </table>
            <br />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="width: 15%;">
                        <telerik:RadLabel ID="lblmBoilerwaterChlorides" runat="server" Text="Boiler Water Chlorides"></telerik:RadLabel>
                    </td>
                    <td style="width: 25%;">
                        <eluc:Number runat="server" ID="txtBoilerWaterChlorides" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />
                        &nbsp;
                            <telerik:RadLabel ID="lblmBoilerwaterChloridesppm" runat="server" Text="ppm"></telerik:RadLabel>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmBilgeTankROB" runat="server" Text="Bilge Tank ROB"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number runat="server" ID="txtBilgeROB" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmSludgeTankROB" runat="server" Text="Sludge Tank ROB"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number runat="server" ID="txtSludgeROB" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmBilgeLanding" runat="server" Text="Last Landing of Bilge"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtBilgeLanding" Enabled="false" CssClass="readonlytextbox" />
                        <telerik:RadTextBox ID="txtBilgeLandingTime" runat="server" Enabled="false" CssClass="readonlytextbox" Width="50px" Visible="false" />
                        &nbsp;    
                                <telerik:RadLabel ID="lblmDays" runat="server" Text="Days"></telerik:RadLabel>
                        &nbsp;
                                <eluc:Number runat="server" ID="txtBilgeLandingDays" CssClass="readonlytextbox" Enabled="false" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmLastLandSludge" runat="server" Text="Last Landing of Sludge"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date runat="server" ID="txtLastLandSludge" Enabled="false" CssClass="readonlytextbox" />
                        <telerik:RadTextBox ID="txtLastLandSludgeTime" runat="server" Enabled="false" CssClass="readonlytextbox" Width="50px" Visible="false" />
                        &nbsp;
                                <telerik:RadLabel ID="lblmLastLandSludgeDays" runat="server" Text="Days"></telerik:RadLabel>
                        &nbsp;
                                <eluc:Number runat="server" ID="txtLastLandingDays" CssClass="readonlytextbox" Enabled="false" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblmChiefEngineerRemarks" runat="server" Text="Chief Engineer Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtRemarksCE" runat="server" CssClass="input" Height="50px" TextMode="MultiLine" Resize="Both"
                            Width="50%" />
                    </td>
                </tr>
            </table>
<%--            <telerik:RadDockZone ID="RadDockZone8" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="100%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock8" runat="server" Title="<b>Consumption</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>--%>
                        <h2> <telerik:RadLabel ID="RadLabel1" runat="server" Text="Consumption"></telerik:RadLabel> </h2>
                        <table width="100%" cellpadding="1" cellspacing="1">
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
                            EnableHeaderContextMenu="true" GroupingEnabled="false" OnPreRender="gvConsumption_PreRender">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDOILCONSUMPTIONID" CommandItemDisplay="Top">
                                <HeaderStyle Width="102px" />
                                <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="AT SEA" Name="AT SEA" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="AT HARBOUR" Name="AT HARBOUR" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Oil Type">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblOilTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOILTYPE")%>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lbloilconsumptiononlaterdateyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOILCONSUMPTIONONLATERDATEYN")%>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblOilTypeName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOILTYPENAME")%>' ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDOILTYPENAME")%>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblOilConsumptionIdItem" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Previous <br /> ROB" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPreviousRob" runat="server" MaxLength="6" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPREVIOUSROB")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="meatsea" HeaderText="M/E" ColumnGroupName="AT SEA" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtSeaME" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAME")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <table cellpadding="0">
                                                <tr>
                                                    <td>M/E</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtSeaMEEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="58px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAME")%>' />
                                                    </td>
                                                    <td>A/E</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtSeaAEEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="58px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAAE")%>' />
                                                    </td>
                                                    <td>BLR</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtSeaBLREdit" runat="server" CssClass="gridinput" MaxLength="6" Width="58px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEABLR")%>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>IGG</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtSeaIGGEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="58px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAIGG")%>' />
                                                    </td>
                                                    <td>C/ENG</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtSeaCARGOENGEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="58px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEACARGOENG")%>' />
                                                    </td>
                                                    <td>C/HTG</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtSeaCARGOHEATINdit" runat="server" CssClass="gridinput" MaxLength="6" Width="58px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEACTHG")%>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>TK CLNG</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtSeaTKCLNGEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="58px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEATKCLNG")%>' />
                                                    </td>
                                                    <td>OTH</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtSeaOTHEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="58px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAOTH")%>' />
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                            </table>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="aeatsea" HeaderText="A/E" ColumnGroupName="AT SEA" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtSeaAE" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAAE")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="blratsea" HeaderText="BLR" ColumnGroupName="AT SEA" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtSeaBLR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEABLR")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="iggatsea" HeaderText="IGG" ColumnGroupName="AT SEA" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtSeaIGG" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAIGG")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="cengatsea" HeaderText="C/ENG" ColumnGroupName="AT SEA" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtSeaCARGOENG" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEACARGOENG")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="chtgatsea" HeaderText="C/HTG" ColumnGroupName="AT SEA" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtSeaCARGOHEATIN" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEACTHG")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="tkclngatsea" HeaderText="TK CLNG" ColumnGroupName="AT SEA" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtSeaTKCLNG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEATKCLNG")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="othatsea" HeaderText="OTH" ColumnGroupName="AT SEA" HeaderStyle-Width="40px" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtSeaOTH" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAOTH")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="ROB @ <br /> EOSP" HeaderStyle-Width="50px" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblRobEsop" runat="server" MaxLength="6" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROBEOSP")%>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="meatharbour" HeaderStyle-Width="40px" HeaderStyle-Wrap="false" HeaderText="M/E" ColumnGroupName="AT HARBOUR">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourME" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURME")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <table cellpadding="0">
                                                <tr>
                                                    <td>M/E</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtHourbourMEEdit" runat="server" CssClass="gridinput" MaxLength="9" Width="58px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURME")%>' />
                                                    </td>
                                                    <td>A/E</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtHourbourAEEdit" runat="server" CssClass="gridinput" MaxLength="9" Width="58px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURAE")%>' />
                                                    </td>
                                                    <td>BLR</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtHourbourBLREdit" runat="server" CssClass="gridinput" MaxLength="9" Width="58px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURBLR")%>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>IGG</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtHourbourIGGEdit" runat="server" CssClass="gridinput" MaxLength="9" Width="58px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURIGG")%>' />
                                                    </td>
                                                    <td>C/ENG</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtHourbourCARGOENGEdit" runat="server" CssClass="gridinput" MaxLength="9" Width="58px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURCARGOENG")%>' />
                                                    </td>
                                                    <td>C/HTG</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtHourbourCARGOHEATINdit" runat="server" CssClass="gridinput" MaxLength="5" Width="58px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURCTHG")%>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>TK CLNG</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtHourbourTKCLNGEdit" runat="server" CssClass="gridinput" MaxLength="5" Width="58px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURTKCLNG")%>' />
                                                    </td>
                                                    <td>OTH</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtHourbourOTHEdit" runat="server" CssClass="gridinput" MaxLength="9" Width="58px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOUROTH")%>' />
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                            </table>

                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="aeatharbour" HeaderStyle-Width="40px" HeaderStyle-Wrap="false" HeaderText="A/E" ColumnGroupName="AT HARBOUR">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourAE" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURAE")%>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="blratharbour" HeaderStyle-Width="40px" HeaderStyle-Wrap="false" HeaderText="BLR" ColumnGroupName="AT HARBOUR">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourBLR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURBLR")%>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="iggatharbour" HeaderStyle-Width="40px" HeaderStyle-Wrap="false" HeaderText="IGG" ColumnGroupName="AT HARBOUR">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourIGG" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURIGG")%>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="cengatharbour" HeaderStyle-Width="40px" HeaderStyle-Wrap="false" HeaderText="C/ENG" ColumnGroupName="AT HARBOUR">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourCARGOENG" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURCARGOENG")%>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="chtgatharbour" HeaderStyle-Width="40px" HeaderStyle-Wrap="false" HeaderText="C/HTG" ColumnGroupName="AT HARBOUR">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourCARGOHEATIN" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURCTHG")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="tkclngatharbour" HeaderStyle-Width="40px" HeaderStyle-Wrap="false" HeaderText="TK CLNG" ColumnGroupName="AT HARBOUR">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourTKCLNG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURTKCLNG")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="othatharbour" HeaderStyle-Width="40px" HeaderStyle-Wrap="false" HeaderText="OTH" ColumnGroupName="AT HARBOUR">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourOTH" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOUROTH")%>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblInPortMEHeader" Visible="true" Text=" M/E " runat="server">
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInPortME" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTME")%>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="ucInPortMEEdit" runat="server" CssClass="gridinput" MaxLength="9" Width="30px"
                                                DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTME")%>' />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblInPortAEHeader" Visible="true" Text=" A/E " runat="server">
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInPortAE" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTAE")%>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="ucInPortAEEdit" runat="server" CssClass="gridinput" MaxLength="9" Width="30px"
                                                DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTAE")%>' />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblInPortBLRHeader" Visible="true" Text=" BLR " runat="server">
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInPortBLR" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTBLR")%>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="ucInPortBLREdit" runat="server" CssClass="gridinput" MaxLength="9" Width="30px"
                                                DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTBLR")%>' />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblInPortIGGHeader" Text="IGG" runat="server"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInPortIGG" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTIGG")%>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="ucInPortIGGEdit" runat="server" CssClass="gridinput" MaxLength="9" Width="30px"
                                                DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTIGG")%>' />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblInPortCARGOENGHeader" Text="C/ENG" runat="server"></telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInPortCARGOENG" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTCARGOENG")%>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="ucInPortCARGOENGEdit" runat="server" CssClass="gridinput" MaxLength="9" Width="30px"
                                                DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTCARGOENG")%>' />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblInPortOTHHeader" Visible="true" Text=" OTH " runat="server">
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInPortOTH" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTOTH")%>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="ucInPortOTHEdit" runat="server" CssClass="gridinput" MaxLength="9" Width="30px"
                                                DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTOTH")%>' />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="bunker" HeaderText="Bunker Qty" HeaderStyle-Width="45px" HeaderStyle-Wrap="True" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblBunkeredItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUNKEREDQTY") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <table cellpadding="0">
                                                <tr>
                                                    <td>Bunkered</td>
                                                    <td>
                                                        <eluc:Number ID="txtBunkeredEdit" runat="server" CssClass="input txtNumber" DecimalPlace="2"
                                                            Width="65px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUNKEREDQTY") %>'
                                                            MaxLength="9" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>Sulphur %</td>
                                                    <td>
                                                        <eluc:Number ID="txtSulphurPercentageEdit" runat="server" CssClass="input txtNumber" DecimalPlace="4"
                                                            Width="65px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSULPHURPERCENTAGE") %>'
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
                                    <telerik:GridTemplateColumn UniqueName="total" HeaderText="Total" HeaderStyle-Width="45px" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTotal" runat="server" MaxLength="6" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTOTALCONSUMPTION")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="ROB @ <br /> FWE" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" >
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblRobFwe" runat="server" MaxLength="6" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROBFWE")%>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="82px" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Bunkering" CommandName="BUNKER" ID="cmdBunkerAdd" ToolTip="BDN">
                                                            <span class="icon"><i class="fas fa-gas-pump"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit" CommandArgument="cmdEdit">
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
                                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="CALCULATE" ID="cmdSave" ToolTip="Save">
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
                                <Scrolling AllowScroll="true" ScrollHeight="" UseStaticHeaders="true" SaveScrollPosition="true" EnableColumnClientFreeze="true"  />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                <ClientEvents OnRowClick="rowClick" />
                            </ClientSettings>
                        </telerik:RadGrid>
            <br />
 <%--                   </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>--%>
            <table width="100%" cellpadding="1" cellspacing="1" style="display: none;">
                <tr>
                    <td style="width: 15%; visibility: hidden;">
                        <telerik:RadLabel ID="lblmTankCleaning" runat="server" Text="HFO Cons for Tank Cleaning (if any)"></telerik:RadLabel>
                    </td>
                    <td style="width: 25%; visibility: hidden;">
                        <eluc:Number runat="server" ID="txtHFOTankCleaning" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 15%; visibility: hidden;">
                        <telerik:RadLabel ID="lblmCargoHeating" runat="server" Text="HFO Cons for Cargo Heating (if any)"></telerik:RadLabel>
                    </td>
                    <td style="width: 25%; visibility: hidden;">
                        <eluc:Number runat="server" ID="txtHFOCargoHeating" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
