<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreEmployeeAddressConfirmationEdit.aspx.cs" Inherits="CrewOffshore_CrewOffshoreEmployeeAddressConfirmationEdit" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
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
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" lang="javascript">

            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode
                if (charCode > 31 && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewNewApplicantAddress" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <eluc:TabStrip ID="CrewNewApplicantAddressMain" runat="server" OnTabStripCommand="CrewNewApplicantAddressMain_TabStripCommand" Title="Address"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <br />
            <table width="100%" cellpadding="1" cellspacing="1">
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
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td nowrap>
                        <b>
                            <telerik:RadLabel ID="lblPermanentAddress" runat="server" Text="Permanent Address (Office)"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkLocalCopyAddress" runat="server" Text="Copy Local Address" Visible="false" AutoPostBack="True"
                            OnCheckedChanged="chkCopyAddress_CheckedChanged" />
                    </td>
                    <td nowrap>
                        <b>
                            <telerik:RadLabel ID="lblPermanentAddressemp" runat="server" Text="Permanent Address (Seafarer)"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkCopyAddress" runat="server" Text="Copy Permanent Address" AutoPostBack="True"
                            OnCheckedChanged="chkCopyAddress_CheckedChanged" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <eluc:CommonAddress ID="PermanentAddress" runat="server" />
                    </td>
                    <td colspan="2">
                        <eluc:CommonAddress ID="PermanentAddressemp" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <b>
                            <telerik:RadLabel ID="lblPermanentContact" runat="server" Text="Permanent Contact(Office)"></telerik:RadLabel>
                        </b>
                    </td>
                    <td colspan="2">
                        <b>
                            <telerik:RadLabel ID="lblPermanentContactemp" runat="server" Text="Permanent Contact(Seafarer)"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPhoneNumber" runat="server" Text="Phone Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <td>
                                    <eluc:PhoneNumber ID="txtPhoneNumber" runat="server" Width="100px" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblPhoneNumber2" runat="server" Text="Phone Number"></telerik:RadLabel>
                                    <eluc:PhoneNumber ID="txtPhoneNumber2" runat="server" Width="100px" />
                                    &nbsp;
                                    <span id="Span2" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip2" Width="300px" ShowEvent="onmouseover"
                                        RelativeTo="Element" Animation="Fade" TargetControlID="Span2" IsClientID="true"
                                        HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true"
                                        Text="For Phone numbers, exclude the '0' before the 'Area Code'. (Eg. For Chennai:44-22222222)">
                                    </telerik:RadToolTip>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td >
                        <telerik:RadLabel ID="lblPhoneNumberemp" runat="server" Text="Phone Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <td>
                                    <eluc:PhoneNumber ID="txtPhoneNumberemp" runat="server" CssClass="input" Width="100px" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblPhoneNumber2emp" runat="server" Text="Phone Number"></telerik:RadLabel>
                                    <eluc:PhoneNumber ID="txtPhoneNumber2emp" runat="server" CssClass="input" Width="100px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMobileNumber" runat="server" Text="Mobile Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 150px">
                                    <%-- <eluc:PhoneNumber ID="txtMobileNumber" IsMobileNumber="true" runat="server" CssClass="input" /> --%>
                                    <eluc:MobileNumber ID="txtMobileNumber" IsMobileNumber="true" runat="server" CssClass="input" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblMobileNumber2" runat="server" Text="Mobile Number"></telerik:RadLabel>
                                    <eluc:MobileNumber ID="txtMobileNumber2" IsMobileNumber="true" runat="server" CssClass="input" />
                                    &nbsp;
                                    <span id="Span3" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip3" Width="300px" ShowEvent="onmouseover"
                                        RelativeTo="Element" Animation="Fade" TargetControlID="Span3" IsClientID="true"
                                        HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true"
                                        Text="For Mobile Numbers, exclude the '0' before the number.">
                                    </telerik:RadToolTip>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMobileNumberemp" runat="server" Text="Mobile Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 150px">
                                
                                    <eluc:MobileNumber ID="txtMobileNumberemp" IsMobileNumber="true" runat="server" CssClass="input" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblMobileNumber2emp" runat="server" Text="Mobile Number"></telerik:RadLabel>
                                    <eluc:MobileNumber ID="txtMobileNumber2emp" IsMobileNumber="true" runat="server" CssClass="input" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMobileNumber3" runat="server" Text="Mobile Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 150px">
                                    <eluc:MobileNumber ID="txtMobileNumber3" IsMobileNumber="true" runat="server" CssClass="input" />
                                    <%--<eluc:PhoneNumber ID="txtMobileNumber3" IsMobileNumber="true" runat="server" CssClass="input" />--%>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblPerRelation" runat="server" Text="Relation"></telerik:RadLabel>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                <eluc:Relation ID="ucPerRelation" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    QuickTypeCode="7" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMobileNumber3emp" runat="server" Text="Mobile Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 150px">
                                    <%--<eluc:PhoneNumber ID="txtLocalMobileNumber3" IsMobileNumber="true" runat="server"  CssClass="input" />--%>
                                    <eluc:MobileNumber ID="txtMobileNumber3emp" IsMobileNumber="true" runat="server" CssClass="input" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblPerRelationemp" runat="server" Text="Relation"></telerik:RadLabel>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                <eluc:Relation ID="ucPerRelationemp" runat="server" CssClass="input" AppendDataBoundItems="true"
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
                    <td>
                        <telerik:RadTextBox ID="txtEmail" runat="server" Width="80%" CssClass="input_mandatory"></telerik:RadTextBox>
                        &nbsp;
                        <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                        <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="300px" ShowEvent="onmouseover"
                            RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true"
                            HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true"
                            Text="(Use &quot;,&quot; to add more E-Mail Eg : (xx@xx.com,yy@yy.com)) Enter only the correct E-Mail. If not available please enter &quot;NA&quot;">
                        </telerik:RadToolTip>
                    </td>
                   
                </tr>
                <tr> <td>
                        <telerik:RadLabel ID="lblNearestAirport" runat="server" Text="Nearest Airport"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Airport runat="server" ID="ucAirport" AppendDataBoundItems="true" CssClass="input" />
                    </td>
                     <td>
                        <telerik:RadLabel ID="lblNearestAirportemp" runat="server" Text="Nearest Airport"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Airport runat="server" ID="ucAirportemp" AppendDataBoundItems="true" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td nowrap>
                        <telerik:RadLabel ID="lblPlaceofEngagement" runat="server" Text="Port of Engagement"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SeaPort ID="ddlPortofEngagement" runat="server" CssClass="input" AppendDataBoundItems="true"
                            Width="150px" />
                    </td>
                     <td nowrap>
                        <telerik:RadLabel ID="lblPlaceofEngagementemp" runat="server" Text="Port of Engagement"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SeaPort ID="ddlPortofEngagementemp" runat="server" CssClass="input" AppendDataBoundItems="true"
                            Width="150px" />
                    </td>
                </tr>
           
                <tr>
                    <td nowrap>
                        <b>
                            <telerik:RadLabel ID="lblLocalAddress" runat="server" Text="Local Address(Office)"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="RadCheckBox1" runat="server" Text="Copy Local Address" AutoPostBack="True"
                            OnCheckedChanged="chkCopyAddress_CheckedChanged" Visible="false" />
                    </td>
                    <td nowrap>
                        <b>
                            <telerik:RadLabel ID="lblLocalAddressemp" runat="server" Text="Local Address"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="RadCheckBox2" runat="server" Text="Copy Permanent Address" AutoPostBack="True"
                            OnCheckedChanged="chkCopyAddress_CheckedChanged" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <eluc:CommonAddress ID="LocalAddress" runat="server" />
                    </td>
                    <td colspan="2">
                        <eluc:CommonAddress ID="LocalAddressemp" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <b>
                            <telerik:RadLabel ID="lblLocalContact" runat="server" Text="Local Contact(office)"></telerik:RadLabel>
                        </b>
                    </td>
                    <td colspan="2">
                        <b>
                            <telerik:RadLabel ID="lblLocalContactemp" runat="server" Text="Local Contact(seafarer)"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbllocalPhoneNumberemp" runat="server" Text="Phone Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <eluc:PhoneNumber ID="txtlocalPhoneNumber" runat="server" Width="100px" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblLocalPhoneNumber2" runat="server" Text="Phone Number"></telerik:RadLabel>
                                    <eluc:PhoneNumber ID="txtLocalPhoneNumber2" runat="server" Width="100px" />
                                    &nbsp;
                                    <span id="Span4" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip4" Width="300px" ShowEvent="onmouseover"
                                        RelativeTo="Element" Animation="Fade" TargetControlID="Span2" IsClientID="true"
                                        HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true"
                                        Text="For Phone numbers, exclude the '0' before the 'Area Code'. (Eg. For Chennai:44-22222222)">
                                    </telerik:RadToolTip>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbllocalPhoneNumber" runat="server" Text="Phone Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <eluc:PhoneNumber ID="txtlocalPhoneNumberemp" runat="server" CssClass="input" Width="100px" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblLocalPhoneNumber2emp" runat="server" Text="Phone Number"></telerik:RadLabel>
                                    <eluc:PhoneNumber ID="txtLocalPhoneNumber2emp" runat="server" CssClass="input" Width="100px" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLocalMobileNumber" runat="server" Text="Mobile Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td style="width: 150px">
                                    <eluc:MobileNumber ID="txtLocalMobileNumber" IsMobileNumber="true" runat="server" CssClass="input" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblLocalMobileNumber2" runat="server" Text="Mobile Number"></telerik:RadLabel>
                                    <eluc:MobileNumber ID="txtLocalMobileNumber2" IsMobileNumber="true" runat="server" CssClass="input" />
                                    &nbsp;
                                    <span id="Span5" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip5" Width="300px" ShowEvent="onmouseover"
                                        RelativeTo="Element" Animation="Fade" TargetControlID="Span3" IsClientID="true"
                                        HideEvent="ManualClose" Position="MiddleRight" EnableRoundedCorners="true"
                                        Text="For Mobile Numbers, exclude the '0' before the number.">
                                    </telerik:RadToolTip>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocalMobileNumberemp" runat="server" Text="Mobile Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td style="width: 150px">
                              
                                    <eluc:MobileNumber ID="txtLocalMobileNumberemp" IsMobileNumber="true" runat="server" CssClass="input" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblLocalMobileNumber2emp" runat="server" Text="Mobile Number"></telerik:RadLabel>
                                    <eluc:MobileNumber ID="txtLocalMobileNumber2emp" IsMobileNumber="true" runat="server" CssClass="input" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLocalMobileNumber3" runat="server" Text="Mobile Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 150px">
                                    <eluc:MobileNumber ID="txtLocalMobileNumber3" IsMobileNumber="true" runat="server" CssClass="input" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblLocRelation" runat="server" Text="Relation"></telerik:RadLabel>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                <eluc:Relation ID="ucLocRelation" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    QuickTypeCode="7" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocalMobileNumber3emp" runat="server" Text="Mobile Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 150px">
                                    <%--<eluc:PhoneNumber ID="txtLocalMobileNumber3" IsMobileNumber="true" runat="server"  CssClass="input" />--%>
                                    <eluc:MobileNumber ID="txtLocalMobileNumber3emp" IsMobileNumber="true" runat="server" CssClass="input" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblLocRelationemp" runat="server" Text="Relation"></telerik:RadLabel>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                <eluc:Relation ID="ucLocRelationemp" runat="server" CssClass="input" AppendDataBoundItems="true"
                                    QuickTypeCode="7" />
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
