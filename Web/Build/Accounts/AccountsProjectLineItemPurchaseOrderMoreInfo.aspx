<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsProjectLineItemPurchaseOrderMoreInfo.aspx.cs" Inherits="Accounts_AccountsProjectLineItemPurchaseOrderMoreInfo" %>

<html>
<head>
    <title></title>
</head>
<body>
    <table>
        <tr valign="top">
            <td nowrap>
                <asp:Literal ID="ltInvoiceNo" runat="server" Text="Invoice Number"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lblInvoiceNo" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDINVOICENUMBER")%>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td>
                <asp:Literal ID="ltInvoiceStatus" runat="server" Text="Invoice Status"></asp:Literal>
            </td>
            <td>
               <asp:Label ID="lblInvoiceStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESTATUS") %>'></asp:Label>
            </td>
        </tr>
        <tr valign="top">
            <td nowrap>
                <asp:Literal ID="ltVoucherNumber" runat="server" Text="Voucher Number"></asp:Literal>
            </td>
            <td>
                 <asp:Label ID="lblVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></asp:Label>
            </td>
        </tr>
       
        
    </table>
</body>
</html>
