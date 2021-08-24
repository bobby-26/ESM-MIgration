<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowProcessAdd.aspx.cs" Inherits="WorkflowProcessAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow - Process Add</title>
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
            <eluc:TabStrip ID="MenuWorkflowProcessAdd" runat="server" OnTabStripCommand="MenuWorkflowProcessAdd_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table style="margin-left: 20px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUniqueName" runat="server" Text="Unique Name" ></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUniqueName" runat="server" Text="" CssClass="input_mandatory" Width="120px"></telerik:RadTextBox>&nbsp;&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Process Name" ></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" Text=""  CssClass="input_mandatory" Width="160px"></telerik:RadTextBox>&nbsp;&nbsp;
                    </td>
                </tr>

               

                  <tr>
                    <td>
                        <telerik:RadLabel ID="lblProcedureName" runat="server" Text="Procedure Name" ></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtProcedureName" runat="server" Text=""  CssClass="input_mandatory" Width="160px"></telerik:RadTextBox>&nbsp;&nbsp;
                    </td>
                </tr>

                 <tr>
                    <td>
                        <telerik:RadLabel ID="lblAdministrator" runat="server" Text="Administrator" ></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAdministrator" runat="server" Text=""  CssClass="input_mandatory" Width="120px"></telerik:RadTextBox>&nbsp;&nbsp;
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Description" ></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDescription" runat="server" Text="" TextMode="MultiLine" Width="240px" Rows="6" CssClass="input_mandatory"></telerik:RadTextBox>&nbsp;&nbsp;
                    </td>
                </tr>
            </table>



        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
