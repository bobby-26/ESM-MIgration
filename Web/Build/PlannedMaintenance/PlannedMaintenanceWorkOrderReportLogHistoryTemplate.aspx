<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderReportLogHistoryTemplate.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderReportLogHistoryTemplate" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reporting Templates</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmWorkOrderReportLogGeneral" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource" OnPreRender="RadGrid1_PreRender"
            OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" Height="10px">
                <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="S.No">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSnumberG2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Form Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDFORMNAME">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFormName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDFORMNAME"]%>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblReportId" Visible="false" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDREPORTID"]%>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblFormId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDFORMID"] %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblFormtype" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDFORMTYPE"] %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblComponentIdGV2" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblWorkOrderIdGV2" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Report Date" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDREPORTDATE">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblReportDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDREPORTDATE"))%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Reported By" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCHIEFENGINEERNAME">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblChiefEngineerName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCHIEFENGINEERNAME"]%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <ItemTemplate>
                            <telerik:RadImageButton runat="server" AlternateText="Save" Image-Url="<%$ PhoenixTheme:images/download_1.png %>"
                                CommandName="DOWNLOAD" CommandArgument='<%# Container.DataItem %>' ID="cmdDownload"
                                ToolTip="DownLoad">
                            </telerik:RadImageButton>
                            <asp:LinkButton runat="server" AlternateText="Excel" ID="cmdExcel"
                                CommandName="EXCEL" CommandArgument='<%# Container.DataItem %>' ToolTip="Excel">
                                <span class="icon"><i class="far fa-file-excel"></i></span>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="false" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" ScrollHeight="" UseStaticHeaders="false" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <eluc:Status runat="server" ID="ucStatus" />
    </form>
</body>
</html>
