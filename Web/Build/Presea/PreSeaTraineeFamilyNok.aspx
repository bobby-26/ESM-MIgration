<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaTraineeFamilyNok.aspx.cs" Inherits="PreSeaTraineeFamilyNok" %>

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
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title>PreSea Personal FamilyNok</title>
    <style type="text/css">
        .style1
        {
            width: 255px;
        }
        .style2
        {
            width: 172px;
        }
        .style3
        {
            width: 13px;
        }
    </style>
   <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

   </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaTraineeFamilyNok" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader">
            <eluc:Title runat="server" ID="ttlAddress" Text="Trainee Family/Nok" ShowMenu="false">
            </eluc:Title>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="PreSeaFamily" runat="server" OnTabStripCommand="PreSeaFamily_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <table width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    First Name
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    Middle Name
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    Last Name
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
                <td>
                    Batch
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtBatch" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
        </table>
        <hr />
        <div>
            <b>Family Member</b>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td rowspan="7" colspan="2" valign="top">
                        <asp:ListBox ID="lstFamily" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            OnSelectedIndexChanged="lstFamily_SelectedIndexChanged" Width="98%" CssClass="input"
                            Height="150px"></asp:ListBox>
                    </td>
                    <td class="style2">
                        First Name
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="txtFamilyFirstName" runat="server" CssClass="input_mandatory" MaxLength="200"></asp:TextBox>
                    </td>
                    <td rowspan="3">
                        <a id="aPreSeaFamilyImg" runat="server">
                            <asp:Image ID="imgPhoto" runat="server" Height="100px" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                                Width="120px" /></a>
                        <asp:ImageButton ID="imgIDCard" runat="server" ImageUrl="<%$ PhoenixTheme:images/id-card.png %>"
                            Visible="false" ToolTip="Family ID Card" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        Middle Name
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="txtFamilyMiddleName" runat="server" CssClass="input" MaxLength="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        Last Name
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="txtFamilyLastName" runat="server" CssClass="input" MaxLength="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        Relationship
                    </td>
                    <td class="style3">
                        <eluc:Relation ID="ucRelation" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true"
                            OnTextChangedEvent="AnniversaryDetails" AppendDataBoundItems="true" />
                    </td>
                    <td colspan="2">
                        <asp:FileUpload ID="txtFileUpload" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        Anniversary Date
                    </td>
                    <td class="style3">
                        <eluc:Date ID="ucAnniversaryDate" Enabled="false" runat="server" CssClass="readonlytextbox" />
                    </td>
                    <td>
                        Nationality
                    </td>
                    <td>
                        <eluc:Nationality ID="ucNatioanlity" runat="server" AppendDataBoundItems="true" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        Sex
                    </td>
                    <td class="style3">
                        <eluc:Sex ID="ucSex" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        Date of Birth
                    </td>
                    <td>
                        <eluc:Date ID="ucDateOfBirth" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td class="style2">
                        NOK
                    </td>
                    <td colspan="3">
                        <asp:CheckBox ID="chkNOK" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                        <table>
                            <tr>
                                <td>
                                    <b>Office/Contact Address Information</b>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkCPA" runat="server" OnClick="CopyAddress_Click">Copy Permanent Address</asp:LinkButton>
                                </td>
                                <td>
                                    <asp:LinkButton ID="lnkCLA" runat="server" OnClick="CopyAddress_Click">Copy Local Address</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" rowspan="3">
                        <eluc:CommonAddress ID="ucAddress" runat="server" />
                    </td>
                    <td>
                        Occupation
                    </td>
                    <td>
                        <asp:TextBox ID="txtOccupation" runat="server" CssClass="input" Width="200"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Phone Number
                    </td>
                    <td>
                        <eluc:PhoneNumber ID="ucPhoneNumber" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        E-Mail
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail" runat="server" CssClass="input" MaxLength="500"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Mobile Number
                    </td>
                    <td>
                        <eluc:PhoneNumber ID="ucMobileNumber" runat="server" CssClass="input" IsMobileNumber="true" />
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Parents Anual Income Rs.
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAnualIncome" runat="server" CssClass="input">
                            <asp:ListItem Text="-Select-" Value="0"></asp:ListItem>
                            <asp:ListItem Text="Less than 120000" Value="1"></asp:ListItem>
                            <asp:ListItem Text="1 Lac 20 Thousand - 2 Lacs 40 Thousand" Value="2"></asp:ListItem>
                            <asp:ListItem Text="2 Lacs 40 Thousand - 3 Lacs 60 Thousand" Value="3"></asp:ListItem>
                            <asp:ListItem Text="3 Lacs 60 Thousand - 6 Lacs " Value="4"></asp:ListItem>
                            <asp:ListItem Text="Above 6 Lacs" Value="5"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        SIMS Siblings
                    </td>
                    <td>
                        <asp:TextBox ID="txtSiblings" runat="server" TextMode="MultiLine" CssClass="input"
                            Width="250" Height="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <font color="blue">Note: &nbsp;For Landline numbers, exclude the '0' before the 'Area
                            Code'. (Eg. For Chennai:&nbsp;&nbsp; 44 &nbsp; &nbsp; &nbsp; 22222222) &nbsp; &nbsp;
                            &nbsp; For Mobile Numbers, exclude the '0' before the number.</font>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                        <b>Bank Details</b>
                    </td>
                </tr>
                <tr>
                    <td class="style1">
                        Bank Name
                    </td>
                    <td>
                        <asp:TextBox ID="txtBankName" runat="server" CssClass="input" MaxLength="200"></asp:TextBox>
                    </td>
                    <td class="style2">
                        Account Number
                    </td>
                    <td class="style3">
                        <asp:TextBox ID="txtAccountNumber" runat="server" CssClass="input" MaxLength="200"></asp:TextBox>
                    </td>
                    <td>
                        Branch
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
                                    <b>Bank Address</b>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" rowspan="3">
                        <eluc:CommonAddress ID="ucBankAddress" runat="server" />
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <eluc:Status ID="ucStatus" runat="server" />
    </div>
    </form>
</body>
</html>
