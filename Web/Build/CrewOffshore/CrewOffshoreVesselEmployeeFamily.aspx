<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreVesselEmployeeFamily.aspx.cs" Inherits="CrewOffshoreVesselEmployeeFamily" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
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
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCrewFamilyNok" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxtoolkit:toolkitscriptmanager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
        <div class="subHeader">
            <span style="float: left">
                <eluc:title runat="server" id="Medical" text="Crew Family/Nok" showmenu="false">
                        </eluc:title>
            </span>
        </div>
        <table width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <asp:Literal ID="lblFirstName" runat="server" Text="First Name"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblMiddleName" runat="server" Text="Middle Name"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblLastName" runat="server" Text="Last Name"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblEmployeeNumber" runat="server" Text="Employee Number"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:TextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
        </table>
        <hr />
        <div>
            <b><asp:Literal ID="lblFamilyMember" runat="server" Text="Family Member"></asp:Literal></b>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td rowspan="7" colspan="2" valign="top">
                        <asp:ListBox ID="lstFamily" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            OnSelectedIndexChanged="lstFamily_SelectedIndexChanged" Width="98%" CssClass="input"
                            Height="150px"></asp:ListBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblFamilyFirstName" runat="server" Text="First Name"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFamilyFirstName" runat="server" CssClass="input_mandatory" MaxLength="200"></asp:TextBox>
                    </td>
                    <td rowspan="3">
                        <a id="aCrewFamilyImg" runat="server">
                            <asp:Image ID="imgPhoto" runat="server" Height="100px" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                Width="120px" /></a>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblFamilyMiddleName" runat="server" Text="Middle Name"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFamilyMiddleName" runat="server" CssClass="input" MaxLength="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblFamilyLastName" runat="server" Text="Last Name"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtFamilyLastName" runat="server" CssClass="input" MaxLength="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblRelationship" runat="server" Text="Relationship"></asp:Literal>
                    </td>
                    <td>
                        <eluc:relation id="ucRelation" runat="server" cssclass="dropdown_mandatory" appenddatabounditems="true" />
                    </td>
                    <td colspan="2">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblAnniversaryDate" runat="server" Text="Anniversary Date"></asp:Literal>
                    </td>
                    <td>
                        <eluc:date id="ucAnniversaryDate" enabled="false" runat="server" cssclass="readonlytextbox" />
                    </td>
                    <td>
                        <asp:Literal ID="lblNationality" runat="server" Text="Nationality"></asp:Literal>
                    </td>
                    <td>
                        <eluc:nationality id="ucNatioanlity" runat="server" appenddatabounditems="true" cssclass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblSex" runat="server" Text="Sex"></asp:Literal>
                    </td>
                    <td>
                        <eluc:sex id="ucSex" runat="server" cssclass="dropdown_mandatory" appenddatabounditems="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblDateofBirth" runat="server" Text="Date of Birth"></asp:Literal>
                    </td>
                    <td>
                        <eluc:date id="ucDateOfBirth" runat="server" cssclass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblNOK" runat="server" Text="NOK"></asp:Literal>
                    </td>
                    <td colspan="3">
                        <asp:CheckBox ID="chkNOK" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                         <b><asp:Literal ID="lblContactInformation" runat="server" Text="Contact Information"></asp:Literal></b>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" rowspan="3">
                        <eluc:commonaddress id="ucAddress" runat="server" />
                    </td>
                    <td>
                        <asp:Literal ID="lblPhoneNumber" runat="server" Text="Phone Number"></asp:Literal>
                    </td>
                    <td>
                        <eluc:phonenumber id="ucPhoneNumber" runat="server" cssclass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblEMail" runat="server" Text="E-Mail"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="input" MaxLength="500"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblMobileNumber" runat="server" Text="Mobile Number"></asp:Literal>
                    </td>
                    <td>
                        <eluc:phonenumber id="ucMobileNumber" runat="server" cssclass="input" ismobilenumber="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <font color="blue"><asp:Literal ID="lblNoteForLandlinenumbersexcludethe0beforetheAreaCodeEgForChennai4422222222ForMobileNumbersexcludethe0beforethenumber" runat="server" Text="Note:For Landline numbers, exclude the '0' before the 'Area Code'. (Eg. For Chennai: 44  22222222)For Mobile Numbers, exclude the '0' before the number."></asp:Literal></font>
                    </td>
                </tr>               
                <tr>
                    <td colspan="6">
                        <hr />
                        <b><asp:Literal ID="lblBankDetails" runat="server" Text="Bank Details"></asp:Literal></b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lbklBankName" runat="server" Text="Bank Name"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtBankName" runat="server" CssClass="input" MaxLength="200"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblAccountNumber" runat="server" Text="Account Number"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="input" MaxLength="200"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblBranch" runat="server" Text="Branch"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtBranch" runat="server" CssClass="input" MaxLength="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                        <table>
                            <tr>
                                <td>
                                    <b><asp:Literal ID="lblBankAddress" runat="server" Text="Bank Address"></asp:Literal></b>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" rowspan="3">
                        <eluc:commonaddress id="ucBankAddress" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <eluc:status id="ucStatus" runat="server" />
    </div>
    </form>
</body>
</html>
