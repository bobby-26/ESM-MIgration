<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceReportCommittedCost.aspx.cs"
    Inherits="PlannedMaintenanceReportCommittedCost" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmReportCommittedCost" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                <eluc:TabStrip ID="MenuReportCommittedCost" runat="server" OnTabStripCommand="MenuReportCommittedCost_TabStripCommand"></eluc:TabStrip>
            </telerik:RadCodeBlock>
            <table cellpadding="1" cellspacing="1" width="80%">
                <tr style="height: 10px;">
                    <td colspan="6">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td>Date Between
                    </td>
                    <td>
                         <eluc:Date runat="server" ID="txtDateFrom" CssClass="input" />
                        -
                         <eluc:Date runat="server" ID="txtDateTo" CssClass="input" />
                    </td>
                    <td colspan="2">&nbsp;
                    </td>
                    <td>
                        <asp:Panel ID="pnlFormat" runat="server" GroupingText="Format">
                            <%--    <asp:RadioButtonList ID="rblFormatList" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="0">Format1</asp:ListItem>
                                <asp:ListItem Value="1">Format2</asp:ListItem>
                            </asp:RadioButtonList>--%>
                            <telerik:RadRadioButtonList ID="rblFormatList" runat="server" RepeatDirection="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Value="0" Text="Format1" />
                                    <telerik:ButtonListItem Value="1" Text="Format2" />
                                </Items>
                            </telerik:RadRadioButtonList>
                        </asp:Panel>

                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
