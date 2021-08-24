<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsOpenReporting.aspx.cs" Inherits="OptionsOpenReporting" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Report an Incident</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
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
            <eluc:TabStrip ID="MenuSecurityUser" runat="server" Title="Details of Report"></eluc:TabStrip>
            <table cellpadding="4" style="width: 672px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input_mandatory"
                            AppendDataBoundItems="true" AutoPostBack="true" OnTextChangedEvent="ddlVessel_Changed" Width="180px" SyncActiveVesselsOnly="true" ActiveVesselsOnly="true" Entitytype="VSL" VesselsOnly="true" />
                        <eluc:Company ID="ucCompany" runat="server" Enabled="false" CssClass="input" AppendDataBoundItems="true" Width="180px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblHeader" runat="server" Font-Bold="true" Text="Comprehensive Description of Near Miss"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtInvestigationAndEvidence" runat="server" CssClass="input_mandatory"
                            Height="425px" Rows="50" TextMode="MultiLine" Resize="Both" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td style="float: right;">
                        <telerik:RadButton ID="btnSubmit" runat="server" Text="Submit" OnClick="btnSubmit_Click"
                            CssClass="cntxMenuSelect" />
                        <telerik:RadButton ID="btnHome" runat="server" Text="Home" OnClick="btnHome_Click" CssClass="cntxMenuSelect" />
                    </td>
                </tr>
            </table>
            <input type="button" runat="server" id="isouterpage" name="isouterpage" style="visibility: hidden" />
            <asp:Button ID="ucConfirm" runat="server" OnClick="ucConfirm_Click" CssClass="hidden" />
            <eluc:Error ID="ucError" runat="server" Text="" CssClass="hidden"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" CssClass="hidden" />
    </form>
</body>
</html>
