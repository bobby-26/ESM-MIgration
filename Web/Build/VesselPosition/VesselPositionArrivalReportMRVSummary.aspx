<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionArrivalReportMRVSummary.aspx.cs" Inherits="VesselPositionArrivalReportMRVSummary" %>

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
    <title>MRV Summary (Arrival)</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="panel1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="DepartureSummary" TabStrip="true" runat="server" OnTabStripCommand="DepartureSummary_TabStripCommand"></eluc:TabStrip>
        <eluc:TabStrip ID="MRVSummary" runat="server" OnTabStripCommand="MRVSummary_TabStripCommand"></eluc:TabStrip>

        <telerik:RadAjaxPanel runat="server" ID="panel1">

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
                    <td valign="middle">
                        <span>
                            <telerik:RadTextBox runat="server" ID="txtVoyage" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                            <%--  <telerik:RadLabel ID="lblBalast" runat="server" Text="Ballast"></telerik:RadLabel>--%>
                            <telerik:RadCheckBox runat="server" ID="chkBallast" Text="Ballast" Enabled="false" />
                            <%--<telerik:RadLabel ID="lblladen" runat="server" Text="Laden"></telerik:RadLabel>--%>
                            <telerik:RadCheckBox runat="server" ID="chkLaden" Text="Laden" Enabled="false" />
                        </span>
                    </td>

                </tr>
                <tr>
                    <td></td>
                    <td>
                        <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="125" height="0" />
                        <u>
                            <b>
                                <telerik:RadLabel ID="lbleuport" runat="server" Text="EU Port?"></telerik:RadLabel>
                            </b>
                        </u>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromPort" runat="server" Text="From"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtFromPort" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                        <telerik:RadCheckBox runat="server" ID="chkEUPortYN" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblToPort" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtToPort" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                        <telerik:RadCheckBox runat="server" ID="chkToPort" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbldistanceTravelled" runat="server" Text="Total Distance Travelled"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txdistanceTravelled" runat="server" CssClass="readonlytextbox" Enabled="false" Width="80px" />
                        &nbsp;<telerik:RadLabel ID="txtdistanceTravelledUnit" runat="server" Text="nm"></telerik:RadLabel>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTimespentatsea" runat="server" Text="Time Spent At Sea"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtTimespentatsea" runat="server" CssClass="readonlytextbox" Enabled="false" Width="80px" />
                        &nbsp;<telerik:RadLabel ID="lblhr" runat="server" Text="hr"></telerik:RadLabel>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTotalCargoTransported" runat="server" Text="Total Cargo Transported"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtTotalCargoTransported" runat="server" CssClass="readonlytextbox" Width="80px" Enabled="false" />
                        &nbsp;
                                <telerik:RadLabel ID="lblMT" runat="server" Text="MT"></telerik:RadLabel>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTotalTransportwork" runat="server" Text="Total Transport Work"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtTotalTransportwork" runat="server" CssClass="readonlytextbox" Width="80px" Enabled="false" />
                        &nbsp;<telerik:RadLabel ID="lblTnm" runat="server" Text="T-nm"></telerik:RadLabel>
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
                            OnNeedDataSource="gvConsumption_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false"
                            Width="100%" CellPadding="3" AllowSorting="false" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false">
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
                                    <telerik:GridColumnGroup HeaderText="FO Cons (MT)" Name="FO Cons (MT)" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Agg CO2" Name="Aggregated CO2" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Average Fuel Consumption" Name="Average Fuel Consumption" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>
                                <ColumnGroups>
                                    <telerik:GridColumnGroup HeaderText="Average CO₂ Emissions" Name="Average CO₂ Emissions" HeaderStyle-HorizontalAlign="Center">
                                    </telerik:GridColumnGroup>
                                </ColumnGroups>

                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Fuel Type" HeaderStyle-Wrap="false" HeaderStyle-Width="60px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblgvItem" runat="server" Text="Fuel Type"></asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOilType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Emission Factor" HeaderStyle-Wrap="true" HeaderStyle-Width="40px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmissionFactorItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCF") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="ROB @ SBE" HeaderStyle-Wrap="false" HeaderStyle-Width="50px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblROBSBEItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBSBE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="ROB @ FWE" HeaderStyle-Wrap="false" HeaderStyle-Width="50px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblROBFWEItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBFWE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Total" ColumnGroupName="FO Cons (MT)" HeaderStyle-Wrap="false" HeaderStyle-Width="40px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTotalConsItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCONSUMPTIONQTY") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Cargo Heating" ColumnGroupName="FO Cons (MT)" HeaderStyle-Wrap="false" HeaderStyle-Width="60px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCargoHeatingsItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOHEATING") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Emitted(MT CO2)" ColumnGroupName="Aggregated CO2" HeaderStyle-Wrap="false" HeaderStyle-Width="65px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAvgCO2EmissionItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCO2EMISSION") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Distance (kg/nm)" ColumnGroupName="Average Fuel Consumption" HeaderStyle-Wrap="false" HeaderStyle-Width="70px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFuelDistanceItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGFUELCONSDISTANCE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Transport Work (g/t-nm)" ColumnGroupName="Average Fuel Consumption" HeaderStyle-Wrap="false" HeaderStyle-Width="95px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFuelTransportworkItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGFUELCONSTRANSWORK") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Distance (kg CO2/nm)" ColumnGroupName="Average CO₂ Emissions" HeaderStyle-Wrap="false" HeaderStyle-Width="87px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCO2EmissionDistanceItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGCO2DISTANCE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Transport Work (g CO2/t-nm)" ColumnGroupName="Average CO₂ Emissions" HeaderStyle-Wrap="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCO2TransportworkItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGCO2TRANSPORTWORK") %>'></asp:Label>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
