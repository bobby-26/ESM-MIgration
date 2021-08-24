<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionWeeklyReport.aspx.cs" Inherits="InspectionWeeklyReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Weekly Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAuditSummary" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" Visible="false" runat="server" Text="" />

            <%--  <eluc:Title Text="Weekly Management Review" ID="ucTitle" runat="server"
                ShowMenu="true" />--%>

            <table id="tblWI" width="100%" runat="server">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselByCompany runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input"
                            VesselsOnly="true" AutoPostBack="true" AssignedVessels="true" OnTextChangedEvent="ucVessel_Changed" Width="40%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbldue" Text="Due / OverDue" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblDueOverdue" runat="server" Width="50%" CssClass="input"
                            RepeatDirection="Horizontal">
                            <asp:ListItem Text="Due" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Over Due" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastDateRange" runat="server" Text="Last"></telerik:RadLabel>
                    </td>
                    <td>


                        <telerik:RadComboBox ID="ddlPastDateRange" runat="server" CssClass="input" AutoPostBack="true"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" Width="40%">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="1 Week" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="2 Weeks" Value="2" Selected="True"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="1 Month" Value="3"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFutureDateRange" runat="server" Text="Next"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlFutureDateRange" runat="server" CssClass="input" AutoPostBack="true" Width="50%"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="1 Week" Value="1"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="2 Weeks" Value="2" Selected="True"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Text="1 Month" Value="3"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="RiskAssessment Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlStatus" runat="server" AppendDataBoundItems="true" Width="40%"
                            CssClass="input">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <br />

            <eluc:TabStrip ID="MenuWeeklyReport" runat="server" OnTabStripCommand="MenuWeeklyReport_TabStripCommand"></eluc:TabStrip>

                <br />
                <b>
                    <telerik:RadLabel ID="lblshipboardtask" runat="server" Text="1) Fleet Wise Corrective Tasks "></telerik:RadLabel>
                </b>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvshipboardtask" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" AllowSorting="true" GroupingEnabled="false" OnItemDataBound="gvshipboardtask_ItemDataBound" EnableHeaderContextMenu="true"
                    ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvshipboardtask_NeedDataSource">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false">
                        <Columns>
                            <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                            <telerik:GridTemplateColumn HeaderText="Fleet">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFleet" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLEETNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="OVERDUE">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblovedue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDUE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="OPEN">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblopentask" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENTASKCOUNT") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="COMPLETED">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblcompleted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDCOUNT") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="TOTAL">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>'></telerik:RadLabel>
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
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

                <br />
                <br />
                <b>
                    <telerik:RadLabel ID="lblPreventivetask" runat="server" Text="2) Fleet Wise Preventive Tasks"></telerik:RadLabel>
                </b>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvpreventivetask" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" AllowSorting="true" GroupingEnabled="false" OnItemDataBound="gvpreventivetask_ItemDataBound" EnableHeaderContextMenu="true"
                    ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvpreventivetask_NeedDataSource">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false">
                        <Columns>
                            <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                            <telerik:GridTemplateColumn HeaderText="Fleet">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFleet" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLEETNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="OVERDUE">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblovedue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDUE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="OPEN">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblopentask" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENTASKCOUNT") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="COMPLETED">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblcompleted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPLETEDCOUNT") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="TOTAL">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTAL") %>'></telerik:RadLabel>
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
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

                <br />
                <br />
                <b>
                    <telerik:RadLabel ID="lblofficetask" runat="server" Text="3) Department Wise Office Tasks"></telerik:RadLabel>
                </b>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvofficetask" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" AllowSorting="true" GroupingEnabled="false" OnItemDataBound="gvofficetask_ItemDataBound" EnableHeaderContextMenu="true"
                    ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvofficetask_NeedDataSource">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false">
                        <Columns>
                            <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                            <telerik:GridTemplateColumn HeaderText="Department">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Count">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKCOUNT") %>'></telerik:RadLabel>
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
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

                <br />
                <br />
                <b>
                    <telerik:RadLabel ID="lblRiskassessment" runat="server" Text="4) Risk Assessment"></telerik:RadLabel>
                </b>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvRiskassessment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" AllowSorting="true" GroupingEnabled="false" OnItemDataBound="gvRiskassessment_ItemDataBound" EnableHeaderContextMenu="true"
                    ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvRiskassessment_NeedDataSource">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false">

                        <Columns>
                            <%--  <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                            <telerik:GridTemplateColumn HeaderText="Risk Assessment Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRAType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRATYPENAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Count">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRACOUNT") %>'></telerik:RadLabel>
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
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

                <br />
                <br />
          
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

