
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseReviewHistory.aspx.cs"
    Inherits="CrewCourseReviewHistory"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Custom" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Course Review History List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCourseHistory" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewCourseEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <asp:Literal ID="lblCourseReviewHistory" runat="server" Text="Course Review History"></asp:Literal>
                    </div>
                </div>
               
                <table cellpadding="2" cellspacing="2" width="100%">
                  
                    <tr>
                        <td>
                            <asp:Literal ID="lblCourse" runat="server" Text="Course"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Course ID="ucCourse" runat="server" AppendDataBoundItems="true" CssClass="readonlytextbox"
                                Enabled="false" />
                        </td>
                        <td>
                            <asp:Literal ID="lblCourseType" runat="server" Text="Course Type"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard runat="server" ID="ucCourseType" CssClass="readonlytextbox" AppendDataBoundItems="true"
                                Enabled="false" HardTypeCode="103" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblRevisionDate" runat="server" Text="Revision Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucRevisionDate" runat="server" CssClass="readonlytextbox"  ReadOnly="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblReviewedBy" runat="server" Text="Reviewed By"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLastEditedBy" runat="server" CssClass="readonlytextbox" Width="220px" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblDescription" runat="server" Text="Description"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescription" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                                Height="40px" Width="420px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblSubjectMatter" runat="server" Text="Subject Matter"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSubjectMatter" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="40px" Width="420px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblLearningObjective" runat="server" Text="Learning Objective"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLearningTarget" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="40px" Width="420px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblMethodology" runat="server" Text="Methodology"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMethodology" runat="server" CssClass="input" TextMode="MultiLine"
                                Height="40px" Width="420px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblMinimumRequirementsforcandidates" runat="server" Text="Minimum Requirements for candidates"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRequirement" runat="server" CssClass="input_mandatory" TextMode="MultiLine"
                                Height="40px" Width="420px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblNotes" runat="server" Text="Notes"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNotes" runat="server" CssClass="input" TextMode="MultiLine" Width="420px"
                                Height="40px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblCourseOutline" runat="server" Text="Course Outline"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <eluc:Custom ID="txtCourseOutline" runat="server" Width="100%" Height="320px" PictureButton="true"
                                DesgMode="true" HTMLMode="true" PrevMode="true" OnFileUploadEvent="btnInsertPic_Click" />
                        </td>
                    </tr>
                  
                </table>
                
            </div>
    </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
