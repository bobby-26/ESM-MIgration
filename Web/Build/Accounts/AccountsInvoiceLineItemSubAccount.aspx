<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsInvoiceLineItemSubAccount.aspx.cs"
    Inherits="AccountsInvoiceLineItemSubAccount" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sub Account</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmInvoice" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInvoice">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading" style="vertical-align: top">
                            <asp:Literal ID="lblSubAccount" runat="server" Text="Sub Account"></asp:Literal>
                        </div>
                        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuInvoice1" runat="server" OnTabStripCommand="Invoice_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div>
                    <table cellpadding="1" cellspacing="1" style="width: 100%">
                        <tr>
                            <td width="20%">
                                <asp:Literal ID="lblInvoiceNumber" runat="server" Text="Invoice Number"></asp:Literal>
                            </td>
                            <td width="30%">
                                <asp:TextBox ID="txtInvoiceNumber" runat="server" MaxLength="25" ReadOnly="true"
                                    CssClass="readonlytextbox" Width="150px"></asp:TextBox>
                            </td>
                            <td width="20%">
                                <asp:Literal ID="lblInvoiceDate" runat="server" Text="Invoice Date"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoiceDateEdit" runat="server" ReadOnly="true" MaxLength="10"
                                    CssClass="readonlytextbox" Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblVesselCode" runat="server" Text="Vessel Code"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVesselCodeEdit" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                    MaxLength="10" Width="150px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVesselNameEdit" runat="server" ReadOnly="true" MaxLength="25"
                                    CssClass="readonlytextbox" Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblSupplierReference" runat="server" Text="Supplier Reference"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSupplierRefEdit" runat="server" ReadOnly="true" MaxLength="25"
                                    CssClass="readonlytextbox" Width="150px"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblPurchaseOrderNumber" runat="server" Text="Purchase Order Number"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPurchaseOrderNumberEdit" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                    Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblPOPayableAmount" runat="server" Text="PO Payable Amount"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPurchasePayableAmountEdit" runat="server" ReadOnly="true" CssClass="readonlytextbox txtNumber"
                                    Width="150px"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="maskeditpurpayableamount" runat="server" TargetControlID="txtPurchasePayableAmountEdit"
                                    OnInvalidCssClass="MaskedEditError" Mask="999,999,999,999.99" MaskType="Number"
                                    InputDirection="RightToLeft" AutoComplete="false" />
                            </td>
                            <td>
                                <asp:Literal ID="lblPOAdvanceAmount" runat="server" Text="PO Advance Amount"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPurchaseAdvanceAmountEdit" runat="server" ReadOnly="true" CssClass="readonlytextbox txtNumber"
                                    Wrap="False" Width="150px"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditpuradvanceamount" runat="server" AutoComplete="false"
                                    InputDirection="LeftToRight" Mask="999,999,999,999.99" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtPurchaseAdvanceAmountEdit" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblInvoicePayableAmount" runat="server" Text="Invoice Payable Amount"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInvoicePayableAmoutEdit" runat="server" ReadOnly="true" CssClass="readonlytextbox txtNumber"
                                    Width="150px"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditInvoiceAmout" runat="server" AutoComplete="false"
                                    InputDirection="LeftToRight" Mask="999,999,999,999.99" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtInvoicePayableAmoutEdit" />
                            </td>
                            <td>
                                <asp:Literal ID="lblIsIncludedinSOA" runat="server" Text="Is Included in SOA"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIncludedinSOAYNEdit" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                    Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblSubAccountCode" runat="server" Text="Sub Account Code"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubAccountCodeEdit" runat="server" MaxLength="15" Text="" CssClass="input_mandatory"
                                    Width="150px"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditSubAccountCodeEdit" runat="server"
                                    AutoComplete="true" InputDirection="RightToLeft" Mask="99999999999999999" MaskType="Number"
                                    OnInvalidCssClass="MaskedEditError" TargetControlID="txtSubAccountCodeEdit" />
                            </td>
                            <td>
                                <asp:Literal ID="lblSubAccountAmount" runat="server" Text="Sub Account Amount"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSubAccAmountEdit" runat="server" CssClass="input_mandatory  txtNumber"
                                    MaxLength="18" Width="150px"></asp:TextBox>
                                <ajaxToolkit:MaskedEditExtender ID="MaskedEditSubAccAmountEdit" runat="server" AutoComplete="true"
                                    InputDirection="RightToLeft" Mask="99,99,99,999.99" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                    TargetControlID="txtSubAccAmountEdit" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <br />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuInvoice" runat="server" OnTabStripCommand="Invoice_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvInvoice" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvInvoice_ItemDataBound"
                        OnRowDeleting="gvInvoice_RowDeleting" ShowFooter="False" EnableViewState="False"
                        ShowHeader="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:TemplateField HeaderText="SubAccount">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSubAccountHeader" runat="server">Sub Account Code&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSubAccount" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNTCODE") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkSubAccount" runat="server" CommandName="EDIT" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICELINEITEMSUBACCOUNTCODE") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNTCODE")  %>' OnCommand="InvoiceClick"></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSubAccountAmount" runat="server" Text="Sub Account Amount"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVesselCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:f2}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>     
    </asp:UpdatePanel>
    </form>
</body>
</html>
