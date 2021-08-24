<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseVendorProduct.aspx.cs" Inherits="PurchaseVendorProduct" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Mask" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vendor Product</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var browserHeight = $telerik.$(window).height();
            var grid = $find("rgvProduct");           
            grid._gridDataDiv.style.height = (browserHeight - 180) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>

</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmRegistersVendorProduct" runat="server">
     <telerik:RadScriptManager ID="RadScriptManager" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxManager ID="RadAjaxManager" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgvProduct">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvProduct" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>

        </telerik:RadAjaxManager>

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadNotification ID="ucStatus" RenderMode="Lightweight" runat="server" AutoCloseDelay="3500" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"  
            EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="TopCenter" Animation="Fade" ShowTitleMenu="false" ></telerik:RadNotification>
        <eluc:TabStrip ID="MenuPurchaseVendor" runat="server" OnTabStripCommand="MenuPurchaseVendor_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuRegistersVendorProduct" runat="server" OnTabStripCommand="RegistersVendorProduct_TabStripCommand">
                    </eluc:TabStrip>

        <telerik:RadFormDecorator RenderMode="Lightweight" ID="FormDecorator"  runat="server" DecorationZoneID="rgvProduct" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="rgvProduct" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnSortCommand="rgvProduct_SortCommand" OnNeedDataSource="rgvProduct_NeedDataSource"  
            OnItemDataBound="rgvProduct_ItemDataBound" OnItemCommand="rgvProduct_ItemCommand" ShowFooter="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDVENDORPRODUCTID" TableLayout="Fixed">
                <Columns>
                            <telerik:GridTemplateColumn HeaderText="Number" UniqueName="NUMBER">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblSFINumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSFINUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Mask ID="txtNumber" MaskText="##.##.##" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSFINUMBER") %>' runat="server" Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>                                    
                                    <eluc:Mask ID="txtNumberAdd" MaskText="##.##.##" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSFINUMBER") %>' runat="server" Width="100%" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn UniqueName="CODE" HeaderText="Code" AllowSorting="true" SortExpression="FLDPRODUCTCODE">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lnkDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtProductCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTCODE") %>'
                                        CssClass="gridinput_mandatory" MaxLength="50" Width="100%"></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtProductCodeAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="50"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Name" UniqueName="NAME" AllowSorting="true" SortExpression="FLDPRODUCTNAME">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblProductName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtProductNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTNAME") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200"></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtProductNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Group" UniqueName="Group">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTGROUP") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtProductGroupEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTGROUP") %>'></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtProductGroupAdd" runat="server" CssClass="input_mandatory" MaxLength="50"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Description" UniqueName="DESCRIPTION">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtDescriptionEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtDescriptionAdd" runat="server" CssClass="gridinput_mandatory"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Unit" UniqueName="UNIT"> 
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                <HeaderStyle HorizontalAlign="Left" Width="10%" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Unit ID="ucUnitEdit" runat="server" AppendDataBoundItems="true" CssClass="gridinput_mandatory" Width="100%" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Unit ID="ucUnitAdd" runat="server" AppendDataBoundItems="true" CssClass="gridinput_mandatory" Width="100%" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ACTION">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.ItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.ItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument="<%# Container.ItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.ItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument="<%# Container.ItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                    PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" EnablePostBackOnRowClick="false" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                <%--<ClientEvents OnGridCreated="GridCreated" />--%>
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
