<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationItems.aspx.cs" Inherits="PurchaseQuotationItems" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Phoenix RFQ</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="shortcut icon" href="../css/Theme1/images/favicon.png" />        
        <link rel="stylesheet" type="text/css" href="../css/Theme1/phoenix.telerik.css" />   
        <link rel="stylesheet" type="text/css" href="../fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="../css/Theme1/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="../js/js_globals.aspx"></script>
    </div>
    <script type="text/javascript">
        function resize() {
            var obj = document.getElementById("filterandsearch");
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) + "px";

        }
        </script> 
</telerik:RadCodeBlock></head>
<body onload="resize()" onresize="resize()">
    <form id="frmPurchaseQuotationItems" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="Radscriptmanager1" runat="server" EnableScriptCombine="false">
            <Scripts>                
                <asp:ScriptReference Path="~/js/phoenix.js" />
            </Scripts>
        </telerik:RadScriptManager>
         <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadWindowManager ID="phoenixPopup" runat="server" EnableShadow="true" RenderMode="Lightweight" ShowContentDuringLoad="true"
            Behaviors="Close, Move, Resize, Maximize, Minimize" DestroyOnClose="true" Opacity="99"
            Height="500" Modal="true" VisibleStatusbar="false" Width="800" >  
        </telerik:RadWindowManager>
            <iframe runat="server" id="filterandsearch" width="100%" frameborder="0" height="100%">
            </iframe>
            <%--<asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />--%>
    </form>
</body>
</html>
