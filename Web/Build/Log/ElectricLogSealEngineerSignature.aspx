<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogSealEngineerSignature.aspx.cs" Inherits="Log_ElectricLogSealEngineerSignature" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Duty Engineer Signature</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style>
            .table-password {
                line-height: 35px;
                /*line-height: 100px;*/
            }
            .lblpassword {
                margin-left: 50px;
                display:block;
            }

            /*.txtPassword {
                margin-right: 5px;
            }*/
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <%--<eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>--%>

        <table class="table-password">
            <tr>
                <td colspan="2" style="text-align: center">
                    <telerik:RadLabel runat="server" Text="Are you sure you want to sign in ?"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td style="width: 200px;">
                    <telerik:RadLabel runat="server" Text="Your Password" CssClass="lblpassword"></telerik:RadLabel>
                </td>
                <td style="width:160px">
                    <telerik:RadTextBox runat="server" ID="txtPassword" Width="150px" TextMode="Password" CssClass="txtPassword" autocomplete="off"></telerik:RadTextBox></td>
            </tr>
            <tr style="margin-top:5%;">
                <td style="text-align: center">
                    <telerik:RadButton runat="server" ID="btnCancel" Text="Cancel" OnClick="btnCancel_Click"></telerik:RadButton>
                </td>
                <td style="text-align: center">
                    <telerik:RadButton runat="server" ID="btnSubmit" Text="Yes" OnClick="btnSubmit_Click"></telerik:RadButton>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
