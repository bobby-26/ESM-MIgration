<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionSIPDocumentationandReporting.aspx.cs" Inherits="VesselPositionSIPDocumentationandReporting" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

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
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Documentation & reporting</title>
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
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlSIPTanksConfuguration" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlSIPTanksConfuguration">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenPick_Click" />
            <eluc:TabStrip ID="TabRiskassessmentplan" Title="Documentation & reporting" runat="server" OnTabStripCommand="TabRiskassessmentplan_TabStripCommand" TabStrip="true" />
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
                    <td colspan="4">
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="Literal3" runat="server" Text="<b>Shipboard fuel oil tank management plans: </b>"></telerik:RadLabel>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinShipboard" runat="server" Text="<b>Documents</b>" OnClick="LinShipboard_Click"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="Literal4" runat="server" Text="<b>Stability booklets: </b>"></telerik:RadLabel>
                                </td>
                                <td>
                                    <asp:LinkButton ID="Linkstablity" runat="server" Text="<b>Documents</b>" OnClick="Linkstablity_Click"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="Literal6" runat="server" Text="<b>Trim booklets: </b>"></telerik:RadLabel>

                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkTrimbook" runat="server" Text="<b>Documents</b>" OnClick="LinkTrimbook_Click"></asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="Literal5" runat="server" Text="<b>Other: </b>"></telerik:RadLabel>
                                </td>
                                <td>
                                    <asp:LinkButton ID="LinkOthers" runat="server" Text="<b>Documents</b>" OnClick="LinkOthers_Click"></asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmodification" runat="server" Text="If there are any modifications planned to the fuel oil system, related documents should be consequently updated."></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblimplimentaion" runat="server" Text="If when following the implementation plan the ship has to bunker and use non-compliant fuel oil due to unavailability of compliant fuel oil safe for use on board the ship, steps to limit the impact of using non-compliant fuel oil could be:"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblFuelNonAvail" runat="server" Text="<b>Text</b>"></telerik:RadLabel>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkFuelNonAvail" runat="server" Text="<b>Documents</b>" OnClick="lnkFuelNonAvail_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr id="trOfficenoncomplientfueldetail" runat="server">
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtOfficenoncomplientfueldetail" runat="server" CssClass="readonlytextbox" Enabled="false" Height="70px" TextMode="MultiLine" Rows="3"
                            Width="98%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadTextBox ID="txtnoncomplientfueldetail" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Rows="4"
                            Width="98%" />
                    </td>
                </tr>
                <tr style="display: none;">
                    <td colspan="4">
                        <telerik:RadLabel ID="Literal1" runat="server" Text="<b>Documents:</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td colspan="4">
                        <span id="spnPickListDocument">
                            <telerik:RadTextBox ID="txtDocumentName" runat="server" Width="338px" Enabled="False" Style="font-weight: bold"
                                CssClass="input">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowDocuments" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtDocumentId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <asp:LinkButton ID="lnkFormAdd" runat="server" OnClick="lnkFormAdd_Click" Text="Add"></asp:LinkButton>
                        <br />
                        <div id="divForms" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 360px; border-width: 1px; border-style: solid; border: 1px solid #9f9fff">
                            <table id="tblForms" runat="server">
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblfuelavailablitydetail" runat="server" Text="The ship should have a procedure for Fuel Oil Non-Availability Reporting (FONAR). The master and chief engineer should be conversant about when and how FONAR should be used and who it should be reported to."></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <span id="spnPickListDocument2">
                            <telerik:RadTextBox ID="txtDocumentName2" runat="server" Width="338px" Enabled="False" Style="font-weight: bold"
                                CssClass="input">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" AlternateText=".." ID="btnShowDocuments2" ToolTip="Pick Document">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>

                            <telerik:RadTextBox ID="txtDocumentId2" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <asp:LinkButton ID="lnkFormAdd2" runat="server" OnClick="lnkFormAdd2_Click" AlternateText="Add" ToolTip="Add">
                            <span class="icon"><i class="fa fa-plus-circle"></i></span>
                        </asp:LinkButton>
                        <br />
                        <div id="divForms2" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 360px; border-width: 1px; border-style: solid; border: 1px solid #9f9fff">
                            <table id="tblForms2" runat="server">
                            </table>
                        </div>
                    </td>
                </tr>
                <tr id="tr1" runat="server">
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtOffiefuelavailablitydetail" runat="server" CssClass="readonlytextbox" Enabled="false" Height="70px" TextMode="MultiLine" Rows="3"
                            Width="98%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadTextBox ID="txtfuelavailablitydetail" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Rows="4"
                            Width="98%" />
                    </td>
                </tr>

                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblother" runat="server" Text="<b>Other Relevant Documents : </b>"></telerik:RadLabel>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="LinOtherDoc" runat="server" Text="<b>Documents</b>" OnClick="LinOtherDoc_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td colspan="4">
                        <telerik:RadLabel ID="Literal2" runat="server" Text="<b>Documents:</b>"></telerik:RadLabel>
                    </td>
                </tr>

            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
