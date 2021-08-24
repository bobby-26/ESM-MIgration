<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaNewApplicantPersonalAddress.aspx.cs"
    Inherits="PreSeaNewApplicantPersonalAddress" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title>PreSea NewApplicant Personal Address</title>
   <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

      </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPreSeaPersonalAddress" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader">
            <eluc:Title runat="server" ID="ttlAddress" Text="Applicant Address" ShowMenu="false">
            </eluc:Title>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="PreSeaPersonalAddressMain" runat="server" OnTabStripCommand="PreSeaPersonalAddressMain_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <table width="100%">
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
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <b>Postal Address </b>
                </td>
                <td colspan="3">
                    <asp:CheckBox ID="chkPostalCopyAddress" runat="server" Text="Copy Permanent Address"
                        AutoPostBack="True" OnCheckedChanged="chkCopyAddress_CheckedChanged" />
                </td>
                <td>
                    <b>Permanent Address </b>
                </td>
                <td colspan="3">
                    <asp:CheckBox ID="chkCopyAddress" runat="server" Text="Copy Postal Address" AutoPostBack="True"
                        OnCheckedChanged="chkCopyAddress_CheckedChanged" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <eluc:CommonAddress ID="PostalAddress" runat="server" />
                </td>
                <td colspan="4">
                    <eluc:CommonAddress ID="PermanentAddress" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <b>Postal Contact</b>
                </td>
                <td colspan="4">
                    <b>Permanent Contact </b>
                </td>
            </tr>
            <tr>
                <td>
                    Phone Number
                </td>
                <td>
                    <eluc:PhoneNumber ID="txtPhoneNumber" runat="server" />
                </td>
                <td>
                    Phone Number
                </td>
                <td>
                    <eluc:PhoneNumber ID="txtPhoneNumber2" runat="server" />
                </td>
                <td>
                    Phone Number
                </td>
                <td>
                    <eluc:PhoneNumber ID="txtPostalPhoneNumber" runat="server" CssClass="input" />
                </td>
                <td>
                    Phone Number
                </td>
                <td>
                    <eluc:PhoneNumber ID="txtPostalPhoneNumber2" runat="server" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td>
                    Mobile Number
                </td>
                <td>
                      <eluc:MobileNumber ID="txtMobileNumber" IsMobileNumber="true" runat="server"  />               
                </td>
                <td>
                    Mobile Number
                </td>
                <td>
                    <eluc:MobileNumber ID="txtMobileNumber2" IsMobileNumber="true" runat="server"  />                         
                </td>
                <td>
                    Mobile Number
                </td>
                <td>
                    <eluc:MobileNumber ID="txtPostalMobileNumber" IsMobileNumber="true" runat="server"  />                       
                </td>
                <td>
                    Mobile Number
                </td>
                <td>
                    <eluc:MobileNumber ID="txtPostalMobileNumber2" IsMobileNumber="true" runat="server"  />                    
                </td>
            </tr>
            <tr>
                <td width="7%">
                    Mobile Number
                </td>
                <td colspan="3">
                    <table cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <eluc:MobileNumber ID="txtMobileNumber3" IsMobileNumber="true" runat="server"  />                                          
                            </td>
                            <td>
                                Relation
                                <eluc:Relation ID="ucPerRelation" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    QuickTypeCode="7" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    Mobile Number
                </td>
                <td colspan="3">
                    <table cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <eluc:PhoneNumber ID="txtPostalMobileNumber3" IsMobileNumber="true" runat="server"
                                    CssClass="input" />
                            </td>
                            <td>
                                Relation
                                <eluc:Relation ID="ucLocRelation" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    QuickTypeCode="7" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    E-Mail
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtEmail" runat="server" Width="90%" CssClass="input_mandatory"></asp:TextBox>
                </td>
            </tr>
        </table>
        <hr />
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td width="7%">
                    Address Valid in Years
                </td>
                <td colspan="3">
                    <table cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <eluc:Number runat="server" ID="ucPostalYears" IsInteger="true" CssClass="input_mandatory" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    Address Valid in Years
                </td>
                <td colspan="3">
                    <table cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <eluc:Number runat="server" ID="ucPermanentYears" IsInteger="true" CssClass="input_mandatory" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td width="7%">
                    Address Valid in Months
                </td>
                <td colspan="3">
                    <table cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <eluc:Number runat="server" ID="ucPostalMonths" IsInteger="true" CssClass="input_mandatory" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    Address Valid in Months
                </td>
                <td colspan="3">
                    <table cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <eluc:Number runat="server" ID="ucPermanentMonths" IsInteger="true" CssClass="input_mandatory" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    Last Update By
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtLastUpdatedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                        Width="150px"></asp:TextBox>
                </td>
                <td>
                    Last Update Date
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtLastUpdateDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
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
