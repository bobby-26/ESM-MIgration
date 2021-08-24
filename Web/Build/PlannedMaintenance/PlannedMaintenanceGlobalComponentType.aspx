<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceGlobalComponentType.aspx.cs" Inherits="PlannedMaintenanceGlobalComponentType" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Component Type</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery.min.js"></script>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized() {
            var sender = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            sender.set_height(browserHeight - 90);

            $("#tvwComponentPanel").height(browserHeight - 130);
        }
    </script>
</head>
<body onresize="PaneResized();" onload="PaneResized();">
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ddlCmpName">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlCmpName"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="ddlStroke" />
                        <telerik:AjaxUpdatedControl ControlID="ddlMake" />
                        <telerik:AjaxUpdatedControl ControlID="ddlModel" />
                        <telerik:AjaxUpdatedControl ControlID="txtNumber" />
                        <telerik:AjaxUpdatedControl ControlID="tvwComponent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvPlannedMaintenanceJob" UpdatePanelHeight="70%"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlStroke">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlCmpName"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="ddlStroke" />
                        <telerik:AjaxUpdatedControl ControlID="ddlMake" />
                        <telerik:AjaxUpdatedControl ControlID="ddlModel" />
                        <telerik:AjaxUpdatedControl ControlID="gvPlannedMaintenanceJob" UpdatePanelHeight="70%"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlMake">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlCmpName"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="ddlStroke" />
                        <telerik:AjaxUpdatedControl ControlID="ddlMake" />
                        <telerik:AjaxUpdatedControl ControlID="ddlModel" />
                        <telerik:AjaxUpdatedControl ControlID="tvwComponent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvPlannedMaintenanceJob" UpdatePanelHeight="70%"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlModel">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="tvwComponent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ddlCmpName"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="ddlStroke" />
                        <telerik:AjaxUpdatedControl ControlID="ddlMake" />
                         <telerik:AjaxUpdatedControl ControlID="ddlModel" />
                        <telerik:AjaxUpdatedControl ControlID="tvwComponent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvPlannedMaintenanceJob" UpdatePanelHeight="70%"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="tvwComponent">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="tvwComponent"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="txtName" />
                        <telerik:AjaxUpdatedControl ControlID="txtComponentNumber" />
                        <telerik:AjaxUpdatedControl ControlID="txtUnits" />
                        <telerik:AjaxUpdatedControl ControlID="txtParent" />
                        <telerik:AjaxUpdatedControl ControlID="hdnComponentID" />
                        <telerik:AjaxUpdatedControl ControlID="hdnComponentTypeID" />
                        <telerik:AjaxUpdatedControl ControlID="gvPlannedMaintenanceJob" UpdatePanelHeight="70%"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                 <telerik:AjaxSetting AjaxControlID="MenuComponent">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuComponent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="tvwComponent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvPlannedMaintenanceJob" UpdatePanelHeight="70%"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuGeneral">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuGeneral"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="tvwComponent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvPlannedMaintenanceJob" UpdatePanelHeight="70%"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="txtName" />
                        <telerik:AjaxUpdatedControl ControlID="txtComponentNumber" />
                        <telerik:AjaxUpdatedControl ControlID="txtUnits" />
                        <telerik:AjaxUpdatedControl ControlID="txtParent" />
                        <telerik:AjaxUpdatedControl ControlID="hdnComponentID" />
                        <telerik:AjaxUpdatedControl ControlID="hdnComponentTypeID" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuJob">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuComponent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuGeneral"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="tvwComponent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuJob"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvPlannedMaintenanceJob" UpdatePanelHeight="70%"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvPlannedMaintenanceJob">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuJob"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvPlannedMaintenanceJob" UpdatePanelHeight="70%"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <%--<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>--%>

        <div style="height: 100%; margin-left: auto; margin-right: auto; vertical-align: middle;">
            <div style="font-weight:600;font-size:12px" runat="server">
                <eluc:TabStrip ID="MenuComponent" runat="server" OnTabStripCommand="MenuComponent_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
            </div>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadAjaxPanel ID="pnlGeneral" runat="server">
                <table style="width:100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCmpName" RenderMode="Lightweight" runat="server" Text="Component"></telerik:RadLabel>
                        </td>
                        <td colspan="2">
                            <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlCmpName" DataTextField="FLDCOMPONENTNAME" DataValueField="FLDCOMPONENTNUMBER" OnSelectedIndexChanged="ddlCmpName_SelectedIndexChanged1" Width="100%" AutoPostBack="true"></telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblStroke" RenderMode="Lightweight" runat="server" Text="Two / Four Stroke"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlStroke" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlStroke_SelectedIndexChanged1">
                                <Items>
                                    <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                    <telerik:RadComboBoxItem Value="1" Text="Two Stroke" />
                                    <telerik:RadComboBoxItem Value="2" Text="Four Stroke" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblMake" RenderMode="Lightweight" runat="server" Text="Make"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlMake" RenderMode="Lightweight" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlMake_SelectedIndexChanged1"  DefaultMessage="Select" DataTextField="FLDMAKE" DataValueField="FLDMAKE"></telerik:RadComboBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblModel" RenderMode="Lightweight" runat="server" Text="Model"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlModel" RenderMode="Lightweight" runat="server" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="ddlModel_SelectedIndexChanged1"  DefaultMessage="Select" DataTextField="FLDTYPE" DataValueField="FLDMODELID"></telerik:RadComboBox>
                            
                        </td>
                        <%--<td>
                            <telerik:RadLabel ID="lblComponent" RenderMode="Lightweight" runat="server" Text="Component Number" Visible="false"></telerik:RadLabel>
                        </td>--%>
                        <%--<td>
                            
                        </td>--%>
                    </tr>
                </table>
            </telerik:RadAjaxPanel>
            <telerik:RadTextBox ID="txtNumber" runat="server" RenderMode="Lightweight" CssClass="hidden"></telerik:RadTextBox>

            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="200px" Width="100%">
                <telerik:RadPane ID="navigationPane" runat="server" Width="400">
                    <eluc:TreeView ID="tvwComponent" runat="server" OnNodeClickEvent="tvwComponent_NodeClickEvent" OnNodeDataBoundEvent="tvwComponent_NodeDataBoundEvent" SearchEmptyMessage="Type to search component" />
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Forward">
                </telerik:RadSplitBar>
                <telerik:RadPane ID="contentPane" runat="server">
                    <div style="font-weight: 600; font-size: 12px" runat="server">
                        <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand"
                            TabStrip="false"></eluc:TabStrip>
                    </div>
                        <br clear="all" />
<%--                    <telerik:RadAjaxPanel ID="RadAjaxPanelEdit" runat="server">
                    </telerik:RadAjaxPanel>--%>
                    <table style="width: 100%;">
                            <tr>
                                <td style="width:90px;">
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtComponentNumber" Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                                    <asp:HiddenField ID="hdnComponentID" runat="server" />
                                    <asp:HiddenField ID="hdnComponentTypeID" runat="server" />
                                </td>
                                <td style="width:90px;">
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtName" runat="server" MaxLength="200" Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                                </td>
                                </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblParent" runat="server" Text="Parent"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtParent" runat="server" Enabled="false" ReadOnly="true"></telerik:RadTextBox>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblUnits" runat="server" RenderMode="Lightweight" Text="NO. Of Units"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox InputType="Number" ID="txtUnits" RenderMode="Lightweight" runat="server" EnabledStyle-HorizontalAlign="Right"></telerik:RadTextBox>
                                </td>
                            </tr>
                        </table>
                    <br clear="all" />
                        <div style="font-weight: 600; font-size: 12px" runat="server">
                                    <eluc:TabStrip ID="MenuJob" runat="server" OnTabStripCommand="MenuJob_TabStripCommand"
                                        TabStrip="false"></eluc:TabStrip>
                                </div>
                    <table style="width:100%;">
                        <tr>
                                    <td style="width:90px;">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtCode"></telerik:RadTextBox>
                                    </td>
                                    <td style="width:90px;">
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtTitle" runat="server" MaxLength="200"></telerik:RadTextBox>
                                        
                                    </td>
                            <td style="width:90px;">
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblShow" runat="server" Text="Show"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDropDownList ID="ddlShow" runat="server" RenderMode="Lightweight">
                                    <Items>
                                        <telerik:DropDownListItem Text="All" Value="2" Selected="true" />
                                        <telerik:DropDownListItem Text="Active" Value="1"/>
                                        <telerik:DropDownListItem Text="Inactive" Value="0"/>
                                    </Items>
                                </telerik:RadDropDownList> 
                            </td>
                                </tr>
                        </table>
                    
                   
                    
                     
                    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvPlannedMaintenanceJob" DecoratedControls="All" EnableRoundedCorners="true" />
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvPlannedMaintenanceJob" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="100%"
                        ShowGroupPanel="true" CellSpacing="0" GridLines="None" OnNeedDataSource="gvPlannedMaintenanceJob_NeedDataSource"
                        OnItemDataBound="gvPlannedMaintenanceJob_ItemDataBound" OnItemCommand="gvPlannedMaintenanceJob_ItemCommand">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDJOBID,FLDGLOBALCOMPONENTJOBID,FLDDTKEY,FLDGLOBALCOMPONENTID,FLDGLOBALCOMPONENTTYPEJOBMAPID">
                            <HeaderStyle Width="102px" />
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="Component" Name="COMPONENT" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="Jobs" Name="JOBS" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ACTION">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="60px"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit" Width="20PX" Height="20PX">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Number" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNUMBER" ColumnGroupName="COMPONENT" UniqueName="COMPONENTNUMBER">
                                    <HeaderStyle Width="90px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblNumber" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNAME" ColumnGroupName="COMPONENT" UniqueName="COMPONENTNAME">
                                    <HeaderStyle Width="180px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="180px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblName" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDJOBCODE" ColumnGroupName="JOBS" UniqueName="CODE">
                                    <HeaderStyle Width="70px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCode" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Title" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDJOBTITLE" ColumnGroupName="JOBS" UniqueName="TITLE">
                                    <HeaderStyle Width="300px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="300px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Category" ColumnGroupName="JOBS" UniqueName="CATEGORY">
                                    <HeaderStyle Width="180px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Active" UniqueName="ACTIVE" ColumnGroupName="JOBS">
                                    <HeaderStyle Width="50px" />
                                    <ItemStyle Width="50px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblActive" runat="server" ></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                                PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
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
