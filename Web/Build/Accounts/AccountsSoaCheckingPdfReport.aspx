<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSoaCheckingPdfReport.aspx.cs"
    Inherits="AccountsSoaCheckingPdfReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Soa Pdf</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <div id="DivHeader" runat="server">
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        </div>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <div>
            <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
        </div>
        <div>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 750px; width: 100%"
                frameborder="0"></iframe>
        </div>
    </form>
</body>
</html>
