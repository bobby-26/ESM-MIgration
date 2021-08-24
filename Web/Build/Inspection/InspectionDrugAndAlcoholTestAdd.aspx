<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionDrugAndAlcoholTestAdd.aspx.cs" Inherits="InspectionDrugAndAlcoholTestAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddrType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiInspector" Src="~/UserControls/UserControlMultiColumnInspector.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Drug and Alcohol Test</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersInspectionIncident" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuInspectionScheduleGeneral" runat="server" OnTabStripCommand="MenuInspectionScheduleGeneral_TabStripCommand"></eluc:TabStrip>
        <table id="tblConfigureInspectionIncident" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input" Width="300px" VesselsOnly="true" AppendDataBoundItems="true" Entitytype="VSL" ActiveVesselsOnly="true"></eluc:Vessel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:MultiPort ID="ucport" runat="server" OnTextChangedEvent="ucport_Changed" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ucAuditCategory" runat="server" AppendDataBoundItems="true" AutoPostBack="true" CssClass="dropdown_mandatory"
                        HardTypeCode="144" OnTextChangedEvent="Bind_UserControls" Width="300px" />
                </td>

            </tr>


            <tr>
                <td>
                    <telerik:RadLabel ID="lblDateofTest" runat="server" Text="Date of Test"></telerik:RadLabel>
                </td>
                <td width="30%">
                    <eluc:Date ID="txtDateofTest" runat="server" CssClass="input_mandatory" DatePicker="true"
                        AutoPostBack="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDrugAndAlcoholTestYN" runat="server" Text="Test Completed Y/N" Visible="false"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkDrugAndAlcoholTestYN" runat="server" AutoPostBack="false" Visible="false"/>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <b>
                        <telerik:RadLabel ID="lblInternalAuditor" runat="server" Text="Internal Auditor / Inspector"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
            <tr>
                <td valign="baseline">
                    <telerik:RadLabel ID="lblNameDesignation" runat="server" Text="Name &amp; Designation"></telerik:RadLabel>
                </td>
                <td valign="baseline">

                    <eluc:MultiInspector ID="ucInspector" runat="server" Width="250px" CssClass="input" />
                    <%-- Enabled="false" --%>

                    <telerik:RadTextBox ID="txtInspector" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                        Width="100px" Visible="false">
                    </telerik:RadTextBox>

                </td>
                <td>
                    <telerik:RadLabel ID="lblOrganization" runat="server" Text="Organization"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtOrganization" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                        Width="100px" Visible="false">
                    </telerik:RadTextBox>
                    <telerik:RadComboBox ID="ddlOrganization" runat="server" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"
                        Width="200px">
                        <%--Enabled="false"--%>
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <hr />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <b>
                        <telerik:RadLabel ID="lblExternalAuditor" runat="server" Text="Extenal Auditor / Inspector"></telerik:RadLabel>
                    </b>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNameDesignation1" runat="server" Text="Name &amp; Designation"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtExternalInspectorName" runat="server" CssClass="input" Width="250px">
                        <%--Enabled="false"--%>
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblOrganization1" runat="server" Text="Organization"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtExternalOrganisationName" runat="server" Enabled="false" CssClass="input"
                        Width="100px" Visible="false">
                    </telerik:RadTextBox>
                    <telerik:RadComboBox ID="ddlExternalOrganizationName" runat="server" Width="200px"
                        EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                        <%-- Enabled="false"--%>
                    </telerik:RadComboBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
