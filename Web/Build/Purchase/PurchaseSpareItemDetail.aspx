<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseSpareItemDetail.aspx.cs"
    Inherits="PurchaseSpareItemDetail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Item Details</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnableScriptCombine="false"/>
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div>
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadEditor ID="txtItemDetails" runat="server" Width="100%" Height="400px" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
                            <ImageManager ViewPaths="~/Attachments/Purchase/Editor"
                                UploadPaths="~/Attachments/Purchase/Editor"
                                EnableAsyncUpload="true"></ImageManager>
                        </telerik:RadEditor>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
