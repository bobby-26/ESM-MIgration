<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowProcessGroupTargetAdd.aspx.cs" Inherits="WorkflowProcessGroupTargetAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ProcessTarget" Src="~/UserControls/UserControlWFProcessTarget.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Process" Src="~/UserControls/UserControlWFProcess.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ProcessGroup" Src="~/UserControls/UserControlWFProcessGroup.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow - Process Group Target Add</title>
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
            <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
            </telerik:RadWindowManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuWorkflowProcessGroupTargetAdd" runat="server" OnTabStripCommand="MenuWorkflowProcessGroupTargetAdd_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table style="margin-left: 20px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProcess" runat="server" Text="Process"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Process ID="UcProcess" runat="server" AutoPostBack="true" AppendDataBoundItems="true" Width="120px" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Group"></telerik:RadLabel>
                    </td>
                    <td>

                        <eluc:ProcessGroup ID="UcProcessGroupAdd" runat="server" AutoPostBack="True" Width="150px" CssClass="input_mandatory" />
                    </td>

                </tr>
                 <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Target"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ProcessTarget ID="UcProcessTargetAdd" runat="server" AutoPostBack="True" Width="150px" CssClass="input_mandatory" />
                    </td>

                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>

</body>
</html>
