<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowRequestFieldEdit.aspx.cs" Inherits="WorkflowRequestFieldEdit" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow - Request Field Edit</title>
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
            <eluc:TabStrip ID="MenuWFRequestFieldEdit" runat="server" OnTabStripCommand="MenuWFRequestFieldEdit_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table style="margin-left: 20px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProcess" runat="server" Text="Process"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" ID="txtprocess" Text="" Width="120px"></telerik:RadLabel>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="FieldName"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFieldName" Text="" Width="120px"></telerik:RadTextBox>
                    </td>
                </tr>
                  <tr>
                    <td>
                         <telerik:RadLabel ID="RadLabel3" runat="server" Text="DataType"></telerik:RadLabel>                                     
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtDataType" Text="" Width="120px"></telerik:RadTextBox>
                    </td>
                </tr>
                  <tr>
                    <td>
                         <telerik:RadLabel ID="RadLabel2" runat="server" Text="Length"></telerik:RadLabel>
                       
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLength" Text="" Width="120px"></telerik:RadTextBox>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Text="Default"></telerik:RadLabel>
                     
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtDefault" Text="" Width="120px"></telerik:RadTextBox>
                    </td>
                </tr>
                </table>
              
      </telerik:RadAjaxPanel>
           </form>
</body>
</html>
