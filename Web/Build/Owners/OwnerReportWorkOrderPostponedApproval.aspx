<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerReportWorkOrderPostponedApproval.aspx.cs" Inherits="OwnerReportWorkOrderPostponedApproval" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Postponement Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">              
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <style type="text/css">
            .rbButton {
                padding: 8px 10px;
            }
        </style>
        <script type="text/javascript">

            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvPostponedApproval.ClientID %>"));
               }, 200);
            }
            function pageLoad() {
                Resize();
            }
            function CloseUrlModelWindow() {
                var wnd = $find('<%=RadWindow_NavigateUrl.ClientID %>');
                wnd.close();
                var masterTable = $find('<%=gvPostponedApproval.ClientID %>').get_masterTableView();
                masterTable.rebind();
            }            
        </script>
    </telerik:RadCodeBlock>

</head>
<body onresize="Resize()" onload="Resize()">
    <form id="frmPostponedApproval" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvPostponedApproval">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvPostponedApproval"/>
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucVessel" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ucVessel">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvPostponedApproval"/>
                        <telerik:AjaxUpdatedControl ControlID="MenuPostponedApproval" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <table width="40%" id="tblFilter" runat="server">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input"
                        OnTextChangedEvent="ucVessel_TextChangedEvent" AutoPostBack="true" Width="200px" />
                </td>                
            </tr>
        </table>
        <eluc:TabStrip ID="MenuPostponedApproval" runat="server" OnTabStripCommand="MenuPostponedApproval_TabStripCommand"></eluc:TabStrip>        
        <telerik:RadGrid ID="gvPostponedApproval" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnItemCommand="gvPostponedApproval_ItemCommand" Width="100%" GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnNeedDataSource="gvPostponedApproval_NeedDataSource" OnItemDataBound="gvPostponedApproval_ItemDataBound">
            <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Vessel Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDVESSELNAME" UniqueName="FLDVESSELNAME">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblVesselid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblRA" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Number" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDWORKORDERNUMBER">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90px"></ItemStyle>
                        <HeaderStyle Width="90px" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRescheduleID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERRESCHEDULEID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblWorkorderid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblWorkorderNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'></telerik:RadLabel>                            
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Title">
                        <HeaderStyle Wrap="false" Width="250px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkJobDetaiil" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME") %>'
                                CommandName="JOBDETAILS" ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNAME").ToString() + ". Click for job details" %>'>                                    
                            </asp:LinkButton>                           
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Component">
                        <HeaderStyle Wrap="false" Width="95px" HorizontalAlign="Left" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"] + " " + ((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>                   
                    <telerik:GridTemplateColumn HeaderText="Class Code">
                        <HeaderStyle Wrap="false" Width="90px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDCLASSCODE"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Frequency">
                        <HeaderStyle Wrap="false" Width="130px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDFREQUENCYNAME"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Priority">
                        <HeaderStyle Wrap="false" Width="65px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDPLANINGPRIORITY"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Responsibility">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Due Date" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDPLANNINGDUEDATE">
                        <HeaderStyle Wrap="false" Width="76px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <asp:Label ID="lblDuedate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNINGDUEDATE")) %>'></asp:Label>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Done Date">
                        <HeaderStyle Wrap="true" Width="80px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTDONEDATE")) %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status">
                        <HeaderStyle Wrap="true" Width="80px" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <%#  DataBinder.Eval(Container,"DataItem.FLDRESCHEDULESTATUSNAME") %>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="50px"></ItemStyle>
                        <ItemTemplate>
                             <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Cancel"
                                CommandName="CANCELREQ" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                ToolTip="Cancel Request" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                            <asp:ImageButton runat="server" AlternateText="Approve"
                                ImageUrl="<%$ PhoenixTheme:images/approve.png %>" ID="cmdApproveReschedule" ToolTip="Approve" CommandName="APPROVERESCHEDULE"></asp:ImageButton>
                            <%--<asp:LinkButton runat="server" AlternateText="Delete"
                                CommandName="APPROVERESCHEDULE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdApproveReschedule"
                                ToolTip="Approve Reschedule" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fa fa-map-marker"></i></span>
                                
                            </asp:LinkButton>--%>
                            <asp:ImageButton runat="server" AlternateText="History"
                                ImageUrl="<%$ PhoenixTheme:images/showlist.png %>" ID="cmdView" ToolTip="History" CommandName="VIEW"></asp:ImageButton>

                            <asp:ImageButton runat="server"  AlternateText="RA as Smart form WP-11" 
                                ImageUrl="<%$ PhoenixTheme:images/post-comment.png %>" ID="cmdFeedback" ToolTip="RA as Smart form WP-11" CommandName="FEEDBACK">
                            </asp:ImageButton>

                            <asp:ImageButton runat="server" AlternateText="Non Routine RA" ImageUrl="<%$ PhoenixTheme:images/BarChart.png %>"
                                ID="cmdRA" ToolTip="Non Routine RA" CommandName="RA">
                            </asp:ImageButton>
                           <asp:LinkButton runat="server" AlternateText="RA as WP-11"
                                CommandName="XLWP11" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdXLWP11"
                                ToolTip="RA as WP-11" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-file-excel"></i></span>
                            </asp:LinkButton>
                            <%--<asp:LinkButton runat="server" AlternateText="Delete"
                                CommandName="VIEW" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdView"
                                ToolTip="History" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fa fa-file"></i></span>
                            </asp:LinkButton>--%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="3" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
         <telerik:RadWindow runat="server" ID="RadWindow_NavigateUrl" Width="500px" Height="365px"
            Modal="true" OffsetElementID="main" VisibleStatusbar="false" KeepInScreenBounds="true" ReloadOnShow="true" ShowContentDuringLoad="false">
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=RadWindow_NavigateUrl.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
