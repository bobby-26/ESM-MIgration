<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCashOutRequestGeneral.aspx.cs"
    Inherits="AccountsCashOutRequestGeneral" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cash Out Request General</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCashOutDetails" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="93%" EnableAJAX="true">


            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <telerik:RadButton ID="cmdHiddenSubmit" Visible="false" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <telerik:RadButton runat="server" ID="cmdHiddenPick" Visible="false" OnClick="cmdHiddenPick_Click" />
            <eluc:TabStrip ID="MenuSave" runat="server" OnTabStripCommand="MenuSave_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>

            <table cellpadding="1" cellspacing="1" style="width: 100%">

                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblSupplierCodeName" runat="server" Text="Supplier Code / Name:"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <telerik:RadTextBox ID="txtSupplierCode" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtSupplierName" runat="server" BorderWidth="1px" Width="180px"
                            CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtSupplierId" runat="server" Width="1" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCashAccount" runat="server" Text="Cash Account"></telerik:RadLabel>
                    </td>
                    <td style="width: 50%">
                        <span id="spnPickListCashAccount">
                            <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" MaxLength="20"
                                Width="20%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountDescription" runat="server" CssClass="input_mandatory"
                                MaxLength="50" Width="36%">
                            </telerik:RadTextBox>
                            <img runat="server" id="imgShowAccount" style="cursor: pointer; vertical-align: top"
                                src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListCashAccount', 'codehelp1', '', '../Common/CommonPickListCashAccount.aspx',true); " />
                            <telerik:RadTextBox ID="txtAccountId" runat="server" CssClass="input" MaxLength="20" Width="10px" OnTextChanged="txtAccountId_Changed"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPaymentCurrency" runat="server" Text="Payment Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubAccountCode" Visible="false" runat="server"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCurrencyId" Visible="false" ReadOnly="true" runat="server" CssClass="input"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCurrencyCode" Visible="true" ReadOnly="true" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                        <telerik:RadLabel ID="lblCashPaymentId" Visible="false" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBankAccount" runat="server" Text="Bank Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListBank">
                            <telerik:RadTextBox ID="txtAccountNo" runat="server" CssClass="input" ReadOnly="false"
                                Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBankName" runat="server" CssClass="input" ReadOnly="false"
                                Width="60px">
                            </telerik:RadTextBox>
                            <img id="imgBankPicklist" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                            <telerik:RadTextBox ID="txtBankID" runat="server" CssClass="hidden" Width="5px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAmount" runat="server" Style="text-align: right;" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCashpaymentVoucherNumber" runat="server" Text="Cash payment Voucher Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVoucherNumber" runat="server" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td colspan="2"></td>
                </tr>
            </table>
            <br />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
