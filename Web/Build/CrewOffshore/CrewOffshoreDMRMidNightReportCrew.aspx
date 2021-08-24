<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRMidNightReportCrew.aspx.cs"
    Inherits="CrewOffshoreDMRMidNightReportCrew" %>

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
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Numerics" Src="~/UserControls/UserControlNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>DMR MidNight Report</title>
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
                                <eluc:Decimal ID="ucAvgSpeed" runat="server" CssClass="readonlytextbox" DecimalPlace="2"
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
                                <eluc:Decimal ID="txtWaveHeight" runat="server"  DecimalPlace="2"
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
                                <eluc:Number ID="ucWindSpeed" runat="server"  DecimalPlace="2" Visible="false" />
                                <eluc:Longitude ID="ucLongitude" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblETADate" runat="server" Text="ETA"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtETADate" runat="server" CssClass="readonlytextbox" ReadOnly="true" TimeProperty="true"
                                    DatePicker="true" />
                                <%--    <telerik:RadTextBox RenderMode="Lightweight" ID="txtETATime" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                        Width="50px" />--%>
                                <%-- <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderETATime" runat="server" AcceptAMPM="false"
                                        ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                        TargetControlID="txtETATime" UserTimeFormat="TwentyFourHour" />--%>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblInstalationType" runat="server" Text="Installation Type"></telerik:RadLabel>
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
                                <eluc:Date ID="txtETDDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" TimeProperty="true"
                                    DatePicker="true" />
                                <%--  <asp:TextBox ID="txtETDTime" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                        Width="50px" />--%>
                                <%-- <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderETDTime" runat="server" AcceptAMPM="false"
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
                                <%--<asp:TextBox ID="txtArrivalTime" runat="server"  Enabled="true" Width="50px" />--%>
                                <%--  <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderArrivalTime" runat="server"
                                        AcceptAMPM="false" ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99"
                                        MaskType="Time" TargetControlID="txtArrivalTime" UserTimeFormat="TwentyFourHour" />--%>
                            </td>
                            <td valign="top">
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text="Departure"></telerik:RadLabel>
                            </td>
                            <td valign="top">
                                <eluc:Date ID="txtDepartureDate" runat="server"  Enabled="true" DatePicker="true" TimeProperty="true" />
                                <%--<asp:TextBox ID="txtDepartureTime" runat="server"  Enabled="true"
                                        Width="50px" />--%>
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
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtGeneralRemarks" runat="server"  Height="50px"
                                    TextMode="MultiLine" Width="500px" />
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
                                        AutoGenerateColumns="false" TableLayout="Fixed" >
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
                                        <PagerStyle Mode="NextPrevNumericAndAdvanced"  AlwaysVisible="true" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Charter matching your search criteria"
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
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtComments" runat="server"  Height="50px" TextMode="MultiLine"
                                    Visible="false" Width="500px" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td width="10%">
                                <telerik:RadLabel ID="lblCrewOff" runat="server" Text="Crew Off"></telerik:RadLabel>
                            </td>
                            <td width="20%">
                                <eluc:Numerics ID="txtCrewOff" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    IsInteger="true" />
                            </td>
                            <td width="10%">
                                <telerik:RadLabel ID="lblCrewOn" runat="server" Text="Crew On"></telerik:RadLabel>
                            </td>
                            <td width="20%">
                                <eluc:Numerics ID="txtCrewOn" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    IsInteger="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPOBCrew" runat="server" Text="POB (Crew)"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Numerics ID="txtPOBCrew" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    IsInteger="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPOBClient" runat="server" Text="POB (Client)"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Numerics ID="txtPOBClient" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    IsInteger="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPOBService" runat="server" Text="POB (Service/Supernumerary)"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Numerics ID="txtPOBService" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    IsInteger="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblTotalOB" runat="server" Text="Total OnBoard"></telerik:RadLabel>
                            </td>
                            <td colspan="5">
                                <eluc:Numerics ID="txtTotalOB" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    IsInteger="true" />
                            </td>
                        </tr>
                        <tr>
                            <td width="10%">
                                <telerik:RadLabel ID="lblPOBMarineCrew" runat="server" Text="POB Marine Crew" Visible="false"></telerik:RadLabel>
                            </td>
                            <td width="20%">
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtPOBMarineCrew" runat="server" CssClass="readonlytextbox" Visible="false"
                                    ReadOnly="true">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblLifeBoatCapacity" runat="server" Text="Life Boat Capacity"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Numerics ID="ucLifeBoatCapacity" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    IsInteger="true" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblPassengerCount" runat="server" Text="No.Of Passenger"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Numerics ID="ucPassenger" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                    IsInteger="true" />
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr>
                            <td>
                                <u><b>
                                    <telerik:RadLabel ID="lblCrewRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                                </b></u>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtCrewRemarks" runat="server"  Height="50px" TextMode="MultiLine"
                                    Width="500px" />
                            </td>
                        </tr>
                        <tr>
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
                        </tr>
                    </table>
                    <hr />
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td width="100%" style="vertical-align: top;">
                                <u><b>
                                    <telerik:RadLabel ID="lblShipCrew" runat="server" Text="Ship's Crew"></telerik:RadLabel>
                                </b></u>
                                <br />
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvShipCrew" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                    CellSpacing="0" GridLines="None"
                                    OnNeedDataSource="gvShipCrew_NeedDataSource"
                                    OnItemDataBound="gvShipCrew_ItemDataBound1">
                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" DataKeyNames="FLDEMPLOYEEID" TableLayout="Fixed"  Height="10px">
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
                                            <telerik:GridTemplateColumn HeaderText="Rank">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblRankHeader" runat="server" Text="Rank"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblSignOnRank" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDSIGNONRANKNAME"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>

                                            <telerik:GridTemplateColumn HeaderText="Name">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblNameHeader" runat="server" Text="Name"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblEmployeeName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNAME"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Days Onboard">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <ItemTemplate>
                                                <telerik:RadLabel ID="lblonboardoverdue" Visible="false" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDAYSONBOARDOVERDUE"]%>'></telerik:RadLabel>

                                                    <telerik:RadLabel ID="lblOnboardDays" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDAYSONBOARD") %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="Last Rest Hour Recorded Date">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblRHDateHeader" runat="server" Text="Last RH Recorded Date"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblRHDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDLASTRHDATE")) %>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn HeaderText="No of NC">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblNCCountHeader" runat="server" Text="No of NC"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblLevelCode" Visible="false" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDLEVELCODE"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblNCCount" Visible="false" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNCCOUNT"]%>'></telerik:RadLabel>
                                                    <asp:LinkButton ID="lnkNCCount" Visible="false" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNCCOUNT"]%>'></asp:LinkButton>
                                                    <asp:Image ID="imgFlag" runat="server" Visible="false" ToolTip="Level 2 – Escalated to office" />
                                                    <asp:Image ID="Imageyellow" runat="server" Visible="false" ToolTip="Level 1 – addressed onboard" />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                        <PagerStyle Mode="NextPrevNumericAndAdvanced"  AlwaysVisible="true" PageButtonCount="10"  PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCrewListSOBYN" runat="server" Text="Is Crew List Correctly reflecting Staff on board YN"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDropDownList ID="ddlCrewListSOBYN" runat="server" >
                                    <Items>
                                        <telerik:DropDownListItem Value="0" Selected="True" Text="No" />
                                        <telerik:DropDownListItem Value="1" Text="Yes" />
                                    </Items>
                                </telerik:RadDropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCrewListSOBRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtCrewListSOBRemarks" runat="server"  Height="50px"
                                    TextMode="MultiLine" Width="500px" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <table>
                        <tr>
                            <td>
                                <img id="ImgGreen" runat="server" alt="" src="<%$ PhoenixTheme:images/green-symbol.png%>" />
                                <telerik:RadLabel ID="lbllast60Days" runat="server" Text="Due within 60 days"></telerik:RadLabel>
                            </td>
                            <td>
                                <img id="ImgYellow" runat="server" alt="" src="<%$ PhoenixTheme:images/yellow-symbol.png%>" />
                                <telerik:RadLabel ID="lbllast30Days" runat="server" Text="Due within 30 days"></telerik:RadLabel>
                            </td>
                            <td>
                                <img id="ImgRed" runat="server" alt="" src="<%$ PhoenixTheme:images/red-symbol.png%>" />
                                <telerik:RadLabel ID="lbllast15Days" runat="server" Text="Due within 15 days"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                    <br />
                    &nbsp;<b>Licence(s)</b>
                    <br />
                    <div id="divGrid" style="position: relative; overflow: auto; z-index: 0">
                        <%--     <asp:GridView ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                OnRowDataBound="gvCrew_RowDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                                EnableViewState="false" AllowSorting="true" OnSorting="gvCrew_Sorting">
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <Columns>
                        --%>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrew" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None"
                            OnItemDataBound="gvCrew_ItemDataBound"
                            OnNeedDataSource="gvCrew_NeedDataSource">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDMETEOROLOGYDATAID" TableLayout="Fixed"  Height="10px">
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
                                            <asp:LinkButton ID="lbkName" runat="server" CommandName="Sort" CommandArgument="FLDNAME">Emp Name&nbsp;</asp:LinkButton>
                                            <img id="FLDNAME" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                            <telerik:RadLabel ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkDocument" runat="server" CommandName="Sort" CommandArgument="FLDDOCUMENTNAME">Document&nbsp;</asp:LinkButton>
                                            <img id="FLDDOCUMENTNAME" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDocument" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLICENCE") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblLicenceNationalityHeader" runat="server">Licence Nationality</telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblLicCourNationality" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCNATIONALITY") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lnkExpiryDate" runat="server" CommandName="Sort" CommandArgument="FLDEXPIRYDATE">Expiry Date&nbsp;</asp:LinkButton>
                                            <img id="FLDEXPIRYDATE" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblExpiryDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE") %>' />
                                            <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" ScrollHeight="" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>

                    </div>

                    <br />
                    &nbsp;<b>Course(s)</b>
                    <br />
                    <div id="div8" style="position: relative; overflow: auto; z-index: 0">
                        <%-- <asp:GridView ID="gvCrewCourse" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            OnRowDataBound="gvCrewCourse_RowDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                            EnableViewState="false" AllowSorting="true" OnSorting="gvCrewCourse_Sorting">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <Columns>--%>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewCourse" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None"
                            OnItemDataBound="gvCrewCourse_ItemDataBound"
                            OnNeedDataSource="gvCrewCourse_NeedDataSource">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed"  Height="10px">
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
                                            <telerik:RadLabel ID="lblName" runat="server" CommandName="Sort" CommandArgument="FLDNAME">
                                                Emp Name&nbsp;
                                            </telerik:RadLabel>
                                            <img id="FLDNAME" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                            <telerik:RadLabel ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblCourseTypeHeader" runat="server">Course Type</telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCourseType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSETYPE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lnkDocument" runat="server" CommandName="Sort" CommandArgument="FLDDOCUMENTNAME">
                                                Document&nbsp;
                                            </telerik:RadLabel>
                                            <img id="FLDDOCUMENTNAME" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDocument" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblCourseNationalityHeader" runat="server">Course Nationality</telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblLicCourNationality" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCNATIONALITY") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lnkExpiryDate" runat="server" CommandName="Sort" CommandArgument="FLDEXPIRYDATE">
                                                Expiry Date&nbsp;
                                            </telerik:RadLabel>
                                            <img id="FLDEXPIRYDATE" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblExpiryDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE") %>' />
                                            <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" ScrollHeight="" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </div>

                    <br />
                    &nbsp;<b>Other Document(s)</b>
                    <br />
                    <div id="div9" style="position: relative; overflow: auto; z-index: 0">
                        <%-- <asp:GridView ID="gvCrewOtherDoc" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            OnRowDataBound="gvCrewOtherDoc_RowDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                            EnableViewState="false" AllowSorting="true" OnSorting="gvCrewOtherDoc_Sorting">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <Columns>--%>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewOtherDoc" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None"
                            OnItemDataBound="gvCrewOtherDoc_ItemDataBound"
                            OnNeedDataSource="gvCrewOtherDoc_NeedDataSource">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed"  Height="10px">
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
                                            <telerik:RadLabel ID="lbkName" runat="server" CommandName="Sort" CommandArgument="FLDNAME">
                                                Emp Name&nbsp;
                                            </telerik:RadLabel>
                                            <img id="FLDNAME" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                            <telerik:RadLabel ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lnkDocument" runat="server" CommandName="Sort" CommandArgument="FLDDOCUMENTNAME">
                                                Document&nbsp;
                                            </telerik:RadLabel>
                                            <img id="FLDDOCUMENTNAME" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDocument" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lnkExpiryDate" runat="server" CommandName="Sort" CommandArgument="FLDEXPIRYDATE">
                                                Expiry Date&nbsp;
                                            </telerik:RadLabel>
                                            <img id="FLDEXPIRYDATE" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblExpiryDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE") %>' />
                                            <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced"  AlwaysVisible="true" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" ScrollHeight="" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </div>

                    <br />
                    &nbsp;<b>Travel Document(s)</b>
                    <br />
                    <div id="div3" style="position: relative; overflow: auto; z-index: 0">
                        <%--  <asp:GridView ID="gvCrewTravel" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            OnRowDataBound="gvCrewTravel_RowDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                            EnableViewState="false" AllowSorting="true" OnSorting="gvCrewTravel_Sorting">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <Columns>--%>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewTravel" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None"
                            OnItemDataBound="gvCrewTravel_ItemDataBound"
                            OnNeedDataSource="gvCrewTravel_NeedDataSource">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed"  Height="10px">
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
                                            <telerik:RadLabel ID="lbkName" runat="server" CommandName="Sort" CommandArgument="FLDNAME">
                                                Emp Name&nbsp;
                                            </telerik:RadLabel>
                                            <img id="FLDNAME" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                            <telerik:RadLabel ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lnkDocument" runat="server" CommandName="Sort" CommandArgument="FLDDOCUMENTNAME">
                                                Document&nbsp;
                                            </telerik:RadLabel>
                                            <img id="FLDDOCUMENTNAME" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDocument" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblNationalityHeader" runat="server">Nationality</telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDocNationality" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCNATIONALITY") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lnkExpiryDate" runat="server" CommandName="Sort" CommandArgument="FLDEXPIRYDATE">
                                                Expiry Date&nbsp;
                                            </telerik:RadLabel>
                                            <img id="FLDEXPIRYDATE" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblExpiryDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE") %>' />
                                            <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced"  AlwaysVisible="true" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" ScrollHeight="" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </div>

                    <br />
                    &nbsp;<b>Medical(s)</b>
                    <br />
                    <div id="div5" style="position: relative; overflow: auto; z-index: 0">
                        <%-- <asp:GridView ID="gvCrewMedical" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            OnRowDataBound="gvCrewMedical_RowDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                            EnableViewState="false" AllowSorting="true" OnSorting="gvCrewMedical_Sorting">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <Columns>--%>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewMedical" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None"
                            OnItemDataBound="gvCrewMedical_ItemDataBound"
                            OnNeedDataSource="gvCrewMedical_NeedDataSource">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" TableLayout="Fixed"  Height="10px">
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
                                            <telerik:RadLabel ID="lbkName" runat="server" CommandName="Sort" CommandArgument="FLDNAME">
                                                Emp Name&nbsp;
                                            </telerik:RadLabel>
                                            <img id="FLDNAME" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                            <telerik:RadLabel ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lnkDocument" runat="server" CommandName="Sort" CommandArgument="FLDDOCUMENTNAME">
                                                Document&nbsp;
                                            </telerik:RadLabel>
                                            <img id="FLDDOCUMENTNAME" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDocument" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCUMENTNAME") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblNationalityHeader1" runat="server">Nationality</telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblMedNationality" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOCNATIONALITY") %>' />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lnkExpiryDate" runat="server" CommandName="Sort" CommandArgument="FLDEXPIRYDATE">
                                                Expiry Date&nbsp;
                                            </telerik:RadLabel>
                                            <img id="FLDEXPIRYDATE" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblExpiryDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE") %>' />
                                            <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" />
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" ScrollHeight="" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </div>

                    <hr />
                    <u><b>
                        <telerik:RadLabel ID="lblPassenger" runat="server" Text="Passenger" Visible="false"></telerik:RadLabel>
                    </b></u>
                    <br />
                    <table width="40%" style="display: none;">
                        <tr class="DataGrid-HeaderStyle">
                            <td>
                                <b>
                                    <telerik:RadLabel ID="lblMeal" runat="server" Text="Meal"></telerik:RadLabel>
                                </b>
                            </td>
                            <td align="right">
                                <b>
                                    <telerik:RadLabel ID="lblClientMeal" runat="server" Text="Client Personnel"></telerik:RadLabel>
                                </b>
                            </td>
                            <td align="right">
                                <b>
                                    <telerik:RadLabel ID="lblSupernumeraryMeal" runat="server" Text="Supernumerary"></telerik:RadLabel>
                                </b>
                            </td>
                        </tr>
                        <tr class="datagrid_alternatingstyle">
                            <td>
                                <telerik:RadLabel ID="lblBreakfast" runat="server" Text="Breakfast"></telerik:RadLabel>
                            </td>
                            <td align="right">
                                <eluc:Number ID="ucBreakfast" runat="server"  DecimalPlace="0" />
                            </td>
                            <td align="right">
                                <eluc:Number ID="ucSupBreakFast" runat="server"  DecimalPlace="0" />
                            </td>
                        </tr>
                        <tr class="datagrid_alternatingstyle">
                            <td>
                                <telerik:RadLabel ID="lblTea1" runat="server" Text="Tea"></telerik:RadLabel>
                            </td>
                            <td align="right">
                                <eluc:Number ID="ucTea1" runat="server"  DecimalPlace="0" />
                            </td>
                            <td align="right">
                                <eluc:Number ID="ucSupTea1" runat="server"  DecimalPlace="0" />
                            </td>
                        </tr>
                        <tr class="datagrid_alternatingstyle">
                            <td>
                                <telerik:RadLabel ID="lblLunch" runat="server" Text="Lunch"></telerik:RadLabel>
                            </td>
                            <td align="right">
                                <eluc:Number ID="ucLunch" runat="server"  DecimalPlace="0" />
                            </td>
                            <td align="right">
                                <eluc:Number ID="ucSupLunch" runat="server"  DecimalPlace="0" />
                            </td>
                        </tr>
                        <tr class="datagrid_alternatingstyle">
                            <td>
                                <telerik:RadLabel ID="lblTea2" runat="server" Text="Tea"></telerik:RadLabel>
                            </td>
                            <td align="right">
                                <eluc:Number ID="ucTea2" runat="server"  DecimalPlace="0" />
                            </td>
                            <td align="right">
                                <eluc:Number ID="ucSupTea2" runat="server"  DecimalPlace="0" />
                            </td>
                        </tr>
                        <tr class="datagrid_alternatingstyle">
                            <td>
                                <telerik:RadLabel ID="lblDinner" runat="server" Text="Dinner"></telerik:RadLabel>
                            </td>
                            <td align="right">
                                <eluc:Number ID="ucDinner" runat="server"  DecimalPlace="0" />
                            </td>
                            <td align="right">
                                <eluc:Number ID="ucSupDinner" runat="server"  DecimalPlace="0" />
                            </td>
                        </tr>
                        <tr class="datagrid_alternatingstyle">
                            <td>
                                <telerik:RadLabel ID="lblSupper" runat="server" Text="Supper"></telerik:RadLabel>
                            </td>
                            <td align="right">
                                <eluc:Number ID="ucSupper" runat="server"  DecimalPlace="0" />
                            </td>
                            <td align="right">
                                <eluc:Number ID="ucSupSupper" runat="server"  DecimalPlace="0" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

        </div>
    </form>
</body>
</html>
