<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSundryPurchaseRequirementItem.aspx.cs"
    Inherits="AccountsSundryPurchaseRequirementItem" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Store Item List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmStoreItemList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
       

        <eluc:TabStrip ID="MenuStockItem" runat="server" OnTabStripCommand="MenuStockItem_TabStripCommand"></eluc:TabStrip>

        <br clear="all" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="98%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <%--    <div id="search">--%>
                    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtNumberSearch" CssClass="input_mandatory" MaxLength="20"
                                    Width="80px">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStoreItemName" runat="server" Text="Store Item Name"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtStockItemNameSearch" CssClass="input" Width="240px"
                                    Text="">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblStoreType" runat="server" Text="Store Type"></telerik:RadLabel>
                            </td>
                            <td colspan="3">
                                <eluc:UserControlHard ID="ddlStockClass" runat="server" Visible="true" CssClass="input"
                                    AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
             <%--   </div>--%>
                <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
                <telerik:RadGrid RenderMode="Lightweight" ID="gvStoreItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvStoreItem_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" OnSelectedIndexChanging="gvStoreItem_SelectedIndexChanging"
                    OnItemDataBound="gvStoreItem_ItemDataBound" OnItemCommand="gvStoreItem_ItemCommand"
                    ShowFooter="false" ShowHeader="true">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDSTOREITEMID">
                        <Columns>
                            <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                            <telerik:GridTemplateColumn HeaderText="Number" AllowSorting="true" SortExpression="FLDNUMBER">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <%--<HeaderTemplate>
                                <asp:LinkButton ID="lblnumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDNUMBER"
                                    ForeColor="White">Number&nbsp;</asp:LinkButton>
                                <img id="FLDNUMBER" runat="server" visible="false" />
                            </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStoreitemid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblStockItemNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Store Item Name" AllowSorting="true" SortExpression="FLDNAME">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <%--  <HeaderTemplate>
                                <asp:LinkButton ID="lblStoreItemNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                    ForeColor="White">Store Item Name&nbsp;</asp:LinkButton>
                                <img id="FLDNAME" runat="server" visible="false" />
                            </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Maker" AllowSorting="true" SortExpression="FLDMAKER">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <%--  <HeaderTemplate>
                                <asp:LinkButton ID="lblMakerHeader" runat="server" CommandName="Sort" CommandArgument="FLDMAKER"
                                    ForeColor="White">Maker&nbsp;</asp:LinkButton>
                                <img id="FLDMAKER" runat="server" visible="false" />
                            </HeaderTemplate>--%>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDMAKER") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="ROB">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDROB") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Quantity">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtQuantity" runat="server" CssClass="input" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>' />
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdUpdate"
                                        ToolTip="Update"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                            </telerik:GridTemplateColumn>
                         
                        </Columns>

                           
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                        EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                
                </telerik:RadGrid>
                   <table width="100%" border="0" cellpadding="1" cellspacing="1">
                            <tr>
                                <td>
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red">Red Line item is not in Market.</asp:Label>
                               </td>
                            </tr>
                        </table>
         </telerik:RadAjaxPanel>
    </form>
</body>
</html>



<%--  <asp:GridView ID="gvStoreItem" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    Font-Size="11px" OnRowDataBound="gvStoreItem_RowDataBound" ShowFooter="false" OnRowEditing="gvStoreItem_RowEditing"
                    OnRowCancelingEdit="gvStoreItem_RowCancelingEdit" OnRowUpdating="gvStoreItem_RowUpdating"
                    ShowHeader="true" Width="100%" EnableViewState="false" AllowSorting="true" OnSorting="gvStoreItem_Sorting"
                    DataKeyNames="FLDSTOREITEMID">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                        <asp:TemplateField HeaderText="Number">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblnumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDNUMBER"
                                    ForeColor="White">Number&nbsp;</asp:LinkButton>
                                <img id="FLDNUMBER" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblStockItemNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="StoreItem Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblStoreItemNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                    ForeColor="White">Store Item Name&nbsp;</asp:LinkButton>
                                <img id="FLDNAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Maker">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblMakerHeader" runat="server" CommandName="Sort" CommandArgument="FLDMAKER"
                                    ForeColor="White">Maker&nbsp;</asp:LinkButton>
                                <img id="FLDMAKER" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDMAKER") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="ROB">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDROB") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Quantity">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtQuantity" runat="server" CssClass="input" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>' />
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
                            <td width="20px">&nbsp;
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
--%>

