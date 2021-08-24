<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreEmployeeDocumentsCrossCheck.aspx.cs"
    Inherits="CrewOffshore_CrewOffshoreEmployeeDocumentsCrossCheck" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cross Check</title><
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />          
         
                <eluc:TabStrip ID="CrossCheckMainMenu" runat="server" OnTabStripCommand="CrossCheckMainMenu_TabStripCommand"></eluc:TabStrip>
       
            <div id="dvTravel" runat="server" visible="false">
                <table width="100%" cellspacing="5">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDocumentName" runat="server" Text="Document Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtDocumentName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDocNo" runat="server" Text="Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTravelDocNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblIssueDate" runat="server" Text="Date of Issue"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucDateOfIssue" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality/Flag"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtNationality" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblValidFrom" runat="server" Text="Valid From"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucValidFromDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblExpiryDate" runat="server" Text="Expiry Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucDateofExpiry" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPlaceofIssue" runat="server" Text="Place of Issue"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtPlaceofIsue" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPassportNo" runat="server" Text="Passport No"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtPassportNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCategory" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNoEntries" runat="server" Text="No of entries"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtNoofEntries" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblContactVessel" runat="server" Text="Connected to Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtContactVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDocStatus" runat="server" Text="Doc Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtDocStatus" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAuthenticateBy" runat="server" Text="Authentication By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtAuthenticateBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAuthenticationDate" runat="server" Text="Authentication Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucAuthenticationDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCrossCheckDate" runat="server" Text="Cross Check Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucCrossCheckDate" runat="server" CssClass="input" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTravelCrossCheckBy" runat="server" Text="Cross Check By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTravelCrossCheckBy" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvLicence" runat="server" visible="false">
                <table width="100%" cellspacing="5">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblLicence" runat="server" Text="Licence"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtLicence" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblLicenceNumber" runat="server" Text="Licence Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtLicenceNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblLicenceDateofIssue" runat="server" Text="Date of Issue"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucLicenceDateofIssue" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblLicenceNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtLicenceNationality" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblLicenceCategory" runat="server" Text="Category"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtLicenceCategory" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblLicenceExpiryDate" runat="server" Text="Expiry Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucLicenceExpiryDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblLicencePlaceofIssue" runat="server" Text="Place of Issue"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtLicencePlaceofIssue" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblLicenceIssueAuthority" runat="server" Text="Issuing Authority"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtLicenceIssueAuthority" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblLicenceType" runat="server" Text="Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtLicenceType" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblLicenceAuthenticateBy" runat="server" Text="Authentication By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtLicenceAuthenticateBy" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblLicenceAuthenticationDate" runat="server" Text="Authentication Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucLicenceAuthenticationDate" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="150px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblLicenceCrossCheckDate" runat="server" Text="Cross Check Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucLicenceCrossCheckDate" runat="server" CssClass="input" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblLicenceCrossCheckBy" runat="server" Text="Cross Check By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtLicenceCrossCheckBy" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvMedical" runat="server" visible="false">
                <table width="100%" cellspacing="5">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblMedical" runat="server" Text="Medical"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtMedical" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFlag" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPlaceofIssueM" runat="server" Text="Place of Issue"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtPlaceofIssueM" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblIssueDateM" runat="server" Text="Issue Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtIssueDateM" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblExpiryDateM" runat="server" Text="Expiry Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtExpiryDateM" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblStatusM" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtStatusM" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAuthenticationByM" runat="server" Text="Authentication By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtAuthenticatedByM" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAuthenticationDateM" runat="server" Text="Authentication Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtAuthenticatedDateM" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="150px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDoctorNameM" runat="server" Text="Doctor Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtDoctorNameM" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCrossCheckByM" runat="server" Text="Cross Check By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCrossCheckByM" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCrossCheckDateM" runat="server" Text="Cross Check Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtCrossCheckDateM" runat="server" CssClass="input" Width="150px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVerificationMethod" runat="server" Text="Verification Method">                                        
                            </telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucVerificationMethod" runat="server" CssClass="input" AppendDataBoundItems="true"
                                QuickTypeCode="131"></eluc:Quick>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvMedicalTest" runat="server" visible="false">
                <table width="100%" cellspacing="5">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTestName" runat="server" Text="Medical Test"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTestName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblTestStatus" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTestStatus" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblTestIssueDate" runat="server" Text="Issue Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucTestIssueDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTestPlaceofIssue" runat="server" Text="Place of Issue"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTestPlaceofIssue" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblTestExpiryDate" runat="server" Text="Expiry Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucTestExpiryDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblDoctorName" runat="server" Text="Doctor Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtDoctorName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTestAuthenticateBy" runat="server" Text="Authentication By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTestAuthenticateBy" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblTestAuthenticationDate" runat="server" Text="Authentication Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucTestAuthenticationDate" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="150px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblTestCrossCheckDate" runat="server" Text="Cross Check Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucTestCrossCheckDate" runat="server" CssClass="input" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblMedicalTestCrossCheckBy" runat="server" Text="Cross Check By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtMedicalTestCrossCheckBy" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvCourse" runat="server">
                <table width="100%" cellspacing="5" visible="false">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCourse" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCertificateNumber" runat="server" Text="Certificate Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCertificateNumber" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCourseIssueDate" runat="server" Text="Issue Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucCourseIssueDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCourseNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCourseNationality" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCourseCategory" runat="server" Text="Category"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCourseCategory" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCourseExpiryDate" runat="server" Text="Expiry Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucCourseExpiryDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCoursePlaceofIssue" runat="server" Text="Place of Issue"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCoursePlaceofIssue" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCourseIssueAuthority" runat="server" Text="Issuing Authority"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCourseIssueAuthority" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCourseType" runat="server" Text="Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCourseType" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCourseInstitute" runat="server" Text="Institution"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCourseInstitute" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCourseAuthenticateBy" runat="server" Text="Authentication By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCourseAuthenticateBy" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCourseAuthenticationDate" runat="server" Text="Authentication Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucCourseAuthenticationDate" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCourseCrossCheckBy" runat="server" Text="Cross Check By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCourseCrossCheckBy" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCourseCrossCheckDate" runat="server" Text="Cross Check Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucCourseCrossCheckDate" runat="server" CssClass="input" Width="150px" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="dvOther" runat="server" visible="false">
                <table width="100%" cellspacing="5">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOtherDocName" runat="server" Text="Document Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOtherDocName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOtherDocNumber" runat="server" Text="Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOtherDocNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOtherDocDateofIssue" runat="server" Text="Date of Issue"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucOtherDocDateofIssue" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOtherDocPlaceofIssue" runat="server" Text="Place of Issue"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOtherDocPlaceofIssue" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOtherDocIssuAuthority" runat="server" Text="Issuing Authority"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOtherDocIssuAuthority" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOtherDocExpiryDate" runat="server" Text="Expiry Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucOtherDocExpiryDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOtherDocCategory" runat="server" Text="Category"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOtherDocCategory" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblotherAuthenticateBy" runat="server" Text="Authentication By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOtherAuthenticateBy" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="150px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOtherDocAuthenticationDate" runat="server" Text="Authentication Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucOtherDocAuthenticationDate" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOtherDocCrossCheckBy" runat="server" Text="Cross Check By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOtherDocCrossCheckBy" runat="server" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOtherDocCrossCheckDate" runat="server" Text="Cross Check Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="ucOtherDocCrossCheckDate" runat="server" CssClass="input" Width="150px" />
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div>
                <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="height: 550px; width: 100%"></iframe>
            </div>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
