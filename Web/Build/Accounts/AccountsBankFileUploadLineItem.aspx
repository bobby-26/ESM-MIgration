<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsBankFileUploadLineItem.aspx.cs"
    Inherits="AccountsBankFileUploadLineItem" %>

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
    <title>Bank File Line Item</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAccountsRemittanceInstructionLineItem" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlRemittanceInstruction">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Uploaded File Data" ShowMenu="false">
                    </eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <table>
                    <tr>
                        <td>
                            <asp:Label ID="lblfilename" Visible="true" runat="server" Text="File Name"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFileName" runat="server" Width="300" CssClass="input" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <table cellpadding="0" cellspacing="1" style="width: 100%">
                    <tr>
                        <td style="vertical-align: top">
                            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                                <asp:GridView ID="gvBankFileupLoadLineItem" runat="server" AutoGenerateColumns="False"
                                    Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvBankFileupLoadLineItem_RowCommand"
                                    OnRowDataBound="gvBankFileupLoadLineItem_ItemDataBound" OnRowDeleting="gvBankFileupLoadLineItem_RowDeleting"
                                    AllowSorting="true" EnableViewState="false" OnSorting="gvBankFileupLoadLineItem_Sorting">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                                    <RowStyle Height="10px" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Account Number">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblAccountNumber" runat="server" Text="Account Number"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblAccountNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTNUMBER") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Originators Reference">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblOriginatorsReference" runat="server" Text="Originators Reference"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblOriginatorsReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTTREFERENCENUMBER") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Bank Reference">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblBankReference" runat="server" Text="Bank Reference"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblBankReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBANKREFERENCE") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debit Currency">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblDebitCurrency" runat="server" Text="Debit Currency"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDebitCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITCURRENCY") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debit Amount">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblDebitAmount" runat="server" Text="Debit Amount"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDebitAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITAMOUNT","{0:n2}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debit Exchange Rate">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblDebitExchangeRate" runat="server" Text="Debit Exchange Rate"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDebitExchangeRate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITEXCHANGERATE","{0:n17}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Debit Value Date">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                Debit Value Date
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblDebitValueDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITVALUEDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payment Currency">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                Payment Currency
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaymentCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTCURRENCY") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payment Amount">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <HeaderTemplate>
                                                Payment Amount
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblPaymentAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTAMOUNT","{0:n2}") %>'></asp:Label>
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
