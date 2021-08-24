<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterAddressAssessmentOptionsAdd.aspx.cs" Inherits="Registers_RegisterAddressAssessmentOptionsAdd" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
     <div>
            <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
            <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand" Title=""></eluc:TabStrip>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblques" Text="Question" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:TextBox ID="txtquestion" runat="server" TextMode="MultiLine" Width="300px"></asp:TextBox></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOption" Text="Option" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtOptionName" runat="server" Width="300px"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblactive" Text="Is active" runat="server"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkactive" runat="server" /></td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
