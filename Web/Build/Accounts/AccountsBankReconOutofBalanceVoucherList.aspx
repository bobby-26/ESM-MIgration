<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsBankReconOutofBalanceVoucherList.aspx.cs" Inherits="Accounts_AccountsBankReconOutofBalanceVoucherList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlBankAccount" Src="~/UserControls/UserControlBankAccount.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bank Statement Upload</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmForwarderExcelUpload" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader">
            <eluc:Title runat="server" ID="Attachment" ShowMenu="false" Text="Vouchers Out of Balance List"></eluc:Title>
            <eluc:Status runat="server" ID="ucStatus" />
        </div>
<%--        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="Bankupload" TabStrip="true" runat="server" OnTabStripCommand="Bankupload_OnTabStripCommand"></eluc:TabStrip>
        </div>--%>
<%--        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuAttachment" runat="server"  OnTabStripCommand="MenuAttachment_OnTabStripCommand"></eluc:TabStrip>
        </div>--%>
        <asp:GridView ID="gvBankLedger" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true" 
            DataKeyNames="FLDVOUCHERLINEITEMID" OnRowDataBound="gvBankLedger_RowDataBound" >
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
            <RowStyle Height="10px" />
            <Columns>
                <%--   Tag ID	Currency	Bank Amount Net	Allocated in Leger	Remaining amount	Allocation Status	Action--%>
                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
<%--                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:CheckBox ID="chkAllRemittance" runat="server" Text="Check All" AutoPostBack="true"
                            OnPreRender="CheckAll" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="chkSelect" runat="server" EnableViewState="true" OnCheckedChanged="BankLedgerSaveCheckedValues" />
                    </ItemTemplate>
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Voucher Ref.">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblRefno" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDREFERENCEDOCUMENTNO")) %>'></asp:Label>
                        <asp:Label ID="lblLedgerLineitemId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDVOUCHERLINEITEMID")%>' Visible="false"></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderTemplate>
                        <asp:Literal ID="lblAmount" runat="server" Text="Amount"></asp:Literal>
                    </HeaderTemplate>
                    <ItemStyle HorizontalAlign="RIGHT"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblRemainingamount" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDAMOUNT", "{0:n2}" )) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Value Date">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblVoucherDate" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE","{0:dd/MMM/yyyy}" )) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Voucher entry">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblAllocatedinLeger" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDVOUCHERNUMBER" )) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Description">
                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblCurrency" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION" )) %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                    <HeaderTemplate>
                        <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                    </HeaderTemplate>
                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                    <ItemTemplate>
<%--                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton ID="cmdDelete" runat="server" AlternateText="Delete" CommandArgument="<%# Container.DataItemIndex %>"
                            CommandName="DELETE" ImageUrl="<%$ PhoenixTheme:images/te_del.png  %>" ToolTip="Delete" />--%>
                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                            CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                            ToolTip="Edit"></asp:ImageButton>
                        <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
<%--                        <asp:ImageButton ID="cmdEditVoucher" runat="server" AlternateText="Archive" CommandArgument="<%# Container.DataItemIndex %>"
                            CommandName="EDITVOUCHER" ImageUrl="<%$ PhoenixTheme:images/edit-info.png %>"
                            ToolTip="Change ledger entries" />--%>
  <%--                      <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:ImageButton ID="cmdVouchershowing" runat="server" AlternateText="Archive"
                            CommandArgument="<%# Container.DataItemIndex %>" CommandName="VOUCHERSHOWING"
                            ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" ToolTip="Voucher showing" />--%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <%--   <table width="100%" border="0" class="datagrid_pagestyle">
            <tr>
                <td nowrap="nowrap" align="center">
                    <asp:Label ID="Label1" runat="server">
                    </asp:Label>
                    <asp:Label ID="Label2" runat="server">
                    </asp:Label>
                    <asp:Label ID="Label3" runat="server">
                    </asp:Label>&nbsp;&nbsp;
                </td>
                <td nowrap="nowrap" align="left" width="50px">
                    <asp:LinkButton ID="LinkButton1" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                </td>
                <td width="20px">
                    &nbsp;
                </td>
                <td nowrap="nowrap" align="right" width="50px">
                    <asp:LinkButton ID="LinkButton2" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                </td>
                <td nowrap="nowrap" align="center">
                    <asp:TextBox ID="TextBox1" MaxLength="3" Width="20px" runat="server" CssClass="input">
                    </asp:TextBox>
                    <asp:Button ID="Button1" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                        Width="40px"></asp:Button>
                </td>
            </tr>
        </table>--%>
    </div>
    </form>
</body>
</html>

