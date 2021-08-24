<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAProcess.aspx.cs"
    Inherits="InspectionRAProcess" %>

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


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Assessment Process</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script language="javascript" type="text/javascript">
            function TxtMaxLength(text, maxLength) {
                text.value = text.value.substring(0, maxLength);
            }

        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmProcess" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="99.9%">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="Process" ShowMenu="true" Visible="false" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuProcess" runat="server" OnTabStripCommand="MenuProcess_TabStripCommand"></eluc:TabStrip>
            <table border="1" width="99.9%" cellpadding="1" cellspacing="1">
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblRefNo" runat="server" Text="Ref Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="360px" CssClass="readonlytextbox" ID="txtRefNo" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRevisionNo" runat="server" Text="Revision No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="135px" CssClass="readonlytextbox" ID="txtRevNO" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblPreparedBy" runat="server" Text="Prepared By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="360px" CssClass="readonlytextbox" ID="txtpreparedby" ReadOnly="true"></telerik:RadTextBox>
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
                        <telerik:RadTextBox runat="server" Width="360px" CssClass="readonlytextbox" ID="txtApprovedby" ReadOnly="true"></telerik:RadTextBox>
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
                        <telerik:RadTextBox runat="server" Width="360px" CssClass="readonlytextbox" ID="txtIssuedBy" ReadOnly="true"></telerik:RadTextBox>
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
                        <telerik:RadLabel ID="lblImportedJHA" runat="server" Text="Imported JHA"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date ID="txtDate" Visible="false" CssClass="readonlytextbox" ReadOnly="false" runat="server" />
                        &nbsp;
                                <asp:LinkButton ID="lnkImportJHA" runat="server" Text="Import JHA"></asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;
                                <br />
                        <div id="dvJHA" runat="server" class="input" style="overflow: auto; width: 35%; height: 80px;">
                            <telerik:RadCheckBoxList ID="chkImportedJHAList" runat="server" Height="99.9%" OnSelectedIndexChanged="chkImportedJHAList_Changed"
                                RepeatLayout="Flow" AutoPostBack="true" Columns="1" Direction="Vertical">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company "></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Company ID="ucCompany" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Quick runat="server" ID="ucProcess" AppendDataBoundItems="true" CssClass="input_mandatory"
                            QuickTypeCode="92" Visible="false" />
                        <telerik:RadTextBox ID="txtProcess" runat="server" CssClass="input_mandatory" Width="360px"
                            Visible="false">
                        </telerik:RadTextBox>
                        <telerik:RadComboBox ID="ddlCategory" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="360px"
                            AutoPostBack="True" DataTextField="FLDNAME" DataValueField="FLDACTIVITYID" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                        <asp:LinkButton ID="lnkRA" runat="server" Text="Multiple RA" OnClick="lnkRA_Clicked"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblProcess" runat="Server" Text="Process"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtActivity" runat="server" CssClass="input" Width="360px"></telerik:RadTextBox>
                        <eluc:Miscellaneous ID="ddlActivity" runat="server" CssClass="dropdown_mandatory"
                            Visible="false" AppendDataBoundItems="true" Type="6" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblActivityCondition" runat="server" Text="Activity / Condition"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtActivityCondition" runat="server" CssClass="input" Width="360px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top">
                        <telerik:RadLabel ID="lblRisksAspects" runat="server" Text="Risks/Aspects"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <%--   <telerik:RadTextBox ID="txtWorkDetails" runat="server" CssClass="input" Width="360px" TextMode="MultiLine"
                                    Rows="4"></telerik:RadTextBox>--%>
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
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblNoofPeopleInvolvedInActivity" runat="server" Text="No of people involved in activity"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadRadioButtonList ID="rblPeopleInvolved" runat="server" Direction="Horizontal"
                            RepeatColumns="4">
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblDurationofWorkActivity" runat="server" Text="Duration of work activity"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadRadioButtonList ID="rblWorkDuration" runat="server" Direction="Horizontal" />
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        <telerik:RadLabel ID="lblFrequencyofWorkActivity" runat="server" Text="Frequency of work activity"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadRadioButtonList ID="rblWorkFrequency" runat="server" Direction="Horizontal" />
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        <telerik:RadLabel ID="lblConditionsForAdditionalRiskAssessment" runat="server" Text="Conditions for Additional Risk Assessment"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBoxList ID="cblReason" runat="server" Direction="Vertical" Columns="3"
                            AutoPostBack="true" OnTextChanged="OtherDetailClick">
                        </telerik:RadCheckBoxList>
                        <br />
                        <telerik:RadTextBox ID="txtOtherReason" runat="server" CssClass="readonlytextbox" TextMode="MultiLine" Width="700px" Height="80px" Resize="Both"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        <telerik:RadLabel ID="lblHealthandSafetyRisk" runat="server" Text="Health and Safety Risk"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadLabel ID="lblHazardType" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></telerik:RadLabel>
                        <eluc:Hazard ID="ucHazardType" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="200px"
                            Type="1" AutoPostBack="true" OnTextChangedEvent="ucHazardType_TextChangedEvent" />
                        <br />
                        <br />
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvHealthSafetyRisk" runat="server" AutoGenerateColumns="False" OnItemCommand="gvHealthSafetyRisk_ItemCommand"
                            Font-Size="11px" Width="99.9%" CellPadding="3" OnItemDataBound="gvHealthSafetyRisk_ItemDataBound" OnNeedDataSource="gvHealthSafetyRisk_NeedDataSource"
                            ShowHeader="true" ShowFooter="true" EnableViewState="true" GroupingEnabled="false" EnableHeaderContextMenu="true">
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
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Impact">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
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
                                    <telerik:GridTemplateColumn HeaderText="Control" Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
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
                                            <telerik:RadLabel ID="lblProposedControlsToReduceRisk" runat="server">Proposed Controls To reduce risk</telerik:RadLabel>
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
                                        <FooterStyle Wrap="False" HorizontalAlign="Center" Width="100px" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="HEALTHSAFETYDELETE" ID="cmdDelete"
                                                ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Add" CommandName="HEALTHSAFETYADD" ID="cmdAdd"
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
                    <td colspan="2">
                        <telerik:RadLabel ID="lblEnvironmentalRisk" runat="server" Text="Environmental Risk"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBoxList ID="cblImpact" runat="server" RepeatDirection="Horizontal" Visible="false">
                            <Items>
                                <telerik:ButtonListItem Text="Direct" Value="1"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Text="Indirect" Value="0"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadCheckBoxList>
                        <telerik:RadLabel ID="lblHazardTypeHeader" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></telerik:RadLabel>
                        <eluc:Hazard ID="ucEnvHazardType" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="200px"
                            Type="2" AutoPostBack="true" OnTextChangedEvent="ucEnvHazardType_OnTextChangedEvent" />
                        <br />
                        <br />
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvEnvironmentalRisk" runat="server" AutoGenerateColumns="False" GroupingEnabled="false" EnableHeaderContextMenu="true"
                            Font-Size="11px" Width="99.9%" CellPadding="3" ShowHeader="true" ShowFooter="true" OnItemCommand="gvEnvironmentalRisk_ItemCommand"
                            EnableViewState="true" OnItemDataBound="gvEnvironmentalRisk_ItemDataBound" OnNeedDataSource="gvEnvironmentalRisk_NeedDataSource">
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
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Impact Type">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblImpactType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACTNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <eluc:Miscellaneous ID="ucImpactType" AppendDataBoundItems="true" runat="server"
                                                CssClass="input_mandatory" Type="3" IncludeOthers="false" width="200px" />
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Impact">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
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
                                            <telerik:RadLabel ID="lblProposedControlsToReduceRisk" runat="server">Proposed Controls To reduce risk</telerik:RadLabel>
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
                                        <FooterStyle Wrap="False" HorizontalAlign="Center" Width="100px" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="ENVIRONMENTALDELETE" ID="cmdDelete"
                                                ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Add" CommandName="ENVIRONMENTALADD" ID="cmdAdd"
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
                    <td colspan="2">
                        <telerik:RadLabel ID="lblEconomicRisks" runat="server" Text="Economic Risks"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadLabel ID="lblHazardType1" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></telerik:RadLabel>
                        <eluc:Hazard ID="ucEconomicHazardType" runat="server" AppendDataBoundItems="true" Width="200px"
                            CssClass="dropdown_mandatory" Type="4" AutoPostBack="true" OnTextChangedEvent="ucEconomicHazardType_OnTextChangedEvent" />
                        <br />
                        <br />
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvEconomicRisk" runat="server" AutoGenerateColumns="False" Font-Size="11px" GroupingEnabled="false" EnableHeaderContextMenu="true"
                            Width="99.9%" ShowHeader="true" ShowFooter="true" EnableViewState="true" OnItemCommand="gvEconomicRisk_ItemCommand"
                            OnItemDataBound="gvEconomicRisk_ItemDataBound" OnNeedDataSource="gvEconomicRisk_NeedDataSource">
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
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Impact">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
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
                                        <FooterStyle Wrap="False" HorizontalAlign="Center" Width="100px" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="CDELETE" ID="cmdDelete"
                                                ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
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
                <tr valign="top">
                    <td colspan="2">
                        <telerik:RadLabel ID="lblOtherRisks" runat="server" Text="Other Risks<br />(Worst Case Scenario)"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBoxList ID="cblOtherRisk" runat="server" Columns="4" Direction="Vertical"
                            AutoPostBack="true" OnTextChanged="OtherDetailClick">
                        </telerik:RadCheckBoxList>
                        <%-- <br />
                                Details
                                <br />--%>
                        <telerik:RadTextBox ID="txtOtherDetails" runat="server" CssClass="readonlytextbox" Width="360px"
                            ReadOnly="true" Visible="false">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td colspan="2">
                        <telerik:RadLabel ID="lblProposedControlsToReduceRisk" runat="server" Text="Proposed Controls To Reduce Risk"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <%--<telerik:RadTextBox ID="txtOtherRisk" runat="server" CssClass="input" Width="360px" TextMode="MultiLine"
                                    Rows="3"></telerik:RadTextBox>--%>
                        <%-- <eluc:CustomEditor ID="txtOtherRisk" runat="server" Width="99.9%" Height="225px"
                        Visible="true" PictureButton="false" TextOnly="true" DesgMode="true" HTMLMode="true"
                        PrevMode="false" ActiveMode="Design" AutoFocus="false" />--%>
                        <telerik:RadEditor ID="txtOtherRisk" runat="server" Width="99.9%" Height="225px">
                            <Modules>
                                <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                                <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                            </Modules>
                        </telerik:RadEditor>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                    <td colspan="3">
                        <telerik:RadRadioButtonList ID="rblType" runat="server" RepeatDirection="Horizontal" Direction="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblType_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Selected="True" Value="0" Text="Forms,Posters,Checklist"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Value="1" Text="Procedures"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Value="2" Text="Contingency/Emergency"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                        <span id="spnPickListDocument">
                            <telerik:RadTextBox ID="txtDocumentName" runat="server" Width="338px" Enabled="False" Style="font-weight: bold"
                                CssClass="input">
                            </telerik:RadTextBox>
                            <asp:LinkButton ID="btnShowDocuments" runat="server"
                                ImageAlign="AbsMiddle" Text="..">
                        <span class="icon"><i class="fas fa-tasks"></i></span></asp:LinkButton>
                            <telerik:RadTextBox ID="txtDocumentId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <asp:LinkButton ID="lnkFormAdd" runat="server" OnClick="lnkFormAdd_Click" Text="" ToolTip="Add">
                        <span class="icon"><i class="fas fa-plus-circle"></i></span>
                        </asp:LinkButton>
                        <br />
                        <%--<asp:ListBox ID="lstFormPoster" runat="server" CssClass="input" Width="360px" AutoPostBack="true" OnSelectedIndexChanged="lstFormPoster_Changed"></asp:ListBox>--%>
                        <%--<asp:RadioButtonList ID="rblFormPoster" runat="server" CssClass="input" Width="360px" AutoPostBack="true" OnSelectedIndexChanged="rblFormPoster_Changed"></asp:RadioButtonList>--%>
                        <div id="divForms" runat="server" style="height: 100px; overflow-y: auto; overflow-x: auto; width: 360px; border-width: 1px; border-style: solid; border: 1px solid #c3cedd">
                            <table id="tblForms" runat="server">
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblControls" runat="server" Text="Controls"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadRadioButtonList ID="rblOtherRiskControl" runat="server" RepeatDirection="Horizontal" RepeatColumns="3" Direction="Horizontal">
                        </telerik:RadRadioButtonList>
                        <asp:CheckBox ID="chkOverrideByMaster" runat="server" Visible="false" Enabled="false" />
                    </td>
                    <%--<td valign="middle">
                                Override by Master
                            </td>
                            <td>
                                
                            </td>--%>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblCompetencyLevelforSupervision" runat="server" Text="Competency Level for Supervision"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Quick ID="ucCompetencyLevel" runat="server" AppendDataBoundItems="true" QuickTypeCode="91" Width="200px" />
                        <telerik:RadTextBox ID="txtMasterRemarks" Visible="false" runat="server" CssClass="readonlytextbox" ReadOnly="true" TextMode="MultiLine" Resize="Both" Width="200px" Height="50px"></telerik:RadTextBox>
                    </td>
                    <%--<td valign="middle">
                                Master's Remarks
                            </td>
                            <td>
                                
                            </td>--%>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
