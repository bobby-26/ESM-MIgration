<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardInfoEPSSInsert.aspx.cs" Inherits="Dashboard_DashboardInfoEPSSInsert" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function htmlEnc(s) {
                return s.replace(/</g, '&lt;')
                    .replace(/>/g, '&gt;')
                    .replace(/"/g, '&#34;');
            }            
            function onSubmitBefore() {
                var help = $find('<%=txtinformation.ClientID%>');
                help.set_value(htmlEnc(help.get_value()));                
            }            
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadSkinManager runat="server"></telerik:RadSkinManager>
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuDashboard" runat="server" OnTabStripCommand="MenuDashboard_TabStripCommand" TabStrip="false"></eluc:TabStrip>
            <table width="50%">
                <tr>
                    <td>

                        <telerik:RadLabel runat="server" ID="RadLabel1" Text="Dashboard"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ddlconfigid" Width="180px"></telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>

                        <telerik:RadLabel runat="server" ID="lblinformation" Text="Information"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtinformation" Width="400px" Rows="7" TextMode="MultiLine"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>

                        <telerik:RadLabel runat="server" ID="lbllink" Text="EPSS Link"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtlink" Width="360px"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>

                        <telerik:RadLabel runat="server" ID="lblHelpLink" Text="Help Link"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtHelpLink" Width="360px"></telerik:RadTextBox>
                    </td>

                </tr>
            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
