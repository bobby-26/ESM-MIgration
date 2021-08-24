<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseVendorDetail.aspx.cs"
    Inherits="PurchaseVendorDetail" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remarks</title>    
    <telerik:RadCodeBlock ID="RadCodeBlock" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormDetail" runat="server" autocomplete="off">
    <%--<ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>--%>
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnableScriptCombine="false" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuFormDetail">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuFormDetail" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
     <%-- <asp:UpdatePanel runat="server" ID="pnlFormDetails">
        <ContentTemplate>--%>
    
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">    
        <div style="font-weight:600;font-size:12px" runat="server">
            <eluc:TabStrip ID="MenuFormDetail" runat="server" OnTabStripCommand="MenuFormDetail_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>             
        <%--<eluc:Custom ID="txtFormDetails" runat="server" Width="100%" Height="275px" PictureButton="false" TextOnly="true"  DesgMode="true" HTMLMode="true" PrevMode="true" OnFileUploadEvent="btnInsertPic_Click" />--%>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
          <%--<eluc:Custom ID="txtItemDetails" runat="server" Width="100%" Height="145px" PictureButton="true" DesgMode="true" HTMLMode="true" PrevMode="true" OnFileUploadEvent="btnInsertPic_Click" />--%>
        <telerik:RadEditor ID="txtFormDetails" runat="server" Width="100%" Height="400px" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
            <ImageManager ViewPaths="~/Attachments/Purchase/Editor"
                    UploadPaths="~/Attachments/Purchase/Editor"
                    EnableAsyncUpload="true"></ImageManager>
        </telerik:RadEditor>
    </telerik:RadAjaxPanel>

   </div>

    </form>
</body>
</html>
