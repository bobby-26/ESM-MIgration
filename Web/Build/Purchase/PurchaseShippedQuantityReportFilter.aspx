<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseShippedQuantityReportFilter.aspx.cs" Inherits="PurchaseShippedQuantityReportFilter" %>


<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCheckBoxList.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Shipped Quantity</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="80%">
        
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
<%--        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />--%>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblUserAccess" runat="server" Text="Form Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtfromnumber" runat="server"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel runat="server" Text="Shipped From Date" />
                </td>
                <td>
                    <eluc:Date runat="server" ID="radfromdate" />
                </td>
                <td>
                    <telerik:RadLabel runat="server" Text="Shipped To Date" />
                </td>
                <td>
                    <eluc:Date runat="server" ID="radtodate" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="Vessel List"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" VesselsOnly="true"  Width="240px" />
                </td>
                <td>
                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="Vendor"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListMaker">
                        <telerik:RadTextBox ID="txtVendorCode" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVendorName" runat="server" Width="240px" CssClass="input"></telerik:RadTextBox>
                        <asp:TextBox ID="txtVendorId" runat="server" Width="0px" CssClass="input" BorderStyle="None"></asp:TextBox>
                        <asp:ImageButton runat="server" ID="cmdShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx', true);"
                            Text=".." />
                    </span>
                </td>
            </tr>


        </table>
        <eluc:TabStrip ID="MenuPhoenixQuery" runat="server" OnTabStripCommand="MenuPhoenixQuery_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvQuery" runat="server" AllowCustomPaging="true" AllowSorting="true"
            AllowPaging="true" CellSpacing="0" GridLines="None" EnableViewState="true" Height="92%" GroupingEnabled="false" EnableHeaderContextMenu="true"
            AutoGenerateColumns="false" OnNeedDataSource="gvQuery_NeedDataSource" OnItemCommand="gvQuery_ItemCommand">
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
                    <telerik:GridTemplateColumn HeaderText="Form Number" AllowFiltering="false" HeaderStyle-Width="40%">
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="true" />
                        <ItemStyle HorizontalAlign="Left" />

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblfomno" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFORMNUMBER")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Vessel Name" AllowFiltering="false" HeaderStyle-Width="50%">
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="true" />
                        <ItemStyle HorizontalAlign="Left" />

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblvesselname" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vendor" AllowFiltering="false" HeaderStyle-Width="50%">
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="true" />
                        <ItemStyle HorizontalAlign="Left" />

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblvendorname" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVENDORNAME")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Shipped Date" AllowFiltering="false" HeaderStyle-Width="40%">
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="true" />
                        <ItemStyle HorizontalAlign="Left" />

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblshippeddate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSHIPPEDDDATE")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="No of Days lying with Forwarder" AllowFiltering="false" HeaderStyle-Width="30%">
                        <HeaderStyle HorizontalAlign="Right" Font-Bold="true" />
                        <ItemStyle HorizontalAlign="Right" />

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblshppedagingdays" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSHIPPEDAGINGDAYS")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="PO Total by Shipped Qty (USD)" AllowFiltering="false" HeaderStyle-Width="30%">
                        <HeaderStyle HorizontalAlign="Right" Font-Bold="true" />
                        <ItemStyle HorizontalAlign="Right" />

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblpototal" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPOAMOUNTBYSHIPPEDQYT")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Invoice Number" AllowFiltering="false" HeaderStyle-Width="50%">
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="true" />
                        <ItemStyle HorizontalAlign="Left" />

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblinvoiceno" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDINVOICENUMBER")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Invoice Date" AllowFiltering="false" HeaderStyle-Width="40%">
                        <HeaderStyle HorizontalAlign="Left" Font-Bold="true" />
                        <ItemStyle HorizontalAlign="Left" />

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblinvoicedate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDINVOICEDATE")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                    PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />

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
