<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRMonthlyReport.aspx.cs"
    Inherits="CrewOffshoreDMRMonthlyReport" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DMR Monthly Report</title>
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
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />


            <eluc:TabStrip ID="MenuReportTap" TabStrip="true" runat="server" OnTabStripCommand="ReportTapp_TabStripCommand"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand"></eluc:TabStrip>

            <asp:HiddenField ID="hdnScroll" runat="server" />
            <div>
                <%--
                <div id="divScroll" style="position: relative; z-index: 0; width: 100%; height: 495px;
                    overflow: auto;" onscroll="javascript:setScroll('divScroll', 'hdnScroll');">--%>
                <div>
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Vessel ID="ucVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true"
                                    AppendDataBoundItems="true" Width="150px" AutoPostBack="true" OnTextChangedEvent="ucVessel_OnTextChangedEvent" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblmMonthandYear" runat="server" Text="Select Month and Year"></asp:Literal>
                            </td>
                            <td colspan="3">
                                <telerik:RadDropDownList ID="ddlMonth" runat="server"  AutoPostBack="true"
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
                                    <telerik:RadDropDownList ID="ddlYear" runat="server"  AutoPostBack="true"
                                        OnTextChanged="ddlMonth_TextChangedEvent">
                                    </telerik:RadDropDownList>
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <table width="100%">
                        <tr valign="top">
                            <td>
                                <%--<telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
                                    <AjaxSettings>
                                        <telerik:AjaxSetting AjaxControlID="RadAjaxPanel1">
                                            <UpdatedControls>
                                                <telerik:AjaxUpdatedControl ControlID="RadGrid1" />
                                            </UpdatedControls>
                                        </telerik:AjaxSetting>
                                        <telerik:AjaxSetting AjaxControlID="RadGrid1">
                                            <UpdatedControls>

                                                <telerik:AjaxUpdatedControl ControlID="btnExport" />
                                                <telerik:AjaxUpdatedControl ControlID="btnFuelCons" />

                                                <telerik:AjaxUpdatedControl ControlID="btnPORT" />
                                                <telerik:AjaxUpdatedControl ControlID="btnSSES" />
                                                <telerik:AjaxUpdatedControl ControlID="btnMAN" />
                                                <telerik:AjaxUpdatedControl ControlID="btnSS" />
                                                <telerik:AjaxUpdatedControl ControlID="btnSTBY" />
                                                <telerik:AjaxUpdatedControl ControlID="btnCARDP" />
                                                <telerik:AjaxUpdatedControl ControlID="btnCAR" />
                                                <telerik:AjaxUpdatedControl ControlID="btnAHD" />
                                                <telerik:AjaxUpdatedControl ControlID="btnROV" />
                                                <telerik:AjaxUpdatedControl ControlID="btnBRD" />
                                                <telerik:AjaxUpdatedControl ControlID="btnDIVE" />
                                                <telerik:AjaxUpdatedControl ControlID="btnTOW" />
                                                <telerik:AjaxUpdatedControl ControlID="btnOTOW" />
                                            </UpdatedControls>
                                        </telerik:AjaxSetting>
                                    </AjaxSettings>
                                </telerik:RadAjaxManager>--%>
                                <%--  <asp:GridView ID="gvOperationalSummary" runat="server" AutoGenerateColumns="False"
                                    Font-Size="11px" Width="95%" CellPadding="2" AllowSorting="true" ShowHeader="true"
                                    ShowFooter="true" EnableViewState="false" OnRowDataBound="gvOperationalSummary_ItemDataBound"
                                    OnRowCommand="gvOperationalSummary_RowCommand" OnRowEditing="gvOperationalSummary_RowEditing">
                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>--%>
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvOperationalSummary" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                    CellSpacing="0" GridLines="None"
                                   
                                    OnNeedDataSource="gvOperationalSummary_NeedDataSource"
                                    OnItemCommand="gvOperationalSummary_ItemCommand" EnableViewState="false">
                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" DataKeyNames="FLDOPERATIONALTASKID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
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
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblActivityHeader" runat="server" Text="Status"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblOperationalTaskid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDOPERATIONALTASKID"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblOperationalTaskName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDTASKNAME"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblShortName" Visible="false" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSHORTNAME"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <telerik:RadLabel ID="lblTotal" runat="server" Text="Total:"></telerik:RadLabel>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="20%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblTimeDurationHeader" runat="server" Text="Time (Hrs.Mins)"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblTimeDuration" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDTIMEDURATION"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <telerik:RadLabel ID="lblTotalTimeDuration" runat="server"></telerik:RadLabel>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="20%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblFuelConsumptionHeader" runat="server" Text="Total Cons (ltrs)"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblFuelConsumption" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDFUELOILCONSUMPTION"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <telerik:RadLabel ID="lblTotalFuelConsumption" runat="server"></telerik:RadLabel>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="20%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblConsumptionRateHeader" runat="server" Text="Cons Rate (ltrs/hr)"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblConsumptionRate" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCONSRATE"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <telerik:RadLabel ID="lblTotalConsumptionRate" runat="server"></telerik:RadLabel>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Center" />
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit"
                                                        CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                                        ToolTip="Trend Graph">
                                                    <span class="icon"><i class="fas fa-chart-bar"></i></span>
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
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td width="90%">
                                <asp:Chart ID="ChartMonthlyActivityPercentage" runat="server" Height="300px" Width="450px">
                                </asp:Chart>
                            </td>
                            <td width="10%">
                                <asp:Button ID="btnExport" runat="server" Text="Export To PDF" OnClick="btnExport_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td width="90%">
                                <asp:Chart ID="ChartFuelConsRate" runat="server" Height="300px" Width="1000px" Visible="false">
                                </asp:Chart>
                            </td>
                            <td width="10%">
                                <asp:Button ID="btnFuelCons" runat="server" Text="Export To PDF" OnClick="btnFuelCons_Click"
                                    Visible="false" />
                            </td>
                        </tr>
                    </table>
                    <table id="tblChart" width="60%" runat="server">
                        <tr>
                            <td width="90%">
                                <asp:Chart ID="ChartPORT" runat="server" Height="300px" Width="1000px">
                                </asp:Chart>
                            </td>
                            <td width="10%">
                                <asp:Button ID="btnPORT" runat="server" Text="Export To PDF" OnClick="btnPORT_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Chart ID="ChartSSES" runat="server" Height="300px" Width="1000px">
                                </asp:Chart>
                            </td>
                            <td width="10%">
                                <asp:Button ID="btnSSES" runat="server" Text="Export To PDF" OnClick="btnSSES_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Chart ID="ChartMAN" runat="server" Height="300px" Width="1000px">
                                </asp:Chart>
                            </td>
                            <td width="10%">
                                <asp:Button ID="btnMAN" runat="server" Text="Export To PDF" OnClick="btnMAN_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Chart ID="ChartSS" runat="server" Height="300px" Width="1000px">
                                </asp:Chart>
                            </td>
                            <td width="10%">
                                <asp:Button ID="btnSS" runat="server" Text="Export To PDF" OnClick="btnSS_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Chart ID="ChartSTBY" runat="server" Height="300px" Width="1000px">
                                </asp:Chart>
                            </td>
                            <td width="10%">
                                <asp:Button ID="btnSTBY" runat="server" Text="Export To PDF" OnClick="btnSTBY_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Chart ID="ChartCARDP" runat="server" Height="300px" Width="1000px">
                                </asp:Chart>
                            </td>
                            <td width="10%">
                                <asp:Button ID="btnCARDP" runat="server" Text="Export To PDF" OnClick="btnCARDP_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Chart ID="ChartCAR" runat="server" Height="300px" Width="1000px">
                                </asp:Chart>
                            </td>
                            <td width="10%">
                                <asp:Button ID="btnCAR" runat="server" Text="Export To PDF" OnClick="btnCAR_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Chart ID="ChartAHD" runat="server" Height="300px" Width="1000px">
                                </asp:Chart>
                            </td>
                            <td width="10%">
                                <asp:Button ID="btnAHD" runat="server" Text="Export To PDF" OnClick="btnAHD_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Chart ID="ChartROV" runat="server" Height="300px" Width="1200px">
                                </asp:Chart>
                            </td>
                            <td width="10%">
                                <asp:Button ID="btnROV" runat="server" Text="Export To PDF" OnClick="btnROV_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Chart ID="ChartBRD" runat="server" Height="300px" Width="1000px">
                                </asp:Chart>
                            </td>
                            <td width="10%">
                                <asp:Button ID="btnBRD" runat="server" Text="Export To PDF" OnClick="btnBRD_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Chart ID="ChartDIVE" runat="server" Height="300px" Width="1000px">
                                </asp:Chart>
                            </td>
                            <td width="10%">
                                <asp:Button ID="btnDIVE" runat="server" Text="Export To PDF" OnClick="btnDIVE_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Chart ID="ChartTOW" runat="server" Height="300px" Width="1000px">
                                </asp:Chart>
                            </td>
                            <td width="10%">
                                <asp:Button ID="btnTOW" runat="server" Text="Export To PDF" OnClick="btnTOW_Click" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Chart ID="ChartOTOW" runat="server" Height="300px" Width="1000px">
                                </asp:Chart>
                            </td>
                            <td width="10%">
                                <asp:Button ID="btnOTOW" runat="server" Text="Export To PDF" OnClick="btnOTOW_Click" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>



    </form>
</body>
</html>
