<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardOfficeV2Crew.aspx.cs" Inherits="DashboardOfficeV2Crew" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Src="~/UserControls/UserControlErrorMessage.ascx" TagPrefix="eluc" TagName="Error" %>

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
            function pageLoad() {
                var code = document.querySelectorAll('[title="Code"]');
                for (i = 0; i < code.length; ++i) {
                    code[i].parentElement.style.display = "none";
                }
                setTimeout(resize, 300);
                resize();
            }
            function resize() {
                var headerRow = document.querySelectorAll(".rpgRowHeaderZoneDiv table tbody tr");
                var dataRow = document.querySelectorAll(".rpgContentZoneDiv table tbody tr");
                for (var i = 0; i < headerRow.length; i++) {
                    var row = headerRow[i]
                    var data = dataRow[i];
                    row.style.height = row.offsetHeight + "px";
                    data.style.height = (row.offsetHeight) + "px";
                }
            }
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
                        panelheight = e.parentElement.parentElement.parentElement.offsetHeight - 70;
                        if (e.id.toLowerCase() != "gvvessel")
                            gridheight += e.offsetHeight;
                    });
                    grid.GridDataDiv.style.height = (panelheight - gridheight) + "px";
                    
                }
            }
            window.onload = window.onresize = resize;        
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .mlabel {
            display: inline;
            padding: .2em .6em .3em;
            font-size: 82%;
            font-weight: 700;
            line-height: 1;
            color: #fff;
            text-align: center;
            white-space: nowrap;
            vertical-align: baseline;
            border-radius: .25em;
        }

        .icon {
            padding-right: 2px;
            font-size: 16px;
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
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" OnAjaxRequest="RadAjaxManager1_AjaxRequest">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="RadAjaxManager1"></telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="GvCrewVslSummary">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="GvCrewVslSummary" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="gvCrewSummary">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvCrewSummary" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <%-- <telerik:AjaxSetting AjaxControlID="RadAjaxManager1">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvVessel" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>--%>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="gray-bg">
            <div class="row page-heading bg-success">
                <div class="col-lg-3">
                    <h2 class="font-bold">
                        <a runat="server" id="lnkDashboard" href="#" style="color: white" onclick="javascript: top.openNewWindow('filter','Filter','Dashboard/DashboardOfficeV2Filter.aspx?d=2', false, 600, 300,null,null,{icon: '<i class=\'fas fa-filter\'></i>'})">Crew <span class="icon"><i class="fas fa-filter"></i></span>
                        </a>
                    </h2>
                </div>
                <div class="col-lg-9">
                    <br />
                    <a id="lnkVetting" runat="server" class="btn btn-warning" href="javascript: top.openNewWindow('dpopup','Vetting','Dashboard/DashboardOfficeV2TechnicalPMS.aspx?mod=vetting')">Vetting</a>
                    <a id="lnkPhoenixAnalytics" class="btn btn-warning" target="_blank" runat="server">Phoenix Analytics</a>
                    <a id="lnkWRHAnalytics" class="btn btn-warning" target="_blank" runat="server">WRH Analytics</a>
                    <a id="lnkAnalytics" class="btn btn-warning" runat="server" href="javascript: top.openNewWindow('dpopup','Analytics','Dashboard/QualityPBI.html')">Analytics</a>

                    <asp:LinkButton ID="btnHSQEA" runat="server" OnClick="btnHSQEA_Click" Text="HSEQA" CssClass="btn btn-primary"></asp:LinkButton>
                    <asp:LinkButton ID="BtnTech" runat="server" OnClick="BtnTech_Click" Text="Tech" CssClass="btn btn-primary"></asp:LinkButton>
                    <asp:LinkButton ID="BtnAccounts" runat="server" OnClick="BtnAccounts_Click" Text="Accounts" CssClass="btn btn-primary"></asp:LinkButton>
                </div>
            </div>
            <div class="row">
                <div class="col-lg-12">
                    <div class="tabs-container">
                        <ul class="nav nav-tabs">
                            <li class="active"><a data-toggle="tab" href="#tab-1">Page 1</a></li>
                            <li class="" style="display: none"><a data-toggle="tab" href="#tab-2">Page 2</a></li>
                        </ul>
                        <div class="tab-content">
                            <div id="tab-1" class="tab-pane active">
                                <div class="panel-body">
                                    <div class="col-lg-12">
                                        <div class="panel-body">
                                            <div class="col-lg-4">

                                                <div class="panel panel-success">
                                                    <div class="panel-heading">
                                                        <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: var name= document.getElementById('txtvesselsearch').value;name = (name == 'Vessel' ? '' : name);top.openNewWindow('dpopup','World map - Fleet List','Dashboard/DashboardV2Map.aspx?name='+name)">World map - Fleet List</a>
                                                        <a id="lnkOtis" class="text-primary pull-right" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('otis', 'OTIS', 'https://system.stratumfive.com/otis/index.html', true);">OTIS</a>
                                                    </div>
                                                    <div class="panel-body" style="height: 1088px">
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
                                                                    <Columns>
                                                                        <telerik:GridTemplateColumn HeaderText="Vessel">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkvessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:LinkButton>
                                                                                <telerik:RadLabel ID="lblvesselid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="22px">
                                                                            <ItemTemplate>
                                                                                <asp:LinkButton ID="lnkcrewlist" runat="server" Text="Crew List" CommandName="CREWLIST" ToolTip="Crew List">
                                                                            <span class="icon"><i class="fas fa-user"></i></span>
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
                                                                                <asp:LinkButton ID="lnkCertificates" runat="server" CommandName="CERTIFICATES" Text="Certificates" ToolTip="Certificates">
                                                                                     <span class="icon"><i class="fas fa-certificates"></i></span>
                                                                                </asp:LinkButton>
                                                                            </ItemTemplate>
                                                                        </telerik:GridTemplateColumn>
                                                                        <telerik:GridTemplateColumn HeaderText="A/C Code">
                                                                            <%-- <ItemStyle Wrap="false"/>--%>
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

                                            <div class="col-lg-8">
                                                <div class="panel panel-success" style="height: 550px; overflow: auto">
                                                    <div class="panel-heading">
                                                        <%-- <telerik:RadLabel ID="lblCrewSummary" runat="server" Text="Crew Summary"></telerik:RadLabel>--%>

                                                        <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('crew','Crew Summary','Dashboard/DashboardOfficeV2CrewSummary.aspx?mod=CREWV2',true)">Crew Summary</a>
                                                    </div>
                                                    <div class="panel-body">
                                                        <telerik:RadPivotGrid ID="gvCrewSummary" runat="server" TotalsSettings-ColumnGrandTotalsPosition="None" OnNeedDataSource="gvCrewSummary_NeedDataSource" ColumnHeaderCellStyle-Width="300px"
                                                            AllowPaging="false" AllowFiltering="true" ShowFilterHeaderZone="false" TotalsSettings-RowGrandTotalsPosition="None" OnCellDataBound="gvCrewSummary_CellDataBound"
                                                            ShowColumnHeaderZone="true" ShowDataHeaderZone="true" ShowRowHeaderZone="true" Height="480px" TotalsSettings-RowsSubTotalsPosition="None">
                                                            <ClientSettings>
                                                                <Resizing AllowColumnResize="true" EnableRealTimeResize="true" />
                                                                <Scrolling SaveScrollPosition="true" AllowVerticalScroll="true" />
                                                            </ClientSettings>
                                                            <RowHeaderCellStyle Width="300px" />
                                                            <ColumnHeaderCellStyle Width="300px" />
                                                            <DataCellStyle Width="300px" />
                                                            <Fields>
                                                                <telerik:PivotGridColumnField DataField="RankName" Caption="Group Rank" SortOrder="None">

                                                                    <CellTemplate>
                                                                        <asp:Label ID="Label1" runat="server" CssClass="rotate" ToolTip='<%# Container.DataItem %>'>
                                                                           <%# Container.DataItem %>
                                                                        </asp:Label>
                                                                    </CellTemplate>
                                                                </telerik:PivotGridColumnField>
                                                                <telerik:PivotGridRowField DataField="Measure" Caption="Measure" CellStyle-Width="285px">
                                                                </telerik:PivotGridRowField>
                                                                <telerik:PivotGridRowField DataField="FLDMEASUREID" Caption="Code" CellStyle-Width="1px" CellStyle-CssClass="hidden">
                                                                </telerik:PivotGridRowField>
                                                                <telerik:PivotGridAggregateField DataField="Count" Caption="Count" Aggregate="Sum" IgnoreNullValues="true" CellStyle-Width="50px"></telerik:PivotGridAggregateField>
                                                            </Fields>
                                                        </telerik:RadPivotGrid>
                                                    </div>
                                                </div>
                                                <div class="panel panel-success" style="height: 550px; overflow: auto">
                                                    <div class="panel-heading">
                                                        <%--<telerik:RadLabel ID="lblVesselSummary" runat="server" Text="Vessel Summary"></telerik:RadLabel>--%>
                                                        <a class="text-primary" style="color: white; text-decoration: underline" href="javascript: top.openNewWindow('crew','Vessel Summary','Dashboard/DashboardOfficeV2CrewVslSummary.aspx?mod=CREWV2',true)">Vessel Summary</a>
                                                    </div>
                                                    <div class="panel-body">
                                                        <telerik:RadPivotGrid ID="GvCrewVslSummary" runat="server" TotalsSettings-ColumnGrandTotalsPosition="None" OnNeedDataSource="GvCrewVslSummary_NeedDataSource"
                                                            AllowPaging="false" AllowFiltering="true" ShowFilterHeaderZone="false" TotalsSettings-RowGrandTotalsPosition="None" OnCellDataBound="GvCrewVslSummary_CellDataBound"
                                                            ShowColumnHeaderZone="true" ShowDataHeaderZone="true" ShowRowHeaderZone="true" Height="480px" TotalsSettings-RowsSubTotalsPosition="None">
                                                            <ClientSettings>
                                                                <Resizing AllowColumnResize="true" EnableRealTimeResize="true" />
                                                                <Scrolling SaveScrollPosition="true" AllowVerticalScroll="true" />
                                                            </ClientSettings>
                                                            <Fields>
                                                                <telerik:PivotGridColumnField DataField="Vessel" Caption="Vessel">
                                                                    
                                                                    <CellTemplate>
                                                                        <asp:Label ID="Label1" runat="server" CssClass="rotate" ToolTip='<%# Container.DataItem %>'>
                                                                         <%# Container.DataItem %>
                                                                        </asp:Label>
                                                                    </CellTemplate>
                                                                </telerik:PivotGridColumnField>
                                                                <telerik:PivotGridRowField DataField="Measure" Caption="Measure" CellStyle-Width="250px">                                                                    
                                                                </telerik:PivotGridRowField>
                                                                <telerik:PivotGridRowField DataField="FLDMEASUREID" Caption="Code" CellStyle-Width="1px" CellStyle-CssClass="hidden">
                                                                </telerik:PivotGridRowField>
                                                                <telerik:PivotGridAggregateField DataField="Count" Caption="Count" Aggregate="Sum" IgnoreNullValues="true" CellStyle-Width="50px"></telerik:PivotGridAggregateField>
                                                            </Fields>
                                                        </telerik:RadPivotGrid>
                                                    </div>
                                                </div>

                                            </div>

                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div id="tab-2" class="tab-pane">
                                <div class="panel-body">
                                    <div class="col-lg-12">

                                        <div class="col-lg-3">
                                            <div class="panel panel-info">

                                                <div class="panel-heading">
                                                    Interview Pending 
                                                </div>

                                                <div class="panel-body text-center" runat="server" id="Cat3and4Planned">
                                                    <div class="col-lg-4">
                                                        <h5>Crew</h5>
                                                        <button type="button" class="btn btn-outline btn-danger" runat="server" id="BtnAssessmentCP">
                                                            <br />
                                                            0</button>

                                                    </div>
                                                    <div class="col-lg-4">
                                                        <h5>Tech</h5>

                                                        <button type="button" class="btn btn-outline btn-warning" runat="server" id="BtnAssessmentTP">
                                                            <br />
                                                            0</button>
                                                    </div>
                                                    <div class="col-lg-4">
                                                        <h5>Enhance</h5>
                                                        <button type="button" class="btn btn-outline btn-primary" runat="server" id="BtnAssessmentEP">
                                                            <br />
                                                            0</button>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
    <script type="text/javascript" src="../Scripts/jquery-3.4.1.min.js"></script>
    <script type="text/javascript" src="../Scripts/bootstrap.min.v3.js"></script>
</body>
</html>
