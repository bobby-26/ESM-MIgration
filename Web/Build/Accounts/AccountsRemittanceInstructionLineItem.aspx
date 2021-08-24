<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsRemittanceInstructionLineItem.aspx.cs"
    Inherits="AccountsRemittanceInstructionLineItem" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Remittence Line Item</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseFormItemDetails" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlRemittanceInstruction">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Remittance Instruction" ShowMenu="false">
                    </eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuRemittanceInstructionMain" runat="server" OnTabStripCommand="MenuRemittanceInstructionMain_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblRemittanceNumber" runat="server" Text="Remittance Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemittancNumber" Width="180px" ReadOnly="true" CssClass="input"
                                runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblAccountCode" runat="server" Text="Account Code"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccountCode" ReadOnly="true" CssClass="input" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblAccountDescription" runat="server" Text="Account Description"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAccountDescription" Width="180px" ReadOnly="true" CssClass="input"
                                runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCurrency" ReadOnly="true" CssClass="input" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="1" style="width: 100%">
                    <tr>
                        <td style="vertical-align: top">
                            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                                <asp:GridView ID="gvRemittanceInstructionLineItem" runat="server" AutoGenerateColumns="False"
                                    Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvRemittanceInstructionLineItem_RowCommand"
                                    OnRowDataBound="gvRemittanceInstructionLineItem_ItemDataBound" OnRowDeleting="gvRemittanceInstructionLineItem_RowDeleting"
                                    AllowSorting="true" EnableViewState="false" OnSorting="gvRemittanceInstructionLineItem_Sorting">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                                    <RowStyle Height="10px" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Supplier Name">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblSupplierCode" runat="server" Text="Supplier Code"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Supplier Name">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblSupplierName" runat="server" Text="Supplier Name"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Supplier Currency Code">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblSupplierCurrencyCode" runat="server" Text="Supplier Currency Code"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCurrencyShortCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERCURRENCYCODE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="TotalAmount">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblTotalAmount" runat="server" Text="Total Amount"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPayableAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALPAYABLEAMOUNT","{0:n2}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
