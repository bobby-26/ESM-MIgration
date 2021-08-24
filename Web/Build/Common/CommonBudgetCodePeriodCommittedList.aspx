<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonBudgetCodePeriodCommittedList.aspx.cs" Inherits="CommonBudgetCodePeriodCommittedList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Budget Code Allocation</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCommonBudgetGroupAllocation" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPeriod" runat="server" Text="Period"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPeriodName" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtYear" runat="server" CssClass="readonlytextbox txtnumber" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbl" runat="server" Text="Budget Group"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBudgetGroup" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>


            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuCommonBudgetGroupAllocation" runat="server" OnTabStripCommand="CommonBudgetGroupAllocation_TabStripCommand"></eluc:TabStrip>


            <telerik:RadGrid RenderMode="Lightweight" ID="gvBudgetCodeAllocation" Height="85%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvBudgetCodeAllocation_ItemCommand" OnItemDataBound="gvBudgetCodeAllocation_ItemDataBound" OnNeedDataSource="gvBudgetCodeAllocation_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDBUDGETCODEPERIODALLOCATIONID">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>

                        <%-- <asp:GridView ID="gvBudgetCodeAllocation" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" GridLines="None" Width="100%" CellPadding="3" 
                    AllowSorting="true" ShowFooter="false" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDBUDGETCODEPERIODALLOCATIONID">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />--%>

                        <telerik:GridTemplateColumn HeaderText="Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBudgetCodeItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Committed">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCommittedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMITTEDAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Paid">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaidAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAIDAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Total">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALEXPENDITURE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Budget Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Allowance">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAllowance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOWANCE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Variance">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVarianceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTHLYVARIANCE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Mngt Variance">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMngtVarianceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANAGEMENTVARIANCE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>


                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
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
