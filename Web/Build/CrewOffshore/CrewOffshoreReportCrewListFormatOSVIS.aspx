<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreReportCrewListFormatOSVIS.aspx.cs"
    Inherits="CrewOffshore_CrewOffshoreReportCrewListFormatOSVIS" %>

<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew List OSVIS Format</title>
    <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
            function resizeFrame() {
                var obj = document.getElementById("ifMoreInfo");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 40 + "px";
            }
        </script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmReportCrewList" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" />

        <eluc:TabStrip ID="MenuOSVISClewList" runat="server" OnTabStripCommand="MenuOSVISClewList_TabStripCommand"></eluc:TabStrip>

        <telerik:RadLabel ID="lblDeckHeader" runat="server" Text="DECK DEPARTMENT" Style="font-weight: bolder; text-decoration: underline;">DECK DEPARTMENT</telerik:RadLabel>
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel runat="server" ID="ltDeck" Text=""></telerik:RadLabel>
                </td>
            </tr>
        </table>
        <br />
        <telerik:RadLabel ID="lblEngineHeader" runat="server" Text="ENGINE DEPARTMENT" Style="font-weight: bolder; text-decoration: underline;">ENGINE DEPARTMENT</telerik:RadLabel>
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel runat="server" ID="ltEngine" Text=""></telerik:RadLabel>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
