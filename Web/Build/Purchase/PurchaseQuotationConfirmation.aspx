<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationConfirmation.aspx.cs"
    Inherits="PurchaseQuotationConfirmation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnableScriptCombine="false" />
    <div>
        <table cellspacing="5" cellpadding="5">
            <tr>
                <td id="Td1" width="100%" valign="top">
                    <h1 id="H1" style="font: 14pt/16pt verdana; color: #0000ff"> <asp:Literal ID="lblThankyouforsubmittingthebid" runat="server" Text="Thank you for submitting the bid."></asp:Literal></h1>
                </td>
            </tr>
            <tr>
                <td id="tableProps" width="100%" valign="top">
                 <asp:Label ID="lbltext" runat="server" > </asp:Label>
                </td>
            </tr>
        </table> 
    </div>
    </form>
</body>
</html>
