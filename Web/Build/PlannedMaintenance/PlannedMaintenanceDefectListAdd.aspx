<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDefectListAdd.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceDefectListAdd" %>

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
    <title>Add</title>
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
                    <telerik:RadLabel ID="lblComponent" runat="server" Text="Component" ></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Component ID="ucComponent" runat="server" Width="300px" ></eluc:Component>
                    <telerik:RadTextBox ID="txtcomponentNo" runat="server" Width="300px" Enabled="false" ></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lbldetailsofthedefect" runat="server" Text="Details" ></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtdetailsofthedefect" runat="server" CssClass="gridinput_mandatory" 
                        TextMode="Multiline" Resize="Both" Width="300px" Rows="8"></telerik:RadTextBox>
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
                    <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadRadioButtonList ID="rdType" runat="server" Direction="Horizontal" >
                        <Items>
                            <telerik:ButtonListItem Text="Defect" Value="1" />
                            <telerik:ButtonListItem Text="Non-Routine Job" Value="2" />
                        </Items>
                    </telerik:RadRadioButtonList>
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
                    <eluc:Discipline ID="ucDisciplineResponsibility" runat="server" AppendDataBoundItems="true" Width="200px" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
 
                    <p style="color:darkblue">
                        Note:<br />
                        Fields highlighted in "Red" color are mandatory.
                    </p>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
