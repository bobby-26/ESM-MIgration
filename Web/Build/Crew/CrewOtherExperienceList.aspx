<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOtherExperienceList.aspx.cs"
    Inherits="CrewOtherExperienceList" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="../UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="OtherCompany" Src="~/UserControls/UserControlOtherCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="../UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="EngineType" Src="../UserControls/UserControlEngineType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="../UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignoffReason" Src="~/UserControls/UserControlSignOffReason.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Nationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SignonReason" Src="~/UserControls/UserControlSignOnReason.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Other Experience</title>
    <telerik:RadCodeBlock ID="CrewCompanyExperiencelink" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewOtherExperienceList" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuCrewOtherExperienceList" runat="server" OnTabStripCommand="CrewOtherExperienceList_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <b >
                <asp:Literal ID="ltrDesc" runat="server" Text="Note: For engine side seafarers, KW or the BHP field is Mandatory"></asp:Literal></b>
            <br />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblManagingCompany" runat="server" Text="Managing Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtManagingCompany" runat="server" Width="50%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblManningCompany" runat="server" Text="Manning Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:OtherCompany ID="ddlManningCompany" runat="server" CssClass="input_mandatory" Width="50%"
                            AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRank" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="input_mandatory" Width="50%"></telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFrom" runat="server" Text="Sign On Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtFrom" runat="server" CssClass="input_mandatory" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTo" runat="server" Text="Sign Off Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtTo" runat="server" CssClass="input_mandatory" Width="50%" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselType ID="ddlVesselType" runat="server" AppendDataBoundItems="true" Width="50%" CssClass="dropdown_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDuration" runat="server" Text="Duration"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDuration" runat="server" ReadOnly="true" Enabled="false" Width="50%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEngineType" runat="server" Text="Engine Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:EngineType ID="ddlEngineType" runat="server" AppendDataBoundItems="true" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEngineModel" runat="server" Text="Engine Model"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEngineModel" runat="server" Width="50%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSignonReason" runat="server" Text="Sign-on Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SignonReason ID="ddlSignonReason" runat="server" AppendDataBoundItems="true" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSignOffReason" runat="server" Text="Sign-off Reason"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:SignoffReason ID="ddlSignOffReason" runat="server" AppendDataBoundItems="true" Width="50%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFlag" runat="server" Text="Flag"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Flag runat="server" ID="ucFlag" AppendDataBoundItems="true" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblpromotion" runat="server" Text="Promoted Onboard y/n"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkPromtedyn" runat="server" ></telerik:RadCheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUMS" runat="server" Text="UMS"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlUMS" runat="server" AppendDataBoundItems="true" Width="50%"
                            ShortNameFilter="S,N" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblKW" runat="server" Text="KW"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtKWT" runat="server" CssClass="input txtNumber" MaxLength="10" MaskText="##########"
                            IsInteger="true" AutoPostBack="true" OnTextChangedEvent="CalculateBHP" Width="50%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGrossTonnage" runat="server" Text="Gross Tonnage"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtGt" runat="server" CssClass="input txtNumber" MaxLength="10" MaskText="##########" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblBHP" runat="server" Text="BHP"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtBHP" runat="server" CssClass="input txtNumber" MaxLength="10" MaskText="##########" Width="50%"
                            IsInteger="true" AutoPostBack="true" OnTextChangedEvent="CalculateKwt" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeadWeightTonnage" runat="server" Text="Deadweight Tonnage"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtDWT" runat="server" CssClass="input txtNumber" MaxLength="10" MaskText="##########" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblTEU" runat="server" Text="TEU"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtTEU" runat="server" CssClass="input txtNumber" MaxLength="10" MaskText="##########" Width="50%" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNationalityofOfficers" runat="server" Text="Nationality of Officers"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Nationality ID="ucNatlyOfficers" runat="server" AppendDataBoundItems="true" Width="50%" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblNationalityofRatings" runat="server" Text="Nationality of Ratings"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Nationality ID="ucNatlyRatings" runat="server" AppendDataBoundItems="true" Width="50%" />
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFramoExp" runat="server" Text="Framo Exp"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkFramo" runat="server"></telerik:RadCheckBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="1">
                        <telerik:RadTextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Width="50%"
                           >
                        </telerik:RadTextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIceExperience" runat="server" Text="Ice Experience"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlIceClass" runat="server" AllowCustomText="true" Width="50%" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="0" />
                                <telerik:RadComboBoxItem Text="Yes" Value="1" />
                                <telerik:RadComboBoxItem Text="No" Value="2" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblGap" runat="server" Text="Gap"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtGap" runat="server" ReadOnly="true" Enabled="false" Width="50%"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
