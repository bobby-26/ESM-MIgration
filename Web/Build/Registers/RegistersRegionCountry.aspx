<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersRegionCountry.aspx.cs" Inherits="Registers_RegistersRegionCountry" %>


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Region</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuRegionCountry" runat="server" OnTabStripCommand="RegionCountry_TabStripCommand"></eluc:TabStrip>

            <%--<asp:GridView ID="gvregioncountry" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowCommand="gvregioncountry_RowCommand"
                ShowFooter="true" ShowHeader="true" OnRowDataBound="gvregioncountry_ItemDataBound"
                EnableViewState="false">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvregioncountry" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvregioncountry_NeedDataSource"
                OnItemDataBound="gvregioncountry_ItemDataBound"
                OnItemCommand="gvregioncountry_ItemCommand"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false" ShowFooter="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
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
                        <telerik:GridTemplateColumn HeaderText="Country" HeaderStyle-Width="80%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCountrycode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCountry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Country ID="ddlcountrylist" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" CountryList='<%# PhoenixRegistersCountry.ListCountry(null)%>' />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="20%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DEL" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="ADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                    ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </div>
    </form>
</body>
</html>
