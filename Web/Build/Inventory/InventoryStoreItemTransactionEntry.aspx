<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryStoreItemTransactionEntry.aspx.cs"
    Inherits="InventoryStoreItemTransactionEntry" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title></title>
   <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server"  autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlStoreItemTransactionEntry">
        <ContentTemplate>
             <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
              <div class="subHeader" style="position: relative">
                    <div id="div2" style="vertical-align: top">
                        <eluc:Title runat="server" ID="Title1" Text="Store Transaction" ShowMenu="false"></eluc:Title>
                    </div>
                    <div>
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit"   />
                    </div>
                </div>
                 <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuStoreItemTransactionEntry" runat="server" OnTabStripCommand="MenuStoreItemTransactionEntry_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                
                 <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuGridStoreItemTransactionEntry" runat="server" OnTabStripCommand="StoreItemTransactionEntry_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                
                <div id="divGrid" style="position: relative;">
                    <asp:GridView ID="gvStoreTransactionEntry" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvStoreTransactionEntry_RowCommand" OnRowDataBound="gvStoreTransactionEntry_ItemDataBound"
                        OnRowCancelingEdit="gvStoreTransactionEntry_RowCancelingEdit" OnRowDeleting="gvStoreTransactionEntry_RowDeleting"
                        OnRowEditing="gvStoreTransactionEntry_RowEditing" Style="margin-bottom: 0px">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTransactionDateHeader" runat="server">Transaction Date&nbsp;</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStoreItemDispositionHeaderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMDISPOSITIONHEADERID") %>'></asp:Label>
                                    <asp:Label ID="lblStoreItemDispositionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMDISPOSITIONID") %>'></asp:Label>
                                    <asp:Label ID="lblStoreItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMID") %>'></asp:Label>
                                    <asp:Label ID="lblTransactionDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPOSITIODATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblItemNumberHeader" runat="server">Item Number&nbsp;<asp:ImageButton
                                        runat="server" ID="cmdItemNumberDsc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click"
                                        CommandName="FLDNUMBER" CommandArgument="1" AlternateText="Item Number" />
                                        <asp:ImageButton runat="server" ID="cmdItemNumberAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                            OnClick="cmdSort_Click" CommandName="FLDNUMBER" CommandArgument="0" />
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblItemNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblItemNameHeader" runat="server">Item Name&nbsp;<asp:ImageButton
                                        runat="server" ID="cmdItemNameDsc" ImageUrl="<%$ PhoenixTheme:images/arrowDown.png %>" OnClick="cmdSort_Click"
                                        CommandName="FLDNAME" CommandArgument="1" AlternateText="Item Name" />
                                        <asp:ImageButton runat="server" ID="cmdItemNameAsc" ImageUrl="<%$ PhoenixTheme:images/arrowUp.png %>"
                                            OnClick="cmdSort_Click" CommandName="FLDNAME" CommandArgument="0" />
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblItemName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblTransactionTypeHeader" runat="server">Transaction Type </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblTransactionType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TRANSACTIONTYPENAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                             <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDispositionQuantityHeader" runat="server">Quantity</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblDispositionQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISPOSITIONQUANTITY") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField> 

                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblComponentNumberHeader" runat="server">Component Number</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblComponentNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.COMPONENTNUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblWorkOrderHeader" runat="server">Work Order</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblWorkOrder" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.WORKORDERTYPENAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblReportedByHeader" runat="server">Reported By</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReportedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.REPORTEDBY") %>'></asp:Label>
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
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Save" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
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
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
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
            <asp:PostBackTrigger ControlID="gvStoreTransactionEntry" />
        </Triggers>
    </asp:UpdatePanel>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
