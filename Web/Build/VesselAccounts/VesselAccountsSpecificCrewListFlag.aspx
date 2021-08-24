<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsSpecificCrewListFlag.aspx.cs" Inherits="VesselAccountsSpecificCrewListFlag" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Specific Crew List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function resizeFrame() {
                var obj = document.getElementById("ifMoreInfo");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 75 + "px";
            }
        </script>

    </telerik:RadCodeBlock>
</head>
<body onload="resizeFrame()">
    <form id="frmReportCommittedCost" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />

            <asp:Button runat="server" ID="cmdHiddenSubmit" />
            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" VesselsOnly="true" Enabled="false"
                            CssClass="dropdown_mandatory" Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFormats" runat="server" Text="Formats"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblFormats" runat="server" RepeatDirection="Horizontal"
                            OnSelectedIndexChanged="rblFormats_SelectedIndexChanged" AutoPostBack="true">
                            <asp:ListItem>Specific Crew List With Flag Details</asp:ListItem>
                            <asp:ListItem>Specific Crew List With Licence</asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <div>
                <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 600px; width: 100%;"></iframe>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
