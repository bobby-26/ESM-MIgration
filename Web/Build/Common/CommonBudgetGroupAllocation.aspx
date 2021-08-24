<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonBudgetGroupAllocation.aspx.cs" Inherits="CommonBudgetGroupAllocation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Budget Group Allocation</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />


        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCommonBudgetGroupAllocation" runat="server" submitdisabledcontrols="true">

        <telerik:RadScriptManager ID="ToolkitScriptManager1"
            runat="server">
        </telerik:RadScriptManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server" ID="pnlCommonBudgetGroupAllocation" EnableAJAX="false">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucSatus" runat="server" Text="" Visible="false" />

            <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />

            <eluc:TabStrip ID="MenuBudgetTab" runat="server" TabStrip="true" OnTabStripCommand="BudgetTab_TabStripCommand"></eluc:TabStrip>
<%--            <telerik:RadButton ID="cmdHiddenSubmit" runat="server" />--%>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                CssClass="hidden" />

            <table id="tblBudgetGroupAllocationSearch" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFinancialYear" runat="server" Text="Financial Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick runat="server" ID="ucFinancialYear" QuickTypeCode="55" CssClass="dropdown_mandatory"
                            AutoPostBack="true" AppendDataBoundItems="true" OnTextChangedEvent="FinancialYear_Changed"/>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFromDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtToDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <br />
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvVesselAllocation" runat="server" AutoGenerateColumns="False"
                Font-Size="11px" GridLines="None" Width="100%" CellPadding="3" OnItemCommand="gvVesselAllocation_ItemCommand" Height="150px"
                OnItemDataBound="gvVesselAllocation_ItemDataBound" OnNeedDataSource="gvVesselAllocation_NeedDataSource" AllowSorting="true"
                OnSelectedIndexChanging="gvVesselAllocation_SelectedIndexChanging" GroupingEnabled="false" EnableHeaderContextMenu="true"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDVESSELBUDGETALLOCATIONID">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <%--                   <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />--%>


                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Rev.No" HeaderStyle-Width="85px">
                            <ItemTemplate>
                                <%#(Container.DataSetIndex+1) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="130px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselAllocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELBUDGETALLOCATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOwnerId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFinancialYearId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALYEAR") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkVessel" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                               <telerik:RadLabel ID="lbllatest" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLATEST") %>'></telerik:RadLabel>

                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblVesselAllocationIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELBUDGETALLOCATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOwnerIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFinancialYear" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINANCIALYEAR") %>'></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblVesselNameEdit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccountIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account" HeaderStyle-Width="245px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Budget Amount" HeaderStyle-Width="122px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Effective Date" HeaderStyle-Width="130px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE", "{0:dd/MM/yyyy}" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" Width="120px" ID="ucDate" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEFFECTIVEDATE" ) %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="117px" HeaderText=" Applied Period">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblappliedperiod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPLIEDPERIODS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Over written" HeaderStyle-Width="110px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOverwritten" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERWRITTEN" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>

                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" Visible="true" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" Visible="true" ID ="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Budget Breakdown" CommandName="BUDGETBREAKDOWN" ID="cmdBudgetBreakDown" ToolTip="Budget Breakdown">
                                    <span class="icon"><i class="fas fa-list-alt"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAttachment" ToolTip="View Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <br />

            <eluc:TabStrip ID="MenuCommonBudgetGroupAllocation" runat="server" OnTabStripCommand="CommonBudgetGroupAllocation_TabStripCommand" TabStrip="true"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvBudgetGroupAllocation" runat="server" AutoGenerateColumns="False"
                Font-Size="11px" GridLines="None" Width="100%" CellPadding="3" OnItemCommand="gvBudgetGroupAllocation_ItemCommand"
                OnItemDataBound="gvBudgetGroupAllocation_ItemDataBound" OnNeedDataSource="gvBudgetGroupAllocation_NeedDataSource" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnSortCommand="gvBudgetGroupAllocation_SortCommand" AllowSorting="true" AllowCustomPaging="true" AllowPaging="true"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDBUDGETGROUPID">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Budget Group">
                            <ItemTemplate>
                                 <telerik:RadLabel ID="lblVesselbudgetAllocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELBUDGETALLOCATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBudgetGroupAllocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPALLOCATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBudgetGroupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkBudgetGroup" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Amount">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Allowance">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAllowance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOWANCE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Access">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccess" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCESSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Apportionment">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApportionment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPORTIONMENTMETHODNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Budget Code" CommandName="BUDGETCODE" ID="cmdBudgetCode" ToolTip="Budget Code Breakup">
                                    <span class="icon"><i class="fas fa-list-alt"></i></span>
                                </asp:LinkButton>


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

            <br />
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvBudgetPeriodAllocation" runat="server" AutoGenerateColumns="False"
                Font-Size="11px" GridLines="None" Width="100%" CellPadding="3" OnItemCommand="gvBudgetPeriodAllocation_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnItemDataBound="gvBudgetPeriodAllocation_ItemDataBound" OnNeedDataSource="gvBudgetPeriodAllocation_NeedDataSource" ShowFooter="false" ShowHeader="true" AllowSorting="true"
                EnableViewState="false">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Monthly Totals" HeaderStyle-HorizontalAlign="Center" Name="Monthlytotals">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Accumulated" HeaderStyle-HorizontalAlign="Center" Name="Accumulated">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Period" ColumnGroupName="Monthlytotals">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPeriod" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERIOD") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblYear" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBudgetGroupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkPeriodName" runat="server" CommandName="BUDGETCODE" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERIODNAME") %>'></asp:LinkButton>
                                 <telerik:RadLabel ID="lblAllocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETPERIODALLOCATIONID") %>'></telerik:RadLabel>   
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Committed" ColumnGroupName="Monthlytotals">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblCommittedAmount" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSCOMMITTED", "{0:##,###,###,##0.00}") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblCommitmentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCOMMITMENTID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Paid" ColumnGroupName="Monthlytotals">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblPaidAmount" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCESSPAID", "{0:##,###,###,##0.00}") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Total" ColumnGroupName="Monthlytotals">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalAmount" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALEXPENDITURE", "{0:##,###,###,##0.00}") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Budgeted" ColumnGroupName="Monthlytotals">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn ColumnGroupName="Monthlytotals" HeaderText="Monthly Variance">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMonthlyVariance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTHLYVARIANCE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Total" ColumnGroupName="Monthlytotals">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccumulatedTotalAmount" runat="server" Text=''></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Budget" ColumnGroupName="Accumulated">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetedTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDBUDGET", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn ColumnGroupName="Accumulated" HeaderText="Allowance">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAllowanceTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOWANCETOTAL", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Variance" ColumnGroupName="Accumulated">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVariance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn ColumnGroupName="Accumulated" HeaderText="Mngt Variance">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMngtVariance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANAGEMENTVARIANCE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Budget Code" ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                    CommandName="BUDGETCODE" CommandArgument="<%# Container.DataItem %>" ID="cmdBudgetCode"
                                    ToolTip="Budget Code Breakup"></asp:ImageButton>
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
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <br />
            <br />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
