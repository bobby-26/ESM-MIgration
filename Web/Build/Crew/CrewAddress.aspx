<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewAddress.aspx.cs" Inherits="CrewAddress" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonAddress" Src="../UserControls/UserControlCommonAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MobileNumber" Src="../UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Airport" Src="../UserControls/UserControlAirport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="../UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Relation" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewAddress" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>            
            <eluc:TabStrip ID="CrewAddressMain" runat="server" OnTabStripCommand="CrewAddressMain_TabStripCommand" Title="Address"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFirstName" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMiddleName" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastName" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtRank" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblPermanentAddress" runat="server" Text="Permanent Address"></telerik:RadLabel>
                        </b>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="chkLocalCopyAddress" runat="server" Text="Copy Local Address" AutoPostBack="True"
                            OnCheckedChanged="chkCopyAddress_CheckedChanged">
                        </telerik:RadCheckBox>
                    </td>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblLocalAddress" runat="server" Text="Local Address"></telerik:RadLabel>
                        </b>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="chkCopyAddress" runat="server" Text="Copy Permanent Address" AutoPostBack="True"
                            OnCheckedChanged="chkCopyAddress_CheckedChanged">
                        </telerik:RadCheckBox>
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
                        <b>
                            <telerik:RadLabel ID="lblPermanentContact" runat="server" Text="Permanent Contact"></telerik:RadLabel>
                        </b>
                    </td>
                    <td colspan="4">
                        <b>
                            <telerik:RadLabel ID="lblLocalContact" runat="server" Text="Local Contact"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPhoneNumber" runat="server" Text="Phone Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:PhoneNumber ID="txtPhoneNumber" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPhoneNumber2" runat="server" Text="Phone Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:PhoneNumber ID="txtPhoneNumber2" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocalPhoneNumber" runat="server" Text="Phone Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:PhoneNumber ID="txtlocalPhoneNumber" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocalPhoneNumber2" runat="server" Text="Phone Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:PhoneNumber ID="txtLocalPhoneNumber2" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMobileNumber" runat="server" Text="Mobile Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MobileNumber ID="txtMobileNumber" IsMobileNumber="true" runat="server"
                           />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMobileNumber2" runat="server" Text="Mobile Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MobileNumber ID="txtMobileNumber2" IsMobileNumber="true" runat="server"
                            />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocalMobileNumber" runat="server" Text="Mobile Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MobileNumber ID="txtLocalMobileNumber" IsMobileNumber="true" runat="server"
                           />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocalMobileNumber2" runat="server" Text="Mobile Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MobileNumber ID="txtLocalMobileNumber2" IsMobileNumber="true" runat="server"
                           />
                    </td>

                </tr>
                <tr>
                    <td width="7%">
                        <asp:Literal ID="lblMobileNumber3" runat="server" Text="Mobile Number"></asp:Literal>
                    </td>
                    <td colspan="3">
                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <td>
                                    <eluc:MobileNumber ID="txtMobileNumber3" IsMobileNumber="true" runat="server"
                                        />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblPerRelation" runat="server" Text="Relation"></telerik:RadLabel>
                                    <eluc:Relation ID="ucPerRelation" runat="server"  AppendDataBoundItems="true"
                                        QuickTypeCode="7" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocalMobileNumber3" runat="server" Text="Mobile Number"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <td>
                                    <eluc:MobileNumber ID="txtLocalMobileNumber3" IsMobileNumber="true" runat="server"
                                       />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblLocRelation" runat="server" Text="Relation"></telerik:RadLabel>
                                    <eluc:Relation ID="ucLocRelation" runat="server"  AppendDataBoundItems="true"
                                        QuickTypeCode="7" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEMail" runat="server" Text="E-Mail"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtEmail" runat="server" Width="360px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNearestAirport" runat="server" Text="Nearest Airport"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Airport runat="server" ID="ucAirport" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofEngagement" runat="server" Text="Port of Engagement"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SeaPort ID="ddlPortofEngagement" runat="server" AppendDataBoundItems="true"
                            Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastUpdateBy" runat="server" Text="Last Update By"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtLastUpdatedBy" runat="server" Width="150px" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastUpdateDate" runat="server" Text="Last Update Date"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtLastUpdateDate" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <telerik:RadLabel ID="lblNote" runat="server" Text="Note:"></telerik:RadLabel>
                        &nbsp;<telerik:RadLabel ID="lbllandlinenote" runat="server" Text="For Landline numbers, exclude the '0' before the 'Area Code'. (Eg. For Chennai:"></telerik:RadLabel>
                        &nbsp;&nbsp;
                              <telerik:RadLabel ID="lbl44" runat="server" Text="44"></telerik:RadLabel>
                        &nbsp; &nbsp; 
                               <telerik:RadLabel ID="lbl22222222" runat="server" Text="22222222,"></telerik:RadLabel>
                        &nbsp;&nbsp;                     
                               <telerik:RadLabel ID="lblmobilenote" runat="server" Text="For Mobile Numbers, exclude the '0' before the number."></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="5">
                        <telerik:RadLabel ID="lblemailnote" runat="server" Text="(Use &quot;,&quot; to add more E-Mail Eg : (xx@xx.com,yy@yy.com)"></telerik:RadLabel>
                    </td>
                </tr>

            </table>

            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
