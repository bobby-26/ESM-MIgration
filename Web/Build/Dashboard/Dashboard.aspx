<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dashboard.aspx.cs" Inherits="Dashboard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
<title></title>
<telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
         <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Visible="false"/>
            <table cellpadding="10" cellspacing="20">
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkPasswordChange" runat="server" Text="Change Password" CssClass="input" OnClick="PasswordChange_Click"
                            Visible="false"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkSecurityQuestion" runat="server" Text="Change Security Question" CssClass="input" OnClick="lnkSecurityQuestion_Click"
                            Visible="false"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkVesselCrewFeedbackForm" runat="server" Text="Joining Feedback" OnClick="JoningFeedback_Click"
                            CssClass="input" Visible="false"></asp:LinkButton>
                    </td>
                </tr>
                 <tr>
                    <td>
                        <asp:LinkButton ID="lnkSignoffFeedBack" runat="server" Text="Sign off Feedback" OnClick="SignoffFeedback_Click"
                            CssClass="input" Visible="false"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
