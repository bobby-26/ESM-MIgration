<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockExport2XL.aspx.cs" Inherits="DryDock_DryDockExport2XL" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title></title>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

  </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1> <asp:Literal ID="lblPleaseWaitExportingExcel" runat="server" Text="Please Wait! Exporting Excel."></asp:Literal></h1>
    </div>
    </form>
</body>
</html>
