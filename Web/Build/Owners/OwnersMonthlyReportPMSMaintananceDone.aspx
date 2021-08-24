<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMonthlyReportPMSMaintananceDone.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="OwnersMonthlyReportPMSMaintananceDone" %>

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
        <%: Scripts.Render("~/bundles/js") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/OwnersReport.css" rel="stylesheet" />
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <table style="width: 100%" runat="server">
            <tr>
                <td style="width: 100%;">
                    <telerik:RadDockZone ID="rdMaintenance" runat="server" Orientation="Vertical" MinHeight="300px" Docks="RadDock1,RadDock2,RadDock3" Height="100%" FitDocks="true">
                        <telerik:RadDock ID="RadDock1" RenderMode="Lightweight" runat="server" AutoPostBack="false"
                            EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                            EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                            <TitlebarTemplate>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                    <ContentTemplate>
                                        <table width="20%">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton
                                                        ID="lnkMADone"
                                                        runat="server"
                                                        Style="text-decoration: underline; color: black;"
                                                        Text="Maintenance Done">
                                                    </asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkMADoneComments" runat="server" ToolTip="Comments">
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
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvMaintenance" runat="server" GridLines="None" OnNeedDataSource="gvMaintenance_NeedDataSource"
                                    Height="300px" AutoGenerateColumns="false" OnItemDataBound="gvMaintenance_ItemDataBound"
                                    GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000"
                                    ShowFooter="false" ShowHeader="true" EnableViewState="false">
                                    <MasterTableView TableLayout="Fixed">
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                        <HeaderStyle Width="102px" />
                                        <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Job Number" HeaderStyle-Width="10%">
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Job Code & Title" HeaderStyle-Width="30%">
                                                <HeaderStyle />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblTitle" runat="server" CommandName="Select" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>' Visible="false"></telerik:RadLabel>
                                                    <asp:LinkButton ID="lnkTitle" runat="server" CommandName="Select" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Component No.">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle Width="10%" />
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Component Name">
                                                <HeaderStyle Width="15%" />
                                                <ItemStyle Width="15%" />
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Priority" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDPLANINGPRIORITY"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Frequency">
                                                <HeaderStyle Width="15%" />
                                                <ItemStyle Width="15%" />
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDFREQUENCYNAME"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Is Defect Job" Visible="false">
                                                <HeaderStyle Width="70px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDJOBDONESTATUS"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Done Date">
                                                <HeaderStyle Width="10%" />
                                                <ItemTemplate>
                                                    <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDWORKDONEDATE"])%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Category">
                                                <HeaderStyle Width="10%" />
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDJOBCATEGORY"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Done By" Visible="false">
                                                <HeaderStyle Width="180px" />
                                                <ItemStyle Width="180px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDREPORTBY"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Remarks" Visible="false">
                                                <HeaderStyle Width="300px" />
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <%# General.SanitizeHtml(((DataRowView)Container.DataItem)["FLDREMARKS"].ToString())%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Action" Visible="false" AllowFiltering="false" AllowSorting="false">
                                                <ItemTemplate>
                                                    <asp:LinkButton runat="server" AlternateText="MaintenanceLog"
                                                        CommandName="MAINTENANCEFORM" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRTemplates"
                                                        ToolTip="Reporting Templates" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-file"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton runat="server" AlternateText="Parameters"
                                                        CommandName="PARAMETERS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdParameters"
                                                        ToolTip="Parameters" Width="20px" Height="20px">
                               <span class="icon"><i class="fas fa-newspaper"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:ImageButton runat="server" ID="cmdAttachments" ToolTip="Attachment"
                                                        CommandName="ATTACHMENTS" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />
                                                    <asp:LinkButton runat="server" AlternateText="Parts"
                                                        CommandName="PARTS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdParts"
                                                        ToolTip="Parts" Width="20px" Height="20px">
                                <span class="icon"><i class="fas fa-cogs"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton runat="server" AlternateText="RA History"
                                                        CommandName="RA" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRA"
                                                        ToolTip="RA" Width="20px" Height="20px">
                                <span class="icon"><i class="fas fa-eye"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton runat="server" AlternateText="PTW History"
                                                        CommandName="PTW" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPTW"
                                                        ToolTip="PTW" Width="20px" Height="20PX">
                                <span class="icon"><i class="fas fa-copy"></i></span>
                                                    </asp:LinkButton>

                                                    <asp:LinkButton runat="server" AlternateText="Report" ID="cmdReschedule" CommandName="RESCHEDULE" ToolTip="Postpone History">
                                 <span class="icon"><i class="far fa-calendar-alt"></i></span>
                                                    </asp:LinkButton>
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
            <tr>
                <td style="width: 100%;">
                    <telerik:RadDockZone ID="rdMaintenanceDue" runat="server" Orientation="Vertical" MinHeight="300px" Docks="RadDock1,RadDock2,RadDock3" Height="100%" FitDocks="true">
                        <telerik:RadDock ID="RadDock2" RenderMode="Lightweight" runat="server" AutoPostBack="false"
                            EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                            EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                            <TitlebarTemplate>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                    <ContentTemplate>
                                        <table width="20%">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton
                                                        ID="lnkMADue"
                                                        runat="server"
                                                        Style="text-decoration: underline; color: black;"
                                                        Text="Maintenance Due">
                                                    </asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkMADueComments" runat="server" ToolTip="Comments">
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
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvMaintenanceDue" runat="server" GridLines="None" OnNeedDataSource="gvMaintenanceDue_NeedDataSource"
                                    Height="300px" AutoGenerateColumns="false" OnItemDataBound="gvMaintenanceDue_ItemDataBound"
                                    GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000"
                                    ShowFooter="false" ShowHeader="true" EnableViewState="false">
                                    <MasterTableView TableLayout="Fixed">
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                        <HeaderStyle Width="102px" />
                                        <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Work Order No." HeaderStyle-Width="10%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblGroupId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERGROUPID") %>' Visible="false"></telerik:RadLabel>
                                                    <asp:LinkButton ID="lnkGroupNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERGROUPNO") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Job Code & Title" HeaderStyle-Width="30%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnktitle" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERNAME"] %>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"] + " - "+ ((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Component No." HeaderStyle-Width="10%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblRespId" runat="server" Visible="false" Text=' <%#((DataRowView)Container.DataItem)["FLDPLANNINGDISCIPLINE"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblWorkOrderId" runat="server" Visible="false" Text=' <%#((DataRowView)Container.DataItem)["FLDWORKORDERID"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblCategoryId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDCATEGORYID"]%>'></telerik:RadLabel>
                                                    <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Component Name" HeaderStyle-Width="15%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Priority" HeaderStyle-HorizontalAlign="Center">
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                <HeaderStyle Width="10%" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANINGPRIORITY") %>' ID="lblPriority"></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Frequency" HeaderStyle-Width="15%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDFREQUENCYNAME"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Due On">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblDue" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="10%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <%# ((DataRowView)Container.DataItem)["FLDJOBCATEGORY"] %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Responsibility" Visible="false">
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Last Done On" Visible="false">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                                <HeaderStyle Width="90px" />
                                                <ItemTemplate>
                                                    <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Status" Visible="false">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                                <HeaderStyle Width="100px" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblStaus" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERSTATUS")%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="" Visible="false">
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container,"DataItem.FLDDEFECT") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Action" Visible="false" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkFind" runat="server" ToolTip="Find Related" Text="Find Related" CommandName="FIND">
                                <span class="icon"><i class="fas fa-search"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton runat="server" AlternateText="Report" ID="cmdReschedule" CommandName="RESCHEDULE" ToolTip="Postpone Job">
                                 <span class="icon"><i class="far fa-calendar-alt"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton runat="server" AlternateText="Docking" ID="cmdDocking" CommandName="DOCKING" ToolTip="Add To Drydock">
                                 <span class="icon"><i class="fas fa-docker"></i></span>
                                                    </asp:LinkButton>
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
            <tr>
                <td style="width: 100%;">
                    <telerik:RadDockZone ID="rdPostponementJob" runat="server" Orientation="Vertical" MinHeight="300px" Docks="RadDock1,RadDock2,RadDock3" Height="100%" FitDocks="true">
                        <telerik:RadDock ID="RadDock3" RenderMode="Lightweight" runat="server" AutoPostBack="false"
                            EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                            EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                            <TitlebarTemplate>
                                <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                    <ContentTemplate>
                                        <table width="20%">
                                            <tr>
                                                <td>
                                                    <asp:LinkButton
                                                        ID="lnkMAPost"
                                                        runat="server"
                                                        Style="text-decoration: underline; color: black;"
                                                        Text="Postponed Jobs">
                                                    </asp:LinkButton>
                                                </td>
                                                <td>
                                                    <asp:LinkButton ID="lnkMAPostComments" runat="server" ToolTip="Comments">
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
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvPostponementJob" runat="server" GridLines="None" OnNeedDataSource="gvPostponementJob_NeedDataSource"
                                    Height="300px" AutoGenerateColumns="false" OnItemDataBound="gvPostponementJob_ItemDataBound"
                                    GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000"
                                    ShowFooter="false" ShowHeader="true" EnableViewState="false">
                                    <MasterTableView TableLayout="Fixed">
                                        <NoRecordsTemplate>
                                            <table width="100%" border="0">
                                                <tr>
                                                    <td align="center">
                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </NoRecordsTemplate>
                                        <HeaderStyle Width="102px" />
                                        <CommandItemSettings RefreshText="Search" ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                        <Columns>
                                            <telerik:GridTemplateColumn HeaderText="Work Order No." HeaderStyle-Width="10%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblGroupId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERGROUPID") %>' Visible="false"></telerik:RadLabel>
                                                    <asp:LinkButton ID="lnkGroupNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERGROUPNO") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Job Code & Title" HeaderStyle-Width="30%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnktitle" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERNAME"] %>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"] + " - "+ ((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Component No." HeaderStyle-Width="10%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblRespId" runat="server" Visible="false" Text=' <%#((DataRowView)Container.DataItem)["FLDPLANNINGDISCIPLINE"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblWorkOrderId" runat="server" Visible="false" Text=' <%#((DataRowView)Container.DataItem)["FLDWORKORDERID"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblCategoryId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDCATEGORYID"]%>'></telerik:RadLabel>
                                                    <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Component Name" HeaderStyle-Width="15%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Priority" HeaderStyle-HorizontalAlign="Center">
                                                <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                <HeaderStyle Width="10%" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANINGPRIORITY") %>' ID="lblPriority"></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Frequency" HeaderStyle-Width="15%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDFREQUENCYNAME"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Due On">
                                                <HeaderStyle Width="10%" />
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblDue" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="10%">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <ItemTemplate>
                                                    <%# ((DataRowView)Container.DataItem)["FLDJOBCATEGORY"] %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Responsibility" Visible="false">
                                                <ItemTemplate>
                                                    <%#((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Last Done On" Visible="false">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                                                <HeaderStyle Width="90px" />
                                                <ItemTemplate>
                                                    <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Status" Visible="false">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                                <HeaderStyle Width="100px" />
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblStaus" RenderMode="Lightweight" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERSTATUS")%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="" Visible="false">
                                                <HeaderStyle Width="100px" />
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                                <ItemTemplate>
                                                    <%# DataBinder.Eval(Container,"DataItem.FLDDEFECT") %>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Action" Visible="false" HeaderStyle-HorizontalAlign="Center" AllowFiltering="false">
                                                <ItemStyle HorizontalAlign="Center" />
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkFind" runat="server" ToolTip="Find Related" Text="Find Related" CommandName="FIND">
                                <span class="icon"><i class="fas fa-search"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton runat="server" AlternateText="Report" ID="cmdReschedule" CommandName="RESCHEDULE" ToolTip="Postpone Job">
                                 <span class="icon"><i class="far fa-calendar-alt"></i></span>
                                                    </asp:LinkButton>
                                                    <asp:LinkButton runat="server" AlternateText="Docking" ID="cmdDocking" CommandName="DOCKING" ToolTip="Add To Drydock">
                                 <span class="icon"><i class="fas fa-docker"></i></span>
                                                    </asp:LinkButton>
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
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
    </form>
</body>
</html>
