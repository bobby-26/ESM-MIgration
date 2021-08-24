<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPEARSRiskAssessmentActivityStepsAdd.aspx.cs" Inherits="InspectionPEARSRiskAssessmentActivityStepsAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>

<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .ddlstyle {                
                text-align:right;
                align-items:center;
            }
            .checkboxstyle tbody tr td {
                width: 550px;
                vertical-align: top;                
            }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="100%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblGeneric" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuActivity" runat="server" OnTabStripCommand="MenuActivity_TabStripCommand"></eluc:TabStrip>
            <table width="100%" cellspacing="3" id="tblGeneric" runat="server" >
                <tr valign="top">
                    <td width="15%">
                        <telerik:RadLabel ID="lblActivityStep" runat="server" Text="Activity Steps"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <telerik:RadTextBox ID="txtActivityStep" runat="server" Text="" TextMode="MultiLine" MaxLength="500" Resize="Both" Width="98%" Height="60px" CssClass="input" ></telerik:RadTextBox>
                    </td>
                    <td width="15%">
                        <telerik:RadLabel ID="lblHazardDesc" runat="server" Text="Hazard Description (Aspect)"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <telerik:RadTextBox ID="txtHazardDesc" runat="server" Text="" TextMode="MultiLine" MaxLength="500" Resize="Both" Width="98%" Height="60px" CssClass="input" ></telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblImpact" runat="server" Text="Hazard Effect (Impact)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtImpact" runat="server" Text="" TextMode="MultiLine" MaxLength="500" Resize="Both" Width="98%" Height="160px" CssClass="input"></telerik:RadTextBox>
                    </td>                    
                    <td>
                        <telerik:RadLabel ID="lblPersons" runat="server" Text="Person Involved"></telerik:RadLabel>
                    </td>
                    <td >
                         <div id="divpersonsinvolved" runat="server" style="height: 160px; overflow-y: auto; overflow-x: auto; width: 98%; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <asp:CheckBoxList ID="ChkgroupMem" runat="server" RepeatDirection="Horizontal" RepeatLayout="Table" RepeatColumns="2" AutoPostBack="false" >
                            </asp:CheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblExisting" runat="server" Text="Existing Controls"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtExisting" runat="server" Text="" TextMode="MultiLine" MaxLength="500" Resize="Both" Width="98%" Height="175px" CssClass="input" ></telerik:RadTextBox>
                    </td>
                    <td colspan="2">
                        <div id="divInit" runat="server" style="border: solid; border-width: thin; border-color:#c3cedd;">
                            <table width="100%" CellSpacing="2" align="Center">
                                <tr>
                                    <td colspan="4" align="center">
                                        <telerik:RadLabel ID="lblinit" runat="server" Font-Bold="true" Text="Initial Risk"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <th width="30%" align="Left">
                                        <telerik:RadLabel ID="lblHazard" runat="server" Text="Hazard Category"></telerik:RadLabel>
                                    </th>
                                    <th width="30%">
                                        <telerik:RadLabel ID="lblSeverity" runat="server" Text="Hazard Severity"></telerik:RadLabel>
                                    </th>
                                    <th width="30%">
                                        <telerik:RadLabel ID="lblLOH" runat="server" Text="Likelihood"></telerik:RadLabel>
                                    </th>
                                    <th width="10%">
                                        <telerik:RadLabel ID="lblRating" runat="server" Text="Rating"></telerik:RadLabel>
                                    </th>
                                </tr>
                                <tr>
                                    <td align="Left">
                                        <telerik:RadLabel ID="lblinitPpl" runat="server" Text="People"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlinitpplSeverity" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDSEVERITYID" Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlinitpplLOH" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDPEARSRISKASSESSMENTLOHID"  Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinitPpl" runat="server" Text="" ReadOnly="true" Width="90%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="Left">
                                        <telerik:RadLabel ID="lblinitEnv" runat="server" Text="Environment"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlinitEnvSeverity" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDSEVERITYID"  Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlinitEnvLOH" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDPEARSRISKASSESSMENTLOHID"  Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinitEnv" runat="server" Text="" ReadOnly="true" Width="90%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr >
                                    <td align="Left">
                                        <telerik:RadLabel ID="lblinitAst" runat="server" Text="Asset"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlinitAstSeverity" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDSEVERITYID" Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlinitAstLOH" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDPEARSRISKASSESSMENTLOHID" Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinitAst" runat="server" Text="" ReadOnly="true" Width="90%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr >
                                    <td align="Left">
                                        <telerik:RadLabel ID="lblinitRep" runat="server" Text="Reputation"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlinitRepSeverity" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDSEVERITYID"  Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlinitRepLOH" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDPEARSRISKASSESSMENTLOHID"  Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinitRep" runat="server" Text="" ReadOnly="true" Width="90%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr >
                                    <td align="Left">
                                        <telerik:RadLabel ID="lblinitSdl" runat="server" Text="Schedule"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlinitSdlSeverity" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDSEVERITYID" Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlinitSdlLOH" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDPEARSRISKASSESSMENTLOHID" Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtinitSdl" runat="server" Text="" ReadOnly="true" Width="90%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblAdditional" runat="server" Text="Additional Controls"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAdditional" runat="server" Text="" TextMode="MultiLine" MaxLength="500" Resize="Both" CssClass="input" Width="98%" Height="175px"></telerik:RadTextBox>
                    </td>
                    <td colspan="2">
                        <div id="devRes" runat="server" style="border: solid; border-width: thin; border-color:#c3cedd;">
                            <table width="100%" CellSpacing="2" align="center">
                                <tr>
                                    <td colspan="4" align="center">
                                        <telerik:RadLabel ID="lblRes" runat="server" Font-Bold="true" Text="Residual Risk"></telerik:RadLabel>
                                    </td>
                                </tr>
                                <tr>
                                    <th width="30%" align="Left">
                                        <telerik:RadLabel ID="lblResHazard" runat="server" Text="Hazard Category"></telerik:RadLabel>
                                    </th>
                                    <th width="30%">
                                        <telerik:RadLabel ID="lblResSeverity" runat="server" Text="Hazard Severity"></telerik:RadLabel>
                                    </th>
                                    <th width="30%">
                                        <telerik:RadLabel ID="lblResLOH" runat="server" Text="Likelihood"></telerik:RadLabel>
                                    </th>
                                    <th width="10%">
                                        <telerik:RadLabel ID="txtResRating" runat="server" Text="Rating"></telerik:RadLabel>
                                    </th>
                                </tr>
                                <tr>
                                    <td align="Left">
                                        <telerik:RadLabel ID="lblResPpl" runat="server" Text="People"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlResPplSeverity" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDSEVERITYID"  Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlResPplLOH" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDPEARSRISKASSESSMENTLOHID"  Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtResPpl" runat="server" Text="" ReadOnly="true" Width="90%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="Left">
                                        <telerik:RadLabel ID="lblResEnv" runat="server" Text="Environment"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlResEnvSeverity" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDSEVERITYID"  Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlResEnvLOH" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDPEARSRISKASSESSMENTLOHID"  Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtResEnv" runat="server" Text="" ReadOnly="true" Width="90%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="Left">
                                        <telerik:RadLabel ID="lblResAst" runat="server" Text="Asset"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlResAstSeverity" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDSEVERITYID"  Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlResAstLOH" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDPEARSRISKASSESSMENTLOHID"  Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtResAst" runat="server" Text="" ReadOnly="true" Width="90%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="Left">
                                        <telerik:RadLabel ID="lblResRep" runat="server" Text="Reputation"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlResRepSeverity" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDSEVERITYID"  Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlResRepLOH" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDPEARSRISKASSESSMENTLOHID"  Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtResRep" runat="server" Text="" ReadOnly="true" Width="90%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="Left">
                                        <telerik:RadLabel ID="lblResSdl" runat="server" Text="Schedule"></telerik:RadLabel>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlResSdlSeverity" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDSEVERITYID"  Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadComboBox ID="ddlResSdlLOH" runat="server" InputCssClass="ddlstyle" AutoPostBack="true" DataTextField="FLDSCORE" DataValueField="FLDPEARSRISKASSESSMENTLOHID"  Width="70px"></telerik:RadComboBox>
                                    </td>
                                    <td>
                                        <telerik:RadTextBox ID="txtResSdl" runat="server" Text="" ReadOnly="true" Width="90%"></telerik:RadTextBox>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
