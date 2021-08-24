<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseCommittedBudget.aspx.cs" Inherits="PurchaseCommittedBudget" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Committed Budget</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function PaneResized(sender, args) {
                
                var browserHeight = $telerik.$(window).height();
                var grid = $find("gvBudget");
                grid._gridDataDiv.style.height = (browserHeight - 230) + "px";
            }
            function pageLoad() {
                PaneResized();
            }
        </script>
</telerik:RadCodeBlock>

</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmCommittedBudget" runat="server" submitdisabledcontrols="true">
    <telerik:RadScriptManager ID="RadScriptManager" runat="server"></telerik:RadScriptManager>
    <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="Menu" runat="server"  Title="Committed Budget" />

        <table style="width: 100%;">
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory" EnableLoadOnDemand="true"
                        DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDACCOUNTID" Width="100%">
                    </telerik:RadComboBox>
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblFinYear" runat="server" Text="Financial Year"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick runat="server" ID="ucFinancialYear" QuickTypeCode="55" CssClass="dropdown_mandatory"
                        AutoPostBack="true" AppendDataBoundItems="true" OnTextChangedEvent="ucFinancialYear_TextChangedEvent"  Width="100%"/>
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblBudgetGroupId" runat="server" Text="Budget Group"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ucBudgetGroup" runat="server" HardTypeCode="30" CssClass="input_mandatory" AppendDataBoundItems="true" Width="100%"
                        AutoPostBack="true" ShortNameFilter="60,63,66,69,72,75,78,81,84,90,96,85,86,88" OnTextChangedEvent="ucBudgetGroup_TextChangedEvent" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtVesselName" runat="server" CssClass="readonlytextbox" Width="100%"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblEffectiveDate" runat="server" Text="Effective Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucEffectiveDate" runat="server" CssClass="readonlytextbox"  Width="100%"/>
                </td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblTotalAmount" runat="server" Text="Total Budget Amount"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtTotalAmount" runat="server" CssClass="readonlytextbox" Width="100%"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblBudgetGroupAmount" runat="server" Text="Budget Group Amount"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtBudgetGroupAmount" runat="server" CssClass="readonlytextbox" Width="100%"></telerik:RadTextBox>
                </td>
                <td></td>
                <td></td>
            </tr>
        </table>

        <br />
        <br />
                <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvBudget" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" 
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvBudget_NeedDataSource" OnItemDataBound="gvBudget_ItemDataBound" ShowFooter="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDPERIOD,FLDVESSELID,FLDACCOUNTID,FLDYEAR,FLDBUDGETGROUPID" >
                        <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Monthly" Name="MONTHLY" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                            <telerik:GridColumnGroup HeaderText="Accumulated" Name="ACCUMULATED" HeaderStyle-HorizontalAlign="Center" ></telerik:GridColumnGroup>
                        </ColumnGroups>
                        <Columns>
                            
                            <telerik:GridTemplateColumn HeaderText="Period" UniqueName="PERIOD">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblPeriodName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTHNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Committed" UniqueName="COMMITTED">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblCommittedAmount" runat="server" CommandName="SELECT" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMITTEDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Revoked" UniqueName="REVOKED">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblRevokedAmount" runat="server" CommandName="SELECT" 
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVOKEDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Charged" UniqueName="CHARGED">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                   <asp:LinkButton ID="lblChargedAmount" runat="server" CommandName="SELECT" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHARGEDAMOUNT", "{0:##,###,###,##0.00}") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Total" UniqueName="TOTAL" ColumnGroupName="MONTHLY" >
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                   <telerik:RadLabel RenderMode="Lightweight" ID="lblTotalAmount" runat="server" 
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCOMMITTEDAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Budget" UniqueName="BUDGET" ColumnGroupName="MONTHLY">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                   <telerik:RadLabel RenderMode="Lightweight" ID="lblBudgetAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Budget Exceeded" UniqueName="BUDGETEXCEEDED" ColumnGroupName="MONTHLY">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                   <telerik:RadLabel RenderMode="Lightweight" ID="lblMonthlyVarianceexceeded" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVARIANCEEXCEEDEDBY", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <%--<telerik:GridTemplateColumn HeaderText="Accumulated Total" UniqueName="ACCUMULATEDTOTAL">
                                <ItemTemplate>
                                   <telerik:RadLabel RenderMode="Lightweight" ID="lblAccumulatedTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDTOTALCOMMITTED", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>

                            <telerik:GridTemplateColumn HeaderText="Total" UniqueName="ACCUMULATEDTOTAL" ColumnGroupName="ACCUMULATED">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                   <telerik:RadLabel RenderMode="Lightweight" ID="lblAccumulatedTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDTOTALCOMMITTED", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Budget" UniqueName="ACCUMULATEDBUDGET" ColumnGroupName="ACCUMULATED">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                   <telerik:RadLabel RenderMode="Lightweight" ID="lblBudgetedTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDBUDGETAMOUNT", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Budget Exceeded" UniqueName="ACCUMULATEDBUDGETEXCEEDED" ColumnGroupName="ACCUMULATED">
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                   <telerik:RadLabel RenderMode="Lightweight" ID="lblVarianceexceeded" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCUMULATEDVARIANCEEXCEEDEDBY", "{0:##,###,###,##0.00}") %>'></telerik:RadLabel>
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

                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                    </ClientSettings>
                    </telerik:RadGrid>
    </form>
</body>
</html>
