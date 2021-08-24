<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersAddressCorrection.aspx.cs" Inherits="RegistersAddressCorrection" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="~/UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MobileNumber" Src="~/UserControls/UserControlMobileNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Address Correction</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatusMessage" />

        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuAddressMain" runat="server" OnTabStripCommand="AddressMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        </div>
        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuOfficeMain" runat="server" OnTabStripCommand="OfficeMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        </div>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <table align="left" width="100%">
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="lblName:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtName" ReadOnly="true" CssClass="readonlytextbox" Width="90%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblContactPerson1" runat="server" Text="Contact Person 1:"></telerik:RadLabel>
                    </td>
                    <td width="80%">
                        <telerik:RadTextBox ID="txtAttention" runat="server" Width="90%" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Country runat="server" ID="ucCountry" AppendDataBoundItems="true"
                            CssClass="readonlytextbox" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInCharge" runat="server" Text="In-Charge:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInCharge" runat="server" Width="90%" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblState" runat="server" Text="State:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:State ID="ucState" Enabled="false" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAddress1" runat="server" Text="Address 1:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAddress1" runat="server" Width="90%" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCity" runat="server" Text="City:"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:City ID="ddlCity" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAddress2" runat="server" Text="Address 2:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAddress2" runat="server" Width="90%" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPostalCode" runat="server" Text="Postal Code:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPostalCode" runat="server" Width="35%" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAddress3" runat="server" Text="Address 3:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAddress3" runat="server" Width="90%" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lblAddress4" runat="server" Text="Address 4:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAddress4" runat="server" ReadOnly="true" CssClass="readonlytextbox" Width="90%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td width="10%">
                        <telerik:RadLabel ID="lblPhone1" runat="server" Text="Phone 1:"></telerik:RadLabel>
                    </td>
                    <td width="40%">
                        <eluc:PhoneNumber ID="txtPhone1" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            IsMobileNumber="true" />
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lblPhone2" runat="server" Text="Phone 2:"></telerik:RadLabel>
                    </td>
                    <td width="40%">
                        <eluc:PhoneNumber ID="txtPhone2" runat="server" ReadOnly="true" CssClass="readonlytextbox" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFax1" runat="server" Text="Fax 1:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFax1" runat="server" Width="80%" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        <eluc:MaskNumber runat="server" ID="maskedittxtFax1" CssClass="input" />
                    </td>
                    <td>
                        <asp:Literal ID="lblFax2" runat="server" Text="Fax 2:"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFax2" runat="server" Width="80%" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        <eluc:MaskNumber runat="server" ID="maskedittxtFax2" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmail1" runat="server" Text="Email 1:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmail1" runat="server" Width="80%" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmail2" runat="server" Text="Email 2:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmail2" runat="server" Width="80%" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAOHTelephone" runat="server" Text="(AOH) Telephone:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtaohTelephoneno" runat="server" Width="80%" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        <eluc:MaskNumber runat="server" ID="maskedittxtaohTelephoneno" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAOHMobile" runat="server" Text="(AOH) Mobile:"></telerik:RadLabel> 
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtaohMobileno" runat="server" Width="80%" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                      <eluc:MaskNumber runat="server" ID="maskedittxtaohMobileno" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWebSite" runat="server" Text="Web Site:"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtURL" runat="server" Width="80%" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
            <br clear="all" />
            <br />
              <telerik:RadPanelBar RenderMode="Lightweight" ID="MyAccordion" runat="server" Width="100%">
                <Items>
                    <telerik:RadPanelItem Text="Product Type" Width="100%">
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblSelecttheProductServicesyouoffer" runat="server" Text="Select the Product/Services you offer"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:CheckBoxList runat="server" ID="cblProduct" Height="26px" RepeatColumns="7"
                                RepeatDirection="Horizontal" RepeatLayout="Table">
                            </asp:CheckBoxList> 
                        </ContentTemplate>
                    </telerik:RadPanelItem>
                </Items>
            </telerik:RadPanelBar>
            <br />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
