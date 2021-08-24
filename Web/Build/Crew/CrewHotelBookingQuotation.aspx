<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewHotelBookingQuotation.aspx.cs" Inherits="CrewHotelBookingQuotation" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Phoenix RFQ</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function resize() {
            var obj = document.getElementById("filterandsearch");
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) + "px";

        }
    </script>
</head>
<body onload="resize()" onresize="resize()">
    <form id="frmHotelBookingQuoteItems" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <iframe runat="server" id="filterandsearch" width="100%" frameborder="0" height="100%"></iframe>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
