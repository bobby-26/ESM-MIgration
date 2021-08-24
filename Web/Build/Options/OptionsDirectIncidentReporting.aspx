<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsDirectIncidentReporting.aspx.cs" Inherits="OptionsDirectIncidentReporting" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Report an Incident</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
        <table class="loginpagebackground" width="100%" align="center" cellpadding="0" cellspacing="0"
            height="60px">
            <tr>
                <td align="left" valign="top">
                    <font class="application_title"><telerik:RadCodeBlock ID="RadCodeBlock2" runat="server"><%=Application["softwarename"].ToString() %></telerik:RadCodeBlock></font>&nbsp;&nbsp;
                </td>
                <td align="right" valign="top">
                    <font class="loginpage_companyname"><b><asp:Literal ID="lblManagement" runat="server"></asp:Literal></b></font>
                    &nbsp;&nbsp;<img id="Img1" runat="server" style="vertical-align: top" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>"
                        alt="Phoenix" />&nbsp;
                </td>
            </tr>
        </table>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:TabStrip ID="MenuSecurityUser" runat="server" Title="Open Report"></eluc:TabStrip>
        <table cellpadding="4" style="width: 672px">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                </td>
                <td colspan="3">
                    <eluc:Vessel ID="ddlVessel" runat="server" Width="180px" CssClass="input_mandatory" AppendDataBoundItems="true" SyncActiveVesselsOnly="true" ActiveVesselsOnly="true" Entitytype="VSL" VesselsOnly="true" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <telerik:RadLabel ID="lblHeader" runat="server" Font-Bold="true" Text="Comprehensive Description of Near Miss"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <telerik:RadTextBox ID="txtInvestigationAndEvidence" Resize="Both" runat="server" CssClass="input_mandatory"
                        Height="425px" Rows="50" TextMode="MultiLine" Width="100%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblRank" runat="server" Text="Rank" Visible="false"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtRank" runat="server" CssClass="input_mandatory" Visible="false" Width="80%"></telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblName" runat="server" Text="Name" Visible="false"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtName" runat="server" CssClass="input_mandatory" Visible="false" Width="80%"></telerik:RadTextBox>
                </td>
            </tr>
            <%--<tr>
                <td colspan="4">
                    <font color="blue"><b><telerik:RadLabel ID="lblNote" runat="server" Visible="false" Text="Note: Entry of Rank and Name is at your option, and not mandatory."></telerik:RadLabel></b></font>
                </td>
            </tr>--%>
            <tr>
                <td></td>
                <td style="float: right;">
                    <telerik:RadButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click" CssClass="cntxMenuSelect" />
                    <telerik:RadButton ID="btnHome" runat="server" Text="Home" OnClick="btnHome_Click" CssClass="cntxMenuSelect" />
                </td>
            </tr>
        </table>
        <input type="button" runat="server" id="isouterpage" name="isouterpage" style="visibility: hidden" />
        <asp:Button ID="ucConfirm" runat="server" OnClick="ucConfirm_Click" CssClass="hidden" />
    </form>
</body>
</html>

