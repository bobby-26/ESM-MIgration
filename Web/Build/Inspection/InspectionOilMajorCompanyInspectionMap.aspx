<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionOilMajorCompanyInspectionMap.aspx.cs"
    Inherits="InspectionOilMajorCompanyInspectionMap" %>

<!DOCTYPE html>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inspection Mapping</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInspectionMapping" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
            <eluc:TabStrip ID="MenuMapping" runat="server" OnTabStripCommand="MenuMapping_TabStripCommand" Title="Audit/Inspection/Vetting Mapping"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td>&nbsp;&nbsp<telerik:RadLabel ID="lblCompanyName" runat="server" Font-Bold="true" Text="Company Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOilMajorCompany" runat="server" ReadOnly="true" Width="320px"
                            CssClass="readonlytextbox">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">&nbsp;&nbsp<telerik:RadLabel ID="lblSource" runat="server" Font-Bold="true" Text="Audit/Inspection/Vetting"></telerik:RadLabel>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        <div id="dvInspection" runat="server" class="input" style="overflow: auto; width: 55%; left: 1%; position: absolute; height: 325px;">
                            <telerik:RadCheckBoxList ID="cblInspection" runat="server" Direction="Vertical" Columns="1">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
