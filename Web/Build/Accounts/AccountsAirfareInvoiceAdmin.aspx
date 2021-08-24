<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAirfareInvoiceAdmin.aspx.cs"
    Inherits="AccountsAirfareInvoiceAdmin" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Credit Note</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCreditNote" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ttlAirfareInvoice" Text="General" ShowMenu="false" Visible="false"></eluc:Title>
            <table cellpadding="2" cellspacing="1" style="width: 100%;">
                <tr>
                    <td style="width: 20%;">
                        <telerik:RadLabel ID="lblRequisitionNo" runat="server" Text="Request No"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%;">
                        <telerik:RadTextBox ID="txtRequestNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadLabel ID="lblInvoiceNo" runat="server" Text="Invoice Number"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%;">
                        <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassengerName" runat="server" Text="Passenger Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassengerName" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="175px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDepartureDate" runat="server" Text="Departure Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucDepartureDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblArrivalDate" runat="server" Text="Arrival Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucArrivalDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOrigin" runat="server" Text="Origin"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOrigin" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTicketNo" runat="server" Text="Ticket No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTicketNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAmountPayable" runat="server" Text="Amount Payable"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtAmountPayable" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBillTo" runat="server" Text="Bill To"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBillTo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDestination" runat="server" Text="Destination"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDestination" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td rowspan="3">
                        <telerik:RadLabel ID="lblAccountsVouchersHeader" runat="server" Text="Accounts Voucher No"></telerik:RadLabel>
                    </td>
                    <td rowspan="3">
                        <telerik:RadTextBox ID="txtAccountsVouchers" runat="server" CssClass="readonlytextbox" ReadOnly="true" Resize="Both"
                            TextMode="MultiLine" Rows="3" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblChargedAmount" runat="server" Text="Charged Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtChargedAmount" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="240px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="readonlytextbox" ReadOnly="true" Resize="Both"
                            TextMode="MultiLine" Rows="2" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
