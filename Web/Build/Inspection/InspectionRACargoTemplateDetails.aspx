<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRACargoTemplateDetails.aspx.cs" Inherits="Inspection_InspectionRACargoTemplateDetails" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
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
<%@ Register TagPrefix="eluc" TagName="CustomEditor" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Assessment Cargo</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    <style type="text/css">
        table.Hazard
        {
            border-collapse: collapse;
        }
        table.Hazard td, table.Hazard th
        {
            border: 1px solid black;
            padding: 5px;
        }
    </style>

    <script language="javascript" type="text/javascript">
    function TxtMaxLength(text,maxLength)
    {
       text.value = text.value.substring(0,maxLength);
    }
    </script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmCargo" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCargo">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Status runat="server" ID="ucStatus" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="Cargo" ShowMenu="true" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuCargo" runat="server" OnTabStripCommand="MenuCargo_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="tblCargo">
                    <table border="1" width="100%" cellpadding="1" cellspacing="1">
                        <tr>
                            <td>
                                <asp:Literal ID="lblRefNo" runat="server" Text="Ref Number"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="150px" CssClass="readonlytextbox" ID="txtRefNo"
                                    ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblRevisionNo" runat="server" Text="Revision No"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="80px" CssClass="readonlytextbox" ID="txtRevNO"
                                    ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblPreparedBy" runat="server" Text="Prepared By"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="150px" CssClass="readonlytextbox" ID="txtpreparedby"
                                    ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="ucCreatedDate" CssClass="readonlytextbox" runat="server"
                                    ReadOnly="true" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblApprovedBy" runat="server" Text="Approved By"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="150px" CssClass="readonlytextbox" ID="txtApprovedby"
                                    ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblDate1" runat="server" Text="Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="ucApprovedDate" CssClass="readonlytextbox" runat="server"
                                    ReadOnly="true" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblIssuedBy" runat="server" Text="Issued By"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox runat="server" Width="150px" CssClass="readonlytextbox" ID="txtIssuedBy"
                                    ReadOnly="true"></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblDate2" runat="server" Text="Date"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Date ID="ucIssuedDate" CssClass="readonlytextbox" runat="server"
                                    ReadOnly="true" Enabled="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                <eluc:Date ID="txtDate" Visible="false" CssClass="readonlytextbox" ReadOnly="false"
                                    runat="server" />
                            </td>
                            <td rowspan="6" colspan="2">
                                <table class="Hazard">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblHeader" runat="server"></asp:Label>
                                        </td>
                                        <td align="center" style="background-color: rgb(220,230,241);">
                                            <asp:Label ID="lblHealthSafety" runat="server">Health and Safety Hazards</asp:Label>
                                        </td>
                                        <td align="center" style="background-color: rgb(216,228,188);">
                                            <asp:Label ID="lblEnviormental" runat="server">Environmental Impact</asp:Label>
                                        </td>
                                        <td align="center" style="background-color: rgb(253,233,217);">
                                            <asp:Label ID="lblEconomic" runat="server">Economic/Process Loss</asp:Label>
                                        </td>
                                        <td align="center" style="background-color: rgb(230,184,183);">
                                            <asp:Label ID="lblWorst" runat="server">Worst Case Scenario</asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblHazard" runat="server">Hazard</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblHarzardHealth" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblHazardEnv" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblHazardEconomic" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblHazardWorst" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblProb" runat="server">Prob of Occurrence</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblProbhealth" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblProbEnv" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblProbEcomoic" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblProbWorst" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblLikelihood" runat="server">Likelihood of Harm</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblLikelihoodHealth" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblLikelihoodEnv" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblLikelihoodEconomic" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblLikelihoodWorst" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblLevelOfControl" runat="server">Level of Control</asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblLevelOfControlHealth" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblLevelOfControlEnv" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblLevelOfControlEconomic" runat="server"></asp:Label>
                                        </td>
                                        <td align="center">
                                            <asp:Label ID="lblLevelOfControlWorst" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblLevelofRisk" runat="server">Level of Risk</asp:Label>
                                        </td>
                                        <td align="center" id="levelofriskhealth" runat="server">
                                            <asp:Label ID="lblLevelofRiskHealth" runat="server"></asp:Label>
                                        </td>
                                        <td align="center" id="levelofriskenv" runat="server">
                                            <asp:Label ID="lblLevelofRiskEnv" runat="server"></asp:Label>
                                        </td>
                                        <td align="center" id="levelofriskeco" runat="server">
                                            <asp:Label ID="lblLevelofRiskEconomic" runat="server"></asp:Label>
                                        </td>
                                        <td align="center" id="levelofriskworst" runat="server">
                                            <asp:Label ID="lblLevelofRiskWorst" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCompany" runat="server" Text="Company"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Company ID="ucCompany" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                            </td>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblDateofIntendedWorkActivity" runat="server" Text="Date of intended Work / Activity"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Date ID="txtIntendedWorkDate" CssClass="input_mandatory" runat="server" DatePicker="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                        AutoPostBack="True" DataTextField="FLDNAME" DataValueField="FLDACTIVITYID">
                                        <asp:ListItem Text="--Select--" Value="DUMMY"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblAmendmentTo" runat="server" Text="Amendment To"></asp:Literal>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlAmendedTo" runat="server" CssClass="input">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblActivityConditionsEquipment" runat="server" Text="Activity / Conditions / Equipment"></asp:Literal>
                                </td>
                                <td>
                                    <%--<eluc:Miscellaneous ID="ddlActivity" runat="server" CssClass="dropdown_mandatory"
                                    AppendDataBoundItems="true" Type="6" />--%>
                                    <asp:TextBox ID="txtActivityCondition" runat="server" CssClass="input_mandatory"
                                        Width="360px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    <asp:Literal ID="lblReasonForAssessment" runat="server" Text="Reason for Assessment"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <asp:CheckBoxList ID="cblReason" runat="server" RepeatDirection="Horizontal" RepeatColumns="3"
                                        AutoPostBack="true" OnTextChanged="OtherDetailClick">
                                    </asp:CheckBoxList>
                                    <br />
                                    <asp:TextBox ID="txtOtherReason" runat="server" CssClass="readonlytextbox" Width="700px"
                                        Height="80px" TextMode="MultiLine" ReadOnly="true" onKeyUp="TxtMaxLength(this,700)"
                                        onChange="TxtMaxLength(this,700)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    <asp:Literal ID="lblAternativeMethod" runat="server" Text="Alternative Method Considered for Work"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtAternativeMethod" runat="server" CssClass="input" Width="700px"
                                        Height="80px" TextMode="MultiLine" onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblNoofPeopleInvolvedInActivity" runat="server" Text="No of people involved in activity / Affected"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblPeopleInvolved" runat="server" RepeatDirection="Horizontal"
                                        RepeatColumns="4">
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblDurationOfWorkActivity" runat="server" Text="Duration of work / activity"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblWorkDuration" runat="server" RepeatDirection="Horizontal" />
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    <asp:Literal ID="lblFrequencyofWorkActivity" runat="server" Text="Frequency of work / activity"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <asp:RadioButtonList ID="rblWorkFrequency" runat="server" RepeatDirection="Horizontal" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal ID="lblRisksAspects" runat="server" Text="Risks/Aspects"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <%--      <asp:TextBox ID="txtWorkDetails" runat="server" CssClass="input_mandatory" Width="360px"
                                        Rows="4" TextMode="MultiLine"></asp:TextBox>--%>
                                    <eluc:CustomEditor ID="txtWorkDetails" runat="server" Width="100%" Height="225px"
                                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="true" HTMLMode="true"
                                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    <asp:Literal ID="lblHealthandSafetyImpact" runat="server" Text="Health and Safety Impact"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <asp:Literal ID="lblHazardType" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
                                    <eluc:Hazard ID="ucHazardType" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                        Type="1" AutoPostBack="true" />
                                    <br />
                                    <br />
                                    <asp:GridView ID="gvHealthSafetyRisk" runat="server" AutoGenerateColumns="False"
                                        OnRowCommand="gvHealthSafetyRisk__RowCommand" Font-Size="11px" Width="100%" CellPadding="3"
                                        ShowHeader="true" ShowFooter="true" EnableViewState="false" OnRowDataBound="gvHealthSafetyRisk_RowDataBound">
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <RowStyle Height="5px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Hazard Type">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblHazardTypeHeader" runat="server">Hazard Type</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblImpactHeader" runat="server">Impact</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategoryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></asp:Label>
                                                    <asp:Label ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="ddlSubHazardType" runat="server" AppendDataBoundItems="true"
                                                        CssClass="dropdown_mandatory">
                                                        <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblControlHeader" runat="server">Control</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLNAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <eluc:Frequency ID="ddlControlAdd" runat="server" CssClass="dropdown_mandatory" FrequencyList='<%# PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4)%>'
                                                        AppendDataBoundItems="true" Type="4" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblProposedControlsToReduceRiskHeader" runat="server"> Proposed Controls To reduce risk</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProposedControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDCONTROLS") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtProposedControlAdd" runat="server" CssClass="input" Width="100%"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                <FooterStyle HorizontalAlign="Center" Width="100px" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                        CommandName="HDELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                                        ToolTip="Delete"></asp:ImageButton>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Add" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                                        CommandName="HADD" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                                        ToolTip="Add"></asp:ImageButton>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    <asp:Literal ID="lblEnvironmentalImpact" runat="server" Text="Environmental Impact"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <asp:CheckBoxList ID="cblImpact" runat="server" RepeatDirection="Horizontal" Visible="false">
                                        <asp:ListItem Text="Direct" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="Indirect" Value="0"></asp:ListItem>
                                    </asp:CheckBoxList>
                                    <asp:Literal ID="lblHazardType1" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
                                    <eluc:Hazard ID="ucEnvHazardType" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                        Type="2" AutoPostBack="true" />
                                    <br />
                                    <br />
                                    <asp:GridView ID="gvEnvironmentalRisk" runat="server" AutoGenerateColumns="False"
                                        Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" ShowFooter="true"
                                        EnableViewState="false" OnRowDataBound="gvEnvironmentalRisk_RowDataBound" OnRowCommand="gvEnvironmentalRisk_RowCommand">
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <RowStyle Height="5px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Hazard Type">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblHazardType" runat="server">Hazard Type</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblImpactTypeHeader" runat="server">Impact Type</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblImpactType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACTNAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <eluc:Miscellaneous ID="ucImpactType" AppendDataBoundItems="true" runat="server"
                                                        CssClass="input_mandatory" Type="3" IncludeOthers="false" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblImpactHeader" runat="server">Impact</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategoryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></asp:Label>
                                                    <asp:Label ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="ddlSubHazardType" runat="server" AppendDataBoundItems="true"
                                                        CssClass="dropdown_mandatory">
                                                        <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblControlHeader" runat="server">Control</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLNAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <eluc:Frequency ID="ddlControlAdd" runat="server" CssClass="dropdown_mandatory" FrequencyList='<%# PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4)%>'
                                                        AppendDataBoundItems="true" Type="4" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblProposedControlsToReduceRiskHeader" runat="server">Proposed Controls To reduce risk</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProposedControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDCONTROLS") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtProposedControlAdd" runat="server" CssClass="input"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                <FooterStyle HorizontalAlign="Center" Width="100px" />
                                                <HeaderTemplate>
                                                    <asp:Literal ID="lblActionHeader" runat="server"> Action</asp:Literal>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                        CommandName="EDELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                                        ToolTip="Delete"></asp:ImageButton>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Add" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                                        CommandName="EADD" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                                        ToolTip="Add"></asp:ImageButton>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblEconomicImpact" runat="server" Text="Economic Impact"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <asp:Literal ID="lblHazard2" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
                                    <eluc:Hazard ID="ucEconomicHazardType" runat="server" AppendDataBoundItems="true"
                                        CssClass="dropdown_mandatory" Type="4" AutoPostBack="true" />
                                    <br />
                                    <br />
                                    <asp:GridView ID="gvEconomicRisk" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" ShowHeader="true" ShowFooter="true" EnableViewState="false" OnRowCommand="gvEconomicRisk_RowCommand"
                                        OnRowDataBound="gvEconomicRisk_RowDataBound">
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <RowStyle Height="5px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Hazard Type">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblHazardTypeHeader" runat="server">Hazard Type</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblImpactHeader" runat="server">Impact</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblCategoryid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCATEGORYID") %>'></asp:Label>
                                                    <asp:Label ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:DropDownList ID="ddlSubHazardType" runat="server" AppendDataBoundItems="true"
                                                        CssClass="dropdown_mandatory">
                                                        <asp:ListItem Value="Dummy" Text="--Select--"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblControlHeader" runat="server">Control</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLNAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <eluc:Frequency ID="ddlControlAdd" runat="server" CssClass="dropdown_mandatory" FrequencyList='<%# PhoenixInspectionRiskAssessmentFrequency.ListRiskAssessmentFrequency(4)%>'
                                                        AppendDataBoundItems="true" Type="4" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField Visible="false">
                                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblProposedControlsToReduceRiskHeader" runat="server">Proposed Controls To reduce risk</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblProposedControl" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROPOSEDCONTROLS") %>'></asp:Label>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtProposedControlAdd" runat="server" CssClass="input"></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                <FooterStyle HorizontalAlign="Center" Width="100px" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblActionHeader" runat="server"> Action</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                        CommandName="CDELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                                        ToolTip="Delete"></asp:ImageButton>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Add" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                                        CommandName="CADD" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                                        ToolTip="Add"></asp:ImageButton>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    <asp:Literal ID="lblWorstCaseScenario" runat="server" Text="Worst Case Scenario"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <asp:CheckBoxList ID="cblOtherRisk" runat="server" RepeatDirection="Horizontal" RepeatColumns="4"
                                        AutoPostBack="true" OnTextChanged="OtherDetailClick">
                                    </asp:CheckBoxList>
                                    <%--<br />
                                Details
                                <br />--%>
                                    <asp:TextBox ID="txtOtherDetails" runat="server" CssClass="readonlytextbox" Width="360px"
                                        Visible="false" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    <asp:Literal ID="lblProposedControlsToReduceRisk" runat="server" Text="Proposed Controls To Reduce Risk"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <asp:CheckBoxList ID="cblProposedControl" runat="server" RepeatDirection="Horizontal"
                                        RepeatColumns="2" AutoPostBack="true" OnTextChanged="OtherDetailClick" />
                                    <br />
                                    <asp:Literal ID="lblDetails" runat="server" Text="Details"></asp:Literal>
                                    <br />
                                    <asp:TextBox ID="txtProposedControlDetails" runat="server" CssClass="readonlytextbox"
                                        TextMode="MultiLine" Rows="3" Width="700px" Height="80px" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblAdditionalSaftyProcedures" runat="server" Text=" Additional Safety Procedures (Emergency)"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <asp:GridView ID="gvCargoSafety" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" ShowHeader="true" ShowFooter="true" EnableViewState="false" OnRowCommand="gvCargoSafety_RowCommand"
                                        OnRowDataBound="gvCargoSafety_RowDataBound" OnRowEditing="gvCargoSafety_RowEditing"
                                        OnRowCancelingEdit="gvCargoSafety_RowCancelingEdit">
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <RowStyle Height="10px" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="S.No">
                                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="1%"></ItemStyle>
                                                <FooterStyle Wrap="false" HorizontalAlign="Right" Width="1%" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblSNo" runat="server">S.No</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></asp:Label>
                                                    <asp:Label ID="lblCargoSafetyId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCARGOSAFETYID") %>'
                                                        Visible="false"></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <eluc:Number ID="ucSNoEdit" runat="server" CssClass="input" IsInteger="true" Width="95%"
                                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>' />
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <eluc:Number ID="ucSNoAdd" runat="server" CssClass="input" Width="95%" IsInteger="true" />
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle HorizontalAlign="Left" Wrap="true" Width="40%"></ItemStyle>
                                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="40%" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblTask" runat="server">Task</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblTask" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASK") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtTaskEdit" runat="server" TextMode="MultiLine" CssClass="input_mandatory"
                                                        Width="98%" Height="25%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASK") %>'
                                                        onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)"></asp:TextBox>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:TextBox ID="txtTaskAdd" runat="server" TextMode="MultiLine" Width="98%" Height="25%"
                                                        CssClass="input_mandatory" onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)"
                                                        s></asp:TextBox>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="12%"></ItemStyle>
                                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="12%" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblPIC" runat="server">PIC</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPIC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICNAME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:Label ID="lblCargoSafetyIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGENERICSAFETYID") %>'></asp:Label>
                                                    <span id="spnPICEdit">
                                                        <asp:TextBox ID="txtPICNameEdit" runat="server" CssClass="input" Enabled="false"
                                                            MaxLength="50" Width="95%"></asp:TextBox>
                                                        <asp:TextBox ID="txtPICRankEdit" runat="server" CssClass="input" Enabled="false"
                                                            MaxLength="50" Width="75%"></asp:TextBox>
                                                        <asp:ImageButton runat="server" ID="btnPICEdit" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                            ImageAlign="Top" Text=".." CommandArgument="<%# Container.DataItemIndex %>" />
                                                        <asp:TextBox ID="txtPICIdEdit" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                                                    </span>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <span id="spnPICAdd">
                                                        <asp:TextBox ID="txtPICNameAdd" runat="server" CssClass="input" Enabled="false" MaxLength="50"
                                                            Width="95%"></asp:TextBox>
                                                        <asp:TextBox ID="txtPICRankAdd" runat="server" CssClass="input" Enabled="false" MaxLength="50"
                                                            Width="75%"></asp:TextBox>
                                                        <asp:ImageButton runat="server" ID="btnPICAdd" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                                            ImageAlign="Top" Text=".." CommandArgument="<%# Container.DataItemIndex %>" />
                                                        <asp:TextBox ID="txtPICIdAdd" runat="server" Width="0px" CssClass="hidden"></asp:TextBox>
                                                    </span>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="8%"></ItemStyle>
                                                <FooterStyle Wrap="False" HorizontalAlign="Left" Width="8%" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblActualFinishDate" runat="server">Completion Date</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblActualFinishDate" runat="server" Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHDATE")) %>'></asp:Label>
                                                    <asp:Label ID="lblActualFinishTime" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHTIME") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <eluc:Date ID="ucActualFinishDateEdit" runat="server" CssClass="input_mandatory"
                                                        Text='<%#  General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDACTUALFINISHDATE")) %>' />
                                                    </br>
                                                    <asp:TextBox ID="txtActualFinishTimeEdit" runat="server" CssClass="input" Width="30px" />
                                                    <asp:Literal ID="lblActualFinishDateTimeEdithrs" runat="server" Text="hrs"></asp:Literal>
                                                    <ajaxToolkit:MaskedEditExtender ID="txtActualFinishTimeEditMask" runat="server" AcceptAMPM="false"
                                                        ClearMaskOnLostFocus="false" ClearTextOnInvalid="true" Mask="99:99" MaskType="Time"
                                                        TargetControlID="txtActualFinishTimeEdit" UserTimeFormat="TwentyFourHour" />
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle HorizontalAlign="Left" Wrap="true" Width="40%"></ItemStyle>
                                                <FooterStyle Wrap="true" HorizontalAlign="Left" Width="40%" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server">Remarks</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></asp:Label>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:TextBox ID="txtRemarksEdit" runat="server" TextMode="MultiLine" CssClass="input_mandatory"
                                                        Width="98%" Height="25%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                                        onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)"></asp:TextBox>
                                                </EditItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="5%"></ItemStyle>
                                                <FooterStyle HorizontalAlign="Center" Width="5%" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblActionHeader" runat="server"> Action</asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                                        ToolTip="Edit"></asp:ImageButton>
                                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                        CommandName="SDELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                                        ToolTip="Delete"></asp:ImageButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                                        CommandName="SUPDATE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                                        ToolTip="Save"></asp:ImageButton>
                                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                        CommandName="CANCEL" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                                        ToolTip="Cancel"></asp:ImageButton>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Add" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                                        CommandName="SADD" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdAdd"
                                                        ToolTip="Add"></asp:ImageButton>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <%--  <asp:TextBox ID="txtAdditionalSafetyProcedures" runat="server" CssClass="input" Width="360px"
                                        Rows="4" TextMode="MultiLine"></asp:TextBox>--%>
                                    <eluc:CustomEditor ID="txtAdditionalSafetyProcedures" runat="server" Width="100%"
                                        Height="225px" Visible="false" PictureButton="false" TextOnly="true" DesgMode="true"
                                        HTMLMode="true" PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                                </td>
                            </tr>
                            <tr valign="middle">
                                <td>
                                    <asp:Literal ID="lblApprovedforUse" runat="server" Text="Approved for use,<BR> subject to completion of Tasks"></asp:Literal>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblOtherRiskControl" runat="server" RepeatDirection="Horizontal"
                                        Visible="false">
                                    </asp:RadioButtonList>
                                    <asp:RadioButtonList ID="rblControlsAdequate" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="1" Text="Yes"></asp:ListItem>
                                        <asp:ListItem Value="0" Text="No"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td valign="middle">
                                    <asp:Literal ID="lblOverridebyMaster" runat="server" Text="Override by Master"></asp:Literal>
                                </td>
                                <td valign="middle">
                                    <asp:CheckBox ID="chkOverrideByMaster" runat="server" Enabled="false" />
                                </td>
                            </tr>
                            <tr valign="top" runat="server" visible="false">
                                <td>
                                    <asp:Literal ID="lblVerification" runat="server" Text="Verification"></asp:Literal>
                                </td>
                                <td colspan="3">
                                    <asp:Literal ID="lblVerifcationQuestion" runat="server" Text="Task was completed in timely manner and all control measures adequate?"></asp:Literal>
                                    <br />
                                    <asp:RadioButtonList ID="rblVerifcation" runat="server" RepeatDirection="Horizontal"
                                        AutoPostBack="true" OnSelectedIndexChanged="rblVerifcation_Changed">
                                        <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <br />
                                    <asp:Literal ID="lblVerifcationRemarks" runat="server" Text="Additional Comments"></asp:Literal>
                                    <br />
                                    <asp:TextBox ID="txtVerificationRemarks" runat="server" CssClass="input" Width="700px"
                                        Height="80px" TextMode="MultiLine" onKeyUp="TxtMaxLength(this,1000)" onChange="TxtMaxLength(this,1000)"></asp:TextBox>
                                </td>
                            </tr>
                            <tr valign="top">
                                <td>
                                    <asp:Literal ID="lblRemarks" runat="server" Text="Remarks"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" runat="server" CssClass="readonlytextbox" Width="360px"
                                        TextMode="MultiLine" Rows="4" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td valign="top">
                                    <asp:Literal ID="lblReasonsforOverride" runat="server" Text="Reasons for Override"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMasterRemarks" runat="server" CssClass="readonlytextbox" TextMode="MultiLine"
                                        Width="280px" Rows="4" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
