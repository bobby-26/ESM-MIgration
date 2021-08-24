<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountAdminLeaveWages.aspx.cs"
    Inherits="VesselAccountAdminLeaveWages" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery.min.js"></script>

    </telerik:RadCodeBlock>

</head>

<body>
    <table style="border-collapse: collapse; width: 100%;" border="1" cellspacing="2" id="tbl">
        <%=BindData("2")%>
    </table>
    <table style="border-collapse: collapse; width: 100%;" border="1" cellspacing="2">

        <%=BindData("1")%>
    </table>
</body>
</html>
