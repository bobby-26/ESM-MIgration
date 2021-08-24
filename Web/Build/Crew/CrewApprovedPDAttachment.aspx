<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewApprovedPDAttachment.aspx.cs"
    Inherits="CrewApprovedPDAttachment" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmCrew" Src="~/UserControls/UserControlConfirmMessageCrew.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew PD attachment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>   
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewPDAttachment" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnableScriptCombine="false"></telerik:RadScriptManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server"></telerik:RadAjaxManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />        
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuPDAttachment" runat="server" OnTabStripCommand="PDAttachment_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAttachmentType" runat="server" Text="Attachment Type"></telerik:RadLabel>
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
            <telerik:RadLabel ID="lblNote" runat="server" ForeColor="Blue" Text="" Visible="false"></telerik:RadLabel>
            <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 500px; height: 800px; width: 100%"></iframe>
            <eluc:ConfirmCrew ID="ucConfirmCrew" runat="server" OnConfirmMesage="btnCrewApprove_Click"
                OKText="Proceed" CancelText="Cancel" Visible="false" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
