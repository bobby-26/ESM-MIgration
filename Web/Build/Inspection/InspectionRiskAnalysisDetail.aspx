<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRiskAnalysisDetail.aspx.cs"
    Inherits="InspectionRiskAnalysisDetail" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategory.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRiskAnalysisDetail" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlRiskAnalysisDetail">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="Title1" Text="Risk Analysis Detail" ShowMenu="<%# Title1.ShowMenu %>">
                        </eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div class="subHeader" style="position: relative;">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="MenuRiskAnalysisDetail" runat="server" OnTabStripCommand="RiskAnalysisDetail_TabStripCommand">
                        </eluc:TabStrip>
                    </span>
                </div>
                <table width="100%" cellpadding="1" cellspacing="3">
                    <tr>
                        <td>
                           <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucDate" runat="server" ReadOnly="true" CssClass="readonlytextbox" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCategory" runat="Server" Text="Category"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Category ID="ucCategory" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                AutoPostBack="true" OnTextChangedEvent="BindSubCategory" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblSubCategory" runat="server" Text="Sub Category"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlSubCategory" runat="server" CssClass="dropdown_mandatory"
                                AppendDataBoundItems="true">
                                <asp:ListItem Text="--Select--" Value="Dummy"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:literal ID="lblActivity" runat="server" Text="Activity"></asp:literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtActivity" runat="server" CssClass="input_mandatory" Width="360px"
                                TextMode="MultiLine" Rows="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblReason" runat="server" Text="Reason"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBoxList ID="cblReason" runat="server" RepeatDirection="Horizontal" RepeatColumns="4">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblHealthandSafety" runat="server" Text="Health and Safety"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblElementsBeingAssessedOn" runat="server" Text="Elements being assessed on"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHSRisk" runat="server" CssClass="input_mandatory" Width="360px"
                                TextMode="MultiLine" Rows="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblActivityDuration" runat="server" Text="Activity Duration"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblHSActivityDuration" runat="server" RepeatDirection="Horizontal"
                                RepeatColumns="4">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblNoOfPeopleInvolved" runat="server" Text="No of people involved"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblHSNoofpeople" runat="server" RepeatDirection="Horizontal"
                                RepeatColumns="4">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblControls" runat="server" Text="Controls"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblHSControls" runat="server" RepeatDirection="Horizontal"
                                RepeatColumns="4">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblHazard" runat="server" Text="Hazard"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBoxList ID="cblHSHazard" runat="server" RepeatDirection="Horizontal" RepeatColumns="4">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table cellpadding="1" cellspacing="1" width="100%">
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblProbabilityofOccurance" runat="server" 
                                            Text="Probabilty of Occurance"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtHSPOC" runat="server" CssClass="readonlytextbox" 
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblLikelyhoodOfHarm" runat="server" Text="Likelyhood of harm"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtHSLOH" runat="server" CssClass="readonlytextbox" 
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblLevelofControl" runat="server" Text="Level of Control"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtHSLOC" runat="server" CssClass="readonlytextbox" 
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lbllevelofRisk" runat="server" Text="Level of risk"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtHSLevelOfRisk" runat="server" CssClass="readonlytextbox" 
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblEnvironmental" runat="server" Text="Environmental"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblElementsBeingAssessedOn1" runat="server" Text="Elements being assessed on"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEnvRisk" runat="server" CssClass="input_mandatory" Width="360px"
                                TextMode="MultiLine" Rows="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblActivityDuration1" runat="server" Text="Activity Duration"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblEnvActivityDuration" runat="server" RepeatDirection="Horizontal"
                                RepeatColumns="4">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblActivityFrequency" runat="server" Text="Activity Frequency"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblEnvActivityFrequency" runat="server" RepeatDirection="Horizontal"
                                RepeatColumns="4">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                           <asp:Literal ID="lblControls1" runat="server" Text="Controls"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblEnvControls" runat="server" RepeatDirection="Horizontal"
                                RepeatColumns="4">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblImpact" runat="server" Text="Impact"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBoxList ID="cblEnvHazard" runat="server" RepeatDirection="Horizontal" RepeatColumns="4">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                      <tr>
                        <td colspan="4">
                            <table cellpadding="1" cellspacing="1" width="100%">
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblProbabilityofOccurance1" runat="server" 
                                            Text="Probabilty of Occurance"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEnvPOC" runat="server" CssClass="readonlytextbox" 
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblLikelyHoodofHarm1" runat="Server" Text="Likelyhood of harm"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEnvLOH" runat="server" CssClass="readonlytextbox" 
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblLevelofControl1" runat="server" Text="Level of Control"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEnvLOC" runat="server" CssClass="readonlytextbox" 
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblLevelofRisk1" runat="server" Text=" Level of risk"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEnvLevelOfRisk" runat="server" CssClass="readonlytextbox" 
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                    </tr>
                     <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblOthers" runat="server" Text="Others"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblElementsBeingAssessedOn2" runat="server" Text="Elements being assessed on"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOthers" runat="server" CssClass="input_mandatory" Width="360px"
                                TextMode="MultiLine" Rows="3"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblActivityDuration2" runat="server" Text="Activity Duration"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblOtherActivityDuration" runat="server" RepeatDirection="Horizontal"
                                RepeatColumns="4">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblActivityFrequency1" runat="server" Text="Activity Frequency"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblOtherActivityFrequency" runat="server" RepeatDirection="Horizontal"
                                RepeatColumns="4">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblControls2" runat="server" Text="Controls"></asp:Literal>
                        </td>
                        <td>
                            <asp:RadioButtonList ID="rblOtherControls" runat="server" RepeatDirection="Horizontal"
                                RepeatColumns="4">
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblConsequences" runat="server" Text="Consequences"></asp:Literal>
                        </td>
                        <td>
                            <asp:CheckBoxList ID="cblOtherConsequences" runat="server" RepeatDirection="Horizontal" RepeatColumns="4">
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                      <tr>
                        <td colspan="4">
                            <table cellpadding="1" cellspacing="1" width="100%">
                                <tr>
                                    <td>
                                        <asp:Literal ID="lblProbabilityofOccurance2" runat="server" 
                                            Text="Probabilty of Occurance"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOtherPOC" runat="server" CssClass="readonlytextbox" 
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblLikelyhoodofHarm2" runat="server" Text="Likelyhood of harm"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOtherLOH" runat="server" CssClass="readonlytextbox" 
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lblLevelofControl2" runat="server" Text="Level of Control"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLOC" runat="server" CssClass="readonlytextbox" 
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:Literal ID="lbLLevelofRisk2" runat="server" Text="Level of risk"></asp:Literal>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtOtherLevelofrisk" runat="server" CssClass="readonlytextbox" 
                                            ReadOnly="true"></asp:TextBox>
                                    </td>
                                </tr>
                            </table>
                    </tr>
                </table>
                <eluc:Status ID="ucStatus" runat="server" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
