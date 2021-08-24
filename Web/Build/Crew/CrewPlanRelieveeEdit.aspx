<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPlanRelieveeEdit.aspx.cs" Inherits="CrewPlanRelieveeEdit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiPort" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Relief Plan Edit</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="CrewRelieverTabs" runat="server" OnTabStripCommand="CrewRelieverTabs_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="50%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOffSigner" runat="server" Text="Off Signer"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtoffsignername" runat="server" Width="80%" Enabled="false" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" Width="80%" Enabled="false" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignerName" runat="server" Text="On Signer"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtonsignername" runat="server" Enabled="false" Width="80%" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtoffsignerRank" runat="server" Enabled="false" Width="80%" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlannedPort" runat="server" Text="Planned Port"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:MultiPort ID="ucport" runat="server" Width="80%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlannedRelief" runat="server" Text="Planned Relief"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtPlannedReliefDate" runat="server" CssClass="input_mandatory" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateOfReadiness" runat="server" Text="Date of Readiness"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtDateofReadiness" runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtcrewchangeremarks" runat="server" TextMode="MultiLine" Width="80%" Height="40px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
