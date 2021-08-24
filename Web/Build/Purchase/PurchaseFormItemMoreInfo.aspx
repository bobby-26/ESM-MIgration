<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFormItemMoreInfo.aspx.cs"
    Inherits="PurchaseFormItemMoreInfo" EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remarks</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseFormItemComment" runat="server" autocomplete="off">
<telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnableScriptCombine="false" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />        
        <div class="navigation" id="navigation" style="width:100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                 <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
            <div style="font-weight:600;font-size:12px">
                <eluc:TabStrip ID="MenuLineItemDetail" runat="server" OnTabStripCommand="MenuLineItemDetail_TabStripCommand">
                </eluc:TabStrip>
            </div>
            <div>
             <%--<eluc:Custom ID="txtItemDetails" runat="server" Width="100%" Height="275px" PictureButton="false" TextOnly="true"  DesgMode="true" HTMLMode="true" PrevMode="true" OnFileUploadEvent="btnInsertPic_Click" />--%>
           <%--<eluc:Custom ID="txtItemDetails" runat="server" Width="100%" Height="145px" PictureButton="true" DesgMode="true" HTMLMode="true" PrevMode="true" OnFileUploadEvent="btnInsertPic_Click" />--%>
        <telerik:RadEditor ID="txtItemDetails" runat="server" Width="100%" Height="400px" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
            <ImageManager ViewPaths="~/Attachments/Purchase/Editor"
                    UploadPaths="~/Attachments/Purchase/Editor"
                    EnableAsyncUpload="true"></ImageManager>
        </telerik:RadEditor>
            </div>
        </div>
    </form>
</body>
</html>
