<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAJobHazardAnalysisReview.aspx.cs"
    Inherits="InspectionRAJobHazardAnalysisReview" %>

<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategory.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Job Hazard Analysis Review</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmJobHazardAnalysisReview" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Status runat="server" ID="ucStatus" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <eluc:Title runat="server" ID="ucTitle" Text="Job Hazard Analysis Review" ShowMenu="false" />
            </div>
        </div>
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                   <asp:Literal ID="lblType" runat="server" Text="Type"></asp:Literal>
                </td>
                <td style="width: 30%">
                    <eluc:Category ID="ddlCategory" runat="server" AppendDataBoundItems="true" CssClass="input"
                        Enabled="false" AutoPostBack="True" OnTextChangedEvent="BindJob_OnSelectedIndexChanged" />
                </td>
                <td>
                   <asp:Literal ID="lblRevisionNo" runat="server" Text="Revision No"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtRevisionno" runat="server" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblCategory" runat="server" Text="Category"></asp:Literal>
                </td>
                <td>
                    <asp:DropDownList ID="ddlJob" runat="server" CssClass="input" Enabled="false" AppendDataBoundItems="true"
                        DataTextField="FLDNAME" DataValueField="FLDACTIVITYID">
                        <asp:ListItem Text="--Select--" Value="DUMMY"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <asp:Literal ID="lblIssued" runat="server" Text="Issued"></asp:Literal>
                </td>
                <td>
                    <eluc:Date ID="txtIssuedDate" runat="server" CssClass="readonlytextbox" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblJob" runat="server" Text="Job"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtJob" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                        Width="220px"></asp:TextBox>
                </td>
            </tr>
        </table>
        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="MenuJobHazardAnalysisReview" runat="server" OnTabStripCommand="MenuJobHazardAnalysisReview_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <asp:GridView ID="gvRiskAssessmentJobHazardAnalysisReview" runat="server" AutoGenerateColumns="False"
            Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false"
            OnRowDataBound="gvRiskAssessmentJobHazardAnalysisReview_RowDataBound" OnPreRender="gvRiskAssessmentJobHazardAnalysisReview_PreRender">
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
            <RowStyle Height="5px" />
            <Columns>
                <asp:TemplateField>
                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle BackColor="#FFFF00" />
                    <HeaderTemplate>
                       <asp:Label ID="lblOperationalHazardsAspectsHeader" runat="server"> Operational Hazards / Aspects</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%--<asp:Label ID="lblOperationalHazard" runat="server" Text='<%# Server.HtmlEncode(DataBinder.Eval(Container,"DataItem.FLDOPERATIONALHAZARD").ToString())  %>'></asp:Label>--%>
                        <asp:Literal ID="ltlOperationalHazard" runat="server" Mode="PassThrough" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONALHAZARD") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="300px"></ItemStyle>
                    <HeaderStyle BackColor="#FF9933" />
                    <HeaderTemplate>
                       <asp:Label ID="lblHealthandSafetyHazardHeader" runat="server"> Health and Safety Hazards</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblHealthandSafety" Width="300px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEALTHANDSAFETY")  %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle BackColor="#66ff33" />
                    <HeaderTemplate>
                        <asp:label ID="lblEnvironmentalImpactHeader" runat="server">Environmental Impact</asp:label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblEnvHazard" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENVIRONMENTALHAZARD")  %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle BackColor="#FFFF99" />
                    <HeaderTemplate>
                        <asp:label ID="lblEconomicProcessLoss" runat="server" >Economic / Process Loss</asp:label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblEconomicHazard" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDECONOMICHAZARD")  %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>                 
                <asp:TemplateField>
                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle BackColor="Red" />
                    <HeaderTemplate>
                        <asp:Label ID="lblWorstCaseScenarioHeader" runat="server" >Worst Case Scenario</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblOtherHazard" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOTHERHAZARD")  %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle BackColor="#66ff33" />
                    <HeaderTemplate>
                        <asp:Label ID="lblControlsPrecaustionsHeader" runat="server" >Controls/Precautions</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%--<asp:Label ID="lblControls" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLS")  %>'></asp:Label>--%>
                        <asp:Literal ID="ltlControls" runat="server" Mode="PassThrough" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTROLS") %>'></asp:Literal>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle BackColor="#006600" />
                    <HeaderTemplate>
                        <asp:Label ID="lblRecommendedPPEHeader" runat="server" >Recommended PPE</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblRecommendedPPE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECOMMENDEPPE")  %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                    <HeaderStyle BackColor="DarkGray" />
                    <HeaderTemplate>
                       <asp:Label ID="lblCompetencyLevelHeader" runat="server">Competency Level <br /> for Supervision</asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblCompetencyLevel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPETENCYLEVEL")  %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
