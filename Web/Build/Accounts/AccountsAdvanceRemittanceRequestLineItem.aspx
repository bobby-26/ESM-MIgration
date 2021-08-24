<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAdvanceRemittanceRequestLineItem.aspx.cs"
    Inherits="AccountsAdvanceRemittanceRequestLineItem" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remittence Line Item</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseFormItemDetails" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Remittance" ShowMenu="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
        </div>
        <table cellpadding="1" cellspacing="1" style="width: 100%">
            <tr>
                <td>
                    <asp:Literal ID="lblRemittanceNumber" runat="server" Text="Remittance Number"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtRemittanceNumber" ReadOnly="true" Width="180px" runat="server"
                        CssClass="input"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblSupplier" runat="server" Text="Supplier"></asp:Literal>
                </td>
                <td>
                    <span id="spnPickListSupplier">
                        <asp:TextBox ID="txtSupplierCode" runat="server" Width="60px" CssClass="readonlytextbox"></asp:TextBox>
                        <asp:TextBox ID="txtSupplierName" runat="server" BorderWidth="1px" Width="180px"
                            CssClass="readonlytextbox"></asp:TextBox>
                        <%-- <asp:ImageButton ID="btnPickSupplier" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            OnClientClick="return showPickList('spnPickListSupplier', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=131', true);" />--%>
                        <asp:TextBox ID="txtSupplierId" runat="server" Width="1" CssClass="input"></asp:TextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblAccountCode" runat="server" Text="Account Code"></asp:Literal>
                </td>
                <td>
                    <eluc:UserControlBankAccount ID="ddlBankAccount" BankAccountList='<%# PhoenixRegistersAccount.ListBankAccount(null,null,iCompanyid)%>'
                        AppendDataBoundItems="true" OnTextChangedEvent="ddlBankAccount_SelectedIndexChanged"
                        AutoPostBack="true" runat="server" CssClass="input_mandatory" />
                </td>
                <td style="width: 380px">
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtSubAccountCode" Visible="false" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtAccountId" Visible="false" runat="server"> </asp:TextBox>
                    <asp:TextBox ID="txtCurrencyId" Visible="FALSE" ReadOnly="true" runat="server" CssClass="input"></asp:TextBox>
                    <asp:TextBox ID="txtCurrencyCode" Visible="true" ReadOnly="true" runat="server" CssClass="input"></asp:TextBox>
                </td>
                <td style="width: 380px">
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblShowAllCurrency" runat="server" Text="Show All Currency"></asp:Literal>
                </td>
                <td>
                    <asp:CheckBox ID="chkShowAll" runat="server" AutoPostBack="true" />
                </td>
                <td style="width: 380px">
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="1" style="width: 100%">
            <tr>
                <td style="vertical-align: top">
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvVoucherDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCommand="gvVoucherDetails_RowCommand" OnRowDataBound="gvVoucherDetails_ItemDataBound"
                            OnRowDeleting="gvVoucherDetails_RowDeleting" AllowSorting="true" EnableViewState="false"
                            OnSorting="gvVoucherDetails_Sorting" DataKeyNames="FLDADVANCEPAYMENTVOUCHERID">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActiveYNHeader" runat="server"> Payment Voucher No.</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox runat="server" ID="chkVoucher" AutoPostBack="true" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCEPAYMENTVOUCHERID") %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>' OnCheckedChanged="CheckBoxClicked" />
                                        <asp:TextBox ID="txtInvoiceLineItemCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCEPAYMENTVOUCHERID") %>'
                                            MaxLength="10" Visible="false"></asp:TextBox>
                                        <asp:TextBox ID="txtVoucherId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCEPAYMENTVOUCHERID") %>'
                                            MaxLength="10" Visible="false"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Voucher Number" Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblVoucherNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDVOUCHERNUMBER"
                                            ForeColor="White">Voucher Number&nbsp;</asp:LinkButton>
                                        <img id="FLDVOUCHERNUMBER" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblVoucherNumber" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></asp:Label>
                                        <asp:Label ID="lblCurrencyCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCY") %>'></asp:Label>
                                        <asp:Label ID="lblSuppCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERCODE") %>'></asp:Label>
                                        <asp:LinkButton ID="lnkVoucherId" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCEPAYMENTVOUCHERID") %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER")  %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Supplier Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblSupplierName" runat="server" Text="Supplier Name"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblReferenceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency Code">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblCurrencyCode" runat="server" Text="Currency Code"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrencyShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                               <%-- <asp:TemplateField HeaderText="Remittance Number">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        Mapped Remittance Number
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemittanceNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCENUMBER") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divPage" style="position: relative;">
                        <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                            <tr>
                                <td nowrap align="center">
                                    <asp:Label ID="lblPagenumber" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblPages" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblRecords" runat="server">
                                    </asp:Label>&nbsp;&nbsp;
                                </td>
                                <td nowrap align="left" width="50px">
                                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                                </td>
                                <td width="20px">
                                    &nbsp;
                                </td>
                                <td nowrap align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap align="center">
                                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                    </asp:TextBox>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                        Width="40px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
