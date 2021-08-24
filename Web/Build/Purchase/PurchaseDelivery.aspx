<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseDelivery.aspx.cs"
    Inherits="PurchaseDelivery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="../UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Purchase Delivery</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseDelivery" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuFormGeneral">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuDelivery" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatusMessage" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <eluc:TabStrip ID="MenuDelivery" Title="General" runat="server" OnTabStripCommand="MenuDelivery_TabStripCommand"></eluc:TabStrip>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatusMessage" />

        <telerik:RadAjaxPanel runat="server" ID="pnlFormGeneral">
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblDeliveryNumber" runat="server" Text="Delivery Number"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadLabel runat="server" ID="lblDeliveryId" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel runat="server" ID="lblOrderId" Visible="false"></telerik:RadLabel>
                        <telerik:RadTextBox ID="txtDeliveryNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblDeliveryStatus" runat="server" Text="Delivery Status"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Hard ID="ucStatus" AppendDataBoundItems="true" ShortNameFilter="ACT,PLD,CNC"
                            CssClass="input" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtTitle" CssClass="input_mandatory" MaxLength="500" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency/Amount"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:UserControlCurrency ID="ucCurrency" AppendDataBoundItems="true" CssClass="input"
                            Enabled="true" runat="server" />
                        /
                                    <eluc:Number ID="txtAmount" runat="server" CssClass="input" DecimalPlace="2" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblForwarder" runat="server" Text="Forwarder"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <span id="spnPickListForwarder">
                            <telerik:RadTextBox ID="txtForwarderCode" runat="server" Width="60px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtForwarderName" runat="server" BorderWidth="1px" Width="180px" Enabled="False"
                                CssClass="input">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="btnPickForwarder" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListForwarder', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=141&framename=ifMoreInfo', true);"
                                Text=".." />
                            <telerik:RadTextBox ID="txtForwarderId" runat="server" Width="1" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblForwarderReceivedOn" runat="server" Text="Forwarder Received On"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Date runat="server" ID="txtReceivedForwarder" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblAgent" runat="server" Text="Agent"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtAgentCode" runat="server" Width="60px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAgentName" runat="server" Width="180px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="cmdShowAgent" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=135&framename=ifMoreInfo', true);"
                                Text=".." />
                            <telerik:RadTextBox ID="txtAgentId" runat="server" Width="1px" CssClass="input"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFormNo" runat="server" Text="Order No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFormNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtVesselName" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Multiport ID="ucPortMulti" runat="server" CssClass="input" Width="350px" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblETA" runat="server" Text="ETA"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Date ID="ucETA" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblETB" runat="server" Text="ETB"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Date ID="ucETB" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblDeliveryBy" runat="server" Text="Deliver By"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Date ID="ucDeliveyBy" runat="server" CssClass="input" />
                    </td>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblShipmentMode" runat="server" Text="Shipment Mode"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtShipmentMode" CssClass="input" MaxLength="20"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblNoofPackages" runat="server" Text="No. of Packages"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Number ID="txtNoOfPackages" CssClass="input" runat="server" DecimalPlace="0"
                            MaxLength="4" />
                    </td>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblTotalWeight" runat="server" Text="Total Weight"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Number ID="ucTotalWeight" CssClass="input" runat="server" DecimalPlace="2"
                            MaxLength="7" />
                        Kg
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblOrigin" runat="server" Text="Origin"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtOrigin" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblStorageLocation" runat="server" Text=" Storage Location"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtLocation" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblHAWBHBL" runat="server" Text="HAWB/HBL"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox ID="txtFormNumber" runat="server" CssClass="readonlytextbox" Visible="false"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtHawb" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblIsDGR" runat="server" Text="Is Dangerous Cargo?"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadComboBox runat="server" ID="ddlDGR" CssClass="input">
                            <Items>
                                <telerik:RadComboBoxItem Text="No" Value="0" Selected="True"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="Yes" Value="1"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 20%">
                        <telerik:RadLabel ID="lblShortNote" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtShortNote" CssClass="input" Width="300px" TextMode="MultiLine"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
