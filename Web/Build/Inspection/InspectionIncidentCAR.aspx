<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentCAR.aspx.cs"
    Inherits="InspectionIncidentCAR" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Incident CAR</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <style type="text/css">
            .style1 {
                width: 368px;
            }

            .style2 {
                width: 33px;
            }

            .style3 {
                width: 3%;
            }
        </style>

        <script language="javascript" type="text/javascript">
            function TxtMaxLength(text, maxLength) {
                text.value = text.value.substring(0, maxLength);
            }
        </script>
        <script type="text/javascript">
            function Confirm(args) {
                if (args) {
                    __doPostBack("<%=ucConfirm.UniqueID %>", "");
                }
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmIncidentCAR" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuInspectionNonConformity" runat="server" OnTabStripCommand="InspectionNonConformity_TabStripCommand" Title="Details"></eluc:TabStrip>
            <table id="tblInspectionObservation" width="100%" style="z-index: +5;" border="0">
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblSummaryDescription" runat="server" Text="Summary Description"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtSummaryDesc" runat="server" CssClass="input" Enabled="false"
                            Height="50px" Rows="4" TextMode="MultiLine" Width="97%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblComprehensiveDescription" runat="server" Text="Comprehensive Description"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtComprehensiveDesc" runat="server" Rows="50" CssClass="input"
                            Enabled="false" Height="100px" TextMode="MultiLine" Width="97%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <br />
                        <span style="color: Black; font-weight: bold">
                            <telerik:RadLabel ID="lblImmediateActionTakenOnboard" runat="server" Text="Immediate Action Taken Onboard"></telerik:RadLabel>
                        </span>
                        <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblImmediateAction" runat="server" Text="Immediate Action"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtImmediateActionTaken" CssClass="input" Enabled="false"
                            TextMode="MultiLine" Height="60px" Width="80%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblAssignedTo" runat="server" Text="Assigned to"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnAssignedTo">
                            <telerik:RadTextBox ID="txtImmediateAssignedToName" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="53%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtImmediateAssignedToRank" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="26%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgImmediateAssignedTo" Style="cursor: pointer; vertical-align: top" Visible="false">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtImmediateAssignedToId" runat="server" CssClass="input" MaxLength="20"
                                Width="10px">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblMaintenanceRequired" runat="server" Text="Maintenance Required"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkMaintenanceRequired" runat="server" Enabled="false" />
                    </td>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblWONumber" runat="server" Text="WO Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtWorkOrderNumber" runat="server" CssClass="input" Height="20px"
                            Rows="3" Enabled="false" Resize="Both" Width="80%" TextMode="MultiLine">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <br />
                        <span style="color: Black; font-weight: bold">
                            <telerik:RadLabel ID="RadLabelOnboard" runat="server" Text="Onboard Investigation"></telerik:RadLabel>
                        </span>
                        <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInvestigatedBy" runat="server" Text="Investigated By Lead Investigator "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbl1" runat="server" Text="1"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnReportedByShip1">
                            <telerik:RadTextBox ID="txtReportedByShipname1" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="40%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtReportedByShipRank1" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="37%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgReportedByShip1" Style="cursor: pointer; vertical-align: top" Visible="false">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtReportedByShipId1" runat="server" CssClass="input" MaxLength="20"
                                Width="0px">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                    <td colspan="3"></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lbl2" runat="server" Text="2"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnReportedByShip2">
                            <telerik:RadTextBox ID="txtReportedByShipname2" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="40%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtReportedByShipRank2" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="37%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgReportedByShip2" Style="cursor: pointer; vertical-align: top" Visible="false">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtReportedByShipId2" runat="server" CssClass="input" MaxLength="20"
                                Width="0px">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                    <td colspan="3"></td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <telerik:RadLabel ID="lbl3" runat="server" Text="3"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnReportedByShip3">
                            <telerik:RadTextBox ID="txtReportedByShipname3" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="40%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtReportedByShipRank3" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="37%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgReportedByShip3" Style="cursor: pointer; vertical-align: top" Visible="false">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox ID="txtReportedByShipId3" runat="server" CssClass="input" MaxLength="20"
                                Width="0px">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                    <td colspan="3"></td>
                </tr>
                <tr>
                    <td colspan="6"></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblStatusofVentilation" runat="server" Text="Status of Ventilation"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucVentilation" runat="server" AppendDataBoundItems="true" CssClass="input"
                            Enabled="false" QuickTypeCode="60" Width="278px" />
                    </td>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblLightingConditions" runat="server" Text="Lighting Conditions"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucLightingCond" runat="server" AppendDataBoundItems="true" CssClass="input"
                            Enabled="false" QuickTypeCode="49" Width="79%" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblContractorRelatedIncident" runat="server" Text="Contractor related Incident"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkContractorIncident" runat="server" Enabled="false" AutoPostBack="true"
                            OnCheckedChanged="chkContractorIncident_CheckedChanged" />
                    </td>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblContractorDetails" runat="server" Text="Contractor Details"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtContractorDetails" runat="server" CssClass="input" Enabled="false"
                            Height="60px" Width="80%" TextMode="MultiLine" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblDrugAlcoholTestDone" runat="server" Text="Drug/Alcohol Test Done?"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkAlcoholtest" runat="server" Enabled="false" />
                        &nbsp;
                    </td>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblTestDate" runat="server" Text="Test Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txttestDate" runat="server" CssClass="input" Enabled="false" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblInvestigation" runat="server" Text="Investigation"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <telerik:RadGrid ID="gvInvestigation" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="98%" CellPadding="3" ShowHeader="true" EnableViewState="false" ShowFooter="true">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDQUESTIONID">
                                <NoRecordsTemplate>
                                    <table runat="server" width="100%" border="0">
                                        <tr>
                                            <td align="center">
                                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                    </table>
                                </NoRecordsTemplate>
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Question">
                                        <HeaderStyle Width="90%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblQuestion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTION") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblInvestigationId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCIDENTINVESTIGATIONID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblQuestionId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUESTIONID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Answer">
                                        <HeaderStyle Width="10%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblAnswer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDANSWERNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <br />
                        <span style="color: Black; font-weight: bold">
                            <telerik:RadLabel ID="lblShoreInvestigation" runat="server" Text="Shore Investigation"></telerik:RadLabel>
                        </span>
                        <hr style="height: -12px" />
                    </td>
                </tr>
                <tr>
                    <td valign="baseline">
                        <telerik:RadLabel ID="lblInvestigationTeamAppointedBy" runat="server" Text="Investigation Team Appointed By"></telerik:RadLabel>
                    </td>
                    <td></td>
                    <td valign="baseline">
                        <span id="spnShoreTeamAppointedBy">
                            <telerik:RadTextBox ID="txtShoreTeamAppointedByName" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="40%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtShoreTeamAppointedByDesignation" runat="server" CssClass="input"
                                Enabled="false" MaxLength="50" Width="37%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgShoreTeamAppointedBy" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox runat="server" ID="txtShoreTeamAppointedById" CssClass="input" Width="0px"
                                MaxLength="20">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtShoreTeamAppointedByEmail" CssClass="input" Width="0px"
                                MaxLength="20">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblDetailedInvestigationRequired" runat="server" Text="Detailed Investigation Required"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkDetailsInvestigationYN" runat="server" OnCheckedChanged="chkDetailsInvestigationYN_CheckedChanged"
                            AutoPostBack="true" Enabled="false" />
                        <asp:LinkButton ID="imgSupdtEventFeedback" Visible="false" CommandName="SUPDTEVENTFEEDBACK" OnClick="SupdtFeedback_Click"
                            runat="server" ToolTip="Supdt Event Feedback">
                            <span class="icon"><i class="fas fa-user-tie"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td valign="baseline">
                        <telerik:RadLabel ID="lblInvestigatedBy1" runat="server" Text="Investigated By Lead Investigator "></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lbl11" runat="server" Text="1"></telerik:RadLabel>
                    </td>
                    <td valign="baseline">
                        <span id="spnReportedByShore1">
                            <telerik:RadTextBox ID="txtReportedByShoreName1" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="40%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtReportedByShoreDesignation1" runat="server" CssClass="input"
                                Enabled="false" MaxLength="50" Width="37%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgReportedByShore" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox runat="server" ID="txtReportedByShoreId1" CssClass="input" Width="0px"
                                MaxLength="20">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtReportedByShoreEmail1" CssClass="input" Width="0px"
                                MaxLength="20">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                    <td colspan="3"></td>
                </tr>
                <tr>
                    <td valign="baseline"></td>
                    <td valign="baseline">
                        <telerik:RadLabel ID="lbl22" runat="server" Text="2"></telerik:RadLabel>
                    </td>
                    <td valign="baseline">
                        <span id="spnReportedByShore2">
                            <telerik:RadTextBox ID="txtReportedByShoreName2" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="40%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtReportedByShoreDesignation2" runat="server" CssClass="input"
                                Enabled="false" MaxLength="50" Width="37%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgReportedByShore2" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox runat="server" ID="txtReportedByShoreId2" CssClass="input" Width="0px"
                                MaxLength="20">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtReportedByShoreEmail2" CssClass="input" Width="0px"
                                MaxLength="20">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                    <td colspan="3"></td>
                </tr>
                <tr>
                    <td valign="baseline"></td>
                    <td valign="baseline">
                        <telerik:RadLabel ID="lbl33" runat="server" Text="3"></telerik:RadLabel>
                    </td>
                    <td valign="baseline">
                        <span id="spnReportedByShore3">
                            <telerik:RadTextBox ID="txtReportedByShoreName3" runat="server" CssClass="input" Enabled="false"
                                MaxLength="50" Width="40%">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtReportedByShoreDesignation3" runat="server" CssClass="input"
                                Enabled="false" MaxLength="50" Width="37%">
                            </telerik:RadTextBox>
                            <asp:LinkButton runat="server" ID="imgReportedByShore3" Style="cursor: pointer; vertical-align: top">
                                <span class="icon"><i class="fas fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox runat="server" ID="txtReportedByShoreId3" CssClass="input" Width="0px"
                                MaxLength="20">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox runat="server" ID="txtReportedByShoreEmail3" CssClass="input" Width="0px"
                                MaxLength="20">
                            </telerik:RadTextBox>
                        </span>
                    </td>
                    <td colspan="3"></td>
                </tr>
                <tr>
                    <td colspan="6">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblExecutivesummary" runat="server" Text="Executive Summary"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtExecutiveSummary" runat="server" Enabled="false" CssClass="readonlytextbox"
                            Height="50px" Rows="4" TextMode="MultiLine" Width="97%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblPostIncident" runat="server" Text="Post Incident"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtPostIncident" runat="server" CssClass="readonlytextbox" Height="50px"
                            Enabled="false" Rows="4" TextMode="MultiLine" Width="97%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblReviewofEmergencyProcedures" runat="server" Text="Review of Emergency Procedures"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtReviewOfEmergencyProcedures" runat="server" Enabled="false" CssClass="readonlytextbox"
                            Height="50px" Rows="4" TextMode="MultiLine" Width="97%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblInvestigationEvidence" runat="server" Text="Investigation &amp; Evidence"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtInvestigationAndEvidence" runat="server" CssClass="input" Height="100px"
                            Rows="50" TextMode="MultiLine" Width="97%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblReviewRemarks" runat="server" Text="Review Remarks"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <telerik:RadTextBox ID="txtReviewRemarks" CssClass="input_mandatory" Enabled="true" Height="100px"
                            Rows="50" TextMode="MultiLine" Width="97%" runat="server" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblTimeline" runat="server" Text="TimeLine"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <telerik:RadGrid ID="gvInspection" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowCustomPaging="true" AllowPaging="true"
                            Width="98%" CellPadding="3" OnItemCommand="gvInspection_ItemCommand" OnItemDataBound="gvInspection_ItemDataBound" OnSortCommand="gvInspection_SortCommand"
                            ShowFooter="true" ShowHeader="true" EnableViewState="false" AllowSorting="true" OnUpdateCommand="gvInspection_UpdateCommand"
                            OnDeleteCommand="gvInspection_DeleteCommand" OnNeedDataSource="gvInspection_NeedDataSource">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDTIMELINEID">
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="S.No" AllowSorting="true" SortExpression="FLDSERIALNUMBER">
                                        <HeaderStyle Width="5%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblSNO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'>
                                            </telerik:RadLabel>
                                            <telerik:RadLabel ID="lblTimeLineId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTIMELINEID") %>' Visible="false">
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadLabel ID="lblTimeLineId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTIMELINEID") %>'
                                                Visible="false">
                                            </telerik:RadLabel>
                                            <eluc:Number ID="ucSNOEdit" runat="server" CssClass="gridinput_mandatory" MaxLength="3"
                                                IsPositive="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></eluc:Number>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <eluc:Number ID="ucSNOAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="3"
                                                IsPositive="true"></eluc:Number>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Date / Time">
                                        <HeaderStyle Width="32%" HorizontalAlign="Left" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEVENTDATE")) %>'></telerik:RadLabel>
                                            &nbsp
                                            <telerik:RadTimePicker ID="txtTime" runat="server" CssClass="readonlytextbox" Enabled="false" Width="80px" DbSelectedDate='<%# Bind("FLDEVENTDATETIME") %>'>
                                            </telerik:RadTimePicker>
                                            hrs

                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <eluc:Date ID="ucDateEdit" CssClass="gridinput_mandatory" runat="server"
                                                Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEVENTDATE")) %>'
                                                DatePicker="true" Width="120px" />
                                            &nbsp
                                            <telerik:RadTimePicker ID="txtTimeEdit" runat="server" Width="80px" CssClass="input_mandatory" DbSelectedDate='<%# Bind("FLDEVENTDATETIME") %>'></telerik:RadTimePicker>
                                            hrs
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <eluc:Date ID="ucDateAdd" CssClass="gridinput_mandatory" runat="server"
                                                DatePicker="true" Width="118px" />
                                            &nbsp
                                            <telerik:RadTimePicker ID="txtTimeAdd" runat="server" Width="70px" CssClass="input_mandatory"></telerik:RadTimePicker>
                                            hrs
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Event">
                                        <HeaderStyle Width="25%" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblEvent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVENT") %>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadTextBox ID="txtEventEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVENT") %>'
                                                CssClass="gridinput_mandatory" Width="220px" MaxLength="200">
                                            </telerik:RadTextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtEventAdd" runat="server" Width="220px" CssClass="gridinput_mandatory"
                                                MaxLength="200">
                                            </telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Cargo Type">
                                        <HeaderStyle Width="30%" />
                                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblRemark" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'>
                                            </telerik:RadLabel>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <telerik:RadTextBox ID="txtRemarkEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                                CssClass="input" MaxLength="300" Width="220px">
                                            </telerik:RadTextBox>
                                        </EditItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadTextBox ID="txtRemarkAdd" runat="server" CssClass="input" MaxLength="300" Width="220px"></telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                                <span class="icon"><i class="fas fa-edit"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                                <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                                <span class="icon"><i class="fas fa-save"></i></span>
                                            </asp:LinkButton>
                                            <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                                <span class="icon"><i class="fas fa-times-circle"></i></span>
                                            </asp:LinkButton>
                                        </EditItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                                <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                            </asp:LinkButton>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                            </MasterTableView>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblDirectorComment" runat="server" Text="Director Comments"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <telerik:RadTextBox runat="server" ID="txtDirectorComment" CssClass="input" Enabled="true"
                            onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)" TextMode="MultiLine"
                            Height="70px" Width="97%" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblDirectorCommentDate" runat="server" Text="Updated Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucDirectorCommentDate" runat="server" CssClass="readonlytextbox" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDirectorCommentedBy" runat="server" Text="Updated By"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox ID="txtDirectorCommentedByName" runat="server" Width="92%" CssClass="readonlytextbox"
                            Enabled="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <br />
            <asp:Button ID="ucConfirm" runat="server" OnClick="ucConfirm_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
