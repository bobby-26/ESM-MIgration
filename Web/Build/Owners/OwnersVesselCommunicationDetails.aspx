<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersVesselCommunicationDetails.aspx.cs" Inherits="OwnersVesselCommunicationDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Mobile" Src="~/UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Phone" Src="~/UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Communication Details</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
    
</telerik:RadCodeBlock></head>
<body>
    <form id="frmOwnerVesselCommunication" runat="server">
     <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    
    <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
        position: absolute;">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" Visible="false" />
        <div class="subHeader" style="position: relative">
            <div id="div1" style="vertical-align: top">
                <asp:Literal ID="lblCommunicationDetails" runat="server" Text="Communication Details"></asp:Literal>
            </div>
        </div>
        <table cellpadding="8" width="100%">
            <tr>
                <td>
                    <table width="100%">
                        <tr>
                            <td style="width: 10%">
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td align="left" style="width: 17%">
                                <asp:TextBox runat="server" ID="txtVesselName" Text="" Width="360px" ReadOnly="true"
                                    CssClass="readonlytextbox"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                            <td>
                            </td>
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
                                               <asp:Literal ID="lblSATBPhone" runat="server" Text="SAT B Phone"></asp:Literal>
                                            </td>
                                            <td style="width: 17%">
                                                <asp:TextBox runat="server" ID="txtBPhone" ReadOnly="true" CssClass="input" MaxLength="50" Width="85%"></asp:TextBox>
                                            </td>
                                            <td style="width: 10%">
                                                <asp:Literal ID="lblSATBFax" runat="server" Text="SAT B Fax"></asp:Literal>
                                            </td>
                                            <td style="width: 17%">
                                                <asp:TextBox runat="server" ID="txtBFax" ReadOnly="true" CssClass="input" MaxLength="50" Width="85%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%">
                                               <asp:Literal ID="lblFleet77Phone" runat="server" Text="Fleet77 Phone"></asp:Literal>
                                            </td>
                                            <td style="width: 17%">
                                                <asp:TextBox runat="server" ID="txtFPhone" ReadOnly="true" CssClass="input" MaxLength="50" Width="85%"></asp:TextBox>
                                            </td>
                                            <td style="width: 10%">
                                               <asp:Literal ID="lblFleet77Fax" runat="server" Text="Fleet77 Fax"></asp:Literal>
                                            </td>
                                            <td style="width: 17%">
                                                <asp:TextBox runat="server" ID="txtFFax" ReadOnly="true" CssClass="input" MaxLength="50" Width="85%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%">
                                               <asp:Literal ID="lblFBBPhone" runat="server" Text="FBB Phone"></asp:Literal>
                                            </td>
                                            <td style="width: 17%">
                                                <asp:TextBox runat="server" ID="txtAPhone" ReadOnly="true" CssClass="input" MaxLength="50" Width="85%"></asp:TextBox>
                                            </td>
                                            <td style="width: 10%">
                                              <asp:Literal ID="lblFBBFax" runat="server" Text="FBB Fax"></asp:Literal>
                                            </td>
                                            <td style="width: 17%">
                                                <asp:TextBox runat="server" ID="txtAFax" ReadOnly="true" CssClass="input" MaxLength="50" Width="85%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%">
                                               <asp:Literal ID="lblSATCTelex" runat="server" Text="SAT C Telex"></asp:Literal>
                                            </td>
                                            <td style="width: 17%">
                                             
                                                <asp:TextBox runat="server" ID="txtCTalex" ReadOnly="true" CssClass="input" MaxLength="50" Width="85%"></asp:TextBox>
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
                                                <asp:Literal ID="lblEmail" runat="server" Text="E-mail"></asp:Literal>
                                            </td>
                                            <td style="width: 17%">
                                                <asp:TextBox runat="server" ID="txtEmail" MaxLength="50" Width="85%" ReadOnly="true" CssClass="input_mandatory"></asp:TextBox>
                                            </td>
                                            <td style="width: 10%">
                                                <asp:Literal ID="lblNotificationEmail" runat="server" Text="Notification E-mail"></asp:Literal>
                                            </td>
                                            <td style="width: 17%">
                                                <asp:TextBox runat="server" ID="txtNotificationEmail" ReadOnly="true" CssClass="input" MaxLength="50"
                                                    Width="85%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td style="width: 10%">
                                                <asp:Literal ID="lblAccountAdministratorEmail" runat="server" Text="Account Administrator E-mail"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtAccAdministratorEmail" CssClass="input" MaxLength="50"
                                                    Width="85%"></asp:TextBox>
                                            </td>
                                            <td style="width: 10%">
                                                 <asp:Literal ID="lblFleetManagerEmail" runat="server" Text="Fleet Manager E-mail"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtFleetManagerEmail" CssClass="input" MaxLength="50"
                                                    Width="85%"></asp:TextBox>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td style="width: 10%">
                                                <asp:Literal ID="lblAccountInchargeEmail" runat="server" Text="Account Incharge E-mail"></asp:Literal>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtAccInchargeEmail" ReadOnly="true" CssClass="input" MaxLength="50"
                                                    Width="85%"></asp:TextBox>
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
                                               <asp:Literal ID="lblVSATPhone" runat="server" Text="VSAT Phone"></asp:Literal>
                                            </td>
                                            <td style="width: 17%">
                                                <asp:TextBox runat="server" ID="txtPhone" ReadOnly="true" CssClass="input" MaxLength="50" Width="85%"></asp:TextBox>
                                            </td>
                                            <td style="width: 10%">
                                               <asp:Literal ID="lblVSATFax" runat="server" Text="VSAT Fax"></asp:Literal>
                                            </td>
                                            <td style="width: 17%">
                                                <asp:TextBox runat="server" ID="txtFax" ReadOnly="true" CssClass="input" MaxLength="50" Width="85%"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 10%">
                                                <asp:Literal ID="lblMobileNumber" runat="server" Text="Mobile Number"></asp:Literal>
                                            </td>
                                            <td>
                                                <eluc:Mobile runat="server" ID="txtMobileNumber" ReadOnly="true" CssClass="input" Width="85%" />
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
