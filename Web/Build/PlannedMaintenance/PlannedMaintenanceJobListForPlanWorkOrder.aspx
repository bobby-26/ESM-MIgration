<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceJobListForPlanWorkOrder.aspx.cs"
    Inherits="PlannedMaintenanceJobListForPlanWorkOrder" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Jobs</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function CloseWindow() {
                document.getElementById('<%=ValidationSummary1.ClientID%>').style.display = 'none';
            }
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                    radwindow.close();
                    var masterTable = $find("<%= gvWorkOrder.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }

        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:TabStrip ID="MenuWorkOrderRequestion" runat="server" OnTabStripCommand="MenuWorkOrderRequestion_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkOrder" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvWorkOrder_NeedDataSource" AllowMultiRowSelection="true" AllowFilteringByColumn="true" FilterType="CheckList"
                OnItemCommand="gvWorkOrder_ItemCommand" Height="90%" OnSortCommand="gvWorkOrder_SortCommand" EnableViewState="true" EnableLinqExpressions="false" OnItemDataBound="gvWorkOrder_ItemDataBound">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" ClientDataKeyNames="FLDWORKORDERID" AllowFilteringByColumn="true">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="130px" HeaderText="Comp. No." DataField="FLDCOMPONENTNUMBER" AutoPostBackOnFilter="false" UniqueName="FLDCOMPONENTNUMBER"
                            AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNUMBER" ShowFilterIcon="false" CurrentFilterFunction="Contains" FilterDelay="2000">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="170px" HeaderText="Comp. Name" AllowSorting="true" DataField="FLDCOMPONENTNAME" AutoPostBackOnFilter="false"
                            ShowSortIcon="true" SortExpression="FLDCOMPONENTNAME" UniqueName="FLDCOMPONENTNAME" CurrentFilterFunction="Contains"
                            FilterDelay="2000" ShowFilterIcon="false" FilterControlWidth="150px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDCOMPONENTID"]%>'></telerik:RadLabel>
                                <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Comp. Category" AutoPostBackOnFilter="false" HeaderStyle-Width="150px" UniqueName="FLDCOMPCATEGORY">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cblCompCategory" runat="server" OnDataBinding="cblCompCategory_DataBinding" AutoPostBack="true" Width="100%" AppendDataBoundItems="true"
                                    OnSelectedIndexChanged="cblCompCategory_SelectedIndexChanged" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("FLDCOMPCATEGORY").CurrentFilterValue %>'
                                    AllowCustomText="true" MarkFirstMatch="true" Text="Select category">
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompCategory" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDCOMPCATEGORY"]%>'></telerik:RadLabel>
                                <%#((DataRowView)Container.DataItem)["FLDCOMPCATEGORYNAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Code/Title" AllowSorting="true" AllowFiltering="false" HeaderStyle-Width="160px"
                            SortExpression="FLDWORKORDERNAME" DataField="FLDWORKORDERNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWorkOrderId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERID"] %>'></telerik:RadLabel>
                                <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="135px" HeaderText="Job Category" AllowSorting="true" UniqueName="FLDJOBCATEGORYID">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cblJobCategory" runat="server" OnDataBinding="cblJobCategory_DataBinding" AutoPostBack="true" Width="100%" AppendDataBoundItems="true"
                                    OnSelectedIndexChanged="cblJobCategory_SelectedIndexChanged" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("FLDJOBCATEGORYID").CurrentFilterValue %>'>
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobCategory" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDJOBCATEGORYID"]%>'></telerik:RadLabel>
                                <%# DataBinder.Eval(Container,"DataItem.FLDJOBCATEGORY") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="135px" HeaderText="Job Class" AllowSorting="true"
                            AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDJOBCLASS") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="135px" HeaderText="Maintenance Interval" AllowSorting="true"
                            AllowFiltering="true" DataField="FLDFREQUENCYNAME" UniqueName="FLDFREQUENCYTYPE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cblFrequencyType" runat="server" OnDataBinding="cblFrequencyType_DataBinding" AutoPostBack="true" Width="100%" AppendDataBoundItems="true"
                                    OnSelectedIndexChanged="cblFrequencyType_SelectedIndexChanged" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("FLDFREQUENCYTYPE").CurrentFilterValue %>'>
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFrequency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCY") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFrequencyType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYTYPE") %>' Visible="false"></telerik:RadLabel>
                                <%#((DataRowView)Container.DataItem)["FLDFREQUENCYNAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Responsibility" HeaderStyle-Width="150px" ShowFilterIcon="true"
                            ShowSortIcon="true" SortExpression="FLDDISCIPLINENAME" DataField="FLDDISCIPLINENAME" UniqueName="FLDPLANNINGDISCIPLINE">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="cblDiscipline" runat="server" OnDataBinding="cblDiscipline_DataBinding" AutoPostBack="true" Width="100%" AppendDataBoundItems="true"
                                    OnSelectedIndexChanged="cblDiscipline_SelectedIndexChanged" SelectedValue='<%# ((GridItem)Container).OwnerTableView.GetColumn("FLDPLANNINGDISCIPLINE").CurrentFilterValue %>'>
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDisplineId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANNINGDISCIPLINE") %>'></telerik:RadLabel>
                                <%#((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due" HeaderStyle-Width="150px" AllowSorting="true" FilterDelay="2000"
                            ShowSortIcon="true" SortExpression="FLDPLANNINGDUEDATE" DataField="FLDPLANNINGDUEDATE" UniqueName="FLDPLANNINGDUEDATE">
                            <FilterTemplate>
                                From<telerik:RadDatePicker ID="FromOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="FromDateSelected"
                                    DbSelectedDate='<%# ViewState["FilterFromDate"].ToString() %>' />
                                <br />
                                To&nbsp&nbsp&nbsp&nbsp<telerik:RadDatePicker ID="ToOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="ToDateSelected"
                                    DbSelectedDate='<%# ViewState["FilterToDate"].ToString() %>' />
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
                                <telerik:RadLabel ID="lblDuedate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Done" HeaderStyle-Width="108px" AllowSorting="true" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridClientSelectColumn UniqueName="ClientSelectColumn" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center" EnableHeaderContextMenu="true">
                        </telerik:GridClientSelectColumn>
                        <telerik:GridTemplateColumn HeaderText="Find Related" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkFind" runat="server" Text="Find" ToolTip="Find Related Jobs" CommandName="FIND">
                                    <span class="icon"><i class="fas fa-search"></i></span>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
        <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
        <telerik:RadWindow ID="modalPopup" runat="server" Width="400px" Height="365px" Modal="true" OffsetElementID="main" OnClientClose="CloseWindow">
            <ContentTemplate>
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="LoadingPanel1">
                    <eluc:TabStrip ID="menuWorkorderCreate" runat="server" OnTabStripCommand="menuWorkorderCreate_TabStripCommand" />
                    <table border="0">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTitle" runat="server" CssClass="input_mandatory upperCase"
                                    MaxLength="200" Width="100%">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDueDate" runat="server" Text="Planned Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="txtDueDate" runat="server" Width="100%" CssClass="input_mandatory" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Assigned To"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Discipline ID="ddlResponsible" runat="server" CssClass="input_mandatory" Width="100%"
                                    AppendDataBoundItems="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPlannedJob" runat="server" Text="Routine WO. ?"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadRadioButtonList ID="rblPlannedJob" runat="server" Direction="Horizontal">
                                    <Items>
                                        <telerik:ButtonListItem Text="Yes" Value="0" />
                                        <telerik:ButtonListItem Text="No" Value="1" Selected="true" />
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ValidationSummary ID="ValidationSummary1" runat="server" Width="200px"
                                    BorderWidth="1px" HeaderText="List of errors"></asp:ValidationSummary>
                            </td>
                        </tr>
                    </table>
                </telerik:RadAjaxPanel>
            </ContentTemplate>
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
