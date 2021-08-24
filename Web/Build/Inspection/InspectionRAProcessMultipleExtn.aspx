<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAProcessMultipleExtn.aspx.cs" Inherits="InspectionRAProcessMultipleExtn" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hazard" Src="~/UserControls/UserControlRAHazardType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SubHazard" Src="~/UserControls/UserControlRASubHazardType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Miscellaneous" Src="~/UserControls/UserControlRAMiscellaneous.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Frequency" Src="~/UserControls/UserControlRAFrequency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CustomEditor" Src="~/UserControls/UserControlEditor.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Assessment Process</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmProcess" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlProcess">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                 <eluc:Status runat="server" ID="ucStatus" />
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="General" ShowMenu="false" />
                        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuProcess" runat="server" OnTabStripCommand="MenuProcess_TabStripCommand">
                    </eluc:TabStrip>
                </div>                
                <div id="tblProcess" class="navSelect3" style="position: relative; z-index: +2">
                    <table border="1" width="100%" cellpadding="1" cellspacing="1">                    
                        <tr>
                            <td>
                                <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCategory" runat="server" CssClass="readonlytextbox" Width="250px" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:literal ID="lblProcess" runat="server" Text="Process"></asp:literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProcess" runat="server" Width="250px" CssClass="readonlytextbox" Enabled="false"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:literal ID="lblActivityCondition" runat="server" Text="Activity / Condition"></asp:literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtActivityCondition" runat="server" CssClass="input_mandatory" Width="360px"></asp:TextBox>  
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:literal ID="lblRisksAspects" runat="server" Text="Risks/Aspects"></asp:literal>
                            </td>
                            <td>
<%--                                <asp:TextBox ID="txtRiskAspects" runat="server" CssClass="input" Width="360px" TextMode="MultiLine"
                                    Rows="4"></asp:TextBox>--%>
                               <eluc:CustomEditor ID="txtRiskAspects" runat="server" Width="100%" Height="175px"
                                Visible="true" PictureButton="false" TextOnly="true" DesgMode="true" HTMLMode="true"
                                PrevMode="false" ActiveMode="Design" AutoFocus="false" />  
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                                <asp:literal ID="lblHealthandSafetyRisk" runat="server" Text="Health and Safety Risk"></asp:literal>
                            </td>
                            <td>
                                <asp:literal ID="lblHazardType" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:literal>
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
                                               <asp:Label ID="lblHazardTypeHeader" runat="server" > Hazard Type</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                               <asp:Label ID="lblImpactHeader" runat="server"> Impact</asp:Label>
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
                                                <asp:label ID="lblControlHeader" runat="server">Control</asp:label>
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
                                                <asp:TextBox ID="txtProposedControlAdd" runat="server" CssClass="input" Width="100%"></asp:TextBox>
                                            </FooterTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                            <FooterStyle Wrap="False" HorizontalAlign="Center" Width="100px" />
                                            <HeaderTemplate>
                                                <asp:label ID="lblActionHeader" runat="server">Action</asp:label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                    CommandName="HDELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                                    ToolTip="Delete"></asp:ImageButton>
                                            </ItemTemplate>
                                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
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
                                <asp:Literal ID="lblEnvironmentalRisk" runat="server" Text="Environmental Risk"></asp:Literal>
                            </td>
                            <td>                                
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
                                                <asp:Literal ID="lblHazardTypeHeader" runat="server">Hazard Type</asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblImpactTypeHeader" runat="server">Impact Type</asp:Literal>
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
                                                <asp:Literal ID="lblImpactHeader" runat="server">Impact</asp:Literal>
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
                                                <asp:label ID="lblControlHeader" runat="server">Control</asp:label>
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
                                            <FooterStyle Wrap="False" HorizontalAlign="Center" Width="100px" />
                                            <HeaderTemplate>
                                                <asp:label ID="lblActionHeader" runat="server">Action</asp:label>
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
                                <asp:literal ID="lblEconomicRisks" runat="server" Text="Economic Risks"></asp:literal>
                            </td>
                            <td>
                                <asp:Literal ID="lblHazardType2" runat="server" Text="Hazard Type&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"></asp:Literal>
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
                                                <asp:Label ID="lblHazardType" runat="server">Hazard Type</asp:Label>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblHazardType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDNAME") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:label ID="lblImpactHeader" runat="server" >Impact</asp:label>
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
                                                <asp:label ID="lblControlHeader" runat="server">Control</asp:label>
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
                                                <asp:label ID="lblProposedControlsToReduceRiskHeader" runat="server">Proposed Controls To reduce risk</asp:label>
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
                                            <FooterStyle Wrap="False" HorizontalAlign="Center" Width="100px" />
                                            <HeaderTemplate>
                                               <asp:label ID="lblActionHeader" runat="server">Action</asp:label>
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
                                <asp:literal ID="lblOtherRisks" runat="server" Text="Other Risks<br />(Worst Case Scenario)"></asp:literal>
                            </td>
                            <td>
                                <asp:CheckBoxList ID="cblOtherRisk" runat="server" RepeatDirection="Horizontal" RepeatColumns="4"
                                    AutoPostBack="true">
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr valign="top">
                            <td>
                               <asp:Literal ID="lblProposedControlsToReduceRisk" runat="server" Text="Proposed Controls To Reduce Risk"></asp:Literal>
                            </td>
                            <td>
                                <%--<asp:TextBox ID="txtOtherRisk" runat="server" CssClass="input" Width="360px" TextMode="MultiLine"
                                    Rows="3"></asp:TextBox>--%>
                                <eluc:CustomEditor ID="txtOtherRisk" runat="server" Width="100%" Height="175px"
                                    Visible="true" PictureButton="false" TextOnly="true" DesgMode="true" HTMLMode="true"
                                    PrevMode="false" ActiveMode="Design" AutoFocus="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblControls" runat="server" Text="Controls"></asp:Literal>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblOtherRiskControl" runat="server" RepeatDirection="Horizontal">
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:literal ID="lblCompetencyLevelforSuperVision" runat="server" Text="Competency Level for Supervision"></asp:literal>
                            </td>
                            <td>
                                <eluc:Quick ID="ucCompetencyLevel" runat="server" AppendDataBoundItems="true" QuickTypeCode="91"
                                    CssClass="input"  Width="200px"/>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
