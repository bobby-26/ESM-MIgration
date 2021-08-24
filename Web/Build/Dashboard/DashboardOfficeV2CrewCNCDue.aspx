<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardOfficeV2CrewCNCDue.aspx.cs" Inherits="DashboardOfficeV2CrewCNCDue" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contact Due</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvcncdue.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="gvcncdueTab" runat="server" OnTabStripCommand="gvcncdueTab_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvcncdue" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" ShowFooter="false" EnableViewState="true" OnSortCommand="gvcncdue_SortCommand"
                OnNeedDataSource="gvcncdue_NeedDataSource" OnItemCommand="gvcncdue_ItemCommand" OnItemDataBound="gvcncdue_ItemDataBound">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
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
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkname" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDEMPLOYEENAME")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="true">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Vessel" AllowSorting="true">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDLASTVESSELNAME")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due Date For Follow Up" AllowSorting="true">
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDFOLLOWUPDATE"])%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Contact Date" AllowSorting="true">
                            <ItemTemplate>
                                <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDLASTCONTACTDATE"])%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Remarks" AllowSorting="true">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDLASTREMARKS")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria" PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
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