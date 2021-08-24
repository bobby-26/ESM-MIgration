<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCommitedCommitmentsGeneral.aspx.cs" Inherits="Accounts_AccountsCommitedCommitmentsGeneral" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoice" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>


        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <%--    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">--%>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />

        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

        <eluc:TabStrip ID="MenuAdvancePayment" runat="server" OnTabStripCommand="MenuAdvancePayment_TabStripCommand"></eluc:TabStrip>

        <table cellpadding="2" cellspacing="1" style="width: 100%; z-index: 5">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPoNumber" runat="server" Text="Po Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtPoNumber" runat="server" ReadOnly="true" CssClass="input"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblSupplierName" runat="server" Text="Supplier Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSupplierName" runat="server" ReadOnly="true" CssClass="input" Width="300px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVesselAccountCode" runat="server" Text="Vessel Account Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtVesselAccountCode" runat="server" ReadOnly="true" CssClass="input" Width="300px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtBudgetCOde" runat="server" ReadOnly="true" CssClass="input" Width="300px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblOrderedDate" runat="server" Text="Ordered Date"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtOrderedDate" runat="server" ReadOnly="true" CssClass="input"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblBudgetGroup" runat="server" Text="Budget Group"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtBudgetGroup" runat="server" ReadOnly="true" CssClass="input" Width="300px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCommittedDate" runat="server" Text="Committed Date"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCommittedDate" runat="server" ReadOnly="true" CssClass="input"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtOwnerBudgetCode" runat="server" ReadOnly="true" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblReasonforReversal" runat="server" Text="Reason for Reversal"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtReasonReversal" runat="server" ReadOnly="true" CssClass="input" Width="300px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblReversedDate" runat="server" Text="Reversed Date"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtReversedDate" runat="server" ReadOnly="true" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblInvoiceStatus" runat="server" Text="Invoice Status"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtInvoiceStatus" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="300px"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPODescription" runat="server" Text="PO Description"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtPODescription" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="300px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPurchaseInvoiceVoucherNumber" runat="server" Text="Purchase Invoice Voucher Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtPurchaseInvNo" runat="server" ReadOnly="true" CssClass="input"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblReasonforchangeinCommittedDate" runat="server" Text="Reason for change in Committed Date"></telerik:RadLabel>
                </td>
                <td rowspan="2">
                    <telerik:RadTextBox ID="txtReasonCommitted" runat="server" ReadOnly="true" CssClass="input" Width="300px" Rows="4" TextMode="MultiLine"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblExcludedWithEffectFrom" runat="server" Text="Excluded With Effect From"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucDate" runat="server" CssClass="input" DatePicker="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblExcludedByOn" runat="server" Text="Excluded By/On"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtExcludedOn" runat="server" CssClass="input" ReadOnly="true" Width="320px"></telerik:RadTextBox>
                </td>
                 <td>
                    <telerik:RadLabel ID="lblGoodsreceiveddate" runat="server" Text="Goods Received Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucgoodsreceiveddate" runat="server" CssClass="input" DatePicker="true" />
                </td>
            </tr>
        </table>


      
    </form>
</body>
</html>
