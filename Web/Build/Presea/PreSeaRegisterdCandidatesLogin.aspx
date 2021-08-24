<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaRegisterdCandidatesLogin.aspx.cs"
    Inherits="PreSeaRegisterdCandidatesLogin" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>SIMS - Registered Candidate's Login</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <table width="100%" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td align="left" style="width: 15%">
                    <img id="img1" runat="server" alt="" src="<%$ PhoenixTheme:images/sims.png %>" />
                </td>
                <td style="font-size: medium; font-weight: bold; width: 60%;" align="center">SAMUNDRA INSTITUTE OF MARITIME STUDIES
                <br />
                    <br />
                    Registered Candidate's Login
                </td>
                <td style="width: 20%">&nbsp;
                </td>
            </tr>
        </table>
        <br style="clear: both;" />
        <hr />
        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0" style="vertical-align: middle">
            <tr>
                <td valign="top" width="20%">&nbsp;
                </td>
                <td valign="top" width="60%" align="center">
                    <asp:Login ID="phoenixLogin" runat="server" CssClass="Loginbox_background" InstructionText="Please enter your Email ID and DOB(DD/MM/YYYY) as a Password to login."
                        TextLayout="TextOnTop" Width="325px" OnAuthenticate="phoenixLogin_Authenticate"
                        UserNameLabelText="Email" PasswordLabelText="Password (DOB - DD/MM/YYYY)" DisplayRememberMe="false">
                        <TitleTextStyle CssClass="login_text" />
                        <LoginButtonStyle CssClass="login_button" />
                        <InstructionTextStyle CssClass="userinstruction_text" />
                        <TextBoxStyle Width="240px" />
                        <FailureTextStyle Height="30px" />
                    </asp:Login>
                </td>
                <td valign="bottom" width="20%">
                    <asp:LinkButton ID="lnkEnquiry" runat="server" OnClick="lnkEnquiry_Click">Enquiry /Query about courses</asp:LinkButton>
                    <br />
                    <br />
                    <asp:LinkButton ID="lnkRegistration" runat="server" OnClick="lnkRegistration_Click">New Candidate's Registration</asp:LinkButton>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
