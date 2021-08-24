<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceGlobalComponentTypeJob.aspx.cs" Inherits="PlannedMaintenanceGlobalComponentTypeJob" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TreeView" Src="~/UserControls/UserControlTreeViewTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Component Type Jobs</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <%--<script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.telerik.js"></script>--%>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
   
     <script type="text/javascript">
        function PaneResized(sender, args) {
            var browserHeight = $telerik.$(window).height();
            var grid = $find("gvPlannedMaintenanceJob");
            grid._gridDataDiv.style.height = (browserHeight - 175) + "px";
         }

         function RowContextMenu(sender, eventArgs) {
             var menu = $find("<%= gvcMenu.ClientID %>");

             menu.hide();

             var domEvent = eventArgs.get_domEvent();
             var source = domEvent.target || domEvent.srcElement;
             index = eventArgs.get_itemIndexHierarchical();

             document.getElementById("radGridClickedRowIndex").value = index;

             var masterTable = sender.get_masterTableView();
             masterTable.clearSelectedItems();
             //select the current row
             masterTable.selectItem(masterTable.get_dataItems()[index].get_element());

             //get the first cell of the row
             var cell = masterTable.get_dataItems()[index].get_element().cells[0];

             if (source == cell) {
                 menu.show(domEvent);
             }
             $telerik.cancelRawEvent(domEvent);
         }
         
        function pageLoad() {
            PaneResized();
        }

            </script>
         </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized();" onload="PaneResized();">
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnableScriptCombine="false" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
                <%--<eluc:TabStrip ID="MenuComponent" runat="server" OnTabStripCommand="MenuComponent_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>--%>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
            <eluc:TabStrip ID="MenuJobs" runat="server" OnTabStripCommand="MenuJobs_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvPlannedMaintenanceJob" DecoratedControls="All" EnableRoundedCorners="true" />
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvPlannedMaintenanceJob" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" AllowFilteringByColumn="true" FilterItemStyle-Width="100%"
                        ShowGroupPanel="false" CellSpacing="0" GridLines="None" OnNeedDataSource="gvPlannedMaintenanceJob_NeedDataSource" OnPreRender="gvPlannedMaintenanceJob_PreRender"
                        OnItemDataBound="gvPlannedMaintenanceJob_ItemDataBound" OnItemCommand="gvPlannedMaintenanceJob_ItemCommand" OnSortCommand="gvPlannedMaintenanceJob_SortCommand" EnableLinqExpressions="false"
                        AllowAutomaticUpdates="true">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"  HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="true"
                         CommandItemDisplay="None" AutoGenerateColumns="false" DataKeyNames="FLDJOBID,FLDGLOBALCOMPONENTJOBID,FLDDTKEY,FLDGLOBALCOMPONENTID,FLDGLOBALCOMPONENTTYPEJOBMAPID" EnableLinqGrouping="false">
                            
                            <HeaderStyle Width="102px" />
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="Type" Name="TYPE" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="Component" Name="COMPONENT" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                                <telerik:GridColumnGroup HeaderText="Jobs" Name="JOBS" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <Columns>
                                 <telerik:GridTemplateColumn HeaderText="" UniqueName="ACTION" AllowFiltering="false">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20px"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20px"></ItemStyle>
                                    <ItemTemplate>
                                        <span class="icon"><i class="fas fa-ellipsis-v"></i></span>    
                                        <%--<asp:LinkButton runat="server" Enabled="false">
                                                
                                        </asp:LinkButton>--%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Make" ColumnGroupName="TYPE" UniqueName="MAKE" Groupable="true"  FilterDelay="5000"
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
                                <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true" ShowSortIcon="true" FilterControlWidth="100%" FilterDelay="5000"
                                        AllowFiltering="true" ShowFilterIcon="false" SortExpression="FLDJOBCODE" ColumnGroupName="JOBS" UniqueName="CODE" Groupable="true">
                                    <HeaderStyle Width="70px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblCode" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Title" AllowSorting="true" ShowSortIcon="true" FilterControlWidth="100%" AllowFiltering="true" FilterDelay="5000"
                                        ShowFilterIcon="false" SortExpression="FLDJOBTITLE" ColumnGroupName="JOBS" UniqueName="TITLE" Groupable="false">
                                    <HeaderStyle Width="300px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="300px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Frequency" ShowFilterIcon="false"
                                        ColumnGroupName="JOBS" UniqueName="FREQUENCY">
                                    <FilterTemplate>
                            <telerik:RadTextBox ID="txtFrequency" runat="server" Width="40%" Text='<%# ViewState["FREQUENCY"].ToString() %>'></telerik:RadTextBox>
                            <telerik:RadComboBox ID="cblFrequencyType" runat="server" OnDataBinding="cblFrequencyType_DataBinding" AutoPostBack="false" Width="60%" AppendDataBoundItems="true"
                                OnClientSelectedIndexChanged="FrequencyIndexChanged" SelectedValue='<%# ViewState["FREQUENCYTYPE"].ToString() %>'>
                            </telerik:RadComboBox>
                            <telerik:RadScriptBlock runat="server">
                                <script type="text/javascript">
                                    function FrequencyIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        var frequency = $find('<%# ((GridItem)Container).FindControl("txtFrequency").ClientID %>');
                                        var freqtype = args.get_item().get_value();
                                        tableView.filter("FREQUENCY", frequency.get_value() + "~" + freqtype, "EqualTo");
                                    }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>
                                    <HeaderStyle Width="120px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFrequency" runat="server" Text=""></telerik:RadLabel>
                                        <telerik:RadNumericTextBox ID="RadLabel1" CssClass="hidden" runat="server" Text='<%#General.GetNullableInteger(DataBinder.Eval(Container,"DataItem.FLDFREQUENCY").ToString()) %>'></telerik:RadNumericTextBox>
                                        <telerik:RadNumericTextBox ID="RadLabel2" CssClass="hidden" runat="server" Text='<%#General.GetNullableInteger(DataBinder.Eval(Container,"DataItem.FLDFREQUENCYTYPE").ToString()) %>'></telerik:RadNumericTextBox>
                                        <telerik:RadNumericTextBox ID="RadLabel3" CssClass="hidden" runat="server" Text='<%#General.GetNullableInteger(DataBinder.Eval(Container,"DataItem.FLDCOUNTERFREQUENCY").ToString()) %>'></telerik:RadNumericTextBox>
                                        <telerik:RadNumericTextBox ID="RadLabel4" CssClass="hidden" runat="server" Text='<%#General.GetNullableInteger(DataBinder.Eval(Container,"DataItem.FLDCOUNTERTYPE").ToString()) %>'></telerik:RadNumericTextBox>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Category" ColumnGroupName="JOBS" UniqueName="CATEGORY" Groupable="false" 
                                    AutoPostBackOnFilter="false" FilterDelay="2000" CurrentFilterFunction="EqualTo">
                                    <HeaderStyle Width="180px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                    <FilterTemplate>
                                        <telerik:RadComboBox ID="ddlJobCategory" runat="server" OnDataBinding="ddlJobCategory_DataBinding" AppendDataBoundItems="true"
                                           SelectedValue ='<%# ViewState["CATEGORY"].ToString() %>'  OnClientSelectedIndexChanged="JobCategoryIndexChanged">
                                        </telerik:RadComboBox>
                                        <telerik:RadScriptBlock runat="server">
                                            <script type="text/javascript">
                                                function JobCategoryIndexChanged(sender, args) {
                                                    var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                                    tableView.filter("CATEGORY", args.get_item().get_value(), "EqualTo");
                                                }
                                            </script>
                                        </telerik:RadScriptBlock>
                                    </FilterTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblClass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORY") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Active" UniqueName="ACTIVE" ColumnGroupName="JOBS" AllowFiltering="true" FilterControlWidth="100%" FilterDelay="5000" ShowFilterIcon="false">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle Width="60px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" ID="lblActive" runat="server" ></telerik:RadLabel>
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
                        <ClientSettings EnableRowHoverStyle="true" EnablePostBackOnRowClick="true" AllowKeyboardNavigation="true" AllowDragToGroup="false" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <ClientEvents OnRowMouseOver="RowContextMenu" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
        <input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex" />
        <telerik:RadContextMenu ID="gvcMenu" runat="server" OnItemClick="gvcMenu_ItemClick"
            EnableRoundedCorners="true" EnableShadows="true">
            <Items>
                <telerik:RadMenuItem Text="Edit" Value="Edit" >
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="Job Description" Value="Description" >
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="History Template" Value="History">
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="Include Jobs" Value="Include">
                </telerik:RadMenuItem>
                <telerik:RadMenuItem Text="Manuals" Value="Manuals">
                </telerik:RadMenuItem>
            </Items>
        </telerik:RadContextMenu>
    </form>
</body>
</html>
