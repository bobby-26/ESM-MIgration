<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardVessel.aspx.cs" Inherits="Dashboard_DashboardVessel" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
        <link href="../Content/dashboard.css" rel="stylesheet" />
        <script type="text/javascript">
            function pageLoad(sender, eventArgs) {
                if (!eventArgs.get_isPartialLoad()) {
                    <%-- setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Operation"); }, 500);
                     setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("OrdersInfomation"); }, 1000);
                     setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("AccidentsNearMisses"); }, 1500);
                     setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("Purchase"); }, 2000);
                     setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("WRH"); }, 2500);--%>
                 }
             }
            document.addEventListener("click", function (e) {
                if (parent.name != null && parent.name.indexOf("tab_") > -1) {
                    var tooltip = parent.$find("UserProfile");
                    tooltip.hide();
                }
            });
            function refreshShipMeanTime() {
                var ajxmgr = $find("RadAjaxManager1");
                if (ajxmgr != null)
                    ajxmgr.ajaxRequest("SMT");
            }
            function load() {
                setInterval(refreshShipMeanTime, 3600000);
            }
            function OnHeaderMenuShowing(sender, args) {

            }
        </script>
        <style type="text/css">
            .tuftbush {
                background-color: rgb(248,203,173) !important;
                 color: black !important;
            }
            .tuftbush a {
                color: black
            }
            .creambrulee {
                background-color: rgb(255,230,153) !important;
                color: black !important;
            }
            .creambrulee a {
                color: black
            }
            .gossip {
                background-color: rgb(169,208,142) !important;
                color: black !important;
            }
            .gossip a {
                color: black
            }
            .bittersweet {
                background-color: rgb(254,119,92) !important;
                color: black !important;
            }
            .bittersweet a {
                color: black
            }
            .sail {
                background-color: rgb(155,194,230) !important;
                color: black !important;
            }
            .sail a {
                color: black
            }
        </style>
    </telerik:RadCodeBlock>

</head>
<body onload="load()">
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <asp:Button ID="btnCrewList" runat="server" OnClick="BtnCrewList_Click" CssClass="hidden" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="btnOperationProgress" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="btnOperationPlanned" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="btnOperationCompleted" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="btnMasterOrders" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="btnCEOrders" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="btnCOOrders" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="ucuact" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divNearMiss" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="divAccidents" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="btnMaintenanceProgress" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="btnMaintenancePlanned" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="btnMaintenanceCompleted" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="btnReqProgress" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="btnReqPen" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="btnReqPenAck" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="gvTimeSheet" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="spnVoyageInfo" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnOperationProgress">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="btnOperationProgress" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnOperationPlanned">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="btnOperationPlanned" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnOperationPlanned">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="btnOperationPlanned" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnMasterOrders">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="btnMasterOrders" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnCEOrders">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="btnCEOrders" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="btnCOOrders">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="btnCOOrders" UpdatePanelCssClass="pull-right" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvTimeSheet">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvTimeSheet" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="RadPivotGrid1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="RadPivotGrid1" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>                
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <div class="gray-bg">
            <div class="row page-heading bg-success">

                <div class="col-lg-12">
                    <h2 class="font-bold"  id="spnVoyageInfo" runat="server">
                        <a href="#" style="color: white" onclick="javascript: top.openNewWindow('dsd','Vessel Particulars','Dashboard/DashboardVesselDetails.aspx')">
                        <asp:Literal ID="lblVessel" runat="server" Text=""></asp:Literal></a>
                        &nbsp; <span style="font-size: 14px; font-weight: 600">
                            <asp:Literal ID="ltrlVoyageInfo" runat="server" Text=""></asp:Literal></span>
                    </h2>
                    <h4 style="text-align:center">

                        <button class="btn btn-primary " type="button" id="btnDMS" runat="server"
                            onclick="javascript: top.openNewWindow('dsd','DMS Search','DocumentManagement/DocumentManagementDocument.aspx')">
                            <i class="fas fa-docking"></i>&nbsp;DMS Search</button>
                        <button class="btn btn-primary" id="btnDeckLog" runat="server" type="button"><i class="fas fa-administration"></i>&nbsp;Deck Logs</button>
                        <button class="btn btn-primary" id="btnEngineLog" runat="server" type="button" onclick="javascript: parent.openNewWindow('EngineLog', 'Cheif Engineer Log', 'Log/ElectricLogEngineLog.aspx', 'true', null, null, null, null, { 'disableMinMax': true })"><i class="fas fa-administration"></i>&nbsp;Engine Logs</button>
                        <%--<button class="btn btn-primary" id="Button3" runat="server" type="button" onclick="javascript: parent.openNewWindow('EngineLog', 'Cheif Engineer Log', 'Log/ElectricLogEngineLog.aspx', 'true', null, null, null, null, { 'disableMinMax': true })"><i class="fas fa-administration"></i>&nbsp;Engine Logs</button>--%>
                        <button class="btn btn-primary" id="btnMarpolLog" runat="server" type="button" onclick="javascript: top.openNewWindow('dsd','Marpol Record Books','Log/ElectricLogMarpolRecordBook.aspx')">
                            <i class="fas fa-administration"></i>&nbsp;MARPOL Record Books</button>                        
                        <%--<button class="btn btn-warning " type="button" onclick="javascript: top.openNewWindow('VoyagePlan','Voyage Plan','PlannedMaintenance/PlannedMaintenanceVoyagePlan.aspx')">
                        <i class="fa fa-anchor"></i>&nbsp;Voyage Plan
                    </button>--%>
                        <button class="btn btn-warning" id="lnkDailyWorkPlan" runat="server" type="button" onclick="javascript: top.openNewWindow('dsd','Daily Work Plan','PlannedMaintenance/PlannedMaintenanceDailyWorkPlanSchedule.aspx',true, null, null, null, null, {icon: '<i class=\'fa fa-calendar\'></i>', disableMinMax:true,helpWinURL:'Help/Panel Overview_Daily Work Plan.pdf'})">
                            <i class="fa fa-calendar"></i>&nbsp;Daily Work Plan
                        </button>

                        <button class="btn btn-info" type="button" id="btnNoonReport" runat="server" onserverclick="NoonReport_ServerClick"><i class="fas fa-vesselposition"></i>&nbsp;Noon Report</button>
                        <button class="btn btn-info" type="button" id="btnDepartureReport" runat="server" onserverclick="DepartureReport_ServerClick"><i class="fas fa-vesselposition"></i>&nbsp;Departure Report</button>
                        <button class="btn btn-info" type="button" id="btnArrivalReport" runat="server" onserverclick="ArrivalReport_ServerClick"><i class="fas fa-vesselposition"></i>&nbsp;Arrival Report</button>
                        <button class="btn btn-info" type="button" id="btnShifitingReport" runat="server" onserverclick="Shifting_ServerClick"><i class="fas fa-vesselposition"></i>&nbsp;Shifting Report</button>
                        <button class="btn btn-info" type="button" id="btnDrill" runat="server" onserverclick="Drill_ServerClick"><i class="fas fa-timeschedule"></i>&nbsp;Drills</button>
                        <button class="btn btn-info" type="button" id="btnDrillTraining" runat="server" onserverclick="DrillTraining_ServerClick"><i class="fas fa-timeschedule"></i>&nbsp;Training</button>

                    </h4>

                </div>

            </div>

            <div class="row">

                <div class="col-lg-4">


                    <div class="col-lg-12">

                        <br />

                        <div class="panel panel-success" style="height: 320px" runat="server" id="divOperations">
                            <div class="panel-heading tuftbush">
                                <a class="text-primary" style="text-decoration: underline" href="javascript: top.openNewWindow('maint','Maintenance','Dashboard/DashboardTechnicalMaintenance.aspx', true)">Maintenance</a>
                                / <a class="text-primary" style="text-decoration: underline" href="javascript: top.openNewWindow('maint','Operations','Dashboard/DashboardTechnicalOperation.aspx', true)">Operations</a>
                                | <a class="text-primary" style="font-size:10px;text-decoration: underline" href="javascript: top.openNewWindow('myjob', 'My Jobs', 'PlannedMaintenance/PlannedMaintenanceOperationsMaintenanceCalendarDay.aspx');">My Jobs</a>
                                <i runat="server" id="btnOperationEPSS" title="Help" visible="false" class="fas fa-chalkboard-teacher pull-right"></i>&nbsp;
                                <i runat="server" id="btnOperationInfo" title="Info" visible="false" class="fas fa-info-circle pull-right"></i>
                            </div>
                            <div class="panel-body">
                                <telerik:RadGrid ID="gvMaintOperation" runat="server" AutoGenerateColumns="false" OnNeedDataSource="gvMaintOperation_NeedDataSource"
                                AllowSorting="false" GroupingEnabled="false" BorderStyle="None" OnItemDataBound="gvMaintOperation_ItemDataBound"
                                EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000">
                                <MasterTableView TableLayout="Fixed">
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
                                        <telerik:GridTemplateColumn HeaderStyle-Width="35%">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkMaintenance" runat="server" Text="Maintenance" OnClientClick="javascript:top.openNewWindow('maint','Maintenance','Dashboard/DashboardTechnicalMaintenance.aspx',true); return false;"></asp:LinkButton>                                                
                                            </HeaderTemplate>
                                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkMaintenanceCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAINTENANCECOUNT") %>'></asp:LinkButton>                                                
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <asp:LinkButton ID="lnkOperation" runat="server" Text="Operations" OnClientClick="javascript:top.openNewWindow('maint','Operation','Dashboard/DashboardTechnicalOperation.aspx',true); return false;"></asp:LinkButton>                                                
                                            </HeaderTemplate>
                                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkOperationCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONCOUNT") %>'></asp:LinkButton>                                                
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>                                        
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                    <Resizing AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                            </div>
                        </div>

                    </div>                    

                    <div class="col-lg-12">

                        <div class="panel panel-success" style="height: 568px" runat="server" id="divTimeSheet">

                            <div class="panel-heading tuftbush">
                                <a class="text-primary" style="text-decoration: underline" href="javascript: top.openNewWindow('CompCategory','Time Sheet','PlannedMaintenance/PlannedMaintenanceTimeSheetList.aspx')">Time Sheet</a>
                                | <a class="text-primary" style="font-size:10px;text-decoration: underline" href="javascript: top.openNewWindow('wo', 'Add Event Time Sheet', 'PlannedMaintenance/PlannedMaintenanceTimeSheet.aspx?ev=aop');">Add Activity</a>
                                | <a class="text-primary" style="font-size:10px;text-decoration: underline" href="javascript: top.openNewWindow('wo', 'Add Event Time Sheet', 'PlannedMaintenance/PlannedMaintenanceTimeSheet.aspx?ev=amaint');">Add Maintenance</a>
                                | <a class="text-primary" style="font-size:10px;text-decoration: underline" href="javascript: top.openNewWindow('wo', 'Add Event Time Sheet', 'PlannedMaintenance/PlannedMaintenanceTimeSheet.aspx?ev=aothr');">Add Others</a>
                                <i runat="server" id="BtnTimeSheetEPSS" title="Help" visible="false" class="fas fa-chalkboard-teacher pull-right"></i>
                                <i runat="server" id="BtnTimeSheetInfo" title="Info" visible="false" class="fas fa-info-circle pull-right"></i>
                            </div>

                            <div class="panel-body">
                                <telerik:RadGrid ID="gvTimeSheet" runat="server" AutoGenerateColumns="false"
                                    AllowSorting="false" GroupingEnabled="false" OnItemDataBound="gvTimeSheet_ItemDataBound"
                                    EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000" OnNeedDataSource="gvTimeSheet_NeedDataSource">
                                    <MasterTableView TableLayout="Fixed">
                                        <Columns>
                                            <telerik:GridBoundColumn UniqueName="FLDVESSELSTATUSNAME" HeaderText="Vessel Status" DataField="FLDVESSELSTATUSNAME"></telerik:GridBoundColumn>
                                            <telerik:GridBoundColumn UniqueName="FLDOPERATION" HeaderText="Operation" DataField="FLDOPERATION"></telerik:GridBoundColumn>
                                            <telerik:GridTemplateColumn UniqueName="FLDDATETIME" HeaderText="Time" AllowSorting="false">                                            
                                                <ItemTemplate>
                                                    <%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATETIME"), DateDisplayOption.DateTimeHR24) %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>                                            
                                            <telerik:GridBoundColumn UniqueName="FLDDETAIL" HeaderText="Details" DataField="FLDDETAIL"></telerik:GridBoundColumn>
                                        </Columns>
                                    </MasterTableView>
                                    <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                        <Virtualization EnableVirtualization="true" InitiallyCachedItemsCount="2000"
                                            LoadingPanelID="RadAjaxLoadingPanel1" ItemsPerView="100" />
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="475px" />
                                        <Resizing AllowColumnResize="true" />
                                    </ClientSettings>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                </telerik:RadGrid>
                            </div>
                        </div>

                    </div>

                    <%--<div class="col-lg-12">

                        <div class="panel panel-success" style="height: 250px" runat="server" id="divAccidentalNearMissPanel">

                            <div class="panel-heading">
                                Accidents / Near Misses, Etc
                            <a href="#" class="btn btn-xs btn-primary pull-right" runat="server" id="BtnAccidentNearMissesEPSS" title="EPSS" target="_blank" visible="false"><i class="fas fa-chalkboard-teacher"></i></a>
                                <button type="button" class="btn btn-xs btn-primary pull-right" runat="server" id="BtnAccidentNearMissesInfo" title="Info" visible="false"><i class="fas fa-info-circle"></i></button>
                            </div>

                            <div class="panel-body">
                                
                                <div class="ibox-content text-center">

                                    <div class="col-lg-4" id="ucuact" runat="server">
                                        <h5>UC/UACTs</h5>
                                        <button type="button" class="btn btn-outline btn-danger" runat="server" id="btnUSNew">
                                            New<br />
                                            0</button>
                                        <button type="button" class="btn btn-outline btn-warning" runat="server" id="btnUSPending">
                                            Pend<br />
                                            0</button>
                                    </div>
                                    <div class="col-lg-4" id="divNearMiss" runat="server">
                                        <h5>Near Miss</h5>
                                        <button type="button" class="btn btn-outline btn-danger" runat="server" id="btnNMNew">
                                            New<br />
                                            0</button>
                                        <button type="button" class="btn btn-outline btn-warning" runat="server" id="btnNMPending">
                                            Pend<br />
                                            0</button>
                                    </div>
                                    <div class="col-lg-4" id="divAccidents" runat="server">
                                        <h5>Accidents</h5>
                                        <button type="button" class="btn btn-outline btn-danger" runat="server" id="btnINCNew">
                                            New<br />
                                            0</button>
                                        <button type="button" class="btn btn-outline btn-warning" runat="server" id="btnINCPending">
                                            Pend<br />
                                            0</button>
                                    </div>

                                </div>

                            </div>
                        </div>

                    </div>--%>


                </div>

                <div class="col-lg-4">
                    <br />

                    <div class="panel panel-success" style="height: 320px" runat="server" id="divFormsRAWorkPermit">
                        <div class="panel-heading creambrulee">
                            Forms / RAs / Work Permits / Accidents / Near Misses, Etc
                        <i runat="server" id="BtnFormsRAWRPEPSS" title="Help" visible="false" class="fas fa-chalkboard-teacher pull-right"></i>&nbsp;
                        <i runat="server" id="BtnFormsRAWRPInfo" title="Info" visible="false" class="fas fa-info-circle pull-right"></i>
                        </div>
                        <div class="panel-body">
                            <telerik:RadGrid ID="gvFormsRA" runat="server" AutoGenerateColumns="false" OnNeedDataSource="gvFormsRA_NeedDataSource"
                                AllowSorting="false" GroupingEnabled="false" BorderStyle="None" OnItemDataBound="gvFormsRA_ItemDataBound"
                                EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000">
                                <MasterTableView TableLayout="Fixed">                                    
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="35%">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Req" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkReqCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUIREDCOUNT") %>'></asp:LinkButton>                                                
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Draft" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkInUseCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINUSECOUNT") %>'></asp:LinkButton>                                                
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Pndg Approval" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblPendingCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGAPPROVALCOUNT") %>'></asp:LinkButton>                                                
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Approved" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblApprovedCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDCOUNT") %>'></asp:LinkButton>                                                
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Expir" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblExpiredCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIREDCOUNT") %>'></asp:LinkButton>                                                
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <NoRecordsTemplate>
                                        <table runat="server" width="100%" border="0">
                                            <tr>
                                                <td align="center">
                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                </MasterTableView>
                                <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                    <Resizing AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                            &nbsp;
                            <telerik:RadGrid ID="gvNearMiss" runat="server" AutoGenerateColumns="false" OnNeedDataSource="gvNearMiss_NeedDataSource"
                                AllowSorting="false" GroupingEnabled="false" BorderStyle="None" OnItemDataBound="gvNearMiss_ItemDataBound"
                                EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000">
                                <MasterTableView TableLayout="Fixed">                                   
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="35%">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="New" HeaderStyle-HorizontalAlign="Center">                                            
                                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkNew" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNEWCOUNT") %>'></asp:LinkButton>                                                
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Pending" HeaderStyle-HorizontalAlign="Center">                                          
                                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkPending" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGCOUNT") %>'></asp:LinkButton>                                                
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>                                        
                                    </Columns>
                                     <NoRecordsTemplate>
                                        <table runat="server" width="100%" border="0">
                                            <tr>
                                                <td align="center">
                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                </MasterTableView>
                                <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                    <Resizing AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                            <%--<p>
                                <h4>Non Routine RAs                                                                                                                       
                                <button type="button" class="btn btn-xs btn-danger pull-right" runat="server" id="btnRAExpired">Expir - 0</button>
                                    <button type="button" class="btn btn-xs btn-success pull-right" runat="server" id="btnRAApproved">Apprvd - 0</button>
                                    <button type="button" class="btn btn-xs btn-info pull-right" runat="server" id="btnRAAFU">Pndg Approval - 0</button>
                                    <button type="button" class="btn btn-xs btn-primary pull-right" runat="server" id="btnRADRT">In Use - 0</button>
                                    <button type="button" class="btn btn-xs btn-warning pull-right" runat="server" id="btnRADue"
                                        onclick="javascript: top.openNewWindow('DailyWorkPlan','Non Routine RAs','Dashboard/DashboardTechnicalRA.aspx')">
                                        Req - 0</button>
                                </h4>
                            </p>
                            <hr />
                            <p>
                                <h4>Work Permits                                                              
                                <button type="button" class="btn btn-xs btn-danger pull-right" runat="server" id="btnPTWExpires"
                                    onclick="javascript: top.openNewWindow('DailyWorkPlan','Work Permits','Dashboard/DashboardTechnicalPTWStatus.aspx?s=5')">
                                    Expir - 0</button>
                                    <button type="button" class="btn btn-xs btn-success pull-right" runat="server" id="Button2"
                                        onclick="javascript: top.openNewWindow('DailyWorkPlan','Work Permits','Dashboard/DashboardTechnicalPTWStatus.aspx?s=8')">
                                        Apprvd - 0</button>
                                    <button type="button" class="btn btn-xs btn-info pull-right" runat="server" id="Button1"
                                        onclick="javascript: top.openNewWindow('DailyWorkPlan','Work Permits','Dashboard/DashboardTechnicalPTWStatus.aspx?s=8')">
                                        Pndg Approval - 0</button>
                                    <button type="button" class="btn btn-xs btn-primary pull-right" runat="server" id="btnPTWInUse"
                                        onclick="javascript: top.openNewWindow('DailyWorkPlan','Work Permits','Dashboard/DashboardTechnicalPTWStatus.aspx?s=1,4,7')">
                                        In Use - 0</button>
                                    <button type="button" class="btn btn-xs btn-warning pull-right" runat="server" id="btnPTWDue"
                                        onclick="javascript: top.openNewWindow('DailyWorkPlan','Work Permits','Dashboard/DashboardTechnicalPTWDue.aspx')">
                                        Req - 0</button>
                                </h4>
                            </p>
                            <hr />
                            <p>
                                <h4>Checklists & Forms
                                <button type="button" class="btn btn-xs btn-info pull-right" runat="server" id="btnFormCheckListApproval"
                                    onclick="javascript: top.openNewWindow('DailyWorkPlan','Checklist & Forms','Dashboard/DashboardTechnicalDMFForms.aspx?s=APPROVAL')">
                                    Pndg Review - 0</button>
                                    <button type="button" class="btn btn-xs btn-primary pull-right" runat="server" id="btnFormCheckListInUse"
                                        onclick="javascript: top.openNewWindow('DailyWorkPlan','Checklist & Forms','Dashboard/DashboardTechnicalDMFForms.aspx?s=INUSE')">
                                        In Use - 0</button>
                                    <button type="button" class="btn btn-xs btn-warning pull-right" runat="server" id="btnFormCheckListDue"
                                        onclick="javascript: top.openNewWindow('DailyWorkPlan','Checklist & Forms','Dashboard/DashboardTechnicalDMFForms.aspx')">
                                        Req - 1</button>
                                </h4>
                            </p>--%>
                        </div>
                    </div>


                    <div class="panel panel-success" style="height: 300px" runat="server" id="divPersonnelOnDuty">

                        <div class="panel-heading gossip">
                            <a class="text-primary" style="text-decoration: underline" href="javascript: top.openNewWindow('CompCategory','Personnel on Duty','Dashboard/DashboardPersonnelOnDuty.aspx')">Personnel on Duty</a>
                            | <a class="text-primary" style="font-size:10px;text-decoration: underline" href="javascript: top.openNewWindow('wo', 'Add On Duty', 'Dashboard/DashboardRestHourDuty.aspx?d=on',false,400,300);">Add On Duty</a>
                            | <a class="text-primary" style="font-size:10px;text-decoration: underline" href="javascript: top.openNewWindow('wo', 'Crew List', 'VesselAccounts/VesselAccountsEmployeeQueryDashboard.aspx');">Crew List / Rest Hour</a>
                            <%--| <span class="label label-primary" style="cursor:pointer" onclick="javascript:top.openNewWindow('wo', 'Work & Rest Hours', 'VesselAccounts/VesselAccountsRHCrewList.aspx'); return false;">Work & Rest Hours</span>--%>
                            <i runat="server" id="BtnPersonnelOnDutyEPSS" title="Help" visible="false" class="fas fa-chalkboard-teacher pull-right"></i>
                            <i runat="server" id="BtnPersonnelOnDutyInfo" title="Info" visible="false" class="fas fa-info-circle pull-right"></i>
                        </div>

                        <div class="panel-body">

                            <telerik:RadPivotGrid ID="RadPivotGrid1" runat="server" TotalsSettings-ColumnGrandTotalsPosition="None" OnNeedDataSource="RadPivotGrid1_NeedDataSource"
                                AllowPaging="false" AllowFiltering="false" ShowFilterHeaderZone="false" TotalsSettings-RowGrandTotalsPosition="None" OnCellDataBound="RadPivotGrid1_CellDataBound"
                                ShowColumnHeaderZone="false" ShowDataHeaderZone="false" ShowRowHeaderZone="false" Height="232px">
                                <ClientSettings Scrolling-AllowVerticalScroll="true">
                                </ClientSettings>
                                <DataCellStyle Width="100px" />
                                <Fields>
                                    <telerik:PivotGridColumnField DataField="FLDDEPARTMENT">
                                    </telerik:PivotGridColumnField>
                                    <telerik:PivotGridRowField DataField="FLDMANAGEMENT">
                                    </telerik:PivotGridRowField>
                                </Fields>
                            </telerik:RadPivotGrid>
                        </div>
                    </div>

                    <div class="panel panel-success" style="height: 250px"  runat="server" id="divAlertsPanel">

                        <div class="panel-heading gossip">
                            <a class="text-primary" style="text-decoration: underline" href="javascript: top.openNewWindow('WRHCrewList','Crew List','VesselAccounts/VesselAccountsEmployeeQueryDashboard.aspx')">Work & Rest Hours</a>
                            
                            <i runat="server" id="BtnAlertsEPSS" title="Help" visible="false" class="fas fa-chalkboard-teacher pull-right"></i>
                            <i runat="server" id="BtnAlertsInfo" title="Info" visible="false" class="fas fa-info-circle pull-right"></i>
                        </div>

                        <div class="panel-body">

                            <telerik:RadGrid ID="gvWRH" runat="server" AutoGenerateColumns="false" OnNeedDataSource="gvWRH_NeedDataSource"
                                AllowSorting="false" GroupingEnabled="false" BorderStyle="None" OnItemDataBound="gvWRH_ItemDataBound"
                                EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000">
                                <MasterTableView TableLayout="Fixed">
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
                                        <telerik:GridTemplateColumn HeaderText="Stage Pending" HeaderStyle-Width="45%">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Seafarer" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkSeafarerCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERCOUNT") %>'></asp:LinkButton>
                                                <telerik:RadLabel ID="lnkSeafarerUrl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAFARERURL") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="HOD" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkHODCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHODCOUNT") %>'></asp:LinkButton>
                                                <telerik:RadLabel ID="lnkHODUrl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHODURL") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Master" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lblMasterCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMASTERCOUNT") %>'></asp:LinkButton>
                                                <telerik:RadLabel ID="lblMasterUrl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMASTERURL") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                    <Resizing AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>

                        </div>
                    </div>



                </div>

                <div class="col-lg-2">
                    <br />

                    <div class="panel panel-success" style="height: 320px" runat="server" id="divOrdersInformation">
                        <div class="panel-heading creambrulee">
                            Orders & Information
                            <i runat="server" id="BtnOrdersInformationEPSS" title="Help" visible="false" class="fas fa-chalkboard-teacher pull-right"></i>
                            <i runat="server" id="BtnOrdersInformationInfo" title="Info" visible="false" class="fas fa-info-circle pull-right"></i>
                        </div>
                        <div class="panel-body">
                            
                            <telerik:RadGrid ID="gvOrderInfo" runat="server" AutoGenerateColumns="false" OnNeedDataSource="gvOrderInfo_NeedDataSource"
                                    GridLines="None" BorderStyle="None" Font-Size="11px" OnItemDataBound="gvOrderInfo_ItemDataBound" 
                                    ShowHeader="false" ShowFooter="false">
                                    <MasterTableView>
                                        
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="35%">
                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                        <NoRecordsTemplate>
                                            <table runat="server" width="100%" border="0">
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                    </MasterTableView>                                   
                                </telerik:RadGrid>
                            <%-- <p>
                                <h4>Master's Orders
                                    <button type="button" class="btn btn-xs btn-primary pull-right" runat="server" id="btnMasterOrders">0</button>
                                </h4>
                            </p>
                            <hr />
                            <p>
                                <h4>C/E's Orders
                                    <button type="button" class="btn btn-xs btn-primary pull-right" runat="server" id="btnCEOrders">0</button>
                                </h4>
                            </p>
                            <hr />
                            <p>
                                <h4>C/O's Orders
                                    <button type="button" class="btn btn-xs btn-primary pull-right" runat="server" id="btnCOOrders">0</button>
                                </h4>
                            </p>
                            <hr />
                            <p>
                                <h4>HSEQA Info
                                        <button type="button" class="btn btn-xs btn-primary pull-right" runat="server" id="btnHSEQA" title="Vessel users unread list">0</button>
                                    <button type="button" class="btn btn-xs btn-danger pull-right" runat="server" id="btnHSEQAInfo" title="Unread Document list">0</button>
                                </h4>
                            </p>--%>
                            
                        </div>
                    </div>

                </div>

                <div class="col-lg-2">
                    <br />

                    <div class="panel panel-success" style="height: 320px" runat="server" id="divStoresSpares">
                        <div class="panel-heading sail">
                            Stores & Spares
                            <i runat="server" id="BtnStoresSparesEPSS" title="Help" visible="false" class="fas fa-chalkboard-teacher pull-right"></i>
                            <i runat="server" id="BtnStoresSparesInfo" title="Info" visible="false" class="fas fa-info-circle pull-right"></i>
                        </div>
                        <div class="panel-body">
                            <telerik:RadGrid ID="gvSpareStores" runat="server" AutoGenerateColumns="false" OnNeedDataSource="gvSpareStores_NeedDataSource"
                                 GridLines="None" BorderStyle="None" OnItemDataBound="gvSpareStores_ItemDataBound" ShowHeader="false" ShowFooter="false">
                                <MasterTableView>                                    
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="35%">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPENAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <NoRecordsTemplate>
                                        <table runat="server" width="100%" border="0">
                                            <tr>
                                                <td align="center">
                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                </MasterTableView>                                
                            </telerik:RadGrid>
                        </div>
                    </div>
                </div>

                <div class="col-lg-4">

                    <div class="panel panel-success" style="height: 300px" runat="server" id="divPlannedMaintenance">

                        <div class="panel-heading sail">
                            <a class="text-primary" style="text-decoration: underline" href="javascript: top.openNewWindow('CompCategory','Filters','Dashboard/DashboardTechnicalComponentCategoryWorkOrder.aspx')">Planned Maintenance</a>
                            | <a class="text-primary" style="font-size:10px;text-decoration: underline" href="javascript: top.openNewWindow('maint', 'Work Orders', 'PlannedMaintenance/PlannedMaintenanceWorkOrderGroupList.aspx');">Work Orders</a>
                            | <a class="text-primary" style="font-size:10px;text-decoration: underline" href="javascript: top.openNewWindow('dsd', 'Defects / Non-Routine Jobs', 'PlannedMaintenance/PlannedMaintenanceDefectListRegister.aspx');">Defects / Non-Routine Jobs</a>
                            | <a class="text-primary" style="font-size:10px;text-decoration: underline" href="javascript: top.openNewWindow('dsd', 'Components', 'Inventory/InventoryComponentTreeDashboard.aspx');">Components</a>
                            <i runat="server" id="BtnPlannedMaintenanceEPSS" title="Help" visible="false" class="fas fa-chalkboard-teacher pull-right"></i>
                            <i runat="server" id="BtnPlannedMaintenanceInfo" title="Info" visible="false" class="fas fa-info-circle pull-right"></i>
                        </div>

                        <div class="panel-body" style="height:250px;overflow-x:auto;width: 100%;padding: 0px 0 15px 0;">
                            <div class="col-lg-6">
                                <telerik:RadGrid ID="GvPMS" runat="server" ShowHeader="false" ShowFooter="false" AutoGenerateColumns="false"
                                    GridLines="None" BorderStyle="None" Font-Size="11px" OnItemCommand="GvPMS_ItemCommand"
                                    OnNeedDataSource="GvPMS_NeedDataSource" OnItemDataBound="GvPMS_ItemDataBound">
                                    <MasterTableView>
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
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblURL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' Visible="false"></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblMeasureCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURECODE") %>' Visible="false"></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblMeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>' Visible="false"></telerik:RadLabel>
                                                    <asp:LinkButton ID="lnkCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>' CommandName="MEASURE"></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>
                            <div class="col-lg-6">
                                <telerik:RadGrid ID="GvPms1" runat="server" ShowHeader="false" ShowFooter="false" AutoGenerateColumns="false"
                                    GridLines="None" BorderStyle="None" Font-Size="11px" OnItemCommand="GvPMS_ItemCommand"
                                    OnNeedDataSource="GvPMS_NeedDataSource" OnItemDataBound="GvPMS_ItemDataBound">
                                    <MasterTableView>
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
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle HorizontalAlign="Left" />
                                                <ItemTemplate>                                                    
                                                    <%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle HorizontalAlign="Right" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblURL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' Visible="false"></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblMeasureCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURECODE") %>' Visible="false"></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblMeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>' Visible="false"></telerik:RadLabel>
                                                    <asp:LinkButton ID="lnkCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>' CommandName="MEASURE"></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                    </MasterTableView>
                                </telerik:RadGrid>
                            </div>
                        </div>
                    </div>
                </div>

                <div class="col-lg-4">
                    <div class="panel panel-success" style="height: 250px" runat="server" id="divCrewDocPanel">

                        <div class="panel-heading bittersweet">
                            Alerts / Crew Documents
                            <i runat="server" id="BtnCrewDocumentsEPSS" title="Help" visible="false" class="fas fa-chalkboard-teacher pull-right"></i>
                            <i runat="server" id="BtnCrewDocumentsInfo" title="Info" visible="false" class="fas fa-info-circle pull-right"></i>
                        </div>

                        <div class="panel-body">

                            <telerik:RadGrid ID="gvAlerts" runat="server" AutoGenerateColumns="false" OnNeedDataSource="gvAlerts_NeedDataSource"
                                AllowSorting="false" GroupingEnabled="false" BorderStyle="None" OnItemDataBound="gvAlerts_ItemDataBound"
                                EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000">
                                <MasterTableView TableLayout="Fixed">
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
                                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="35%">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Overdue" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkOverdue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERDUECOUNT") %>'></asp:LinkButton>
                                                <telerik:RadLabel ID="lblOverdueurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERDUEURL") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="15 Days" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnk15Days" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD15COUNT") %>'></asp:LinkButton>
                                                <telerik:RadLabel ID="lbl15Daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD15URL") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="45 Days" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnk45Days" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD45COUNT") %>'></asp:LinkButton>
                                                <telerik:RadLabel ID="lbl45Daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD45URL") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                </MasterTableView>
                                <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                    <Resizing AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>

                        </div>
                    </div>
                </div>

            </div>
        </div>
    </form>
    <script type="text/javascript" src="../Scripts/jquery-3.4.1.min.js"></script>
    <script type="text/javascript" src="../Scripts/bootstrap.min.v3.js"></script>
    <script type="text/javascript" src="../Scripts/dashboard.js"></script>
</body>
</html>
