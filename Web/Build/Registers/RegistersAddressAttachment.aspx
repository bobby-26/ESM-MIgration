<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAddressAttachment.aspx.cs" Inherits="RegistersAddressAttachment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Address Attachment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <div id="DivHeader" runat="server">
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        </div>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAddressAttachment" runat="server">
        <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlAddressAttachment">
            <ContentTemplate>
                <eluc:Title runat="server" ID="ucTitle" Text="Address Attachment" Visible="false"></eluc:Title>
                <eluc:TabStrip ID="MenuAddressAttachment" runat="server" OnTabStripCommand="AddressAttachment_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblAttachmentType" runat="server" Text="Attachment Type"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblAttachmentType" runat="server" AppendDataBoundItems="false"
                                RepeatDirection="Horizontal" AutoPostBack="true" RepeatLayout="Table" OnSelectedIndexChanged="SetValue"
                                CssClass="readonlytextbox" Enabled="true">
                                <asp:ListItem Selected="True"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 500px; height: 800px; width: 100%"></iframe>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
