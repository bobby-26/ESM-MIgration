<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionIMODCSReportBDNSummary.aspx.cs"
    Inherits="VesselPositionIMODCSReportBDNSummary" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reports</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {

                TelerikGridResize($find("<%=gvConsumption.ClientID %>"));
           }
           window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
               fade('statusmessage');
           }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewCourseList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlCourseListEntry" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlDayToDayReport">
            <eluc:TabStrip ID="MainTab" runat="server" TabStrip="true" OnTabStripCommand="MainTab_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselSearch" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input" AppendDataBoundItems="true" AutoPostBack="true" VesselsOnly="true" SyncActiveVesselsOnly="True" AssignedVessels="true"
                            OnTextChangedEvent="ddlVessel_TextChangedEvent" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlFleet" runat="server" CssClass="input" AutoPostBack="true" OnTextChanged="ddlVessel_TextChangedEvent" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblyear" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtFrom" CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblmonth" runat="server" Text="Todate"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtTo" CssClass="input" DatePicker="true" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuDayToDayReportTab" runat="server" OnTabStripCommand="DayToDayReportTab_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvConsumption" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                OnNeedDataSource="gvConsumption_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false"
                Width="100%" CellPadding="3" AllowSorting="false" ShowFooter="false" ShowHeader="true" EnableViewState="false"
                AllowCustomPaging="true" AllowPaging="true" OnItemCommand="gvConsumption_ItemCommand">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Wrap="false" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date of Operation" HeaderStyle-Wrap="false" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOilType" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DO/GO">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmissionFactorItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMDOMGO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="LFO" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblROBFWEItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLFO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HFO" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBunkeredItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHFO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="LPG(P)" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblROBSBEItem" runat="server" Text=""></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="LPG(B)" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalConsItem" runat="server" Text=""></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="LNG" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCargoHeatingsItem" runat="server" Text=""></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Others(CF)" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAvgCO2EmissionItem" runat="server" Text=""></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Descriptions" HeaderStyle-Wrap="false">
                            <ItemStyle HorizontalAlign="Left" Wrap="False"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAvgCO2EmissionItems" runat="server" Text=""></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <%--<telerik:RadGrid ID="gvDayToDayReport" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" Height="45%" CellPadding="3" OnItemCommand="gvDayToDayReport_RowCommand"
                AllowSorting="false" ShowFooter="false" ShowHeader="true"
                EnableViewState="false" OnNeedDataSource="gvDayToDayReport_NeedDataSource"
                AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDPRSMPADAYTODAYREPORTID">
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
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Fuel Consumption (Metric tons)" Name="ConsGroup" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                    <Columns>
                         <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Wrap="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="false" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReportID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRSMPADAYTODAYREPORTID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date From" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStartDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date To" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEndDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTODATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="IMO number" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIMONumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMONUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ship type" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShipType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Gross <br/>tonnage (GT)" HeaderStyle-Wrap="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGrossTonnage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREGISTEREDGT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Net <br/>tonnage (NT)" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNetTonnage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREGISTEREDNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Deadweight <br/> tonnage (DWT)" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDeadweightTonnage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDWTSUMMER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="EEDI(gCO2/t.nm)" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEEDI" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEEDI") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ice class" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIceclass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDICECLASS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Main propulsion <br/> power" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMainpropulsionpower" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEPOWEROUTPUT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Auxiliary <br/> engine(s)" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuxiliaryenginer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAEPOWEROUTPUT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Distance <br/> travelled (nm)" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDistancetravelled" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISTANCETRAVELLED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hours underway (h)" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHoursunderway" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOURSUNDERWAY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DO/GO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ConsGroup" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDieselGasOil" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMDOMGOCONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="LFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ConsGroup"  HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLFO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLFOCONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ConsGroup"  HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHFO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHFOCONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="LPG (P)" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ConsGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLPG" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLPGPROPANE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="LPG (B)" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ConsGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLPGB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLPGBUTANE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="LNG" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ConsGroup"  HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLNG" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLNG") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderText="Other" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ConsGroup"  HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOther" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Methanol" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMethanol" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMETHANOL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ethanol" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEthanol" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDETHANOL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Method of <br/> measure cons" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMethodofmeasurecons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELCONSMEASUREMETHOD") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
