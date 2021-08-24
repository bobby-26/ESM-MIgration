<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseVendorRemark.aspx.cs" Inherits="PurchaseVendorRemark" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PO Confirmation</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        function resize() {
            var obj = document.getElementById("ifVendorRemarks");
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) + "px";
        }
    </script>
    </telerik:RadCodeBlock>

</head>
<body onload="resize()" onresize="resize()">
    <form id="frmVendorEdit" runat="server" autocomplete="off">
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnableScriptCombine="false" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
            <iframe runat="server" id="ifVendorRemarks" width="100%" frameborder="0" height="100%">
            </iframe>
    </form>
</body>
</html>
