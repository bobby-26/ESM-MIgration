<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsPostInvoice.aspx.cs"
    Inherits="AccountsPostInvoice" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tax" Src="~/UserControls/UserControlTaxMaster.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TaxType" Src="~/UserControls/UserControlTaxType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCompany" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInvoice" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" cssclass="hidden" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <asp:Button runat="server" cssclass="hidden" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" />
            <eluc:TabStrip ID="MenuInvoice1" runat="server" OnTabStripCommand="Invoice_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <div id="RefDuplicate" style="top: 0px; right: 0px; position: absolute;" visible="false">
                    <tr>
                        <td colspan="5">
                            <asp:HyperLink ID="HlinkRefDuplicate" runat="server" Text="Possible Duplicate Invoices exist for this Supplier. Click here to view the Invoice List "
                                ToolTip="Vendor Invoice Duplicate" Visible="False" Font-Bold="False" Font-Size="Large"
                                Font-Underline="True" ForeColor="Red" BorderColor="Red"></asp:HyperLink>
                        </td>
                    </tr>
                </div>
                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblVendorInvoiceNumber" runat="server" Text="Vendor Invoice Number"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <telerik:RadTextBox ID="txtSupplierRefEdit" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                            MaxLength="200" Width="293px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblESMRegisterNumber" runat="server" Text="Register Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" MaxLength="25" ReadOnly="true"
                            CssClass="readonlytextbox" Width="246px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <td width="15%">
                    <telerik:RadLabel ID="Literal1" runat="server" Text="Supplier Code"></telerik:RadLabel>
                </td>
                <td width="35%" colspan="2">
                    <span id="spnPickListMaker">
                        <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input" ReadOnly="false"
                            Width="90px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="input" ReadOnly="false"
                            Width="200px">
                        </telerik:RadTextBox>
                        <img id="ImgSupplierPickList" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                            style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                        <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                    </span>
                </td>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBankInformation" runat="server" Text="Bank Information Page"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="ucBankInfoPageNumber" runat="server" MaxLength="3"></eluc:Number>
                        <asp:HyperLink ID="lnkBankInformation" Target="_blank" Text="Bank information" runat="server"
                            ImageUrl="<%$ PhoenixTheme:images/bank-info.png%>" Visible="false" ToolTip="Bank Information">
                        </asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCurrencyCode" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="246px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBankInformationNotAvailable" runat="server" Text="Bank Information Not Available"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListBank0">
                            <asp:CheckBox ID="chkBankInformationAvailable" runat="server" OnCheckedChanged="chkBankInformationAvailable_CheckedChanged" />
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAmount" runat="server" Text="Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceAmoutEdit" runat="server" CssClass="readonlytextbox txtNumber"
                            ReadOnly="true" Width="246px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSupplierBankingDetails" runat="server" Text="Supplier Banking Details"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListBank">
                            <telerik:RadTextBox ID="txtAccountNo" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBankName" runat="server" CssClass="input_mandatory" ReadOnly="false"
                                Width="180px">
                            </telerik:RadTextBox>
                            <img id="imgBankPicklist" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                            <telerik:RadTextBox ID="txtBankID" runat="server" Width="0px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAdjustmentAmount" runat="server" Text="Adjustment Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAdjustmentAmount" runat="server" CssClass="readonlytextbox txtNumber"
                            Width="246px" ReadOnly="true">
                        </telerik:RadTextBox>
                        <asp:ImageButton ID="imgAdjustmentAttachment" runat="server" AlternateText="Attachment"
                            ImageUrl="<%$ PhoenixTheme:images/attachment.png%>" ToolTip="Attachment" />
                    </td>
                    <td rowspan="2">
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td rowspan="2">
                        <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="2" Width="246px"
                            CssClass="input">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtDTKey" runat="server" Visible="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGST" runat="server" Text="GST"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtGST" runat="server" CssClass="readonlytextbox txtNumber" Width="246px"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDelayedUtilization" runat="server" Text="Delayed Utilization"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkDelayedUtilization" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPriorityInvoice" runat="server" Text="Priority Invoice"></telerik:RadLabel>
                    </td>
                    <td>
                        <input type="checkbox" id="chkPriorityInv" value="0" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblApplicabilitySTax" runat="server" Text="Applicabilitiy of Service Tax"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkServiceTax" runat="server" OnCheckedChanged="chkServiceTax_Changed"
                            AutoPostBack="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblApplicabilityTDS" runat="server" Text="Applicabilitiy of TDS"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkTDS" runat="server" OnCheckedChanged="chkTDS_Changed" AutoPostBack="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblServiceTaxType" runat="server" Text="Service Tax Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlServiceTaxType" runat="server" CssClass="input" Enabled="false"
                            OnTextChanged="ddlServiceTaxType_TextChanged" AutoPostBack="true" Width="246px">
                        </telerik:RadDropDownList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTDSType" runat="server" Text="TDS Section Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlTDSType" runat="server" CssClass="input" Enabled="false"
                            AutoPostBack="true" OnTextChanged="ddlTDSType_TextChanged" Width="246px">
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblServiceTax" Text="Service Tax" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtServiTax" runat="server" CssClass="readonlytextbox" Mask="999.999"
                            ReadOnly="true" Width="246px" />
                        <telerik:RadLabel ID="lblServiceTaxPercentage" runat="server" Text="%"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTDSRate" runat="server" Text="TDS Rate"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtTDSRate" runat="server" CssClass="readonlytextbox" Mask="999.999"
                            ReadOnly="true" Width="246px" />
                        <telerik:RadLabel ID="lblTDSPercentage" runat="server" Text="%"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTDSOnWCT" Text="TDS on WCT" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="ddlWCTType" CssClass="input" Enabled="false"
                            OnTextChanged="ddlWCTType_TextChanged" AutoPostBack="true" Width="246px">
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWCTRate" Text="WCT Rate" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal ID="txtWCTRate" runat="server" CssClass="readonlytextbox" Mask="999.999"
                            ReadOnly="true" Width="246px" />
                        <telerik:RadLabel ID="lblWCTPercentage" runat="server" Text="%"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOrderLine" Height="40%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvOrderLine_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="" Visible="false" AllowSorting="true">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                    CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="true">
                            <HeaderStyle Width="283px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELACCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code" AllowSorting="true">
                            <HeaderStyle Width="128px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner Budget Code" AllowSorting="true">
                            <HeaderStyle Width="141px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code Amount" AllowSorting="true">
                            <HeaderStyle Width="163px" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetCodeAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Discount for Vessel" AllowSorting="true">
                            <HeaderStyle Width="163px" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDiscountforVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELDISCOUNTAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="GST for Vessel" AllowSorting="true">
                            <HeaderStyle Width="125px" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGSTforVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODEGSTFORVESSEL","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Amount" AllowSorting="true">
                            <HeaderStyle Width="130px" HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALVESSELAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <telerik:RadCodeBlock runat="server" ID="RadCodeBlock3">
                <table width="100%">
                    <tr>
                        <td>
                            <table cellspacing="0" cellpadding="3" rules="all" border="1" id="Table3" style="font-size: 11px; width: 50%; border-collapse: collapse;">
                                <tr class="DataGrid-HeaderStyle">
                                    <th scope="col">
                                        <span id="gvOrderLine_ctl01_lblVesselAccountHeader"></span>
                                    </th>
                                    <th scope="col">
                                        <span id="gvOrderLine_ctl01_lblBudgetCodeHeader" style="color: White;">
                                            <telerik:RadLabel ID="lblSupplierCodeCaption" runat="server" Text="Supplier Code"></telerik:RadLabel>
                                        </span>
                                    </th>
                                    <th scope="col" align="right">
                                        <span id="gvOrderLine_ctl01_lblBudgetCodeAmountHeader">
                                            <telerik:RadLabel ID="lblInvoicePayableAmount" runat="server" Text="Invoice Payable Amount"></telerik:RadLabel>
                                        </span>
                                    </th>
                                    <th scope="col" align="right">
                                        <span id="gv_OrderLine_ct101_lblBudgetCodeTDSPayable">
                                            <telerik:RadLabel ID="lblTDSPayable" runat="server" Text="TDS Payable"></telerik:RadLabel>
                                        </span>
                                    </th>
                                    <th scope="col" align="right">
                                        <span id="gv_OrderLine_ct101_lblBudgetCodeServicePayable">
                                            <telerik:RadLabel ID="lblServicePayable" runat="server" Text="Service Tax Payable"></telerik:RadLabel>
                                        </span>
                                    </th>
                                    <th scope="col" align="right">
                                        <span id="gv_OrderLine_ct101_lblBudgetCodeWCTPayable">
                                            <telerik:RadLabel ID="lblWCTPayable" runat="server" Text="WCT Payable"></telerik:RadLabel>
                                        </span>
                                    </th>
                                </tr>
                                <tr style="height: 10px;">
                                    <td align="left" style="white-space: nowrap;">
                                        <span id="gvOrderLine_ctl02_lblVesselAccount">
                                            <%=strCreditorAccountDetails%></span>
                                    </td>
                                    <td align="left" style="white-space: nowrap;">
                                        <span id="gvOrderLine_ctl02_lblBudgetCode">
                                            <%=strSupplierCode%></span>
                                    </td>
                                    <td align="right" style="white-space: nowrap;">
                                        <span id="gvOrderLine_ctl02_lblBudgetCodeAmount">
                                            <%= strInvoicePayableAmount%></span>
                                    </td>
                                    <td align="right" style="white-space: nowrap;">
                                        <span id="gv_OrderLine_ct102_lblTDSPayable">
                                            <%=strTDSPayable%></span>
                                    </td>
                                    <td align="right" style="white-space: nowrap;">
                                        <span id="gv_OrderLine_ct102_lblServicePayable">
                                            <%=strServiceTaxPayable%></span>
                                    </td>
                                    <td align="right" style="white-space: nowrap;">
                                        <span id="gv_OrderLine_ct102_lblWCTPayable">
                                            <%= strWCTPayable%></span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table cellspacing="0" cellpadding="3" rules="all" border="1" id="Table1" style="font-size: 11px; width: 50%; border-collapse: collapse;"
                                runat="server">
                                <tr class="DataGrid-HeaderStyle">
                                    <th scope="col" align="right">
                                        <span id="gvOrderLine_ctl01_lblDiscountforVesselHeader">
                                            <telerik:RadLabel ID="lblDiscount" runat="server" Text="Discount"></telerik:RadLabel>
                                        </span>
                                    </th>
                                    <th scope="col" align="right">
                                        <span id="gvOrderLine_ctl01_lblBudgetIdHeader">
                                            <telerik:RadLabel ID="lblGSTonDiscountCaption" runat="server" Text="GST on Discount"></telerik:RadLabel>
                                        </span>
                                    </th>
                                    <th scope="col" align="right">
                                        <span id="gvOrderLine_ctl01_lblVesselAmountHeader">
                                            <telerik:RadLabel ID="lblPayableAmount" runat="server" Text="Payable Amount"></telerik:RadLabel>
                                        </span>
                                    </th>
                                </tr>
                                <tr style="height: 10px;">
                                    <td align="right" style="white-space: nowrap;">
                                        <span id="gvOrderLine_ctl02_lblDiscountforVessel">
                                            <%=strCreditorsdiscountAmount%></span>
                                    </td>
                                    <td align="right" style="white-space: nowrap;">
                                        <span id="gvOrderLine_ctl02_lblGSTforVessel">
                                            <%=strGSTonDiscount %></span>
                                    </td>
                                    <td align="right" style="white-space: nowrap;">
                                        <span id="gvOrderLine_ctl02_lblVesselAmount">
                                            <%  if (dPayableamount != 0)
                                                { %>
                                                -  <%= strPayableAmount %>
                                            <%} %>


                                            <%  if (dPayableamount == 0)
                                                { %>
                                            <%= strPayableAmount %>
                                            <%} %>
                                        </span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table cellspacing="0" cellpadding="3" rules="all" border="1" id="Table2" style="font-size: 11px; width: 100%; border-collapse: collapse;">
                    <tr>
                        <td colspan="6">
                            <span id="spnCreditNoteDisc" runat="server">
                                <table width="100%">
                                    <tr class="DataGrid-HeaderStyle">
                                        <th scope="col">
                                            <span id="Span1"></span>
                                        </th>
                                        <th scope="col" align="right">
                                            <span id="Span2" style="color: White;">
                                                <telerik:RadLabel ID="lblDiscountRebate" runat="server" Text="Discount/Rebate"></telerik:RadLabel>
                                            </span>
                                        </th>
                                        <th scope="col" align="right">
                                            <span id="Span3">
                                                <telerik:RadLabel ID="lblGSTForVessel" runat="server" Text="GST For Vessel"></telerik:RadLabel>
                                            </span>
                                        </th>
                                        <th scope="col">
                                            <span id="Span4"></span>
                                        </th>
                                        <th scope="col">
                                            <span id="Span5"></span>
                                        </th>
                                        <th scope="col" align="right">
                                            <span id="Span6">
                                                <telerik:RadLabel ID="lblIncomeAmount" runat="server" Text="Income Amount"></telerik:RadLabel>
                                            </span>
                                        </th>
                                    </tr>
                                    <tr style="height: 10px;">
                                        <td align="left" style="white-space: nowrap;">
                                            <span id="Span7">
                                                <%=strIncomeAccountDetails%></span>
                                        </td>
                                        <td align="right" style="white-space: nowrap;">
                                            <span id="Span8">
                                                <%=strDiscountrebateincome%></span>
                                        </td>
                                        <td align="right" style="white-space: nowrap;">
                                            <span id="Span9">
                                                <%=strGSTForVessel%></span>
                                        </td>
                                        <td align="right" style="white-space: nowrap;">
                                            <span id="Span10"></span>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <span id="Span11"></span>
                                        </td>
                                        <td align="right" style="white-space: nowrap;">
                                            <span id="Span12">
                                                <%if (strTotalIncomeAmount != "0.00")
                                                    { %>
                                                    -
                                                    <%} %>
                                                <%=strTotalIncomeAmount%></span>
                                        </td>
                                    </tr>
                                    <tr class="DataGrid-HeaderStyle">
                                        <th scope="col">
                                            <span id="Span13"></span>
                                        </th>
                                        <th scope="col" align="right">
                                            <span id="Span14" style="color: White;">
                                                <telerik:RadLabel ID="lblGSTonInvoice" runat="server" Text="GST on Invoice"></telerik:RadLabel>
                                            </span>
                                        </th>
                                        <th scope="col" align="right">
                                            <span id="Span15">
                                                <telerik:RadLabel ID="lblGSTonDiscount" runat="server" Text="GST on Discount"></telerik:RadLabel>
                                            </span>
                                        </th>
                                        <th scope="col">
                                            <span id="Span16"></span>
                                        </th>
                                        <th scope="col">
                                            <span id="Span17"></span>
                                        </th>
                                        <th scope="col" align="right">
                                            <span id="Span18">
                                                <telerik:RadLabel ID="lblGSTClaim" runat="server" Text="GST Claim"></telerik:RadLabel>
                                            </span>
                                        </th>
                                    </tr>
                                    <tr style="height: 10px;">
                                        <td align="left" style="white-space: nowrap;">
                                            <span id="Span19">
                                                <%=strGSTClaimAccountDetails%></span>
                                        </td>
                                        <td align="right" style="white-space: nowrap;">
                                            <span id="Span20">
                                                <%=strGstAmount%></span>
                                        </td>
                                        <td align="right" style="white-space: nowrap;">
                                            <span id="Span21">
                                                <%=strGSTonDiscount%></span>
                                        </td>
                                        <td align="right" style="white-space: nowrap;">
                                            <span id="Span22"></span>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <span id="Span23"></span>
                                        </td>
                                        <td align="right" style="white-space: nowrap;">
                                            <span id="Span24">
                                                <%=strGstClaim%></span>
                                        </td>
                                    </tr>
                                    <tr class="DataGrid-HeaderStyle">
                                        <th scope="col">
                                            <span id="Span25"></span>
                                        </th>
                                        <th scope="col">
                                            <span id="Span26" style="color: White;">
                                                <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Supplier Code"></telerik:RadLabel>
                                            </span>
                                        </th>
                                        <th scope="col">
                                            <span id="Span27"></span>
                                        </th>
                                        <th scope="col">
                                            <span id="Span28"></span>
                                        </th>
                                        <th scope="col">
                                            <span id="Span29"></span>
                                        </th>
                                        <th scope="col" align="right">
                                            <span id="Span30">
                                                <telerik:RadLabel ID="lblRebateReceivable" runat="server" Text="Rebate Receivable"></telerik:RadLabel>
                                            </span>
                                        </th>
                                    </tr>
                                    <tr style="height: 10px;">
                                        <td align="left" style="white-space: nowrap;">
                                            <span id="Span31">
                                                <%=strRebateAccountDetails%></span>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <span id="Span32">
                                                <%=strSupplierCode%></span>
                                        </td>
                                        <td align="right" style="white-space: nowrap;">
                                            <span id="Span33"></span>
                                        </td>
                                        <td align="right" style="white-space: nowrap;">
                                            <span id="Span34"></span>
                                        </td>
                                        <td align="left" style="white-space: nowrap;">
                                            <span id="Span35"></span>
                                        </td>
                                        <td align="right" style="white-space: nowrap;">
                                            <span id="Span36">
                                                <%=strRebateReceivableAmount%></span>
                                        </td>
                                    </tr>
                                </table>
                            </span>
                        </td>
                    </tr>
                </table>
            </telerik:RadCodeBlock>
            <br />
        </telerik:RadAjaxPanel>
        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
