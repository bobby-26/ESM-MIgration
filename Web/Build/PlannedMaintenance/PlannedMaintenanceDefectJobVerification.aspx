<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceDefectJobVerification.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceDefectJobVerification" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Review</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmdefectjobverify" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuSave" runat="server" OnTabStripCommand="MenuSave_TabStripCommand"></eluc:TabStrip>
        <table cellpadding="3" cellspacing="3" width="100%">
            <tr>
                <td width="20%">
                    <telerik:RadLabel ID="lbldefectjobid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEFECTJOBID") %>'></telerik:RadLabel>
                    <telerik:RadLabel ID="lbldefectno" runat="server" Text="Defect No"></telerik:RadLabel>
                </td>
                <td width="80%">
                    <telerik:RadTextBox ID="txtdefectno" runat="server" Enabled="false" ReadOnly="true" CssClass="readonlytextbox" Width="40%"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblworkorderrequired" runat="server" Text="Work Order Required"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadRadioButtonList runat="server" ID="RadioButtonlistwork" Direction="Horizontal" AutoPostBack="false">
                        <Items>
                            <telerik:ButtonListItem Text="No" Value="0" />
                            <telerik:ButtonListItem Text="Yes" Value="1" />
                        </Items>
                    </telerik:RadRadioButtonList>
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
 
                    <p style="color:darkblue">
                        Note:<br />
                        Raise Work Order if the repairs require spares/ stores or taking machinery out of service or are not within the capabilities of the ship's staff.
                    </p>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
