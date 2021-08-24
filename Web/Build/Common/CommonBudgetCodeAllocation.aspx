<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonBudgetCodeAllocation.aspx.cs" Inherits="CommonBudgetCodeAllocation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Budget Code Allocation</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />


        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCommonBudgetGroupAllocation" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel runat="server" ID="pnlCommonBudgetGroupAllocation">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFinancialYear" runat="server" Text="Financial Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFinancialYear" runat="server" CssClass="readonlytextbox txtnumber" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBudgetGroup" runat="server" Text="Budget Group"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBudgetGroup" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBudgetAmount" runat="server" Text="Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBudgetAmount" runat="server" CssClass="readonlytextbox txtnumber" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

            <telerik:RadButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuCommonBudgetGroupAllocation" runat="server" OnTabStripCommand="CommonBudgetGroupAllocation_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvBudgetCodeAllocation" runat="server" AutoGenerateColumns="False" EnableHeaderContextMenu="true" 
                Font-Size="11px" GridLines="None" Width="100%" CellPadding="3" OnItemCommand="gvBudgetCodeAllocation_ItemCommand" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                OnItemDataBound="gvBudgetCodeAllocation_ItemDataBound" OnNeedDataSource="gvBudgetCodeAllocation_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" DataKeyNames="FLDBUDGETCODEALLOCATIONID">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                   <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
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
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Budget Code" AllowSorting="true" >
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetCodeAllocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODEALLOCATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBudgetGroupAllocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPALLOCATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBudgetGroupId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBudgetId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkBudgetCode" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description" AllowSorting="true" SortExpression="FLDDESCRIPTION">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Amount" AllowSorting="true" SortExpression="FLDBUDGETAMOUNT">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucBudgetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETAMOUNT", "{0:##,###,###,##0.00}") %>' CssClass="input" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Allowance" AllowSorting="true" SortExpression="FLDALLOWANCE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAllowance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOWANCE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucAllowanceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOWANCE", "{0:##,###,###,##0.00}") %>' CssClass="input" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Apportionment" AllowSorting="true" SortExpression="FLDAPPORTIONMENTMETHODNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApportionment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPORTIONMENTMETHODNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="RadLabel1" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPORTIONMENTMETHOD") %>'></telerik:RadLabel>
                                <eluc:Hard runat="server" ID="ucApportionmentMethod" CssClass="dropdown_mandatory" AppendDataBoundItems="true" OnTextChangedEvent="ucApportionmentMethod_TextChangedEvent"
                                    HardList='<%# PhoenixRegistersHard.ListHard(106, 0, "", null)%>' SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDAPPORTIONMENTMETHOD") %>' AutoPostBack="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="" >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
            </telerik:RadGrid>

            <br />
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvBudgetPeriodAllocation" runat="server" AutoGenerateColumns="False" EnableHeaderContextMenu="true" GroupingEnabled="false"
                Font-Size="11px" GridLines="None" Width="100%" CellPadding="3" ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvBudgetPeriodAllocation_NeedDataSource" AllowSorting="true" AllowPaging="true" AllowCustomPaging="true"
                EnableViewState="false">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                   <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Period" AllowSorting="true" SortExpression="FLDPERIODNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPeriodName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERIODNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Committed" AllowSorting="true" SortExpression="FLDCOMMITTEDAMOUNT">
                             <ItemTemplate>
                                <telerik:RadLabel ID="lblCommittedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMITTEDAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Paid" AllowSorting="true" SortExpression="FLDPAIDAMOUNT">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaidAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAIDAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Total" AllowSorting="true" SortExpression="FLDTOTALEXPENDITURE">
                             <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALEXPENDITURE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Budgeted" AllowSorting="true" SortExpression="FLDBUDGETAMOUNT">
                           <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Monthly Variance" AllowSorting="true" SortExpression="FLDMONTHLYVARIANCE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMonthlyVariance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTHLYVARIANCE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Total" AllowSorting="true" SortExpression="FLDACCUMULATEDTOTAL">
                           <ItemTemplate>
                                <telerik:RadLabel ID="lblAccumulatedTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDTOTAL", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Budget" AllowSorting="true" SortExpression="FLDACCUMULATEDBUDGET">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetedTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDBUDGET", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Allowance" AllowSorting="true" SortExpression="FLDALLOWANCETOTAL">
                             <ItemTemplate>
                                <telerik:RadLabel ID="lblAllowanceTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALLOWANCETOTAL", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Variance" AllowSorting="true" SortExpression="FLDVARIANCE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVariance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Mngt Variance" AllowSorting="true" SortExpression="FLDMANAGEMENTVARIANCE">
                             <ItemTemplate>
                                <telerik:RadLabel ID="lblMngtVariance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANAGEMENTVARIANCE", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
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
            </telerik:RadGrid>
            <br />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
