<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionShiftingReportMRVSummary.aspx.cs" Inherits="VesselPositionShiftingReportMRVSummary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Voyage" Src="~/UserControls/UserControlVoyage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Direction" Src="~/UserControls/UserControlDircection.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Emission In Port</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="panel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="panel1">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <eluc:TabStrip ID="DepartureSummary" TabStrip="true" runat="server" OnTabStripCommand="DepartureSummary_TabStripCommand"></eluc:TabStrip>

            <table width="50%" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="width: 15%;">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 25%;">
                        <telerik:RadTextBox runat="server" ID="txtVessel" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoyage" runat="server" Text="Voyage No"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td><telerik:RadTextBox runat="server" ID="txtVoyage" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox></td>
                                <td><telerik:RadLabel ID="lblBalast" runat="server" Text="Ballast"></telerik:RadLabel></td>
                                <td><telerik:RadCheckBox runat="server" ID="chkBallast" Enabled="false" /></td>
                                <td><telerik:RadLabel ID="lblladen" runat="server" Text="Laden"></telerik:RadLabel></td>
                                <td><telerik:RadCheckBox runat="server" ID="chkLaden" Enabled="false" /></td>
                            </tr>
                        </table>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <table>
                            <tr>
                                <td><telerik:RadTextBox runat="server" ID="txtFromPort" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox></td>
                                <td><telerik:RadLabel ID="lbleuport" runat="server" Text="EU Port?"></telerik:RadLabel></td>
                                <td><telerik:RadCheckBox runat="server" ID="chkEUPortYN" Enabled="false" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblArrival" runat="server" Text="Arrival"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date ID="txtarrival" runat="server" CssClass="readonlytextbox" Enabled="false" />
                        <telerik:RadTimePicker ID="txtArivalTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>

                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeparture" runat="server" Text="Departure (SBE)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date ID="txtDeparture" runat="server" CssClass="readonlytextbox" Enabled="false" />
                        <telerik:RadTimePicker ID="txtDepartureTime" runat="server" Width="80px" CssClass="readonlytextbox" Enabled="false"
                            DateInput-DateFormat="HH:mm" TimeView-TimeFormat="HH:mm">
                        </telerik:RadTimePicker>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTotaltime" runat="server" Text="Total Time in Port"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtStopped" runat="server" CssClass="readonlytextbox" Width="80px" Enabled="false" />
                        &nbsp;
                        <telerik:RadLabel ID="lbltotalhours" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmanouveringdist" runat="server" Text="Logged Manouvering Distance"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtmanouveringdist" runat="server" CssClass="readonlytextbox" Width="80px" Enabled="false" />
                        &nbsp;
                        <telerik:RadLabel ID="lblmanouveringdistUnit" runat="server" Text="nm"></telerik:RadLabel>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCO2Emitted" runat="server" Text="Total Aggregated CO2 Emitted"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtCO2Emitted" runat="server" CssClass="readonlytextbox" Width="80px" Enabled="false" />
                        &nbsp;<telerik:RadLabel ID="txtTCO2" runat="server" Text="T-CO2"></telerik:RadLabel>
                    </td>

                </tr>
            </table>
            <br />
            <br />

            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td colspan="4">
                        <b>
                            <telerik:RadLabel ID="lblConsumption" runat="server" Text="Consumption"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadGrid ID="gvConsumption" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" AllowSorting="false" ShowFooter="false" ShowHeader="true" EnableViewState="false"
                            EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvConsumption_NeedDataSource">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false"  CommandItemDisplay="Top">
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
                                    <telerik:GridTemplateColumn HeaderText="Oil Type" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblOilType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Emission Factor" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblEmissionFactorItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCF") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="ROB @ FWE" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblROBFWEItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBFWE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Bunkered" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblBunkeredItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUNKEREDQTY") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="ROB @ SBE" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblROBSBEItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBONSBE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Total FO Cons" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTotalConsItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCONSUMPTIONQTY") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Cargo Heating Cons" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCargoHeatingsItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOHEATING") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Aggregated CO2 Emitted(MT CO2)" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAvgCO2EmissionItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCO2EMISSION") %>'></telerik:RadLabel>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
