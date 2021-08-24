<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsProjectCodeDebitCreditNoteGenerateShipOwners.aspx.cs" Inherits="Accounts_AccountsProjectCodeDebitCreditNoteGenerateShipOwners" %>

<%@ Import Namespace="SouthNests.Phoenix.Accounts" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiOwnerBudgetCode" Src="~/UserControls/UserControlMultiColumnOwnerBudgetCodeT.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Debit/Credit Note</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDebitCreditNote" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">


                <eluc:TabStrip ID="MenuDebitCreditNote" runat="server" OnTabStripCommand="MenuDebitCreditNote_TabStripCommand"></eluc:TabStrip>

                <br />
                <table cellpadding="2" cellspacing="1" style="width: 100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlType" runat="server" CssClass="dropdown_mandatory" DataSource='<%# PhoenixRegistersQuick.ListQuick(1, 140)%>'
                                DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" OnDataBound="ddlType_DataBound" Width="150px" Filter="Contains" EmptyMessage="Type to select">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlVessel" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                AutoPostBack="true" DataSource='<%# PhoenixRegistersVesselAccount.ListUniqueVesselAccount(null, 1)%>'
                                DataTextField="FLDVESSELACCOUNTNAME" DataValueField="FLDVESSELACCOUNTID" OnDataBound="ddlVessel_DataBound"
                                OnTextChanged="ddlVessel_Changed" Width="150px" Filter="Contains" EmptyMessage="Type to select">
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblsubtype" runat="server" Text="Sub-Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlSubtype" runat="server" CssClass="dropdown_mandatory" DataSource='<%# PhoenixRegistersQuick.ListQuick(1, 154)%>'
                                DataTextField="FLDQUICKNAME" DataValueField="FLDQUICKCODE" OnDataBound="ddlSubtype_DataBound" Width="150px" Filter="Contains" EmptyMessage="Type to select">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:BudgetCode ID="ucBudgetCode" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                OnTextChangedEvent="ucBudgetCode_Changed" AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblBillingCompany" runat="server" Text="Billing Company"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:UserControlCompany ID="ddlBillToCompany" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                                AutoPostBack="true" CssClass="input_mandatory" runat="server" AppendDataBoundItems="true"
                                OnTextChangedEvent="ddlBillToCompany_Changed" Enabled="false" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOwnerBudget" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                            </br>
                        </td>
                        <td>
                            <eluc:MultiOwnerBudgetCode ID="ucOwnerBudgetCode" runat="server" CssClass="input_mandatory"
                                Width="40%" Enabled="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblBank" runat="server" Text="Bank receiving funds"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlBank" runat="server" CssClass="dropdown_mandatory" Width="270px" DataTextField="FLDBANKACCOUNTNUMBER"
                                DataValueField="FLDSUBACCOUNTID" OnDataBound="ddlBank_DataBound" Enabled="false" Filter="Contains" EmptyMessage="Type to select">
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOwner" runat="server" Text="Owner"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Address runat="server" ID="ddlOwner" CssClass="dropdown_mandatory" Width="270px" AddressType="128,127"
                                AutoPostBack="true" AppendDataBoundItems="true" Enabled="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lbldate" runat="server" Text="Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVoucherLineItemDesc" runat="server" Text="Voucher Long Description"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtVoucherLineItemDesc" runat="server" CssClass="input_mandatory" TextMode="MultiLine" Width="240px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Currency ID="ucCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                CssClass="dropdown_mandatory" runat="server" AppendDataBoundItems="true" AutoPostBack="true" Enabled="false" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference No."></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtReferenceNo" runat="server" CssClass="input" ReadOnly="true" Width="240px" MaxLength="50" Enabled="false"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                    </tr>
                </table>
                <br />
                <div id="div1" style="position: relative; z-index: 1; width: 100%;">
                    <%--   <asp:GridView ID="gvLineItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvLineItem_ItemDataBound" ShowFooter="true"
                        OnRowCreated="gvLineItem_RowCreated" OnRowCommand="gvLineItem_RowCommand" OnRowCancelingEdit="gvLineItem_RowCancelingEdit"
                        OnRowUpdating="gvLineItem_RowUpdating" OnRowEditing="gvLineItem_RowEditing" ShowHeader="true"
                        OnRowDeleting="gvLineItem_RowDeleting" EnableViewState="false" AllowSorting="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvLineItem" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvLineItem_NeedDataSource"
                        OnItemCommand="gvLineItem_ItemCommand"
                        OnItemDataBound="gvLineItem_ItemDataBound1"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" ShowFooter="true">
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
                                <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="300px">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblLineItemId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERDEBITCREDITNOTELINEITEMID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblLineItemIdEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERDEBITCREDITNOTELINEITEMID") %>'></telerik:RadLabel>
                                        <telerik:RadTextBox ID="txtDescriptionEdit" runat="server" CssClass="input_mandatory" Width="100%"
                                            TextMode="MultiLine" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'>
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtDescriptionAdd" runat="server" CssClass="input_mandatory" Width="100%"
                                            TextMode="MultiLine">
                                        </telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Amount" HeaderStyle-Width="150px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTINUSD","{0:###,###,###,##0.00}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="txtAmountEdit" runat="server" CssClass="input_mandatory txtNumber"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTINUSD") %>' Width="100%"></eluc:Number>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Number ID="txtAmountAdd" runat="server" CssClass="input_mandatory txtNumber"
                                            Width="100%"></eluc:Number>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            CommandName="Edit" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>
                                        <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Reject" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Delete" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                            CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                            ToolTip="Save"></asp:ImageButton>
                                        <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                            width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEditCancel"
                                            ToolTip="Cancel"></asp:ImageButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                            CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                            ToolTip="Add New"></asp:ImageButton>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>

                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
