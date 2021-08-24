<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoicePaymentVoucherLineItemDetails.aspx.cs"
    Inherits="AccountsInvoicePaymentVoucherLineItemDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice Payment VoucherLine ItemDetails</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmMessage.UniqueID %>", "");
                }
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormItemDetails" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <%--     <eluc:Confirm ID="ucConfirmMessage" runat="server" OnConfirmMesage="OnAction_Click"
                OKText="Yes" CancelText="No" />--%>
            <asp:Button ID="ucConfirmMessage" runat="server" OnConfirmMesage="OnAction_Click" CssClass="hidden" />

            <eluc:Title runat="server" ID="Title1" Text="Invoice Payment Voucher Details" Visible="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuRevoke" runat="server" OnTabStripCommand="MenuRevoke_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" style="width: 100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="VoucherNumber"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVoucherNumber" runat="server" MaxLength="50" ReadOnly="true"
                            Width="240px" CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                        <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                            ID="cmdApprove" OnClick="cmdApprove_OnClientClick" ToolTip="Approve"></asp:ImageButton>
                    </td>
                     <td>
                        <telerik:RadLabel ID="lblBankName" runat="server" Text="Bank Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBeneficiaryBankName" runat="server" CssClass="readonlytextbox"
                            Width="240px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherDate" runat="server" Text="Voucher Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVoucherDate" runat="server" CssClass="readonlytextbox" MaxLength="50"
                            ReadOnly="true" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBeneficiaryName" runat="server" Text="Beneficiary Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBeneficiaryName" runat="server" CssClass="readonlytextbox" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Payee"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListVendor">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="90px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVendorName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="200" Width="250px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVendorId" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBankAccountNumber" runat="server" Text="Bank Account Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAccountNumber" runat="server" CssClass="readonlytextbox" Width="160px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lblBankdetails" runat="server" Text="Banking Details"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBankdetails" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlCurrency" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            AppendDataBoundItems="true" Enabled="false" runat="server" CssClass="input readonlytextbox" />
                    </td>
                    <td rowspan="2">
                        <telerik:RadLabel runat="server" ID="lblRevokeRemarks" Text="Revoke Remarks"></telerik:RadLabel>
                    </td>
                    <td rowspan="2">
                        <telerik:RadTextBox runat="server" ID="txtRevokeRemarks" CssClass="input_mandatory" TextMode="MultiLine" Resize="Both" Rows="3" Width="300px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPayableAmount" runat="server" Text="Payable Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAmount" runat="server" MaxLength="50" ReadOnly="true" Width="120px"
                            CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                        <%--            <ajaxtoolkit:maskededitextender id="MaskNumber" runat="server" targetcontrolid="txtAmount"
                            mask="999,999,999.99" masktype="Number" inputdirection="RightToLeft" acceptnegative="Left">
                            </ajaxtoolkit:maskededitextender>--%>
                    </td>
                    <td colspan="6"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRevokedBy" runat="server" Text="Revoked By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRevokeBy" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="Literal1" runat="server" Text="Revoked Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRevokedDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="8">
                        <hr />
                    </td>
                </tr>
            </table>
            <table cellpadding="0" cellspacing="1" style="width: 100%">
                <tr>
                    <td style="vertical-align: top">
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvInvoicePo" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnSelectedIndexChanging="gvInvoicePo_SelectedIndexChanging" EnableHeaderContextMenu="true" GroupingEnabled="true"
                            OnPreRender="gvInvoicePo_PreRender" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvInvoicePo_NeedDataSource"
                            OnItemDataBound="gvInvoicePo_ItemDataBound" OnItemCommand="gvInvoicePo_ItemCommand"
                            AllowSorting="true" ShowFooter="true">
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false">
                                <Columns>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblSupplierCurrencyMismatch" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERCURRENCYMISMATCH") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                            <asp:ImageButton runat="server" AlternateText="SUPPLIERMISMATCH" ImageUrl="<%$ PhoenixTheme:images/suppliercode_mismatch.png %>"
                                                CommandName="SUPPLIERMISMATCH" CommandArgument="<%# Container.DataItem %>"
                                                ID="imbSupplierMismatch" Enabled="false" ToolTip="Supplier Mismatch" Visible="false"></asp:ImageButton>
                                            <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                            <asp:ImageButton ID="imbCurrencyMismatch" runat="server" AlternateText="CURRENCYMISMATCH"
                                                CommandArgument="<%# Container.DataItem %>" CommandName="CURRENCYMISMATCH"
                                                ImageUrl="<%$ PhoenixTheme:images/currency_mismatch.png%>" Enabled="false" ToolTip="Currency Mismatch"
                                                Visible="false" />
                                            <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                            <asp:ImageButton ID="imgReceivedBeforeInvoice" runat="server" AlternateText="RECEIVEBEFOREINV"
                                                CommandArgument="<%# Container.DataItem %>" CommandName="RECEIVEBEFOREINV"
                                                ImageUrl="<%$ PhoenixTheme:images/Calendar.png%>" Enabled="false" ToolTip="PO Created After Invoice is Registered"
                                                Visible="false" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Voucher Number" HeaderStyle-Width="13%">
                                        <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HlinkRefDuplicate" runat="server" Text="Vendor invoice already exists. <br/> Click here to view the invoice list "
                                                ToolTip="Vendor Invoice Duplicate" Visible="False" Font-Bold="true" Font-Size="Smaller"
                                                Font-Underline="True" ForeColor="Red" BorderColor="Red"></asp:HyperLink>
                                            <br />
                                            <telerik:RadLabel ID="lblVendorinvoiceduplicateexists" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORINVOICENUMBERALREADYEXISTS") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <asp:LinkButton ID="lnkInvoiceNumber" runat="server" CommandName="SelectInv" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER")  %>'></asp:LinkButton>
                                            <telerik:RadLabel ID="lblInvoicenumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <br />
                                            <telerik:RadLabel ID="lblPurchaseInvoiceVoucherNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEINVOICEVOUCHERNUMBER") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Invoice Reference" HeaderStyle-Width="12%">
                                        <HeaderStyle HorizontalAlign="Left" Wrap="true" />
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInvoiceRef" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESUPPLIERREFERENCE") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblInvoiceCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICECODE") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="PO Number" HeaderStyle-Width="11%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPoNumber" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblInvoicePoId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERIDLIST") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblBudgetGroupId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENTBUDGETID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Vessel Name" HeaderStyle-Width="17%" HeaderStyle-Wrap="true">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME")  %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderStyle-Width="8%">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblBudgetGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                                            <br />
                                            <telerik:RadLabel ID="lblSubAccountDetails" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNTDETAILS") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Committed amount" HeaderStyle-Width="7%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblCommittedAmtHeader" runat="server">
                                                Committed amount (<%=strReportCurrencyCode %>)&nbsp;
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCommittedAmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMITTEDAMOUNTINREPORTCURRENCY","{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Difference" HeaderStyle-Width="7%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblInvoiceDiffHeader" runat="server">
                                                Difference (<%=strReportCurrencyCode %>)&nbsp;
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblInvoiceDiff" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDIFFERENCEAMOUNTINREPORTCURRENCY","{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <%-- <telerik:GridTemplateColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblExDiffHeader" runat="server">Exchange difference (<%=strReportCurrencyCode %>)&nbsp;
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblExDiffAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATEDIFFERENCE","{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>--%>
                                    <telerik:GridTemplateColumn HeaderText="Charged amount" HeaderStyle-Width="7%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblChargedAmtHeader" runat="server">
                                                Charged amount (<%=strReportCurrencyCode %>) &nbsp;
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblChargedAmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHARGEDAMOUNTINREPORTCURRENCY" ,"{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadLabel ID="lblTotal" runat="server" Text="Total :"></telerik:RadLabel>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Vessel amount" HeaderStyle-Width="7%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                       <%-- <HeaderTemplate>
                                            <telerik:RadLabel ID="lblVesselAmtHeader" runat="server">
                                                Vessel amount  (<%=strTranCurrencyCode %>)&nbsp;
                                            </telerik:RadLabel>
                                        </HeaderTemplate>--%>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblVesselAmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALVESSELAMOUNT" ,"{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <%=dGrandVesselTotal%>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Payable" HeaderStyle-Width="7%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblPayableAmtHeader" runat="server">
                                                Payable (<%=strTranCurrencyCode %>)&nbsp;
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPayableAmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALPAYABLEAMOUNT" ,"{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <%=dGrandPayableTotal%>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="GST Claim amount" HeaderStyle-Width="7%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblGSTClaimAmtHeader" runat="server">
                                                GST Claim amount (<%=strTranCurrencyCode %>)&nbsp;
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblGSTClaimAmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGSTCLAIMAMOUNT" ,"{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <%=dGrandGstTotal %>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Income/ (Expenses)" HeaderStyle-Width="7%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblIncomeAmtHeader" runat="server">
                                                Income/ (Expenses) (<%=strTranCurrencyCode %>)&nbsp;
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblIncomeAmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCOMEEXPENSEAMOUNT" ,"{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <%=dGrandIncomeTotal%>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Rebates Receivable" HeaderStyle-Width="7%">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblRebatesAmtHeader" runat="server">
                                                Rebates Receivable (<%=strTranCurrencyCode %>)&nbsp;
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblRebatesAmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREBATERECEIVABLEAMOUNT" ,"{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <%=dGrandRebateTotal%>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <%-- <telerik:GridTemplateColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblPayingAmtHeader" runat="server">Paying amount &nbsp;
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPayingAmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEPAYINGAMOUNT" ,"{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <%=dGrandPayingTotal%>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblUnpaidAmtHeader" runat="server">Unpaid amount &nbsp;
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblUnpaidAmt" runat="server" Text='0.00'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblInstallmentAmtHeader" runat="server">Next Installment Due Date &nbsp;
                                            </telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>--%>
                                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="5%">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                CommandName="DELETE" CommandArgument='<%# Container.DataItem %>' ID="cmdDelete"
                                                ToolTip="Delete"></asp:ImageButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <NoRecordsTemplate>
                                    <table runat="server" width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="false" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top">
                        <b>
                            <telerik:RadLabel ID="lblLessAdvancePayments" runat="server" Text="Less: Advance Payments"></telerik:RadLabel>
                        </b>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvAdvancePayments" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnNeedDataSource="gvAdvancePayments_NeedDataSource"
                            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                                    <telerik:GridTemplateColumn HeaderText="Advance Payment Voucher Number">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblVoucherCreditNotesDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADVANCEPAYMENTNUMBER") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Bank Payment Voucher - Row">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblBankpaymentvoucherRow" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKPAYMENTVOUCHERNO") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="PO Number">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblVoucherCreditNotesDocumentNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCEDOCUMENT") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Amount">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblVoucherCreditNotesAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="false" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblLessCreditNotesOverpayment" runat="server" Text="Less: Credit Notes/ Overpayment"></telerik:RadLabel>
                        </b>
                        <div class="navSelect">
                            <eluc:TabStrip ID="MenuOrderAdd" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                                TabStrip="true"></eluc:TabStrip>
                        </div>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCreditNotes" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnItemCommand="gvCreditNotes_ItemCommand" OnItemDataBound="gvCreditNotes_ItemDataBound" OnNeedDataSource="gvCreditNotes_NeedDataSource"
                            ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false" AllowSorting="true">
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
                                    <telerik:GridTemplateColumn HeaderText="Cno Register No">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPaymentVOucherId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblCreditNoteId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITNOTEVOUCHERID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblCreditMappingId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITMAPPINGID") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblCreditNoteRegisterNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCNREGISTERNO") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Vendor Credit Note">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblVendorCreditNote" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Credit Note Voucher No">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCreditNoteVoucherNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Amount">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Already Utilized">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAlreadyUtilized" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALREADYUTILIZED","{0:n2}") %>'></telerik:RadLabel>
                                            <asp:LinkButton ID="lnkAmountUtilized" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALREADYUTILIZED","{0:n2}") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Current Utilization">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCurrentUtlizition" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTUTILIZED","{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblCreditMappingIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITMAPPINGID") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadTextBox ID="txtCurrentUtilization" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTUTILIZED","{0:n2}") %>' CssClass="input_mandatory" Width="99%"></telerik:RadTextBox>

                                            <%--    <ajaxtoolkit:maskededitextender id="MaskAmount" runat="server" targetcontrolid="txtCurrentUtilization"
                                            mask="999,999,999.99" masktype="Number" inputdirection="RightToLeft">
                                            </ajaxtoolkit:maskededitextender>--%>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Balance">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblBalance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALANCE","{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                                ToolTip="Edit"></asp:ImageButton>
                                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                CommandName="DELETE" CommandArgument='<%# Container.DataItem %>' ID="cmdDelete"
                                                ToolTip="Delete"></asp:ImageButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                CommandName="SAVE" CommandArgument="<%# Container.DataItem %>" ID="cmdSave"
                                                ToolTip="Save"></asp:ImageButton>
                                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                width="3" />
                                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                CommandName="CANCEL" CommandArgument="<%# Container.DataItem %>" ID="cmdCancel"
                                                ToolTip="Cancel"></asp:ImageButton>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="false" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkCNOverPayPending" runat="server" OnClick="lnkCNOverPayPending_Click" Text="Show List of Credit Note/Overpayment Pending">
                        </asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblListofCreditNoteOverpaymentPending" runat="server" Text="List of Credit Note/Overpayment Pending" Visible="false"></telerik:RadLabel>
                        </b>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCreditNotePending" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvCreditNotePending_NeedDataSource"
                            AllowSorting="true" EnableHeaderContextMenu="true" GroupingEnabled="true" Visible="false">
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
                                    <telerik:GridTemplateColumn HeaderText="Cno Register No">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCnoRegisterNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCNREGISTERNO") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Vendor Credit Note No">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="VendorCreditNote" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENO") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Credit Note Voucher No">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCreditVourcherNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Currency">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Amount">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Total Utlized">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTotalUtilized" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALUTILIZEDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Balance">
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblBalance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALANCEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="false" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                        <br />
                        <br />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
