<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDefectListUpdate.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceDefectListUpdate" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Component" Src="~/UserControls/UserControlMultiColumnComponents.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Edit</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmdefectupdate" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="MenuSave" runat="server" OnTabStripCommand="MenuSave_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="3" cellspacing="3" width="100%">
            <tr>
                <td width="18%">
                    <telerik:RadLabel ID="lbldefectjobid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFECTJOBID") %>'></telerik:RadLabel>
                    <telerik:RadLabel ID="lbldefectno" runat="server" Text="Defect Number" Width="50%"></telerik:RadLabel>
                </td>
                <td width="82%">
                    <telerik:RadTextBox ID="txtdefectno" runat="server" Enabled="false" ReadOnly="true" CssClass="readonlytextbox" Width="50%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="ucComponent" runat="server" Width="300px" Enabled="false" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lbldetailsofthedefect" runat="server" Text="Defect Details"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtdetailsofthedefect" runat="server" TextMode="Multiline" Rows="8" Resize="Both" CssClass="gridinput_mandatory" Width="300px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblActionRequired" runat="server" Text="Action Required"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtActionRequired" runat="server" CssClass="input" TextMode="MultiLine" Resize="Both" Width="300px" Rows="8"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblduedate" runat="server" Text="Due Date"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="ucDueDate" runat="server" CssClass="gridinput_mandatory" Width="150px"></eluc:Date>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblResponsibilitysearch" runat="server" Text="Responsibility"></telerik:RadLabel>
                </td>

                <td>
                    <eluc:Discipline ID="ucDisciplineResponsibility" runat="server" Width="200px" AppendDataBoundItems="true" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
