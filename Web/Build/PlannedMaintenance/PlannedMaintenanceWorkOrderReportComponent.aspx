<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderReportComponent.aspx.cs"
    Inherits="PlannedMaintenance_PlannedMaintenanceWorkOrderReportComponent" MaintainScrollPositionOnPostback="true" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>  
         <script type="text/javascript">
            function refreshParent() {                
                top.closeTelerikWindow(null,'maint,RadWindow_NavigateUrl');
            }
         </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="formPMHistoryTemplate" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" EnableAJAX="true">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <br />
            <b>
                <telerik:RadLabel ID="lblorder" runat="server" Text="Work Order Templates" Font-Bold="true"></telerik:RadLabel>
            </b>
            <br />
            

                <telerik:RadGrid ID="gvHistoryTemplate" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnItemCommand="gvHistoryTemplate_ItemCommand" Height="40%"
                    OnNeedDataSource="gvHistoryTemplate_NeedDataSource" OnItemDataBound="gvHistoryTemplate_ItemDataBound">
                    <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                        <HeaderStyle Width="102px" />
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="S.No">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSnumberG1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Template Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFormID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblFormType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPE") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkTemplateName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItem %>"></asp:LinkButton>
                                    <telerik:RadLabel ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblComponentId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWorkOrderId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblFormIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtTemplateNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNAME") %>'
                                        CssClass="gridinput_mandatory" MaxLength="200">
                                    </telerik:RadTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Excel" ID="cmdExcel"
                                        CommandName="EXCEL" CommandArgument='<%# Container.DataItem %>' ToolTip="Excel">
                                    <span class="icon"><i class="far fa-file-excel"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                         <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

            

            <br />
            <b>
                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Work Order Reports" Font-Bold="true"></telerik:RadLabel>
            </b>
            <br />
            <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>
            </telerik:RadCodeBlock>
 
            <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkReport" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnItemCommand="gvWorkReport_ItemCommand" Height="45%"
                    OnNeedDataSource="gvWorkReport_NeedDataSource" OnItemDataBound="gvWorkReport_ItemDataBound">
                    <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                        <HeaderStyle Width="102px" />
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="S.No">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSnumberG2" runat="server" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
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
                                    <telerik:RadLabel ID="lblReportDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDREPORTDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reported By" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCHIEFENGINEERNAME">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblChiefEngineerName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCHIEFENGINEERNAME"]%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/download_1.png %>"
                                        CommandName="DOWNLOAD" CommandArgument='<%# Container.DataItem %>' ID="cmdDownload"
                                        ToolTip="DownLoad"></asp:ImageButton>
                                    <asp:LinkButton runat="server" AlternateText="Excel" ID="cmdExcel"
                                        CommandName="EXCEL" CommandArgument='<%# Container.DataItem %>' ToolTip="Excel">
                                    <span class="icon"><i class="far fa-file-excel"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                       <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
           
      </telerik:RadAjaxPanel>
    </form>
</body>
</html>
