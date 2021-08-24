<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSundryPurchaseGeneral.aspx.cs" Inherits="AccountsSundryPurchaseGeneral" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sundry Purchase General</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">


            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <eluc:TabStrip ID="MenuCrewBond" runat="server" OnTabStripCommand="MenuCrewBond_TabStripCommand"></eluc:TabStrip>

            <div id="divMain" runat="server" style="width: 100%;">
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVesselOffice" runat="server" Text="Vessel/Office"></telerik:RadLabel>
                        </td>
                        <td colspan="5">
                            <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input_mandatory"
                                AssignedVessels="true" VesselsOnly="true" Enabled="True" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblOrderNo" runat="server" Text="Order No"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtRefNo" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOrderDate" runat="server" Text="Order Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtOrderDate" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPaymentTerms" runat="server" Text="Payment Terms"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <asp:RadioButtonList ID="rblPaymentTerm" runat="server" DataTextField="FLDHARDNAME"
                                DataValueField="FLDHARDCODE" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblStockType" runat="server" Text="Stock Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ddlStock" runat="server" AppendDataBoundItems="true" HardTypeCode="97"
                                ShortNameFilter="HOT,STA" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <span id="spnPickListSupplier">
                                <telerik:RadTextBox ID="txtSupplierCode" runat="server" Width="60px" CssClass="input_mandatory"
                                    Enabled="False">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtSupplierName" runat="server" Width="180px" CssClass="input_mandatory"
                                    Enabled="False">
                                </telerik:RadTextBox>
                                <asp:ImageButton runat="server" ID="cmdShowSupplier" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                    ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickListSupplier', 'codehelp1', '', 'Common/CommonPickListVesselSupplier.aspx', true);"
                                    Text=".." />
                                <telerik:RadTextBox ID="txtSupplierId" runat="server" Width="1px" CssClass="input"></telerik:RadTextBox>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Currency ID="ddlCurrency" AppendDataBoundItems="true" CssClass="input" runat="server"
                                ActiveCurrency="false" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblExchangeRate1USD" runat="server" Text="Exchange Rate (1 USD = )"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtExchangeRate" runat="server" CssClass="input" DefaultZero="false"
                                Width="90px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblTotalAmountPaidUSD" runat="server" Text="Total Amount Paid (USD)"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtTotalAmount" runat="server" CssClass="input" DefaultZero="false"
                                Width="90px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>

                        <td>
                            <telerik:RadLabel ID="lblDiscount" runat="server" Text="Discount %"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtDiscount" runat="server" CssClass="input" DefaultZero="false"
                                Width="90px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">&nbsp;
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRoundoffAmount" runat="server" Text="Round off Amount"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtRoundOff" runat="server" CssClass="readonlytextbox" DefaultZero="false"
                                Width="90px" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divSub" runat="server" style="width: 100%;">
                <table width="50%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblReceivedSeaPort" runat="server" Text="Received Sea Port"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:SeaPort runat="server" ID="ddlSeaPort" CssClass="input_mandatory" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblReceivedDate" runat="server" Text="Received Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtReceivedDate" runat="server" CssClass="input_mandatory" />
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

