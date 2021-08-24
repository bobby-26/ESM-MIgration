<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewVesselMasterAdmin.aspx.cs" Inherits="Crew_CrewVesselMasterAdmin" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
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
    <title>Admin </title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegisterParticulars" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
        </telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:UserControlStatus ID="ucStatus" runat="server" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuVesselAdmin" runat="server" OnTabStripCommand="MenuVesselAdmin_TabStripCommand"></eluc:TabStrip>
            <table width="100%">
                <tr>
                    <td valign="baseline" style="width: 10%">Vessel 
                    </td>
                    <td valign="baseline" align="left">
                        <telerik:RadTextBox runat="server" ID="txtVesselName" Text="" Width="80%" ReadOnly="true" Enabled="false" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td valign="baseline">Crewing Fleet
                    </td>
                    <td valign="baseline" width="35%">
                        <eluc:Fleet runat="server" Enabled="false" ID="ucFleet" Width="80%" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td valign="baseline">Technical Fleet
                    </td>
                    <td>
                        <eluc:Fleet runat="server" ID="ucTechFleet" Enabled="false" Width="80%" AppendDataBoundItems="true" />
                    </td>
                    <td valign="baseline">Accounts Fleet
                    </td>
                    <td>
                        <eluc:Fleet runat="server" ID="ucAcctFleet" Enabled="false" Width="80%" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Officer Pool
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:Pool runat="server" ID="ucOfficerPool" Enabled="false" AppendDataBoundItems="true" />
                    </td>
                    <td valign="baseline" style="width: 10%">Rating Pool
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:Pool runat="server" ID="ucRatingPool" Enabled="false" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">P & I Club
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:AddressType runat="server" ID="ucAddrPandIClub" Enabled="false" AddressType='<%# ((int)PhoenixAddressType.PANDICLUB).ToString() %>' AppendDataBoundItems="true" Width="80%" />
                    </td>
                    <td valign="baseline" style="width: 10%">Medical Type
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:Hard runat="server" ID="ucMedicals" Enabled="false" AppendDataBoundItems="true" HardTypeCode="95"
                            HardList='<%# PhoenixRegistersHard.ListHard(1, 95, 0, "UKP,P&I,PMU") %>' ShortNameFilter="UKP,P&I,PMU" />
                    </td>
                </tr>
                <tr>
                    <td valign="baseline">Insurance Currency
                    </td>
                    <td valign="baseline">
                        <eluc:Currency runat="server" ID="ucCurrency" Enabled="false" AppendDataBoundItems="true" ActiveCurrency="true" />
                    </td>
                    <td valign="baseline" style="width: 10%">Insurance deductibles
                    </td>
                    <td valign="baseline" width="20%">
                        <telerik:RadTextBox runat="server" ID="txtDeductible" ReadOnly="true" Enabled="false" MaxLength="100" Style="text-align: right;" Width="70px"></telerik:RadTextBox>

                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Tech. Superintendent 
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <span id="spnPickListSupt">
                            <telerik:RadTextBox ID="txtSuptName" runat="server" Enabled="false" MaxLength="200" Width="40%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtSuptDesignation" runat="server" Enabled="false" MaxLength="50" Width="35%"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowSupt" Enabled="false" Style="cursor: pointer; vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                OnClientClick="return showPickList('spnPickListSupt', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                            <telerik:RadTextBox runat="server" ID="txtSupt"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtSuptEmailHidden" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td valign="baseline" style="width: 10%">Email
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtSuptEmail" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Fleet Manager
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <span id="spnPickListFleetManager">
                            <telerik:RadTextBox ID="txtFleetManagerName" runat="server" Enabled="false" MaxLength="200" Width="40%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtFleetManagerDesignation" runat="server" Enabled="false" MaxLength="50" Width="35%"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowFleetManager" Enabled="false" Style="cursor: pointer; vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                OnClientClick="return showPickList('spnPickListFleetManager', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                            <telerik:RadTextBox runat="server" ID="txtFleetManager"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtFleetManagerEmailHidden" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td valign="baseline" style="width: 10%">Email
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtFleetManagerEmail" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Tech. Director
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <span id="spnTechDirector">
                            <telerik:RadTextBox ID="txtTechDirectorName" runat="server" Enabled="false" MaxLength="200" Width="40%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtTechDirectorDesignation" runat="server" Enabled="false" MaxLength="50" Width="35%"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowTechDirector" Enabled="false" Style="cursor: pointer; vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                OnClientClick="return showPickList('spnTechDirector', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                            <telerik:RadTextBox runat="server" ID="txtTechDirector"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtTechDirectorEmailHidden" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td valign="baseline" style="width: 10%">Email
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtTechDirectorEmail" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Personnel Officer
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <span id="spnPLPersonalOfficer">
                            <telerik:RadTextBox ID="txtPOName" runat="server" Enabled="false" MaxLength="200" Width="40%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtPODesignation" runat="server" Enabled="false" MaxLength="50" Width="35%"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowPO" Enabled="false" Style="cursor: pointer; vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                OnClientClick="return showPickList('spnPLPersonalOfficer', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                            <telerik:RadTextBox runat="server" ID="txtPersonalOfficer"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtPOEmailHidden" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td valign="baseline" style="width: 10%">Email
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtPOEmail" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Fleet Personnel Manager
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <span id="spnFPM">
                            <telerik:RadTextBox ID="txtFPMName" runat="server" Enabled="false" MaxLength="200" Width="40%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtFPMDesignation" runat="server" Enabled="false" MaxLength="50" Width="35%"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowFPM" Enabled="false" Style="cursor: pointer; vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                OnClientClick="return showPickList('spnFPM', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                            <telerik:RadTextBox runat="server" ID="txtFPM"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtFPMEmailHidden" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td valign="baseline" style="width: 10%">Email
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtFPMEmail" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Fleet Personnel Superintendent
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <span id="spnFPS">
                            <telerik:RadTextBox ID="txtFPSName" runat="server" Enabled="false" MaxLength="200" Width="40%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtFPSDesignation" runat="server" Enabled="false" MaxLength="50" Width="35%"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowFPS" Enabled="false" Style="cursor: pointer; vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                OnClientClick="return showPickList('spnFPS', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                            <telerik:RadTextBox runat="server" ID="txtFPS"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtFPSEmailHidden" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td valign="baseline" style="width: 10%">Email
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtFPSEmail" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Vessel PIC (Crew, HO)
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <span id="spnCrewManager">
                            <telerik:RadTextBox ID="txtCrewManagerName" runat="server" Enabled="false" MaxLength="200" Width="40%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtCrewManagerDesignation" runat="server" Enabled="false" MaxLength="50" Width="35%"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowCrewManager" Enabled="false" Style="cursor: pointer; vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                OnClientClick="return showPickList('spnCrewManager', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                            <telerik:RadTextBox runat="server" ID="txtCrewManager"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtCrewManagerEmailHidden" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td valign="baseline" style="width: 10%">Email
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtCrewManagerEmail" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Travel PIC
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <span id="spnPLTravelPIC">
                            <telerik:RadTextBox ID="txtTravelPICName" runat="server" Enabled="false" MaxLength="200" Width="40%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtTravelPICDesignation" runat="server" Enabled="false" MaxLength="50" Width="35%"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowTravelPIC" Enabled="false" Style="cursor: pointer; vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                OnClientClick="return showPickList('spnPLTravelPIC', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                            <telerik:RadTextBox runat="server" ID="txtTravelPIC"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtTravelPICEmailHidden" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td valign="baseline" style="width: 10%">Email
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtTravelPICEmail" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Account Administrator
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <span id="spnAccountAdmin">
                            <telerik:RadTextBox ID="txtAccountAdminName" runat="server" Enabled="false" MaxLength="200" Width="40%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtAccountAdminDesignation" runat="server" Enabled="false" MaxLength="50" Width="35%"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowAccountAdmin" Enabled="false" Style="cursor: pointer; vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                OnClientClick="return showPickList('spnAccountAdmin', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                            <telerik:RadTextBox runat="server" ID="txtAccountAdmin"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtAccountAdminEmailHidden" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td valign="baseline" style="width: 10%">Email
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtAccountAdminEmail" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" valign="baseline">Purchaser 
                    </td>
                    <td valign="baseline">
                        <span id="spnPickListPurchaser">
                            <telerik:RadTextBox ID="txtPurchaserName" runat="server" Enabled="false" MaxLength="200" Width="40%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtPurchaserDesignation" runat="server" Enabled="false" MaxLength="50" Width="35%"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowPurchaser" Enabled="false" Style="cursor: pointer; vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                OnClientClick="return showPickList('spnPickListPurchaser', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                            <telerik:RadTextBox runat="server" ID="txtPurchaser"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtPurchaserEmailHidden" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td valign="baseline" style="width: 10%">Email
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtPurchaserEmail" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" valign="baseline">Quality PIC
                    </td>
                    <td valign="baseline">
                        <span id="spnPickListQualityPIC">
                            <telerik:RadTextBox ID="txtQualityPICName" runat="server" Enabled="false" MaxLength="200" Width="40%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtQualityPICDesignation" runat="server" Enabled="false" MaxLength="50" Width="35%"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowQualityPIC" Enabled="false" Style="cursor: pointer; vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                OnClientClick="return showPickList('spnPickListQualityPIC', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                            <telerik:RadTextBox runat="server" ID="txtQualityPIC"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtQualityPICEmailHidden" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td valign="baseline" style="width: 10%">Email
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtQualityPICEmail" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" valign="baseline">QA Manager
                    </td>
                    <td valign="baseline">
                        <span id="spnPickListQAManger">
                            <telerik:RadTextBox ID="txtQAManagerName" runat="server" Enabled="false" MaxLength="200" Width="40%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtQAManagerDesignation" runat="server" Enabled="false" MaxLength="50" Width="35%"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowQAManager" Enabled="false" Style="cursor: pointer; vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                OnClientClick="return showPickList('spnPickListQAManger', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                            <telerik:RadTextBox runat="server" ID="txtQAManager"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtQAManagerEmailHidden" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td valign="baseline" style="width: 10%">Email
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtQAManagerEmail" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" valign="baseline">DPA
                    </td>
                    <td valign="baseline">
                        <span id="spnPickListDPA">
                            <telerik:RadTextBox ID="txtDPAName" runat="server" Enabled="false" MaxLength="200" Width="40%"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtDPADesignation" runat="server" Enabled="false" MaxLength="50" Width="35%"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="imgShowDPA" Enabled="false" Style="cursor: pointer; vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                OnClientClick="return showPickList('spnPickListDPA', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                            <telerik:RadTextBox runat="server" ID="txtDPA"></telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtDPAEmailHidden" Width="10px"></telerik:RadTextBox>
                        </span>
                    </td>
                    <td valign="baseline" style="width: 10%">Email
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtDPAEmail" CssClass="readonlytextbox" ReadOnly="true" Enabled="false" Width="250px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Officer Wage Scale/Expiry Date
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtOfficerWageScale" Width="320px" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgOfficerClip" runat="server" />
                    </td>
                    <td valign="baseline" style="width: 10%">Ratings Wage Scale/Expiry Date
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtRatingsWageScale" Width="320px" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgRatingClip" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Seniority Wage Scale
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtSeniorityWageScale" Width="320px" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td valign="baseline">Standard Wage Components
                    </td>
                    <td valign="baseline">
                        <telerik:RadTextBox runat="server" ID="txtESMStdWage" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Crewing Agency
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <asp:ListBox ID="lbCrewingAgency" Enabled="false" runat="server" Width="81%" SelectionMode="Multiple"></asp:ListBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
