<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMonthlyReportPMSSummary.aspx.cs" Inherits="OwnersMonthlyReportPMSSummary" %>


<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.1//EN" "http://www.w3.org/TR/xhtml11/DTD/xhtml11.dtd">

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
        <%: Scripts.Render("~/bundles/js") %>
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
            background-color:rgb(194, 220, 252) !important;
            background-image: linear-gradient(rgb(244, 248, 250), rgb(233, 242, 251) 50%, rgb(221, 231, 245) 50%, rgb(228, 237, 248)) !important;
            color: black !important;
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
                        <telerik:RadDockZone ID="rdpmsSummary" runat="server" MinHeight="650px" Orientation="Vertical" Docks="RadDock1,RadDock2" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock1" RenderMode="Lightweight" runat="server" Height="650px" Title="Summary"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate>
                                    <asp:UpdatePanel runat="server" ID="UpdatePanel3">
                                        <ContentTemplate>
                                            <table width="35%">
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel runat="server" ID="lblSummary" Text="Summary"></telerik:RadLabel>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkSummaryComments" runat="server" ToolTip="Comments">
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
                                    <telerik:RadGrid ID="gvSummary" runat="server" AutoGenerateColumns="false" ShowHeader ="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None" Height="600px"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvSummary_NeedDataSource"
                                        OnItemDataBound="gvSummary_ItemDataBound" Width="100%">
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
                                        <ClientSettings ReorderColumnsOnClient="true" AllowColumnsReorder="true" ColumnsReorderMethod="Reorder">
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                            <Resizing AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                    <td style="width: 60%; vertical-align: top;">
                        <telerik:RadDockZone ID="rdRunningHours" runat="server" MinHeight="650px" Orientation="Vertical" Docks="RadDock1,RadDock2" Height="100%" FitDocks="true">
                            <telerik:RadDock ID="RadDock2" RenderMode="Lightweight" runat="server" Height="650px" Title="Running Hours"
                                EnableAnimation="true" EnableDrag="true" DockMode="Docked" DockHandle="TitleBar"
                                EnableRoundedCorners="true" Resizable="false" CssClass="higherZIndex" Width="100%">
                                <TitlebarTemplate >
                                    <asp:UpdatePanel runat="server" id="UpdatePanel1">
                                      <ContentTemplate>
                                          <table width="30%">
                                                <tr>
                                                    <td>
                                                        <asp:LinkButton                                            
                                            id="lnkRunning"
                                            runat="server"                                                                                  
                                            style="text-decoration:underline;
                                            color: black;"
                                            Text="Running Hours">
                                        </asp:LinkButton>
                                                    </td>
                                                    <td>
                                                        <asp:LinkButton ID="lnkRunningHoursComments" runat="server" ToolTip="Comments">
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
                                    <telerik:RadGrid ID="gvRunningHours" runat="server" AutoGenerateColumns="false"
                                        AllowSorting="false" GroupingEnabled="false" BorderStyle="None" Height="600px"
                                        EnableHeaderContextMenu="true" AllowPaging="false" PageSize="1000" OnNeedDataSource="gvRunningHours_NeedDataSource">
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
                                                <telerik:GridTemplateColumn HeaderText="Component No." ColumnGroupName="ComponentNo" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNUMBER" HeaderStyle-Width="20%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblComponentID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblCounterID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTERID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblPartNumber" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Component Name" ColumnGroupName="ComponentName" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNAME" HeaderStyle-Width="50%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPartName" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Value" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="15%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container,"DataItem.FLDCURRENTVALUE").ToString().Replace(".00",string.Empty) %>
                                                    </ItemTemplate>                                                    
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Date Read" HeaderStyle-Width="15%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblReadingDate" runat="server" Visible="True" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDREADINGDATE")) %>'></telerik:RadLabel>
                                                    </ItemTemplate>                                                    
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" Visible="false">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                                            ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Save"
                                                            CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                                            ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                                            CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                                            ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                                        </asp:LinkButton>
                                                    </EditItemTemplate>
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
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
