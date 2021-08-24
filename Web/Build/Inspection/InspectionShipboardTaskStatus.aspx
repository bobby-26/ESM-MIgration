<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionShipboardTaskStatus.aspx.cs" Inherits="InspectionShipboardTaskStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Shipboard Task Status</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
            }

            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("gvShipboardTasks").height(browserHeight - 80);
            });

            //script added for fixing Div width for the comments table
        </script>
    </telerik:RadCodeBlock>
</head>
<body onload="resizediv();">
    <form id="frmShipboardTaskStatus" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Title runat="server" ID="Title1" Text="Shipboard Task Status" ShowMenu="false" Visible="false"></eluc:Title>
        <br />
        <telerik:RadCheckBox ID="chkShowAll" runat="server" Text="Show All" AutoPostBack="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvShipboardTasks" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" Height="90%" CellPadding="3" AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvShipboardTasks_NeedDataSource"
            ShowHeader="true" EnableViewState="false" AllowSorting="true" DataKeyNames="FLDINSPECTIONCORRECTIVEACTIONID">
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <ColumnGroups>
                    <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                </ColumnGroups>
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Vessel Name">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Task">
                        <ItemStyle HorizontalAlign="Left" Wrap="True" />
                        <ItemTemplate>
                            <asp:Label ID="lblTask" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCORRECTIVEACTION") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Target Date">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <asp:Label ID="lblTargetDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Completion Date">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <asp:Label ID="lblCompletionDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Reschedule Date">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <asp:Label ID="lblRescheduleDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLATESTRESCHEDULEDATE")) %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status">
                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
