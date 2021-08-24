<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceRoundsJob.aspx.cs" Inherits="PlannedMaintenanceRoundsJob" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Routine</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
        function PaneResized(sender, args) {
            
            var browserHeight = $telerik.$(window).height();
            var grid = $find("gvRoundsJob");
            var height = browserHeight - 120;
            grid._gridDataDiv.style.height = height + "px";
            
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmgvUsesParts" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
        <eluc:TabStrip ID="MenuRoundsJob" runat="server"></eluc:TabStrip>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRoundsJob" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvRoundsJob_NeedDataSource" OnItemDataBound="gvRoundsJob_ItemDataBound" 
                OnItemCommand="gvRoundsJob_ItemCommand" OnDeleteCommand="gvRoundsJob_DeleteCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false" DataKeyNames="FLDNUMBER" OnSortCommand="gvRoundsJob_SortCommand">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" AutoGenerateColumns="false" 
                    TableLayout="Fixed" DataKeyNames="FLDCOMPONENTJOBID">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Job Code" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNUMBER">
                            <HeaderStyle Width="85px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Title" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNAME">
                            <HeaderStyle Width="180px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipJobTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>' TargetControlId="lblJobTitle" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component No." AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNUMBER">
                            <HeaderStyle Width="111px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNUMBER") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNAME">
                            <HeaderStyle Width="125px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></telerik:RadLabel>
                                <%--<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>--%>
                                <eluc:ToolTip ID="ucToolTipCompName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>' TargetControlId="lblCompName" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Frequency">
                            <HeaderStyle Width="135px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Done Date">
                            <HeaderStyle Width="107px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDJOBLASTDONEDATE")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Priority">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDPRIORITY") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Resp Discipline">
                            <HeaderStyle Width="135px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container,"DataItem.FLDDISCIPLINENAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Next Due Date">
                            <HeaderStyle Width="107px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDJOBNEXTDUEDATE")) %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="true"
                        PageSizeLabelText="Records per page:" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" ScrollHeight="150px" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>

        <%--   <asp:GridView ID="gvRoundsJob" runat="server" AutoGenerateColumns="False" Width="100%"
                Font-Size="11px" ShowHeader="true" EnableViewState="false"
                AllowSorting="true" OnRowDataBound="gvRoundsJob_RowDataBound"
                OnSorting="gvRoundsJob_Sorting" OnRowDeleting="gvRoundsJob_RowDeleting" DataKeyNames="FLDCOMPONENTJOBID">--%>
    </form>
</body>
</html>
