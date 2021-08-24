<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderDone.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderDone" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job Done History</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function PaneResized(sender, args) {

                var browserHeight = $telerik.$(window).height();
                var grid = $find("gvWorkOrder");
                var height = (browserHeight - 110);
                grid._gridDataDiv.style.height = height + "px";
                
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

        <telerik:RadAjaxManager runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvWorkOrder">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvWorkOrder" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>

        
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
        
    </form>
</body>
</html>
