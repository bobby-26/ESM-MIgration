<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryStoreTransactionHistory.aspx.cs" Inherits="InventoryStoreTransactionHistory" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Store Transaction</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvStoreEntryDetail.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmStoreInOut" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <div><br clear="all" /></div>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDispositionDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDispositionTodate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTransactiontype" runat="server" Text="Transaction type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlHard ID="ddlDispositionType" RenderMode="Lightweight" runat="server" Width="135px" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromItemNumber" runat="server" Text="From Item Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MaskNumber ID="txtItemNumber" RenderMode="Lightweight" runat="server" Width="135px" CssClass="input" MaskText="##.##.##" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToItemNumber" runat="server" Text="To Item Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MaskNumber ID="txtItemNumberTo" RenderMode="Lightweight" runat="server" Width="135px" CssClass="input" MaskText="##.##.##" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblItemName" runat="server" Text="Item Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtItemName" RenderMode="Lightweight" runat="server" Width="135px" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStoreType" runat="server" Text="Store Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlHard ID="ddlStockClass" runat="server" RenderMode="Lightweight" Visible="true" CssClass="input"
                            AppendDataBoundItems="true" />
                    </td>
                    <td colspan="4"></td>
                </tr>
            </table>
           <div> <br clear="all" /></div>
            <eluc:TabStrip ID="MenuGridStoreInOut" runat="server" OnTabStripCommand="MenuGridStoreInOut_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvStoreEntryDetail" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvStoreEntryDetail_NeedDataSource" AutoGenerateColumns="false"
                OnSortCommand="gvStoreEntryDetail_SortCommand" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings SortedBackColor="#CCE5FF" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Transaction" Name="Transaction" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn ColumnGroupName="Transaction" HeaderText="Date" AllowSorting="true" HeaderStyle-Width="10%" SortExpression="FLDDISPOSITIODATE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStoreItemDispositionHeaderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMDISPOSITIONHEADERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStoreItemDispositionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMDISPOSITIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStoreItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTransactionDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPOSITIODATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ColumnGroupName="Transaction" HeaderText="Type" AllowSorting="true" HeaderStyle-Width="10%" SortExpression="FLDTRANSACTIONTYPENAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTransactionHeader" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRANSACTIONTYPENAME") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item No." AllowSorting="true" SortExpression="FLDNUMBER" HeaderStyle-Width="7%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblnumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item Name" AllowSorting="true" SortExpression="FLDNAME" HeaderStyle-Width="33%">
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Qty" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDispositionQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPOSITIONQUANTITY" ,"{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ROB" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDispositionROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB" ,"{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Price" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDispositionPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEPRICE" ,"{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Number" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormnumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reported By" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReportedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTEDBY") %>'></telerik:RadLabel>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
