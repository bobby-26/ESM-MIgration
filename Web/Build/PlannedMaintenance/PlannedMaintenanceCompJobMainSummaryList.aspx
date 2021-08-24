<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceCompJobMainSummaryList.aspx.cs"
    Inherits="PlannedMaintenanceCompJobMainSummaryList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Work order</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvWorkOrder.ClientID %>"));
                }, 200);
            }
            function CloseUrlModelWindow() {
                var wnd = $find('<%=RadWindow_NavigateUrl.ClientID %>');
                wnd.close();
                var masterTable = $find('<%=gvWorkOrder.ClientID %>').get_masterTableView();
                masterTable.rebind();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onload="Resize();" onresize="Resize();">
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" EnableAJAX="true">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuWorkOrder" runat="server"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvWorkOrder" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid ID="gvWorkOrder" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnItemDataBound="gvWorkOrder_ItemDataBound"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvWorkOrder_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                EnableViewState="true" OnItemCommand="gvWorkOrder_ItemCommand1" OnSortCommand="gvWorkOrder_SortCommand"
                EnableHeaderContextMenu="true" EnableLinqExpressions="false">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDCOMPJOBSUMMARYID" ClientDataKeyNames="FLDCOMPJOBSUMMARYID" AllowFilteringByColumn="true">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="45px" HeaderText="S.No." AllowSorting="false" AllowFiltering="false" ItemStyle-Width="45px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" AllowSorting="true" FilterDelay="3000" ShowSortIcon="true" SortExpression="FLDTITLE"
                            ShowFilterIcon="false" CurrentFilterFunction="Contains" UniqueName="FLDTITLE">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtTitleEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Created by" AllowSorting="false" AllowFiltering="false" HeaderStyle-Width="150px" UniqueName="FLDCREATEDBYNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCreatedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDBYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date Created" AllowSorting="true" ShowFilterIcon="false" HeaderStyle-Width="150px" UniqueName="FLDCREATEDDATE" ShowSortIcon="true" SortExpression="FLDCREATEDDATE">
                            <FilterTemplate>
                                From<telerik:RadDatePicker ID="FromOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="FromDateSelected"
                                    DbSelectedDate='<%# ViewState["FDATE"].ToString() %>' />
                                <br />
                                To&nbsp&nbsp&nbsp&nbsp<telerik:RadDatePicker ID="ToOrderDatePicker" runat="server" Width="85%" ClientEvents-OnDateSelected="ToDateSelected"
                                    DbSelectedDate='<%# ViewState["TDATE"].ToString() %>' />
                                <telerik:RadScriptBlock ID="RadScriptBlock1" runat="server">
                                    <script type="text/javascript">
                                        function FromDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var ToPicker = $find('<%# ((GridItem)Container).FindControl("ToOrderDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(sender);
                                            var toDate = FormatSelectedDate(ToPicker);

                                            tableView.filter("FLDCREATEDDATE", fromDate + "~" + toDate, "Between");

                                        }
                                        function ToDateSelected(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            var FromPicker = $find('<%# ((GridItem)Container).FindControl("FromOrderDatePicker").ClientID %>');

                                            var fromDate = FormatSelectedDate(FromPicker);
                                            var toDate = FormatSelectedDate(sender);

                                            tableView.filter("FLDCREATEDDATE", fromDate + "~" + toDate, "Between");
                                        }
                                        function FormatSelectedDate(picker) {
                                            var date = picker.get_selectedDate();
                                            var dateInput = picker.get_dateInput();
                                            var formattedDate = dateInput.get_dateFormatInfo().FormatDate(date, dateInput.get_displayDateFormat());

                                            return formattedDate;
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCreatedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" AllowSorting="false" AllowFiltering="false" HeaderStyle-Width="100px" UniqueName="FLDTYPE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel’s Applicable" AllowSorting="false" AllowFiltering="false" HeaderStyle-Width="120px" UniqueName="FLDVESSELNAMELIST" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <telerik:RadToolTip ID="ttvessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAMELIST") %>' TargetControlID="LnkVessel" HideEvent="ManualClose"></telerik:RadToolTip>
                                <asp:LinkButton runat="server" ID="LnkVessel">
                                    <span class="icon"><i class="fas fa-ship"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="publish" ID="cmdPublish" CommandName="PUBLISH" ToolTip="Publish" >
                                    <span class="icon"><i class="fas fa-passport"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <telerik:RadWindow runat="server" ID="RadWindow_NavigateUrl" Width="900px" Height="365px"
            Modal="true" OffsetElementID="main" VisibleStatusbar="false" KeepInScreenBounds="true" ReloadOnShow="true" ShowContentDuringLoad="false">
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server">
            <script type="text/javascript">
                Sys.Application.add_load(function () {
                    setTimeout(function () {
                        TelerikGridResize($find("<%= gvWorkOrder.ClientID %>"));
                    }, 200);
                });
                $modalWindow.modalWindowID = "<%=RadWindow_NavigateUrl.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
