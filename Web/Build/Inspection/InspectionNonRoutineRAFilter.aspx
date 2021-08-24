<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionNonRoutineRAFilter.aspx.cs" Inherits="Inspection_InspectionNonRoutineRAFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inspection RA Generic Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSealUsageFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuRAGenericFilter" runat="server" OnTabStripCommand="MenuRAGenericFilter_TabStripCommand"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRAType" runat="server" Text="Non Routine Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlRAType" runat="server" Width="240px"
                            OnSelectedIndexChanged="ddlRAType_Changed" Filter="Contains">
                            <Items>
                                <telerik:RadComboBoxItem Text="Generic" Value="2" Selected="True" />
                                <telerik:RadComboBoxItem Text="Navigation" Value="1" />
                                <telerik:RadComboBoxItem Text="Machinery" Value="3" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRefNo" runat="server" Text="Ref Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRefNo" runat="server" Width="205px"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFleet" runat="server" Text="Fleet"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Fleet runat="server" ID="ucTechFleet" Width="240px" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblActivityConditions" runat="server" Text="Activity Conditions"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtActivityConditions" runat="server" Width="205px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" Width="240px" AssignedVessels="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPreparedDate" runat="server" Text="Prepared Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDatePreparedFrom" runat="server" />
                        &nbsp;-&nbsp;
                        <eluc:Date ID="ucDatePreparedTo" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlStatus" runat="server" AppendDataBoundItems="true" Width="240px"
                             Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIntendedWork" runat="server" Text="Intended Work Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDateIntendedWorkFrom" runat="server" />
                        &nbsp;-&nbsp;
                        <eluc:Date ID="ucDateIntendedWorkTo" runat="server" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
