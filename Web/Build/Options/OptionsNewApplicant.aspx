<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsNewApplicant.aspx.cs" Inherits="OptionsNewApplicant" %>

<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Telephone" Src="~/UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Registration</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="radscript1" runat="server"></asp:ScriptManager>

        <%--<telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>--%>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

        <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table class="loginpagebackground" width="100%" align="center" cellpadding="0" cellspacing="0" height="30px">
                <tr>
                    <td align="left" valign="middle">
                        <h2 class="logo">&nbsp;&nbsp;<telerik:RadLabel ID="lblPhoenix" runat="server" Text="Phoenix"></telerik:RadLabel>
                        </h2>
                    </td>
                    <td align="right" valign="middle">
                        <%--<header class="tm-header" id="logo"></header>--%>
                        <h2 class="logo">
                            <img runat="server" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>" alt="Phoenix" height="20" width="30" />
                            <telerik:RadLabel ID="lblManagement" runat="server"></telerik:RadLabel>
                        </h2>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuSecurityUser" runat="server" OnTabStripCommand="SecurityUser_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="4">
                <tr>
                    <td colspan="2">
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center; color: red"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="OnClick"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true" HideDelay="100000" Font-Bold="true"
                            HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true" Font-Size="XX-Large" BorderColor="Red"
                            Text="The application process will require you to enter following personal and business related information.Please complete all mandatory fields marked * in order to get your application processed.">
                        </telerik:RadToolTip>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFirstName" MaxLength="100" Width="360px" CssClass="input"></telerik:RadTextBox>
                        <b><font color="red">*</font></b>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" MaxLength="100" Width="360px" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLastName" MaxLength="100" Width="360px" CssClass="input"></telerik:RadTextBox>
                        <b><font color="red">*</font></b>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblGender" runat="server" Text="Gender"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlGender" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        <b><font color="red">*</font></b>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Nationality ID="ddlNationality" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        <b><font color="red">*</font></b>

                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTelephoneNumber" runat="server" Text="Mobile Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Telephone runat="server" ID="ucTelephone" CssClass="input" />
                        <b><font color="red">*</font></b>
                    </td>
                    <td>
                        <telerik:RadLabel ID="EMail" runat="server" Text="E-Mail"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEMail" MaxLength="100" Width="360px" CssClass="input"></telerik:RadTextBox>
                        <b><font color="red">*</font></b>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSeamanBookNumberCDC" runat="server" Text="Seaman Book Number (CDC)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSeamanBookNumber" MaxLength="100" Width="360px" CssClass="input"></telerik:RadTextBox>
                        <b><font color="red">*</font></b>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPassportNumber" runat="server" Text="Passport Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtPassport" MaxLength="100" Width="360px" CssClass="input"></telerik:RadTextBox>
                        <b><font color="red">*</font></b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofBirth" runat="server" Text="Date of Birth"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDateofBirth" runat="server" CssClass="input" />
                        <b><font color="red">*</font></b>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Applied Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank runat="server" ID="ucRankApplied" CssClass="input" AppendDataBoundItems="true" />
                        <b><font color="red">*</font></b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFresherNoSeaExperience" runat="server" Text="Fresher(No Sea Experience)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkSeaFarerExp" runat="server" AutoPostBack="true" OnCheckedChanged="DeselectVesseltype" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUploadResume" runat="server" Text="Upload Resume"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:FileUpload ID="txtFileUpload" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselTypesPreviousExperience" runat="server" Text="Vessel Types (Previous Experience)"></telerik:RadLabel>
                    </td>
                    <td>
                        <div runat="server" id="dvVesselType" class="input_mandatory" style="width: 260px; height: 100px">
                            <telerik:RadListBox runat="server" ID="cblVesselType" Height="100%" RepeatColumns="1" CheckBoxes="true"
                                RepeatDirection="Horizontal" RepeatLayout="Flow" Width="260px">
                            </telerik:RadListBox>
                        </div>
                        <%--<asp:ListBox runat="server" ID="lstVesselType" Width="100px" CssClass="input" Height="100px" SelectionMode="Multiple">
                        </asp:ListBox>--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWordVerification" runat="server" Text="Word Verification"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblTypethecharactersyouseeinthepicturebelow" runat="server" Text="Type the characters you see in the picture below."></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UP1" runat="server">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td style="height: 50px; width: 100px;">
                                                        <asp:Image ID="imgCaptcha" runat="server" />
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:ImageButton ID="btnRefresh" runat="server" ImageUrl="<%$ PhoenixTheme:images/refresh.png %>"
                                                            OnClick="GenerateAccessCode" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadTextBox runat="server" ID="txtAccessCode" CssClass="input"></telerik:RadTextBox>
                                    <b><font color="red">*</font></b>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblLettersarenotcasesensitive" runat="server" Text="Letters are not case-sensitive"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <input type="button" runat="server" id="isouterpage" name="isouterpage" style="visibility: hidden" />
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="btnConfirm_Click" CancelText="Ok" YesButtonVisible="false" />
        </div>
    </form>
</body>
</html>
