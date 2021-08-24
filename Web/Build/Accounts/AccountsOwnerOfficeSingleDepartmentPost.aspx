<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOwnerOfficeSingleDepartmentPost.aspx.cs"
    Inherits="AccountsOwnerOfficeSingleDepartmentPost" %>

<%@ Import Namespace="SouthNests.Phoenix.Accounts" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiOwnerBudgetCode" Src="~/UserControls/UserControlMultipleColumnOwnerBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Projectcode" Src="~/UserControls/UserControlProjectCode.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Office Fund</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOfficeFund" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Title runat="server" ID="ucTitle" Text="Bank Charges Apportionment" ShowMenu="false" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="MenuOfficeFund" runat="server" OnTabStripCommand="MenuOfficeFund_TabStripCommand" Title="Bank Charges Apportionment"
                TabStrip="true"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" CssClass="hidden" />
            <table width="60%">
                <tr>
                    <td width="20%">
                        <telerik:RadLabel ID="lblFundBankChargeId" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                    <td width="40%"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account Code/ Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtAccountSearch" CssClass="input" MaxLength="10"
                            Width="80%">
                        </telerik:RadTextBox>&nbsp;
                        <asp:ImageButton runat="server" ImageUrl="<%$ PhoenixTheme:images/search.png %>"
                            ID="cmdAccountSearch" OnClick="cmdSearchAccount_Click" ToolTip="Search" />
                        <div runat="server" id="Div2" class="input" style="overflow: auto; width: 80%; height: 80px">
                            <asp:RadioButtonList ID="rblAccount" runat="server" Height="100%" RepeatColumns="1"
                                RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="True" OnSelectedIndexChanged="rblAccount_Changed">
                            </asp:RadioButtonList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSubAccount" runat="server" Text="Sub Account "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlBudgetCode" runat="server" CssClass="dropdown_mandatory" Width="25%"
                            AppendDataBoundItems="true" OnTextChanged="ddlBudgetCode_Changed" AutoPostBack="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadTextBox ID="txtOwnerBudgetCode" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>--%>
                        <eluc:MultiOwnerBudgetCode ID="ucOwnerBudgetCode" runat="server" CssClass="input_mandatory"
                            Width="40%" Enabled="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProjectCode" runat="server" Text="Project Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Projectcode ID="ucProjectcode" runat="server" AppendDataBoundItems="true" AutoPostBack="true" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount  "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAmount" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuOfficeFundGrid" runat="server" OnTabStripCommand="MenuOfficeFundGrid_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvBankCharge" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" CellPadding="3" OnItemDataBound="gvBankCharge_ItemDataBound" ShowFooter="false" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnItemCommand="gvBankCharge_ItemCommand" OnNeedDataSource="gvBankCharge_NeedDataSource" OnRowEditing="gvBankCharge_RowEditing"
                ShowHeader="true" EnableViewState="false" AllowSorting="true">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="true">
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
                        <telerik:GridTemplateColumn HeaderText="Account Code/ Description" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFundBankChargeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUNDBANKCHARGEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccountdesc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccountid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub Account" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSubAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner Budget Code" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOwnerBudgetCodeId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODEID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Project Code" HeaderStyle-Width="15%">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProjectCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROJECTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Right">
                            <ItemStyle Wrap="true" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="Edit" CommandArgument='<%# Container.DataItem %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Reject" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Delete" CommandArgument='<%# Container.DataItem %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
