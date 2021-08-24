<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCashOutMDApprovalLineItems.aspx.cs"
    Inherits="AccountsCashOutMDApprovalLineItems" %>

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
<head id="Head1" runat="server">
    <title>Cash Out Line Item</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <style type="text/css">
        .style1
        {
            height: 29px;
        }
    </style>
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
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title1" Text="Cash Request Line Items" ShowMenu="false"></eluc:Title>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
        </div>
        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuTools" runat="server" OnTabStripCommand="MenuTools_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <table cellpadding="0" cellspacing="1" style="width: 100%">
            <tr>
                <td style="vertical-align: top">
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvCashOutDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCommand="gvCashOutDetails_RowCommand" OnRowDataBound="gvCashOutDetails_ItemDataBound"
                            OnRowDeleting="gvCashOutDetails_RowDeleting" AllowSorting="true" EnableViewState="false"
                            OnSorting="gvCashOutDetails_Sorting" DataKeyNames="FLDCASHPAYMENTID">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField HeaderText="Cash Request Number">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCashRequestNumber" runat="server" Text="Cash Request Number"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCashPaymentNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCASHPAYMENTNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Cash Account Description">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCashAccountDescription" runat="server" Text="Cash Account Description"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAccountDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:Label>
                                    <asp:Label ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></asp:Label>
                                    <asp:Label ID="lblCashPaymentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCASHPAYMENTID") %>'></asp:Label>  
                                </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Payee">
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblPayee" runat="server" Text="Payee"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkPayee" runat="server" CommandArgument='<%# Bind("FLDCASHPAYMENTID") %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME") %>'></asp:LinkButton>
                                        <%--<asp:Label ID="Payee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME") %>'></asp:Label>--%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Source">
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblSource" runat="server" Text="Source"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="Payee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency">
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblAmount" runat="server" Text="Amount"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCASHPAYMENTAMOUNT","{0:n2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="USD Equivalent">
                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblUSDEquivalent" runat="server" Text="USD Equivalent"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblUSDEquivalent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSDEQUVALENT","{0:n2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Batch Status">
                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblBatchStatus" runat="server" Text="Batch Status"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBatchStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHSTATUS") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField FooterStyle-HorizontalAlign="Center">
                                    <HeaderStyle />
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Confirm" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                            CommandName="APPROVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdApprove"
                                            ToolTip="Approve"></asp:ImageButton>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton ID="cmdReject" runat="server" AlternateText="Order" CommandArgument="<%# Container.DataItemIndex %>"
                                            CommandName="REJECT" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ToolTip="Reject" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divPage" style="position: relative;">
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
                               <td width="20px">
                                    &nbsp;
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
                        <br />
                    </div>
    </form>
</body>
</html>
