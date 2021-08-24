<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceRounds.aspx.cs"
    Inherits="PlannedMaintenanceRounds" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ComponentTypeTreeView" Src="~/UserControls/UserControlTreeView.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var splitter = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            splitter.set_height(browserHeight - 40);
            splitter.set_width("100%");
            var grid = $find("gvRounds");
            var contentPane = splitter.getPaneById("listPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 100) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmComponentJob" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
<%--        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvRounds">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvRounds" />
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" LoadingPanelID="RadAjaxLoadingPanel" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>--%>
     <telerik:RadAjaxPanel ID="pnlComponentJobGeneral" runat="server">
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuComponentJob" runat="server" OnTabStripCommand="MenuComponentJob_TabStripCommand" TabStrip="true"></eluc:TabStrip>

        <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
            <telerik:RadPane ID="generalPane" runat="server" Scrolling="None">
                <iframe runat="server" id="ifMoreInfo" style="height: 560px; width: 100%"></iframe>
            </telerik:RadPane>
            <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both"></telerik:RadSplitBar>
            <telerik:RadPane ID="listPane" runat="server" Scrolling="None" OnClientResized="PaneResized">

                <eluc:TabStrip ID="MenuDivComponentJob" runat="server" OnTabStripCommand="MenuDivComponentJob_TabStripCommand"></eluc:TabStrip>

                <telerik:RadGrid RenderMode="Lightweight" ID="gvRounds" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Width="100%"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvRounds_NeedDataSource" OnSortCommand="gvRounds_SortCommand" GroupingEnabled="false"
                    EnableHeaderContextMenu="true" OnItemCommand="gvRounds_ItemCommand" DataKeyNames="FLDCOMPONENTJOBID" OnItemDataBound="gvRounds_ItemDataBound">
                    <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed">
                        <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Job Code" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDJOBCODE">
                                <HeaderStyle Width="86px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbljobId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>' Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblFunctionNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Job Title" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDJOBTITLE">
                                <HeaderStyle Width="400px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblComponentJobId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTJOBID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblWorkOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkStockItemName" runat="server" CommandName="Select" CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTJOBID") %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>'></asp:LinkButton>
                                    <eluc:ToolTip ID="ucToolTipJobTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBTITLE") %>' TargetControlId="lnkStockItemName" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Frequency">
                                <HeaderStyle Width="107px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREQUENCYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Last Done Date">
                                <HeaderStyle Width="107px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblLastDonedate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDJOBLASTDONEDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Priority">
                                <HeaderStyle Width="58px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPriority" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRIORITY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Resp Discipline">
                                <HeaderStyle Width="130px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCIPLINENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Next Due Date">
                                <HeaderStyle Width="105px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblNextDuedate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDJOBNEXTDUEDATE")) %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Delete"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                        ToolTip="Delete">
                                                <span class="icon"><i class="fas fa-trash"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Report Work"
                                        CommandName="REPORT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdReport"
                                        ToolTip="Report">
                                                <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save"
                                        CommandName="Save" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                        ToolTip="Save" Width="20PX" Height="20PX">
                                                <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:LinkButton runat="server" AlternateText="Cancel"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel" Width="20PX" Height="20PX">
                                                <span class="icon"><i class="fas fa-times-circle"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
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
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadPane>
        </telerik:RadSplitter>
         </telerik:RadAjaxPanel>
    </form>
</body>
</html>
