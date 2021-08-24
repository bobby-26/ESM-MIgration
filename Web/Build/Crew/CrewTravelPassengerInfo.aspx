<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelPassengerInfo.aspx.cs"
    Inherits="CrewTravelPassengerInfo" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Passenger Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewMainPersonel" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="88%">            
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="Server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFirstname" runat="server"  Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="Server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMiddlename" runat="server"  Width="80%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLastname" runat="server"  Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofBirth" runat="Server" Text="Date of Birth"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateofBirth" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassportNo" runat="server" Text="Passport No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassport" runat="server"  Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofIssue" runat="server" Text="Date of issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtpdateofissue" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofIssue" runat="server" Text="Place of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtpplaceodissue" runat="server"  Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofExpiry1" runat="Server" Text="Date of Expiry"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtpdateofexpiry" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCDCNo" runat="server" Text="CDC No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtcdcno" runat="server"  Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofIssue1" runat="server" Text="Date of issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtcdcdateofissue" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlaceofIssue1" runat="server" Text="Place of Issue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtcdcplaceofissue" runat="server"  Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDateofExpiry" runat="server" Text=" Date of Expiry"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtcdcdateofexpiry" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblUSVISA" runat="server" Text="US VISA"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtusvisa" runat="server"  Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOtherVisaDetails" runat="server" Text="Other Visa Details"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtothervisa" runat="server"  Width="80%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Nationality ID="ucnationality" runat="server" AppendDataBoundItems="true"  />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOnOffSigner" runat="server" Text="On/Off Signer"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlonsigner" runat="server" >
                            <Items>
                                <telerik:RadComboBoxItem Text="NA" Value="2" />
                                <telerik:RadComboBoxItem Text="off-signer" Value="0" />
                                <telerik:RadComboBoxItem Text="on-signer" Value="1" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOrigin" runat="server" Text="Origin"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtOrigin"  Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTravelDate" runat="Server" Text="Travel Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDateOftravel" runat="server"  />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDestination" runat="server" Text="Destination"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDestination" runat="server" Width="80%" ></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblArrivalDate" runat="server" Text=" Arrival Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDateOfArrival" runat="server"  />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
