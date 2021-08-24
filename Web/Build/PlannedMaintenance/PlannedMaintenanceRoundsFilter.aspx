<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceRoundsFilter.aspx.cs"
    Inherits="PlannedMaintenanceRoundsFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job Filter</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
            <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        </telerik:RadCodeBlock>
        <script type="text/javascript">
            document.onkeydown = function (e) {
                var keyCode = (e) ? e.which : event.keyCode;
                if (keyCode == 13) {
                    __doPostBack('MenuJobFilter$dlstTabs$ctl00$btnMenu', '');
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmJobFilter" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <%--<telerik:RadLabel runat="server" ID="lblCaption" Font-Bold="true" Text="Rounds Filter"></telerik:RadLabel>--%>


        <eluc:TabStrip ID="MenuJobFilter" runat="server" OnTabStripCommand="JobFilter_TabStripCommand"></eluc:TabStrip>


        <asp:UpdatePanel runat="server" ID="pnlJobFilter">
            <ContentTemplate>
                <table width="100%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblJobCode" runat="server" Text="Job Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtJobCode" CssClass="input" MaxLength="20"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblJobTitle" runat="server" Text="Job Title"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtJobTitle" CssClass="input" MaxLength="200" Width="240px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblJobClass" runat="server" Text="Job Class"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucJobClass" runat="server" QuickTypeCode="34" AppendDataBoundItems="true" />
                        </td>

                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
