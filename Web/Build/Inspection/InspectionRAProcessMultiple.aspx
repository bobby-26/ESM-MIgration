<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAProcessMultiple.aspx.cs" Inherits="InspectionRAProcessMultiple" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hazard" Src="~/UserControls/UserControlRAHazardType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubHazard" Src="~/UserControls/UserControlRASubHazardType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Miscellaneous" Src="~/UserControls/UserControlRAMiscellaneous.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Frequency" Src="~/UserControls/UserControlRAFrequency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="CustomEditor" Src="~/UserControls/UserControlEditor.ascx" %>--%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Assessment Process</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
            <eluc:Title runat="server" ID="ucTitle" Text="General" ShowMenu="false" Visible="false" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:TabStrip ID="MenuProcess" runat="server" OnTabStripCommand="MenuProcess_TabStripCommand" Title="General"></eluc:TabStrip>
            <table border="1" width="99.9%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCategory" runat="server" CssClass="readonlytextbox" Width="360px" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProcess" runat="server" Text="Process"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtProcess" runat="server" Width="360px" CssClass="readonlytextbox" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblActivityCondition" runat="server" Text="Activity / Condition"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtActivityCondition" runat="server" CssClass="input_mandatory" Width="360px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRisksAspects" runat="server" Text="Risks/Aspects"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadEditor ID="txtRiskAspects" runat="server" Width="99.9%" Height="225px">
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
                    <td>
                        <telerik:RadLabel ID="lblHealthandSafetyRisk" runat="server" Text="Health and Safety Risk"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblHazardType" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></telerik:RadLabel>
                        <eluc:Hazard ID="ucHazardType" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="200px"
                            Type="1" AutoPostBack="true" OnTextChangedEvent="ucHazardType_TextChangedEvent" />
                        <br />
                        <br />
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvHealthSafetyRisk" runat="server" AutoGenerateColumns="False" GroupingEnabled="false" EnableHeaderContextMenu="true"
                            OnItemCommand="gvHealthSafetyRisk_ItemCommand" Font-Size="11px" Width="99.9%" CellPadding="3" OnNeedDataSource="gvHealthSafetyRisk_NeedDataSource"
                            ShowHeader="true" ShowFooter="true" EnableViewState="true" OnItemDataBound="gvHealthSafetyRisk_ItemDataBound">
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
                                        <FooterStyle Wrap="False" HorizontalAlign="Center" Width="100px" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="HDELETE" ID="cmdDelete"
                                                ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
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
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblEnvironmentalRisk" runat="server" Text="Environmental Risk"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblHazardType1" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></telerik:RadLabel>
                        <eluc:Hazard ID="ucEnvHazardType" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="200px"
                            Type="2" AutoPostBack="true" OnTextChangedEvent="ucEnvHazardType_OnTextChangedEvent" />
                        <br />
                        <br />
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvEnvironmentalRisk" runat="server" AutoGenerateColumns="False" GroupingEnabled="false" EnableHeaderContextMenu="true"
                            Font-Size="11px" Width="99.9%" CellPadding="3" ShowHeader="true" ShowFooter="true" OnNeedDataSource="gvEnvironmentalRisk_NeedDataSource"
                            EnableViewState="true" OnItemDataBound="gvEnvironmentalRisk_ItemDataBound" OnItemCommand="gvEnvironmentalRisk_ItemCommand">
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
                                        <FooterStyle Wrap="False" HorizontalAlign="Center" Width="100px" />
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
                        <telerik:RadLabel ID="lblEconomicRisks" runat="server" Text="Economic Risks"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblHazardType2" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></telerik:RadLabel>
                        <eluc:Hazard ID="ucEconomicHazardType" runat="server" AppendDataBoundItems="true" Width="200px"
                            CssClass="dropdown_mandatory" Type="4" AutoPostBack="true" OnTextChangedEvent="ucEconomicHazardType_OnTextChangedEvent" />
                        <br />
                        <br />
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvEconomicRisk" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="99.9%" ShowHeader="true" ShowFooter="true" EnableViewState="true" OnItemCommand="gvEconomicRisk_ItemCommand" GroupingEnabled="false" EnableHeaderContextMenu="true"
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
                                            <telerik:RadTextBox ID="txtProposedControlAdd" runat="server" CssClass="input"></telerik:RadTextBox>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <FooterStyle Wrap="False" HorizontalAlign="Center" Width="100px" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Delete"
                                                CommandName="CDELETE" ID="cmdDelete"
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
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblOtherRisks" runat="server" Text="Other Risks<br />(Worst Case Scenario)"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBoxList ID="cblOtherRisk" runat="server" RepeatDirection="Horizontal" RepeatColumns="4" Direction="Vertical" Columns="4"
                            AutoPostBack="true">
                        </telerik:RadCheckBoxList>
                    </td>
                </tr>
                <tr valign="top">
                    <td>
                        <telerik:RadLabel ID="lblProposedControlsToReduceRisk" runat="server" Text="Proposed Controls To Reduce Risk"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<telerik:RadTextBox ID="txtOtherRisk" runat="server" CssClass="input" Width="360px" TextMode="MultiLine"
                                    Rows="3"></telerik:RadTextBox>--%>
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
                    <td>
                        <telerik:RadLabel ID="lblControls" runat="server" Text="Controls"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblOtherRiskControl" runat="server" Direction="Horizontal">
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompetencyLevelforSuperVision" runat="server" Text="Competency Level for Supervision"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucCompetencyLevel" runat="server" AppendDataBoundItems="true" QuickTypeCode="91" Width="200px" />
                    </td>
                </tr>
            </table>
            <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
