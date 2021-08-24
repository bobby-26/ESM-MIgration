<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentJobsLastDoneUpdate.aspx.cs"
    Inherits="PlannedMaintenanceComponentJobsLastDoneUpdate" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html>
<script runat="server">
</script>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component Jobs</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvComponentJob.ClientID %>"),null,17);
                },200);
                setValue();
            }
        </script>
    </telerik:RadCodeBlock>
    <style>
        .imgbtn-height {
            height: 20px;
        }
    </style>
</head>
<body onload="Resize();" onresize="Resize();">
    <form id="frmComponentJob" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadToolTipManager ID="RadToolTipManager1" runat="server" Position="BottomCenter"
            Animation="Fade" AutoTooltipify="false" Width="300px" Font-Size="Large" RenderInPageRoot="true" AutoCloseDelay="80000">
            <TargetControls>
            </TargetControls>
        </telerik:RadToolTipManager>
        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1" Height="100%">
            <telerik:RadNotification ID="RadNotification1" runat="server" RenderMode="Lightweight" Width="250px" Height="150px" Font-Bold="true" Font-Size="Medium"
                AutoCloseDelay="2000" ShowCloseButton="false" Title="Component Job" TitleIcon="none" ContentIcon="none" Position="Center"></telerik:RadNotification>
            <eluc:TabStrip ID="MainMenu" runat="server" OnTabStripCommand="MainMenu_TabStripCommand"></eluc:TabStrip>
            <br />
            <telerik:RadLabel ID="dtDate" runat="server" Text="Last Done Date"></telerik:RadLabel>
            <telerik:RadDatePicker ID="dtDonedate" runat="server"></telerik:RadDatePicker>
            <br />
            <br />
            <eluc:TabStrip ID="MenuDivComponentJob" runat="server" OnTabStripCommand="MenuDivComponentJob_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="Status" runat="server" />
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvComponentJob" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvComponentJob" Height="91%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableViewState="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvComponentJob_NeedDataSource" OnPreRender="gvComponentJob_PreRender" OnSortCommand="gvComponentJob_SortCommand"
                OnItemCommand="gvComponentJob_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true" EnableLinqExpressions="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings CaseSensitive="false" />
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCOMPONENTJOBID" TableLayout="Fixed" AllowFilteringByColumn="true">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Component No." FilterDelay="2000" FilterControlWidth="100%" ShowFilterIcon="false" CurrentFilterFunction="Contains" UniqueName="FLDCOMPONENTNUMBER" DataField="FLDCOMPONENTNUMBER">
                            <HeaderStyle Width="70px" HorizontalAlign="Left" />
                            <ItemStyle Width="70px" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Name" FilterDelay="2000" FilterControlWidth="100%" ShowFilterIcon="false" CurrentFilterFunction="Contains" UniqueName="FLDCOMPONENTNAME" DataField="FLDCOMPONENTNAME">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Code" FilterDelay="2000" FilterControlWidth="100%" ShowFilterIcon="false" CurrentFilterFunction="Contains" UniqueName="FLDJOBCODE" DataField="FLDJOBCODE">
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponentJobId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTJOBID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblJobID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFunctionCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Title" FilterDelay="2000" FilterControlWidth="100%" ShowFilterIcon="false" CurrentFilterFunction="Contains" UniqueName="FLDJOBTITLE" DataField="FLDJOBTITLE">
                            <HeaderStyle Width="200px" HorizontalAlign="Left" />
                            <ItemStyle Width="200px" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Category" AllowSorting="true" UniqueName="FLDJOBCATEGORY" ShowSortIcon="false" FilterControlWidth="99%"
                            AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="EqualTo">
                            <HeaderStyle Width="120px" HorizontalAlign="Left" />
                            <ItemStyle Width="120px" HorizontalAlign="Left" />
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cblJobCategory" runat="server" OnDataBinding="cblJobCategory_DataBinding" AppendDataBoundItems="true"
                                    SelectedValue='<%# ViewState["FLDJOBCATEGORY"].ToString() %>' OnClientSelectedIndexChanged="CategoryIndexChanged" Width="98%">
                                </telerik:RadComboBox>
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function CategoryIndexChanged(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            tableView.filter("FLDJOBCATEGORY", args.get_item().get_value(), "EqualTo");
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCATEGORY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Priority" FilterDelay="2000" FilterControlWidth="100%" ShowFilterIcon="false" CurrentFilterFunction="Contains" UniqueName="FLDPRIORITY" DataField="FLDPRIORITY">
                            <HeaderStyle Width="60px" HorizontalAlign="Center" />
                            <ItemStyle Width="60px" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPriority" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRIORITY") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Res Descipline" ColumnGroupName="ResDescipline" HeaderStyle-Width="150px" UniqueName="FLDDISCIPLINENAME"
                            AllowSorting="true" SortExpression="FLDDISCIPLINENAME" FilterControlWidth="80px" FilterDelay="2000"
                            AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="EqualTo">
                            <HeaderStyle Width="150px" HorizontalAlign="Left" />
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlResponsibility" OnDataBinding="ddlResponsibility_DataBinding" AppendDataBoundItems="true"
                                    SelectedValue='<%# ViewState["FLDDISCIPLINENAME"].ToString() %>' OnClientSelectedIndexChanged="ResponsibilityIndexChanged"
                                    runat="server">
                                </telerik:RadComboBox>
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function ResponsibilityIndexChanged(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        tableView.filter("FLDDISCIPLINENAME", args.get_item().get_value(), "EqualTo");
                                    }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCIPLINENAME") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDiscipline" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDISCIPLINE") %>' Visible="false" />
                                <eluc:Discipline ID="ucDiscipline" runat="server" CssClass="input" AppendDataBoundItems="true" DisciplineList='<%# PhoenixRegistersDiscipline.ListDiscipline() %>'
                                    SelectedDiscipline='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDISCIPLINE")  %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Frequency" ColumnGroupName="Frequency" AllowFiltering="false">
                            <HeaderStyle Width="140px" HorizontalAlign="Left" />
                            <ItemStyle Wrap="false" Width="140px" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYNAME") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Done Date" ColumnGroupName="LastDoneDate" SortExpression="FLDJOBLASTDONEDATE" AllowFiltering="false">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Width="60px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastDonedate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDJOBLASTDONEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtLastDonedate" runat="server" Width="100%" MaxLength="10" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDJOBLASTDONEDATE")) %>'></eluc:Date>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Done Hour" ColumnGroupName="LastDoneHour" AllowFiltering="false">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Width="60px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastDoneHour" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDLASTDONEHOURS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtLastDoneHour" Width="100%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTDONEHOURS") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                Sys.Application.add_load(function () {
                    setTimeout(function () {
                        TelerikGridResize($find("<%= gvComponentJob.ClientID %>"));
                    }, 200);
                });
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>

