<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDashboardOfficeIncidentListExtn.aspx.cs"
    Inherits="InspectionDashboardOfficeIncidentListExtn" %>

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
                    AutoGenerateColumns="false" DataKeyNames="FLDINSPECTIONINCIDENTID" AllowFilteringByColumn="true">
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
                                <telerik:RadLabel ID="lblVesselCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ref. No" AllowSorting="true" SortExpression="FLDINCIDENTREFNO" DataField="FLDINCIDENTREFNO" UniqueName="FLDINCIDENTREFNO"
                            FilterDelay="2000" HeaderStyle-Width="8%" FilterControlWidth="99%"
                            AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInspectionIncidentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONINCIDENTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStatusid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSOFINCIDENT" ) %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkIncidentRefNo" runat="server" CommandName="EDIT"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTREFNO") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" ShowFilterIcon="false" AllowFiltering="false" UniqueName="FLDISINCIDENTORNEARMISS" DataField="FLDISINCIDENTORNEARMISS" AutoPostBackOnFilter="true">
                           <%-- <FilterTemplate>
                                <telerik:RadComboBox ID="ddlIncidentNearmiss" runat="server" Width="97%" AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlIncidentNearmiss_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlType"] %>' >
                                    <Items>
                                        <telerik:RadComboBoxItem Text="All" Value="0" Selected="True" />
                                        <telerik:RadComboBoxItem Text="Accident" Value="1" />
                                        <telerik:RadComboBoxItem Text="Near Miss" Value="2" />
                                    </Items>
                                </telerik:RadComboBox>
                            </FilterTemplate> --%>                           
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClassification" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTCLASSIFICATION" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" AllowSorting="true" SortExpression="FLDCATEGORYNAME" DataField="FLDCATEGORY" UniqueName="FLDCATEGORY"
                            FilterDelay="2000" FilterControlWidth="99%"
                            AutoPostBackOnFilter="true" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlCategory"] %>'
                                    OnDataBinding="ddlCategory_DataBinding" AppendDataBoundItems="true" DataTextField="FLDNAME" DataValueField="FLDINCIDENTNEARMISSCATEGORYID" Width="97%"></telerik:RadComboBox>                                
                            </FilterTemplate>
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Subcategory" FilterDelay="2000" FilterControlWidth="99%" DataField="FLDSUBCATEGORY" UniqueName="FLDSUBCATEGORY"
                            AutoPostBackOnFilter="true" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlSubCategory" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSubCategory_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlSubCategory"] %>'
                                    OnDataBinding="ddlSubCategory_DataBinding" AppendDataBoundItems="true" DataTextField="FLDNAME" DataValueField="FLDINCIDENTNEARMISSSUBCATEGORYID" Width="97%"></telerik:RadComboBox>                                
                            </FilterTemplate>
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Cons" FilterDelay="2000" FilterControlWidth="99%" DataField="FLDINCIDENTCATEGORY" UniqueName="FLDINCIDENTCATEGORY"
                            AutoPostBackOnFilter="true" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <FilterTemplate>
                                <%--<eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="168" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlStatus"] %>' Width="100%" />--%>
                                <telerik:RadComboBox ID="ddlCon" runat="server" AutoPostBack="true" AppendDataBoundItems="true" OnDataBinding="ddlCon_DataBinding"
                                     DataValueField="FLDHARDCODE" DataTextField="FLDHARDNAME" OnSelectedIndexChanged="ddlCon_SelectedIndexChanged" SelectedValue='<%# ViewState["ddlCon"] %>' Width="97%">
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <HeaderStyle Width="6%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblConsequenceCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSEQUENCECATEGORY" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" DataField="FLDINCIDENTTITLE" UniqueName="FLDINCIDENTTITLE"
                            FilterDelay="2000" FilterControlWidth="99%"
                            AutoPostBackOnFilter="true" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <HeaderStyle Width="12%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTTITLE").ToString().Length > 30 ? DataBinder.Eval(Container, "DataItem.FLDINCIDENTTITLE").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDINCIDENTTITLE").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucIncidentTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTTITLE") %>' TargetControlId="lblTitle" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reported" AllowSorting="true" SortExpression="FLDREPORTEDDATE" FilterDelay="2000" 
                            ShowSortIcon="true" DataField="FLDREPORTEDDATE" UniqueName="FLDREPORTEDDATE" AutoPostBackOnFilter="true">
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
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
                        <telerik:GridTemplateColumn HeaderText="Incident" AllowSorting="true" SortExpression="FLDINCIDENTDATE" 
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
                            <HeaderStyle Width="10%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIncidentDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDINCIDENTDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" DataField="FLDSTATUSOFINCIDENT" UniqueName="FLDSTATUSOFINCIDENT" AutoPostBackOnFilter="true">
                            <FilterTemplate>
                                
                                <telerik:RadComboBox ID="ddlStatus" runat="server" AutoPostBack="true" AppendDataBoundItems="true" OnDataBinding="ddlStatus_DataBinding"
                                     DataValueField="FLDHARDCODE" DataTextField="FLDHARDNAME" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" SelectedValue='<%# ViewState["Status"] %>' Width="97%">
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSOFINCIDENTNAME" ) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <%--<telerik:GridTemplateColumn HeaderText="Closed" AllowFiltering="false">
                            <HeaderStyle Width="7%" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClosedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCLOSEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Closed By" AllowFiltering="false">
                            <HeaderStyle Width="8%" HorizontalAlign="Left" />
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClosedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLOSEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>--%>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Approve" CommandName="APPROVE" ID="cmdApprove"
                                    ToolTip="Close">
                                    <span class="icon"><i class="fas fa-user-check"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCELINCIDENT" ID="cmdCancel" ToolTip="Cancel Incident">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete Incident"
                                    Visible="false">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Incident Report" CommandName="INCIDENTREPORT"
                                    ID="cmdReport" ToolTip="Incident / Near Miss Report">
                                    <span class="icon"><i class="fas fa-chart-bar"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Third Party Incident Report" CommandName="THIRDPARTYINCIDENTREPORT"
                                    ID="cmdThirdPartyReport" ToolTip="Third Party Incident / Near Miss Report">
                                    <span class="icon"><i class="fas fa-book"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Unlock Incident" CommandName="UNLOCK"
                                    Visible="false" ID="cmdUnlockIncident" ToolTip="Unlock Incident/Near Miss for Vessel">
                                    <span class="icon"><i class="fas fa-unlock"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Director Comments" CommandName="DIRECTORCOMMENTS"
                                    ID="imgDirectorComments" ToolTip="Director Comments">
                                    <span class="icon"><i class="fas fa-comments"></i></span>
                                </asp:LinkButton>
                                <%-- <asp:LinkButton runat="server" AlternateText="Communication"
                                    CommandName="COMMUNICATION" ID="lnkCommunication" ToolTip="Communication">
                                <span class="icon"><i class="fas fa-postcomment"></i></span>
                                </asp:LinkButton>--%>
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
