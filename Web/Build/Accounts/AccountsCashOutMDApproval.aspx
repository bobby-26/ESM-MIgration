<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsCashOutMDApproval.aspx.cs"
    Inherits="AccountsCashOutMDApproval" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="Form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager2" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlStockItemEntry">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="MD Approval "></eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuNavigate" runat="server" OnTabStripCommand="MenuNavigate_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>   
                </div>
                <table id="tbldiv" runat="server" cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <div class="subHeader" style="position: relative;">
                                <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                                    <eluc:TabStrip ID="MenuApprove" runat="server" OnTabStripCommand="MenuApprove_TabStripCommand">
                                    </eluc:TabStrip>
                                </span>
                            </div>
                        </td>
                    </tr>
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuTools" runat="server" OnTabStripCommand="MenuTools_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1; width: 100%;">
                    <asp:GridView ID="gvCashOut" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                        OnPreRender="gvCashOut_PreRender" OnRowCommand="gvCashOut_RowCommand" OnRowDataBound = "gvCashOut_ItemDataBound" DataKeyNames="FLDCASHPAYMENTID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkAllCashOut" runat="server" Text="Check All" AutoPostBack="true"
                                        OnPreRender="CheckAll" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" EnableViewState="true" OnCheckedChanged="SaveCheckedValues" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Cash Account Description">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCashAccountDescription" runat="server" Text="Cash Account Description"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblAccountDescription" runat="server" CommandArgument='<%# Bind("FLDCASHPAYMENTID") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION")  %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payee">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPayee" runat="server" Text="Payee"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="Payee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME") %>'></asp:Label>
                                    <asp:Label ID="lblPayeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERID") %>'></asp:Label>
                                    <asp:Label ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></asp:Label>
                                    <asp:Label ID="lblCashPaymentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCASHPAYMENTID") %>'></asp:Label>
                                    <asp:Label ID="lblAccountIdCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'
                                        Visible="false"></asp:Label>  
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Currency">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCurrencyHdr" runat="server" Text="Currency"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Amount">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAmountHdr" runat="server" Text="Amount"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="USD Equivalent">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblUSDEquivalentHdr" runat="server" Text="USD Equivalent"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblUSDEquivalent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSDEQUVALENTAMOUNT","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSumUSDEquivalentHdr" runat="server" Text="Sum USD Equivalent"></asp:Literal><%-- ( <%=strdTotalUSDAmount %> )--%>&nbsp; 
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Right" Wrap="False" VerticalAlign="Bottom" />
                                <ItemTemplate>
                                    <b>
                                        <asp:Label ID="lblSumUSDEquivalent" runat="server" Text=''></asp:Label>
                                    </b>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField Visible="false">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSumUSDEquivalentHdr1" runat="server" Text="Sum USD Equivalent"></asp:Literal><%-- ( <%=strdTotalUSDAmount %> )--%>&nbsp; 
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Right" Wrap="False" VerticalAlign="Bottom" />
                                <ItemTemplate>
                                    <b>
                                        <asp:Label ID="lblTotalSumUSDEquivalent" runat="server" Text=''></asp:Label>
                                    </b>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField FooterStyle-HorizontalAlign="Center">
                                <HeaderStyle />
                                <HeaderTemplate>
                                    <asp:Label ID="lblActionHeader" runat="server"> Action </asp:Label>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="cmdReject" runat="server" AlternateText="Order" CommandArgument="<%# Container.DataItemIndex %>"
                                        CommandName="REJECT" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>" ToolTip="Un-Batch" />
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
                        <td width="20px">
                            <input type="hidden" id="isouterpage" name="isouterpage" />
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
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
