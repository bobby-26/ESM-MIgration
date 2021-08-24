<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionSealUsageRemarks.aspx.cs"
    Inherits="InspectionSealUsageRemarks" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiInspection" Src="~/UserControls/UserControlMultiColumnInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiIncident" Src="~/UserControls/UserControlMultiColumnIncident.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Seal Usage Remarks</title>
    <telerik:RadCodeBlock runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSealUsageRemarks" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div  style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuSealUsage" runat="server" OnTabStripCommand="MenuSealUsage_TabStripCommand"></eluc:TabStrip>
            </div>
            <div id="divRemarksView" runat="server">
                <table width="100%" cellpadding="2" cellspacing="2">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblInspectionView" runat="server" Text="Inspection"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtInspectionView" runat="server" Width="250px" CssClass="readonlytextbox"
                                ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblIncidentView" runat="server" Text="Incident"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtIncidentView" runat="server" Width="250px" CssClass="readonlytextbox"
                                ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRemarksView" runat="server" Text="Remarks"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtRemarksView" runat="server" TextMode="MultiLine" Width="450px"
                                Height="200px" CssClass="readonlytextbox" ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="divRemarksEdit" runat="server">
                <table width="100%" cellpadding="2" cellspacing="2">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblInspection" runat="server" Text="Inspection"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:MultiInspection ID="ucMultiInspection" CssClass="input" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblIncident" runat="server" Text="Incident"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:MultiIncident ID="ucMultiIncident" CssClass="input" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="input" TextMode="MultiLine"
                                Width="450px" Height="200px" MaxLength="500">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <eluc:Status ID="ucStatus" runat="server"></eluc:Status>

    </form>
</body>
</html>
