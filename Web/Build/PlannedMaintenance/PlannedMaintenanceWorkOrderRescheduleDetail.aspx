<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderRescheduleDetail.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderRescheduleDetail" ValidateRequest="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Job Reschedule History</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">           
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function PaneResized(sender, args) {

                var browserHeight = $telerik.$(window).height();
                var grid = $find("gvWRes");
                var height = (browserHeight - 110);
                grid._gridDataDiv.style.height = height + "px";
                
            }
            function pageLoad() {
                PaneResized();
            }
        </script>
    </telerik:RadCodeBlock>
    <style>
        .imgbtn-height {
            height: 20px;
        }
    </style>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmWorkOrderScheduleDetail" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />


        <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
            <eluc:TabStrip ID="MenuDivWorkOrderReschedule" runat="server" OnTabStripCommand="MenuDivWorkOrderReschedule_TabStripCommand"></eluc:TabStrip>
        </telerik:RadCodeBlock>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvWRes" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="gvWRes" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource" OnPreRender="RadGrid1_PreRender"
            OnItemCommand="RadGrid1_ItemCommand" OnItemDataBound="RadGrid1_ItemDataBound" GroupingEnabled="false" EnableHeaderContextMenu="true">
            <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
                <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn>
                        <HeaderStyle Width="30px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="download"
                                CommandName="DOWNLOAD" CommandArgument='<%# Container.DataSetIndex %>' ID="imgDownload"
                                ToolTip="Download" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-file-excel"></i></span>
                            </asp:LinkButton>

                            <telerik:RadLabel ID="lblRADTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAFLDDTKEY") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblRAReportId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAREPORTID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblRA" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblRescheduleID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERRESCHEDULEID") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Postpone Date" HeaderStyle-Wrap="false">
                        <HeaderStyle Width="70px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%--   <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPOSTPONEDATE")) %>--%>
                            <telerik:RadLabel ID="lblPostponeDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPOSTPONEDATE")) %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Postpone Reason">
                        <HeaderStyle Width="130px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblPostPoneReasonHeader" runat="server" Text="Postpone Reason"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDPOSTPONEREASON")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Due Date">
                        <HeaderStyle Width="70px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDUEDATE")) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Approved By">
                        <HeaderStyle Width="70px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDAPPROVEDBY") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Approved Date" HeaderStyle-Wrap="false">
                        <HeaderStyle Width="70px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDAPPROVEDDATE")) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Linked RA">
                        <HeaderStyle Width="100px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDRAREFNO") + " - " + (DataBinder.Eval(Container, "DataItem.FLDRISKASSESSMENT").ToString() == string.Empty ? DataBinder.Eval(Container, "DataItem.FLDNAME").ToString() : DataBinder.Eval(Container, "DataItem.FLDRISKASSESSMENT").ToString())%>
                            &nbsp;
                            <telerik:RadImageButton runat="server" CssClass="imgbtn-height" AlternateText="Show RA Details" Image-Url="<%$ PhoenixTheme:images/BarChart.png %>"
                                ID="cmdRA" ToolTip="Show PDF" CommandName="RA">
                            </telerik:RadImageButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Remarks">
                        <HeaderStyle Width="80px" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDPOSTPONEREMARKS")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Status">
                        <HeaderStyle Width="80px" />
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDRESCHEDULESTATUSNAME")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle Width="90px" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Width="90px" Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit" Visible="false"
                                ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <telerik:RadImageButton runat="server" CssClass="imgbtn-height" AlternateText="Map RA" 
                                Image-Url="<%$ PhoenixTheme:images/risk-assessment.png %>" ID="cmdMapRA" ToolTip="Map RA" CommandName="RA">
                            </telerik:RadImageButton>

                            <telerik:RadImageButton runat="server" CssClass="imgbtn-height" AlternateText="Postpone Feedback" 
                                Image-Url="<%$ PhoenixTheme:images/post-comment.png %>" ID="cmdFeedback" ToolTip="Postpone Feedback" CommandName="FEEDBACK">
                            </telerik:RadImageButton>

                            <telerik:RadImageButton runat="server" CssClass="imgbtn-height" AlternateText="Confirm" Visible="false"
                                Image-Url="<%$ PhoenixTheme:images/edit-confirm.png %>" ID="cmdConfirm" ToolTip="Confirm" CommandName="CONFIRM">
                            </telerik:RadImageButton>

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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="150px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

    </form>
</body>
</html>
