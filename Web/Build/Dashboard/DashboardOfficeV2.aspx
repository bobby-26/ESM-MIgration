<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardOfficeV2.aspx.cs" Inherits="Dashboard_DashboardOfficeV2" MaintainScrollPositionOnPostback="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
        <link href="../Content/dashboard.css" rel="stylesheet" />
        <script type="text/javascript">
            function resize() {
                var $ = $telerik.$;
                var height = $(window).height();
                var grid = $find("gvVessel");
                if (grid != null && grid.GridDataDiv != null) {
                    var gridPagerHeight = (grid.PagerControl) ? grid.PagerControl.offsetHeight : 0;
                    grid.GridDataDiv.style.height = (height - gridPagerHeight) + "px";
                    const cols = document.querySelector("#panel1").children;
                    var gridheight = 0;
                    var panelheight = 0;
                    [].forEach.call(cols, (e) => {
                        panelheight = e.parentElement.parentElement.parentElement.offsetHeight - 80;
                        if (e.id.toLowerCase() != "gvvesselpanel")
                            gridheight += e.offsetHeight;
                    });
                    grid.GridDataDiv.style.height = (panelheight - gridheight) + "px";
                }
                
                var frameheight = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 115 + "px";
                var obj = document.getElementById("ifMoreInfoQuality");
                obj.style.height = frameheight;
                var dobj = document.getElementById("ifMoreInfoDahboard2");
                dobj.style.height = frameheight;
                var pobj = document.getElementById("divDashboard1Content");
                pobj.style.height = frameheight;                
            }          
            window.onload = window.onresize = resize;
            function pageLoad() {
                resize();                
            }
            function Onclicktab(id) {
                //Get the Button reference and trigger the click event    
                if (id == 1) {
                    //setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("DASHBOARD2"); }, 100);
                    document.getElementById("ifMoreInfoDahboard2").src = "../Dashboard/DashboardOfficeV2Extn.aspx?type=<%= ViewState["type"]%>";
                }
                if (id == 2) {
                    setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("INCIDENTS"); }, 100);
                }
                if (id == 3) {
                    setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("INTERNALAUDIT"); }, 100);
                }
                if (id == 4) {
                    setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("EXTERNALAUDIT"); }, 100);
                }
                if (id == 5) {
                    setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("TASK"); }, 100);
                }
                if (id == 6) {
                    setTimeout(function () { $find("<%= RadAjaxManager1.ClientID %>").ajaxRequest("RISKASSESSMENT"); }, 100);
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .panelHeight {
            height: 440px;
        }

        .panelfont {
            overflow: auto;
            font-size: 11px;
        }

        .icon {
            padding-right: 2px;
            font-size: 16px;
        }

        .radMapWrapper {
            padding: 0 21px 21px 21px;
            background-color: #EBEDEE;
            display: inline-block;
            *display: inline;
            zoom: 1;
        }

        .leftCol {
            float: left;
        }

        .rightCol {
            padding-left: 55px;
            font-size: 14px;
            text-align: left;
            line-height: 19px;
        }

        .leftCol .vessel {
            font-weight: bold;
        }

        .leftCol .location {
            border-top: 1px solid #c9c9c9;
            margin-top: 10px;
            padding-top: 10px;
        }

        .RadGrid .rgHeader, .RadGrid th.rgResizeCol, .RadGrid .rgRow td, .RadGrid .rgAltRow td {
            padding-left: 2px !important;
            padding-right: 2px !important;
        }

        .fa-certificates::before {
            background-image: url(../css/Theme1/images/Certificates.png) !important;
            background-size: 16px 16px;
            display: inline-block;
            width: 16px;
            height: 16px;
            content: "" !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">

            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvVessel" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                 <telerik:AjaxSetting AjaxControlID="ifMoreInfoQuality">
                    <UpdatedControls>                     
                         <telerik:AjaxUpdatedControl ControlID="ifMoreInfoQuality" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>               
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <div class="gray-bg">
            <div class="row page-heading bg-success">
                <div class="col-lg-3">
                    <h2 class="font-bold">
                        <a runat="server" id="lnkDashboard" href="#" style="color: white" onclick="javascript: top.openNewWindow('filter','Filter','Dashboard/DashboardOfficeV2Filter.aspx?d=1', false, 600, 300,null,null,{icon: '<i class=\'fas fa-filter\'></i>'})">Dashboard<i class="fas fa-filter"></i>
                        </a>
                    </h2>
                </div>
                <div class="col-lg-9">
                    <br />
                    <a id="lnkVetting" runat="server" class="btn btn-warning" href="javascript: top.openNewWindow('dpopup','Vetting','Dashboard/DashboardOfficeV2TechnicalPMS.aspx?mod=vetting')">Vetting</a>
                    <a id="lnkPhoenixAnalytics" class="btn btn-warning" target="_blank" runat="server">Phoenix Analytics</a>
                    <a id="lnkWRHAnalytics" class="btn btn-warning" target="_blank" runat="server">WRH Analytics</a>
                    <a id="lnkAnalytics" class="btn btn-warning" runat="server" href="javascript: top.openNewWindow('dpopup','Analytics','Dashboard/QualityPBI.html')">Analytics</a>

                    <asp:LinkButton ID="btnCrew" runat="server" OnClick="btnCrew_Click" Text="Crew" CssClass="btn btn-primary"></asp:LinkButton>
                    <asp:LinkButton ID="BtnAccounts" runat="server" OnClick="BtnAccounts_Click" Text="Accounts" CssClass="btn btn-primary"></asp:LinkButton>
                    <asp:LinkButton ID="BtnTech" runat="server" OnClick="BtnTech_Click" Text="Tech" CssClass="btn btn-primary"></asp:LinkButton>
                    <asp:LinkButton ID="BtnHSQEA" runat="server" OnClick="BtnHSQEA_Click" Text="HSEQA" CssClass="btn btn-primary"></asp:LinkButton>
                </div>
            </div>
            <div class="row">

                <div class="col-lg-12">
                    <div class="tabs-container">
                        <ul class="nav nav-tabs">
                            <li class="active"><a data-toggle="tab" href="#tab-1">Dashboard 1</a></li>
                            <li class="" id="liPage2" runat="server"><a data-toggle="tab" onclick="return Onclicktab(1);" href="#tab-2">Dashboard 2</a></li>
                            <li class="" id="idIncidents" runat="server">
                                <a data-toggle="tab" onclick="return Onclicktab(2);" href="#tab-3">Incidents</a>
                            </li>
                            <li class="" id="idInternalAudit" runat="server">
                                <a data-toggle="tab" onclick="return Onclicktab(3);" href="#tab-3">Internal Audit</a>
                            </li>
                             <li class="" id="idExternalAudit" runat="server">
                                <a data-toggle="tab" onclick="return Onclicktab(4);" href="#tab-3">External Audit</a>
                            </li>
                             <li class="" id="idTask" runat="server">
                                <a data-toggle="tab" onclick="return Onclicktab(5);" href="#tab-3">Task</a>
                            </li>
                             <li class="" id="idRiskAssessment" runat="server">
                                <a data-toggle="tab" onclick="return Onclicktab(6);" href="#tab-3">Risk Assessment</a>
                            </li>
                        </ul>
                        <div class="tab-content">
                            <div id="tab-1" class="tab-pane active">
                                <div class="panel-body" id="divDashboard1Content" style="overflow:auto">

                                    <div class="col-lg-4">

                                        <div class="panel panel-success">
                                            <div class="panel-heading">
                                                <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: var name= document.getElementById('txtvesselsearch').value;name = (name == 'Vessel' ? '' : name);top.openNewWindow('dpopup','World map - Fleet List','Dashboard/DashboardV2Map.aspx?name='+name)">World map - Fleet List</a>
                                                <a id="lnkOtis" class="text-primary pull-right" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('otis', 'OTIS', 'https://system.stratumfive.com/otis/index.html', true);">OTIS</a>
                                            </div>
                                            <div class="panel-body" style="height: 1120px" runat="server" id="devvessel">
                                                <telerik:RadAjaxPanel ID="panel1" runat="server">
                                                    <div class="ibox float-e-margins">

                                                        <div id="world-map" style="height: 300px;">
                                                            <telerik:RadMap runat="server" ID="RadMap1" Zoom="2" OnItemDataBound="RadMap1_ItemDataBound" Height="300px">
                                                                <CenterSettings Latitude="23" Longitude="10" />
                                                                <DataBindings>
                                                                    <MarkerBinding DataTitleField="FLDVESSELNAME" DataLocationLatitudeField="FLDDECIMALLAT" DataLocationLongitudeField="FLDDECIMALLONG" />
                                                                </DataBindings>
                                                                <LayersCollection>
                                                                    <telerik:MapLayer Type="Tile" Subdomains="a,b,c"
                                                                        UrlTemplate="https://#= subdomain #.tile.openstreetmap.org/#= zoom #/#= x #/#= y #.png"
                                                                        Attribution="&copy; <a href='http://osm.org/copyright' title='OpenStreetMap contributors' target='_blank'>OpenStreetMap contributors</a>.">
                                                                    </telerik:MapLayer>
                                                                </LayersCollection>
                                                            </telerik:RadMap>
                                                        </div>
                                                    </div>

                                                    <div class="ibox-content">
                                                        <telerik:RadTextBox ID="txtvesselsearch" runat="server" Width="200px" AutoPostBack="true" EmptyMessage="Vessel" OnTextChanged="txtvesselsearch_TextChanged"></telerik:RadTextBox>
                                                        <%-- <input type="text" class="form-control input-sm m-b-xs" id="filter"
                                                        placeholder="Search in table" />--%>
                                                    </div>
                                                    <telerik:RadGrid ID="gvVessel" runat="server" AutoGenerateColumns="false"
                                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None"
                                                        EnableHeaderContextMenu="true" AllowPaging="true" PageSize="1000" OnNeedDataSource="gvVessel_NeedDataSource"
                                                        OnItemDataBound="gvVessel_ItemDataBound" OnItemCommand="gvVessel_ItemCommand">
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
                                                                <telerik:GridTemplateColumn HeaderText="Vessel">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkvessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                                                                        <telerik:RadLabel ID="lblvesselid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="22px" UniqueName="FLDVERSION">
                                                                    <ItemTemplate>
                                                                        <telerik:RadLabel ID="lblVersion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERSION") %>'></telerik:RadLabel>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="22px" UniqueName="CREWLIST">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkcrewlist" runat="server" Text="Crew List" CommandName="CREWLIST" ToolTip="Crew List">
                                                                            <span class="icon"><i class="fas fa-user"></i></span>
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="22px" UniqueName="CREWEVENT">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkcrewevent" runat="server" Text="Crew Event" CommandName="CREWEVENT" ToolTip="Event">
                                                                             <span class="icon"><i class="fas fa-calendar-alt"></i></span>
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="22px" UniqueName="CREWRELIEFPLAN">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkReliefPlan" runat="server" Text="Relief Plan" CommandName="CREWRELIEFPLAN" ToolTip="Relief Plan">
                                                                             <span class="icon"><i class="fas fa-tasks-Planned"></i></span>
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
																<telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="22px">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkDashboard" runat="server" CommandName="DASHBOARD" Text="Dashboard" ToolTip="Dashboard">
                                                                             <span class="icon"><i class="fas fa-tachometer-alt"></i></span>
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="22px">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkownerreport" runat="server" CommandName="OWNERSREPORT" Text="Owners Report" ToolTip="Owners Report">
                                                                             <span class="icon"><i class="fa-file-tn"></i></span>
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="22px">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkComponentHierarchy" runat="server" CommandName="COMPONENTHIERARCHY" Text="Component Hierarchy" ToolTip="Component Hierarchy">
                                                                             <span class="icon"><i class="fas fa-component"></i></span>
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="22px">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkCertificates" runat="server" CommandName="CERTIFICATES" Text="Certificates" ToolTip="Certificates">
                                                                             <span class="icon"><i class="fas fa-certificates"></i></span>
                                                                        </asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="A/C Code">
                                                                    <ItemTemplate>
                                                                        <%# DataBinder.Eval(Container,"DataItem.FLDVESSELCODE") %> - <%# DataBinder.Eval(Container,"DataItem.FLDVESSELACCOUNT").ToString().Trim().TrimEnd(',') %>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                        <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                                                            <Resizing AllowColumnResize="true" />
                                                        </ClientSettings>
                                                        <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                                    </telerik:RadGrid>
                                                </telerik:RadAjaxPanel>
                                            </div>
                                        </div>

                                    </div>


                                    <div class="col-lg-4" runat="server">

                                        <div class="panel panel-success">

                                            <div class="panel-heading">
                                                <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpms','Certificates / Surveys','Dashboard/DashboardOfficeV2TechnicalCertificate.aspx?mod=INSSCHEDUL',false)">Certificates / Surveys</a>
                                            </div>
                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvCertificateSchedule" runat="server" AutoGenerateColumns="false"
													AllowSorting="false" GroupingEnabled="false" Height="350px" BorderStyle="None"
                                                    EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvCertificateSchedule_NeedDataSource"
                                                    OnItemDataBound="gvCertificateSchedule_ItemDataBound">
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
                                                            <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="55%">
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
                                                                    <asp:LinkButton ID="lnk30Days" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD30COUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lbl30Daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD30URL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="60 Days" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnk60Days" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD60COUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lbl60Daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD60URL") %>'></telerik:RadLabel>
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
                                        <div class="panel panel-success" runat="server" id="divTechTask">

                                            <div class="panel-heading">
                                                <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpms','Tasks','Dashboard/DashboardOfficeV2TechnicalTask.aspx?mod=TASK',false)">Tasks</a>
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvTechTask" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="230px" BorderStyle="None"
                                                    EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvTechTask_NeedDataSource"
                                                    OnItemDataBound="gvTechTask_ItemDataBound">
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
                                                            <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="23%">
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
                                                            <telerik:GridTemplateColumn HeaderText="15 Days" HeaderStyle-Width="12%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnk30Days" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD30COUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lbl30Daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD30URL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Pndg Closure" HeaderStyle-Width="14%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkPndgClosure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblPndgClosure" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Extn. Req." HeaderStyle-Width="12%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkExtnReq" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblExtnReq" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="2ry Pndg Approval" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnk2ryPndg" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPSACOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lbl2ryPndg" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPSAURL") %>'></telerik:RadLabel>
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

                                        <div class="panel panel-success" id="divAudit" runat="server">

                                            <div class="panel-heading">
                                                <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpms','Audit / Inspection','Dashboard/DashboardOfficeV2TechnicalAuditInspection.aspx?mod=INSLOG',false)">Audit / Inspection</a>
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvInspectionStatus" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="440px" BorderStyle="None"
                                                    EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvInspectionStatus_NeedDataSource"
                                                    OnItemDataBound="gvInspectionStatus_ItemDataBound">
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
                                                            <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="22%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="60 Days" HeaderTooltip="60 Days Due" HeaderStyle-Wrap="true" HeaderStyle-Width="13%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnk60count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD60COUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lbl60url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD60URL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="O'due" HeaderTooltip="Overdue" HeaderStyle-Wrap="true" HeaderStyle-Width="13%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkOverduecount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblOverdueurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Cmpl'd" HeaderTooltip="Completed" HeaderStyle-Wrap="true" HeaderStyle-Width="13%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkCompleted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCMPCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblCompletedurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCMPURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Review O'due" HeaderTooltip="Review Overdue" HeaderStyle-Wrap="true" HeaderStyle-Width="13%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkReviewOverduecount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVOVDCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblReviewOverdueurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVOVDURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Rev'd" HeaderTooltip="Reviewed" HeaderStyle-Wrap="true" HeaderStyle-Width="13%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkReviewedcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblReviewedurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Closure O'due" HeaderTooltip="Closure Overdue" HeaderStyle-Wrap="true" HeaderStyle-Width="13%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkClosureOverduecount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLDOVDCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblClosureOverdueurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLDOVDURL") %>'></telerik:RadLabel>
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

                                        <div class="panel panel-success" runat="server" id="divPMS">

                                            <div class="panel-heading">
                                                <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpopup','PMS','Dashboard/DashboardOfficeV2TechnicalPMS.aspx?mod=pmsv2')">PMS</a>
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="GvPMS" runat="server" ShowHeader="false" ShowFooter="false" AutoGenerateColumns="false"
                                                    GridLines="None" BorderStyle="None" Height="350px" OnItemCommand="GvPMS_ItemCommand"
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
                                                                <ItemStyle HorizontalAlign="Center" />
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


                                    <div class="col-lg-4" runat="server">
                                        <div class="panel panel-success" runat="server" id="divDeficiencies">

                                            <div class="panel-heading">
                                                <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpms','Deficiencies Status','Dashboard/DashboardOfficeV2HSEQA.aspx?mod=INSDEF',false)">Deficiencies</a>
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvDeficiency" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="440px" BorderStyle="None"
                                                    EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvDeficiency_NeedDataSource"
                                                    OnItemDataBound="gvDeficiency_ItemDataBound">
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
                                                            <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="60%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Ship" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkShip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblShip" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Office" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkOffice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICECOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblOffice" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICEURL") %>'></telerik:RadLabel>
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

                                        <div class="panel panel-success" id="divTechAudit" runat="server">

                                            <div class="panel-heading">
                                                <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpms','Audit / Inspection','Dashboard/DashboardOfficeV2TechnicalAuditInspection.aspx?mod=INSLOG',false)">Audit / Inspection</a>
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvTechAudit" runat="server" AutoGenerateColumns="false"
													AllowSorting="false" GroupingEnabled="false" Height="350px" BorderStyle="None"
                                                    EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvTechAudit_NeedDataSource"
                                                    OnItemDataBound="gvTechAudit_ItemDataBound">
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
                                                            <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="22%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="60 Days" HeaderTooltip="60 Days Due" HeaderStyle-Wrap="true" HeaderStyle-Width="13%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnk60count" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD60COUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lbl60url" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD60URL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="O'due" HeaderTooltip="Overdue" HeaderStyle-Wrap="true" HeaderStyle-Width="13%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkOverduecount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblOverdueurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Cmpl'd" HeaderTooltip="Completed" HeaderStyle-Wrap="true" HeaderStyle-Width="13%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkCompleted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCMPCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblCompletedurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCMPURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Review O'due" HeaderTooltip="Review Overdue" HeaderStyle-Wrap="true" HeaderStyle-Width="13%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkReviewOverduecount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVOVDCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblReviewOverdueurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVOVDURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Rev'd" HeaderTooltip="Reviewed" HeaderStyle-Wrap="true" HeaderStyle-Width="13%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkReviewedcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblReviewedurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Closure O'due" HeaderTooltip="Closure Overdue" HeaderStyle-Wrap="true" HeaderStyle-Width="13%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkClosureOverduecount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLDOVDCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblClosureOverdueurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLDOVDURL") %>'></telerik:RadLabel>
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

                                        <div class="panel panel-success" runat="server" id="divQMSTask">

                                            <div class="panel-heading">
                                                <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpms','Tasks','Dashboard/DashboardOfficeV2TechnicalTask.aspx?mod=TASK',false)">Tasks</a>
                                            </div>

                                            <div class="panel-body">
                                                <telerik:RadGrid ID="gvTask" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" Height="440px" BorderStyle="None"
                                                    EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvTask_NeedDataSource"
                                                    OnItemDataBound="gvTask_ItemDataBound">
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
                                                            <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="23%">
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
                                                            <telerik:GridTemplateColumn HeaderText="15 Days" HeaderStyle-Width="12%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnk30Days" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLD30COUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lbl30Daysurl" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLD30URL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Pndg Closure" HeaderStyle-Width="14%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkPndgClosure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblPndgClosure" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Extn. Req." HeaderStyle-Width="12%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkExtnReq" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblExtnReq" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXTURL") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="2ry Pndg Approval" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnk2ryPndg" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPSACOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lbl2ryPndg" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPSAURL") %>'></telerik:RadLabel>
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
<div class="panel panel-success" runat="server" id="divvess">

                                                <div class="panel-heading">
                                                    <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpopup','Vessel Position','Dashboard/DashboardOfficeV2TechnicalPMS.aspx?mod=VPSRPT')" >Vessel Position Reports</a>                                                  
                                                </div>

                                                <div class="panel-body">
                                                    <telerik:RadGrid ID="gvVPRSReports" runat="server" AutoGenerateColumns="false"
                                                        AllowSorting="false" GroupingEnabled="false" Height="230px" BorderStyle="None"
                                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvVPRSReports_NeedDataSource"
                                                        OnItemDataBound="gvVPRSReports_ItemDataBound">
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
                                                                <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="50%">
                                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                    <ItemTemplate>                                                                        
                                                                        <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>                                                               
                                                                <telerik:GridTemplateColumn HeaderText="Overdue" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkoverdue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERDUECOUNT") %>'></asp:LinkButton>
                                                                        <telerik:RadLabel ID="lbloverdue" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOUVERDUEURL") %>'></telerik:RadLabel>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                                <telerik:GridTemplateColumn HeaderText="Pending Review" HeaderStyle-Width="30%" HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkreview" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWCOUNT") %>'></asp:LinkButton>
                                                                        <telerik:RadLabel ID="lblreview" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWURL") %>'></telerik:RadLabel>
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

                                        <div class="panel panel-success" runat="server" id="divPurchase">

                                            <div class="panel-heading">
                                                <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('dpopup','Purchase','Dashboard/DashboardOfficeV2TechnicalPMS.aspx?mod=purchasev2')">Purchase</a>
                                            </div>

                                            <div class="panel-body">
                                                <div class="col-lg-6">
                                                    <telerik:RadGrid ID="GvPurchase" runat="server" ShowHeader="false" ShowFooter="false" AutoGenerateColumns="false"
                                                        GridLines="None" BorderStyle="None" Height="350px"
                                                        OnNeedDataSource="GvPurchase_NeedDataSource" OnItemDataBound="GvPurchase_ItemDataBound">
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
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </div>
                                                <div class="col-lg-6">
                                                    <telerik:RadGrid ID="GvPurchase1" runat="server" ShowHeader="false" ShowFooter="false" AutoGenerateColumns="false"
                                                        GridLines="None" BorderStyle="None" Height="350px"
                                                        OnNeedDataSource="GvPurchase_NeedDataSource" OnItemDataBound="GvPurchase_ItemDataBound">
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
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lnkCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </telerik:GridTemplateColumn>
                                                            </Columns>
                                                        </MasterTableView>
                                                    </telerik:RadGrid>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div id="tab-2" class="tab-pane">
                                <iframe runat="server" id="ifMoreInfoDahboard2" frameborder="0" style="width: 100%; height:80%"></iframe>
                            </div>

                            <div id="tab-3" class="tab-pane">
                                <iframe runat="server" id="ifMoreInfoQuality" frameborder="0" style="width: 100%; height:80%"></iframe>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--<telerik:RadTabStrip ID="RadTabStrip1" runat="server" MultiPageID="RadMultiPage1"
            SelectedIndex="0">
            <Tabs>
                <telerik:RadTab Text="Page 1" NavigateUrl="defaultcs.aspx" runat="server" Selected="True">
                </telerik:RadTab>
                <telerik:RadTab Text="Page 2" NavigateUrl="defaultcs.aspx?page=webinars" runat="server">
                </telerik:RadTab>
            </Tabs>
        </telerik:RadTabStrip>
        <telerik:RadPageView runat="server" ID="RadMultiPage1" SelectedIndex="0">
            <telerik:RadPageView runat="server" ID="RadPageView1">
            </telerik:RadPageView>
            <telerik:RadPageView runat="server" ID="PageView2">
            </telerik:RadPageView>
        </telerik:RadPageView>--%>
    </form>
    <script type="text/javascript" src="../Scripts/jquery-3.4.1.min.js"></script>
    <script type="text/javascript" src="../Scripts/bootstrap.min.v3.js"></script>
</body>
</html>