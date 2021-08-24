<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionReOpenTaskHistory.aspx.cs" Inherits="InspectionReOpenTaskHistory" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Re-Open Task History</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function resizediv() {
                var tbl = document.getElementById("tblComments");
                if (tbl != null) {
                    for (var i = 0; i < tbl.rows.length; i++) {
                        tbl.rows[i].cells[2].getElementsByTagName("div")[0].style.width = tbl.rows[i].cells[2].offsetWidth + "px";
                    }
                }
            }//script added for fixing Div width for the comments table
        </script>
    </telerik:RadCodeBlock>
</head>
<body onload="resizediv();">
    <form id="frmIncidentCARExtensionReason" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadGrid ID="gvReopentaskReason" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" Height="99%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0"
                GridLines="None" EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvReopentaskReason_NeedDataSource"
                CellPadding="3" OnItemCommand="gvReopentaskReason_ItemCommand" OnItemDataBound="gvReopentaskReason_ItemDataBound"
                ShowHeader="true" EnableViewState="false" DataKeyNames="FLDINSPECTIONREOPENTASKHISTORYID">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDINSPECTIONREOPENTASKHISTORYID">
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
                        <telerik:GridTemplateColumn HeaderText="Old Target Date">
                            <HeaderStyle Width="20%" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHistoryId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONREOPENTASKHISTORYID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOldTargetDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDOLDTARGETDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="New Target Date">
                            <HeaderStyle Width="20%" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNewTargetDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDNEWTARGETDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Re-Open Task Reason">
                            <HeaderStyle Width="25%" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREOPENREASON").ToString().Length > 50 ? DataBinder.Eval(Container, "DataItem.FLDREOPENREASON").ToString().Substring(0, 50) + "..." : DataBinder.Eval(Container, "DataItem.FLDREOPENREASON").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="Reason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREOPENREASON") %>' TargetControlId="lblReason" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reopened By">
                            <HeaderStyle Width="15%" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPostedBy" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reopened Date">
                            <HeaderStyle Width="20%" HorizontalAlign="Left" />
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPostedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
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
