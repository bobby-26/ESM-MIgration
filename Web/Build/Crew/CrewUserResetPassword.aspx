<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewUserResetPassword.aspx.cs" Inherits="CrewUserResetPassword" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlHardEntry">
        <ContentTemplate>    
    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
        <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align:top">
                <eluc:Title runat="server" ID="ucTitle" Text="Reset Password" />
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:tabstrip id="MenuSecurityResetPassword" runat="server" ontabstripcommand="SecurityResetPassword_TabStripCommand">
                    </eluc:tabstrip>
        </div>
        <eluc:Status runat="server" ID="ucStatus" />
        <table cellpadding="8">
            <tr>
                <td colspan="2">
                    <asp:Literal ID="lblEntertheName" runat="server" Text="Enter the User Name, New Password and Confirm New Password to
                    reset the password."></asp:Literal>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblUserName" runat="server" Text="User Name"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtUserName" MaxLength="100" Width="250px" CssClass="input" Enabled="false"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblNewPassword" runat="server" Text="New Password"></asp:Literal>
                    
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtNewPassword" MaxLength="100" Width="250px" TextMode="Password" CssClass="input_mandatory"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblConfirmNewPassword" runat="server" Text="Confirm New Password"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtConfirmNewPassword" MaxLength="100" Width="250px" TextMode="Password" CssClass="input_mandatory"></asp:TextBox>
                </td>
            </tr>
        </table>
    </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>

</body>
</html>
