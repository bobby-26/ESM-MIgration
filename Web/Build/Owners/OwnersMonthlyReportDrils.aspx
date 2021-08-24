<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMonthlyReportDrils.aspx.cs" MaintainScrollPositionOnPostback="true" Inherits="OwnersMonthlyReportDrils" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
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
                        <telerik:RadDockZone ID="rddrildue" runat="server" Orientation="Vertical" MinHeight="320px" Docks="RadDock1,RadDock2,RadDock3,RadDock4" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock1" RenderMode="Lightweight" runat="server" Height="320px"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton
                                                            ID="lnkDrillDue"
                                                            runat="server"
                                                            Style="text-decoration: underline; color: black;"
                                                            Text="Drill Due">
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkDrillDueInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                    </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkDrillDueComments" runat="server" ToolTip="Comments">
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
                                    <telerik:RadGrid ID="gvdrildue" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvdrildue_NeedDataSource">
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
                                                <telerik:GridTemplateColumn HeaderText="Drill" HeaderStyle-Width="50%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRILLNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Due On" HeaderTooltip="60 Days Due" HeaderStyle-Wrap="true" HeaderStyle-Width="25%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lbldue" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDRILLDUEDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Overdue By" HeaderTooltip="Overdue" HeaderStyle-Wrap="true" HeaderStyle-Width="25%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lbloverdue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERDUE") %>'></telerik:RadLabel>
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
                    <td style="width: 50%; vertical-align: top;">
                        <telerik:RadDockZone ID="rdTrainingDue" runat="server" Orientation="Vertical" MinHeight="320px" Docks="RadDock1,RadDock2,RadDock3,RadDock4" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock4" RenderMode="Lightweight" runat="server"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar" Height="320px"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel2">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton
                                                            ID="lnkTrainingDue"
                                                            runat="server"
                                                            Style="text-decoration: underline; color: black;"
                                                            Text="Training Due">
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkTrainingDueInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                    </asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:LinkButton ID="lnkTrainingDueComments" runat="server" ToolTip="Comments">
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
                                    <telerik:RadGrid ID="gvTrainingDue" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvTrainingDue_NeedDataSource">
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
                                                <telerik:GridTemplateColumn HeaderText="Training" HeaderStyle-Width="50%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Due On" HeaderTooltip="60 Days Due" HeaderStyle-Wrap="true" HeaderStyle-Width="25%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lbldue" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTRAININGONBOARDDUEDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Overdue By" HeaderTooltip="Overdue" HeaderStyle-Wrap="true" HeaderStyle-Width="25%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lbloverdue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERDUE") %>'></telerik:RadLabel>
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
                    <td style="width: 50%; vertical-align: top;">
                        <telerik:RadDockZone ID="rdDrill" runat="server" Orientation="Vertical" MinHeight="320px" Docks="RadDock1,RadDock2,RadDock3,RadDock4" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock3" RenderMode="Lightweight" runat="server"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar" Height="320px"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel1">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton
                                                            ID="lnkDrillDone"
                                                            runat="server"
                                                            Style="text-decoration: underline; color: black;"
                                                            Text="Drill Done">
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkDrillDoneInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                    </asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        &nbsp;<asp:LinkButton ID="lnkDrillDoneComments" runat="server" ToolTip="Comments">
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
                                    <telerik:RadGrid ID="gvDrill" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None" OnItemDataBound="gvDrill_ItemDataBound"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvDrill_NeedDataSource">
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
                                                <telerik:GridTemplateColumn HeaderText="Drill" HeaderStyle-Width="50%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRILLNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Done Date" HeaderTooltip="60 Days Due" HeaderStyle-Wrap="true" HeaderStyle-Width="25%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
<telerik:RadLabel ID="radid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDRILLSCHEDULEID") %>' Visible="false"></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lbldue" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDRILLLASTDONEDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Next Due" HeaderTooltip="Overdue" HeaderStyle-Wrap="true" HeaderStyle-Width="25%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lbloverdue" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDRILLDUEDATE")) %>'></telerik:RadLabel>
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
                    <td style="width: 50%; vertical-align: top;">
                        <telerik:RadDockZone ID="rdTraining" runat="server" Orientation="Vertical" MinHeight="320px" Docks="RadDock1,RadDock2,RadDock3,RadDock4" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock2" RenderMode="Lightweight" runat="server" Title="Training Done" Height="320px"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel4">
                                        <ContentTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton
                                                            ID="lnkTrainingDone"
                                                            runat="server"
                                                            Style="text-decoration: underline; color: black;"
                                                            Text="Training Done">
                                                        </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkTrainingDoneInfo" runat="server" ToolTip="Info">
                                                   <span class="icon"><i class="fas fa-info-circle" style="color:black;"></i></span>
                                                    </asp:LinkButton>
                                                    </td>
                                                    <td>&nbsp;<asp:LinkButton ID="lnkTrainingDoneComments" runat="server" ToolTip="Comments">
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
                                    <telerik:RadGrid ID="gvTraining" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvTraining_NeedDataSource"
                                         OnItemDataBound="gvTraining_ItemDataBound">
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
                                                <telerik:GridTemplateColumn HeaderText="Training" HeaderStyle-Width="50%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblmeasure" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Done Date" HeaderTooltip="60 Days Due" HeaderStyle-Wrap="true" HeaderStyle-Width="25%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="radTid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAININGONBOARDSCHEDULEID") %>' Visible="false"></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lbldue" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTRAININGONBOARDLASTDONEDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Next Due" HeaderTooltip="Overdue" HeaderStyle-Wrap="true" HeaderStyle-Width="25%" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lbloverdue" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTRAININGONBOARDDUEDATE")) %>'></telerik:RadLabel>
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
