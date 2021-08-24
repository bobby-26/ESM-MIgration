<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseOrderLineItemSelection.aspx.cs"
    Inherits="PurchaseOrderLineItemSelection" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <%-- <div id="Div1" runat="server">--%>
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />


    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <%--  </div>--%>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="div1" style="vertical-align: top">
            <eluc:Title runat="server" ID="ucTitle" Text="Stock Item" ShowMenu="false" />
        </div>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuStoreItemInOutTransaction" runat="server" OnTabStripCommand="StoreItemInOutTransaction_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlStoreItemInOutTransaction">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="0" cellspacing="0" style="float: left; width: 100%;">
                <tr style="position: relative; vertical-align: top">
                    <td>
                        <table cellpadding="1" cellspacing="1" style="float: left; width: 100%;">
                            <tr>
                                <td>
                                   <asp:Literal ID="lblPartNumber" runat="server" Text="Part Number"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPartNumber" runat="server" CssClass="input"></asp:TextBox>
                                </td>
                                <td>
                                   <asp:Literal ID="lblItemName" runat="server" Text="Item Name"></asp:Literal>
                                    
                                </td>
                                <td>
                                    <asp:TextBox ID="txtItemName" runat="server" CssClass="input"></asp:TextBox>
                                </td>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblClassName" runat="server"> </asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlComponentClass" runat="server" CssClass="input" Visible="false" DataTextField="FLDCOMPONENTNAME" DataValueField="FLDCOMPONENTID" />
                                        <eluc:Hard ID="ddlStockClassType" runat="server" CssClass="input" AppendDataBoundItems="true"
                                            Visible="false" />
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMakerRef" runat="server" Text="Maker Reference"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtMakerReference" runat="server" CssClass="input" MaxLength="100"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="4">
                                        <div id="divGrid" style="position: relative;">
                                            <asp:GridView ID="gvItemList" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                                Width="100%" CellPadding="3" OnRowCommand="gvItemList_RowCommand" OnRowDataBound="gvItemList_ItemDataBound"
                                                OnRowCancelingEdit="gvItemList_RowCancelingEdit" OnRowDeleting="gvItemList_RowDeleting"
                                                OnSorting="gvItemList_Sorting" OnRowEditing="gvItemList_RowEditing" AllowSorting="true"
                                                ShowFooter="false" ShowHeader="true" EnableViewState="false" OnRowUpdating="gvItemList_RowUpdating"
                                                OnRowCreated="gvItemList_RowCreated">
                                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                                <RowStyle Height="10px" />
                                                <Columns>
                                                    <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                                                    <asp:TemplateField HeaderText="Item Number">
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lblnumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDNUMBER"
                                                                ForeColor="White">Number</asp:LinkButton>
                                                            <img id="FLDNUMBER" runat="server" visible="false" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Item Name">
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderTemplate>
                                                            <asp:LinkButton ID="lblStockItemNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                                                ForeColor="White">Item Name</asp:LinkButton>
                                                            <img id="FLDNAME" runat="server" visible="false" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStoreItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMID") %>'></asp:Label>
                                                             <asp:Label ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></asp:Label>
                                                            <asp:Label ID="lblIsInMarket" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISINMARKET") %>'></asp:Label>
                                                            <asp:Label ID="lblStoreItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Maker Name">
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblMakerNameHeader" runat="server">Maker Name</asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMaker" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.MAKER") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Maker Reference">
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblMakerReferenceHeader" runat="server">Maker Reference</asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblMakerReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>                                                    
                                                    <asp:TemplateField HeaderText="Quantity">
                                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblWantedHeader" runat="server">Quantity
                                                            </asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="txtQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY","{0:n2}") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:Label ID="lblOrderLineId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>'
                                                                Visible="false"></asp:Label>
                                                            <asp:TextBox ID="txtQuantityEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY","{0:n2}") %>'
                                                                CssClass="gridinput txtNumber" Width="60"></asp:TextBox>
                                                        </EditItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Purchased Currency">
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblPurchasedCurrencyHeader" runat="server">Unit </asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPurchasedCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                        <HeaderTemplate>
                                                            <asp:Label ID="lblActionHeader" runat="server">Action </asp:Label>
                                                        </HeaderTemplate>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                                CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                                                ToolTip="Edit"></asp:ImageButton>
                                                            <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                        </ItemTemplate>
                                                        <EditItemTemplate>
                                                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                                CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdUpdate"
                                                                ToolTip="Update"></asp:ImageButton>
                                                            <img runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                                CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                                                ToolTip="Cancel"></asp:ImageButton>
                                                        </EditItemTemplate>
                                                        <FooterStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </div>
                                        <table width="100%" border="0" cellpadding="1" cellspacing="1">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red">Red Line item is not in Market. </asp:Label>
                                                </td>
                                            </tr>
                                        </table>
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
                                                        <asp:TextBox ID="txtnopage" MaxLength="5" Width="20px" runat="server" CssClass="input">
                                                        </asp:TextBox>
                                                        <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                                            Width="40px"></asp:Button>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvItemList" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
