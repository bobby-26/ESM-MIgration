<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentFilter.aspx.cs"
    Inherits="InspectionIncidentFilter" %>

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
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Incident Filter</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionScheduleFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="80%">
            <eluc:TabStrip ID="MenuIncidentFilter" runat="server" OnTabStripCommand="MenuIncidentFilter_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="2" cellspacing="2" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Fleet runat="server" ID="ucTechFleet" Width="240px" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOwner" runat="server" Text="Owner"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Owner runat="server" ID="ucAddrOwner" AddressType="128" Width="240px" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="ucAddrOwner_Changed" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselByOwner runat="server" ID="ucVessel" AppendDataBoundItems="true" Width="240px" VesselsOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucVesselType" runat="server" Width="240px" AppendDataBoundItems="true" HardTypeCode="81" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIncidentClassification" Text="Incident Classification" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlIncidentNearmiss" runat="server" Width="240px" AutoPostBack="true"
                            OnSelectedIndexChanged="ddlIncidentNearmiss_Changed" Filter="Contains">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="0" Selected="True" />
                                <telerik:RadComboBoxItem Text="Accident" Value="1" />
                                <telerik:RadComboBoxItem Text="Near Miss" Value="2" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblActivityRelevanttotheEvent" runat="server" Text="Activity relevant to the Event"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" Width="240px" ID="ucActivity" AppendDataBoundItems="true" HardTypeCode="170" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIncidentNearMissType" runat="server" Text="Incident Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Category ID="ucCategory" Width="240px" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucCategory_Changed" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIncidentNearMissSubtype" runat="server" Text="Incident Subcategory"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SubCategory ID="ucSubcategory" Width="240px" runat="server" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRefNo" runat="server" MaxLength="20" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTitle" runat="server" MaxLength="20" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="168" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblConsequenceCategorization" runat="server" Text="Consequence Categorization"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucConsequenceCategory" AppendDataBoundItems="true"
                            HardTypeCode="169" Width="240px" ShortNameFilter="A,B,C" />
                    </td>
                    <td>
                        <%--Potential Categorization--%>
                    </td>
                    <td>
                        <eluc:Hard ID="ucPotentialCategory" runat="server" Width="240px" AppendDataBoundItems="true" 
                            Visible="false" HardTypeCode="169" ShortNameFilter="A,B,C" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIncidentDateFrom" runat="server" Text="Incident Date From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucFromDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIncidentDateTo" runat="server" Text="Incident Date To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucToDate" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReportedDateFrom" runat="server" Text="Reported Date From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucReportedDateFrom" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReportedDateTo" runat="server" Text="Reported Date To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucReportedDateTo" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblShowOnlyOfficeIncidentNearMiss" runat="server" Text="Show Only Office Incident / Near Miss"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkOfficeAuditIncident" runat="server" AutoPostBack="true" OnCheckedChanged="chkOfficeAuditIncident_CheckedChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucCompany" runat="server" Enabled="false" Width="240px" CompanyList='<%# PhoenixRegistersCompany.ListCompany()%>'
                            AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblContractedRelatedIncidentYN" runat="server" Text="Contracted Related Incident Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkContractedRelatedIncidentYN" runat="server" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
