<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReconcilationStagingRWAForPurchase.aspx.cs"
    Inherits="AccountsReconcilationStagingRWAForPurchase" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TaxType" Src="~/UserControls/UserControlTaxType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagName="UserControlCurrency" TagPrefix="eluc" Src="../UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="../UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselAccount" Src="../UserControls/UserControlMultipleColumnVesselAccountCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Projectcode" Src="~/UserControls/UserControlProjectCode.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
    </telerik:RadCodeBlock>
</head>
<body>

    <form id="frmPurchaseSatging" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" CssClass="hidden" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuLineItem" runat="server" OnTabStripCommand="MenuLineItem_TabStripCommand"></eluc:TabStrip>
            <table id="tbldiv" runat="server" cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <eluc:TabStrip ID="MenuStaging" runat="server" OnTabStripCommand="MenuStaging_TabStripCommand"></eluc:TabStrip>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:ConfirmMessage runat="server" ID="ucConfirmSent" Visible="false" Text="" OnConfirmMesage="ucConfirmSent_OnClick"></eluc:ConfirmMessage>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPONumber" runat="server" Text="PO Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPONumber" runat="server" CssClass="input" ReadOnly="true" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSupplier" runat="server" Text="Supplier"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVendorCode" runat="server" CssClass="input" ReadOnly="true" Width="90px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVenderName" runat="server" CssClass="input" ReadOnly="true" Width="360px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px" CssClass="hidden"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListVessel">
                            <telerik:RadTextBox ID="txtVesselId" runat="server" CssClass="hidden" MaxLength="20"
                                Width="0px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="input" ReadOnly="True" Width="180px"></telerik:RadTextBox>
                            <asp:ImageButton ID="imgShowVessel" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                        </span>
                        <asp:ImageButton runat="server" AlternateText="ChangeVessel" ImageUrl="<%$ PhoenixTheme:images/refresh.png %>"
                            CommandName="VesselUpdate" ID="cmdVesselUpdate" ToolTip="Vessel Update"
                            OnClick="VesselUpdate"></asp:ImageButton>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselAccount ID="ucVesselAccount" runat="server" CssClass="input" Width="300px" />
                        <%--<asp:TextBox ID="ucVesselAccount" runat="server" CssClass="input" Width="300px"></telerik:RadTextBox>--%>
                        <asp:ImageButton runat="server" AlternateText="ChangeVesselAccount" ImageUrl="<%$ PhoenixTheme:images/refresh.png %>"
                            CommandName="VesselAccountUpdate" ID="cmdVesselAccount" ToolTip="Vessel Account Update"
                            OnClick="VesselAccountUpdate"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPOCurrencyPOPayableCurrency" runat="server" Text="PO Currency/PO Payable Currency"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCurrency" runat="server" Width="90px" CssClass="readonlytextbox txtCurreny"
                            ReadOnly="true" Enabled="False">
                        </telerik:RadTextBox>
                        <eluc:UserControlCurrency ID="ucCurrency" runat="server" Enabled="false" Visible="false" />
                        <telerik:RadTextBox ID="txtExchangeRateEdit" runat="server" CssClass="input txtNumber" Wrap="False" Mask="9,999.999999"
                            Width="150px">
                        </telerik:RadTextBox>
                        <%--<eluc:Number ID="txtExchangeRateEdit" runat="server" Width="90px" Mask="9,999.999999" CssClass="input txtNumber" />
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExchangeRate" runat="server" AutoComplete="true"
                                        InputDirection="RightToLeft" Mask="9,999.999999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                        TargetControlID="txtExchangeRateEdit" />--%>
                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/refresh.png %>"
                            CommandName="CurrencyConvert" ID="cmdCurrencyConvert" ToolTip="Currency Convert"
                            OnClick="Convertcurrency"></asp:ImageButton>
                    </td>
                    <td />
                    <td />

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGoodsReceivedDate" runat="server" Text="Goods Received Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReceivedDate" runat="server" CssClass="input" ReadOnly="True"
                            Width="90px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPortName" runat="server" Text="Port Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPortName" runat="server" CssClass="input" ReadOnly="True" Width="360px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceNumber" runat="server" CssClass="readonlytextbox txtInvoiceAmount"
                            ReadOnly="True" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblInvoicePayableAmount" runat="server" Text="Invoice Payable Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInvoiceAmount" runat="server" CssClass="readonlytextbox txtInvoiceAmount"
                            ReadOnly="True" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPOAmount" runat="server" Text="PO Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPOAmount" runat="server" CssClass="readonlytextbox txtBalanceAmount"
                            ReadOnly="True" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPOAmountAccordingtoReceivedGoods" runat="server" Text="PO Amount According to Received Goods"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPOAmountAccordingRecievedGoods" runat="server" CssClass="readonlytextbox txtBalanceAmount"
                            ReadOnly="True" Width="180px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblApproverStatus" runat="server" Text="Approver Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAprooverStatus" runat="server" CssClass="readonlytextbox txtApprovalStatus"
                            ReadOnly="True" Width="180px">
                        </telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblBalanceAmount" runat="server" Text="Balance Amount"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBalanceAmount" runat="server" CssClass="readonlytextbox txtBalanceAmount"
                            ReadOnly="True" Width="180px">
                        </telerik:RadTextBox>
                        <asp:ImageButton runat="server" ID="cmdShowInvoiceDiff" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle"
                            Text=".." />
                    </td>


                </tr>
                <tr>
                    <td>Budget Code
                    </td>
                    <td>
                        <span id="spnPickListBulkBudget">
                            <telerik:RadTextBox ID="txtBulkBudgetCode" runat="server"
                                MaxLength="20" CssClass="input" Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBulkBudgetName" runat="server" Width="0px" CssClass="hidden" Enabled="False"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowBulkBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtBulkBudgetId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBulkBudgetgroupId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td>Owner Budget Code
                    </td>
                    <td>
                        <span id="spnPickListBulkOwnerBudget">
                            <telerik:RadTextBox ID="txtBulkOwnerBudgetCode" runat="server"
                                MaxLength="20" CssClass="input" Width="60px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBulkOwnerBudgetName" runat="server" Width="0px" CssClass="hidden"
                                Enabled="False">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowBulkOwnerBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtBulkOwnerBudgetId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBulkOwnerBudgetgroupId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <asp:ImageButton runat="server" AlternateText="Refresh Budget Codes" ImageUrl="<%$ PhoenixTheme:images/refresh.png %>"
                            CommandName="RefreshBudgetCode" ID="btnRefreshBudgetCode" ToolTip="Refresh Budget Codes"
                            OnClick="RefreshBudgetCode"></asp:ImageButton>
					</td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProjectCode" runat="server" Text="Project Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Projectcode ID="ucProjectcode" runat="server" AppendDataBoundItems="true" AutoPostBack="true" />
                    </td>
                    </td>
					</td>
				<tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuRegistersStockItem" runat="server" OnTabStripCommand="RegistersStockItem_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOrderLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvOrderLine_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" OnSelectedIndexChanging="gvOrderLine_SelectedIndexChanging"
                OnItemDataBound="gvOrderLine_ItemDataBound" OnItemCommand="gvOrderLine_ItemCommand"
                ShowFooter="true" ShowHeader="true" OnSortCommand="gvOrderLine_SortCommand" Height="40%">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <Columns>
                        <telerik:GridTemplateColumn Visible="false">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                    CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Part Number" Visible="false" AllowSorting="true" SortExpression="FLDPARTNUMBER">
                            <HeaderStyle Width="2%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOrderLineId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPartNumber" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNUMBER") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="true" SortExpression="FLDSERIALNO">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblItemSerialNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Line Item" AllowSorting="true" SortExpression="FLDPARTNAME">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <asp:LinkButton ID="lnkStockItemCode" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO Unit Price">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblQuotedPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRICE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtUnitPriceEdit" runat="server" Width="50px" CssClass="gridinput"
                                    Mask="9999999.999" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRICE","{0:n3}") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Received Quantity">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrderQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Original Budget Code">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPOBudgetId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSUBACCOUNT") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Original Owner Code">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOriginalOwnerCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODENAME") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="New Budget Code">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListTaxBudgetEdit">
                                    <telerik:RadTextBox ID="txtBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                        MaxLength="20" CssClass="input" Width="40px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" Width="0px" AutoPostBack="true" OnTextChanged="txtBudgetIdEdit_TextChanged" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODEID") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="New Owner Code">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNewOwnerCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWOWNERBUDGETCODENAME") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOwnerBudgetEdit">
                                    <telerik:RadTextBox ID="txtOwnerBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWOWNERBUDGETCODENAME") %>'
                                        MaxLength="20" CssClass="input" Width="40px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="input"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowOwnerBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtOwnerBudgetIdEdit" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWOWNERBUDGETID") %>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Project Code">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" Font-Bold="true"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProjectCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROJECTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Projectcode ID="ucProjectcodeEdit" runat="server" AppendDataBoundItems="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Discount">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" Font-Bold="true"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOwnerDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERDISCOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtOwnerDiscountEdit" runat="server" Width="40px" CssClass="gridinput"
                                    Mask="9999999.99" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERDISCOUNT","{0:n2}") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Difference">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" Font-Bold="true"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInvoiceDifference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDIFFERENCEAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipRemark" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDIFFERENCEREMARKS") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" Font-Bold="true"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTWITHINVOICEDIFFAMT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblAmountfooter" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTWITHINVOICEDIFFAMT","{0:n2}") %>'></telerik:RadLabel>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Original Cr. Note Discount ">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" Font-Bold="true"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPODiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPODISCOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="New Cr. Note Discount">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" Font-Bold="true"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtDiscountEdit" runat="server" Width="60px" CssClass="gridinput"
                                    Mask="9999999.99" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblDiscountfooter" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount After Discount">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right" Font-Bold="true"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="txtTotalPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNTWITHINVOICEDIFFAMT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <telerik:RadLabel ID="lblTotalAmountfooter" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="35%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Visible="false"></asp:ImageButton>
                                <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="35" />
                                <asp:ImageButton runat="server" AlternateText="Update Discount for All line items"
                                    ImageUrl="<%$ PhoenixTheme:images/update-discount.png%>" CommandName="DISCOUNTUPDATEFORALL"
                                    CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDiscountUpdateForAll"
                                    ToolTip="Update Discount for All line items"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Save" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdUpdate"
                                    ToolTip="Update"></asp:ImageButton>
                                <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <b>
                <telerik:RadLabel ID="lblAdditionalChargeItem" runat="server" Text="Additional Charge Item:"></telerik:RadLabel>
            </b>

            <eluc:TabStrip ID="AdditionalChargeItem" runat="server" OnTabStripCommand="AdditionalChargeItem_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvTax" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvTax_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false"
                OnItemDataBound="gvTax_ItemDataBound" OnItemCommand="gvTax_ItemCommand" Height="50%"
                ShowFooter="true" ShowHeader="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Charge Description" FooterText="Total PO Amount">
                            <HeaderStyle Width="159px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAdditionalDiscountYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDITIONALDISCOUNTYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCRIPTION") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsGst" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISGST") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtTaxMapCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERTAXCODE") %>'></telerik:RadTextBox>
                            </ItemTemplate>
                            <FooterTemplate>
                                <table width="100%">
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <telerik:RadTextBox ID="txtDescriptionAdd" Text='' runat="server" CssClass="input_mandatory"
                                                MaxLength="45">
                                            </telerik:RadTextBox>
                                        </td>
                                    </tr>
                                    <tr class="datagrid_footerstyle">
                                        <td>
                                            <telerik:RadLabel ID="lbl" runat="server" Text='Total' Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" Visible="false">
                            <HeaderStyle Width="57px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAXTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <table width="100%">
                                    <tr class="datagrid_alternatingstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr class="datagrid_footerstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Value">
                            <HeaderStyle Width="54px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE" ,"{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblValueEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE" ,"{0:n2}") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtValueEdit" runat="server" Width="90px" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE" ,"{0:n2}") %>'
                                    Mask="9999999.99" Visible="false" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <table width="100%">
                                    <tr class="datagrid_alternatingstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr class="datagrid_footerstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Original Budget Code">
                            <HeaderStyle Width="75px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPoBudgetId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSUBACCOUNT") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <table width="100%">
                                    <tr class="datagrid_alternatingstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr class="datagrid_footerstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Original Owner Code">
                            <HeaderStyle Width="75px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOriginalOwnerCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERBUDGETCODE") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <table width="100%">
                                    <tr class="datagrid_alternatingstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr class="datagrid_footerstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="New Budget Code">
                            <HeaderStyle Width="105px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblBudgetId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListTaxBudgetEdit">
                                    <telerik:RadTextBox ID="txtBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                        MaxLength="20" CssClass="input_mandatory" Width="55px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="hidden" Enabled="False"></telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" Width="0px" AutoPostBack="true" OnTextChanged="TaxBudgetIdEdit_TextChanged" CssClass="hidden" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETID") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <table width="100%">
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <span id="spnPickListTaxBudget">
                                                <telerik:RadTextBox ID="txtBudgetCodeEdit" runat="server" Width="51px" CssClass="input_mandatory"></telerik:RadTextBox>
                                                <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="hidden" Enabled="False"></telerik:RadTextBox>
                                                <asp:ImageButton ID="btnShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                    ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" />
                                                <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                                <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr class="datagrid_footerstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="New Owner Code">
                            <HeaderStyle Width="105px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNewOwnerCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWOWNERBUDGETCODE") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOwnerBudgetEdit">
                                    <telerik:RadTextBox ID="txtOwnerBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWOWNERBUDGETCODE") %>'
                                        MaxLength="20" CssClass="input_mandatory" Width="55px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="hidden"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowOwnerBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtOwnerBudgetIdEdit" runat="server" Width="0px" CssClass="hidden"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWOWNERBUDGETID") %>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <table width="100%">
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <span id="spnPickListOwnerBudget">
                                                <telerik:RadTextBox ID="txtOwnerBudgetCodeEdit" runat="server" MaxLength="20" CssClass="input_mandatory" Width="51px"></telerik:RadTextBox>
                                                <telerik:RadTextBox ID="txtOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="hidden"
                                                    Enabled="False">
                                                </telerik:RadTextBox>
                                                <asp:ImageButton ID="btnShowOwnerBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                    ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" />
                                                <telerik:RadTextBox ID="txtOwnerBudgetIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                                <telerik:RadTextBox ID="txtOwnerBudgetgroupIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                            </span>
                                        </td>
                                    </tr>
                                    <tr class="datagrid_footerstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Project Code">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProjectCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROJECTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Projectcode ID="ucProjectcodeEdit" runat="server" AppendDataBoundItems="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <table width="100%">
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <eluc:Projectcode ID="ucProjectcodeAdd" runat="server" AppendDataBoundItems="true" />
                                        </td>
                                    </tr>
                                    <tr class="datagrid_footerstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Original Amount">
                            <HeaderStyle Width="65px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOriginalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOLDAMOUNT","{0:n2}") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblOriginalAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOLDAMOUNT","{0:n2}") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <table width="100%">
                                    <tr class="datagrid_alternatingstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr class="datagrid_footerstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="New Amount">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTaxAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsStagingAdded" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSTAGINGADDEDITEM") %>'
                                    CssClass="txtNumber" Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblTaxAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'
                                    CssClass="txtNumber " Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsStagingAddedItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSTAGINGADDEDITEM") %>'
                                    CssClass="txtNumber" Visible="false">
                                </telerik:RadLabel>
                                <%--  <telerik:RadLabel ID="lblAmountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'
                                    CssClass="txtNumber"></telerik:RadLabel>--%>
                                <eluc:Number ID="ucAmountEdit" runat="server" Width="85px" CssClass="gridinput input_mandatory" Mask="9999999.99"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <table width="100%">
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <eluc:Number ID="txtAmount" runat="server" Width="80px" CssClass="input_mandatory"
                                                Mask="9999999.99" />
                                        </td>
                                    </tr>
                                    <tr class="datagrid_footerstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblTaxAmountFooter" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'
                                                Font-Bold="true">
                                            </telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Invoice Difference">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTaxInvoiceDifferenceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDIFFERENCEAMOUNT","{0:n2}") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipTaxRemark" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDIFFERENCEREMARKS") %>' />
                            </ItemTemplate>
                            <FooterTemplate>
                                <table width="100%">
                                    <tr class="datagrid_alternatingstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr class="datagrid_footerstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cr. Note Discount">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPODiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPODISCOUNT","{0:n2}") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <table width="100%">
                                    <tr class="datagrid_alternatingstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr class="datagrid_footerstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="New Cr. Note Discount">
                            <HeaderStyle Width="87px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtDiscountEdit" runat="server" Width="90px" CssClass="gridinput"
                                    Mask="9999999.99" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>' />
                                <telerik:RadLabel ID="lblDiscountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>' Visible="false"></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <table width="100%">
                                    <tr class="datagrid_alternatingstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr class="datagrid_footerstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblTaxDiscountFooter" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>'
                                                Font-Bold="true">
                                            </telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount After Discount">
                            <HeaderStyle Width="74px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTaxTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNTWITHINVOICEDIFFAMT","{0:n2}") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <table width="100%">
                                    <tr class="datagrid_alternatingstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                    <tr class="datagrid_footerstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblTaxTotalFooter" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNTWITHINVOICEDIFFAMT","{0:n2}") %>'
                                                Font-Bold="true">
                                            </telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="80px"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="86px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Visible="false"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Save" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <table width="100%">
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                                CommandName="ADD" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                                ToolTip="Add New"></asp:ImageButton>
                                        </td>
                                    </tr>
                                    <tr class="datagrid_footerstyle">
                                        <td>&nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <b>
                <telerik:RadLabel ID="lblAdditionalDiscount" runat="server" Text="Additional Discount:"></telerik:RadLabel>
            </b>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvAdditionalDiscount" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvAdditionalDiscount_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false"
                OnItemDataBound="gvAdditionalDiscount_ItemDataBound" OnItemCommand="gvAdditionalDiscount_ItemCommand"
                ShowFooter="true" ShowHeader="true" Height="40%">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <Columns>
                        <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                        <telerik:GridTemplateColumn HeaderText="Original Additional Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIsGst" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISGST") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOriginalAdditionlaAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOLDAMOUNT","{0:n2}") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="New Additional Amount">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right"></FooterStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNewAdditionlaAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT","{0:n2}") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucAdditionalAmountEdit" runat="server" Width="90px" CssClass="gridinput"
                                    Mask="9999999.99" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAMOUNT","{0:n2}") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Additional Discount For Vessel">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALVESSELAMOUNT","{0:n2}") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucVesselAmountEdit" runat="server" Width="90px" CssClass="gridinput"
                                    Mask="9999999.99" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALVESSELAMOUNT","{0:n2}") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" Visible="false">
                            <HeaderStyle Width="0%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrderTaxCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERTAXCODE") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOrderId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAXTYPE") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblAdditionalDiscountYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDITIONALDISCOUNTYN") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblBudgetId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMBUDGETID") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPoBudgetId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMBUDGETCODE") %>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListTaxBudgetEdit">
                                    <telerik:RadTextBox ID="txtBudgetCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMBUDGETCODE") %>'
                                        MaxLength="20" CssClass="input" Width="60px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetNameEdit" runat="server" Width="0px" CssClass="input" Enabled="False"></telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtBudgetIdEdit" runat="server" AutoPostBack="true" OnTextChanged="AdditionalBudgetIdEdit_TextChanged" Width="0px" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMBUDGETID") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Owner Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOriginalOwnerCode" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDNEWOWNERBUDGETCODE").ToString().Equals("")) ? DataBinder.Eval(Container, "DataItem.FLDOWNERBUDGETCODE") : DataBinder.Eval(Container, "DataItem.FLDNEWOWNERBUDGETCODE")%>'
                                    CssClass="txtNumber">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <span id="spnPickListOwnerBudgetEdit">
                                    <telerik:RadTextBox ID="txtOwnerBudgetCodeEdit" runat="server" Text='<%# (DataBinder.Eval(Container, "DataItem.FLDNEWOWNERBUDGETCODE").ToString().Equals("")) ? DataBinder.Eval(Container, "DataItem.FLDOWNERBUDGETCODE") : DataBinder.Eval(Container, "DataItem.FLDNEWOWNERBUDGETCODE")%>'
                                        MaxLength="20" CssClass="input" Width="60px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetNameEdit" runat="server" Width="0px" CssClass="input"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="btnShowOwnerBudgetEdit" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtOwnerBudgetIdEdit" runat="server" Width="0px" CssClass="input"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWOWNERBUDGETID") %>'>
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtOwnerBudgetgroupIdEdit" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Project Code">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <FooterStyle Wrap="False" HorizontalAlign="Right"></FooterStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProjectCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROJECTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Projectcode ID="ucProjectcodeEdit" runat="server" AppendDataBoundItems="true" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Visible="false"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Save" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
            <asp:Button ID="confirm" runat="server" CssClass="hidden" Text="confirm" OnClick="confirm_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>



