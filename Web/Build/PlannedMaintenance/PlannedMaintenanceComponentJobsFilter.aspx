<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceComponentJobsFilter.aspx.cs" Inherits="PlannedMaintenanceComponentJobsFilter" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmComponentJobFilter" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuComponentFilter" runat="server" OnTabStripCommand="MenuComponentFilter_TabStripCommand"></eluc:TabStrip>
            </div>
        </telerik:RadCodeBlock>

        <br clear="all" />
        <asp:UpdatePanel runat="server" ID="pnlDiscussion">
            <ContentTemplate>
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td style="width: 20%">
                            <telerik:RadLabel ID="lblComponentNumber" runat="server" Text="Component Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCompNumber" runat="server" Width="180px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblComponentName" runat="server" Text="Component Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCompName" runat="server" Width="180px"></telerik:RadTextBox>
                        </td>
                    </tr>

                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblJobCode" runat="server" Text="Job Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtJobcode" runat="server" Width="180px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblJobTitle" runat="server" Text="Job Title"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtJobTitle" runat="server" Width="180px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Responsibility"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Discipline ID="ucDiscipline" runat="server" AppendDataBoundItems="true" Width="180px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                        </td>
                        <td valign="top">
                            <eluc:Number runat="server" ID="txtPriority" MaskText="#" Mask="9" CssClass="input" Width="60px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblcancelledjob" runat="server" Text="Cancelled Job"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadCheckBox ID="chkCancelledjob" runat="server"></telerik:RadCheckBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblClasscode" runat="server" Text="Class Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtClasscode" runat="server" Width="120px"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
