<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentJob.aspx.cs"
    Inherits="PlannedMaintenanceComponentJob" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ComponentTypeTreeView" Src="~/UserControls/UserControlTreeView.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
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
            var grid = $find("gvWorkOrder");
            var contentPane = splitter.getPaneById("contentPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 110) + "px";
            //var genPane = splitter.getPaneById("navigationPane");
            //document.getElementById('ifMoreInfo').style.height = (genPane._contentElement.offsetHeight) + "px";
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
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>

        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvComponentJob">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvComponentJob" LoadingPanelID="RadAjaxLoadingPanel1" />
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucConfirm" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ucConfirm">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="pnlComponentJobGeneral" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxPanel ID="pnlComponentJobGeneral" runat="server">
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <%--<eluc:Confirm ID="ucConfirm" runat="server" Visible="false" OnConfirmMesage="ucConfirm_ConfirmMesage" OKText="Yes" CancelText="No" />--%>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" CssClass="hidden" OnClick="cmdHiddenSubmit_Click" />
                <div style="font-weight: 600; font-size: 12px;" runat="server">
                    <eluc:TabStrip ID="MenuComponentJob" runat="server" OnTabStripCommand="MenuComponentJob_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>

                <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
                    <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                        <iframe runat="server" id="ifMoreInfo" style="height: 600px; width: 100%" frameborder="0"></iframe>
                    </telerik:RadPane>
                    <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
                    </telerik:RadSplitBar>
                    <telerik:RadPane ID="contentPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                        <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>

                        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvStockItem" DecoratedControls="All" EnableRoundedCorners="true" />
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkOrder" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvWorkOrder_ItemCommand"
                OnNeedDataSource="gvWorkOrder_NeedDataSource" OnPreRender="gvWorkOrder_PreRender" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnItemDataBound="gvWorkOrder_ItemDataBound">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDWORKORDERID">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridBoundColumn HeaderText="Job Number" UniqueName="FLDWORKORDERNUMBER" DataField="FLDWORKORDERNUMBER" DataType="System.String">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Job Title" UniqueName="FLDWORKORDERNAME" DataField="FLDWORKORDERNAME" DataType="System.String">
                            <HeaderStyle Width="410px" />
                            <ItemStyle Width="410px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Category" UniqueName="FLDJOBCATEGORY" DataField="FLDJOBCATEGORY" DataType="System.String">
                            <HeaderStyle Width="180px" />
                            <ItemStyle Width="180px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Frequency" UniqueName="FLDFREQUENCYNAME" DataField="FLDFREQUENCYNAME" DataType="System.String">
                            <HeaderStyle Width="130px" />
                            <ItemStyle Width="130px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Priority" UniqueName="FLDPLANINGPRIORITY" DataField="FLDPLANINGPRIORITY" DataType="System.Int16">
                            <HeaderStyle Width="70px" />
                            <ItemStyle Width="70px" HorizontalAlign="Right" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Job Done (Yes / Defect)" UniqueName="FLDJOBDONESTATUS" DataField="FLDJOBDONESTATUS" DataType="System.String">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Width="120px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Done Date" UniqueName="FLDWORKDONEDATE" DataField="FLDWORKDONEDATE" DataType="System.DateTime" DataFormatString="{0:dd-MM-yyyy}" >
                            <HeaderStyle Width="100px" />
                            <ItemStyle Width="100px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Done By" UniqueName="FLDWORKDONEBY" DataField="FLDWORKDONEBY" DataType="System.String">
                            <HeaderStyle Width="180px" />
                            <ItemStyle Width="180px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn HeaderText="Remarks" UniqueName="FLDREMARKS" DataField="FLDREMARKS" DataType="System.String">
                            <HeaderStyle Width="300px" />
                            <ItemStyle Width="300px" />
                        </telerik:GridBoundColumn>
                        
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="MaintenanceLog"
                                    CommandName="MAINTENANCEFORM" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRTemplates"
                                    ToolTip="Reporting Templates" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-part-detail"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Parameters"
                                    CommandName="PARAMETERS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdParameters"
                                    ToolTip="Parameters" Width="20px" Height="20px">
                               <span class="icon"><i class="fas fa-newspaper"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachments"
                                    CommandName="ATTACHMENTS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttachments"
                                    ToolTip="Attachments" Width="20px" Height="20px">
                                <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Parts"
                                    CommandName="PARTS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdParts"
                                    ToolTip="Parts" Width="20px" Height="20px">
                                <span class="icon"><i class="fas fa-cogs"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="RA History" 
                                    CommandName="RA" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdRA"
                                    ToolTip="RA" Width="20px" Height="20px">
                                <span class="icon"><i class="fas fa-chart-bar"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="PTW History" 
                                    CommandName="PTW" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPTW"
                                    ToolTip="PTW" Width="20px" Height="20PX">
                                <span class="icon"><i class="fas fa-envelope-open-text"></i></span>
                                </asp:LinkButton>
                                
                                <asp:LinkButton runat="server" AlternateText="Report" ID="cmdReschedule" CommandName="RESCHEDULE" ToolTip="Postpone History">
                                 <span class="icon"><i class="far fa-calendar-alt"></i></span>
                                </asp:LinkButton>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
                    </telerik:RadPane>
                </telerik:RadSplitter>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
