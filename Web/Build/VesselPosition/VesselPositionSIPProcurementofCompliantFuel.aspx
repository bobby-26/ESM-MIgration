<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionSIPProcurementofCompliantFuel.aspx.cs" Inherits="VesselPositionSIPProcurementofCompliantFuel" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

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
    <title>Procurement of compliant fuel oil</title>
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
            <eluc:TabStrip ID="TabRiskassessmentplan" Title="Procurement of compliant fuel oil" runat="server" OnTabStripCommand="TabRiskassessmentplan_TabStripCommand" TabStrip="true" />

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

            <br />
            <table width="100%">
                <tr>
                    <td colspan="6">
                        <telerik:RadLabel ID="lblriskassest" runat="server" Text="<b>Details of fuel purchasing procedure to source compliant fuels, including procedures in cases where compliant fuel oil is not readily available</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadLabel ID="lblPurchasedetails" runat="server" Text="<b>Text</b>"></telerik:RadLabel>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkPurchasedetails" runat="server" Text="<b>Documents</b>" OnClick="lnkPurchasedetails_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr id="trOfficeComment" runat="server">
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtOfficepurchasedetail" runat="server" CssClass="readonlytextbox" Enabled="false" Height="70px" TextMode="MultiLine" Resize="Both" Rows="3"
                            Width="98%" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtpurchasedetail" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Resize="Both" Rows="4" Width="98%" /></td>
                </tr>
                <tr style="display: none;">
                    <td colspan="6">
                        <telerik:RadLabel ID="Literal1" runat="server" Text="<b>Documents:</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td colspan="6">
                        <span id="spnPickListDocument">
                            <telerik:RadTextBox ID="txtDocumentName" runat="server" Width="338px" Enabled="False" Style="font-weight: bold"
                                CssClass="input">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowDocuments" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtDocumentId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <asp:LinkButton ID="lnkFormAdd" runat="server"  Text="Add"></asp:LinkButton>
                        <br />
                        <div id="divForms" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 360px; border-width: 1px; border-style: solid; border: 1px solid #9f9fff">
                            <table id="tblForms" runat="server">
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" style="width: 50%;">
                        <telerik:RadLabel ID="lblfirstbunkerdate" runat="server" Text="Estimated date for first bunkering of compliant fuel oil, not later than 24:00 hrs, 31 December 2019"></telerik:RadLabel>
                    </td>
                    <td colspan="3" align="left" style="width: 50%;">
                        <eluc:Date ID="UcFirstbunkeringdate" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <telerik:RadLabel ID="lblchartererresponsibleyn" runat="server" Text="Is charterer responsible for fuel?"></telerik:RadLabel>
                    </td>
                    <td colspan="3" align="left">
                        <telerik:RadRadioButtonList ID="rdchartererresponsibleyn" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Value="0" Text="Yes"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Value="1" Text="No" Selected="True"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                        <asp:CheckBox ID="chkchartererresponsibleyn" runat="server" AutoPostBack="false" OnCheckedChanged="chkchartererresponsibleyn_CheckedChanged" Visible="false" />
                    </td>
                </tr>
                <tr id="tracceptcharterer" runat="server">
                    <td colspan="3">
                        <telerik:RadLabel ID="lblacceprchartereyn" runat="server" Text="Accept charter party contracts that don't have specified obligation to provide compliant fuel after 01 june 2019 or other date to be identified:?"></telerik:RadLabel>
                    </td>
                    <td colspan="3" align="left">
                        <telerik:RadRadioButtonList ID="rdacceprchartereyn" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Value="0" Text="Yes"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Value="1" Text="No" Selected="True"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                        <asp:CheckBox ID="chkacceprchartereyn" runat="server" AutoPostBack="true" OnCheckedChanged="chkacceprchartereyn_CheckedChanged" Visible="false" />
                    </td>
                </tr>
                <tr id="tralternaternatehead" runat="server">
                    <td colspan="6">
                        <telerik:RadLabel ID="lblalternatestepdetails" runat="server" Text="<b>Details of alternate steps taken:</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadLabel ID="lblalternatestep" runat="server" Text="<b>Text</b>"></telerik:RadLabel>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="lnkAlternatestep" runat="server" Text="<b>Documents</b>" OnClick="lnkAlternatestep_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr id="tr1" runat="server">
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtOfficealternatestepdetails" runat="server" CssClass="readonlytextbox" Enabled="false" Height="70px" TextMode="MultiLine" Resize="Both" Rows="3"
                            Width="98%" />
                    </td>
                </tr>
                <tr id="tralternaternateDetail" runat="server">
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtalternatestepdetails" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Resize="Both" Rows="4"
                            Width="98%" />
                    </td>
                </tr>
                <tr style="display: none;">
                    <td colspan="6">
                        <telerik:RadLabel ID="Literal2" runat="server" Text="<b>Documents:</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr id="tralternaternateDocument" runat="server" style="display: none;">
                    <td colspan="6">
                        <span id="spnPickListDocument2">
                            <telerik:RadTextBox ID="txtDocumentName2" runat="server" Width="338px" Enabled="False" Style="font-weight: bold"
                                CssClass="input">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowDocuments2" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtDocumentId2" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <asp:LinkButton ID="lnkFormAdd2" runat="server"  Text="Add"></asp:LinkButton>
                        <br />
                        <div id="divForms2" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 360px; border-width: 1px; border-style: solid; border: 1px solid #9f9fff">
                            <table id="tblForms2" runat="server">
                            </table>
                        </div>
                    </td>
                </tr>
                <tr id="trconfirmationsupplier" runat="server">
                    <td colspan="3">
                        <telerik:RadLabel ID="lblconfirmfromsupplier" runat="server" Text="Is there confirmation from bunker supplier(s) to provide compliant fuel oil on the specified date?"></telerik:RadLabel>
                    </td>
                    <td colspan="3" align="left">
                        <telerik:RadRadioButtonList ID="rdconfirmfromsupplier" runat="server" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Value="0" Text="Yes"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Value="1" Text="No" Selected="True"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                        <asp:CheckBox ID="chkconfirmfromsupplier" runat="server" AutoPostBack="false" OnCheckedChanged="chkconfirmfromsupplier_CheckedChanged" Visible="false" />
                    </td>
                </tr>
                <tr id="tralternatedetailHead" runat="server">
                    <td colspan="6">
                        <telerik:RadLabel ID="lblalternetstepdetail" runat="server" Text="<b>Details of alternate steps taken to ensure timely availability of compliant fuel oil:</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadLabel ID="lblArrangements" runat="server" Text="<b>Text</b>"></telerik:RadLabel>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="linkArrangements" runat="server" Text="<b>Documents</b>" OnClick="linkArrangements_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr id="tr2" runat="server">
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtOfficealternetstepavailablity" runat="server" Height="70px" CssClass="readonlytextbox" Enabled="false" TextMode="MultiLine" Resize="Both" Rows="4"
                            Width="98%" />
                    </td>
                </tr>
                <tr id="tralternatedetail" runat="server">

                    <td colspan="6">
                        <telerik:RadTextBox ID="txtalternetstepavailablity" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Resize="Both" Rows="4"
                            Width="98%" />
                    </td>
                </tr>
                <tr style="display: none;">
                    <td colspan="6">
                        <telerik:RadLabel ID="Literal3" runat="server" Text="<b>Documents:</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td colspan="6">
                        <span id="spnPickListDocument3">
                            <telerik:RadTextBox ID="txtDocumentName3" runat="server" Width="338px" Enabled="False" Style="font-weight: bold"
                                CssClass="input">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowDocuments3" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtDocumentId3" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <asp:LinkButton ID="lnkFormAdd3" runat="server"  Text="Add"></asp:LinkButton>
                        <br />
                        <div id="divForms3" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 360px; border-width: 1px; border-style: solid; border: 1px solid #9f9fff">
                            <table id="tblForms3" runat="server">
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadLabel ID="lblheaderplace" runat="server" Text="<b>Details of arrangements (if any planned) to dispose of any remaining non-compliant fuel oil:</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td colspan="5">
                        <eluc:Date ID="UcDisposeDate" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                </tr>
                <tr style="display: none;">
                    <td style="width: 16%;">
                        <telerik:RadLabel ID="lblDisposeRegion" runat="server" Text="Region"></telerik:RadLabel>
                    </td>
                    <td style="width: 16%;">
                        <asp:DropDownList ID="ddlDisposeRegion" runat="server" Width="180px" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="lblDisposeRegion_SelectedIndexChanged"></asp:DropDownList></td>
                    <td style="width: 16%;">
                        <telerik:RadLabel ID="lblDisposeCountry" runat="server" Text="Country"></telerik:RadLabel>
                    </td>
                    <td style="width: 16%;">
                        <asp:DropDownList ID="ddlDisposeCountry" runat="server" Width="180px" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlDisposeCountry_SelectedIndexChanged"></asp:DropDownList></td>
                    <td style="width: 16%;">
                        <telerik:RadLabel ID="lblDisposePort" runat="server" Text="Port"></telerik:RadLabel>
                    </td>
                    <td style="width: 16%;">
                        <asp:DropDownList ID="ddlDisposePort" runat="server" Width="180px" CssClass="input"></asp:DropDownList></td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadLabel ID="Literal5" runat="server" Text="<b>Text</b>"></telerik:RadLabel>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="LinkComplientFuel" runat="server" Text="<b>Documents</b>" OnClick="LinkComplientFuel_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr id="tr3" runat="server">
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtOfficedisposedetials" runat="server" Height="70px" CssClass="readonlytextbox" Enabled="false" TextMode="MultiLine" Resize="Both" Rows="4"
                            Width="98%" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtdisposedetials" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Resize="Both" Rows="4"
                            Width="98%" />
                    </td>
                </tr>
                                <tr style="display:none;">
                    <td colspan="6">
                        <telerik:RadLabel ID="Literal4" runat="server" Text="<b>Documents:</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr style="display: none;">
                    <td colspan="6">
                        <span id="spnPickListDocument4">
                            <telerik:RadTextBox ID="txtDocumentName4" runat="server" Width="338px" Enabled="False" Style="font-weight: bold"
                                CssClass="input">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowDocuments4" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtDocumentId4" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <asp:LinkButton ID="lnkFormAdd4" runat="server" Text="Add"></asp:LinkButton>
                        <br />
                        <div id="divForms4" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 360px; border-width: 1px; border-style: solid; border: 1px solid #9f9fff">
                            <table id="tblForms4" runat="server">
                            </table>
                        </div>
                    </td>
                </tr>
            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
