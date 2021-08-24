<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsUsageBank.aspx.cs"
    Inherits="AccountsUsageBank" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sub Account Code</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <%--                            <asp:Literal ID="lblSubAccount" runat="server" Text="Sub Account"></asp:Literal>--%>
        <eluc:TabStrip ID="MenuBudget" runat="server" OnTabStripCommand="Budget_TabStripCommand"></eluc:TabStrip>
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblAccountCode" runat="server" Text="Account Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel runat="server" ID="lblSubAccountMapId" Visible="false"></telerik:RadLabel>
                    <telerik:RadTextBox ID="txtAccountCode" runat="server" MaxLength="10" Width="150px" ReadOnly="true"
                        CssClass="readonlytextbox">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblAccountDescription" runat="server" Text="Account Description"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtAccountDescription" runat="server" MaxLength="10" Width="200px"
                        ReadOnly="true" CssClass="readonlytextbox">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblAccountUsage" runat="server" Text="Account Usage"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtAccountUsage" runat="server" MaxLength="10" Width="150px" ReadOnly="true"
                        CssClass="readonlytextbox">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSubAccountCode" runat="server" Text="Sub Account Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSubAccount" runat="server" MaxLength="50" Width="150px" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDesc" runat="server" Text="Description"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtDescription" runat="server" MaxLength="200" Width="400px" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVoucherPrefixAccountCod" runat="server" Text="Voucher Prefix Account Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtVoucherPrefixAccountCode" MaxLength="1" CssClass="input_mandatory"
                        Width="100px">
                    </telerik:RadTextBox>
                </td>
            </tr>

            <tr>
                <td>
                    <telerik:RadLabel ID="lblActive" runat="server" Text="Active"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkActive" runat="server"></telerik:RadCheckBox>
                </td>
            </tr>

        </table>
        <eluc:TabStrip ID="MenuRegistersBudget" runat="server" OnTabStripCommand="RegistersBudgetMenu_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gdBudget" runat="server" AutoGenerateColumns="False" Font-Size="11px" Height="40%"
            Width="100%" CellPadding="3" OnItemCommand="gdBudget_ItemCommand" OnItemDataBound="gdBudget_ItemDataBound" AllowPaging="true" AllowCustomPaging="false"
            OnSortCommand="gdBudget_Sorting" OnNeedDataSource="gdBudget_NeedDataSource" GroupingEnabled="false" EnableHeaderContextMenu="true"
            AllowSorting="true" ShowFooter="false" ShowHeader="true" EnableViewState="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
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
                    <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" SortExpression="FLDSUBACCOUNT">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBudgetid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkBudget" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItem %>'
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Description" AllowSorting="true" SortExpression="FLDDESCRIPTION">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="VoucherPrefixAccountCode">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblVoucherPrefixAccountCodeHeader" runat="server" ForeColor="White">Voucher Prefix AccountCode&nbsp;</telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVoucherPrefixAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERPREFIXACCOUNTCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Active">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblActiveHeader" runat="server" ForeColor="White">Active&nbsp;</telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblActive" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Center">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblActionHeader" runat="server">
                                Action
                            </telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/edit-info.png %>"
                                CommandName="EDITBANK" CommandArgument="<%# Container.DataItem %>" ID="imgEditBank"
                                ToolTip="Edit Bank Information"></asp:ImageButton>
                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/72.png %>"
                                CommandName="EDITMAPPING" CommandArgument="<%# Container.DataItem %>" ID="imgEditTemplate"
                                ToolTip="Template Mapping"></asp:ImageButton>
                            <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="DELETE" CommandArgument="<%# Container.DataItem %>" ID="cmdDelete"
                                ToolTip="Delete"></asp:ImageButton>
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
    </form>
</body>
</html>
