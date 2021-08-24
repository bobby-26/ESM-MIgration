<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowRequestFieldAdd.aspx.cs" Inherits="WorkflowRequestFieldAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Process" Src="~/UserControls/UserControlWFProcess.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow Process Request Field Add</title>
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
            <eluc:TabStrip ID="MenuWorkflowRequestFieldAdd" runat="server" OnTabStripCommand="MenuWorkflowRequestFieldAdd_TabStripCommand" TabStrip="true">

            </eluc:TabStrip>
            <table style="margin-left: 20px">
                 <tr>
                    <td>
                        <telerik:RadLabel ID="lblProcess" runat="server" Text="Process"></telerik:RadLabel>
                    
                    </td>
                    <td>
                        <eluc:Process ID="UcProcess" runat="server" AutoPostBack="true" AppendDataBoundItems="true" Width="120px" />
                    </td>

                </tr>
                <tr>
                     <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Field Name"></telerik:RadLabel>
                    
                    </td>
                     <td>
                        <telerik:RadTextBox ID="txtFieldName" runat="server" Text=""></telerik:RadTextBox>
                    
                    </td>
                </tr>
                 <tr>
                     <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Text="Data Type"></telerik:RadLabel>
                    
                    </td>
                     <td>
                        <telerik:RadTextBox ID="txtDataType" runat="server" Text=""></telerik:RadTextBox>
                    
                    </td>
                </tr>
                 <tr>
                     <td>
                        <telerik:RadLabel ID="RadLabel3" runat="server" Text="Length"></telerik:RadLabel>
                    
                    </td>
                     <td>
                        <telerik:RadTextBox ID="txtLength" runat="server" Text=""></telerik:RadTextBox>
                    
                    </td>
                </tr>
                  <tr>
                     <td>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Text="Default"></telerik:RadLabel>
                    
                    </td>
                     <td>
                        <telerik:RadTextBox ID="txtDefault" runat="server" Text=""></telerik:RadTextBox>
                    
                    </td>
                </tr>

            </table>


            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
