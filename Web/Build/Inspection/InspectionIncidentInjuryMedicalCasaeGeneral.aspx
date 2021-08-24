<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentInjuryMedicalCasaeGeneral.aspx.cs" Inherits="InspectionIncidentInjuryMedicalCasaeGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlNTBRManager">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:UserControlStatus ID="ucStatus" runat="server" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Medical Case" ShowMenu="false"></eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuIncidentInjurySearch" TabStrip="true" runat="server" OnTabStripCommand="MenuIncidentInjurySearch_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="PNIListMain" runat="server" OnTabStripCommand="PNIListMain_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblInspectionPNI" width="100%">
                        <tr>
                            <td style="width: 15%">
                               <asp:Literal ID="lblCaseNo" runat="server" Text=" Case No"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <asp:TextBox ID="txtcaseNo" CssClass="input" runat="server" MaxLength="200"></asp:TextBox>
                            </td>
                            <td style="width: 15%">
                                <asp:Literal ID="lblIllnessDate" runat="server" Text="Illness Date"></asp:Literal>
                            </td>
                            <td style="width: 35%">
                                <eluc:Date ID="ucInjuryDate" runat="server" CssClass="input" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVesselName" CssClass="input" runat="server" MaxLength="200" ReadOnly="true"></asp:TextBox>                               
                            </td>
                            <td>
                               <asp:Literal ID="lblTypeofCase" runat="server" Text="Type of Case"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Hard ID="ucTypeofcase" runat="server" AppendDataBoundItems="true" CssClass="input"
                                    HardTypeCode="174" AutoPostBack="true" OnTextChangedEvent="ucTypeofcase_Changed" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lbLCrewName" runat="server" Text="Crew Name"></asp:Literal>
                            </td>
                            <td>
                                <span id="spnCrewInCharge">
                                    <asp:TextBox ID="txtCrewName" runat="server" CssClass="input" Enabled="false" MaxLength="50"
                                        Width="35%"></asp:TextBox>
                                    <asp:TextBox ID="txtCrewRank" runat="server" CssClass="input" Enabled="false" MaxLength="50"
                                        Width="25%"></asp:TextBox>
                                    <img runat="server" id="imgShowCrewInCharge" style="cursor: pointer; vertical-align: top"
                                        src="<%$ PhoenixTheme:images/picklist.png %>" />
                                    <asp:TextBox ID="txtCrewId" runat="server" CssClass="input" MaxLength="20" Width="10px"></asp:TextBox>
                                </span>
                            </td>
                            <td>
                                <asp:Literal ID="lblCrewHospitalized" runat="server" Text="Crew Hospitalized"></asp:Literal>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkHospital" runat="server" Text="" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblPOoftheVessel" runat="server" Text="PO of the Vessel"></asp:Literal>
                            </td>
                            <td>
                                <span id="spnPLPersonalOfficer">
                                    <asp:TextBox ID="txtponame" runat="server" CssClass="input" Enabled="false" MaxLength="200"
                                        Width="35%"></asp:TextBox>
                                    <asp:TextBox ID="txtpoDesignation" runat="server" CssClass="input" Enabled="false"
                                        MaxLength="50" Width="25%"></asp:TextBox>
                                    <asp:ImageButton runat="server" ID="imgShowPO" Style="cursor: pointer; vertical-align: top"
                                        ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showPickList('spnPLPersonalOfficer', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                                    <asp:TextBox runat="server" ID="txtPersonalOfficer" CssClass="input" Width="75px"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtPOEmail" CssClass="input" Width="10px"></asp:TextBox>
                                </span>
                            </td>
                            <td>
                                <asp:Literal ID="lblSigaporePIC" runat="server" Text="Sigapore PIC"></asp:Literal>
                            </td>
                            <td>
                                <span id="spnPLPIC">
                                    <asp:TextBox ID="txtSigaporeName" runat="server" CssClass="input" Enabled="false"
                                        MaxLength="200" Width="35%"></asp:TextBox>
                                    <asp:TextBox ID="txtSigaporeDesignation" runat="server" CssClass="input" Enabled="false"
                                        MaxLength="50" Width="25%"></asp:TextBox>
                                    <asp:ImageButton runat="server" ID="imgShowPic" Style="cursor: pointer; vertical-align: top"
                                        ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" OnClientClick="return showPickList('spnPLPIC', 'codehelp1', '', '../Common/CommonPickListUser.aspx', true); " />
                                    <asp:TextBox runat="server" ID="txtPic" CssClass="input" Width="75px"></asp:TextBox>
                                    <asp:TextBox runat="server" ID="txtPICEmailHidden" CssClass="input" Width="10px"></asp:TextBox>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblDaysLost" runat="server" Text="Days Lost"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtDaysLost" runat="server" CssClass="input" IsInteger="true" IsPositive="true" />
                            </td>
                            <td>
                                <asp:Literal ID="lblServiceYears" runat="server" Text="Service Years"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtServiceYears" runat="server" CssClass="readonlytextbox" IsInteger="true"
                                    IsPositive="true" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lbPartsofBodyInjured" runat="server" Text="Parts of Body Injured"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Quick runat="server" ID="ucPartsinjured" AppendDataBoundItems="true" CssClass="input"
                                    Enabled="false" QuickTypeCode="68" />
                            </td>
                            <td>
                                <asp:Literal ID="lblTypeofInjury" runat="server" Text="Type of Injury"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Quick runat="server" ID="ucInjuryType" AppendDataBoundItems="true" CssClass="input"
                                    Enabled="false" QuickTypeCode="69" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCategoryofWorkInjury" runat="server" Text="Category of Work Injury"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Quick runat="server" ID="ucInjuryCategory" AppendDataBoundItems="true" CssClass="input"
                                    Enabled="false" QuickTypeCode="70" />
                            </td>
                            <td>
                                <asp:Literal ID="lblComprehesiveDescription" runat="server" Text="Comprehensive Description"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDescription" runat="server" CssClass="input" Height="75px" TextMode="MultiLine"
                                    Width="80%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
