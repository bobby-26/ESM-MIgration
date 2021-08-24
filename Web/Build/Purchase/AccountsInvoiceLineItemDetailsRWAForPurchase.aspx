<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceLineItemDetailsRWAForPurchase.aspx.cs"
    Inherits="AccountsInvoiceLineItemDetailsRWAForPurchase" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function fnConfirmTelerik(sender, msg) {
                var callBackFn = function (shouldSubmit) {
                    if (shouldSubmit) {
                        //sender.click();
                        //if (Telerik.Web.Browser.ff) {
                        //    sender.get_element().click();
                        //}
                        eval(sender.target.parentElement.parentElement.href);
                    }
                    else {
                        if (e.which)
                            e.stopPropagation();
                        else
                            window.event.cancelBubble = true;
                        return false;
                    }
                }
                var confirm;

                if (msg == null)
                    confirm = radconfirm("Are you sure you want to delete this record?", callBackFn);
                else
                    confirm = radconfirm(msg, callBackFn);

                return false;
            }
        </script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>
        <script type="text/javascript">
            function confirmdelete(args) {
                if (args) {
                    __doPostBack("<%=confirmdelete.UniqueID %>", "");
                }
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormItemDetails" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuLineItem" runat="server" OnTabStripCommand="MenuLineItem_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuOrderLineItem" runat="server" OnTabStripCommand="MenuOrderLineItem_TabStripCommand"></eluc:TabStrip>
            <asp:HyperLink ID="HlinkRefDuplicate" runat="server" Text="Possible Duplicate Voucher exist for this Invoice Reference"
                ToolTip="Vendor Invoice Duplicate" Visible="False" Font-Bold="False" Font-Size="Large"
                Font-Underline="True" ForeColor="Red" BorderColor="Red"></asp:HyperLink>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvLineItem" Height="85%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvLineItem_ItemCommand" OnItemDataBound="gvLineItem_ItemDataBound" OnNeedDataSource="gvLineItem_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                        <telerik:GridTemplateColumn HeaderText="Offset" AllowSorting="true">
                            <HeaderStyle Width="50px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOffset" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAXBASIS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Approval Status" AllowSorting="true">
                            <HeaderStyle Width="5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblApproval" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALSTATUS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPOReceived" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORECEIVEDINVESSEL") %>'></telerik:RadLabel>
                                <asp:ImageButton ID="cmdApproved" runat="server" AlternateText="Approval Status"
                                    ImageUrl="<%$ PhoenixTheme:images/approved.png%>" CommandName="APPROVED" CommandArgument="<%# Container.DataSetIndex %>"
                                    ToolTip="Approved" Enabled="false" />
                                <asp:ImageButton ID="cmdAwaitingForApproval" runat="server" AlternateText="Awaiting For Approval"
                                    ImageUrl="<%$ PhoenixTheme:images/waiting-approve.png%>" CommandName="AWAITINGFORAPPROVAL"
                                    CommandArgument="<%# Container.DataSetIndex %>" ToolTip="Awaiting Approval"
                                    Enabled="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO Number" AllowSorting="true">
                            <HeaderStyle Width="132px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgReceivedBeforeInvoice" runat="server" AlternateText="RECEIVEBEFOREINV"
                                    CommandArgument="<%# Container.DataSetIndex %>" CommandName="RECEIVEBEFOREINV"
                                    ImageUrl="<%$ PhoenixTheme:images/Calendar.png%>" ToolTip="PO Approved After Invoice is Registered"
                                    Visible="false" />
                                <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <telerik:RadLabel ID="lnkPurchaseOrderNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEORDERNUMBER") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtInvoiceLineItemCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICELINEITEMCODE") %>'
                                    MaxLength="10" Visible="false">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="lblStockType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtOrderId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'
                                    MaxLength="10" Visible="false">
                                </telerik:RadTextBox>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name" AllowSorting="true">
                            <HeaderStyle Width="135px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO Currency" AllowSorting="true">
                            <HeaderStyle Width="73px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPOCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOCURRENCYCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPOCurrencyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOCURRENCYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInvoiceCurrencyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICECURRENCYID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO Committed" AllowSorting="true">
                            <HeaderStyle Width="83px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurCommittedAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMITTEDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO Short Delivery" AllowSorting="true">
                            <HeaderStyle Width="5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurShortDeliveryAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTDELIVERYAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO Additional Charges" AllowSorting="true">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurAddditionalCost" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDITIONALCOST","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Payable Amount:  PO Payable Amount. " AllowSorting="true">
                            <HeaderStyle Width="99px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblTranAmount" runat="server"></telerik:RadLabel>
                                Invoice Payable Amount: &nbsp;(<%=InvoicePayableAmount%>)<br />
                                &nbsp;PO Payable Amount. &nbsp;(<%=POPayableAmount%>)
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPOPayableAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEPAYABLEAMOUNTWITHINVOICEDIFF","{0:n2}") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPurPayableAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEPAYABLEAMOUNT","{0:n2}") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblInvoicePayableAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEPAYABLEAMOUNT","{0:n2}") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Advance Payment" AllowSorting="true">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAdvPayment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENT","{0:n2}") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkAdvanceAmount" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENT","{0:n2}") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="GST On PO Payable" AllowSorting="true">
                            <HeaderStyle Width="65px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGSTOnInvoice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGSTAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="GST On Discount" AllowSorting="true">
                            <HeaderStyle Width="65px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGSTOnDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGSTDISCOUNTAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Discount Amount" AllowSorting="true">
                            <HeaderStyle Width="66px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTotalDiscountAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERFORMTOTALDISCOUNTAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Discount For Vessel" AllowSorting="true">
                            <HeaderStyle Width="71px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselAllocatedDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELALLOCATEDDISCOUNTAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Service Charge" AllowSorting="true">
                            <HeaderStyle Width="66px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblServiceCharges" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERVICECHARGE","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Receipt Last Updated By" AllowSorting="true">
                            <HeaderStyle Width="166px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPOLastUpdatedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORECEIPTLASTUPDATEDBY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true">
                            <HeaderStyle HorizontalAlign="Center" Width="70px" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                               <%-- <asp:ImageButton ID="cmdViewPOForm" runat="server" Visible="false" AlternateText="Purchase Form"
                                    CommandArgument="<%# Container.DataSetIndex %>" CommandName="PURCHASEFORM" ImageUrl="<%$ PhoenixTheme:images/view-po-detail.png%>"
                                    ToolTip="View Purchase Form" />
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton ID="cmdOrder" runat="server" AlternateText="Order" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="ORDER" ImageUrl="<%$ PhoenixTheme:images/task-list.png%>" ToolTip="Order" />
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton ID="cmdReceipt" runat="server" AlternateText="Receipt" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="RECEIPT" ImageUrl="<%$ PhoenixTheme:images/task-list.png%>" ToolTip="Receipt" />
                                <telerik:RadLabel ID="lblOrdertype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERFORMTYPE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <img id="Img7" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                                <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />--%>
                                <asp:ImageButton ID="cmdPOApprove" runat="server" AlternateText="POApprove" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="POAPPROVE" ImageUrl="<%$ PhoenixTheme:images/approve.png %>" ToolTip="PO Approve"
                                         Visible="true" />
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton ID="cmdPOReconsilationStaging" runat="server" AlternateText="Reconsillation"
                                    CommandArgument="<%# Container.DataSetIndex %>" CommandName="PORECONSILATIONSTAGING"
                                    ImageUrl="<%$ PhoenixTheme:images/po-staging.png %>" ToolTip="Reconciliation Staging" />
                               <%-- <asp:ImageButton ID="cmdPOPostInvoiceStaging" runat="server" AlternateText="PostInvoice"
                                    CommandArgument="<%# Container.DataSetIndex %>" CommandName="POPOSTINVOICESTAGING"
                                    ImageUrl="<%$ PhoenixTheme:images/po-staging.png %>" ToolTip="Post Invoice Staging" />
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton ID="cmdRefresh" runat="server" AlternateText="Refresh" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="POREFRESH" ImageUrl="<%$ PhoenixTheme:images/in-progress.png %>"
                                    ToolTip="Refresh" />--%>
                                <%--  <asp:ImageButton ID="cmdPOApprove" runat="server" AlternateText="POApprove" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="POAPPROVE" ImageUrl="<%$ PhoenixTheme:images/approve.png %>" ToolTip="PO Approve"
                                        Enabled="false" Visible="false" />--%>
                                <%--<img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton ID="cmdPurAttachment" runat="server" AlternateText="Purchase Attachment" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="PURATTACHMENT" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    ToolTip="Purchase Attachment" />--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <%--<eluc:Confirm ID="ucConfirm" runat="server" OnConfirmMesage="ucConfirm_Click" OKText="Yes"
                CancelText="No" />--%>
            <asp:Button ID="confirm" runat="server" CssClass="hidden" Text="confirm" OnClick="confirm_Click" />
            <asp:Button ID="confirmdelete" runat="server" CssClass="hidden" Text="confirmdelete" OnClick="confirmdelete_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
