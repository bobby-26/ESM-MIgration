<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMonthlyReportCrew.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="OwnersMonthlyReportCrew" %>

<!DOCTYPE html>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%--        <%: Styles.Render("~/bundles/css") %>--%>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
        <link href="../Content/OwnersReport.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
        <script type="text/javascript">
            //Dummy function to ignore javascript page error
            function OnHeaderMenuShowing(sender, args) {
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        /*.panelHeight {
            height: 440px;
        }*/

        /*.panelfont {
            overflow: auto;
            font-size: 11px;
        }*/

        .RadGrid .rgHeader, .RadGrid th.rgResizeCol, .RadGrid .rgRow td, .RadGrid .rgAltRow td {
            padding: 1px !important;
            padding-left: 1px !important;
            padding-right: 1px !important;
            text-wrap: avoid !important;
        }

        .higherZIndex {
            z-index: 2;
        }

        .rdTitle {
            height: 16px !important;
        }

        .rdTitleWrapper, .rdTitleBar {
            background-color: rgb(194, 220, 252) !important;
            background-image: linear-gradient(rgb(244, 248, 250), rgb(233, 242, 251) 50%, rgb(221, 231, 245) 50%, rgb(228, 237, 248)) !important;
            color: black !important;
        }

        body {
            font-size: 11px !important;
        }



        .rgRow {
            background-color: #f0f8ff !important;
        }

        .RadGrid_Windows7 .rgHeader {
            color: black !important;
        }
        /*.RadGrid .rgHeader, .RadGrid th.rgResizeCol {
            padding-top: 1px !important;
            padding-bottom: 1px !important;
            border-bottom: 1px solid transparent;
        }*/
        .RadDockZone {
            padding: 1.5px !important;
            height: 99.9% !important;
            width: 99.5% !important;
        }

        .RadDock {
            width: 99.5% !important;
        }

        .rdContentWrapper {
            overflow: hidden !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="93%" EnableAJAX="false">
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table style="width: 99.9%; height: 99.9%" runat="server">
                <tr>
                    <td style="width: 99.9%; vertical-align: top;">
                        <telerik:RadDockZone ID="tdsummary" runat="server" MinHeight="140px" Orientation="Vertical" Docks="RadDock1,RadDock2,RadDock3" FitDocks="true">
                            <telerik:RadDock ID="RadDock1" RenderMode="Lightweight" runat="server" Title="Summary"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel runat="server" ID="lblSummary" Text="Summary"></telerik:RadLabel>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkSummaryInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                    </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkSummaryComments" runat="server" ToolTip="Comments">
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
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 33.3%; vertical-align: top;">
                                                <telerik:RadGrid ID="gvCrew1" runat="server" ShowHeader="false" ShowFooter="false" AutoGenerateColumns="false"
                                                    GridLines="None" BorderStyle="None" Height="140px"
                                                    OnNeedDataSource="gvCrew_NeedDataSource" OnItemDataBound="gvCrew_ItemDataBound">
                                                    <MasterTableView>
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
                                                            <telerik:GridTemplateColumn HeaderText="Measure">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Count">
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblurl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' Visible="false"></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </td>
                                            <td style="width: 33.3%; vertical-align: top;">
                                                <telerik:RadGrid ID="gvCrew2" runat="server" ShowHeader="false" ShowFooter="false" AutoGenerateColumns="false"
                                                    GridLines="None" BorderStyle="None" Height="140px"
                                                    OnNeedDataSource="gvCrew_NeedDataSource" OnItemDataBound="gvCrew_ItemDataBound">
                                                    <MasterTableView>
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
                                                            <telerik:GridTemplateColumn HeaderText="Measure">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Count">
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblurl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' Visible="false"></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </td>
                                            <td style="width: 33.3%; vertical-align: top;">
                                                <telerik:RadGrid ID="gvCrew3" runat="server" ShowHeader="false" ShowFooter="false" AutoGenerateColumns="false"
                                                    GridLines="None" BorderStyle="None" Height="140px"
                                                    OnNeedDataSource="gvCrew_NeedDataSource" OnItemDataBound="gvCrew_ItemDataBound">
                                                    <MasterTableView>
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
                                                            <telerik:GridTemplateColumn HeaderText="Measure">
                                                                <ItemStyle HorizontalAlign="Left" />
                                                                <ItemTemplate>
                                                                    <%# DataBinder.Eval(Container,"DataItem.FLDMEASURENAME") %>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Count">
                                                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblurl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' Visible="false"></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                </telerik:RadGrid>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; vertical-align: top;">
                        <telerik:RadDockZone ID="rdReleif" runat="server" MinHeight="240px" Orientation="Vertical" Docks="RadDock1,RadDock2,RadDock3" FitDocks="true">
                            <telerik:RadDock ID="RadDock2" RenderMode="Lightweight" runat="server" Title="Relief Planner"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar" Height="240px"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton
                                                            ID="lnkRelief"
                                                            runat="server"
                                                            Style="text-decoration: underline; color: black;"
                                                            Text="Relief Planner">
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkReliefInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                    </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkReliefComments" runat="server" ToolTip="Comments">
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
                                    <telerik:RadGrid ID="gvReleif" runat="server" ShowHeader="true" ShowFooter="false" AutoGenerateColumns="false"
                                        GridLines="None" BorderStyle="None" Height="240px" ClientIDMode="AutoID"
                                        OnNeedDataSource="gvReleif_NeedDataSource" OnItemDataBound="gvReleif_ItemDataBound">
                                        <MasterTableView ShowHeader="true" TableLayout="Fixed">
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
                                                <telerik:GridTemplateColumn HeaderText="Off-Signer">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkRelievee" CommandName="SELECTROW" CommandArgument='<%# Container.DataSetIndex %>'
                                                            runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                                        <asp:Label ID="lblCrewPlanId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWPLANID") %>'></asp:Label>
                                                        <asp:Label ID="lblName" runat="server" Visible="false" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Rank">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmployeeId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'
                                                            Visible="false">
                                                        </asp:Label>
                                                        <asp:Label ID="lblRankId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'
                                                            Visible="false">
                                                        </asp:Label>
                                                        <asp:Label ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'
                                                            Visible="false">
                                                        </asp:Label>
                                                        <asp:Label ID="lblVesseltype" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>'
                                                            Visible="false">
                                                        </asp:Label>
                                                        <asp:Label ID="lblTrainingmatrixid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGMATRIXID") %>'></asp:Label>
                                                        <asp:Label ID="lblJoinDate" runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE", "{0:dd/MMM/yyyy}") %>'
                                                            Visible="false">
                                                        </asp:Label>
                                                        <asp:Label ID="lnkRank" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'>
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Nationality">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblOffsignerNationality" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNATIONALITY") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Wages">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDPRate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDONBOARDEMPWAGES")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Sign on Date">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDateJoined" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERJOINDATE")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="End of Contract">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReliefDue" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Reliever">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRelieverId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERID") %>'
                                                            Visible="false">
                                                        </asp:Label>
                                                        <asp:LinkButton ID="lnkReliever" CommandName="SELECTROW" CommandArgument='<%# Container.DataSetIndex %>'
                                                            runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNAME") %>' Visible="false" Enabled="false"></asp:LinkButton>
                                                        <asp:Label ID="lblRelieverName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNAME") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Nationality">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRelieverNationality" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNATIONALITY") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Rank">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblRelieverRankId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERRANKID") %>'
                                                            Visible="false">
                                                        </asp:Label>
                                                        <asp:Label ID="RadLabel14" runat="server" Text=' <%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERRANK") %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Wages">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDPRate1" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTOTALAMOUNT")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Planned Date">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblReliefDue1" Visible="false" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE")) %>'></asp:Label>
                                                        <asp:Label ID="lblJoiningDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDEXPECTEDJOINDATE")) %>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Joining Port">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container, "DataItem.FLDSEAPORTNAME")%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Remarks">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPDStatusID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUSID")%>'></asp:Label>
                                                        <asp:Label ID="lblRemarks" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS").ToString().Length>20 ? HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS").ToString()).ToString().Substring(0, 20)+ "..." : HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS").ToString()) %>'></asp:Label>
                                                        <asp:LinkButton ID="imgRemarks" runat="server" CommandArgument='<%# Container.DataSetIndex %>'>
                                                           <span class="icon"><i class="fas fa-glasses"></i></span>
                                                        </asp:LinkButton>
                                                        <eluc:ToolTip ID="ucToolTipAddress" Width="200px" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPROPOSALREMARKS") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Employee Status">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPDStatus" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPDSTATUS")%>'></asp:Label>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                                            <Resizing AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td style="width: 100%; vertical-align: top;">
                        <telerik:RadDockZone ID="rdSignoff" runat="server" MinHeight="240px" Orientation="Vertical" Docks="RadDock1,RadDock2,RadDock3" FitDocks="true">
                            <telerik:RadDock ID="RadDock3" RenderMode="Lightweight" runat="server" Title="Sign on/Sign Off"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar" Height="240px"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton
                                                            ID="lnksign"
                                                            runat="server"
                                                            Style="text-decoration: underline; color: black;"
                                                            Text="Sign on/Sign Off">
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnksignInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                    </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnksignComments" runat="server" ToolTip="Comments">
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
                                    <telerik:RadGrid ID="gvSignoff" runat="server" ShowHeader="true" ShowFooter="false" AutoGenerateColumns="false"
                                        GridLines="None" BorderStyle="None" Height="400px"
                                        OnNeedDataSource="gvSignoff_NeedDataSource" OnItemDataBound="gvSignoff_ItemDataBound">
                                        <MasterTableView ShowHeader="true" TableLayout="Fixed">
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
                                                <telerik:GridTemplateColumn HeaderText="Off-Signer">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblSignoffName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Rank">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblSignoffRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERRANKNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Nationality">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblSignoffNationality" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNATIONALITY") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Wages">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblSignoffWages" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERWAGES") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Sign off Date">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblSignoffdate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERSIGNOFFDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Port">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblSignoffPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERSEAPORTNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Reason for Sign Off">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <HeaderStyle Wrap="true" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblSignoffReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERREASON") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="On-Signer">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Rank">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERRANKNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Nationality">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERNATIONALITY") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Wages">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblWages" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERWAGES") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Sign on Date">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblSignondate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDONSIGNERSIGNONDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Port">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERSEAPORTNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Documents">
                                                    <ItemStyle HorizontalAlign="Left" />
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDocument" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERDOCUMENTS") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" />
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
