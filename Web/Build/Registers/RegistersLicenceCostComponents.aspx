<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersLicenceCostComponents.aspx.cs" Inherits="RegistersLicenceCostComponents" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Licence Cost Components</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLicenceCostComponents" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuLicenceCostComponents" runat="server" Title="Licence-Cost Components" OnTabStripCommand="LicenceCostComponents_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="80%">
            <telerik:RadTextBox runat="server" ID="txtLevel" Style="text-align: right;" Width="360px"
                CssClass="input" Visible="false">
            </telerik:RadTextBox>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="2">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblLicenceComponents" runat="server" Text="Licence Components"></telerik:RadLabel>
                    </td>
                    <td>
                        <div runat="server" id="dvRank" class="input_mandatory" style="overflow: auto; width: 100%; height: 140px">
                            <telerik:RadCheckBoxList runat="server" ID="cblLicenceComponents" Height="100%" Columns="1" Direction="Horizontal" Width="500px">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
