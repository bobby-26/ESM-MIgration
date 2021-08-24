<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewoffshoreOtherCompanyExperienceAdd.aspx.cs"
    Inherits="CrewOffshore_CrewoffshoreOtherCompanyExperienceAdd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="../UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DocumentType" Src="~/UserControls/UserControlDocumentType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OtherCompany" Src="~/UserControls/UserControlOtherCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="../UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EngineType" Src="../UserControls/UserControlEngineType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="../UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignoffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignonReason" Src="~/UserControls/UserControlSignOnReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="DPClass" Src="~/UserControls/UserControlDPClass.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Propulsion" Src="~/UserControls/UserControlPropulsion.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TradingArea" Src="~/UserControls/UserControlTradingArea.ascx" %>
<%@ Register TagPrefix="eluc" TagName="InstallType" Src="~/UserControls/UserControlInstallType.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>


</head>
<body>
    <form id="frmCrewOtherExperienceList" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">


            <eluc:TabStrip ID="MenuCrewOtherExperienceList" Title="Crew Other Experience" runat="server" OnTabStripCommand="CrewOtherExperienceList_TabStripCommand"></eluc:TabStrip>

            <b style="color: Blue;">
                <telerik:RadLabel ID="ltrDesc" runat="server" Text="Note: For engine side seafarers,KW or the BHP field is Mandatory"></telerik:RadLabel></b>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblManagingCompany" runat="server" Text="Managing Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtManagingCompany" runat="server" ></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblManningCompany" runat="server" Text="Manning Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:OtherCompany ID="ddlManningCompany" runat="server" CssClass="input_mandatory"
                            AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFrom" runat="server" Text="Sign On Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFrom" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTo" runat="server" Text="Sign Off Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtTo" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselType ID="ddlVesselType" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEngineType" runat="server" Text="Engine Type"></telerik:RadLabel><br />
                        <telerik:RadLabel ID="lblEngineModel" runat="server" Text="Engine Model"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:EngineType ID="ddlEngineType" runat="server" AppendDataBoundItems="true"  />
                        <br />
                        <telerik:RadTextBox ID="txtEngineModel" runat="server" ></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignonReason" runat="server" Text=" Sign-on Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SignonReason ID="ddlSignonReason" Width="63%" runat="server" 
                            AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOffReason" runat="server" Text="Sign-off Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SignoffReason ID="ddlSignOffReason" runat="server"  AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Flag runat="server" ID="ucFlag" Width="63%"  AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblpromotion" runat="server"> Promoted Onboard y/n</telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkPromtedyn" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUMS" runat="server" Text=" UMS"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlUMS" runat="server" Width="63%" AppendDataBoundItems="true" 
                            ShortNameFilter="S,N" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblKW" runat="server" Text=" KW"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtKWT" runat="server" CssClass="input txtNumber" MaxLength="8"
                            IsInteger="true" AutoPostBack="true" OnTextChangedEvent="CalculateBHP" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGrossTonnage" runat="server" Text="Gross Tonnage"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtGt" runat="server" CssClass="input txtNumber" MaxLength="8" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBHP" runat="server" Text=" BHP"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtBHP" runat="server" CssClass="input txtNumber" MaxLength="8"
                            IsInteger="true" AutoPostBack="true" OnTextChangedEvent="CalculateKwt" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeadWeightTonnage" runat="Server" Text=" Deadweight Tonnage"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtDWT" runat="server" CssClass="input txtNumber" MaxLength="8" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNationalityofOfficers" runat="Server" Text="Nationality of Officers"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Nationality ID="ucNatlyOfficers" runat="server" AppendDataBoundItems="true"
                             />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNationalityofRatings" runat="server" Text="Nationality of Ratings"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Nationality ID="ucNatlyRatings" runat="server" Width="63%" AppendDataBoundItems="true"
                             />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="1">
                        <telerik:RadTextBox ID="txtRemarks" runat="server"  TextMode="MultiLine"
                            Width="220px" Height="32px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFramoExp" runat="server" Text="Framo Exp" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkFramo" runat="server" Visible="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblIceExperience" runat="server" Text="Ice Experience" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlIceClass" runat="server"  Visible="false">
                            <asp:ListItem Text="--Select--" Value="0" />
                            <asp:ListItem Text="Yes" Value="1" />
                            <asp:ListItem Text="No" Value="2" />
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDP123" runat="server" Text="DP 1/2/3"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:DPClass runat="server" ID="DPClass" Width="63%" AppendDataBoundItems="true"
                             />
                    </td>
                    <%--<td>
                            <telerik:RadTextBox ID="txtDP123" runat="server"  Width="240px"></telerik:RadTextBox>
                        </td>--%>
                    <td>
                        <telerik:RadLabel ID="lblDPMakeandModel" runat="server" Text="DP Make and Model"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDPMakeandModel" runat="server"  Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPropulsion" runat="server" Text="Propulsion"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Propulsion runat="server" ID="ucPropulsion" Width="63%" AppendDataBoundItems="true"
                             />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVoltage" runat="server" Text="Operating Voltage"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtVoltage" runat="server" CssClass="input txtNumber" MaxLength="3"
                            IsInteger="true" />
                        V
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbloperatingarea" runat="server" Text="Operating Area"></telerik:RadLabel>
                    </td>
                    <td width="40%">
                        <eluc:TradingArea runat="server" ID="ucTradingArea" Width="63%" AppendDataBoundItems="true"
                             />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblcharterer" runat="server" Text="Charterer"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtcharterer" runat="server"  Width="240px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTypeofinstallation" runat="server" Text="Type of Installation Served"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:InstallType runat="server" ID="ucInstalationType" Width="63%" AppendDataBoundItems="true"
                             />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDuration" runat="Server" Text="Duration"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDuration" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblGap" runat="Server" Text=" Gap"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtGap" runat="server" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
             
            </table>
            <eluc:Status runat="server" ID="ucStatus" />
        </div>

    </form>
</body>
</html>
