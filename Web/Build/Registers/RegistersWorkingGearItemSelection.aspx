<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersWorkingGearItemSelection.aspx.cs"
    Inherits="RegistersWorkingGearItemSelection" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="WorkingGearType" Src="~/UserControls/UserControlWorkingGearType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Store Item List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmWorkingGearItemList" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <eluc:Title runat="server" ID="ucTitle" Text="Store Item" ShowMenu="false" />
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
                            <asp:Literal ID="lblItemName" runat="server" Text="Item Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemSearch" runat="server" MaxLength="100" CssClass="input" Width="300px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblGearType" runat="server" Text="Gear Type"></asp:Literal>
                        </td>
                        <td>
                            <eluc:WorkingGearType ID="ucWorkingGearType" CssClass="input" AppendDataBoundItems="true"
                                runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblPricerange" runat="server" Text="Price range"></asp:Literal>
                        </td>
                        <td colspan="3">
                             <asp:Literal ID="lblFrom" runat="server" Text="From"></asp:Literal>&nbsp;
                            <eluc:Number ID="txtPricefrom" runat="server" CssClass="input" Width="90px" IsPositive="true"
                                IsInteger="false" DecimalPlace="2" />
                            &nbsp; &nbsp;  <asp:Literal ID="lblTo" runat="server" Text="To"></asp:Literal>&nbsp;
                            <eluc:Number ID="txtPriceto" runat="server" CssClass="input" Width="90px" IsPositive="true"
                                IsInteger="false" DecimalPlace="2" />
                        </td>
                    </tr>
                </table>
            </div>
            <asp:GridView ID="gvWorkingGearItem" runat="server" AutoGenerateColumns="False" CellPadding="3"
                Font-Size="11px" OnRowDataBound="gvWorkingGearItem_RowDataBound" ShowFooter="false"
                OnRowEditing="gvWorkingGearItem_RowEditing" OnRowCancelingEdit="gvWorkingGearItem_RowCancelingEdit"
                OnRowUpdating="gvWorkingGearItem_RowUpdating" ShowHeader="true" Width="100%"
                EnableViewState="false" AllowSorting="true" OnSorting="gvWorkingGearItem_Sorting"
                DataKeyNames="FLDWorkingGearItemID">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                    <asp:TemplateField HeaderText="WorkingGearItem Name">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <asp:LinkButton ID="lblWorkingGearItemNameHeader" runat="server" CommandName="Sort"
                                CommandArgument="FLDWORKINGGEARITEMNAME" ForeColor="White">Working Gear Item Name&nbsp;</asp:LinkButton>
                            <img id="FLDWORKINGGEARITEMNAME" runat="server" visible="false" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDWORKINGGEARITEMNAME")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Unit">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDUNITNAME")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDUNITNAME")%>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Price">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDUNITPRICE")%>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDUNITPRICE")%>
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Quantity">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtQuantity" runat="server" CssClass="input" Width="80px" IsInteger="true"
                                IsPositive="true" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                        </HeaderTemplate>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                ToolTip="Edit"></asp:ImageButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                CommandName="Update" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdUpdate"
                                ToolTip="Update"></asp:ImageButton>
                            <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                width="3" />
                            <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                CommandName="Cancel" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                ToolTip="Cancel"></asp:ImageButton>
                        </EditItemTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
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
            <asp:PostBackTrigger ControlID="gvWorkingGearItem" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
