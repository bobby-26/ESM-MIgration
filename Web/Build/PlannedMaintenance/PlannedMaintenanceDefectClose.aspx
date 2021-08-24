<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDefectClose.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceDefectClose" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Complete</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmdefectjobadd" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuSave" runat="server" OnTabStripCommand="MenuSave_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="3" cellspacing="3" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblduedate" runat="server" Text="Done Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucDoneDate" runat="server" CssClass="gridinput_mandatory" Width="150px"></eluc:Date>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lbldetailsofthedefect" runat="server" Text="Comments"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtComments" runat="server" CssClass="gridinput_mandatory"
                        TextMode="Multiline" Resize="Both" Width="300px" Rows="8">
                    </telerik:RadTextBox>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
