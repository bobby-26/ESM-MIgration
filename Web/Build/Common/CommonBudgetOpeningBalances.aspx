<%@ Page Language="C#" EnableEventValidation="false" AutoEventWireup="true" CodeFile="CommonBudgetOpeningBalances.aspx.cs" Inherits="CommonBudgetOpeningBalances" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvFinancialYearSetup.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVesselFinancialYearSetup" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Button runat="server" ID="cmdHiddenSubmit" />

            <eluc:TabStrip ID="MenuBudgetTab" runat="server" TabStrip="false" OnTabStripCommand="BudgetTab_TabStripCommand"></eluc:TabStrip>

            <div id="divFind" style="position: relative; z-index: 2">
                <table id="tblFinancialYearSetup" width="100%">
                    <tr>
                        <td>Vessel Account
                        </td>
                        <td>
                            <telerik:RadComboBox runat="server" ID="ucVesselAccount" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                AutoPostBack="true" OnSelectedIndexChanged="ucVesselAccount_SelectedIndexChanged" Width="250px" Filter="Contains"
                                EmptyMessage="Type to select Vessel Account">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="" Selected="True" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td>Vessel Financial Year Start Date        
                        </td>
                        <td>
                            <eluc:UserControlDate ID="ucFinancialYearStart" runat="server" CssClass="input" DatePicker="false" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>Owner Reporting Phoenix Start Date
                        </td>
                        <td>
                            <eluc:UserControlDate ID="ucOwnerReportingStart" runat="server" CssClass="input_mandatory" DatePicker="false" />
                        </td>
                        <td>Committed Costs Included in Opening Balances
                        </td>
                        <td>
                            <telerik:RadCheckBox ID="chkCommittedCostsIncluded" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>

            <eluc:TabStrip ID="MenuFinancialYearSetup" runat="server" OnTabStripCommand="FinancialYearSetup_TabStripCommand"></eluc:TabStrip>

            <div id="divGrid" style="position: relative;">
                <%--   <asp:GridView ID="gvFinancialYearSetup" runat="server" AutoGenerateColumns="False"
                    CellPadding="3" Font-Size="11px" OnRowCommand="gvFinancialYearSetup_RowCommand"
                    OnRowDataBound="gvFinancialYearSetup_ItemDataBound" OnRowDeleting="gvFinancialYearSetup_RowDeleting"
                    OnSorting="gvFinancialYearSetup_Sorting" OnRowEditing="gvFinancialYearSetup_RowEditing"
                    OnRowCancelingEdit="gvFinancialYearSetup_RowCancelingEdit" OnRowCreated="gvFinancialYearSetup_RowCreated"
                    OnRowUpdating="gvFinancialYearSetup_RowUpdating"
                    AllowSorting="true" ShowFooter="True" Style="margin-bottom: 0px" Width="100%"
                    EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle" />
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvFinancialYearSetup" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvFinancialYearSetup_NeedDataSource"
                    OnItemCommand="gvFinancialYearSetup_ItemCommand"
                    OnItemDataBound="gvFinancialYearSetup_ItemDataBound"
                    OnSortCommand="gvFinancialYearSetup_SortCommand"
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
                        <HeaderStyle Width="75px" Font-Size="Smaller" />
                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Since Take Over As On Phoenix Start Date Accumulated" Name="Part1" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                             <telerik:GridColumnGroup HeaderText="Vessel YTD As On Phoenix Start Date YTD" Name="Part2" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>

                            <telerik:GridTemplateColumn HeaderText=" Vessel Account" HeaderStyle-Width="150px">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVesselAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELACCOUNTID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVesselAccountDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTDESCRIPTION") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="LblOpeningBalanceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENINGBALANCEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblBudgetId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Budget Code" HeaderStyle-Width="60px">

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Budget Code Description" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBudgetCodeDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Expenses" HeaderStyle-Width="60px" ColumnGroupName="Part1">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblAccumulatedExpenses" Text="Expenses" runat="server"></telerik:RadLabel>
                                    (
                                    <%=strAccumulatedExpensesTotal%>
                                    )  
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAccumulatedGlBal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDGLBALANCES","{0:#,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucAccumulatedGlBal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDGLBALANCES") %>'
                                        CssClass="input" DecimalPlace="2" Width="100%" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Committed Cost" HeaderStyle-Width="60px" ColumnGroupName="Part1">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAccumulatedCommittedCost" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDCOMMITTEDCOST","{0:#,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucAccumulatedCommittedCost" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDCOMMITTEDCOST") %>'
                                        CssClass="input" DecimalPlace="2" Width="100%" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Total Expense" HeaderStyle-Width="60px" ColumnGroupName="Part1">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAccumulatedTotalExpense" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDTOTALEXPENSE","{0:#,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn Visible="false" ColumnGroupName="Part1">
                                <HeaderTemplate>
                                    Accumulated Not Shown Amount
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAccumulatedNotShownAmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDNOTSHOWNAMOUNT","{0:#,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucAccumulatedNotShownAmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDNOTSHOWNAMOUNT") %>'
                                        CssClass="input" DecimalPlace="2" Width="100%" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Budget" HeaderStyle-Width="60px" ColumnGroupName="Part1">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblAccumulatedBudget" runat="server" Text="Budget"></telerik:RadLabel>
                                    (
                                    <%=strAccumulatedBudgetTotal%>
                                    ) 
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblAccumulatedBudget" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDBUDGET","{0:#,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucAccumulatedBudget" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDBUDGET") %>'
                                        CssClass="input" DecimalPlace="2" Width="100%" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Expenses" HeaderStyle-Width="60px" ColumnGroupName="Part2">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblYTDExpenses" runat="server" Text="Expenses"></telerik:RadLabel>
                                    (
                                    <%=strYtdExpensesTotal%>
                                    )
                                     
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblYTDGLBal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYTDGLBALANCE","{0:#,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucYTDGLBal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYTDGLBALANCE") %>'
                                        CssClass="input" DecimalPlace="2" Width="100%" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Committed Cost" HeaderStyle-Width="60px"  ColumnGroupName="Part2">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblYTDCommittedCost" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYTDCOMMITTEDCOST","{0:#,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucYTDCommittedCost" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYTDCOMMITTEDCOST") %>'
                                        CssClass="input" DecimalPlace="2" Width="100%" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Total Expense" HeaderStyle-Width="60px"  ColumnGroupName="Part2">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblYTDTotalExpense" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYTDTOTALEXPENSE","{0:#,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn Visible="false"  ColumnGroupName="Part2">
                                <HeaderTemplate>
                                    YTD Not Shown Amount
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblYTDNotShownAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYTDNOTSHOWNAMOUNT","{0:#,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucYTDNotShownAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYTDNOTSHOWNAMOUNT") %>'
                                        CssClass="input" DecimalPlace="2" Width="100%" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Budget" HeaderStyle-Width="60px"  ColumnGroupName="Part2">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblYtdBudget" runat="server" Text="Budget"></telerik:RadLabel>
                                    (
                                    <%=strYtdBudgetTotal%>
                                    )
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblYTDBudget" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYTDBUDGET","{0:#,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucYTDBudget" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYTDBUDGET") %>'
                                        CssClass="input" DecimalPlace="2" Width="100%" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="FY Start Accumulated Committed Cost" HeaderStyle-Width="60px 
                                ">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStartAccumulatedCommittedCost" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTACCUMULATEDCOMMITTEDCOST","{0:#,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucStartAccumulatedCommittedCost" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTACCUMULATEDCOMMITTEDCOST") %>'
                                        CssClass="input" DecimalPlace="2" Width="100%" />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px" />

                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="cmdEdit" runat="server" AlternateText="Edit" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="EDIT" ImageUrl="<%$ PhoenixTheme:images/te_edit.png%>" ToolTip="Edit" />
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <%--<asp:ImageButton ID="cmdDelete" runat="server" AlternateText="Delete" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="DELETE" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ToolTip="Delete"
                                        Visible="false" />--%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton ID="cmdSave" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="Update" ImageUrl="<%$ PhoenixTheme:images/save.png%>" ToolTip="Save" />
                                    <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
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


        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
