<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAGeneric.aspx.cs"
    Inherits="Inspection_InspectionRAGeneric" MaintainScrollPositionOnPostback="true" %>

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
<%@ Register TagPrefix="eluc" TagName="SubHazard" Src="~/UserControls/UserControlRASubHazardType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Miscellaneous" Src="~/UserControls/UserControlRAMiscellaneous.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Frequency" Src="~/UserControls/UserControlRAFrequency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="CustomEditor" Src="~/UserControls/UserControlEditor.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Assessment Generic</title>
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
    <form id="frmGeneric" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="99.9%">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="Generic" ShowMenu="true" Visible="false" />
            <eluc:TabStrip ID="MenuGeneric" runat="server" OnTabStripCommand="MenuGeneric_TabStripCommand" Title="Generic"></eluc:TabStrip>
            <table border="1" width="99.9%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRefNo" runat="server" Text="Ref.No"></telerik:RadLabel>
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
                        <telerik:RadTextBox runat="server" Width="135px" CssClass="readonlytextbox" ID="txtRevNO"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
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
                    <td>
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
                    <td>
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
                    <td>
                        <telerik:RadLabel ID="lblrejectedby" runat="server" Text="Rejected By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="330px" CssClass="readonlytextbox" ID="txtrejectedby"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblrejecteddate" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucrejecteddate" CssClass="readonlytextbox" runat="server"
                            ReadOnly="true" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="330px"></telerik:RadTextBox>
                        <eluc:Date ID="txtDate" Visible="false" CssClass="readonlytextbox" ReadOnly="false"
                            runat="server" />
                    </td>
                    <td rowspan="6" colspan="2">
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
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucCompany" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="330px" />
                    </td>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDateofIntendedWorkActivity" runat="server" Text="Date of intended Work / Activity"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtIntendedWorkDate" CssClass="input_mandatory" runat="server" DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlCategory" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="330px"
                                AutoPostBack="True" DataTextField="FLDNAME" DataValueField="FLDACTIVITYID" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                <Items>
                                    <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblAmendmentTo" runat="server" Text="Amendment To"></telerik:RadLabel>
                        </td>
                        <td>
                            <span id="spnRAEdit">
                                <telerik:RadTextBox ID="txtRANumberEdit" runat="server" CssClass="input" Enabled="false"
                                    MaxLength="50" Width="80px">
                                </telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtRAEdit" runat="server" CssClass="input" Enabled="false" MaxLength="50"
                                    Width="250px">
                                </telerik:RadTextBox>
                                <asp:LinkButton runat="server" ID="imgShowRAEdit">
                                                <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <telerik:RadTextBox ID="txtRAIdEdit" runat="server" CssClass="hidden" MaxLength="20" Width="0px"></telerik:RadTextBox>
                                <telerik:RadTextBox ID="txtRaTypeEdit" runat="server" CssClass="hidden" MaxLength="2" Width="0px"></telerik:RadTextBox>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblActivityConditionsEquipment" runat="server" Text="Activity"></telerik:RadLabel>
                        </td>
                        <td>
                            <%--<eluc:Miscellaneous ID="ddlActivity" runat="server" CssClass="dropdown_mandatory"
                                    AppendDataBoundItems="true" Type="6" />--%>
                            <telerik:RadTextBox ID="txtActivityCondition" runat="server" CssClass="input_mandatory"
                                Width="330px">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            <telerik:RadLabel ID="lblReasonForAssessment" runat="server" Text="Reason for Assessment"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadCheckBoxList ID="cblReason" runat="server" Columns="3" Direction="Vertical"
                                AutoPostBack="true" OnTextChanged="OtherDetailClick">
                            </telerik:RadCheckBoxList>
                            <br />
                            <telerik:RadTextBox ID="txtOtherReason" runat="server" CssClass="readonlytextbox" Width="700px" Resize="Both"
                                Height="80px" TextMode="MultiLine" ReadOnly="true" onKeyUp="TxtMaxLength(this,700)"
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
                        <td>
                            <telerik:RadLabel ID="lblNoofPeopleInvolvedInActivity" runat="server" Text="No of people involved in activity / Affected"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadRadioButtonList ID="rblPeopleInvolved" runat="server" Direction="Vertical" Columns="4">
                            </telerik:RadRadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDurationOfWorkActivity" runat="server" Text="Duration of work / activity"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadRadioButtonList ID="rblWorkDuration" runat="server" Direction="Horizontal" />
                        </td>
                    </tr>
                    <tr valign="top">
                        <td>
                            <telerik:RadLabel ID="lblFrequencyofWorkActivity" runat="server" Text="Frequency of work / activity"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadRadioButtonList ID="rblWorkFrequency" runat="server" Direction="Horizontal" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle">
                            <telerik:RadLabel ID="lblRisksAspects" runat="server" Text="Risks/Aspects"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <%--      <telerik:RadTextBox ID="txtWorkDetails" runat="server" CssClass="input_mandatory" Width="360px" Resize="Both"
                                        Rows="4" TextMode="MultiLine"></telerik:RadTextBox>--%>
                            <telerik:RadEditor ID="txtWorkDetails" runat="server" Width="99.9%" Height="225px">
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
                        <td>
                            <telerik:RadLabel ID="lblHealthandSafetyImpact" runat="server" Text="Health and Safety Impact"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadLabel ID="lblHazardType" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></telerik:RadLabel>
                            <eluc:Hazard ID="ucHazardType" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="200px"
                                Type="1" AutoPostBack="true" OnTextChangedEvent="ucHazardType_TextChangedEvent" />
                            <br />
                            <br />
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvHealthSafetyRisk" runat="server" AutoGenerateColumns="False" GroupingEnabled="false" EnableHeaderContextMenu="true"
                                Font-Size="11px" Width="99.9%" CellPadding="3" OnItemCommand="gvHealthSafetyRisk_ItemCommand" OnNeedDataSource="gvHealthSafetyRisk_NeedDataSource"
                                ShowHeader="true" ShowFooter="true" EnableViewState="false" OnItemDataBound="gvHealthSafetyRisk_ItemDataBound">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false">
                                    <ColumnGroups>
                                        <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                    </ColumnGroups>
                                    <NoRecordsTemplate>
                                        <table width="99.9%" border="0">
                                            <tr>
                                                <td align="center">
                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                    <HeaderStyle Width="102px" />
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Hazard Type">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Impact">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblCategoryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadComboBox ID="ddlSubHazardType" runat="server" AppendDataBoundItems="true" Width="200px"
                                                    CssClass="dropdown_mandatory" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Value="Dummy" Text="--Select--"></telerik:RadComboBoxItem>
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn Visible="false">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblControlHeader" runat="server">Control</telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <eluc:Frequency ID="ddlControlAdd" runat="server" CssClass="dropdown_mandatory" FrequencyList='<%# PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4)%>'
                                                    AppendDataBoundItems="true" Type="4" />
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn Visible="false">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblProposedControlsToReduceRiskHeader" runat="server">Proposed Controls To reduce risk</telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblProposedControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDCONTROLS") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadTextBox ID="txtProposedControlAdd" runat="server" CssClass="input" Width="99.9%"></telerik:RadTextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            <FooterStyle HorizontalAlign="Center" Width="100px" />
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="HDELETE" ID="cmdDelete"
                                                    ToolTip="Delete">
                                                <span class="icon"><i class="fas fa-trash"></i></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="HADD" ID="cmdAdd"
                                                    ToolTip="Add">
                                            <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                </asp:LinkButton>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                    <tr valign="middle">
                        <td>
                            <telerik:RadLabel ID="lblEnvironmentalImpact" runat="server" Text="Environmental Impact"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadCheckBoxList ID="cblImpact" runat="server" Direction="Horizontal" Visible="false">
                                <Items>
                                    <telerik:ButtonListItem Text="Direct" Value="1"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Text="Indirect" Value="0"></telerik:ButtonListItem>
                                </Items>
                            </telerik:RadCheckBoxList>
                            <telerik:RadLabel ID="lblHazardType1" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></telerik:RadLabel>
                            <eluc:Hazard ID="ucEnvHazardType" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="200px"
                                Type="2" AutoPostBack="true" OnTextChangedEvent="ucEnvHazardType_OnTextChangedEvent" />
                            <br />
                            <br />
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvEnvironmentalRisk" runat="server" AutoGenerateColumns="False" GroupingEnabled="false" EnableHeaderContextMenu="true"
                                Font-Size="11px" Width="99.9%" CellPadding="3" ShowHeader="true" ShowFooter="true" OnNeedDataSource="gvEnvironmentalRisk_NeedDataSource"
                                EnableViewState="false" OnItemDataBound="gvEnvironmentalRisk_ItemDataBound" OnItemCommand="gvEnvironmentalRisk_ItemCommand">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false">
                                    <ColumnGroups>
                                        <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                    </ColumnGroups>
                                    <NoRecordsTemplate>
                                        <table width="99.9%" border="0">
                                            <tr>
                                                <td align="center">
                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                    <HeaderStyle Width="102px" />
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Hazard Type">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Impact Type">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblImpactType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACTNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <eluc:Miscellaneous ID="ucImpactType" AppendDataBoundItems="true" runat="server" Width="200px"
                                                    CssClass="input_mandatory" Type="3" IncludeOthers="false" />
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Impact">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblCategoryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadComboBox ID="ddlSubHazardType" runat="server" AppendDataBoundItems="true" Width="200px"
                                                    CssClass="dropdown_mandatory" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Value="Dummy" Text="--Select--"></telerik:RadComboBoxItem>
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn Visible="false">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblControlHeader" runat="server">Control</telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <eluc:Frequency ID="ddlControlAdd" runat="server" CssClass="dropdown_mandatory" FrequencyList='<%# PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4)%>'
                                                    AppendDataBoundItems="true" Type="4" />
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn Visible="false">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblProposedControlsToReduceRiskHeader" runat="server">Proposed Controls To reduce risk</telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblProposedControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDCONTROLS") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadTextBox ID="txtProposedControlAdd" runat="server" CssClass="input"></telerik:RadTextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            <FooterStyle HorizontalAlign="Center" Width="100px" />
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="EDELETE" ID="cmdDelete"
                                                    ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="EADD" ID="cmdAdd"
                                                    ToolTip="Add">
                                            <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                </asp:LinkButton>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblEconomicImpact" runat="server" Text="Economic Impact"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadLabel ID="lblHazard2" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></telerik:RadLabel>
                            <eluc:Hazard ID="ucEconomicHazardType" runat="server" AppendDataBoundItems="true" Width="200px"
                                CssClass="dropdown_mandatory" Type="4" AutoPostBack="true" OnTextChangedEvent="ucEconomicHazardType_OnTextChangedEvent" />
                            <br />
                            <br />
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvEconomicRisk" runat="server" AutoGenerateColumns="False" Font-Size="11px" GroupingEnabled="false" EnableHeaderContextMenu="true"
                                Width="99.9%" ShowHeader="true" ShowFooter="true" EnableViewState="false" OnNeedDataSource="gvEconomicRisk_NeedDataSource"
                                OnItemDataBound="gvEconomicRisk_ItemDataBound" OnItemCommand="gvEconomicRisk_ItemCommand">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false">
                                    <ColumnGroups>
                                        <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                    </ColumnGroups>
                                    <NoRecordsTemplate>
                                        <table width="99.9%" border="0">
                                            <tr>
                                                <td align="center">
                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                    <HeaderStyle Width="102px" />
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="Hazard Type">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Impact">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblCategoryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadComboBox ID="ddlSubHazardType" runat="server" AppendDataBoundItems="true" Width="200px"
                                                    CssClass="dropdown_mandatory" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                                    <Items>
                                                        <telerik:RadComboBoxItem Value="Dummy" Text="--Select--"></telerik:RadComboBoxItem>
                                                    </Items>
                                                </telerik:RadComboBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn Visible="false">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblControlHeader" runat="server">Control</telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <eluc:Frequency ID="ddlControlAdd" runat="server" CssClass="dropdown_mandatory" FrequencyList='<%# PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4) %>'
                                                    AppendDataBoundItems="true" Type="4" />
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn Visible="false">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <telerik:RadLabel ID="lblProposedControlsToReduceRiskHeader" runat="server">Proposed Controls To reduce risk</telerik:RadLabel>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblProposedControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDCONTROLS") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadTextBox ID="txtProposedControlAdd" runat="server" CssClass="input"></telerik:RadTextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            <FooterStyle HorizontalAlign="Center" Width="100px" />
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="CDELETE" ID="cmdDelete"
                                                    ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="CADD" ID="cmdAdd"
                                                    ToolTip="Add">
                                                <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                </asp:LinkButton>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                        </td>
                    </tr>
                    <tr valign="middle">
                        <td>
                            <telerik:RadLabel ID="lblWorstCaseScenario" runat="server" Text="Worst Case Scenario"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadCheckBoxList ID="cblOtherRisk" runat="server" Direction="Vertical" Columns="4"
                                AutoPostBack="true" OnTextChanged="OtherDetailClick">
                            </telerik:RadCheckBoxList>
                            <%--<br />
                                Details
                                <br />--%>
                            <telerik:RadTextBox ID="txtOtherDetails" runat="server" CssClass="readonlytextbox" Width="360px"
                                Visible="false" ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr valign="middle">
                        <td>
                            <telerik:RadLabel ID="lblProposedControlsToReduceRisk" runat="server" Text="Proposed Controls To Reduce Risk"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadCheckBoxList ID="cblProposedControl" runat="server" Direction="Vertical" Columns="2"
                                RepeatColumns="2" AutoPostBack="true" OnTextChanged="OtherDetailClick" />
                            <br />
                            <telerik:RadLabel ID="lblDetails" runat="server" Text="Details"></telerik:RadLabel>
                            <br />
                            <telerik:RadTextBox ID="txtProposedControlDetails" runat="server" CssClass="readonlytextbox" Resize="Both"
                                TextMode="MultiLine" Rows="3" Width="360px" ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle">
                            <telerik:RadLabel ID="lblAdditionalSaftyProcedures" runat="server" Text=" Additional Safety Procedures (Emergency)"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadGrid RenderMode="Lightweight" ID="gvGenericSafety" runat="server" AutoGenerateColumns="False" Font-Size="11px" GroupingEnabled="false" EnableHeaderContextMenu="true"
                                Width="99.9%" ShowHeader="true" ShowFooter="true" EnableViewState="false" OnNeedDataSource="gvGenericSafety_NeedDataSource"
                                OnItemDataBound="gvGenericSafety_ItemDataBound" OnItemCommand="gvGenericSafety_ItemCommand">
                                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                    AutoGenerateColumns="false">
                                    <ColumnGroups>
                                        <telerik:GridColumnGroup Name="FromToPort" HeaderText="Port" HeaderStyle-HorizontalAlign="Center"></telerik:GridColumnGroup>
                                    </ColumnGroups>
                                    <NoRecordsTemplate>
                                        <table width="99.9%" border="0">
                                            <tr>
                                                <td align="center">
                                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                </td>
                                            </tr>
                                        </table>
                                    </NoRecordsTemplate>
                                    <HeaderStyle Width="102px" />
                                    <Columns>
                                        <telerik:GridTemplateColumn HeaderText="S.No" HeaderStyle-Width="63px">
                                            <ItemStyle Wrap="False" HorizontalAlign="Right" Width="1%"></ItemStyle>
                                            <FooterStyle Wrap="false" HorizontalAlign="Right" Width="1%" />
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblGenericSafetyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGENERICSAFETYID") %>'
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
                                        <telerik:GridTemplateColumn HeaderText="Task" HeaderStyle-Width="200px">
                                            <ItemStyle HorizontalAlign="Left" Wrap="true" Width="40%"></ItemStyle>
                                            <FooterStyle Wrap="true" HorizontalAlign="Left" Width="40%" />
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblTask" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASK") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadTextBox ID="txtTaskEdit" runat="server" TextMode="MultiLine" CssClass="input_mandatory" Resize="Both"
                                                    Width="98%" Height="25%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASK") %>'
                                                    onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)">
                                                </telerik:RadTextBox>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <telerik:RadTextBox ID="txtTaskAdd" runat="server" TextMode="MultiLine" Width="98%" Height="25%" Resize="Both"
                                                    CssClass="input_mandatory" onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)"
                                                    s>
                                                </telerik:RadTextBox>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="PIC">
                                            <ItemStyle Wrap="true" HorizontalAlign="Left" Width="12%"></ItemStyle>
                                            <FooterStyle Wrap="true" HorizontalAlign="Left" Width="12%" />
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblPIC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <span id="spnPICEdit">
                                                    <telerik:RadTextBox ID="txtPICNameEdit" runat="server" CssClass="input" Enabled="false"
                                                        MaxLength="50" Width="50%">
                                                    </telerik:RadTextBox>
                                                    <telerik:RadTextBox ID="txtPICRankEdit" runat="server" CssClass="input" Enabled="false"
                                                        MaxLength="50" Width="40%">
                                                    </telerik:RadTextBox>
                                                    <asp:LinkButton runat="server" ID="btnPICEdit">
                                                <span class="icon"><i class="fas fa-tasks"></i></span>
                                                    </asp:LinkButton>
                                                    <telerik:RadTextBox ID="txtPICIdEdit" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                                </span>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <span id="spnPICAdd">
                                                    <telerik:RadTextBox ID="txtPICNameAdd" runat="server" CssClass="input" Enabled="false" MaxLength="50"
                                                        Width="50%">
                                                    </telerik:RadTextBox>
                                                    <telerik:RadTextBox ID="txtPICRankAdd" runat="server" CssClass="input" Enabled="false" MaxLength="50"
                                                        Width="40%">
                                                    </telerik:RadTextBox>
                                                    <asp:LinkButton runat="server" ID="btnPICAdd">
                                                <span class="icon"><i class="fas fa-tasks"></i></span>
                                                    </asp:LinkButton>
                                                    <telerik:RadTextBox ID="txtPICIdAdd" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                                </span>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="EstimatedStart Date" HeaderStyle-Width="145px">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                            <FooterStyle Wrap="False" HorizontalAlign="Left" Width="7%" />
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblEstimatedStartDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDESTIMATEDSTARTDATE")) %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblEstimatedStartTime" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDESTIMATEDSTARTTIME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Date ID="ucEstimatedStartDateEdit" runat="server" CssClass="input_mandatory"
                                                    Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDESTIMATEDSTARTDATE")) %>' />
                                                </br>
                                                    <telerik:RadTextBox ID="txtEstimatedStartTimeEdit" runat="server" CssClass="input" Width="15%" />
                                                <telerik:RadLabel ID="lblEstimatedStartTimeEdithrs" runat="server" Text="hrs"></telerik:RadLabel>
                                                <%--     <ajaxToolkit:MaskedEditExtender ID="txtEstimatedStartTimeEditMask" runat="server"
                                            AcceptAMPM="false" ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99"
                                            MaskType="Time" TargetControlID="txtEstimatedStartTimeEdit" UserTimeFormat="TwentyFourHour" />--%>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <eluc:Date ID="ucEstimatedStartDateAdd" runat="server" CssClass="input_mandatory" />
                                                </br>
                                                    <telerik:RadTextBox ID="txtEstimatedStartTimeAdd" runat="server" CssClass="input" Width="30px" />
                                                <telerik:RadLabel ID="lblEstimatedStartDateTimeAddhrs" runat="server" Text="hrs"></telerik:RadLabel>
                                                <%--    <ajaxToolkit:MaskedEditExtender ID="txtEstimatedStartTimeAddMask" runat="server"
                                            AcceptAMPM="false" ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99"
                                            MaskType="Time" TargetControlID="txtEstimatedStartTimeAdd" UserTimeFormat="TwentyFourHour" />--%>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="EstimatedFinish Date" HeaderStyle-Width="145px">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                            <FooterStyle Wrap="False" HorizontalAlign="Left" Width="7%" />
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblEstimatedFinishDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDESTIMATEDFINISHDATE")) %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblEstimatedFinishTime" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDESTIMATEDFINISHTIME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Date ID="ucEstimatedFinishDateEdit" runat="server" CssClass="input_mandatory"
                                                    Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDESTIMATEDFINISHDATE")) %>' />
                                                </br>
                                                    <telerik:RadTextBox ID="txtEstimatedFinishTimeEdit" runat="server" CssClass="input" Width="30px" />
                                                <telerik:RadLabel ID="lblEstimatedFinishDateTimeEdithrs" runat="server" Text="hrs"></telerik:RadLabel>
                                                <%-- <ajaxToolkit:MaskedEditExtender ID="txtEstimatedFinishTimeEditMask" runat="server"
                                            AcceptAMPM="false" ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99"
                                            MaskType="Time" TargetControlID="txtEstimatedFinishTimeEdit" UserTimeFormat="TwentyFourHour" />--%>
                                            </EditItemTemplate>
                                            <FooterTemplate>
                                                <eluc:Date ID="ucEstimatedFinishDateAdd" runat="server" CssClass="input_mandatory" />
                                                </br>
                                                    <telerik:RadTextBox ID="txtEstimatedFinishTimeAdd" runat="server" CssClass="input" Width="30px" />
                                                <telerik:RadLabel ID="lblEstimatedFinishDateTimeAddhrs" runat="server" Text="hrs"></telerik:RadLabel>
                                                <%--    <ajaxToolkit:MaskedEditExtender ID="txtEstimatedFinishTimeAddMask" runat="server"
                                            AcceptAMPM="false" ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99"
                                            MaskType="Time" TargetControlID="txtEstimatedFinishTimeAdd" UserTimeFormat="TwentyFourHour" />--%>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="ActualStart Date" HeaderStyle-Width="145px">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                            <FooterStyle Wrap="False" HorizontalAlign="Left" Width="7%" />
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblActualStartDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDACTUALSTARTDATE")) %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblActualStartTime" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDACTUALSTARTTIME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <telerik:RadLabel ID="lblGenericSafetyIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGENERICSAFETYID") %>'
                                                    Visible="false">
                                                </telerik:RadLabel>
                                                <eluc:Date ID="ucActualStartDateEdit" runat="server" CssClass="input"
                                                    Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDACTUALSTARTDATE")) %>' />
                                                </br>
                                                    <telerik:RadTextBox ID="txtActualStartTimeEdit" runat="server" CssClass="input" Width="30px" />
                                                <telerik:RadLabel ID="lblActualStartTimeEdithrs" runat="server" Text="hrs"></telerik:RadLabel>
                                                <%--     <ajaxToolkit:MaskedEditExtender ID="txtActualStartTimeEditMask" runat="server" AcceptAMPM="false"
                                            ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                            TargetControlID="txtActualStartTimeEdit" UserTimeFormat="TwentyFourHour" />--%>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="ActualFinish Date" HeaderStyle-Width="145px">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                            <FooterStyle Wrap="False" HorizontalAlign="Left" Width="8%" />
                                            <ItemTemplate>
                                                <telerik:RadLabel ID="lblActualFinishDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHDATE")) %>'></telerik:RadLabel>
                                                <telerik:RadLabel ID="lblActualFinishTime" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHTIME") %>'></telerik:RadLabel>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <eluc:Date ID="ucActualFinishDateEdit" runat="server" CssClass="input"
                                                    Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHDATE")) %>' />
                                                </br>
                                                    <telerik:RadTextBox ID="txtActualFinishTimeEdit" runat="server" CssClass="input" Width="30px" />
                                                <telerik:RadLabel ID="lblActualFinishDateTimeEdithrs" runat="server" Text="hrs"></telerik:RadLabel>
                                                <%--   <ajaxToolkit:MaskedEditExtender ID="txtActualFinishTimeEditMask" runat="server" AcceptAMPM="false"
                                            ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                            TargetControlID="txtActualFinishTimeEdit" UserTimeFormat="TwentyFourHour" />--%>
                                            </EditItemTemplate>
                                        </telerik:GridTemplateColumn>
                                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="5%"></ItemStyle>
                                            <FooterStyle HorizontalAlign="Center" Width="5%" />
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                                    ToolTip="Edit">
                                                <span class="icon"><i class="fas fa-edit"></i></span>
                                                </asp:LinkButton>
                                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="SDELETE" ID="cmdDelete"
                                                    ToolTip="Delete">
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
                                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="SADD" ID="cmdAdd"
                                                    ToolTip="Add">
                                             <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                </asp:LinkButton>
                                            </FooterTemplate>
                                        </telerik:GridTemplateColumn>
                                    </Columns>
                                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                                </MasterTableView>
                                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                </ClientSettings>
                            </telerik:RadGrid>
                            <%--  <telerik:RadTextBox ID="txtAdditionalSafetyProcedures" runat="server" CssClass="input" Width="360px"
                                        Rows="4" TextMode="MultiLine"></telerik:RadTextBox>--%>
                            <telerik:RadEditor ID="txtAdditionalSafetyProcedures" runat="server" Width="99.9%" Height="225px" Visible="false">
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
                        <td>
                            <telerik:RadLabel ID="lblApprovedforUse" runat="server" Text="Approved for Use"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="rblOtherRiskControl" runat="server" Direction="Horizontal"
                                Visible="false">
                            </telerik:RadRadioButtonList>
                            <telerik:RadRadioButtonList ID="rblControlsAdequate" runat="server" Direction="Horizontal">
                                <Items>
                                    <telerik:ButtonListItem Value="1" Text="Yes"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Value="0" Text="No"></telerik:ButtonListItem>
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
                    <tr valign="middle">
                        <td>
                            <telerik:RadLabel ID="lblVerification" runat="server" Text="Verification"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadLabel ID="lblVerifcationQuestion" runat="server" Text="Task was completed in timely manner and all control measures adequate?"></telerik:RadLabel>
                            <br />
                            <telerik:RadRadioButtonList ID="rblVerifcation" runat="server" Direction="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblVerifcation_Changed">
                                <Items>
                                    <telerik:ButtonListItem Text="Yes" Value="1"></telerik:ButtonListItem>
                                    <telerik:ButtonListItem Text="No" Value="0"></telerik:ButtonListItem>
                                </Items>
                            </telerik:RadRadioButtonList>
                            <br />
                            <telerik:RadLabel ID="lblVerifcationRemarks" runat="server" Text="Additional Comments"></telerik:RadLabel>
                            <br />
                            <telerik:RadTextBox ID="txtVerificationRemarks" runat="server" CssClass="input" Width="700px" Resize="Both"
                                Height="80px" TextMode="MultiLine" onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr valign="middle">
                        <td>
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtRemarks" runat="server" CssClass="readonlytextbox" Width="360px" Resize="Both"
                                TextMode="MultiLine" Rows="4" ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                        <td valign="top">
                            <telerik:RadLabel ID="lblReasonsforOverride" runat="server" Text="Reasons for Override"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtMasterRemarks" runat="server" CssClass="readonlytextbox" TextMode="MultiLine" Resize="Both"
                                Width="280px" Rows="4" ReadOnly="true">
                            </telerik:RadTextBox>
                        </td>
                    </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
