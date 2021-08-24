<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderReportLogGeneral.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderReportLogGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>  
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrderReportLogGeneral" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadCodeBlock runat="server" ID="RadCodeBlock2">
            <eluc:TabStrip ID="MenuWorkOrderGeneral" runat="server" OnTabStripCommand="MenuWorkOrderGeneral_TabStripCommand"></eluc:TabStrip>
        </telerik:RadCodeBlock>

        <br clear="all" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table width="100%" cellpadding="1" cellspacing="1">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtWorkOrderNumber" runat="server" Enabled="false" CssClass="readonlytextbox"
                        MaxLength="20" Width="110px" ReadOnly="true">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtWorkOrderTitle" runat="server" CssClass="readonlytextbox" Width="320px"
                        ReadOnly="true">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblComponentNumber" runat="server" Text="Component Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="readonlytextbox" MaxLength="20"
                        Width="80px" ReadOnly="true">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblComponentName" runat="server" Text="Component Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox  ID="txtComponentName" runat="server" CssClass="readonlytextbox" MaxLength="20"
                        Width="320px" ReadOnly="true">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblStartedDate" runat="server" Text="Started Date"></telerik:RadLabel>
                </td>
                <td>
                    <%--<eluc:Date ID="txtStartedDate" runat="server" ReadOnly="true" CssClass="input readonlytextbox" />--%>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtStartedDate" runat="server" ReadOnly="true" MaxLength="20" 
                        Width="100px" CssClass="readonlytextbox">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblCompletedDate" runat="server" Text="Completed Date"></telerik:RadLabel>
                </td>
                <td>
                    <%--<eluc:Date ID="txtCompletedDate" runat="server" ReadOnly="true" CssClass="input readonlytextbox" />--%>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtCompletedDate" runat="server" ReadOnly="true" MaxLength="20"
                        Width="100px" CssClass="readonlytextbox">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDateDone" runat="server" Text="Date Done"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtWorkDoneDate" runat="server" CssClass="readonlytextbox" MaxLength="20"
                        Width="100px" ReadOnly="true">
                    </telerik:RadTextBox>
                </td>
                 <td>
                        <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Responsibility"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtResponsibility" runat="server" Width="320px"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCounterHrs" runat="server" Text="Counter(Hrs.)"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCounter" runat="server" CssClass="readonlytextbox txtNumber"
                        ReadOnly="true" Width="60px">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblDefectList" runat="server" Text="Defect List"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkDefectList" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblMaintenanceType" runat="server" Text="Maintenance Type"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucMainType" runat="server" AppendDataBoundItems="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblMaintenanceClass" runat="server" Text="Maintenance Class"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucMaintClass" runat="server" AppendDataBoundItems="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblMaintenanceClaim" runat="server" Text="Maintenance Claim"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Quick ID="ucMainCause" runat="server" AppendDataBoundItems="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblUnplannedWork" runat="server" Text="Unplanned Work"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkUnexpected" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVslVerified" runat="server" Text="Vessel Verified / Date"></telerik:RadLabel>
                 </td>
                <td>
                    <telerik:RadTextBox ID="txtVslVerified" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtVslVerifiedDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblSupntVerified" runat="server" Text="Supnt Verified / Date"></telerik:RadLabel>
                 </td>
                <td>
                    <telerik:RadTextBox ID="txtSupntVerified" runat="server" CssClass="readonlytextbox" ReadOnly="true" ></telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtSupntVerifiedDate" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
             <td>
                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="Total Duration(Hrs.)"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtWorkDuration" runat="server" CssClass="readonlytextbox txtNumber"
                        ReadOnly="true" Width="60px">
                    </telerik:RadTextBox>
                </td>
           
            </tr>
        </table>
    </form>
</body>
</html>
