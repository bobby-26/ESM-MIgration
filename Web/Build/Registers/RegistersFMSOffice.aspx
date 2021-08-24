<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersFMSOffice.aspx.cs"
    Inherits="RegistersFMSOffice" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Bank" Src="~/UserControls/UserControlBank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Owner" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="RFQPreference" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Clinic" Src="~/UserControls/UserControlClinic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Address</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
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
        <eluc:Status runat="server" ID="ucStatus" />

        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuAddressMain" runat="server" OnTabStripCommand="AddressMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        </div>
        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuOfficeMain" runat="server" OnTabStripCommand="OfficeMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        </div>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">

            <eluc:Address runat="server" ID="ucAddress" EnableAOH="true"></eluc:Address>
            <br clear="all" />
            <table>
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblBusinessProfile" runat="server" Text="Business Profile"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtBusinessProfile" CssClass="input" Width="350px"
                            Height="100px" TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                    <td width="15%">
                        <telerik:RadLabel ID="lblFeedBackAboutVendor" runat="server" Text="FeedBack About Vendor"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFeedback" CssClass="input" Width="350px" Height="100px"
                            TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <telerik:RadPanelBar RenderMode="Lightweight" ID="MyAccordion" runat="server" Width="100%">
                <Items>
                    <telerik:RadPanelItem Text="Department" Width="100%">
                        <HeaderTemplate>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Department"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <telerik:RadCheckBoxList RenderMode="Lightweight" runat="server" ID="cblAddressDepartment" Height="90%" Columns="5"
                                Direction="Vertical" Layout="Flow" AutoPostBack="false">
                            </telerik:RadCheckBoxList>
                        </ContentTemplate>
                    </telerik:RadPanelItem>
                </Items>
            </telerik:RadPanelBar>
            <telerik:RadPanelBar RenderMode="Lightweight" ID="RadPanelBar1" runat="server" Width="100%">
                <Items>
                    <telerik:RadPanelItem Text="Configuration" Width="100%">
                        <HeaderTemplate>
                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Configuration"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td colspan="4">
                                        <b>
                                            <telerik:RadLabel ID="lblsupConfig" runat="server" Text="Supplier"></telerik:RadLabel>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblIfSupplierselectRFQPreference" runat="server" Text="RFQ Preference"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:RFQPreference ID="ucRFQPreference" runat="server" CssClass="input" HardTypeCode="75"
                                            AppendDataBoundItems="true" Width="200px" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblNoofCreditDays" runat="server" Text="No of Credit Days"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtNoOfCreditDays" runat="server" CssClass="input"></telerik:RadTextBox>
                                        <eluc:MaskNumber runat="server" ID="MaskedEditNoofcreditdays" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblDefaultDiscount" runat="server" Text="Default Discount %"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtDefDiscount" runat="server" CssClass="input"></telerik:RadTextBox>
                                        <eluc:MaskNumber runat="server" ID="MaskedEditExtender1" CssClass="input" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblDiscountEffectiveDate" runat="server" Text="Discount Effective Date"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date runat="server" ID="txtEffDate" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblACAsstInCharge" runat="server" Text="A/C Asst. In-Charge"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:User ID="ucACAsstIncharge" runat="server" AppendDataBoundItems="true" Width="200px"
                                            CssClass="input" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblACManagerInCharge" runat="server" Text="A/C Manager In-Charge"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:User ID="ucACManagerInCharge" runat="server" AppendDataBoundItems="true" Width="200px"
                                            CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblGSTApplicable" runat="server" Text="GST Applicable"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkGSTApplicable" runat="server" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblDelayedUtilizationApplicable" runat="server" Text="Delayed Utilization Applicable"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkDelayedUtilizationApplicable" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPrioritySupplier" runat="server" Text="Priority Supplier"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <asp:CheckBox runat="server" ID="chkPrioritySupplier" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblDefaultBankCharges" runat="server" Text="Default Bank Charges"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Hard ID="ddlBankChargebasis" runat="server" CssClass="input" HardTypeCode="133"
                                            HardList='<%# PhoenixRegistersHard.ListHard(1, 133) %>' AppendDataBoundItems="true"
                                            Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblCreditNoteEffectiveDate" runat="server" Text="Credit Note Discount Effective"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date runat="server" ID="txtEffectiveDate" CssClass="input" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblPaymentonReceiveofInvoice" runat="server" Text="Payment on Receive of Invoice"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkPaymentOnInvReceived" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblCreditNoteDisc" runat="server" Text="Default Credit Note Discount %"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtDefaultDiscount" runat="server" CssClass="input"></telerik:RadTextBox>
                                        <eluc:MaskNumber runat="server" ID="mskDefaultDiscount" CssClass="input" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblNotReceivingInvoicesfromSupplier" runat="server" Text="Not Receiving Invoices from Supplier"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkReceiveFromSupplier" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <hr />
                                        <b>
                                            <telerik:RadLabel ID="lblunionConfig" runat="server" Text="Union"></telerik:RadLabel>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblIfUnionSelectTimeLimitForMedicalCase" runat="server" Text="Time Limit For Medical Case"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtTimelimit" runat="server" CssClass="input" IsInteger="true" IsPositive="true" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblunionwageexp" runat="server" Text="Contract Wage Experience (in Years)"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkWageExp" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr />
                                        <b>
                                            <telerik:RadLabel ID="lblRegistered" runat="server" Text="Registered Owner"></telerik:RadLabel>
                                        </b>
                                    </td>
                                    <td colspan="2">
                                        <hr />
                                        <b>
                                            <telerik:RadLabel ID="lblClinic" runat="server" Text="Clinic"></telerik:RadLabel>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblIfRegisteredOwnerSelectPrincipal" runat="server" Text="Principal"></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblOwner" runat="server" Visible="false"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Owner runat="server" ID="ucPrincipal" Width="200px" CssClass="input" AddressType="128"
                                            AppendDataBoundItems="true" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblIfClinicselectZone" runat="server" Text="Zone"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Zone ID="ddlZone" runat="server" AppendDataBoundItems="true" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <hr />
                                        <b>
                                            <telerik:RadLabel ID="lblConsulate" runat="server" Text="Consulate"></telerik:RadLabel>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblIfConsulateselectFlag" runat="server" Text="Flag"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Flag ID="ucFlag" runat="server" CssClass="input" Width="200px" AppendDataBoundItems="true" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblReceivingInvoicesFromConsulate" runat="server" Text="Receiving Invoice"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkreceiveinvoice" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblDepositAmount" runat="server" Text="Deposit Amount"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Currency ID="ucDepositCurrency" runat="server" AppendDataBoundItems="true"
                                            CssClass="input" />
                                        <eluc:Number ID="ucDepositAmount" runat="server" CssClass="input" DefaultZero="false"
                                            DecimalPlace="2" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <hr />
                                        <b>
                                            <telerik:RadLabel ID="lblYardConfig" runat="server" Text="Yard"></telerik:RadLabel>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Multiport ID="ucPortMulti" runat="server" CssClass="input" Width="300px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <hr />
                                        <b>
                                            <telerik:RadLabel ID="lblManager" runat="server" Text="Manager" Visible="false"></telerik:RadLabel>
                                        </b>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblImoNumber" runat="server" Text="IMO No." Visible="false"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtIMONo" runat="server" CssClass="input" Width="100px" Visible="false"></telerik:RadTextBox>
                                    </td>
                                </tr>

                            </table>
                        </ContentTemplate>
                    </telerik:RadPanelItem>
                </Items>
            </telerik:RadPanelBar>
            <telerik:RadPanelBar RenderMode="Lightweight" ID="AccordionPane1" runat="server" Width="100%">
                <Items>
                    <telerik:RadPanelItem Text="Address Type" Width="100%">
                        <HeaderTemplate>
                            <telerik:RadLabel ID="RadLabel3" runat="server" Text="Address Type"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <telerik:RadCheckBoxList RenderMode="Lightweight" runat="server" ID="cblAddressType" Height="90%" Columns="5"
                                Direction="Vertical" Layout="Flow" AutoPostBack="false">
                            </telerik:RadCheckBoxList>
                        </ContentTemplate>
                    </telerik:RadPanelItem>
                </Items>
            </telerik:RadPanelBar>


            <%--<ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                <Header>
                    <a href="" class="accordionLink">Product/Services</a>
                </Header>
                <Content>
                    <asp:CheckBoxList runat="server" ID="cblProduct" Height="26px" RepeatColumns="5"
                        RepeatDirection="Horizontal" RepeatLayout="Table">
                    </asp:CheckBoxList>
                </Content>
            </ajaxToolkit:AccordionPane>--%>
            <telerik:RadPanelBar RenderMode="Lightweight" ID="AccordionPane2" runat="server" Width="100%">
                <Items>
                    <telerik:RadPanelItem Text="Product/Services" Width="100%">
                        <HeaderTemplate>
                            <telerik:RadLabel ID="RadLabel4" runat="server" Text="Product/Services"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <telerik:RadCheckBoxList RenderMode="Lightweight" runat="server" ID="cblProduct" Height="90%" Columns="3"
                                Direction="Vertical" Layout="Flow" AutoPostBack="false">
                            </telerik:RadCheckBoxList>
                        </ContentTemplate>
                    </telerik:RadPanelItem>
                </Items>
            </telerik:RadPanelBar>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
