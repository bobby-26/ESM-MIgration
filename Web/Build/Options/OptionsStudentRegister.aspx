<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsStudentRegister.aspx.cs" Inherits="Options_OptionsStudentRegister" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <table class="loginpagebackground" width="100%" align="center" cellpadding="0" cellspacing="0"
        height="60px">
        <tr>
            <td align="left" valign="top">
                <font class="application_title"><asp:Literal ID="lblPhoenix" runat="server" Text="Phoenix"></asp:Literal></font>&nbsp;&nbsp;
            </td>
            <td align="right" valign="top">
                <font class="loginpage_companyname"><b><asp:Literal ID="lblManagement" runat="server"></asp:Literal></b></font>
                &nbsp;&nbsp;<img id="Img1" runat="server" style="vertical-align: top" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>"
                    alt="Phoenix" />&nbsp;
            </td>
        </tr>
    </table>
    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div class="divFloatLeft">
                <eluc:Title runat="server" ID="ucTitle" Text="Register" ShowMenu="false"></eluc:Title>
            </div>
            <div style="position:absolute; right:0px">
                <eluc:TabStrip ID="MenuSecurityChangePassword" runat="server" OnTabStripCommand="SecurityChangePassword_TabStripCommand">
            </eluc:TabStrip>
            </div>
        </div>
        <eluc:Status runat="server" ID="ucStatus" />
        <table cellpadding="8">
            <tr>
                <td colspan="2">
                    <b><asp:Literal ID="lblEnteryourRollNumberNewPasswordandConfirmNewPasswordtoRegister" runat="server" Text="Enter your Roll Number, New Password and Confirm New Password to Register."></asp:Literal></b>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblRollNumber" runat="server" Text="Roll Number"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtUserName" MaxLength="100" Width="360px" CssClass="input_mandatory"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblPassword" runat="server" Text="Password"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtNewPassword" MaxLength="100" Width="360px" TextMode="Password"
                        CssClass="input_mandatory"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblConfirmPassword" runat="server" Text="Confirm Password"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtConfirmNewPassword" MaxLength="100" Width="360px"
                        TextMode="Password" CssClass="input_mandatory"></asp:TextBox>
                </td>
            </tr>
        </table>
        <input type="button" runat="server" id="isouterpage" name="isouterpage" style="visibility: hidden" />
    </form>
</body>
</html>
