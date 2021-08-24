<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAdvancePaymentVoucherGeneral.aspx.cs"
    Inherits="AccountsAdvancePaymentVoucherGeneral" EnableEventValidation="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice Payment Voucher</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoicePaymentVoucher" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <eluc:TabStrip ID="MenuVoucher" runat="server" Title="General" OnTabStripCommand="Voucher_TabStripCommand"></eluc:TabStrip>
        <br />
        <table cellpadding="1" cellspacing="1" style="width: 100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtVoucherNumber" runat="server" MaxLength="50" ReadOnly="true"
                        Width="120px" CssClass="readonlytextbox">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListVendor">
                        <telerik:RadTextBox ID="txtVendorCode" runat="server" ReadOnly="false" CssClass="input_mandatory"
                            MaxLength="20" Width="90px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVendorName" runat="server" ReadOnly="false" CssClass="input_mandatory"
                            Width="300px">
                        </telerik:RadTextBox>
                        <img runat="server" id="ImgShowMakerVendor" style="cursor: pointer; vertical-align: top"
                            src="<%$ PhoenixTheme:images/audit_start.png %>" onclick="return showPickList('spnPickListVendor', 'codehelp1', '', 'Common/CommonPickListAddress.aspx?framename=ifMoreInfo&addresstype=131', true); " />
                        <telerik:RadTextBox ID="txtVendorId" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:UserControlCurrency ID="ddlCurrency" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                        AppendDataBoundItems="true" runat="server" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtStatus" runat="server" CssClass="input" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblAmountPayable" runat="server" Text="Amount Payable"></telerik:RadLabel>
                </td>
                <td>
                    <asp:TextBox ID="txtAmount" runat="server" MaxLength="50" Style="text-align: right;" ReadOnly="true" Width="120px"
                        CssClass="readonlytextbox"></asp:TextBox>
                    <ajaxToolkit:MaskedEditExtender ID="MaskNumber" runat="server" TargetControlID="txtAmount"
                        Mask="999,999,999.99" MaskType="Number" InputDirection="RightToLeft">
                    </ajaxToolkit:MaskedEditExtender>
                </td>
            </tr>
        </table>
        <%--<eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />--%>
    </form>
</body>
</html>
