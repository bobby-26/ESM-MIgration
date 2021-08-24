<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterFMSManualCategoryAdd.aspx.cs" Inherits="Registers_RegisterFMSManualCategoryAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Manual Category Add</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersManual" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuRegistersTypeofChangeCategoryAdd" runat="server" OnTabStripCommand="MenuRegistersTypeofChangeCategoryAdd_TabStripCommand"
            TabStrip="true" />
        <br />
        <table runat="server">
            <tr>
                <td style="padding-right: 40px">
                    &nbsp&nbsp&nbsp<telerik:RadLabel runat="server" ID="lblcode2" Text="Code"></telerik:RadLabel>
                </td>
                <td style="padding-right: 40px">
                    <telerik:RadTextBox ID="txtcode" runat="server" MaxLength="3" Width="264px" CssClass="gridinput_mandatory"
                        Visible="true">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="padding-right: 40px">
                    &nbsp&nbsp&nbsp<telerik:RadLabel runat="server" ID="lblname" Text="Name"></telerik:RadLabel>
                </td>
                <td style="padding-right: 40px">
                    <telerik:RadTextBox ID="txtname" runat="server" TextMode="MultiLine" Resize="Both"
                        Width="270px" Rows="3" CssClass="gridinput_mandatory">
                    </telerik:RadTextBox>
                </td>
            </tr>
        </table>
        <eluc:Status runat="server" ID="ucStatus" />
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>

