<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotation.aspx.cs" Inherits="PurchaseQuotation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TaxType" Src="~/UserControls/UserControlTaxType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OwnerBudgetCode" Src="~/UserControls/UserControlOwnerBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ApprovalBy" Src="~/UserControls/UserControlMultiColumnApprovalBy.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Quotation</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmQuotationVendor" runat="server" autocomplete="off">
        <%--<script type="text/javascript">
            function openCreditNoteDiscount() {
                var quotationid = '<%= ViewState["quotationid"] == null ? "" : ViewState["quotationid"].ToString() %>';
                var txtDiscount = '<%=txtDiscount.ClientID %>';
                openpopup('codehelp1', '', '../Purchase/PurchaseQuotationEsmDiscount.aspx?quotationid=' + quotationid + '&discount=' + document.getElementById(txtDiscount).value, true);
            }
        </script>--%>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <eluc:Title runat="server" ID="Title1" Text="Quotation" ShowMenu="false"></eluc:Title>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuVendor" runat="server" OnTabStripCommand="MenuVendor_TabStripCommand"></eluc:TabStrip>
        </div>
        <br clear="all" />
        <asp:UpdatePanel runat="server" ID="pnlFormGeneral">
            <ContentTemplate>
                <div class="navigation" id="Div2" style="top: 30px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Status runat="server" ID="ucStatus" />
                    <eluc:Error ID="Error1" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td>
                                <asp:Literal ID="lblVendor" runat="server" Text="Vendor"></asp:Literal>
                            </td>
                            <td>
                                <span id="spnPickListMaker">
                                    <asp:TextBox ID="txtVendorCode" ReadOnly="true" runat="server" Width="60px" CssClass="readonlytextbox"></asp:TextBox>
                                    <asp:TextBox ID="txtVendorName" ReadOnly="true" runat="server" BorderWidth="1px" Width="180px" CssClass="readonlytextbox"></asp:TextBox>
                                    <asp:TextBox ID="txtVendorID" runat="server" Visible="false" CssClass="input"></asp:TextBox>
                                </span>
                                <asp:TextBox ID="txtVendorReference" runat="server" Width="90px" MaxLength="50" Visible="false"
                                    CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblQuotationRef" runat="server" Text="Reference"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtQtnRefenceno" runat="server" Width="90px" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblReceivedDate" runat="server" Text="Received Date"></asp:Literal>
                            </td>
                            <td class="style1">
                                <asp:TextBox ID="txtRecivedDate" runat="server" Width="90px" CssClass="readonlytextbox"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="ceRecivedDate" runat="server" Format="dd/MMM/yyyy"
                                    Enabled="True" TargetControlID="txtRecivedDate" PopupPosition="TopLeft">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblapprovalby" runat="server" Text="Approval By"></asp:Literal>
                            </td>
                            <td>
                                <table>
                                    <tr>
                                        <td>
                                            <eluc:ApprovalBy ID="ucApprovalBy" runat="server" CssClass="input" Width="200px" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="cmdMap" runat="server" ImageUrl="<%$ PhoenixTheme:images/Modify.png %>" Style="cursor: pointer; margin-top: -0.5%; margin-left: -3%; position: absolute;"
                                                ImageAlign="AbsMiddle" Text=".." OnClick="cmdMapUser_Click" ToolTip="Map Approval BY" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:Literal ID="lblType" runat="server" Text="Items Type"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Hard ID="ddlType" runat="server" AppendDataBoundItems="true" CssClass="input" HardTypeCode="244" />
                            </td>
                            <td>
                                <asp:Label ID="lblPartPaid" runat="server">  Part Paid  </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPartPaid" runat="server" Width="100px" CssClass="readonlytextbox" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblExpirydate" runat="server" Text="Expiry date"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtExpirationDate" runat="server" Width="90px" CssClass="input"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="ceExpirationDate" runat="server" Format="dd/MMM/yyyy"
                                    Enabled="True" TargetControlID="txtExpirationDate" PopupPosition="TopLeft">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td>
                                <asp:Literal ID="lblSentDate" runat="server" Text="Sent Date"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSentDate" runat="server" Width="90px" CssClass="readonlytextbox" Enabled="false"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="ceSentDate" runat="server" Format="dd/MMM/yyyy"
                                    Enabled="True" TargetControlID="txtSentDate" PopupPosition="TopLeft">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td>
                                <asp:Literal ID="lblRejectedDate" runat="server" Text="Rejected Date"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRejectedDate" runat="server" Width="90px" CssClass="readonlytextbox" Enabled="false"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="ceRejectedDate" runat="server" Format="dd/MMM/yyyy"
                                    Enabled="True" TargetControlID="txtRejectedDate" PopupPosition="TopLeft">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblQuotationValidUntil" runat="server" Text="Valid Until"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderDate" runat="server" Width="90px" CssClass="readonlytextbox" Enabled="false"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="ceOrderDate" runat="server" Format="dd/MMM/yyyy"
                                    Enabled="True" TargetControlID="txtOrderDate" PopupPosition="TopLeft">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td>
                                <asp:Literal ID="lblSentBy" runat="server" Text="Sent By"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNameSentBy" runat="server" CssClass="readonlytextbox" Width="40%"
                                    Enabled="false"></asp:TextBox>
                                <asp:TextBox ID="txtSentById" runat="server" CssClass="input"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblRejectedBy" runat="server" Text="Rejected By"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNameRejectedBy" runat="server" CssClass="readonlytextbox" Width="40%"
                                    Enabled="false"></asp:TextBox>
                                <asp:TextBox ID="txtRejectedById" runat="server" CssClass="input"></asp:TextBox>
                                <eluc:Decimal ID="ucTotalAmount" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Visible="false" Width="90px" />
                                <eluc:Decimal ID="txtTotalInUSD" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Visible="false" Width="90px" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblQuotedCurrency" runat="server" Text="Quoted Currency"></asp:Literal>
                            </td>
                            <td>
                                <eluc:UserControlCurrency ID="ucCurrency" AppendDataBoundItems="true" Enabled="false" CssClass="readonlytextbox"
                                    runat="server" />
                                /
                            <asp:TextBox ID="lblExchangeRate" runat="server" Text="" Width="90px" ReadOnly="true"
                                CssClass="readonlytextbox txtNumber"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblRateTotalPrice" runat="server" Text="Total Price"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Decimal ID="txtRate" runat="server" Width="120px" ReadOnly="true" CssClass="readonlytextbox" />
                            </td>
                            <td>
                                <%--Total Discount--%>
                                <asp:Label ID="lblTotalDisc" runat="server" Text="Total Discount"></asp:Label>
                            </td>
                            <td>
                                <eluc:Decimal ID="ucTotalDiscount" runat="server" Width="90px" ReadOnly="true" CssClass="readonlytextbox" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="lblUSD" runat="server" Text="USD / 1.000000"></asp:Literal>
                            </td>
                            <td>
                                <asp:Literal ID="lblUsdTotalPrice" runat="server" Text="Total Price"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Decimal ID="txtUsdPrice" runat="server" Width="120px" ReadOnly="true" CssClass="readonlytextbox" />
                            </td>
                            <td>
                                <%--Total Discount--%>
                                <asp:Label ID="lblUSDTotalDisc" runat="server" Text="Total Discount"></asp:Label>
                            </td>
                            <td>
                                <eluc:Decimal ID="txtTotalDiscount" runat="server" Width="90px" ReadOnly="true" CssClass="readonlytextbox" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblDiscount" runat="server" Text="Discount %"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSupplierDiscount" runat="server" Width="90px" CssClass="input txtNumber"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblDelTimeDays" runat="server" Text="Del. Time(Days)"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDeliveryTime" runat="server" Width="90px" CssClass="input txtNumber"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="meeDeliveryTime" runat="server" TargetControlID="txtDeliveryTime"
                                    Mask="999" MaskType="Number" InputDirection="RightToLeft">
                                </ajaxToolkit:MaskedEditExtender>
                            </td>
                            <td>
                                <asp:Label ID="lblCreditNoteDisc" runat="server" Text="Credit Note Discount %"></asp:Label>
                            </td>
                            <td class="style1">
                                <span id="spnDiscount">
                                    <asp:TextBox ID="txtDiscount" runat="server" Width="90px" CssClass="input txtNumber"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="meeDiscount" runat="server" TargetControlID="txtDiscount"
                                        Mask="99.9999999" MaskType="Number" InputDirection="RightToLeft">
                                    </ajaxToolkit:MaskedEditExtender>
                                    <asp:ImageButton ID="cmdDiscount" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." OnClick="cmdDiscount_Click" />
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblDeliveryTerms" runat="server" Text="Delivery Terms"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Quick ID="UCDeliveryTerms" AppendDataBoundItems="true" CssClass="input" runat="server" />
                            </td>
                            <td>
                                <asp:Literal ID="lblPaymentTerms" runat="server" Text="Payment Terms"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Quick ID="UCPaymentTerms" AppendDataBoundItems="true" CssClass="input" runat="server" />
                            </td>
                            <td>
                                <asp:Literal ID="lblModeofTransport" runat="server" Text="Mode of Transport"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Quick runat="server" ID="ucModeOfTransport" CssClass="input" QuickTypeCode="77"
                                    AppendDataBoundItems="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblEmailIDsCaption" runat="server" Text="Email IDs"></asp:Literal>
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="lblEmailIds" runat="server" CssClass="input" Width="800px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblVesselAccount" runat="server" Text="Vessel Account " Visible="false"></asp:Literal>
                            </td>
                            <td colspan="5">
                                <asp:DropDownList ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory"
                                    OnDataBound="ddlAccountDetails_DataBound" DataTextField="FLDVESSELACCOUNTDESCRIPTION"
                                    DataValueField="FLDVESSELACCOUNTID" OnSelectedIndexChanged="ddlAccountDetails_SelectedIndexChanged"
                                    AutoPostBack="true" Visible="false">
                                </asp:DropDownList>
                            </td>
                        </tr>

                        <tr>
                            <td colspan="6">
                                <asp:HiddenField ID="hdnprincipalId" runat="server" />
                            </td>
                        </tr>
                    </table>

                    <br clear="all" />

                    <asp:GridView ID="gvTax" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                        Width="100%" CellPadding="3" OnRowCommand="gvTax_RowCommand" OnRowCancelingEdit="gvTax_RowCancelingEdit"
                        OnRowEditing="gvTax_RowEditing" OnRowDeleting="gvTax_RowDeleting" OnRowDataBound="gvTax_RowDataBound"
                        ShowFooter="true" ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField FooterText="New MaritalStatus">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTaxNameHeader" runat="server">Delivery/Tax/Others Charge Description&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                        CssClass="gridinput_mandatory" MaxLength="50"></asp:TextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="txtDescriptionAdd" Text='' runat="server" CssClass="gridinput_mandatory"
                                        MaxLength="50"></asp:TextBox>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTypeHeader" runat="server">Type</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAXTYPENAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:TaxType ID="ucTaxTypeEdit" runat="server" TaxType='<%# DataBinder.Eval(Container,"DataItem.FLDTAXTYPE") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:TaxType ID="ucTaxTypeAdd" runat="server" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblValueHeader" runat="server">Value</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblValue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE" ,"{0:f2}") %>'></asp:Label>
                                    <asp:TextBox ID="txtTaxMapCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONTAXMAPCODE") %>'></asp:TextBox>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtTaxMapCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONTAXMAPCODE") %>'></asp:TextBox>
                                    <asp:TextBox ID="txtValueEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUE","{0:f2}") %>'
                                        CssClass="gridinput_mandatory txtNumber" MaxLength="200"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="MaskedEditTotalPayableAmout" runat="server" AutoComplete="false"
                                        InputDirection="RightToLeft" Mask="99,99,99,999.99" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                        TargetControlID="txtValueEdit" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Decimal ID="txtValueAdd" runat="server" CssClass="gridinput_mandatory" Mask="999,999.99"
                                        Width="90px" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTaxAmountHeader" runat="server">Amount</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTaxAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTAXAMOUNT","{0:f2}") %>'
                                        CssClass="txtNumber"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblBudgetIdHeader" runat="server">Budget Code</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'
                                        CssClass="txtNumber"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:BudgetCode ID="ucBudgetCodeEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucBudgetCode_TextChangedEvent" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:BudgetCode ID="ucBudgetCode" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucBudgetCode_TextChangedEvent" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblBudgetIdHeader" runat="server">Owner Budget</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOwnerBudgetId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERACCOUNT") %>'
                                        CssClass="txtNumber"></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:OwnerBudgetCode ID="ucOwnerBudgetCodeEdit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:OwnerBudgetCode ID="ucOwnerBudgetCode" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server" Text="Action" />
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Save" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <br clear="all" />

                    <div class="navSelect* " style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuVendorList" runat="server" OnTabStripCommand="MenuVendorList_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%; overflow: scroll">
                        <asp:GridView ID="gvVendor" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCommand="gvVendor_RowCommand" OnRowDataBound="gvVendor_RowDataBound"
                            EnableViewState="False" AllowSorting="true" OnSorting="gvVendor_Sorting" OnRowDeleting="gvVendor_RowDeleting" DataKeyNames="FLDQUOTATIONID">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblImageHeader" runat="server"> </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkSelect" Checked="false" runat="server" EnableViewState="true" />
                                        <asp:ImageButton ID="imgFlag" runat="server" Enabled="false" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" />
                                        <asp:Label ID="lblIsSelected" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSELECTEDFORORDER") %>'></asp:Label>
                                        <asp:Label ID="lblIsApproved" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.APPOVEDSTATUS") %>'></asp:Label>
                                        <asp:ImageButton ID="imgRemarks" runat="server" Visible="false" ImageUrl="<%$ PhoenixTheme:images/te_view.png %>" />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="number">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblVendorHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                            ForeColor="White">Vendor Name</asp:LinkButton>
                                        <img id="FLDNAME" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblQuotationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONID") %>'></asp:Label>
                                        <asp:Label ID="lblApprovalExists" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVALEXISTS") %>'></asp:Label>
                                        <asp:Label ID="lblVendorId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORID") %>'></asp:Label>
                                        <asp:Label ID="lblFormType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPE") %>'></asp:Label>
                                        <asp:Label ID="lblWebSession" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWEBSESSIONSTATUS") %>'></asp:Label>
                                        <asp:Label ID="lblDtKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></asp:Label>
                                        <asp:Label ID="lblActiveCur" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVEYN") %>'></asp:Label>
                                        <asp:Label ID="lblApprovalType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONAPPROVAL") %>'></asp:Label>
                                        <asp:Label ID="lblTechdirector" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTECHDIRECTOR") %>'></asp:Label>
                                        <asp:Label ID="lblFleetManager" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLEETMANAGER") %>'></asp:Label>
                                        <asp:Label ID="lblSupdt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPT") %>'></asp:Label>
                                        <asp:LinkButton ID="lnkVendorName" runat="server" OnCommand="onPurchaseQuotation" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                            CommandArgument='<%# Container.DataItemIndex %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME").ToString().Length>15 ? DataBinder.Eval(Container, "DataItem.FLDNAME").ToString().Substring(0, 15) + "..." : DataBinder.Eval(Container, "DataItem.FLDNAME").ToString() %>'></asp:LinkButton>
                                        <eluc:ToolTip ID="ucCommentsToolTip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKONVENDOR") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="StockItem Name" Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblSendDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDSENTDATE"
                                            ForeColor="White">Sent Date</asp:LinkButton>
                                        <img id="FLDSENTDATE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSendDateCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSENTDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Maker">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblReceivedDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDRECEIVEDDATE"
                                            ForeColor="White">Received Date</asp:LinkButton>
                                        <img id="FLDRECEIVEDDATE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRecivedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="PreferredVendor" Visible="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblRejectedDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDREJECTEDDATE"
                                            ForeColor="White">Rejected Date</asp:LinkButton>
                                        <img id="FLDREJECTEDDATE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRejectDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREJECTEDDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Amount">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblAmountHeader" runat="server">Amount
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALPRICE","{0:n2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblDiscountHeader" runat="server">Discount
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDisc" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALDISCOUNT","{0:n2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTotalAmountHeader" runat="server">Total Amount                               
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TOTALAMOUNT","{0:n2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Time">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTimeHeader" runat="server">Del. Time(Days)
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVERYTIME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Time">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblSTATUSHeader" runat="server">Quoted
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSTATUS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.STATUS") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Approval Status">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAppStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.APPOVEDSTATUS ").ToString() == "1" ? "Approved" : "Partially Approved" %>'></asp:Label>
                                        <eluc:ToolTip ID="ucToolTipAddress" runat="server" Text='<%#HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDAPPROVALSTATUS").ToString()) %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Port">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Label ID="lblPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                            OnCommand="onPurchaseQuotationEdit" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                            ToolTip="Edit"></asp:ImageButton>

                                        <asp:ImageButton runat="server" AlternateText="Select" ImageUrl="<%$ PhoenixTheme:images/select.png %>"
                                            CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSelect"
                                            ToolTip="Select"></asp:ImageButton>
                                        <asp:ImageButton runat="server" AlternateText="De-Select" ImageUrl="<%$ PhoenixTheme:images/de-select.png %>"
                                            CommandName="DESELECT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDeSelect"
                                            ToolTip="De-Select"></asp:ImageButton>

                                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                            CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                            ToolTip="Delete"></asp:ImageButton>

                                        <asp:ImageButton runat="server" AlternateText="Vendor Details" ImageUrl="<%$ PhoenixTheme:images/vendor-detail.png %>"
                                            CommandName="VENDORDETAILS" CommandArgument="<%# Container.DataItemIndex %>"
                                            ID="cmdVendor" ToolTip="Vendor Details"></asp:ImageButton>

                                        <asp:ImageButton runat="server" AlternateText="Export to Excel RFQ" ImageUrl="<%$ PhoenixTheme:images/icon_xls.png %>"
                                            CommandName="RFQEXCEL" CommandArgument="<%# Container.DataItemIndex %>"
                                            ID="cmdExcelRFQ" ToolTip="Export to Excel RFQ"></asp:ImageButton>
                          
                                        <asp:ImageButton runat="server" AlternateText="View Queries Sent" ImageUrl="<%$ PhoenixTheme:images/48.png %>"
                                            CommandName="VIEWQUERIESSENT" CommandArgument="<%# Container.DataItemIndex %>"
                                            ID="cmdViewQuery" ToolTip="View Queries Sent"></asp:ImageButton>
         
                                        <asp:ImageButton runat="server" AlternateText="Approve" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                            CommandName="APPROVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdApprove"
                                            ToolTip="Approve"></asp:ImageButton>
                                
                                        <asp:ImageButton runat="server" AlternateText="Revoke approval" ImageUrl="<%$ PhoenixTheme:images/cancel.png %>"
                                            CommandName="DEAPPROVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDeApprove"
                                            ToolTip="Revoke approval"></asp:ImageButton>
                
                                        <asp:ImageButton runat="server" AlternateText="WhatIfQty" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                            CommandName="WHATIFQTY" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdWhatIfQty"
                                            ToolTip="What If Qty"></asp:ImageButton>
                                     
                                        <asp:ImageButton runat="server" AlternateText="Re-Quote" ImageUrl="<%$ PhoenixTheme:images/quotation-requote.png %>"
                                            CommandName="REQUOTE" CommandArgument='<%# Container.DataItemIndex %>' ID="imgRequote"
                                            ToolTip="Allow to Re-quote"></asp:ImageButton>
                                     
                                        <asp:ImageButton runat="server" AlternateText="Audit Trail" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png %>"
                                            CommandName="AUDITTRAIL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAudit"
                                            ToolTip="Audit Trail"></asp:ImageButton>

                                        <asp:ImageButton runat="server" AlternateText="Remarks" ImageUrl="<%$ PhoenixTheme:images/text-detail.png %>"
                                            CommandName="REMARKS" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdRemarks"
                                            ToolTip="Remarks"></asp:ImageButton>

                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="ATTACHMENTS" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAttachment"
                                            ToolTip="Attachment"></asp:ImageButton>
                                        <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                            CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdNoAttachment"
                                            ToolTip="No Attachment"></asp:ImageButton>

                                        <asp:ImageButton runat="server" AlternateText="Delivery Instructions" ImageUrl="<%$ PhoenixTheme:images/add-instruction.png %>"
                                            CommandName="DELIVERY" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelivery"
                                            ToolTip="Delivery Instructions"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>

                    <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">&nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>

                    <div id="div2" style="position: relative;">
                        <table width="100%" border="0" cellpadding="1" cellspacing="1">
                            <tr>
                                <td>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red">  All amounts are in USD. </asp:Label>
                                </td>
                                <td></td>
                            </tr>
                        </table>
                    </div>



                </div>
            </ContentTemplate>
            <%-- <Triggers>
                <asp:PostBackTrigger ControlID="gvTax" />
            </Triggers>--%>
        </asp:UpdatePanel>
        <eluc:Confirm ID="ucConfirmMessage" runat="server" OnConfirmMesage="CopyForm_Click" OKText="Hide PO Amount"
            CancelText="Show PO Amount" Visible="false" />
    </form>
</body>
</html>
