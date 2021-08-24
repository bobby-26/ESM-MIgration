<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersUsageVessel.aspx.cs"
    Inherits="RegistersUsageVessel" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sub Account Code</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div>
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel runat="server" ID="lblSubAccountMapId" Visible="false"></telerik:RadLabel>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtAccountCode" runat="server" MaxLength="10" Width="150px" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAccountDescription" runat="server" Text="Description"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtAccountDescription" runat="server" MaxLength="10" Width="200px" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAccountUsage" runat="server" Text="Usage"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtAccountUsage" runat="server" MaxLength="10" Width="150px" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <eluc:TabStrip ID="MenuRegistersBudget" runat="server" OnTabStripCommand="RegistersBudgetMenu_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gdBudget" Height="86%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gdBudget_ItemCommand" OnRowDataBound="gdBudget_ItemDataBound" OnNeedDataSource="gdBudget_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDBUDGETID" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Sub Account" AllowSorting="true" SortExpression="FLDSUBACCOUNT">
                            <HeaderStyle Width="17%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkBudget" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Description" AllowSorting="true" SortExpression="FLDDESCRIPTION">
                            <HeaderStyle Width="17%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Account Type" AllowSorting="true" SortExpression="FLDACCOUNTTYPENAME">
                            <HeaderStyle Width="17%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%--<telerik:RadLabel ID="lblAccountTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTYPEID") %>'></telerik:RadLabel>--%>
                                <telerik:RadLabel ID="lblAccountType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Budget Group" AllowSorting="true" SortExpression="FLDBUDGETGROUPNAME">
                            <HeaderStyle Width="17%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%--  <telerik:RadLabel ID="lblBudgetGroupid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPID") %>'></telerik:RadLabel>--%>
                                <telerik:RadLabel ID="lblBudgetGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETGROUPNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Budgeted Expense" AllowSorting="true" SortExpression="FLDBUDGETEDEXPENSE">
                            <HeaderStyle Width="17%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetedExpense" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDBUDGETEDEXPENSE").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
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
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
    </form>
</body>
</html>
