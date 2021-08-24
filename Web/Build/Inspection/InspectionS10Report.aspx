<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionS10Report.aspx.cs" Inherits="InspectionS10Report" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>S10 Report</title>
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
            <eluc:Title Text="S10 - Navigation Performance Analysis" ID="ucTitle" runat="server" Visible="false"
                ShowMenu="true" />
            <table id="tblWI" width="60%" runat="server">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFrom" runat="server" Text="From"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlFrommonth" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="1" Text="Jan"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2" Text="Feb"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="3" Text="Mar"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="4" Text="Apr"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="5" Text="May"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="6" Text="Jun"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="7" Text="Jul"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="8" Text="Aug"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="9" Text="Sep"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="10" Text="Oct"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="11" Text="Nov"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="12" Text="Dec"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                        <telerik:RadComboBox ID="ddlFromYear" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlTomonth" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="1" Text="Jan"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="2" Text="Feb"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="3" Text="Mar"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="4" Text="Apr"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="5" Text="May"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="6" Text="Jun"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="7" Text="Jul"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="8" Text="Aug"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="9" Text="Sep"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="10" Text="Oct"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="11" Text="Nov"></telerik:RadComboBoxItem>
                                <telerik:RadComboBoxItem Value="12" Text="Dec"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                        <telerik:RadComboBox ID="ddlToYear" runat="server" CssClass="input_mandatory" AutoPostBack="true"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuS10Report" runat="server" OnTabStripCommand="MenuS10Report_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr >
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblSummary" runat="server" Text="1) Total Audits/Observations"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr >
                    <td >
                        <telerik:RadLabel ID="lblGridAuditObservation" runat="server" Text="" width="100%"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblNavAuditSummary" runat="server" Text="2) Navigation Audits by Internal / External Parties"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvNavAuditSummary" runat="server" AutoGenerateColumns="False" Font-Size="11px" GroupingEnabled="false" EnableHeaderContextMenu="true"
                            Width="100%" CellPadding="3" OnRowCreated="gvNavAuditSummary_RowCreated" AllowSorting="true" OnNeedDataSource="gvNavAuditSummary_NeedDataSource"
                            ShowFooter="false" ShowHeader="true" EnableViewState="false">
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
                                    <telerik:GridTemplateColumn HeaderText="Year" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERIODNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Total No. of Audits" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblIntTotalAudits" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAUDITSINTERNAL") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Total No. of Observations" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblIntTotalObs" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALOBSERVATIONINTERNAL") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Average Observations/inspection" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblIntAverage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVERAGEINTERNAL") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Total No. of Audits" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblExtTotalAudits" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALAUDITSEXTERNAL") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Total No. of Observations" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblExtTotalObs" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALOBSERVATIONEXTERNAL") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Average Observations/inspection" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblExtAverage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVERAGEEXTERNAL") %>'></telerik:RadLabel>
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
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblCategorywiseBreakup" runat="server" Text="3) Category wise break up of these observations by the external parties"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCategorywiseBreakupObs" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCreated="gvCategorywiseBreakupObs_RowCreated" AllowSorting="true" GroupingEnabled="false" EnableHeaderContextMenu="true"
                            ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvCategorywiseBreakupObs_NeedDataSource">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false">
                                <ColumnGroups>
                                    <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                </ColumnGroups>
                                <NoRecordsTemplate>
                                    <table width="99.9%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Category">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Total Observations" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALOBS") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Observations/inspection" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAverage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVERAGE") %>'></telerik:RadLabel>
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
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblNavSummary" runat="server" Text="4) Summary of all the Navigation audits"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGridSummary" runat="server" Text="" Width="100%"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblCategorybreakupsummary" runat="server" Text="5) Breakup of observations as per categories"></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCategorywiseBreakupSummary" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCreated="gvCategorywiseBreakupSummary_RowCreated" AllowSorting="true" GroupingEnabled="false" EnableHeaderContextMenu="true"
                            ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvCategorywiseBreakupSummary_NeedDataSource">
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
                                    <telerik:GridTemplateColumn HeaderText="Category">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Total Observations" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTotal" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALOBS") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Observations/inspection" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAverage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVERAGE") %>'></telerik:RadLabel>
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
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
