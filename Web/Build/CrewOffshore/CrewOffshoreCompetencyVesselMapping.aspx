<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreCompetencyVesselMapping.aspx.cs" Inherits="CrewOffshore_CrewOffshoreCompetencyVesselMapping" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Map Vessel Type</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmTemplateMapping" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="88%">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuTemplateMapping" runat="server" OnTabStripCommand="MenuTemplateMapping_TabStripCommand"
                Title="Map Vessel Type"></eluc:TabStrip>
            <br />
            <table cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcat" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtevent" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="385px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmeasure" runat="server" Text="Measure"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtmeasure" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="385px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblevent" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <div id="divVesseltype" runat="server" visible="true" class="input" style="overflow-y: auto; overflow-x: auto; width: 100%; height: 100%">
                            <telerik:RadCheckBoxList ID="cblvesseltype" runat="server"  Columns="3">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>


