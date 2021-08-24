<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogEngineLogPinkSheet.aspx.cs" Inherits="Log_ElectricLogEngineLogPinkSheet" %>

<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Engine Log Book</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/OwnersReport.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
        <script type="text/javascript">
            //Dummy function to ignore javascript page error
            function OnHeaderMenuShowing(sender, args) {
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

        .RadGrid .rgHeader, .RadGrid th.rgResizeCol, .RadGrid .rgRow td, .RadGrid .rgAltRow td {
            padding-left: 2px !important;
            padding-right: 2px !important;
        }

        .higherZIndex {
            z-index: 2;
        }

        .rdTitleWrapper, .rdTitleBar {
            background-color: rgb(194, 220, 252) !important;
            background-image: linear-gradient(rgb(244, 248, 250), rgb(233, 242, 251) 50%, rgb(221, 231, 245) 50%, rgb(228, 237, 248)) !important;
            color: black !important;
        }

        .col-left {
            width: 40%;
            vertical-align: top;
        }

        .col-right {
            width: 60%;
            vertical-align: top;
        }

        .RadGrid_Windows7 .rgHeader {
            color: black !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1"></telerik:RadAjaxLoadingPanel>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="93%">
            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>

            <asp:Button ID="cmdEngineDepartment" runat="server" OnClick="cmdEngineDepartment_Click" CssClass="hidden" />
            <asp:Button ID="cmdLO" runat="server" OnClick="cmdLO_Click" CssClass="hidden" />
            <asp:Button ID="cmdEmergencySafety" runat="server" OnClick="cmdEmergencySafety_Click" CssClass="hidden" />
            <asp:Button ID="cmdTurbo" runat="server" OnClick="cmdTurbo_Click" CssClass="hidden" />
            <asp:Button ID="cmdAuxillary" runat="server" OnClick="cmdAuxillary_Click" CssClass="hidden" />
            <asp:Button ID="cmdAirCompressor" runat="server" OnClick="cmdAirCompressor_Click" CssClass="hidden" />
            <asp:Button ID="cmdMainteince" runat="server" OnClick="cmdMainteince_Click" CssClass="hidden" />
            <asp:Button ID="cmEngineUnitWrapper" runat="server" OnClick="cmEngineUnitWrapper_Click" CssClass="hidden" />
            <asp:Button ID="cmdEngineUnit" runat="server" OnClick="cmdEngineUnit_Click" CssClass="hidden" />
            <asp:Button ID="cmdWrapper" runat="server" OnClick="cmdWrapper_Click" CssClass="hidden" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table style="width: 100%" runat="server">
                <tr>
                    <td class="col-left">
                        <telerik:RadDockZone ID="EngineDockZone" runat="server" Orientation="Vertical" Docks="RadDock1,RadDock2,RadDock3,RadDock4,RadDock5,RadDock6,RadDock7" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock1" RenderMode="Lightweight" runat="server" Height="300px"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblEngineDepartmentStaff" runat="server" Text="Engine Department Staff"></telerik:RadLabel>
                                            </td>
                                            <td>&nbsp;<asp:LinkButton ID="lnkEngineDepartmentStaffInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                            </asp:LinkButton>
                                            </td>
                                            <td>&nbsp;<asp:LinkButton ID="lnkEngineDepartmentStaffComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                            </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </TitlebarTemplate>
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvEngineDepartment" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvEngineDepartment_NeedDataSource"
                                        Height="250px"
                                        Width="100%">
                                        <MasterTableView TableLayout="Fixed" EnableHeaderContextMenu="false">
                                            <NoRecordsTemplate>
                                                <table runat="server" width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="FLDNAME" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDRANKCODE" HeaderText="Rank" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDDATESIGNEDON" DataFormatString="{0:dd-MM-yyyy}" HeaderText="Signed - on" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDDATESIGNEDOFF" DataFormatString="{0:dd-MM-yyyy}" HeaderText="Signed - off" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                            <Resizing AllowColumnResize="true" />
                                        </ClientSettings>
                                        <PagerStyle Mode="NextPrevNumericAndAdvanced"></PagerStyle>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>

                    <td class="col-right">
                        <telerik:RadDockZone ID="TurboChargerDockZone" runat="server" Orientation="Vertical" Docks="RadDock1,RadDock2,RadDock3,RadDock4,RadDock5,RadDock6,RadDock7" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock2" RenderMode="Lightweight" runat="server" Height="300px"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel
                                                    ID="lnkTurboChargers"
                                                    runat="server"
                                                    Text="Turbo Chargers">
                                                </telerik:RadLabel>
                                            </td>
                                            <td>&nbsp;<asp:LinkButton ID="lnkTurboChargersInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                            </asp:LinkButton>
                                            </td>
                                            <td>&nbsp;<asp:LinkButton ID="lnkTurboChargersComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                            </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </TitlebarTemplate>
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvTurboChargers" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvTurboChargers_NeedDataSource"
                                        Height="250px"
                                        Width="100%">
                                        <MasterTableView TableLayout="Fixed" EnableHeaderContextMenu="false">
                                            <NoRecordsTemplate>
                                                <table runat="server" width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="FLDMEASURENAME" HeaderText="T/C  Working Hrs/Date" UniqueName="measure" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT1" HeaderText="T/C - 1" UniqueName="unit1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT2" HeaderText="T/C - 2" UniqueName="unit2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT3" HeaderText="T/C - 3" UniqueName="unit3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT4" HeaderText="T/C - 4" UniqueName="unit4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                            <Resizing AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td class="col-left">
                        <telerik:RadDockZone ID="LODockZone" runat="server" Orientation="Vertical" Docks="RadDock1,RadDock2,RadDock3,RadDock4,RadDock5,RadDock6,RadDock7" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock3" RenderMode="Lightweight" runat="server" Height="300px"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel
                                                    ID="lnkLOAnalysis"
                                                    runat="server"
                                                    Text="L.O. Analysis">
                                                </telerik:RadLabel>
                                            </td>
                                            <td>&nbsp;<asp:LinkButton ID="lnkLOAnalysisInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                            </asp:LinkButton>
                                            </td>
                                            <td>&nbsp;<asp:LinkButton ID="lnkLOAnalysisComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                            </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </TitlebarTemplate>
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvEngineLOAnalysis" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvEngineLOAnalysis_NeedDataSource"
                                        Height="250px"
                                        Width="100%">
                                        <MasterTableView TableLayout="Fixed" EnableHeaderContextMenu="false">
                                            <NoRecordsTemplate>
                                                <table runat="server" width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="FLDMEASURENAME" HeaderText="L. O Analysis" UniqueName="mainengine" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDMEUNIT1" HeaderText="Main Engine" UniqueName="mainengine" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDAUXUNIT1" HeaderText="A/E - 1" UniqueName="unit1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDAUXUNIT2" HeaderText="A/E - 2" UniqueName="unit2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDAUXUNIT3" HeaderText="A/E - 3" UniqueName="unit3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDAUXUNIT4" HeaderText="A/E - 4" UniqueName="unit4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" Visible="false" ItemStyle-Width="50px" HeaderStyle-Width="50px"></telerik:GridBoundColumn>

                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                            <Resizing AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>

                    <td class="col-right">
                        <telerik:RadDockZone ID="AuxilaryDockZone" runat="server" Orientation="Vertical" Docks="RadDock1,RadDock2,RadDock3,RadDock4,RadDock5,RadDock6,RadDock7" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock4" RenderMode="Lightweight" runat="server" Height="300px"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <table>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel
                                                    ID="lnkAuxEngines"
                                                    runat="server"
                                                    Text="Aux. Engines">
                                                </telerik:RadLabel>
                                            </td>
                                            <td>&nbsp;<asp:LinkButton ID="lnkAuxEnginesInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                            </asp:LinkButton>
                                            </td>
                                            <td>&nbsp;<asp:LinkButton ID="lnkAuxEnginesComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                            </asp:LinkButton>
                                            </td>
                                        </tr>
                                    </table>
                                </TitlebarTemplate>
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>

                                    <telerik:RadGrid ID="gvAuxillaryEngine" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvAuxillaryEngine_NeedDataSource"
                                        Height="250px"
                                        Width="100%">
                                        <MasterTableView TableLayout="Fixed" EnableHeaderContextMenu="false">
                                            <NoRecordsTemplate>
                                                <table runat="server" width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="FLDMEASURENAME" HeaderText="Aux. Engines Working Hrs/Date" UniqueName="mainengine" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Left"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT1" HeaderText="A/E - 1" UniqueName="unit1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Width="50px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT2" HeaderText="A/E - 2" UniqueName="unit2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Width="50px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT3" HeaderText="A/E - 3" UniqueName="unit3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Width="50px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT4" HeaderText="A/E - 4" UniqueName="unit4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Width="50px" Visible="false"></telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                            <Resizing AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>

                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td class="col-left">
                        <telerik:RadDockZone ID="EmergencyDockZone" runat="server" Orientation="Vertical" Docks="RadDock1,RadDock2,RadDock3,RadDock4,RadDock5,RadDock6,RadDock7" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock5" RenderMode="Lightweight" runat="server" Height="300px"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar" ToolTip="Emergency & Safety Equipment Notes (Dates Tested)"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
<table>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel
                                                ID="lnkEmergencySafety"
                                                runat="server"
                                                Text="Emergency & Safety Equipment Notes (Dates Tested)">
                                            </telerik:RadLabel>
                                        </td>
                                        <td>
                                             &nbsp;<asp:LinkButton ID="lnkEmergencySafetyInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                        </asp:LinkButton>
                                        </td>
                                        <td>
                                            &nbsp;<asp:LinkButton ID="lnkEmergencySafetyComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
</TitlebarTemplate>
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>

                                    <table style="width: 100%; line-height: 30px;">
                                        <tr>
                                            <td>Emergency Generator :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblEngineGenerator" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td>Life-Boat Engines :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblLifeBoatEngine" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Emergency Fire Pump:</td>
                                            <td>
                                                <telerik:RadLabel ID="lblEmergencyFirePump" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td>Life-Boat Davit Safeties:</td>
                                            <td>
                                                <telerik:RadLabel ID="lblLifeBoatDavitSafeties" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Emergency Air Comp :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblEmergencyAirComp" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td>Fire-Doors & W.T. Doors Closing :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblFireDoorsWTDoorsClosing" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>E/R Fire Detectors 1:</td>
                                            <td>
                                                <telerik:RadLabel ID="lblERFireDetectors1" runat="server"></telerik:RadLabel>
                                            </td>
                                            <%--<td>E/R Crane Safeties :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblERCraneSafeties" runat="server"></telerik:RadLabel>
                                            </td>--%>
                                            <td>Cargo Crane Safeties :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblCargoCraneSafeties" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>E/R Fire Detectors 2:</td>
                                            <td>
                                                <telerik:RadLabel ID="lblERFireDetectors2" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td>Provision Crane Safeties :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblProvisionCraneSafeties" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Quick-Closing Valves :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblQuickClosingValves" runat="server"></telerik:RadLabel>
                                            </td>

                                            <td>Accom Ladder Safeties :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblAccomLadderSafeties" runat="server"></telerik:RadLabel>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>Emergency Trips 1:</td>
                                            <td>
                                                <telerik:RadLabel ID="lblEmergencyTrips" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td>M/E Shut-Down Safeties :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblMEShutdownSafeties" runat="server"></telerik:RadLabel>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>Emergency Trips 1:</td>
                                            <td>
                                                <telerik:RadLabel ID="RadLabel2" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td>A/E Shutdown Safeties 1 :
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="lblAEShutdownSafeties1" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Co2 Alarm :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblCO2Alarm" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td>A/E Shutdown Safeties 2 :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblAEShutdownSafeties2" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Co2 Pressure Alarm :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblCo2PressureAlarm" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td>A/E Shutdown Safeties 3 :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblAEShutdownSafeties3" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>E/R Flaps :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblERFlapsSkyLights" runat="server"></telerik:RadLabel>
                                            </td>

                                            <td>E/R Bilge Alarms :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblERBilgeAlarams" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Sky-Lights :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblSkyLights" runat="server"></telerik:RadLabel>
                                            </td>

                                            <td>Other Bilge Alarms :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblOtherBilge" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>

                                            <td>Emergency Steering Test :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblEmergencySteeringTest" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td>Other Bilge Alarms 1:</td>
                                            <td>
                                                <telerik:RadLabel ID="lblOtherBilgeAlarm1" runat="server"></telerik:RadLabel>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>M/E Emergency Controls :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblMEEmergencyControls" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td>Other Bilge Alarms 2:</td>
                                            <td>
                                                <telerik:RadLabel ID="lblOtherBilgeAlarm2" runat="server"></telerik:RadLabel>
                                            </td>

                                        </tr>
                                        <tr>
                                            <td>Boiler Safety Cut-Out :</td>
                                            <td>
                                                <telerik:RadLabel ID="lblBoilerSafetyCutOut" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td>Other Bilge Alarms 3:</td>
                                            <td>
                                                <telerik:RadLabel ID="lblOtherBilgeAlarm3" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                    <td class="col-right">
                        <telerik:RadDockZone ID="AirCompressorDockZone" runat="server" Orientation="Vertical" Docks="RadDock1,RadDock2,RadDock3,RadDock4,RadDock5,RadDock6,RadDock7" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock6" RenderMode="Lightweight" runat="server" Height="300px"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
<table>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel
                                                ID="lnkAirCompressor"
                                                runat="server"
                                                Text="Air Compressor">
                                            </telerik:RadLabel>
                                        </td>
                                        <td>
                                             &nbsp;<asp:LinkButton ID="lnkAirCompressorInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                        </asp:LinkButton>
                                        </td>
                                        <td>
                                            &nbsp;<asp:LinkButton ID="lnkAirCompressorComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
</TitlebarTemplate>
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>

                                    <telerik:RadGrid ID="gvAirCompressor" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvAirCompressor_NeedDataSource"
                                        Height="250px"
                                        Width="100%">
                                        <MasterTableView TableLayout="Fixed" EnableHeaderContextMenu="false">
                                            <NoRecordsTemplate>
                                                <table runat="server" width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="FLDMEASURENAME" HeaderText="Main Air Comp. Working Hrs/Date" UniqueName="mainengine" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT1" HeaderText="A/C - 1" UniqueName="unit1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Width="50px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT2" HeaderText="A/C - 2" UniqueName="unit2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Width="50px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT3" HeaderText="A/C - 3" UniqueName="unit3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Width="50px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT4" HeaderText="A/C - 4" UniqueName="unit4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="50px" HeaderStyle-Width="50px" Visible="false"></telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                            <Resizing AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>


                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadDockZone ID="MaintenanceDockZone" runat="server" Orientation="Vertical" Docks="RadDock1,RadDock2,RadDock3,RadDock4,RadDock5,RadDock6,RadDock7" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock7" RenderMode="Lightweight" runat="server" Height="400px"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
<table>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel
                                                ID="lblMaintenancePosition"
                                                runat="server"
                                                Text="MaintenancePosition">
                                            </telerik:RadLabel>
                                        </td>
                                        <td>
                                             &nbsp;<asp:LinkButton ID="lnkMaintenancePositionInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                        </asp:LinkButton>
                                        </td>
                                        <td>
                                            &nbsp;<asp:LinkButton ID="lnkMaintenancePositionComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                            </asp:LinkButton>
                                        </td>
                                    </tr>
                                </table>
</TitlebarTemplate>
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvMaintaincePosition" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvMaintaincePosition_NeedDataSource"
                                        Width="100%">
                                        <MasterTableView TableLayout="Fixed" EnableHeaderContextMenu="false">
                                            <NoRecordsTemplate>
                                                <table runat="server" width="100%" border="0">
                                                    <tr>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </NoRecordsTemplate>
                                            <Columns>
                                                <telerik:GridBoundColumn DataField="FLDMEASURENAME" HeaderText="ME Working Hours" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="250px" HeaderStyle-Width="250px"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT1" HeaderText="Unit No. 1" UniqueName="unit1" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT2" HeaderText="Unit No. 2" UniqueName="unit2" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT3" HeaderText="Unit No. 3" UniqueName="unit3" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT4" HeaderText="Unit No. 4" UniqueName="unit4" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT5" HeaderText="Unit No. 5" UniqueName="unit5" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT6" HeaderText="Unit No. 6" UniqueName="unit6" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT7" HeaderText="Unit No. 7" UniqueName="unit7" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT8" HeaderText="Unit No. 8" UniqueName="unit8" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT9" HeaderText="Unit No. 9" UniqueName="unit9" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT10" HeaderText="Unit No. 10" UniqueName="unit10" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT11" HeaderText="Unit No. 11" UniqueName="unit11" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT12" HeaderText="Unit No. 12" UniqueName="unit12" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT13" HeaderText="Unit No. 13" UniqueName="unit13" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT14" HeaderText="Unit No. 14" UniqueName="unit14" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT15" HeaderText="Unit No. 15" UniqueName="unit15" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDUNIT16" HeaderText="Unit No. 16" UniqueName="unit16" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px" Visible="false"></telerik:GridBoundColumn>
                                                <telerik:GridBoundColumn DataField="FLDTOTALHOURS" HeaderText="Total ME Hours till date" UniqueName="totalmehours" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px" HeaderStyle-Width="100px"></telerik:GridBoundColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                            <Resizing AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>

                </tr>

            </table>

        </telerik:RadAjaxPanel>

    </form>
    <script type="text/javascript" src="../Scripts/jquery-3.4.1.min.js"></script>
    <script type="text/javascript" src="../Scripts/bootstrap.min.v3.js"></script>
    <script type="text/javascript" src="../Scripts/dashboard.js"></script>
</body>
</html>

