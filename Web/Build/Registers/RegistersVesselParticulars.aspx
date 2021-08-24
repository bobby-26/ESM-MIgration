<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselParticulars.aspx.cs" Inherits="RegistersVesselParticulars" EnableViewStateMac="false" EnableEventValidation="false" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

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
<%@ Register TagPrefix="eluc" TagName="MCUser" Src="~/UserControls/UserControlMultiColumnUser.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Admin</title>

    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegisterParticulars" runat="server">
        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <eluc:TabStrip ID="MenuVesselList" runat="server" OnTabStripCommand="MenuVesselList_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuVesselParticulars" runat="server" OnTabStripCommand="VesselParticulars_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <eluc:UserControlStatus ID="ucStatus" runat="server" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <%--                <div class="subHeader" style="position: relative">
                    <div id="div1" style="vertical-align: top">
                        Admin
                            <asp:Button ID="cmdHiddenPick" runat="server" OnClick="cmdHiddenSubmit_Click" />
                    </div>
                </div>--%>


            <table width="100%">
                <tr>
                    <td valign="baseline" style="width: 10%">Vessel 
                    </td>
                    <td valign="baseline" align="left">
                        <telerik:RadTextBox runat="server" ID="txtVesselName" Text="" Width="80%" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td valign="baseline">Crewing Fleet
                    </td>
                    <td valign="baseline" width="25%">
                        <eluc:Fleet runat="server" ID="ucFleet" Width="80%" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td valign="baseline">Technical Fleet
                    </td>
                    <td>
                        <eluc:Fleet runat="server" ID="ucTechFleet" Width="80%" AppendDataBoundItems="true" />
                    </td>
                    <td valign="baseline">Accounts Fleet
                    </td>
                    <td>
                        <eluc:Fleet runat="server" ID="ucAcctFleet" Width="80%" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Officer Pool
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:Pool runat="server" ID="ucOfficerPool" Width="80%" AppendDataBoundItems="true" />
                    </td>
                    <td valign="baseline" style="width: 10%">Rating Pool
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:Pool runat="server" ID="ucRatingPool" Width="80%" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">P & I Club
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:AddressType runat="server" ID="ucAddrPandIClub" AddressType='<%# ((int)PhoenixAddressType.PANDICLUB).ToString() %>' AppendDataBoundItems="true" Width="80%" />
                    </td>
                    <td valign="baseline" style="width: 10%">Medical Type
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:Hard runat="server" ID="ucMedicals" Width="80%" AppendDataBoundItems="true" CssClass="dropdown_mandatory" HardTypeCode="95"
                            HardList='<%# PhoenixRegistersHard.ListHard(1, 95, 0, "UKP,P&I,PMU") %>' ShortNameFilter="UKP,P&I,PMU" />
                    </td>
                </tr>
                <tr>
                    <td valign="baseline">Insurance Currency
                    </td>
                    <td valign="baseline">
                        <eluc:Currency runat="server" ID="ucCurrency" Width="80%" AppendDataBoundItems="true" ActiveCurrency="true" />
                    </td>
                    <td valign="baseline" style="width: 10%">Insurance deductibles
                    </td>
                    <td valign="baseline" width="20%">
                        <eluc:Decimal ID="txtDeductible" Mask="###,###.##" MaxValue="999999.99" runat="server" Width="80%" DecimalDigits="2" />
                        <%--<telerik:RadMaskedTextBox runat="server" ID="txtDeductible" MaxLength="100" Style="text-align: right;" Width="80%" Mask="###,###.##" DisplayFormatPosition="Right" ButtonsPosition="Right" NumericRangeAlign="Right" inputdirection="Right"></telerik:RadMaskedTextBox>--%>
                        <%--                        <asp:TextBox runat="server" ID="txtDeductible" MaxLength="100" Style="text-align: right;" Width="80%"></asp:TextBox>
                        <ajaxToolkit:MaskedEditExtender ID="MaskedEditExtender8" runat="server" TargetControlID="txtDeductible"
                            Mask="999,999.99" MaskType="Number" InputDirection="RightToLeft">
                        </ajaxToolkit:MaskedEditExtender>--%>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Tech. Superintendent 
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:MCUser ID="RadMcUserSup" runat="server" Width="80%" emailrequired="true" designationrequired="true" />
                    </td>
                    <td valign="baseline" style="width: 10%">Fleet Manager
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:MCUser ID="RadMcUserFM" runat="server" Width="80%" emailrequired="true" designationrequired="true" />

                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Tech. Director
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:MCUser ID="RadMcUserTD" runat="server" Width="80%" emailrequired="true" designationrequired="true" />
                    </td>
                    <td valign="baseline" style="width: 10%">Personnel Officer
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:MCUser ID="RadMcUserPO" runat="server" Width="80%" emailrequired="true" designationrequired="true" />
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Fleet Personnel Manager
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:MCUser ID="RadMcUserFPM" runat="server" Width="80%" emailrequired="true" designationrequired="true" />
                    </td>
                    <td valign="baseline" style="width: 10%">Fleet Personnel Superintendent
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:MCUser ID="RadMcUserFPS" runat="server" Width="80%" emailrequired="true" designationrequired="true" />
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Vessel PIC (Crew, HO)
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:MCUser ID="RadMcUserVP" runat="server" Width="80%" emailrequired="true" designationrequired="true" />
                    </td>
                    <td valign="baseline" style="width: 10%">Travel PIC
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:MCUser ID="RadMcUserTP" runat="server" Width="80%" emailrequired="true" designationrequired="true" />
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Account Administrator
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:MCUser ID="RadMcUserAA" runat="server" Width="80%" emailrequired="true" designationrequired="true" />
                    </td>
                    <td valign="baseline" valign="baseline">Purchaser 
                    </td>
                    <td valign="baseline">
                        <eluc:MCUser ID="RadMcUserP" runat="server" Width="80%" emailrequired="true" designationrequired="true" />
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" valign="baseline">Quality PIC
                    </td>
                    <td valign="baseline">
                        <eluc:MCUser ID="RadMcUserQP" runat="server" Width="80%" emailrequired="true" designationrequired="true" />
                    </td>
                    <td valign="baseline" valign="baseline">QA Manager
                    </td>
                    <td valign="baseline">
                        <eluc:MCUser ID="RadMcUserQA" runat="server" Width="80%" emailrequired="true" designationrequired="true" />
                    </td>
                </tr>
                <tr>
                     <td valign="baseline" style="width: 10%">Marine Superintendent 
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <eluc:MCUser ID="MCMarineSupt" runat="server" Width="80%" emailrequired="true" designationrequired="true" />
                    </td>  
                    <td valign="baseline" valign="baseline">DPA
                    </td>
                    <td valign="baseline">
                        <eluc:MCUser ID="RadMcUserDPA" runat="server" Width="80%" emailrequired="true" designationrequired="true" />
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Officer Wage Scale/Expiry Date
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtOfficerWageScale" Width="360px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgOfficerClip" runat="server" />
                    </td>
                    <td valign="baseline" style="width: 10%">Ratings Wage Scale/Expiry Date
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtRatingsWageScale" Width="80%" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        <asp:Image ImageUrl="<%$ PhoenixTheme:images/attachment.png %>" ID="imgRatingClip" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td valign="baseline" style="width: 10%">Seniority Wage Scale
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <telerik:RadTextBox runat="server" ID="txtSeniorityWageScale" Width="360px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td valign="baseline">Standard Wage Components
                    </td>
                    <td valign="baseline">
                        <telerik:RadTextBox runat="server" ID="txtESMStdWage" Width="80%" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>                 
                    <td valign="baseline" style="width: 10%">Crewing Agency
                    </td>
                    <td valign="baseline" style="width: 20%">
                        <asp:ListBox ID="lbCrewingAgency" runat="server" Width="360px" SelectionMode="Multiple"></asp:ListBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
