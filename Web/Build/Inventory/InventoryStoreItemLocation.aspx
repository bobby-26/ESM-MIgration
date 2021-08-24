<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryStoreItemLocation.aspx.cs"
    Inherits="InventoryStoreItemLocation" MaintainScrollPositionOnPostback="true" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Store Item Location</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvStockItem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvLocation" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table>
                <tr style="position: relative; vertical-align: top">
                    <td>
                        <table cellpadding="0" cellspacing="0" style="float: left; width: 50%;">
                            <tr style="position: relative">
                                <td>
                                    <telerik:RadDropDownList ID="ddlLocationAdd" runat="server" Width="240px" CssClass="input"
                                        OnSelectedIndexChanged="ddlLocationAdd_SelectedIndexChanged" AutoPostBack="true">
                                        <Items>
                                            <telerik:DropDownListItem Text="From List" Value="1" Selected="true" />
                                            <telerik:DropDownListItem Text="From tree" Value="2" />
                                        </Items>
                                    </telerik:RadDropDownList>
                                </td>
                            </tr>
                            <tr style="position: relative">
                                <td>
                                    <div id="divLocationTree" runat="server">
                                        <eluc:TreeView runat="server" ID="tvwLocation" OnNodeClickEvent="tvwLocation_NodeClickEvent"></eluc:TreeView>
                                        <telerik:RadLabel runat="server" ID="lblSelectedNode"></telerik:RadLabel>
                                    </div>
                                    <div id="divLocationList" runat="server">
                                        <asp:Repeater ID="repLocationList" runat="server">
                                            <HeaderTemplate>
                                                <table border="1" cellpadding="0" cellspacing="0" width="100%">
                                                    <tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblLocationName" runat="server" Text="Location Name"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblLocationCode" runat="server" Text="Location Code"></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblLocationID" runat="server" Visible="false" Text='<%# Eval("FLDLOCATIONID")%>'></telerik:RadLabel>
                                                        <asp:LinkButton ID="lnkLocationName" runat="server" OnCommand="lnkLocationName" CommandArgument='<%# Eval("FLDLOCATIONID") + "," + Eval("FLDLOCATIONNAME") %>'
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME")%>'></asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <telerik:RadLabel ID="Label1" runat="server" Visible="true" Text='<%# Eval("FLDLOCATIONCODE")%>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </table>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <table id="tblmain" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 990px">
                                    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvStockItem" DecoratedControls="All" EnableRoundedCorners="true" />
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvLocation" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None"
                                        OnNeedDataSource="gvLocation_NeedDataSource"
                                        OnItemCommand="gvLocation_ItemCommand"
                                        OnItemDataBound="gvLocation_ItemDataBound"
                                        OnUpdateCommand="gvLocation_UpdateCommand"
                                        OnDeleteCommand="gvLocation_DeleteCommand">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" DataKeyNames="FLDSTOREITEMLOCATIONID" ShowFooter="true">
                                            <HeaderStyle Width="102px" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Location" HeaderStyle-Width="115px" Visible="false" AllowSorting="true" SortExpression="FLDUNITNAME">
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblStockItemLocationId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMLOCATIONID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblLocationId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblStockItemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMID") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <telerik:RadLabel ID="lblStockItemLocationIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOREITEMLOCATIONID") %>'></telerik:RadLabel>
                                                        <telerik:RadTextBox ID="txtLocationCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONID") %>'
                                                            CssClass="gridinput_mandatory" MaxLength="6">
                                                        </telerik:RadTextBox>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Default" HeaderStyle-Width="30px" AllowSorting="true">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgFlag" runat="server" Enabled="false" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" />
                                                        <%--<telerik:RadLabel ID="lblFlag" runat="server" Enabled="false"><i class="fas fa-star"></i></telerik:RadLabel>--%>
                                                        <telerik:RadLabel ID="lblLocationIsDefault" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISDEFAULT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Location" HeaderStyle-Width="70px" AllowSorting="true">
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblLocationName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'></telerik:RadLabel>
                                                        <asp:LinkButton ID="lnkLocationName" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <telerik:RadLabel ID="lblLocationNameEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'></telerik:RadLabel>
                                                        <telerik:RadTextBox ID="txtLocationNameEdit" ReadOnly="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'
                                                            CssClass="gridinput" MaxLength="200">
                                                        </telerik:RadTextBox>
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <telerik:RadTextBox ID="txtStockItemLocationIdAdd" CssClass="gridinput" runat="server" Visible="false"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKITEMLOCATIONID") %>'>
                                                        </telerik:RadTextBox>
                                                        <telerik:RadTextBox ID="txtLocationNameAdd" runat="server" ReadOnly="true"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONNAME") %>'>
                                                        </telerik:RadTextBox>
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Department" HeaderStyle-Width="70px" AllowSorting="true">
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDepartmentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn ItemStyle-HorizontalAlign="Right" FooterStyle-HorizontalAlign="Right" HeaderText="Current Stock" HeaderStyle-Width="70px" AllowSorting="true">
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblCurrentStock" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <%--<EditItemTemplate>
                                                        <telerik:RadLabel ID="lblItemQuantityEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                                        <eluc:Number ID="txtItemQuantityEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>' MaxLength="9" />
                                                    </EditItemTemplate>--%>
                                                    <FooterTemplate>
                                                        <telerik:RadTextBox ID="txtLocationIdAdd" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATIONID","{0:n0}") %>'></telerik:RadTextBox>
                                                        <%--<eluc:Number ID="txtItemQuantityAdd" Width="100px" runat="server" ReadOnly="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>' MaxLength="9" />--%>
                                                        <telerik:RadTextBox InputType="Number" Enabled="false" ID="txtItemQuantityAdd" Width="100px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>' MaxLength="9" />
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn UniqueName="Action" HeaderStyle-HorizontalAlign="Center" HeaderText="Action">
                                                    <HeaderStyle Width="40px" />
                                                    <ItemStyle Width="40px" Wrap="false" />
                                                    <ItemTemplate>
                                                       <%-- <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                                            <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                                        </asp:LinkButton>--%>
                                                        <asp:LinkButton runat="server" CommandName="DEFAULT" ID="cmdDefault" ToolTip="Set Default Location">
                                                            <span class="icon"><i class="fas fa-globe"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <%--<EditItemTemplate>
                                                        <asp:LinkButton runat="server" CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                                            <span class="icon"><i class="fas fa-save"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                                            <span class="icon"><i class="fas fa-times"></i></span>
                                                        </asp:LinkButton>
                                                    </EditItemTemplate>--%>
                                                    <FooterTemplate>
                                                        <asp:LinkButton runat="server" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                                             <span class="icon"><i class="fas fa-plus-square"></i></span>
                                                        </asp:LinkButton>
                                                    </FooterTemplate>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                                                PageSizeLabelText="Records per page:" />
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>

                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <b><i><telerik:RadLabel ID="lblTotalstockCaption" runat="server" Text="Total stock"></telerik:RadLabel></i></b>:
                                    <telerik:RadLabel ID="lblTotalStock" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <div id="divPage" style="float: none; margin-left: 400px">
                <br />
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
