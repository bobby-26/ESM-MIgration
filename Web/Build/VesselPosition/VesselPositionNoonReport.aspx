<%@ Page Language="C#" AutoEventWireup="true" maintainScrollPositionOnPostBack = "true" CodeFile="VesselPositionNoonReport.aspx.cs" Inherits="VesselPositionNoonReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Cargo" Src="~/UserControls/UserControlCargo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Direction" Src="~/UserControls/UserControlDircection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Voyage" Src="~/UserControls/UserControlVoyage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="../UserControls/UserControlDecimal.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Noon Report</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }
    </style>
</head>
<body>
    <form id="frmNoonReport" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlNoonReportData" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="tblnoonreport">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="tblnoonreport" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="cmdPickListEntry">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="cmdPickListEntry" />
                        <telerik:AjaxUpdatedControl ControlID="cmdHiddenSubmit" />
                        <telerik:AjaxUpdatedControl ControlID="cmdHiddenPick" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="cmdPickListExit">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="cmdPickListExit" />
                        <telerik:AjaxUpdatedControl ControlID="cmdHiddenSubmit" />
                        <telerik:AjaxUpdatedControl ControlID="cmdHiddenPick" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <eluc:TabStrip ID="MenuNRSubTab" runat="server" TabStrip="true" OnTabStripCommand="MenuNRSubTab_TabStripCommand"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand"></eluc:TabStrip>

        <telerik:RadAjaxPanel runat="server" ID="pnlNoonReportData" Height="85%" CssClass="scrolpan">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenPick_Click" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            

            <table cellpadding="2" cellspacing="1" width="100%" id="tblnoonreport">
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblAlertSenttoOFC" runat="server" Text="Report Sent to Office" Visible="false" Font-Bold="False" Font-Size="Large"
                            Font-Underline="True" ForeColor="Red" BorderColor="Red">
                        </telerik:RadLabel>
                        <br />
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
                                <telerik:RadComboBoxItem Text="+" Value="+"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="-" Value="-"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                        <eluc:Decimal runat="server" ID="txtShipMeanTime" CssClass="input_mandatory" DecimalDigits="1" Mask="99.9" Width="50px"></eluc:Decimal>
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
                    <td width="15%">
                        <telerik:RadLabel ID="lblReportDate" runat="server" Text="Report Date"></telerik:RadLabel>
                    </td>
                    <td width="75%">
                        <eluc:Date ID="txtCurrentDate" runat="server" CssClass="readonlytextbox" Enabled="false" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtReportTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
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
                        <telerik:RadLabel ID="lblIDLCrossing" runat="server" Text="IDL Crossing"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkIDLCrossing" runat="server" OnCheckedChanged="chkIDLCrossing_OnCheckedChanged" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIDLDirection" runat="server" Text="IDL Direction"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlIDLDirection" runat="server" CssClass="input" Enabled="false"
                            OnTextChanged="ddlIDLDirection_OnTextChanged" AutoPostBack="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="Dummy" Text="--Select--"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="1" Text="East to West"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2" Text="West to East"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoyageNo" runat="server" Text="Voyage No"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListVoyage">
                            <telerik:RadTextBox ID="txtVoyageName" runat="server" Width="135px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                            <asp:LinkButton runat="server" AlternateText="Add" ID="btnShowVoyage" ToolTip="Voyage" Visible="false">
                             <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtVoyageId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselsStatus" runat="server" Text="Vessel's Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlVesselStatus" CssClass="dropdown_mandatory"
                            AppendDataBoundItems="true" AutoPostBack="true" OnTextChanged="VesselStatus_Changed">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="In Port" Value="INPORT"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="At Anchor" Value="ATANCHOR"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="At Sea" Value="ATSEA"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Drifting" Value="DRIFTING"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrentPort" runat="server" Text="Current Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadComboBox ID="ddlCurrentPort" runat="server" CssClass="input" DataTextField="FLDSEAPORTNAME" DataValueField="FLDPORTCALLID" Enabled="false">
                                    </telerik:RadComboBox>
                                </td>
                                <td>
                                    <b>
                                        <telerik:RadCheckBox runat="server" ID="chkOffPortLimits" Text="Off Port Limits" />
                                    </b>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBerthLocation" runat="server" Text="Berth / Location"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtBerthLocation" Width="135px" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLatitude" runat="server" Text="Latitude"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Latitude ID="ucLatitude" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLongitude" runat="server" Text="Longitude"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Longitude ID="ucLongitude" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNextPort" runat="server" Text="Next Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlNextPort" runat="server" CssClass="input" DataTextField="FLDSEAPORTNAME" DataValueField="FLDPORTCALLID" Enabled="false"
                            AutoPostBack="true" OnTextChanged="ddlNextPort_Changed">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblETA" runat="server" Text="ETA"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtETA" runat="server" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtTimeOfETA" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <telerik:RadLabel ID="txtETAhrs" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblETB" runat="server" Text="ETB"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtETB" runat="server" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtTimeOfETB" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <telerik:RadLabel ID="lblETBhrs" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblETCD" runat="server" Text="ETC/D"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtETC" runat="server" CssClass="input" DatePicker="true" />
                        <telerik:RadTimePicker ID="txtTimeOfETC" runat="server" Width="80px" CssClass="input"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <telerik:RadLabel ID="txtETChrs" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBallastLaden" runat="server" Text="Ballast/Laden"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rbtnBallastLaden" runat="server" AutoPostBack="True" Enabled="false" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Text="Ballast" Value="0"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="Laden" Value="1"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFullSpeed" runat="server" Text="Full Speed"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtFullSpeed" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblFullSpeedhrs" runat="server" Text="hrs"></telerik:RadLabel>
                        &nbsp;
                                <eluc:Number ID="txtFSDistance" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblFullSpeednm" runat="server" Text="nm"></telerik:RadLabel>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReducedSpeed" runat="server" Text="Reduced Speed"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtReducedSpeed" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblReducedSpeedhrs" runat="server" Text="hrs"></telerik:RadLabel>
                        &nbsp;
                                <eluc:Number ID="txtRSDistance" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblReducedSpeednm" runat="server" Text="nm"></telerik:RadLabel>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStopped" runat="server" Text="Stopped"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtStopped" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblStoppedhrs" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDistanceObserved" runat="server" Text="Distance Observed"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtDistanceObserved" runat="server" ReadOnly="true" CssClass="readonlytextbox" MaxLength="9" Width="80px" Enabled="false" />
                        &nbsp;
                        <telerik:RadLabel ID="lblDistanceObservednm" runat="server" Text="nm"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLogSpeed" runat="server" Text="Obs Speed (SOG)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtLogSpeed" runat="server" CssClass="input" MaxLength="9" Enabled="false" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblLogSpeedkts" runat="server" Text="kts"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNoonSpeed" runat="server" Text="EM Log Speed (LOG)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtNoonSpeed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblNoonSpeedkts" runat="server" Text="kts"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoyageAverageSpeed" runat="server" Text="Voyage Average Speed"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtVoyageAverageSpeed" runat="server" ReadOnly="true" CssClass="readonlytextbox" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblVoyageAverageSpeedkts" runat="server" Text="kts"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDistancetogo" runat="server" Text="Distance To Go"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtDistancetogo" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblDistancetogonm" runat="server" Text="nm"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoyageOrderSpeed" runat="server" Text="Voyage Order Speed"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtVOSpeed" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblVoyageOrderSpeedkts" runat="server" Text="kts"></telerik:RadLabel>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoyageOrderCons" runat="server" Text="Voyage Order ME FO Cons"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtVOCons" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblVoyageOrderConsmt" runat="server" Text="mt"></telerik:RadLabel>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoyageOrderDOCons" runat="server" Text="Voyage Order ME DO Cons"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number runat="server" ID="txtVoDOCons" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblVoDOCons" runat="server" Text="mt"></telerik:RadLabel>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtCourse" MaxLength="9" IsInteger="true" IsPositive="true" Width="80px" runat="server" CssClass="input" />
                        &nbsp;
                        <telerik:RadLabel ID="lblCourseT" runat="server" Text="T"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDraftF" runat="server" Text="Draft F"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtDraftF" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblDraftFm" runat="server" Text="m"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDraftA" runat="server" Text="Draft A"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtDraftA" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblDraftAm" runat="server" Text="m"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDWTDisplacement" runat="server" Text="Displacement"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtDWT" runat="server" IsInteger="true" IsPositive="true" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblDWTDisplacementmt" runat="server" Text="mt"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWindDirection" runat="server" Text="Wind Direction"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Direction ID="ucWindDirection" runat="server" AppendDataBoundItems="true" CssClass="input" Width="160px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWindForce" runat="server" Text="Wind Force"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtWindForce" runat="server" IsInteger="true" CssClass="input" MaxLength="9" Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSeaHeight" runat="server" Text="Sea Height"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtSeaHeight" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblSeaHeightm" runat="server" Text="m"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSeaDirection" runat="server" Text="Sea Direction"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Direction ID="ucSeaDirection" runat="server" AppendDataBoundItems="true" CssClass="input" Width="120px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSwell" runat="server" Text="Swell Height"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtSwell" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="txtSwellm" runat="server" Text="m"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrentSetandDrift" runat="server" Text="Current Set and Drift"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtCurrentSpeed" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblCurrentSetandDriftkts" runat="server" Text="kts"></telerik:RadLabel>
                        <eluc:Direction ID="ucCurrentDirection" runat="server" AppendDataBoundItems="true" CssClass="input" Width="120px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAirTemp" runat="server" Text="Air Temp"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtAirTemp" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                        &nbsp; &deg;
                        <telerik:RadLabel ID="lblAirTempC" runat="server" Text="C"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIcingonDeck" runat="server" Text="Icing on Deck?"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkIceDeck" AutoPostBack="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcallingUSPort" runat="server" Text="Is vessel calling US Port within next 7 days?"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="ChkUSWaters" AutoPostBack="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblvesselinUSWaterEntry" runat="server" Text="Entry into US Water"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                     <span id="spnPickListEntryDate">
                            <telerik:RadLabel ID="lblDateofEntry" runat="server" Text="Date"></telerik:RadLabel>
                            <telerik:RadTextBox ID="txtEntryDate" runat="server" Width="130px" CssClass="input" Enabled="false"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtShipCalendarIdEntry" runat="server" Width="1px" CssClass="hidden"
                                ></telerik:RadTextBox>
                            <telerik:RadLabel ID="lblEntryHour" runat="server" Text="Hour"></telerik:RadLabel>
                            <telerik:RadTextBox ID="txtEntryHour" runat="server" Width="50px" Enabled="false"
                                CssClass="input" ></telerik:RadTextBox>
                            </span>
                                </td>
                                <td>
                                <asp:LinkButton runat="server" AlternateText="Entry Date" OnClientClick="return showPickList('spnPickListEntryDate', 'codehelp1', '', '../Common/CommonPickListRHShipWork.aspx', true);" 
                                ID="cmdPickListEntry" ToolTip="Entry into US Water">
                             <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblvesselinUSWaterExit" runat="server" Text="Exit From US Water"></telerik:RadLabel>
                    </td>
                    <td>
                         <table>
                            <tr>
                                <td>
                        <span id="spnPickListExitDate">
                            <telerik:RadLabel ID="lblDateofExit" runat="server" Text="Date"></telerik:RadLabel>
                            <telerik:RadTextBox ID="txtExitDate" runat="server" Width="130px" CssClass="input" Enabled="false" ></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtShipCalendarIdExit" runat="server" Width="1px" CssClass="hidden"
                                ></telerik:RadTextBox>
                            <telerik:RadLabel ID="lblExitHour" runat="server" Text="Hour"></telerik:RadLabel>
                            <telerik:RadTextBox ID="txtExitHour" runat="server" Width="50px" Enabled="false"
                                CssClass="input" ></telerik:RadTextBox>
                             
                        </span>
                                    </td>
                                <td>
                                   <asp:LinkButton runat="server" AlternateText="Exit Date" OnClientClick="return showPickList('spnPickListExitDate', 'codehelp1', '', '../Common/CommonPickListRHShipWork.aspx', true);" 
                                ID="cmdPickListExit" ToolTip="Exit From US Water">
                             <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblpassingthroughHRA" runat="server" Text="Is vessel Transiting Gulf of Aden or passing through HRA in next 14 days?"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox runat="server" ID="ChkThroughHRA" AutoPostBack="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEntryintoECA" runat="server" Text="Entry into ECA"></telerik:RadLabel>
                    </td>
                    <td>


                        <eluc:Date ID="txtECAEntryDate" runat="server" CssClass="input" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtTimeofECAEntry" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                        <asp:CheckBox runat="server" ID="ChkECAyn" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblExitECA" runat="server" Text="Exit From ECA"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtExitECADate" runat="server" CssClass="input" DatePicker="true" Enabled="false" />
                        <telerik:RadTimePicker ID="txtExitECATime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
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
                        <telerik:RadLabel ID="lblFuelusedinECA" runat="server" Text="Fuel used in ECA"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlECAOilType" runat="server" CssClass="input" DataValueField="FLDOILTYPECODE" DataTextField="FLDOILTYPENAME">
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
                    <td valign="center">
                        <telerik:RadLabel ID="lblMasterRemarks" runat="server" Text="Master Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" Height="50px" TextMode="MultiLine" Resize="Both"
                            Width="50%" />
                    </td>
                </tr>
                <tr runat="server" id="trreviewby">
                    <td valign="center">
                        <telerik:RadLabel ID="lblreviewedby" runat="server" Text="Reviewed By"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtreviewedby" runat="server" Enabled="false" CssClass="readonlytextbox"   Resize="Both"
                            Width="50%" />
                    </td>
                </tr>
                <tr runat="server" id="trreviewdate">
                    <td>
                        <telerik:RadLabel ID="lblReviewedDate" runat="server" Text="Reviewed Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucReviewedDate" runat="server" Enabled="false" CssClass="readonlytextbox" DatePicker="true" />

                    </td>
                </tr>
                <tr>
                    <td style="width: 15%;">
                        <telerik:RadLabel ID="lblOilmajor" runat="server" Text="Oil major" Visible="false"></telerik:RadLabel>
                    </td>
                    <td style="width: 75%;">
                        <telerik:RadComboBox runat="server" ID="ddlOilMajor" CssClass="input" Enabled="false" Visible="false"
                            AppendDataBoundItems="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value=""></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="BP" Value="BP"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Exxon" Value="EXXON"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Shell" Value="SHELL"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Chevron" Value="CHEVRON"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Total" Value="TOTAL"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Others" Value="OTHERS"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 15%;">
                        <telerik:RadLabel ID="lblOilMajorCargoOnboard" runat="server" Visible="false" Text="Oil major cargo on board"></telerik:RadLabel>
                    </td>
                    <td style="width: 75%;">
                        <telerik:RadCheckBox runat="server" ID="ChkOilMajorCargoOnboard" Visible="false" Enabled="false" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEMLogCounter" runat="server" Text="EM Log Counter" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtEMLogCounter" runat="server" CssClass="input" MaxLength="15" DecimalPlace="2" Width="80px" Visible="false" />
                        &nbsp;
                        <telerik:RadCheckBox runat="server" ID="chkCounterDefective" AutoPostBack="True" Visible="false" OnCheckedChanged="chkCounterDefective_OnCheckedChanged" Text="counter defective / Reset" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEMLogDistance" runat="server" Text="EM Log Distance" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtEMLogDistance" runat="server" CssClass="readonlytextbox" Visible="false" MaxLength="9" DecimalPlace="2" Enabled="false" Width="80px" />
                        &nbsp;
                        <telerik:RadLabel ID="lblEMLogDistanceUnit" runat="server" Text="nm" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
