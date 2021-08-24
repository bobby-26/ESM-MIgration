<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAJobHazardAnalysis.aspx.cs"
    Inherits="InspectionRAJobHazardAnalysis" %>

<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hazard" Src="~/UserControls/UserControlRAHazardType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Miscellaneous" Src="~/UserControls/UserControlRAMiscellaneous.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CustomEditor" Src="~/UserControls/UserControlEditor.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRAJobHazard" runat="server">
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <eluc:TabStrip ID="MenuJobHazardGeneral" runat="server" OnTabStripCommand="MenuJobHazardGeneral_TabStripCommand"></eluc:TabStrip>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1" border="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblHazardNo" runat="server" Text="Hazard Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" CssClass="readonlytextbox" ID="txtHazid" ReadOnly="true"
                            Width="270px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRevisionNo" runat="server" Text="Revision No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" CssClass="readonlytextbox" ID="txtRevNO" ReadOnly="true" Width="270px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPreparedBy" runat="server" Text="Prepared By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" CssClass="readonlytextbox" ID="txtpreparedby" ReadOnly="true"
                            Width="270px">
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
                        <telerik:RadTextBox runat="server" CssClass="readonlytextbox" ID="txtApprovedby" ReadOnly="true"
                            Width="270px">
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
                        <telerik:RadTextBox runat="server" CssClass="readonlytextbox" ID="txtIssuedBy" ReadOnly="true"
                            Width="270px">
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
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Company "></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company ID="ucCompany" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="268px" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="270px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox ID="txtCategory" runat="server" Text="Process" ReadOnly="true" CssClass="readonlytextbox"
                            Width="270px" Enabled="false">
                        </telerik:RadTextBox>
                        <eluc:Category ID="ddlCategory" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            AutoPostBack="True" OnTextChangedEvent="BindJob_OnSelectedIndexChanged" Visible="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlJob" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            DataTextField="FLDNAME" DataValueField="FLDACTIVITYID" Width="270px" Filter="Contains">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblJob" runat="server" Text="Job"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtJob" runat="server" CssClass="input_mandatory" Width="270px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtStatus" runat="server" CssClass="readonlytextbox" Width="270px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblHealthandSafetyRisk" runat="server" Text="Health and Safety Risk"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadLabel ID="RadlblHazardType" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></telerik:RadLabel>
                        <eluc:Hazard ID="ucHazardType" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            Type="1" AutoPostBack="true" Width="20%" OnTextChangedEvent="ucHazardType_TextChangedEvent" />
                        <br />
                        <br />
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvHealthSafetyRisk" runat="server" CellSpacing="0" GridLines="None"
                            OnItemCommand="gvHealthSafetyRisk_ItemCommand" Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" ShowFooter="true"
                            EnableViewState="true" OnItemDataBound="gvHealthSafetyRisk_ItemDataBound">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDHEALTHHAZARDID">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Hazard Type">
                                        <HeaderStyle Width="30%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Impact">
                                        <HeaderStyle Width="60%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblHealthid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEALTHHAZARDID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadComboBox ID="ddlSubHazardType" runat="server" AppendDataBoundItems="true"
                                                CssClass="dropdown_mandatory" Width="460px" Filter="Contains">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="HEALTHSAFETYDELETE" ID="cmdDelete" ToolTip="Delete">
                                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Add" CommandName="HEALTHSAFETYADD" ID="cmdAdd" ToolTip="Add">
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
                    <td>
                        <telerik:RadLabel ID="lblEnvironmentalRisk" runat="server" Text="Environmental Risk"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadLabel ID="lblHazard" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></telerik:RadLabel>
                        <eluc:Hazard ID="ucEnvHazardType" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                            Type="2" AutoPostBack="true" Width="40%" OnTextChangedEvent="ucEnvHazardType_TextChangedEvent" />
                        <br />
                        <br />
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvEnvironmentalRisk" runat="server" CellSpacing="0" GridLines="None" Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" ShowFooter="true"
                            EnableViewState="true" OnItemDataBound="gvEnvironmentalRisk_ItemDataBound" OnItemCommand="gvEnvironmentalRisk_ItemCommand">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDHEALTHHAZARDID">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Hazard Type">
                                        <HeaderStyle Width="15%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Impact Type">
                                        <HeaderStyle Width="15%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblImpactType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMPACTNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <eluc:Miscellaneous ID="ucImpactType" AppendDataBoundItems="true" runat="server"
                                                CssClass="input_mandatory" Type="3" IncludeOthers="false" />
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Impact">
                                        <HeaderStyle Width="60%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblEnvironmentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEALTHHAZARDID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadComboBox ID="ddlSubHazardType" runat="server" AppendDataBoundItems="true"
                                                CssClass="dropdown_mandatory" Width="100%" Filter="Contains">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="ENVIRONMENTALDELETE" ID="cmdDelete" ToolTip="Delete">
                                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Add" CommandName="ENVIRONMENTALADD" ID="cmdAdd" ToolTip="Add">
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
                    <td>
                        <telerik:RadLabel ID="lblEconomicRisks" runat="server" Text="Economic Risks"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadLabel ID="lblHazardType1" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></telerik:RadLabel>
                        <eluc:Hazard ID="ucEconomicHazardType" runat="server" AppendDataBoundItems="true" OnTextChangedEvent="ucEconomicHazardType_TextChangedEvent"
                            CssClass="dropdown_mandatory" Type="4" AutoPostBack="true" Width="20%" />
                        <br />
                        <br />
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvEconomicRisk" runat="server" CellSpacing="0" GridLines="None" Font-Size="11px"
                            Width="100%" ShowHeader="true" ShowFooter="true" EnableViewState="true" OnItemCommand="gvEconomicRisk_ItemCommand"
                            OnItemDataBound="gvEconomicRisk_ItemDataBound">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDHEALTHHAZARDID">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Hazard Type">
                                        <HeaderStyle Width="30%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Impact">
                                        <HeaderStyle Width="60%" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblEconomicid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEALTHHAZARDID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblCategory" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBHAZARDNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <telerik:RadComboBox ID="ddlSubHazardType" runat="server" AppendDataBoundItems="true"
                                                CssClass="dropdown_mandatory" Width="300px" Filter="Contains">
                                                <Items>
                                                    <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                                </Items>
                                            </telerik:RadComboBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action">
                                        <HeaderStyle Width="10%" HorizontalAlign="Center" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="ECONOMICDELETE" ID="cmdDelete" ToolTip="Delete">
                                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Add" CommandName="ECONOMICADD" ID="cmdAdd" ToolTip="Add">
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
                    <td>
                        <telerik:RadLabel ID="lblWorstCaseHazard" runat="server" Text="Worst Case Hazard"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBoxList ID="cblOtherRisk" runat="server" Direction="Vertical" Columns="4">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOperationalHazard" runat="server" Text="Operational Hazard"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtOperationalHazard" runat="server" CssClass="input" Visible="false"
                            Height="150px" Rows="50" TextMode="MultiLine" Width="100%">
                        </telerik:RadTextBox>
                        <telerik:RadEditor ID="ucEditorOperationalHazard" runat="server" Width="100%" Height="225px">
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
                    <td>
                        <telerik:RadLabel ID="lblControlsPrecautions" runat="server" Text="Controls/Precautions"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox ID="txtConrolPrecautions" runat="server" CssClass="input" Height="150px"
                            Visible="false" Rows="50" TextMode="MultiLine" Width="100%">
                        </telerik:RadTextBox>
                        <telerik:RadEditor ID="ucEditorConrolPrecautions" runat="server" Width="100%" Height="225px" RenderMode="Lightweight"
                            SkinID="DefaultSetOfTools">
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
                    <td>
                        <telerik:RadLabel ID="lblRecommendedPPE" runat="server" Text="Recommended PPE"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadCheckBoxList ID="cblRecomendedPPE" runat="server" Direction="Vertical" Columns="4">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompetencyLevelforSupervision" runat="server" Text="Competency Level for Supervision"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucCompetencyLevel" runat="server" AppendDataBoundItems="true" QuickTypeCode="91"
                            Width="210px" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarks" TextMode="MultiLine" MaxLength="6" runat="server" CssClass="readonlytextbox"
                            Width="500px" Resize="Both">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
