<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCashOutVoucherPosting.aspx.cs"
    Inherits="AccountsCashOutVoucherPosting" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="YesNo" Src="~/UserControls/UserControlYesNo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlCurrency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cash Out Request</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmPurchaseForm" runat="server" autocomplete="off">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCashOut">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="frmTitle" Text="Cash Payment and Voucher Posting"></eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvCashOut" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvCashOut_RowCommand" OnRowDataBound="gvCashOut_ItemDataBound"
                        OnRowCancelingEdit="gvCashOut_RowCancelingEdit" OnRowDeleting="gvCashOut_RowDeleting"
                        AllowSorting="true" OnRowEditing="gvCashOut_RowEditing" EnableViewState="false"
                        OnSorting="gvCashOut_Sorting" DataKeyNames="FLDCASHPAYMENTID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Cash Account Description">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCashAccountDescription" runat="server" Text="Cash Account Description"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAccountDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:Label>
                                    <asp:Label ID="lblCashPaymentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCASHPAYMENTID") %>'></asp:Label> 
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Voucher Number" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCashOutNumberHeader" runat="server" CommandName="Sort"
                                        CommandArgument="FLDCASHPAYMENTNUMBER" ForeColor="White">Cash Request Number&nbsp;</asp:Label>
                                    <img id="FLDCASHPAYMENTNUMBER" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCashOutId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCASHPAYMENTID") %>'></asp:Label>
                                    <asp:Label ID="lblCurrencyCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCY") %>'></asp:Label>
                                    <asp:LinkButton ID="lnkCashOutid" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCASHPAYMENTNUMBER")  %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Supplier Code" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSupplierCode" runat="server" Text="Supplier Code"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSupplierCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Approved Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblApprovedDate" runat="server" Text="Approved Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDMODIFIEDDATE", "{0:dd/MMM/yyyy}")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payee">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPayee" runat="server" Text="Payee"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payment Mode" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPaymentmode" runat="server" Text="Payment mode"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPaymentmode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTTYPE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cash Account">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCashAccount" runat="server" Text="Cash Account"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCashAccount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Currency">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCurrency" runat="server" Text="Currency"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAmount" runat="server" Text="Amount"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRemittanceamount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCASHPAYMENTAMOUNT","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Source" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSource" runat="server" Text="Source"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSource" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCASHPAYMENTSTATUS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cash Payment Voucher Number" Visible="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCashPaymentVoucherNumber" runat="server" Text="Cash Payment Voucher Number"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPaymentVoucherNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTVOUCHERNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server">
                                    Action
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                        ToolTip="Paid"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <%--<asp:ImageButton runat="server" AlternateText="On Hold" ImageUrl="<%$ PhoenixTheme:images/waiting-approve.png %>"
                                        CommandName="OnHold" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdOnHold"
                                        ToolTip="On Hold"></asp:ImageButton>--%>
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAtt"
                                        ToolTip="Attachment"></asp:ImageButton>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />   
                                    <asp:ImageButton runat="server" AlternateText="Confirm" ImageUrl="<%$ PhoenixTheme:images/approve.png %>"
                                            CommandName="POST" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdPost"
                                            ToolTip="Post"></asp:ImageButton>
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
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
