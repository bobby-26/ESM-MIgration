<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportExpiringCourse.aspx.cs"
    Inherits="Crew_CrewReportExpiringCourse" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Report Expiring Course</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrew.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigureCity"
        runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
            TabStrip="false"></eluc:TabStrip>
        <table>
            <tr style="color: Blue">
                &nbsp;&nbsp; <span id="Span3" class="icon" style="align-self: center" runat="server">
                    <font color="blue" size="2px">Note : To view the Guidelines, place the mouse pointer
                        over the&nbsp;&nbsp;<i class="fas fa-question-circle"></i></span>&nbsp;&nbsp;button.</font>
                <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="GuidlinesTooltip"
                    Width="300px" ShowEvent="onmouseover" RelativeTo="Element" Animation="Fade" TargetControlID="Span3"
                    IsClientID="true" HideEvent="LeaveToolTip" Position="MiddleRight" EnableRoundedCorners="true"
                    HideDelay="5000" Text="Please note: <br/> 1) For onboard, the Documents which are going to expire in given no. of days from sign on date will be determined. <br/> 2) For onleave and planned, the Documents which are going to expire in given no. of days from current date will be determined.">
                </telerik:RadToolTip>
            </tr>
        </table>
        <table border="1" style="border-collapse: collapse;">
            <tr valign="top">
                <td>
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text="Status">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Hard ID="ddlSelectFrom" runat="server" HardTypeCode="54" ShortNameFilter="ONB,ONL,PLN"
                                    OnTextChangedEvent="EnableCBT" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    SortByShortName="true" AutoPostBack="true" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Vessel ID="ucVessel" VesselsOnly="true" AppendDataBoundItems="true" runat="server" Width="100%"
                                    Entitytype="VSL" AssignedVessels="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblBatch" runat="server" Text="Batch">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Batch ID="ucBatch" runat="server" AppendDataBoundItems="true" Width="100%"  />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCourse" runat="server" Text="Course">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Course ID="ddlCourse" runat="server" CourseList="<%#PhoenixRegistersDocumentCourse.ListNonCBTDocumentCourse()%>"
                                    ListCBTCourse="false" AppendDataBoundItems="true" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDocumentsExpiringInNext" runat="server" Text="Documents Expiring In Next">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Number ID="txtNextDays" runat="server" IsPositive="true" IsInteger="true" Tooltip="Enter the No.of Days"
                                    CssClass="input_mandatory" Width="60px" MaxLength="3" />
                                <telerik:RadLabel ID="lblNextDays" runat="server" Text="Days">
                                </telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblRank" runat="server" Text="Rank">
                                </telerik:RadLabel>
                                <br />
                                <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCourseNationality" runat="server" Text="Course Nationality">
                                </telerik:RadLabel>
                                <br />
                                <eluc:Nationality ID="ucNationality1" runat="server" AppendDataBoundItems="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality">
                                </telerik:RadLabel>
                                <br />
                                <eluc:Nationality ID="ucNationality" runat="server" AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
        </eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" 
            EnableViewState="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" GroupingEnabled="false" OnNeedDataSource="gvCrew_NeedDataSource"
            EnableHeaderContextMenu="true" OnItemDataBound="gvCrew_ItemDataBound" OnSortCommand="gvCrew_SortCommand"
            OnItemCommand="gvCrew_ItemCommand" ShowFooter="False" AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
                <HeaderStyle Width="102px" />
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                                    Font-Bold="true">
                                </telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
               
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="File No" AllowSorting="false" HeaderStyle-Width="40px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEmpFileNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                            <telerik:RadLabel ID="lblEmpCode" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" HeaderStyle-Width="150px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                            <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" HeaderStyle-Width="45px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Batch" AllowSorting="false" HeaderStyle-Width="60px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblBatch" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" HeaderStyle-Width="85px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lnkVesselOn" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Sign On Date" AllowSorting="false" HeaderStyle-Width="60px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblSignOnDate" Visible="true" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE","{0:dd/MMM/yyyy}")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Relief Due" AllowSorting="false" HeaderStyle-Width="60px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRelifDue" Visible="true" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDRELIEFDUEDATE","{0:dd/MMM/yyyy}")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Course Type" AllowSorting="false" HeaderStyle-Width="60px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCourseType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSETYPE") %>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Document" AllowSorting="false" HeaderStyle-Width="120px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDocument" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Course Nationality" AllowSorting="false"
                        HeaderStyle-Width="80px" ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="true" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLicCourNationality" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCNATIONALITY") %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Expiry Date" AllowSorting="false" HeaderStyle-Width="60px"
                        ShowSortIcon="true">
                        <HeaderStyle HorizontalAlign="Center" />
                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblExpiryDate" Visible="true" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE","{0:dd/MMM/yyyy}")) %>' />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                    EnableNextPrevFrozenColumns="true" ScrollHeight="415px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
