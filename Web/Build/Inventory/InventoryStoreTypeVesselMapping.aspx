<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventoryStoreTypeVesselMapping.aspx.cs"
    Inherits="InventoryStoreTypeVesselMapping" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>StoreType Vessel mapping</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmStoreTypeVesselMapping" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuGridStoreType" runat="server"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvStoreTypeVesselMapping" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvStoreTypeVesselMapping_NeedDataSource" OnItemDataBound="gvStoreTypeVesselMapping_ItemDataBound"
            OnItemCommand="gvStoreTypeVesselMapping_ItemCommand" OnSortCommand="gvStoreTypeVesselMapping_SortCommand" GroupingEnabled="false" EnableHeaderContextMenu="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDSTORETYPEID">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Vessel Code" HeaderStyle-Width="30%" AllowSorting="true" SortExpression="FLDVESSELCODE">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblStoreTypeVesselMappingId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTORETYPEVESSELMAPPINGID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVesselCode" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel Name" HeaderStyle-Width="60%" AllowSorting="true" SortExpression="FLDVESSELNAME">
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>
                             <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                        <HeaderStyle Width="10%" />
                        <ItemStyle Width="10%" Wrap="false" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete"
                                CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
                                        <span class="icon"><i class="fas fa-trash-alt"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="true"
                    PageSizeLabelText="Records per page:" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
