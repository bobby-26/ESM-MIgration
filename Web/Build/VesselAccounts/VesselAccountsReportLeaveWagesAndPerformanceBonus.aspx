﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsReportLeaveWagesAndPerformanceBonus.aspx.cs"
    Inherits="VesselAccountsReportLeaveWagesAndPerformanceBonus" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Leave Wages And Performance Bonus</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function resize() {
                var obj = document.getElementById("ifMoreInfo");
                obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 75 + "px";
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body onload="resize();" onresize="resize();">
    <form id="frmLeaveWagesPerformanceBonus" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="MenuReportLeaveWagesPerformanceBonus" runat="server" OnTabStripCommand="MenuReportLeaveWagesPerformanceBonus_TabStripCommand"></eluc:TabStrip>

            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFromDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblClosingDate" runat="server" Text="Closing Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtToDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPortageBillHistory" runat="server" Text="Portage Bill History"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox DropDownPosition="Static" ID="ddlHistory" runat="server" EnableLoadOnDemand="True"
                            EmptyMessage="Type to select Type" Filter="Contains" MarkFirstMatch="true" DataTextField="FLDPBCLOSGINDATE"
                            DataValueField="FLDPBCLOSGINDATE" OnSelectedIndexChanged="ddlHistory_SelectedIndexChanged">
                        </telerik:RadComboBox>

                    </td>
                </tr>
            </table>

            <div>
                <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 200px; width: 99.5%;"></iframe>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
