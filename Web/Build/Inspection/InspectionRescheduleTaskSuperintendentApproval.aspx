﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRescheduleTaskSuperintendentApproval.aspx.cs" Inherits="InspectionRescheduleTaskSuperintendentApproval" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Superintendent Approval</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">

        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDirectorComment" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" Localization-OK="Yes" Localization-Cancel="No" Width="100%">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuSuperintendentComment" runat="server" OnTabStripCommand="MenuSuperintendentComment_TabStripCommand"></eluc:TabStrip>

        <table width="50%">
            <tr>
                <td width="10%">
                    <b>
                        <telerik:RadLabel ID="lblSuperintendentComments" runat="server" Text="Superintendent Comments"></telerik:RadLabel>
                </td>
                <td width="85%">
                    <telerik:RadTextBox ID="txtSuperintendentComments" runat="server" CssClass="input_mandatory" Height="80px" Rows="4"
                        TextMode="MultiLine" Width="100%" Resize="Both"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblRescheduleDate" runat="server" Text="Reschedule Date"></telerik:RadLabel>
                </td>
                <td colspan="2">
                    <eluc:Date ID="ucRescheduleDate" runat="server" DatePicker="true" CssClass="input_mandatory" />
                </td>
            </tr>
            <%--        <tr>
             <td colspan="2">
                 <telerik:RadLabel ID="lblApprovedDate" runat="server" Text="Approved Date"></telerik:RadLabel>
             </td>
             <td colspan="2">
                 <eluc:Date ID="ucApprovedDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
             </td>
        </tr>--%>
            <%--        <tr>
             <td colspan="2">
                 <telerik:RadLabel ID="lblApprovedBy" runat="server" Text="Approved By"></telerik:RadLabel>
             </td>
             <td colspan="2">
                 <telerik:RadTextBox ID="txtApprovedByName" runat="server"  CssClass="readonlytextbox"
                     Enabled="false"></telerik:RadTextBox>
             </td>
        </tr> --%>
            <tr>
                <td colspan="4">
                    <telerik:RadLabel  ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small" Width="150%"></telerik:RadLabel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
