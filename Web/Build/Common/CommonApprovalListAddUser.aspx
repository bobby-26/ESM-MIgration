<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonApprovalListAddUser.aspx.cs"
    Inherits="CommonApprovalListAddUser" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add User</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmApproval" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <eluc:error id="ucError" runat="server" text="" visible="false">
        </eluc:error>
        <eluc:status runat="server" id="ucStatus" />
        <eluc:tabstrip id="MenuBack" runat="server" ontabstripcommand="MenuBack_TabStripCommand">
        </eluc:tabstrip>
        <eluc:tabstrip id="MenuAddUser" runat="server" ontabstripcommand="MenuAddUser_TabStripCommand">
        </eluc:tabstrip>
        <table cellpadding="5">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblApprovalName" runat="server" Text="Approval Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtApprovalType" runat="server" CssClass="readonlytextbox" ReadOnly="true" Visible="false"
                        Width="300px">
                    </telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtApprovalName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                        Width="300px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblusername" runat="server" Text="User Name"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnUserName">
                        <telerik:RadTextBox ID="txtUserCode" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtUserName" runat="server" Width="300px" CssClass="input_mandatory"></telerik:RadTextBox>
                        <asp:ImageButton ID="cmdShowUser" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnUserName', 'codehelp1', '', '../Common/CommonPickListUserName.aspx', true);"
                            Text=".." />
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lbldesignation" runat="server" Text="Designation"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtdesignation" runat="server" MaxLength="100" Width="300px" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblemail" runat="server" Text="E-Mail"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtemail" runat="server" MaxLength="100" Width="300px" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblonbehalf1" runat="server" Text="On Behalf1"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnonbehalf1">
                        <telerik:RadTextBox ID="txtonbehalf1usercode" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtonbehalf1username" runat="server" Width="300px" CssClass="input"></telerik:RadTextBox>
                        <asp:ImageButton ID="imgonbehalf1" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnonbehalf1', 'codehelp1', '', '../Common/CommonPickListUserName.aspx', true);"
                            Text=".." />

                    </span>&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblonbehalf2" runat="server" Text="On Behalf2"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnonbehalf2">
                        <telerik:RadTextBox ID="txtonbehalf2usercode" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>

                        <telerik:RadTextBox ID="txtonbehalf2username" runat="server" Width="300px" CssClass="input"></telerik:RadTextBox>
                        <asp:ImageButton ID="imgonbehalf2" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                            ImageAlign="AbsMiddle" OnClientClick="return showPickList('spnonbehalf2', 'codehelp1', '', '../Common/CommonPickListUserName.aspx', true);"
                            Text=".." />

                    </span>&nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblsequence" runat="server" Text="Sequence"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtsequence" runat="server" MaxLength="100" Width="300px" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblset" runat="server" Text="Set"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtset" runat="server" MaxLength="100" Width="300px" CssClass="input"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblsendemail" runat="server" Text="Send Email"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chksendemail" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblstatus" runat="server" Text="Status"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:hard id="ddlStatusAdd" runat="server" cssclass="input_mandatory" appenddatabounditems="true" AutoPostBack="false" Width="300px"
                        hardtypecode="99" hardlist='<%#PhoenixRegistersHard.ListHard(PhoenixSecurityContext.CurrentSecurityContext.UserCode, 99) %>' />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblproceed" runat="server" Text="Proceed"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkproceed" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblhideonbehalf" runat="server" Text="Hide On Behalf"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadCheckBox ID="chkhideonbehalf" runat="server" />
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
