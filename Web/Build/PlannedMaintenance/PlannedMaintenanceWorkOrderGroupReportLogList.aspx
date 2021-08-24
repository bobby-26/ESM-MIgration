<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderGroupReportLogList.aspx.cs" 
    Inherits="PlannedMaintenanceWorkOrderGroupReportLogList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Work Order Log</title>
    <telerik:RadCodeBlock ID="radCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
     <script type="text/javascript">
        function PaneResized(sender, args) {
            var splitter = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            splitter.set_height(browserHeight - 40);
            splitter.set_width("100%");
            var grid = $find("gvWorkOrder");
            var contentPane = splitter.getPaneById("listPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 150) + "px";
            var genPane = splitter.getPaneById("generalPane");
            document.getElementById('ifMoreInfo').style.height = (genPane._contentElement.offsetHeight) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <asp:UpdatePanel runat="server" ID="pnlComponent">
            <ContentTemplate>
                <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
                    <AjaxSettings>
                        <telerik:AjaxSetting AjaxControlID="gvWorkOrder">
                            <UpdatedControls>
                                <telerik:AjaxUpdatedControl ControlID="gvWorkOrder" />
                                <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" LoadingPanelID="RadAjaxLoadingPanel" />
                                <telerik:AjaxUpdatedControl ControlID="ucError" />
                            </UpdatedControls>
                        </telerik:AjaxSetting>
                    </AjaxSettings>
                </telerik:RadAjaxManager>
                <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
                    <telerik:RadPane ID="generalPane" runat="server">
                        <iframe runat="server" id="ifMoreInfo" style="height: 560px; width: 100%" frameborder="0"></iframe>
                    </telerik:RadPane>
                    <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both"></telerik:RadSplitBar>
                    <telerik:RadPane ID="listPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                        <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkOrder" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Width="100%"
                            CellSpacing="0" GridLines="None" OnNeedDataSource="gvWorkOrder_NeedDataSource" GroupingEnabled="false" EnableHeaderContextMenu="true"
                            OnItemCommand="gvWorkOrder_ItemCommand" DataKeyNames="FLDWORKORDERID" OnItemDataBound="gvWorkOrder_ItemDataBound">
                            <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed">
                                <CommandItemSettings ShowRefreshButton="false" RefreshText="Search" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="">
                                        <HeaderStyle Wrap="false" Width="65px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="Templates" CssClass="imgbtn-height" runat="server" AlternateText="Templates" ImageUrl="<%$ PhoenixTheme:images/te_notes.png %>"
                                                CommandName="TEMPLATE" ToolTip="Template" />
                                            <asp:ImageButton ID="Attachments" CssClass="imgbtn-height" runat="server" ToolTip="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" />
                                            <asp:ImageButton ID="RAjob" CssClass="imgbtn-height" runat="server" AlternateText="Risk Assessment" ImageUrl="<%$ PhoenixTheme:images/BarChart.png %>"
                                                CommandName="RAJOB" ToolTip="Risk Assessment" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Job No." AllowSorting="true" ShowSortIcon="true" SortExpression="FLDWORKORDERNUMBER">
                                        <HeaderStyle Width="111px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblWorkorderNumber" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Title" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDWORKORDERNAME">
                                        <HeaderStyle Width="370px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblWorkOrderId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                            <asp:LinkButton ID="lnkWorkorderName" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDWorkOrderID") %>'
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Component No." AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNUMBER">
                                        <HeaderStyle Width="111px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblComponentNumber" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Component Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNAME">
                                        <HeaderStyle Width="315px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblAttachmentCode" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDATTACHMENTCODE") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblTemplate" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTEMPLATE") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblRaid" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDRAID") %>' Visible="false"></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblRAJob" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDRAJOB") %>' Visible="false"></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Class Code" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCLASSCODE">
                                        <HeaderStyle Width="85px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container,"DataItem.FLDCLASSCODE") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Priority">
                                        <HeaderStyle Width="65px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container,"DataItem.FLDPLANINGPRIORITY") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Frequency">
                                        <HeaderStyle Width="150px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container,"DataItem.FLDFREQUENCYNAME") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Done Date" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDWORKDONEDATE">
                                        <HeaderStyle Width="90px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDoneDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWORKDONEDATE")) %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Job Class" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDJOBCLASS">
                                        <HeaderStyle Width="90px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container,"DataItem.FLDJOBCLASS") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Total Duration" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDWORKDURATION">
                                        <HeaderStyle Width="111px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDuration" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDWORKDURATION") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Started" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDWORKORDERSTARTEDDATE">
                                        <HeaderStyle Width="85px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblStartDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWORKORDERSTARTEDDATE")) %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Completed" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDWORKORDERCOMPLETEDDATE">
                                        <HeaderStyle Width="85px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCompletedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDWORKORDERCOMPLETEDDATE")) %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Report By">
                                        <HeaderStyle Width="150px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%#DataBinder.Eval(Container,"DataItem.FLDREPORTBY") %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>

                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <td>
                                    <img id="Img2" src="<%$ PhoenixTheme:images/attachment.png%>" runat="server" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblDue" runat="server" Text="* Attachment"></telerik:RadLabel>
                                </td>
                                <td>
                                    <img id="Img4" src="<%$ PhoenixTheme:images/te_notes.png%>" runat="server" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblOfficeApproval" runat="server" Text="*Template"></telerik:RadLabel>
                                </td>
                                <td>
                                    <img id="Img5" src="<%$ PhoenixTheme:images/BarChart.png%>" runat="server" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="Literal1" runat="server" Text="*Risk Assessment"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </telerik:RadPane>
                </telerik:RadSplitter>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
