<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalRA.aspx.cs" Inherits="Dashboard_DashboardTechnicalRA" %>

<%@ Import Namespace="System.Data" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvForms.ClientID %>"));
               }, 200);
            }
            window.onresize = window.onload = Resize;
            function pageLoad() {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel" LoadingPanelID="RadAjaxLoadingPanel1">
            <asp:Button ID="cmdHiddenSubmit" runat="server" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
            <telerik:RadGrid ID="gvForms" runat="server" AutoGenerateColumns="false"
                AllowSorting="false" GroupingEnabled="false" OnItemDataBound="gvForms_ItemDataBound"
                EnableHeaderContextMenu="true" AllowCustomPaging="true" AllowPaging="true" OnNeedDataSource="gvForms_NeedDataSource">
                <MasterTableView TableLayout="Fixed">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Work Order">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkForm" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"] + " - " + ((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component">
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDCOMPONENT"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date">
                            <ItemTemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDPLANNINGDUEDATE"].ToString())%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Non Routine RA">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkRiskCreate" runat="server" Text="Required" CommandName="RACREATE"></asp:LinkButton>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
