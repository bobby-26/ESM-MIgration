<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionCancelReasonUpdate.aspx.cs" Inherits="InspectionCancelReasonUpdate" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cancel Reason</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCancelReason" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuCancelReason" runat="server" OnTabStripCommand="MenuCancelReason_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1" width="75%">
            <tr>
                <td colspan="2">
                    <b>
                        <telerik:RadLabel ID="lblCancelReason" runat="server" Text="Cancel Reason"></telerik:RadLabel></b>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadTextBox  ID="txtCancelReason" runat="server" CssClass="input_mandatory" Height="100px" Rows="4"
                        TextMode="MultiLine" Width="100%" Resize="Both"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadLabel ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small"></telerik:RadLabel>
                </td>
            </tr>
        </table>
     </form>
</body>
</html>
