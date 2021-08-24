<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsRemitanceEmpBankDetails.aspx.cs"
    Inherits="AccountsRemitanceEmpBankDetails" %>

<html>
<head>
    <style type="text/css">
        .lblheader {
            font-weight: bold;
        }
    </style>
</telerik:RadCodeBlock></head>
<body>
    <table style="width: 100%;">
        <tr>
            <td colspan="6" style="align-items: center;"><b>Bank Details</b> </td>
        </tr>
        <tr valign="top">
            <td class="lblheader">
                <asp:Literal ID="lblBeneficiary" runat="server" Text="Beneficiary"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltBeneficiary" runat="server"></asp:Label>
            </td>
            <td class="lblheader">
                <asp:Literal ID="lblAccountNumber" runat="server" Text="Account Number"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltAccountNumber" runat="server"></asp:Label>
            </td>
            <td class="lblheader">
                <asp:Literal ID="lblBank" runat="server" Text="Bank"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltBank" runat="server"></asp:Label>
            </td>
        </tr>
        <tr valign="top">

            <td class="lblheader">
                <asp:Literal ID="lblSwiftCode" runat="server" Text="Swift Code"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltSwiftCode" runat="server"></asp:Label>
            </td>
            <td class="lblheader">
                <asp:Literal ID="lblIFSCCode" runat="server" Text="IFSC Code"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltIFSCCode" runat="server"></asp:Label>
            </td>
            <td class="lblheader">
                <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
            </td>
            <td>
                <asp:Label ID="lbltCurrency" runat="server"></asp:Label>
            </td>
        </tr>

        <tr valign="top">
            <td class="lblheader">
                <asp:Literal ID="lblAddress" runat="server" Text="Address"></asp:Literal>
            </td>
            <td colspan="5">
                <asp:Label ID="lbltAddress" runat="server"></asp:Label>
            </td>
        </tr>

    </table>
</body>
</html>
