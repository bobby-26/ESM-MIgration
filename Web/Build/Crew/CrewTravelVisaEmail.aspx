<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTravelVisaEmail.aspx.cs"
    Inherits="CrewTravelVisaEmail" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Mail Compose</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="75%" EnableAJAX="false">
            <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
            <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
            <eluc:TabStrip ID="MenuMailRead" runat="server" OnTabStripCommand="MenuMailRead_TabStripCommand"></eluc:TabStrip>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" border="1">
                <tr>
                    <td width="40%">
                        <table width="100%">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblTo" runat="server" Text="To"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtTo" runat="server" CssClass="input" Width="90%" ReadOnly="true"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblCc" runat="server" Text="Cc"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtCc" runat="server" CssClass="input" Width="90%" ReadOnly="true"></telerik:RadTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadTextBox ID="txtSubject" runat="server" CssClass="input" Width="90%" Text="For Vessel Accounting Initialization."></telerik:RadTextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td width="40%"></td>
                </tr>
            </table>
            <table>
                <tr>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAttachement" runat="server" Font-Bold="true" Text="Attachment:"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblFileName" runat="server" Font-Bold="true"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:LinkButton ID="lknfilename" runat="server" Text="View" OnClick="lknfilename_OnClick"
                            Font-Bold="true"></asp:LinkButton>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td colspan="2" width="100%">
                        <telerik:RadEditor ID="edtBody" runat="server" Width="99%"  RenderMode="Lightweight" EditModes="Design" SkinID="DefaultSetOfTools">
                        </telerik:RadEditor>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
