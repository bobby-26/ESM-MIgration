<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceLedgerPostingConfirmation.aspx.cs"
    Inherits="AccountsInvoiceLedgerPostingConfirmation" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <style type="text/css">
            .style1 {
                height: 20px;
            }
        </style>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPurchaseFormItemDetails" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="MenuLineItem" runat="server" OnTabStripCommand="MenuLineItem_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="1" style="width: 100%">
                <tr>
                    <td colspan="4">
                        <asp:HyperLink ID="HlinkRefDuplicate" runat="server" Text="Vendor Invoice Number already exists for this Supplier. Click here to view the Invoice List "
                            ToolTip="Attachments" Visible="False" Font-Bold="False" Font-Size="Large" Font-Underline="True"
                            ForeColor="Red" BorderColor="Red"></asp:HyperLink>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoicenumber" runat="server" Text="Invoice number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" MaxLength="25" ReadOnly="true"
                            CssClass="readonlytextbox" Width="150px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPostDate" runat="server" Text="Post Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="txtInvoicePostdate" runat="server" CssClass="input_mandatory"
                            Width="150px" />
                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/refresh.png %>"
                            OnClick="GetExchangeRate" CommandName="EDIT" ID="cmdEdit" ToolTip="Refresh Exchange Rate"></asp:ImageButton>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPurchaseInvoiceVoucherNumber" runat="server" Text="Purchase Invoice Voucher Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPurchaseInvoiceVoucherNumber" runat="server" CssClass="readonlytextbox"
                            Width="150px" ReadOnly="true" MaxLength="50">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCurrency" runat="server" CssClass="readonlytextbox" Width="150px"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBaseExchangeRate" runat="server" Text="Base Exchange Rate"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBaseexchangerate" RenderMode="Lightweight" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATE") %>'></telerik:RadTextBox>
                        <%--<eluc:Number ID="txtBaseexchangerate" runat="server" Width="150px" CssClass="input_mandatory"
                            Mask="99999.99999999999999999" />--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportExchangeRate" runat="server" Text="Report Exchange Rate"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReportexchangerate" RenderMode="Lightweight" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXCHANGERATE") %>'></telerik:RadTextBox>
                        <%-- <eluc:Number ID="txtReportexchangerate" runat="server" Width="150px" CssClass="input_mandatory"
                            Mask="99999.99999999999999999" />--%>
                    </td>
                </tr>
            </table>
            <br />
            <telerik:RadLabel ID="lblCommondescription" runat="server" Text="Description" Visible="false"></telerik:RadLabel>
            <telerik:RadTextBox ID="txtCommondescription" runat="server" CssClass="input" Visible="false" />
            <asp:ImageButton runat="server" ImageUrl="<%$ PhoenixTheme:images/refresh.png %>"
                OnClick="CommondescriptionUpdate" ID="Imgrefresh" Visible="false"></asp:ImageButton>
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOrderLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvOrderLine_NeedDataSource" Height="20%"
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
                        <telerik:GridTemplateColumn Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                    CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELACCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Amount">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" Font-Bold="true"></FooterStyle>

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
            <telerik:RadCodeBlock runat="server" ID="divTable">
                <table cellspacing="0" cellpadding="3" rules="all" border="1" id="Table1" style="font-size: 11px; width: 100%; border-collapse: collapse;">
                    <tr class="DataGrid-HeaderStyle">
                        <th scope="col" class="style1">
                            <span id="gvOrderLine_ctl01_lblVesselAccountHeader"></span>
                        </th>
                        <th scope="col" colspan="4" class="style1">
                            <span id="gvOrderLine_ctl01_lblBudgetCodeHeader" style="color: White;">
                                <telerik:RadLabel ID="lblSupplierCodeCaption" runat="server" Text="Supplier Code"></telerik:RadLabel>
                            </span>
                        </th>
                        <th scope="col" class="style1" align="right">
                            <span id="gvOrderLine_ctl01_lblVesselAmountHeader">
                                <telerik:RadLabel ID="lblPayableAmount" runat="server" Text="Payable Amount"></telerik:RadLabel>
                            </span>
                        </th>
                    </tr>
                    <tr style="height: 10px;">
                        <td align="left" style="white-space: nowrap;">
                            <span id="gvOrderLine_ctl02_lblVesselAccount">
                                <%=strCreditorAccountDetails%></span>
                        </td>
                        <td align="left" style="white-space: nowrap;" colspan="4">
                            <span id="gvOrderLine_ctl02_lblBudgetCode">
                                <%=strSupplierCode%></span>
                        </td>
                        <td align="right" style="white-space: nowrap;">
                            <span id="gvOrderLine_ctl02_lblVesselAmount">
                                <%  if (dPayableamount != 0) //BUG ID - 27704
                                    { %>
                                                -  <%= strPayableAmount %>
                                <%} %>


                                <%  if (dPayableamount == 0) //BUG ID - 27704
                                    { %>
                                <%= strPayableAmount %>
                                <%} %>
                                              
                            </span>
                        </td>
                    </tr>
                    <tr class="DataGrid-HeaderStyle">
                        <th scope="col" colspan="5">
                            <span id="Span1"></span>
                        </th>
                        <th scope="col" align="right">
                            <span id="Span6">
                                <telerik:RadLabel ID="lblIncomeAmount" runat="server" Text="Income Amount"></telerik:RadLabel>
                            </span>
                        </th>
                    </tr>
                    <tr style="height: 10px;">
                        <td align="left" style="white-space: nowrap;" colspan="5">
                            <span id="Span7">
                                <%=strIncomeAccountDetails%></span>
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
                        <th scope="col" colspan="4" align="right">
                            <span id="Span14" style="color: White;">
                                <telerik:RadLabel ID="lblGSTonInvoice" runat="server" Text="GST on Invoice"></telerik:RadLabel>
                            </span>
                        </th>
                        <th scope="col" align="right">
                            <span id="Span15">
                                <telerik:RadLabel ID="lblGSTAmount" runat="server" Text="GST Amount"></telerik:RadLabel>
                            </span>
                        </th>
                    </tr>
                    <tr style="height: 10px;">
                        <td align="left" style="white-space: nowrap;" colspan="5">
                            <span id="Span19">
                                <%=strGSTClaimAccountDetails%></span>
                        </td>
                        <td align="right" style="white-space: nowrap;">
                            <span id="Span24">
                                <%=strGstAmount%></span>
                        </td>
                    </tr>
                    <tr class="DataGrid-HeaderStyle">
                        <th scope="col">
                            <span id="Span25"></span>
                        </th>
                        <th scope="col" colspan="4">
                            <span id="Span26" style="color: White;">
                                <telerik:RadLabel ID="lblSupplierCode" runat="server" Text="Supplier Code"></telerik:RadLabel>
                            </span>
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
                        <td align="left" style="white-space: nowrap;" colspan="4">
                            <span id="Span32">
                                <%=strSupplierCode%></span>
                        </td>
                        <td align="right" style="white-space: nowrap;">
                            <span id="Span36">
                                <%=strRebateReceivableAmount%></span>
                        </td>
                    </tr>
                    <tr class="DataGrid-HeaderStyle">
                        <th scope="col" align="right">
                            <span id="Span37">
                                <telerik:RadLabel ID="lblTDSPayable" runat="server" Text="TDS Payable"></telerik:RadLabel>
                            </span>
                        </th>
                        <th scope="col" colspan="4" align="right">
                            <span id="Span38">
                                <telerik:RadLabel ID="lblServicePayable" runat="server" Text="Service Tax Payable"></telerik:RadLabel>
                            </span>
                        </th>
                        <th scope="col" align="right">
                            <span id="Span39">
                                <telerik:RadLabel ID="lblWCTPayable" runat="server" Text="WCT Payable"></telerik:RadLabel>
                            </span>
                        </th>
                    </tr>
                    <tr style="height: 10px;">
                        <td align="right" style="white-space: nowrap;">
                            <span id="Span40">
                                <%=strTDSPayable%></span>
                        </td>
                        <td align="right" style="white-space: nowrap;" colspan="4">
                            <span id="Span41">
                                <%=strServiceTaxPayable%></span>
                        </td>
                        <td align="right" style="white-space: nowrap;">
                            <span id="Span42">
                                <%=strWCTPayable%></span>
                        </td>
                    </tr>
                </table>
            </telerik:RadCodeBlock>
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvLineItem" Height="40%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvLineItem_NeedDataSource" OnItemDataBound="gvLineItem_ItemDataBound" OnItemCommand="gvLineItem_ItemCommand"
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
                        <telerik:GridTemplateColumn HeaderText="Row Number">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkVoucherLineItemNo" runat="server" CommandArgument="<%# (Container.DataSetIndex)%>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMNO") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVoucherLineId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAccountUsage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTUSAGENAME") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Description">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub Account Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudget" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBudgetId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="12%"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERACCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Transaction Currency">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Prime Amount">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPrimeAmount" runat="server" Text="Prime Amount"></telerik:RadLabel>
                                (
                                    <%=strTransactionAmountTotal%>
                                    )
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTranAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRANSACTIONAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <telerik:RadLabel ID="lblDebitsTotal" runat="server" Text="Debits Total"></telerik:RadLabel>
                                </b>
                                <br />
                                <b>
                                    <telerik:RadLabel ID="lblCreditsTotal" runat="server" Text="Credits Total :"></telerik:RadLabel>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Base Amount">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblBaseAmount" runat="server" Text="Base Amount"></telerik:RadLabel>
                                (
                                    <%=strBaseAmountTotal%>
                                    )
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBaseAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <%=strBaseDebitTotal%>
                                </b>
                                <br />
                                <b>
                                    <%=strBaseCrebitTotal%>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <HeaderStyle HorizontalAlign="Right" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblReportAmount" runat="server" Text="Report Amount"></telerik:RadLabel>
                                (
                                    <%=strReportAmountTotal%>
                                    )
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReportAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <b>
                                    <%=strReportDebitTotal%>
                                </b>
                                <br />
                                <b>
                                    <%=strReportCrebitTotal%>
                                </b>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Description">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDescription" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION")%>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton ID="cmdSave" runat="server" AlternateText="Save" CommandArgument="<%# Container.DataSetIndex %>"
                                    CommandName="Save" ImageUrl="<%$ PhoenixTheme:images/save.png%>" ToolTip="Save" />
                                <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
