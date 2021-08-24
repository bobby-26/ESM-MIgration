<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersVesselAdmin.aspx.cs"
    Inherits="OwnersVesselAdmin" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Particulars</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div2" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmVesselAdmin" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
        position: absolute;">
        <asp:UpdatePanel runat="server" ID="pnlVesselAdmin">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="div1" style="vertical-align: top">
                    </div>
                </div>
                <table width="100%">
                    <tr>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td valign="baseline" align="left">
                            <asp:TextBox runat="server" ID="txtVesselName" Text="" Width="80%" ReadOnly="true"
                                CssClass="readonlytextbox"></asp:TextBox>
                        </td>
                        <td valign="baseline">
                            <asp:Literal ID="lblCrewingFleet" runat="server" Text="Crewing Fleet"></asp:Literal>
                        </td>
                        <td valign="baseline" width="25%">
                            <eluc:Fleet runat="server" ID="ucFleet" Width="80%" CssClass="input" AppendDataBoundItems="true" Enabled="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline">
                            <asp:Literal ID="lblTechnicalFleet" runat="server" Text="Technical Fleet"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Fleet runat="server" ID="ucTechFleet" Width="80%" CssClass="input" AppendDataBoundItems="true" Enabled="false"/>
                        </td>
                        <td valign="baseline">
                            <asp:Literal ID="lblAccountsFleet" runat="server" Text="Accounts Fleet"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Fleet runat="server" ID="ucAcctFleet" Width="80%" CssClass="input" AppendDataBoundItems="true" Enabled="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblOfficerPool" runat="server" Text="Officer Pool"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <eluc:Pool runat="server" ID="ucOfficerPool" CssClass="input" AppendDataBoundItems="true" Enabled="false"/>
                        </td>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblRatingPool" runat="server" Text="Rating Pool"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <eluc:Pool runat="server" ID="ucRatingPool" CssClass="input" AppendDataBoundItems="true" Enabled="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblPIClub" runat="server" Text="P & I Club"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <eluc:AddressType runat="server" ID="ucAddrPandIClub" AddressType='<%# ((int)PhoenixAddressType.PANDICLUB).ToString() %>'
                                CssClass="input" AppendDataBoundItems="true" Width="80%" Enabled="false"/>
                        </td>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblMedicalType" runat="server" Text="Medical Type"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <eluc:Hard runat="server" ID="ucMedicals" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                HardTypeCode="95" HardList='<%# PhoenixRegistersHard.ListHard(1, 95, 0, "UKP,P&I") %>'
                                ShortNameFilter="UKP,P&I" Enabled="false"/>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline">
                            <asp:Literal ID="lblInsuranceCurrency" runat="server" Text="Insurance Currency"></asp:Literal>
                        </td>
                        <td valign="baseline">
                            <eluc:Currency runat="server" ID="ucCurrency" AppendDataBoundItems="true" ActiveCurrency="true"
                                CssClass="input" Enabled="false"/>
                        </td>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblInsurancedeductibles" runat="server" Text="Insurance deductibles"></asp:Literal>
                        </td>
                        <td valign="baseline" width="20%">
                            <asp:TextBox runat="server" ID="txtDeductible" CssClass="input" MaxLength="100" Style="text-align: right;"
                                Width="70px" Enabled="false"></asp:TextBox>
                            <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender8" runat="server" TargetControlID="txtDeductible"
                                Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                            </ajaxToolkit:MaskedEditExtender>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblTechSuperintendent" runat="server" Text="Tech. Superintendent"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox ID="txtSuptName" runat="server" CssClass="input" Enabled="false" MaxLength="200"
                                Width="35%"></asp:TextBox>
                            <asp:TextBox ID="txtSuptDesignation" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="25%"></asp:TextBox>
                        </td>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblSuptEmail" runat="server" Text="Email"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox runat="server" ID="txtSuptEmail" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblFleetManager" runat="server" Text="Fleet Manager"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox ID="txtFleetManagerName" runat="server" CssClass="input" Enabled="false"
                                MaxLength="200" Width="35%"></asp:TextBox>
                            <asp:TextBox ID="txtFleetManagerDesignation" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="25%"></asp:TextBox>
                        </td>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblFleetManagerEmail" runat="server" Text="Email"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox runat="server" ID="txtFleetManagerEmail" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblTechDirector" runat="server" Text="Tech. Director"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox ID="txtTechDirectorName" runat="server" CssClass="input" Enabled="false"
                                MaxLength="200" Width="35%"></asp:TextBox>
                            <asp:TextBox ID="txtTechDirectorDesignation" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="25%"></asp:TextBox>
                        </td>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblTechDirectorEmail" runat="server" Text="Email"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox runat="server" ID="txtTechDirectorEmail" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblPersonnelOfficer" runat="server" Text="Personnel Officer"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox ID="txtPOName" runat="server" CssClass="input" Enabled="false" MaxLength="200"
                                Width="35%"></asp:TextBox>
                            <asp:TextBox ID="txtPODesignation" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="25%"></asp:TextBox>
                        </td>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblPOEmail" runat="server" Text="Email"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox runat="server" ID="txtPOEmail" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblFleetPersonnelManager" runat="server" Text="Fleet Personnel Manager"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox ID="txtFPMName" runat="server" CssClass="input" Enabled="false" MaxLength="200"
                                Width="35%"></asp:TextBox>
                            <asp:TextBox ID="txtFPMDesignation" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="25%"></asp:TextBox>
                        </td>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblFPEmail" runat="server" Text="Email"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox runat="server" ID="txtFPMEmail" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblFleetPersonnelSuperintendent" runat="server" Text="Fleet Personnel Superintendent"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox ID="txtFPSName" runat="server" CssClass="input" Enabled="false" MaxLength="200"
                                Width="35%"></asp:TextBox>
                            <asp:TextBox ID="txtFPSDesignation" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="25%"></asp:TextBox>
                        </td>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblFPSEmail" runat="server" Text="Email"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox runat="server" ID="txtFPSEmail" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblTravelPIC" runat="server" Text="Travel PIC"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox ID="txtTravelPICName" runat="server" CssClass="input" Enabled="false"
                                MaxLength="200" Width="35%"></asp:TextBox>
                            <asp:TextBox ID="txtTravelPICDesignation" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="25%"></asp:TextBox>
                        </td>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblTravelPICEmail" runat="server" Text="Email"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox runat="server" ID="txtTravelPICEmail" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblAccountAdministrator" runat="server" Text="Account Administrator"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox ID="txtAccountAdminName" runat="server" CssClass="input" Enabled="false"
                                MaxLength="200" Width="35%"></asp:TextBox>
                            <asp:TextBox ID="txtAccountAdminDesignation" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="25%"></asp:TextBox>
                        </td>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblAccountAdminEmail" runat="server" Text="Email"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox runat="server" ID="txtAccountAdminEmail" CssClass="readonlytextbox"
                                ReadOnly="true" Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" valign="baseline">
                            <asp:Literal ID="lblPurchaser" runat="server" Text="Purchaser"></asp:Literal>
                        </td>
                        <td valign="baseline">
                            <asp:TextBox ID="txtPurchaserName" runat="server" CssClass="input" Enabled="false"
                                MaxLength="200" Width="35%"></asp:TextBox>
                            <asp:TextBox ID="txtPurchaserDesignation" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="25%"></asp:TextBox>
                        </td>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblPurchaserEmail" runat="server" Text="Email"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox runat="server" ID="txtPurchaserEmail" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" valign="baseline">
                            <asp:Literal ID="lblQualityPIC" runat="server" Text="Quality PIC"></asp:Literal>
                        </td>
                        <td valign="baseline">
                            <asp:TextBox ID="txtQualityPICName" runat="server" CssClass="input" Enabled="false"
                                MaxLength="200" Width="35%"></asp:TextBox>
                            <asp:TextBox ID="txtQualityPICDesignation" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="25%"></asp:TextBox>
                        </td>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblQualityPICEmail" runat="server" Text="Email"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox runat="server" ID="txtQualityPICEmail" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" valign="baseline">
                            <asp:Literal ID="lblQAManager" runat="server" Text="QA Manager"></asp:Literal>
                        </td>
                        <td valign="baseline">
                            <asp:TextBox ID="txtQAManagerName" runat="server" CssClass="input" Enabled="false"
                                MaxLength="200" Width="35%"></asp:TextBox>
                            <asp:TextBox ID="txtQAManagerDesignation" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="25%"></asp:TextBox>
                        </td>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblQAManagerEmail" runat="server" Text="Email"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox runat="server" ID="txtQAManagerEmail" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" valign="baseline">
                            <asp:Literal ID="lblDPA" runat="server" Text="DPA"></asp:Literal>
                        </td>
                        <td valign="baseline">
                            <asp:TextBox ID="txtDPAName" runat="server" CssClass="input" Enabled="false" MaxLength="200"
                                Width="35%"></asp:TextBox>
                            <asp:TextBox ID="txtDPADesignation" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="25%"></asp:TextBox>
                        </td>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblDPAEmail" runat="server" Text="Email"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox runat="server" ID="txtDPAEmail" CssClass="readonlytextbox" ReadOnly="true"
                                Width="250px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblOfficerWageScale" runat="server" Text="Officer Wage Scale"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox runat="server" ID="txtOfficerWageScale" Width="320px" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblRatingsWageScale" runat="server" Text="Ratings Wage Scale"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox runat="server" ID="txtRatingsWageScale" Width="320px" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblSeniorityWageScale" runat="server" Text="Seniority Wage Scale"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:TextBox runat="server" ID="txtSeniorityWageScale" Width="320px" CssClass="readonlytextbox"
                                ReadOnly="true"></asp:TextBox>
                        </td>
                        <td valign="baseline">
                            <asp:Literal ID="lblESMStdWageComponents" runat="server" Text="Std Wage Components"></asp:Literal>
                        </td>
                        <td valign="baseline">
                            <asp:TextBox runat="server" ID="txtESMStdWage" CssClass="readonlytextbox" ReadOnly="true"
                                Width="320px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="baseline" style="width: 10%">
                            <asp:Literal ID="lblCrewingAgency" runat="server" Text="Crewing Agency"></asp:Literal>
                        </td>
                        <td valign="baseline" style="width: 20%">
                            <asp:ListBox ID="lbCrewingAgency" runat="server" Width="81%" SelectionMode="Multiple"
                                CssClass="input"></asp:ListBox>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
