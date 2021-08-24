<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewFamilyNok.aspx.cs" Inherits="CrewFamilyNok" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
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
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="../UserControls/UserControlNationality.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
    <form id="frmCrewFamilyNok" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:TabStrip ID="CrewFamilyTabs" runat="server" OnTabStripCommand="CrewFamilyTabs_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="CrewFamily" runat="server" OnTabStripCommand="CrewFamily_TabStripCommand" Title="Family Nok"></eluc:TabStrip>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                     <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtFirstName" runat="server" Width="80%" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtMiddleName" runat="server" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                     <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtLastName" runat="server" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeNumber" runat="server" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td >
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                    </td>
                     <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <b>
                            <telerik:RadLabel ID="lblFamilyMember" runat="server" Text="Family Member"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td rowspan="6" colspan="2" valign="top" style="width: 33%;">
                        <telerik:RadListBox ID="lstFamily" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            OnSelectedIndexChanged="lstFamily_SelectedIndexChanged" Width="85%"
                            Height="150px">
                        </telerik:RadListBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblFamilyFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox ID="txtFamilyFirstName" runat="server" CssClass="input_mandatory" MaxLength="200" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td rowspan="3" colspan="2" style="width: 33%;">
                        <a id="aCrewFamilyImg" runat="server">
                            <asp:Image ID="imgPhoto" runat="server" Height="100px" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                Width="120px" /></a>
                        <asp:ImageButton ID="imgIDCard" runat="server" ImageUrl="<%$ PhoenixTheme:images/id-card.png %>"
                            ToolTip="Family ID Card" />
                        <telerik:RadUpload ID="txtFileUpload" runat="server" MaxFileInputsCount="1" OverwriteExistingFiles="false"
                            ControlObjectsVisibility="None" Skin="Silk">
                        </telerik:RadUpload>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFamilyMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFamilyMiddleName" runat="server" MaxLength="200" Width="180px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFamilyLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFamilyLastName" runat="server" MaxLength="200" Width="180px"></telerik:RadTextBox>

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
                    <td>
                        <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Nationality ID="ucNatioanlity" runat="server" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAnniversaryDate" runat="server" Text="Anniversary Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucAnniversaryDate" Enabled="false" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofBirth" runat="server" Text="Date of Birth"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDateOfBirth" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSex" runat="server" Text="Gender"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Sex ID="ucSex" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNOK" runat="server" Text="NOK"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkNOK" runat="server" Value="0"></telerik:RadCheckBox>
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
                                    <telerik:RadLabel ID="lblPhoneNumber" runat="server" Text="Phone No."></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:PhoneNumber ID="ucPhoneNumber" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblEMail" runat="server" Text="E-Mail"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtEmail" runat="server" MaxLength="500" Width="360px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblMobileNumber" runat="server" Text="Mobile No."></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:MobileNumber ID="ucMobileNumber" runat="server" CssClass="input_mandatory" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <font color="blue">
                                        <telerik:RadLabel ID="lblNote"
                                            runat="server" Text="Note:For Landline numbers, exclude the '0' before the 'Area Code'.">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblmobileno"
                                            runat="server" Text="(Eg. For Chennai: 44  22222222)For Mobile Numbers, exclude the '0' before the number.">
                                        </telerik:RadLabel>
                                    </font>
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
                                    <telerik:RadLabel ID="lblBankName" runat="server" Text="Bank Name"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtBankName" runat="server" MaxLength="200" Width="360px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblAccountNumber" runat="server" Text="Account No."></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtAccountNumber" runat="server" MaxLength="200" Width="360px"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblBranch" runat="server" Text="Branch"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtBranch" runat="server" MaxLength="200" Width="360px"></telerik:RadTextBox>
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
        </telerik:RadAjaxPanel>
        <eluc:Status ID="ucStatus" runat="server" />
    </form>
</body>
</html>
