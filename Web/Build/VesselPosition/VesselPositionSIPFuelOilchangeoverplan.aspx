<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionSIPFuelOilchangeoverplan.aspx.cs" Inherits="VesselPositionSIPFuelOilchangeoverplan" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

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
    <title>Fuel oil changeover plan</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
            <eluc:TabStrip ID="TabRiskassessmentplan" Title="Fuel oil changeover plan" runat="server" OnTabStripCommand="TabRiskassessmentplan_TabStripCommand" TabStrip="true" />
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
                    <td colspan="2" style="width: 50%;">
                        <telerik:RadLabel ID="lblstsyn" runat="server" Text="Is a ship-specific fuel changeover plan available?"></telerik:RadLabel>
                    </td>
                    <td colspan="2" style="width: 50%;">
                        <telerik:RadRadioButtonList ID="rdstsyn" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Value="0" Text="Yes"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Value="1" Text="No"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Value="2" Text="NA" Selected="True"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                        <telerik:RadCheckBox ID="chkstsyn" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblFuelChangeover" runat="server" Text="<b>Text</b>"></telerik:RadLabel>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="linkFuelChangeover" runat="server" Text="<b>Documents</b>" OnClick="linkFuelChangeover_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr id="trOfficestsdetail" runat="server">
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtOfficestsdetail" runat="server" CssClass="readonlytextbox" Enabled="false" Height="70px" TextMode="MultiLine" Rows="3"
                            Width="98%" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtstsdetail" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Rows="4"
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
                    <td colspan="2" style="width: 50%;">
                        <telerik:RadLabel ID="lblmaxperiodreq" runat="server" Text="Maximum period required to changeover fuel at all combustion units:"></telerik:RadLabel>
                    </td>
                    <td colspan="2" style="width: 50%;">
                        <telerik:RadTextBox ID="txtmaxperiodreq" runat="server" CssClass="input" Width="180px" /></td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 50%;">
                        <telerik:RadLabel ID="lblETDChangeoverDate" runat="server" Text="Expected date of completion of changeover procedure:"></telerik:RadLabel>
                    </td>
                    <td colspan="2" style="width: 50%;">
                        <eluc:Date ID="UcETDChangeoverDate" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 50%;">
                        <telerik:RadLabel ID="lbltrainingreq" runat="server" Text="Is training of officers and crew needed?"></telerik:RadLabel>
                    </td>
                    <td colspan="2" style="width: 50%;">
                        <telerik:RadRadioButtonList ID="rdtrainingreq" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Value="0" Text="Yes"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Value="1" Text="No"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Value="2" Text="NA" Selected="True"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                        <telerik:RadCheckBox ID="chktrainingreq" runat="server" Visible="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lbltrainingdetails" runat="server" Text="<b>Training details:</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadLabel ID="lblTraining" runat="server" Text="<b>Text</b>"></telerik:RadLabel>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkTraining" runat="server" Text="<b>Documents</b>" OnClick="lnkTraining_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr id="tr1" runat="server">
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtOfficetrainingdetails" runat="server" CssClass="readonlytextbox" Enabled="false" Height="70px" TextMode="MultiLine" Rows="3"
                            Width="98%" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txttrainingdetails" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Rows="4"
                            Width="98%" />
                    </td>
                </tr>
                <tr style="display: none;">
                    <td colspan="4">
                        <telerik:RadLabel ID="Literal2" runat="server" Text="<b>Documents:</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td colspan="4">
                        <span id="spnPickListDocument2">
                            <telerik:RadTextBox ID="txtDocumentName2" runat="server" Width="338px" Enabled="False" Style="font-weight: bold"
                                CssClass="input">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowDocuments2" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtDocumentId2" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <asp:LinkButton ID="lnkFormAdd2" runat="server" OnClick="lnkFormAdd2_Click" Text="Add"></asp:LinkButton>
                        <br />
                        <div id="divForms2" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 360px; border-width: 1px; border-style: solid; border: 1px solid #9f9fff">
                            <table id="tblForms2" runat="server">
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
