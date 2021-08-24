<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentHistoryTemplate.aspx.cs"
    Inherits="PlannedMaintenanceComponentHistoryTemplate" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ucVessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="HistoryTemplate" Src="~/UserControls/UserControlHistoryTemplateList.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report Template</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
        function PaneResized(sender, args) {
            var browserHeight = $telerik.$(window).height();
            var grid = $find("gvComponentHistoryTemplateReports");           
            grid._gridDataDiv.style.height = (browserHeight - 130) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
    </telerik:RadCodeBlock>

</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="formPMComponentHistoryTemplateReports" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvComponentHistoryTemplateReports">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvComponentHistoryTemplateReports" LoadingPanelID="RadAjaxLoadingPanel" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table style="width:100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFomrName" runat="server" Text="Form Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:HistoryTemplate ID="ucHistory" runat="server" AppendDataBoundItems="true" Width="100%" />
                    </td>
                    <%--<tr id="vesselrow" runat="server" visible="false">--%>
                    <td id="vesselrow" runat="server" visible="false">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:ucVessel ID="ucVessel" runat="server" CssClass="input" AppendDataBoundItems="true"
                            AssignedVessels="true" Width="80%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFrom" runat="server" Text="From "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTo" runat="server" Text="To "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtworkorderid" CssClass="input" Visible="false" runat="server"
                            Text="" Width="80%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuComponentHistoryTemplateReports" runat="server" OnTabStripCommand="MenuComponentHistoryTemplateReports_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvComponentHistoryTemplateReports" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvComponentHistoryTemplateReports_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvComponentHistoryTemplateReports_NeedDataSource" OnPreRender="gvComponentHistoryTemplateReports_PreRender"
                OnItemDataBound="gvComponentHistoryTemplateReports_ItemDataBound">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S No.">
                            <HeaderStyle Width="58px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20px"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Form Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDFORMNAME">
                            <HeaderStyle Width="760px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFormName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblReportId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFormId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFormtype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblActiveyn" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDACTIVEYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Report Date" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDREPORTDATE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDREPORTDATE"))%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reported By">
                            <HeaderStyle Width="134px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChiefEngineerNameItem" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCHIEFENGINEERNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle Width="50px" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="DOWNLOAD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDownload"
                                    ToolTip="DownLoad" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fa fa-download"></i></span>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
