<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersMaintenanceWorkOrder.aspx.cs"
    Inherits="OwnersMaintenanceWorkOrder" %>

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
    <title>Maintenance Due</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmWorkOrder" DecoratedControls="All" />
    <form id="frmWorkOrder" runat="server">
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
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="Date Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" DatePicker="true" />
                        <eluc:Date ID="txtToDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblComponentName" runat="server" Text="Component Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentName" runat="server"  MaxLength="15" Width="180px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblWorkOrderNumber" runat="server" Text="Work Order Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtWorkordernumber" runat="server"  MaxLength="15" Width="180px"></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblWorkOrderTitle" runat="server" Text="Work Order Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtWorkorderName" runat="server"  MaxLength="50" Width="180px"></telerik:RadTextBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Responsibility"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Discipline ID="ucDiscipline" runat="server"  AppendDataBoundItems="true" Width="180px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDefectList" runat="server" Text="Defect List"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkDefect" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList runat="server" ID="ddlPriority" Width="80px">
                            <Items>
                                <telerik:DropDownListItem Text="--Select--" Value="" />
                                <telerik:DropDownListItem Text="1" Value="1" />
                                <telerik:DropDownListItem Text="2" Value="2" />
                                <telerik:DropDownListItem Text="3" Value="3" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                    <td>
                        <img id="Img1" src="<%$ PhoenixTheme:images/red-symbol.png%>" runat="server" />

                        <telerik:RadLabel ID="lblOverdue" runat="server" Text="* Overdue"></telerik:RadLabel>
                    </td>
                    <td>
                        <img id="Img2" src="<%$ PhoenixTheme:images/yellow-symbol.png%>" runat="server" />

                        <telerik:RadLabel ID="lblDue" runat="server" Text="* Overdue"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuMaintenanceDue" runat="server" OnTabStripCommand="MenuMaintenanceDue_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkOrder" Height="80%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvWorkOrder_ItemCommand" OnItemDataBound="gvWorkOrder_ItemDataBound" EnableViewState="false"
                ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvWorkOrder_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" TableLayout="Fixed" ItemStyle-Wrap="false" HeaderStyle-Wrap="false" HeaderStyle-Width="100%" GroupHeaderItemStyle-Wrap="false" Width="100%" CommandItemDisplay="Top">
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Date" Name="Date" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Approved" Name="Approved" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Work Order" Name="Workorder" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Component" Name="Component" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="">
                            <HeaderStyle Width="35px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgFlag" runat="server" Visible="false" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Number" ColumnGroupName="Workorder">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJobId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkJobID" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>'>
                                </asp:LinkButton>
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
                        <telerik:GridTemplateColumn HeaderText="Priority">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDPLANINGPRIORITY"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Resp Discipline">
                            <HeaderStyle Width="130px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDiscipline" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="110px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblstatusdesc" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDHARDNAME"]%>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDHARDNAME"]%>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Due" ColumnGroupName="Date">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDuedate" runat="server" Text='<%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDPLANNINGDUEDATE"])%>' ToolTip='<%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDPLANNINGDUEDATE"])%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Done" ColumnGroupName="Date">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastdonedate" runat="server" Text='<%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDLASTDONEDATE"])%>' ToolTip='<%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDLASTDONEDATE"])%>'></telerik:RadLabel>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="By" ColumnGroupName="Approved">
                            <HeaderStyle Width="120px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblapprovedby" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDAPPROVEDBY"]%>' ToolTip='<%#((DataRowView)Container.DataItem)["FLDAPPROVEDBY"]%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" ColumnGroupName="Approved">
                            <HeaderStyle Width="90px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDAPPROVEDDATE"])%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="90px" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblRunHourSince" runat="server" Text="RunHour Since"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <%#((DataRowView)Container.DataItem)["FLDRUNHOURSINCE"]%>
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
