<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportDataExportPDMSList.aspx.cs" Inherits="Crew_CrewReportDataExportPDMSList" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
     <title>Data Export to PDMS</title>
     <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         
        <script type="text/javascript">
            function resizeFrame() 
            {
                var obj = document.getElementById("ifMoreInfo");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 40 + "px";
            }
        </script>
   </telerik:RadCodeBlock>
</head>
<body onload="resizeFrame()" onresize="resizeFrame()">
   <form id="form1" runat="server">
   <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false">
        </eluc:Error>
               
            <eluc:TabStrip ID="MenuCrewSelection" runat="server" OnTabStripCommand="MenuCrewSelectionType_TabStripCommand" TabStrip="true">
            </eluc:TabStrip>
        
            <iframe runat="server" id="ifMoreInfo" style="min-height: 620px; width: 100%; border:0" scrolling="yes" ></iframe>
     </telerik:RadAjaxPanel>
    </form>
</body>
</html>