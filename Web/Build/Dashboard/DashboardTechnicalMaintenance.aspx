<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalMaintenance.aspx.cs" Inherits="Dashboard_DashboardTechnicalMaintenance" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>  
        <script type="text/javascript">  
            <%--var DWPgrid = null;
            function resize() {
                var $ = $telerik.$;
                var height = $(window).height();
                if (DWPgrid != null && DWPgrid.GridDataDiv != null) {
                    var gridPagerHeight = (DWPgrid.PagerControl) ? DWPgrid.PagerControl.offsetHeight : 0;
                    DWPgrid.GridDataDiv.style.height = (height - gridPagerHeight - 100) + "px";
                } else {
                    var gvPlanned = $find("<%= gvMaintenancePlanned.ClientID %>");
                    var gvProgress = $find("<%= gvMaintenanceProgress.ClientID %>");
                    var gvCompleted = $find("<%= gvMaintenanceCompleted.ClientID %>");
                    var gvPlannedPagerHeight = (gvPlanned.PagerControl) ? gvPlanned.PagerControl.offsetHeight : 0;
                    var gvProgressPagerHeight = (gvProgress.PagerControl) ? gvProgress.PagerControl.offsetHeight : 0;
                    var gvCompletedPagerHeight = (gvCompleted.PagerControl) ? gvCompleted.PagerControl.offsetHeight : 0;

                    gvPlanned.GridDataDiv.style.height = (Math.round(height / 3) - gvPlannedPagerHeight - 65) + "px";
                    gvProgress.GridDataDiv.style.height = (Math.round(height / 3) - gvProgressPagerHeight - 65) + "px";
                    gvCompleted.GridDataDiv.style.height = (Math.round(height / 3) - gvCompletedPagerHeight - 65) + "px";
                }
            }--%>           
            function resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvMaintenanceProgress.ClientID %>"));
                }, 200);
            }            
            function pageLoad() {
                resize();
            }
            window.onresize = window.onload = resize;
            function CloseUrlModelWindow(gridid) {
                var wnd = $find('<%=modelPopup.ClientID %>');
                wnd.close();
                if (gridid != null) {
                    var masterTable = $find(gridid).get_masterTableView();
                    masterTable.rebind();
                }
                else {
                    var masterTable = $find('<%=gvMaintenanceProgress.ClientID%>').get_masterTableView();
                    masterTable.rebind();
                }
            }   
            function refresh() {
                var ajxmgr = parent.frames[1].$find("RadAjaxManager1");
                if (ajxmgr != null)
                    ajxmgr.ajaxRequest("OPERATION");
            }
            <%--function expandcollapse(gridid) {
                top.maintenanceexpandcollapse = gridid;
                var $ = $telerik.$;
                var height = $(window).height();
                var atab = document.querySelector('#MenuPlanned_dlstTabs');
                var btab = document.querySelector('#MenuProgress_dlstTabs');
                var ctab = document.querySelector('#MenuCompleted_dlstTabs');

                var gvPlanned = $find("<%= gvMaintenancePlanned.ClientID %>");
                var gvProgress = $find("<%= gvMaintenanceProgress.ClientID %>");
                var gvCompleted = $find("<%= gvMaintenanceCompleted.ClientID %>");
                DWPgrid = $find(gridid);
                var collapse = false;
                if (DWPgrid != gvPlanned) {
                    var visible = gvPlanned.get_visible();
                    gvPlanned.set_visible(!visible);
                    atab.style.display = visible ? 'none' : '';
                    collapse = visible;
                }
                if (DWPgrid != gvProgress) {
                    var visible = gvProgress.get_visible();
                    gvProgress.set_visible(!visible);
                    btab.style.display = visible ? 'none' : '';
                    collapse = visible;
                }
                if (DWPgrid != gvCompleted) {
                    var visible = gvCompleted.get_visible();
                    gvCompleted.set_visible(!visible);
                    ctab.style.display = visible ? 'none' : '';
                    collapse = visible;
                }
                if (!collapse) {
                    DWPgrid = null;
                    top.maintenanceexpandcollapse = null;
                }
                resize();
            }
            function pageLoad() {
                if (top.maintenanceexpandcollapse != null) {
                    expandcollapse(top.maintenanceexpandcollapse);
                }
            }--%>
            function itemClicked(list, args) {                
                var $ = $telerik.$;                
                var gridRowElem = $(list._element).parents("tr").first()[0];
                var grid = $find("gvMaintenanceProgress");
                var masterTable = grid.get_masterTableView();
                var item = masterTable.get_dataItems()[gridRowElem.rowIndex - 1];                 
                var isUnPlanned = gridRowElem.querySelector("span[id*='lblIsUnPlannedJob']").innerText;                
                var woname = $telerik.findElement(masterTable.get_element(), "lblWO").innerHTML;   
                var cancel = $telerik.findElement(masterTable.get_element(), "cmdCancel"); 
                var did = item.getDataKeyValue("FLDWODETAILID");
                var woid = item.getDataKeyValue("FLDWOGROUPID");
                var val = args.get_item().get_value();
                var txt = args.get_item().get_text();                
                var callBackFn = function (shouldSubmit, e) {
                    Telerik.Web.UI.RadWindowUtils.Localization =
                    {
                        "OK": "OK",
                        "Cancel": "Cancel"
                    };
                    var button = $find("<%= cmdHiddenActivity.ClientID %>");
                    if (shouldSubmit) {
                        if (val != "3") {
                            button.set_commandArgument(val + "~" + did + '~3');
                            button.click();
                        }                        
                    }
                    else {
                        if (isUnPlanned == "1" && val == "3") {
                            $(cancel).find("i").trigger("click");                 
                        }
                        if (e != null && e.which)
                            e.stopPropagation();
                        else
                            window.event.cancelBubble = true;
                        return false;
                    }
                }
                if (val == "5" || val == "1") {
                    radconfirm('Are you sure this WO is "' + txt + '"', callBackFn);
                }                
                else if (val == "3") {
                    $modalWindow.modalWindowUrl = '../PlannedMaintenance/PlannedMaintenanceWorkOrderGroupReschedule.aspx?groupId=' + woid + '&woplanid=' + did + '&unplanned=' + isUnPlanned;
                    showDialog('Postpone/Cancel WO - ' + woname);
                }
                else {
                    top.openNewWindow('activity', 'Maintenance Activity - ' + txt, SitePath + '/PlannedMaintenance/PlannedMaintenanceActivity.aspx?wopi=' + did + '&status=' + val); return false;
                }
            }                      
        </script>      
        <style type="text/css">
             span.bold-red {
                color: red;
                font-weight: bold;
            }
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
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">            
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvMaintenanceProgress">
                    <UpdatedControls>
                        <%--<telerik:AjaxUpdatedControl ControlID="gvMaintenancePlanned" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                        <telerik:AjaxUpdatedControl ControlID="gvMaintenanceProgress" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <%--<telerik:AjaxUpdatedControl ControlID="gvMaintenanceCompleted" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>--%>
                         <%--<telerik:AjaxUpdatedControl ControlID="MenuPlanned"></telerik:AjaxUpdatedControl>--%>
                        <telerik:AjaxUpdatedControl ControlID="MenuProgress"></telerik:AjaxUpdatedControl>
                        <%--<telerik:AjaxUpdatedControl ControlID="MenuCompleted"></telerik:AjaxUpdatedControl>--%>
                         <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>                        
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <%--<telerik:AjaxSetting AjaxControlID="gvMaintenancePlanned">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvMaintenancePlanned" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvMaintenanceProgress" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvMaintenanceCompleted" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="MenuPlanned"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuProgress"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuCompleted"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvMaintenanceCompleted">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvMaintenancePlanned" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvMaintenanceProgress" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="gvMaintenanceCompleted" LoadingPanelID="RadAjaxLoadingPanel1"></telerik:AjaxUpdatedControl>
                         <telerik:AjaxUpdatedControl ControlID="MenuPlanned"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuProgress"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="MenuCompleted"></telerik:AjaxUpdatedControl>
                        <telerik:AjaxUpdatedControl ControlID="ucError"></telerik:AjaxUpdatedControl>
                    </UpdatedControls>
                </telerik:AjaxSetting>     --%>                          
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
        <telerik:RadButton runat="server" ID="cmdHiddenActivity" CssClass="hidden" OnClick="cmdHiddenActivity_Click1"/>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuProgress" runat="server" OnTabStripCommand="MenuMaintenance_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvMaintenanceProgress" runat="server" OnItemDataBound="gvMaintenance_ItemDataBound" EnableLinqExpressions="false"
            OnNeedDataSource="gvMaintenance_NeedDataSource" OnItemCommand="gvMaintenance_ItemCommand" AllowFilteringByColumn="true"
            ShowFooter="false" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
            <MasterTableView AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" ClientDataKeyNames="FLDWODETAILID,FLDWOGROUPID"
                DataKeyNames="FLDWODETAILID,FLDWOGROUPID">
                <Columns>
                    <telerik:GridTemplateColumn HeaderStyle-Width="25px" AllowFiltering="false">
                        <ItemTemplate>                                    
                            <img id="imgYellow" runat="server" height="16" width="16" src="" title="Carried over from previous day"/>
                            <telerik:RadLabel ID="lblIsUnPlannedJob" CssClass="hidden" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDISUNPLANNEDJOB"]%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Work Order No."  AllowSorting="false" AllowFiltering="true" UniqueName="FLDWORKORDERNUMBER"
                        FilterControlWidth="98%" FilterDelay="1000" AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <HeaderStyle Width="90px" Wrap="false" HorizontalAlign="Left" />
                        <ItemStyle Width="90px" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDailyWorkPlanId" Visible="false" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDDAILYWORKPLANID"]%>'></telerik:RadLabel>  
                            <asp:LinkButton ID="lblWO" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"]%>'></asp:LinkButton>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Dept" AllowSorting="false" AllowFiltering="true" UniqueName="FLDDEPARTMENTID"
                        FilterControlWidth="98%" FilterDelay="1000" AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <HeaderStyle Width="90px" Wrap="false" HorizontalAlign="Left" />
                        <ItemStyle Width="90px" HorizontalAlign="Left" />
                        <FilterTemplate>                                
                            <telerik:RadComboBox ID="ddlDepartment" runat="server" OnDataBinding="ddlDepartment_DataBinding" AppendDataBoundItems="true"
                                SelectedValue ='<%# ViewState["DEPT"].ToString() %>' OnClientSelectedIndexChanged="DeptIndexChanged" Width="98%">                                                                    
                            </telerik:RadComboBox>    
                            <telerik:RadScriptBlock runat="server">
                                <script type="text/javascript">
                                    function DeptIndexChanged(sender, args) {
                                        var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                        tableView.filter("FLDDEPARTMENTID", args.get_item().get_value(), "EqualTo");
                                    }
                                </script>
                            </telerik:RadScriptBlock>
                        </FilterTemplate>
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDDEPARTMENT"]%>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Work Order" AllowSorting="false" AllowFiltering="true" UniqueName="FLDWORKORDERNAME"
                        FilterControlWidth="98%" FilterDelay="1000" AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <HeaderStyle Width="250px" Wrap="false" HorizontalAlign="Left" />
                        <ItemStyle Width="250px" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%> <br /> <b>due on <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDPLANNINGDUEDATE"])%></b>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date" UniqueName="FLDDATE">
                        <HeaderStyle Width="120px" HorizontalAlign="Left" />
                        <ItemStyle Width="120px" HorizontalAlign="Left" />
                        <FilterTemplate>
                                From&nbsp&nbsp<telerik:RadDatePicker ID="FromOrderDatePicker" runat="server" Width="70%" ClientEvents-OnDateSelected="FromDateSelected"
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
                                To&nbsp&nbsp<telerik:RadDatePicker ID="ToOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="ToDateSelected"
                                    DbSelectedDate='<%# ViewState["TDATE"].ToString() %>' />                               
                            </FilterTemplate>
                        <ItemTemplate>
                            <%#PadZero(((DataRowView)Container.DataItem)["FLDESTSTARTTIME"].ToString())%> <b>/</b> <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDSTARTTIME"], DateDisplayOption.TimeHR24).Replace(":", "")%>
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>
                    <%-- <telerik:GridTemplateColumn HeaderText="Start Time"  HeaderStyle-Width="110px" AllowFiltering="false">
                        <ItemTemplate>
                            
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>--%>
                    <telerik:GridTemplateColumn HeaderText="Est. End Time / End Time" AllowFiltering="false">
                        <HeaderStyle Width="120px" HorizontalAlign="Left" />
                        <ItemStyle Width="120px" HorizontalAlign="Left" />
                        <ItemTemplate>
                            <%#PadZero(((DataRowView)Container.DataItem)["FLDDURATION"].ToString())%> <b>/</b> <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDCOMPLETEDTIME"], DateDisplayOption.TimeHR24).Replace(":", "")%>
                        </ItemTemplate>                                       
                    </telerik:GridTemplateColumn>
                     <%--<telerik:GridTemplateColumn HeaderText="End Time"  HeaderStyle-Width="110px" AllowFiltering="false">
                        <ItemTemplate>
                             
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>--%>
                    <telerik:GridTemplateColumn HeaderText="PIC" Visible="false" AllowFiltering="false">
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
                    <telerik:GridTemplateColumn HeaderText="Status" UniqueName="FLDSTATUS">
                        <HeaderStyle Width="120px" HorizontalAlign="Left" />
                        <ItemStyle Width="120px" HorizontalAlign="Left" />
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
                            <telerik:RadLabel ID="lblCompletionStatus" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDMAINTENANCESTATUS"]%>' 
                                ToolTip='<%# ((DataRowView)Container.DataItem)["FLDMAINTENANCESTATUS"] + " / " + ((DataRowView)Container.DataItem)["FLDCOMPLETIONSTATUSNAME"]%>'></telerik:RadLabel>                            
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>   
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" AllowFiltering="false">
                        <HeaderStyle Width="250px" HorizontalAlign="Left" />
                        <ItemStyle Width="250px" HorizontalAlign="Left" />
                        <HeaderStyle />
                        <ItemStyle Wrap="false" VerticalAlign="Bottom" />
                        <ItemTemplate>
                            <telerik:RadRadioButtonList ID="rblInProgress" runat="server" ClientEvents-OnItemClicked="itemClicked" Width="140px" CssClass="align">
                                <Items>
                                    <telerik:ButtonListItem Text="Work Completed" Value="4" />
                                    <telerik:ButtonListItem Text="Cont. to next day" Value="5" />
                                </Items>
                            </telerik:RadRadioButtonList>
                            <telerik:RadRadioButtonList ID="rblPlanned" runat="server" ClientEvents-OnItemClicked="itemClicked" Width="140px" CssClass="align">
                                <Items>
                                    <%--<telerik:ButtonListItem Text="Done as Scheduled" Value="1" />--%>
                                    <telerik:ButtonListItem Text="Done" Value="2" />
                                    <telerik:ButtonListItem Text="Not Done" Value="3" />
                                </Items>
                            </telerik:RadRadioButtonList>&nbsp&nbsp&nbsp&nbsp
                             <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDITR" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Start" CommandName="START" ID="cmdStart" ToolTip="Start">
                                    <span class="icon"><i class="fas fa-hourglass-start"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Report" ID="cmdReschedule" CommandName="RESCHEDULE" ToolTip="Reschedule">
                                 <span class="icon"><i class="far fa-calendar-alt"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                             <asp:LinkButton runat="server" AlternateText="Toolbox" ID="cmdToolBox" CommandName="TOOLBOX" ToolTip="Tool box meet">
                                <span class="icon"><i class="fas fa-toolbox"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Complete" CommandName="COMPLETE" ID="cmdComplete" ToolTip="Complete">
                                    <span class="icon"><i class="fas fa-check"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Report" ID="cmdReport" CommandName="REPORT" ToolTip="Report Work Order">
                                 <span class="icon"><i class="fas fa-clipboard-list"></i></span>
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
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px"/>
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <%--<br />
        <eluc:TabStrip ID="MenuPlanned" runat="server" OnTabStripCommand="MenuMaintenance_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvMaintenancePlanned" runat="server" OnItemDataBound="gvMaintenance_ItemDataBound"
            OnNeedDataSource="gvMaintenance_NeedDataSource" OnItemCommand="gvMaintenance_ItemCommand"
            ShowFooter="false" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
            <MasterTableView AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDWODETAILID,FLDWOGROUPID">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Work Order No.">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDailyWorkPlanId" Visible="false" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDDAILYWORKPLANID"]%>'></telerik:RadLabel>  
                            <asp:LinkButton ID="lblWO" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"]%>'></asp:LinkButton>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Work Order">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date">
                        <ItemTemplate>
                             <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"])%>'></telerik:RadLabel>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Est Start Time">
                        <ItemTemplate>
                            <%#PadZero(((DataRowView)Container.DataItem)["FLDESTSTARTTIME"].ToString())%>
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Est. End Time">
                        <ItemTemplate>
                            <%#PadZero(((DataRowView)Container.DataItem)["FLDDURATION"].ToString())%>
                        </ItemTemplate>                                       
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="PIC" Visible="false">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDPERSONINCHARGENAME"]%>
                        </ItemTemplate>                         
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Team Members">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDOTHERMEMBERSNAME"].ToString().Trim(',')%>
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>   
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                        <HeaderStyle />
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDITR" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Start" CommandName="START" ID="cmdStart" ToolTip="Start">
                                    <span class="icon"><i class="fas fa-hourglass-start"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Report" ID="cmdReschedule" CommandName="RESCHEDULE" ToolTip="Postpone job">
                                 <span class="icon"><i class="far fa-calendar-alt"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                             <asp:LinkButton runat="server" AlternateText="Toolbox" ID="cmdToolBox" CommandName="TOOLBOX" ToolTip="Tool box meet">
                                <span class="icon"><i class="fas fa-toolbox"></i></span>
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
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px"/>
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <br />
         <eluc:TabStrip ID="MenuCompleted" runat="server" OnTabStripCommand="MenuMaintenance_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvMaintenanceCompleted" runat="server" OnItemDataBound="gvMaintenance_ItemDataBound"
            OnNeedDataSource="gvMaintenance_NeedDataSource"
            ShowFooter="false" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
            <MasterTableView AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDWODETAILID">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Work Order No.">
                        <ItemTemplate>
                            <asp:LinkButton ID="lblWO" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"]%>'></asp:LinkButton>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Work Order">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date">
                        <ItemTemplate>                             
                            <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATE"])%>'></telerik:RadLabel>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                   <telerik:GridTemplateColumn HeaderText="Start Time">
                        <ItemTemplate>
                            <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDSTARTTIME"], DateDisplayOption.DateTime)%>
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Completed Time">
                        <ItemTemplate>
                             <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDCOMPLETEDTIME"], DateDisplayOption.DateTime)%>
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="PIC" Visible="false">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDPERSONINCHARGENAME"]%>
                        </ItemTemplate>                         
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Team Members">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDOTHERMEMBERSNAME"].ToString().Trim(',')%>
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>   
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                        <HeaderStyle />
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Report" ID="cmdReport" CommandName="REPORT" ToolTip="Report Work Order">
                                 <span class="icon"><i class="fas fa-clipboard-list"></i></span>
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
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px"/>
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>--%>
         <telerik:RadWindow runat="server" ID="modelPopup" Width="900px" Height="365px" InitialBehaviors="Maximize"
            Modal="true" OffsetElementID="main" VisibleStatusbar="false" KeepInScreenBounds="true" ReloadOnShow="true" ShowContentDuringLoad="false"
             Behaviors="Maximize,Close,Minimize,Move,Resize" OnClientClose="onClose">
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modelPopup.ClientID %>";
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
