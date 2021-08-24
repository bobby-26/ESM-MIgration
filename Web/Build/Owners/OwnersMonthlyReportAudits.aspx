<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMonthlyReportAudits.aspx.cs" MaintainScrollPositionOnPostback="false" Inherits="OwnersMonthlyReportAudits" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%--        <%: Styles.Render("~/bundles/css") %>--%>
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
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="93%" EnableAJAX="false">
            <table style="width: 100%" runat="server">
                <tr>
                    <td style="width: 50%; vertical-align: top;">
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadDockZone ID="rdauditinspection" runat="server" Orientation="Vertical" MinHeight="300px" Docks="RadDock1,RadDock2,RadDock3,RadDock4,RadDock5" Height="100%" FitDocks="true">
                                        <telerik:RadDock ID="RadDock1" RenderMode="Lightweight" runat="server" Height="300px" Title="Audit / Inspection"
                                            EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                            EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                            <TitlebarTemplate>
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <telerik:RadLabel runat="server" ID="lblAuditInspection" Text="Audit / Inspection"></telerik:RadLabel>
                                                                </td>
                                                                <td>&nbsp;<asp:LinkButton ID="lnkAuditInspectionInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                                </asp:LinkButton>
                                                                </td>
                                                                <td>&nbsp;
                                                                    <asp:LinkButton ID="lnkAuditInspectionComments" runat="server" ToolTip="Comments">
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
                                                <telerik:RadGrid ID="gvInspectionStatus" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" BorderStyle="None"
                                                    EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvInspectionStatus_NeedDataSource"
                                                    OnItemDataBound="gvInspectionStatus_ItemDataBound">
                                                    <MasterTableView TableLayout="Fixed">
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
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                    </telerik:RadDockZone>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadDockZone ID="rddeficiency" runat="server" Orientation="Vertical" MinHeight="300px" Docks="RadDock1,RadDock2,RadDock3,RadDock4,RadDock5" Height="100%" FitDocks="true">
                                        <telerik:RadDock ID="RadDock2" RenderMode="Lightweight" runat="server" Title="Deficiency" Height="300px"
                                            EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                            EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                            <TitlebarTemplate>
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <telerik:RadLabel runat="server" ID="lblDeficiency" Text="Deficiency"></telerik:RadLabel>
                                                                </td>
                                                                <td>&nbsp;<asp:LinkButton ID="lnkDeficiencyInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                                </asp:LinkButton>
                                                                </td>
                                                                <td>&nbsp;  
                                                                    <asp:LinkButton ID="lnkDeficiencyComments" runat="server" ToolTip="Comments">
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
                                                <telerik:RadGrid ID="gvDeficiency" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" BorderStyle="None"
                                                    EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvDeficiency_NeedDataSource"
                                                    OnItemDataBound="gvDeficiency_ItemDataBound">
                                                    <MasterTableView TableLayout="Fixed">
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
                                                            <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="60%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Count" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lnkShip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPCOUNT") %>'></asp:LinkButton>
                                                                    <telerik:RadLabel ID="lblShip" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPURL") %>'></telerik:RadLabel>
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
                                <td>
                                    <telerik:RadDockZone ID="rdtask" runat="server" Orientation="Vertical" MinHeight="300px" Docks="RadDock1,RadDock2,RadDock3,RadDock4,RadDock5" Height="100%" FitDocks="true">
                                        <telerik:RadDock ID="RadDock3" RenderMode="Lightweight" runat="server" Title="Task"
                                            EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar" Height="300px"
                                            EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                            <TitlebarTemplate>
                                                <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                                    <ContentTemplate>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <telerik:RadLabel runat="server" ID="lblTask" Text="Task"></telerik:RadLabel>
                                                                </td>
                                                                <td>&nbsp;<asp:LinkButton ID="lnkTaskInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                                </asp:LinkButton>
                                                                </td>
                                                                <td>&nbsp;  
                                                                    <asp:LinkButton ID="lnkTaskComments" runat="server" ToolTip="Comments">
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
                                                <telerik:RadGrid ID="gvTechTask" runat="server" AutoGenerateColumns="false"
                                                    AllowSorting="false" GroupingEnabled="false" BorderStyle="None"
                                                    EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvTechTask_NeedDataSource"
                                                    OnItemDataBound="gvTechTask_ItemDataBound">
                                                    <MasterTableView TableLayout="Fixed">
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
                                                            <telerik:GridTemplateColumn HeaderText="30 Days" HeaderStyle-Width="12%" HeaderStyle-HorizontalAlign="Center">
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
                                            </ContentTemplate>
                                        </telerik:RadDock>
                                    </telerik:RadDockZone>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 50%; vertical-align: top;">
                        <telerik:RadDockZone ID="rdkpi" runat="server" Orientation="Vertical" MinHeight="930px" Docks="RadDock1,RadDock2,RadDock3,RadDock4,RadDock5" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock4" RenderMode="Lightweight" runat="server" Title="KPI's"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar" Height="930px"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel5">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel runat="server" ID="lblKPIs" Text="KPI's"></telerik:RadLabel>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkKPIsInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                    </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp; 
                                                        <asp:LinkButton ID="lnkKPIsComments" runat="server" ToolTip="Comments">
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

                                    <telerik:RadGrid ID="GVKPI" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None" ShowHeader="false"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="GVKPI_NeedDataSource"
                                        OnItemDataBound="GVKPI_ItemDataBound">
                                        <MasterTableView TableLayout="Fixed">
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
                                                <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="60%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURE") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Count" HeaderStyle-Width="20%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkShip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNT") %>'></asp:LinkButton>
                                                        <telerik:RadLabel ID="lblShip" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>'></telerik:RadLabel>
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
                    <td style="width: 100%; vertical-align: top;" colspan="2">
                        <telerik:RadDockZone ID="rdauditinspectionlog" runat="server" Orientation="Vertical" MinHeight="300px" Docks="RadDock1,RadDock2,RadDock3,RadDock4,RadDock5" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock5" RenderMode="Lightweight" runat="server"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton
                                                            ID="lnkAuditInspection"
                                                            runat="server"
                                                            Style="text-decoration: underline; color: black;"
                                                            Text="Audit / Inspection">
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkAuditInspectionInfo2" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                    </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;
                                                        <asp:LinkButton ID="lnkAuditInspectionComments2" runat="server" ToolTip="Comments">
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
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvAuditRecordList" runat="server" GridLines="None" OnNeedDataSource="gvAuditRecordList_NeedDataSource"
                                        OnSortCommand="gvAuditRecordList_SortCommand" Height="300px" AutoGenerateColumns="false" OnItemDataBound="gvAuditRecordList_ItemDataBound"
                                        GroupingEnabled="false" EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000"
                                        ShowFooter="false" ShowHeader="true" EnableViewState="false">
                                        <MasterTableView TableLayout="Fixed">
                                            <ColumnGroups>
                                                <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                                <telerik:GridColumnGroup Name="AuditInsp" HeaderText="Audit/Inspection" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                                <telerik:GridColumnGroup Name="DeficiencyCount" HeaderText="Def Count" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                            </ColumnGroups>
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
                                                <telerik:GridTemplateColumn HeaderText="Ref. No" HeaderStyle-Width="106px">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblInspectionRefNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER").ToString() %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblInspectionScheduleId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVIEWSCHEDULEID").ToString() %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="95px" HeaderText="Name">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="130px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkInspection" runat="server" CommandName="NAVIGATE" CommandArgument="<%# Container.DataItem %>" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE").ToString().Length>35 ? DataBinder.Eval(Container, "DataItem.FLDSHORTCODE").ToString().Substring(0, 35)+ "..." : DataBinder.Eval(Container, "DataItem.FLDSHORTCODE").ToString() %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderStyle-Width="70PX" HeaderText="Done Date">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblInspectionCompletionDate" runat="server" Text='<%#General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE"))%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="From" HeaderStyle-Width="80px" ColumnGroupName="FromToPort">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblFromPort" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMPORT").ToString() %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="To" HeaderStyle-Width="80px" ColumnGroupName="FromToPort">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblToPort" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOPORT").ToString() %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Auditor" ColumnGroupName="AuditInsp">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="110px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblNameOfInspector" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFINSPECTOR").ToString() %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Organization" HeaderStyle-Width="85px" ColumnGroupName="AuditInsp">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOrganisation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORGANISATION").ToString() %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Total" HeaderStyle-Width="45px" ColumnGroupName="DeficiencyCount" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="60px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDeficiencyCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFICIENCYCOUNT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="MNC" HeaderStyle-Width="43px" ColumnGroupName="DeficiencyCount" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblMajorNCCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAJORNCCOUNT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="NC" HeaderStyle-Width="38px" ColumnGroupName="DeficiencyCount" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblNCCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNCCOUNT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="OBS" HeaderStyle-Width="40px" ColumnGroupName="DeficiencyCount" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblOBSCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOBSCOUNT") %>'></telerik:RadLabel>
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
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
    </form>
</body>
</html>
