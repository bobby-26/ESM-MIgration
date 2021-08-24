<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreTrainingNeeds.aspx.cs"
    Inherits="CrewOffshoreTrainingNeeds" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>


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
    <title>Crew Recommended Courses</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewRecommendedCourses" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">


                <%--<eluc:TabStrip ID="TrainingNeed" runat="server" TabStrip="true" OnTabStripCommand="TrainingNeed_TabStripCommand"></eluc:TabStrip>--%>

                <eluc:TabStrip ID="MenuOffshoreTraining" runat="server" OnTabStripCommand="MenuOffshoreTraining_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: 1">
                    <%-- <asp:GridView ID="gvOffshoreTraining" runat="server" AutoGenerateColumns="False"
                    Font-Size="11px" Width="100%" CellPadding="3" ShowFooter="false" OnRowCommand="gvOffshoreTraining_RowCommand"
                    ShowHeader="true" EnableViewState="false" OnRowDataBound="gvOffshoreTraining_RowDataBound"
                    AllowSorting="true" OnSorting="gvOffshoreTraining_Sorting"
                    OnDataBound="OnDataBound">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvOffshoreTraining" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOffshoreTraining_NeedDataSource"
                        OnItemCommand="gvOffshoreTraining_ItemCommand"
                        OnItemDataBound="gvOffshoreTraining_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
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
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="CBT" Name="CBT" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="Training Course" Name="Training" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                              <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="Task" Name="Task" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <Columns>
                                  <telerik:GridTemplateColumn HeaderText="Vessel" UniqueName="vessel">
                                    <HeaderStyle Width="150px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblvessel" runat="server"  Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                      
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name">
                                    <HeaderStyle Width="200px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>' CommandName="GETEMPLOYEEDETAILS"></asp:LinkButton>

                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="File No">
                                    <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rank">
                                    <HeaderStyle Width="75px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Pending" ColumnGroupName="CBT">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblcbtpending" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGCBTCOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Completed" ColumnGroupName="CBT">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTrainingpending" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDCBTCOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Overridden" ColumnGroupName="CBT">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblcompleted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERRIDECBTCOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Pending" ColumnGroupName="Training">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblcompleted2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGTRAINIGCOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Completed" ColumnGroupName="Training">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblover2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDTRAININGCOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Overridden" ColumnGroupName="Training">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbloverride" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERRIDETRAININGCOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                       <telerik:GridTemplateColumn HeaderText="Pending" ColumnGroupName="Task">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbltaskcompleted2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPENDINGTASKCOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Completed" ColumnGroupName="Task">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbltaskover2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDTASKCOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Overridden" ColumnGroupName="Task">
                                    <HeaderStyle Width="60px" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbltaskoverride" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERRIDETASKCOUNT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>

                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="employeeDetail" 
                                            CommandName="GETEMPLOYEE" CommandArgument="<%# Container.DataSetIndex %>" ID="emplyeedetail"
                                            ToolTip="View Detail">
                                            <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>

</body>
</html>
