<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionOfficeCorrectiveTasks.aspx.cs" Inherits="InspectionOfficeCorrectiveTasks" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="head1" runat="server">
    <title>Corrective Tasks</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvShipBoardTasks").height(browserHeight - 98);
            });

        </script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvShipBoardTasks.ClientID %>"));
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
    <form id="frmRegistersBudgetBillingList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Title runat="server" ID="Title1" Text="Office Corrective Tasks" Visible="false" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" TabStrip="true" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuShipBoardTasks" runat="server" OnTabStripCommand="MenuShipBoardTasks_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvShipBoardTasks" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                OnSorting="gvShipBoardTasks_Sorting" Width="100%" CellPadding="3" OnItemCommand="gvShipBoardTasks_ItemCommand" OnItemDataBound="gvShipBoardTasks_ItemDataBound"
                AllowSorting="true" ShowFooter="false" GroupingEnabled="false" EnableHeaderContextMenu="true" OnNeedDataSource="gvShipBoardTasks_NeedDataSource"
                ShowHeader="true" DataKeyNames="FLDINSPECTIONCORRECTIVEACTIONID" GridLines="None" EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
                    <ColumnGroups>
                        <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Width="100px" AllowSorting="true" SortExpression="FLDVESSELNAME" ShowSortIcon="true">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="30%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselName" Width="98%" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSourceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCORRECTIVESOURCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSourceType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCETYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Source Ref.No" HeaderStyle-Width="110px" AllowSorting="true" SortExpression="FLDINSPECTIONCORRECTIVEACTIONID" ShowSortIcon="true">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="27%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCorrectiveActionid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONCORRECTIVEACTIONID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDFROM") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lnkTaskSource" runat="server" CommandName="SHOWSOURCE" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDFROMNAME") %>'></asp:LinkButton>
                                <%--<asp:LinkButton ID="lnkTask" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDFROMNAME") %>'></asp:LinkButton>--%>
                                <telerik:RadLabel ID="lblTaskk" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDFROMNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCreatedFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDFROMNAME") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Source" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="false" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="45px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="3%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Item" HeaderStyle-Width="118px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMNAME").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMNAME")%>' TargetControlId="lblItem"
                                    Width="100px" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Task" HeaderStyle-Width="140px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLongTermActionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONCORRECTIVEACTIONID") %>'></telerik:RadLabel>
                                <%--<telerik:RadLabel ID="lblTask" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCORRECTIVEACTIONTEXT") %>' Width="150"></telerik:RadLabel>--%>
                                <asp:LinkButton ID="lnkTaskDetails" runat="server" CommandName="SELECT"
                                    CommandArgument='<%# Container.DataItem %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDCORRECTIVEACTION").ToString() %>'></asp:LinkButton>
                                <eluc:ToolTip ID="ucToolTip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCORRECTIVEACTION")%>'
                                    Width="450px" TargetControlId="lnkTaskDetails" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Department" HeaderStyle-Width="130px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="5%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldepartment" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME").ToString() %>'>
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipdepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME")%>'
                                    Width="450px" TargetControlId="lbldepartment" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Target" HeaderStyle-Width="73px" AllowSorting="true" SortExpression="FLDTARGETDATE" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="5%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTargetDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed" HeaderStyle-Width="78px" AllowSorting="true" SortExpression="FLDCOMPLETIONDATE" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="5%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Closed" HeaderStyle-Width="73px" AllowSorting="true" SortExpression="FLDCACLOSEOUTVERIFIEDDATE" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="5%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblClosedDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCACLOSEOUTVERIFIEDDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="72px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPBOARDSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="76px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="5%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Reschedule" CommandName="RESCHEDULE"
                                    ID="imgReschedule" ToolTip="Reschedule">
                                <span class="icon"><i class="fas fa-history"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Superintendent Approval" CommandName="SUPERINTENDENTAPPROVAL"
                                    ID="imgSuperintendentComments" ToolTip="Superintendent Approval" Visible="false">
                                 <span class="icon"><i class="fas fa-user-tie"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAtt"
                                    ToolTip="Upload Evidence">
                                <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <%-- <asp:LinkButton runat="server" AlternateText="Communication"
                                CommandName="COMMUNICATION" ID="lnkCommunication" ToolTip="Communication">
                                <span class="icon"><i class="fas fa-postcomment"></i></span>
                            </asp:LinkButton>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <table cellpadding="1" cellspacing="1">
            <tr>
                <td>
                     <table>
                         <tr style="background-color:red">
                             <td width="5px" height="10px"></td>
                         </tr>
                     </table>                     
                </td>
                <td>
                    <b>
                    <telerik:RadLabel ID="lblOverdue" runat="server" Text=" - Overdue"></telerik:RadLabel></b>
                </td>
                <td>
                     <table>
                         <tr style="background-color:darkviolet">
                             <td width="5px" height="10px"></td>
                         </tr>
                     </table>                     
                </td>
                <td>
                    <b><telerik:RadLabel ID="lblPostponed" runat="server" Text=" - Postponed"></telerik:RadLabel></b>
                </td>
            </tr>
        </table>
        <triggers>
            <asp:PostBackTrigger ControlID="gvShipBoardTasks" />
        </triggers>
    </form>
</body>
</html>

