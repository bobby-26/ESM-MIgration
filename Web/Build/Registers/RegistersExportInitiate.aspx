<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersExportInitiate.aspx.cs" Inherits="Registers_RegistersExportInitiate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:TextBox runat="server" ID="txtSearch" Text="A"></asp:TextBox>
        <asp:TextBox runat="server" ID="txtAbbreviation" Text=""></asp:TextBox>
        <asp:Button runat="server" ID="cmdExport" PostBackUrl="~/Registers/RegistersExportToExcel.aspx" />
    </div>
    </form>
</body>
</html>
