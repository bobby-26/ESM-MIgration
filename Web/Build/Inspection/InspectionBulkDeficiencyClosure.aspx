<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionBulkDeficiencyClosure.aspx.cs" Inherits="Inspection_InspectionBulkDeficiencyClosure" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmDirectorComment" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuOfficeRemarks" runat="server" OnTabStripCommand="MenuOfficeRemarks_TabStripCommand" Title="Bulk Deficiency Closure"></eluc:TabStrip>
            <br />
            <table cellpadding="1" cellspacing="1" width="60%">
                <tr>
                    <td width="35%">
                        <telerik:RadLabel ID="lblOfficeRemarks" runat="server" Text="Close Out Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtOfficeRemarks" runat="server" CssClass="input_mandatory" Height="100px" Rows="15"
                            TextMode="MultiLine" Width="100%" Resize="Both"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr style="height: 35px">
                    <td style="width: 30%">Closed
                    </td>
                    <td>
                        <eluc:Date ID="ucCompletionDate" runat="server" CssClass="input_mandatory" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblMessage" runat="server" ForeColor="Red" Font-Bold="true" Font-Size="Small" Width="150%"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
