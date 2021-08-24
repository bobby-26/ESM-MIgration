<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListWorkOrderCompleted.aspx.cs" Inherits="CommonPickListWorkOrderCompleted" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Work Order List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuOrder" runat="server" OnTabStripCommand="MenuOrder_TabStripCommand"></eluc:TabStrip>
            </div>
            <br clear="all" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="search">
                <table>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNumber" runat="server" Text="Work Order Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtWorkOrderNumber" CssClass="input" runat="server" Text=""></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDescription" runat="server" Text="Title"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" CssClass="input" ID="txtTitle" Text=""></telerik:RadTextBox>
                        </td>

                    </tr>
                </table>
            </div>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvOrder" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOrder" runat="server" Height="80%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableViewState="false"
                CellSpacing="0" GridLines="None" Font-Size="11px" OnItemCommand="gvOrder_ItemCommand" OnSortCommand="gvOrder_SortCommand"
                OnNeedDataSource="gvOrder_NeedDataSource" ShowFooter="False" ShowHeader="true" Width="100%" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Work order Number" AllowSorting="true" SortExpression="FLDCOMPONENTNUMBER">
                            <HeaderStyle Width="8%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblWorkOrderId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblWorkOrderNumber" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkWorkOrderNumber" runat="server" CommandName="PICKLIST"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Work Order Description" AllowSorting="true" SortExpression="FLDNAME">
                            <HeaderStyle Width="22%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'
                                    Visible="true">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Number" AllowSorting="true" SortExpression="FLDNAME">
                            <HeaderStyle Width="22%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponentNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'
                                    Visible="true">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component" AllowSorting="true" SortExpression="FLDNAME">
                            <HeaderStyle Width="22%" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'
                                    Visible="true">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" />
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