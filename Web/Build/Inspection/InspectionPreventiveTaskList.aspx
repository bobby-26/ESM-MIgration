<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPreventiveTaskList.aspx.cs" Inherits="InspectionPreventiveTaskList" %>

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

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Long Term Action List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvPlanner").height(browserHeight - 98);
            });

        </script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvLongTermAction.ClientID %>"));
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
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" Localization-OK="Yes" Localization-Cancel="No" Width="100%">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Title runat="server" ID="Title1" Text="Preventive Task List" Visible="false" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:TabStrip ID="MenuLongTermAction" runat="server" OnTabStripCommand="MenuLongTermAction_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvLongTermAction" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                OnSorting="gvLongTermAction_Sorting" Width="100%" CellPadding="3" OnItemCommand="gvLongTermAction_ItemCommand"
                OnItemDataBound="gvLongTermAction_ItemDataBound" OnNeedDataSource="gvLongTermAction_NeedDataSource" OnSortCommand="gvLongTermAction_SortCommand"
                AllowSorting="true" ShowFooter="false" GroupingEnabled="false" EnableHeaderContextMenu="true"
                ShowHeader="true" EnableViewState="false" DataKeyNames="FLDINSPECTIONPREVENTIVEACTIONID"
                OnRowCreated="gvLongTermAction_RowCreated">
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
                        <telerik:GridTemplateColumn HeaderText="" HeaderStyle-Width="98px" AllowSorting="true" SortExpression="FLDVESSELNAME" ShowSortIcon="true">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="6%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkVesselHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME">Vessel</asp:LinkButton>
                                <img id="FLDVESSELNAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSourceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONNONCONFORMITYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSourceType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCETYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Source" HeaderStyle-Width="130px" AllowSorting="true" SortExpression="FLDCREATEDFROM" ShowSortIcon="true">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="17%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDFROM") %>' Visible="false"></telerik:RadLabel>
                                <asp:LinkButton ID="lnkTaskSource" runat="server" CommandName="SHOWSOURCE" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDFROMNAME") %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblCreatedFrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDFROMNAME") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="45px" AllowSorting="true" SortExpression="FLDTYPE" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="3%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Task" HeaderStyle-Width="155px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLongTermActionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONPREVENTIVEACTIONID") %>'></telerik:RadLabel>
                                <%--<asp:LinkButton ID="lnkTask" runat="server" CommandName="SELECT" CommandArgument='<%# Container.DataItemIndex %>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVENTIVEACTION").ToString().Length>40 ? DataBinder.Eval(Container, "DataItem.FLDPREVENTIVEACTION").ToString().Substring(0, 40)+ "..." : DataBinder.Eval(Container, "DataItem.FLDPREVENTIVEACTION").ToString() %>'></asp:LinkButton>--%>
                                <%--<telerik:RadLabel ID="lblTask" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVENTIVEACTION").ToString().Length>40 ? DataBinder.Eval(Container, "DataItem.FLDPREVENTIVEACTION").ToString().Substring(0, 40)+ "..." : DataBinder.Eval(Container, "DataItem.FLDPREVENTIVEACTION").ToString() %>' ></telerik:RadLabel>--%>
                                <asp:LinkButton ID="lnkTask" Width="98%" runat="server" CommandName="NAVIGATE" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVENTIVEACTION").ToString() %>'></asp:LinkButton>
                                <eluc:ToolTip ID="ucToolTip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPREVENTIVEACTION")%>' TargetControlId="lnkTask" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="170px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="12%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkCategoryHeader" runat="server" CommandName="Sort" CommandArgument="FLDCATEGORY">Category</asp:LinkButton>
                                <img id="FLDCATEGORY" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKCATEGORYNAME").ToString()%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCategoryShortCode" Visible="false" runat="server" Width="100" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKCATEGORYSHORTCODE") %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKCATEGORYNAME")%>' TargetControlId="lblCategory" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub Category" HeaderStyle-Width="120px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="14%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKSUBCATEGORYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="85px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="4%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Department" HeaderStyle-Width="116px">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkDepartmentHeader" runat="server" CommandName="Sort" CommandArgument="FLDDEPARTMENTNAME">Department</asp:LinkButton>
                                <img id="FLDDEPARTMENTNAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAssignedDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sub Department" HeaderStyle-Width="113px" Visible="false">
                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSubDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBDEPARTMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Target" HeaderStyle-Width="75px" AllowSorting="true" SortExpression="FLDTARGETDATE" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTargetDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDTARGETDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed" HeaderStyle-Width="77px" AllowSorting="true" SortExpression="FLDCOMPLETIONDATE" ShowSortIcon="true">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCompletedDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCOMPLETIONDATE")) %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="70px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="2%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Show Pending Tasks in Vessels" CommandName="SHOWSTATUS" ID="cmdShowStatus"
                                    ToolTip="Show Pending Tasks in Vessels" Visible="false">
                                <span class="icon"><i class="fas fa-list-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAtt"
                                    ToolTip="Upload Evidence">
                                <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                                <%--<asp:LinkButton runat="server" AlternateText="Communication"
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
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
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
            <asp:PostBackTrigger ControlID="gvLongTermAction" />
        </triggers>
    </form>
</body>
</html>
