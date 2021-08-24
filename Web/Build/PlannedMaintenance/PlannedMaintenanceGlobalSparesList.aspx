<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceGlobalSparesList.aspx.cs" Inherits="PlannedMaintenanceGlobalSparesList" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Component" Src="~/UserControls/UserControlMultiColumnComponentType.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Spares</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
   
     <script type="text/javascript">
        function PaneResized(sender, args) {
            var browserHeight = $telerik.$(window).height();
            var grid = $find("gvSpares");
            grid._gridDataDiv.style.height = (browserHeight - 350) + "px";
         }
         function pageLoad() {
             PaneResized();
         }
     </script>
    </telerik:RadCodeBlock>

</head>
<body onresize="PaneResized();" onload="PaneResized();">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"/>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager" runat="server"></telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvSpares">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvSpares"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting> 
                <telerik:AjaxSetting AjaxControlID="gvBooks">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvBooks"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvSpares"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuSpares"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting> 
                </AjaxSettings>
        </telerik:RadAjaxManager>
        
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuSpares" runat="server" ></eluc:TabStrip>    
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvBooks" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvBooks" runat="server" AllowCustomPaging="false" AllowSorting="false" AllowPaging="false" Height="180px"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvBooks_NeedDataSource" OnItemCommand="gvBooks_ItemCommand" EnableLinqExpressions="false"
            OnItemUpdated="gvBooks_ItemUpdated">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="true"
                CommandItemDisplay="Top" AutoGenerateColumns="false" DataKeyNames="FLDBOOKID" EnableLinqGrouping="false">
            <CommandItemSettings AddNewRecordText="Add New Book" ShowRefreshButton="false"/>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Action" Groupable="false" EnableHeaderContextMenu="false" ShowFilterIcon="false" AllowFiltering="false"> 
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20%"></ItemStyle>
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
                    <telerik:GridTemplateColumn UniqueName="TITLE"  HeaderText="Title">
                        <ItemTemplate>
                            <telerik:RadLabel runat="server" ID="lblTitle" RenderMode="Lightweight" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadLabel>
                            
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel runat="server" ID="lblID" Visible="false" RenderMode="Lightweight" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBOOKID") %>'></telerik:RadLabel>
                            <telerik:RadTextBox runat="server" ID="txtTitle" Width="240px" RenderMode="Lightweight" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadTextBox>
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
            <ClientSettings EnableRowHoverStyle="true" AllowKeyboardNavigation="true" AllowDragToGroup="false" EnablePostBackOnRowClick="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true"  />
                <ClientEvents />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>


            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvSpares" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvSpares" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" AllowFilteringByColumn="true" FilterItemStyle-Width="100%"
            ShowGroupPanel="false" CellSpacing="0" GridLines="None" OnNeedDataSource="gvSpares_NeedDataSource" 
            OnItemDataBound="gvSpares_ItemDataBound" OnItemCommand="gvSpares_ItemCommand" OnSortCommand="gvSpares_SortCommand" EnableLinqExpressions="false"
            OnItemUpdated="gvSpares_ItemUpdated"> 
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="true"
                CommandItemDisplay="Top" AutoGenerateColumns="false" DataKeyNames="FLDID,FLDGLOBALCOMPONENTID" EnableLinqGrouping="false">
                <CommandItemSettings AddNewRecordText="Add New Spare" ShowRefreshButton="false" ShowExportToExcelButton="true" ExportToExcelText="Export" />
                <HeaderStyle Width="102px" />
                <Columns>
                     <telerik:GridTemplateColumn HeaderText="Action" Groupable="false" Exportable="false" EnableHeaderContextMenu="false" ShowFilterIcon="false" AllowFiltering="false"> 
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="50px"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
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
                    <telerik:GridTemplateColumn HeaderText="Number" UniqueName="NUMBER" ShowFilterIcon="false" FilterDelay="5000" FilterControlWidth="100%">
                        <ItemStyle Width="70px" />
                        <HeaderStyle Width="70px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNumber" runat="server" RenderMode="Lightweight" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblMakerID" runat="server" RenderMode="Lightweight" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVendorID" runat="server" RenderMode="Lightweight" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREFERREDVENDORID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblComponentID" runat="server" RenderMode="Lightweight" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGLOBALCOMPONENTTYPEID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblUnitID" runat="server" RenderMode="Lightweight" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblID" runat="server" RenderMode="Lightweight" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" UniqueName="NAME" ShowFilterIcon="false" FilterDelay="5000" FilterControlWidth="100%">
                         <ItemStyle Width="180px" />
                        <HeaderStyle Width="180px" />
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox RenderMode="Lightweight" Width="100%" ID="txtName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' CssClass="input_mandatory"></telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Maker Ref" UniqueName="MAKERREF" ShowFilterIcon="false" FilterDelay="5000" FilterControlWidth="100%">
                         <ItemStyle Width="100px" />
                        <HeaderStyle Width="100px" />
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblMakerRef" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREF") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="txtMakerRef" Width="100%" runat="server" RenderMode="Lightweight" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREF") %>'></telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component" UniqueName="COMPONENT" ShowFilterIcon="false" FilterDelay="5000" FilterControlWidth="100%">
                        <ItemStyle Width="120px" />
                        <HeaderStyle Width="120px" />
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblComponent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<eluc:Component ID="ucComponent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'
                                 Model='<%# ViewState["MODELID"].ToString()%>' />--%>
                            <telerik:RadComboBox ID="ddlComponent" Width="100%" runat="server" DataTextField="FLDCOMPONENTNAME" DataValueField="FLDGLOBALCOMPONENTTYPEID" EnableLoadOnDemand="true">

                            </telerik:RadComboBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Maker" UniqueName="MAKER" AllowFiltering="false">
                        <ItemStyle Width="120px" />
                        <HeaderStyle Width="120px" />
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblMaker" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Address ID="ucMaker" runat="server" AddressType=",130," Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKER") %>' 
                                Width="100%"/>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vendor" UniqueName="VENDOR" AllowFiltering="false">
                        <ItemStyle Width="120px" />
                        <HeaderStyle Width="120px" />
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblVendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREFERREDVENDOR") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Address ID="ucVendor" runat="server" AddressType=",131," Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREFERREDVENDOR") %>' 
                                Width="100%"/>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Unit" UniqueName="UNIT" AllowFiltering ="false">
                        <ItemStyle Width="70px" />
                        <HeaderStyle Width="70px" />
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" Width="100%" ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Unit ID="ucUnit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="100%"/>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Critical" UniqueName="CRITICAL" AllowFiltering="false">
                        <ItemStyle Width="40px" />
                        <HeaderStyle Width="40px" />
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblCritical" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISCRITICAL").ToString() == "1" ?"Yes":"No" %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadCheckBox ID="chkIsCritical" RenderMode="Lightweight" runat="server" Checked='<%# DataBinder.Eval(Container,"DataItem.FLDISCRITICAL").ToString() =="1"? true:false %>'></telerik:RadCheckBox>
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
                        <ExportSettings Excel-FileExtension="xls" FileName="GlobalSpares" ExportOnlyData="true">
                        </ExportSettings>
                        <ClientSettings EnableRowHoverStyle="true" AllowKeyboardNavigation="true" AllowDragToGroup="false" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <ClientEvents />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>

        </telerik:RadGrid>
    
    </form>
</body>
</html>
