<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelPassengersEntry.aspx.cs"
    Inherits="CrewTravelPassengersEntry" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Time" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TravelReason" Src="~/UserControls/UserControlTravelReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MUCCity" Src="~/UserControls/UserControlMultiColumnCity.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew travel</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:status ID="ucstatus" runat="server" Visible="false" />
            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuCrewTraveladd" runat="server" OnTabStripCommand="MenuCrewTraveladd_TabStripCommand"></eluc:TabStrip>
            <asp:Panel ID="pnlpersonal" runat="server" GroupingText="Personal Details">
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td width="15%">
                            <telerik:RadLabel ID="lblFirstName" runat="Server" Text="First Name"></telerik:RadLabel>
                        </td>
                        <td width="25%">
                            <span id="spnPickListOfficeStaff">
                                <telerik:RadTextBox ID="txtStaffName" runat="server" CssClass="input_mandatory" MaxLength="200"
                                    Width="80%">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtStaffId" runat="server" CssClass="hidden" Enabled="false" MaxLength="50"
                                    Width="5px">
                                </telerik:RadTextBox>
                                <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top"
                                    ID="imguser" OnClientClick="return showPickList('spnPickListOfficeStaff', 'codehelp1', '', 'Common/CommonPickListOfficeStaff.aspx?framename=ifMoreInfo', true); "
                                    ToolTip="Select passenger from user">
                                <span class="icon"><i class="fas fa-list-alt-picklist"></i></span>
                                </asp:LinkButton>
                            </span>
                        </td>
                        <td width="15%">
                            <telerik:RadLabel ID="lblMiddleName" runat="Server" Visible="false" Text="Middle Name"></telerik:RadLabel>
                        </td>
                        <td width="15%">
                            <telerik:RadTextBox ID="txtmiddlename" runat="server" Visible="false" Width="80%"></telerik:RadTextBox>
                        </td>
                        <td width="15%">
                            <telerik:RadLabel ID="lblLastName" runat="server" Visible="false" Text="Last Name"></telerik:RadLabel>
                        </td>
                        <td width="15%">
                            <telerik:RadTextBox ID="txtlastname" runat="server" Visible="false" Width="80%"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <table width="100%" cellpadding="1" cellspacing="1" id="details" runat="server" visible="false">
                    <tr>
                        <td width="15%">
                            <telerik:RadLabel ID="lblDateofBirth" runat="Server" Text="Date of Birth"></telerik:RadLabel>
                        </td>
                        <td width="20%">
                            <eluc:Date ID="txtdob" runat="server" CssClass="input_mandatory" Width="80%" />
                        </td>
                        <td width="15%">
                            <telerik:RadLabel ID="lblPassportNo" runat="server" Text="Passport No"></telerik:RadLabel>
                        </td>
                        <td width="15%">
                            <telerik:RadTextBox ID="txtPassport" runat="server" CssClass="input_mandatory" Width="80%"></telerik:RadTextBox>
                        </td>
                        <td width="15%">
                            <telerik:RadLabel ID="lblPlaceofIssue" runat="server" Text="Place of Issue"></telerik:RadLabel>
                        </td>
                        <td width="15%">
                            <telerik:RadTextBox ID="txtpplaceodissue" runat="server" Width="80%"></telerik:RadTextBox>
                        </td>

                    </tr>
                    <tr>
                        <td width="15%">
                            <telerik:RadLabel ID="lblDateofIssue" runat="server" Text="Date of Issue"></telerik:RadLabel>
                        </td>
                        <td width="25%">
                            <eluc:Date ID="txtpdateofissue" runat="server" Width="80%" />
                        </td>
                        <td width="15%">
                            <telerik:RadLabel ID="lblDateofExpiry1" runat="Server" Text="Date of Expiry"></telerik:RadLabel>
                        </td>
                        <td width="15%">
                            <eluc:Date ID="txtpdateofexpiry" runat="server" Width="80%" />
                        </td>
                        <td width="15%">
                            <telerik:RadLabel ID="lblCDCNo" runat="server" Text="CDC No."></telerik:RadLabel>
                        </td>
                        <td width="15%">
                            <telerik:RadTextBox ID="txtcdcno" runat="server" Width="80%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="15%">
                            <telerik:RadLabel ID="lblPlaceofIssue1" runat="server" Text="Place of Issue"></telerik:RadLabel>
                        </td>
                        <td width="25%">
                            <telerik:RadTextBox ID="txtcdcplaceofissue" runat="server" Width="80%"></telerik:RadTextBox>
                        </td>
                        <td width="15%">
                            <telerik:RadLabel ID="lblDateofIssue1" runat="server" Text="Date of Issue"></telerik:RadLabel>
                        </td>
                        <td width="15%">
                            <eluc:Date ID="txtcdcdateofissue" runat="server" Width="80%" />
                        </td>
                        <td width="15%">
                            <telerik:RadLabel ID="lblDateofExpiry" runat="server" Text=" Date of Expiry"></telerik:RadLabel>
                        </td>
                        <td width="15%">
                            <eluc:Date ID="txtcdcdateofexpiry" runat="server" Width="80%" />
                        </td>
                    </tr>
                    <tr>
                        <td width="15%">
                            <telerik:RadLabel ID="lblOnOffSigner" runat="server" Text="On/Off Signer"></telerik:RadLabel>
                        </td>
                        <td width="25%">
                            <telerik:RadComboBox ID="ddlonsigner" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="80%"
                                Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                                <Items>
                                    <telerik:RadComboBoxItem Value="2" Text="NA" />
                                    <telerik:RadComboBoxItem Value="0" Text="off-signer" />
                                    <telerik:RadComboBoxItem Value="1" Text="on-signer" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td width="15%">
                            <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                        </td>
                        <td width="15%">
                            <eluc:Nationality ID="ucnationality" runat="server" AppendDataBoundItems="true"  Width="80%" CssClass="dropdown_mandatory" />
                        </td>
                        <td width="15%">
                            <telerik:RadLabel ID="lblUSVISA" runat="server" Text="US Visa"></telerik:RadLabel>
                        </td>
                        <td width="15%">
                            <telerik:RadTextBox ID="txtusvisa" runat="server" Width="80%"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td width="15%">
                            <telerik:RadLabel ID="lblOtherVisaDetails" runat="server" Text="Other Visa"></telerik:RadLabel>
                        </td>
                        <td width="15%" colspan="5">
                            <telerik:RadTextBox ID="txtothervisa" runat="server" Width="23%"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="Panel1" runat="server" GroupingText="Travel Details">
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td width="15%">
                            <telerik:RadLabel ID="lblOrigin" runat="server" Text="Origin"></telerik:RadLabel>
                        </td>
                        <td width="25%">
                            <eluc:MUCCity ID="txtOrigin" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td width="15%">
                            <telerik:RadLabel ID="lblDestination" runat="server" Text="Destination"></telerik:RadLabel>
                        </td>
                        <td width="15%">
                            <eluc:MUCCity ID="txtDestination" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td width="15%">
                            <telerik:RadLabel ID="lblPaymentMode" runat="server" Text="Payment Mode"></telerik:RadLabel>
                        </td>
                        <td width="15%">
                            <eluc:Hard ID="ucPaymentmodeAdd" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                HardList="<%# PhoenixRegistersHard.ListHard(1,185) %>" HardTypeCode="185" />
                        </td>
                    </tr>
                    <tr>
                        <td width="15%">
                            <telerik:RadLabel ID="DepatureDate" runat="server" Text="Departure Date"></telerik:RadLabel>
                        </td>
                        <td width="15%">
                            <eluc:Date ID="txtorigindate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                            <telerik:RadComboBox ID="ddlampmdeparture" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="20%"
                                Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                                <Items>
                                    <telerik:RadComboBoxItem Value="1" Text="AM" />
                                    <telerik:RadComboBoxItem Value="2" Text="PM" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td width="15%">
                            <telerik:RadLabel ID="lblArrivalDate" runat="server" Text="Arrival Date"></telerik:RadLabel>
                        </td>
                        <td width="20%">
                            <eluc:Date ID="txtDestinationdate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                            &nbsp;&nbsp;                                           
                         <telerik:RadComboBox ID="ddlampmarrival" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="20%"
                             Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                             <Items>
                                 <telerik:RadComboBoxItem Value="1" Text="AM" />
                                 <telerik:RadComboBoxItem Value="2" Text="PM" />
                             </Items>
                         </telerik:RadComboBox>
                        </td>
                    </tr>
                    <td width="15%"></td>
                    <td width="15%"></td>
                </table>
            </asp:Panel>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
