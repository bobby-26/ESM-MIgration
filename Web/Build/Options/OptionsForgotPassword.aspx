<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsForgotPassword.aspx.cs"
    Inherits="OptionsForgotPassword" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
   <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>     

        <script type="text/javascript">
            function pageLoad() {
                //disable
                $telerik.$(".rwzButton.rwzPrevious").prop("disabled", true);
                $telerik.$(".rwzButton.rwzPrevious").css('opacity', '0.6');
                //hide
                $telerik.$(".rwzButton.rwzPrevious").hide();
            }
        </script>
    
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager" EnableScriptCombine="false" />
        <telerik:RadSkinManager ID="RadSkinManager" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="SecurityWizard">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="SecurityWizard" />
                        <telerik:AjaxUpdatedControl ControlID="ValidationSummary" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
    <table class="loginpagebackground" width="80%" align="center" cellpadding="0" cellspacing="0" height="60px">
        <tr>
            <td align="left" valign="top">
                <font class="application_title"><telerik:RadCodeBlock ID="RadCodeBlock2" runat="server"><%=Application["softwarename"].ToString() %></telerik:RadCodeBlock></font>&nbsp;&nbsp;
            </td>
            <td align="right" valign="top">
                <font class="loginpage_companyname"><b><asp:Literal ID="lblExecutiveShipManagementPteLtd" runat="server" Text="Executive Ship Management Pte Ltd."></asp:Literal></b></font>                
                <telerik:RadCodeBlock ID="RadCodeBlock3" runat="server">
            &nbsp;&nbsp;<img id="Img1" runat="server" style="vertical-align: top" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>" alt="Phoenix" />&nbsp;
                    </telerik:RadCodeBlock>
            </td>
        </tr>
    </table>
        <div style="top:100px; margin-left:auto; margin-right:auto; vertical-align:middle; width:440px; border:1px; border-style:solid; border-color:lightgray" >
        <div class="subHeader" style="position: relative">
            <div id="divHeading">
                <asp:Literal ID="lblForgotPassword" runat="server" Text="Forgot Password"></asp:Literal>
            </div>
            <asp:HiddenField runat="server" ID="isouterpage" Value="" />
        </div>          
            <telerik:RadAjaxPanel ID="pnlEmail" runat="server">
                <table align="center" cellpadding="8">
                    <tr>
                        <td colspan="2">
                            <asp:Literal ID="lblEnteryourEMailaddresstoreceivethepassword" runat="server" Text="Enter your E-Mail address to receive the password."></asp:Literal>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblEMail" runat="server" Text="E-Mail"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtEMail" MaxLength="500" Width="360px" CssClass="input"></asp:TextBox><br />
                            <asp:Label runat="server" ID="lblMessage" Visible="false"></asp:Label>
                            <%--<asp:HyperLink runat="server" ID="hlnkBack" Text="Back to Login" NavigateUrl="~/Default.aspx"></asp:HyperLink>--%>
                            <asp:LinkButton runat="server" ID="lnkBack" Text="Back to Login" OnClick="btnlogin_Click"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="right">
                            <asp:Button runat="server" ID="cmdSubmit" Text="Submit" OnClick="cmdSubmit_Click" />
                        </td>
                    </tr>
                </table>
            </telerik:RadAjaxPanel>
            
            <telerik:RadWizard runat="server" ID="SecurityWizard" Width="100%" OnNextButtonClick="SecurityWizard_NextButtonClick" 
            OnFinishButtonClick="SecurityWizard_FinishButtonClick" Localization-Finish="Save" OnNavigationBarButtonClick="SecurityWizard_NavigationBarButtonClick">
            <WizardSteps >
                <telerik:RadWizardStep ID="rwCredential" Title="User" StepType="Start" AllowReturn="true">
                    <table cellpadding="8">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblUserName" runat="server" Text="Username"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox  runat="server" ID="txtUserName" MaxLength="100" Width="300px" CssClass="input_mandatory"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator
                                    ID="TextBoxRequiredFieldValidator"
                                    runat="server"
                                    Display="Dynamic"
                                    ControlToValidate="txtUserName"
                                    EnableClientScript="true" ForeColor="Red"
                                    ValidationGroup="UserName"
                                    ErrorMessage="* Username cannot be empty!">*
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                    </table>
                </telerik:RadWizardStep>
                <telerik:RadWizardStep ID="rwAnswer" Title="Answer Security Question" StepType="Step">
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblSecurityQuestion" runat="server" Text="Security Question"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Quick ID="ucSecurityQuestion" runat="server" QuickTypeCode="175" Width="300px" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAnswer" runat="server" Text="Answer" RenderMode="Lightweight"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtAnswer" runat="server" Width="300px" CssClass="input_mandatory"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator2"
                                    runat="server"
                                    Display="Dynamic"
                                    ControlToValidate="txtAnswer"
                                    EnableClientScript="true" ForeColor="Red"
                                    ValidationGroup="Answer"
                                    ErrorMessage="* Answer cannot be empty!">*
                                </asp:RequiredFieldValidator>
                                <%--<asp:CompareValidator
                                    ID="CompareValidator1"
                                    runat="server"
                                    Display="Dynamic"
                                    ControlToValidate="txtAnswer"
                                    ControlToCompare ="hdnAnswer"
                                    EnableClientScript="true" ForeColor="Red"
                                    Type="String"
                                    ValidationGroup="Answer"
                                    
                                    ErrorMessage="* Answer not maching!">*
                                </asp:CompareValidator>--%>
                                <telerik:RadTextBox ID="hdnAnswer" Text="" runat="server" Width="0px" Visible="false"></telerik:RadTextBox>
                                <%--<asp:HiddenField ID="hdnAnswer" runat="server" />--%>
                            </td>
                        </tr>
                    </table>
                </telerik:RadWizardStep>
                 <telerik:RadWizardStep ID="rwFinish" Title="Reset Password" StepType="Finish">
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblNewPassword" runat="server" Text="New Password"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox TextMode="Password" ID="txtNewPassword" runat="server" Width="300px" CssClass="input_mandatory"></telerik:RadTextBox>
                                <asp:RequiredFieldValidator
                                    ID="RequiredFieldValidator3"
                                    runat="server"
                                    Display="Dynamic"
                                    ControlToValidate="txtNewPassword"
                                    EnableClientScript="true" ForeColor="Red"
                                    ValidationGroup="Reset"
                                    ErrorMessage="* Password cannot be empty!">*
                                </asp:RequiredFieldValidator>
                                <asp:CompareValidator
                                    ID="CompareValidator2"
                                    runat="server"
                                    Display="Dynamic"
                                    ControlToValidate="txtNewPassword"
                                    ControlToCompare ="txtConfirmPassword"
                                    EnableClientScript="true" ForeColor="Red"
                                    Type="String"
                                    ValidationGroup="Reset"
                                    ErrorMessage="* The Password and the confirm password does not match">*
                                </asp:CompareValidator>
                            </td>
                            </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblConfirmPassword" runat="server" Text="Confirm Password"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtConfirmPassword" runat="server" Width="300px" CssClass="input_mandatory" TextMode="Password"></telerik:RadTextBox>
                            </td>
                            </tr>
                        </table>
                     </telerik:RadWizardStep>
                <telerik:RadWizardStep ID="rwComplete" Title="Reset Password" StepType="Complete">
                    <table style="align-content:center">
                        <tr>
                            <td>
                                <p>Password reset successfully!</p>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:HyperLink runat="server" ID="HyperLink1" Text="Click here to Login" NavigateUrl="~/Default.aspx"></asp:HyperLink>
                            </td>
                        </tr>
                    </table>
                    
                    
                </telerik:RadWizardStep>
            </WizardSteps>
        </telerik:RadWizard>
            <telerik:RadAjaxPanel ID="ValidationSummary" runat="server">
                <table style="width: 100%;">
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="ValidationSummary1" runat="server" Width="200px"
                                BorderWidth="1px" HeaderText="List of errors" ValidationGroup="UserName"></asp:ValidationSummary>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" Width="200px"
                                BorderWidth="1px" HeaderText="List of errors" ValidationGroup="Answer"></asp:ValidationSummary>
                        </td>
                    </tr>
                     <tr>
                        <td>
                            <asp:ValidationSummary ID="ValidationSummary3" runat="server" Width="200px"
                                BorderWidth="1px" HeaderText="List of errors" ValidationGroup="Reset"></asp:ValidationSummary>
                        </td>
                    </tr>
                </table>
            </telerik:RadAjaxPanel>
    </div>
        
    </form>
</body>
</html>