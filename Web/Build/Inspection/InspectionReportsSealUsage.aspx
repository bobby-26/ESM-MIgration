<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionReportsSealUsage.aspx.cs"
    Inherits="InspectionReportsSealUsage" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seal Usage Report</title>
    <telerik:RadCodeBlock runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSealUsageReport" runat="server">
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
                            <eluc:Quick ID="ucSealType" Width="200px" runat="server" AppendDataBoundItems="true"
                                QuickTypeCode="87" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblLocation" runat="server" Text="Location"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlLocation" EmptyMessage="--Select--" Filter="Contains" MarkFirstMatch="true" runat="server" DataTextField="FLDLOCATIONNAME"
                                DataValueField="FLDLOCATIONID">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="dummy" />
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSealNo" runat="server" Text="Seal Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSealNumber" runat="server"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblSealAffixedBy" runat="server" Text="Seal Affixed by"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtSealAffixedby" runat="server"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAffixedFrom" runat="server" Text="Affixed From "></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtFromDate" runat="server" DatePicker="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblAffixedTo" runat="server" Text="Affixed To"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtToDate" runat="server" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblReason" runat="server" Text="Reason"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucReason" runat="server" AppendDataBoundItems="true"
                                QuickTypeCode="88" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblShowAllSealLocations" runat="server" Text="Show all seal locations"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadCheckBox ID="chkShowall" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuSealExport" runat="server" OnTabStripCommand="MenuSealExport_TabStripCommand"></eluc:TabStrip>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <%--  <asp:GridView ID="gvSealUsage" runat="server" AutoGenerateColumns="False" Font-Size="11px" OnRowCommand="gvSealUsage_RowCommand"
                    Width="100%" CellPadding="3" OnRowDataBound="gvSealUsage_RowDataBound" ShowHeader="true"
                    EnableViewState="false" OnSorting="gvSealUsage_Sorting" AllowSorting="true">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvSealUsage" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    OnNeedDataSource="gvSealUsage_NeedDataSource"
                    OnItemDataBound="gvSealUsage_ItemDataBound">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" Height="10px">
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

                            <telerik:GridTemplateColumn HeaderText="">
                                <HeaderStyle Width="30px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="2%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Location">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="200px"></ItemStyle>

                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDLOCATIONNAME"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Seal Point">
                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="200px"></ItemStyle>

                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDSEALPOINTNAME"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Seal Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>

                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDSEALNO"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Seal Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>

                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDSEALTYPENAME"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Seal Affixed by">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>

                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDPERSONAFFIXINGSEAL"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Date Affixed">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                            
                                <ItemTemplate>
                                    <%#  General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDDATEAFFIXED"])%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reason">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="80px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblReasonHeader" runat="server">Reason</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDREASONNAME"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" ScrollHeight="" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>

            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <img id="Img1" src="<%$ PhoenixTheme:images/red-symbol.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOverDue" runat="server" Text=" * Replacement Overdue"></telerik:RadLabel>
                    </td>
                    <td>
                        <img id="Img2" src="<%$ PhoenixTheme:images/yellow-symbol.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDueWithin30days" runat="server" Text=" * Replacement Due within 30 days"></telerik:RadLabel>
                    </td>
                    <td>
                        <img id="Img5" src="<%$ PhoenixTheme:images/green-symbol.png%>" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDueWithin60Days" runat="server" Text=" * Replacement Due within 60 days"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </div>

    </form>
</body>
</html>
