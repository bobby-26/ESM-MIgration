<%@ Page Language="C#" AutoEventWireup="True" CodeFile="RegistersWorkingGearItem.aspx.cs"
    Inherits="RegistersWorkingGearItem" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="WorkingGearType" Src="~/UserControls/UserControlWorkingGearType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="WorkingGearItemType" Src="~/UserControls/UserControlWorkingGearItemType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Size" Src="~/UserControls/UserControlSize.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Working Gear Items</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkingGearAdditionalItems" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTitle" runat="server" Title="Working Gear Item"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="93%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="" OnClick="cmdHiddenSubmit_Click" />
            <table id="tblConfigureWorkingGearItem" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblItemName" runat="server" Text="Working Gear Item"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtItemSearch" runat="server" MaxLength="100" Width="300px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblGearType" runat="server" Text="Working Gear Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:WorkingGearType ID="ucWorkingGearType" AppendDataBoundItems="true" runat="server" />
                    </td>

                </tr>
            </table>
            <eluc:TabStrip ID="MenuRegistersWorkingGearItem" runat="server" OnTabStripCommand="RegistersWorkingGearItem_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRegistersworkinggearitem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvRegistersworkinggearitem_ItemCommand" OnNeedDataSource="gvRegistersworkinggearitem_NeedDataSource" Height="85%"
                OnItemDataBound="gvRegistersworkinggearitem_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Working Gear Item" HeaderStyle-Width="300px" SortExpression="FLDWORKINGGEARITEMNAME" AllowSorting="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                        
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkingGearitemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkWorkingGearItemName" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblItemidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkWorkingGearItemNameedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>                                
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="300px" HeaderText="Working Gear Item Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                        
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGearitemTypeiditem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMTYPENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblGearitemTypeidedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMTYPEID") %>'></telerik:RadLabel>
                                <eluc:WorkingGearItemType ID="ucWorkingGearItemTypesEdit" AppendDataBoundItems="true" runat="server"
                                    CssClass="gridinput_mandatory" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:WorkingGearItemType ID="ucWorkingGearItemTypesAdd" AppendDataBoundItems="true" runat="server"
                                    TypeList='<%#PhoenixRegistersWorkingGearItemType.ListWorkingGearType(null)%>' CssClass="gridinput_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="130px" HeaderText="Size">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>                            
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSizeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSize" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblSize" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZENAME") %>'></telerik:RadLabel>
                                <eluc:Size ID="ucSizeEdit" runat="server" AppendDataBoundItems="true" CssClass="gridinput_mandatory" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Size ID="ucSizeAdd" runat="server" AppendDataBoundItems="true" SizeList='<%#PhoenixRegistersSize.ListSize() %>'
                                    CssClass="gridinput_mandatory" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="Action">                           
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" ToolTip="Add New" Width="20PX" Height="20PX"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>                            
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
