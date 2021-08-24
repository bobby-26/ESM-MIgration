<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionReportCAR.aspx.cs" Inherits="InspectionReportCAR" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagName="AuditSchedule" TagPrefix="eluc" Src="~/UserControls/UserControlAuditSchedule.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>CAR Report</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">
        function resizeFrame() {
                                var obj = document.getElementById("ifMoreInfo");
                                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 40 + "px";
                               }
        </script>
</telerik:RadCodeBlock></head>
<body onload="resizeFrame()">
    <form id="frmReportIncident" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server" />
     <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="IncidentReprot" Text="CAR" ShowMenu="true">
            </eluc:Title>
            <asp:Button runat="server" ID="cmdHiddenSubmit" />
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>
        </div>
        <div>
            <table width="100%">
                <tr>                    
                    <td>
                        <asp:Literal ID="lblAudit" runat="server" Text="Audit "></asp:Literal>
                    </td>
                    <td>
                        <eluc:AuditSchedule ID="ucAuditSchedule" runat="server" CssClass="input" AppendDataBoundItems="true" 
                            AuditScheduleStatus="717,1049" AutoPostBack="true" OnTextChangedEvent="ucAuditSchedule_TextChanged" />
                    </td>
                    <td>
                        <asp:Literal ID="lblNonConformity" runat="server" Text="Non Conformity"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlNonconformity" runat="server" CssClass="dropdown_mandatory"></asp:DropDownList>
                    </td>
                </tr>
            </table>
        </div>
        <br />
        <div>
            <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 600px;
                width: 100%;"></iframe>
        </div>
    </div>
    </form>
</body>
</html>

