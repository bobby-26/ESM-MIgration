<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFormItemComment.aspx.cs"
    Inherits="PurchaseFormItemComment" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Line Item Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormItemComment" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnableScriptCombine="false" />
         <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <%--<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuLineItem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuLineItemDetail" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>--%>
        <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>

            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuLineItemDetail" runat="server" OnTabStripCommand="MenuLineItemDetail_TabStripCommand"></eluc:TabStrip>
            </div>
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
    
        
    <%--<br clear="all" />--%>
    
    <%--<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">--%>
          <%--<eluc:Custom ID="txtItemDetails" runat="server" Width="100%" Height="145px" PictureButton="true" DesgMode="true" HTMLMode="true" PrevMode="true" OnFileUploadEvent="btnInsertPic_Click" />--%>
        
    <%--</telerik:RadAjaxPanel>--%>
    </form>
</body>
</html>
