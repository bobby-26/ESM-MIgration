<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionNoonReportEngine.aspx.cs"
    Inherits="VesselPositionNoonReportEngine" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Direction" Src="~/UserControls/UserControlDircection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Noon Report</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function rowClick(sender, eventArgs) {
                sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
            }
        </script>
    </telerik:RadCodeBlock>
        <Style>
        .RadGrid .rgHeader, .RadGrid th.rgResizeCol {
            padding-left: 2px !important;
            padding-right: 2px !important;
        }
        .RadGrid .rgRow td,.RadGrid .rgAltRow td
        {
            padding-left: 2px !important;
            padding-right: 2px !important;    
        }

    </Style>
</head>
<body>
    <form id="frmNoonReport" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true" />
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlNoonReportData" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlNoonReportData">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuNRSubTab" runat="server" TabStrip="true" OnTabStripCommand="MenuNRSubTab_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand"></eluc:TabStrip>

            <telerik:RadLabel ID="lblAlertSenttoOFC" runat="server" Text="Report Sent to Office" Visible="false" Font-Bold="False" Font-Size="Large"
                Font-Underline="True" ForeColor="Red" BorderColor="Red">
            </telerik:RadLabel>

            <telerik:RadDockZone ID="RadDockZone2" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="100%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock1" runat="server" Title="<b>Genaral</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table width="100%">
                            <tr>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblmEngineDistance" runat="server" Text="Engine Distance"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtEngineDistance" runat="server" CssClass="input" MaxLength="9"
                                        Width="80px" />
                                    &nbsp;
                                    <telerik:RadLabel ID="lblmEngineDistancenm" runat="server" Text="nm"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblSWTemp" runat="server" Text="SW Temp"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtSwellTemp" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                    &nbsp; &deg;
                                    <telerik:RadLabel ID="lblSWTempC" runat="server" Text="C"></telerik:RadLabel>
                                </td>

                            </tr>
                            <tr>
                                <td id="lblLogSpeed" style="width: 25%">
                                    <telerik:RadLabel ID="lblmSlip" runat="server" Text="Slip"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtSlip" runat="server" CssClass="readonlytextbox" Enabled="false"
                                        Width="80px" MaxLength="9" />
                                    &nbsp;
                                    <telerik:RadLabel ID="lblmSlipPercentage" runat="server" Text="%"></telerik:RadLabel>
                                </td>

                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblmSWPress" runat="server" Text="SW Press"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number runat="server" ID="txtSWPress" CssClass="input" MaxLength="9" DecimalPlace="2"
                                        Width="80px" />
                                    &nbsp;
                                    <telerik:RadLabel ID="lblmSWPressbar" runat="server" Text="bar"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblmERTemp" runat="server" Text="ER Temp"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtERExhTemp" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                    &nbsp; &deg;
                                    <telerik:RadLabel ID="lblmERTempC" runat="server" Text="C"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
            <telerik:RadDockZone ID="RadDockZone1" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="100%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock2" runat="server" Title="<b>Main Engine</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 50%">
                                    <table width="100%" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblrevcounter" runat="server" Text="M/E Rev Counter"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <eluc:Number runat="server" ID="txtrevcounter" CssClass="input" MaxLength="15" DecimalPlace="2" IsPositive="true"
                                                    Width="80px" />
                                                &nbsp;
                                    <telerik:RadCheckBox runat="server" ID="chkRevCounterDefective" AutoPostBack="True" OnCheckedChanged="chkRevCounterDefective_OnCheckedChanged" Text=" counter defective / Reset" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmRPM" runat="server" Text="Average RPM"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <eluc:Number runat="server" ID="txtMERPM" CssClass="input" MaxLength="9" IsPositive="true"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 50%">
                                                <telerik:RadLabel ID="lblPowerOutput" runat="server" Text="Power Output"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 50%">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <eluc:Number ID="txtBHP" runat="server" CssClass="input" DecimalPlace="2" MaxLength="9" Width="80px" />
                                                        </td>
                                                        <td>
                                                            <telerik:RadRadioButtonList ID="rbtnPowerUnit" runat="server" AutoPostBack="True" Direction="Horizontal">
                                                                <Items>
                                                                    <telerik:ButtonListItem Value="BHP" Text="BHP"></telerik:ButtonListItem>
                                                                    <telerik:ButtonListItem Value="KW" Text="KW"></telerik:ButtonListItem>
                                                                </Items>
                                                            </telerik:RadRadioButtonList>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>

                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblcalculatedBHP" runat="server" Text="Calculated BHP"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Number ID="txtcalculatedBHP" runat="server" CssClass="input" DecimalPlace="2" MaxLength="12" Enabled="false" Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmGovernorSetting" runat="server" Text="Governor Setting or Fuel rack"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <eluc:Number runat="server" ID="txtGovernorSetting" CssClass="input" MaxLength="9"
                                                    DecimalPlace="2" IsPositive="true" Width="80px" />
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
                                                <telerik:RadLabel ID="lblMaxExhTemp" runat="server" Text="Exh Temp Max"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <eluc:Number ID="txtMaxExhTemp" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                                &nbsp; &deg;
                                    <telerik:RadLabel ID="lblMaxExhTempC" runat="server" Text="C"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblExhTempMin" runat="server" Text="Exh Temp Min"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <eluc:Number ID="txtMinExhTemp" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                                &nbsp; &deg;
                                    <telerik:RadLabel ID="lblExhTempMinC" runat="server" Text="C"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblScavAirTemp" runat="server" Text="Scav Air Temp"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <eluc:Number ID="txtScavAirTemp" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                                &nbsp; &deg;
                                    <telerik:RadLabel ID="lblScavAirTempC" runat="server" Text="C"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmFOInletTemp" runat="server" Text="FO Inlet Temp"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <eluc:Number runat="server" ID="txtFOInletTemp" CssClass="input" MaxLength="9" DecimalPlace="2"
                                                    Width="80px" />
                                                &nbsp; &deg;
                                    <telerik:RadLabel ID="lblmFOInletTempC" runat="server" Text="C"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="width: 50%">
                                    <table width="100%" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblScavAirPress" runat="server" Text="Scav Air Press"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <eluc:Number runat="server" ID="txtScavAirPress" CssClass="input" MaxLength="9" DecimalPlace="2" Width="80px" />
                                                &nbsp;
                                    <telerik:RadLabel ID="lblScavAirPressbar" runat="server" Text="bar"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmFuelOilPress" runat="server" Text="Fuel Oil Press"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <eluc:Number runat="server" ID="txtFuelOilPress" CssClass="input" MaxLength="9" DecimalPlace="2"
                                                    Width="80px" />
                                                &nbsp;
                                    <telerik:RadLabel ID="lblmFuelOilPressbar" runat="server" Text="bar"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblTC1RPM" runat="server" Text="T/C1 RPM"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <eluc:Number ID="txtTCRPMInboard" runat="server" CssClass="input" DecimalPlace="2" MaxLength="9" Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblTC2RPM" runat="server" Text="T/C2 RPM"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <eluc:Number ID="txtTCRPMOutboard" runat="server" CssClass="input" DecimalPlace="2" MaxLength="9" Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblExhTCInboardBefore" runat="server" Text="T/C1 Exh Gas Temp In"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <eluc:Number ID="txtExhTCInboardBefore" runat="server" CssClass="input" DecimalPlace="2" MaxLength="9" Width="80px" />
                                                &nbsp; &deg;
                                    <telerik:RadLabel ID="lblExhTCInboardBeforeC" runat="server" Text="C"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblExhTCInboardAfter" runat="server" Text="T/C1 Exh Gas Temp Out"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <eluc:Number ID="txtExhTCInboardAfter" runat="server" CssClass="input" DecimalPlace="2" MaxLength="9" Width="80px" />
                                                &nbsp; &deg;
                                    <telerik:RadLabel ID="lblExhTCInboardAfterC" runat="server" Text="C"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblExhTCOutboardBefore" runat="server" Text="T/C2 Exh Gas Temp In"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <eluc:Number ID="txtExhTCOutboardBefore" runat="server" CssClass="input" DecimalPlace="2" MaxLength="9" Width="80px" />
                                                &nbsp; &deg;
                                    <telerik:RadLabel ID="lblExhTCOutboardBeforeC" runat="server" Text="C"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblExhTCOutboardAfter" runat="server" Text="T/C2 Exh Gas Temp Out"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <eluc:Number ID="txtExhTCOutboardAfter" runat="server" CssClass="input" DecimalPlace="2" MaxLength="9" Width="80px" />
                                                &nbsp; &deg;
                                    <telerik:RadLabel ID="lblExhTCOutboardAfterC" runat="server" Text="C"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td id="Td1">
                                                <telerik:RadLabel ID="lblFOCatFine" runat="server" Text="FO Cat Fines"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <eluc:Number ID="txtFOCatFines" runat="server" CssClass="input"
                                                    Width="80px" MaxLength="9" />

                                            </td>
                                        </tr>

                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
            <telerik:RadDockZone ID="RadDockZone3" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="100%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock3" runat="server" Title="<b>Aux Engine</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table width="100%" cellpadding="1" cellspacing="1">

                            <tr>
                                <td style="width: 25%"></td>
                                <td style="width: 25%"></td>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblOperatinghrs" runat="server" Text="Hours Of Operation"></telerik:RadLabel>

                                </td>
                                <td style="width: 25%"></td>
                            </tr>
                            <tr>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblAENo1GeneratorLoad" runat="server" Text="A/E No 1. Generator Load"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtGeneratorLoadAE1" runat="server" MaxLength="9" DecimalPlace="2" CssClass="input" Width="80px" />
                                    &nbsp;
                                    <telerik:RadLabel ID="lblAENo1GeneratorLoadkw" runat="server" Text="kw"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtGeneratorLoadAE1OPHrs" runat="server" CssClass="input"
                                        Width="80px" MaxLength="9" />
                                    <telerik:RadLabel ID="lblGeneratorLoadAE1hrs" runat="server" Text="hrs"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%"></td>
                            </tr>
                            <tr>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblAENo2GeneratorLoad" runat="server" Text="A/E No 2. Generator Load"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtGeneratorLoadAE2" runat="server" MaxLength="9" DecimalPlace="2" CssClass="input" Width="80px" />
                                    &nbsp;
                                    <telerik:RadLabel ID="lblAENo2GeneratorLoadkw" runat="server" Text="kw"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtGeneratorLoadAE2OPHrs" runat="server" CssClass="input"
                                        Width="80px" MaxLength="9" />
                                    <telerik:RadLabel ID="lblGeneratorLoadAE2hrs" runat="server" Text="hrs"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%"></td>
                            </tr>
                            <tr>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblAENo3GeneratorLoad" runat="server" Text="A/E No 3. Generator Load"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtGeneratorLoadAE3" runat="server" MaxLength="9" DecimalPlace="2" CssClass="input" Width="80px" />
                                    &nbsp;
                                    <telerik:RadLabel ID="lblAENo3GeneratorLoadkw" runat="server" Text="kw"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtGeneratorLoadAE3OPHrs" runat="server" CssClass="input"
                                        Width="80px" MaxLength="9" />
                                    <telerik:RadLabel ID="lblGeneratorLoadAE3hrs" runat="server" Text="hrs"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%"></td>
                            </tr>

                            <tr>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblAENo4GeneratorLoad" runat="server" Text="A/E No 4. Generator Load"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtGeneratorLoadAE4" runat="server" MaxLength="9" DecimalPlace="2" CssClass="input" Width="80px" />
                                    &nbsp;
                                    <telerik:RadLabel ID="lblAENo4GeneratorLoadkw" runat="server" Text="kw"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtGeneratorLoadAE4OPHrs" runat="server" CssClass="input"
                                        Width="80px" MaxLength="9" />
                                    <telerik:RadLabel ID="lblGeneratorLoadAE4hrs" runat="server" Text="hrs"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%"></td>
                            </tr>
                            <tr>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblEarthFaultMonitor400" runat="server" Text="Earth Fault Monitor 440 Volts"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtEarthFaultMonitor400" runat="server" MaxLength="9" DecimalPlace="2" CssClass="input" Width="80px" />
                                    &nbsp;
                                    <telerik:RadLabel ID="lblunitEarthFaultMonitor400" runat="server" Text="MΩ"></telerik:RadLabel>
                                </td>

                            </tr>
                            <tr>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblEarthFaultMonitor230or110" runat="server" Text="Earth Fault Monitor 230/110 Volts"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtEarthFaultMonitor230or110" runat="server" MaxLength="9" DecimalPlace="2" CssClass="input" Width="80px" />
                                    &nbsp;
                                    <telerik:RadLabel ID="lblunitEarthFaultMonitor230or110" runat="server" Text="MΩ"></telerik:RadLabel>
                                </td>

                            </tr>

                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
            <telerik:RadDockZone ID="RadDockZone4" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="100%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock4" runat="server" Title="<b>Purifier</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table width="50%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 25%"></td>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblHrsOfOperation" runat="server" Text="Hours Of Operation"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblhfopfr1" runat="server" Text="HFO PFR 1"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txthfopfr1" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblhfopfr2" runat="server" Text="HFO PFR 2"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txthfopfr2" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lbldopfr" runat="server" Text="DO PFR "></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtdopfr" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblmelopfr" runat="server" Text="ME LO PFR"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtmelopfr" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblaelopfr" runat="server" Text="AE LO PFR"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtaelopfr" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                </td>
                            </tr>
                            <tr>
                                <td>&nbsp</td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
            <telerik:RadDockZone ID="RadDockZone5" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="100%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock5" runat="server" Title="<b>Fine Filter</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 25%"></td>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblCounters" runat="server" Text="Counter"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblnoofoperations" runat="server" Text="No Of Operations"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%"></td>
                            </tr>
                            <tr>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblMEFOAUTOBACKWASHFILTER" runat="server" Text="ME FO AUTO BACK WASH FILTER"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtMEFOAUTOBACKWASHFILTER" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtNOOFOPERATIONSMEFOAUTOBACKWASHFILTER" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="9" Width="80px" />
                                    &nbsp;
                                    <telerik:RadCheckBox runat="server" ID="chkFOCounterDefective" AutoPostBack="True" OnCheckedChanged="chkFOCounterDefective_OnCheckedChanged" Text="counter defective / Reset" />
                                </td>
                                <td style="width: 25%"></td>
                            </tr>
                            <tr>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblMELOAUTOBACKWASHFILTER" runat="server" Text="ME LO AUTO BACK WASH FILTER"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtMELOAUTOBACKWASHFILTER" runat="server" CssClass="input" MaxLength="9" Width="80px" />
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number ID="txtNOOFOPERATIONSMELOAUTOBACKWASHFILTER" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="9" Width="80px" />
                                    &nbsp;
                                    <telerik:RadCheckBox runat="server" ID="chkLOCounterDefective" AutoPostBack="True" OnCheckedChanged="chkLOCounterDefective_OnCheckedChanged" Text="counter defective / Reset" />
                                </td>
                                <td style="width: 25%"></td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>

            <telerik:RadDockZone ID="RadDockZone6" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="100%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock6" runat="server" Title="<b>Fresh Water</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table cellpadding="1" cellspacing="1" width="100%">

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

                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 15%;">
                                    <br />
                                </td>
                                <td style="width: 25%;"></td>
                                <td></td>
                                <td></td>
                            </tr>
                            <tr>
                                <td style="width: 25%">
                                    <telerik:RadLabel ID="lblmBoilerwaterChlorides" runat="server" Text="Boiler water Chlorides"></telerik:RadLabel>
                                </td>
                                <td style="width: 25%">
                                    <eluc:Number runat="server" ID="txtBoilerWaterChlorides" CssClass="input" MaxLength="9"
                                        DecimalPlace="2" Width="80px" />
                                    &nbsp;
                                    <telerik:RadLabel ID="lblmBoilerwaterChloridesppm" runat="server" Text="ppm"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
            <telerik:RadDockZone ID="RadDockZone7" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="100%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock7" runat="server" Title="<b>Bilge and Sludge</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table width="67%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblmBilgeTankROB" runat="server" Text="Bilge Tank ROB"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number runat="server" ID="txtBilgeROB" CssClass="input" MaxLength="9" DecimalPlace="2"
                                        Width="80px" />
                                    &nbsp;
                                    <telerik:RadLabel ID="lblmBilgeTankROBcum" runat="server" Text="cu. m"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblmSludgeTankROB" runat="server"  Text="Total Sludge Retained onboard"></telerik:RadLabel>
                                    <eluc:Tooltip ID="ucSludgeTankROBAlert" runat="server" Text="Including all sludge , waste oil and oily bilge water tanks." />
                                </td>
                                <td>
                                    <eluc:Number runat="server" ID="txtSludgeROB" CssClass="input" MaxLength="9" DecimalPlace="2"
                                        Width="80px" />
                                    &nbsp;
                                    <telerik:RadLabel ID="lblmSludgeTankROBcum" runat="server" Text="cu. m"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblmBilgeLanding" runat="server" Text="Last landing of Bilge Water"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date runat="server" ID="txtBilgeLanding" Enabled="false" CssClass="readonlytextbox" />
                                    <telerik:RadTextBox ID="txtBilgeLandingTime" runat="server" Enabled="false" CssClass="readonlytextbox"
                                        Width="50px" Visible="false" />
                                    &nbsp;&nbsp;
                                    <telerik:RadLabel ID="lblmBilgeLandingDays" runat="server" Text="Days"></telerik:RadLabel>
                                    <eluc:Number runat="server" ID="txtBilgeLandingDays" CssClass="readonlytextbox" Enabled="false"
                                        Width="80px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblmLastLandSludge" runat="server" Text="Last landing of Sludge"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date runat="server" ID="txtLastLandSludge" Enabled="false" CssClass="readonlytextbox" />
                                    <telerik:RadTextBox ID="txtLastLandSludgeTime" runat="server" Enabled="false" CssClass="readonlytextbox"
                                        Width="50px" Visible="false" />
                                    &nbsp;&nbsp;
                                    <telerik:RadLabel ID="lblmLastLandSludgeDays" runat="server" Text="Days"></telerik:RadLabel>
                                    <eluc:Number runat="server" ID="txtLastLandingDays" CssClass="readonlytextbox" Enabled="false"
                                        Width="80px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblmChiefEngineerRemarks" runat="server" Text="Chief Engineer Remarks"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtRemarksCE" runat="server" CssClass="input" Height="50px" TextMode="MultiLine" Resize="Both"
                                        Width="260px" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
            <telerik:RadDockZone ID="RadDockZone8" BorderStyle="None" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%">
                <telerik:RadDock Width="100%" RenderMode="Lightweight" EnableDrag="false" ID="RadDock8" runat="server" Title="<b>Consumption</b>" EnableAnimation="true"
                    EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                    <Commands>
                        <telerik:DockExpandCollapseCommand />
                    </Commands>
                    <ContentTemplate>
                        <table cellpadding="1" cellspacing="1" width="100%">
                            <tr>
                                <td colspan="6">
                                    <telerik:RadLabel ID="lblAlert" runat="server" Text="The FOC rate/per 24hrs exceeds the max FOC rate at MCR. Pls check the FOC and Hrs run values!" ForeColor="Red" Font-Bold="true" Visible="false"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="color: Blue;">
                                    <telerik:RadLabel ID="lblmFuelOilConsumption" runat="server" Text="* Fuel Oil Consumption in mT"></telerik:RadLabel>
                                </td>
                                <td>
                                    <b>
                                        <telerik:RadLabel ID="lblFOConsPerday" runat="server" Text="FO Cons Rate (mt/day)"></telerik:RadLabel>
                                    </b>

                                </td>
                                <td align="left">
                                    <eluc:Number runat="server" ID="txtFOConsPerday" CssClass="readonlytextbox" MaxLength="9" DecimalPlace="4"
                                        Width="80px" />
                                </td>
                                <td>
                                    <b>
                                        <telerik:RadLabel ID="lblDensity" runat="server" Text="Density @ 15˚C"></telerik:RadLabel>
                                    </b>
                                </td>
                                <td align="left">
                                    <eluc:Number runat="server" ID="txtDensity" CssClass="input" MaxLength="9" DecimalPlace="4"
                                        Width="80px" />
                                </td>
                            </tr>
                            <tr>
                                <td style="color: Blue">
                                    <telerik:RadLabel ID="lblmLubOilConsumption" runat="server" Text="* Lub Oil Consumption in Ltr"></telerik:RadLabel>
                                </td>
                                <td>
                                    <b>
                                        <telerik:RadLabel ID="lblDOConsRate" runat="server" Text="DO Cons Rate (mt/day)"></telerik:RadLabel>
                                    </b>
                                </td>
                                <td align="left">
                                    <eluc:Number runat="server" ID="txtDOConsPerday" CssClass="readonlytextbox" MaxLength="9" DecimalPlace="2"
                                        Width="80px" />
                                </td>
                                <td>
                                    <b>
                                        <telerik:RadLabel ID="lblsulphur" runat="server" Text="Sulphur Content %"></telerik:RadLabel>
                                    </b>
                                </td>
                                <td align="left">
                                    <eluc:Number runat="server" ID="txtSulphur" CssClass="input" MaxLength="9" DecimalPlace="2"
                                        Width="80px" />
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
                                    <telerik:GridColumnGroup HeaderText="IN PORT" Name="IN PORT" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>

                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Oil Type">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblOilTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOILTYPE")%>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lbloilconsumptiononlaterdateyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDOILCONSUMPTIONONLATERDATEYN")%>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblOilTypeName" runat="server" Visible="true" ToolTip='<%# DataBinder.Eval(Container, "DataItem.FLDOILTYPENAME")%>' Text='<%# DataBinder.Eval(Container, "DataItem.FLDOILTYPENAME")%>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblOilConsumptionIdItem" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Previous<br />ROB" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPreviousRob" runat="server" MaxLength="6" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPREVIOUSROB")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="meatsea" HeaderStyle-HorizontalAlign="Center" HeaderText="M/E" HeaderStyle-Width="40px" ColumnGroupName="AT SEA" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtSeaME" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAME")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <table cellpadding="0">
                                                <tr>
                                                    <td>M/E</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtSeaMEEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="60px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAME")%>' />
                                                    </td>
                                                    <td>A/E</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtSeaAEEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="60px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAAE")%>' />
                                                    </td>
                                                    <td>BLR</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtSeaBLREdit" runat="server" CssClass="gridinput" MaxLength="6" Width="60px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEABLR")%>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>IGG</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtSeaIGGEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="60px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAIGG")%>' />
                                                    </td>
                                                    <td>C/ENG</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtSeaCARGOENGEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="60px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEACARGOENG")%>' />
                                                    </td>
                                                    <td>C/HTG</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtSeaCARGOHEATINdit" runat="server" CssClass="gridinput" MaxLength="6" Width="60px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEACTHG")%>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>TK CLNG</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtSeaTKCLNGEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="60px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEATKCLNG")%>' />
                                                    </td>
                                                    <td>OTH</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtSeaOTHEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="60px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAOTH")%>' />
                                                    </td>
                                                    <td></td>
                                                    <td></td>
                                                </tr>
                                            </table>


                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="aeatsea" HeaderStyle-HorizontalAlign="Center" HeaderText="A/E" HeaderStyle-Width="40px" ColumnGroupName="AT SEA" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtSeaAE" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAAE")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="blratsea" HeaderStyle-HorizontalAlign="Center" HeaderText="BLR" HeaderStyle-Width="40px" ColumnGroupName="AT SEA" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtSeaBLR" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEABLR")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="iggatsea" HeaderStyle-HorizontalAlign="Center" HeaderText="IGG" HeaderStyle-Width="40px" ColumnGroupName="AT SEA" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtSeaIGG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAIGG")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="cengatsea" HeaderStyle-HorizontalAlign="Center" HeaderText="C/ENG" HeaderStyle-Width="40px" ColumnGroupName="AT SEA" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtSeaCARGOENG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEACARGOENG")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="chtgatsea" HeaderStyle-HorizontalAlign="Center" HeaderText="C/HTG" HeaderStyle-Width="40px" ColumnGroupName="AT SEA" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtSeaCARGOHEATIN" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEACTHG")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="tkclngatsea" HeaderStyle-HorizontalAlign="Center" HeaderText="TK CLNG" HeaderStyle-Width="40px" ColumnGroupName="AT SEA" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtSeaTKCLNG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEATKCLNG")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="othatsea" HeaderStyle-HorizontalAlign="Center" HeaderText="OTH" HeaderStyle-Width="40px" ColumnGroupName="AT SEA" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtSeaOTH" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATSEAOTH")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourMEHeader" Visible="true" Text=" M/E " runat="server">
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourME" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURME")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="ucAtHourbourMEEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="98%"
                                                DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURME")%>' />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourAEHeader" Visible="true" Text=" A/E " runat="server">
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourAE" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURAE")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="ucAtHourbourAEEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="98%"
                                                DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURAE")%>' />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourBLRHeader" Visible="true" Text=" BLR " runat="server">
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourBLR" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURBLR")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="ucAtHourbourBLREdit" runat="server" CssClass="gridinput" MaxLength="6" Width="98%"
                                                DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURBLR")%>' />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourIGGHeader" Visible="true" Text="IGG" runat="server">
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourIGG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURIGG")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="ucAtHourbourIGGEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="98%"
                                                DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURIGG")%>' />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourCARGOENGHeader" Visible="true" Text="C/ENG" runat="server">
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourCARGOENG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURCARGOENG")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="ucAtHourbourCARGOENGEdit" runat="server" CssClass="gridinput" MaxLength="6" WWidth="98%"
                                                DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOURCARGOENG")%>' />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourOTHHeader" Visible="true" Text=" OTH " runat="server">
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourOTH" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOUROTH")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="ucAtHourbourOTHEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="98%"
                                                DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.ATHARBOUROTH")%>' />
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="meinport" HeaderStyle-HorizontalAlign="Center" HeaderText="M/E" HeaderStyle-Width="40px" ColumnGroupName="IN PORT" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInPortME" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTME")%>'></telerik:RadLabel>
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
                                                        <eluc:Number ID="ucAtHourbourCARGOHEATINdit" runat="server" CssClass="gridinput" MaxLength="6" Width="60px"
                                                            DecimalPlace="2" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTCTHG")%>' />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>TK CLNG</td>
                                                    <td>
                                                        <eluc:Number ID="ucAtHourbourTKCLNGEdit" runat="server" CssClass="gridinput" MaxLength="6" Width="60px"
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
                                    <telerik:GridTemplateColumn UniqueName="aeinport" HeaderStyle-HorizontalAlign="Center" HeaderText="A/E" HeaderStyle-Width="40px" ColumnGroupName="IN PORT" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInPortAE" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTAE")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="blrinport" HeaderStyle-HorizontalAlign="Center" HeaderText="BLR" HeaderStyle-Width="40px" ColumnGroupName="IN PORT" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInPortBLR" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTBLR")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="igginport" HeaderStyle-HorizontalAlign="Center" HeaderText="IGG" HeaderStyle-Width="40px" ColumnGroupName="IN PORT" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInPortIGG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTIGG")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="cenginport" HeaderStyle-HorizontalAlign="Center" HeaderText="C/ENG" HeaderStyle-Width="40px" ColumnGroupName="IN PORT" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInPortCARGOENG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTCARGOENG")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="chtginport" HeaderStyle-HorizontalAlign="Center" HeaderText="C/HTG" HeaderStyle-Width="40px" ColumnGroupName="IN PORT" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourCARGOHEATIN" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTCTHG")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="tkclnginport" HeaderStyle-HorizontalAlign="Center" HeaderText="TK CLNG" HeaderStyle-Width="40px" ColumnGroupName="IN PORT" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAtHourbourTKCLNG" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTTKCLNG")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="othinport" HeaderStyle-HorizontalAlign="Center" HeaderText="OTH" HeaderStyle-Width="40px" ColumnGroupName="IN PORT" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInPortOTH" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.INPORTOTH")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="bunker" HeaderText="Bunker Qty" HeaderStyle-Width="45px" HeaderStyle-HorizontalAlign="Center">
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
                                    <telerik:GridTemplateColumn UniqueName="sulphur" HeaderText="Sulphur<br />%" HeaderStyle-Wrap="false" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblSulphurPercentage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSULPHURPERCENTAGE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="total" HeaderText="Total" HeaderStyle-Wrap="false" HeaderStyle-Width="45px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTotal" runat="server" MaxLength="6" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTOTALCONSUMPTION")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="ROB<br />at<br />Noon" HeaderStyle-Wrap="false" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblRobAtNoon" runat="server" MaxLength="6" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROBATNOON")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="82px" HeaderStyle-HorizontalAlign="Center">
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
                                <Scrolling AllowScroll="true" ScrollHeight="" UseStaticHeaders="true" SaveScrollPosition="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                <ClientEvents OnRowClick="rowClick" />
                            </ClientSettings>
                        </telerik:RadGrid>

                    </ContentTemplate>
                </telerik:RadDock>
            </telerik:RadDockZone>
            <table width="100%" cellpadding="1" cellspacing="1" style="display: none;">
                <tr>
                    <td style="width: 15%; display: none;">
                        <telerik:RadLabel ID="lblmTankCleaning" runat="server" Text="HFO Cons for Tank Cleaning (if any)"></telerik:RadLabel>
                    </td>
                    <td style="width: 25%; display: none;">
                        <eluc:Number runat="server" ID="txtHFOTankCleaning" CssClass="input" MaxLength="9"
                            DecimalPlace="2" Width="80px" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td style="width: 15%; display: none;">
                        <telerik:RadLabel ID="lblmCargoHeating" runat="server" Text="HFO Cons for Cargo Heating (if any)"></telerik:RadLabel>
                    </td>
                    <td style="width: 25%; display: none;">
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
