<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PortalStudentDefault.aspx.cs" Inherits="Portal_PortalStudentDefault" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>    
        <%=Application["softwarename"].ToString() %>
        - <%=Session["companyname"]%></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">  
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off" >
    <table class="loginpagebackground" width="80%" align="center" cellpadding="0" cellspacing="0"
        height="60px">
        <tr>
            <td align="left" valign="top">
                    &nbsp;&nbsp;<img id="Img1"
                    runat="server" style="vertical-align: top" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>"
                    alt="Phoenix" onclick="showDatabaseChooser();" />&nbsp;            
                &nbsp;&nbsp;<font class="application_title"><asp:Literal runat="server" ID="litTitle" Text=""></asp:Literal></font>
            </td>
            <td align="right" valign="top">
                <font class="loginpage_companyname"><b><%=Session["companyname"]%></b></font>
            </td>
        </tr>
    </table>
    <br />
    <table width="80%" border="0" align="center" cellpadding="0" cellspacing="0" style="vertical-align: middle">
        <tr>
            <td valign="top" width="50%">
                <asp:Login ID="phoenixLogin" runat="server" CssClass="Loginbox_background" InstructionText="Please enter your User Name and Password to login."
                    TextLayout="TextOnTop" PasswordRecoveryText="Forgot your password." PasswordRecoveryUrl="~/Options/OptionsForgotPassword.aspx"
                    Width="325px" OnAuthenticate="phoenixLogin_Authenticate" UserNameLabelText="User Name" PasswordLabelText="Password" DisplayRememberMe="false">
                    <TitleTextStyle CssClass="login_text" />
                    <LoginButtonStyle CssClass="login_button" />
                    <InstructionTextStyle CssClass="userinstruction_text" />
                    <TextBoxStyle Width="240px" />
                    <FailureTextStyle Height="30px" />
                </asp:Login>
            </td>
            <td valign="top" width="50%">
                <table width="100%">
                    <tr runat="server" id="trSignup">
                        <%--<td class="login_text" align="center">Sign Up</td>   --%>  
                        <td class="login_text" align="center">
                            <asp:Label ID="lblLoginLinks" runat="server" CssClass="login_text" Text="Sign Up" Width="100%"></asp:Label>
                        </td>                   
                    </tr>                    
                    <tr id="trnewapp" runat="server"> 
                        <td height="20px">
                            <asp:LinkButton runat="server" Text="Register" ID="lnkRegister" OnClick="lnkOptions_Click" CommandName="REGISTER"></asp:LinkButton>                            
                        </td>                        
                    </tr>                               
                </table>
            </td>
        </tr>
    </table>
    <div id="defaultfooter">
        <div id="altnav">
            <font class="browser_text"><asp:Literal ID="lblFullScreenView" runat="server" Text="For best view use 1024 x 768 or higher resolution in Full
                Screen mode. For full screen press F11 key on your key board. Press F11 key again
                to revert back to normal view."></asp:Literal></font>
        </div>
    </div>
    </form>
    <script type="text/javascript" language="javascript">
        if (top.frames.length != 0)
            top.location = "PhoenixLogout.aspx";
            //top.location=self.document.location;
    </script>     
</body>
</html>
