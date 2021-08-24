<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantFamilyNok.aspx.cs"
    Inherits="CrewNewApplicantFamilyNok" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonAddress" Src="../UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Sex" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ECNR" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Relation" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MobileNumber" Src="../UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="../UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewNewApplicantFamilyNok" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
        <eluc:TabStrip ID="CrewFamily" runat="server" OnTabStripCommand="CrewFamily_TabStripCommand" Title="Family NOK"></eluc:TabStrip>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtEmployeeFirstName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtEmployeeMiddleName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtEmployeeLastName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Applied Rank"></telerik:RadLabel>

                </td>
                <td colspan="5">
                    <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
            </tr>
        </table>
        <hr />
        <b>Family Member</b>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td rowspan="7" colspan="2" valign="top">
                    <telerik:RadListBox ID="lstFamily" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                        OnSelectedIndexChanged="lstFamily_SelectedIndexChanged" Width="98%" 
                        Height="150px">
                    </telerik:RadListBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblFamilyFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtFamilyFirstName" runat="server" CssClass="input_mandatory" MaxLength="200"></telerik:RadTextBox>
                </td>
                <td rowspan="3">
                    <a id="aCrewFamilyImg" runat="server">
                        <asp:Image ID="imgPhoto" runat="server" Height="100px" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                            Width="120px" /></a>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFamilyMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtFamilyMiddleName" runat="server"  MaxLength="200"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFamilyLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtFamilyLastName" runat="server"  MaxLength="200"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblRelationship" runat="server" Text="Relationship"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Relation ID="ucRelation" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true"
                        OnTextChangedEvent="AnniversaryDetails" AppendDataBoundItems="true" />
                </td>
                <td colspan="2">
                    <%--<asp:FileUpload ID="txtFileUpload" runat="server"  />--%>
                    <telerik:RadUpload ID="txtFileUpload" runat="server" MaxFileInputsCount="1" OverwriteExistingFiles="false"
                        ControlObjectsVisibility="None" Skin="Silk">
                    </telerik:RadUpload>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblAnniversaryDate" runat="server" Text="Anniversary"></telerik:RadLabel>

                </td>
                <td>
                    <eluc:Date ID="ucAnniversaryDate" Enabled="false" runat="server" CssClass="readonlytextbox" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>

                </td>
                <td>
                    <eluc:Nationality ID="ucNatioanlity" runat="server" AppendDataBoundItems="true"  />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSex" runat="server" Text="Gender"></telerik:RadLabel>

                </td>
                <td>
                    <eluc:Sex ID="ucSex" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="130px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblDateofBirth" runat="server" Text="D.O.B."></telerik:RadLabel>

                </td>
                <td>
                    <eluc:Date ID="ucDateOfBirth" runat="server"  />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNOK" runat="server" Text="NOK"></telerik:RadLabel>

                </td>
                <td colspan="3">
                    <asp:CheckBox ID="chkNOK" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="6">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="3" valign="top">
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td>
                                <b>
                                    <telerik:RadLabel ID="lblContactInformation" runat="server" Text="Contact Information"></telerik:RadLabel>
                                </b>
                            </td>
                            <td>
                                <telerik:RadPushButton ID="lnkCPA" runat="server" OnClick="CopyAddress_Click" Text="Copy Permanent Address"></telerik:RadPushButton>

                                <telerik:RadPushButton ID="lnkCLA" runat="server" OnClick="CopyAddress_Click" Text="Copy Local Address"></telerik:RadPushButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <eluc:CommonAddress ID="ucAddress" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 16%;">
                                <telerik:RadLabel ID="lblPhoneNumber" runat="server" Text="Phone Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:PhoneNumber ID="ucPhoneNumber" runat="server" />
                                <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                                <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                                    RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                                    HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true"
                                    Text="For Landline numbers, exclude the '0' before the 'AreaCode'. (Eg. For Chennai: 44 - 22222222)">
                                </telerik:RadToolTip>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblEMail" runat="server" Text="E-Mail"></telerik:RadLabel>

                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtEmail" runat="server"  MaxLength="500" Width="300px"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblMobileNumber" runat="server" Text="Mobile Number"></telerik:RadLabel>

                            </td>
                            <td>
                                <eluc:MobileNumber ID="ucMobileNumber" runat="server"  IsMobileNumber="true" />
                                <%--<eluc:PhoneNumber ID="ucMobileNumber" runat="server"  IsMobileNumber="true" />--%>
                                <span id="Span2" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                                <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip2" Width="300px" ShowEvent="onmouseover"
                                    RelativeTo="Element" Animation="Fade" TargetControlID="Span2" IsClientID="true"
                                    HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true"
                                    Text="For Mobile Numbers, exclude the '0' before the number.">
                                </telerik:RadToolTip>
                            </td>
                        </tr>
                    </table>
                </td>
                <td colspan="3" valign="top">
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td colspan="2">
                                <b>
                                    <telerik:RadLabel ID="lblBankDetails" runat="server" Text="Bank Details"></telerik:RadLabel>
                                </b>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 16%;">
                                <telerik:RadLabel ID="lblBankName" runat="server" Text="Bank"></telerik:RadLabel>

                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtBankName" runat="server"  MaxLength="200"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAccountNumber" runat="server" Text="Account Number"></telerik:RadLabel>

                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtAccountNumber" runat="server"  MaxLength="200"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblBranch" runat="server" Text="Branch"></telerik:RadLabel>

                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtBranch" runat="server"  MaxLength="200"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <eluc:CommonAddress ID="ucBankAddress" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <eluc:Status ID="ucStatus" runat="server" />
    </form>
</body>
</html>
