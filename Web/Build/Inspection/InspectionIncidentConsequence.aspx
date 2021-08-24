<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentConsequence.aspx.cs" Inherits="InspectionIncidentConsequence" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Incident Consequence</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <style type="text/css">
        .RadCheckBox {
            width: 99% !important;
        }
        .rbText {
            text-align: left;
            width: 89% !important;
        }

        .rbVerticalList {
            width: 32% !important;
        }
    </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmIncidentDamageSearch" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <%--TabStrip Binding Again--%>
            <%--<eluc:TabStrip ID="MenuIncidentGeneral" runat="server" TabStrip="true" OnTabStripCommand="IncidentGeneral_TabStripCommand"></eluc:TabStrip>--%>
            <eluc:TabStrip ID="MenuIncidentConsequence" TabStrip="false" runat="server" OnTabStripCommand="MenuIncidentConsequence_TabStripCommand" Title="Other Consequence"></eluc:TabStrip>
            <table id="tbldiv" runat="server" cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                       
                    </td>
                </tr>
            </table>

            <table id="tblConsequence" runat="server" width="100%">
                <tr>
                    <td colspan="4">
                        <span style="color: Black; font-weight: bold">
                            <telerik:RadLabel ID="lblPropertyDamage" runat="server" Text="Property Damage"></telerik:RadLabel></span>
                        <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblDetails" runat="server" Text="Details"></telerik:RadLabel>
                    </td>
                    <td style="width: 40%">
                        <telerik:RadTextBox ID="txtDetails" runat="server" CssClass="input" Width="300px" TextMode="MultiLine"
                            Rows="3" Height="70px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>
                    </td>
                    <td style="width: 40%">
                        <span id="spnPickListComponent">
                            <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="input" Enabled="false"
                                MaxLength="20" Width="80px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input" Enabled="false"
                                MaxLength="20" Width="260px"></telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgComponent" >
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="imgClearParentComponent" ToolTip="Clear Value"
                                OnClick="ClearComponent">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="hidden" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadCheckBoxList ID="cblPropertyDamage" runat="server" Columns="3">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                        <span style="color: Black; font-weight: bold">
                            <telerik:RadLabel ID="lblProcessLoss" runat="server" Text="Process Loss"></telerik:RadLabel></span>
                        <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadCheckBoxList ID="cblProcessLoss" runat="server" Columns="3">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblnumberofhourslost" runat="server" Text="Number of Hours lost"></telerik:RadLabel>
                    </td>
                    <td style="width: 40%">
                        <eluc:Number ID="txtnumberofhourslost" runat="server" CssClass="input txtNumber" MaxLength="5" Tooltip="Enter the Number of Hours lost in off hire cases under incidents" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                        <span style="color: Black; font-weight: bold">
                            <telerik:RadLabel ID="lblSecurity" runat="server" Text="Security"></telerik:RadLabel></span>
                        <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadCheckBoxList ID="cblSecurity" runat="server" Columns="3">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <br />
                        <span style="color: Black; font-weight: bold">
                            <telerik:RadLabel ID="lblEnvironmental" runat="server" Text="Environmental"></telerik:RadLabel></span>
                        <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblNameofSubstance" runat="server" Text="Name of Substance"></telerik:RadLabel>
                    </td>
                    <td style="width: 40%">
                        <telerik:RadTextBox ID="txtSubstance" runat="server" CssClass="input" Width="300px" TextMode="MultiLine"
                            Rows="3" Height="70px"></telerik:RadTextBox>
                    </td>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblCargo" runat="server" Text="Cargo Y/N"></telerik:RadLabel>
                    </td>
                    <td style="width: 40%">
                        <telerik:RadCheckBox ID="chkCargo" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4"></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadCheckBoxList ID="cblEnvironmental" runat="server" Columns="3">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
