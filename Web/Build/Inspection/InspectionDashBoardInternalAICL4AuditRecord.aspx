<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDashBoardInternalAICL4AuditRecord.aspx.cs" Inherits="InspectionDashBoardInternalAICL4AuditRecord" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="IChapter" Src="~/UserControls/UserControlInspectionChapter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ITopic" Src="~/UserControls/UserControlInspectionTopic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Checklist" Src="~/UserControls/UserControlInspectionChecklist.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
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
                    TelerikGridResize($find("<%= gvAuditRecordList.ClientID %>"));
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
    <form id="frmInspectionScheduleSearch" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:TabStrip ID="MenuARSubTab" runat="server" TabStrip="true" OnTabStripCommand="MenuARSubTab_TabStripCommand"></eluc:TabStrip>
            <%--        <eluc:TabStrip ID="MenuGeneral" runat="server" TabStrip="true" OnTabStripCommand="MenuGeneral_TabStripCommand"></eluc:TabStrip>--%>
            <eluc:TabStrip ID="MenuInspectionScheduleSearch" runat="server" OnTabStripCommand="InspectionScheduleSearch_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:Title runat="server" ID="ucTitle" Text="Audit / Inspection Log" Visible="false"></eluc:Title>
            <asp:Button ID="ucConfirm" runat="server" OKText="Yes" CancelText="No" OnClick="ucConfirm_Click" />
            <asp:Button ID="ucConfirmDelete" runat="server" Text="ConfirmDelete" OnClick="btnConfirmDelete_Click" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAuditRecordList" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" AutoGenerateColumns="False" Font-Size="11px"
                CellSpacing="0" GridLines="None" Width="100%" OnItemCommand="gvAuditRecordList_ItemCommand" OnNeedDataSource="gvAuditRecordList_NeedDataSource"
                OnItemDataBound="gvAuditRecordList_ItemDataBound" OnSortCommand="gvAuditRecordList_SortCommand" FilterMenu-EnableViewState="true"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                ShowFooter="false" ShowHeader="true" EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDREVIEWSCHEDULEID" AllowFilteringByColumn="true">
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup Name="AuditInsp" HeaderText="Audit/Inspection" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup Name="DeficiencyCount" HeaderText="Def Count" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="85px" UniqueName="FLDVESSELID">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ucVessel" runat="server" OnDataBinding="ucVessel_DataBinding" AppendDataBoundItems="true" AutoPostBack="true"
                                    Width="100%" OnSelectedIndexChanged="ucVessel_DataBinding_SelectedIndexChanged" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID">
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <asp:LinkButton ID ="lnkVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblVesselCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>                                
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="M/C" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblManual" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISMANUAL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ref. No" HeaderStyle-Width="106px" AllowSorting="true" SortExpression="FLDREFERENCENUMBER" DataField="FLDREFERENCENUMBER" UniqueName="FLDREFERENCENUMBER"
                            FilterDelay="2000" FilterControlWidth="99%"
                            AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInspectionRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER").ToString() %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipInspectionRefNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>' TargetControlId="lblInspectionRefNo" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="95px" HeaderText="Audit / Inspection" UniqueName="FLDSHORTCODE" DataField="FLDINSPECTIONID">
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlAudit" runat="server" AutoPostBack="true"  AppendDataBoundItems="true"
                                Width="100%" OnDataBinding="ddlAudit_DataBinding" OnSelectedIndexChanged="ddlAudit_SelectedIndexChanged" DataTextField="FLDSHORTCODE" DataValueField="FLDINSPECTIONID"
                                    SelectedValue='<%# ViewState["ucAudit"] %>'>
                            </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="130px"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkInspectionNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDREVIEWNAME">Audit / Inspection</asp:LinkButton>
                                <img id="FLDREVIEWNAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInspectionScheduleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWSCHEDULEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblPlannerId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWSCHEDULEPLANNERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInspectionShortcode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblInspectionDtKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkInspection" runat="server" CommandName="NAVIGATE" CommandArgument="<%# Container.DataItem %>" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE").ToString().Length>35 ? DataBinder.Eval(Container, "DataItem.FLDSHORTCODE").ToString().Substring(0, 35)+ "..." : DataBinder.Eval(Container, "DataItem.FLDSHORTCODE").ToString() %>'></asp:LinkButton>
                                <eluc:ToolTip ID="ucToolTipName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>' TargetControlId="lnkInspection" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="100PX" HeaderText="Last Done" DataField="FLDCOMPLETIONDATE" UniqueName="FLDCOMPLETIONDATE" AutoPostBackOnFilter="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                            <FilterTemplate>
                                From<telerik:RadDatePicker ID="FromCloseDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="FromDateSelected"
                                    DbSelectedDate='<%# ViewState["ucFrom"].ToString() %>' />
                                <br />
                                To&nbsp&nbsp&nbsp&nbsp&nbsp<telerik:RadDatePicker ID="ToCloseDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="ToDateSelected"
                                    DbSelectedDate='<%# ViewState["ucTo"].ToString() %>' />
                                <telerik:RadScriptBlock ID="RadClosedatepickerScriptBlock" runat="server">
                                    <script type="text/javascript">
                                        function FromDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var ToPicker = $find('<%# ((GridItem)Container).FindControl("ToCloseDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(sender);
                                            var toDate = FormatSelectedDate(ToPicker);

                                            tableView.filter("FLDCOMPLETIONDATE", fromDate + "~" + toDate, "Between");

                                        }
                                        function ToDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromCloseDatePicker").ClientID %>');

                                        var fromDate = FormatSelectedDate(FromPicker);
                                        var toDate = FormatSelectedDate(sender);

                                        tableView.filter("FLDCOMPLETIONDATE", fromDate + "~" + toDate, "Between");
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
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkCompletionDateHeaderr" runat="server" CommandName="Sort" CommandArgument="FLDCOMPLETIONDATE">Last Done</asp:LinkButton>
                                <img id="FLDCOMPLETIONDATE" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInspectionCompletionDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE"))%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Credit" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="52px" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="70px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCreditdata" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREDITEDYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="70px" UniqueName="FLDSTATUSNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60px"></ItemStyle>
                            <FilterTemplate>
                                <telerik:RadComboBox ID="ddlStatus" runat="server" AutoPostBack="true" AppendDataBoundItems="true" OnDataBinding="ddlStatus_DataBinding"
                                     DataValueField="FLDHARDCODE" DataTextField="FLDHARDNAME" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged" SelectedValue='<%# ViewState["ucStatus"] %>' Width="97%">
                                </telerik:RadComboBox>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Width="50px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipStatus" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>' TargetControlId="lblStatus" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From" HeaderStyle-Width="80px" ColumnGroupName="FromToPort" AllowFiltering="false">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFromPort" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMPORT").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipFromPort" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMPORT") %>' TargetControlId="lblFromPort" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To" HeaderStyle-Width="80px" ColumnGroupName="FromToPort" AllowFiltering="false">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblToPort" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOPORT").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipToPort" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOPORT") %>' TargetControlId="lblToPort" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" ColumnGroupName="AuditInsp" AllowFiltering="false">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="110px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNameOfInspector" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipInspector" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR") %>' TargetControlId="lblNameOfInspector" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Organization" HeaderStyle-Width="85px" ColumnGroupName="AuditInsp" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrganisation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORGANISATION").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipOrganisation" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORGANISATION") %>' TargetControlId="lblOrganisation" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Attached By" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAttachmentcreatedby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTCREATEDBY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="45px" ColumnGroupName="DeficiencyCount" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDeficiencyCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="MNC" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="43px" ColumnGroupName="DeficiencyCount" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMajorNCCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAJORNCCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="NC" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="38px" ColumnGroupName="DeficiencyCount" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNCCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNCCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="OBS" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="40px" ColumnGroupName="DeficiencyCount" AllowFiltering="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOBSCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOBSCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="112px" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAtt" ToolTip="Attachment">
                                     <span class="icon"> <i class="fa fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Summary" CommandName="DEFICIENCYSUMMARY"
                                    ID="cmdDeficiencySummary" ToolTip="View Deficiency Summary">
                                     <span class="icon"><i class="fas fa-eye"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Export XL" CommandName="EXPORT2XL" ID="cmdExport2XL" ToolTip="Export XL">
                                      <span class="icon"><i class="fas fa-file-excel"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" Visible="true" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="CHECKLIST" ID="cmdChecklist"
                                    ToolTip="Checklist Verification">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
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
        <table cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblManual" runat="server" Text="* M - Manual "></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblConfiguration" runat="server" Text="* C - Configuration"></telerik:RadLabel>
                </td>
            </tr>
        </table>

    </form>
</body>
</html>
