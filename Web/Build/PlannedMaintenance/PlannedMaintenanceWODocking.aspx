<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWODocking.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceWODocking" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="FormWOWorkRequest" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuWOWorkRequest" runat="server" OnTabStripCommand="MenuWOWorkRequest_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server">

            <table width="100%" runat="server">
                <tr>
                        <td style="width:10%;">
                            <telerik:RadLabel runat="server" ID="lblJobNumber" Text="Job Number"></telerik:RadLabel>
                        </td>
                        <td style="width:25%;">
                            <telerik:RadTextBox runat="server" ID="txtJobNumber" Width="300px" Enabled="false"></telerik:RadTextBox>
                        </td>
                        <td style="width:10%;">
                            <telerik:RadLabel runat="server" ID="lblTitle" Text="Job Title"></telerik:RadLabel>
                        </td>
                        <td style="width:25%;">
                            <telerik:RadTextBox runat="server" ID="txtJobName" Width="300px" Enabled="false"></telerik:RadTextBox>
                        </td>
                    </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaintenanceClass" runat="server" Text="Maintenance Class"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMaintClass" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                            Width="300px" />
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblMaintenanceType" runat="server" Text="Maintenance Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMainType" runat="server" AppendDataBoundItems="true" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td width="20%">
                        <telerik:RadLabel runat="server" ID="lblWorktobeSurveyedBy" Text="Surveyed by"></telerik:RadLabel>
                    </td>
                    <td width="30%">
                        <div style="height: 100px; width: 85%; overflow: auto;" class="input">
                            <telerik:RadCheckBoxList runat="server" ID="cblWorkSurvey" Width="85%"></telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td width="20%">
                        <telerik:RadLabel runat="server" ID="lblMaterial" Text="Material"></telerik:RadLabel>
                    </td>
                    <td width="30%">
                        <div style="height: 100px; width: 85%; overflow: auto;" class="input">
                            <telerik:RadCheckBoxList runat="server" ID="cblMaterial"></telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblEnclosed" Text="Enclosed"></telerik:RadLabel>
                    </td>
                    <td>
                        <div style="height: 100px; width: 85%; overflow: auto;" class="input">
                            <telerik:RadCheckBoxList runat="server" ID="cblEnclosed"></telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblJobType" runat="server" Text="Job Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlJobType" runat="server" AppendDataBoundItems="true" Width="85%">
                            <Items>
                                <telerik:DropDownListItem Value="" Text="--Select--" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblInclude" Text="Include"></telerik:RadLabel>
                    </td>
                    <td>
                        <div style="height: 100px; width: 85%; overflow: auto;" class="input">
                            <telerik:RadCheckBoxList ID="cblInclude" runat="server">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>

            </table>

        </telerik:RadAjaxPanel>

    </form>
</body>
</html>
