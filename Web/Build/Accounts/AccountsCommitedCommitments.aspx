<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCommitedCommitments.aspx.cs"
    Inherits="Accounts_AccountsCommitedCommitments" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">

            function PaneResized(sender, args) {
                var splitter = $find('RadSplitter1');
                var browserHeight = $telerik.$(window).height();
                splitter.set_height(browserHeight - 10);
                splitter.set_width("100%");
                var grid = $find("gvAdvancePayment");
                var contentPane = splitter.getPaneById("listPane");
                grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 140) + "px";
                //console.log(grid._gridDataDiv.style.height, contentPane._contentElement.offsetHeight);
            }
            function pageLoad() {
                PaneResized();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>

        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />


        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
            <telerik:RadPane ID="generalPane" runat="server" Scrolling="None">
                <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 600px; width: 100%" frameborder="0"></iframe>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both"></telerik:RadSplitBar>
            <telerik:RadPane ID="listPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>


                <%-- <asp:GridView ID="gvAdvancePayment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowCommand="gvAdvancePayment_RowCommand" OnRowDataBound="gvAdvancePayment_ItemDataBound"
                    OnRowCancelingEdit="gvAdvancePayment_RowCancelingEdit" OnRowDeleting="gvAdvancePayment_RowDeleting"
                    AllowSorting="true" OnRowEditing="gvAdvancePayment_RowEditing" EnableViewState="false"
                    OnSorting="gvAdvancePayment_Sorting" OnSelectedIndexChanging="gvAdvancePayment_SelectedIndexChanging"
                    DataKeyNames="FLDCOMMITTEDCOSTBREAKUPID">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                    <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvAdvancePayment" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvAdvancePayment_NeedDataSource"
                    OnItemCommand="gvAdvancePayment_ItemCommand"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDCOMMITTEDCOSTBREAKUPID" CommandItemDisplay="Top">
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                        <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="100px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Vessel Account" HeaderStyle-Width="80px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVesselAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELACCOUNT") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Supplier Name" HeaderStyle-Width="150px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="PO Number" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPoNumber" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMITTEDCOSTBREAKUPID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkPoNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPONUMBER") %>'
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex%>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Order Date" HeaderStyle-Width="80px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOrderedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERDATE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Prime Currency" HeaderStyle-Width="80px">
                               <ItemTemplate>
                                    <telerik:RadLabel ID="lblPrimecurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Prime Amount" HeaderStyle-Width="80px">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPrimeamount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRIMEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Amount(USD)" HeaderStyle-Width="80px">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Sub A/C" HeaderStyle-Width="80px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSubAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Owner Budget Code" HeaderStyle-Width="80px">
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text=""></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOwnerBudgetCodeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUP") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Committed Date" HeaderStyle-Width="80px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCommittedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMITTEDDATE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Excluded Date" HeaderStyle-Width="80px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblExcludedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCLUDEDDATE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reversed Date" HeaderStyle-Width="80px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReversedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVERSEDDATE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Invoice Status" HeaderStyle-Width="80px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblInvoiceStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESTATUS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Purchase Invoice Number" HeaderStyle-Width="150px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPurchaseInvNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEINVNO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Project Code" HeaderStyle-Width="90px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblProjectcode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROJECTCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Po Type" HeaderStyle-Width="80px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPoType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOTYPE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="PO Description" HeaderStyle-Width="150px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPODescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPODESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="PO Status" HeaderStyle-Width="100px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPOStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSTATUS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" EnableNextPrevFrozenColumns="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

            </telerik:RadPane>
        </telerik:RadSplitter>

    </form>
</body>
</html>
