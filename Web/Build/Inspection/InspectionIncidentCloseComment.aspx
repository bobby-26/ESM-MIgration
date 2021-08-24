<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentCloseComment.aspx.cs" Inherits="Inspection_InspectionIncidentCloseComment" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Close Comments</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDirectorComment" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuCloseComment" runat="server" OnTabStripCommand="MenuCloseComment_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1" width="50%">
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblCloseComment" runat="server" Text="Close out remarks"></telerik:RadLabel>
                    </b>
                </td>
                <td colspan="2">
                    <telerik:RadTextBox ID="txtcloseComments" runat="server" CssClass="input_mandatory" Height="100px" Rows="4"
                        TextMode="MultiLine" Width="100%" Resize="Both">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblCloseCommentDate" runat="server" Text="Closed Date"></telerik:RadLabel>
                </td>
                <td colspan="2">
                    <eluc:Date ID="ucCloseCommentDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblCloseCommentedBy" runat="server" Text="Closed By"></telerik:RadLabel>
                </td>
                <td colspan="2">
                    <telerik:RadTextBox ID="txtCloseCommentedByName" runat="server" CssClass="readonlytextbox"
                        Enabled="false" Width="270px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <telerik:RadLabel ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small" Width="150%"></telerik:RadLabel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>

