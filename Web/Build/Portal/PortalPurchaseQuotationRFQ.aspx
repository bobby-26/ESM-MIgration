<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PortalPurchaseQuotationRFQ.aspx.cs"
    Inherits="PortalPurchaseQuotationRFQ" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Numeber" Src="../UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TaxType" Src="~/UserControls/UserControlTaxType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="../UserControls/UserControlVendorCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ucUnit" Src="~/UserControls/UserControlPurchaseUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vendor RFQ</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">        
        <link rel="stylesheet" type="text/css" href="../css/Theme1/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="../fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="../js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="../js/phoenix.js"></script>                       
        <script>
            function confirm(args) {
            if (args) {
                 __doPostBack("<%=btnconfirm.UniqueID %>", "");
            } 
        }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseQuotationRFQ" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnableScriptCombine="false" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
                <AjaxSettings>
                    <telerik:AjaxSetting AjaxControlID="MenuQuotationLineItem">
                        <UpdatedControls>
                            <telerik:AjaxUpdatedControl ControlID="MenuQuotationLineItem" />
                            <telerik:AjaxUpdatedControl ControlID="RadAjaxPanel1" />
                            <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                            <telerik:AjaxUpdatedControl ControlID="ucError" />
                        </UpdatedControls>
                    </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>

        <%--<div style="height: 60px;" class="pagebackground">
            <div style="position: absolute; top: 15px;">
                <img id="Img1" runat="server" style="vertical-align: middle" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>"
                    alt="Phoenix" onclick="parent.hideMenu();" />
                <span class="title" style="color: White">
                    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                    <%=Application["softwarename"].ToString() %>
                        </telerik:RadCodeBlock>

                </span>
                <br />
            </div>
        </div>--%>
        <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MenuMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuQuotationLineItem" runat="server" OnTabStripCommand="MenuQuotationLineItem_TabStripCommand"></eluc:TabStrip>
        
        <div class="navigation" id="navigation" style="width: 100%; top: 60px;">
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="CopyForm_Click" OKText="Yes" CancelText="No" />
            <asp:Button ID="btnconfirm" runat="server" CssClass="hidden" OnClick="btnconfirm_Click" />
            <br clear="all" />
            <b style="color: Blue;">
                <telerik:RadLabel RenderMode="Lightweight" ID="lblGuidelinestoFilltheRFQ" runat="server" Text="Guidelines to Fill the RFQ:"></telerik:RadLabel>
                <br />
            </b>
            <ul>
                <li style="color: Blue;">
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblToQuoteprovidethequantityunitandpriceClickonSavebuttontosavetheinformation" runat="server" Text="To Quote, provide the quantity, unit and price. Click on Save button to save the information."></telerik:RadLabel>
                </li>
                <li style="color: Blue;">
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblForequipmentinformationpleasecheckindetailsscreen" runat="server" Text="For equipment information please check in details screen"></telerik:RadLabel>
                </li>
                <li style="color: Blue;">
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblTosendthequotationoncompletionClickonSendtoESMbutton" runat="server" Text="To send the quotation on completion, Click on Send Quote button."></telerik:RadLabel>
                </li>
                <li style="color: Blue;">
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblNoteYoucannotmakechangestothequotationonceyouSendtoESMifyouwanttochangeafterconfirmationyoucanaskthepurchasertoallowyoutorequote" runat="server" Text=" Note: You cannot make changes to the quotation once you Send Quote.If you want to change after confirmation, you can ask the purchaser to allow you to re-quote."></telerik:RadLabel>
                </li>
            </ul>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtVessel" runat="server" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                        <asp:HiddenField ID="hndVesselID" runat="server" />
                        <a id="A1" href="Help Document" onclick="javascript:return Openpopup('help', '', '../Purchase/PurchaseQuotationHelp.aspx');" runat="server">Help Document </a>

                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblIMONo" runat="server" Text="IMO No."></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtIMONo" runat="server" ReadOnly="true" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblHullNo" runat="server" Text="Hull No."></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtHullNo" runat="server" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtVesseltype" runat="server" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblYard" runat="server" Text="Yard"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtYard" ReadOnly="true" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblYearBuilt" runat="server" Text="Year Built"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtYearBuilt" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblSentby" runat="server" Text="Sent by"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtSenderName" runat="server" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDeliveryAddress" runat="server" Text="Delivery Address"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtDeliveryPlace" runat="server" ReadOnly="true" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblContactNo" runat="server" Text="Contact No."></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtContactNo" runat="server" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblSendersEMailId" runat="server" Text="Sender's E-Mail Id"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtSenderEmailId" ReadOnly="true" Width="200px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDeliveryPort" runat="server" Text="Delivery Port"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtPortName" ReadOnly="true" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblSentdate" runat="server" Text="Sent date"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtSentDate" runat="server" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="ltrlComponentName" runat="server" Text="Component Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentName" runat="server" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="ltrlComponentModel" runat="server" Text="Component Model"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentModel" runat="server" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="ltrlComponentSerialNo" runat="server" Text="Component Serial No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtComponentSerialNo" runat="server" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblVendorName" runat="server" Text="Vendor Name"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtVendorName" runat="server" Width="200px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblTelephone" runat="server" Text="Telephone"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtTelephone" runat="server" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblFax" runat="server" Text="Fax"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtFax" runat="server" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblAddress" runat="server" Text="Address"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtVendorAddress" runat="server" TextMode="MultiLine"
                            ReadOnly="true" Height="20px" Width="200px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblEmail" runat="server" Text="Email"></telerik:RadLabel>

                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtEmail" runat="server" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblQuotationRef" runat="server" Text="Quotation Ref."></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtVenderReference" runat="server" Width="120px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblQuotationValidUntil" runat="server" Text="Quotation Valid Until"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Date ID="txtOrderDate" runat="server" Width="120px" />
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblExpiryDate" runat="server" Text="Expiry Date"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Date ID="txtExpirationDate" runat="server" Enabled="false" ReadOnly="true" Width="120px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtPriority" runat="server" ReadOnly="true" Width="120px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDeliveryTerms" runat="server" Text="Delivery Terms"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Quick ID="UCDeliveryTerms" AppendDataBoundItems="true" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblPaymentTerms" runat="server" Text="Payment Terms"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Quick ID="UCPaymentTerms" AppendDataBoundItems="true" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDelTimeDays" runat="server" Text="Del. Time(Days)"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Numeber ID="txtDeliveryTime" runat="server" Width="120px" />
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblModeofTransport" runat="server" Text="Mode of Transport"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Quick runat="server" ID="ucModeOfTransport" QuickTypeCode="77"
                            AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblType" runat="server" Text="Items Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ddlType" AppendDataBoundItems="true" HardTypeCode="244" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ucCurrency" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblDisconInvoice" runat="server" Text="Disc. on Invoice(%)"></telerik:RadLabel>

                    </td>
                    <td colspan="2">
                        <eluc:Decimal DecimalDigits="2" Mask="99.99" RenderMode="Lightweight" ID="txtSupplierDiscount" runat="server" Width="120px" CssClass="txtNumber"></eluc:Decimal>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblPricewithoutDisc" runat="server" Text="Price without Disc."></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtPrice" runat="server" CssClass="txtNumber" ReadOnly="True"
                            Width="120px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblAdditionalLumpsumDisc" runat="server" Text="Additional Lump sum Disc."></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtTotalDiscount" runat="server" Width="120px"
                            CssClass="txtNumber">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtTotalCharges" runat="server" Width="80px" Visible="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblPricewithCharges" runat="server" Text="Price with Charges"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtTotalPrice" runat="server" CssClass="txtNumber"
                            ReadOnly="True" Width="120px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblCreditNoteDisc" runat="server" Text="Credit Note Disc.(%)"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <eluc:Decimal ID="txtDiscount" runat="server" Width="120px" CssClass="txtNumber" DecimalDigits="2" Mask="99.99"></eluc:Decimal>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
            </table>
            </telerik:RadAjaxPanel>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvTax" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvTax" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnDeleteCommand="gvTax_DeleteCommand" ShowFooter="true" Width="100%" Height="200px"
                OnNeedDataSource="gvTax_NeedDataSource" OnEditCommand="gvTax_EditCommand"
                OnItemDataBound="gvTax_ItemDataBound1" OnItemCommand="gvTax_ItemCommand" OnUpdateCommand="gvTax_UpdateCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDQUOTATIONTAXMAPCODE" TableLayout="Fixed">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Delivery/Tax/Other Charges Description">
                            <ItemStyle Wrap="false" Width="40%" />
                            <HeaderStyle Width="40%" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                    CssClass="gridinput_mandatory" MaxLength="50">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtDescriptionAdd" Text='' runat="server" CssClass="gridinput_mandatory"
                                    MaxLength="50">
                                </telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Type">
                            <ItemStyle Wrap="False" Width="20%"></ItemStyle>
                            <HeaderStyle Width="20%" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAXTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:TaxType ID="ucTaxTypeEdit" runat="server" TaxType='<%# DataBinder.Eval(Container,"DataItem.FLDTAXTYPE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:TaxType ID="ucTaxTypeAdd" runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Value">
                            <ItemStyle Wrap="False" Width="10%" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE" ,"{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Decimal ID="txtValueEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE","{0:n2}") %>'
                                    CssClass="gridinput_mandatory txtNumber" DecimalDigits="2" Mask="99999.99"></eluc:Decimal>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Decimal ID="txtValueAdd" runat="server" CssClass="gridinput_mandatory txtNumber"
                                    Width="100%" DecimalDigits="2" Mask="999999.99"></eluc:Decimal>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <ItemStyle Wrap="False" Width="10%" HorizontalAlign="Right"></ItemStyle>
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblTaxAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAXAMOUNT","{0:n2}") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <HeaderStyle Width="120px" HorizontalAlign="Center" />
                            <ItemStyle Width="120px" HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add"
                                    CommandName="ADD" ID="cmdAdd" ToolTip="Add">
                                            <span class="icon"><i class="fas fa-plus"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                        PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuRegistersStockItem" runat="server" OnTabStripCommand="RegistersStockItem_TabStripCommand"></eluc:TabStrip>
            </div>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvVendorItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnDeleteCommand="gvTax_DeleteCommand" ShowFooter="false" Width="100%" Height="600px"
                OnNeedDataSource="gvVendorItem_NeedDataSource" OnEditCommand="gvVendorItem_EditCommand"
                OnItemDataBound="gvVendorItem_ItemDataBound" OnItemCommand="gvVendorItem_ItemCommand" OnUpdateCommand="gvVendorItem_UpdateCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDQUOTATIONLINEID,FLDORDERLINEID,FLDDTKEY,FLDCOMPONENTID,FLDVESSELID,FLDATTACHMENTFLAG,FLDPARTID,FLDNOTES" TableLayout="Fixed">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No">
                            <ItemStyle Wrap="false" HorizontalAlign="Center" Width="40px"/>
                            <HeaderStyle Width="40px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number">
                            <ItemStyle HorizontalAlign="Left" Wrap="false" Width="120px"></ItemStyle>
                            <HeaderStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblNumber" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Maker Reference" UniqueName="MAKERREF" >
                            <ItemStyle HorizontalAlign="Left" Wrap="true" Width="120px"></ItemStyle>
                            <HeaderStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblMakerReference" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Drawing No/Position">
                            <ItemStyle HorizontalAlign="Left" Width="90px"></ItemStyle>
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblPosition" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSITIONDRAWING") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Details" UniqueName="DETAILS">
                            <ItemStyle HorizontalAlign="Left" Wrap="true" Width="180px"></ItemStyle>
                            <HeaderStyle Width="180px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDetails" runat="server" Visible="true" Width="140px" Text='<%# "<b>Serial No:</b> " + DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") + "<br/>" + "<b>Type:</b> " + DataBinder.Eval(Container,"DataItem.FLDTYPE") +"<br/>" +"<b>Maker :</b> " + DataBinder.Eval(Container,"DataItem.FLDMAKERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="240px"></ItemStyle>
                            <HeaderStyle Width="240px" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkStockItemCode" runat="server" CommandName="EDIT" 
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                <br />
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblComponentName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME")%>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipComponent" runat="server" Text='<%# "Serial No: " + DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") + "<br/>" + "Type: " + DataBinder.Eval(Container,"DataItem.FLDTYPE") +"<br/>" +"Maker : " + DataBinder.Eval(Container,"DataItem.FLDMAKERNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblVendorNotes" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORNOTES") %>'
                                    Visible="false"></telerik:RadLabel>
                                <asp:LinkButton runat="server" AlternateText="remarks" 
                                            CommandName="ViewRecord" ID="imgVendorNotes" ToolTip="View">
                                            <span class="icon"><i class="fas fa-glasses"></i></span>
                                        </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Order Qty">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="90px"></ItemStyle>
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblOrderQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Quoted Qty">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="90px"></ItemStyle>
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Decimal ID="txtQuantityEdit" runat="server" CssClass="gridinput_mandatory" MinValue="0" DecimalDigits="0" Width="100%"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>' InterceptArrowKeys="false" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Unit">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:ucUnit ID="ucUnit" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="100%"
                                    SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Unit Price">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="90px"></ItemStyle>
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblQuotedPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Decimal ID="txtQuotedPriceEdit" runat="server" Width="60px" CssClass="gridinput_mandatory" MinValue="0"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE","{0:n2}") %>' DecimalPlace="3" MaxLength="16" InterceptArrowKeys="false" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Disc (%)">
                            <ItemStyle Wrap="true" HorizontalAlign="Right" Width="90px"></ItemStyle>
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Decimal ID="txtDiscountEdit" runat="server" Width="60px" CssClass="gridinput" MinValue="0" DecimalDigits="2" MaxValue="99.99"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Total Price">
                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="90px"></ItemStyle>
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="txtTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TOTALPRICE","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Del. Time (Days)">
                            <ItemStyle HorizontalAlign="Right" Width="60px"></ItemStyle>
                            <HeaderStyle Width="60px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblDeliveryTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVERYTIME","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Decimal ID="txtDeliveryTimeEdit" runat="server" Width="100%" CssClass="gridinput" MinValue="0" MaxValue="999" 
                                   DecimalDigits="0" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVERYTIME","{0:n0}") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="90" ></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="90px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                
                                <asp:LinkButton runat="server" AlternateText="View Attachment"
                                    CommandName="VIEWATTACHMENT" ID="cmdViewAttachment"
                                    ToolTip="View Attachment">
                                        <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="View Details"
                                    CommandName="VIEWDETAILS" ID="cmdDetails"
                                    ToolTip="View Details">
                                        <span class="icon"><i class="fas fa-info"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Item Details"
                                    CommandName="ITEMDETAILS" ID="cmdDetail"
                                    ToolTip="Item Details">
                                        <span class="icon"><i class="fas fa-cogs"></i></span>
                                </asp:LinkButton>
                                
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                        PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" Position="Top" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                </ClientSettings>
            </telerik:RadGrid>
        </div>
    </form>
</body>
</html>
