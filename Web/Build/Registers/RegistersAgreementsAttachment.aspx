<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAgreementsAttachment.aspx.cs"
    Inherits="RegistersAgreementsAttachment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Agreements</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAgreementAttachment" runat="server">
    <div>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

                   <eluc:TabStrip ID="MenuAgreementsAttachment" runat="server" OnTabStripCommand="AgreementsAttachment_TabStripCommand" TabStrip="true"></eluc:TabStrip>

                    <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 500px; height: 800px;
                        width: 100%"></iframe> 
     </div>
    </form>
</body>
</html>
