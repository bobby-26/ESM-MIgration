<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionUnsafeActsConditionsFilter.aspx.cs"
    Inherits="Inspection_InspectionUnsafeActsConditionsFilter" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Owner" Src="~/UserControls/UserControlAddressOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="addresstype" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByOwner" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Unsafe Acts / Conditions Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOpenReportsFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuOpenReportsFilter" runat="server" OnTabStripCommand="MenuOpenReportsFilter_TabStripCommand"></eluc:TabStrip>
            <br />
            <table width="90%">
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
                        <eluc:VesselByOwner ID="ucVessel" runat="server" Width="240px" AppendDataBoundItems="true" VesselsOnly="true" />
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
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucCategory" runat="server" AppendDataBoundItems="true" HardTypeCode="208" AutoPostBack="true" Width="240px" OnTextChangedEvent="ucCategory_TextChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSubCategory" runat="server" Text="Sub-category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSubcategory" Width="240px" runat="server" Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReferenceNumber" runat="server" Width="242px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucStatusofUA" runat="server" AppendDataBoundItems="true" Width="240px" HardTypeCode="146"
                            ShortNameFilter="OPN,CMP,CLD,CAD" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIncidentFromDate" runat="server" Text="Incident From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucIncidentNearMissFromDate" runat="server" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIncidentToDate" runat="server" Text="Incident To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucIncidentNearMissToDate" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <%--Reported From Date--%>
                    </td>
                    <td>
                        <eluc:Date ID="ucCreatedFromDate" runat="server" Visible="false" />
                    </td>
                    <td>
                        <%--Reported To Date--%>
                    </td>
                    <td>
                        <eluc:Date ID="ucCreatedToDate" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIncidentRaisedYN" runat="server" Text="Incident / Near Miss Raised YN "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkIncidentNearMissRaisedYN" runat="server" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
