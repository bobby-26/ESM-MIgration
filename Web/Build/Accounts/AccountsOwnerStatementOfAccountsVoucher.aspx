<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsOwnerStatementOfAccountsVoucher.aspx.cs"
    Inherits="AccountsOwnerStatementOfAccountsVoucher" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="div" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmOwnersAccounts" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="frmTitle" Text="Statement of Accounts" ShowMenu="false"></eluc:Title>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
        </div>
        <div runat="server" id="divFind">
            <table>
                <tr>
                    <td>
                        <b><asp:Literal ID="lblAccountCodeDescriptionHeader" runat="server" Text="Account Code / Description"></asp:Literal></b>
                    </td>
                    <td>
                        <asp:Label ID="lblAccountId" Visible="false" runat="server"></asp:Label>
                        <%--<asp:Label ID="lblAccountCOde" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp; / &nbsp;&nbsp;--%>
                        <asp:Label ID="lblAccountCOdeDescription" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b><asp:Literal ID="lblOwnerBudgetCodeDescription" runat="server" Text="Owner Budget Code / Description"></asp:Literal></b>
                    </td>
                    <td>
                        <asp:Label ID="lnkOwnerBudgetCode" runat="server"></asp:Label>&nbsp;&nbsp;&nbsp; / &nbsp;&nbsp;
                        <asp:Label ID="lblBudgetCodeDescription" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b><asp:Literal ID="lblStatementReferenceHeader" runat="server" Text="Statement Reference"></asp:Literal></b>
                    </td>
                    <td>
                        <asp:Label ID="lblStatementReference" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b><asp:Literal ID="lblBudgetCodeTotal" runat="server" Text="Budget Code Total"></asp:Literal></b>
                    </td>
                    <td>
                        <b><asp:Literal ID="lblUSD" runat="server" Text="USD"></asp:Literal> <asp:Label runat="server" ID="lblTotal"></asp:Label></b>
                    </td>
                </tr>
            </table>
        </div>
        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div id="divScroll" style="position: relative; z-index: 1; width: 100%; overflow: auto;">
            <div id="divGrid" style="position: relative; z-index: 1; width: 100%;">
                <asp:GridView ID="gvOwnersAccount" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" AllowSorting="true" ShowHeader="true" EnableViewState="false"
                    OnRowDataBound="gvOwnersAccount_RowDataBound">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                    <RowStyle Height="10px" />
                    <Columns>
                         <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Literal ID="lblSNo" runat="server" Text="S.No."></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Literal ID="lblVoucherDate" runat="server" Text="Voucher Date"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVoucherDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Literal ID="lblVoucherRow" runat="server" Text="Voucher Row"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblVoucherRow" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERROW") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:Literal ID="lblParticulars" runat="server" Text="Particulars"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblParticulars" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Right" />
                            <HeaderTemplate>
                                <asp:Literal ID="lblAmountUSD" runat="server" Text="Amount(USD)"></asp:Literal>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    </form>
</body>
</html>

