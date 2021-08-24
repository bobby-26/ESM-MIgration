<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalJobCategoryPlanned.aspx.cs"
    Inherits="DashboardTechnicalJobCategoryPlanned" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">             
             function CloseModelWindow(woname) {                 
                 if (typeof parent.setWO === "function")
                     parent.setWO(woname);
                 if (typeof parent.CloseUrlModelWindow === "function")
                     parent.CloseUrlModelWindow();
                 var obj = parent.document.getElementById("cmdHiddenSubmitWO")
                 if (obj != null)
                     obj.click();
             }

         </script>
    </telerik:RadCodeBlock>    
</head>
<body>
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
        </telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvWorkOrder">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvWorkOrder" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="89%"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuWorkOrder">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuWorkOrder" />
                        <telerik:AjaxUpdatedControl ControlID="gvWorkOrder" LoadingPanelID="RadAjaxLoadingPanel1" UpdatePanelHeight="89%"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
       
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>            
        <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvWorkOrder" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvWorkOrder_NeedDataSource" FilterType="CheckList" AllowMultiRowSelection="true"
                OnItemCommand="gvWorkOrder_ItemCommand" EnableViewState="true" Height="95%" OnSortCommand="gvWorkOrder_SortCommand"
                EnableHeaderContextMenu="true" EnableLinqExpressions="false"> 

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERGROUPID" ClientDataKeyNames="FLDWORKORDERGROUPID" AllowFilteringByColumn="true">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" HeaderText="Work Order No." UniqueName="FLDWORKORDERNUMBER" 
                            AllowSorting="false" FilterDelay="2000" ShowFilterIcon="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>                                
                                <%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" UniqueName="FLDWORKORDERNAME"  AllowSorting="false" FilterDelay="2000" ShowFilterIcon="false">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkOrderName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>                           
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" FilterDelay="2000" UniqueName="FLDJOBCATEGORYID" DataField="FLDJOBCATEGORYID"
                            AllowSorting="false" ShowFilterIcon="false" SortExpression="FLDJOBCATEGORY">                            
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cblJobCategory" OnDataBinding="cblJobCategory_DataBinding" AppendDataBoundItems="true" 
                                    SelectedValue='<%# ViewState["JobCategoryFilter"].ToString() %>' OnClientSelectedIndexChanged="JobCategoryIndexChanged"
                                    runat="server">                                                                    
                                </telerik:RadComboBox>    
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function JobCategoryIndexChanged(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            tableView.filter("FLDJOBCATEGORYID", args.get_item().get_value(), "EqualTo");
                                        }
                                    </script>
                                </telerik:RadScriptBlock>                                                                  
                            </FilterTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDJOBCATEGORY") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Planned Date" HeaderStyle-Width="100px" AllowSorting="true" FilterDelay="2000"
                            ShowSortIcon="true" SortExpression="FLDPLANNINGDUEDATE" DataField="FLDPLANNINGDUEDATE" UniqueName="FLDPLANNINGDUEDATE">
                            <FilterTemplate>
                                From<telerik:RadDatePicker ID="FromOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="FromDateSelected"
                                    DbSelectedDate='<%# ViewState["FDATE"].ToString() %>' />
                                <br />
                                To&nbsp&nbsp&nbsp&nbsp<telerik:RadDatePicker ID="ToOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="ToDateSelected"
                                    DbSelectedDate='<%# ViewState["TDATE"].ToString() %>' />
                                <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                    <script type="text/javascript">
                                        function FromDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        var ToPicker = $find('<%# ((GridItem)Container).FindControl("ToOrderDatePicker").ClientID %>');

                                        var fromDate = FormatSelectedDate(sender);
                                        var toDate = FormatSelectedDate(ToPicker);

                                        tableView.filter("FLDPLANNINGDUEDATE", fromDate + "~" + toDate, "Between");

                                    }
                                    function ToDateSelected(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromOrderDatePicker").ClientID %>');

                                        var fromDate = FormatSelectedDate(FromPicker);
                                        var toDate = FormatSelectedDate(sender);

                                        tableView.filter("FLDPLANNINGDUEDATE", fromDate + "~" + toDate, "Between");
                                    }
                                    function FormatSelectedDate(picker) {
                                        var date = picker.get_selectedDate();
                                        var dateInput = picker.get_dateInput();
                                        var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());

                                        return formattedDate;
                                    }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>
                            </ItemTemplate>                          
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Duration" AllowFiltering="false">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDDURATION"]%>                                
                            </ItemTemplate>                            
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Assigned To" HeaderStyle-Width="125px" AllowSorting="true" ShowFilterIcon="false"
                            ShowSortIcon="true" SortExpression="FLDDISCIPLINENAME" DataField="FLDPLANNINGDISCIPLINE" FilterDelay="200" UniqueName="FLDPLANNINGDISCIPLINE">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlResponsibility" OnDataBinding="ddlResponsibility_DataBinding" AppendDataBoundItems="true" 
                                    SelectedValue='<%# ViewState["filterDiscipline"].ToString() %>' OnClientSelectedIndexChanged="ResponsibilityIndexChanged"
                                    runat="server">                                                                    
                                </telerik:RadComboBox>    
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function ResponsibilityIndexChanged(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            tableView.filter("FLDPLANNINGDISCIPLINE", args.get_item().get_value(), "EqualTo");
                                        }
                                    </script>
                                </telerik:RadScriptBlock>                                                                  
                            </FilterTemplate>   
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>
                            </ItemTemplate>                           
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="80px" ShowFilterIcon="false" UniqueName="FLDSTATUSCODE"
                            AllowSorting="true" ShowSortIcon="true" SortExpression="FLDHARDNAME" DataField="FLDSTATUSCODE" FilterDelay="200">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlStatus" runat="server" OnDataBinding="ddlStatus_DataBinding"
                                    SelectedValue='<%# ViewState["filterStatus"].ToString() %>' AppendDataBoundItems="true" OnClientSelectedIndexChanged="StatusIndexChanged">
                                </telerik:RadComboBox>
                                 <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function StatusIndexChanged(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            tableView.filter("FLDSTATUSCODE", args.get_item().get_value(), "EqualTo");
                                        }
                                    </script>
                                </telerik:RadScriptBlock>      
                            </FilterTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDSTATUS"]%>                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" EnableHeaderContextMenu="true">
                        </telerik:GridClientSelectColumn>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>       
    </form>
</body>
</html>
