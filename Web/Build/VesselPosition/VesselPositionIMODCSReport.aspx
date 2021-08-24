<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionIMODCSReport.aspx.cs"
    Inherits="VesselPositionIMODCSReport" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Reports</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvDayToDayReport.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
               fade('statusmessage');
           }
        </script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewCourseList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlCourseListEntry" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlDayToDayReport">
            <eluc:TabStrip ID="MainTab" runat="server" TabStrip="true" OnTabStripCommand="MainTab_TabStripCommand"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input" AppendDataBoundItems="true" AutoPostBack="true" VesselsOnly="true" SyncActiveVesselsOnly="True" AssignedVessels="true"
                            OnTextChangedEvent="ddlVessel_TextChangedEvent"/>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlFleet" runat="server" CssClass="input" AutoPostBack="true" OnTextChanged="ddlVessel_TextChangedEvent" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblyear" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtFrom" CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblmonth" runat="server" Text="Todate"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtTo" CssClass="input" DatePicker="true" />
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuDayToDayReportTab" runat="server" OnTabStripCommand="DayToDayReportTab_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvDayToDayReport" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvDayToDayReport_RowCommand"
                AllowSorting="false" ShowFooter="false" ShowHeader="true"
                EnableViewState="false" OnNeedDataSource="gvDayToDayReport_NeedDataSource"
                AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
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
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                    <ColumnGroups>
                            <telerik:GridColumnGroup HeaderText="Fuel Oil Consumption (MT)" Name="ConsGroup" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                         <telerik:GridColumnGroup HeaderText="ROB (MT)" Name="ROBGroup" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                         <telerik:GridColumnGroup HeaderText="Bunkered (MT)" Name="BUNKERGroup" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                         <telerik:GridColumnGroup HeaderText="De-Bunkered (MT)" Name="DeBUNKERGroup" HeaderStyle-HorizontalAlign="Center">
                            </telerik:GridColumnGroup>
                        </ColumnGroups>
                    <Columns>
                         <telerik:GridTemplateColumn HeaderText="Vessel" HeaderStyle-Wrap="false">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStartDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Time" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEndDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEUTC","{0:HH:mm}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Report Type" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReportType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREPORTTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Laden/Ballast" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLadenBallast" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBALASTLADEN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Cargo (MT)" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCargoob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOONBOARD") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Port" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPort" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="UNLOCODE" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUNLOCODE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="IMO number" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIMONumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMONUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ship type" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblShipType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Gross <br/>tonnage (GT)" HeaderStyle-Wrap="true" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGrossTonnage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREGISTEREDGT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Net <br/>tonnage (NT)" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNetTonnage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREGISTEREDNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Deadweight <br/> tonnage (DWT)" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDeadweightTonnage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDWTSUMMER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="EEDI(gCO2/t.nm)" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEEDI" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEEDI") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ice class" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIceclass" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDICECLASS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Main propulsion <br/> power" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMainpropulsionpower" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEPOWEROUTPUT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Auxiliary <br/> engine(s)" HeaderStyle-HorizontalAlign="Center" Visible="false">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAuxiliaryenginer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAEPOWEROUTPUT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Distance <br/> travelled (nm)" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDistancetravelled" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISTANCETRAVELLED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Hours underway (h)" HeaderStyle-HorizontalAlign="Center">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblHoursunderway" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOURSUNDERWAY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ConsGroup" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDHFOCONS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHFOCONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="LFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ConsGroup" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDLFOCONS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLSFOROB") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="VLSFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ConsGroup"  HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDVLSFORMCONS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVLSFORMCONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ULSFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ConsGroup"  HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDULSFORMCONS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDULSFORMCONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="MDO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ConsGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDMDOCONS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMDOCONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="MGO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ConsGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDMGOCONS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMGOCONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="VLSMGO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ConsGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDVLSFOFMCONS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVLSFODMCONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ULMGO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ConsGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDULSFOFMCONS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDULSFOFMCONS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="HFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ROBGroup" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDHFOROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHFOROB") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="LFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ROBGroup" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDLSFOROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLSFOROB") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="VLSFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ROBGroup"  HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDVLSFORMROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVLSFORMROB") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ULSFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ROBGroup"  HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDULSFORMROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDULSFORMROB") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="MDO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ROBGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDMDOROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMDOROB") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="MGO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ROBGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDMGOROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMGOROB") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="VLSMGO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ROBGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDVLSFODMROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVLSFODMROB") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ULMGO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="ROBGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDULSFOFMROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDULSFOFMROB") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="HFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="BUNKERGroup" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDHFOBUNKER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHFOBUNKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="LFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="BUNKERGroup" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDLSFOBUNKER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLSFOBUNKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="VLSFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="BUNKERGroup"  HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDVLSFORMBUNKER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVLSFORMBUNKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ULSFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="BUNKERGroup"  HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDULSFORMBUNKER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDULSFORMBUNKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="MDO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="BUNKERGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDMDOBUNKER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMDOBUNKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="MGO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="BUNKERGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDMGOBUNKER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMGOBUNKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="VLSMGO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="BUNKERGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDVLSFODMBUNKER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVLSFODMBUNKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ULMGO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="BUNKERGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDULSFOFMBUNKER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDULSFOFMBUNKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>




                        <telerik:GridTemplateColumn HeaderText="HFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="DeBUNKERGroup" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDHFODEBUNKER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHFODEBUNKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="LFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="DeBUNKERGroup" HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDLSFODEBUNKER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLSFODEBUNKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="VLSFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="DeBUNKERGroup"  HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDVLSFORMDEBUNKER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVLSFORMDEBUNKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ULSFO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="DeBUNKERGroup"  HeaderStyle-Width="50px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDULSFORMDEBUNKER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDULSFORMDEBUNKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="MDO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="DeBUNKERGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDMDODEBUNKER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMDODEBUNKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="MGO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="DeBUNKERGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDMGODEBUNKER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMGODEBUNKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="VLSMGO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="DeBUNKERGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDVLSFODMDEBUNKER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVLSFODMDEBUNKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ULMGO" HeaderStyle-HorizontalAlign="Center" ColumnGroupName="DeBUNKERGroup"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="FLDULSFOFMDEBUNKER" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDULSFOFMDEBUNKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="BDN Attached" HeaderStyle-HorizontalAlign="Center"  HeaderStyle-Width="70px">
                            <ItemStyle Wrap="True" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="BDNYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBDNYN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
