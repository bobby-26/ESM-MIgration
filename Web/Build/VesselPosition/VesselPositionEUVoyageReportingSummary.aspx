<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionEUVoyageReportingSummary.aspx.cs" Inherits="VesselPositionEUVoyageReportingSummary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Owner" Src="~/UserControls/UserControlAddressOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Departure Report..</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvConsumption.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
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

        <telerik:RadAjaxPanel runat="server" ID="panel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuDepartureReport" runat="server" OnTabStripCommand="MenuDepartureReport_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <table width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblmVesselName" runat="server" Text="Vessel"></asp:Literal>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="input" Width="180px" AppendDataBoundItems="true" AutoPostBack="true" SyncActiveVesselsOnly="True" AssignedVessels="true" OnTextChangedEvent="UcVessel_OnTextChangedEvent" VesselsOnly="true" />
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <asp:Literal ID="lblPort" runat="server" Text="Port"></asp:Literal>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <eluc:Multiport ID="ucPortMulti" runat="server" CssClass="input" Width="300px" />
                    </td>
                    <td>
                        <asp:Literal ID="lblCommencedFromDate" runat="server" Text="Commenced From - To"></asp:Literal>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtCommencedFrom" CssClass="input" DatePicker="true" />
                        <asp:Literal ID="lblCommencedTo" runat="server" Text=" - "></asp:Literal>
                        <eluc:Date runat="server" ID="txtCommencedTo" CssClass="input" DatePicker="true" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal></td>
                    <td>
                        <eluc:Fleet runat="server" ID="ucFleet" Width="180px" CssClass="input" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucFleet_OnTextChangedEvent" />
                    </td>
                    <td>
                        <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Owner runat="server" ID="ucOwner" CssClass="input" AddressType="128" Width="300px" AutoPostBack="true" OnTextChangedEvent="ucOwner_OnTextChangedEvent"
                            AppendDataBoundItems="true" Enabled="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblCompletedFrom" runat="server" Text="Completed From - To"></asp:Literal>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtCompletedFrom" CssClass="input" DatePicker="true" />
                        <asp:Literal ID="lblCompletedTo" runat="server" Text=" - "></asp:Literal>
                        <eluc:Date runat="server" ID="txtCompletedTo" CssClass="input" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal runat="server" ID="lblShowNonEU" Text="Show All Voyages"></asp:Literal>
                    </td>
                    <td>
                        <asp:CheckBox runat="server" ID="chkShowNonEU" />
                    </td>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuCrewCourseList" runat="server" OnTabStripCommand="CrewCourseList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvConsumption" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                CellPadding="3" AllowSorting="true" ShowFooter="false" ShowHeader="true" EnableViewState="false"
                OnItemCommand="gvConsumption_RowCommand" OnItemDataBound="gvConsumption_ItemDataBound" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnNeedDataSource="gvConsumption_NeedDataSource" AllowCustomPaging="true" AllowPaging="true">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDID">
                    <NoRecordsTemplate>
                        <table  width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Average CO₂ Emissions" Name="Group" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="17%">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="EU Voy" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkEUVoyYN" runat="server" AutoPostBack="true" OnCheckedChanged="chkEUVoyYN_CheckedChanged" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Voy No" HeaderStyle-Wrap="false" HeaderStyle-Width="70px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVoyNoItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOYAGENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From" HeaderStyle-Wrap="false" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFromtem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROM") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To" HeaderStyle-Wrap="false" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ballast/<br>Laden" HeaderStyle-Wrap="false" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBallastLadenItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALLASTRLADEN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Commenced" HeaderStyle-Wrap="false" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCommencedItem" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMMENCED")) + " " + DataBinder.Eval(Container,"DataItem.FLDCOMMENCED", "{0:HH:mm}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed" HeaderStyle-Wrap="false" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletedItem" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETED")) + " " + DataBinder.Eval(Container,"DataItem.FLDCOMPLETED", "{0:HH:mm}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Report<br>Type" HeaderStyle-Wrap="false" HeaderStyle-Width="60px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReportTypeItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Distance<br>(nm)" HeaderStyle-Wrap="false" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDistanceItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISTANCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Time<br>At Sea" HeaderStyle-Wrap="false" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTimeAtSeaItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTIMEATSEA") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Anchorage<br>Time" HeaderStyle-Wrap="false" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAnchorageItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANCHORAGETIME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cargo<br>Qty(MT)" HeaderStyle-Wrap="false" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCargoQtyItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOQTYONARRIVAL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="HFO<br>Cons(MT)" HeaderStyle-Wrap="false" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHFOItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHFOCONSATSEA") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="MDO/MGO<br>(MT)" HeaderStyle-Wrap="false" HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMDOItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMDOCONSATSEA") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Transport<br>Work (T-nm)" HeaderStyle-Wrap="false" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTransportWorkItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRANSPORTWORK") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Agg CO₂<br>Emitted<br>(T-CO₂)" HeaderStyle-Wrap="false" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAggCo2EmittedItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCO2EMISSIONATSEA") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Per Dist<br>(Kg CO₂/nm)" ColumnGroupName="Group" HeaderStyle-Wrap="false" HeaderStyle-Width="80px">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPerDistco2Item" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCO2EMISSIONPERDIST") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Per Transport<br>Work (g CO₂/T-nm)" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="Group" HeaderStyle-Width="120px" HeaderStyle-Wrap="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPerTransportWorkItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCO2EMISSIONPERTRANSPORTWORK") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Wrap="false" HeaderStyle-Width="50px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
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
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
