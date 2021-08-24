<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotedVendorList.aspx.cs"
    Inherits="PurchaseQuotedVendorList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
    <form id="frmPurchaseQuotedVendorList" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
     <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="Title1" Text="Vendors" ShowMenu="false"></eluc:Title>
        </div>
         <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    </div>
      <asp:UpdatePanel runat="server" ID="pnlFormDetails">
        <ContentTemplate>
    
    <div class="navigation" id="navigation" style=" margin-left: 0px; vertical-align: top; width: 100%">
        <div class="navSelect* " style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuVendorList" runat="server" OnTabStripCommand="MenuVendorList_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
            <asp:GridView ID="gvVendorList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowCommand="gvVender_RowCommand" OnRowDataBound="gvVender_ItemDataBound"
                ShowHeader="true" EnableViewState="false">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:TemplateField Visible ="false" >
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="FLDVENDORID">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblVenderCodeHeader" runat="server">Vendor Code<asp:ImageButton runat="server"
                                ID="cmdVenderCodeDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click"
                                CommandName="FLDVENDORID" CommandArgument="1" />
                                <asp:ImageButton runat="server" ID="cmdVenderCodeAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                    OnClick="cmdSort_Click" CommandName="FLDVENDORID" CommandArgument="0" />
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblVenderCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORCODE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="number">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblVenderHeader" runat="server">Vendor Nane&nbsp;<asp:ImageButton
                                runat="server" ID="cmdVenderDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click"
                                CommandName="FLDNAME" CommandArgument="1" />
                                <asp:ImageButton runat="server" ID="cmdVenderAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                    OnClick="cmdSort_Click" CommandName="FLDNAME" CommandArgument="0" />
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="StockItem Name" Visible ="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblSendDateHeader" runat="server">Send Date&nbsp;<asp:ImageButton
                                runat="server" ID="cmdSendDateDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click"
                                CommandName="FLDSENTDATE" CommandArgument="1" AlternateText="StockItem name desc" />
                                <asp:ImageButton runat="server" ID="cmdSendDateAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                    OnClick="cmdSort_Click" CommandName="FLDSENTDATE" CommandArgument="0" />
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblSendDateCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSENTDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Maker">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblRecivedDateHeader" runat="server">Recive Date&nbsp;<asp:ImageButton
                                runat="server" ID="cmdRecivedDateDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click"
                                CommandName="FLDRECEIVEDDATE" CommandArgument="1" />
                                <asp:ImageButton runat="server" ID="cmdRecivedDateAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                    OnClick="cmdSort_Click" CommandName="FLDRECEIVEDDATE" CommandArgument="0" />
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblRecivedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>                 
                    <asp:TemplateField HeaderText="FLDCURRENCYCODE">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblCurencyHeader" runat="server">Currency<asp:ImageButton
                                runat="server" ID="cmdCurencyDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click"
                                CommandName="FLDCURRENCYCODE" CommandArgument="1" />
                                <asp:ImageButton runat="server" ID="cmdCurencyAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                    OnClick="cmdSort_Click" CommandName="FLDCURRENCYCODE" CommandArgument="0" />
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCurency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYCODE") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Time">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblTimeHeader" runat="server">Delivery Time<asp:ImageButton
                                runat="server" ID="cmdTimeDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click"
                                CommandName="FLDDELIVERYTIME" CommandArgument="1" />
                                <asp:ImageButton runat="server" ID="cmdTimeAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>" OnClick="cmdSort_Click"
                                    CommandName="FLDDELIVERYTIME" CommandArgument="0" />
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVERYTIME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Wanted">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblPriceHeader" runat="server">Price<asp:ImageButton runat="server"
                                ID="cmdPriceDesc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click" CommandName="FLDQUOTEDPRICE"
                                CommandArgument="1" />
                                <asp:ImageButton runat="server" ID="cmdPriceAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                    OnClick="cmdSort_Click" CommandName="FLDQUOTEDPRICE" CommandArgument="0" />
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE","{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField>
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblDiscountHeader" runat="server">Discount </asp:Label>
                               
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALDISCOUNT","{0:n2}") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField>
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Label ID="lblTotalAmountHeader" runat="server">Total Amount
                            </asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblTotalAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TOTALaMOUNT","{0:n2}") %>'></asp:Label>
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
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
