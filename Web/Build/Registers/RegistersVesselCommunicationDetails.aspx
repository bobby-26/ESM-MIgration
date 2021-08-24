<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselCommunicationDetails.aspx.cs"
    Inherits="RegistersVesselCommunicationDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%--<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>--%>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Mobile" Src="~/UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Phone" Src="~/UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Common Equipments</title>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmRegisterVesselCommunication" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <%--<ajaxtoolkit:toolkitscriptmanager combinescripts="false" id="ToolkitScriptManager1" runat="server">
    </ajaxtoolkit:toolkitscriptmanager>--%>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" Visible="false" />
        <%--<telerik:RadLabel ID="lblCommunicationDetails" runat="server" Text="Communication Details"></telerik:RadLabel>--%>
        <div style="font-weight: 600;" runat="server">
            <eluc:TabStrip ID="MenuVesselList" runat="server" OnTabStripCommand="MenuVesselList_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        </div>
        <eluc:TabStrip ID="MenuVesselCommunicationDetails" runat="server" OnTabStripCommand="VesselCommunicationDetails_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="8" width="100%">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="width: 10%">
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            </td>
                            <td align="left" style="width: 17%">
                                <telerik:RadTextBox runat="server" ID="txtVesselName" Text="" Width="310px" ReadOnly="true"
                                    CssClass="readonlytextbox">
                                </telerik:RadTextBox>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Panel ID="pnlSatcom" runat="server" GroupingText="Satcom" Width="100%" Height="40%">
                                    <table width="100%">

                                        <tr>
                                            <td style="width: 10%">
                                                <telerik:RadLabel ID="lblSATBPhone" runat="server" Text="SAT B Phone"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 17%">
                                                <telerik:RadTextBox runat="server" ID="txtBPhone" CssClass="input" MaxLength="50" Width="85%"></telerik:RadTextBox>
                                            </td>
                                            <td style="width: 10%">
                                                <telerik:RadLabel ID="lblSATBFax" runat="server" Text="SAT B Fax"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 17%">
                                                <telerik:RadTextBox runat="server" ID="txtBFax" CssClass="input" MaxLength="50" Width="85%"></telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%">
                                                <telerik:RadLabel ID="lblFleet77Phone" runat="server" Text="Fleet77 Phone"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 17%">
                                                <telerik:RadTextBox runat="server" ID="txtFPhone" CssClass="input" MaxLength="50" Width="85%"></telerik:RadTextBox>
                                            </td>
                                            <td style="width: 10%">
                                                <telerik:RadLabel ID="lblFleet77Fax" runat="server" Text="Fleet77 Fax"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 17%">
                                                <telerik:RadTextBox runat="server" ID="txtFFax" CssClass="input" MaxLength="50" Width="85%"></telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%">
                                                <telerik:RadLabel ID="lblFBBPhone" runat="server" Text="FBB Phone"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 17%">
                                                <telerik:RadTextBox runat="server" ID="txtAPhone" CssClass="input" MaxLength="50" Width="85%"></telerik:RadTextBox>
                                            </td>
                                            <td style="width: 10%">
                                                <telerik:RadLabel ID="lblFBBFax" runat="server" Text="FBB Fax"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 17%">
                                                <telerik:RadTextBox runat="server" ID="txtAFax" CssClass="input" MaxLength="50" Width="85%"></telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%">
                                                <telerik:RadLabel ID="lblSATCTelex" runat="server" Text="SAT C Telex"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 17%">

                                                <telerik:RadTextBox runat="server" ID="txtCTalex" CssClass="input" MaxLength="50" Width="85%"></telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlEmail" runat="server" GroupingText="E-mail" Width="100%" Height="40%">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 10%">
                                                <telerik:RadLabel ID="lblEmail" runat="server" Text="E-mail"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 17%">
                                                <telerik:RadTextBox runat="server" ID="txtEmail" MaxLength="100" Width="85%" CssClass="input_mandatory"></telerik:RadTextBox>
                                            </td>
                                            <td style="width: 10%">
                                                <telerik:RadLabel ID="lblNotificationEmail" runat="server" Text="Notification E-mail"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 17%">
                                                <telerik:RadTextBox runat="server" ID="txtNotificationEmail" CssClass="input" MaxLength="50"
                                                    Width="85%">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td style="width: 10%">
                                                <telerik:RadLabel ID="lblAccountAdministratorEmail" runat="server" Text="Account Administrator E-mail"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox runat="server" ID="txtAccAdministratorEmail" CssClass="input" MaxLength="50"
                                                    Width="85%"></telerik:RadTextBox>
                                            </td>
                                            <td style="width: 10%">
                                                <telerik:RadLabel ID="lblFleetManagerEmail" runat="server" Text="Fleet Manager E-mail"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox runat="server" ID="txtFleetManagerEmail" CssClass="input" MaxLength="50"
                                                    Width="85%"></telerik:RadTextBox>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td style="width: 10%">
                                                <telerik:RadLabel ID="lblAccountInchargeEmail" runat="server" Text="Account Incharge E-mail"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox runat="server" ID="txtAccInchargeEmail" CssClass="input" MaxLength="50"
                                                    Width="85%">
                                                </telerik:RadTextBox>
                                            </td>
                                            <td style="width: 10%">
                                                <telerik:RadLabel ID="lblvcgtechemail" runat="server" Text="VCG Tech E-Mail"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <telerik:RadTextBox runat="server" ID="txtvcgtechemail" CssClass="input" MaxLength="50"
                                                    Width="85%">
                                                </telerik:RadTextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Panel ID="pnlPhone" runat="server" GroupingText="Phone" Width="100%" Height="40%">
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 10%">
                                                <telerik:RadLabel ID="lblVSATPhone" runat="server" Text="VSAT Phone"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 17%">
                                                <telerik:RadTextBox runat="server" ID="txtPhone" CssClass="input" MaxLength="50" Width="85%"></telerik:RadTextBox>
                                            </td>
                                            <td style="width: 10%">
                                                <telerik:RadLabel ID="lblVSATFax" runat="server" Text="VSAT Fax"></telerik:RadLabel>
                                            </td>
                                            <td style="width: 17%">
                                                <telerik:RadTextBox runat="server" ID="txtFax" CssClass="input" MaxLength="50" Width="85%"></telerik:RadTextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%">
                                                <telerik:RadLabel ID="lblMobileNumber" runat="server" Text="Mobile Number"></telerik:RadLabel>
                                            </td>
                                            <td>
                                                <eluc:Mobile runat="server" ID="txtMobileNumber" CssClass="input" Width="85%" />
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </div>
    </form>
</body>
</html>
