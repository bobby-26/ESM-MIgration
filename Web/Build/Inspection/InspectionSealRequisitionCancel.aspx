<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSealRequisitionCancel.aspx.cs" Inherits="InspectionSealRequisitionCancel" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cancel Reason Update</title>
    <telerik:RadCodeBlock runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCancelReason" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
       
         <div  style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuCancelReason" runat="server" OnTabStripCommand="MenuCancelReason_TabStripCommand"></eluc:TabStrip>
        </div>
        <table cellpadding="1" cellspacing="1" width="50%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCancelReason" runat="server" Text="Cancel Reason"></telerik:RadLabel>
                </td>
                <td>
                    <asp:TextBox ID="txtCancelReason" runat="server" CssClass="input_mandatory" Height="50px" Rows="4"
                        TextMode="MultiLine" Width="97%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCancelledBy" runat="server" Text="Cancelled By"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtName" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="110px"></telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtDesignation" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="90px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCancelledDate" runat="server" Text="Cancelled Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucCancelDate" runat="server" ReadOnly="true" CssClass="readonlytextbox" />
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
