<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCommittedCommitmentspost.aspx.cs"
    Inherits="Accounts_AccountsCommittedCommitmentspost" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselAccount" Src="~/UserControls/UserControlVesselAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Accounts Committed Cost post</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoice" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>

        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="Error1" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />

                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

                <eluc:TabStrip ID="MenuCommittedcostpostTab" runat="server" TabStrip="false" OnTabStripCommand="CommittedcostsubacTab_TabStripCommand"></eluc:TabStrip>

                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblVesselAccountSetup" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVesselAccountCode" runat="server" Text="Vessel Account Code"></telerik:RadLabel>
                            </td>
                            <td>
                                <td>
                                    <telerik:RadComboBox runat="server" ID="ucVesselAccount" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                        AutoPostBack="true" OnSelectedIndexChanged="ucVesselAccount_SelectedIndexChanged" Filter="Contains" EmptyMessage="Type to select">
                                        <Items>
                                            <telerik:RadComboBoxItem Text="--Select--" Value="" Selected="True"></telerik:RadComboBoxItem>
                                        </Items>

                                    </telerik:RadComboBox>
                                </td>
                            </td>
                            <td>As on Date        
                            </td>
                            <td>
                                <eluc:UserControlDate ID="ucAsOnDate" runat="server" CssClass="input_mandatory" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />

                <%--<eluc:TabStrip ID="MenuCommittedcostsubacTab" runat="server" TabStrip="false" OnTabStripCommand="CommittedcostsubacTab_TabStripCommand"></eluc:TabStrip>--%>

                <%--  <asp:GridView ID="gvcommitedcostposted" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                    Width="100%" CellPadding="3" Style="margin-bottom: 0px"
                    OnRowCommand="gvcommitedcostposted_RowCommand" OnRowDataBound="gvcommitedcostposted_ItemDataBound"
                    OnRowCreated="gvcommitedcostposted_RowCreated" OnRowEditing="gvcommitedcostposted_edit" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle" />
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvcommitedcostposted" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvcommitedcostposted_NeedDataSource"
                    OnEditCommand="gvcommitedcostposted_EditCommand"
                    OnItemCommand="gvcommitedcostposted_ItemCommand"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
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
                            <telerik:GridTemplateColumn HeaderText=" Sub A/C">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSubAcCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblBudgetId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkSubAcCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>' CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex%>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Owner Budget Code">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETGROUP") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadLabel ID="lblTotal" runat="server" Visible="true" Text="Total:"></telerik:RadLabel>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reversed (USD)">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReversedcharge" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTINUSD") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%=dSumReversed%>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Charged (USD)">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblcharged" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHARGED") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <%=dSumCharged%>
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


                <br />
                <br />

                <eluc:TabStrip ID="MenucommitedcostPo" runat="server" OnTabStripCommand="MenucommitedcostPo_TabStripCommand"></eluc:TabStrip>

                <%-- <asp:GridView ID="gvcommitedcostPoSearch" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                    Width="100%" CellPadding="3" Style="margin-bottom: 0px" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle" />
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvcommitedcostPoSearch" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvcommitedcostPoSearch_NeedDataSource"
                    OnItemCommand="gvcommitedcostPoSearch_ItemCommand"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
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
                            <telerik:GridTemplateColumn HeaderText="PO Number">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPoNumber" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPONUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Supplier">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSupplier" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Sub A/C">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSubAcNo" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="   Owner Budget Code">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBudgetCode" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Committed Date">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbldatetoapprove" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFAPPROVAL") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Amount(USD)">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblamtusd" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTINUSD") %>'></telerik:RadLabel>
                                </ItemTemplate>
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

                <br />
                <br />
                <br />

                <%--<eluc:TabStrip ID="TabStrip1" runat="server" OnTabStripCommand="MenucommitedcostPo_TabStripCommand"></eluc:TabStrip>--%>

                <%-- <asp:GridView ID="Gvcommittedcostvouchers" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                    Width="100%" CellPadding="3" Style="margin-bottom: 0px" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle" />
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="Gvcommittedcostvouchers" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="Gvcommittedcostvouchers_NeedDataSource"
                    OnItemCommand="Gvcommittedcostvouchers_ItemCommand"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
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
                            <telerik:GridTemplateColumn HeaderText="Vessel">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Account Code">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAccountCode" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Date">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reversed(USD)">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReversed" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTINUSD") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText=" Charged(USD)">
                                <HeaderStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCharged" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHARGED") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Voucher Number">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVoucherNumber" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
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

                <br />
                <br />
            </div>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>

