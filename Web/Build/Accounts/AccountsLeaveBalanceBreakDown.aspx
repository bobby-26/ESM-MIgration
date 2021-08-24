<%@ Page Language="C#" AutoEventWireup="True" CodeFile="AccountsLeaveBalanceBreakDown.aspx.cs"
    Inherits="AccountsLeaveBalanceBreakDown" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Rank</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function pageLoad() {
                PaneResized();
            }
            function PaneResized(sender, args) {
                var browserHeight = $telerik.$(window).height();
                var grid = $find("gvLVP");
                grid._gridDataDiv.style.height = (browserHeight - 150) + "px";
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmLeaveBreakDown" runat="server">
        <telerik:RadFormDecorator ID="Decorator" runat="server" DecorationZoneID="frmLeaveBreakDown" DecoratedControls="All" />
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuLeaveBreakDown" runat="server" OnTabStripCommand="MenuLeaveBreakDown_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvLVP" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvLVP_NeedDataSource" Width="100%" EnableViewState="false" ShowHeader="true"
                OnItemCommand="gvLVP_ItemCommand" OnItemDataBound="gvLVP_ItemDataBound">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDEMPLOYEEID">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <HeaderStyle HorizontalAlign="Left" Width="14%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Crew Name">
                            <HeaderStyle HorizontalAlign="Left" Width="15%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDEMPLOYEENAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <HeaderStyle HorizontalAlign="Left" Width="12%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Portage Bill Date">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDPBDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Contract Commence Date">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDCOCDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign Off/ ETOD">
                            <HeaderStyle HorizontalAlign="Left" Width="14%" />
                            <ItemTemplate>
                                <%# string.Format("{0:dd-MMM-yyyy}", DataBinder.Eval(Container, "DataItem.FLDLASTSIGNOFF"))%> /
                                <%# string.Format("{0:dd-MMM-yyyy}", DataBinder.Eval(Container, "DataItem.FLDLASTETOD"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Monthly Leave Wages">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDMONTHLYWAGES")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Leave Days">
                            <HeaderStyle HorizontalAlign="Left" Width="6%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDLEAVEDAYS")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Balance Leave Days">
                            <HeaderStyle HorizontalAlign="Left" Width="8%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDBALANCELEAVEDAYS")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="BTB Days">
                            <HeaderStyle HorizontalAlign="Left" Width="8%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDBTBDAYS")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Balance BTB Days">
                            <HeaderStyle HorizontalAlign="Left" Width="8%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDBALANCEBTBDAYS")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" Width="10%" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Allotment Request" ImageUrl="<%$ PhoenixTheme:images/45.png %>"
                                    CommandName="REQUEST" ID="cmdRequest" ToolTip="Allotment Request"></asp:ImageButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <telerik:RadGrid ID="gvLAR" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" Width="100%" CellPadding="3" EnableViewState="false" ShowHeader="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDREQUESTID">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Request No">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDREQUESTNO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Request Date">
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDREQUESTDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Paid From Date">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDFROMDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Days Deducted">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDLEAVEDEDUCTION")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Monthly Leave Wages Amount">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDMONTHLYWAGES")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Leave Amount">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDLEAVEAMOUNT")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Amount">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDPAYMENTAMOUNT")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Exchange Rate">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDEXCHANGERATE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Date">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemTemplate>
                                <%# string.Format("{0:dd/MMM/yyyy}", DataBinder.Eval(Container, "DataItem.FLDPAYMENTDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Reference">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDPAYMENTREFERENCE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
