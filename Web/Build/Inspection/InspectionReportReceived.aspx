<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionReportReceived.aspx.cs"
    Inherits="InspectionReportReceived" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report Received Date</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status ID="ucStatus" runat="server" />
    <div class="subHeader" style="position: relative">
        <div class="subHeader" style="position: relative">
            <eluc:Title runat="server" ID="ucTitle" Text="RCA Completion" ShowMenu="false"></eluc:Title>
        </div>
    </div>
    <br />
    <table cellpadding="1" cellspacing="1" width="30%" style = "overflow: hidden">
        <tr>
            <td colspan="2">
                <b>
                    <asp:Literal ID="lblEnterCompletionDate" runat="server" Text="Enter Completion Date"></asp:Literal></b>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblReferenceNo" runat="server" Text="Reference Number"></asp:Literal>
            </td>
            <td>
                <asp:TextBox ID="txtReferenceNumber" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Literal ID="lblRCACompletionDate" runat="server" Text="RCA Completion Date"></asp:Literal>
            </td>
            <td>
                <eluc:Date ID="txtCompletionDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small"></asp:Label>
            </td>
        </tr>
    </table>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuReport" runat="server" OnTabStripCommand="MenuReport_TabStripCommand">
        </eluc:TabStrip>
    </div>
    </form>
</body>
</html>
