<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsDirectIncidentNearmissReporting.aspx.cs"
    Inherits="OptionsDirectIncidentNearmissReporting" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
            height="10px">
            <tr>
                <td align="left" valign="top">
                    <font class="application_title"><telerik:RadLabel ID="lblPhoenix" runat="server" Text="Phoenix"></telerik:RadLabel></font>&nbsp;&nbsp;
                </td>
                <td align="right" valign="top">
                    <font class="loginpage_companyname"><b><telerik:RadLabel ID="lblManagement" runat="server"></telerik:RadLabel></b></font>
                    &nbsp;&nbsp;<img id="Img1" runat="server" style="vertical-align: top" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>"
                        alt="Phoenix" />&nbsp;
                </td>
            </tr>
        </table>
            <eluc:TabStrip ID="MenuInspectionUnSafe" runat="server"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <table cellpadding="4" style="width: 672px">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="240px" SyncActiveVesselsOnly="true" ActiveVesselsOnly="true" Entitytype="VSL" VesselsOnly="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLocation" runat="server" Text="Location"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtLocation" runat="server" CssClass="input_mandatory" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDate" runat="server" DatePicker="true" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTime" runat="server" Text="Time"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTimePicker ID="txtTimeOfIncident" runat="server" Width="80px" CssClass="input_mandatory"></telerik:RadTimePicker>
                        <telerik:RadLabel ID="lblhrs" runat="server" Text="hrs"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucCategory" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" HardTypeCode="208" AutoPostBack="true" OnTextChangedEvent="ucCategory_TextChanged"
                             Width="240px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSubcategory" runat="server" Text="Sub-category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlSubcategory" runat="server" CssClass="input_mandatory" Width="240px" DataTextField="FLDIMMEDIATECAUSE" DataValueField="FLDIMMEDIATECAUSEID"
                            Filter="Contains">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblHeader" runat="server" Font-Bold="true" Text="Comprehensive Description of Near Miss"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtInvestigationAndEvidence" runat="server" CssClass="input_mandatory"
                            Height="300px" Rows="50" TextMode="MultiLine" Width="100%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRank" runat="server" CssClass="input" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" CssClass="input" Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblNote" ForeColor="Blue" Font-Bold="true" runat="server" Text="Note: Entry of Rank and Name is at your option, and not mandatory."></telerik:RadLabel>
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
            <asp:Button ID="ucConfirm" runat="server" OnClick="btnConfirm_Click" CssClass="hidden" />
    </form>
</body>
</html>
