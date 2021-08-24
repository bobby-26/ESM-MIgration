<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDashBoardMachineryDamageDBListExtn.aspx.cs"
    Inherits="InspectionDashBoardMachineryDamageDBListExtn" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlIncidentNearMissCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubCategory" Src="~/UserControls/UserControlIncidentNearMissSubCategory.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Incident List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvInspectionIncidentSearch.ClientID %>"));
               }, 200);
           }
           window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
               fade('statusmessage');
           }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionIncidentSearch" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuARSubTab" runat="server" TabStrip="true" OnTabStripCommand="MenuARSubTab_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <%--<eluc:TabStrip ID="MenuIncidentGeneral" TabStrip="true" runat="server" OnTabStripCommand="IncidentGeneral_TabStripCommand"></eluc:TabStrip>--%>
            <eluc:TabStrip ID="MenuIncidentSearch" runat="server" OnTabStripCommand="IncidentSearch_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvInspectionIncidentSearch" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvInspectionIncidentSearch_ItemCommand" FilterMenu-EnableViewState="true"
                OnItemDataBound="gvInspectionIncidentSearch_ItemDataBound" ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvInspectionIncidentSearch_NeedDataSource"
                OnSortCommand="gvInspectionIncidentSearch_SortCommand1">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDMACHINERYDAMAGEID" AllowFilteringByColumn="true">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="true" SortExpression="FLDVESSELNAME" ShowFilterIcon="false" UniqueName="FLDVESSELID" DataField="FLDVESSELID">
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ucVessel" runat="server" OnDataBinding="ucVessel_DataBinding" AppendDataBoundItems="true" AutoPostBack="true"
                                    Width="100%" OnSelectedIndexChanged="ucVessel_DataBinding_SelectedIndexChanged" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID">
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'>
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ref. No" AllowSorting="true" SortExpression="FLDREFERENCENUMBER" DataField="FLDREFERENCENUMBER" UniqueName="FLDREFERENCENUMBER"
                            FilterDelay="2000" HeaderStyle-Width="10%" FilterControlWidth="100%"
                            AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkReferenceNumber" runat="server" CommandName="NAVIGATE" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblMachineryDamageId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMACHINERYDAMAGEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Incident [LT]" AllowSorting="true" SortExpression="FLDINCIDENTDATE"
                            DataField="FLDINCIDENTDATE" UniqueName="FLDINCIDENTDATE" AutoPostBackOnFilter="true" FilterDelay="2000">
                            <FilterTemplate>
                                From<telerik:RadDatePicker ID="FromOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="FromDateSelected"
                                    DbSelectedDate='<%# ViewState["FDATE"].ToString() %>' />
                                <br />
                                To&nbsp&nbsp&nbsp&nbsp&nbsp<telerik:RadDatePicker ID="ToOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="ToDateSelected"
                                    DbSelectedDate='<%# ViewState["TDATE"].ToString() %>' />
                                <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
                                    <script type="text/javascript">
                                        function FromDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var ToPicker = $find('<%# ((GridItem)Container).FindControl("ToOrderDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(sender);
                                            var toDate = FormatSelectedDate(ToPicker);

                                            tableView.filter("FLDINCIDENTDATE", fromDate + "~" + toDate, "Between");

                                        }
                                        function ToDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromOrderDatePicker").ClientID %>');

                                        var fromDate = FormatSelectedDate(FromPicker);
                                        var toDate = FormatSelectedDate(sender);

                                        tableView.filter("FLDINCIDENTDATE", fromDate + "~" + toDate, "Between");
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
                            <HeaderStyle Width="12%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIncidentDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDINCIDENTDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reported" AllowSorting="true" SortExpression="FLDREPORTEDDATE" FilterDelay="2000" 
                            ShowSortIcon="true" DataField="FLDREPORTEDDATE" UniqueName="FLDREPORTEDDATE" AutoPostBackOnFilter="true">
                            <HeaderStyle Width="12%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FilterTemplate>
                                From<telerik:RadDatePicker ID="FromReportDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="FromReportDateSelected"
                                    DbSelectedDate='<%# ViewState["FRDATE"].ToString() %>' />
                                <br />
                                To&nbsp&nbsp&nbsp&nbsp<telerik:RadDatePicker ID="ToReportDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="ToReportDateSelected"
                                    DbSelectedDate='<%# ViewState["TRDATE"].ToString() %>' />
                                <telerik:RadScriptBlock ID="RadScriptBlock3" runat="server">
                                    <script type="text/javascript">
                                        function FromReportDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var ToPicker = $find('<%# ((GridItem)Container).FindControl("ToReportDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(FromPicker);
                                            var toDate = FormatSelectedDate(sender);

                                            tableView.filter("FLDREPORTEDDATE", fromDate + "~" + toDate, "Between");

                                        }
                                        function ToReportDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromReportDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(FromPicker);
                                            var toDate = FormatSelectedDate(sender);

                                        tableView.filter("FLDREPORTEDDATE", fromDate + "~" + toDate, "Between");
                                    }
                                    function FromReportDateSelected(picker) {
                                        var date = picker.get_selectedDate();
                                        var dateInput = picker.get_dateInput();
                                        var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());

                                        return formattedDate;
                                    }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReportedBy" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDREPORTEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                         <telerik:GridTemplateColumn HeaderText="Title" DataField="FLDTITLE" UniqueName="FLDTITLE"
                            FilterDelay="2000" FilterControlWidth="99%"
                            AutoPostBackOnFilter="true" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <HeaderStyle Width="12%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE").ToString().Length > 30 ? DataBinder.Eval(Container, "DataItem.FLDTITLE").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDTITLE").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucIncidentTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>' TargetControlId="lblTitle" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Component" AllowFiltering="false">
                            <HeaderStyle Width="16%" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblComponentId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME").ToString() %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Cons" FilterDelay="2000" FilterControlWidth="99%" DataField="FLDCONSEQUENCECATEGORYNAME" UniqueName="FLDCONSEQUENCECATEGORYNAME"
                            AutoPostBackOnFilter="true" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <FilterTemplate>
                                <%--<eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="168" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlStatus"] %>' Width="100%" />--%>
                                <telerik:RadComboBox ID="ddlCon" runat="server" AutoPostBack="true" AppendDataBoundItems="true" OnDataBinding="ddlCon_DataBinding"
                                     DataValueField="FLDHARDCODE" DataTextField="FLDHARDNAME" OnSelectedIndexChanged="ddlCon_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlCon"] %>' Width="110%">
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <HeaderStyle Width="6%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConsequenceCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSEQUENCECATEGORY" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Cargo NM" AllowFiltering="false">
                            <HeaderStyle Width="6%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCargoNM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGONEARMISSYN") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Nav NM" AllowFiltering="false">
                            <HeaderStyle Width="6%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNavNM" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAVIGATIONNEARMISSYN") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Incident Ref. No." AllowFiltering="false">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIncidentRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTREFNO") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Closed" SortExpression="FLDCLOSEDDATE" FilterDelay="2000" 
                            ShowSortIcon="true" DataField="FLDCLOSEDDATE" UniqueName="FLDCLOSEDDATE" AutoPostBackOnFilter="true">
                            <FilterTemplate>
                                From<telerik:RadDatePicker ID="FromCloseDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="FromDateSelected"
                                    DbSelectedDate='<%# ViewState["ucClosedFrom"].ToString() %>' />
                                <br />
                                To&nbsp&nbsp&nbsp&nbsp&nbsp<telerik:RadDatePicker ID="ToCloseDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="ToDateSelected"
                                    DbSelectedDate='<%# ViewState["ucClosedTo"].ToString() %>' />
                                <telerik:RadScriptBlock ID="RadClosedatepickerScriptBlock" runat="server">
                                    <script type="text/javascript">
                                        function FromDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var ToPicker = $find('<%# ((GridItem)Container).FindControl("ToCloseDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(sender);
                                            var toDate = FormatSelectedDate(ToPicker);

                                            tableView.filter("FLDCLOSEDDATE", fromDate + "~" + toDate, "Between");

                                        }
                                        function ToDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromCloseDatePicker").ClientID %>');

                                        var fromDate = FormatSelectedDate(FromPicker);
                                        var toDate = FormatSelectedDate(sender);

                                        tableView.filter("FLDCLOSEDDATE", fromDate + "~" + toDate, "Between");
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
                            <HeaderStyle Width="12%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClosedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCLOSEDDATE")) %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Status" DataField="FLDSTATUSNAME" UniqueName="FLDSTATUSOFINCIDENT" AutoPostBackOnFilter="true">
                            <FilterTemplate>
                                <%--<eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="168" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlStatus"] %>' Width="100%" />--%>
                                <telerik:RadComboBox ID="ddlStatus" runat="server" AutoPostBack="true" AppendDataBoundItems="true" OnDataBinding="ddlStatus_DataBinding"
                                     DataValueField="FLDSHORTNAME" DataTextField="FLDSTATUSNAME" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" SelectedValue='<%# ViewState["Status"] %>' Width="100%">
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLDSTATUSNAME" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
       
                        <telerik:GridTemplateColumn HeaderText="Action" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Generic PDF" CommandName="MACHINERY" ID="cmdMachinery" ToolTip="Show PDF">
                                    <span class="icon"><i class="fas fa-chart-bar"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
