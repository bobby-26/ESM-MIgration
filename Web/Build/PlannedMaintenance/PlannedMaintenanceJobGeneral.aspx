<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceJobGeneral.aspx.cs"
    Inherits="PlannedMaintenanceJobGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.PlannedMaintenance" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register Src="../UserControls/UserControlVesselType.ascx" TagName="UserControlVesselType" TagPrefix="uc1" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="../UserControls/UserControlRank.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job Detail</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPlannedMaintenanceJobGeneral" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuPlannedMaintenanceJobGeneral" runat="server" OnTabStripCommand="PlannedMaintenanceJobGeneral_TabStripCommand"></eluc:TabStrip>

        <br clear="all" />

        <table width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblJobCode" runat="server" Text="Job Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtJobCode" CssClass="input_mandatory" MaxLength="20" Width="260px"></telerik:RadTextBox>
                </td>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblJobTitle" runat="server" Text="Job Title"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtJobTitle" CssClass="input_mandatory" MaxLength="200"
                        Width="260px">
                    </telerik:RadTextBox>
                </td>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblJobClass" runat="server" Text="Job Class"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucJobClass" runat="server" CssClass="dropdown_mandatory"
                        QuickTypeCode="34" AppendDataBoundItems="true" Width="260px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblPostOverhaulcheck" runat="server" Text="Post Overhaul Operational Test and Check"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkPostOverHaulCheck" runat="server" OnCheckedChanged="chkPostOverHaulCheck_CheckedChanged"></telerik:RadCheckBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucCategory" runat="server" CssClass="dropdown_mandatory"
                        QuickTypeCode="165" AppendDataBoundItems="true" Width="260px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblDetailofCheck" runat="server" Text="Details of Check"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtDetilsofCheck" runat="server" MaxLength="200"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="PTWApproval" runat="server" Text="PTW Approval"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hard ID="ucWTOApproval" runat="server"
                        HardTypeCode="117" AppendDataBoundItems="true" DataBoundItemName="None" Width="260px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblInterval" runat="server" Text="Interval"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadRadioButtonList ID="rblInterval" runat="server">
                        <Items>
                            <telerik:ButtonListItem Value="0" Text="Hours" />
                            <telerik:ButtonListItem Value="1" Text="Days" />
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCeVerify" runat="server" Text="Vessel Verification Required"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Rank ID="ucRank" runat="server" Width="260px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblIntervalValue" runat="server" Text="Interval after the Overhaul "></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtInterValue" runat="server">
                        <ClientEvents OnKeyPress="keyPress" />
                    </telerik:RadTextBox>
                    <script type="text/javascript">
                        function keyPress(sender, args) {
                            var text = sender.get_value() + args.get_keyCharacter();
                            if (!text.match('^[0-9]+$'))
                                args.set_cancel(true);
                        }
                    </script>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSupntVerify" runat="server" Text="Supnt Verification Required"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkSupntVerification" runat="server"></telerik:RadCheckBox>
                </td>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblIsAttReq" runat="server" Text="Attachment"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkIsAttReq" runat="server" OnCheckedChanged="chkIsAttReq_CheckedChanged" AutoPostBack="true"></telerik:RadCheckBox>
                </td>
                <td colspan="2">
                    <telerik:RadTextBox runat="server" ID="txtInstructions" Resize="Both" TextMode="MultiLine" Rows="2" EmptyMessage="Type Attachment Instructions" Width="100%"></telerik:RadTextBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
