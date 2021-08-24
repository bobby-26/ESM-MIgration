<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSealInventory.aspx.cs" Inherits="InspectionSealInventory" %>

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Seal Inventory</title>
    <telerik:RadCodeBlock runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSealInventory" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
            <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            </div>
            <div id="divFind" runat="server">
                <table id="tblFind" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSealType" runat="server" Text=" Seal Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucSealType" Width="200px" runat="server" AutoPostBack="true"  OnTextChangedEvent="ucSealType_TextChangedEvent" AppendDataBoundItems="true" QuickTypeCode="87" />
                        </td>
                    </tr>
                </table>
            </div>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuSeal" runat="server" OnTabStripCommand="MenuSeal_TabStripCommand"></eluc:TabStrip>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <%-- <asp:GridView ID="gvSealInventory" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" OnRowDataBound="gvSealInventory_RowDataBound" ShowHeader="true"
                    EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>--%>


                <telerik:RadGrid RenderMode="Lightweight" ID="gvSealInventory" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    OnNeedDataSource="gvSealInventory_NeedDataSource"
                    OnItemCommand="gvSealInventory_ItemCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" Height="10px">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Seal Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                              
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDSEALTYPENAME"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Received">
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="80px"></ItemStyle>
                              
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDRECEIVED"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Issued">
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="80px"></ItemStyle>
                             
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDISSUED"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="In Use">
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="80px"></ItemStyle>
                            
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDINUSE"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Disposed">
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="80px"></ItemStyle>
                            
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDSCRAPPED"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="ROB">
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="80px"></ItemStyle>
                             
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDROB"]%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                        </Columns>
                        <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

            </div>

            <div id="divGuidance" runat="server">
                <telerik:RadLabel ID="lblGuidance" runat="server" Text="* Note: ROB = Received - [Issued + In Use + Disposed]." CssClass="guideline_text"></telerik:RadLabel>
            </div>
        </div>

    </form>
</body>
</html>
