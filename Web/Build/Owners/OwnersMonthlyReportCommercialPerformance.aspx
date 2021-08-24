<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMonthlyReportCommercialPerformance.aspx.cs" Inherits="OwnersMonthlyReportCommercialPerformance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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

        .RadGrid_Windows7 .rgHeader {
            color: black !important;
        }

        .higherZIndex {
            z-index: 2;
        }

        .rdTitleWrapper, .rdTitleBar {
            background-color: rgb(194, 220, 252) !important;
            background-image: linear-gradient(rgb(244, 248, 250), rgb(233, 242, 251) 50%, rgb(221, 231, 245) 50%, rgb(228, 237, 248)) !important;
            color: black !important;
        }

        .RadDock .rdTitleWrapper {
            padding: 5px 5px !important;
            line-height: 9px !important;
            height: 16px !important;
        }

        .fas, .fa {
            height: 11px !important;
            width: 11px !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="93%" EnableAJAX="false">
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table style="width: 100%" runat="server">
                <tr>
                    <td style="width: 40%; vertical-align: top;">
                        <telerik:RadDockZone ID="rdsummary" runat="server" Orientation="Vertical" MinHeight="610px" Docks="RadDock1,RadDock2" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock1" RenderMode="Lightweight" runat="server" Height="610px" Title="Summary"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel runat="server" ID="lblSummary" Text="Summary"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                         <asp:LinkButton ID="lnkSummaryInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:LinkButton ID="lnkSummaryComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                                        </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </TitlebarTemplate>
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <table cellpadding="2" cellspacing="2" width="100%" style="height: 460px; border-width: 0px; border-style: solid; border: 0px solid #c3cedd">
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblcharter" runat="server" Text="Current charterer"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <telerik:RadLabel ID="txtcharterer" runat="server" Enabled="false" Width="99%"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblCargoes" runat="server" Text="Cargoes Carried"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <telerik:RadLabel ID="txtCargoes" runat="server" Enabled="false" Width="99%"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblCleaning" runat="server" Text="Last Hull Cleaning"></telerik:RadLabel>
                                            </td>
                                            <td colspan="3">
                                                <telerik:RadLabel ID="txtCleaning" runat="server" Enabled="false" Width="99%"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="4">
                                                <telerik:RadLabel ID="RadLabel1" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td width="40%" height="25" style="background-image: linear-gradient(#f4f8fa, #e9f2fb 50%, #dde7f5 50%, #dde8f6);"></td>
                                            <td width="20%" align="center" style="background-image: linear-gradient(#f4f8fa, #e9f2fb 50%, #dde7f5 50%, #dde8f6);">
                                                <telerik:RadLabel ID="lblmBallast" runat="server" Text="Ballast"></telerik:RadLabel>
                                            </td>
                                            <td width="20%" align="center" style="background-image: linear-gradient(#f4f8fa, #e9f2fb 50%, #dde7f5 50%, #dde8f6);">
                                                <telerik:RadLabel ID="lblmLoaded" runat="server" Text="Loaded"></telerik:RadLabel>
                                            </td>
                                            <td width="20%" align="center" style="background-image: linear-gradient(#f4f8fa, #e9f2fb 50%, #dde7f5 50%, #dde8f6);">
                                                <telerik:RadLabel ID="lblmTotal" runat="server" Text="Total"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmSteamingTime" runat="server" Text="Steaming Time"></telerik:RadLabel>
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtballaststeamingtime" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtloadedsteamingtime" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txttotalsteamingtime" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmFullSpeed" runat="server" Text="Full Speed"></telerik:RadLabel>
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtballastfullspeed" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtloadedfullspeed" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txttotalfullspeed" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmReducedSpeed" runat="server" Text="Reduced Speed"></telerik:RadLabel>
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtballastreducedspeed" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtloadedreducedspeed" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txttotalreducedspeed" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmManoeveringTime" runat="server" Text="Manoevering Time"></telerik:RadLabel>
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtballastmanoeveringtime" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtloadedmanoeveringtime" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txttotalmanoeveringtime" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmMEStoppage" runat="server" Text="M/E Stoppage"></telerik:RadLabel>
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtballastmestoppage" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtloadedmestoppage" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txttotalmestoppage" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmDeviationorDelay" runat="server" Text="Deviation or Delay (if any)"></telerik:RadLabel>
                                            </td>
                                            <td align="center"></td>
                                            <td align="center"></td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txttotaldeviationordelay" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmDistanceSteamed" runat="server" Text="Distance Steamed"></telerik:RadLabel>
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtballastdistancesteamed" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtloadeddistancesteamed" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txttotaldistancesteamed" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmEngineDistance" runat="server" Text="Engine Distance"></telerik:RadLabel>
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtballastenginedistance" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtloadedenginedistance" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txttotalenginedistance" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmAverageSpeed" runat="server" Text="Average Speed"></telerik:RadLabel>
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtballastavgspeed" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtloadedavgspeed" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txttotalavgspeed" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmAverageSlip" runat="server" Text="Average Slip"></telerik:RadLabel>
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtballastavgslip" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtloadedavgslip" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txttotalavgslip" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmAverageRPM" runat="server" Text="Average RPM"></telerik:RadLabel>
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtballastavgrpm" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtloadedavgrpm" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txttotalavgrpm" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmAverageBHP" runat="server" Text="Average kW"></telerik:RadLabel>
                                            </td>
                                            <td align="center"></td>
                                            <td align="center"></td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txttotalavgbhp" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmAverageFOCons" runat="server" Text="Average FO Cons/Day"></telerik:RadLabel>
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtballastavgfoconsumptionperday" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtloadedavgfoconsumptionperday" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txttotalavgfoconsumptionperday" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <telerik:RadLabel ID="lblmAvgEEOI" runat="server" Text="Avg EEOI"></telerik:RadLabel>
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtballastAvgEEOI" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txtloadedAvgEEOI" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                            <td align="center">
                                                <telerik:RadLabel ID="txttotalAvgEEOI" runat="server" Enabled="false"
                                                    Width="80px" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                    <td style="width: 60%; vertical-align: top;">
                        <telerik:RadDockZone ID="rdlubeoil" runat="server" Orientation="Vertical" MinHeight="300px" Docks="RadDock2,RadDock3,RadDock4" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock2" RenderMode="Lightweight" runat="server" Height="300px" Title="Lub Oils"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel runat="server" ID="lblLubOils" Text="Lub Oils"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkLubOilsInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:LinkButton ID="lnkLubOilsComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                                        </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </TitlebarTemplate>
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvlubeoil" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None" Height="265px"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvlubeoil_NeedDataSource">
                                        <MasterTableView TableLayout="Fixed" EnableHeaderContextMenu="false" Width="100%">
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
                                                <telerik:GridTemplateColumn HeaderText="Lub Oils" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="17%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lbloilconsumptionid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblloilTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></telerik:RadLabel>
                                                        <%#  DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME")%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Cons" Visible="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblluboilconsumption" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILCONSUMPTION") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="ROB Prv Mth" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="17%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPreviousROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENINGROB") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Received" HeaderStyle-Width="17%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblluboilreceived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILRECEIVED") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="ROB" HeaderStyle-Width="17%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblluboilrob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILROB") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Total Cons" HeaderStyle-Width="17%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblTotalCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCONSUMPTION") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Avg Cons/Day" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAvgCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGCONSUMPTION") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                            <Resizing AllowColumnResize="true" />
                                            <ClientEvents OnHeaderMenuShowing="OnHeaderMenuShowing" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                        <telerik:RadDockZone ID="rdfreshwater" runat="server" Orientation="Vertical" MinHeight="300px" Docks="RadDock2,RadDock3,RadDock4" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock3" RenderMode="Lightweight" runat="server" Height="300px" Title="Fresh Water"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel runat="server" ID="lblFreshWater" Text="FreshWater"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkFreshWaterInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:LinkButton ID="lnkFreshWaterComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                                        </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </TitlebarTemplate>
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvfreshwater" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None" Height="265px"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvfreshwater_NeedDataSource">
                                        <MasterTableView TableLayout="Fixed" EnableHeaderContextMenu="false" Width="100%">
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
                                                <telerik:GridTemplateColumn HeaderText="Fresh Water" HeaderStyle-Width="17%" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lbloilconsumptionid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblloilTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></telerik:RadLabel>
                                                        <%#  DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME")%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="ROB Prv Mth" HeaderStyle-Width="17%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPreviousROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENINGROB") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Produced" HeaderStyle-Width="17%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblfBunkered" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATERRECEIVED") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Cons" Visible="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblwaterconsumption" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATERCONSUMPTION") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="ROB" HeaderStyle-Width="17%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblwaterrob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATERROB") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Total Cons" HeaderStyle-Width="17%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblTotalCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCONSUMPTION") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Avg Cons/Day" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAvgCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGCONSUMPTION") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                            <Resizing AllowColumnResize="true" />
                                            <ClientEvents OnHeaderMenuShowing="OnHeaderMenuShowing" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%;" colspan="2">
                        <telerik:RadDockZone ID="rdFuleOil" runat="server" Orientation="Vertical" MinHeight="400px" Docks="RadDock2,RadDock3,RadDock4" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock4" RenderMode="Lightweight" runat="server" Height="400px" Title="Fuel Oil"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel runat="server" ID="lblFuelOil" Text="FuelOil"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkFuelOilInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:LinkButton ID="lnkFuelOilComments" runat="server" ToolTip="Comments">
                                                   <span class="icon"><i class="fa fa-comments-blue"></i></span>
                                                        </asp:LinkButton>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </TitlebarTemplate>
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <telerik:RadGrid ID="gvFuleOil" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None" Height="355px"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvFuleOil_NeedDataSource" Width="100%">
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
                                                <telerik:GridTemplateColumn HeaderText="Fuel Oil" HeaderStyle-Width="15%" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lbloilconsumptionid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblloilTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></telerik:RadLabel>
                                                        <%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME")%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="ROB Prv Mth" HeaderStyle-Width="9%" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPreviousROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENINGROB") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Bunkered" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblfBunkered" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILBUNKERD") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="M/E" HeaderStyle-Width="6%" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblfueloilconsumptionme" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="A/E" HeaderStyle-Width="6%" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblfueloilconsumptionae" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONAE") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Boiler" HeaderStyle-Width="6%" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblfueloilconsumptionboiler" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONBOILER") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="IGG" HeaderStyle-Width="6%" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblfueloilconsumptionigg" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONIGG") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="C/E" HeaderStyle-Width="6%" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblfueloilconsumptionce" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONCE") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="C/HTG" HeaderStyle-Width="6%" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lbltxtCTHG" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONCTHG") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="TK CLNG" HeaderStyle-Width="6%" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblTKCLNG" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONTKCLNG") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="OTH" HeaderStyle-Width="6%" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOTH" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONOTH") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>

                                                <telerik:GridTemplateColumn HeaderText="ROB" HeaderStyle-Width="6%" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblfueloilconsumptionrob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONROB") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn Visible="false" HeaderText="Revised ROB" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRevisedrob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISEDROB") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Total Cons" HeaderStyle-Width="7%" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblTotalCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCONSUMPTION") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Avg Cons/Day" HeaderStyle-Width="10%" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAvgCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGCONSUMPTION") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
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
</body>
</html>

