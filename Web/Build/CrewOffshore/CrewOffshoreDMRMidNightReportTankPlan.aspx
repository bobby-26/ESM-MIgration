<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRMidNightReportTankPlan.aspx.cs"
    Inherits="CrewOffshoreDMRMidNightReportTankPlan" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Product" Src="~/UserControls/UserControlDMRProduct.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="overflow-y: hidden">
<head id="Head1" runat="server">
    <title>DMR MidNight Report</title>
    <telerik:RadCodeBlock ID="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-1.4.2.min.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">

        <div>
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />


            <eluc:TabStrip ID="MenuReportTap" TabStrip="true" runat="server" OnTabStripCommand="ReportTapp_TabStripCommand"></eluc:TabStrip>



            <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand"></eluc:TabStrip>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />


            <asp:HiddenField ID="hdnScroll" runat="server" />
            <div id="divScroll" style="position: relative; z-index: 0; width: 100%; height: 600px; overflow: auto;"
                onscroll="javascript:setScroll('divScroll', 'hdnScroll');">
                <div style="top: 40px; position: relative;">
                    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAlert" runat="server" Text="" Font-Size="Large" ForeColor="Red"
                                    BorderColor="Red">
                                </telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                  
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td width="10%">
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            </td>
                            <td width="20%">
                                <eluc:Vessel ID="ucVessel" runat="server" CssClass="readonlytextbox" VesselsOnly="true"
                                    AppendDataBoundItems="true" Width="150px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCrew" runat="server" Text="Crew"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Number ID="txtCrew" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    IsInteger="true" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td>
                                <u><b>
                                    <telerik:RadLabel ID="lblTankPlan" runat="server" Text="Tank Plan"></telerik:RadLabel>
                                </b></u>
                            </td>
                        </tr>
                    </table>
                    <hr />
                </div>
                <div style="top: 40px; position: relative; margin-left: 6px">
                    <%--<asp:GridView ID="gvDryBulk" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="40%" CellPadding="3" ShowFooter="false" AllowSorting="true" Style="margin-bottom: 0px"
                        EnableViewState="false" OnRowCreated="gvDryBulk_RowCreated">
                        <FooterStyle ForeColor="#000066" BackColor="#dfdfdf"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle BackColor="#f9f9fa" />
                        <SelectedRowStyle BackColor="#bbddff" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvDryBulk" runat="server" AllowCustomPaging="true" Width="50%" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None"
                        OnNeedDataSource="gvDryBulk_NeedDataSource"
                        OnItemDataBound="gvDryBulk_ItemDataBound">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                            <HeaderStyle Width="102px" />
                            <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
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
                                <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lbloilTypeNameHeader" runat="server">
                                            Dry Bulk
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lbloilTypeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblcapacityat100percentageHeader" runat="server">
                                            100% Vol
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblcapacityat100percentage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPACITYAT100PERCENT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblcapacityat85percentageHeader" runat="server">
                                            85% Vol
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblcapacityat85percentage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPACITYAT85PERCENT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblrobmtHeader" runat="server">
                                            ROB
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblrobmt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBCUM") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="right" Width="15%"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblpercentagefillHeader" runat="server">
                                            % Fill
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblpercentagefill" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.PERCENTAGEFILL") %>'></telerik:RadLabel>
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
                    <%--<asp:GridView ID="gvLiquidBulk" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="40%" CellPadding="3" ShowFooter="false" AllowSorting="true" Style="margin-bottom: 0px"
                        EnableViewState="false">
                        <FooterStyle ForeColor="#000066" BackColor="#dfdfdf"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle BackColor="#f9f9fa" />
                        <SelectedRowStyle BackColor="#bbddff" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvLiquidBulk" runat="server" AllowCustomPaging="true" Width="50%" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None"
                        OnNeedDataSource="gvLiquidBulk_NeedDataSource">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                            <HeaderStyle Width="102px" />
                            <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
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
                                <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblRemarksHeader1" runat="server">
                                            Liquid Bulk
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblWorkingGearitemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblRemarksHeader2" runat="server">
                                            100% Vol
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lnkGearType1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPACITYAT100PERCENT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblRemarksHeader3" runat="server">
                                            85% Vol
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lnkGearType2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPACITYAT85PERCENT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblRemarksHeader4" runat="server">
                                            ROB
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lnkGearType3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBCUM") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblRemarksHeader5" runat="server">
                                            % Fill
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lnkGearType4" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.PERCENTAGEFILL") %>'></telerik:RadLabel>
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
                    <%-- <asp:GridView ID="gvMethanol" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="40%" CellPadding="3" ShowFooter="false" AllowSorting="true" Style="margin-bottom: 0px"
                        EnableViewState="false">
                        <FooterStyle ForeColor="#000066" BackColor="#dfdfdf"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle BackColor="#f9f9fa" />
                        <SelectedRowStyle BackColor="#bbddff" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvMethanol" runat="server" AllowCustomPaging="true" Width="50%" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None"
                        OnNeedDataSource="gvMethanol_NeedDataSource">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                            <HeaderStyle Width="102px" />
                            <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
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
                                <telerik:GridButtonColumn Text="DoubleClick" CommandName="Edit" Visible="false" />
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblRemarksHeader1" runat="server">
                                            Methanol
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblWorkingGearitemId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblRemarksHeader2" runat="server">
                                            100% Vol
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lnkGearType1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPACITYAT100PERCENT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblRemarksHeader3" runat="server">
                                            85% Vol
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lnkGearType2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPACITYAT85PERCENT") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblRemarksHeader4" runat="server">
                                            ROB
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lnkGearType3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBCUM") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn>
                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblRemarksHeader5" runat="server">
                                            % Fill
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lnkGearType4" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.PERCENTAGEFILL") %>'></telerik:RadLabel>
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
                <div style="top: 40px; position: relative;" id="diva" runat="server">
                    <span id="spdata" runat="server"></span>
                    <table id="tblq">
                    </table>
                    <table id="tblData" width="100%" runat="server" style="font-size: 12px">
                        <tr style="border-color: Black">
                            <td>
                                <span id="spn1" runat="server">
                                    <table id="tbl1" runat="server" width="100%" border="1">
                                        <tr>
                                            <td align="center" style="font-weight: bold" class="DataGrid-HeaderStyle">
                                                <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                                            </td>
                                            <td align="center" style="font-weight: bold" class="DataGrid-HeaderStyle">
                                                <telerik:RadLabel ID="lblPortInner" runat="server" Text="Port Inner"></telerik:RadLabel>
                                            </td>
                                            <td align="center" style="font-weight: bold" class="DataGrid-HeaderStyle">
                                                <telerik:RadLabel ID="lblCenter" runat="server" Text="Center"></telerik:RadLabel>
                                            </td>
                                            <td align="center" style="font-weight: bold" class="DataGrid-HeaderStyle">
                                                <telerik:RadLabel ID="lblstdbInner" runat="server" Text="Stbd Inner"></telerik:RadLabel>
                                            </td>
                                            <td align="center" style="font-weight: bold" class="DataGrid-HeaderStyle">
                                                <telerik:RadLabel ID="lblStbd" runat="server" Text="Stbd"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnP1" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionP1" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDP1" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDP1" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoP1I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoP1" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP1I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP1" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP1I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP1" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductP1I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductP1" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductP1" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityP1I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityP1" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--<telerik:RadLabel ID="lblSpcGravityP1" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedP1" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedP1" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTP1" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTP1" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMP1" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMP1" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitP1" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitP1" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedP1" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedP1" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedP1_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateP1" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateP1" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNP1" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableP1" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertP1" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertP1" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksP1" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksP1" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnPI1" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionPI1" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDPI1" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDPI1" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoPI1I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoPI1" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI1I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI1" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI1I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI1" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductPI1I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductPI1" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductPI1" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityPI1I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityPI1" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravityPI1" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedPI1" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedPI1" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTPI1" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTPI1" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMPI1" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMPI1" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitPI1" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitPI1" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedPI1" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedPI1" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedPI1_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDatePI1" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDatePI1" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNPI1" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpablePI1" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertPI1" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertPI1" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksPI1" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksPI1" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnC1" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionC1" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDC1" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDC1" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoC1I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoC1" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC1I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC1" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC1I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC1" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductC1I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductC1" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--   <telerik:RadLabel ID="lblProductC1" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityC1I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityC1" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityC1" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedC1" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedC1" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTC1" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTC1" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMC1" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMC1" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitC1" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitC1" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedC1" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedC1" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedC1_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateC1" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateC1" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNC1" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableC1" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertC1" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertC1" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksC1" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksC1" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnSI1" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionSI1" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDSI1" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDSI1" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoSI1I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoSI1" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI1I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI1" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI1I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI1" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductSI1I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductSI1" runat="server" CssClass="gridinput_mandatory" Width="70px" />
                                                                <%-- <telerik:RadLabel ID="lblProductSI1" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravitySI1I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravitySI1" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--<telerik:RadLabel ID="lblSpcGravitySI1" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedSI1" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedSI1" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTSI1" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTSI1" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMSI1" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMSI1" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitSI1" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitSI1" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedSI1" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedSI1" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedSI1_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateSI1" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateSI1" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNSI1" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableSI1" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertSI1" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertSI1" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksSI1" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksSI1" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnS1" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionS1" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDS1" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDS1" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoS1I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoS1" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS1I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS1" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS1I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS1" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductS1I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductS1" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductS1" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityS1I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityS1" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--   <telerik:RadLabel ID="lblSpcGravityS1" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedS1" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedS1" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTS1" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTS1" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMS1" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMS1" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitS1" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitS1" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedS1" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedS1" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedS1_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateS1" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateS1" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNS1" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableS1" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertS1" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertS1" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksS1" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksS1" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </td>
                        </tr>
                        <tr style="border-color: Black">
                            <td>
                                <span id="spn2" runat="server">
                                    <table id="tbl2" runat="server" width="100%" border="1">
                                        <tr>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnP2" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionP2" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDP2" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDP2" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoP2I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoP2" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP2I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP2" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP2I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP2" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductP2I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductP2" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductP2" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityP2I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityP2" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--<telerik:RadLabel ID="lblSpcGravityP2" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedP2" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedP2" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTP2" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTP2" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMP2" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMP2" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitP2" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitP2" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedP2" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedP2" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedP2_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateP2" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateP2" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNP2" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableP2" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertP2" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertP2" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksP2" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksP2" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnPI2" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionPI2" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDPI2" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDPI2" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoPI2I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoPI2" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI2I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI2" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI2I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI2" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductPI2I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductPI2" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductPI2" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityPI2I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityPI2" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--<telerik:RadLabel ID="lblSpcGravityPI2" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedPI2" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedPI2" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTPI2" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTPI2" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMPI2" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMPI2" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitPI2" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitPI2" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedPI2" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedPI2" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedPI2_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDatePI2" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDatePI2" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNPI2" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpablePI2" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertPI2" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertPI2" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksPI2" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksPI2" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnC2" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionC2" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDC2" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDC2" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoC2I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoC2" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC2I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC2" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC2I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC2" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductC2I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductC2" runat="server" CssClass="gridinput_mandatory" Width="70px" />
                                                                <%-- <telerik:RadLabel ID="lblProductC2" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityC2I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityC2" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityC2" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedC2" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedC2" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTC2" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTC2" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMC2" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMC2" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitC2" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitC2" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedC2" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedC2" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedC2_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateC2" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateC2" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNC2" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableC2" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertC2" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertC2" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksC2" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksC2" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnSI2" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionSI2" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDSI2" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDSI2" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoSI2I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoSI2" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI2I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI2" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI2I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI2" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductSI2I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductSI2" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductSI2" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravitySI2I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravitySI2" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--<telerik:RadLabel ID="lblSpcGravitySI2" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedSI2" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedSI2" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTSI2" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTSI2" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMSI2" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMSI2" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitSI2" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitSI2" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedSI2" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedSI2" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedSI2_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateSI2" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateSI2" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNSI2" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableSI2" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertSI2" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertSI2" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksSI2" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksSI2" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnS2" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionS2" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDS2" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDS2" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoS2I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoS2" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS2I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS2" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS2I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS2" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductS2I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductS2" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductS2" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityS2I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityS2" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityS2" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedS2" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedS2" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTS2" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTS2" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMS2" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMS2" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitS2" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitS2" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedS2" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedS2" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedS2_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateS2" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateS2" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNS2" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableS2" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertS2" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertS2" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksS2" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksS2" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </td>
                        </tr>
                        <tr style="border-color: Black">
                            <td>
                                <span id="spn3" runat="server">
                                    <table id="tbl3" runat="server" width="100%" border="1">
                                        <tr>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnP3" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionP3" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDP3" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDP3" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoP3I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoP3" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP3I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP3" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP3I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP3" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductP3I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductP3" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductP3" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityP3I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityP3" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityP3" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedP3" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedP3" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTP3" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTP3" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMP3" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMP3" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitP3" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitP3" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedP3" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedP3" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedP3_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateP3" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateP3" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNP3" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableP3" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertP3" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertP3" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksP3" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksP3" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnPI3" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionPI3" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDPI3" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDPI3" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoPI3I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoPI3" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI3I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI3" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI3I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI3" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductPI3I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductPI3" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductPI3" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityPI3I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityPI3" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--<telerik:RadLabel ID="lblSpcGravityPI3" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedPI3" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedPI3" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTPI3" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTPI3" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMPI3" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMPI3" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitPI3" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitPI3" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedPI3" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedPI3" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedPI3_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDatePI3" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDatePI3" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNPI3" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpablePI3" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertPI3" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertPI3" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksPI3" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksPI3" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnC3" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionC3" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDC3" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDC3" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoC3I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoC3" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC3I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC3" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC3I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC3" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductC3I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductC3" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductC3" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityC3I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityC3" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravityC3" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedC3" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedC3" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTC3" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTC3" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMC3" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMC3" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitC3" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitC3" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedC3" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedC3" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedC3_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateC3" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateC3" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNC3" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableC3" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertC3" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertC3" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksC3" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksC3" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnSI3" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionSI3" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDSI3" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDSI3" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoSI3I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoSI3" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI3I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI3" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI3I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI3" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductSI3I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductSI3" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductSI3" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravitySI3I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravitySI3" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravitySI3" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedSI3" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedSI3" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTSI3" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTSI3" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMSI3" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMSI3" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitSI3" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitSI3" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedSI3" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedSI3" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedSI3_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateSI3" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateSI3" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNSI3" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableSI3" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertSI3" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertSI3" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksSI3" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksSI3" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnS3" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionS3" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDS3" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDS3" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoS3I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoS3" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS3I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS3" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS3I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS3" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductS3I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductS3" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductS3" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityS3I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityS3" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--   <telerik:RadLabel ID="lblSpcGravityS3" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedS3" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedS3" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTS3" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTS3" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMS3" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMS3" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitS3" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitS3" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedS3" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedS3" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedS3_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateS3" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateS3" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNS3" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableS3" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertS3" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertS3" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksS3" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksS3" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </td>
                        </tr>
                        <tr style="border-color: Black">
                            <td>
                                <span id="spn4" runat="server">
                                    <table id="tbl4" runat="server" width="100%" border="1">
                                        <tr>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnP4" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionP4" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDP4" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDP4" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoP4I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoP4" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP4I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP4" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP4I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP4" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductP4I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductP4" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductP4" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityP4I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityP4" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravityP4" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedP4" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedP4" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTP4" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTP4" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMP4" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMP4" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitP4" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitP4" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedP4" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedP4" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedP4_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateP4" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateP4" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNP4" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableP4" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertP4" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertP4" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksP4" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksP4" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnPI4" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionPI4" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDPI4" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDPI4" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoPI4I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoPI4" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI4I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI4" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI4I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI4" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductPI4I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductPI4" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductPI4" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityPI4I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityPI4" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--<telerik:RadLabel ID="lblSpcGravityPI4" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedPI4" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedPI4" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTPI4" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTPI4" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMPI4" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMPI4" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitPI4" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitPI4" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedPI4" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedPI4" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedPI4_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDatePI4" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDatePI4" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNPI4" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpablePI4" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertPI4" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertPI4" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksPI4" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksPI4" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnC4" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionC4" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDC4" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDC4" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoC4I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoC4" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC4I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC4" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC4I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC4" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductC4I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductC4" runat="server" CssClass="gridinput_mandatory" Width="70px" />
                                                                <%-- <telerik:RadLabel ID="lblProductC4" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityC4I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityC4" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--     <telerik:RadLabel ID="lblSpcGravityC4" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedC4" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedC4" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTC4" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTC4" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMC4" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMC4" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitC4" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitC4" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedC4" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedC4" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedC4_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateC4" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateC4" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNC4" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableC4" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertC4" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertC4" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksC4" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksC4" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnSI4" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionSI4" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDSI4" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDSI4" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoSI4I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoSI4" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI4I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI4" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI4I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI4" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductSI4I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductSI4" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--  <telerik:RadLabel ID="lblProductSI4" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravitySI4I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravitySI4" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravitySI4" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedSI4" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedSI4" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTSI4" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTSI4" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMSI4" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMSI4" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitSI4" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitSI4" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedSI4" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedSI4" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedSI4_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateSI4" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateSI4" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNSI4" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableSI4" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertSI4" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertSI4" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksSI4" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksSI4" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnS4" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionS4" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDS4" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDS4" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoS4I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoS4" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS4I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS4" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS4I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS4" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductS4I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductS4" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductS4" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityS4I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityS4" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--   <telerik:RadLabel ID="lblSpcGravityS4" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedS4" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedS4" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTS4" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTS4" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMS4" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMS4" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitS4" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitS4" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedS4" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedS4" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedS4_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateS4" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateS4" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNS4" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableS4" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertS4" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertS4" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksS4" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksS4" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </td>
                        </tr>
                        <tr style="border-color: Black">
                            <td>
                                <span id="spn5" runat="server">
                                    <table id="tbl5" runat="server" width="100%" border="1">
                                        <tr>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnP5" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionP5" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDP5" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDP5" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoP5I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoP5" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP5I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP5" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP5I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP5" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductP5I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductP5" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--  <telerik:RadLabel ID="lblProductP5" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityP5I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityP5" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityP5" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedP5" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedP5" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTP5" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTP5" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMP5" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMP5" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitP5" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitP5" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedP5" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedP5" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedP5_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateP5" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateP5" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNP5" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableP5" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertP5" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertP5" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksP5" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksP5" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnPI5" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionPI5" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDPI5" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDPI5" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoPI5I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoPI5" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI5I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI5" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI5I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI5" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductPI5I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductPI5" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductPI5" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityPI5I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityPI5" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravityPI5" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedPI5" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedPI5" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTPI5" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTPI5" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMPI5" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMPI5" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitPI5" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitPI5" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedPI5" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedPI5" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedPI5_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDatePI5" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDatePI5" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNPI5" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpablePI5" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertPI5" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertPI5" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksPI5" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksPI5" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnC5" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionC5" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDC5" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDC5" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoC5I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoC5" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC5I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC5" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC5I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC5" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductC5I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductC5" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductC5" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityC5I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityC5" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityC5" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedC5" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedC5" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTC5" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTC5" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMC5" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMC5" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitC5" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitC5" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedC5" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedC5" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedC5_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateC5" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateC5" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNC5" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableC5" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertC5" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertC5" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksC5" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksC5" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnSI5" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionSI5" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDSI5" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDSI5" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoSI5I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoSI5" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI5I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI5" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI5I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI5" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductSI5I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductSI5" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--  <telerik:RadLabel ID="lblProductSI5" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravitySI5I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravitySI5" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravitySI5" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedSI5" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedSI5" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTSI5" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTSI5" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMSI5" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMSI5" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitSI5" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitSI5" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedSI5" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedSI5" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedSI5_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateSI5" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateSI5" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNSI5" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableSI5" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertSI5" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertSI5" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksSI5" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksSI5" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnS5" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionS5" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDS5" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDS5" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoS5I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoS5" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS5I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS5" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS5I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS5" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductS5I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductS5" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductS5" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityS5I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityS5" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityS5" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedS5" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedS5" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTS5" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTS5" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMS5" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMS5" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitS5" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitS5" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedS5" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedS5" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedS5_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateS5" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateS5" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNS5" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableS5" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertS5" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertS5" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksS5" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksS5" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </td>
                        </tr>
                        <tr style="border-color: Black">
                            <td>
                                <span id="spn6" runat="server">
                                    <table id="tbl6" runat="server" width="100%" border="1">
                                        <tr>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnP6" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionP6" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDP6" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDP6" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoP6I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoP6" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP6I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP6" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP6I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP6" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductP6I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductP6" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductP6" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityP6I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityP6" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravityP6" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedP6" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedP6" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTP6" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTP6" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMP6" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMP6" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitP6" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitP6" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedP6" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedP6" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedP6_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateP6" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateP6" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNP6" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableP6" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertP6" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertP6" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksP6" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksP6" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnPI6" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionPI6" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDPI6" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDPI6" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoPI6I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoPI6" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI6I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI6" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI6I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI6" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductPI6I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductPI6" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductPI6" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityPI6I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityPI6" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityPI6" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedPI6" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedPI6" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTPI6" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTPI6" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMPI6" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMPI6" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitPI6" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitPI6" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedPI6" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedPI6" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedPI6_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDatePI6" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDatePI6" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNPI6" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpablePI6" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertPI6" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertPI6" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksPI6" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksPI6" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnC6" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionC6" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDC6" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDC6" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoC6I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoC6" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC6I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC6" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC6I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC6" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductC6I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductC6" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductC6" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityC6I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityC6" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravityC6" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedC6" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedC6" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTC6" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTC6" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMC6" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMC6" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitC6" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitC6" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedC6" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedC6" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedC6_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateC6" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateC6" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNC6" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableC6" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertC6" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertC6" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksC6" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksC6" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnSI6" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionSI6" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDSI6" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDSI6" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoSI6I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoSI6" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI6I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI6" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI6I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI6" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductSI6I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductSI6" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--  <telerik:RadLabel ID="lblProductSI6" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravitySI6I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravitySI6" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--    <telerik:RadLabel ID="lblSpcGravitySI6" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedSI6" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedSI6" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTSI6" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTSI6" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMSI6" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMSI6" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitSI6" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitSI6" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedSI6" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedSI6" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedSI6_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateSI6" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateSI6" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNSI6" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableSI6" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertSI6" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertSI6" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksSI6" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksSI6" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnS6" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionS6" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDS6" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDS6" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoS6I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoS6" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS6I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS6" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS6I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS6" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductS6I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductS6" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductS6" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityS6I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityS6" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityS6" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedS6" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedS6" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTS6" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTS6" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMS6" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMS6" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitS6" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitS6" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedS6" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedS6" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedS6_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateS6" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateS6" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNS6" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableS6" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertS6" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertS6" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksS6" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksS6" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </td>
                        </tr>
                        <tr style="border-color: Black">
                            <td>
                                <span id="spn7" runat="server">
                                    <table id="tbl7" runat="server" width="100%" border="1">
                                        <tr>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnP7" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionP7" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDP7" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDP7" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoP7I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoP7" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP7I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP7" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP7I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP7" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductP7I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductP7" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductP7" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityP7I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityP7" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravityP7" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedP7" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedP7" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTP7" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTP7" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMP7" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMP7" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitP7" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitP7" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedP7" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedP7" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedP7_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateP7" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateP7" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNP7" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableP7" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertP7" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertP7" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksP7" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksP7" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnPI7" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionPI7" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDPI7" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDPI7" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoPI7I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoPI7" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI7I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI7" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI7I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI7" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductPI7I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductPI7" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductPI7" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityPI7I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityPI7" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--<telerik:RadLabel ID="lblSpcGravityPI7" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedPI7" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedPI7" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTPI7" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTPI7" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMPI7" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMPI7" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitPI7" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitPI7" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedPI7" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedPI7" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedPI7_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDatePI7" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDatePI7" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNPI7" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpablePI7" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertPI7" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertPI7" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksPI7" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksPI7" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnC7" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionC7" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDC7" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDC7" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoC7I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoC7" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC7I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC7" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC7I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC7" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductC7I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductC7" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--   <telerik:RadLabel ID="lblProductC7" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityC7I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityC7" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityC7" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedC7" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedC7" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTC7" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTC7" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMC7" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMC7" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitC7" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitC7" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedC7" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedC7" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedC7_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateC7" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateC7" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNC7" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableC7" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertC7" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertC7" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksC7" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksC7" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnSI7" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionSI7" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDSI7" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDSI7" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoSI7I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoSI7" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI7I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI7" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI7I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI7" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductSI7I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductSI7" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductSI7" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravitySI7I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravitySI7" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--    <telerik:RadLabel ID="lblSpcGravitySI7" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedSI7" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedSI7" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTSI7" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTSI7" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMSI7" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMSI7" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitSI7" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitSI7" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedSI7" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedSI7" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedSI7_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateSI7" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateSI7" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNSI7" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableSI7" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertSI7" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertSI7" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksSI7" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksSI7" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnS7" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionS7" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDS7" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDS7" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoS7I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoS7" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS7I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS7" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS7I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS7" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductS7I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductS7" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductS7" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityS7I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityS7" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--   <telerik:RadLabel ID="lblSpcGravityS7" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedS7" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedS7" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTS7" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTS7" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMS7" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMS7" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitS7" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitS7" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedS7" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedS7" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedS7_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateS7" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateS7" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNS7" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableS7" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertS7" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertS7" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksS7" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksS7" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </td>
                        </tr>
                        <tr style="border-color: Black">
                            <td>
                                <span id="spn8" runat="server">
                                    <table id="tbl8" runat="server" width="100%" border="1">
                                        <tr>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnP8" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionP8" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDP8" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDP8" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoP8I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoP8" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP8I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP8" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP8I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP8" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductP8I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductP8" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductP8" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityP8I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityP8" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityP8" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedP8" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedP8" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTP8" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTP8" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMP8" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMP8" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitP8" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitP8" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedP8" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedP8" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedP8_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateP8" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateP8" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNP8" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableP8" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertP8" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertP8" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksP8" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksP8" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnPI8" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionPI8" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDPI8" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDPI8" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoPI8I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoPI8" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI8I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI8" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI8I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI8" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductPI8I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductPI8" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductPI8" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityPI8I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityPI8" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravityPI8" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedPI8" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedPI8" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTPI8" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTPI8" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMPI8" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMPI8" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitPI8" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitPI8" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedPI8" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedPI8" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedPI8_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDatePI8" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDatePI8" runat="server" CssClass="input" AutoPostBack="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNPI8" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpablePI8" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertPI8" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertPI8" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksPI8" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksPI8" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnC8" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionC8" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDC8" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDC8" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoC8I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoC8" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC8I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC8" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC8I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC8" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductC8I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductC8" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--  <telerik:RadLabel ID="lblProductC8" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityC8I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityC8" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityC8" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedC8" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedC8" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTC8" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTC8" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMC8" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMC8" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitC8" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitC8" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedC8" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedC8" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedC8_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateC8" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateC8" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNC8" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableC8" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertC8" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertC8" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksC8" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksC8" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnSI8" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionSI8" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDSI8" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDSI8" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoSI8I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoSI8" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI8I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI8" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI8I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI8" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductSI8I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductSI8" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--  <telerik:RadLabel ID="lblProductSI8" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravitySI8I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravitySI8" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravitySI8" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedSI8" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedSI8" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTSI8" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTSI8" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMSI8" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMSI8" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitSI8" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitSI8" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedSI8" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedSI8" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedSI8_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateSI8" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateSI8" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNSI8" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableSI8" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertSI8" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertSI8" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksSI8" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksSI8" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnS8" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionS8" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDS8" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDS8" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoS8I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoS8" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS8I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS8" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS8I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS8" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductS8I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductS8" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductS8" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityS8I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityS8" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravityS8" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedS8" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedS8" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTS8" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTS8" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMS8" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMS8" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitS8" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitS8" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedS8" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedS8" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedS8_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateS8" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateS8" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNS8" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableS8" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertS8" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertS8" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksS8" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksS8" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </td>
                        </tr>
                        <tr style="border-color: Black">
                            <td>
                                <span id="spn9" runat="server">
                                    <table id="tbl9" runat="server" width="100%" border="1">
                                        <tr>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnP9" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionP9" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDP9" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDP9" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoP9I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoP9" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP9I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP9" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP9I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP9" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductP9I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductP9" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductP9" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityP9I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityP9" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--    <telerik:RadLabel ID="lblSpcGravityP9" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedP9" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedP9" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTP9" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTP9" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMP9" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMP9" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitP9" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitP9" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedP9" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedP9" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedP9_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateP9" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateP9" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNP9" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableP9" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertP9" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertP9" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksP9" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksP9" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnPI9" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionPI9" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDPI9" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDPI9" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoPI9I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoPI9" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI9I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI9" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI9I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI9" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductPI9I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductPI9" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductPI9" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityPI9I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityPI9" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravityPI9" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedPI9" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedPI9" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTPI9" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTPI9" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMPI9" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMPI9" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitPI9" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitPI9" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedPI9" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedPI9" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedPI9_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDatePI9" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDatePI9" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNPI9" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpablePI9" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertPI9" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertPI9" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksPI9" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksPI9" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnC9" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionC9" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDC9" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDC9" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoC9I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoC9" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC9I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC9" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC9I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC9" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductC9I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductC9" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductC9" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityC9I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityC9" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityC9" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedC9" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedC9" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTC9" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTC9" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMC9" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMC9" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitC9" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitC9" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedC9" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedC9" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedC9_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateC9" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateC9" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNC9" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableC9" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertC9" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertC9" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksC9" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksC9" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnSI9" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionSI9" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDSI9" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDSI9" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoSI9I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoSI9" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI9I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI9" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI9I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI9" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductSI9I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductSI9" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductSI9" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravitySI9I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravitySI9" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--   <telerik:RadLabel ID="lblSpcGravitySI9" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedSI9" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedSI9" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTSI9" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTSI9" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMSI9" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMSI9" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitSI9" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitSI9" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedSI9" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedSI9" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedSI9_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateSI9" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateSI9" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNSI9" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableSI9" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertSI9" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertSI9" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksSI9" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksSI9" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnS9" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionS9" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDS9" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDS9" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoS9I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoS9" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS9I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS9" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS9I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS9" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductS9I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductS9" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductS9" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityS9I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityS9" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--   <telerik:RadLabel ID="lblSpcGravityS9" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedS9" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedS9" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTS9" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTS9" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMS9" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMS9" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitS9" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitS9" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedS9" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedS9" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedS9_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateS9" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateS9" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNS9" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableS9" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertS9" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertS9" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksS9" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksS9" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </td>
                        </tr>
                        <tr style="border-color: Black">
                            <td>
                                <span id="spn10" runat="server">
                                    <table id="tbl10" runat="server" width="100%" border="1">
                                        <tr>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnP10" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionP10" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDP10" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDP10" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoP10I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoP10" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP10I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP10" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP10I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP10" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductP10I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductP10" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--  <telerik:RadLabel ID="lblProductP10" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityP10I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityP10" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--   <telerik:RadLabel ID="lblSpcGravityP10" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedP10" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedP10" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTP10" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTP10" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMP10" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMP10" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitP10" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitP10" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedP10" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedP10" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedP10_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateP10" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateP10" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNP10" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableP10" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertP10" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertP10" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksP10" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksP10" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnPI10" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionPI10" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDPI10" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDPI10" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoPI10I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoPI10" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI10I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI10" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI10I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI10" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductPI10I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductPI10" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductPI10" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityPI10I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityPI10" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityPI10" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedPI10" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedPI10" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTPI10" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTPI10" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMPI10" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMPI10" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitPI10" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitPI10" runat="server" CssClass="readonlytextbox"
                                                                    DecimalPlace="2" Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedPI10" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedPI10" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedPI10_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDatePI10" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDatePI10" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNPI10" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpablePI10" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertPI10" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertPI10" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksPI10" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksPI10" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnC10" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionC10" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDC10" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDC10" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoC10I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoC10" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC10I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC10" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC10I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC10" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductC10I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductC10" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--  <telerik:RadLabel ID="lblProductC10" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityC10I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityC10" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravityC10" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedC10" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedC10" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTC10" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTC10" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMC10" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMC10" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitC10" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitC10" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedC10" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedC10" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedC10_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateC10" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateC10" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNC10" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableC10" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertC10" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertC10" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksC10" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksC10" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnSI10" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionSI10" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDSI10" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDSI10" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoSI10I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoSI10" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI10I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI10" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI10I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI10" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductSI10I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductSI10" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductSI10" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravitySI10I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravitySI10" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravitySI10" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedSI10" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedSI10" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTSI10" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTSI10" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMSI10" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMSI10" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitSI10" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitSI10" runat="server" CssClass="readonlytextbox"
                                                                    DecimalPlace="2" Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedSI10" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedSI10" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedSI10_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateSI10" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateSI10" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNSI10" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableSI10" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertSI10" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertSI10" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksSI10" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksSI10" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnS10" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionS10" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDS10" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDS10" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoS10I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoS10" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS10I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS10" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS10I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS10" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductS10I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductS10" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductS10" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityS10I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityS10" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--   <telerik:RadLabel ID="lblSpcGravityS10" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedS10" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedS10" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTS10" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTS10" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMS10" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMS10" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitS10" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitS10" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedS10" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedS10" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedS10_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateS10" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateS10" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNS10" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableS10" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertS10" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertS10" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksS10" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksS10" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </td>
                        </tr>
                        <tr style="border-color: Black">
                            <td>
                                <span id="spn11" runat="server">
                                    <table id="tbl11" runat="server" width="100%" border="1">
                                        <tr>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnP11" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionP11" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDP11" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDP11" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoP11I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoP11" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP11I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP11" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP11I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP11" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductP11I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductP11" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductP11" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityP11I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityP11" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravityP11" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedP11" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedP11" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTP11" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTP11" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMP11" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMP11" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitP11" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitP11" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedP11" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedP11" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedP11_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateP11" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateP11" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNP11" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableP11" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertP11" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertP11" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksP11" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksP11" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnPI11" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionPI11" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDPI11" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDPI11" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoPI11I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoPI11" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI11I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI11" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI11I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI11" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductPI11I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductPI11" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductPI11" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityPI11I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityPI11" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravityPI11" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedPI11" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedPI11" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTPI11" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTPI11" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMPI11" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMPI11" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitPI11" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitPI11" runat="server" CssClass="readonlytextbox"
                                                                    DecimalPlace="2" Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedPI11" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedPI11" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedPI11_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDatePI11" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDatePI11" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNPI11" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpablePI11" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertPI11" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertPI11" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksPI11" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksPI11" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnC11" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionC11" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDC11" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDC11" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoC11I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoC11" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC11I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC11" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC11I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC11" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductC11I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductC11" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductC11" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityC11I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityC11" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravityC11" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedC11" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedC11" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTC11" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTC11" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMC11" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMC11" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitC11" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitC11" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedC11" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedC11" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedC11_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateC11" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateC11" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNC11" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableC11" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertC11" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertC11" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksC11" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksC11" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnSI11" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionSI11" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDSI11" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDSI11" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoSI11I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoSI11" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI11I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI11" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI11I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI11" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductSI11I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductSI11" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--  <telerik:RadLabel ID="lblProductSI11" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravitySI11I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravitySI11" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravitySI11" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedSI11" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedSI11" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTSI11" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTSI11" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMSI11" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMSI11" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitSI11" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitSI11" runat="server" CssClass="readonlytextbox"
                                                                    DecimalPlace="2" Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedSI11" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedSI11" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedSI11_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateSI11" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateSI11" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNSI11" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableSI11" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertSI11" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertSI11" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksSI11" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksSI11" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnS11" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionS11" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDS11" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDS11" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoS11I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoS11" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS11I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS11" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS11I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS11" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductS11I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductS11" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductS11" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityS11I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityS11" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--    <telerik:RadLabel ID="lblSpcGravityS11" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedS11" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedS11" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTS11" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTS11" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMS11" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMS11" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitS11" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitS11" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedS11" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedS11" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedS11_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateS11" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateS11" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNS11" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableS11" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertS11" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertS11" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksS11" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksS11" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </td>
                        </tr>
                        <tr style="border-color: Black">
                            <td>
                                <span id="spn12" runat="server">
                                    <table id="tbl12" runat="server" width="100%" border="1">
                                        <tr>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnP12" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionP12" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDP12" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDP12" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoP12I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoP12" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP12I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP12" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP12I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP12" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductP12I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductP12" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductP12" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityP12I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityP12" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--    <telerik:RadLabel ID="lblSpcGravityP12" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedP12" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedP12" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTP12" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTP12" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMP12" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMP12" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitP12" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitP12" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedP12" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedP12" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedP12_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateP12" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateP12" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNP12" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableP12" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertP12" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertP12" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksP12" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksP12" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnPI12" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionPI12" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDPI12" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDPI12" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoPI12I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoPI12" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI12I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI12" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI12I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI12" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductPI12I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductPI12" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductPI12" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityPI12I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityPI12" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityPI12" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedPI12" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedPI12" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTPI12" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTPI12" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMPI12" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMPI12" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitPI12" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitPI12" runat="server" CssClass="readonlytextbox"
                                                                    DecimalPlace="2" Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedPI12" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedPI12" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedPI12_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDatePI12" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDatePI12" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNPI12" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpablePI12" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertPI12" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertPI12" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksPI12" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksPI12" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnC12" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionC12" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDC12" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDC12" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoC12I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoC12" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC12I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC12" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC12I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC12" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductC12I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductC12" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductC12" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityC12I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityC12" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityC12" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedC12" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedC12" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTC12" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTC12" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMC12" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMC12" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitC12" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitC12" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedC12" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedC12" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedC12_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateC12" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateC12" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNC12" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableC12" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertC12" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertC12" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksC12" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksC12" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnSI12" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionSI12" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDSI12" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDSI12" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoSI12I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoSI12" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI12I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI12" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI12I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI12" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductSI12I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductSI12" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--   <telerik:RadLabel ID="lblProductSI12" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravitySI12I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravitySI12" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravitySI12" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedSI12" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedSI12" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTSI12" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTSI12" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMSI12" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMSI12" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitSI12" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitSI12" runat="server" CssClass="readonlytextbox"
                                                                    DecimalPlace="2" Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedSI12" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedSI12" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedSI12_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateSI12" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateSI12" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNSI12" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableSI12" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertSI12" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertSI12" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksSI12" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksSI12" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnS12" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionS12" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDS12" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDS12" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoS12I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoS12" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS12I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS12" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS12I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS12" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductS12I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductS12" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductS12" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityS12I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityS12" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--   <telerik:RadLabel ID="lblSpcGravityS12" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedS12" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedS12" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTS12" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTS12" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMS12" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMS12" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitS12" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitS12" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedS12" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedS12" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedS12_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateS12" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateS12" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNS12" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableS12" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertS12" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertS12" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksS12" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksS12" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </td>
                        </tr>
                        <tr style="border-color: Black">
                            <td>
                                <span id="spn13" runat="server">
                                    <table id="tbl13" runat="server" width="100%" border="1">
                                        <tr>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnP13" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionP13" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDP13" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDP13" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoP13I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoP13" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP13I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP13" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP13I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP13" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductP13I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductP13" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductP13" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityP13I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityP13" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityP13" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedP13" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedP13" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTP13" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTP13" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMP13" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMP13" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitP13" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitP13" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedP13" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedP13" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedP13_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateP13" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateP13" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNP13" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableP13" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertP13" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertP13" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksP13" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksP13" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnPI13" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionPI13" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDPI13" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDPI13" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoPI13I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoPI13" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI13I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI13" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI13I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI13" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductPI13I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductPI13" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductPI13" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityPI13I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityPI13" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravityPI13" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedPI13" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedPI13" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTPI13" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTPI13" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMPI13" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMPI13" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitPI13" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitPI13" runat="server" CssClass="readonlytextbox"
                                                                    DecimalPlace="2" Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedPI13" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedPI13" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedPI13_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDatePI13" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDatePI13" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNPI13" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpablePI13" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertPI13" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertPI13" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksPI13" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksPI13" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnC13" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionC13" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDC13" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDC13" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoC13I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoC13" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC13I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC13" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC13I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC13" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductC13I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductC13" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductC13" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityC13I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityC13" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityC13" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedC13" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedC13" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTC13" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTC13" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMC13" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMC13" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitC13" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitC13" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedC13" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedC13" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedC13_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateC13" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateC13" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNC13" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableC13" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertC13" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertC13" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksC13" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksC13" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnSI13" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionSI13" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDSI13" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDSI13" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoSI13I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoSI13" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI13I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI13" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI13I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI13" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductSI13I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductSI13" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductSI13" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravitySI13I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravitySI13" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--    <telerik:RadLabel ID="lblSpcGravitySI13" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedSI13" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedSI13" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTSI13" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTSI13" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMSI13" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMSI13" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitSI13" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitSI13" runat="server" CssClass="readonlytextbox"
                                                                    DecimalPlace="2" Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedSI13" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedSI13" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedSI13_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateSI13" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateSI13" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNSI13" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableSI13" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertSI13" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertSI13" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksSI13" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksSI13" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnS13" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionS13" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDS13" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDS13" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoS13I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoS13" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS13I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS13" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS13I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS13" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductS13I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductS13" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductS13" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityS13I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityS13" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--     <telerik:RadLabel ID="lblSpcGravityS13" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedS13" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedS13" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTS13" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTS13" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMS13" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMS13" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitS13" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitS13" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedS13" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedS13" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedS13_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateS13" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateS13" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNS13" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableS13" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertS13" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertS13" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksS13" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksS13" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </td>
                        </tr>
                        <tr style="border-color: Black">
                            <td>
                                <span id="spn14" runat="server">
                                    <table id="tbl14" runat="server" width="100%" border="1">
                                        <tr>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnP14" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionP14" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDP14" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDP14" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoP14I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoP14" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP14I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP14" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP14I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP14" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductP14I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductP14" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductP14" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityP14I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityP14" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--  <telerik:RadLabel ID="lblSpcGravityP14" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedP14" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedP14" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTP14" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTP14" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMP14" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMP14" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitP14" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitP14" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedP14" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedP14" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedP14_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateP14" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateP14" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNP14" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableP14" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertP14" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertP14" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksP14" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksP14" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnPI14" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionPI14" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDPI14" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDPI14" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoPI14I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoPI14" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI14I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI14" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI14I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI14" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductPI14I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductPI14" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductPI14" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityPI14I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityPI14" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityPI14" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedPI14" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedPI14" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTPI14" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTPI14" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMPI14" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMPI14" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitPI14" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitPI14" runat="server" CssClass="readonlytextbox"
                                                                    DecimalPlace="2" Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedPI14" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedPI14" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedPI14_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDatePI14" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDatePI14" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNPI14" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpablePI14" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertPI14" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertPI14" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksPI14" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksPI14" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnC14" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionC14" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDC14" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDC14" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoC14I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoC14" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC14I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC14" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC14I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC14" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductC14I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductC14" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductC14" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityC14I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityC14" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--   <telerik:RadLabel ID="lblSpcGravityC14" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedC14" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedC14" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTC14" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTC14" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMC14" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMC14" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitC14" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitC14" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedC14" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedC14" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedC14_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateC14" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateC14" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNC14" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableC14" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertC14" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertC14" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksC14" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksC14" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnSI14" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionSI14" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDSI14" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDSI14" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoSI14I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoSI14" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI14I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI14" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI14I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI14" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductSI14I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductSI14" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--   <telerik:RadLabel ID="lblProductSI14" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravitySI14I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravitySI14" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--<telerik:RadLabel ID="lblSpcGravitySI14" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedSI14" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedSI14" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTSI14" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTSI14" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMSI14" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMSI14" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitSI14" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitSI14" runat="server" CssClass="readonlytextbox"
                                                                    DecimalPlace="2" Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedSI14" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedSI14" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedSI14_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateSI14" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateSI14" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNSI14" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableSI14" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertSI14" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertSI14" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksSI14" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksSI14" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnS14" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionS14" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDS14" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDS14" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoS14I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoS14" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS14I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS14" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS14I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS14" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductS14I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductS14" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductS14" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityS14I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityS14" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%-- <telerik:RadLabel ID="lblSpcGravityS14" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedS14" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedS14" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTS14" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTS14" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMS14" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMS14" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitS14" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitS14" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedS14" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedS14" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedS14_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateS14" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateS14" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNS14" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableS14" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertS14" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertS14" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksS14" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksS14" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </td>
                        </tr>
                        <tr style="border-color: Black">
                            <td>
                                <span id="spn15" runat="server">
                                    <table id="tbl15" runat="server" width="100%" border="1">
                                        <tr>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnP15" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionP15" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDP15" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDP15" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoP15I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoP15" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP15I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolP15" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP15I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolP15" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductP15I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductP15" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductP15" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityP15I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityP15" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--    <telerik:RadLabel ID="lblSpcGravityP15" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedP15" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedP15" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTP15" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTP15" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMP15" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMP15" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitP15" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitP15" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedP15" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedP15" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedP15_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateP15" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateP15" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNP15" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableP15" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertP15" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertP15" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksP15" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksP15" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnPI15" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionPI15" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDPI15" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDPI15" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoPI15I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoPI15" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI15I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolPI15" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI15I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolPI15" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductPI15I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductPI15" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductPI15" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityPI15I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityPI15" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--   <telerik:RadLabel ID="lblSpcGravityPI15" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedPI15" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedPI15" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTPI15" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTPI15" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMPI15" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMPI15" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitPI15" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitPI15" runat="server" CssClass="readonlytextbox"
                                                                    DecimalPlace="2" Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedPI15" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedPI15" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedPI15_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDatePI15" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDatePI15" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNPI15" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpablePI15" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertPI15" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertPI15" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksPI15" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksPI15" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnC15" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionC15" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDC15" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDC15" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoC15I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoC15" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC15I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolC15" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC15I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolC15" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductC15I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductC15" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--  <telerik:RadLabel ID="lblProductC15" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityC15I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityC15" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--   <telerik:RadLabel ID="lblSpcGravityC15" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedC15" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedC15" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTC15" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTC15" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMC15" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMC15" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitC15" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitC15" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedC15" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedC15" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedC15_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateC15" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateC15" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNC15" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableC15" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertC15" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertC15" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksC15" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksC15" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px">
                                                <span id="spnSI15" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionSI15" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDSI15" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDSI15" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoSI15I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoSI15" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI15I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolSI15" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI15I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolSI15" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductSI15I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductSI15" runat="server" CssClass="gridinput_mandatory" />
                                                                <%--<telerik:RadLabel ID="lblProductSI15" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravitySI15I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravitySI15" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--    <telerik:RadLabel ID="lblSpcGravitySI15" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedSI15" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedSI15" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTSI15" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTSI15" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMSI15" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMSI15" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitSI15" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitSI15" runat="server" CssClass="readonlytextbox"
                                                                    DecimalPlace="2" Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedSI15" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedSI15" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedSI15_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateSI15" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateSI15" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNSI15" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableSI15" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertSI15" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertSI15" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksSI15" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksSI15" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                            <td style="width: 20%; height: 200px; background-color: #D9EDFA;">
                                                <span id="spnS15" runat="server">
                                                    <table width="100%">
                                                        <tr>
                                                            <td>
                                                                <asp:HiddenField ID="hdnConsumptionS15" runat="server" />
                                                                <asp:HiddenField ID="hdnMidnightReportIDS15" runat="server" />
                                                                <asp:HiddenField ID="hdnConfiguratoinIDS15" runat="server" />
                                                                <telerik:RadLabel ID="lblTankNoS15I" runat="server" Text="Tank No"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankNoS15" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS15I" runat="server" Text="100% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl100VolS15" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS15I" runat="server" Text="85% Vol Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadLabel ID="lbl85VolS15" runat="server" Text=""></telerik:RadLabel>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblProductS15I" runat="server" Text="Product"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Product ID="lblProductS15" runat="server" CssClass="gridinput_mandatory" />
                                                                <%-- <telerik:RadLabel ID="lblProductS15" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblSpcGravityS15I" runat="server" Text="Specific Gravity"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="lblSpcGravityS15" runat="server" CssClass="input_mandatory" DecimalPlace="2" />
                                                                <%--     <telerik:RadLabel ID="lblSpcGravityS15" runat="server" Text=""></telerik:RadLabel>--%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateLoadedS15" runat="server" Text="Date Loaded"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateLoadedS15" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBMTS15" runat="server" Text="ROB MT"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBMTS15" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCUMS15" runat="server" Text="ROB Cu.M"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCUMS15" runat="server" CssClass="input" DecimalPlace="2" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblROBCharterUnitS15" runat="server"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Number ID="ucROBCharterUnitS15" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                                                    Visible="false" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblTankCleanedS15" runat="server" Text="Tank Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkTankCleanedS15" runat="server" AutoPostBack="true" OnCheckedChanged="chkTankCleanedS15_OnCheckedChanged" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblDateS15" runat="server" Text="Date Last Cleaned"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <eluc:Date ID="ucDateS15" runat="server" CssClass="input" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblUnpumpableYNS15" runat="server" Text="Unpumpable" Visible="false"></telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkUnpumpableS15" runat="server" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertS15" runat="server" Text="Postpone Alert by 7 days"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadCheckBox ID="chkpostponealertS15" runat="server" Visible="false" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <telerik:RadLabel ID="lblpostponealertremarksS15" runat="server" Text="Postpone explanatory remarks"
                                                                    Visible="false">
                                                                </telerik:RadLabel>
                                                            </td>
                                                            <td>
                                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtpostponeexpremarksS15" runat="server" CssClass="input" TextMode="MultiLine" Visible="false"></telerik:RadTextBox>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </span>
                                            </td>
                                        </tr>
                                    </table>
                                </span>
                            </td>
                        </tr>
                    </table>
                </div>
                <%-- <div style="top: 40px; position: relative;" id="div1" runat="server" visible="false">
                    <table cellpadding="2" cellspacing="2" width='40%' border="1">
                        <tr>
                            <td colspan="7" align="center">
                                Methanol Tanks
                            </td>
                        </tr>
                        <tr>
                            <td>
                                TankNo
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPortTankNo" runat="server"></telerik:RadLabel>
                                 <telerik:RadLabel ID="lblTankPortConfigid" runat="server" Visible="false"></telerik:RadLabel>
                                  <telerik:RadLabel ID="lblPortCounsumptionid" runat="server" Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                Port
                            </td>
                            <td>
                            </td>
                            <td>
                                TankNo
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStbdTankNo" runat="server"></telerik:RadLabel>
                                 <telerik:RadLabel ID="lblTankStbdConfigid" runat="server" Visible="false"></telerik:RadLabel>
                                  <telerik:RadLabel ID="lblStbdCounsumptionid" runat="server" Visible="false"></telerik:RadLabel>
                            </td>
                            <td>
                                Stbd
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                100% Vol Cu.M
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCap100Port" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                            </td>
                            <td colspan="2">
                                100% Vol Cu.M
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCap100Stbd" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                85% Vol Cu.M
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCap85Port" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                            </td>
                            <td colspan="2">
                                85% Vol Cu.M
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCap85Stbd" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                Product Name
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblProductPort" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                            </td>
                            <td colspan="2">
                                Product Name
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblProductStbd" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                Date Loaded
                            </td>
                            <td>
                                <eluc:Date ID="txtDateLoadedPort" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                            <td>
                            </td>
                            <td colspan="2">
                                Date Loaded
                            </td>
                            <td>
                                <eluc:Date ID="txtDateLoadedstbd" runat="server" CssClass="input" DatePicker="true" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                ROB Cu.M
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtrobCumPort" runat="server" CssClass="input"></telerik:RadTextBox>
                            </td>
                            <td>
                            </td>
                            <td colspan="2">
                                ROB Cu.M
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtrobCumstbd" runat="server" CssClass="input"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </div>--%>
                <%--</div>--%>
            </div>
        </div>
    </form>
</body>
</html>
