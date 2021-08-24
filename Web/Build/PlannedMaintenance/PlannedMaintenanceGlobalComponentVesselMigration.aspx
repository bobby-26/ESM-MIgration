<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceGlobalComponentVesselMigration.aspx.cs" Inherits="PlannedMaintenanceGlobalComponentVesselMigration" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Model" Src="~/UserControls/UserControlMultiColumnComponentModel.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Migrate to Vessel</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function PaneResized() {
                var sender = $find('RadSplitter1');
                var browserHeight = $telerik.$(window).height();
                var windowWidth = $telerik.$(window).width();
                sender.set_height(browserHeight - 110);
                sender.set_width(windowWidth);
                $telerik.$(".rdTreeScroll").height($telerik.$("#navigationPane").height() - 75);
                //$telerik.$("#contentPane").set_height(browserHeight - 65);
            }

            function UpdateAllChildren(nodes, checked) {
                var i;
                for (i = 0; i < nodes.get_count(); i++) {
                    if (checked) {
                        nodes.getNode(i).check();
                    }
                    else {
                        nodes.getNode(i).set_checked(false);
                    }

                    if (nodes.getNode(i).get_nodes().get_count() > 0) {
                        UpdateAllChildren(nodes.getNode(i).get_nodes(), checked);
                    }
                }
            }
            function treeCollapseAllNodes(sender, eventArgs) {

                var node = eventArgs.get_node();
                if (node.get_value() == "_Root") {
                    var treeView = $find("<%= tvwComponent.ClientID %>");
                    var nodes = treeView.get_allNodes();
                    for (var i = 0; i < nodes.length; i++) {
                        if (nodes[i].get_nodes() != null) {
                            nodes[i].collapse();
                        }
                    }
                }

            }
            function clientNodeChecked(sender, eventArgs) {
                var node = eventArgs.get_node();

                var childNodes = eventArgs.get_node().get_nodes();
                var isChecked = eventArgs.get_node().get_checked();
                UpdateAllChildren(childNodes, isChecked);

                if (node.get_checked()) {
                    while (node.get_parent().set_checked != null) {
                        node.get_parent().set_checked(true);
                        node = node.get_parent();
                    }
                }
            }
            function clientNodeEdited(sender, eventArgs) {
                var treeview = $find('tvwComponent');
                treeview.get_element().focus();
            }
            function keyPress(sender, eventArgs) {
                var node = eventArgs.get_node();
                node.scrollIntoView();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized();" onload="PaneResized();">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="ucVEssel">
                    <UpdatedControls>
                         <telerik:AjaxUpdatedControl ControlID="tvwComponent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvModel" UpdatePanelHeight="100%"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucVessel" ></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuComponent"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvModel">
                    <UpdatedControls>
                         <telerik:AjaxUpdatedControl ControlID="tvwComponent" ></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvModel" UpdatePanelHeight="100%"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="tvwComponent">
                    <UpdatedControls>
                         <telerik:AjaxUpdatedControl ControlID="tvwComponent"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucStatus"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuComponent">
                    <UpdatedControls>
                         <telerik:AjaxUpdatedControl ControlID="tvwComponent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuComponent"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadProgressManager1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="RadProgressArea1"></telerik:AjaxUpdatedControl>

                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucStatus"></telerik:AjaxUpdatedControl>
                        </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="imgbtnSend">
                    <UpdatedControls>
                         <telerik:AjaxUpdatedControl ControlID="imgbtnSend"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucStatus"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadNotification ID="ucStatus" RenderMode="Lightweight" runat="server" AutoCloseDelay="3500" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"  
            EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="TopCenter" Animation="Fade" ShowTitleMenu="false" ></telerik:RadNotification>
                <eluc:TabStrip ID="MenuComponent" runat="server" OnTabStripCommand="MenuComponent_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
        <table style="width: 100%">
            <tr>
                <td style="width: 20%">
                    <telerik:RadLabel RenderMode="Lightweight" ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td style="width: 30%">
                    <eluc:Vessel ID="ucVessel" runat="server" Width="180px" AutoPostBack="true" OnTextChangedEvent="ucVessel_TextChangedEvent" VesselsOnly="true"/>
                </td>
                <td>
                    <asp:Panel ID="pnlCopy" runat="server" GroupingText="Copy">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblCopyTo" runat="server" Text="Copy To"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Vessel ID="ucToVessel" runat="server" CssClass="input_mandatory" Width="180px" VesselsOnly="true" AppendDataBoundItems="true" />
                                        </td>
                                        <td>
                                            <asp:ImageButton runat="server" ID="imgbtnSend" CommandName="COPY" ImageUrl="~/css/Theme1/images/24.png" OnClick="imgbtnSend_Click" ToolTip="Copy"  AlternateText="Copy"/>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                </td>
            </tr>
        </table>
        <%--<telerik:RadProgressManager ID="RadProgressManager1" runat="server" />
        <telerik:RadProgressArea RenderMode="Lightweight" ID="RadProgressArea1" runat="server" Width="500px" />--%>
        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Height="200px" Width="100%">
            <telerik:RadPane ID="contentPane" runat="server" OnClientResized="PaneResized" Width="60%" >
                <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvPlannedMaintenanceJob" DecoratedControls="All" EnableRoundedCorners="true" />

            <telerik:RadGrid RenderMode="Lightweight" ID="gvModel" runat="server" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" Height="100%" 
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvModel_NeedDataSource" OnItemDataBound="gvModel_ItemDataBound" OnItemCommand="gvModel_ItemCommand"
                            OnUpdateCommand="gvModel_UpdateCommand">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDCONFIGURATIONID,FLDGLOBALCOMPONENTID,FLDCOMPONENTNUMBER" CommandItemDisplay="Top">
                            <CommandItemSettings AddNewRecordText="Add" ShowExportToExcelButton="false" ShowRefreshButton="false" ShowPrintButton="false" />
                            <HeaderStyle Width="102px" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Comp Number" UniqueName="COMPONENTNUMBER">
                                    <ItemStyle Width="10%" />
                                    <HeaderStyle Width="10%" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONFIGURATIONID") %>' ID="lblID"></telerik:RadLabel>
                                        <telerik:RadLabel RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>' ID="lblNumber"></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Component" UniqueName="COMPONENTNAME">
                                    <ItemStyle Wrap="false" HorizontalAlign="Left" Width="150px" />
                                    <HeaderStyle Wrap="false" HorizontalAlign="Left" Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>' ID="lblName"></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlCmpName" DataTextField="FLDCOMPONENTNAME" DataValueField="FLDCOMPONENTNUMBER" Width="100%" AutoPostBack="true"
                                            OnSelectedIndexChanged="RadComboBoxes_SelectedIndexChanged" OnDataBinding="RadComboBoxes_DataBinding" OnDataBound="RadComboBoxes_DataBound" ></telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Make">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblMake" runat="server" RenderMode="Lightweight" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="rcbMake" runat="server" AutoPostBack="true" Width="100%"
                                            DataTextField="FLDMAKE" DataValueField="FLDMAKE" OnSelectedIndexChanged="RadComboBoxes_SelectedIndexChanged"
                                            EmptyMessage="Type or Select Maker" OnDataBinding="RadComboBoxes_DataBinding" OnDataBound="RadComboBoxes_DataBound">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Model/Type">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblType" runat="server" RenderMode="Lightweight" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblModelID" Visible="false" runat="server" RenderMode="Lightweight" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODELID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="rcbType" runat="server" AutoPostBack="true" Width="100%"
                                            DataTextField="FLDTYPE" DataValueField="FLDMODELID" OnSelectedIndexChanged="RadComboBoxes_SelectedIndexChanged"
                                            EmptyMessage="Type or Select Model/Type" OnDataBinding="RadComboBoxes_DataBinding" OnDataBound="RadComboBoxes_DataBound">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Spare Parts">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSpareParts" runat="server" RenderMode="Lightweight" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblBookID" runat="server" Visible="false" RenderMode="Lightweight" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBOOKID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox ID="rcbSpareParts" runat="server" AutoPostBack="true" Width="100%"
                                            DataTextField="FLDTITLE" DataValueField="FLDBOOKID" OnSelectedIndexChanged="RadComboBoxes_SelectedIndexChanged"
                                            EmptyMessage="Type or Select Spare Parts" OnDataBinding="RadComboBoxes_DataBinding" OnDataBound="RadComboBoxes_DataBound">
                                        </telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Unit NO">
                                    <ItemStyle Wrap="false" HorizontalAlign="Left"  Width="60px"/>
                                    <HeaderStyle Wrap="false" HorizontalAlign="Left" Width="60px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblUnit" runat="server" RenderMode="Lightweight" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Number ID="ucUnitNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNO") %>' MaxLength="1" Width="100%" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Parent">
                                    <ItemStyle Wrap="false" HorizontalAlign="Left" Width="70px" />
                                    <HeaderStyle Wrap="false" HorizontalAlign="Left" Width="70px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblParent" runat="server" RenderMode="Lightweight" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARENTNUMBER") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtParentNumber" Width="100%" runat="server" RenderMode="Lightweight" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARENTNUMBER") %>' MaxLength="20" ></telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="Refresh" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdRefresh"
                                            ToolTip="Refresh" Width="20PX" Height="20PX">
                                            <span class="icon"><i class="fas fa-redo"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Delete"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                            ToolTip="Delete" Width="20PX" Height="20PX">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save" 
                                            CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                        
                                        <asp:LinkButton runat="server" AlternateText="Cancel" 
                                            CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
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
                
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="splitbar" runat="server" CollapseMode="Both" ></telerik:RadSplitBar>
            <telerik:RadPane ID="navigationPane" runat="server" Width="40%">
                <table style="width: 100%">
                    <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" runat="server" Text="NO Of Units" ID="lblUnits"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number runat="server" ID="txtUnits" />
                        </td>
                    </tr>
                </table>
                <%--<telerik:RadTreeView ID="tvwComponent" runat="server" OnNodeClickEvent="tvwComponent_NodeClickEvent" OnNodeDataBoundEvent="tvwComponent_NodeDataBoundEvent" SearchEmptyMessage="Type to search component" />--%>
                <div class="rdTreeFilter" runat="server" id="divTreeFilter">
                    <telerik:RadTextBox ClientEvents-OnLoad="telerik.clientTreeSearch" ID="treeViewSearch" runat="server" Width="100%"
                        EmptyMessage="Type to search Component" />
                </div>
                <div class="rdTreeScroll">
                    <telerik:RadTreeView RenderMode="Lightweight" ID="tvwComponent" runat="server" OnNodeDataBound="tvwComponent_NodeDataBoundEvent" CheckBoxes="true" CheckChildNodes="false" 
                        AllowNodeEditing="true" OnNodeEdit="tvwComponent_NodeEdit" OnClientNodeChecked="clientNodeChecked" OnClientNodeClicking="clientNodeEdited"  TabIndex="1" OnClientNodeEdited="clientNodeEdited"
                        OnClientNodeClicked="keyPress" OnNodeDrop="tvwComponent_NodeDrop" OnClientNodeCollapsing="treeCollapseAllNodes" EnableDragAndDrop="true" EnableDragAndDropBetweenNodes="true">
                        <NodeTemplate>
                            <telerik:RadLabel runat="server" RenderMode="Lightweight" ID="lblDisplayName" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                            <asp:LinkButton runat="server" AlternateText="ADD"
                                CommandName="ADD" ID="cmdAdd" OnClick="cmdAdd_Click" 
                                ToolTip="Add" Width="20PX" Height="20PX">
                                            <span class="icon"><i class="fas fa-plus"></i></span>
                            </asp:LinkButton>
                        </NodeTemplate>
                        <ExpandAnimation Type="None" />
                        <CollapseAnimation Type="None" />
                        <DataBindings>
                            <telerik:RadTreeNodeBinding Expanded="true"></telerik:RadTreeNodeBinding>
                        </DataBindings>
                        <KeyboardNavigationSettings CommandKey="Cmd" FocusKey="F2" />
                    </telerik:RadTreeView>
                </div>
            </telerik:RadPane>
            
        </telerik:RadSplitter>
    </form>
</body>
</html>
