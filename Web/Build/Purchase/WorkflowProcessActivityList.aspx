<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowProcessActivityList.aspx.cs" Inherits="WorkflowProcessActivityList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ProcessActivity" Src="~/UserControls/UserControlWFProcessActivity.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow - Process Transition Activity Add</title>
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
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuWFTransitionActivityList" runat="server" OnTabStripCommand="MenuWFTransitionActivityList_TabStripCommand"></eluc:TabStrip>

            <table style="margin-left: 20px" >
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Activity"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <eluc:ProcessActivity ID="UcProcessActivity" runat="server" AutoPostBack="True" Width="150px" CssClass="input_mandatory" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel7" runat="server" Text="Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" runat="server" TextMode="MultiLine" Rows="8" Text="" Width="240px" Resize="Both" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
