<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaAddressCorrection.aspx.cs"
    Inherits="PreSeaAddressCorrection" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="~/UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MobileNumber" Src="~/UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Address</title>
   <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status runat="server" ID="ucStatusMessage" />
    <div class="subHeader" style="position: relative">
        <div id="div1" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Address Correction"
                Width="360px"></asp:Label>
        </div>
    </div>
    <div runat="server" id="divSubHeader" class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="Label1" Font-Bold="true" Text="" Width="360px"></asp:Label>
        </div>
    </div>
    <div class="navSelectHeader" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuAddressMain" runat="server" TabStrip="true" OnTabStripCommand="AddressMain_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <div class="navSelect" style="top: 28px; right: 0px; position: absolute;">
        <eluc:TabStrip ID="MenuOfficeMain" runat="server" OnTabStripCommand="OfficeMain_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <asp:UpdatePanel ID="pnlAddressEntry" runat="server">
        <ContentTemplate>
            <table align="left" width="100%">
                <tr valign="top">
                    <td>
                        Name:
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtName" ReadOnly="true" CssClass="readonlytextbox"
                            Width="90%"></asp:TextBox>
                    </td>
                    <td>
                        Contact Person 1:
                    </td>
                    <td width="80%">
                        <asp:TextBox ID="txtAttention" runat="server" Width="90%" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Country:
                    </td>
                    <td>
                        <eluc:Country runat="server" ID="ucCountry" AppendDataBoundItems="true" CssClass="readonlytextbox"
                            Enabled="false" />
                    </td>
                    <td>
                        In-Charge:
                    </td>
                    <td>
                        <asp:TextBox ID="txtInCharge" runat="server" Width="90%" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        State:
                    </td>
                    <td>
                        <eluc:State ID="ucState" Enabled="false" runat="server" AppendDataBoundItems="true"
                            CssClass="readonlytextbox" />
                    </td>
                    <td>
                        Address 1:
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress1" runat="server" Width="90%" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        City:
                    </td>
                    <td>
                        <eluc:City ID="ddlCity" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                            Enabled="false" />
                    </td>
                    <td>
                        Address 2:
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress2" runat="server" Width="90%" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        Postal Code:
                    </td>
                    <td>
                        <asp:TextBox ID="txtPostalCode" runat="server" Width="35%" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                    </td>
                    <td>
                        Address 3:
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress3" runat="server" Width="90%" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                        Address 4:
                    </td>
                    <td>
                        <asp:TextBox ID="txtAddress4" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td width="10%">
                        Phone 1:
                    </td>
                    <td width="40%">
                        <eluc:PhoneNumber ID="txtPhone1" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            IsMobileNumber="true" />
                    </td>
                    <td width="10%">
                        Phone 2:
                    </td>
                    <td width="40%">
                        <eluc:PhoneNumber ID="txtPhone2" runat="server" ReadOnly="true" CssClass="readonlytextbox" />
                    </td>
                </tr>
                <tr>
                    <td>
                        Fax 1:
                    </td>
                    <td>
                        <asp:TextBox ID="txtFax1" runat="server" Width="80%" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="maskedittxtFax1" runat="server" TargetControlID="txtFax1"
                            OnInvalidCssClass="MaskedEditError" Mask="9999999999" MaskType="Number" InputDirection="LeftToRight"
                            AutoComplete="false">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                    <td>
                        Fax 2:
                    </td>
                    <td>
                        <asp:TextBox ID="txtFax2" runat="server" Width="80%" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="maskedittxtFax2" runat="server" TargetControlID="txtFax2"
                            OnInvalidCssClass="MaskedEditError" Mask="9999999999" MaskType="Number" InputDirection="LeftToRight"
                            AutoComplete="false">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        Email 1:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail1" runat="server" Width="80%" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                    </td>
                    <td>
                        Email 2:
                    </td>
                    <td>
                        <asp:TextBox ID="txtEmail2" runat="server" Width="80%" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        (AOH) Telephone:
                    </td>
                    <td>
                        <asp:TextBox ID="txtaohTelephoneno" runat="server" Width="80%" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="maskedittxtaohTelephoneno" runat="server" TargetControlID="txtaohTelephoneno"
                            OnInvalidCssClass="MaskedEditError" Mask="99999999999" MaskType="Number" InputDirection="LeftToRight"
                            AutoComplete="false">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                    <td>
                        (AOH) Mobile:
                    </td>
                    <td>
                        <asp:TextBox ID="txtaohMobileno" runat="server" Width="80%" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="maskedittxtaohMobileno" runat="server" TargetControlID="txtaohMobileno"
                            OnInvalidCssClass="MaskedEditError" Mask="99999999999" MaskType="Number" InputDirection="LeftToRight"
                            AutoComplete="false">
                        </ajaxToolkit:MaskedEditExtender>
                    </td>
                </tr>
                <tr>
                    <td>
                        Web Site:
                    </td>
                    <td>
                        <asp:TextBox ID="txtURL" runat="server" Width="80%" ReadOnly="true" CssClass="readonlytextbox"></asp:TextBox>
                    </td>
                    <td>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <br clear="all" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
