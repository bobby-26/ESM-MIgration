<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreCancelAppointmentReasonUpdate.aspx.cs" Inherits="CrewOffshoreCancelAppointmentReasonUpdate" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cancel Reason Update</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCancelReason" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuCancelReason" runat="server" OnTabStripCommand="MenuCancelReason_TabStripCommand"></eluc:TabStrip>
        <br />
        <table cellpadding="1" cellspacing="1" width="30%">
            <tr>
                <td>
                    <b>
                        <telerik:RadLabel ID="lblCancellationDate" runat="server" Text="Cancellation Date"></telerik:RadLabel></b>
                </td>
                <td>
                    <eluc:Date ID="ucDate" runat="server" CssClass="input_mandatory" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <b>
                        <telerik:RadLabel ID="lblCancelReason" runat="server" Text="Cancellation Reason"></telerik:RadLabel></b>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <telerik:RadTextBox ID="txtCancelReason" runat="server" CssClass="input_mandatory" Height="50px" Rows="4"
                        TextMode="MultiLine" Width="97%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small"></asp:Label>
                </td>
            </tr>
        </table>
      
            
       
    </form>
</body>
</html>
