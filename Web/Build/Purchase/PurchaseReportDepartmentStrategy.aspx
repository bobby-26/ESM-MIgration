<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseReportDepartmentStrategy.aspx.cs" Inherits="PurchaseReportDepartmentStrategy" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Department Strategy Report</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>

        <script type="text/javascript">
        function resizeFrame() 
        {
          var obj = document.getElementById("ifMoreInfo");
          obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - 40 + "px";
        }
        </script>

    </div>
</telerik:RadCodeBlock></head>
<body onload="resizeFrame()">
   <form id="frmReportPurchaseStrategy" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server" />
    <div class="navigation" id="Div2" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="Title3" Text="Department Strategy Report" ShowMenu="true">
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
                       <asp:Literal ID="lblInvoiceReceivedFromDate" runat="server" Text="Invoice Received From Date"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    </td>
                    <td>
                       <asp:Literal ID="lblInvoiceReceivedToDate" runat="server" Text="Invoice Received To Date"></asp:Literal>
                        
                    </td>
                    <td>
                        <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <iframe runat="server" id="ifMoreInfo" scrolling="auto" style="min-height: 600px;
                width: 100%;"></iframe>
        </div>
    </div>
    </form>
</body>
</html>
