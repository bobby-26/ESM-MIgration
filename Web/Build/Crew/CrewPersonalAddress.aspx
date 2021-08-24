<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPersonalAddress.aspx.cs" Inherits="CrewPersonalAddress" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonAddress" Src="../UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MobileNumber" Src="../UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Airport" Src="../UserControls/UserControlAirport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="../UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Relation" Src="../UserControls/UserControlQuick.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
   <form id="frmCrewPersonalAddress" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader">
            <eluc:Title runat="server" ID="ttlAddress" Text="Crew Address" ShowMenu="false">
            </eluc:Title>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="CrewAddressMain" runat="server" OnTabStripCommand="CrewAddressMain_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <table width="100%">
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
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <b><asp:Literal ID="lblPermanentAddress" runat="server" Text="Permanent Address"></asp:Literal> </b>
                </td>
                <td colspan="3">
                    <asp:CheckBox ID="chkLocalCopyAddress" runat="server" Text="Copy Local Address" AutoPostBack="True"
                        OnCheckedChanged="chkCopyAddress_CheckedChanged" />
                </td>
                <td>
                    <b><asp:Literal ID="lblLocalAddress" runat="server" Text="Local Address"></asp:Literal> </b>
                </td>
                <td colspan="3">
                    <asp:CheckBox ID="chkCopyAddress" runat="server" Text="Copy Permanent Address" AutoPostBack="True"
                        OnCheckedChanged="chkCopyAddress_CheckedChanged" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <eluc:CommonAddress ID="PermanentAddress" runat="server" />
                </td>
                <td colspan="4">
                    <eluc:CommonAddress ID="LocalAddress" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <b><asp:Literal ID="lblPermanent" runat="server" Text="Permanent Contact"></asp:Literal> </b>
                </td>
                <td colspan="4">
                    <b><asp:Literal ID="lblLocal" runat="server" Text="Local Contact"></asp:Literal></b>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblPhoneNumber1" runat="server" Text="Phone Number"></asp:Literal>
                </td>
                <td>
                    <eluc:PhoneNumber ID="txtPhoneNumber"  runat="server" />
                </td>
                <td>
                    <asp:Literal ID="lblPhoneNumber2" runat="server" Text="Phone Number"></asp:Literal>
                </td>
                <td>
                    <eluc:PhoneNumber ID="txtPhoneNumber2" runat="server" />
                </td>
                <td>
                    <asp:Literal ID="lblLocalPhoneNumber" runat="server" Text="Phone Number"></asp:Literal>
                </td>
                <td>
                    <eluc:PhoneNumber ID="txtlocalPhoneNumber"  runat="server" CssClass="input" />
                </td>
                <td>
                    <asp:Literal ID="lblLocalPhoneNumber2" runat="server" Text="Phone Number"></asp:Literal>
                </td>
                <td>
                    <eluc:PhoneNumber ID="txtLocalPhoneNumber2"  runat="server" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblMobileNumber" runat="server" Text="Mobile Number"></asp:Literal>
                </td>
                <td>
                    <eluc:PhoneNumber ID="txtMobileNumber" IsMobileNumber="true"  runat="server"
                        CssClass="input" />
                </td>
                <td>
                    <asp:Literal ID="lblMobileNumber2" runat="server" Text="Mobile Number"></asp:Literal>
                </td>
                <td>
                    <eluc:PhoneNumber ID="txtMobileNumber2" IsMobileNumber="true"  runat="server"
                        CssClass="input" />
                </td>
                <td>
                    <asp:Literal ID="lbllocalMobileNumber" runat="server" Text="Mobile Number"></asp:Literal>
                </td>
                <td>
                    <eluc:PhoneNumber ID="txtLocalMobileNumber" IsMobileNumber="true"  runat="server"
                        CssClass="input" />
                </td>
                <td>
                    <asp:Literal ID="lblLocalMobileNumber2" runat="server" Text="Mobile Number"></asp:Literal>
                </td>
                <td>
                     <eluc:PhoneNumber ID="txtLocalMobileNumber2" IsMobileNumber="true"  runat="server"
                        CssClass="input" />
                </td>
                
            </tr>
            <tr>
                <td width="7%">
                    <asp:Literal ID="lblMobileno" runat="server" Text="Mobile Number"></asp:Literal>
                </td>
                <td colspan="3">
                    <table cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                           
                                <eluc:PhoneNumber ID="txtMobileNumber3" IsMobileNumber="true"  runat="server"
                                    CssClass="input" />
                            </td>
                            <td>
                              <asp:Literal ID="lblRelation" runat="server" Text="Relation"></asp:Literal>
                                <eluc:Relation ID="ucPerRelation" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    QuickTypeCode="7" />
                                   
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <asp:Literal ID="lblMobile" runat="server" Text="Mobile Number"></asp:Literal>
                </td>
                <td colspan="3">
                    <table cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <eluc:PhoneNumber ID="txtLocalMobileNumber3" IsMobileNumber="true"  runat="server"
                                    CssClass="input" />
                            </td>
                            <td>
                    <asp:Literal ID="lblRelation1" runat="server" Text="Relation"></asp:Literal>
                                <eluc:Relation ID="ucLocRelation" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    QuickTypeCode="7" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblEmail" runat="server" Text="E-Mail"></asp:Literal>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtEmail" runat="server" Width="90%" CssClass="input_mandatory"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblNearestAirport" runat="server" Text="Nearest Airport"></asp:Literal>
                </td>
                <td colspan="3">
                    <eluc:Airport runat="server" ID="ucAirport" AppendDataBoundItems="true" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td colspan="4" align="center">
                    <font color="blue">(Use "," to add more E-Mail Eg : (xx@xx.com,yy@yy.com)</font>
                </td>
                <td colspan="4">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="5">
                    <font color="blue">Note: &nbsp;For Landline numbers, exclude the '0' before the 'Area
                        Code'. (Eg. For Chennai:&nbsp;&nbsp; 44 &nbsp; &nbsp; &nbsp; 22222222) &nbsp;&nbsp;
                        &nbsp; &nbsp; For Mobile Numbers, exclude the '0' before the number.</font>
                </td>
            </tr>
        </table>
        
        <eluc:Status ID="ucStatus" runat="server" />
    </div>
    </form>
</body>
</html>
