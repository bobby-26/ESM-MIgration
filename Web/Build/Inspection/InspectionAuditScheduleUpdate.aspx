<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditScheduleUpdate.aspx.cs" Inherits="InspectionAuditScheduleUpdate" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiInspector" Src="~/UserControls/UserControlMultiColumnInspector.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Schedule Update</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionScheduleFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" EnableAJAX="false">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuScheduleFilter" runat="server"  OnTabStripCommand="MenuScheduleFilter_TabStripCommand"></eluc:TabStrip>
                <table cellpadding="2" cellspacing="2" width="100%">

                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel "></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtvesselname" Width="270px" Enabled="false" runat="server"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblMC" runat="server" Text="M/C"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtMC" Width="270px" Enabled="false" runat="server"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblinspection" runat="server" Text="Audit / Inspection"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtinspectionname" Width="270px" Enabled="false" runat="server"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblcategory" runat="server" Text="Category"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtcategoryname" Width="270px" Enabled="false" runat="server"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lbllastdone" runat="server" Text="Last Done"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtlastdone" runat="server" DatePicker="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblduedate" runat="server" Text="Due"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtduedate" runat="server" DatePicker="true" />
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblplandate" runat="server" Text="Planned Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtplandate" runat="server" DatePicker="true" />
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFromPort" runat="server" Text="From Port"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:MultiPort ID="ucFromPort" runat="server" Width="270px" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblToPort" runat="server" Text="To Port"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:MultiPort ID="ucToPort" runat="server" Width="270px" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAttendingSupt" runat="server" Text="Attending Supt"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:MultiInspector ID="ucInspector" runat="server" Width="270px" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblExternalAuditor" runat="server" Text="External Auditor"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtExternalAuditor" runat="server" Width="270px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr runat="server" visible="false">
                        <td>
                            <telerik:RadLabel ID="lblOrganizationStatus" runat="server" Text="Organization"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOrganizationStatus" runat="server" Width="270px"></telerik:RadTextBox>

                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
         </telerik:RadAjaxPanel>
    </form>
</body>
</html>

