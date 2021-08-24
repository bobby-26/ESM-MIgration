<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceGlobalModelRegister.aspx.cs" Inherits="PlannedMaintenanceGlobalModelRegister" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Model Register</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">


            function CloseWindow() {
                <%--document.getElementById('<%=DateFromFieldValidator.ClientID%>').innerHTML = "";                
                document.getElementById('<%=ValidationSummary1.ClientID%>').style.display = 'none';            --%>    
            }
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvModel.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
            function RowContextMenu(e,index) {
                var menu = $find("<%= gvcMenu.ClientID %>");

                var length = menu.get_allItems().length;
                menu.trackChanges();
                menu.get_items().clear();
                menu.commitChanges();   
                //alert(length);
                //for (var i = 2; i <= length; i++) {
                //    menu.get_items().remove(menu.get_allItems()[i]);
                //}

                
                var p = {
                    "menu 1": "value1",
                    "menu 2": "value2",
                    "menu 3": "value3"
                };

                for (var key in p) {
                    if (p.hasOwnProperty(key)) {
                        console.log(key + " -> " + p[key]);
                        var menuItem = new Telerik.Web.UI.RadMenuItem();
                        menuItem.set_text(key);
                        menuItem.set_value(p[key]);
                        //menu.trackChanges();
                        menu.get_items().add(menuItem);
                    }
                };

                

                var grid = $find("<%= gvModel.ClientID%>")
                menu.show(e)

                document.getElementById("radGridClickedRowIndex").value = index;
                //var masterTable = grid.get_masterTableView();
                //masterTable.clearSelectedItems();
                ////select the current row
                //masterTable.selectItem(masterTable.get_dataItems()[index].get_element());
            }
        </script>
        <style>
            div.myCustomWidth {
                width: 500px !important;
            }

            div.myCustomWidth ul {
                float: none !important;
            }

            div.myCustomWidth ul .rgFilterListMenu .rmContent {
                max-width: none !important;
            }

            div.myCustomWidth ul .rmContent .RadListBox {
                width: 100% !important;
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
   <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvModel">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvModel" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="100%" />
                        <%--<telerik:AjaxUpdatedControl ControlID="gvcMenu" />--%>
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuPlannedMaintenance">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvModel" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="100%" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                        <telerik:AjaxUpdatedControl ControlID="MenuPlannedMaintenance" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text=""></eluc:Status>

            <%--<div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuPlannedMaintenance" runat="server" OnTabStripCommand="PlannedMaintenance_TabStripCommand"></eluc:TabStrip>
            </div>--%>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvPlannedMaintenanceJob" DecoratedControls="All" EnableRoundedCorners="true" />
            <input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvModel" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="true" Height="100%"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvModel_NeedDataSource" OnItemDataBound="gvModel_ItemDataBound" OnItemCommand="gvModel_ItemCommand"
                            OnUpdateCommand="gvModel_UpdateCommand"
                AllowFilteringByColumn="true" FilterType="HeaderContext" EnableHeaderContextFilterMenu="true" FilterItemStyle-Width="180px"   EnableHeaderContextMenu="true" OnFilterCheckListItemsRequested="gvModel_FilterCheckListItemsRequested"
                >
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDMODELID,FLDCOMPONENTNUMBER" CommandItemDisplay="Top" >
                            <HeaderStyle Width="102px" />
                            <CommandItemSettings ShowRefreshButton="false" RefreshText="Search" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="true" AddNewRecordText="Add Model" ShowExportToPdfButton="false" />
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
                                
                                <telerik:GridTemplateColumn HeaderText="Comp Number" UniqueName="COMPONENTNUMBER" AllowFiltering="true" Groupable="false" SortExpression="FLDCOMPONENTNUMBER" DataField="FLDCOMPONENTNUMBER" FilterCheckListEnableLoadOnDemand="true">
                                    <ItemStyle Width="90px" />
                                    <HeaderStyle Width="90px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>' ID="lblNumber"></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Component" UniqueName="COMPONENTNAME" AllowFiltering="true" SortExpression="FLDCOMPONENTNAME" Groupable="false" DataField="FLDCOMPONENTNAME" FilterCheckListEnableLoadOnDemand="true" FilterControlWidth="180px">
                                    <ItemStyle Width="180px" />
                                    <HeaderStyle Width="180px" />
                                    
                                    <ItemTemplate>
                                        <telerik:RadLabel RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>' ID="lblName"></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ddlCmpNameEdit" DataTextField="FLDCOMPONENTNAME" DataValueField="FLDCOMPONENTNUMBER" Width="100%"></telerik:RadComboBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                
                                <telerik:GridTemplateColumn HeaderText="Make" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDMAKE" Groupable="false" UniqueName="MAKE" FilterCheckListEnableLoadOnDemand="true" AllowFiltering="true" DataField="FLDMAKE">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                                <telerik:RadLabel ID="lblID" RenderMode="Lightweight" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODELID") %>'></telerik:RadLabel>    
                                                <telerik:RadLabel ID="lblMake" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                         <telerik:RadLabel ID="lblModelID" RenderMode="Lightweight" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODELID") %>'></telerik:RadLabel>
                                        <telerik:RadTextBox ID="txtMakeEdit" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKE") %>' Width="100%"></telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Type" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDTYPE" Groupable="false" UniqueName="TYPE" FilterCheckListEnableLoadOnDemand="true" AllowFiltering="true" DataField="FLDTYPE">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtTypeEdit" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>' Width="100%"></telerik:RadTextBox>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                
                                <telerik:GridTemplateColumn HeaderText="Two / Four Stroke" UniqueName="STROKE" SortExpression="FLDTWOORFOURSTROKENAME" DataField="FLDTWOORFOURSTROKENAME" FilterCheckListEnableLoadOnDemand="true" Groupable="false">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStroke" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTWOORFOURSTROKENAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblStrokeEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTWOORFOURSTROKE") %>'></telerik:RadLabel>
                                        <telerik:RadDropDownList ID="ddlStrokeEdit" runat="server" RenderMode="Lightweight" DefaultMessage="--Select--" AppendDataBoundItems="true">
                                            <Items>
                                                <telerik:DropDownListItem Value="Dummy" Text="--Select--" />
                                                <telerik:DropDownListItem Value="1" Text="Two Stroke" />
                                                <telerik:DropDownListItem Value="2" Text="Four Stroke" />
                                            </Items>
                                        </telerik:RadDropDownList>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action" Groupable="false" EnableHeaderContextMenu="false"> 
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="70px"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="70px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="Edit" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit" Width="20PX" Height="20PX">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                          <asp:LinkButton runat="server" AlternateText="Delete"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                            ToolTip="Delete" Width="20PX" Height="20PX">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Populate"
                                            CommandName="Populate" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdPopulate"
                                            ToolTip="Populate" Width="20PX" Height="20PX">
                                            <span class="icon"><i class="fas fa-download"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Copy"
                                            CommandName="COPY" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCopy"
                                            ToolTip="Copy" Width="20PX" Height="20PX">
                                            <span class="icon"><i class="fas fa-copy"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Jobs"
                                            CommandName="JOBS" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdJobs"
                                            ToolTip="Jobs" Width="20PX" Height="20PX">
                                            <span class="icon"><i class="fas fa-tools"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Spare Parts"
                                            CommandName="PARTS" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSpareParts" 
                                            ToolTip="Spare Parts" Width="20PX" Height="20PX">
                                            <span class="icon"><i class="fas fa-book"></i></span>
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
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                                PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                    <telerik:RadContextMenu ID="gvcMenu" runat="server" OnItemClick="gvcMenu_ItemClick" 
                                    EnableRoundedCorners="true" EnableShadows="true">
                    <Items>
                        <telerik:RadMenuItem Text="Add New" Value="ADD">
                        </telerik:RadMenuItem>
                    </Items>
                </telerik:RadContextMenu>
                <telerik:RadWindow ID="modalPopup" runat="server" Width="500px" Height="365px" Modal="true" OnClientClose="CloseWindow" OffsetElementID="main"
                        VisibleStatusbar="false" KeepInScreenBounds="true">    
                    <ContentTemplate>
                        <eluc:TabStrip ID="MenuAddNew" runat="server" OnTabStripCommand="MenuAddNew_TabStripCommand"></eluc:TabStrip>
                        <table style="width:100%">
                            <tr>
                                <td style="width:20%">
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblTitle" Text="Title" runat="server"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox RenderMode="Lightweight" runat="server" id="txtTitle"></telerik:RadTextBox>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                        
                        
                </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
        </div>
    </form>
</body>
</html>
