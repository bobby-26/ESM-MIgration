<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceGlobalComponentTypeList.aspx.cs" Inherits="PlannedMaintenanceGlobalComponentTypeList" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Component Type</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>
            <script type="text/javascript">
                function PaneResized(sender, args) {
                    var browserHeight = $telerik.$(window).height();
                    var grid = $find("gvPlannedMaintenanceJob");
                    grid._gridDataDiv.style.height = (browserHeight - 135) + "px";
                }
                function pageLoad() {
                    PaneResized();
                }

        </script>
</head>
<body onresize="PaneResized();" onload="PaneResized();">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"/>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
          <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
        <%--<telerik:RadAjaxManager ID="RadAjaxManager" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID ="gvPlannedMaintenanceJob">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvPlannedMaintenanceJob" UpdatePanelHeight="90%" />
                    </UpdatedControls>
                </telerik:AjaxSetting> 
            </AjaxSettings>
        </telerik:RadAjaxManager>--%>
        <%--<eluc:TabStrip ID="MenuComponent" runat="server" OnTabStripCommand="MenuComponent_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>--%>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvPlannedMaintenanceJob" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvPlannedMaintenanceJob" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" AllowFilteringByColumn="true" FilterItemStyle-Width="100%"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvPlannedMaintenanceJob_NeedDataSource" OnPreRender="gvPlannedMaintenanceJob_PreRender"
            OnItemDataBound="gvPlannedMaintenanceJob_ItemDataBound" OnItemCommand="gvPlannedMaintenanceJob_ItemCommand" OnSortCommand="gvPlannedMaintenanceJob_SortCommand" EnableLinqExpressions="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true" />
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="true"
                CommandItemDisplay="None" AutoGenerateColumns="false" DataKeyNames="FLDGLOBALCOMPONENTTYPEID,FLDDTKEY,FLDGLOBALCOMPONENTID" EnableLinqGrouping="false">
                <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToExcelButton="false" ShowPrintButton="false" RefreshText="Search" />
                <HeaderStyle Width="102px" />
                <ColumnGroups>
                    <telerik:GridColumnGroup HeaderText="Type" Name="TYPE" HeaderStyle-HorizontalAlign="Center">
                    </telerik:GridColumnGroup>
                    <telerik:GridColumnGroup HeaderText="Component" Name="COMPONENT" HeaderStyle-HorizontalAlign="Center">
                    </telerik:GridColumnGroup>
                </ColumnGroups>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ACTION" AllowFiltering="false">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Manuals"
                                            CommandName="MANUALS" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdManuals"
                                            ToolTip="Manuals" Width="20PX" Height="20PX">
                                            <span class="icon"><i class="fas fa-book"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Jobs"
                                            CommandName="JOBS" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdJobs"
                                            ToolTip="Jobs" Width="20PX" Height="20PX">
                                            <span class="icon"><i class="fas fa-tools"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Delete"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete" 
                                            ToolTip="Delete" Width="20PX" Height="20PX">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Make" ColumnGroupName="TYPE" UniqueName="MAKE" Groupable="true" FilterDelay="5000"
                        AllowFiltering="true" ShowFilterIcon="false" FilterControlWidth="100%">
                        <HeaderStyle Width="90px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMake" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Model/Type" ColumnGroupName="TYPE" UniqueName="TYPE" Groupable="true" FilterDelay="5000"
                        AllowFiltering="true" ShowFilterIcon="false" FilterControlWidth="100%">
                        <HeaderStyle Width="90px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblType" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Number" AllowSorting="true" ShowSortIcon="true" FilterControlWidth="100%" FilterDelay="5000"
                        AllowFiltering="true" ShowFilterIcon="false" SortExpression="FLDCOMPONENTNUMBER" ColumnGroupName="COMPONENT" UniqueName="COMPONENTNUMBER">
                        <HeaderStyle Width="90px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNumber" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" ShowSortIcon="true" FilterControlWidth="100%" FilterDelay="5000"
                        AllowFiltering="true" ShowFilterIcon="false" SortExpression="FLDCOMPONENTNAME" ColumnGroupName="COMPONENT" UniqueName="COMPONENTNAME">
                        <HeaderStyle Width="180px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="180px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblName" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Category" ColumnGroupName="COMPONENT" UniqueName="CATEGORY" Groupable="false" AllowFiltering="false">
                        <HeaderStyle Width="180px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
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

                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" AllowDragToGroup="false" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
              </telerik:RadAjaxPanel>
    </form>
</body>
</html>
