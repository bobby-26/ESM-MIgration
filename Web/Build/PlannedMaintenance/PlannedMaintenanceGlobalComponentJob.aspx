<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceGlobalComponentJob.aspx.cs" Inherits="PlannedMaintenanceGlobalComponentJob" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Component</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized() {
            var sender = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            sender.set_height(browserHeight - 05);
            $telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 35);
        }
    </script>
</head>
<body onresize="PaneResized();" onload="PaneResized();">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadPersistenceManager runat="server" ID="RadPersistenceManager1">
            <PersistenceSettings>
                <telerik:PersistenceSetting ControlID="tvwComponent" />
            </PersistenceSettings>
        </telerik:RadPersistenceManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="tvwComponent">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvPlannedMaintenanceJob" UpdatePanelHeight="94%"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuJob">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuComponent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuGeneral"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="tvwComponent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadAjaxPanelEdit" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuJob"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvPlannedMaintenanceJob" UpdatePanelHeight="94%"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvPlannedMaintenanceJob">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucStatus"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuJob"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvPlannedMaintenanceJob" UpdatePanelHeight="94%"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
        <div style="height: 100%; margin-left: auto; margin-right: auto; vertical-align: middle;">
            <%--<div style="font-weight:600;font-size:12px" runat="server">
                <eluc:TabStrip ID="MenuComponent" runat="server" OnTabStripCommand="MenuComponent_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
            </div>--%>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="200px" Width="100%">
                <telerik:RadPane ID="navigationPane" runat="server" Width="400">
                    <%--<eluc:TreeView ID="tvwComponent" runat="server" OnNodeClickEvent="tvwComponent_NodeClickEvent" OnNodeDataBoundEvent="tvwComponent_NodeDataBoundEvent" SearchEmptyMessage="Type to search component"/>--%>
                    <div>
                        <div class="rdTreeFilter" runat="server" id="divTreeFilter">
                            <telerik:RadTextBox ClientEvents-OnLoad="telerik.clientTreeSearch" ID="treeViewSearch" runat="server" Width="100%" EmptyMessage="Type to search Component" />
                        </div>
                        <div class="rdTreeScroll">
                            <telerik:RadTreeView RenderMode="Lightweight" ID="tvwComponent" runat="server" OnNodeDataBound="tvwComponent_NodeDataBoundEvent"
                                OnNodeClick="tvwComponent_NodeClickEvent" AllowNodeEditing="false">
                                <ExpandAnimation Type="None" />
                                <CollapseAnimation Type="None" />
                                <DataBindings>
                                    <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                                </DataBindings>
                            </telerik:RadTreeView>
                        </div>
                    </div>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="contentPane" runat="server">
                    <eluc:TabStrip ID="MenuJob" runat="server" OnTabStripCommand="MenuJob_TabStripCommand"
                        TabStrip="false" Title="Jobs"></eluc:TabStrip>
                    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvPlannedMaintenanceJob" DecoratedControls="All" EnableRoundedCorners="true" />
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvPlannedMaintenanceJob" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="100%" AllowFilteringByColumn="true" FilterItemStyle-Width="100%"
                        ShowGroupPanel="false" CellSpacing="0" GridLines="None" OnNeedDataSource="gvPlannedMaintenanceJob_NeedDataSource" OnSortCommand="gvPlannedMaintenanceJob_SortCommand"
                        OnItemDataBound="gvPlannedMaintenanceJob_ItemDataBound" OnItemCommand="gvPlannedMaintenanceJob_ItemCommand" EnableLinqExpressions="false" AllowMultiRowSelection="true">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDJOBID,FLDGLOBALCOMPONENTJOBID,FLDDTKEY,FLDGLOBALCOMPONENTID">
                            <HeaderStyle Width="102px" />
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="Component" Name="COMPONENT" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="Job" Name="JOBS" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Number" AllowFiltering="false" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNUMBER" ColumnGroupName="COMPONENT" UniqueName="COMPONENTNUMBER">
                                    <HeaderStyle Width="90px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblNumber" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" AllowFiltering="false" ShowSortIcon="true" SortExpression="FLDCOMPONENTNAME" ColumnGroupName="COMPONENT" UniqueName="COMPONENTNAME">
                                    <HeaderStyle Width="180px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="180px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblName" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" AllowFiltering="true" FilterDelay="5000" ShowFilterIcon="false" FilterControlWidth="100%" ShowSortIcon="true" SortExpression="FLDJOBCODE" ColumnGroupName="JOBS" UniqueName="CODE">
                                    <HeaderStyle Width="70px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCode" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Title" AllowSorting="true" ShowSortIcon="true" AllowFiltering="true" FilterDelay="5000" ShowFilterIcon="false" FilterControlWidth="100%" SortExpression="FLDJOBTITLE" ColumnGroupName="JOBS" UniqueName="TITLE">
                                    <HeaderStyle Width="300px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="300px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Category" ColumnGroupName="JOBS" UniqueName="CATEGORY" AllowFiltering="false">
                                    <HeaderStyle Width="180px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                 <telerik:GridTemplateColumn HeaderText="Frequency" AllowFiltering="false" AllowSorting="true" ShowSortIcon="true" >
                                    <HeaderStyle Width="90px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                    <ItemTemplate>
                                      <telerik:RadLabel ID="lblfrequency" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYNAME") %>'></telerik:RadLabel>
                                 </ItemTemplate>
                                </telerik:GridTemplateColumn>
                               
                                <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ACTION" AllowFiltering="false">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
                                    <ItemTemplate>
                                         <telerik:RadCheckBox ID="ChkComJob" runat="server"></telerik:RadCheckBox>
                                        <asp:LinkButton runat="server" AlternateText="Add" Visible="false"
                                            CommandName="ADD" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                            ToolTip="Add" Width="20PX" Height="20PX">
                                            <span class="icon"><i class="fas fa-plus"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Job Description" Visible="false"
                                            CommandName="JOBDESCRIPTION" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdJobDesc"
                                            ToolTip="Job Description" Width="20PX" Height="20PX">
                                        <span class="icon"><i class="fas fa-list-alt"></i></span>
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
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                                PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="false" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </telerik:RadPane>
            </telerik:RadSplitter>
        </div>
    </form>
</body>
</html>
