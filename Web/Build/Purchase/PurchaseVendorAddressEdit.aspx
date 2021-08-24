<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseVendorAddressEdit.aspx.cs" Inherits="PurchaseVendorAddressEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit Your Address</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <script type="text/javascript">
        function resize() {
            var obj = document.getElementById("filterandsearch");
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) + "px";

        }
    </script> 
</telerik:RadCodeBlock></head>
<body onload="resize()" onresize="resize()">
    <form id="frmVendorEdit" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="RadScriptManager" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
            <iframe runat="server" id="filterandsearch" width="100%" frameborder="0" height="100%">
            </iframe>
    </form>
</body>
</html>
