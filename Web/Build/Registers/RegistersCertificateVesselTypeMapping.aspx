<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCertificateVesselTypeMapping.aspx.cs" Inherits="RegistersCertificateVesselTypeMapping" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel Type Mapping</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVesselTypeMapping" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuVesselTypeMapping" runat="server" OnTabStripCommand="MenuVesselTypeMapping_TabStripCommand"></eluc:TabStrip>
            <br clear />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <b><telerik:RadLabel ID="lblCertificate" runat="server" Text="Certificate"></telerik:RadLabel></b>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCertificate" runat="server" Width="400px" CssClass="input" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadCheckBox ID="chkCheckAll" runat="server" Text="Check All" AutoPostBack="true"
                            OnCheckedChanged="SelectAll" />
                    </td>
                </tr>
            </table>
            <telerik:RadCheckBoxList runat="server" ID="cblVesselType" Height="90%" Columns="3" Direction="Vertical" Layout="Flow" AutoPostBack="true">
            </telerik:RadCheckBoxList>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
