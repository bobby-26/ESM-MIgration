<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsCTMGeneral.aspx.cs"
    Inherits="VesselAccountsCTMGeneralNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHardExtn.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiVesselSupplier" Src="~/UserControls/UserControlMultiColumnVesselSupplier.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlVesselMappingCurrency.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cash Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="form1" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuCTMMain" runat="server" OnTabStripCommand="MenuCTMMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuCTM" runat="server" OnTabStripCommand="MenuCTM_TabStripCommand"></eluc:TabStrip>
            <div id="divMain" runat="server" style="width: 99%; padding: 5px 5px 5px 5px;">
                <fieldset>
                    <legend>
                        <telerik:RadLabel ID="lblhead" runat="server" Text='<%#SouthNests.Phoenix.Framework.PhoenixSecurityContext.CurrentSecurityContext.VesselName %>'></telerik:RadLabel>
                    </legend>
                    <div id="div1" runat="server" style="width: 100%;">
                        <fieldset style="border: none;">
                            <legend style="color: Black; font-weight: bold;">
                                <telerik:RadLabel ID="Label1" runat="server" Text=''></telerik:RadLabel>
                            </legend>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>

                                    <td style="width: 15%;">
                                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Purpose"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 25%;">
                                        <eluc:Hard ID="ucPurposeHard" runat="server" HardTypeCode="268" Width="135px" AppendDataBoundItems="true"
                                            AutoPostBack="true" CssClass="input_mandatory" SortByShortName="false" />
                                    </td>
                                    <td style="width: 15%;">
                                        <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 45%;">
                                        <eluc:MultiPort ID="ddlPort" runat="server" CssClass="input_mandatory" Width="330px" />
                                    </td>
                                </tr>
                                <tr>

                                    <td style="width: 15%;">
                                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 25%;">
                                        <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblPortAgent" runat="server" Text="Port Agent"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:MultiVesselSupplier ID="ucVesselSupplier" runat="server" CssClass="input_mandatory" Width="330px" />
                                        <%-- <span id="spnPickListSupplier">
                                            <telerik:RadTextBox ID="txtSupplierCode" runat="server" Width="90px" CssClass="input_mandatory"
                                                Enabled="False">
                                            </telerik:RadTextBox>
                                            <telerik:RadTextBox ID="txtSupplierName" runat="server" Width="220px" CssClass="input_mandatory"
                                                Enabled="False">
                                            </telerik:RadTextBox>
                                            <asp:ImageButton runat="server" ID="cmdShowSupplier" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListSupplier', 'codehelp1', '', '../Common/CommonPickListVesselSupplier.aspx', true);"
                                                Text=".." />
                                            <telerik:RadTextBox ID="txtSupplierId" runat="server" Width="1px" CssClass="input"></telerik:RadTextBox>
                                        </span>--%>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblETA" runat="server" Text="ETA"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtETA" runat="server" CssClass="input_mandatory" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblETD" runat="server" Text="ETD"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date ID="txtETD" runat="server" CssClass="input_mandatory" />
                                    </td>

                                </tr>
                                <tr>

                                    <td>
                                        <telerik:RadLabel ID="lblcurrency" runat="server" Text="Requested Currency"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Currency ID="ddlCurrency" runat="server" CssClass="input_mandatory"
                                            AutoPostBack="true" Width="135px"
                                            OnTextChangedEvent="ddlCurrency_SelectedIndexChanged" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblExchageRate" runat="server" Text="ExChange Rate"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtExchangeRate" runat="server" DecimalPlace="17" MaxLength="25" Width="135px" Text="" CssClass="readonlytextbox input" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>

                                    <td>
                                        <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAmount" CssClass="readonlytextbox input" ReadOnly="true" runat="server"
                                            Width="135px" MaxLength="8" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblArrangedAmount" runat="server" Text="Arranged Amount"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtAmountArranged" CssClass="input" runat="server" Width="90px"
                                            MaxLength="8" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblOfficeRemarks" runat="server" Text="Office Remarks"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" TextMode="MultiLine"
                                            Width="250px" Height="35px">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div id="divSub" runat="server" style="width: 100%;">
                        <fieldset style="border: none;">
                            <legend style="color: Black; font-weight: bold;">
                                <telerik:RadLabel ID="lblhead1" runat="server" Text='Received'></telerik:RadLabel>
                            </legend>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td style="width: 15%;">
                                        <telerik:RadLabel ID="lblReceivedAmount" runat="server" Text="Amount"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 25%;">
                                        <eluc:Number ID="txtReceivedAmount" runat="server" CssClass="input" Width="135px"
                                            MaxLength="8" />
                                    </td>
                                    <td style="width: 15%;">
                                        <telerik:RadLabel ID="lblReceivedDate" runat="server" Text="Date"></telerik:RadLabel>
                                    </td>
                                    <td style="width: 45%;">
                                        <eluc:Date ID="txtReceivedDate" runat="server" CssClass="input" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </fieldset>
            </div>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
