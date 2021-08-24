<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentInjuryGeneral.aspx.cs"
    Inherits="InspectionIncidentInjuryGeneral" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hazard" Src="~/UserControls/UserControlRAHazardType.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Incident Injury General</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmIncidentInjuryGeneral" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status runat="server" ID="ucStatus" />
        <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <eluc:TabStrip ID="MenuIncidentInjuryGeneral" runat="server" OnTabStripCommand="MenuIncidentInjuryGeneral_TabStripCommand" Title="Health and Safety"></eluc:TabStrip>
        <table id="tblConfigureIncidentInjury" width="100%">
            <tr>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblThirdPartyInjury" runat="server" Text="Third Party Injury"></telerik:RadLabel>
                </td>
                <td style="width: 35%">
                    <telerik:RadCheckBox ID="chkThirdPartyInjury" runat="server" AutoPostBack="true" OnCheckedChanged="ThirdParty_Changed" />
                </td>
                <td colspan="2"></td>
            </tr>
            <tr>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblInjuredName" runat="server" Text="Injured's Name"></telerik:RadLabel>
                </td>
                <td style="width: 35%">
                    <span id="spnCrewInCharge" runat="server">
                        <telerik:RadTextBox ID="txtCrewName" runat="server" CssClass="input_mandatory" Enabled="false"
                            MaxLength="50" Width="50%">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCrewRank" runat="server" CssClass="input_mandatory" Enabled="false"
                            MaxLength="50" Width="30%">
                        </telerik:RadTextBox>
                        <asp:LinkButton runat="server" ID="imgShowCrewInCharge">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox ID="txtCrewId" runat="server" CssClass="hidden" MaxLength="20" Width="10px"></telerik:RadTextBox>
                    </span>
                    <span id="spnPersonInChargeOffice" runat="server">
                        <telerik:RadTextBox ID="txtOfficePersonName" runat="server" CssClass="input_mandatory" Enabled="false"
                            MaxLength="50" Width="50%">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtOfficePersonDesignation" runat="server" CssClass="input_mandatory" Enabled="false"
                            MaxLength="50" Width="23%">
                        </telerik:RadTextBox>
                        <asp:LinkButton runat="server" ID="imgPersonOffice">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                        </asp:LinkButton>
                        <telerik:RadTextBox runat="server" ID="txtPersonOfficeId" CssClass="hidden" Width="0px"
                            MaxLength="20">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox runat="server" ID="txtPersonOfficeEmail" CssClass="hidden" Width="0px"
                            MaxLength="20">
                        </telerik:RadTextBox>
                    </span>
                    <span id="spnThirdParty" runat="server" visible="false">
                        <telerik:RadTextBox ID="txtThirdPartyName" runat="server" CssClass="input_mandatory" MaxLength="50"
                            Width="50%">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtThirdPartyDesignation" runat="server" CssClass="input_mandatory"
                            MaxLength="50" Width="30%">
                        </telerik:RadTextBox>
                    </span>
                    <telerik:RadLabel ID="lblDtkey" runat="server" Visible="false"></telerik:RadLabel>
                </td>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblAge" runat="server" Text="Age"></telerik:RadLabel>
                </td>
                <td style="width: 35%">
                    <telerik:RadTextBox ID="txtAge" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                        MaxLength="20" Width="20%">
                    </telerik:RadTextBox>
                    <eluc:Number ID="txtThirdPartyAge" runat="server" CssClass="input_mandatory txtNumber"
                        MaxLength="20" Width="20%" IsPositive="true" Visible="false" IsInteger="true" />
                </td>
            </tr>
            <tr>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblServiceYearsinCompany" runat="Server" Text="Service Years in Company"></telerik:RadLabel>
                </td>
                <td style="width: 35%">
                    <telerik:RadTextBox ID="txtServiceYears" runat="server" CssClass="readonlytextbox" MaxLength="20"
                        ReadOnly="true" Width="20%">
                    </telerik:RadTextBox>
                </td>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblServiceYearsatSea" runat="server" Text="Service Years at Sea"></telerik:RadLabel>
                </td>
                <td style="width: 35%">
                    <telerik:RadTextBox ID="txtServiceYearsAtSea" runat="server" CssClass="readonlytextbox"
                        MaxLength="20" ReadOnly="true" Width="20%">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblTypeofInjury" runat="server" Text="Type of Injury"></telerik:RadLabel>
                </td>
                <td style="width: 35%">
                    <eluc:Quick ID="ucTypeOfInjury" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                        QuickTypeCode="69" Width="240px" />7
                </td>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblPartoftheBodyInjured" runat="server" Text="Part of the Body Injured"></telerik:RadLabel>
                </td>
                <td style="width: 35%">
                     <telerik:RadComboBox ID="ddlPartofTheBodyInjured" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                            EmptyMessage="Type to select " Filter="Contains" CssClass="input_mandatory" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true"
                            Width="240px">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblCategoryofWorkInjury" runat="server" Text="Category of Work Injury"></telerik:RadLabel>
                </td>
                <td style="width: 35%">
                    <telerik:RadComboBox ID="ddlWorkInjuryCategory" runat="server" Width="240px" CssClass="input_mandatory" Filter="Contains">
                    </telerik:RadComboBox>
                </td>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblWorkDaysLost" runat="server" Text="Work days lost"></telerik:RadLabel>
                </td>
                <td style="width: 35%">
                    <eluc:Number ID="ucManHoursLost" runat="server" CssClass="input txtNumber" IsInteger="true"
                        IsPositive="true" MaxLength="8" Width="20%" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblHealthandSafetyCategory" runat="server" Text="Health and Safety Category"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Hazard ID="ucHealthSafetyCategory" Type="1" runat="server" AutoPostBack="true"
                        Width="240px" AppendDataBoundItems="true" OnTextChangedEvent="ucHealthSafetyCategory_Changed"
                        CssClass="input_mandatory" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblHealthandSafetySubcategory" runat="server" Text="Health and Safety Subcategory"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadComboBox ID="ddlHealthSafetySubCategory" Width="240px" AppendDataBoundItems="true"
                        runat="server" CssClass="input_mandatory" DataTextField="FLDNAME" DataValueField="FLDSUBHAZARDID"
                        Filter="Contains">
                    </telerik:RadComboBox>
                </td>
            </tr>
            <tr>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                </td>
                <td style="width: 35%">
                    <telerik:RadTextBox runat="server" ID="txtRemarks" CssClass="input" TextMode="MultiLine"
                        Height="70px" Width="81%">
                    </telerik:RadTextBox>
                    <telerik:RadTextBox runat="server" ID="txtDescription" CssClass="input" TextMode="MultiLine"
                        Height="70px" Width="80%" Visible="false">
                    </telerik:RadTextBox>
                </td>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblEstimatedCostinUSD" runat="server" Text="Estimated Cost in USD"></telerik:RadLabel>
                </td>
                <td style="width: 35%">
                    <eluc:Number ID="ucExtimatedCost" runat="server" CssClass="input txtNumber" DecimalPlace="2"
                        MaxLength="10" />
                </td>
            </tr>
            <tr>
                <td style="width: 15%">
                    <telerik:RadLabel ID="lblConsequencePotentialCategory" runat="server" Text="Consequence/Potential Category"></telerik:RadLabel>
                </td>
                <td style="width: 35%">
                    <telerik:RadTextBox ID="txtCategory" runat="server" Enabled="false" Width="30px" ReadOnly="true"
                        CssClass="input">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <asp:LinkButton ID="lnkSicknessReport" runat="server" Text="CR11 Sickness Report"></asp:LinkButton>
                </td>
                <td colspan="2"></td>
            </tr>
        </table>
    </form>
</body>
</html>
