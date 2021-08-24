<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAddressEmail.aspx.cs" Inherits="RegistersAddressEmail" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Emails of the address</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAddressEmail" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <div>
        <telerik:RadTextBox runat="server" ID="txtEmails" CssClass="input" ReadOnly="true" TextMode="MultiLine" Height="100px" Width="100%"></telerik:RadTextBox>
    </div>
    </form>
</body>
</html>
