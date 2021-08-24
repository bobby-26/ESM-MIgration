<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantRegister.aspx.cs"
    Inherits="CrewNewApplicantRegister" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Telephone" Src="~/UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MobileNumber" Src="../UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPool.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Registration</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .fon {
                font-size: small !important;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" RenderMode="Lightweight" EnableRoundedCorners="true" DecoratedControls="All" DecorationZoneID="form1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>
        <eluc:TabStrip ID="MenuSecurityUser" runat="server" OnTabStripCommand="SecurityUser_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="4" id="table1" runat="server">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtFirstName" MaxLength="100" Width="260px" CssClass="input_mandatory upperCase"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtMiddleName" MaxLength="100" Width="260px" CssClass="input upperCase"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtLastName" MaxLength="100" Width="260px" CssClass="input upperCase"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblGender" runat="server" Text="Gender"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ddlGender" runat="server" AppendDataBoundItems="true" Width="260px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Nationality ID="ddlNationality" runat="server" AppendDataBoundItems="true" Width="260px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true" Width="260px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblTelephoneNumber" runat="server" Text="Phone Number"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Telephone runat="server" ID="ucTelephone" Width="210px" />
                    &nbsp;
                    <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                        RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true" CssClass="fon"
                        HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true"
                        Text="Exclude the '0' before the 'Area Code'. (Eg. For Chennai:44 22222222)">
                    </telerik:RadToolTip>
                </td>
                <td>
                    <telerik:RadLabel ID="lblMobileNumber" runat="server" Text="Mobile Number"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MobileNumber runat="server" ID="ucMobile" CssClass="input_mandatory" IsMobileNumber="true" Width="260px" />
                    &nbsp;&nbsp;
                    <span id="Span2" class="icon" style="align-self: center" runat="server"><i class="fas fa-info-circle"></i></span>
                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip2" Width="200px" ShowEvent="onmouseover"
                        RelativeTo="Element" Animation="Fade" TargetControlID="Span2" IsClientID="true" CssClass="fon"
                        HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true"
                        Text="Exclude the '0' before the number.">
                    </telerik:RadToolTip>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEmail" runat="server" Text="E-Mail"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtEMail" MaxLength="100" Width="260px" CssClass="input_mandatory lowerCase"></telerik:RadTextBox>
                    &nbsp;&nbsp;
                    <span id="Span3" class="icon" style="align-self: center" runat="server"><i class="fas fa-info-circle"></i></span>
                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip3" Width="300px" ShowEvent="onmouseover"
                        RelativeTo="Element" Animation="Fade" TargetControlID="Span3" IsClientID="true" CssClass="fon"
                        HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true" HideDelay="5000"
                        Text="Enter only the correct E-Mail( For eg. xx@xx.com) If not available,please enter 'NA'">
                    </telerik:RadToolTip>
                </td>
                <td>
                    <telerik:RadLabel ID="lblSeamanBook" runat="server" Text="Seaman Book Number (CDC)"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtSeamanBookNumber" MaxLength="100" Width="260px"
                        CssClass="input_mandatory upperCase">
                    </telerik:RadTextBox>
                    &nbsp;&nbsp;
                    <span id="Span4" class="icon" style="align-self: center" runat="server"><i class="fas fa-info-circle"></i></span>
                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip4" Width="300px" ShowEvent="onmouseover"
                        RelativeTo="Element" Animation="Fade" TargetControlID="Span4" IsClientID="true" CssClass="fon"
                        HideEvent="ManualClose" Position="BottomCenter" EnableRoundedCorners="true" HideDelay="5000" BorderWidth="100"
                        Text="Enter only the correct CDC No without space( For eg. Mum123456). If not available,please enter 'NA'">
                    </telerik:RadToolTip>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPassportNumber" runat="server" Text="Passport"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtPassport" MaxLength="100" Width="260px" CssClass="upperCase"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblDateofBirth" runat="server" Text="D.O.B."></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucDateofBirth" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Applied Rank"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Rank runat="server" ID="ucRankApplied" CssClass="input_mandatory" AppendDataBoundItems="true" Width="260px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblFresher" runat="server" Text="Fresher(No Sea Experience)"></telerik:RadLabel>
                </td>
                <td>
                    <asp:CheckBox ID="chkSeaFarerExp" runat="server" AutoPostBack="true" OnCheckedChanged="DeselectVesseltype" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVesselTypes" runat="server" Text="Vessel Types (Previous Experience)"></telerik:RadLabel>
                </td>
                <td>
                    <div runat="server" id="dvVesselType" class="input_mandatory" style="width: 260px; height: 100px">
                        <telerik:RadListBox runat="server" ID="cblVesselType" Height="100%" RepeatColumns="1" AutoPostBack="false"
                            RepeatDirection="Horizontal" RepeatLayout="Flow" CheckBoxes="true" Width="260px">
                        </telerik:RadListBox>
                    </div>
                </td>
                <td>
                    <telerik:RadLabel ID="lblUploadResume" runat="server" Text="Upload Resume"></telerik:RadLabel>
                </td>
                <td>
                    <%--<asp:FileUpload ID="txtFileUpload" runat="server" CssClass="input" Width="260px" />--%>
                    <telerik:RadUpload ID="txtFileUpload" runat="server" MaxFileInputsCount="1" OverwriteExistingFiles="false"
                        ControlObjectsVisibility="None" Skin="Silk" Width="260px">
                    </telerik:RadUpload>
                </td>
            </tr>
        </table>
        <eluc:Status runat="server" ID="ucStatus" />
    </form>
</body>
</html>
