<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAMachineryDetails.aspx.cs"
    Inherits="InspectionRAMachineryDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hazard" Src="~/UserControls/UserControlRAHazardType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Miscellaneous" Src="~/UserControls/UserControlRAMiscellaneous.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Frequency" Src="~/UserControls/UserControlRAFrequency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CustomEditor" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Assessment Machinery</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            table.Hazard {
                border-collapse: collapse;
            }

                table.Hazard td, table.Hazard th {
                    border: 1px solid black;
                    padding: 5px;
                }
        </style>
        <script language="javascript" type="text/javascript">
            function TxtMaxLength(text, maxLength) {
                text.value = text.value.substring(0, maxLength);
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmMachinery" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuMachinery" runat="server" OnTabStripCommand="MenuMachinery_TabStripCommand"></eluc:TabStrip>
            <table width="99.9%" border="1" cellpadding="1" cellspacing="1">
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblRefNo" runat="server" Text="Ref Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="330px" CssClass="readonlytextbox" ID="txtRefNo"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRevisionNo" runat="server" Text="Revision No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="134px" CssClass="readonlytextbox" ID="txtRevNO"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblPreparedBy" runat="server" Text="Prepared By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="330px" CssClass="readonlytextbox" ID="txtpreparedby"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucCreatedDate" CssClass="readonlytextbox" runat="server"
                            ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblApprovedBy" runat="server" Text="Approved By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="330px" CssClass="readonlytextbox" ID="txtApprovedby"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate1" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucApprovedDate" CssClass="readonlytextbox" runat="server"
                            ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblIssuedBy" runat="server" Text="Issued By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="330px" CssClass="readonlytextbox" ID="txtIssuedBy"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDate2" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucIssuedDate" CssClass="readonlytextbox" runat="server"
                            ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="330px"></telerik:RadTextBox>
                        <eluc:Date ID="txtDate" Visible="false" CssClass="readonlytextbox" ReadOnly="false"
                            runat="server" DatePicker="false" />
                    </td>
                    <td rowspan="7" colspan="2">
                        <table class="Hazard">
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblHeader" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" style="background-color: rgb(220,230,241);">
                                    <telerik:RadLabel ID="lblHealthSafety" runat="server">Health and Safety Hazards</telerik:RadLabel>
                                </td>
                                <td align="center" style="background-color: rgb(216,228,188);">
                                    <telerik:RadLabel ID="lblEnviormental" runat="server">Environmental Impact</telerik:RadLabel>
                                </td>
                                <td align="center" style="background-color: rgb(253,233,217);">
                                    <telerik:RadLabel ID="lblEconomic" runat="server">Economic/Process Loss</telerik:RadLabel>
                                </td>
                                <td align="center" style="background-color: rgb(230,184,183);">
                                    <telerik:RadLabel ID="lblWorst" runat="server">Worst Case Scenario</telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="lblHazard" runat="server">Hazard</telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblHarzardHealth" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblHazardEnv" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblHazardEconomic" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblHazardWorst" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="lblProb" runat="server">Prob of Occurrence</telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblProbhealth" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblProbEnv" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblProbEcomoic" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblProbWorst" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="lblLikelihood" runat="server">Likelihood of Harm</telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblLikelihoodHealth" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblLikelihoodEnv" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblLikelihoodEconomic" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblLikelihoodWorst" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="lblLevelOfControl" runat="server">Level of Control</telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblLevelOfControlHealth" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblLevelOfControlEnv" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblLevelOfControlEconomic" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblLevelOfControlWorst" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="lblLevelofRisk" runat="server">Level of Risk</telerik:RadLabel>
                                </td>
                                <td align="center" id="levelofriskhealth" runat="server">
                                    <telerik:RadLabel ID="lblLevelofRiskHealth" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="levelofriskenv" runat="server">
                                    <telerik:RadLabel ID="lblLevelofRiskEnv" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="levelofriskeco" runat="server">
                                    <telerik:RadLabel ID="lblLevelofRiskEconomic" runat="server"></telerik:RadLabel>
                                </td>
                                <td align="center" id="levelofriskworst" runat="server">
                                    <telerik:RadLabel ID="lblLevelofRiskWorst" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucCompany" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="330px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblDateofIntendedWorkActivity" runat="server" Text="Date of intended work activity"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtIntendedWorkDate" CssClass="input_mandatory" runat="server" DatePicker="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCategory" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="330px"
                            AutoPostBack="True" DataTextField="FLDNAME" DataValueField="FLDACTIVITYID" OnSelectedIndexChanged="ddlCategory_Changed" Filter="Contains">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY" />
                            </Items>
                        </telerik:RadComboBox>
                        <telerik:RadLabel ID="lblCategoryShortCode" runat="server" Visible="false"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblAmendmentTo" runat="server" Text="Amendment To"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAmendedTo" runat="server" CssClass="input" Filter="Contains" Width="330px">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblEquipmentBeingAssessed" runat="server" Text="Equipment being assessed"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListComponent">
                            <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="input" MaxLength="20"
                                Enabled="false" Width="80px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input" MaxLength="20"
                                Enabled="false" Width="250px">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgComponent">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="imgClearParentComponent" OnClick="ClearComponent">
                                <span class="icon"><i class="fas fa-paint-brush"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="input" Width="5px"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblActivityConditionsEquipment" runat="server" Text="Activity / Conditions / Equipment"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<eluc:Miscellaneous ID="ddlActivity" runat="server" CssClass="dropdown_mandatory"
                                    AppendDataBoundItems="true" Type="6" />--%>
                        <telerik:RadTextBox ID="txtActivityCondition" runat="server" CssClass="readonlytextbox"
                            Enabled="false" Width="360px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        <telerik:RadLabel ID="lblFunctionalityoftheEquipment" runat="server" Text="Functionality of the Equipment"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBoxList ID="cblFunctionality" runat="server" Columns="3" AutoPostBack="true" OnTextChanged="OtherDetailClick" />
                        <br />
                        <telerik:RadLabel ID="lblDetails" runat="server" Text="Details"></telerik:RadLabel>
                        <br />
                        <telerik:RadTextBox ID="txtFunctionality" runat="server" CssClass="readonlytextbox" Width="700px"
                            Height="80px" onKeyUp="TxtMaxLength(this,700)" onChange="TxtMaxLengththis,700)"
                            TextMode="MultiLine" Rows="3" ReadOnly="true" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        <telerik:RadLabel ID="lblReasonforAssessment" runat="server" Text="Reason for assessment"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBoxList ID="cblReason" runat="server" Columns="4" AutoPostBack="true" OnTextChanged="OtherDetailClick">
                        </telerik:RadCheckBoxList>
                        <br />
                        <telerik:RadTextBox ID="txtOtherReason" runat="server" CssClass="readonlytextbox" TextMode="MultiLine"
                            Width="700px" Height="80px" ReadOnly="true" onKeyUp="TxtMaxLength(this,700)" Resize="Both"
                            onChange="TxtMaxLength(this,700)">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblAternativeMethod" runat="server" Text="Alternative Method Considered for Work"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtAternativeMethod" runat="server" CssClass="input" Width="700px" Resize="Both"
                            Height="80px" TextMode="MultiLine" onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblNoofPeopleInvolvedInActivity" runat="server" Text="No of people involved in activity"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadRadioButtonList ID="rblPeopleInvolved" runat="server" Columns="4">
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblDurationofWorkActivity" runat="server" Text="Duration of work activity"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadRadioButtonList ID="rblWorkDuration" runat="server" />
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        <telerik:RadLabel ID="lblFrequencyofWorkActivity" runat="server" Text="Frequency of work activity"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadRadioButtonList ID="rblWorkFrequency" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top">
                        <telerik:RadLabel ID="lblRisksAspects" runat="server" Text="Risks/Aspects"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <%-- <telerik:RadTextBox ID="txtWorkDetails" runat="server" CssClass="input_mandatory" Width="360px"
                                    TextMode="MultiLine" Resize="Both" Rows="4"></telerik:RadTextBox>--%>
                        <telerik:RadEditor ID="txtWorkDetails" runat="server" Width="100%" Height="225px">
                            <Modules>
                                <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                                <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                            </Modules>
                        </telerik:RadEditor>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        <telerik:RadLabel ID="lblHealthandSafetyImpact" runat="Server" Text="Health and Safety Impact"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadLabel ID="RadlblHazardType" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></telerik:RadLabel>
                        <eluc:Hazard ID="ucHazardType" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            Type="1" AutoPostBack="true" Width="20%" />
                        <br />
                        <br />
                        <telerik:RadGrid ID="gvHealthSafetyRisk" runat="server" AutoGenerateColumns="False" CellSpacing="0" GridLines="None"
                            OnItemCommand="gvHealthSafetyRisk__ItemCommand" Font-Size="11px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false"
                            ShowHeader="true" ShowFooter="true" EnableViewState="true" OnItemDataBound="gvHealthSafetyRisk_ItemDataBound">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDCATEGORYID">
                                <NoRecordsTemplate>
                                    <table runat="server" width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Hazard Type">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Impact">
                                        <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCategoryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadComboBox ID="ddlSubHazardType" runat="server" AppendDataBoundItems="true"
                                                CssClass="dropdown_mandatory" Filter="Contains" Width="70%">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Control" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <eluc:Frequency ID="ddlControlAdd" runat="server" CssClass="dropdown_mandatory" FrequencyList='<%# PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4)%>'
                                                AppendDataBoundItems="true" Type="4" />
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Proposed Controls To reduce risk" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblProposedControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDCONTROLS") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtProposedControlAdd" runat="server" CssClass="input" Width="100%"></telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="HDELETE" ID="cmdDelete" ToolTip="Delete">
                                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Add" CommandName="HADD" ID="cmdAdd" ToolTip="Add">
                                                        <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                            </asp:LinkButton>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        <telerik:RadLabel ID="lblEnvironmentalImpact" runat="server" Text="Environmental Impact"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBoxList ID="cblImpact" runat="server" RepeatDirection="Horizontal" Visible="false">
                            <Items>
                                <telerik:ButtonListItem Text="Direct" Value="1" />
                                <telerik:ButtonListItem Text="Indirect" Value="0" />
                            </Items>
                        </telerik:RadCheckBoxList>
                        <telerik:RadLabel ID="lblHazardType2" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></telerik:RadLabel>
                        <eluc:Hazard ID="ucEnvHazardType" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            Type="2" AutoPostBack="true" Width="20%" />
                        <br />
                        <br />
                        <telerik:RadGrid ID="gvEnvironmentalRisk" runat="server" AutoGenerateColumns="False" CellSpacing="0" GridLines="None"
                            Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" ShowFooter="true" EnableViewState="true"
                            OnItemDataBound="gvEnvironmentalRisk_ItemDataBound" OnItemCommand="gvEnvironmentalRisk_ItemCommand" EnableHeaderContextMenu="true" GroupingEnabled="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDCATEGORYID">
                                <NoRecordsTemplate>
                                    <table runat="server" width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Hazard Type">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Impact Type">
                                        <HeaderStyle HorizontalAlign="Left" Width="15%" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblImpactType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACTNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <eluc:Miscellaneous ID="ucImpactType" AppendDataBoundItems="true" runat="server"
                                                CssClass="input_mandatory" Type="3" IncludeOthers="false" />
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Impact">
                                        <HeaderStyle HorizontalAlign="Left" Width="55%" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCategoryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadComboBox ID="ddlSubHazardType" runat="server" AppendDataBoundItems="true"
                                                CssClass="dropdown_mandatory" Filter="Contains" Width="70%">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Control" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <eluc:Frequency ID="ddlControlAdd" runat="server" CssClass="dropdown_mandatory" FrequencyList='<%# PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4)%>'
                                                AppendDataBoundItems="true" Type="4" />
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Proposed Controls To reduce risk" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblProposedControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDCONTROLS") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtProposedControlAdd" runat="server" CssClass="input" Width="100%"></telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Hazard Type">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <FooterStyle HorizontalAlign="Center" Width="100px" />
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblActionHeader" runat="server">Action</telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="EDELETE" ID="cmdDelete" ToolTip="Delete">
                                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Add" CommandName="EADD" ID="cmdAdd" ToolTip="Add">
                                                        <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                            </asp:LinkButton>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblEconomicImpact" runat="server" Text="Economic Impact"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadLabel ID="lblHazardType1" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></telerik:RadLabel>
                        <eluc:Hazard ID="ucEconomicHazardType" runat="server" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" Type="4" AutoPostBack="true" Width="20%" />
                        <br />
                        <br />
                        <telerik:RadGrid ID="gvEconomicRisk" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" ShowHeader="true" ShowFooter="true" EnableViewState="true" OnItemCommand="gvEconomicRisk_ItemCommand"
                            OnItemDataBound="gvEconomicRisk_ItemDataBound" EnableHeaderContextMenu="true" GroupingEnabled="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDCATEGORYID">
                                <NoRecordsTemplate>
                                    <table runat="server" width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Hazard Type">
                                        <HeaderStyle HorizontalAlign="Left" Width="20%" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Impact">
                                        <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCategoryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadComboBox ID="ddlSubHazardType" runat="server" AppendDataBoundItems="true"
                                                CssClass="dropdown_mandatory" Filter="Contains" Width="70%">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Control" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <eluc:Frequency ID="ddlControlAdd" runat="server" CssClass="dropdown_mandatory" FrequencyList='<%# PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4)%>'
                                                AppendDataBoundItems="true" Type="4" />
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Proposed Controls To reduce risk" Visible="false">
                                        <HeaderStyle HorizontalAlign="Left" Width="0%" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblProposedControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDCONTROLS") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtProposedControlAdd" runat="server" CssClass="input" Width="100%"></telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="CDELETE" ID="cmdDelete" ToolTip="Delete">
                                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Add" CommandName="CADD" ID="cmdAdd" ToolTip="Add">
                                                        <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                            </asp:LinkButton>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblWorstCaseScenario" runat="server" Text="Worst Case Scenario"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBoxList ID="cblOtherRisk" runat="server" Columns="4" AutoPostBack="true" OnTextChanged="OtherDetailClick">
                        </telerik:RadCheckBoxList>
                        <%--<br />
                                Details
                                <br />--%>
                        <telerik:RadTextBox ID="txtOtherDetails" runat="server" CssClass="readonlytextbox" Width="360px"
                            ReadOnly="true" Visible="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <%--<tr valign="top">
                            <td colspan="2">
                                Proposed Controls To Reduce Risk
                            </td>
                            <td colspan="2">
                                <telerik:RadTextBox ID="txtOtherRisk" runat="server" CssClass="input" Width="360px" TextMode="MultiLine"
                                    Rows="3"></telerik:RadTextBox>
                            </td>
                        </tr>--%>
                <tr valign="top">
                    <td colspan="2">
                        <telerik:RadLabel ID="lblPersonnalSparesandTools" runat="server" Text="Personnel, Spares and Tools"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadGrid ID="gvPersonal" runat="server" CellSpacing="0" GridLines="None" AutoGenerateColumns="False"
                            Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" ShowFooter="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                            EnableViewState="true" OnItemCommand="gvPersonal_ItemCommand" OnItemDataBound="gvPersonal_ItemDataBound"
                            OnUpdateCommand="gvPersonal_UpdateCommand">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDCONTROLID">
                                <NoRecordsTemplate>
                                    <table runat="server" width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Item">
                                        <HeaderStyle HorizontalAlign="Left" Width="70%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblItemid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEM") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Option">
                                        <HeaderStyle HorizontalAlign="Left" Width="30%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblOption" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPTION") %>'></telerik:RadLabel>
                                            <telerik:RadRadioButtonList ID="rblOptionEdit" runat="server" Columns="3" AutoPostBack="true" OnSelectedIndexChanged="rblOptions_SelectedIndexChanged">
                                                <Items>
                                                    <telerik:ButtonListItem Value="1" Text="Yes" />
                                                    <telerik:ButtonListItem Value="0" Text="No" />
                                                    <telerik:ButtonListItem Value="2" Text="N/A" />
                                                </Items>
                                            </telerik:RadRadioButtonList>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblAreAdequateStandByUnit" runat="server" Text="Are Adequate StandBy unit(s) Available ?"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadRadioButtonList ID="rblStandByUnit" runat="server" Columns="3">
                            <Items>
                                <telerik:ButtonListItem Value="1" Text="Yes" />
                                <telerik:ButtonListItem Value="0" Text="No" />
                                <telerik:ButtonListItem Value="2" Text="N/A" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        <telerik:RadLabel ID="lblDetailsofStandByUnitRedundancy" runat="server" Text="Details of StandBy Unit/Redundancy"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBoxList ID="cblStandByUnit" runat="server" Columns="3" AutoPostBack="true" OnTextChanged="OtherDetailClick" />
                        <br />
                        <telerik:RadLabel ID="lblDetails1" runat="server" Text="Details"></telerik:RadLabel>
                        <br />
                        <telerik:RadTextBox ID="txtStandByUnitDetails" runat="server" CssClass="readonlytextbox"
                            Width="700px" Height="80px" onKeyUp="TxtMaxLength(this,700)" onChange="TxtMaxLengththis,700)"
                            TextMode="MultiLine" Rows="3" ReadOnly="true" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <%--<tr>
                            <td colspan="2">
                                Is the effectiveness of the StandBy facility
                                <br />
                                sufficient for the intended operation ?
                            </td>
                            <td colspan="3">
                                <telerik:RadRadioButtonList ID="rblStandByEffective" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                    <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="N/A"></asp:ListItem>
                                </telerik:RadRadioButtonList>
                            </td>
                        </tr>--%>
                <tr valign="top">
                    <td colspan="2">
                        <telerik:RadLabel ID="lblProposedControlstoReduceRisk" runat="server" Text="Proposed Controls to reduce Risk"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBoxList ID="cblProposedControl" runat="server" Columns="3" AutoPostBack="true" OnTextChanged="OtherDetailClick" />
                        <br />
                        <telerik:RadLabel ID="lblDetails2" runat="server" Text="Details"></telerik:RadLabel>
                        <br />
                        <telerik:RadTextBox ID="txtProposedControlDetails" runat="server" CssClass="readonlytextbox"
                            Width="700px" Height="80px" onKeyUp="TxtMaxLength(this,700)" onChange="TxtMaxLengththis,700)"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblCommissioningandTestingProcedures" runat="server" Text="Commissioning and Testing procedures"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBoxList ID="cblCommisionProcedure" runat="server" Columns="3" AutoPostBack="true" OnTextChanged="OtherDetailClick" />
                        <br />
                        <telerik:RadLabel ID="lblDetails3" runat="server" Text="Details"></telerik:RadLabel>
                        <br />
                        <telerik:RadTextBox ID="txtCtDetail" runat="server" CssClass="readonlytextbox" Width="700px"
                            Height="80px" onKeyUp="TxtMaxLength(this,700)" onChange="TxtMaxLengththis,700)"
                            TextMode="MultiLine" Rows="3" ReadOnly="true" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblAdditionalSafetyProcedures" runat="server" Text="Additional Safety Procedures (Emergency)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadGrid ID="gvMachinerySafety" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" ShowHeader="true" ShowFooter="true" EnableViewState="true" OnItemCommand="gvMachinerySafety_ItemCommand"
                            OnItemDataBound="gvMachinerySafety_ItemDataBound" EnableHeaderContextMenu="true" GroupingEnabled="false">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDMACHINERYSAFETYID">
                                <NoRecordsTemplate>
                                    <table runat="server" width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="S.No">
                                        <HeaderStyle HorizontalAlign="Left" Width="7%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblMachinerySafetyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMACHINERYSAFETYID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Number ID="ucSNoEdit" runat="server" CssClass="input" IsInteger="true" Width="95%"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>' />
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <eluc:Number ID="ucSNoAdd" runat="server" CssClass="input" Width="95%" IsInteger="true" />
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Task">
                                        <HeaderStyle HorizontalAlign="Left" Width="25%" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                        <HeaderTemplate>
                                            <telerik:RadLabel ID="lblTask" runat="server">Task</telerik:RadLabel>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblTask" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASK") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblMachinerySafetyIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMACHINERYSAFETYID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <telerik:RadTextBox ID="txtTaskEdit" runat="server" TextMode="MultiLine" Resize="Both" CssClass="input_mandatory"
                                                Width="98%" Height="25%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASK") %>'
                                                onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)">
                                            </telerik:RadTextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtTaskAdd" runat="server" TextMode="MultiLine" Resize="Both" Width="98%" Height="25%"
                                                CssClass="input_mandatory" onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)">
                                            </telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="PIC">
                                        <HeaderStyle HorizontalAlign="Left" Width="22%" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblPIC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <span id="spnPICEdit">
                                                <telerik:RadTextBox ID="txtPICNameEdit" runat="server" CssClass="input" Enabled="false"
                                                    MaxLength="50" Width="95%">
                                                </telerik:RadTextBox>
                                                <telerik:RadTextBox ID="txtPICRankEdit" runat="server" CssClass="input" Enabled="false"
                                                    MaxLength="50" Width="75%">
                                                </telerik:RadTextBox>
                                                <asp:LinkButton ID="btnPICEdit" runat="server">
                                                            <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                                </asp:LinkButton>
                                                <telerik:RadTextBox ID="txtPICIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                            </span>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <span id="spnPICAdd">
                                                <telerik:RadTextBox ID="txtPICNameAdd" runat="server" CssClass="input" Enabled="false" MaxLength="50"
                                                    Width="95%">
                                                </telerik:RadTextBox>
                                                <telerik:RadTextBox ID="txtPICRankAdd" runat="server" CssClass="input" Enabled="false" MaxLength="50"
                                                    Width="75%">
                                                </telerik:RadTextBox>
                                                <asp:LinkButton ID="btnPICAdd" runat="server">
                                                    <span class="icon"><i class="fas fas fa-tasks"></i></span>
                                                </asp:LinkButton>
                                                <telerik:RadTextBox ID="txtPICIdAdd" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                            </span>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Completion Date">
                                        <HeaderStyle HorizontalAlign="Left" Width="17%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblActualFinishDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHDATE")) %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblActualFinishTime" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHTIME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Date ID="ucActualFinishDateEdit" runat="server" CssClass="input_mandatory"
                                                Width="120px" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHDATE")) %>' />
                                            </br>
                                            <telerik:RadTimePicker ID="txtActualFinishTimeEdit" runat="server" Width="80px" CssClass="input_mandatory" DbSelectedDate='<%# Bind("FLDACTUALFINISHTIME") %>'></telerik:RadTimePicker>
                                            hrs
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Remarks">
                                        <HeaderStyle HorizontalAlign="Left" Width="27%" />
                                        <ItemStyle HorizontalAlign="Left" Wrap="true"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadTextBox ID="txtRemarksEdit" runat="server" TextMode="MultiLine" Resize="Both" CssClass="input_mandatory"
                                                Width="98%" Height="25%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                                onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)">
                                            </telerik:RadTextBox>
                                        </EditItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" Width="10%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                                <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="SDELETE" ID="cmdDelete" ToolTip="Delete">
                                                <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="SUPDATE" ID="cmdSave" ToolTip="Save">
                                                <span class="icon"><i class="fas fa-save"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="cmdCancel" ToolTip="Cancel">
                                                <span class="icon"><i class="fas fa-times-circle"></i></span>
                                            </asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Add" CommandName="SADD" ID="cmdAdd" ToolTip="Add">
                                                <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                            </asp:LinkButton>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                        <%--  <telerik:RadTextBox ID="txtAdditionalSafetyProcedures" runat="server" CssClass="input" Width="360px"
                                    Rows="4" TextMode="MultiLine"></telerik:RadTextBox>--%>
                        <telerik:RadEditor ID="txtAdditionalSafetyProcedures" runat="server" Width="100%" Height="225px">
                            <Modules>
                                <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                                <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                            </Modules>
                        </telerik:RadEditor>
                    </td>
                </tr>
                <tr valign="middle">
                    <td colspan="2">
                        <telerik:RadLabel ID="lblApprovedForUse" runat="server" Text="Approved for use,<BR> subject to completion of Tasks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblOtherRiskControl" runat="server"
                            Visible="false">
                        </telerik:RadRadioButtonList>
                        <telerik:RadRadioButtonList ID="rblControlsAdequate" runat="server" Columns="2">
                            <Items>
                                <telerik:ButtonListItem Value="1" Text="Yes" />
                                <telerik:ButtonListItem Value="0" Text="No" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                    <td valign="middle">
                        <telerik:RadLabel ID="lblOverridebyMaster" runat="server" Text="Override by Master"></telerik:RadLabel>
                    </td>
                    <td valign="middle">
                        <telerik:RadCheckBox ID="chkOverrideByMaster" runat="server" Enabled="false" />
                    </td>
                </tr>
                <tr valign="top" runat="server" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblVerification" runat="server" Text="Verification"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadLabel ID="lblVerifcationQuestion" runat="server" Text="Task was completed in timely manner and all control measures adequate?"></telerik:RadLabel>
                        <br />
                        <telerik:RadRadioButtonList ID="rblVerifcation" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblVerifcation_Changed">
                            <Items>
                                <telerik:ButtonListItem Text="Yes" Value="1" />
                                <telerik:ButtonListItem Text="No" Value="0" />
                            </Items>
                        </telerik:RadRadioButtonList>
                        <br />
                        <telerik:RadLabel ID="lblVerifcationRemarks" runat="server" Text="Additional Comments"></telerik:RadLabel>
                        <br />
                        <telerik:RadTextBox ID="txtVerificationRemarks" runat="server" CssClass="input" Width="700px"
                            Height="80px" TextMode="MultiLine" Resize="Both" onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="readonlytextbox" Width="360px"
                            TextMode="MultiLine" Resize="Both" Rows="4" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblReasonsforOverride" runat="server" Text="Reasons for Override"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtMasterRemarks" runat="server" CssClass="readonlytextbox" TextMode="MultiLine"
                            Width="280px" Rows="4" ReadOnly="true" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
