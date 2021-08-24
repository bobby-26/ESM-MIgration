<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditInterfaceHistory.aspx.cs" Inherits="InspectionAuditInterfaceHistory" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>History</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvInspectionhistory" runat="server" CellSpacing="0" GridLines="None"
                AllowSorting="true" EnableHeaderContextMenu="true" EnableViewState="false"
                GroupingEnabled="false" OnNeedDataSource="gvInspectionhistory_NeedDataSource">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">

                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>

                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Current" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="100px"></ItemStyle>
                            <HeaderStyle Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Previous" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="140px"></ItemStyle>
                            <HeaderStyle Width="140px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPrevious" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVIOUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Verified" HeaderStyle-HorizontalAlign="Left">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" Width="180px"></ItemStyle>
                            <HeaderStyle Width="180px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVerified" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERIFIED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>

    </form>
</body>
</html>

