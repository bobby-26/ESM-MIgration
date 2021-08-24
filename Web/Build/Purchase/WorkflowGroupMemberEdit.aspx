<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkflowGroupMemberEdit.aspx.cs" Inherits="WorkflowGroupMemberEdit" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Group" Src="~/UserControls/UserControlWFGroup.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Target" Src="~/UserControls/UserControlWFTarget.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Workflow - Group Member Edit</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
             <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuGroupMemberEdit" runat="server" OnTabStripCommand="MenuGroupMemberEdit_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            <table style="margin-left: 20px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel6" runat="server" Text="Group"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Group ID="UcGroup" runat="server" Width="220px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel7" runat="server" Text="Target"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Target ID="UcTarget" runat="server" Width="220px" />
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="User"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListFleetManager">
                            <telerik:RadTextBox ID="txtUserName" runat="server" CssClass="input_mandatory" MaxLength="160" AutoPostBack="true"
                                Width="280px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtuserDesignation" runat="server" CssClass="hidden" Enabled="false"
                                MaxLength="30" Width="5px" ReadOnly="true">
                            </telerik:RadTextBox>

                            <asp:LinkButton ID="cmdUser" runat="server" ImageAlign="AbsMiddle" Text=".."
                                OnClientClick="return showPickList('spnPickListFleetManager', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); ">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox runat="server" ID="txtuserid" CssClass="hidden"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtuserEmailHidden" CssClass="hidden" Width="5px"></telerik:RadTextBox>
                        </span>

                    </td>
                    
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
