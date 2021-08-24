<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrder.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrder" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Component</title>

    <style type="text/css">
        .datagrid_selectedstyle1 {
            color: Black;
            height: 10px;
        }
    </style>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function PaneResized(sender, args) {
                var splitter = $find('RadSplitter1');
                var browserHeight = $telerik.$(window).height();
                splitter.set_height(browserHeight - 40);
                splitter.set_width("100%");
                var grid = $find("gvWorkOrder");
                var contentPane = splitter.getPaneById("contentPane");
                grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 130) + "px";
            }
            function pageLoad() {
                PaneResized();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="pnlComponentJobGeneral" runat="server">
            <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
            <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 100%">
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>
                <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
                    <telerik:RadPane ID="navigationPane" runat="server" Scrolling="None">
                        <iframe runat="server" id="ifMoreInfo" scrolling="yes" style="min-height: 600px; width: 100%" frameborder="0"></iframe>
                    </telerik:RadPane>
                    <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both">
                    </telerik:RadSplitBar>
                    <telerik:RadPane ID="contentPane" runat="server" Scrolling="None" OnClientResized="PaneResized">
                        <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>
                        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvWorkOrder" DecoratedControls="All" EnableRoundedCorners="true" />
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkOrder" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None" OnDeleteCommand="gvWorkOrder_DeleteCommand" OnNeedDataSource="gvWorkOrder_NeedDataSource" OnSortCommand="gvWorkOrder_SortCommand"
                            OnItemDataBound="gvWorkOrder_ItemDataBound" OnItemCommand="gvWorkOrder_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID">
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Image ID="imgApproval" runat="server" Visible="false" ToolTip="Office Approval Required to Reschedule" />
                                            <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                            <asp:ImageButton ID="Attachments" runat="server" AlternateText="Component Job Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                                CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItem %>' ToolTip="Component Job Attachment"></asp:ImageButton>
                                            <asp:Image ID="imgRaPending" runat="server" Visible="false" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderStyle-Width="111px" HeaderText="Job No." AllowSorting="true" ShowSortIcon="true" SortExpression="FLDWORKORDERNUMBER">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblWorkorderNo" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNO") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblWorkorderNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderStyle-Width="285px" HeaderText="Title" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDWORKORDERNAME">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDCOMPONENTID"]%>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblWorkOrderId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERID"] %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblComponentJobID" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDCOMPONENTJOBID"] %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblJobID" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDJOBID"] %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDDTKEY"] %>'></telerik:RadLabel>
                                            <asp:LinkButton ID="lnkWorkorderName" runat="server" CommandName="Select" CommandArgument='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERID"]%>'
                                                Text=' <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderStyle-Width="112px" HeaderText="Component No." AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNUMBER">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderStyle-Width="250px" HeaderText="Component Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDCOMPONENTNAME">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderStyle-Width="85px" HeaderText="Class Code" SortExpression="FLDCLASSCODE" AllowSorting="true">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%#((DataRowView)Container.DataItem)["FLDCLASSCODE"]%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderStyle-Width="130px" HeaderText="Frequency">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%#((DataRowView)Container.DataItem)["FLDFREQUENCYNAME"]%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Priority" HeaderStyle-Width="63px" ItemStyle-HorizontalAlign="Right" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDPLANINGPRIORITY">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%#((DataRowView)Container.DataItem)["FLDPLANINGPRIORITY"]%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Resp Discipline" HeaderStyle-Width="125px" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDDISCIPLINENAME">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%#((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="66px" HeaderStyle-HorizontalAlign="Center" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDHARDNAME">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblStatus" runat="server" Text='<%#Bind("FLDHARDNAME")%>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Due Date" HeaderStyle-Width="76px" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDPLANNINGDUEDATE">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDuedate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Last Done Date" HeaderStyle-Width="108px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Work Order No." HeaderStyle-Width="108px">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%#((DataRowView)Container.DataItem)["FLDWORKORDERGROUPNO"]%>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Approve"
                                                CommandName="APPROVE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdApprove"
                                                ToolTip="Approve" Width="20PX" Height="20PX">
                                                    <span class="icon"><i class="fas fa-badge-check"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Delete"
                                                CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                                ToolTip="Delete" Width="20PX" Height="20PX">
                                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Cancel"
                                                CommandName="CANCEL" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                                ToolTip="Cancel" Width="20PX" Height="20PX">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save"
                                                CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                                ToolTip="Save" Width="20PX" Height="20PX">
                                                    <span class="icon"><i class="fas fa-save"></i></span>
                                            </asp:LinkButton>
                                            <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                            <asp:LinkButton runat="server" AlternateText="Cancel"
                                                CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="ecmdCancel"
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
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                                    PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder" EnablePostBackOnRowClick="true">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                        <table cellpadding="1" cellspacing="1">
                            <tr>
                                <td>
                                    <img id="Img1" src="<%$ PhoenixTheme:images/red-symbol.png%>" runat="server" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblOverdue" runat="server" Text="* Overdue"></telerik:RadLabel>
                                </td>
                                <td>
                                    <img id="Img2" src="<%$ PhoenixTheme:images/yellow-symbol.png%>" runat="server" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblDue" runat="server" Text="* Due"></telerik:RadLabel>
                                </td>
                                <td>
                                    <img id="Img4" src="<%$ PhoenixTheme:images/14.png%>" runat="server" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblOfficeApproval" runat="server" Text="* Office Approval Required to Reschedule"></telerik:RadLabel>
                                </td>
                                <td>
                                    <img id="Img5" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblRaPending" runat="server" Text="* RA Pending Approval"></telerik:RadLabel>
                                </td>
                                <td>
                                    <img id="Img3" src="<%$ PhoenixTheme:images/green.png%>" runat="server" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="Literal1" runat="server" Text="* RA Approved for use"></telerik:RadLabel>
                                </td>
                                <td>
                                    <img id="Img6" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="Literal2" runat="server" Text="* RA Rejected"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </telerik:RadPane>
                </telerik:RadSplitter>

            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
