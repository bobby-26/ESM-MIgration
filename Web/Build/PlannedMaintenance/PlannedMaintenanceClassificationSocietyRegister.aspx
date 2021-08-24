<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceClassificationSocietyRegister.aspx.cs" Inherits="PlannedMaintenanceClassificationSocietyRegister" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Class Codes</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvClassMap">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvClassMap" UpdatePanelHeight="93%" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>

        <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuPlannedMaintenance" runat="server" OnTabStripCommand="MenuPlannedMaintenance_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvClassMap" DecoratedControls="All" EnableRoundedCorners="true" />

            <telerik:RadGrid RenderMode="Lightweight" ID="gvClassMap" runat="server" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" Height="100%" ShowFooter="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvClassMap_NeedDataSource" OnItemCommand="gvClassMap_ItemCommand" >
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDID,FLDADDRESSCODE" CommandItemDisplay="None">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ACTION">
                            <ItemStyle Width="90px" />
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                 <asp:LinkButton runat="server" AlternateText="Delete"
                                CommandName="DELETE" ID="cmdDelete"
                                ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                            </ItemTemplate>
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add"
                                CommandName="ADD" ID="cmdAdd"
                                ToolTip="Add">
                                            <span class="icon"><i class="fas fa-plus-circle"></i></span>
                            </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" UniqueName="NAME">
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Address ID="ucAddress" runat="server" AddressType="137" Width="240px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowKeyboardNavigation="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            </div>
    </form>
</body>
</html>
