<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogGRB2BookEntryDelete.aspx.cs" Inherits="Log_ElectricLogGRB2BookEntryDelete" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <title>Log Book Delete</title>
      <telerik:radcodeblock id="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:radcodeblock>
    <style>
        .container {
            padding: 20px 20px; 
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div>
         <telerik:radscriptmanager runat="server" id="RadScriptManager1" />
        <telerik:radskinmanager id="RadSkinManager1" runat="server" />

        <telerik:radwindowmanager rendermode="Lightweight" id="RadWindowManager1" runat="server" enableshadow="true">
        </telerik:radwindowmanager>
        <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <div class="container">
            <h3><telerik:RadLabel Text="Enter your login to delete the log entry" ID="lblheading" runat="server"></telerik:RadLabel></h3>
            <table>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblRankId" runat="server" Visible="false"></telerik:RadLabel>
                    <telerik:RadLabel ID="lblRank" runat="server" Visible="false"></telerik:RadLabel>
                    <telerik:RadLabel ID="lblName" runat="server" Visible="false"></telerik:RadLabel>
                    <telerik:RadLabel ID="lblUsername" runat="server" Text="Username"></telerik:RadLabel></td>
                <td><telerik:RadTextBox ID="txtUsername" runat="server" CssClass="input_mandatory"></telerik:RadTextBox></td>
                <td><telerik:RadLabel ID="txtUserCode" runat="server" Visible="false"></telerik:RadLabel></td>
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lblPassword" runat="server" Text="Password"></telerik:RadLabel></td>
                <td colspan="2"><telerik:RadTextBox ID="txtPassword" TextMode="Password" runat="server" CssClass="input_mandatory"></telerik:RadTextBox></td>
            </tr>
            <tr>
                <td></td>
                <td colspan="2">
                    <telerik:RadButton ID="btnLogin" OnClick="cmdLogin_Click" runat="server" Text="Login"></telerik:RadButton>
                </td>
              
            </tr>
            <tr>
                <td><telerik:RadLabel ID="lblLoggedUser" runat="server" Visible="false"></telerik:RadLabel></td>
            </tr>
        </table>
        </div>
    </div>
    </form>
</body>
</html>