<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsChangeSecurityQuestion.aspx.cs"
    Inherits="OptionsChangeSecurityQuestion" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery.min.js"></script>
        
    </telerik:RadCodeBlock>
    <script type="text/javascript">      

        function pageLoad() {
            //disable
            $telerik.$(".rwzButton.rwzPrevious").prop("disabled", true);
            $telerik.$(".rwzButton.rwzPrevious").css('opacity', '0.6');

            //hide
            $telerik.$(".rwzButton.rwzPrevious").hide();
        }


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager" />
        <telerik:RadSkinManager ID="RadSkinManager" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <%--<eluc:TabStrip ID="MenuSecurityChangePassword" runat="server" OnTabStripCommand="SecurityChangePassword_TabStripCommand">
        </eluc:TabStrip>--%>
        <eluc:Status runat="server" ID="ucStatus" />
        <telerik:RadFormDecorator runat="server" ID="RadFormDecorator" DecorationZoneID="SecurityWizard" EnableRoundedCorners="true" RenderMode="Lightweight" />
        <telerik:RadWizard runat="server" ID="SecurityWizard" Width="50%" OnNextButtonClick="SecurityWizard_NextButtonClick"
            OnFinishButtonClick="SecurityWizard_FinishButtonClick" Localization-Finish="Save" OnNavigationBarButtonClick="SecurityWizard_NavigationBarButtonClick">
            <WizardSteps >
                <telerik:RadWizardStep ID="rwCredential" Title="Validate User" StepType="Start" AllowReturn="true">
                    <table cellpadding="8">
                        <tr>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblUserName" runat="server" Text="User Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtUserName" MaxLength="100" Width="360px" CssClass="input_mandatory"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblCurrentPassword" runat="server" Text="Current Password"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtCurrentPassword" MaxLength="100" Width="360px"
                                    TextMode="Password" CssClass="input">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </telerik:RadWizardStep>
                <telerik:RadWizardStep ID="rwFinish" Title="Security Question" StepType="Finish">
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblSecurityQuestion" runat="server" Text="Security Question"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Quick ID="ucSecurityQuestion" runat="server" QuickTypeCode="175" Width="360px"  CssClass="input_mandatory"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAnswer" runat="server" Text="Answer" RenderMode="Lightweight"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtAnswer" runat="server" Width="360px" CssClass="input_mandatory"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </telerik:RadWizardStep>
                <telerik:RadWizardStep ID="rwComplete" Title="Security Question" StepType="Complete">
                    <p>Security Question updated successfully!</p>
                </telerik:RadWizardStep>
            </WizardSteps>
        </telerik:RadWizard>
        
    </form>
</body>
</html>
