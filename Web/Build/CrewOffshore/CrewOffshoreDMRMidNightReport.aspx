<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreDMRMidNightReport.aspx.cs"
    MaintainScrollPositionOnPostback="true" Inherits="CrewOffshoreDMRMidNightReport" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>


<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskedTextbox.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlOffshoreVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Direction" Src="~/UserControls/UserControlDircection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaCondition" Src="~/UserControls/UserControlSeaCondtion.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Numerics" Src="~/UserControls/UserControlMaskNumber.ascx" %>

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
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <%--<telerik:RadSkinManager ID="RadSkinManager1" runat="server" />--%>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <div>
                <%-- new --%>

                <%--                <div class="subHeader" style="position: relative">
                    <div class="subHeader" style="position: relative">--%>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
                <eluc:Status runat="server" ID="ucStatus" />
                <%--</div>--%>

                <eluc:TabStrip ID="MenuReportTap" TabStrip="true" runat="server" OnTabStripCommand="ReportTapp_TabStripCommand"></eluc:TabStrip>



                <eluc:TabStrip ID="MenuNewSaveTabStrip" runat="server" OnTabStripCommand="NewSaveTap_TabStripCommand"></eluc:TabStrip>


                <%--</div>--%>
                <%--<div style="top: 40px; position: relative;">--%>
                <%--<table width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCrewChangeAlert" runat="server" Text="Boat and Fire Drill is to be carried out within 24hrs of Departure"
                                    Font-Size="Large" ForeColor="Red" Visible="false" BorderColor="Red"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>--%>
                <br clear="all" />
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td colspan="6">

                            <telerik:RadLabel ID="lblAlertSenttoOFC" runat="server" Text="Report Sent to Office" Visible="false"
                                Font-Bold="False" Font-Size="Large" Font-Underline="True" ForeColor="Red" BorderColor="Red">
                            </telerik:RadLabel>
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td width="10%">
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td width="10%">
                            <eluc:Vessel ID="ucVessel" runat="server" CssClass="input_mandatory" VesselsOnly="true"
                                AppendDataBoundItems="true" AutoPostBack="true" Width="150px" OnTextChangedEvent="ucVessel_OnTextChangedEvent" />
                        </td>
                        <td width="10%">
                            <telerik:RadLabel ID="lblMaster" runat="server" Text="Master"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtMaster" runat="server" RenderMode="Lightweight" Width="200PX"
                                ReadOnly="true">
                            </telerik:RadTextBox>

                        </td>
                        <td width="10%">
                            <telerik:RadLabel ID="lblPOB" runat="server" Text="POB"></telerik:RadLabel>
                        </td>
                        <td width="20%">
                            <eluc:Numerics ID="txtPOB" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate" runat="server" CssClass="input_mandatory" DatePicker="true"
                                AutoPostBack="true" OnTextChangedEvent="txtDate_TextChanged" />
                            <%--ImageUrl="<%$ PhoenixTheme:images/search.png %>"--%>
                            <asp:ImageButton ID="cmdDateBaseSearch" runat="server" ImageUrl="<%$ PhoenixTheme:images/search.png %>"
                                ImageAlign="AbsMiddle" Text=".." OnClick="cmdSearch_Click" Visible="false" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblClient" Visible="false" runat="server" Text="Client"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAvgSpeed" runat="server" Text="Avg Spd"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtClient" runat="server" RenderMode="Lightweight" Visible="false" CssClass="readonlytextbox"
                                ReadOnly="true">
                            </telerik:RadTextBox>
                            <eluc:Decimal ID="ucAvgSpeed" runat="server" ReadOnly="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCrew" runat="server" Text="Crew"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Numerics ID="txtCrew" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSwell" runat="server" Visible="false" Text="Swell"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblVoyageNo" runat="server" Text="Charter"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="ucSwell" runat="server" Visible="false" DecimalPlace="2" />
                            <span id="spnPickListVoyage">
                                <telerik:RadTextBox ID="txtVoyageName" RenderMode="Lightweight" runat="server" Width="200px" CssClass="readonlytextbox"
                                    ReadOnly="true">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtVoyageId" RenderMode="Lightweight" Visible="false" runat="server" Width="0px"></telerik:RadTextBox>
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
                            <eluc:Number ID="txtWaveHeight" runat="server" DecimalPlace="2"
                                Visible="false" />
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtSeaCondition" runat="server" Visible="false"></telerik:RadTextBox>
                            <eluc:Latitude ID="ucLatitude" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:MultiPort ID="ucPort" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                                Width="375px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblETD" runat="server" Text="Location"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtLocation" RenderMode="Lightweight" runat="server"></telerik:RadTextBox>
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
                            <eluc:Number ID="ucWindSpeed" runat="server" DecimalPlace="2" Visible="false" />
                            <eluc:Longitude ID="ucLongitude" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblETADate" runat="server" Text="ETA"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtETADate" runat="server" CssClass="readonlytextbox" />
                            <telerik:RadTimePicker ID="txtETATime" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="75px" />

                            <%-- <asp:TextBox ID="txtETATime" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="50px" />--%>
                            <%--  <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderETATime" runat="server" AcceptAMPM="false"
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
                            <telerik:RadComboBox ID="ddlAdvanceRetard" runat="server" AutoPostBack="true"
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
                            <eluc:Date ID="txtETDDate" runat="server" CssClass="readonlytextbox" />
                            <telerik:RadTimePicker ID="txtETDTime" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="75px" />
                            <%--    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderETDTime" runat="server" AcceptAMPM="false"
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
                                            <telerik:RadLabel ID="lblDraft" runat="server" Text="Draft" Font-Underline="true"></telerik:RadLabel>
                                        </b></u>
                                    </th>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblFord" runat="server" Text="For'd"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Decimal ID="ucFord" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblMidship" runat="server" Text="Midship"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Decimal ID="ucMidship" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblAft" runat="server" Text="Aft"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Decimal ID="ucAft" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadLabel ID="lblAverage" runat="server" Text="Average"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <eluc:Decimal ID="ucAverage" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td valign="top">
                            <telerik:RadLabel ID="lblArrivalDate" runat="server" Text="Arrival"></telerik:RadLabel>
                        </td>
                        <td valign="top">
                            <eluc:Date ID="txtArrivalDate" runat="server" Enabled="true" />
                            <telerik:RadTimePicker RenderMode="Lightweight" ID="txtArrivalTime" runat="server"
                                Enabled="true" Width="75px" />
                            <%-- <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderArrivalTime" runat="server"
                                    AcceptAMPM="false" ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99"
                                    MaskType="Time" TargetControlID="txtArrivalTime" UserTimeFormat="TwentyFourHour" />--%>
                        </td>
                        <td valign="top">
                            <telerik:RadLabel ID="lblDepartureDate" runat="server" Text="Departure"></telerik:RadLabel>
                        </td>
                        <td valign="top">
                            <eluc:Date ID="txtDepartureDate" runat="server" Enabled="true" />
                            <telerik:RadTimePicker ID="txtDepartureTime" runat="server" Enabled="true"
                                Width="75px" />
                            <%--    <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderDepartureTime" runat="server"
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
                        <td colspan="2">
                            <font color="red">
                                <telerik:RadLabel ID="lblFireDrillYN" Visible="false" runat="server" Text="Boat and Fire Drill done"></telerik:RadLabel>
                            </font>
                        </td>
                        <td>
                            <telerik:RadCheckBox ID="chkFireDrillYN" Visible="false" runat="server" />
                        </td>
                        <td></td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <u><b>
                                <telerik:RadLabel ID="lblGenRemarks" runat="server" Font-Underline="true" Text="Issues/Complaints By Client"></telerik:RadLabel>
                            </b></u>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtGeneralRemarks" runat="server" Height="50px"
                                TextMode="MultiLine" Width="500px" />
                        </td>
                    </tr>
                </table>
                <hr />
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td width="100%">
                            <u><b>
                                <telerik:RadLabel ID="lblPlannedActivity" runat="server" Text="Look Ahead Planned Activity"
                                    BackColor="#88bbee" Font-Underline="true">
                                </telerik:RadLabel>
                            </b></u>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;" width="50px">
                            <%-- <asp:GridView ID="gvPlannedActivity" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" EnableViewState="false">
                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>--%>
                            <telerik:RadGrid ID="gvPlannedActivity" RenderMode="Lightweight" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
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
                                        <telerik:GridTemplateColumn HeaderStyle-Width="10%">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
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
                                        <telerik:GridTemplateColumn HeaderStyle-Width="90%">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblActivityHeader" runat="server" Text="Activity"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblActivity" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDACTIVITY"]%>'></telerik:RadLabel>
                                                <telerik:RadTextBox ID="txtActivityEdit" RenderMode="Lightweight" runat="server" Width="1000px"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDACTIVITY"]%>'>
                                                </telerik:RadTextBox>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Charter matching your search criteria"
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
                                <telerik:RadLabel ID="lblLookAheadRemarks" runat="server" Font-Underline="true" Text="Remarks"></telerik:RadLabel>
                            </b></u>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtLookAheadRemarks" runat="server" Height="50px"
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
                                <telerik:RadLabel ID="lblVesselMovements" runat="server" Font-Underline="true" Text="Vessel's Movements"></telerik:RadLabel>
                            </b></u>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;" width="100%">

                            <eluc:TabStrip ID="MenuVesselMovements" runat="server"></eluc:TabStrip>

                            <%--  <asp:GridView ID="gvVesselMovements" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="true"
                                    EnableViewState="false" OnRowDataBound="gvVesselMovements_ItemDataBound" OnRowCancelingEdit="gvVesselMovements_RowCancelingEdit"
                                    OnRowDeleting="gvVesselMovements_RowDeleting" OnRowEditing="gvVesselMovements_RowEditing"
                                    OnRowUpdating="gvVesselMovements_RowUpdating" OnRowCommand="gvVesselMovements_RowCommand">
                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>--%>

                            <telerik:RadGrid ID="gvVesselMovements" RenderMode="Lightweight" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false"
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
                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtDescriptionEdit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDESCRIPTION"]%>'></telerik:RadTextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtDescriptionAdd" runat="server"></telerik:RadTextBox>
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
                                                <eluc:Number MaskText="##:##" ID="txtFromTime" runat="server" Width="50px" Text='<%# string.Format("{0:HH:mm}",((DataRowView)Container.DataItem)["FLDFROMDATETIME"])%>' />
                                                <%--  <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderFromTime" runat="server" AcceptAMPM="false"
                                                        ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                                        TargetControlID="txtFromTime" UserTimeFormat="TwentyFourHour" />--%>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <eluc:Number MaskText="##:##" ID="txtFromTimeAdd" runat="server" Width="50px" />
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
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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
                                <telerik:RadLabel ID="lblVesselMovementsRemarks" runat="server" Font-Underline="true" Text="Remarks"></telerik:RadLabel>
                            </b></u>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtVesselMovementsRemarks" runat="server" Height="50px"
                                TextMode="MultiLine" Width="500px" />
                        </td>
                    </tr>
                </table>
                <hr />
                <telerik:RadDockZone ID="RadDockZone2" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                    <telerik:RadDock Width="60%" RenderMode="Lightweight" ID="RadDock1" runat="server" Title="Anchor Handling" EnableDrag="false" EnableAnimation="true"
                        EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Collapsed="true" Closed="false">
                        <Commands>
                            <telerik:DockExpandCollapseCommand />
                        </Commands>
                        <ContentTemplate>
                            <table cellpadding="2" cellspacing="2" width="60%">
                                <%--   <tr>
                        <td><u><b>
                            <telerik:RadLabel ID="lblanchorhanlding" runat="server" Text="Anchor Handling"></telerik:RadLabel>
                        </b></u>
                        </td>
                    </tr>--%>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <%-- <asp:GridView ID="gvAnchor" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                                    Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                                                    EnableViewState="false" OnRowDataBound="gvAnchor_ItemDataBound">
                                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                                    <Columns>--%>
                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvAnchor" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                            CellSpacing="0" GridLines="None"
                                            OnItemDataBound="gvAnchor_ItemDataBound"
                                            OnNeedDataSource="gvAnchor_NeedDataSource">
                                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                AutoGenerateColumns="false" DataKeyNames="FLDANCHOROPERATIONDETAILID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
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
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderTemplate>
                                                            <telerik:RadLabel ID="lblItemHdr" runat="server" Text="Item"></telerik:RadLabel>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblItem" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDOPERATIONNAME"]%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblOperationId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDANCHOROPERATIONDETAILID"]%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblOperationName" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDOPERATIONITEM"]%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblValueType" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDVALUETYPE"]%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblValue" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDOPERATIONVALUE"]%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblValue2" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDOPERATIONVALUE2"]%>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderTemplate>
                                                            <telerik:RadLabel ID="lblDetailsHdr" runat="server" Text="Details"></telerik:RadLabel>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtValueDecimal" runat="server" Width="50px" />
                                                            <%--  <ajaxToolkit:MaskedEditExtender ID="MaskedtxtValueDecimal" runat="server" AutoComplete="true"
                                                                    InputDirection="RightToLeft" Mask="999.99" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                                                    TargetControlID="txtValueDecimal" />--%>
                                                            <telerik:RadCheckBoxList ID="chkValue" runat="server" DataBindings-DataTextField="FLDANCHORHANDLINGTYPENAME"
                                                                DataBindings-DataValueField="FLDANCHORHANDLINGTYPEID" Height="40%" RepeatDirection="Vertical"
                                                                Width="100%">
                                                            </telerik:RadCheckBoxList>
                                                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtValueTextSize" runat="server" Width="45px"></telerik:RadTextBox>
                                                            <%-- <ajaxToolkit:MaskedEditExtender ID="MaskedtxtValuesize" runat="server" AutoComplete="true"
                                                                    InputDirection="RightToLeft" Mask="999" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                                                    TargetControlID="txtValueTextSize" />--%>
                                                            <telerik:RadLabel ID="lblSize" runat="server" Visible="false" Text="mm * "></telerik:RadLabel>
                                                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtValueTextLen" runat="server" Width="45px"></telerik:RadTextBox>
                                                            <%--  <ajaxToolkit:MaskedEditExtender ID="MaskedtxtValueLength" runat="server" AutoComplete="true"
                                                                    InputDirection="RightToLeft" Mask="999.99" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                                                    TargetControlID="txtValueTextLen" />--%>
                                                            <telerik:RadLabel ID="lblLength" runat="server" Visible="false" Text="m "></telerik:RadLabel>
                                                            <telerik:RadRadioButtonList ID="rblValue" runat="server">
                                                                <Items>
                                                                    <telerik:ButtonListItem Text="Yes" Value="1"></telerik:ButtonListItem>
                                                                    <telerik:ButtonListItem Text="No" Value="0"></telerik:ButtonListItem>
                                                                </Items>
                                                            </telerik:RadRadioButtonList>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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
                                            <telerik:RadLabel ID="lblAnchorRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                                        </b></u>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtAnchorRemarks" runat="server" Height="50px"
                                            TextMode="MultiLine" Width="500px" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </telerik:RadDock>
                </telerik:RadDockZone>
                <telerik:RadDockZone ID="RadDockZone1" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                    <telerik:RadDock Width="60%" RenderMode="Lightweight" ID="RadDock2" runat="server" Title="Summary of ROV Operations" EnableDrag="false" EnableAnimation="true"
                        EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Collapsed="true" Closed="false">
                        <Commands>
                            <telerik:DockExpandCollapseCommand />
                        </Commands>
                        <ContentTemplate>
                            <table cellpadding="2" cellspacing="2" width="60%">
                                <%-- <tr>
                                    <td><u><b>
                                        <telerik:RadLabel ID="lblsummary" runat="server" Text="Summary of ROV Operations"></telerik:RadLabel>
                                    </b></u>
                                    </td>
                                </tr>--%>
                                <tr>
                                    <td style="vertical-align: top;">
                                        <%-- <asp:GridView ID="gvROVoperation" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                                    Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                                                    EnableViewState="false" OnRowDataBound="gvROVoperation_ItemDataBound">
                                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                                    <Columns>--%>
                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvROVoperation" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                            CellSpacing="0" GridLines="None"
                                            OnItemDataBound="gvROVoperation_ItemDataBound"
                                            OnNeedDataSource="gvROVoperation_NeedDataSource">
                                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                AutoGenerateColumns="false" DataKeyNames="FLDROVOPERATIONDETAILID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
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
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderTemplate>
                                                            <telerik:RadLabel ID="lblItemHdr" runat="server" Text="Item"></telerik:RadLabel>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblItem" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDROVOPERATIONNAME"]%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblROVOperationId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDROVOPERATIONDETAILID"]%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblROVOperationName" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDROVOPERATIONITEM"]%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblValueType" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDVALUETYPE"]%>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblValue" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDROVOPERATIONVALUE"]%>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                        <HeaderTemplate>
                                                            <telerik:RadLabel ID="lblDetailsHdr" runat="server" Text="Details"></telerik:RadLabel>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtValueDecimal" runat="server" Width="50px" />
                                                            <%--   <ajaxToolkit:MaskedEditExtender ID="MaskedtxtValueDecimal" runat="server" AutoComplete="true"
                                                                    InputDirection="RightToLeft" Mask="999.99" MaskType="Number" OnInvalidCssClass="MaskedEditError"
                                                                    TargetControlID="txtValueDecimal" />--%>
                                                            <telerik:RadCheckBoxList ID="chkValue" runat="server" DataBindings-DataTextField="FLDDMRROVTYPENAME"
                                                                DataBindings-DataValueField="FLDDMRROVTYPEID" Height="40%" RepeatDirection="Vertical" Width="100%">
                                                            </telerik:RadCheckBoxList>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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
                                            <telerik:RadLabel ID="lblROVRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                                        </b></u>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtROVRemarks" runat="server" Height="50px" TextMode="MultiLine"
                                            Width="500px" />
                                    </td>
                                </tr>
                            </table>

                        </ContentTemplate>
                    </telerik:RadDock>
                </telerik:RadDockZone>
                <hr />

                <eluc:TabStrip ID="MenuTabSaveHSEIndicators" runat="server" OnTabStripCommand="MenuTabSaveHSEIndicators_TabStripCommand"></eluc:TabStrip>

                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <u><b>
                                <telerik:RadLabel ID="lblHSEIndicators" runat="server" Font-Underline="true" Text="HSE Indicators"></telerik:RadLabel>
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
                            <%--<asp:CheckBox ID="chkHSEIndicators" runat="server" AutoPostBack="true" OnCheckedChanged="chkHSEIndicators_OnCheckedChanged" />--%>
                        </td>
                    </tr>
                </table>
                <table width="60%">
                    <tr>
                        <td style="vertical-align: top;">
                            <u><b>
                                <telerik:RadLabel ID="Literal3" runat="server" Font-Underline="true" Text="Unsafe Acts/Conditions Reported in 24 hrs"></telerik:RadLabel>
                            </b></u>
                            <br />

                            <eluc:TabStrip ID="MenuUnsafeActsAdd" runat="server" OnTabStripCommand="MenuUnsafeActsAdd_TabStripCommand"></eluc:TabStrip>

                            <%--<asp:GridView ID="gvUnsafe" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                                    EnableViewState="false" OnRowDataBound="gvUnsafe_ItemDataBound" OnRowCommand="gvUnsafe_RowCommand"
                                    DataKeyNames="FLDDIRECTINCIDENTID">
                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>--%>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvUnsafe" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
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
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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

                <eluc:TabStrip ID="MenuHSEIndicators" runat="server" Visible="false" OnTabStripCommand="MenuHSEIndicators_TabStripCommand"></eluc:TabStrip>

                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <%--  <asp:GridView ID="gvHSEIndicators" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    BackColor="#EEEEEE" Width="97%" CellPadding="2" AllowSorting="true" ShowHeader="true"
                                    ShowFooter="false" EnableViewState="false" OnRowCreated="gvHSEIndicators_RowCreated"
                                    OnRowDataBound="gvHSEIndicators_ItemDataBound">
                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                   
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>--%>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvHSEIndicators" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                CellSpacing="0" GridLines="None"
                                OnItemDataBound="gvHSEIndicators_ItemDataBound1"
                                OnNeedDataSource="gvHSEIndicators_NeedDataSource">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                                    <HeaderStyle Width="102px" />
                                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                                    <ColumnGroups>
                                        <telerik:GridColumnGroup HeaderText="HSE Indicators" Name="HSE" HeaderStyle-HorizontalAlign="Center">
                                        </telerik:GridColumnGroup>
                                        <telerik:GridColumnGroup HeaderText="Lagging Indicators" Name="Lagging" HeaderStyle-HorizontalAlign="Center">
                                        </telerik:GridColumnGroup>
                                          <telerik:GridColumnGroup HeaderText="Leading Indicators" Name="Leading" HeaderStyle-HorizontalAlign="Center">
                                        </telerik:GridColumnGroup>
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
                                    <Columns>
                                        <telerik:GridTemplateColumn ColumnGroupName ="HSE">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                            <HeaderTemplate>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblhpiitem" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNAME"]%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn ColumnGroupName="Lagging" HeaderStyle-Width="70px" HeaderTooltip="Exposure Hour" UniqueName="lblEHHeader">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
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
                                        <telerik:GridTemplateColumn ColumnGroupName="Lagging" HeaderStyle-Width="50px" HeaderTooltip="No of High Potential Incident">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
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
                                        <telerik:GridTemplateColumn ColumnGroupName="Lagging" HeaderStyle-Width="50px" HeaderTooltip="Env Release">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
                                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblEnvironmentalIncidentHeader" runat="server" Text="Env Release"></telerik:RadLabel>
                                                <eluc:Tooltip ID="ucEnvironmentalIncidentHeader" runat="server" Text="Environmental Release" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkEnvRelease" runat="server" Width="50px" Text='<%# ((DataRowView)Container.DataItem)["FLDENVIRONMENTALINCIDENT"]%>'
                                                    Visible="false"></asp:LinkButton>
                                                <telerik:RadLabel ID="lblEnvironmentalIncident" runat="server"  Text='<%# ((DataRowView)Container.DataItem)["FLDENVIRONMENTALINCIDENT"]%>'></telerik:RadLabel>
                                                <%--<eluc:Number ID="txtEnvironmentalIncident" runat="server" Width="50px" IsInteger="true"
                                                    CssClass="readonlytextbox" Enabled="false" Text='<%# ((DataRowView)Container.DataItem)["FLDENVIRONMENTALINCIDENT"]%>' />--%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn ColumnGroupName="Lagging" HeaderStyle-Width="50px" HeaderTooltip="No of Lost time Injury">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
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
                                        <telerik:GridTemplateColumn ColumnGroupName="Lagging" HeaderStyle-Width="50px" HeaderTooltip="No of Restricted Work Cases">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
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
                                        <telerik:GridTemplateColumn ColumnGroupName="Lagging" HeaderStyle-Width="50px" HeaderTooltip="No of Medical Treatment Case">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
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
                                        <telerik:GridTemplateColumn ColumnGroupName="Lagging" HeaderStyle-Width="50px" HeaderTooltip="No of First Aid Case">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
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
                                        <telerik:GridTemplateColumn ColumnGroupName="Leading" HeaderStyle-Width="50px" HeaderTooltip="Near Misses">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
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
                                        <telerik:GridTemplateColumn ColumnGroupName="Leading" HeaderStyle-Width="50px" HeaderTooltip="Unsafe Acts / Unsafe Conditions">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
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
                                        <telerik:GridTemplateColumn ColumnGroupName="Leading" HeaderStyle-Width="50px" HeaderTooltip="No. of STOP Cards">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
                                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblstopcardsHeader" runat="server" Text="STOP  Cards"></telerik:RadLabel>
                                                <eluc:Tooltip ID="ucstopcardsHeader" runat="server" Text="No. of STOP Cards" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <eluc:Numerics ID="ucstopcards" runat="server" Type="Number" Width="100%" CssClass="input_mandatory"
                                                    Enabled="true" IsInteger="true" Text='<%# ((DataRowView)Container.DataItem)["FLDNOOFSTOPCARDS"]%>'
                                                    Visible="false" />
                                                <telerik:RadLabel ID="lblstopcards" runat="server" Width="50px" Text='<%# ((DataRowView)Container.DataItem)["FLDNOOFSTOPCARDS"]%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn ColumnGroupName="Leading" HeaderStyle-Width="50px" HeaderTooltip="No. of Risk Assessment">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
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
                                        <telerik:GridTemplateColumn ColumnGroupName="Leading" HeaderStyle-Width="50px" HeaderTooltip="No of Exercises and Drills">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
                                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblExercisesandDrillsHeader" runat="server" Text="ED"></telerik:RadLabel>
                                                <eluc:Tooltip ID="ucExercisesandDrillsHeader" runat="server" Text="No of Exercises and Drills" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <eluc:Numerics ID="ucExercisesandDrills" runat="server"  CssClass="input_mandatory"
                                                    Enabled="true" IsInteger="true" Width="100%" Text='<%# ((DataRowView)Container.DataItem)["FLDEXERCISESANDDRILLS"]%>'
                                                    Visible="false" />
                                                <telerik:RadLabel ID="lblExercisesandDrills" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDEXERCISESANDDRILLS"]%>'></telerik:RadLabel>
                                                <%--<eluc:Number ID="txtExercisesandDrills" runat="server" Width="50px" CssClass="readonlytextbox" Enabled="false" 
                                                    IsInteger="true" Text='<%# ((DataRowView)Container.DataItem)["FLDEXERCISESANDDRILLS"]%>' />--%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn ColumnGroupName="Leading" HeaderStyle-Width="50px" HeaderTooltip="No of HSE Meeting Held">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
                                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblnoofsafetyHeader" runat="server" Text="HSE Meeting Held"></telerik:RadLabel>
                                                <eluc:Tooltip ID="ucnoofsafetyHeader" runat="server" Text="No of HSE Meeting Held" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <eluc:Numerics ID="ucnoofsafety" runat="server" CssClass="input_mandatory"
                                                    Enabled="true" IsInteger="true" Width="100%"  Text='<%# ((DataRowView)Container.DataItem)["FLDNOOFSAFETY"]%>'
                                                    Visible="false" />
                                                <telerik:RadLabel ID="lblnoofsafety" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNOOFSAFETY"]%>'></telerik:RadLabel>
                                                <%--<eluc:Number ID="txtnoofsafety" runat="server" Width="50px" CssClass="readonlytextbox" Enabled="false" IsInteger="true"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDNOOFSAFETY"]%>' />--%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn ColumnGroupName="Leading" HeaderStyle-Width="50px" HeaderTooltip="No of Permit To Work Issued">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
                                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblPTWIssuedHeader" runat="server" Text="PTW Issued"></telerik:RadLabel>
                                                <eluc:Tooltip ID="uclblPTWIssuedHeader" runat="server" Text="No of Permit To Work Issued" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <eluc:Numerics ID="ucPTWIssued" runat="server" Width="100%" CssClass="input_mandatory"
                                                    Enabled="true" IsInteger="true" Text='<%# ((DataRowView)Container.DataItem)["FLDPTWISSUED"]%>'
                                                    Visible="false" />
                                                <telerik:RadLabel ID="lblPTWIssued" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDPTWISSUED"]%>'></telerik:RadLabel>
                                                <%--<eluc:Number ID="txtPTWIssued" runat="server" Width="50px" CssClass="readonlytextbox" Enabled="false" IsInteger="true"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDPTWISSUED"]%>' />--%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                        PageSizeLabelText="Records per page:" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" ScrollHeight="" />
                                 <%--   <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />--%>
                                    <ClientMessages DragToGroupOrReorder="" DragToResize="" DropHereToReorder="" />   
                                </ClientSettings>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                </table>
                <table cellpadding="2" cellspacing="2" width="60%">
                    <tr>
                        <td>
                            <%--<asp:GridView ID="gvLaggingIndicators" runat="server" AutoGenerateColumns="False"
                                    Font-Size="11px" Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true"
                                    ShowFooter="true" EnableViewState="false" OnRowDataBound="gvLaggingIndicators_ItemDataBound"
                                    OnRowCommand="gvLaggingIndicators_RowCommand" DataKeyNames="FLDINSPECTIONINCIDENTID">
                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>--%>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvLaggingIndicators" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
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
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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
                                <telerik:RadLabel ID="lblComments" runat="server" Font-Underline="true" Text="Remarks"></telerik:RadLabel>
                            </b></u>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtComments" runat="server" Height="50px" TextMode="MultiLine"
                                Width="500px" />
                        </td>
                    </tr>
                </table>
                <hr />

                <eluc:TabStrip ID="MenuTabSaveMeteorologyData" runat="server" OnTabStripCommand="MenuTabSaveMeteorologyData_TabStripCommand"></eluc:TabStrip>

                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td width="40%">
                            <u><b>
                                <telerik:RadLabel ID="lblMeteorologyData" runat="server" Font-Underline="true" Text="Meteorology Data"></telerik:RadLabel>
                            </b></u>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <%--<asp:GridView ID="gvMeteorologyData" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                                    EnableViewState="false" OnRowDataBound="gvMeteorologyData_ItemDataBound" OnRowCommand="gvMeteorologyData_RowCommand"
                                    OnRowDeleting="gvMeteorologyData_RowDeleting" OnRowEditing="gvMeteorologyData_RowEditing"
                                    OnRowCancelingEdit="gvMeteorologyData_RowCancelingEdit" OnRowUpdating="gvMeteorologyData_RowUpdating">
                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>--%>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvMeteorologyData" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                CellSpacing="0" GridLines="None"
                                OnItemDataBound="gvMeteorologyData_ItemDataBound"
                                OnNeedDataSource="gvMeteorologyData_NeedDataSource">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" DataKeyNames="FLDMETEOROLOGYDATAID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
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
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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
                                <telerik:RadLabel ID="lblMeterologyRemarks" Font-Underline="true" runat="server" Text="Remarks"></telerik:RadLabel>
                            </b></u>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtMeterologyRemarks" runat="server" Height="50px"
                                TextMode="MultiLine" Width="500px" />
                        </td>
                    </tr>
                </table>
                <hr />

                <eluc:TabStrip ID="MenuTabSaveFO" runat="server" OnTabStripCommand="MenuTabSaveFO_TabStripCommand"></eluc:TabStrip>

                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td width="50%" valign="top">
                            <table width="100%">
                                <tr>
                                    <td colspan="5">
                                        <u><b>
                                            <telerik:RadLabel ID="lblFOFlowmeterReading" runat="server" Font-Underline="true" Text="FO Flowmeter Reading"></telerik:RadLabel>
                                        </b></u>
                                    </td>
                                </tr>
                                <tr class="rgHeader">
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
                                        <eluc:Numerics ID="txtme1lasthrs" Width="60px" runat="server" />
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
                                        <eluc:Numerics ID="lblme1returnlasthrs" Width="60px" runat="server" IsInteger="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"></td>
                                    <td align="right" colspan="2">
                                        <telerik:RadLabel ID="lblme1Total" runat="server" Text="Total"></telerik:RadLabel>
                                    </td>
                                    <td align="right">
                                        <eluc:Numerics ID="txtme1Total" Width="60px" runat="server" DecimalPlace="0" />
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
                                        <eluc:Numerics ID="txtme2lasthrs" Width="60px" runat="server" IsInteger="false" />
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
                                        <eluc:Numerics ID="lblme2returnlasthrs" Width="60px" runat="server" />
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
                                        <eluc:Numerics ID="txtAE1lasthrs" Width="60px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2"></td>
                                    <td align="right" colspan="2">
                                        <telerik:RadLabel ID="lblAE1Consumption" runat="server" Text="AE 1 Cons. (ltrs)"></telerik:RadLabel>
                                    </td>
                                    <td align="right">
                                        <eluc:Numerics ID="txtAE1Consumption" Width="60px" runat="server" />
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
                                            IsInteger="false" />
                                    </td>
                                    <td align="right">
                                        <eluc:Numerics ID="txtAE2lasthrs" Width="60px" runat="server" IsInteger="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right" colspan="4">
                                        <telerik:RadLabel ID="lblAE2Consumption" runat="server" Text="AE 2 Cons. (ltrs)"></telerik:RadLabel>
                                    </td>
                                    <td align="right">
                                        <eluc:Numerics ID="txtAE2Consumption" Width="60px" runat="server" IsInteger="true" />
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
                                        <eluc:Numerics ID="ucotherConsumption" Width="60px" runat="server" IsInteger="true" />
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
                                            <telerik:RadLabel ID="lblFlowmeterRemarks" runat="server" Font-Underline="true" Text="Remarks"></telerik:RadLabel>
                                        </b></u>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtFlowmeterRemarks" runat="server" Height="50px"
                                            TextMode="MultiLine" Width="500px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td width="50%" valign="top">
                            <br />
                            <table width="100%">
                                <tr class="rgHeader">
                                    <th colspan="3" align="center" scope="col">
                                        <telerik:RadLabel ID="lblPropulsion" runat="server" Text="Propulsion & Aux Machinery Run Time"></telerik:RadLabel>
                                    </th>
                                </tr>
                                <tr class="rgHeader">
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
                                        <eluc:Numerics Width="60px" ID="ucMEPortTotalRunHrs" runat="server" />
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
                                        <eluc:Numerics Width="60px" ID="ucMEStbdTotalRunHrs" runat="server" />
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
                                        <eluc:Numerics Width="60px" ID="ucAEITotalRunHrs" runat="server" />
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
                                        <eluc:Numerics Width="60px" ID="ucAEIITotalRunHrs" runat="server" />
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
                                        <eluc:Numerics Width="60px" ID="ucBT1TotalRunHrs" runat="server" />
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
                                        <eluc:Numerics Width="60px" ID="ucBT2TotalRunHrs" runat="server" />
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
                                        <eluc:Numerics Width="60px" ID="ucST1TotalRunHrs" runat="server" />
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
                                        <eluc:Numerics Width="60px" ID="ucST2TotalRunHrs" runat="server" />
                                    </td>
                                    <td align="right">
                                        <eluc:Numerics Width="60px" ID="ucST2" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                            <u><b>
                                <telerik:RadLabel ID="lbl" runat="server" Font-Underline="true" Text="Operational Summary"></telerik:RadLabel>
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
                                            <ItemStyle Wrap="true" HorizontalAlign="Left" ></ItemStyle>
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
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblTimeDurationHeader" runat="server" Text="Time duration(hh.mm)"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblTimeEdit" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDTIME"]%>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblTimeDurationEdit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDTIMEDURATION"]%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadLabel ID="lblTotalTime" runat="server"  Font-Bold="true"></telerik:RadLabel>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
                                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblFuelConsumptionHeader" runat="server" Text="Fuel Oil Cons(ltr)"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <eluc:Decimal  ID="ucFuelConsumptionEdit" runat="server" DecimalPlace="2" Width="100%"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDFUELOILCONSUMPTION"]%>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadLabel ID="lblTotalFOC" runat="server"  Font-Bold="true"></telerik:RadLabel>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblFuelConsHrHeader" runat="server" Text="Fuel Oil Cons/hr"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <eluc:Decimal  ID="ucFuelConsHrEdit"  Width="100%" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                                    DecimalPlace="2" />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadLabel ID="lblTotalFOChr" runat="server"  Font-Bold="true"></telerik:RadLabel>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
                                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblDistanceHeader" runat="server" Text="Distance(nm)"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <eluc:Decimal ID="ucSeaStreamDistanceEdit" runat="server" DecimalPlace="2"  Width="100%"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDDISTANCE"]%>' />
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadLabel ID="lblTotalDistance" runat="server"  Font-Bold="true"></telerik:RadLabel>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" ></ItemStyle>
                                            <FooterStyle Wrap="false" HorizontalAlign="Right" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblSpeedHeader" runat="server" Text="Speed"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <eluc:Decimal  ID="ucSpeedEdit" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                                    DecimalPlace="2"   Width="100%"/>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                        PageSizeLabelText="Records per page:" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" ScrollHeight="" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                            <telerik:RadLabel ID="lblTotalDPTime" runat="server" Text="Total DP Time / Fuel Consumption"></telerik:RadLabel>
                            <eluc:Numerics Width="60px" ID="txtDPTime" runat="server" ReadOnly="true" />
                            <eluc:Numerics Width="60px" ID="txtDPFuelConsumption" runat="server" ReadOnly="true" />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkFuelConsShowGraph" runat="server" Text="Show Report"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <hr />
                <table width="50%">
                    <tr>
                        <td style="vertical-align: top;">
                            <u><b>
                                <telerik:RadLabel ID="Literal4" runat="server" Font-Underline="true" Text="Machinery/Equipment Failures"></telerik:RadLabel>
                            </b></u>
                            <br />
                            <br />
                            <telerik:RadLabel ID="lblMachineryNote" runat="server" Text="Has there been any Machinery / Equipment failure on board the vessel since the last report. "></telerik:RadLabel>
                            <telerik:RadRadioButtonList ID="rblMachineryFailure" runat="server" RepeatDirection="Horizontal"
                                AutoPostBack="true" OnSelectedIndexChanged="rblMachineryFailure_OnSelectedIndexChanged">
                                <Items>
                                    <telerik:ButtonListItem Value="1" Text="Yes"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Value="0" Text="No" Selected="True"></telerik:ButtonListItem>
                                </Items>
                            </telerik:RadRadioButtonList>
                            <%--<asp:CheckBox ID ="chkMachineryFailure" runat="server" AutoPostBack="true" OnCheckedChanged="chkMachineryFailure_OnCheckedChanged" />--%>

                            <eluc:TabStrip ID="MenuMachineryFailure" runat="server" Visible="false"></eluc:TabStrip>

                            <br />
                            <%--<asp:GridView ID="gvWorkOrder" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="true"
                                    EnableViewState="false" OnRowDataBound="gvWorkOrder_ItemDataBound">
                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>--%>


                            <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkOrder" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                CellSpacing="0" GridLines="None"
                                OnItemDataBound="gvWorkOrder_ItemDataBound"
                                OnNeedDataSource="gvWorkOrder_NeedDataSource">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERNUMBER" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
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
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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
                                <telerik:RadLabel ID="Literal1" runat="server" Font-Underline="true" Text="PMS Overdue"></telerik:RadLabel>
                            </b></u>
                            <br />
                            <%--<asp:GridView ID="gvPMSoverdue" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                                    OnRowDataBound="gvPMSoverdue_ItemDataBound" EnableViewState="false">
                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>--%>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvPMSoverdue" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                CellSpacing="0" GridLines="None"
                                OnItemDataBound="gvPMSoverdue_ItemDataBound1"
                                OnNeedDataSource="gvPMSoverdue_NeedDataSource">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" DataKeyNames="FLDDMRPMSOVERDUEID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
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
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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
                                <telerik:RadLabel ID="Literal5" runat="server" Font-Underline="true" Text="External Inspections"></telerik:RadLabel>
                            </b></u>
                            <br />
                            <%-- <asp:GridView ID="gvExternalInspection" runat="server" AutoGenerateColumns="False"
                                    Font-Size="11px" Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true"
                                    ShowFooter="true" EnableViewState="false" OnRowDataBound="gvExternalInspection_ItemDataBound"
                                    OnRowCancelingEdit="gvExternalInspection_RowCancelingEdit" OnRowDeleting="gvExternalInspection_RowDeleting"
                                    OnRowEditing="gvExternalInspection_RowEditing" OnRowUpdating="gvExternalInspection_RowUpdating"
                                    OnRowCommand="gvExternalInspection_RowCommand">
                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>--%>
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
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDINSPECTIONTYPE"]%>' Width="100%">
                                                </telerik:RadTextBox>

                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtTypeOfInspectionAdd" Width="100%" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>

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
                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtInspectingCompanyEdit" Width="100%" runat="server" CssClass="input_mandatory"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDINSPECTINGCOMPANY"]%>'>
                                                </telerik:RadTextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtInspectingCompanyAdd" Width="100%" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
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
                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtInspectorNameEdit" Width="100%" runat="server" CssClass="input_mandatory"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDINSPECTORNAME"]%>'>
                                                </telerik:RadTextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtInspectorNameAdd" Width="100%" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
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
                                                <eluc:Numerics ID="txtNumberOfNCEdit" runat="server" Width="100%" Type="Number" CssClass="input_mandatory" Text='<%# ((DataRowView)Container.DataItem)["FLDNUMBEROFNC"]%>' />
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <eluc:Numerics ID="txtNumberOfNCAdd" Type="Number" Width="100%" runat="server" CssClass="input_mandatory" />
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblActionHeader" runat="server" Text="Action"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20%"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Edit"
                                                    CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                                    ToolTip="Edit">
                                                     <span class="icon"><i class="fas fa-edit"></i></span>
                                                </asp:LinkButton>
                                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                    width="3" />
                                                <asp:LinkButton runat="server" AlternateText="Delete"
                                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                                    ToolTip="Delete">
                                                    <span class="icon"><i class="fa fa-trash"></i></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Save"
                                                    CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                                    ToolTip="Save">
                                                    <span class="icon"><i class="fas fa-save"></i></span>
                                                </asp:LinkButton>
                                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                    width="3" />
                                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                                    CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                                    ToolTip="Cancel">
                                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                                </asp:LinkButton>
                                            </EditItemTemplate>
                                            <FooterStyle HorizontalAlign="Center" Width="20%" />
                                            <FooterTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Save"
                                                    CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                                    ToolTip="Add New">
                                                     <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                </asp:LinkButton>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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
                <table width="100%">
                    <tr>
                        <td>
                            <u><b>
                                <telerik:RadLabel ID="lblDeckCargo" runat="server" Font-Underline="true" Text="Deck Cargo"></telerik:RadLabel>
                            </b></u>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <%--  <asp:GridView ID="gvDeckCargo" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                                    EnableViewState="false" OnRowDataBound="gvDeckCargo_ItemDataBound">
                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>--%>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvDeckCargo" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                CellSpacing="0" GridLines="None"
                                OnNeedDataSource="gvDeckCargo_NeedDataSource">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" DataKeyNames="FLDDECKCARGOITEMID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
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
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="5%"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblSNoHdr" runat="server" Text="No"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblSNo" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDROWNUMBER"]%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="45%"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblItemHeader" runat="server" Text="Item"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblItem" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDECKCARGOITEMNAME"]%>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblItemId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDDECKCARGOITEMID"]%>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblDMRItemId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDDECKCARGODETAILSID"]%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblUnitHeader" runat="server" Text="Unit"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblUnitName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblUnitId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>'></telerik:RadLabel>
                                                <%--<eluc:Unit ID="ucUnitEdit" runat="server" AppendDataBoundItems="true" UnitList='<%#PhoenixRegistersUnit.ListUnit() %>'
                                                    SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>'  />--%>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblPreviousQuantityHdr" runat="server" Text="Previous ROB"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblPreviousQuantity" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDPREVIOUSQUANTITY"]%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn Visible="false">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblPreviousROBHdr" runat="server" Text="Previous ROB"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblPreviousROB" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDPREVIOUSROB"]%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblNoFOUnitLoadedHeader" runat="server" Text="No. of Units Loaded"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <eluc:Numerics Width="60px" ID="txtNoFOUnitLoaded" runat="server" DecimalPlace="2"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDNOOFUNITLOADED"]%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblWtloadedInMTHeader" runat="server" Text="Weight Loaded in MT"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <eluc:Numerics Width="60px" ID="txtWtloadedInMT" runat="server" DecimalPlace="2"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDWEIGHTLOADEDINMT"]%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblNoFOUnitDischargedHeader" runat="server" Text="No. of Units Discharged"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <eluc:Numerics Width="60px" ID="txtNoFOUnitDischarged" runat="server" DecimalPlace="2"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDNOOFUNITDISCHARGED"]%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblWtDischargedInMTHeader" runat="server" Text="Weight Discharged in MT"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <eluc:Numerics Width="60px" ID="txtWtDischargedInMT" runat="server" DecimalPlace="2"
                                                    Text='<%# ((DataRowView)Container.DataItem)["FLDWEIGHTDISCHARGEDINMT"]%>' />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblCurrentQuantityHdr" runat="server" Text="Current ROB"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblCurrentQuantity" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCURRENTQUANTITY"]%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn Visible="false">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="10%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblCurrentROBHdr" runat="server" Text="Current ROB"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblCurrentROB" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCURRENTROB"]%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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
                <table width="50%">
                    <tr>
                        <td>
                            <u><b>
                                <telerik:RadLabel ID="lblDeckCargoSummary" runat="server" Font-Underline="true" Text="Summary of Weights"></telerik:RadLabel>
                            </b></u>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <%-- <asp:GridView ID="gvDeckCargoSummary" runat="server" AutoGenerateColumns="False"
                                    Font-Size="11px" Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true"
                                    ShowFooter="false" EnableViewState="false" OnRowDataBound="gvDeckCargoSummary_ItemDataBound">
                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>--%>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvDeckCargoSummary" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                CellSpacing="0" GridLines="None"
                                OnNeedDataSource="gvDeckCargoSummary_NeedDataSource">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" DataKeyNames="FLDDECKCARGOSUMMARYID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
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
                                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="55%"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblItemHeader" runat="server" Text="Item"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblOperationName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDOPERATIONNAME"]%>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblOperationType" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDOPERATIONTYPE"]%>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblSummaryId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDDECKCARGOSUMMARYID"]%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn>
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="45%"></ItemStyle>
                                            <HeaderStyle HorizontalAlign="Center" />
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblValueHeader" runat="server" Text="MT"></telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblValue" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDOPERATIONVALUE"]%>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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
                                <telerik:RadLabel ID="lblDeckRemarks" Font-Underline="true" runat="server" Text="Remarks"></telerik:RadLabel>
                            </b></u>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtDeckRemarks" runat="server" Height="50px" TextMode="MultiLine"
                                Width="500px" />
                        </td>
                    </tr>
                </table>
                <hr />

                <eluc:TabStrip ID="MenuTabSaveBulks" runat="server" OnTabStripCommand="MenuTabSaveBulks_TabStripCommand"></eluc:TabStrip>

                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <u><b>
                                <telerik:RadLabel ID="lblBulks" runat="server" Font-Underline="true" Text="Bulks"></telerik:RadLabel>
                            </b></u>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top;">
                            <%-- <asp:GridView ID="gvBulks" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                                    EnableViewState="false" OnRowDataBound="gvBulks_ItemDataBound" OnRowCommand="gvBulks_RowCommand"
                                    OnRowDeleting="gvBulks_RowDeleting" OnRowEditing="gvBulks_RowEditing" OnRowCancelingEdit="gvBulks_RowCancelingEdit"
                                    OnRowUpdating="gvBulks_RowUpdating" OnRowCreated="gvBulks_RowCreated">
                                    <FooterStyle CssClass="datagrid_footerstyle" />
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>--%>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvBulks" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                CellSpacing="0" GridLines="None"
                                OnNeedDataSource="gvBulks_NeedDataSource"
                                OnItemDataBound="gvBulks_ItemDataBound1">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" DataKeyNames="FLDOILTYPECODE" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
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
                                                <eluc:Decimal Width="60px" ID="txtOpeningStock" runat="server" Visible="false"
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
                                                <eluc:Decimal Width="60px" ID="ucChartererLoadedEdit" runat="server" IsInteger="true"
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
                                                <eluc:Decimal Width="60px" ID="ucChartererDischargedEdit" runat="server" IsInteger="true"
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
                                                <eluc:Decimal Width="60px" ID="ucConsumedEdit" runat="server" IsInteger="true"
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
                                                <asp:Label ID="lblRemainedOnBoard" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCURRENTROB"]%>'></asp:Label>
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
                                <telerik:RadLabel ID="lblbulkRemarks" runat="server"  Font-Underline="true" Text="Remarks"></telerik:RadLabel>
                            </b></u>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtbulkRemarks" runat="server" Height="50px" TextMode="MultiLine"
                                Width="500px" />
                        </td>
                    </tr>
                </table>
                <hr />

                <eluc:TabStrip ID="MenuTabSavePassenger" runat="server" OnTabStripCommand="MenuTabSavePassenger_TabStripCommand"></eluc:TabStrip>

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
                                <telerik:RadLabel ID="Literal2" runat="server" Font-Underline="true" Text="Certificates Due in 15 & 30 Days"></telerik:RadLabel>
                            </b></u>
                            <br />
                            <%-- <asp:GridView ID="gvCertificates" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                                OnRowDataBound="gvCertificates_RowDataBound" EnableViewState="false">
                                <FooterStyle CssClass="datagrid_footerstyle" />
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <Columns>--%>
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
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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
                                <telerik:RadLabel ID="lblAudit" runat="server" Font-Underline="true" Text="Ship Audits Due in 15 & 30 Days"></telerik:RadLabel>
                            </b></u>
                            <br />
                            <%-- <asp:GridView ID="gvShipAudit" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                                OnRowDataBound="gvShipAudit_RowDataBound" EnableViewState="false">
                                <FooterStyle CssClass="datagrid_footerstyle" />
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <Columns>--%>
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
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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
                                <telerik:RadLabel ID="lblRequisionsandPO" runat="server" Font-Underline="true" Text="Requisition and PO's"></telerik:RadLabel>
                            </b></u>
                            <br />
                            <%-- <asp:GridView ID="gvRequisionsandPO" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                                OnRowDataBound="gvRequisionsandPO_ItemDataBound" EnableViewState="false">
                                <FooterStyle CssClass="datagrid_footerstyle" />
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <Columns>--%>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvRequisionsandPO" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                                CellSpacing="0" GridLines="None"
                                OnItemDataBound="gvRequisionsandPO_ItemDataBound1"
                                OnNeedDataSource="gvRequisionsandPO_NeedDataSource">
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
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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
                                <telerik:RadLabel ID="lblShipTasksDue" runat="server" Font-Underline="true" Text="Ship Tasks Due in 15 & 30 Days"></telerik:RadLabel>
                            </b></u>
                            <br />
                            <%--<asp:GridView ID="gvShipTasks" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                                OnRowDataBound="gvShipTasks_ItemDataBound" EnableViewState="false">
                                <FooterStyle CssClass="datagrid_footerstyle" />
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <Columns>--%>
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
                                                <telerik:RadLabel ID="lblShipTaskHeader" Font-Underline="true" Text="Ship Task" runat="server"></telerik:RadLabel>
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
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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
                        <td width="60%" style="vertical-align: top;">
                            <br />
                            <table width="100%">
                                <tr>
                                    <td width="20%">
                                        <telerik:RadLabel ID="lblDNAConducted" runat="server" Font-Underline="true" Text="DNA Test Conducted" Visible="false"></telerik:RadLabel>
                                    </td>
                                    <td align="center">
                                        <telerik:RadCheckBox ID="chkDNATest" runat="server" Visible="false" />
                                    </td>
                                    <td align="right">
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtDNATime" runat="server" Width="50px" Visible="false" />
                                        <%-- <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtenderFromTime" runat="server" AcceptAMPM="false"
                                                ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                                TargetControlID="txtDNATime" UserTimeFormat="TwentyFourHour" />--%>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtDNARemarks" Width="500px" Height="15px" TextMode="MultiLine"
                                            Visible="false" runat="server">
                                        </telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                            <table width="125%">
                                <tr>
                                    <td style="vertical-align: top;">
                                        <table>
                                            <tr>
                                                <td width="40%">
                                                    <telerik:RadLabel ID="lblSatC" runat="server" Text="Is Sat C Operational and switched on ?"></telerik:RadLabel>
                                                </td>
                                                <td width="20%">
                                                    <telerik:RadRadioButtonList ID="rblSatC" runat="server" Direction="Horizontal" AutoPostBack="true">
                                                        <Items>
                                                            <telerik:ButtonListItem Value="1" Text="Yes"></telerik:ButtonListItem>
                                                            <telerik:ButtonListItem Value="0" Text="No" Selected="True"></telerik:ButtonListItem>
                                                        </Items>

                                                    </telerik:RadRadioButtonList>
                                                </td>
                                                <td width="40%">
                                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtSatCRemarks" runat="server" Width="500px"></telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadLabel ID="lblCCTV" runat="server" Text="Is the CCTV Unit operational, all cameras working and unit is switched on ?"></telerik:RadLabel>
                                                </td>
                                                <td>
                                                    <telerik:RadRadioButtonList ID="rblCCTV" runat="server" Direction="Horizontal" AutoPostBack="true">
                                                        <Items>
                                                            <telerik:ButtonListItem Value="1" Text="Yes"></telerik:ButtonListItem>
                                                            <telerik:ButtonListItem Value="0" Text="No" Selected="True"></telerik:ButtonListItem>
                                                        </Items>
                                                    </telerik:RadRadioButtonList>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtCCTVRemarks" runat="server" Width="500px"></telerik:RadTextBox>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <telerik:RadLabel ID="lblHiPap" runat="server" Text="Is the VDR Operational, switched and no alarms ?"></telerik:RadLabel>
                                                </td>
                                                <td>
                                                    <telerik:RadRadioButtonList ID="rblHiPap" runat="server" Direction="Horizontal" AutoPostBack="true">
                                                        <Items>
                                                            <telerik:ButtonListItem Value="1" Text="Yes"></telerik:ButtonListItem>
                                                            <telerik:ButtonListItem Value="0" Text="No" Selected="True"></telerik:ButtonListItem>
                                                        </Items>
                                                    </telerik:RadRadioButtonList>
                                                </td>
                                                <td>
                                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtHiPapRemarks" runat="server" Width="500px"></telerik:RadTextBox>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        <u><b>
                                            <telerik:RadLabel ID="lblPassenger" runat="server" Font-Underline="true" Text="Passenger"></telerik:RadLabel>
                                        </b></u>
                                        <br />
                                        <%--<asp:GridView ID="gvPassenger" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                            Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                                            EnableViewState="false" OnRowDataBound="gvPassenger_ItemDataBound">
                                            <FooterStyle CssClass="datagrid_footerstyle" />
                                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                            <Columns>--%>
                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvPassenger" Visible="false" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                            CellSpacing="0" GridLines="None"
                                            OnNeedDataSource="gvPassenger_NeedDataSource">
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
                                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="70%"></ItemStyle>
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNAME"]%>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn>
                                                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="30%"></ItemStyle>
                                                        <HeaderTemplate>
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblCount" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCOUNT"]%>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                </Columns>
                                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                                    PageSizeLabelText="Records per page:" />
                                            </MasterTableView>
                                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                                <Scrolling AllowScroll="true" ScrollHeight="" />
                                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                        <table width="40%">
                                            <tr class="rgHeader">
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
                                                    <eluc:Numerics Width="60px" ID="ucBreakfast" runat="server" DecimalPlace="0" />
                                                </td>
                                                <td align="right">
                                                    <eluc:Numerics Width="60px" ID="ucSupBreakFast" runat="server" DecimalPlace="0" />
                                                </td>
                                            </tr>
                                            <tr class="datagrid_alternatingstyle">
                                                <td>
                                                    <telerik:RadLabel ID="lblTea1" runat="server" Text="Tea"></telerik:RadLabel>
                                                </td>
                                                <td align="right">
                                                    <eluc:Numerics Width="60px" ID="ucTea1" runat="server" DecimalPlace="0" />
                                                </td>
                                                <td align="right">
                                                    <eluc:Numerics Width="60px" ID="ucSupTea1" runat="server" DecimalPlace="0" />
                                                </td>
                                            </tr>
                                            <tr class="datagrid_alternatingstyle">
                                                <td>
                                                    <telerik:RadLabel ID="lblLunch" runat="server" Text="Lunch"></telerik:RadLabel>
                                                </td>
                                                <td align="right">
                                                    <eluc:Numerics Width="60px" ID="ucLunch" runat="server" DecimalPlace="0" />
                                                </td>
                                                <td align="right">
                                                    <eluc:Numerics Width="60px" ID="ucSupLunch" runat="server" DecimalPlace="0" />
                                                </td>
                                            </tr>
                                            <tr class="datagrid_alternatingstyle">
                                                <td>
                                                    <telerik:RadLabel ID="lblTea2" runat="server" Text="Tea"></telerik:RadLabel>
                                                </td>
                                                <td align="right">
                                                    <eluc:Numerics Width="60px" ID="ucTea2" runat="server" DecimalPlace="0" />
                                                </td>
                                                <td align="right">
                                                    <eluc:Numerics Width="60px" ID="ucSupTea2" runat="server" DecimalPlace="0" />
                                                </td>
                                            </tr>
                                            <tr class="datagrid_alternatingstyle">
                                                <td>
                                                    <telerik:RadLabel ID="lblDinner" runat="server" Text="Dinner"></telerik:RadLabel>
                                                </td>
                                                <td align="right">
                                                    <eluc:Numerics Width="60px" ID="ucDinner" runat="server" DecimalPlace="0" />
                                                </td>
                                                <td align="right">
                                                    <eluc:Numerics Width="60px" ID="ucSupDinner" runat="server" DecimalPlace="0" />
                                                </td>
                                            </tr>
                                            <tr class="datagrid_alternatingstyle">
                                                <td>
                                                    <telerik:RadLabel ID="lblSupper" runat="server" Text="Supper"></telerik:RadLabel>
                                                </td>
                                                <td align="right">
                                                    <eluc:Numerics Width="60px" ID="ucSupper" runat="server" DecimalPlace="0" />
                                                </td>
                                                <td align="right">
                                                    <eluc:Numerics Width="60px" ID="ucSupSupper" runat="server" DecimalPlace="0" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <br />
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <u><b>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Font-Underline="true" Text="Remarks"></telerik:RadLabel>
                            </b></u>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtRemarks" runat="server" Height="50px" TextMode="MultiLine"
                                Width="500px" />
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
                            <eluc:Numerics Width="60px" ID="txtCrewOff" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                IsInteger="true" />
                        </td>
                        <td width="10%">
                            <telerik:RadLabel ID="lblCrewOn" runat="server" Text="Crew On"></telerik:RadLabel>
                        </td>
                        <td width="20%">
                            <eluc:Numerics Width="60px" ID="txtCrewOn" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                IsInteger="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPOBCrew" runat="server" Text="POB (Crew)"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Numerics Width="60px" ID="txtPOBCrew" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPOBClient" runat="server" Text="POB (Client)"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Numerics Width="60px" ID="txtPOBClient" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                IsInteger="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPOBService" runat="server" Text="POB (Service/Supernumerary)"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Numerics Width="60px" ID="txtPOBService" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                IsInteger="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblTotalOB" runat="server" Text="Total OnBoard"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Numerics Width="60px" ID="txtTotalOB" runat="server" CssClass="readonlytextbox" ReadOnly="true"
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
                </table>
                <table>
                    <tr>
                        <td>
                            <u><b>
                                <telerik:RadLabel ID="lblCrewRemarks" runat="server" Font-Underline="true" Text="Remarks"></telerik:RadLabel>
                            </b></u>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtCrewRemarks" runat="server" Height="50px" TextMode="MultiLine"
                                Width="500px" />
                        </td>
                    </tr>
                </table>
                <hr />

                <eluc:TabStrip ID="MenuTabShipCrew" runat="server" OnTabStripCommand="MenuTabShipCrew_TabStripCommand"></eluc:TabStrip>

                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td width="50%" style="vertical-align: top;">
                            <u><b>
                                <telerik:RadLabel ID="lblShipCrew" runat="server" Font-Underline="true" Text="Ship's Crew"></telerik:RadLabel>
                            </b></u>
                            <br />
                            <%--<asp:GridView ID="gvShipCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" ShowFooter="false"
                                EnableViewState="false" OnRowDataBound="gvShipCrew_ItemDataBound">
                                <FooterStyle CssClass="datagrid_footerstyle" />
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <Columns>--%>
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvShipCrew" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                CellSpacing="0" GridLines="None"
                                OnNeedDataSource="gvShipCrew_NeedDataSource"
                                OnItemDataBound="gvShipCrew_ItemDataBound1">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false" DataKeyNames="FLDEMPLOYEEID" TableLayout="Fixed" CommandItemDisplay="Top" >
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
                                                <telerik:RadLabel ID="lblonboardoverdue" Visible="false" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDDAYSONBOARDOVERDUE"]%>'></telerik:RadLabel>

                                                <telerik:RadLabel ID="lblLevelCode" Visible="false" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDLEVELCODE"]%>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblNCCount" Visible="false" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNCCOUNT"]%>'></telerik:RadLabel>
                                                <asp:LinkButton ID="lnkNCCount" Visible="false" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDNCCOUNT"]%>'></asp:LinkButton>
                                                <asp:Image ID="imgFlag" runat="server" Visible="false" ToolTip="Level 2 – Escalated to office" />
                                                <asp:Image ID="Imageyellow" runat="server" Visible="false" ToolTip="Level 1 – addressed onboard" />
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                        PageSizeLabelText="Records per page:" />
                                 <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>

                        </td>
                    </tr>
                </table>
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCrewListSOBYN" runat="server" Text="Is the Crew List correctly reflecting the staff onboard : "></telerik:RadLabel>
                            <telerik:RadDropDownList ID="ddlCrewListSOBYN" runat="server">
                                <Items>
                                    <telerik:DropDownListItem Value="0" Selected="True" Text="No" />
                                    <telerik:DropDownListItem Value="1" Text="Yes" />
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <u><b>
                                <telerik:RadLabel ID="lblCrewListSOBRemarks" runat="server" Font-Underline="true" Text="Remarks"></telerik:RadLabel>
                            </b></u>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtCrewListSOBRemarks" runat="server" Height="50px"
                                TextMode="MultiLine" Width="500px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <font color="blue">Note: If the crew list is not correct please provide details
                                in the 'Remarks' box.
                        </td>
                    </tr>
                </table>
            </div>
            <%--</div>--%>
        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
