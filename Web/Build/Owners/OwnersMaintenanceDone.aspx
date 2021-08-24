<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMaintenanceDone.aspx.cs"
    Inherits="OwnersMaintenanceDone" %>

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlOwnersVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserVessel" Src="~/UserControls/UserControlUserVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Maintenance Done</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmWorkOrderReportList" DecoratedControls="All" />
    <form id="frmWorkOrderReportList" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="dropdown_mandatory" Width="180px" />
                        <eluc:UserVessel ID="ddlUserVessel" runat="server" CssClass="dropdown_mandatory" Width="180px" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDoneFromDate" runat="server" Text="Done Date Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="input" DatePicker="true" />

                        <eluc:Date ID="txtToDate" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWorkOrderNumber" runat="server" Text="Work Order Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtWorkordernumber" runat="server" CssClass="input" MaxLength="15" Width="180px"></telerik:RadTextBox>
                        <telerik:RadCheckBox ID="chkDefect" runat="server" Text="Defect List" />
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblComponentName" runat="server" Text="Component Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input" MaxLength="15" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWorkOrderTitle" runat="server" Text="Work Order Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtWorkorderName" runat="server" CssClass="input" MaxLength="50" Width="180px"></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Responsibility"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Discipline ID="ucDiscipline" runat="server" CssClass="input" AppendDataBoundItems="true" Width="180px" />
                    </td>

                </tr>
            </table>

            <eluc:TabStrip ID="MenuMaintenanceDone" runat="server" OnTabStripCommand="MenuMaintenanceDone_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkOrderReport" Height="85%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvWorkOrderReport_ItemCommand" OnItemDataBound="gvWorkOrderReport_ItemDataBound" EnableViewState="false"
                ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvWorkOrderReport_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" TableLayout="Fixed" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-Width="100%" GroupHeaderItemStyle-Wrap="false" Width="100%" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <ColumnGroups>

                        <telerik:GridColumnGroup HeaderText="Work Order" Name="Workorder" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Component" Name="Component" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="">
                            <HeaderStyle Width="35px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Attachment"
                                    CommandName="Attachment" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                    ToolTip="Attachment"><span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number" ColumnGroupName="Workorder">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkJobID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Title" ColumnGroupName="Workorder">
                            <HeaderStyle Width="200px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblwoname" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number" ColumnGroupName="Component">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcomno" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" ColumnGroupName="Component">
                            <HeaderStyle Width="150px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcomname" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Class Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="80px" />
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDCLASSCODE"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Frequency">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblfrequency" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDFREQUENCYNAME"]%>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDFREQUENCYNAME"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Last Done On">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastdonedate" runat="server" Text='<%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDWORKDONEDATE"])%>' ToolTip='<%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDWORKDONEDATE"])%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Job Class">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobclass" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDJOBCLASS"]%>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDJOBCLASS"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Duration">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDWORKDURATION"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Started">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                <%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDWORKORDERSTARTEDDATE"])%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Completed">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                <%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDWORKORDERCOMPLETEDDATE"])%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="History">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="WORKORDERID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="DONEID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDONEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="FORMURL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMURL") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="REPORTHISTORY" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTHISTORY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="HISTORY" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHISTORY") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <asp:LinkButton runat="server" AlternateText="History"
                                    CommandName="HISTORY" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdHistory"
                                    ToolTip="History"><span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>

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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="5" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" ResizeGridOnColumnResize="false" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
