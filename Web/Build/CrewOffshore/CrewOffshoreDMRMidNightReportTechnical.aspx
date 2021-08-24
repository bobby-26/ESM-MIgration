<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRMidNightReportTechnical.aspx.cs"
    Inherits="CrewOffshoreDMRMidNightReportTechnical" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>
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
                                <eluc:Decimal ID="ucSwell" runat="server"  Visible="false" DecimalPlace="2" />
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
                                    OnSelectedIndexChanged="ddlvesselstatus_SelectedIndexChanged" EmptyMessage="--Select--" Filter="Contains" MarkFirstMatch="true"
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
                                <eluc:Number ID="ucWindSpeed" runat="server"  DecimalPlace="2" Visible="false" />
                                <eluc:Longitude ID="ucLongitude" runat="server" CssClass="input_mandatory" />
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblETADate" runat="server" Text="ETA"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtETADate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    DatePicker="true" TimeProperty="true" />
                                <%--<asp:TextBox ID="txtETATime" runat="server" CssClass="readonlytextbox" ReadOnly="true"
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
                                <asp:Label ID="lblAdvanceRetard" runat="server" Text="Advance/Retard"></asp:Label>
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
                                <eluc:Date ID="txtETDDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    DatePicker="true" TimeProperty="true" />
                                <%--   <asp:TextBox ID="txtETDTime" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                        Width="50px" />--%>
                                <%--  <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderETDTime" runat="server" AcceptAMPM="false"
                                        ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                        TargetControlID="txtETDTime" UserTimeFormat="TwentyFourHour" />--%>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <th scope="col">
                                            <u><b>
                                                <telerik:RadLabel ID="lblDraft" runat="server" Text="Draft"></telerik:RadLabel>
                                            </b></u>
                                        </th>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblFord" runat="server" Text="For'd"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Numerics ID="ucFord" runat="server"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblMidship" runat="server" Text="Midship"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Numerics ID="ucMidship" runat="server"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblAft" runat="server" Text="Aft"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Numerics ID="ucAft" runat="server"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadLabel ID="lblAverage" runat="server" Text="Average"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <eluc:Numerics ID="ucAverage" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <telerik:RadLabel ID="lblArrivalDate" runat="server" Text="Arrival"></telerik:RadLabel>
                            </td>
                            <td valign="top">
                                <eluc:Date ID="txtArrivalDate" runat="server"  Enabled="true" DatePicker="true" TimeProperty="true" />
                                <%--<asp:TextBox ID="txtArrivalTime" runat="server"  Enabled="true" Width="50px" />--%>
                                <%--   <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderArrivalTime" runat="server"
                                        AcceptAMPM="false" ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99"
                                        MaskType="Time" TargetControlID="txtArrivalTime" UserTimeFormat="TwentyFourHour" />--%>
                            </td>
                            <td valign="top">
                                <telerik:RadLabel ID="lblDepartureDate" runat="server" Text="Departure"></telerik:RadLabel>
                            </td>
                            <td valign="top">
                                <eluc:Date ID="txtDepartureDate" runat="server"  Enabled="true" DatePicker="true" TimeProperty="true" />
                                <%--  <asp:TextBox ID="txtDepartureTime" runat="server"  Enabled="true"
                                        Width="50px" />--%>
                                <%--  <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderDepartureTime" runat="server"
                                        AcceptAMPM="false" ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99"
                                        MaskType="Time" TargetControlID="txtDepartureTime" UserTimeFormat="TwentyFourHour" />--%>
                            </td>
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
                            <td></td>
                            <td></td>
                            <td></td>
                            <td></td>
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
                            <td>
                                <eluc:Numerics ID="txtPOBClient" runat="server"  IsInteger="true" Visible="false" />
                                <eluc:Numerics ID="txtPOBService" runat="server"  IsInteger="true"
                                    Visible="false" />
                                <eluc:Numerics ID="txtTotalOB" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Visible="false" IsInteger="true" />
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtComments" runat="server"  Height="50px" TextMode="MultiLine"
                                    Visible="false" Width="500px" />
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

                    <eluc:TabStrip ID="MenuTabSaveVesselMovements" runat="server" OnTabStripCommand="MenuTabSaveVesselMovements_TabStripCommand"></eluc:TabStrip>

                    <table cellpadding="2" cellspacing="2" width="60%">
                        <tr>
                            <td width="100%">
                                <u><b>
                                    <telerik:RadLabel ID="lblVesselMovements" runat="server" Text="Vessel's Movements"></telerik:RadLabel>
                                </b></u>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top;" width="100%">

                                <eluc:TabStrip ID="MenuVesselMovements" runat="server"></eluc:TabStrip>

                                <telerik:RadGrid ID="gvVesselMovements" RenderMode="Lightweight" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                    CellSpacing="0" GridLines="None"
                                    OnDeleteCommand="gvVesselMovements_DeleteCommand"
                                    OnNeedDataSource="gvVesselMovements_NeedDataSource"
                                    OnUpdateCommand="gvVesselMovements_UpdateCommand"
                                    OnItemDataBound="gvVesselMovements_ItemDataBound"
                                    OnItemCommand="gvVesselMovements_ItemCommand"
                                    OnPreRender="gvVesselMovements_PreRender" ShowFooter="true">
                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" DataKeyNames="FLDVESSELMOVEMENTSID" TableLayout="Fixed" CommandItemDisplay="Top">
                                        <HeaderStyle Width="102px" />
                                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" AddNewRecordText="Vessel Movements" ShowExportToPdfButton="false" />

                                        <Columns>
                                            <telerik:GridTemplateColumn>
                                                <HeaderStyle Width="150px" />
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Left" Width="30%" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblActivityHeader" runat="server" Text="Vessel Status"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblVesselMovementsId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELMOVEMENTSID"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblActivity" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDTASKNAME"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblShortName" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSHORTNAME"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <telerik:RadLabel ID="lblVesselMovementsEditId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELMOVEMENTSID"]%>'></telerik:RadLabel>
                                                    <telerik:RadComboBox ID="ddlActivityEdit" runat="server" CssClass="input_mandatory"
                                                        Width="150px" DataTextField="FLDTASKNAME" DataValueField="FLDOPERATIONALTASKID" Filter="Contains" MarkFirstMatch="true">
                                                        <DefaultItem Text="--Select--" Value="" />
                                                    </telerik:RadComboBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <telerik:RadComboBox ID="ddlActivityAdd" runat="server" CssClass="input_mandatory"
                                                        Width="150px" DataTextField="FLDTASKNAME" DataValueField="FLDOPERATIONALTASKID" Filter="Contains" MarkFirstMatch="true">
                                                        <DefaultItem Text="--Select--" Value="" />
                                                    </telerik:RadComboBox>

                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Left" Width="30%" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblDescriptionHeader" runat="server" Text="Activity"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDESCRIPTION"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtDescriptionEdit" runat="server"  Text='<%# ((DataRowView)Container.DataItem)["FLDDESCRIPTION"]%>'></telerik:RadTextBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtDescriptionAdd" runat="server" ></telerik:RadTextBox>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="20%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" Width="20%" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblTimeHeader" runat="server" Text="Time"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblFromTime" runat="server" Text='<%# string.Format("{0:HH:mm}",((DataRowView)Container.DataItem)["FLDFROMDATETIME"])%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <eluc:Number MaskText="##:##" ID="txtFromTime" runat="server"  Width="50px" Text='<%# string.Format("{0:HH:mm}",((DataRowView)Container.DataItem)["FLDFROMDATETIME"])%>' />
                                                    <%--  <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderFromTime" runat="server" AcceptAMPM="false"
                                                        ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                                        TargetControlID="txtFromTime" UserTimeFormat="TwentyFourHour" />--%>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <eluc:Number MaskText="##:##" ID="txtFromTimeAdd" runat="server"  Width="50px" />
                                                    <%--   <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderFromTimeAdd" runat="server"
                                                        AcceptAMPM="false" ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99"
                                                        MaskType="Time" TargetControlID="txtFromTimeAdd" UserTimeFormat="TwentyFourHour" />--%>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="30%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblTimeDurationHeader" runat="server" Text="Time duration(hh.mm)"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblTimeDuration" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDTIMEDURATION"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <telerik:RadLabel ID="lblTimeDurationEdit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDTIMEDURATION"]%>'
                                                        Visible="false">
                                                    </telerik:RadLabel>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <telerik:RadLabel ID="lblTimeDurationAdd" runat="server"></telerik:RadLabel>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblActionHeader" runat="server" Text="Action"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20%"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit"
                                                        CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                                        ToolTip="Edit">
                                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                                    </asp:LinkButton>
                                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                        width="3" />
                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete"
                                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                                        ToolTip="Delete">
                                                        <span class="icon"><i class="fa fa-trash"></i></span>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                                        CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                                        ToolTip="Save">
                                                        <span class="icon"><i class="fas fa-save"></i></span>
                                                    </asp:LinkButton>
                                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                        width="3" />
                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel"
                                                        CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                                        ToolTip="Cancel">
                                                        <span class="icon"><i class="fa fa-trash"></i></span>
                                                    </asp:LinkButton>
                                                </EditItemTemplate>
                                                <FooterStyle HorizontalAlign="Center" Width="20%" />
                                                <FooterTemplate>
                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                                        CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                                        ToolTip="Add New">
                                                        <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                    </asp:LinkButton>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>
                                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                            PageSizeLabelText="Records per page:" />
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
                                    <telerik:RadLabel ID="lblVesselMovementsRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                                </b></u>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtVesselMovementsRemarks" runat="server"  Height="50px"
                                    TextMode="MultiLine" Width="500px" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <div class="divFloat" style="clear: right">
                        <eluc:TabStrip ID="MenuTabSaveMeteorologyData" runat="server" OnTabStripCommand="MenuTabSaveMeteorologyData_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td width="40%">
                                <u><b>
                                    <telerik:RadLabel ID="lblMeteorologyData" runat="server" Text="Meteorology Data"></telerik:RadLabel>
                                </b></u>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top;">
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvMeteorologyData" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                    CellSpacing="0" GridLines="None"
                                    OnItemDataBound="gvMeteorologyData_ItemDataBound"
                                    OnNeedDataSource="gvMeteorologyData_NeedDataSource">
                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" DataKeyNames="FLDMETEOROLOGYDATAID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                                        <HeaderStyle Width="102px" />
                                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                        <Columns>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Parameter
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblMeteorologyDId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDMETEOROLOGYDATAID"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblMeteorologyName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDMETEOROLOGYNAME"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    0600
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Numerics Width="90px" ID="txtValueDecimal6Edit" runat="server" CssClass="input txtNumber"
                                                        IsPositive="true" MaxLength="9" />
                                                    <eluc:Direction ID="ucDirection6Edit" runat="server" AppendDataBoundItems="true"
                                                         DirectionList='<%# PhoenixRegistersDirection.ListDirection() %>'
                                                        SelectedDirection='<%# ((DataRowView)Container.DataItem)["FLDMETEOROLOGYVALUE"]%>' />
                                                    <eluc:SeaCondition ID="ucSeaCondtion6Edit" runat="server" AppendDataBoundItems="true"
                                                         SeaConditionList='<%# PhoenixRegistersSeaCondition.ListSeaCondition() %>'
                                                        SelectedSeaCondition='<%# ((DataRowView)Container.DataItem)["FLDMETEOROLOGYVALUE"]%>' />
                                                    <telerik:RadLabel ID="lblValueType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVALUETYPE") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblMeteorologyDataId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDMETEOROLOGYDATAID"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblMeteorologyId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDMETEOROLOGYID"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblShortname" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSHORTNAME"]%>'></telerik:RadLabel>
                                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtMeteorologyValueEdit" runat="server" Visible="false" 
                                                        Text='<%# ((DataRowView)Container.DataItem)["FLDMETEOROLOGYVALUE"]%>'>
                                                    </telerik:RadTextBox>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    1200
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Numerics Width="90px" ID="txtValueDecimal12Edit" runat="server" CssClass="input txtNumber"
                                                        IsPositive="true" MaxLength="9" />
                                                    <eluc:Direction ID="ucDirection12Edit" runat="server" AppendDataBoundItems="true"
                                                         DirectionList='<%# PhoenixRegistersDirection.ListDirection() %>'
                                                        SelectedDirection='<%# ((DataRowView)Container.DataItem)["FLDMETEOROLOGY1200VALUE"]%>' />
                                                    <eluc:SeaCondition ID="ucSeaCondtion12Edit" runat="server" AppendDataBoundItems="true"
                                                         SeaConditionList='<%# PhoenixRegistersSeaCondition.ListSeaCondition() %>'
                                                        SelectedSeaCondition='<%# ((DataRowView)Container.DataItem)["FLDMETEOROLOGY1200VALUE"]%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    1800
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Numerics Width="90px" ID="txtValueDecimal18Edit" runat="server" CssClass="input txtNumber"
                                                        IsPositive="true" MaxLength="9" />
                                                    <eluc:Direction ID="ucDirection18Edit" runat="server" AppendDataBoundItems="true"
                                                         DirectionList='<%# PhoenixRegistersDirection.ListDirection() %>'
                                                        SelectedDirection='<%# ((DataRowView)Container.DataItem)["FLDMETEOROLOGY1800VALUE"]%>' />
                                                    <eluc:SeaCondition ID="ucSeaCondtion18Edit" runat="server" AppendDataBoundItems="true"
                                                         SeaConditionList='<%# PhoenixRegistersSeaCondition.ListSeaCondition() %>'
                                                        SelectedSeaCondition='<%# ((DataRowView)Container.DataItem)["FLDMETEOROLOGY1800VALUE"]%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    2400
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Numerics Width="90px" ID="txtValueDecimal24Edit" runat="server" CssClass="input txtNumber"
                                                        IsPositive="true" MaxLength="9" />
                                                    <eluc:Direction ID="ucDirection24Edit" runat="server" AppendDataBoundItems="true"
                                                         DirectionList='<%# PhoenixRegistersDirection.ListDirection() %>'
                                                        SelectedDirection='<%# ((DataRowView)Container.DataItem)["FLDMETEOROLOGY2400VALUE"]%>' />
                                                    <eluc:SeaCondition ID="ucSeaCondtion24Edit" runat="server" AppendDataBoundItems="true"
                                                         SeaConditionList='<%# PhoenixRegistersSeaCondition.ListSeaCondition() %>'
                                                        SelectedSeaCondition='<%# ((DataRowView)Container.DataItem)["FLDMETEOROLOGY2400VALUE"]%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    Forecast for Next 24hrs
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Numerics Width="90px" ID="txtValueDecimalNext24HrsEdit" runat="server" CssClass="input txtNumber"
                                                        IsPositive="true" MaxLength="9" />
                                                    <eluc:Direction ID="ucDirectionNext24HrsEdit" runat="server" AppendDataBoundItems="true"
                                                         DirectionList='<%# PhoenixRegistersDirection.ListDirection() %>'
                                                        SelectedDirection='<%# ((DataRowView)Container.DataItem)["FLDMETEOROLOGYNEXT24HRS"]%>' />
                                                    <eluc:SeaCondition ID="ucSeaCondtionNext24HrsEdit" runat="server" AppendDataBoundItems="true"
                                                         SeaConditionList='<%# PhoenixRegistersSeaCondition.ListSeaCondition() %>'
                                                        SelectedSeaCondition='<%# ((DataRowView)Container.DataItem)["FLDMETEOROLOGYNEXT24HRS"]%>' />
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
                    <table>
                        <tr>
                            <td>
                                <u><b>
                                    <telerik:RadLabel ID="lblMeterologyRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                                </b></u>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtMeterologyRemarks" runat="server"  Height="50px"
                                    TextMode="MultiLine" Width="500px" />
                            </td>
                        </tr>
                    </table>
                    <table width="60%">
                        <tr>
                            <td style="vertical-align: top;">
                                <u><b>
                                    <telerik:RadLabel ID="Literal4" runat="server" Text="Machinery/Equipment Failures"></telerik:RadLabel>
                                </b></u>
                                <br />
                                <telerik:RadLabel ID="lblMachineryNote" runat="server" Text="Has there been any Machinery / Equipment failure on board the vessel since the last report. "></telerik:RadLabel>
                                <telerik:RadRadioButtonList ID="rblMachineryFailure" runat="server" RepeatDirection="Horizontal"
                                    AutoPostBack="true" OnSelectedIndexChanged="rblMachineryFailure_OnSelectedIndexChanged">
                                    <Items>
                                        <telerik:ButtonListItem Value="1" Text="Yes"></telerik:ButtonListItem>
                                        <telerik:ButtonListItem Value="0" Text="No" Selected="True"></telerik:ButtonListItem>
                                    </Items>
                                </telerik:RadRadioButtonList>
                                <div class="navSelect" style="position: relative; width: 15px">
                                    <eluc:TabStrip ID="MenuMachineryFailure" runat="server" Visible="false"></eluc:TabStrip>
                                </div>
                                <br />
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkOrder" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                    CellSpacing="0" GridLines="None"
                                    OnItemDataBound="gvWorkOrder_ItemDataBound"
                                    OnNeedDataSource="gvWorkOrder_NeedDataSource">
                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                                        <HeaderStyle Width="102px" />
                                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                        <Columns>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Left" Width="30%" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblWorkOrderNoHeader" runat="server" Text="Work Order No"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblWorkOrderId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERID"]%>'></telerik:RadLabel>
                                                    <asp:LinkButton ID="lblWorkOrderNo" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"]%>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Left" Width="30%" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblComponentNoHeader" runat="server" Text="Component No"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblComponentNo" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Left" Width="30%" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblComponentHeader" runat="server" Text="Component Name"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblComponent" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Left" Width="20%" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblTitleHeader" runat="server" Text="Title"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblTitle" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>'></telerik:RadLabel>
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
                                    <telerik:RadLabel ID="Literal1" runat="server" Text="PMS Overdue"></telerik:RadLabel>
                                </b></u>
                                <br />
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvPMSoverdue" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                    CellSpacing="0" GridLines="None"
                                    OnItemDataBound="gvPMSoverdue_ItemDataBound1"
                                    OnNeedDataSource="gvPMSoverdue_NeedDataSource">
                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" DataKeyNames="FLDDMRPMSOVERDUEID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                                        <HeaderStyle Width="102px" />
                                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                        <Columns>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70%"></ItemStyle>
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblDMROverDueId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDDMRPMSOVERDUEID"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDUETYPE"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblType" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDTYPE"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="30%"></ItemStyle>
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkCount" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCOUNT"]%>'></asp:LinkButton>
                                                    <telerik:RadLabel ID="lblCount" Visible="false" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCOUNT"]%>'></telerik:RadLabel>
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
                                    <telerik:RadLabel ID="Literal5" runat="server" Text="External Inspections"></telerik:RadLabel>
                                </b></u>
                                <br />
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvExternalInspection" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                    CellSpacing="0" GridLines="None"
                                    OnUpdateCommand="gvExternalInspection_UpdateCommand"
                                    OnNeedDataSource="gvExternalInspection_NeedDataSource"
                                    OnItemCommand="gvExternalInspection_ItemCommand">
                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" DataKeyNames="FLDEXTERNALINSPECTIONID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px" ShowFooter="true">
                                        <HeaderStyle Width="102px" />
                                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                        <Columns>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Left" Width="30%" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblTypeOfInspectionHeader" runat="server" Text="Type of Inspection / Audit"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblExternalInspectionId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEXTERNALINSPECTIONID"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblInspection" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDINSPECTIONTYPE"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <telerik:RadLabel ID="lblExternalInspectionIdEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEXTERNALINSPECTIONID"]%>'></telerik:RadLabel>
                                                    <%--<telerik:RadLabel ID="lblTypeOfInspection" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDINSPECTIONTYPEID"]%>'></telerik:RadLabel>--%>
                                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtTypeOfInspectionEdit" runat="server" CssClass="input_mandatory"
                                                        Text='<%# ((DataRowView)Container.DataItem)["FLDINSPECTIONTYPE"]%>'>
                                                    </telerik:RadTextBox>

                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtTypeOfInspectionAdd" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>

                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Left" Width="30%" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblInspectingCompanyHeader" runat="server" Text="Inspecting Authority / Company"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblInspectingCompany" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDINSPECTINGCOMPANY"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtInspectingCompanyEdit" runat="server" CssClass="input_mandatory"
                                                        Text='<%# ((DataRowView)Container.DataItem)["FLDINSPECTINGCOMPANY"]%>'>
                                                    </telerik:RadTextBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtInspectingCompanyAdd" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Left" Width="20%" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblInspectorNameHeader" runat="server" Text="Name of Inspector / Auditor"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblInspectorName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDINSPECTORNAME"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtInspectorNameEdit" runat="server" CssClass="input_mandatory"
                                                        Text='<%# ((DataRowView)Container.DataItem)["FLDINSPECTORNAME"]%>'>
                                                    </telerik:RadTextBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtInspectorNameAdd" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="30%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblNumberOfNCHeader" runat="server" Text="Number of NCs / Observations"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblNumberOfNC" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNUMBEROFNC"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <eluc:Numerics ID="txtNumberOfNCEdit" runat="server" CssClass="input_mandatory" Text='<%# ((DataRowView)Container.DataItem)["FLDNUMBEROFNC"]%>' />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <eluc:Numerics ID="txtNumberOfNCAdd" runat="server" CssClass="input_mandatory" />
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblActionHeader" runat="server" Text="Action"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20%"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit"
                                                        CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                                        ToolTip="Edit">
                                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                                    </asp:LinkButton>
                                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                        width="3" />
                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete"
                                                        CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                                        ToolTip="Delete">
                                                    <span class="icon"><i class="fa fa-trash"></i></span>
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                                        CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                                        ToolTip="Save">
                                                    <span class="icon"><i class="fas fa-save"></i></span>
                                                    </asp:LinkButton>
                                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                        width="3" />
                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel"
                                                        CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                                        ToolTip="Cancel">
                                                    <span class="icon"><i class="fa fa-trash"></i></span>
                                                    </asp:LinkButton>
                                                </EditItemTemplate>
                                                <FooterStyle HorizontalAlign="Center" Width="20%" />
                                                <FooterTemplate>
                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                                        CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                                        ToolTip="Add New">
                                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                    </asp:LinkButton>
                                                </FooterTemplate>
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
                    <hr />
                    <div class="divFloat" style="clear: right">
                        <eluc:TabStrip ID="MenuTabSaveFO" runat="server" OnTabStripCommand="MenuTabSaveFO_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td width="50%" valign="top">
                                <table width="100%">
                                    <tr>
                                        <td colspan="5">
                                            <u><b>
                                                <telerik:RadLabel ID="lblFOFlowmeterReading" runat="server" Text="FO Flowmeter Reading"></telerik:RadLabel>
                                            </b></u>
                                        </td>
                                    </tr>
                                    <tr class="DataGrid-HeaderStyle">
                                        <td colspan="2"></td>
                                        <td align="right" colspan="2">
                                            <b>
                                                <telerik:RadLabel ID="lblinitialhrs" runat="server" Text="0000 Hrs"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td align="right">
                                            <b>
                                                <telerik:RadLabel ID="lbllasthrs" runat="server" Text="2400 Hrs"></telerik:RadLabel>
                                            </b>
                                        </td>
                                    </tr>
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblme1" runat="server" Text="ME Port"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="chkMEPortNoFM" runat="server" AutoPostBack="true" OnCheckedChanged="chkMEPortNoFM_OnCheckedChanged" />
                                            <telerik:RadLabel ID="lblMEPortNoFM" runat="server" Text="No FM"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="chkMEPortFlowDetective" runat="server" AutoPostBack="true" OnCheckedChanged="chkMEPortFlowDetective_OnCheckedChanged" />
                                            <telerik:RadLabel ID="lblMEPortFlowDetective" runat="server" Text="Defective FM"></telerik:RadLabel>
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="txtme1initialhrs" Width="60px" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="txtme1lasthrs" Width="60px" runat="server"  />
                                        </td>
                                    </tr>
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblme1return" runat="server" Text="ME Port Return"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="chkMEPortreturnNoFM" runat="server" AutoPostBack="true" OnCheckedChanged="chkMEPortreturnNoFM_OnCheckedChanged" />
                                            <telerik:RadLabel ID="lblMEPortreturnNoFM" runat="server" Text="No FM"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="chkMEPortreturnFlowDetective" runat="server" AutoPostBack="true"
                                                OnCheckedChanged="chkMEPortreturnFlowDetective_OnCheckedChanged" />
                                            <telerik:RadLabel ID="lblMEPortreturnFlowDetective" runat="server" Text="Defective FM"></telerik:RadLabel>
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="lblme1returninitialhrs" Width="60px" runat="server" CssClass="readonlytextbox"
                                                ReadOnly="true" IsInteger="true" />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="lblme1returnlasthrs" Width="60px" runat="server"  IsInteger="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td align="right" colspan="2">
                                            <telerik:RadLabel ID="lblme1Total" runat="server" Text="Total"></telerik:RadLabel>
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="txtme1Total" Width="60px" runat="server"  DecimalPlace="0" />
                                        </td>
                                    </tr>
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblme2" runat="server" Text="ME Stbd"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="chkMEStbdNoFM" runat="server" AutoPostBack="true" OnCheckedChanged="chkMEStbdNoFM_OnCheckedChanged" />
                                            <telerik:RadLabel ID="lblMEStbdNoFM" runat="server" Text="No FM"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="chkMEStbdFlowDetective" runat="server" AutoPostBack="true" OnCheckedChanged="chkMEStbdFlowDetective_OnCheckedChanged" />
                                            <telerik:RadLabel ID="lblMEStbdFlowDetective" runat="server" Text="Defective FM"></telerik:RadLabel>
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="txtme2initialhrs" Width="60px" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                                IsInteger="true" />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="txtme2lasthrs" Width="60px" runat="server"  IsInteger="true" />
                                        </td>
                                    </tr>
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblme2return" runat="server" Text="ME Stbd Return"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="chkMEStbdreturnNoFM" runat="server" AutoPostBack="true" OnCheckedChanged="chkMEStbdreturnNoFM_OnCheckedChanged" />
                                            <telerik:RadLabel ID="lblMEStbdreturnNoFM" runat="server" Text="No FM"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="chkMEStbdreturnFlowDetective" runat="server" AutoPostBack="true"
                                                OnCheckedChanged="chkMEStbdreturnFlowDetective_OnCheckedChanged" />
                                            <telerik:RadLabel ID="lblMEStbdreturnFlowDetective" runat="server" Text="Defective FM"></telerik:RadLabel>
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="lblme2returninitialhrs" Width="60px" runat="server" CssClass="readonlytextbox"
                                                ReadOnly="true" />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="lblme2returnlasthrs" Width="60px" runat="server"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td align="right" colspan="2">
                                            <telerik:RadLabel ID="lblme2Total" runat="server" Text="Total"></telerik:RadLabel>
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="txtme2Total" Width="60px" runat="server"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <telerik:RadLabel ID="lblgrandtotal" runat="server" Text="ME Total (ltrs)"></telerik:RadLabel>
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="txtgrandtotal" Width="60px" runat="server" CssClass="readonlytextbox"
                                                ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <telerik:RadLabel ID="lblnotes" runat="server" Text="If there is no return flow meter, record '0'"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                            <b>
                                                <telerik:RadLabel ID="lblAEConsumption" runat="server" Text="AE Consumption"></telerik:RadLabel>
                                            </b>
                                        </td>
                                    </tr>
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblAE1Port" runat="server" Text="AE 1 Port"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="chkAE1NoFM" runat="server" AutoPostBack="true" OnCheckedChanged="chkAE1NoFM_OnCheckedChanged" />
                                            <telerik:RadLabel ID="lblAE1NoFM" runat="server" Text="No FM"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="chkAE1FlowDetective" runat="server" AutoPostBack="true" OnCheckedChanged="chkAE1FlowDetective_OnCheckedChanged" />
                                            <telerik:RadLabel ID="lblAE1FlowDetective" runat="server" Text="Defective FM"></telerik:RadLabel>
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="txtAE1initialhrs" runat="server" Width="60px" CssClass="readonlytextbox" ReadOnly="true"
                                                IsInteger="true" />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="txtAE1lasthrs" Width="60px" runat="server"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2"></td>
                                        <td align="right" colspan="2">
                                            <telerik:RadLabel ID="lblAE1Consumption" runat="server" Text="AE 1 Cons. (ltrs)"></telerik:RadLabel>
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="txtAE1Consumption" Width="60px" runat="server"  />
                                        </td>
                                    </tr>
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblAE2Port" runat="server" Text="AE 2 Port"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="chkAE2NoFM" runat="server" AutoPostBack="true" OnCheckedChanged="chkAE2NoFM_OnCheckedChanged" />
                                            <telerik:RadLabel ID="lblAE2NoFM" runat="server" Text="No FM"></telerik:RadLabel>
                                        </td>
                                        <td>
                                            <telerik:RadCheckBox ID="chkAE2FlowDetective" runat="server" AutoPostBack="true" OnCheckedChanged="chkAE2FlowDetective_OnCheckedChanged" />
                                            <telerik:RadLabel ID="lblAE2FlowDetective" runat="server" Text="Defective FM"></telerik:RadLabel>
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="txtAE2initialhrs" Width="60px" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                                IsInteger="true" />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="txtAE2lasthrs" Width="60px" runat="server"  IsInteger="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" colspan="4">
                                            <telerik:RadLabel ID="lblAE2Consumption" runat="server" Text="AE 2 Cons. (ltrs)"></telerik:RadLabel>
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics ID="txtAE2Consumption" Width="60px" runat="server"  IsInteger="true" />
                                        </td>
                                    </tr>
                                    <tr class="datagrid_alternatingstyle">
                                        <td colspan="2">
                                            <b>
                                                <telerik:RadLabel ID="lblOtherConsumption" runat="server" Text="Miscellaneous Consumptions"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td align="right" colspan="2"></td>
                                        <td align="right">
                                            <eluc:Numerics ID="ucotherConsumption" Width="60px" runat="server"  IsInteger="true" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <b>
                                                <telerik:RadLabel ID="lblTotalConsumption" runat="server" Text="Total Consumption"></telerik:RadLabel>
                                            </b>
                                        </td>
                                        <td align="right" colspan="2"></td>
                                        <td align="right">
                                            <eluc:Numerics ID="ucTotalConsumption" Width="60px" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                                IsInteger="true" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <u><b>
                                                <telerik:RadLabel ID="lblFlowmeterRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                                            </b></u>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtFlowmeterRemarks" runat="server"  Height="50px"
                                                TextMode="MultiLine" Width="500px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td width="50%" valign="top">
                                <br />
                                <table width="100%">
                                    <tr class="DataGrid-HeaderStyle">
                                        <th colspan="3" align="center" scope="col">
                                            <telerik:RadLabel ID="lblPropulsion" runat="server" Text="Propulsion & Aux Machinery Run Time"></telerik:RadLabel>
                                        </th>
                                    </tr>
                                    <tr class="DataGrid-HeaderStyle">
                                        <td width="40%">
                                            <telerik:RadLabel ID="lblCounter" runat="server" Text="Equipment"></telerik:RadLabel>
                                        </td>
                                        <td width="25%">
                                            <telerik:RadLabel ID="lblTotalHrs" runat="server" Text="Total Run Hours today"></telerik:RadLabel>
                                        </td>
                                        <td width="35%">
                                            <telerik:RadLabel ID="lblLastrunHrs" runat="server" Text="Phoenix Run Hours at 2400 Hrs"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblMEPort" runat="server" Text="ME Port"></telerik:RadLabel>
                                            <eluc:Numerics Width="60px" ID="ucMEPortFirstRunHrs" runat="server" Visible="false" CssClass="readonlytextbox"
                                                ReadOnly="true" />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics Width="60px" ID="ucMEPortTotalRunHrs" runat="server"  />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics Width="60px" ID="ucMEPort" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblMEStbd" runat="server" Text="ME Stbd"></telerik:RadLabel>
                                            <eluc:Numerics Width="60px" ID="ucMEStbdFirstRunHrs" runat="server" Visible="false" CssClass="readonlytextbox"
                                                ReadOnly="true" />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics Width="60px" ID="ucMEStbdTotalRunHrs" runat="server"  />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics Width="60px" ID="ucMEStbd" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblAEI" runat="server" Text="AE I"></telerik:RadLabel>
                                            <eluc:Numerics Width="60px" ID="ucAEIFirstRunHrs" runat="server" Visible="false" CssClass="readonlytextbox"
                                                ReadOnly="true" />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics Width="60px" ID="ucAEITotalRunHrs" runat="server"  />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics Width="60px" ID="ucAEI" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblAEII" runat="server" Text="AE II"></telerik:RadLabel>
                                            <eluc:Numerics Width="60px" ID="ucAEIIFirstRunHrs" runat="server" Visible="false" CssClass="readonlytextbox"
                                                ReadOnly="true" />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics Width="60px" ID="ucAEIITotalRunHrs" runat="server"  />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics Width="60px" ID="ucAEII" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblBT1" runat="server" Text="BT1"></telerik:RadLabel>
                                            <eluc:Numerics Width="60px" ID="ucBT1FirstRunHrs" runat="server" Visible="false" CssClass="readonlytextbox"
                                                ReadOnly="true" />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics Width="60px" ID="ucBT1TotalRunHrs" runat="server"  />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics Width="60px" ID="ucBT1" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblBT2" runat="server" Text="BT2"></telerik:RadLabel>
                                            <eluc:Numerics Width="60px" ID="ucBT2FirstRunHrs" runat="server" Visible="false" CssClass="readonlytextbox"
                                                ReadOnly="true" />
                                        </td>
                                        <td align="right">
                                            <eluc:Number ID="ucBT2TotalRunHrs" runat="server"  />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics Width="60px" ID="ucBT2" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblST1" runat="server" Text="ST1"></telerik:RadLabel>
                                            <eluc:Numerics Width="60px" ID="ucST1FirstRunHrs" runat="server" Visible="false" CssClass="readonlytextbox"
                                                ReadOnly="true" />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics Width="60px" ID="ucST1TotalRunHrs" runat="server"  />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics Width="60px" ID="ucST1" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                                        </td>
                                    </tr>
                                    <tr class="datagrid_alternatingstyle">
                                        <td>
                                            <telerik:RadLabel ID="lblST2" runat="server" Text="ST2"></telerik:RadLabel>
                                            <eluc:Numerics Width="60px" ID="ucST2FirstRunHrs" runat="server" Visible="false" CssClass="readonlytextbox"
                                                ReadOnly="true" />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics Width="60px" ID="ucST2TotalRunHrs" runat="server"  />
                                        </td>
                                        <td align="right">
                                            <eluc:Numerics Width="60px" ID="ucST2" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                                <u><b>
                                    <telerik:RadLabel ID="lbl" runat="server" Text="Operational Summary"></telerik:RadLabel>
                                </b></u>
                                <%-- <asp:GridView ID="gvOperationalTimeSummary" runat="server" AutoGenerateColumns="False"
                                    Font-Size="11px" Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true"
                                    ShowFooter="True" EnableViewState="false" OnRowDataBound="gvOperationalTimeSummary_ItemDataBound"
                                    OnRowCommand="gvOperationalTimeSummary_RowCommand" OnRowDeleting="gvOperationalTimeSummary_RowDeleting"
                                    OnRowEditing="gvOperationalTimeSummary_RowEditing" OnRowCancelingEdit="gvOperationalTimeSummary_RowCancelingEdit"
                                    OnRowUpdating="gvOperationalTimeSummary_RowUpdating">
                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" Wrap="true" HorizontalAlign="Center" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>--%>
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvOperationalTimeSummary" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                    CellSpacing="0" GridLines="None"
                                    OnItemDataBound="gvOperationalTimeSummary_ItemDataBound"
                                    OnItemCommand="gvOperationalTimeSummary_ItemCommand"
                                    OnNeedDataSource="gvOperationalTimeSummary_NeedDataSource">
                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" DataKeyNames="FLDOPERATIONALTASKID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px" ShowFooter="true">
                                        <HeaderStyle Width="102px" />
                                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                        <FooterStyle BackColor="#cc6633"></FooterStyle>
                                        <Columns>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="50%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblActivityHeader" runat="server" Text="Status"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblOperationalTaskId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDOPERATIONALTASKID"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblOperationalTimeSummaryEditId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDOPERATIONALSUMMERYID"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblOperationalTimeSummaryNameEdit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDTASKNAME"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblShortName" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSHORTNAME"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblDistanceApplicable" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDDISTANCEAPPLICABLE"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblTimeDurationHeader" runat="server" Text="Time duration(hh.mm)"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblTimeEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDTIME"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblTimeDurationEdit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDTIMEDURATION"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <telerik:RadLabel ID="lblTotalTime" runat="server" Width="100px" Font-Bold="true"></telerik:RadLabel>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblFuelConsumptionHeader" runat="server" Text="Fuel Oil Cons(ltr)"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Decimal Width="60PX" ID="ucFuelConsumptionEdit" runat="server"  DecimalPlace="2"
                                                        Text='<%# ((DataRowView)Container.DataItem)["FLDFUELOILCONSUMPTION"]%>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <telerik:RadLabel ID="lblTotalFOC" runat="server" Width="100px" Font-Bold="true"></telerik:RadLabel>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="20%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblFuelConsHrHeader" runat="server" Text="Fuel Oil Cons/hr"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Decimal Width="60px" ID="ucFuelConsHrEdit" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                                        DecimalPlace="2" />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <telerik:RadLabel ID="lblTotalFOChr" runat="server" Width="100px" Font-Bold="true"></telerik:RadLabel>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="20%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblDistanceHeader" runat="server" Text="Distance(nm)"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Decimal Width="60px" ID="ucSeaStreamDistanceEdit" runat="server"  DecimalPlace="2"
                                                        Text='<%# ((DataRowView)Container.DataItem)["FLDDISTANCE"]%>' />
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <telerik:RadLabel ID="lblTotalDistance" runat="server" Width="100px" Font-Bold="true"></telerik:RadLabel>
                                                </FooterTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblSpeedHeader" runat="server" Text="Speed"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Decimal Width="60px" ID="ucSpeedEdit" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                                        DecimalPlace="2" />
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
                                <telerik:RadLabel ID="lblTotalDPTime" runat="server" Text="Total DP Time / Fuel Consumption"></telerik:RadLabel>
                                <eluc:Numerics Width="60px" ID="txtDPTime" runat="server"  ReadOnly="true" />
                                <eluc:Numerics Width="60px" ID="txtDPFuelConsumption" runat="server"  ReadOnly="true" />
                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkFuelConsShowGraph" runat="server" Text="Show Report"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <div class="divFloat" style="clear: right">
                        <eluc:TabStrip ID="MenuTabSaveBulks" runat="server" OnTabStripCommand="MenuTabSaveBulks_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <table cellpadding="2" cellspacing="2" width="100%">
                        <tr>
                            <td>
                                <u><b>
                                    <telerik:RadLabel ID="lblBulks" runat="server" Text="Bulks"></telerik:RadLabel>
                                </b></u>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top;">
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvBulks" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                    CellSpacing="0" GridLines="None"
                                    OnNeedDataSource="gvBulks_NeedDataSource">
                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" DataKeyNames="FLDOILTYPECODE" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                                        <HeaderStyle Width="102px" />
                                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                        <Columns>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblProductsHeader" runat="server" Text="Products"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblProductName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDOILTYPENAME"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblOilTypeCode" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDOILTYPECODE"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblOilLoadedConsumptionId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDOILLOADEDANDCONSUMPTION"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblUnit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDUNITNAME"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblActiveYN" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDACTIVEYN"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblOilShortname" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDSHORTNAME"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblConversionfactorM3" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDCONVERSIONFACTORM3"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="5%"></ItemStyle>
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblUnitHeader" runat="server" Text="Unit"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblUnitName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDUNITNAME"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblOpeningStocksHeader" runat="server" Text="Previous Stock"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblOpeningStocks" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDPREVIOUSROB"]%>'></telerik:RadLabel>
                                                    <eluc:Numerics Width="60px" ID="txtOpeningStock" runat="server"  Visible="false"
                                                        IsInteger="true" Text='<%# ((DataRowView)Container.DataItem)["FLDPREVIOUSROB"]%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblChartererHeader" runat="server" Text="Loaded"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Numerics Width="60px" ID="ucChartererLoadedEdit" runat="server"  IsInteger="true"
                                                        Text='<%# ((DataRowView)Container.DataItem)["FLDLOADEDCHARTERER"]%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblChartererDischargedHeader" runat="server" Text="Discharged"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Numerics Width="60px" ID="ucChartererDischargedEdit" runat="server"  IsInteger="true"
                                                        Text='<%# ((DataRowView)Container.DataItem)["FLDDISCHARGEDCHARTERER"]%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblConsumedHeader" runat="server" Text="Consumed"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <eluc:Numerics Width="60px" ID="ucConsumedEdit" runat="server"  IsInteger="true"
                                                        Text='<%# ((DataRowView)Container.DataItem)["FLDCONSUMED"]%>' />
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                                <HeaderStyle HorizontalAlign="Center" />
                                                <HeaderTemplate>
                                                    <telerik:RadLabel ID="lblRemainedOnBoardHeader" runat="server" Text="Remaining OnBoard"></telerik:RadLabel>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblRemainedOnBoard" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCURRENTROB"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                        </Columns>

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
                    <table>
                        <tr>
                            <td>
                                <u><b>
                                    <telerik:RadLabel ID="lblbulkRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                                </b></u>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtbulkRemarks" runat="server"  Height="50px" TextMode="MultiLine"
                                    Width="500px" />
                            </td>
                        </tr>
                    </table>
                    <hr />
                    <table width="60%">
                        <tr>
                            <td style="vertical-align: top;">
                                <u><b>
                                    <telerik:RadLabel ID="lblRequisionsandPO" runat="server" Text="Requisition and PO's"></telerik:RadLabel>
                                </b></u>
                                <br />
                                <telerik:RadGrid RenderMode="Lightweight" ID="gvRequisionsandPO" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                    CellSpacing="0" GridLines="None"
                                    OnItemDataBound="gvRequisionsandPO_ItemDataBound1"
                                    OnNeedDataSource="gvRequisionsandPO_NeedDataSource">
                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                        AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                                        <HeaderStyle Width="102px" />
                                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                        <Columns>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70%"></ItemStyle>
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <telerik:RadLabel ID="lblName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNAME"]%>'></telerik:RadLabel>
                                                    <telerik:RadLabel ID="lblType" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDTYPE"]%>'></telerik:RadLabel>
                                                </ItemTemplate>
                                            </telerik:GridTemplateColumn>
                                            <telerik:GridTemplateColumn>
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="30%"></ItemStyle>
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lblCount" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCOUNT"]%>'></asp:LinkButton>
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
                    <table cellpadding="2" cellspacing="2" width="100%" runat="server" visible="false">
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
