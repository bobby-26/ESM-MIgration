<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseOrderLineItemSelectService.aspx.cs"
    Inherits="PurchaseOrderLineItemSelectService" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ComponentTreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlPurchaseUnit.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stock Item</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function PaneResized(sender, args) {

                var grid = $find("gvItemList");
                var maintbl = document.getElementById("tblmain");
                var subtbl = document.getElementById("details");
                grid._gridDataDiv.style.height = (maintbl.offsetHeight - 150) + "px";

            }
            function rowClick(sender, eventArgs) {
                sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
            }
            function pageLoad() {
                PaneResized();
            }
        </script>
    </telerik:RadCodeBlock>

</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>

        <eluc:TabStrip ID="MenuStoreItemInOutTransaction" Title="Stock Item" runat="server" OnTabStripCommand="StoreItemInOutTransaction_TabStripCommand"></eluc:TabStrip>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadAjaxPanel runat="server" ID="pnlStoreItemInOutTransaction" Height="93%">
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <table cellpadding="0" cellspacing="0" style="float: left; width: 100%; height: 100%" id="tblmain">
                <tr style="position: relative; vertical-align: top">
                    <td width="10%">
                        <div id="divComponentTree" runat="server" style="height: 100%; overflow-y:auto; width: 300px; overflow:auto;">
                            <eluc:ComponentTreeView ID="tvwComponent" runat="server" OnNodeClickEvent="ucTree_SelectNodeEvent" />
                            <telerik:RadLabel runat="server" ID="lblSelectedNode"></telerik:RadLabel>
                        </div>
                    </td>
                    <td width="90%">
                        <table cellpadding="0" cellspacing="0" width="100%" style="height: 100%;" id="details">
                            <tr style="height:10%">

                                <td>
                                    <table width="100%">
                                        <tr>

                                            <td>
                                                <telerik:RadLabel ID="lblWorkOrderNumber" runat="server" Text="Job Number"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtPartNumber" runat="server" CssClass="input"></telerik:RadTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblWorkOrderName" runat="server" Text="Job Title"></telerik:RadLabel>

                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtItemName" runat="server" CssClass="input"></telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="ltrlComponentNo" runat="server" Text="Component No"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtComponentNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox ID="txtComponent" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="height:90%">
                                <td colspan="4" style="vertical-align:top;">
                                    <telerik:RadGrid ID="gvItemList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="3" OnItemCommand="gvItemList_RowCommand" OnItemCreated="gvItemList_ItemDataBound"
                                        OnSortCommand="gvItemList_Sorting" AllowSorting="true" OnNeedDataSource="gvItemList_NeedDataSource"
                                        ShowFooter="false" ShowHeader="true" EnableViewState="false" OnUpdateCommand="gvItemList_RowUpdating"
                                        AllowPaging="true" AllowCustomPaging="true">
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID">
                                            <HeaderStyle Width="102px" />
                                            <NoRecordsTemplate>
                                                <table width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Job Number" SortExpression="FLDWORKORDERNUMBER" HeaderStyle-Width="50px">
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" Width="50px" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel runat="server" ID="lblWorkOrderId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel runat="server" ID="lblWorkOrderNumber" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <asp:LinkButton ID="lnkWorkOrderNumber" runat="server" CommandArgument='<%# Container.DataItem %>'
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Job Title">
                                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'
                                                            Visible="true">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblComponentid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="Unit" HeaderStyle-Width="50px">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="txtUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:Unit ID="ucUnit" AppendDataBoundItems="true" CssClass="input_mandatory" runat="server" Width="98%"
                                                            SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>'
                                                            PurchaseUnitList="<%# PhoenixRegistersUnit.ListPurchaseUnit(null,null,0) %>" />
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Quantity" HeaderStyle-Width="50px">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="50px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="txtQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY","{0:n2}") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <telerik:RadLabel ID="lblOrderLineId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadTextBox ID="txtQuantityEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY","{0:n2}") %>'
                                                            CssClass="gridinput txtNumber" Width="60">
                                                        </telerik:RadTextBox>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="40px">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="40px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                            CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                                            ToolTip="Edit"></asp:ImageButton>

                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                            CommandName="Update" CommandArgument="<%# Container.DataItem %>" ID="cmdUpdate"
                                                            ToolTip="Update"></asp:ImageButton>
                                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                            CommandName="Cancel" CommandArgument="<%# Container.DataItem %>" ID="cmdCancel"
                                                            ToolTip="Cancel"></asp:ImageButton>
                                                    </EditItemTemplate>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                                PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                            <ClientEvents OnRowClick="rowClick" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
