<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseRequestDetails.aspx.cs"
    Inherits="CrewCourseRequestDetails" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Course Request Details</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCourseRequestDetails" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeCode" runat="server" Text="Employee Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeCode" runat="server" CssClass="readonlytextbox" ReadOnly="True"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeName" runat="server" Text="Employee Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeName" runat="server" MaxLength="50" CssClass="readonlytextbox"
                            ReadOnly="True" Width="300px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRefNo" runat="server" Text="Ref No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRefNo" runat="server" MaxLength="20" CssClass="readonlytextbox"
                            ReadOnly="True">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <b>
                <telerik:RadLabel ID="lblCourseCompletiondetails" runat="server" Text="Course Completion details"></telerik:RadLabel>
            </b>
            <eluc:TabStrip ID="MenuCourseCompletion" runat="server" OnTabStripCommand="CrewCourseCompletion_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewCourseCompletion" runat="server" AllowCustomPaging="true"
                AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None"
                OnNeedDataSource="gvCrewCourseCompletion_NeedDataSource"
                EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <%--asp:GridView ID="gvCrewCourseCompletion" runat="server" AutoGenerateColumns="False"
                        Width="100%" CellPadding="3" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Course Name">
                            <ItemStyle HorizontalAlign="Left" Wrap="true" Width="50%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDCOURSE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Certificate Issued Date">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDDATEOFISSUE", "{0:dd/MMM/yyyy}")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Institute">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblInstitute" runat="server" Text="Institute"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <br />
            <b>
                <telerik:RadLabel ID="lblCoursesPlanneddetails" runat="server" Text="Courses Planned details"></telerik:RadLabel>
            </b>

            <div id="div2" style="position: relative; z-index: 0; width: 100%;">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewCoursePlanned" runat="server" AllowCustomPaging="true"
                    AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None"
                    OnNeedDataSource="gvCrewCoursePlanned_NeedDataSource"
                    EnableViewState="false"  GroupingEnabled="false" EnableHeaderContextMenu="true">
                    <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
                        <HeaderStyle Width="102px" />
                        <%--<asp:GridView ID="gvCrewCoursePlanned" runat="server" AutoGenerateColumns="False"
                        Width="100%" CellPadding="3" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Course Name">
                                <ItemStyle HorizontalAlign="Left" Wrap="true" Width="50%" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCOURSE")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Institute">
                                <ItemStyle HorizontalAlign="Left" Wrap="true" Width="30%" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONNAME") %>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="From Date">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDFROMDATE", "{0:dd/MMM/yyyy}"))%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="To Date">
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemTemplate>
                                    <%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTODATE", "{0:dd/MMM/yyyy}"))%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
            <br />
            <b>
                <telerik:RadLabel ID="lblCoursePending" runat="server" Text="Courses Pending"></telerik:RadLabel>
            </b>

            <div id="dvPending" style="position: relative; z-index: 0; width: 50%;">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewCoursePending" runat="server" AllowCustomPaging="true"
                    AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None"
                    OnNeedDataSource="gvCrewCoursePending_NeedDataSource"
                    EnableViewState="false"  GroupingEnabled="false" EnableHeaderContextMenu="true">
                    <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
                        <HeaderStyle Width="102px" />

                        <%--<asp:GridView ID="gvCrewCoursePending" runat="server" AutoGenerateColumns="False"
                        Width="100%" CellPadding="3" ShowFooter="false" ShowHeader="true" EnableViewState="false">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Course Name">
                                <ItemStyle HorizontalAlign="Left" Wrap="true" Width="50%" />
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCOURSE")%>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
