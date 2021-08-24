<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceBySupplierLineItemDetails.aspx.cs"
    Inherits="AccountsInvoiceBySupplierLineItemDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseFormItemDetails" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlStockItemEntry">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Items "></eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuLineItem" runat="server" OnTabStripCommand="MenuLineItem_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div class="navSelect* " style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOrderLineItem" runat="server" OnTabStripCommand="MenuOrderLineItem_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1; width: 100%;">
                    <asp:GridView ID="gvLineItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvLineItem_RowCommand" OnRowDataBound="gvLineItem_RowDataBound"
                        OnRowCancelingEdit="gvLineItem_RowCancelingEdit" OnRowDeleting="gvLineItem_RowDeleting"
                        OnRowEditing="gvLineItem_RowEditing" OnRowCreated="gvLineItem_RowCreated" ShowHeader="true"
                        EnableViewState="false" AllowSorting="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="PurchaseOrderNumber">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblOffsetHeader" runat="server" ForeColor="White">Offset &nbsp;</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOffset" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAXBASIS") %>'></asp:Label>
                                    <asp:ImageButton ID="cmdOffset" runat="server" AlternateText="Offset Vessel" ImageUrl="<%$ PhoenixTheme:images/Vessel.png%>"
                                        CommandName="OFFSET" CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Offset Vessel" />
                                    <img id="Img9" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ApprovalStatus">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <%--<asp:Label ID="lblApprovalStatus" runat="server" ForeColor="White">Approval Status &nbsp;</asp:Label>--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblApproval" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALSTATUS") %>'></asp:Label>
                                    <asp:Label ID="lblPOReceived" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORECEIVEDINVESSEL") %>'></asp:Label>
                                    <asp:ImageButton ID="cmdApproved" runat="server" AlternateText="Approval Status"
                                        ImageUrl="<%$ PhoenixTheme:images/approved.png%>" CommandName="APPROVED" CommandArgument="<%# Container.DataItemIndex %>"
                                        ToolTip="Approved" Enabled="false" />
                                    <asp:ImageButton ID="cmdAwaitingForApproval" runat="server" AlternateText="Awaiting For Approval"
                                        ImageUrl="<%$ PhoenixTheme:images/waiting-approve.png%>" CommandName="AWAITINGFORAPPROVAL"
                                        CommandArgument="<%# Container.DataItemIndex %>" ToolTip="Awaiting Approval"
                                        Enabled="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PurchaseOrderNumber">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblPurchaseOrderNumberHeader" runat="server" ForeColor="White">PO Number&nbsp;</asp:Label>
                                    <img id="FLDPURCHASEORDERNUMBER" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lnkPurchaseOrderNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEORDERNUMBER") %>'></asp:Label>
                                    <asp:TextBox ID="txtInvoiceLineItemCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICELINEITEMCODE") %>'
                                        MaxLength="10" Visible="false"></asp:TextBox>
                                    <asp:Label ID="lblStockType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></asp:Label>
                                    <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                    <asp:TextBox ID="txtOrderId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'
                                        MaxLength="10" Visible="false"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="VesselName">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="POCurrencyHeader">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPOCurrency" runat="server" Text="PO Currency"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPOCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOCURRENCYCODE") %>'></asp:Label>
                                    <asp:Label ID="lblPOCurrencyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOCURRENCYID") %>'></asp:Label>
                                    <asp:Label ID="lblInvoiceCurrencyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICECURRENCYID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PurCommittedAmount">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPOCommittedAmount" runat="server" Text="PO Committed Amount"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPurCommittedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMITTEDAMOUNT","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PurShortDeliveryAmount">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPOShortDeliveryAmount" runat="server" Text="PO Short Delivery Amount"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPurShortDeliveryAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTDELIVERYAMOUNT","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PurAddditionalCost">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPOAdditionalCharges" runat="server" Text="PO Additional Charges"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPurAddditionalCost" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDITIONALCOST","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="PurPayableAmount">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblInvoicePayableAmount" runat="server" Text="Invoice Payable Amount:"></asp:Literal>
                                    <%=InvoicePayableAmount%><br />
                                    PO Payable Amount. (Running Total :
                                    <%=POPayableAmount %>)
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPOPayableAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEPAYABLEAMOUNTWITHINVOICEDIFF","{0:n2}") %>'></asp:Label>
                                    <asp:Label ID="lblPurPayableAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEPAYABLEAMOUNT","{0:n2}") %>'
                                        Visible="false"></asp:Label>
                                    <asp:Label ID="lblInvoicePayableAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEPAYABLEAMOUNT","{0:n2}") %>'
                                        Visible="false"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="AdvancePayment">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAdvancePayment" runat="server" Text="Advance Payment"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAdvPayment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENT","{0:n2}") %>'
                                        Visible="false"></asp:Label>
                                    <asp:LinkButton ID="lnkAdvanceAmount" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENT","{0:n2}") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GSTOnInvoice">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblGSTOnPOPayable" runat="server" Text="GST On PO Payable"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblGSTOnInvoice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGSTAMOUNT","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="GSTOnDiscount">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblGSTOnDiscount" runat="server" Text="GST On Discount"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblGSTOnDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGSTDISCOUNTAMOUNT","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TotalDiscountAmount">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblTotalDiscountAmount" runat="server" Text="Total Discount Amount"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalDiscountAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERFORMTOTALDISCOUNTAMOUNT","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="VesselAllocatedDiscount">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblDiscountForVessel" runat="server" Text="Discount For Vessel"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselAllocatedDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELALLOCATEDDISCOUNTAMOUNT","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ServiceChargesHeader">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblServiceCharge" runat="server" Text="Service Charge"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblServiceCharges" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERVICECHARGE","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="POLastUpdatedByHeader">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblReceiptLastUpdatedBy" runat="server" Text="Receipt Last Updated By"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPOLastUpdatedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORECEIPTLASTUPDATEDBY") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server"> Action </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                                <ItemTemplate>
                                    <%--<asp:ImageButton ID="cmdViewPOForm" runat="server" AlternateText="Purchase Form"
                                        CommandArgument="<%# Container.DataItemIndex %>" CommandName="PURCHASEFORM" ImageUrl="<%$ PhoenixTheme:images/view-po-detail.png%>"
                                        ToolTip="View Purchase Form" />
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdOrder" runat="server" AlternateText="Order" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="ORDER" ImageUrl="<%$ PhoenixTheme:images/task-list.png%>" ToolTip="Order" />
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdReceipt" runat="server" AlternateText="Receipt" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="RECEIPT" ImageUrl="<%$ PhoenixTheme:images/task-list.png%>" ToolTip="Receipt" />
                                    <asp:Label ID="lblOrdertype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERFORMTYPE") %>'
                                        Visible="false"></asp:Label>
                                    <img id="Img7" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />--%>
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                    <%--<img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdPOReconsilationStaging" runat="server" AlternateText="Reconsillation"
                                        CommandArgument="<%# Container.DataItemIndex %>" CommandName="PORECONSILATIONSTAGING"
                                        ImageUrl="<%$ PhoenixTheme:images/po-staging.png %>" ToolTip="Reconciliation Staging" />
                                    <asp:ImageButton ID="cmdPOPostInvoiceStaging" runat="server" AlternateText="PostInvoice"
                                        CommandArgument="<%# Container.DataItemIndex %>" CommandName="POPOSTINVOICESTAGING"
                                        ImageUrl="<%$ PhoenixTheme:images/po-staging.png %>" ToolTip="Post Invoice Staging" />
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdRefresh" runat="server" AlternateText="Refresh" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="POREFRESH" ImageUrl="<%$ PhoenixTheme:images/in-progress.png %>"
                                        ToolTip="Refresh" />--%>
                                   <%--<asp:ImageButton ID="cmdPOApprove" runat="server" AlternateText="POApprove" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="POAPPROVE" ImageUrl="<%$ PhoenixTheme:images/approve.png %>" ToolTip="PO Approve"
                                        Enabled="false" Visible="false" />--%>
                                    <%--<img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton ID="cmdPurAttachment" runat="server" AlternateText="Purchase Attachment" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="PURATTACHMENT" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        ToolTip="Purchase Attachment" />--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="ucConfirm_Click" OKText="Yes"
                CancelText="No" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
