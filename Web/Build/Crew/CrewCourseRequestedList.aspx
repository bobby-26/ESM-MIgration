<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseRequestedList.aspx.cs" Inherits="Crew_CrewCourseRequestedList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Src="~/UserControls/UserControlErrorMessage.ascx" TagPrefix="eluc" TagName="Error" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Course requested list</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCourseReq" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <%--<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">--%>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvReq" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvReq_ItemCommand" OnNeedDataSource="gvReq_NeedDataSource"
                EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="File No">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20px"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDFILENO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <%--<HeaderTemplate>
                            <telerik:RadLabel ID="lblFirstHeader" runat="server">Name</telerik:RadLabel>
                        </HeaderTemplate>--%>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lnkEmployee" runat="server" Width="180px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <%--<HeaderTemplate>
                            <telerik:RadLabel ID="lblRankHeader" runat="server">Rank</telerik:RadLabel>
                        </HeaderTemplate>--%>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Zone">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDZONE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Available From">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" Width="20px"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDFROMDATE", "{0:dd/MMM/yyyy}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Available To">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" Width="20px"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDTODATE", "{0:dd/MMM/yyyy}") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Enrolled YN">
                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container, "DataItem.FLDENROLLED")%>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
