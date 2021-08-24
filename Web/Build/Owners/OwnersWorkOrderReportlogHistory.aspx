<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersWorkOrderReportlogHistory.aspx.cs"
    Inherits="Owners_OwnersWorkOrderReportlogHistory"%>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title></title>

        <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
       </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Literal ID="lblHistory" runat="server" Text="History"></asp:Literal>
        </div>
    </div>
    <div>
        <table width="100%">
            <tr>
                <td>
                    <asp:Label ID="lblJobdescription" runat="server" Text='History' Font-Size="Small" Visible =false e></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <eluc:Custom ID="txtJobDetail" runat="server" Width="90%" Height="260px" PictureButton="false"
                        DesgMode="true" HTMLMode="false" PrevMode="true" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
