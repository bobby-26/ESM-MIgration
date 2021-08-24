<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceRotation.aspx.cs" Inherits="PlannedMaintenanceRotation" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                TelerikGridResize('gridtable');
                }, 200);
            }

            function confirm(args) {
                if (args) {
                    __doPostBack("<%= btnConfirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPlannedMaintenanceJob" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="pnlComponentGeneral" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MenuMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuPMS" runat="server" OnTabStripCommand="PMS_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellpadding="1" cellspacing="1" style="height:100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblrotationdate" runat="server" Text="Rotated On"></telerik:RadLabel>
                        &nbsp;&nbsp;&nbsp;
                        <eluc:Date ID="txtrotationdate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <eluc:TabStrip ID="MenuPMSRotation" runat="server" OnTabStripCommand="PMSRotation_TabStripCommand" Title="To be Rotated"></eluc:TabStrip>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvRotation" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                CellSpacing="0" GridLines="None" OnNeedDataSource="gvRotation_NeedDataSource" GroupingEnabled="false" EnableHeaderContextMenu="true" SelectedItemStyle-Font-Bold="true" SelectedItemStyle-Font-Size="12px"
                                OnItemCommand="gvRotation_ItemCommand" OnItemDataBound="gvRotation_ItemDataBound" DataKeyNames="FLDCOMPONENTID" EnableLinqExpressions="false">
                                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDCOMPONENTID" AllowFilteringByColumn="true">
                                    <HeaderStyle Width="102px" />
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Number" AllowFiltering="true" UniqueName="FLDCOMPONENTNUMBER" ShowFilterIcon="false" 
                                            CurrentFilterFunction="Contains" FilterDelay="3000" FilterControlWidth="100%">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="txtRoationCompId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>' />
                                                <%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Name" AllowFiltering="true" UniqueName="FLDCOMPONENTNAME" ShowFilterIcon="false" 
                                            CurrentFilterFunction="Contains" FilterDelay="3000" FilterControlWidth="100%">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn AllowFiltering="false">
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" AlternateText="Select" ImageUrl="<%$ PhoenixTheme:images/select.png %>"
                                                    CommandName="SELECT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                                    ToolTip="Select"></asp:ImageButton>
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
                                    <PagerStyle Mode="NextPrevAndNumeric" Width="50px" PageButtonCount="2" AlwaysVisible="true" PageSizeLabelText="Records per page:"
                                        CssClass=" RadGrid_Default rgPagerTextBox" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="320px" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                    </td>
                    <td valign="top">
                        <eluc:TabStrip ID="MenuPMSRotation1" runat="server" OnTabStripCommand="PMSRotation1_TabStripCommand" Title="Replaced By"></eluc:TabStrip>
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvRotation1" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvRotation1_NeedDataSource" DataKeyNames="FLDCOMPONENTID" SelectedItemStyle-Font-Bold="true" SelectedItemStyle-Font-Size="12px"
                                    OnItemCommand="gvRotation1_ItemCommand" OnItemDataBound="gvRotation1_ItemDataBound" GroupingEnabled="false" EnableHeaderContextMenu="true" EnableLinqExpressions="false">
                                    <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDCOMPONENTID" AllowFilteringByColumn="true">
                                        <HeaderStyle Width="102px" />
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Number" AllowFiltering="true" ShowFilterIcon="false" UniqueName="FLDCOMPONENTNUMBER" 
                                                CurrentFilterFunction="Contains" FilterDelay="3000" FilterControlWidth="100%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:Label ID="txtRoation1CompId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>' />
                                                    <%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Name" AllowFiltering="true" ShowFilterIcon="false" UniqueName="FLDCOMPONENTNAME" 
                                                CurrentFilterFunction="Contains" FilterDelay="3000" FilterControlWidth="100%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn AllowFiltering="false">
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                                                    <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Select" ImageUrl="<%$ PhoenixTheme:images/select.png %>"
                                                        CommandName="SELECT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                                        ToolTip="Select"></asp:ImageButton>
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
                                        <PagerStyle Mode="NextPrevAndNumeric" AlwaysVisible="true" PageButtonCount="2" PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                                        <%-- PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"--%>
                                    </MasterTableView>
                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="320px" />
                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                    </ClientSettings>
                                </telerik:RadGrid>
                    </td>
                </tr>
            </table>

            <eluc:Status ID="ucStatus" runat="server" />
            
        </telerik:RadAjaxPanel>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
              Sys.Application.add_load(function () {
                    setTimeout(function () {
                        TelerikGridResize($find("<%= gvRotation.ClientID %>"), null, 98);
                        TelerikGridResize($find("<%= gvRotation1.ClientID %>"),null,98);
                    }, 200);
                });
            </script>
        </telerik:RadCodeBlock>
            <asp:Button ID="btnConfirm" runat="server" Text="confirm" OnClick="btnConfirm_Click"/>
            <asp:Button ID="btnCancel" runat="server" Text="confirm" OnClick="btnCancel_Click"/>
    </form>
</body>
</html>
