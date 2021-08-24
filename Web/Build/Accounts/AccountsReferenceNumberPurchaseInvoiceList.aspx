<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsReferenceNumberPurchaseInvoiceList.aspx.cs"
    Inherits="AccountsReferenceNumberPurchaseInvoiceList" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseInvoiceVoucherList" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlVoucher">
        <ContentTemplate>
            <div class="navigation" id="Div3" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <div class="subHeader" style="position: relative">
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuVoucher" runat="server" OnTabStripCommand="MenuVoucher_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div>
                    &nbsp;<b><asp:Literal ID="lblSameReferenceNumberisExistsinbelowmentionedVouchersClicktheCreateUpdatebuttontocontinueorclicktheCancelbuttontostop" runat="server" Text="Same Reference Number is Exists in below mentioned Vouchers. Click the Create/Update button to continue or click the Cancel button to stop"></asp:Literal></b>
                    <div id="divGrid" style="position: relative; z-index: 1">
                        <asp:GridView ID="gvVoucherLineItem" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" AllowSorting="true" ShowFooter="FALSE" Style="margin-bottom: 0px"
                            EnableViewState="false" OnSelectedIndexChanging="gvVoucherLineItem_SelectedIndexChanging"
                            OnSorting="gvVoucherLineItem_Sorting">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                                <asp:TemplateField HeaderText="Voucher Number">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblVoucherNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDVOUCHERNUMBER"
                                            ForeColor="White">Voucher Number&nbsp;</asp:LinkButton>
                                        <img id="FLDVOUCHERNUMBER" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblVoucherID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERID") %>'></asp:Label>
                                        <asp:LinkButton ID="lnkVoucherNumber" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERID") %>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER")  %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Voucher Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblVoucherDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDVOUCHERDATE"
                                            ForeColor="White">Voucher Date&nbsp;</asp:LinkButton>
                                        <img id="FLDVOUCHERDATE" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblVoucherDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divLineItem" style="position: relative;">
                        <table width="100%" border="0" class="datagrid_pagestyle">
                            <tr>
                                <td nowrap="nowrap" align="center">
                                    <asp:Label ID="lblPagenumber" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblPages" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblRecords" runat="server">
                                    </asp:Label>&nbsp;&nbsp;
                                </td>
                                <td nowrap="nowrap" align="left" width="50px">
                                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                                </td>
                                <td width="20px">
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap="nowrap" align="center">
                                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                    </asp:TextBox>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                        Width="40px"></asp:Button>
                                </td>
                            </tr>
                            <eluc:Status runat="server" ID="Status1" />
                        </table>
                    </div>
                    &nbsp;<b><asp:Literal ID="lblLineItemDetailsNotePleaseselectaVouchertoviewlineitemdetails" runat="server" Text="Line Item Details (Note : Please select a Voucher to view line item details)"></asp:Literal></b>
                    <div id="div2" style="position: relative; z-index: 2; width: 100%;">
                        <asp:GridView ID="gvLineItemDetails" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblVoucherLineItemNoHeader" runat="server">Row Number&nbsp;
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblVoucherLineItemNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMNO") %>'></asp:Label>
                                        <asp:Label ID="lblVoucherLineId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMID") %>'
                                            Visible="false"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Id">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblAccountCode" runat="server" Text="AccountCode"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Account Description">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblAccountDescription" runat="server" Text="Account Description"></asp:Literal>&nbsp;
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblAccountDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Budget Id">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblSubAccountCode" runat="server" Text="Sub Account Code"></asp:Literal>&nbsp;
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBudget" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBUDGETCODE") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Currency">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblTransactionCurrency" runat="server" Text="Transaction Currency"></asp:Literal>&nbsp;
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYNAME")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblPrimeAmount" runat="server" Text="Prime Amount"></asp:Literal>&nbsp;(
                                        <%=strTransactionAmountTotal%>
                                        )
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblTranAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRANSACTIONAMOUNT","{0:n2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblBaseAmount" runat="server" Text="Base Amount"></asp:Literal>&nbsp; (
                                        <%=strBaseAmountTotal%>
                                        )
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblBaseAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBASEAMOUNT","{0:n2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Amount">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblReportAmount" runat="server" Text="Report Amount"></asp:Literal>&nbsp; (
                                        <%=strReportAmountTotal%>
                                        )
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblReportAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTAMOUNT","{0:n2}") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div id="divPage" style="position: relative;">
                            <table width="100%" border="0" class="datagrid_pagestyle">
                                <tr>
                                    <td nowrap="nowrap" align="center">
                                        <asp:Label ID="lblLIDetailsPagenumber" runat="server">
                                        </asp:Label>
                                        <asp:Label ID="lblLIDetailsPages" runat="server">
                                        </asp:Label>
                                        <asp:Label ID="lblLIDetailsRecords" runat="server">
                                        </asp:Label>&nbsp;&nbsp;
                                    </td>
                                    <td nowrap="nowrap" align="left" width="50px">
                                        <asp:LinkButton ID="cmdLIDetailsPrevious" runat="server" OnCommand="LIDetailsPagerButtonClick"
                                            CommandName="prev">Prev << </asp:LinkButton>
                                    </td>
                                    <td width="20px">
                                        &nbsp;
                                    </td>
                                    <td nowrap="nowrap" align="right" width="50px">
                                        <asp:LinkButton ID="cmdLIDetailsNext" OnCommand="LIDetailsPagerButtonClick" runat="server"
                                            CommandName="next">Next >></asp:LinkButton>
                                    </td>
                                    <td nowrap="nowrap" align="center">
                                        <asp:TextBox ID="txtLIDetailsnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                        </asp:TextBox>
                                        <asp:Button ID="btnLIDetailsGo" runat="server" Text="Go" OnClick="cmdLIDetailsGo_Click"
                                            CssClass="input" Width="40px"></asp:Button>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
        </ContentTemplate>
        <Triggers>
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
