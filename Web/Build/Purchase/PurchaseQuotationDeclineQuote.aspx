<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationDeclineQuote.aspx.cs" Inherits="PurchaseQuotationDeclineQuote" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Decline Quote</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
       <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/jquery-1.12.4.min.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmDeliveryInstruction" runat="server">    
    <telerik:RadScriptManager ID="Radscriptmanager1" runat="server" EnableScriptCombine="false"></telerik:RadScriptManager>
         <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
    <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 100%">
        <div style="font-weight:600;font-size:12px" runat="server">
                <eluc:TabStrip ID="MenuDeclineQuote" runat="server" OnTabStripCommand="MenuDeclineQuote_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
        <br clear="all" />
        <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtDeclineQuote" CssClass="input_mandatory" TextMode="MultiLine"
         Width="95%" Height="200px"></telerik:RadTextBox>
    </div>
    </form>
</body>
</html>
