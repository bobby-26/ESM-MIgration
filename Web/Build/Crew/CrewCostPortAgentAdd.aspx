<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCostPortAgentAdd.aspx.cs"
    Inherits="CrewCostPortAgentAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Port Agent List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:TabStrip ID="MenuAddress" runat="server" OnTabStripCommand="MenuAddress_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtNameSearch" runat="server" Text=""></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCode" Text=""></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCountryNameSearch" Text=""></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAddress" runat="server" EnableViewState="false" Height="95%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvAddress_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvAddress_ItemDataBound"
                OnItemCommand="gvAddress_ItemCommand" OnSortCommand="gvAddress_SortCommand" ShowFooter="false"
                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNAME" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblAddressCode" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSCODE") %>' Visible="false">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkAddressName" runat="server" CommandArgument='<%# Container.DataSetIndex %>' CommandName="SELECT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Phone 1" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPhone1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHONE1") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="City" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="State" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblState" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Country" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCountry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
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
