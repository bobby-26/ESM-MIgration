<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderRAList.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderRAList" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Risk Assessment</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
        function PaneResized(sender, args) {

                var browserHeight = $telerik.$(window).height();
                var grid = $find("RadGrid1");
                var height = (browserHeight - 110);
                grid._gridDataDiv.style.height = height + "px";
                
            }
            function pageLoad() {
                PaneResized();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmgvUsesParts" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <asp:UpdatePanel runat="server" ID="pnlRAList">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="RadGrid1" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource" OnPreRender="RadGrid1_PreRender"
                    OnItemCommand="RadGrid1_ItemCommand" OnItemDataBound="RadGrid1_ItemDataBound" GroupingEnabled="false" EnableHeaderContextMenu="true">
                    <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="RA Ref.">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblraid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblLocation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREFNO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE")  %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Activity / Conditions">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblWorkActivity" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRISKASSESSMENT").ToString().Length > 30 ? DataBinder.Eval(Container, "DataItem.FLDRISKASSESSMENT").ToString().Substring(0, 30) + "..." : DataBinder.Eval(Container, "DataItem.FLDRISKASSESSMENT").ToString() %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucWorkActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKASSESSMENT") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCreated" runat="server">Created Date</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDCREATEDDATE"]) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Approved Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDAPPROVEDDATE"]) %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Approved by">
                                <ItemStyle Wrap="False" HorizontalAlign="left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblApprovedby" runat="server" Visible="True" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEBY") %>'></asp:Label>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="RAjob" CssClass="imgbtn-height" runat="server" AlternateText="Risk Assessment" ImageUrl="<%$ PhoenixTheme:images/BarChart.png %>"
                                        CommandName="RAJOB" CommandArgument='<%# Container.DataItem %>' ToolTip="Risk Assessment">
                                    </asp:ImageButton>
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
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="false" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="150px" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
