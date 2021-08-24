<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMachineryDamageFilter.aspx.cs"
    Inherits="InspectionMachineryDamageFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlIncidentNearMissCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubCategory" Src="~/UserControls/UserControlIncidentNearMissSubCategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Owner" Src="~/UserControls/UserControlAddressOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByOwner" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register Src="../UserControls/UserControlQuick.ascx" TagName="Quick" TagPrefix="eluc" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Machinery Damage Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionScheduleFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuIncidentFilter" Title="Machinery Damage Filter" runat="server" OnTabStripCommand="MenuIncidentFilter_TabStripCommand"></eluc:TabStrip>
            <br />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table runat="server" cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Fleet runat="server" ID="ucTechFleet" Width="270px" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOwner" runat="server" Text="Owner"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Owner runat="server" ID="ucAddrOwner" Width="270px" AddressType="128" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="ucAddrOwner_Changed" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselByOwner runat="server" ID="ucVessel" AppendDataBoundItems="true" VesselsOnly="true"
                            AutoPostBack="true" Width="270px" OnTextChangedEvent="ucVessel_Changed" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucVesselType" runat="server" AppendDataBoundItems="true" Width="270px" HardTypeCode="81" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListComponent">
                            <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="input" Enabled="false"
                                MaxLength="20" Width="70px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input" Enabled="false"
                                MaxLength="20" Width="198px"></telerik:RadTextBox>
                            <asp:LinkButton id="imgComponent" runat="server" style="cursor: pointer; vertical-align: middle;">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>                            
                            <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="input" Width="0px"></telerik:RadTextBox>
                            <asp:LinkButton ID="imgClearParentComponent" runat="server" ImageAlign="Middle" Style="vertical-align: middle;" 
                                OnClick="ClearComponent" ToolTip="Clear Value">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                        </span>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCritical" runat="server" Text="Critical"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkCritical" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadComboBox ID="ddlCategory" runat="server" AutoPostBack="true"
                            Width="270px" OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblSubCategory" runat="server" Text="Sub Category"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadComboBox ID="ddlSubCategory" runat="server" Width="270px" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProcessLoss" runat="server" Text="Process Loss"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlProcessLoss" runat="server" Width="270px" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCost" runat="server" Text="Cost"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCost" runat="server" Width="270px" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRefNo" runat="server" CssClass="input" MaxLength="20" Width="270px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTitle" runat="server" CssClass="input" Width="270px" MaxLength="100"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlStatus" runat="server" Width="270px" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblConsequenceCategory" runat="server" Text="Consequence Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucConsequenceCategory" AppendDataBoundItems="true"
                            Width="270px" HardTypeCode="169" ShortNameFilter="A,B,C" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIncidentFromDate" runat="server" Text="Incident Date [LT] From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucIncidentFromDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIncidentToDate" runat="server" Text="Incident Date [LT] To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucIncidentToDate" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReportedFromDate" runat="server" Text="Reported Date From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucReportedFromDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportedToDate" runat="server" Text="Reported Date To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucReportedToDate" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblClosedFromDate" runat="server" Text="Closed Date From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucClosedFromDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblClosedToDate" runat="server" Text="Closed Date To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucClosedToDate" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselActivity" runat="server" Text="Vessel Activity"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucQuickVesselActivity" runat="server" AppendDataBoundItems="true" QuickTypeCode="48" Width="270px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNearmiss" runat="server" Text="Near Miss"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblNearmiss" runat="server" Direction="Horizontal">
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
