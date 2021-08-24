<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PortalSeafarerTrainingTasksCompleted.aspx.cs" Inherits="PortalSeafarerTrainingTasksCompleted" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToBeDoneBy" Src="~/UserControls/UserControlOffshoreToBeDoneBy.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <telerik:RadScriptManager ID="ToolkitScriptManager1"
                runat="server">
            </telerik:RadScriptManager>
            <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
            </telerik:RadWindowManager>
            <telerik:RadAjaxPanel runat="server" ID="pnlCrewRecommendedCourses">

                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server" />
                <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
                <eluc:TabStrip ID="TrainingNeed" runat="server" TabStrip="true" OnTabStripCommand="TrainingNeed_TabStripCommand"></eluc:TabStrip>

                <table width="100%">
                    <tr align="left">
                        <td>
                            <telerik:RadLabel ID="lblfileno" runat="server" Text="File No."></telerik:RadLabel>

                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtfileno" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblfirstname" runat="server" Text="Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="lblfname" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRANKname" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td></td>
                        <td>
                            <telerik:RadTextBox ID="lblrname" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <telerik:RadRadioButtonList ID="rbtoption" runat="server" AutoPostBack="True" OnSelectedIndexChanged="rbtoption_SelectedIndexChanged" Direction="Horizontal">
                    <Items>
                        <telerik:ButtonListItem Text="Pending" Value="PENDING"></telerik:ButtonListItem>
                        <telerik:ButtonListItem Text="Completed" Value="COMPLETED"></telerik:ButtonListItem>
                        <telerik:ButtonListItem Text="Overriden" Value="OVERRIDEN"></telerik:ButtonListItem>
                    </Items>
                </telerik:RadRadioButtonList>
                <telerik:RadLabel ID="lblPendingRecommendedcbt" runat="server" Text="Pending Recommended CBT"
                    Font-Bold="true">
                </telerik:RadLabel>
                <eluc:TabStrip ID="MenuOffshoreTraining1" runat="server" OnTabStripCommand="MenuOffshoreTraining1_TabStripCommand"></eluc:TabStrip>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvTask" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowSorting="true"
                    Width="100%" CellPadding="3" ShowFooter="false" GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AllowPaging="true" AllowCustomPaging="true" Height=""
                    OnNeedDataSource="gvTask_NeedDataSource"
                    OnItemDataBound="gvTask_ItemDataBound"
                    ShowHeader="true" EnableViewState="false" DataKeyNames="FLDTRAININGNEEDID">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                        <CommandItemSettings ShowRefreshButton="false" ShowAddNewRecordButton="false" />

                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Category" HeaderStyle-Width="200px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEmployeeid" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>

                                    <telerik:RadLabel ID="lblcategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Sub Category" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblsubcategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBCATEGORYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Task Name" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbltaskname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Description" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbltaskdes" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKDESCRIPTION") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Level" HeaderStyle-Width="70px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbllevel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVEL") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="To be done" HeaderStyle-Width="100px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbltobedone" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOBEDONEBYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblstatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKSTATUS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Assess By" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblassessby" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSESSEDBY") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDateCompleted" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDASSESSEDDATE")) %>'></telerik:RadLabel>

                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Comments" HeaderStyle-Width="150px">
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblcomments" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASSESSORCOMMENT") %>'></telerik:RadLabel>

                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Status" Visible="false" HeaderStyle-Width="75px">
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Override" CommandName="OVERRIDE" CommandArgument="<%# Container.DataItem %>" ID="cmdOverride" ToolTip="Training Need Override">
                                  <span class="icon"><i class="fas fa-calendar-times"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </telerik:RadAjaxPanel>
        </div>
    </form>
</body>
</html>
