<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewActivityLog.aspx.cs"
    Inherits="CrewActivityLog" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOnReason" Src="~/UserControls/UserControlSignOnReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignOffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Activity</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Approve(args) {
                if (args) {
                    __doPostBack("<%=Approve.UniqueID %>", "");
                }
            }
        </script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvActivity.ClientID %>"));
}, 200);
}
window.onresize = window.onload = Resize;

function pageLoad(sender, eventArgs) {
    Resize();
    fade('statusmessage');
}
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmActivity" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <asp:Button ID="Approve" runat="server" Text="Approve" OnClick="confirm_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuActivity" runat="server" Title="Activity Log" OnTabStripCommand="Activity_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeCode" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeCode" runat="server" ReadOnly="true" Width="50%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Employee Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeName" runat="server" MaxLength="50" Width="50%" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPresentRank" runat="server" Text="Present Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPayRank" runat="server" MaxLength="20" ReadOnly="true" Width="50%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActivity" runat="server" Text="Activity"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlActivity" runat="server" HardTypeCode="77" AppendDataBoundItems="true" ShortNameFilter="SI,STB,STL,UPL,SEC,CRB,TT,TF,SS,LV,AME,VTO,COR,NOT,NOR,EXI,NCD,RFD,APL,UAL,USL,NJM,UAO"
                            CssClass="dropdown_mandatory" AutoPostBack="true" OnTextChangedEvent="ddlActivity_TextChangedEvent" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input_mandatory" AutoPostBack="true" Width="50%"
                            OnTextChangedEvent="txtFromDate_TextChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" Width="50%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" AppendDataBoundItems="true"
                            VesselsOnly="true" Entitytype="VSL" AssignedVessels="true" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReasonOn" runat="server" Text="Reason On"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SignOnReason ID="ddlSignOnReason" runat="server" AppendDataBoundItems="true" Width="50%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignOnPort" runat="server" Text="Sign On Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Port ID="ddlSignOnPort" runat="server" AppendDataBoundItems="true" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOffReason" runat="server" Text="Sign Off Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SignOffReason ID="ddlSignOffReason" runat="server" AppendDataBoundItems="true" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOffPort" runat="server" Text="Sign Off Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Port ID="ddlSignOffPort" runat="server" AppendDataBoundItems="true" Width="50%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Course ID="ucCourse" runat="server" AppendDataBoundItems="true" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCourseVenue" runat="server" Text="Course Venue"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCourseVenue" runat="server" Width="50%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCourseCountry" runat="server" Text="Course Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Country ID="ucCountry" runat="server" AppendDataBoundItems="true" Width="50%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="50%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbltravelDays" runat="server" Text="Travel Days"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txttravelDays" runat="server" IsInteger="true" Width="50px" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuCrewActivityLog" runat="server" OnTabStripCommand="MenuCrewActivityLog_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvActivity" runat="server" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvActivity_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvActivity_ItemDataBound" OnEditCommand="gvActivity_EditCommand"
                OnItemCommand="gvActivity_ItemCommand" ShowFooter="false"
                OnSortCommand="gvActivity_SortCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="Activity Code" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActivityLogId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYLOGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSystemGen" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSYSTEMGEN") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOverLap" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERLAP") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblActivityCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Activity Name" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActivityName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACTIVITYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From Date" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFromDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDFROMDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Date" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblToDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDTODATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" ToolTip="Delete" CommandName="ACTIVITYDELETE" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <%-- <asp:LinkButton runat="server" AlternateText="Delete" ID="cmdDelete" CommandName="ACTIVITYDELETE" ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <table>
                            <tr class="rowoverlap">
                                <td width="5px" height="10px"></td>
                            </tr>
                        </table>
                    </td>
                    <td>
                        <asp:Literal ID="lblOverlappingActivities" runat="server" Text="Overlapping Activities"></asp:Literal>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
