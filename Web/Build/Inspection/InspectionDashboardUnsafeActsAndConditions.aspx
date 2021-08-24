<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDashboardUnsafeActsAndConditions.aspx.cs" Inherits="InspectionDashboardUnsafeActsAndConditions" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvDirectIncident.ClientID %>"));
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
    <form id="frmInspection" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuARSubTab" runat="server" TabStrip="true" OnTabStripCommand="MenuARSubTab_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuRegistersInspection" runat="server" OnTabStripCommand="RegistersInspection_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvDirectIncident" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true" FilterMenu-EnableViewState="true"
                Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvDirectIncident_ItemCommand" OnItemDataBound="gvDirectIncident_ItemDataBound"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" AllowSorting="true" GridLines="None" OnNeedDataSource="gvDirectIncident_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDDIRECTINCIDENTID" AllowFilteringByColumn="true">
                    <Columns>
                         <telerik:GridTemplateColumn HeaderText="Vessel" UniqueName="FLDVESSELID" DataField="FLDVESSELID">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                             <FilterTemplate>
                                <telerik:RadComboBox ID="ucVessel" runat="server" OnDataBinding="ucVessel_DataBinding" AppendDataBoundItems="true" AutoPostBack="true"
                                    Width="100%" OnSelectedIndexChanged="ucVessel_DataBinding_SelectedIndexChanged" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID">
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblVessel" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ref. No" DataField="FLDINCIDENTNEARMISSREFNO" UniqueName="FLDINCIDENTNEARMISSREFNO"
                            FilterDelay="2000" HeaderStyle-Width="8%" FilterControlWidth="99%"
                            AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <HeaderStyle Width="11%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRefno" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTNEARMISSREFNO") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reported"  DataField="FLDREPORTEDDATE" UniqueName="FLDREPORTEDDATE" AutoPostBackOnFilter="true" FilterDelay="2000">
                            <HeaderStyle Width="12%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <FilterTemplate>
                                From<telerik:RadDatePicker ID="FromOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="FromDateSelected"
                                    DbSelectedDate='<%# ViewState["ucFrom"].ToString() %>' />
                                <br />
                                To&nbsp&nbsp&nbsp&nbsp&nbsp<telerik:RadDatePicker ID="ToOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="ToDateSelected"
                                    DbSelectedDate='<%# ViewState["ucTo"].ToString() %>' />
                                <telerik:RadScriptBlock ID="RadScriptBlock2" runat="server">
                                    <script type="text/javascript">
                                        function FromDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var ToPicker = $find('<%# ((GridItem)Container).FindControl("ToOrderDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(sender);
                                            var toDate = FormatSelectedDate(ToPicker);

                                            tableView.filter("FLDREPORTEDDATE", fromDate + "~" + toDate, "Between");

                                        }
                                        function ToDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromOrderDatePicker").ClientID %>');

                                        var fromDate = FormatSelectedDate(FromPicker);
                                        var toDate = FormatSelectedDate(sender);

                                        tableView.filter("FLDREPORTEDDATE", fromDate + "~" + toDate, "Between");
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
                                <telerik:RadLabel ID="lblReportedDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREPORTEDDATE")) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDirectIncidentId" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDDIRECTINCIDENTID")) %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Incident" DataField="FLDINCIDENTDATE" AllowFiltering="false" UniqueName="FLDINCIDENTDATE" AutoPostBackOnFilter="true">
                            <HeaderStyle Width="7%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <%--<FilterTemplate>
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

                                            tableView.filter("FLDINCIDENTDATE", fromDate + "~" + toDate, "Between");

                                        }
                                        function ToReportDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromReportDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(FromPicker);
                                            var toDate = FormatSelectedDate(sender);

                                            tableView.filter("FLDINCIDENTDATE", fromDate + "~" + toDate, "Between");
                                    }
                                    function FromReportDateSelected(picker) {
                                        var date = picker.get_selectedDate();
                                        var dateInput = picker.get_dateInput();
                                        var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());

                                        return formattedDate;
                                    }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDINCIDENTDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Time" AllowFiltering="false">
                            <HeaderStyle Width="5%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTTIME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" FilterDelay="2000" FilterControlWidth="99%" DataField="FLDICCATEGORYNAME" UniqueName="FLDICCATEGORYNAME"
                            AutoPostBackOnFilter="true" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlCategory" runat="server" AutoPostBack="true" AppendDataBoundItems="true" OnDataBinding="ddlCategory_DataBinding"
                                     DataValueField="FLDHARDCODE" DataTextField="FLDHARDNAME" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" SelectedValue='<%# ViewState["ucCategory"] %>' Width="100%">
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDICCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub-category" DataField="FLDICSUBCATEGORYNAME" UniqueName="FLDICSUBCATEGORYNAME">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlSubcategory" Width="100%" runat="server" AutoPostBack="true" AppendDataBoundItems="true" OnDataBinding="ddlSubcategory_DataBinding"
                                    DataValueField="FLDIMMEDIATECAUSEID" DataTextField="FLDIMMEDIATECAUSE" OnSelectedIndexChanged="ddlSubcategory_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlSubcategory"] %>'>
                        </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDICSUBCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Details" AllowFiltering="false">
                            <HeaderStyle Width="12%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSummaryFirstLine" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUMMARY").ToString().Length>35 ? DataBinder.Eval(Container, "DataItem.FLDSUMMARY").ToString().Substring(0, 35)+ "..." : DataBinder.Eval(Container, "DataItem.FLDSUMMARY").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipSummary" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUMMARY") %>' TargetControlId="lblSummaryFirstLine" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Location" AllowFiltering="false">
                            <HeaderStyle Width="9%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLocation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLOCATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action Taken" AllowFiltering="false">
                            <HeaderStyle Width="14%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActionTaken" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCORRECTIVEACTION").ToString().Length>35 ? DataBinder.Eval(Container, "DataItem.FLDCORRECTIVEACTION").ToString().Substring(0, 35)+ "..." : DataBinder.Eval(Container, "DataItem.FLDCORRECTIVEACTION").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipActionTaken" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCORRECTIVEACTION") %>' TargetControlId="lblActionTaken" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" DataField="FLDSTATUSNAME" UniqueName="FLDSTATUSNAME" AutoPostBackOnFilter="true">
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <FilterTemplate>
                                <%--<eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="168" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlStatus"] %>' Width="100%" />--%>
                                <telerik:RadComboBox ID="ddlStatus" runat="server" AutoPostBack="true" AppendDataBoundItems="true" OnDataBinding="ddlStatus_DataBinding"
                                     DataValueField="FLDHARDCODE" DataTextField="FLDHARDNAME" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" SelectedValue='<%# ViewState["Status"] %>' Width="100%">
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblcompletedyn" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="5%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCELACTION" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
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
