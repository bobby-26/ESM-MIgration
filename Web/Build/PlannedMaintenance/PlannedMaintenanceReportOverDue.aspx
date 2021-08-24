<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceReportOverDue.aspx.cs"
    Inherits="PlannedMaintenanceReportOverDue" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmReportOverdue" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                <eluc:TabStrip ID="MenuPrintReportOverdue" runat="server" OnTabStripCommand="MenuPrintReportOverdue_TabStripCommand"></eluc:TabStrip>
            </telerik:RadCodeBlock>

            <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
            </telerik:RadWindowManager>

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr style="height: 5px;">
                    <td colspan="6">&nbsp;</td>
                </tr>
                <tr>
                    <td style="width: 9%; vertical-align: top">
                        <asp:Literal ID="lblDateBetween" runat="server" Text="Date Between"></asp:Literal>
                    </td>
                    <td style="width: 13%; vertical-align: top">
                        <eluc:Date runat="server" ID="txtDateFrom" CssClass="input" />
                    </td>
                    <td style="width: 1%; vertical-align: top"><b>-</b>
                    </td>
                    <td style="width: 13%; vertical-align: top; align-items: ">
                        <eluc:Date runat="server" ID="txtDateTo" CssClass="input" />
                    </td>
                    <td style="width: 4%; vertical-align: top">
                        <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                    </td>
                    <td style="width: 10%; vertical-align: top">
                        <telerik:RadDropDownList ID="drdwnFleet" runat="server" AutoPostBack="true" Width="100px"
                            CssClass="input" DataTextField="FLDFLEETDESCRIPTION"
                            DataValueField="FLDFLEETID"
                            OnSelectedIndexChanged="drdwnFleet_SelectedIndexChanged"
                            OnDataBound="drdwnFleet_DataBound">
                        </telerik:RadDropDownList>
                    </td>
                    <td style="width: 4%; vertical-align: top">
                        <asp:Literal ID="lblVessels" runat="server" Text="Vessels"></asp:Literal>
                    </td>
                    <td style="width: 43%; vertical-align: top">
                        <div runat="server" id="dvClass" class="input" style="overflow: auto; width: 60%; height: 100px;">
                            <telerik:RadCheckBoxList ID="chkVessels" runat="server" DataBindings-DataTextField="FLDVESSELNAME" DataBindings-DataValueField="FLDVESSELID"></telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>

            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="RadGrid1" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource" OnPreRender="RadGrid1_PreRender"
                OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand" AutoGenerateColumns="false">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">

                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="false" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No of due">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDNOOFDUE") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="No of Work Orders">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDNOOFWO") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Overdue">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPercent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Overdue Critical Jobs">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblCritcal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFCRITIACLDUE") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Overdue > 30 days">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblExceed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOOFDUEEXCEED30DAYS") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unplanned Jobs" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDNOOFUNPLANNED") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <%--       <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Cities matching your search criteria"
                        PageSizeLabelText="Cities per page:" />--%>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <%--<Virtualization EnableVirtualization="true" InitiallyCachedItemsCount="100" LoadingPanelID="RadAjaxLoadingPanel1" ItemsPerView="100" />--%>
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
                <%-- <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>--%>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
