<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListPreferredCity.aspx.cs" Inherits="Common_CommonPickListPreferredCity" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>City</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
       <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />        
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="95%">
            <eluc:TabStrip ID="Menucity" runat="server" OnTabStripCommand="Menucity_TabStripCommand"></eluc:TabStrip>      
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>             
                    <table id="tblConfigureCity" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Country ID="ddlcountrylist" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    AutoPostBack="true" OnTextChangedEvent="ddlCountry_Changed" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblState" runat="server" Text="State"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:State runat="server" ID="ucState" AppendDataBoundItems="true" CssClass="input"
                                    OnTextChangedEvent="ucState_Changed" AutoPostBack="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblSearchCity" runat="server" Text="City"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="100" CssClass="input"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>              
                <br />              
                    &nbsp;<b><telerik:RadLabel ID="lblCityList" runat="server" Text="City List"></telerik:RadLabel></b>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCity" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCity_ItemCommand" OnNeedDataSource="gvCity_NeedDataSource" Height="90%"
                OnItemDataBound="gvCity_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true"
                OnSortCommand="gvCity_SortCommand">
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
                   <%-- <asp:GridView ID="gvCity" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvCity_RowCommand" OnRowDataBound="gvCity_ItemDataBound"
                        ShowHeader="true" OnSorting="gvCity_Sorting" AllowSorting="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="City" HeaderStyle-Width="300px" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCITYNAME">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCityid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkCityName" runat="server" CommandName="SELECT" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITYNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="State" HeaderStyle-Width="300px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStateId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCountryName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Country" HeaderStyle-Width="300px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCountryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lnkairportName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                     <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
