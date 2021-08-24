<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseEvaluationList.aspx.cs"
    Inherits="CrewCourseEvaluationList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Evaluation List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCourseEvaluationList" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxtoolkit:toolkitscriptmanager>
    <asp:UpdatePanel runat="server" ID="pnlCrewCourseEntry">
        <ContentTemplate>
            <eluc:error id="ucError" runat="server" text="" visible="false"></eluc:error>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <eluc:status runat="server" id="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <asp:Literal ID="lblCourseEvaluation" runat="server" Text="Course Evaluation"></asp:Literal>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:tabstrip id="MenuCourseEvaluationList" runat="server" ontabstripcommand="CourseEvaluationList_TabStripCommand">
                    </eluc:tabstrip>
                </div>
                <br />
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                        </td>
                        <td>
                            <eluc:course id="ucCourse" runat="server" appenddatabounditems="true" cssclass="readonlytextbox"
                                enabled="false" />
                        </td>
                        <td>
                            <asp:Literal ID="lblCourseType" runat="server" Text="Course Type"></asp:Literal>
                        </td>
                        <td>
                            <eluc:hard runat="server" id="ucCourseType" cssclass="readonlytextbox" appenddatabounditems="true"
                                enabled="false" hardtypecode="103" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblItemCode" runat="server" Text="Item Code"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtItemCode" MaxLength="10" Width="120px" CssClass="readonlytextbox"
                                Enabled="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblItemName" runat="server" Text="Item Name"></asp:Literal>
                        </td>
                        <td>
                            <eluc:hard id="ucItemName" appenddatabounditems="true" cssclass="readonlytextbox"
                                enabled="false" runat="server" hardtypecode="149" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblDescription" runat="server" Text="Description"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemDescription" runat="server" CssClass="readonlytextbox" TextMode="MultiLine"
                                Enabled="false" Width="360px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblSortOrder" runat="server" Text="Sort Order"></asp:Literal>
                        </td>
                        <td>
                          <eluc:Number runat="server" ID="txtSortOrder" CssClass="input_mandatory" IsInteger="true" MaxLength="3"  ></eluc:Number>
                           
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRatingScale" runat="server" Text="Rating Scale"></asp:Literal>
                        </td>
                        <td>
                            <eluc:hard id="ucRatingScale" appenddatabounditems="true" cssclass="readonlytextbox"
                                enabled="false" runat="server" hardtypecode="150" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <font color="blue"><b><asp:Literal ID="lblNote" runat="server" Text="Note:"></asp:Literal> </b> <asp:Literal ID="lblBelowisalistofdefaultdescriptionsforeachratingTousecustomizeddescriptionsselecttheoptionCustomizeDescriptionsandentertherequireddescriptionsbelowThesedescriptionswillbeusedwhentheaboveitemisevaluated" runat="server" Text="Below is a list of default descriptions for each rating. To use customized descriptions select the option 'Customize Descriptions' and enter the required descriptions below. These descriptions will be used when the above item is evaluated."></asp:Literal></font>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <b><asp:Literal ID="lblDefaultDescription" runat="server" Text="Default Description"></asp:Literal></b>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkEditDescriptors" runat="server" Font-Bold="true" Text="Customize Description" AutoPostBack="true"
                            OnCheckedChanged="EnableDescriptor"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDescriptor1" runat="server">Very Poor</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescriptor1" runat="server" CssClass="readonlytextbox" TextMode="MultiLine"
                                Enabled="false" Width="360px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDescriptor1Update" runat="server" CssClass="readonlytextbox"
                                TextMode="MultiLine" Enabled="false" Width="360px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDescriptor2" runat="server" Text=" Poor"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescriptor2" runat="server" CssClass="readonlytextbox" TextMode="MultiLine"
                                Enabled="false" Width="360px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDescriptor2Update" runat="server" CssClass="readonlytextbox"
                                TextMode="MultiLine" Enabled="false" Width="360px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDescriptor3" runat="server" Text="Average"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescriptor3" runat="server" CssClass="readonlytextbox" TextMode="MultiLine"
                                Enabled="false" Width="360px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDescriptor3Update" runat="server" CssClass="readonlytextbox"
                                TextMode="MultiLine" Enabled="false" Width="360px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDescriptor4" runat="server" Text="Good"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescriptor4" runat="server" CssClass="readonlytextbox" TextMode="MultiLine"
                                Enabled="false" Width="360px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDescriptor4Update" runat="server" CssClass="readonlytextbox"
                                TextMode="MultiLine" Enabled="false" Width="360px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDescriptor5" runat="server" Text="Very Good"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescriptor5" runat="server" CssClass="readonlytextbox" TextMode="MultiLine"
                                Enabled="false" Width="360px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDescriptor5Update" runat="server" CssClass="readonlytextbox"
                                TextMode="MultiLine" Enabled="false" Width="360px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDescriptor6" runat="server" Text="Excellent"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescriptor6" runat="server" CssClass="readonlytextbox" TextMode="MultiLine"
                                Enabled="false" Width="360px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDescriptor6Update" runat="server" CssClass="readonlytextbox"
                                TextMode="MultiLine" Enabled="false" Width="360px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDescriptor7" runat="server" Text="Comment1"></asp:Label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescriptor7" runat="server" CssClass="readonlytextbox" TextMode="MultiLine"
                                Enabled="false" Width="360px"></asp:TextBox>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDescriptor7Update" runat="server" CssClass="readonlytextbox"
                                TextMode="MultiLine" Enabled="false" Width="360px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
