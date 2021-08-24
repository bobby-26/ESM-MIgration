<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRMonthlyReportList.aspx.cs" Inherits="CrewOffshoreDMRMonthlyReportList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlOffshoreVessel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DMR MidNight Report List</title>
    <telerik:RadCodeBlock ID="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <div>

            <div>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

                <div class="Header">
                    <div style="font-weight: 600; font-size: 12px;" runat="server">
                        <eluc:TabStrip ID="MenuReportTab" TabStrip="true" runat="server"></eluc:TabStrip>
                    </div>
                </div>
                <div>
                    <table runat="server" id="tblSearch">
                        <tr>
                            <td>
                                <eluc:TabStrip ID="MenuReportList" runat="server" OnTabStripCommand="ReportList_TabStripCommand"></eluc:TabStrip>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Vessel ID="ucVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true" AutoPostBack="true" OnTextChangedEvent="ucVessel_TextChangedEvent"
                                    AppendDataBoundItems="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <%--   <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowCommand="gvReport_RowCommand" OnRowDataBound="gvReport_ItemDataBound"
                            OnRowDeleting="gvReport_RowDeleting" OnRowCreated="gvReport_RowCreated" AllowSorting="true"
                            OnRowEditing="gvReport_RowEditing" OnSorting="gvReport_Sorting" ShowHeader="true"
                            EnableViewState="false" DataKeyNames="FLDMONTHLYREPORTID">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvReport" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None"
                        OnItemDataBound="gvReport_ItemDataBound"
                        OnNeedDataSource="gvReport_NeedDataSource"
                        OnItemCommand ="gvReport_ItemCommand">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDMONTHLYREPORTID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                            <HeaderStyle Width="102px" />
                            <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                            <Columns>
                                <telerik:GridTemplateColumn HeaderStyle-Width ="40%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel Name"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width ="40%">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblReportDate" runat="server" Text="Report Date"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReportID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTHLYREPORTID") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkReportID" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATE")) %>'
                                            runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderStyle-Width ="20%">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="120px"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                            <telerik:RadLabel ID="lbllAction" runat="server" Text="Action"></telerik:RadLabel>
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                    <ItemTemplate>
                                        <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit" 
                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete" 
                                            CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                            ToolTip="Delete">
                                            <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>
                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="DatewiseDMRMonthlyReport" 
                                            CommandName="DATEWISEREPORT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDatewiseReport"
                                            ToolTip="Summary">
                                            <span class="icon"><i class="fas fa-clipboard-list"></i></span>
                                        </asp:LinkButton>
                                        <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="DatewiseDMRMonthlyReport(Excel)" 
                                            CommandName="DATEWISEREPORTEXCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDatewiseExcel"
                                            ToolTip="Summary Excel">
                                            <span class="icon"><i class="fas fa-file-excel"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>
               
            </div>

        </div>
    </form>
</body>
</html>
