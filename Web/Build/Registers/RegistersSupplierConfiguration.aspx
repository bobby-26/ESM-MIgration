<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersSupplierConfiguration.aspx.cs"
    Inherits="Registers_RegistersSupplierConfiguration" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
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
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Address</title>
    <telerik:RadCodeBlock runat="server" ID="dvlink">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuSupplierRegister" runat="server" OnTabStripCommand="MenuSupplierRegister_TabStripCommand"></eluc:TabStrip>
            <eluc:Address runat="server" ID="ucAddress" EnableAOH="true" Visible="false"></eluc:Address>
            <br clear="all" />
            <table width="100%">
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblBusinessProfile" runat="server" Text="Business Profile" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtBusinessProfile" CssClass="input" Width="350px" Height="100px" Visible="false"
                            TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                    <td width="15%">
                        <telerik:RadLabel ID="lblFeedBackAboutVendor" runat="server" Text="FeedBack About Vendor" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFeedback" CssClass="input" Width="350px" Height="100px"
                            TextMode="MultiLine" Visible="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <content>
                            <table width="100%" cellpadding="1" cellspacing="1">
                                <tr>
                                    <td>
                                        <b><telerik:RadLabel ID="lblIfSupplierselectRFQPreference" runat="server" Text="If Supplier, select RFQ Preference"></telerik:RadLabel></b>
                                    </td>
                                    <td>
                                        <eluc:RFQPreference ID="ucRFQPreference" runat="server" CssClass="input" HardTypeCode="75"
                                            AppendDataBoundItems="true" />
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblNoofCreditDays" runat="server" Text="No of Credit Days"></telerik:RadLabel>
                                    </td>
                                    <td>
                                         <eluc:Number ID="txtNoOfCreditDays" runat="server"  Width="120px"></eluc:Number>
                                       <%-- <asp:TextBox ID="txtNoOfCreditDays" runat="server" CssClass="input"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditNoofcreditdays" runat="server" AutoComplete="true"
                                            InputDirection="RightToLeft" Mask="999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                            TargetControlID="txtNoOfCreditDays" />--%>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblDefaultDiscount" runat="server" Text="Default Discount %"></telerik:RadLabel>
                                    </td>
                                    <td>
                                          <eluc:Number ID="txtDefDiscount" runat="server"  Width="120px"></eluc:Number>
                                       <%-- <asp:TextBox ID="txtDefDiscount" runat="server" CssClass="input"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender1" runat="server" AutoComplete="true"
                                            InputDirection="RightToLeft" Mask="999.9999999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                            TargetControlID="txtDefDiscount" />--%>
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
                                        <eluc:User ID="ucACAsstIncharge" runat="server" AppendDataBoundItems="true" Width="200px" CssClass="input" />                                        
                                    </td>
                                    <td>
                                        <telerik:RadLabel ID="lblACManagerInCharge" runat="server" Text="A/C Manager In-Charge"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:User ID="ucACManagerInCharge" runat="server" AppendDataBoundItems="true" Width="200px" CssClass="input" />  
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
                                        <telerik:RadLabel ID="lblDefaultBankCharges" runat="server" Text="Default Bank Charges" Visible="false"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Hard ID="ddlBankChargebasis" runat="server" CssClass="input" HardTypeCode="133"
                                            HardList='<%# PhoenixRegistersHard.ListHard(1, 133) %>' AppendDataBoundItems="true"
                                            Width="300px" Visible="false"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b><telerik:RadLabel ID="lblIfRegisteredOwnerSelectPrincipal" runat="server" Text="If Registered Owner, Select Principal"></telerik:RadLabel></b>
                                        <asp:Label ID="lblOwner" runat="server" Visible="false"></asp:Label>
                                    </td>
                                    <td>
                                        <eluc:Owner runat="server" ID="ucPrincipal" CssClass="input" AddressType="128" AppendDataBoundItems="true" />
                                    </td>
                                    <td>
                                        <b><telerik:RadLabel ID="lblIfUnionSelectTimeLimitForMedicalCase" runat="server" Text="If Union, Select Time Limit For Medical Case"></telerik:RadLabel></b>
                                        
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtTimelimit" runat="server" CssClass="input" IsInteger="true" IsPositive="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b><telerik:RadLabel ID="lblIfConsulateselectFlag" runat="server" Text="If Consulate, select Flag"></telerik:RadLabel></b>
                                    </td>
                                    <td>
                                        <eluc:Flag ID="ucFlag" runat="server" CssClass="input" AppendDataBoundItems="true" />
                                    </td>
                                    <td>
                                        <b><telerik:RadLabel ID="lblIfClinicselectZone" runat="server" Text="If Clinic, select Zone"></telerik:RadLabel></b>
                                    </td>
                                    <td >
                                        <eluc:Zone ID="ddlZone" runat="server" AppendDataBoundItems="true" CssClass="input" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b><telerik:RadLabel ID="lblReceivingInvoicesFromConsulate" runat="server" Text="Receiving Invoices From Consulate"></telerik:RadLabel></b>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkreceiveinvoice" runat="server" />
                                    </td>
                                    <td>
                                        <b><telerik:RadLabel ID="lblPaymentonReceiveofInvoice" runat="server" Text="Payment on Receive of Invoice"></telerik:RadLabel></b>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkPaymentOnInvReceived" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <b><telerik:RadLabel ID="lblDepositAmount" runat="server" Text="Deposit Amount"></telerik:RadLabel></b>
                                    </td>
                                    <td>
                                        <eluc:Currency ID="ucDepositCurrency" runat="server" AppendDataBoundItems="true" CssClass="input" />
                                        <eluc:Number ID="ucDepositAmount" runat="server" CssClass="input" DefaultZero="false" DecimalPlace="2" />
                                    </td>
                                    <td>
                                        <b><telerik:RadLabel ID="lblNotReceivingInvoicesfromSupplier" runat="server" Text="Not Receiving Invoices from Supplier"></telerik:RadLabel></b>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkReceiveFromSupplier" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <%--Default Credit Note Discount %--%>
                                        <telerik:RadLabel ID="lblCreditNoteDisc" runat="server" Text="Default Credit Note Discount %"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Number ID="txtDefaultDiscount" runat="server"  Width="120px"></eluc:Number>

                                     <%--   <asp:TextBox ID="txtDefaultDiscount" runat="server" CssClass="input"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="mskDefaultDiscount" runat="server" AutoComplete="true"
                                            InputDirection="RightToLeft" Mask="999.9999999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                            TargetControlID="txtDefaultDiscount" />--%>
                                    </td>
                                    <td>
                                        <%--Credit Note Discount Effective Date--%>
                                        <telerik:RadLabel ID="lblCreditNoteEffectiveDate" runat="server" Text="Credit Note Discount Effective Date"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Date runat="server" ID="txtEffectiveDate" CssClass="input" />
                                         <telerik:RadLabel ID="txtportid" runat="server" visible="false"></telerik:RadLabel>

                                    </td>
                                </tr>
                                <tr>
                                     <td>
                                        <telerik:RadLabel ID="lblduedays" runat="server" Text="Rebate Due Days"></telerik:RadLabel>
                                    </td>
                                    <td colspan="3">
                                         <eluc:Number ID="txtduedays" runat="server"  Width="120px"></eluc:Number>
                                                                          </td>
                                </tr>
                            </table>
                        </content>
            <ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                FadeTransitions="false" FramesPerSecond="40" TransitionDuration="50" AutoSize="None"
                RequireOpenedPane="false" SuppressHeaderPostbacks="true">
                <Panes>
                    <ajaxToolkit:AccordionPane ID="AccordionPane4" runat="server" Visible="false">
                        <Header>
                            <a href="" class="accordionLink">Department</a>
                        </Header>
                        <Content>
                            <asp:CheckBoxList runat="server" ID="cblAddressDepartment" Height="26px" RepeatColumns="7"
                                RepeatDirection="Horizontal" RepeatLayout="Table">
                            </asp:CheckBoxList>
                        </Content>
                    </ajaxToolkit:AccordionPane>
                    <ajaxToolkit:AccordionPane ID="AccordionPane3" runat="server" Visible="false">
                        <Header>
                            <a href="" class="accordionLink">Supplier Configuration</a>
                        </Header>

                    </ajaxToolkit:AccordionPane>
                    <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server" Visible="false">
                        <Header>
                            <a href="" class="accordionLink">Address Type</a>
                        </Header>
                        <Content>
                            <asp:CheckBoxList runat="server" ID="cblAddressType" Height="26px" RepeatColumns="5"
                                RepeatDirection="Horizontal" RepeatLayout="Table">
                            </asp:CheckBoxList>
                        </Content>
                    </ajaxToolkit:AccordionPane>
                    <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server" Visible="false">
                        <Header>
                            <a href="" class="accordionLink">Product/Services</a>
                        </Header>
                        <Content>
                            <asp:CheckBoxList runat="server" ID="cblProduct" Height="26px" RepeatColumns="5"
                                RepeatDirection="Horizontal" RepeatLayout="Table">
                            </asp:CheckBoxList>
                        </Content>
                    </ajaxToolkit:AccordionPane>
                </Panes>
            </ajaxToolkit:Accordion>
            <br />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
