<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionUnsafeActsConditionsAdd.aspx.cs" Inherits="InspectionUnsafeActsConditionsAdd" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Details of Unsafe Acts / Conditions</title>
    <telerik:RadCodeBlock runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuInspectionIncident" runat="server" OnTabStripCommand="InspectionIncident_TabStripCommand"></eluc:TabStrip>
            <table runat="server" cellpadding="4" style="width: 100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselByCompany ID="ddlVessel" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" VesselsOnly="true" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocation" runat="server" Text="Location"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLocation" runat="server" CssClass="input_mandatory" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbldate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDate" runat="server" DatePicker="true" CssClass="input_mandatory" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTime" runat="server" Text="Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTimePicker ID="rtpunsafetime" runat="server" Width="80px" CssClass="input_mandatory"></telerik:RadTimePicker>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucCategory" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            HardTypeCode="208" AutoPostBack="true" OnTextChangedEvent="ucCategory_TextChanged" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblsubCategory" runat="server" Text="Sub-category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSubcategory" runat="server" CssClass="input_mandatory"
                            DataTextField="FLDIMMEDIATECAUSE" DataValueField="FLDIMMEDIATECAUSEID" Filter="Contains" MarkFirstMatch="true" Width="240px">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblHeader" runat="server" Font-Bold="true" Text="Comprehensive Description of Unsafe acts / conditions"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtInvestigationAndEvidence" runat="server" CssClass="input_mandatory"
                            Height="300px" Rows="50" TextMode="MultiLine" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" CssClass="input_mandatory" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="input_mandatory" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <b>
                            <telerik:RadLabel ID="lblNote" runat="server" ForeColor="Blue" Text="Note: Entry of Rank and Name is at your option, and not mandatory."></telerik:RadLabel>
                        </b>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
