<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseOrderLineItemServiceSelection.aspx.cs"
    Inherits="PurchaseOrderLineItemServiceSelection" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ComponentTreeView" Src="~/UserControls/UserControlTreeView.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlPurchaseUnit.ascx" %>
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
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td>
                       <asp:Literal ID="lblWorkOrderNumber" runat="server" Text="Work Order Number"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPartNumber" runat="server" CssClass="input"></asp:TextBox>
                    </td>
                    <td>
                       <asp:Literal ID="lblWorkOrderName" runat="server" Text="Work Order Name"></asp:Literal>
                        
                    </td>
                    <td>
                        <asp:TextBox ID="txtItemName" runat="server" CssClass="input"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                       <asp:Literal ID="lblComponent" runat="server" Text="Component"></asp:Literal>
                        
                    </td>
                    <td colspan="3">
                        <span id="spnPickComponent">
                            <asp:TextBox ID="txtComponent" runat="server" Width="90px" Enabled="false" CssClass="input"></asp:TextBox>
                            <asp:TextBox ID="txtComponentName" runat="server" Width="180px" Enabled="false" CssClass="input"></asp:TextBox>
                            <asp:ImageButton runat="server" ID="cmdShowComponent" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnPickComponent', 'codehelp1', '', '../Common/CommonPickListComponent.aspx', true);"
                                Text=".." />
                            <asp:TextBox ID="txtComponentID" runat="server" Width="16px" />
                        </span>
                    </td>
                </tr>
            </table>
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
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblOrderNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDWORKORDERNUMBER"
                                    ForeColor="White">Work Order Number</asp:LinkButton>
                                <img id="FLDNAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblWorkOrderId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'
                                    Visible="false"></asp:Label>
                                <asp:Label runat="server" ID="lblWorkOrderNumber" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'
                                    Visible="false"></asp:Label>
                                <asp:LinkButton ID="lnkWorkOrderNumber" runat="server" CommandArgument='<%# Container.DataItemIndex %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblOrderTitleHeader" runat="server">Work Order Description </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'
                                    Visible="true"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblComponentNumberHeader" runat="server">Component Number </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblComponentNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'
                                    Visible="true"></asp:Label>
                                <asp:Label ID="lblComponentid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'
                                    Visible="false"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:Label ID="lblComponentHeader" runat="server">Component Name </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblComponent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'
                                    Visible="true"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Unit">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblWantedUnitHeader" runat="server">Unit
                                </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="txtUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Unit ID="ucUnit" AppendDataBoundItems="true" CssClass="input_mandatory" runat="server"
                                     SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>' 
                                  PurchaseUnitList="<%# PhoenixRegistersUnit.ListPurchaseUnit(null,null,0) %>" />
                            </EditItemTemplate>
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
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
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
                            <asp:TextBox ID="txtnopage" MaxLength="5" Width="20px" runat="server" CssClass="input">
                            </asp:TextBox>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                Width="40px"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="gvItemList" />
        </Triggers>
    </asp:UpdatePanel>
    </form>
</body>
</html>
