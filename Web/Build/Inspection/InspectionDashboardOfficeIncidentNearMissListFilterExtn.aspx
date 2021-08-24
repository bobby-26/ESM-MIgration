<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDashboardOfficeIncidentNearMissListFilterExtn.aspx.cs" Inherits="InspectionDashboardOfficeIncidentNearMissListFilterExtn" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
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
    <title>Accident and Near Miss Filter</title>
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
                        <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlFleet" runat="server" DataTextField="FLDFLEETDESCRIPTION" DataValueField="FLDFLEETID" AutoPostBack="true"
                            EmptyMessage="Type to select fleet" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            OnItemChecked="ddlFleet_ItemChecked" Width="270px">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblOwner" runat="server" Text="Owner"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Owner ID="ucOwner" runat="server" EmptyMessage="Type to select rank" Filter="Contains" Width="270px" AddressType='<%# ((int)PhoenixAddressType.PRINCIPAL).ToString() %>' />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlVessel" runat="server" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" Width="270px">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                    </td>
                    <td>
                        <eluc:vesseltype ID="ucVesselType" runat="server" EmptyMessage="Type to select rank" Filter="Contains" Width="270px" />
                    </td>
                </tr>            
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblstatus" Text="Status" runat="server"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblIncidentClassification" Visible="false" Text="Incident Classification" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true" HardTypeCode="168" Width="270px" />
                        <telerik:RadComboBox ID="ddlIncidentNearmiss" Visible="false" runat="server" Width="270px" AutoPostBack="true"
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
                        <eluc:Hard runat="server" Width="270px" ID="ucActivity" AppendDataBoundItems="true" HardTypeCode="170" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIncidentNearMissType" runat="server" Text="Incident Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Category ID="ucCategory" Width="270px" runat="server" AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ucCategory_Changed" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIncidentNearMissSubtype" runat="server" Text="Incident Subtype"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SubCategory ID="ucSubcategory" Width="270px" runat="server" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRefNo" runat="server" MaxLength="20" Width="270px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTitle" runat="server" MaxLength="20" Width="270px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFrom" runat="server" Text="Incident Date From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucIncidentFrom" runat="server" />
                        </td>
                    <td>
                        <telerik:RadLabel ID="lblTo" runat="server" Text="Incident Date To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucIncidentTo" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReportedFrom" runat="server" Text="Reported Date From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucReportedFrom" runat="server" />
                        </td>
                    <td>
                        <telerik:RadLabel ID="lblReortedTo" runat="server" Text="Reported Date To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucReportedTo" runat="server" />
                    </td>
                </tr>
                <tr>                   
                    <td>
                        <telerik:RadLabel ID="lblConsequenceCategorization" runat="server" Text="Consequence Categorization"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucConsequenceCategory" AppendDataBoundItems="true"
                            HardTypeCode="169" Width="270px" ShortNameFilter="A,B,C" />
                    </td>
                </tr>                  
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

