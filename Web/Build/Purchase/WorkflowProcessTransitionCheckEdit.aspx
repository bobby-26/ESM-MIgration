<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowProcessTransitionCheckEdit.aspx.cs" Inherits="WorkflowProcessTransitionCheckEdit" %>

<!DOCTYPE html>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow -Transition Check Edit</title>
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

            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
             <eluc:TabStrip ID="MenuWorkFlowTransitionCheckEdit" runat="server" OnTabStripCommand="MenuWorkFlowTransitionCheckEdit_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table style="margin-left: 20px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProcess" runat="server" Text="Process"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>                                                               
                        <telerik:RadLabel runat="server" ID="txtProcess" Text="" Width="180px"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTransition" runat="server" Text="Transition"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" ID="txtTransition" Text="" Width="180px"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Short Code"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" ID="txtShortCode" Text="" Width="180px"></telerik:RadLabel>
                    </td>
                </tr>
                  <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Name"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtName" Text="" Width="180px"></telerik:RadTextBox>
                    </td>
                </tr>
                </table>


        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
