<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventorySpareTransactionHistory.aspx.cs" Inherits="InventorySpareTransactionHistory" %>

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
    <title>Spare Transaction</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvSpareEntryDetail.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmStoreInOut" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
        <div><br clear="all" /></div>
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
                    <eluc:UserControlHard ID="ddlDispositionType" runat="server" RenderMode="Lightweight" CssClass="input" AppendDataBoundItems="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPartFromNumber" runat="server" Text="Part From Number"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MaskNumber ID="txtItemNumber" RenderMode="Lightweight" runat="server" Width="135px" CssClass="input" MaskText="###.##.##.###" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPartToNumber" runat="server" Text="Part To Number"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MaskNumber ID="txtItemNumberTo" RenderMode="Lightweight" runat="server" Width="135px" CssClass="input" MaskText="###.##.##.###" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblItemName" runat="server" Text="Item Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtItemName" RenderMode="Lightweight" runat="server" Width="160px" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblComponentFromNumber" runat="server" Text="Component From Number"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MaskNumber ID="txtComponentNumber" RenderMode="Lightweight" runat="server" Width="135px" CssClass="input" MaskText="###.##.##" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblComponentToNumber" runat="server" Text="Component To Number"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MaskNumber ID="txtComponentNumberTo" RenderMode="Lightweight" runat="server" Width="135px" CssClass="input" MaskText="###.##.##" />
                </td>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblComponentName" runat="server" Text="Component Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtComponentName" RenderMode="Lightweight" runat="server" Width="160px" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblWorkorderNumber" runat="server" Text="Work order Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtWorkOrderNo" RenderMode="Lightweight" runat="server" Width="135px" CssClass="input"></telerik:RadTextBox>
                </td>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblCritical" runat="server" Text="Critical"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkCritical" RenderMode="Lightweight" runat="server"></telerik:RadCheckBox>
                </td>
                <td style="width: 15%"></td>
                <td></td>
            </tr>
        </table>
        <div><br clear="all" /></div>
            <eluc:TabStrip ID="MenuGridStoreInOut" runat="server" OnTabStripCommand="MenuGridStoreInOut_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvSpareEntryDetail" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvSpareEntryDetail_NeedDataSource" AutoGenerateColumns="false" 
                OnSortCommand="gvSpareEntryDetail_SortCommand">
                <SortingSettings SortedBackColor="#CCE5FF" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Transaction" Name="Transaction" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="13%">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Component" Name="Component" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="20%">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn ColumnGroupName="Transaction" HeaderText="Date" HeaderStyle-Wrap="false" HeaderStyle-Width="7%" AllowSorting="true" SortExpression="FLDDISPOSITIODATE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSpareItemDispositionHeaderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMDISPOSITIONHEADERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSpareItemDispositionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMDISPOSITIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSpareItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPAREITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTransactionDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPOSITIODATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ColumnGroupName="Transaction" HeaderText="Type" HeaderStyle-Wrap="false" HeaderStyle-Width="6%" AllowSorting="true" SortExpression="FLDTRANSACTIONTYPENAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTransactionType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRANSACTIONTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item No." HeaderStyle-Wrap="false" HeaderStyle-Width="8%" AllowSorting="true" SortExpression="FLDNUMBER">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblnumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item Name" HeaderStyle-Wrap="false" HeaderStyle-Width="15%" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNAME">
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ColumnGroupName="Component" HeaderText="Number" HeaderStyle-Wrap="false" HeaderStyle-Width="8%" AllowSorting="true" SortExpression="FLDCOMPONENTNUMBER">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ColumnGroupName="Component" HeaderText="Name" HeaderStyle-Wrap="false" HeaderStyle-Width="12%" AllowSorting="true" SortExpression="FLDCOMPONENTNAME">
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Qty" HeaderStyle-Wrap="false" HeaderStyle-Width="4%" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDispositionQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPOSITIONQUANTITY" ,"{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ROB" HeaderStyle-Wrap="false" HeaderStyle-Width="4%" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDispositionROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB" ,"{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Price" HeaderStyle-Wrap="false" HeaderStyle-Width="8%" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDispositionPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEPRICE" ,"{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order No." HeaderStyle-Wrap="false" HeaderStyle-Width="9%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormnumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Work Order" HeaderStyle-Wrap="false" HeaderStyle-Width="9%" AllowSorting="true" SortExpression="FLDWORKORDERTYPENAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reported By" HeaderStyle-Wrap="false" HeaderStyle-Width="10%">
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="true"
                        PageSizeLabelText="Records per page:" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
