<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOtherOwnerFundRequestAdd.aspx.cs" Inherits="AccountsOtherOwnerFundRequestAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Accounts" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiOwnerBudgetCode" Src="~/UserControls/UserControlMultipleColumnOwnerBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Other Owner Type</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
     <form id="frmDebitCreditNote" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Title runat="server" ID="ucTitle" Text="Debit/Credit Note" ShowMenu="false" Visible="false"></eluc:Title>
            <eluc:TabStrip ID="MenuDebitCreditNote" runat="server" OnTabStripCommand="MenuDebitCreditNote_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <tr>
                     <td>
                        <telerik:RadLabel ID="lblfundtype" runat="server" Text="Fund Request Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlfundtype" runat="server" Enabled="false" CssClass="dropdown_mandatory">
                           <Items>
                              <telerik:RadComboBoxItem Value="1" Text="Other Owners"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
           
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlType" runat="server" CssClass="dropdown_mandatory" DataSource='<%# PhoenixRegistersQuick.ListQuick(1, 140)%>'
                            DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" OnDataBound="ddlType_DataBound" Width="240px">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAccountCodeAccountDescription" runat="server" Text="Account Code/Account Description"></telerik:RadLabel>
                    </td>
                    <td style="white-space: nowrap" rowspan="2">

                        <telerik:RadTextBox runat="server" ID="txtAccountSearch" CssClass="input" MaxLength="50"
                            Width="240px">
                        </telerik:RadTextBox>&nbsp;
                            <asp:ImageButton runat="server" ImageUrl="<%$ PhoenixTheme:images/search.png %>"
                                ID="cmdAccountSearch" OnClick="cmdSearchAccount_Click" ToolTip="Search" />
                        <div runat="server" id="Div2" class="input_mandatory" style="overflow: auto; width: 240px; height: 75px">
                            <asp:RadioButtonList ID="rblAccount" runat="server" Height="100%" RepeatColumns="1"
                                RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="True" OnSelectedIndexChanged="AccountSelection">
                            </asp:RadioButtonList>
                        </div>
                    </td>
                    <%--<td>
                            <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSubject" runat="server" CssClass="input_mandatory" TextMode="MultiLine"></telerik:RadTextBox>
                        </td>--%>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBillingCompany" runat="server" Text="Billing Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCompany ID="ddlBillToCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            AutoPostBack="true" CssClass="input_mandatory" runat="server" AppendDataBoundItems="true" Width="240px"
                            OnTextChangedEvent="ddlBillToCompany_Changed" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBank" runat="server" Text="Bank receiving funds"></telerik:RadLabel>
                    </td>
                    <td>

                        <telerik:RadComboBox ID="ddlBank" runat="server" CssClass="dropdown_mandatory" Width="240px">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblsubaccount" runat="server" Text="Sub Account"></telerik:RadLabel>
                    </td>
                    <td rowspan="2">

                        <br />
                        <telerik:RadTextBox ID="txtBudgetCodeSearch" runat="server" CssClass="input"
                            MaxLength="50" Width="240px">
                        </telerik:RadTextBox>
                        &nbsp;<asp:ImageButton ID="cmdBudgetCodeSearch" runat="server"
                            ImageUrl="<%$ PhoenixTheme:images/search.png %>"
                            OnClick="cmdSearchSubAccount_Click" ToolTip="Search" />
                        <br />
                        <div runat="server" id="Div3" class="input_mandatory" style="overflow: auto; width: 240px; height: 75px">
                            <asp:RadioButtonList ID="rblBudgetCode" runat="server" Height="100%" RepeatColumns="1"
                                RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="True" OnSelectedIndexChanged="SubAccountSelection">
                            </asp:RadioButtonList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbldate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Currency ID="ucCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" AutoPostBack="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOwnerBudget" runat="server" Text="Owner Budget Code"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:MultiOwnerBudgetCode ID="ucOwnerBudgetCode" runat="server" CssClass="input_mandatory"
                            Width="240px" Enabled="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReferenceNo" runat="server" CssClass="input_mandatory" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAddressee" runat="server" Text="Addressee"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAddressee" runat="server" CssClass="input_mandatory" Width="240px" Resize="Both"
                            TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubject" runat="server" CssClass="input_mandatory" TextMode="MultiLine" Resize="Both" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVoucherLineItemDesc" runat="server" Text="Voucher Long Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVoucherLineItemDesc" runat="server" CssClass="input_mandatory" TextMode="MultiLine" Resize="Both" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvLineItem" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                Width="100%" CellPadding="3" OnItemDataBound="gvLineItem_ItemDataBound" ShowFooter="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnItemCommand="gvLineItem_ItemCommand" ShowHeader="true" OnNeedDataSource="gvLineItem_NeedDataSource" EnableViewState="false" AllowSorting="true">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true"
                    AllowNaturalSort="false"
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
                        <telerik:GridTemplateColumn HeaderText="Description">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLineItemId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEDEBITCREDITNOTELINEITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblLineItemIdEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEDEBITCREDITNOTELINEITEMID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" CssClass="input_mandatory" Width="99%"
                                    TextMode="MultiLine" Resize="Both" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'>
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" CssClass="input_mandatory" Width="99%"
                                    TextMode="MultiLine" Resize="Both">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input_mandatory txtNumber"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>' Width="120px"></eluc:Number>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtAmountAdd" runat="server" CssClass="input_mandatory txtNumber"
                                    Width="120px"></eluc:Number>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
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
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument='<%# Container.DataItem %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataItem %>' ID="cmdEditCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="Add" CommandArgument='<%# Container.DataItem %>' ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" SaveScrollPosition="false" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="3" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
   </form>
</body>
</html>
