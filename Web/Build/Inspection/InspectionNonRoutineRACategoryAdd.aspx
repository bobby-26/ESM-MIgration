<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionNonRoutineRACategoryAdd.aspx.cs" Inherits="InspectionNonRoutineRACategoryAdd" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Non Routine RA Category</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmNonRoutineRACategoryAdd" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <eluc:TabStrip ID="MenuNonRoutineRACategoryAdd" runat="server" OnTabStripCommand="MenuNonRoutineRACategoryAdd_TabStripCommand" Title="Category"></eluc:TabStrip>
            <table id="tblDetails" runat="server" width="100%">
                <tr>
                    <td style="width: 20%" valign="top">
                        <telerik:RadLabel ID="lblCode" runat="server" Text="Code :"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtShortcode" runat="server" CssClass="input_mandatory" Width="90px" MaxLength="5" >
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" valign="top">
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name :"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="input_mandatory" Width="360px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="width: 15%" valign="top">
                        <telerik:RadLabel ID="lblActiveyn" runat="server" Text="Active Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="cbActive" runat="server" ></telerik:RadCheckBox>
                    </td>
                </tr>                    
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
