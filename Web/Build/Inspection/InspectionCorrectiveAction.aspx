<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionCorrectiveAction.aspx.cs"
    Inherits="InspectionCorrectiveAction" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>CAR</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script language="Javascript">
            function isNumberKey(evt) {
                var charCode = (evt.which) ? evt.which : event.keyCode;
                if (charCode != 46 && charCode > 31
            && (charCode < 48 || charCode > 57))
                    return false;

                return true;
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionIncidentCriticalFactor" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <eluc:TabStrip ID="MenuCARGeneral" runat="server" OnTabStripCommand="MenuCARGeneral_TabStripCommand"></eluc:TabStrip>
            <table id="tblDetails" runat="server" width="100%">
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblChecklistRefNo" runat="server" Text="Checklist Ref No."></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtChecklistRefNo" runat="server" Width="100px"
                            onkeypress="return isNumberKey(event)"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblDeficiencyDetails" runat="server" Text="Deficiency Details"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDeficiencyDetails" runat="server" Height="50px"
                            Rows="4" TextMode="MultiLine" Width="97%" Resize="Both"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCorrectiveAction" runat="server" Text="Corrective Action"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <telerik:RadTextBox ID="txtCorrectiveAction" runat="server" CssClass="input_mandatory" Height="50px"
                            Rows="4" TextMode="MultiLine" Width="97%" Resize="Both"></telerik:RadTextBox>
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblDepartment" runat="server" Text="Department ( Assigned to )"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Department ID="ucDept" runat="server" AppendDataBoundItems="true" 
                            Width="270px" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblTargetDate" runat="server" Text="Target Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucTargetDate" runat="server" CssClass="input_mandatory" COMMANDNAME="TARGETDATE"
                            DatePicker="true" />
                    </td>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblVerificationLevel" runat="server" Text="Verification Level"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Hard ID="ucVerficationLevel" runat="server" AppendDataBoundItems="true" 
                            Width="270px" HardTypeCode="195" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%">
                        <telerik:RadLabel ID="lblCompletionDate" runat="server" Text="Completion Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 35%">
                        <eluc:Date ID="ucCompletionDate" runat="server" DatePicker="true" />
                    </td>
                    <td colspan="2"></td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
