<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsOrderFormGeneral.aspx.cs"
    Inherits="VesselAccountsOrderFormGeneral" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MapCurrency" Src="~/UserControls/UserControlVesselMappingCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }

        .fon {
            font-size: small !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="OrderForm_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuCrewBond" runat="server" OnTabStripCommand="MenuCrewBond_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%" CssClass="scrolpan">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucstatus" runat="server" Visible="false" />
            <div id="divMain" runat="server" style="width: 99%; padding: 5px 5px 5px 5px;">
                <div id="divsub1" runat="server" style="width: 100%;">
                    <fieldset style="border: none;">
                        <legend style="color: Black; font-weight: bold;">
                            <u>
                                <telerik:RadLabel ID="Label1" runat="server" Text="Order"></telerik:RadLabel>
                            </u>
                        </legend>
                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 8%;">
                                    <telerik:RadLabel ID="lblOrderNo" runat="server" Text="Order No."></telerik:RadLabel>
                                </td>
                                <td style="width: 28%;">
                                    <telerik:RadTextBox ID="txtRefNo" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"
                                        Width="180px">
                                    </telerik:RadTextBox>
                                </td>
                                <td style="width: 8%;">
                                    <telerik:RadLabel ID="lblOrderDate" runat="server" Text="Order On"></telerik:RadLabel>
                                </td>
                                <td style="width: 18%;">
                                    <eluc:Date ID="txtOrderDate" runat="server" CssClass="input_mandatory" />
                                </td>
                                <td style="width: 16%;">
                                    <telerik:RadLabel ID="lblPaymentTerms" runat="server" Text="Payment Terms"></telerik:RadLabel>
                                </td>
                                <td style="width: 20%;">
                                    <asp:RadioButtonList ID="rblPaymentTerm" runat="server" DataTextField="FLDHARDNAME" AutoPostBack="true"
                                        DataValueField="FLDHARDCODE" RepeatDirection="Horizontal" RepeatLayout="Flow" OnSelectedIndexChanged="rblPaymentTerm_OnSelectedIndexChanged">
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblShipChandlerName" runat="server" Text="Ship Chandler"></telerik:RadLabel>
                                </td>
                                <td>
                                    <span id="spnPickListSupplier">
                                        <telerik:RadTextBox ID="txtSupplierCode" runat="server" Width="78px" CssClass="input_mandatory"
                                            Enabled="False">
                                        </telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtSupplierName" runat="server" Width="178px" CssClass="input_mandatory"
                                            Enabled="False">
                                        </telerik:RadTextBox>
                                        <asp:LinkButton runat="server" ID="cmdShowSupplier" OnClientClick="return showPickList('spnPickListSupplier', 'codehelp1', '', '../Common/CommonPickListVesselSupplier.aspx', true);">
                                    <span class="icon"><i class="fas fas fa-list-alt"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtSupplierId" runat="server" Width="1px" CssClass="input"></telerik:RadTextBox>
                                    </span>
                                </td>

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
                                        Width="135px" DecimalPlace="6" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblForStock" runat="server" Text="For Stock"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadCheckBox ID="chkForStock" runat="server" Checked="true"></telerik:RadCheckBox>
                                </td>
                                <td colspan="2">&nbsp;
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblDiscount" runat="server" Text="Discount %"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtDiscount" runat="server" CssClass="input" DefaultZero="false"
                                        Width="135px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">&nbsp;
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblLineItemAmount" runat="server" Text="Total Provision Cost(USD)(A)"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtLineItemAmount" runat="server" CssClass="readonlytextbox" DefaultZero="false"
                                        Width="135px" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div id="divsub2" runat="server" style="width: 100%;">
                    <fieldset style="border: none;">
                        <legend style="color: Black; font-weight: bold;">
                            <u>
                                <telerik:RadLabel ID="lblDeliveryDetails" runat="server" Text="Delivery"></telerik:RadLabel>
                            </u>
                        </legend>
                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 8%;">
                                    <telerik:RadLabel ID="lblExpectedPort" runat="server" Text="Port"></telerik:RadLabel>
                                </td>
                                <td style="width: 28%;">
                                    <eluc:MultiPort ID="ucPort" runat="server" CssClass="input_mandatory" Width="290px" />
                                </td>
                                <td style="width: 8%;">
                                    <telerik:RadLabel ID="lblETA" runat="server" Text="ETA"></telerik:RadLabel>
                                </td>
                                <td style="width: 18%;">
                                    <eluc:Date ID="txtETA" runat="server" CssClass="input_mandatory" />
                                    <telerik:RadMaskedTextBox RenderMode="Lightweight" ID="txtETATime" runat="server" Mask="<0..23>:<0..59>"
                                        Width="50px">
                                    </telerik:RadMaskedTextBox>
                                </td>
                                <td style="width: 16%;">
                                    <telerik:RadLabel ID="lblETB" runat="server" Text="ETB"></telerik:RadLabel>
                                </td>
                                <td style="width: 20%;">
                                    <eluc:Date ID="txtETB" runat="server" CssClass="input" />
                                    <telerik:RadMaskedTextBox RenderMode="Lightweight" ID="txtETBTime" runat="server" Mask="<0..23>:<0..59>"
                                        Width="50px">
                                    </telerik:RadMaskedTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblAgentAddress" runat="server" Text="Port Agent"></telerik:RadLabel>
                                </td>
                                <td>
                                    <span id="spnPickListForwarder">
                                        <telerik:RadTextBox ID="txtForwarderCode" runat="server" Width="60px" CssClass="input" Enabled="false"></telerik:RadTextBox>
                                        <telerik:RadTextBox ID="txtForwarderName" runat="server" BorderWidth="1px" Width="180px"
                                            CssClass="input" Enabled="false">
                                        </telerik:RadTextBox>
                                        <asp:LinkButton runat="server" ID="btnPickForwarder" OnClientClick="return showPickList('spnPickListForwarder', 'codehelp1', '', '../Common/CommonPickListAddress.aspx?addresstype=135', true);">
                                    <span class="icon"><i class="fas fas fa-list-alt"></i></span>
                                        </asp:LinkButton>
                                        <telerik:RadTextBox ID="txtForwarderId" runat="server" Width="1" CssClass="hidden"></telerik:RadTextBox>
                                    </span>
                                    <asp:Image runat="server" ID="cmdForwarderAddress" ImageUrl="<%$ PhoenixTheme:images/supplier-address.png %>"
                                        ToolTip="Address" Style="cursor: pointer; vertical-align: top"></asp:Image>
                                    <asp:ImageButton ID="imgClear2" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                                        ImageAlign="AbsMiddle" Text=".." OnClick="cmdClearAddress_Click" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblETD" runat="server" Text="ETD"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="txtETD" runat="server" CssClass="input" />
                                    <telerik:RadMaskedTextBox RenderMode="Lightweight" ID="txtETDTime" runat="server" Mask="<0..23>:<0..59>"
                                        Width="50px">
                                    </telerik:RadMaskedTextBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbldeliveryCharges" runat="server" Text="Charges"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtDeliverycharges" runat="server" CssClass="input" DefaultZero="false" IsPositive="true"
                                        Width="135px" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4"></td>
                                <td>
                                    <telerik:RadLabel ID="Literal2" runat="server" Text="Discount On Charges (%)"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtChargesDiscount" runat="server" CssClass="input" DefaultZero="false"
                                        Width="135px" />
                                </td>


                            </tr>
                            <tr>
                                <td colspan="4"></td>
                                <td>
                                    <telerik:RadLabel ID="lbldeliverytotal" runat="server" Text="Charges after Discount (USD)(B)"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtdeliverytotal" runat="server" CssClass="readonlytextbox" DefaultZero="false"
                                        Width="135px" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div id="divSub" runat="server" style="width: 100%;">
                    <fieldset style="border: none;">
                        <legend style="border: none; color: Black; font-weight: bold;">
                            <u>
                                <telerik:RadLabel ID="Label2" runat="server" Text="Received & Payment Details"></telerik:RadLabel>
                            </u>
                        </legend>
                        <table width="100%" cellpadding="1" cellspacing="1">
                            <tr>
                                <td style="width: 8%;">
                                    <telerik:RadLabel ID="lblReceivedSeaPort" runat="server" Text="Port"></telerik:RadLabel>
                                </td>
                                <td style="width: 28%;">
                                    <eluc:MultiPort ID="ddlSeaPort" runat="server" CssClass="input" Width="290px" />
                                </td>
                                <td style="width: 8%;">
                                    <telerik:RadLabel ID="lblReceivedDate" runat="server" Text="Date"></telerik:RadLabel>
                                </td>
                                <td style="width: 18%;">
                                    <eluc:Date ID="txtReceivedDate" runat="server" CssClass="input" />
                                </td>
                                <td style="width: 16%;">
                                    <telerik:RadLabel ID="lblgrandtotal" runat="server" Text="Grand Total (USD)(A+B)"></telerik:RadLabel>
                                </td>
                                <td style="width: 20%;">
                                    <eluc:Number ID="txtGrandTotal" runat="server" CssClass="readonlytextbox" DefaultZero="false"
                                        Width="135px" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">&nbsp;
                                </td>
                                <td>
                                    <telerik:RadLabel ID="Literal1" runat="server" Text="Paid Currency"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:MapCurrency ID="ddlPaidcurrency" runat="server" CssClass="input_mandatory"
                                        AutoPostBack="true" Width="90px"
                                        OnTextChangedEvent="ddlPaidcurrency_SelectedIndexChanged" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">&nbsp;
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTotalAmountPaidUSD" runat="server" Text="Total Amount Paid"></telerik:RadLabel>
                                </td>
                                <td>

                                    <eluc:Number ID="txtTotalAmount" runat="server" CssClass="input" DefaultZero="false"
                                        Width="135px" />
                                </td>
                            </tr>

                            <tr>
                                <td colspan="4" style="width: 60%;">&nbsp;
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblRoundoffAmount" runat="server" Text="Round off Amount"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Number ID="txtRoundOff" runat="server" CssClass="readonlytextbox" DefaultZero="false"
                                        Width="135px" ReadOnly="true" />
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>

            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
