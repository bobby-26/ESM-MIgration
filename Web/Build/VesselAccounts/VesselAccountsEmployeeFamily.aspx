<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsEmployeeFamily.aspx.cs"
    Inherits="VesselAccountsEmployeeFamily" %>

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
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewFamilyNok" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>          
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuActivityFilterMain" runat="server" Title="Family/Nok"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" Enabled="false" Width="80%" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" Enabled="false" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                    </td>
                    <td style="width: 13%;">
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 20%;">
                        <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" Enabled="false" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" Enabled="false" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" Enabled="false" ReadOnly="true" Width="80%"></telerik:RadTextBox>
                    </td>
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
                        <asp:ListBox ID="lstFamily" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            OnSelectedIndexChanged="lstFamily_SelectedIndexChanged" Width="85%"
                            Height="150px"></asp:ListBox>
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
                        <eluc:Relation ID="ucRelation" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
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
                        <eluc:Date ID="ucAnniversaryDate" Enabled="false" runat="server" CssClass="readonlytextbox" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofBirth" runat="server" Text="D.O.B"></telerik:RadLabel>
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
                        <table cellpadding="1" cellspacing="1" width="100%" id="tbl1" runat="server">
                            <tr>
                                <td colspan="2">
                                    <b>
                                        <telerik:RadLabel ID="lblContactInformation" runat="server" Text="Contact Information"></telerik:RadLabel>
                                    </b>
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
                                    <eluc:PhoneNumber ID="ucMobileNumber" runat="server" IsMobileNumber="true" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <font color="blue">
                                        <telerik:RadLabel ID="RadLabel1"
                                            runat="server" Text="Note:For Landline numbers, exclude the '0' before the 'Area Code'.">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="RadLabel2"
                                            runat="server" Text="(Eg. For Chennai: 44  22222222)For Mobile Numbers, exclude the '0' before the number.">
                                        </telerik:RadLabel>
                                    </font>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td colspan="3" valign="top">
                        <table cellpadding="1" cellspacing="1" width="100%"  id="tbl2" runat="server">
                            <tr>
                                <td colspan="2">
                                    <b>
                                        <telerik:RadLabel ID="lblBankDetails" runat="server" Text="Bank Details"></telerik:RadLabel>
                                    </b>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 16%;">
                                    <telerik:RadLabel ID="lbklBankName" runat="server" Text="Bank Name"></telerik:RadLabel>
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
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
