<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFormDetail.aspx.cs"
    Inherits="PurchaseFormDetail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Details</title>    
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormDetail" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnableScriptCombine="false"/>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuFormDetail">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuFormDetail" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
     
        <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top;
                width: 100%">    
            <div style="font-weight: 600; font-size: 12px;" runat="server">             
            <eluc:TabStrip ID="MenuFormDetail" runat="server" OnTabStripCommand="MenuFormDetail_TabStripCommand" Title="Details">
             </eluc:TabStrip>
                </div>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
            <%--<eluc:Custom ID="txtFormDetails" runat="server" Width="100%" Height="275px" PictureButton="false" TextOnly="true"  DesgMode="true" HTMLMode="true" PrevMode="true" OnFileUploadEvent="btnInsertPic_Click" />--%>
            <telerik:RadEditor ID="txtFormDetails" runat="server" Width="100%" Height="400px" RenderMode="Lightweight" SkinID="DefaultSetOfTools" 
                 StripFormattingOptions="MsWord,Span,Css,ConvertWordLists" EditModes="All">
                
            <ImageManager ViewPaths="~/Attachments/Purchase/Editor"
                    UploadPaths="~/Attachments/Purchase/Editor"
                    EnableAsyncUpload="true"></ImageManager>
        </telerik:RadEditor>
            </div>
    </form>
</body>
</html>
