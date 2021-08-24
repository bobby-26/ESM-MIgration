<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersReportWorkOrderReportList.aspx.cs" Inherits="OwnersReportWorkOrderReportList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= RadGrid1.ClientID %>"));
                }, 200);
            }
            function pageLoad() {
                Resize();
            }

    </script>
    </telerik:RadCodeBlock>
    
    <style>
        .imgbtn-height {
            height: 20px;
        }
    </style>
</head>
<body onresize="Resize()" onload="Resize()">
    <form id="frmWorkOrderReportList" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
             <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuDivWorkOrder">
                    <UpdatedControls>                        
                        <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


        <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>

        <telerik:RadGrid  ID="RadGrid1" runat="server" AllowCustomPaging="true" AllowSorting="false" AllowPaging="true" Width="100%"
            CellSpacing="0" GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource" OnPreRender="RadGrid1_PreRender" GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnItemCommand="RadGrid1_ItemCommand" DataKeyNames="FLDWORKORDERID" OnItemDataBound="RadGrid1_ItemDataBound" AllowFilteringByColumn="true" EnableLinqExpressions="false">
            <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
                <CommandItemSettings ShowRefreshButton="false" RefreshText="Search" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Job Number" UniqueName="FLDWORKORDERNUMBER" FilterDelay="2000"
                             ShowFilterIcon="false" CurrentFilterFunction="Contains" FilterControlWidth="95%">  
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <%--<telerik:GridBoundColumn HeaderText="Job Title" UniqueName="FLDWORKORDERNAME" DataField="FLDWORKORDERNAME" DataType="System.String">
                        <HeaderStyle Width="410px" />
                        <ItemStyle Width="410px" />
                    </telerik:GridBoundColumn>--%>
                    <telerik:GridTemplateColumn UniqueName="FLDWORKORDERNAME" HeaderText="Job Code & Title"  AllowSorting="false" FilterDelay="2000"
                             ShowFilterIcon="false" CurrentFilterFunction="Contains" FilterControlWidth="95%"> 
                        <HeaderStyle Width="280px" />                        
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTitle" runat="server" CommandName="Select" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>' Visible="false"></telerik:RadLabel>
                            <asp:LinkButton ID="lnkTitle" runat="server" CommandName="Select" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component No." UniqueName="FLDCOMPONENTNUMBER" FilterDelay="2000"
                             ShowFilterIcon="false" CurrentFilterFunction="Contains" FilterControlWidth="95%">
                        <HeaderStyle Width="90px" />
                        <ItemStyle Width="90px" />
                         <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component Name" UniqueName="FLDCOMPONENTNAME" FilterDelay="2000"
                             ShowFilterIcon="false" CurrentFilterFunction="Contains" FilterControlWidth="95%">
                        <HeaderStyle Width="180px" />
                        <ItemStyle Width="180px" />
                         <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Category" UniqueName="FLDJOBCATEGORY" AllowFiltering="true" ShowSortIcon="false" FilterControlWidth="80px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="EqualTo">
                        <HeaderStyle Width="140px" />
                         <FilterTemplate>
                            <telerik:RadComboBox ID="ddlJobCategory" runat="server" OnDataBinding="ddlJobCategory_DataBinding" AppendDataBoundItems="true"
                               SelectedValue ='<%# ViewState["JOBCATEGORY"].ToString() %>'  OnClientSelectedIndexChanged="JobCategoryIndexChanged">
                            </telerik:RadComboBox>
                            <telerik:RadScriptBlock runat="server">
                                <script type="text/javascript">
                                    function JobCategoryIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        tableView.filter("FLDJOBCATEGORY", args.get_item().get_value(), "EqualTo");
                                    }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDJOBCATEGORY"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Frequency" UniqueName="FLDFREQUENCYTYPE" AllowSorting="false"  ShowSortIcon="false" FilterControlWidth="80px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="EqualTo">
                        <HeaderStyle Width="130px" />
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
                                        tableView.filter("FLDFREQUENCYTYPE", frequency.get_value() + "~" + freqtype, "EqualTo");
                                    }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>
                        <ItemStyle Width="130px" />
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDFREQUENCYNAME"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Priority" UniqueName="FLDPLANINGPRIORITY" FilterDelay="2000"
                             ShowFilterIcon="false" CurrentFilterFunction="EqualTo" FilterControlWidth="95%">
                        <HeaderStyle Width="70px" />
                        <ItemStyle HorizontalAlign="Center" />
                         <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDPLANINGPRIORITY"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Is Defect Job" UniqueName="FLDJOBDONESTATUS" AllowFiltering="false">                       
                        <HeaderStyle Width="70px" />
                        <ItemStyle HorizontalAlign="Center" /><ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDJOBDONESTATUS"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Done Date" UniqueName="FLDWORKDONEDATE" DataField="FLDWORKDONEDATE" AllowFiltering="false">
                        <HeaderStyle Width="150px" />
                       <FilterTemplate>                                      
                            From<telerik:RadDatePicker ID="FromOrderDatePicker" runat="server" Width="100px" ClientEvents-OnDateSelected="FromDateSelected"
                                    DbSelectedDate='<%# ViewState["FDATE"].ToString() %>' />
                               
                                <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                    <script type="text/javascript">
                                        function FromDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var ToPicker = $find('<%# ((GridItem)Container).FindControl("ToOrderDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(sender);
                                            var toDate = FormatSelectedDate(ToPicker);

                                            tableView.filter("FLDWORKDONEDATE", fromDate + "~" + toDate, "Between");

                                        }
                                        function ToDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromOrderDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(FromPicker);
                                            var toDate = FormatSelectedDate(sender);

                                            tableView.filter("FLDWORKDONEDATE", fromDate + "~" + toDate, "Between");
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
                            <ItemTemplate>
                                <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDWORKDONEDATE"])%>
                            </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Done By" UniqueName="FLDREPORTBY" AllowFiltering="false">
                        <HeaderStyle Width="180px" />
                        <ItemStyle Width="180px" />
                        <FilterTemplate>
                             To&nbsp&nbsp&nbsp&nbsp<telerik:RadDatePicker ID="ToOrderDatePicker" runat="server" Width="100px" ClientEvents-OnDateSelected="ToDateSelected"
                                    DbSelectedDate='<%# ViewState["TDATE"].ToString() %>' />
                        </FilterTemplate>
                        <ItemStyle HorizontalAlign="Center" /><ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDREPORTBY"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Remarks" UniqueName="FLDREMARKS" AllowFiltering="false">
                        <HeaderStyle Width="300px" /> 
                        <ItemStyle HorizontalAlign="Center" /><ItemTemplate>
                            <%# General.SanitizeHtml(((DataRowView)Container.DataItem)["FLDREMARKS"].ToString())%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Action" AllowFiltering="false" AllowSorting="false">
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="MaintenanceLog"
                                CommandName="MAINTENANCEFORM" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRTemplates"
                                ToolTip="Reporting Templates" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-file"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Parameters"
                                CommandName="PARAMETERS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdParameters"
                                ToolTip="Parameters" Width="20px" Height="20px">
                               <span class="icon"><i class="fas fa-newspaper"></i></span>
                            </asp:LinkButton>
                            <asp:ImageButton runat="server" ID="cmdAttachments" ToolTip="Attachment" 
                                CommandName="ATTACHMENTS" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />
<%--                            <asp:LinkButton runat="server" AlternateText="Attachments"
                                CommandName="ATTACHMENTS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttachments"
                                ToolTip="Attachments" Width="20px" Height="20px">
                                <span class="icon"><i class="fas fa-paperclip"></i></span>
                            </asp:LinkButton>--%>
                            <asp:LinkButton runat="server" AlternateText="Parts"
                                CommandName="PARTS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdParts"
                                ToolTip="Parts" Width="20px" Height="20px">
                                <span class="icon"><i class="fas fa-cogs"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="RA History"
                                CommandName="RA" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRA"
                                ToolTip="RA" Width="20px" Height="20px">
                                <span class="icon"><i class="fas fa-eye"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="PTW History"
                                CommandName="PTW" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPTW"
                                ToolTip="PTW" Width="20px" Height="20PX">
                                <span class="icon"><i class="fas fa-copy"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Report" ID="cmdReschedule" CommandName="RESCHEDULE" ToolTip="Postpone History">
                                 <span class="icon"><i class="far fa-calendar-alt"></i></span>
                            </asp:LinkButton>
                            <asp:ImageButton ID="lnkPtwWaive" runat="server" ImageUrl="<%$ PhoenixTheme:images/45.png %>" ToolTip="Waive" CommandName="WAIVE" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                   PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
