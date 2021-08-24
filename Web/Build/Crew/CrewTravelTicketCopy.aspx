<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelTicketCopy.aspx.cs"
    Inherits="CrewTravelTicketCopy" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ticket</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmComment" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <eluc:TabStrip ID="MenuComment" runat="server" Title="Ticket" OnTabStripCommand="MenuComment_TabStripCommand"></eluc:TabStrip>

            <table id="note" runat="server" style="color: Blue" visible="false">
                <tr>
                    <td>Note : Click 'New'to paste the Ticket and cilck 'Save'.
                    </td>
                </tr>
            </table>
            <telerik:RadTextBox ID="txtComment" runat="server" CssClass="input" TextMode="MultiLine"
                Visible="false" Wrap="false" Height="100%" Width="100%">
            </telerik:RadTextBox>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTicketList" runat="server" AllowCustomPaging="true" AllowSorting="true" Height="99%"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvTicketList_NeedDataSource" AllowPaging="false" EnableViewState="false"
                GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTicketCopy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTICKETCOPY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <%--   <asp:Repeater runat="server" ID="repTicketList">
            <ItemTemplate>
                <telerik:RadTextBox ID="txtComment" runat="server" TextMode="MultiLine" BorderColor="black"
                    Text='<%#DataBinder.Eval(Container, "DataItem.FLDTICKETCOPY")%>' Wrap="false"
                    Height="100%" Width="100%" ReadOnly="true"></telerik:RadTextBox>
                <br />
            </ItemTemplate>
        </asp:Repeater>--%>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
