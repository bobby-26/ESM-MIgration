<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionAuditInterfaceBulkUpdate.aspx.cs" Inherits="InspectionAuditInterfaceBulkUpdate" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:TabStrip ID="MenuUpload" runat="server" OnTabStripCommand="MenuUpload_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <eluc:Status runat="server" ID="ucStatus" />

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table id="tblSearch">
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel2" runat="server" Font-Bold="true" Text="Select" Width="50px"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadRadioButtonList runat="server" ID="btnAnsSearch" Direction="Horizontal" CssClass="content" DataBindings-DataTextField="FLDQUICKNAME"
                            DataBindings-DataValueField="FLDQUICKCODE">
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel4" runat="server" Font-Bold="true" Text="Remarks" Width="50px"></telerik:RadLabel>
                        &nbsp;&nbsp;
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" Width="360px" Rows="6" TextMode="MultiLine" Resize="Both"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
