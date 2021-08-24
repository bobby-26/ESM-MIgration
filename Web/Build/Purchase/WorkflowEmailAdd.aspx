<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowEmailAdd.aspx.cs" Inherits="WorkflowEmailAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow - Email</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:TabStrip ID="MenuWorkflowEmail" runat="server" OnTabStripCommand="MenuWorkflowEmail_TabStripCommand" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table style="margin-left: 20px">            
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTo" runat="server" CssClass="input_mandatory" Width="180px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel6" runat="server" Text="Cc"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCC" runat="server" CssClass="input_mandatory" Width="180px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Text="Subject"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSubject" runat="server" Width="850px"  CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel5" runat="server" Text="Body"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtbody" runat="server" Resize="Both" Width="850px" TextMode="MultiLine" Rows="8" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
