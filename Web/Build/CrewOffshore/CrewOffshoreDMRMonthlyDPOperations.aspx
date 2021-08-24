<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRMonthlyDPOperations.aspx.cs"
    Inherits="CrewOffshoreDMRMonthlyDPOperations" %>

<%@ Import Namespace="System.Data" %>
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
<head runat="server">
    <title>DMR Monthly Report</title>
    <telerik:RadCodeBlock ID="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
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
                <eluc:Status runat="server" ID="Status1" />


                <eluc:TabStrip ID="MenuReportTap" TabStrip="true" runat="server" OnTabStripCommand="ReportTapp_TabStripCommand"></eluc:TabStrip>

                <div>
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Vessel ID="ucVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true"
                                    AppendDataBoundItems="true" Width="150px" AutoPostBack="true" OnTextChangedEvent="ucVessel_OnTextChangedEvent" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblmMonthandYear" runat="server" Text="Month and Year"></telerik:RadLabel>
                            </td>
                            <td colspan="3">
                                <telerik:RadDropDownList ID="ddlMonth" runat="server" AutoPostBack="true"
                                    OnTextChanged="ddlMonth_TextChangedEvent">
                                    <Items>


                                        <telerik:DropDownListItem Value="" Text="--Select--"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="1" Text="Jan"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="2" Text="Feb"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="3" Text="Mar"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="4" Text="Apr"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="5" Text="May"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="6" Text="Jun"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="7" Text="Jul"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="8" Text="Aug"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="9" Text="Sep"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="10" Text="Oct"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="11" Text="Nov"></telerik:DropDownListItem>
                                        <telerik:DropDownListItem Value="12" Text="Dec"></telerik:DropDownListItem>
                                    </Items>
                                </telerik:RadDropDownList>
                                &nbsp;&nbsp;
                                   <telerik:RadDropDownList ID="ddlYear" runat="server" AutoPostBack="true"
                                       OnTextChanged="ddlMonth_TextChangedEvent">
                                   </telerik:RadDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblNoOfDays" runat="server" Text="No of Days & Hours"></telerik:RadLabel>
                            </td>
                            <td colspan="3">
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtNoOfDays" runat="server" CssClass="readonlytextbox" Width="70px">
                                </telerik:RadTextBox>
                                &nbsp;
                                    <%--<telerik:RadLabel ID="lblNoOfHours" runat="server" Text="No of Hours"></telerik:RadLabel>--%>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtNoOfHours" runat="server" CssClass="readonlytextbox" Width="70px">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <hr />
                <div id="divGrid" style="position: relative; z-index: 0">
                    <%--  <asp:GridView ID="gvDPOperation" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowDataBound="DPOperation_ItemDataBound" OnRowCreated="gvDPOperation_OnRowCreated"
                            AllowSorting="true" ShowHeader="true" EnableViewState="false">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvDPOperation" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None"
                        OnNeedDataSource="gvDPOperation_NeedDataSource">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDMIDNIGHTREPORTDATE" TableLayout="Fixed" Height="10px">
                            <HeaderStyle Width="102px" />
                            <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                            <ColumnGroups>
                                <telerik:GridColumnGroup HeaderText="1st Operation" Name="1st" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                                 <telerik:GridColumnGroup HeaderText="2nd Operation" Name="2nd" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                                   <telerik:GridColumnGroup HeaderText="3rd Operation" Name="3rd" HeaderStyle-HorizontalAlign="Center">
                                </telerik:GridColumnGroup>
                            </ColumnGroups>
                            <Columns>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblDateHdr" runat="server" Text="Date"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDMIDNIGHTREPORTDATE") )%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="1st" HeaderStyle-Width="50px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblFrom1hdr" runat="server" Text="From"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFrom1" runat="server" Text='<%# string.Format("{0:HH:mm}",((DataRowView)Container.DataItem)["FLDFIRSTOPERATIONFROMDATETIME"])%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="1st" HeaderStyle-Width="50px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblTo1Hdr" runat="server" Text="To"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTo1" runat="server" Text='<%# string.Format("{0:HH:mm}",((DataRowView)Container.DataItem)["FLDFIRSTOPERATIONTODATETIME"])%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="1st" HeaderStyle-Width="50px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblhrs1Hdr" runat="server" Text="Hrs"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblHrs1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTOPERATIONTIMEDURATION") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="2nd" HeaderStyle-Width="50px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblFrom2hdr" runat="server" Text="From"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFrom2" runat="server" Text='<%# string.Format("{0:HH:mm}",((DataRowView)Container.DataItem)["FLDSECONDOPERATIONFROMDATETIME"])%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="2nd" HeaderStyle-Width="50px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblTo2Hdr" runat="server" Text="To"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTo2" runat="server" Text='<%# string.Format("{0:HH:mm}",((DataRowView)Container.DataItem)["FLDSECONDOPERATIONTODATETIME"])%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="2nd" HeaderStyle-Width="50px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblhrs2Hdr" runat="server" Text="Hrs"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblHrs2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSECONDOPERATIONTIMEDURATION") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="3rd" HeaderStyle-Width="50px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblFrom3hdr" runat="server" Text="From"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFrom3" runat="server" Text='<%# string.Format("{0:HH:mm}",((DataRowView)Container.DataItem)["FLDTHIRDOPERATIONFROMDATETIME"])%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="3rd" HeaderStyle-Width="50px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblTo3Hdr" runat="server" Text="To"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTo3" runat="server" Text='<%# string.Format("{0:HH:mm}",((DataRowView)Container.DataItem)["FLDTHIRDOPERATIONTODATETIME"])%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn ColumnGroupName="3rd" HeaderStyle-Width="50px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblhrs3Hdr" runat="server" Text="Hrs"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblHrs3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTHIRDOPERATIONTIMEDURATION") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblTotalHrsHdr" runat="server" Text="Total Hrs"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTotalHrs" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALHRS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="7%" />
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblHeaderDPDayYN" runat="server" Text="DP Day"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDPDayYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDPDAYYN") %>'></telerik:RadLabel>
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
                <eluc:Status runat="server" ID="ucStatus" />
            </div>

        </div>
    </form>
</body>
</html>
