<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRMidNightReportHSE.aspx.cs"
    MaintainScrollPositionOnPostback="true" Inherits="CrewOffshoreDMRMidNightReportHSE" %>

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
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Direction" Src="~/UserControls/UserControlDircection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaCondition" Src="~/UserControls/UserControlSeaCondtion.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Numerics" Src="~/UserControls/UserControlNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DMR MidNight Report</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="form1" runat="server">

        <div>
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />


            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <%--<asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />--%>
            <eluc:Status runat="server" ID="ucStatus" />


            <eluc:TabStrip ID="MenuReportTap" TabStrip="true" runat="server" OnTabStripCommand="ReportTapp_TabStripCommand"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand"></eluc:TabStrip>


            <div>
                <div style="top: 40px; position: relative;">
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td width="10%">
                                <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                            </td>
                            <td width="10%">
                                <eluc:Vessel ID="ucVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true"
                                    AppendDataBoundItems="true" Width="150px" />
                            </td>
                            <td width="10%">
                                <telerik:RadLabel ID="lblMaster" runat="server" Text="Master"></telerik:RadLabel>
                            </td>
                            <td width="40%">
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtMaster" runat="server" CssClass="readonlytextbox" Width="200PX"
                                    ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                            <td width="10%">
                                <telerik:RadLabel ID="lblPOB" runat="server" Text="POB"></telerik:RadLabel>
                            </td>
                            <td width="20%">
                                <eluc:Numerics ID="txtPOB" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    IsInteger="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true"
                                    ReadOnly="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblClient" Visible="false" runat="server" Text="Client"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAvgSpeed" runat="server" Text="Avg Spd:"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtClient" runat="server" Visible="false" CssClass="readonlytextbox"
                                    ReadOnly="true">
                                </telerik:RadTextBox>
                                <eluc:Numerics ID="ucAvgSpeed" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
                                    ReadOnly="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblCrew" runat="server" Text="Crew"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Numerics ID="txtCrew" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    IsInteger="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSwell" runat="server" Visible="false" Text="Swell"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVoyageNo" runat="server" Text="Charter"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Numerics ID="ucSwell" runat="server"  Visible="false" DecimalPlace="2" />
                                <span id="spnPickListVoyage">
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtVoyageName" runat="server" Width="200px" CssClass="readonlytextbox"
                                        ReadOnly="true">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtVoyageId" Visible="false" runat="server" Width="0px" ></telerik:RadTextBox>
                                </span>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblLocation" Visible="false" runat="server" Text="Location"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselStatus" runat="server" Text="Vessel Status"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlvesselstatus" runat="server" Width="300px" 
                                    OnSelectedIndexChanged="ddlvesselstatus_SelectedIndexChanged1" EmptyMessage="--Select--" Filter="Contains" MarkFirstMatch="true"
                                    AutoPostBack="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="--Select--" Value="Dummy"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="In Port" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="En-Route to Port" Value="2"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="En-Route to Location" Value="3"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="At Location" Value="4"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Dry Dock" Value="5"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblEstimatedDays" runat="server" Text="Remaining days for Charter"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Numerics ID="txtEstimatedDuration" runat="server" CssClass="readonlytextbox"
                                    ReadOnly="true" IsInteger="true" />
                                Days
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblWaveHeight" runat="server" Text="Wave Height" Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblSeaCondition" runat="server" Text="Sea" Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLatitude" runat="server" Text="Latitude"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Numerics ID="txtWaveHeight" runat="server"  DecimalPlace="2"
                                    Visible="false" />
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtSeaCondition" runat="server" Visible="false" ></telerik:RadTextBox>
                                <eluc:Latitude ID="ucLatitude" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:MultiPort ID="ucPort" runat="server" Enabled="false" CssClass="readonlytextbox"
                                    Width="375px" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblETD" runat="server" Text="Location"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtLocation" runat="server" ></telerik:RadTextBox>
                                <telerik:RadComboBox ID="ddlETDLocation" runat="server" CssClass="readonlytextbox" Width="150px"
                                    Visible="false" Enabled="false" DataTextField="FLDLOCATIONNAME" DataValueField="FLDLOCATIONID">
                                    <DefaultItem Text="--Select--" Value="-1" />
                                </telerik:RadComboBox>
                                <telerik:RadComboBox ID="ddlETALocation" runat="server" Enabled="false" CssClass="readonlytextbox"
                                    Visible="false" Width="150px" DataTextField="FLDLOCATIONNAME" DataValueField="FLDLOCATIONID">
                                    <DefaultItem Text="--Select--" Value="-1" />
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblWind" runat="server" Text="Wind" Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblLongitude" runat="server" Text="Longitude"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Direction ID="ucWindDirection" runat="server" AppendDataBoundItems="true" 
                                    Width="120px" Visible="false" />
                                <eluc:Numerics ID="ucWindSpeed" runat="server"  DecimalPlace="2" Visible="false" />
                                <eluc:Longitude ID="ucLongitude" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblETADate" runat="server" Text="ETA"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtETADate" runat="server" CssClass="readonlytextbox" />
                                <telerik:RadTimePicker ID="txtETATime" runat="server" CssClass="readonlytextbox"
                                    Width="75px" />
                                <%-- <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderETATime" runat="server" AcceptAMPM="false"
                                        ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                        TargetControlID="txtETATime" UserTimeFormat="TwentyFourHour" />--%>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblInstalationType" runat="server" Text="Intsallation Type"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlInstalationType" runat="server" CssClass="readonlytextbox"
                                    ReadOnly="true" EmptyMessage="--Select--" Filter="Contains" MarkFirstMatch="true">
                                    <DefaultItem Text="--Select--" Value="0" />
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblAdvanceRetard" runat="server" Text="Advance/Retard"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlAdvanceRetard" runat="server"  AutoPostBack="true"
                                    OnSelectedIndexChanged="ddlAdvanceRetard_SelectedIndexChanged"
                                    EmptyMessage="--Select--" Filter="Contains" MarkFirstMatch="true">
                                    <Items>
                                        <telerik:RadComboBoxItem Text="Nil" Value="0"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Advance 1.0 hr" Value="1"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Advance 0.5 hr" Value="2"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Retard 0.5 hr" Value="3"></telerik:RadComboBoxItem>
                                        <telerik:RadComboBoxItem Text="Retard 1.0 hr" Value="4"></telerik:RadComboBoxItem>
                                    </Items>
                                </telerik:RadComboBox>
                                <telerik:RadRadioButtonList ID="rbnhourchange" runat="server" RepeatDirection="Horizontal"
                                    Visible="false">
                                    <Items>
                                        <telerik:ButtonListItem Text="Advance" Value="1"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Text="Retard" Value="2"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Text="None" Value="0" Selected="True"></telerik:ButtonListItem>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblETDDate" runat="server" Text="ETD"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtETDDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                                <telerik:RadTimePicker ID="txtETDTime" runat="server" CssClass="readonlytextbox"
                                    Width="75px" />
                                <%--  <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderETDTime" runat="server" AcceptAMPM="false"
                                        ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                        TargetControlID="txtETDTime" UserTimeFormat="TwentyFourHour" />--%>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <font color="blue">Note: Advance / Retard to be done between 0000 – 2400 hrs.</font>
                                <telerik:RadRadioButtonList ID="rbnhourvalue" runat="server" RepeatDirection="Horizontal"
                                    Visible="false">
                                    <Items>
                                        <telerik:ButtonListItem Text="0.5h" Value="1"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Text="1.0h" Value="2"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Text="None" Value="0" Selected="True"></telerik:ButtonListItem>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td valign="top">
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text="Arrival"></telerik:RadLabel>
                            </td>
                            <td valign="top">
                                <eluc:Date ID="txtArrivalDate" runat="server"  Enabled="true" DatePicker="true" TimeProperty="true" />
                                <telerik:RadTimePicker ID="txtArrivalTime" runat="server"  Enabled="true" Width="75px" />
                                <%--<ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderArrivalTime" runat="server"
                                        AcceptAMPM="false" ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99"
                                        MaskType="Time" TargetControlID="txtArrivalTime" UserTimeFormat="TwentyFourHour" />--%>
                            </td>
                            <td valign="top">
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text="Departure"></telerik:RadLabel>
                            </td>
                            <td valign="top">
                                <eluc:Date ID="txtDepartureDate" runat="server"  Enabled="true" DatePicker="true" TimeProperty="true" />
                                <telerik:RadTimePicker ID="txtDepartureTime" runat="server"  Enabled="true"
                                    Width="75px" />
                                <%--  <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderDepartureTime" runat="server"
                                        AcceptAMPM="false" ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99"
                                        MaskType="Time" TargetControlID="txtDepartureTime" UserTimeFormat="TwentyFourHour" />--%>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <u><b>
                                    <telerik:RadLabel ID="lblGenRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                                </b></u>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtGeneralRemarks" runat="server"  Height="50px"
                                    TextMode="MultiLine" Width="500px" />
                            </td>
                            <td>
                                <eluc:Numerics ID="txtPOBClient" runat="server"  IsInteger="true" Visible="false" />
                                <eluc:Numerics ID="txtPOBService" runat="server"  IsInteger="true"
                                    Visible="false" />
                                <eluc:Numerics ID="txtTotalOB" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Visible="false" IsInteger="true" />
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtbulkRemarks" runat="server"  Height="50px" TextMode="MultiLine"
                                    Visible="false" Width="500px" />
                                <eluc:Numerics ID="txtDPTime" runat="server"  ReadOnly="true" Visible="false" />
                                <eluc:Numerics ID="txtDPFuelConsumption" runat="server"  ReadOnly="true"
                                    Visible="false" />
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtMeterologyRemarks" runat="server"  Height="50px"
                                    Visible="false" TextMode="MultiLine" Width="500px" />
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtVesselMovementsRemarks" runat="server"  Height="50px"
                                    Visible="false" TextMode="MultiLine" Width="500px" />
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtFlowmeterRemarks" runat="server"  Height="50px"
                                    Visible="false" TextMode="MultiLine" Width="500px" />
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtCrewRemarks" runat="server"  Height="50px" TextMode="MultiLine"
                                    Visible="false" Width="500px" />
                                <eluc:Numerics ID="ucBreakfast" runat="server"  DecimalPlace="0" Visible="false" />
                                <eluc:Numerics ID="ucLunch" runat="server"  DecimalPlace="0" Visible="false" />
                                <eluc:Numerics ID="ucDinner" runat="server"  DecimalPlace="0" Visible="false" />
                                <eluc:Numerics ID="ucBunks" runat="server"  DecimalPlace="0" Visible="false" />
                                <eluc:Numerics ID="ucSupBreakFast" runat="server"  DecimalPlace="0"
                                    Visible="false" />
                                <eluc:Numerics ID="ucTea1" runat="server"  DecimalPlace="0" Visible="false" />
                                <eluc:Numerics ID="ucSupTea1" runat="server"  DecimalPlace="0" Visible="false" />
                                <eluc:Numerics ID="ucSupLunch" runat="server"  DecimalPlace="0" Visible="false" />
                                <eluc:Numerics ID="ucTea2" runat="server"  DecimalPlace="0" Visible="false" />
                                <eluc:Numerics ID="ucSupTea2" runat="server"  DecimalPlace="0" Visible="false" />
                                <eluc:Numerics ID="ucSupDinner" runat="server"  DecimalPlace="0" Visible="false" />
                                <eluc:Numerics ID="ucSupper" runat="server"  DecimalPlace="0" Visible="false" />
                                <eluc:Numerics ID="ucSupSupper" runat="server"  DecimalPlace="0" Visible="false" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td width="100%">
                                <u><b>
                                    <telerik:RadLabel ID="lblPlannedActivity" runat="server" Text="Look Ahead Planned Activity"></telerik:RadLabel>
                                </b></u>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top;" width="50px">
                                <telerik:RadGrid ID="gvPlannedActivity" RenderMode="Lightweight" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                    CellSpacing="0" GridLines="None">
                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                                        <HeaderStyle Width="102px" />
                                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
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
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblTimeHeader" runat="server" Text="Date"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblPlannedActivityId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDPLANNEDACTIVITYID"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lbllookaheaddate" Visible="false" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNEDDATE")) %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblPlannedActivityEditId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDPLANNEDACTIVITYID"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lbllookaheaddateEdit" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDPLANNEDDATE")) %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="90%"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblActivityHeader" runat="server" Text="Activity"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblActivity" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDACTIVITY"]%>'></telerik:RadLabel>
                                                    <telerik:RadTextBox ID="txtActivityEdit" RenderMode="Lightweight" runat="server"  Width="1000px"
                                                        Text='<%# ((DataRowView)Container.DataItem)["FLDACTIVITY"]%>'>
                                                    </telerik:RadTextBox>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Charter matching your search criteria"
                                            PageSizeLabelText="Charter per page:" />
                                    </MasterTableView>
                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                    </ClientSettings>
                                </telerik:RadGrid>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <u><b>
                                    <telerik:RadLabel ID="lblLookAheadRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                                </b></u>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtLookAheadRemarks" runat="server"  Height="50px"
                                    TextMode="MultiLine" Width="500px" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td>
                                <u><b>
                                    <telerik:RadLabel ID="lblHSEIndicators" runat="server" Text="HSE Indicators"></telerik:RadLabel>
                                </b></u>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblHSEIndicatorsNote" runat="server" Text="Has there been any Incident / Near Miss on board the vessel since the last report. "></telerik:RadLabel>
                                <telerik:RadRadioButtonList ID="rblHSEIndicators" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="rblHSEIndicators_OnSelectedIndexChanged">
                                    <Items>
                                        <telerik:ButtonListItem Value="1" Text="Yes"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="0" Text="No" Selected="True"></telerik:ButtonListItem>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                        </tr>
                    </table>
                    <table width="60%">
                        <tr>
                            <td style="vertical-align: top;">
                                <u><b>
                                    <telerik:RadLabel ID="Literal3" runat="server" Text="Unsafe Acts/Conditions Reported in 24 hrs"></telerik:RadLabel>
                                </b></u>
                                <br />

                                <eluc:TabStrip ID="MenuUnsafeActsAdd" runat="server" OnTabStripCommand="MenuUnsafeActsAdd_TabStripCommand"></eluc:TabStrip>

                                <telerik:RadGrid RenderMode="Lightweight" ID="gvUnsafe" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                    CellSpacing="0" GridLines="None"
                                    OnItemCommand="gvUnsafe_ItemCommand"
                                    OnNeedDataSource="gvUnsafe_NeedDataSource">
                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" DataKeyNames="FLDDIRECTINCIDENTID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
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
                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="70%"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblUnsafeActsHeader" Text="Unsafe Acts/Conditions" runat="server"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblDirectIncidentId" Visible="false" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDIRECTINCIDENTID"]%>'></telerik:RadLabel>
                                                    <asp:LinkButton ID="lblUnsafeActs" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSUMMARY"]%>'
                                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                                    <%-- <telerik:RadLabel ID="lblType" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDTYPE"]%>'></telerik:RadLabel>--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn Visible="false">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="30%"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblDateHeader" Text="Date" runat="server"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDINCIDENTDATE"]%>'></telerik:RadLabel>
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
                    <br />
                    <br />
                    <div class="navSelect" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuHSEIndicators" runat="server" Visible="false" OnTabStripCommand="MenuHSEIndicators_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td>
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvHSEIndicators" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                    CellSpacing="0" GridLines="None"
                                    OnItemDataBound="gvHSEIndicators_ItemDataBound1"
                                    OnNeedDataSource="gvHSEIndicators_NeedDataSource">
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
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="15%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblhpiitem" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNAME"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="7%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblEHHeader" runat="server" Text="Exp Hrs"></telerik:RadLabel>
                                                    <eluc:Tooltip ID="ucEHHeader" runat="server" Text="Exposure Hour" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblEH" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDEH"]%>'></telerik:RadLabel>
                                                    <%--<eluc:Number ID="txtEH" runat="server" Width="50px"  CssClass="readonlytextbox" Enabled="false" IsInteger="true"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDEH"]%>'  BackColor="DarkGray" />--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="7%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblhpiHeader" runat="server" Text="HPI"></telerik:RadLabel>
                                                    <eluc:Tooltip ID="uchpiHeader" runat="server" Text="No of High Potential Incident" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkHpi" runat="server" Width="50px" Text='<%# ((DataRowView)Container.DataItem)["FLDHPI"]%>'
                                                        Visible="false"></asp:LinkButton>
                                                    <telerik:RadLabel ID="lblhpi" runat="server" Width="50px" Text='<%# ((DataRowView)Container.DataItem)["FLDHPI"]%>'></telerik:RadLabel>
                                                    <%--<eluc:Number ID="txthpi" runat="server" Width="50px" CssClass="readonlytextbox" Enabled="false"  IsInteger="true"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDHPI"]%>' />--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="7%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblEnvironmentalIncidentHeader" runat="server" Text="Env Release"></telerik:RadLabel>
                                                    <eluc:Tooltip ID="ucEnvironmentalIncidentHeader" runat="server" Text="Environmental Release" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkEnvRelease" runat="server" Width="50px" Text='<%# ((DataRowView)Container.DataItem)["FLDENVIRONMENTALINCIDENT"]%>'
                                                        Visible="false"></asp:LinkButton>
                                                    <telerik:RadLabel ID="lblEnvironmentalIncident" runat="server" Width="50px" Text='<%# ((DataRowView)Container.DataItem)["FLDENVIRONMENTALINCIDENT"]%>'></telerik:RadLabel>
                                                    <%--<eluc:Number ID="txtEnvironmentalIncident" runat="server" Width="50px" IsInteger="true"
                                                    CssClass="readonlytextbox" Enabled="false" Text='<%# ((DataRowView)Container.DataItem)["FLDENVIRONMENTALINCIDENT"]%>' />--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="7%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblltiHeader" runat="server" Text="LTI"></telerik:RadLabel>
                                                    <eluc:Tooltip ID="ucltiHeader" runat="server" Text="No of Lost time Injury" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkLit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDLTI"]%>'
                                                        Visible="false"></asp:LinkButton>
                                                    <telerik:RadLabel ID="lbllit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDLTI"]%>'></telerik:RadLabel>
                                                    <%--<eluc:Number ID="txtlti" runat="server" Width="50px" CssClass="readonlytextbox" Enabled="false" IsInteger="true"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDLTI"]%>' />--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="7%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblrwcHeader" runat="server" Text="RWC"></telerik:RadLabel>
                                                    <eluc:Tooltip ID="ucrwcHeader" runat="server" Text="No of Restricted Work Cases" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkrwc" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDRWC"]%>'
                                                        Visible="false"></asp:LinkButton>
                                                    <telerik:RadLabel ID="lblrwc" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDRWC"]%>'></telerik:RadLabel>
                                                    <%--<eluc:Number ID="txtrwc" runat="server" CssClass="readonlytextbox" Enabled="false" Width="50px" IsInteger="true"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDRWC"]%>' />--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="7%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblmtcHeader" runat="server" Text="MTC"></telerik:RadLabel>
                                                    <eluc:Tooltip ID="ucmtcHeader" runat="server" Text="No of Medical Treatment Case" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkMtc" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDMTC"]%>'
                                                        Visible="false"></asp:LinkButton>
                                                    <telerik:RadLabel ID="lblmtc" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDMTC"]%>'></telerik:RadLabel>
                                                    <%--<eluc:Number ID="txtmtc" runat="server" Width="50px" CssClass="readonlytextbox" Enabled="false" IsInteger="true"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDMTC"]%>' />--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="7%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblfacHeader" runat="server" Text="FAC"></telerik:RadLabel>
                                                    <eluc:Tooltip ID="ucfacHeader" runat="server" Text="No of First Aid Case" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkFac" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDFAC"]%>'
                                                        Visible="false"></asp:LinkButton>
                                                    <telerik:RadLabel ID="lblfac" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDFAC"]%>'></telerik:RadLabel>
                                                    <%--<eluc:Number ID="txtfac" runat="server" Width="50px" CssClass="readonlytextbox" Enabled="false" IsInteger="true"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDFAC"]%>' />--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="7%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblnearmissHeader" runat="server" Text="NM"></telerik:RadLabel>
                                                    <eluc:Tooltip ID="ucnearmissHeader" runat="server" Text="Near Misses" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkNearmiss" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNEARMISS"]%>'
                                                        Visible="false"></asp:LinkButton>
                                                    <telerik:RadLabel ID="lblnearmiss" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNEARMISS"]%>'></telerik:RadLabel>
                                                    <%--<eluc:Number ID="txtnearmiss" CssClass="readonlytextbox" Enabled="false" runat="server" Width="50px" IsInteger="true"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDNEARMISS"]%>' />--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="7%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblUnsafeActsHeader" runat="server" Text="UAUC"></telerik:RadLabel>
                                                    <eluc:Tooltip ID="ucUnsafeActsHeader" runat="server" Text="Unsafe Acts / Unsafe Conditions" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkUnsafeActs" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDUNSAFEACTS"]%>'
                                                        Visible="false"></asp:LinkButton>
                                                    <telerik:RadLabel ID="lblUnsafeActs" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDUNSAFEACTS"]%>'></telerik:RadLabel>
                                                    <%--<eluc:Number ID="txtUnsafeActs" CssClass="readonlytextbox" Enabled="false" runat="server" Width="50px" IsInteger="true"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDUNSAFEACTS"]%>' />--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="7%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblstopcardsHeader" runat="server" Text="STOP  Cards"></telerik:RadLabel>
                                                    <eluc:Tooltip ID="ucstopcardsHeader" runat="server" Text="No. of STOP Cards" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Numerics ID="ucstopcards" runat="server" Type="Number" Width="65px" CssClass="input_mandatory"
                                                        Enabled="true" IsInteger="true" Text='<%# ((DataRowView)Container.DataItem)["FLDNOOFSTOPCARDS"]%>'
                                                        Visible="false" />
                                                    <telerik:RadLabel ID="lblstopcards" runat="server" Width="50px" Text='<%# ((DataRowView)Container.DataItem)["FLDNOOFSTOPCARDS"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="7%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblNoofRiskAssesmentHeader" runat="server" Text="RA"></telerik:RadLabel>
                                                    <eluc:Tooltip ID="ucNoofRiskAssesmentHeader" runat="server" Text="No. of Risk Assessment" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkNoofRiskAssesment" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNOOFRISKASSESSMENT"]%>'
                                                        Visible="false"></asp:LinkButton>
                                                    <telerik:RadLabel ID="lblNoofRiskAssesment" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNOOFRISKASSESSMENT"]%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="7%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblExercisesandDrillsHeader" runat="server" Text="ED"></telerik:RadLabel>
                                                    <eluc:Tooltip ID="ucExercisesandDrillsHeader" runat="server" Text="No of Exercises and Drills" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Numerics ID="ucExercisesandDrills" runat="server" Width="65px" CssClass="input_mandatory"
                                                        Enabled="true" IsInteger="true" Text='<%# ((DataRowView)Container.DataItem)["FLDEXERCISESANDDRILLS"]%>'
                                                        Visible="false" />
                                                    <telerik:RadLabel ID="lblExercisesandDrills" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDEXERCISESANDDRILLS"]%>'></telerik:RadLabel>
                                                    <%--<eluc:Number ID="txtExercisesandDrills" runat="server" Width="50px" CssClass="readonlytextbox" Enabled="false" 
                                                    IsInteger="true" Text='<%# ((DataRowView)Container.DataItem)["FLDEXERCISESANDDRILLS"]%>' />--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="7%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblnoofsafetyHeader" runat="server" Text="HSE Meeting Held"></telerik:RadLabel>
                                                    <eluc:Tooltip ID="ucnoofsafetyHeader" runat="server" Text="No of HSE Meeting Held" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Numerics ID="ucnoofsafety" runat="server" Width="50px" CssClass="input_mandatory"
                                                        Enabled="true" IsInteger="true" Text='<%# ((DataRowView)Container.DataItem)["FLDNOOFSAFETY"]%>'
                                                        Visible="false" />
                                                    <telerik:RadLabel ID="lblnoofsafety" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNOOFSAFETY"]%>'></telerik:RadLabel>
                                                    <%--<eluc:Number ID="txtnoofsafety" runat="server" Width="50px" CssClass="readonlytextbox" Enabled="false" IsInteger="true"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDNOOFSAFETY"]%>' />--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="7%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblPTWIssuedHeader" runat="server" Text="PTW Issued"></telerik:RadLabel>
                                                    <eluc:Tooltip ID="uclblPTWIssuedHeader" runat="server" Text="No of Permit To Work Issued" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Numerics ID="ucPTWIssued" runat="server" Width="50px" CssClass="input_mandatory"
                                                        Enabled="true" IsInteger="true" Text='<%# ((DataRowView)Container.DataItem)["FLDPTWISSUED"]%>'
                                                        Visible="false" />
                                                    <telerik:RadLabel ID="lblPTWIssued" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDPTWISSUED"]%>'></telerik:RadLabel>
                                                    <%--<eluc:Number ID="txtPTWIssued" runat="server" Width="50px" CssClass="readonlytextbox" Enabled="false" IsInteger="true"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDPTWISSUED"]%>' />--%>
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
                    <table cellpadding="2" cellspacing="2" width="60%">
                        <tr>
                            <td>
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvLaggingIndicators" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                    CellSpacing="0" GridLines="None"
                                    OnItemCommand="gvLaggingIndicators_ItemCommand"
                                    OnNeedDataSource="gvLaggingIndicators_NeedDataSource">
                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" DataKeyNames="FLDINSPECTIONINCIDENTID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
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
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblReferenceNumberHeader" runat="server" Text="Reference Number"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblReferenceId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDINSPECTIONINCIDENTID"]%>'></telerik:RadLabel>
                                                    <asp:LinkButton ID="lblReferenceNumber" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDINCIDENTREFNO"]%>'
                                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblClassificationHeader" runat="server" Text="Classification"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblClassification" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDINCIDENTCLASSIFICATION"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblCategoryHeader" runat="server" Text="Category"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCATEGORYNAME"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblSubCategoryHeader" runat="server" Text="Sub Category"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblSubCategory" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSUBCATEGORYNAME"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblConsequenceCategoryHeader" runat="server" Text="Consequence Category"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblConsequenceCategory" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCONSEQUENCECATEGORY"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblTitleHeader" runat="server" Text="Title"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDINCIDENTTITLE"]%>'></telerik:RadLabel>
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
                    <table cellpadding="2" cellspacing="2">
                        <tr>
                            <td>
                                <u><b>
                                    <telerik:RadLabel ID="lblComments" runat="server" Text="Remarks"></telerik:RadLabel>
                                </b></u>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtComments" runat="server"  Height="50px" TextMode="MultiLine"
                                    Width="500px" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <table>
                        <tr>
                            <td>
                                <img id="ImgYellow" runat="server" alt="" src="<%$ PhoenixTheme:images/yellow-symbol.png%>" />
                                <telerik:RadLabel ID="lbl30Days" runat="server" Text="Due within 30 Days"></telerik:RadLabel>
                            </td>
                            <td>
                                <img id="ImgGreen" runat="server" alt="" src="<%$ PhoenixTheme:images/ORANGE-symbol.png%>" />
                                <telerik:RadLabel ID="lbl15Days" runat="server" Text="Due within 15 Days"></telerik:RadLabel>
                            </td>
                            <td>
                                <img id="ImgRed" runat="server" alt="" src="<%$ PhoenixTheme:images/red-symbol.png%>" />
                                <telerik:RadLabel ID="lblOverDue" runat="server" Text="Overdue"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table width="60%">
                        <tr>
                            <td style="vertical-align: top;">
                                <u><b>
                                    <telerik:RadLabel ID="lblAudit" runat="server" Text="Ship Audits Due in 30 Days"></telerik:RadLabel>
                                </b></u>
                                <br />
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvShipAudit" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                    CellSpacing="0" GridLines="None"
                                    OnItemDataBound="gvShipAudit_ItemDataBound"
                                    OnNeedDataSource="gvShipAudit_NeedDataSource">
                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" DataKeyNames="FLDREFERENCENUMBER" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
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
                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="70%"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblAuditHeader" Text="Audit" runat="server"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDREFERENCENUMBER"]%>'></telerik:RadLabel>
                                                    <%-- <telerik:RadLabel ID="lblType" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDTYPE"]%>'></telerik:RadLabel>--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblDateHeader" Text="Due Date" runat="server"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                                    <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDUEDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>

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
                    <table width="60%">
                        <tr>
                            <td style="vertical-align: top;">
                                <u><b>
                                    <telerik:RadLabel ID="lblShipTasksDue" runat="server" Text="Ship Tasks Due in 30 Days"></telerik:RadLabel>
                                </b></u>
                                <br />
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvShipTasks" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                    CellSpacing="0" GridLines="None"
                                    OnItemDataBound="gvShipTasks_ItemDataBound1"
                                    OnNeedDataSource="gvShipTasks_NeedDataSource">
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
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="50%"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblShipTaskHeader" Text="Ship Task" runat="server"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblShipTask" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCORRECTIVEACTION"]%>'></telerik:RadLabel>
                                                    <%-- <telerik:RadLabel ID="lblType" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDTYPE"]%>'></telerik:RadLabel>--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblDateHeader" Text="Due Date" runat="server"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                                    <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDUEDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>

                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblShipTaskDeficiencyHeader" Text="Deficiency Description" runat="server"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblShipTaskDeficiency" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDEFICIENCYDETAILS"]%>'></telerik:RadLabel>
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
                    <table width="60%">
                        <tr>
                            <td style="vertical-align: top;">
                                <u><b>
                                    <telerik:RadLabel ID="Literal2" runat="server" Text="Certificates Due in 30 Days"></telerik:RadLabel>
                                </b></u>
                                <br />
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvCertificates" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                    CellSpacing="0" GridLines="None"
                                    OnItemDataBound="gvCertificates_ItemDataBound"
                                    OnNeedDataSource="gvCertificates_NeedDataSource">
                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" DataKeyNames="FLDCERTIFICATENO" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
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
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblCertificatesHeader" Text="Certificates" runat="server"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblCertificates" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCERTIFICATENO"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblExpiryDateHeader" Text="Expiry Date" runat="server"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDATEOFEXPIRY", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                                                    <%--<asp:Image ID="imgFlag" runat="server" Visible="false" />--%>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblSurveyTypeHeader" Text="Next Survey Type" runat="server"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblSurveyTypeName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSURVEYTYPENAME"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblDueDateHeader" Text="Due Date" runat="server"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                                    <telerik:RadLabel ID="lblDueDate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDUEDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>

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
                    <table id="Table1" cellpadding="2" cellspacing="2" width="100%" runat="server" visible="false">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCrewListSOBYN" runat="server" Text="Is Crew List Correctly reflecting Staff on board YN"></telerik:RadLabel>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlCrewListSOBYN" runat="server" >
                                    <asp:ListItem Value="0" Selected="True">No</asp:ListItem>
                                    <asp:ListItem Value="1">Yes</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCrewListSOBRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCrewListSOBRemarks" runat="server"  Height="50px"
                                    TextMode="MultiLine" Width="500px" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
