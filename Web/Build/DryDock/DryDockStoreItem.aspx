<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockStoreItem.aspx.cs"
    Inherits="DryDockStoreItem" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Store Item List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmStoreItemList" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="ucTitle" Text="Stock Item" ShowMenu="false" />
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuStockItem" runat="server" OnTabStripCommand="MenuStockItem_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <br clear="all" />
    <asp:UpdatePanel runat="server" ID="pnlStockItem">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="search">
                <table width="100%">
                    <tr>
                        <td>
                           <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtNumberSearch" CssClass="input_mandatory" MaxLength="20"
                                Width="80px"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblStoreItemName" runat="server" Text="Store Item Name"></asp:Literal>
                            
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtStockItemNameSearch" CssClass="input" Width="240px"
                                Text=""></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblStoreType" runat="server" Text="Store Type"></asp:Literal>
                            
                        </td>
                        <td>
                            <eluc:UserControlHard ID="ddlStockClass" runat="server" Visible="true" CssClass="input"
                                AppendDataBoundItems="true" />
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
            </div>
            <div id="divGrid" style="position: relative;">
                <asp:GridView ID="gvStoreItem" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    Font-Size="11px" OnRowCommand="gvStoreItem_RowCommand" OnRowDataBound="gvStoreItem_ItemDataBound"
                    ShowFooter="false" ShowHeader="true" Width="100%" EnableViewState="false" AllowSorting="true"
                    OnSorting="gvStoreItem_Sorting">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:TemplateField HeaderText="Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                           <asp:Literal ID="lblNumber" runat="server" Text="Number"></asp:Literal>
                                 </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStockItemNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="StoreItem Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                           <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStockItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMID") %>'></asp:Label>
                                <asp:Label ID="lblIsInMarket" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISINMARKET") %>'></asp:Label>
                                <asp:LinkButton ID="lnkStockItemName" runat="server" CommandName="ADD" CommandArgument='<%# Container.DataItemIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Maker">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                           <asp:Literal ID="lblMaker" runat="server" Text="Maker"></asp:Literal>
                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblMaker" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKER") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ROB">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                           <asp:Literal ID="lblAvailable" runat="server" Text="Available"></asp:Literal>
                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDROB") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                           <asp:Literal ID="lblRequired" runat="server" Text="Required"></asp:Literal>
                                
                            </HeaderTemplate>
                            <ItemTemplate>
                                <eluc:Number ID="txtQuantity" runat="server" CssClass="input" Width="80px" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div id="divPage" style="position: relative;">
                <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle">
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
                            <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev"><< Prev </asp:LinkButton>
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
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvStoreItem" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
