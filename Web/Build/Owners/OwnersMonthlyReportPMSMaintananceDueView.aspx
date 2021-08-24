<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMonthlyReportPMSMaintananceDueView.aspx.cs" Inherits="OwnersMonthlyReportPMSMaintananceDueView" %>

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
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvMaintenanceDue.ClientID %>"));
                }, 200);
           }
        </script> 
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <table style="width: 100%" runat="server">
                <tr>
                    <td style="width: 100%;">
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvMaintenanceDue" runat="server" GridLines="None" OnNeedDataSource="gvMaintenanceDue_NeedDataSource"
                            AutoGenerateColumns="false" OnItemDataBound="gvMaintenanceDue_ItemDataBound"
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
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
    </form>
</body>
</html>
