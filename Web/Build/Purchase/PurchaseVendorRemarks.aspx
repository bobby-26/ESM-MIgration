<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseVendorRemarks.aspx.cs"
    Inherits="PurchaseVendorRemarks"  %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PO Confirmation</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormRemarks" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnableScriptCombine="false" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <div style="height: 60px;" class="pagebackground">
            <div style="position: absolute; top: 15px;">
                <img id="Img1" runat="server" style="vertical-align: middle" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>"
                    alt="Phoenix" onclick="parent.hideMenu();" />
                <span class="title" style="color: White">
                    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                        <%=Application["softwarename"].ToString() %>
                    </telerik:RadCodeBlock>

                </span>
                <br />
            </div>
        </div>
        <eluc:TabStrip ID="MenuFormRemarks" runat="server" OnTabStripCommand="MenuFormRemarks_TabStripCommand" TabStrip="false"></eluc:TabStrip>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>


        <br clear="all" />
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td width="20%">
                    <telerik:RadLabel ID="lblDeliveryDate" runat="server" Text="Delivery Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="txtDeliveryDate" runat="server" Width="120px" />
                </td>
            </tr>
            <tr>
                <td width="20%">
                    <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>

                </td>
                <td>
                    <%--<Custom:CustomEditor ID="txtRemarks" runat="server"  CssClass="input" Style="width:300px; height:20px;"/>--%>
                    <telerik:RadEditor ID="txtRemarks" runat="server" Width="100%" Height="400px" RenderMode="Lightweight" SkinID="DefaultSetOfTools">
                        <ImageManager ViewPaths="~/Attachments/Purchase/Editor"
                            UploadPaths="~/Attachments/Purchase/Editor"
                            EnableAsyncUpload="true"></ImageManager>
                    </telerik:RadEditor>
                </td>
            </tr>
        </table>

        <div>
            <input type="button" runat="server" id="isouterpage" name="isouterpage" style="visibility: hidden" />
        </div>
    </form>
</body>
</html>
