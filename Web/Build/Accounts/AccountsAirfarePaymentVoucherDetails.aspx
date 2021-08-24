<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAirfarePaymentVoucherDetails.aspx.cs" Inherits="Accounts_AccountsAirfarePaymentVoucherDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice Payment VoucherLine ItemDetails</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAirfareFormItemDetails" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Title runat="server" ID="Title1" Text="Invoice Payment Voucher Details" Visible="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                CssClass="hidden" />
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand" Title="Invoice Payment VoucherLine ItemDetails"
                TabStrip="true"></eluc:TabStrip>
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
                        <telerik:RadTextBox ID="txtAccountNumber" runat="server" CssClass="readonlytextbox" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lblBankdetails" runat="server" Text="Banking Details"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBankdetails" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlCurrency ID="ddlCurrency" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                            AppendDataBoundItems="true" Enabled="false" runat="server" CssClass="readonlytextbox" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPayableAmount" runat="server" Text="Payable Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadTextBox ID="txtAmount" runat="server" MaxLength="50" ReadOnly="true" Width="120px"
                                CssClass="readonlytextbox"></telerik:RadTextBox>
                            <ajaxToolkit:MaskedEditExtender ID="MaskNumber" runat="server" TargetControlID="txtAmount"
                                Mask="999,999,999.99" MaskType="Number" InputDirection="RightToLeft">
                            </ajaxToolkit:MaskedEditExtender>--%>
                        <eluc:Number ID="txtAmount" runat="server" ReadOnly="true" Width="240px" CssClass="readonlytextbox" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvVessel" runat="server" AutoGenerateColumns="False" Font-Size="11px" EnableHeaderContextMenu="true" GroupingEnabled="false"
                Width="100%" CellPadding="3" OnItemDataBound="gvVessel_ItemDataBound" OnItemCommand="gvVessel_ItemCommand"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" AllowSorting="true" OnNeedDataSource="gvVessel_NeedDataSource">
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
                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lblVessel" runat="server" CommandArgument='<%# Container.DataItem %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCancelledyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCANCELLEDYN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblNonVesselyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNONVESSELYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT") %>'></telerik:RadLabel>
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
            <b>
                <telerik:RadLabel ID="lblLessCreditNotesOverpayment" runat="server" Text="Less: Credit Notes/ Overpayment"></telerik:RadLabel>
            </b>
            <eluc:TabStrip ID="MenuOrderAdd" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand" Visible="false"
                TabStrip="true"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCreditNotes" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvCreditNotes_ItemCommand" OnItemDataBound="gvCreditNotes_ItemDataBound"
               ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false"
               OnNeedDataSource="gvCreditNotes_NeedDataSource"  AllowSorting="true">
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
                                    Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCreditNoteId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITNOTEVOUCHERID") %>'
                                    Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCreditMappingId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITMAPPINGID") %>'
                                    Visible="false"></telerik:RadLabel>
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
                                <telerik:RadLabel ID="lblAlreadyUtilized" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDALREADYUTILIZED","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Current Utilization">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrentUtlizition" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTUTILIZED","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCreditMappingIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITMAPPINGID") %>'
                                    Visible="false"></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtCurrentUtilization" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTUTILIZED","{0:n2}") %>'
                                    CssClass="input_mandatory">
                                </telerik:RadTextBox>
                                <%--   <ajaxtoolkit:maskededitextender id="MaskAmount" runat="server" targetcontrolid="txtCurrentUtilization"
                                mask="999,999,999.99" masktype="Number" inputdirection="RightToLeft">
                                    </ajaxtoolkit:maskededitextender>--%>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Balance">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBalance" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALANCE","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
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
                    <Scrolling AllowScroll="false" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <b>
                <telerik:RadLabel ID="lblListofCreditNoteOverpaymentPending" runat="server" Text="List of Credit Note/Overpayment Pending"></telerik:RadLabel>
            </b>
            <br />
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShowZeroAmount" runat="server" Text="Show Zero Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkZeroAmount" runat="server" AutoPostBack="true" OnCheckedChanged="CheckZeroAmount" />
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCreditNotePending" OnItemCommand="gvCreditNotePending_ItemCommand"
                OnItemCreated="gvCreditNotePending_ItemCreated" runat="server" AutoGenerateColumns="False"
                Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false"
                AllowSorting="true" OnNeedDataSource="gvCreditNotePending_NeedDataSource">
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
                                <telerik:RadLabel ID="lblairfareCreditId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAIRFARECREDITNOTEID") %>'></telerik:RadLabel>
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
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Add" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="ADD" CommandArgument="<%# Container.DataItem %>" ID="cmdAdd"
                                    ToolTip="Add"></asp:ImageButton>
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
