<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionMonthlyReport.aspx.cs"
    Inherits="VesselPositionMonthlyReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Cargo" Src="~/UserControls/UserControlCargo.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Direction" Src="~/UserControls/UserControlDircection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Voyage" Src="~/UserControls/UserControlVoyage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Monthly Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmMonthlyReport" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlMonthlyReportData" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuMonthlyReportTap" TabStrip="true" runat="server" OnTabStripCommand="MonthlyReportTapp_TabStripCommand"></eluc:TabStrip>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand"></eluc:TabStrip>
            <asp:Button runat="server" ID="cmdHiddenSubmits" OnClick="cmdHiddenSubmit_Click" />
        <telerik:RadAjaxPanel runat="server" ID="pnlMonthlyReportData">
            

            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtCurrentDate" runat="server" CssClass="input" Enabled="false" Visible="false" />
                    </td>
                </tr>
            </table>
            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmMonthandYear" runat="server" Text="Select Month and Year"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadComboBox ID="ddlMonth" runat="server" CssClass="input" AutoPostBack="true"
                            OnTextChanged="ddlMonth_TextChangedEvent">
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
                        &nbsp;&nbsp;
                            <telerik:RadComboBox ID="ddlYear" runat="server" CssClass="input" AutoPostBack="true"
                                OnTextChanged="ddlMonth_TextChangedEvent">
                            </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="1">
                        <telerik:RadLabel ID="lblmFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtFromDate" runat="server" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="1">
                        <telerik:RadLabel ID="lblmToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtToDate" runat="server" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lblmBallast" runat="server" Text="Ballast"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblmLoaded" runat="server" Text="Loaded"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblmTotal" runat="server" Text="Total"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmSteamingTime" runat="server" Text="Steaming Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballaststeamingtime" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedsteamingtime" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalsteamingtime" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmFullSpeed" runat="server" Text="Full Speed"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastfullspeed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedfullspeed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalfullspeed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmReducedSpeed" runat="server" Text="Reduced Speed"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastreducedspeed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedreducedspeed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalreducedspeed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmManoeveringTime" runat="server" Text="Manoevering Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastmanoeveringtime" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedmanoeveringtime" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalmanoeveringtime" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmMEStoppage" runat="server" Text="M/E Stoppage"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastmestoppage" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedmestoppage" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalmestoppage" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmDeviationorDelay" runat="server" Text="Deviation or Delay (if any)"></telerik:RadLabel>
                    </td>
                    <td></td>
                    <td></td>
                    <td>
                        <eluc:Number ID="txttotaldeviationordelay" runat="server" CssClass="input" MaxLength="5"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmDistanceSteamed" runat="server" Text="Distance Steamed"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastdistancesteamed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadeddistancesteamed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotaldistancesteamed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmEngineDistance" runat="server" Text="Engine Distance"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastenginedistance" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedenginedistance" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalenginedistance" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmAverageSpeed" runat="server" Text="Average Speed"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastavgspeed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedavgspeed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalavgspeed" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmAverageSlip" runat="server" Text="Average Slip"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastavgslip" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedavgslip" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalavgslip" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmAverageRPM" runat="server" Text="Average RPM"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastavgrpm" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedavgrpm" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalavgrpm" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmAverageBHP" runat="server" Text="Average BHP"></telerik:RadLabel>
                    </td>
                    <td></td>
                    <td></td>
                    <td>
                        <eluc:Number ID="txttotalavgbhp" runat="server" CssClass="input" MaxLength="9"
                            Width="80px" />
                         
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblavgkw" runat="server" Text="Average KW"></telerik:RadLabel>
                    </td>
                    <td></td>
                    <td></td>
                    <td>
                        <eluc:Number ID="txttotalavgkw" runat="server" Enabled="false" CssClass="readonlytextbox" MaxLength="9"
                            Width="80px" />
                    </td>
                </tr>                
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmAverageFOCons" runat="server" Text="Average FO Cons/Day"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastavgfoconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedavgfoconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalavgfoconsumptionperday" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmAvgEEOI" runat="server" Text="Avg EEOI"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtballastAvgEEOI" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="9" DecimalPlace="6"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txtloadedAvgEEOI" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="9" DecimalPlace="6"
                            Width="80px" />
                    </td>
                    <td>
                        <eluc:Number ID="txttotalAvgEEOI" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="9" DecimalPlace="6"
                            Width="80px" />
                    </td>
                </tr>

                <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmOverallESI" runat="server" Text="Overall ESI"></telerik:RadLabel>
                    </td>
                    <td></td>
                    <td></td>
                    <td>
                        <eluc:Number ID="txtOverallESI" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="9" DecimalPlace="3"
                            Width="80px" />
                    </td>
                </tr>

                <tr>
                    <td>Co2 Emission (MT)</td>
                    <td></td>
                    <td></td>
                    <td>
                        <eluc:Number ID="txtCO2Emission" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="9" DecimalPlace="3"
                            Width="80px" />
                    </td>
                </tr>

                <tr>
                    <td>Co2 Index (kg/nm)</td>
                    <td></td>
                    <td></td>
                    <td>
                        <eluc:Number ID="txtCO2Index" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="9" DecimalPlace="3"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>EEOI Co2 (g/nm-t)
                    </td>
                    <td></td>
                    <td></td>
                    <td>
                        <eluc:Number ID="txtEEOICO2" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="9" DecimalPlace="3"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>SOx Emission (MT)</td>
                    <td></td>
                    <td></td>
                    <td>
                        <eluc:Number ID="txtSOxEmission" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="9" DecimalPlace="3"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>SOx Index (kg/nm)</td>
                    <td></td>
                    <td></td>
                    <td>
                        <eluc:Number ID="txtSOxIndex" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="9" DecimalPlace="3"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>EEOI&nbsp; SOx (g/nm-t)</td>
                    <td></td>
                    <td></td>
                    <td>
                        <eluc:Number ID="txtEEOISOx" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="9" DecimalPlace="3"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>NOx Emission (MT)</td>
                    <td></td>
                    <td></td>
                    <td>
                        <eluc:Number ID="txtNOxEmission" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="9" DecimalPlace="3"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>NOx Index (kg/nm)</td>
                    <td></td>
                    <td></td>
                    <td>
                        <eluc:Number ID="txtNOxIndex" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="9" DecimalPlace="3"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>EEOI NOx (g/nm-t)</td>
                    <td></td>
                    <td></td>
                    <td>
                        <eluc:Number ID="txtEEOINOx" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="9" DecimalPlace="3"
                            Width="80px" />
                    </td>
                </tr>


                <tr>
                    <td colspan="4">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmRadioTraffic" runat="server" Text="Radio Traffic Charterer's A/C (USD)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Number ID="txtradiotrafficcharterersac" runat="server" CssClass="input" MaxLength="5"
                            Width="80px" />
                    </td>
                </tr>
            </table>
            <br />
            <telerik:RadGrid ID="gvFuelOil" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnNeedDataSource="gvFuelOil_NeedDataSource"  EnableHeaderContextMenu="true" GroupingEnabled="false"
                ShowHeader="true" EnableViewState="false"  AllowSorting="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDVESSELMONTHLYREPORTID">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />


                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Fuel Oil" HeaderStyle-Width="15%" HeaderStyle-Wrap="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbloilconsumptionid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblloilTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></telerik:RadLabel>
                            <%# DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="ROB Prv Mth" HeaderStyle-Width="9%" HeaderStyle-Wrap="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPreviousROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENINGROB") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Bunkered" HeaderStyle-Wrap="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblfBunkered" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILBUNKERD") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="M/E" HeaderStyle-Width="6%" HeaderStyle-Wrap="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblfueloilconsumptionme" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lbloilTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></telerik:RadLabel>
                            <eluc:Number ID="txtME" runat="server" CssClass="input" MaxLength="5" Visible="false"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONME") %>' />
                            <telerik:RadLabel ID="lblME" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONME") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="A/E" HeaderStyle-Width="6%" HeaderStyle-Wrap="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblfueloilconsumptionae" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONAE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtAE" runat="server" CssClass="input" MaxLength="5" Visible="false"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONAE") %>' />
                            <telerik:RadLabel ID="lblfueloilconsumptionaeEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONAE") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Boiler" HeaderStyle-Width="6%" HeaderStyle-Wrap="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblfueloilconsumptionboiler" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONBOILER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtBoiler" runat="server" CssClass="input" MaxLength="5" Visible="false"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONBOILER") %>' />
                            <telerik:RadLabel ID="lblfueloilconsumptionboilerEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONBOILER") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="IGG" HeaderStyle-Width="6%" HeaderStyle-Wrap="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblfueloilconsumptionigg" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONIGG") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtIGG" runat="server" CssClass="input" MaxLength="5" Width="80px" Visible="false"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONIGG") %>' />
                            <telerik:RadLabel ID="lblfueloilconsumptioniggEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONIGG") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="C/E" HeaderStyle-Width="6%" HeaderStyle-Wrap="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblfueloilconsumptionce" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONCE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtCE" runat="server" CssClass="input" MaxLength="5" Visible="false"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONCE") %>' />
                            <telerik:RadLabel ID="lblfueloilconsumptionceedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONCE") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="C/HTG" HeaderStyle-Width="6%" HeaderStyle-Wrap="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbltxtCTHG" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONCTHG") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtCTHG" runat="server" CssClass="input" MaxLength="5" Visible="false"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONCTHG") %>' />
                            <telerik:RadLabel ID="lbltxtCTHGedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONCTHG") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="TK CLNG" HeaderStyle-Width="6%" HeaderStyle-Wrap="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTKCLNG" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONTKCLNG") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtTKCLNG" runat="server" CssClass="input" MaxLength="5" Visible="false"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONTKCLNG") %>' />
                            <telerik:RadLabel ID="lblTKCLNGedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONTKCLNG") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="OTH" HeaderStyle-Width="6%" HeaderStyle-Wrap="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOTH" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONOTH") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtOTH" runat="server" CssClass="input" MaxLength="5" Visible="false"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONOTH") %>' />
                            <telerik:RadLabel ID="lblOTHedit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONOTH") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="ROB" HeaderStyle-Width="6%" HeaderStyle-Wrap="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblfueloilconsumptionrob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONROB") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtROB" runat="server" CssClass="input" MaxLength="5" Visible="false"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONROB") %>' />
                            <telerik:RadLabel ID="lblfueloilconsumptionrobEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUELOILCONSUMPTIONROB") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false" HeaderText="Revised ROB" HeaderStyle-Wrap="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRevisedrob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISEDROB") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtRevisedROB" runat="server" CssClass="input" DecimalPlace="2"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISEDROB") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Total Cons" HeaderStyle-Width="7%" HeaderStyle-Wrap="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTotalCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCONSUMPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtTotalCons" runat="server" CssClass="input" MaxLength="5" Visible="false"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCONSUMPTION") %>' />
                            <telerik:RadLabel ID="lblTotalConsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCONSUMPTION") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Avg Cons/Day" HeaderStyle-Width="10%" HeaderStyle-Wrap="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAvgCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGCONSUMPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtAvgCons" runat="server" CssClass="input" MaxLength="5" Visible="false"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGCONSUMPTION") %>' />
                            <telerik:RadLabel ID="lblAvgConsEdit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAVGCONSUMPTION") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                    </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table cellpadding="2" cellspacing="2" style="visibility: hidden">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmHFOConsforTankCleaning" runat="server" Text="HFO Cons for Tank Cleaning"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txthfoconsumptionfortankcleaning" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmHFOConsforCargoHeating" runat="server" Text="HFO Cons for Cargo Heating"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtHFOConsforCargoHeating" runat="server" CssClass="readonlytextbox" Enabled="false" MaxLength="5"
                            Width="80px" />
                    </td>
                </tr>
            </table>
            <telerik:RadGrid ID="gvLubOil" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnNeedDataSource="gvLubOil_NeedDataSource" AllowSorting="false"
                ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDOILCONSUMPTIONID">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />

                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Lub Oils" HeaderStyle-Width="17%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbloilconsumptionid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblloilTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></telerik:RadLabel>
                            <%#  DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Cons" Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblluboilconsumption" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILCONSUMPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lbloilTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></telerik:RadLabel>
                            <eluc:Number ID="txtLUBOILCONSUMPTION" runat="server" CssClass="input" MaxLength="5"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILCONSUMPTION") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false" HeaderText="Qty in Sump">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblluboilqtyinsump" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILQTYINSUMP") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtLUBOILQTYINSUMP" runat="server" CssClass="input" MaxLength="5"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILQTYINSUMP") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="ROB Prv Mth" HeaderStyle-Width="17%">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPreviousROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENINGROB") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Received" HeaderStyle-Width="17%">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblluboilreceived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILRECEIVED") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblluboilreceivedEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILRECEIVED") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="ROB" HeaderStyle-Width="17%">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblluboilrob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILROB") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblluboilrobEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILROB") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false" HeaderText="Revised ROB" HeaderStyle-Width="17%">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRevisedrob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISEDROB") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtRevisedROB" runat="server" CssClass="input" DecimalPlace="2"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREVISEDROB") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false" HeaderText="SG">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblluboilsg" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILSG") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtLUBOILSG" runat="server" CssClass="input" MaxLength="6" DecimalPlace="4"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILSG") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false" HeaderText="Sp Cons">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblluboilspconsumption" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILSPCONSUMPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblluboilspconsumptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILSPCONSUMPTION") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Av Cons/Day" Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblluboilavgconsumptionperday" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILAVGCONSUMPTIONPERDAY") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblluboilavgconsumptionperdayEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILAVGCONSUMPTIONPERDAY") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn Visible="false" HeaderText="Reason for Loss/High Cons">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblluboilreasonforlosshighcons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILREASONFORLOSSHIGHCONS") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadTextBox ID="txtLUBOILREASONFORLOSSHIGHCONS" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLUBOILREASONFORLOSSHIGHCONS") %>'></telerik:RadTextBox>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Total Cons" HeaderStyle-Width="17%">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTotalCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCONSUMPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblTotalConsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCONSUMPTION") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Avg Cons/Day" HeaderStyle-Width="10%">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAvgCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGCONSUMPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lblAvgConsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGCONSUMPTION") %>'></telerik:RadLabel>
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                    </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <b></b>
            <br />
            <telerik:RadGrid ID="gvWater" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnNeedDataSource="gvWater_NeedDataSource" AllowSorting="false"
                ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDVESSELMONTHLYREPORTID">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />


                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Fresh Water" HeaderStyle-Width="17%">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lbloilconsumptionid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILCONSUMPTIONID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblloilTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></telerik:RadLabel>
                            <%#  DataBinder.Eval(Container,"DataItem.FLDOILTYPENAME")%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="ROB Prv Mth" HeaderStyle-Width="17%">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPreviousROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPENINGROB") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Received" HeaderStyle-Width="17%">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblfBunkered" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATERRECEIVED") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Cons" Visible="false">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblwaterconsumption" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATERCONSUMPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <telerik:RadLabel ID="lbloilTypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOILTYPECODE") %>'></telerik:RadLabel>
                            <eluc:Number ID="txtWATERCONSUMPTION" runat="server" CssClass="input" MaxLength="5"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATERCONSUMPTION") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="ROB" HeaderStyle-Width="17%">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblwaterrob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATERROB") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtWATERROB" runat="server" CssClass="input" MaxLength="5"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATERROB") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Total Cons" HeaderStyle-Width="17%">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblTotalCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCONSUMPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtTotalCons" runat="server" CssClass="input" MaxLength="5"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALCONSUMPTION") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Avg Cons/Day" HeaderStyle-Width="10%">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAvgCons" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGCONSUMPTION") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtAvgCons" runat="server" CssClass="input" MaxLength="5"
                                Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAVGCONSUMPTION") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                    </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            <table cellpadding="2" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmTotalFWProduction" runat="server" Text="Total FW Production"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txttotalfwproduction" runat="server" CssClass="readonlytextbox" Enabled="false"
                            Width="80px" />
                    </td>
                </tr>
            </table>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
