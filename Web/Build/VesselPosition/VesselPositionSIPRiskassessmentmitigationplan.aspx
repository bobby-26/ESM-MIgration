<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionSIPRiskassessmentmitigationplan.aspx.cs" Inherits="VesselPositionSIPRiskassessmentmitigationplan" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CustomEditor" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Risk assessment & mitigation plan</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSIPTanksConfuguration" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlVPRSLocation" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlSIPTanksConfuguration">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="TabRiskassessmentplan" Title="Risk assessment & mitigation plan" runat="server" OnTabStripCommand="TabRiskassessmentplan_TabStripCommand" TabStrip="true" />
            <eluc:TabStrip ID="MenuRiskassessmentplan" runat="server" OnTabStripCommand="MenuRiskassessmentplan_TabStripCommand" />


            <table id="tblSearch" width="100%" style="display: none;">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" SyncActiveVesselsOnly="True" AssignedVessels="true"
                            VesselsOnly="true" AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>

            <table width="100%">
                <tr>
                    <td style="width: 40%;">
                        <telerik:RadLabel ID="lblriskassest" runat="server" Text="Is risk assessment (impact of new fuels) performed?"></telerik:RadLabel></td>
                    <td style="width: 60%;">
                        <telerik:RadRadioButtonList ID="rdriskassesment" runat="server" Direction="Horizontal">
                            <Items>
                            <telerik:ButtonListItem Value="0" Text="Yes"></telerik:ButtonListItem>
                            <telerik:ButtonListItem Value="1" Text="No" Selected="True"></telerik:ButtonListItem>
                                </Items>
                        </telerik:RadRadioButtonList>
                        <asp:CheckBox ID="chkriskassest" runat="server" Visible="false" />
                    </td>

                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="Literal2" runat="server" Text="<b>Details :</b>"></telerik:RadLabel>&nbsp;&nbsp;&nbsp;
                    </td>
                </tr>
                <tr style="display: none;">
                    <td colspan="4">
                        <telerik:RadLabel ID="lbldetails" runat="server" Text="<b>Text</b>"></telerik:RadLabel>&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkRiskAssesment" runat="server" Text="<b>Documents</b>" OnClick="lnkRiskAssesment_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr>

                    <td colspan="4">
                        <telerik:RadTextBox ID="txtOfficeDescription" runat="server" CssClass="readonlytextbox" Enabled="false" Height="70px" TextMode="MultiLine" Resize="Both"
                            Width="98%" />
                    </td>
                </tr>
                <tr>

                    <td colspan="4">
                        <telerik:RadTextBox ID="txtDescription" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Resize="Both"
                            Width="98%" />
                    </td>
                </tr>

                <tr>
                    <td colspan="4" valign="middle">
                        <span id="spnRA">
                            <telerik:RadTextBox ID="txtRANumber" runat="server" CssClass="input" Enabled="false" MaxLength="50"
                                Width="100px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtRA" runat="server" CssClass="input" Enabled="false" MaxLength="50"
                                Width="120px"></telerik:RadTextBox>
                            <asp:LinkButton runat="server" AlternateText=".." ID="imgShowRA" ToolTip="Show RA">
                                <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                            <telerik:RadTextBox ID="txtRAId" runat="server" CssClass="input" MaxLength="20" Width="0px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtRaType" runat="server" CssClass="input" MaxLength="2" Width="0px"></telerik:RadTextBox>
                        </span>
                        <asp:LinkButton runat="server" AlternateText=".."  ID="cmdClear21" ToolTip="Clear" OnClick="cmdRAClear_Click">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                                </asp:LinkButton>
                        <asp:LinkButton runat="server" AlternateText=".." CommandName="RA"  ID="cmdRA" Visible="false" ToolTip="Show PDF">
                                <span class="icon"><i class="fas fa-file-pdf"></i></span>
                                </asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="blblinkedyn" runat="server" Text="Linked to onboard Safety Management System (SMS)?"></telerik:RadLabel></td>
                    <td>
                        <telerik:RadRadioButtonList ID="rdlinkedyn" runat="server" Direction="Horizontal">
                            <Items>
                            <telerik:ButtonListItem Value="0" Text="Yes"></telerik:ButtonListItem>
                            <telerik:ButtonListItem Value="1" Text="No" Selected="True"></telerik:ButtonListItem>
                                </Items>
                        </telerik:RadRadioButtonList>
                        <telerik:RadCheckBox ID="chklinkedyn" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="Literal1" runat="server" Text="<b>Upload relevant documents:</b>"></telerik:RadLabel>&nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="linkOnboardSafty" runat="server" Text="<b>onboard Safety Documents</b>" OnClick="linkOnboardSafty_Click"></asp:LinkButton>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
