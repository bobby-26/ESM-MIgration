<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFormItemComponentDetails.aspx.cs"
    Inherits="PurchaseFormItemComponentDetails" EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseFormItemComponentDetails" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
        
        <div class="navigation" id="navigation" style="width:100%">
            <div class="subHeader" style="position: relative">
                <div id="divHeading" style="vertical-align: top">
                    <eluc:Title runat="server" ID="Title1" Text="Component Details" ShowMenu="false"></eluc:Title>
                </div>
                 <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                 <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
            </div>
            <div>
                <asp:TextBox ID="txtComponentDetails" runat="server" CssClass="input" 
                    TextMode="MultiLine" MaxLength="800"
                    Height="424px" Width="99%"></asp:TextBox>
            </div>
        </div>
        
        
        
    </form>
</body>
</html>
