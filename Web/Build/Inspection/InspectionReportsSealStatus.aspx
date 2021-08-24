<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionReportsSealStatus.aspx.cs" Inherits="InspectionReportsSealStatus" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seal Status Report</title>
   <telerik:RadCodeBlock runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

   </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSealStatusReport" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

           <br />
            <div id="divFind" runat="server">
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSealType" runat="server" Text="Seal Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucSealType" Width="200px" runat="server"  AppendDataBoundItems="true"
                                QuickTypeCode="87"  />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucStatus" runat="server"  AppendDataBoundItems="true"
                                HardTypeCode="197"  ShortNameFilter="SFO,WMS,ISS,INU,SCR,NRD,DAM" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSealNo" runat="server" Text="Seal Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSealNumber" runat="server" ></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div  style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuSealExport" runat="server" OnTabStripCommand="MenuSealExport_TabStripCommand"></eluc:TabStrip>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <%-- <asp:GridView ID="gvSealStatus" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnRowCommand="gvSealStatus_RowCommand" 
                        Width="100%" CellPadding="3" OnRowDataBound="gvSealStatus_RowDataBound" ShowHeader="true"
                        EnableViewState="false" OnSorting="gvSealStatus_Sorting" AllowSorting="true">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>--%>

                <telerik:RadGrid RenderMode="Lightweight" ID="gvSealStatus" runat="server" AllowCustomPaging="true"  AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                   GroupingEnabled="false" EnableHeaderContextMenu="true"  
                    OnNeedDataSource="gvSealStatus_NeedDataSource"
                    OnItemCommand="gvSealStatus_ItemCommand">
                    <SortingSettings  SortedBackColor="#FFF6D6" EnableSkinSortStyles="true"></SortingSettings>

                    <MasterTableView AllowCustomSorting="true" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed"  Height="10px">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
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
                            <telerik:GridTemplateColumn HeaderText="Seal Type">
                                <itemstyle wrap="False" horizontalalign="Left" width="150px"></itemstyle>
                          
                                <itemtemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDSEALTYPENAME"]%>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn AllowSorting="true" SortExpression="FLDSEALNO" HeaderText="Seal Number">
                                <itemstyle wrap="False" horizontalalign="Left" width="80px"></itemstyle>
                             
                                <itemtemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDSEALNO"]%>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status">
                                <itemstyle wrap="False" horizontalalign="Left" width="120px"></itemstyle>
                                <headertemplate>
                                    <telerik:RadLabel ID="lblStatusHeader" runat="server">Status</telerik:RadLabel>
                                </headertemplate>
                                <itemtemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDSTATUSNAME"]%>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:"  CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>

        </div>

    </form>
</body>
</html>
