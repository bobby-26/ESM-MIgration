<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceToolboxMeeting.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceToolboxMeeting" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>        
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuToolBoxMeet" runat="server" OnTabStripCommand="MenuToolBoxMeet_TabStripCommand" />
        <table border="0">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDateTime" runat="server" Text="Date & Time"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDateTimePicker ID="txtDateTime" runat="server" Width="200px">
                    </telerik:RadDateTimePicker>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPIC" runat="server" Text="PIC"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlPersonIncharge" runat="server" DataTextField="FLDEMPLOYEENAME" DataValueField="FLDSIGNONOFFID"
                        EmptyMessage="Type to select crew" Filter="Contains" MarkFirstMatch="true" Width="200px">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblOtherMembers" runat="server" Text="Other Members"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlOtherMembers" runat="server" DataTextField="FLDEMPLOYEENAME" DataValueField="FLDSIGNONOFFID" Width="200px"
                        EmptyMessage="Type to select crew" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNotes" runat="server" Text="Notes"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtNotes" runat="server" TextMode="MultiLine" Width="200px"></telerik:RadTextBox>
                </td>
            </tr>
        </table>         
    </form>
</body>
</html>
