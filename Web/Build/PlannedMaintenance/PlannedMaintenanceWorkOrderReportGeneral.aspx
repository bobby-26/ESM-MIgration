<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderReportGeneral.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderReportGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmWorkOrderGeneral" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel ID="pnlMenuWorkOrderGeneral" runat="server">
            <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
                <eluc:TabStrip ID="MenuWorkOrderGeneral" runat="server" OnTabStripCommand="MenuWorkOrderGeneral_TabStripCommand"></eluc:TabStrip>
            </telerik:RadCodeBlock>

            <br clear="all" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateDone" runat="server" Text="Date Done"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtWorkDoneDate" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTotalManHours" runat="server" Text="Total Man Hours"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<eluc:Decimal ID="txtWorkDuration" runat="server" CssClass="input" Mask="999.99" Width="60px" Text="" />--%>
                     <telerik:RadMaskedTextBox ID="txtWorkDuration" runat="server" CssClass="input" Mask="###.##" Width="60px" />

                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td colspan="3">
                        <telerik:RadCheckBox ID="chkUnplanned" Text="Unplanned Work" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <telerik:RadCheckBox ID="chkHistory" Text="History Template" runat="server" />
                    </td>
                    <td></td>
                    <td>
                        <telerik:RadCheckBox ID="chkUsedParts" Text="Stock Used" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <telerik:RadCheckBox ID="chkResources" Text="Resources Used" runat="server" />
                    </td>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <hr />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCounter" runat="server" Text="Counter"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Hard ID="ucCounter" runat="server" CssClass="input" HardTypeCode="111" AppendDataBoundItems="true" />
                        <eluc:Decimal ID="txtCurrentValues" runat="server" CssClass="input" MaxLength="20"
                            Width="90px" Mask="999999999.99" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLastRead" runat="server" Text="Last Read"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReadDate" RenderMode="Lightweight" runat="server" CssClass="input readonlytextbox" Width="90px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastReadValue" runat="server" Text="Last Read Value"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReadValue" runat="server" RenderMode="Lightweight" CssClass="input readonlytextbox txtNumber" Width="90px" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
