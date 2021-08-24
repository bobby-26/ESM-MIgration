<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCtmPaymentVoucherLineItemDetails.aspx.cs"
    Inherits="AccountsCtmPaymentVoucherLineItemDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Ctm Payment VoucherLine ItemDetails</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
            <eluc:Title runat="server" ID="Title1" Text="Invoice Payment Voucher Details" Visible="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden"/>
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuRevoke" runat="server" OnTabStripCommand="MenuRevoke_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" style="width: 100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVoucherNumber" runat="server" Text="Voucher Number"></telerik:RadLabel>
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
                        <telerik:RadLabel ID="lblPortAgentVesselSupplier" runat="server" Text="Payee"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListVendor">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVendorName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="200" Width="240px">
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
                        <telerik:RadTextBox runat="server" ID="txtRevokeRemarks" CssClass="input" TextMode="MultiLine" Resize="Both"
                            Rows="3" Width="300px">
                        </telerik:RadTextBox>
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
                        <%--  <ajaxtoolkit:maskededitextender id="MaskNumber" runat="server" targetcontrolid="txtAmount"
                            mask="999,999,999.99" masktype="Number" inputdirection="RightToLeft">
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
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCTM" runat="server" AutoGenerateColumns="False" Font-Size="11px" EnableHeaderContextMenu="true" GroupingEnabled="false"
                Width="100%" CellPadding="3" OnItemDataBound="gvCTM_ItemDataBound" ShowHeader="true" OnNeedDataSource="gvCTM_NeedDataSource" AllowPaging="true" AllowCustomPaging="true"
                EnableViewState="false" OnItemCommand="gvCTM_ItemCommand">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCAPTAINCASHID">
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
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCaptainCashId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDCAPTAINCASHID"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblvesselname" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELNAME"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reference No" HeaderStyle-Width="10%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkReferenceNumber" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTNUMBER"]%>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="EDA" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbleda" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDETA"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Port" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSeaPort" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSEAPORTNAME"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <HeaderTemplate>
                                    Company
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCompany" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCOMPANYNAME"]%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Arranged Via" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrangedVia" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDARRANGEDVIA"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                <HeaderTemplate>
                                    Delivered By
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDeliveredBy" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDELIVEREDBY"]%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Arranged Amount" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblArrangedAmount" runat="server" Text='<%# String.Format("{0:n2}", ((DataRowView)Container.DataItem)["FLDAMOUNTARRANGED"]) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Charges" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCharges" runat="server" Text='<%# String.Format("{0:n2}", ((DataRowView)Container.DataItem)["FLDTOTALCHARGES"])%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Income" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHandlingFees" runat="server" Text='<%# String.Format("{0:n2}", ((DataRowView)Container.DataItem)["FLDHANDLINGFEESAMT"])%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payable Amount" HeaderStyle-Width="8%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPayableAmount" runat="server" Text='<%# String.Format("{0:n2}", ((DataRowView)Container.DataItem)["FLDREMITTANCEAMOUNT"])%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="8%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataItem %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
