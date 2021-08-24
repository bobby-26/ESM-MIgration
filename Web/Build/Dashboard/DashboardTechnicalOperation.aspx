<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalOperation.aspx.cs" Inherits="Dashboard_DashboardTechnicalOperation" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Operation</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">                                 
            function resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvProgress.ClientID %>"));
                }, 200);
            }
            function pageLoad() {
                resize();
            }
            window.onresize = window.onload = resize;
            function CloseUrlModelWindow(gridid) {
                var wnd = $find('<%=RadWindow_NavigateUrl.ClientID %>');
                wnd.close();
                var masterTable = $find('<%=gvProgress.ClientID %>').get_masterTableView();
                masterTable.rebind();
            }
            function refresh() {
                var ajxmgr = parent.frames[1].$find("RadAjaxManager1");
                if (ajxmgr != null)
                    ajxmgr.ajaxRequest("OPERATION");
            }            
            function itemClicked(list, args) {
                var $ = $telerik.$;
                var gridRowElem = $(list._element).parents("tr").first()[0];
                var grid = $find("gvProgress");
                var masterTable = grid.get_masterTableView();
                var item = masterTable.get_dataItems()[gridRowElem.rowIndex - 1];
                var did = item.getDataKeyValue("FLDDAILYPLANACTIVITYID");
                var val = args.get_item().get_value();
                var txt = args.get_item().get_text();
                
                function onConfirmStatus(arg) {
                    if (arg) {

                        var button = $find("<%= cmdHiddenActivity.ClientID %>");
                        button.set_commandArgument(did + "~" + val);
                        button.click();
                    }
                }

                if (val == "5" || val == "1" || val == "3") {
                    radconfirm('Are you sure this activity is "' + txt + '"', onConfirmStatus);
                }                
                else {
                    top.openNewWindow('activity', 'Operation Activity - ' + txt, SitePath + '/PlannedMaintenance/PlannedMaintenanceActivity.aspx?actpi=' + did + '&status=' + val); return false;
                }
            }
        </script>
        <style type="text/css">
            .align{
                 vertical-align:middle !important;
             }
            .RadGrid .rgFilterRow>td {
                    padding-top: 0px !important;
                    padding-bottom: 0px !important;
                    padding-left: 0px !important;
                    padding-right: 3px !important;
                }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnablePageMethods="true" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvProgress">
                    <UpdatedControls>
                        <%--<telerik:AjaxUpdatedControl ControlID="gvPlanned" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                        <telerik:AjaxUpdatedControl ControlID="gvProgress" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <%--<telerik:AjaxUpdatedControl ControlID="gvCompleted" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                        <%--<telerik:AjaxUpdatedControl ControlID="MenuPlanned"></telerik:AjaxUpdatedControl>--%>
                        <telerik:AjaxUpdatedControl ControlID="MenuProgress"></telerik:AjaxUpdatedControl>
                        <%--<telerik:AjaxUpdatedControl ControlID="MenuCompleted"></telerik:AjaxUpdatedControl>--%>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuProgress">
                    <UpdatedControls>
                        <%--<telerik:AjaxUpdatedControl ControlID="gvPlanned" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                        <telerik:AjaxUpdatedControl ControlID="gvProgress" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <%--<telerik:AjaxUpdatedControl ControlID="gvCompleted" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                        <%--<telerik:AjaxUpdatedControl ControlID="MenuPlanned"></telerik:AjaxUpdatedControl>--%>
                        <telerik:AjaxUpdatedControl ControlID="MenuProgress"></telerik:AjaxUpdatedControl>
                        <%--<telerik:AjaxUpdatedControl ControlID="MenuCompleted"></telerik:AjaxUpdatedControl>--%>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
          </AjaxSettings>
        </telerik:RadAjaxManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
        <telerik:RadButton runat="server" ID="cmdHiddenActivity" CssClass="hidden" OnClick="cmdHiddenActivity_Click1"/>
        <eluc:TabStrip ID="MenuProgress" runat="server" OnTabStripCommand="MenuMaintenance_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvProgress" runat="server" OnItemDataBound="gvOperation_ItemDataBound" EnableLinqExpressions="false"
            OnNeedDataSource="gvOperation_NeedDataSource" OnItemCommand="gvOperation_ItemCommand" GroupingEnabled="false"
            ShowFooter="false" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true" AllowFilteringByColumn="false">
            <MasterTableView AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" AllowFilteringByColumn="true"
                ShowHeadersWhenNoRecords="true" ClientDataKeyNames="FLDDAILYPLANACTIVITYID" DataKeyNames="FLDDAILYPLANACTIVITYID">
                <Columns>
                    <telerik:GridTemplateColumn HeaderStyle-Width="25px" AllowFiltering="false">
                        <ItemTemplate>                                    
                            <img id="imgYellow" runat="server" height="16" width="16" src="" title="Carried over from previous day"/>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                   <telerik:GridTemplateColumn HeaderText="Process" AllowSorting="false" AllowFiltering="true" UniqueName="FLDELEMENTNAME"
                        FilterControlWidth="98%" FilterDelay="1000" AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                       <HeaderStyle Width="130px" HorizontalAlign="Left" />
                        <ItemStyle Width="130px" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDELEMENTNAME"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Activity" AllowSorting="false" AllowFiltering="true" UniqueName="FLDACTIVITYNAME"
                        FilterControlWidth="98%" FilterDelay="1000" AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <HeaderStyle Width="130px" HorizontalAlign="Left" />
                        <ItemStyle Width="130px" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDailyWorkPlanId" Visible="false" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDDAILYWORKPLANID"]%>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblActivity" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDACTIVITYNAME"]%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                   <telerik:GridTemplateColumn HeaderText="Date" UniqueName="FLDDATE">
                       <HeaderStyle Width="120px" HorizontalAlign="Left" />
                        <ItemStyle Width="120px" HorizontalAlign="Left" />
                        <FilterTemplate>
                                From&nbsp<telerik:RadDatePicker ID="FromOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="FromDateSelected"
                                    DbSelectedDate='<%# ViewState["FDATE"].ToString() %>' />                              
                                <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                    <script type="text/javascript">
                                        function FromDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var ToPicker = $find('<%# ((GridItem)Container).FindControl("ToOrderDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(sender);
                                            var toDate = FormatSelectedDate(ToPicker);

                                            tableView.filter("FLDDATE", fromDate + "~" + toDate, "Between");

                                        }
                                        function ToDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromOrderDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(FromPicker);
                                            var toDate = FormatSelectedDate(sender);

                                            tableView.filter("FLDDATE", fromDate + "~" + toDate, "Between");
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
                            <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"])%>'></telerik:RadLabel>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                   <telerik:GridTemplateColumn HeaderText="Est. Start Time / Start Time" UniqueName="FLDESTSTARTTIME">
                       <HeaderStyle Width="120px" HorizontalAlign="Left" />
                        <ItemStyle Width="120px" HorizontalAlign="Left" />
                        <FilterTemplate>                                
                                To&nbsp<telerik:RadDatePicker ID="ToOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="ToDateSelected"
                                    DbSelectedDate='<%# ViewState["TDATE"].ToString() %>' />                               
                            </FilterTemplate>
                        <ItemTemplate>
                            <%#PadZero(((DataRowView)Container.DataItem)["FLDESTSTARTTIME"].ToString())%> <b>/</b> <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDSTARTTIME"], DateDisplayOption.TimeHR24).Replace(":", "")%>
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>
                   <telerik:GridTemplateColumn HeaderText="Est. End Time / End Time" AllowFiltering="false">
                       <HeaderStyle Width="120px" HorizontalAlign="Left" />
                        <ItemStyle Width="120px" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%#PadZero(((DataRowView)Container.DataItem)["FLDDURATION"].ToString())%> <b>/</b> <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDCOMPLETEDTIME"], DateDisplayOption.TimeHR24).Replace(":", "")%>
                        </ItemTemplate>                                       
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="PIC" Visible="false" AllowSorting="false" AllowFiltering="false">
                        <HeaderStyle Width="0px" HorizontalAlign="Left" />
                        <ItemStyle Width="0px" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDPERSONINCHARGENAME"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Team Members" AllowSorting="false" AllowFiltering="true" UniqueName="FLDOTHERMEMBERSNAME"
                        FilterControlWidth="98%" FilterDelay="1000" AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDOTHERMEMBERSNAME"].ToString().Trim(',')%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="RA" AllowSorting="false" AllowFiltering="false">
                        <HeaderStyle Width="80px" HorizontalAlign="Left" />
                        <ItemStyle Width="80px" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkRA" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDRANUMBER"]%>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Forms & Checklists" UniqueName="FLDFORMLIST" AllowSorting="false" AllowFiltering="false">
                        <HeaderStyle Width="180px" HorizontalAlign="Left" />
                        <ItemStyle Width="180px" HorizontalAlign="Left" />
                        <ItemTemplate>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>                    
                    <telerik:GridTemplateColumn HeaderText="Status" UniqueName="FLDSTATUS">
                        <HeaderStyle Width="100px" HorizontalAlign="Left" />
                        <ItemStyle Width="100px" HorizontalAlign="Left" />
                        <FilterTemplate>                                
                                <telerik:RadComboBox ID="ddlStatus" runat="server" OnDataBinding="ddlStatus_DataBinding" AppendDataBoundItems="true"
                                SelectedValue ='<%# ViewState["STATUS"].ToString() %>' OnClientSelectedIndexChanged="StatusIndexChanged" Width="98%">                                                                    
                            </telerik:RadComboBox>    
                            <telerik:RadScriptBlock runat="server">
                                <script type="text/javascript">
                                    function StatusIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        tableView.filter("FLDSTATUS", args.get_item().get_value(), "EqualTo");
                                    }                            
                                </script>
                            </telerik:RadScriptBlock>
                            </FilterTemplate>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCompletionStatus" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDOPERATIONSTATUS"]%>' 
                                ToolTip='<%# ((DataRowView)Container.DataItem)["FLDOPERATIONSTATUS"] + " / " +((DataRowView)Container.DataItem)["FLDCOMPLETIONSTATUSNAME"]%>'></telerik:RadLabel>
                            
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>   
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" AllowSorting="false" AllowFiltering="false">
                        <HeaderStyle Width="180px" HorizontalAlign="Left" />
                        <ItemStyle Width="180px" HorizontalAlign="Left" />
                        <HeaderStyle />
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>
                            <telerik:RadRadioButtonList ID="rblInProgress" runat="server" ClientEvents-OnItemClicked="itemClicked" Width="135px" CssClass="align">
                                <Items>
                                    <telerik:ButtonListItem Text="Completed" Value="4" />
                                    <telerik:ButtonListItem Text="Cont. to next day" Value="5" />
                                </Items>
                            </telerik:RadRadioButtonList>
                            <telerik:RadRadioButtonList ID="rblPlanned" runat="server" ClientEvents-OnItemClicked="itemClicked" Width="135px" CssClass="align">
                                <Items>
                                    <%--<telerik:ButtonListItem Text="Done as Scheduled" Value="1" />--%>
                                    <telerik:ButtonListItem Text="Done" Value="2" />
                                    <telerik:ButtonListItem Text="Not Done" Value="3" />
                                </Items>
                            </telerik:RadRadioButtonList> 
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDITR" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Start" CommandName="START" ID="cmdStart" ToolTip="Start">
                                    <span class="icon"><i class="fas fa-hourglass-start"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Report" ID="cmdReschedule" CommandName="RESCHEDULE" ToolTip="Postpone">
                                 <span class="icon"><i class="far fa-calendar-alt"></i></span>
                            </asp:LinkButton>                            
                            <asp:LinkButton runat="server" AlternateText="Complete" CommandName="COMPLETE" ID="cmdComplete" ToolTip="Complete">
                                    <span class="icon"><i class="fas fa-check"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Timesheet" CommandName="TIMESHEET" ID="cmdTimeSheet" ToolTip="Time Sheet">
                                    <span class="icon"><i class="fas fa-hourglass-start"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
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
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <telerik:RadWindow runat="server" ID="RadWindow_NavigateUrl" Width="900px" Height="365px" InitialBehaviors="Maximize" Behaviors="Maximize,Close,Minimize,Resize" OnClientClose="onClose"
            Modal="true" OffsetElementID="main" VisibleStatusbar="false" KeepInScreenBounds="true" ReloadOnShow="true" ShowContentDuringLoad="false">
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=RadWindow_NavigateUrl.ClientID %>";
            </script>
            <script type="text/javascript">
                function onClose() {
                    document.getElementById("cmdHiddenSubmit").click();
                }
                
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
