<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewCourseOrganization.aspx.cs"
    Inherits="CrewCourseOrganization" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="../UserControls/UserControlAddressType.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Organization List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCourseOrganization" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCourseOrgainzationEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <asp:Literal ID="lblOrganization" runat="server" Text="Organization"></asp:Literal>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuCourseOrganization" runat="server" OnTabStripCommand="CourseOrganization_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <br />
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
                        <%--<td>
                            Course Status
                        </td>
                        <td>
                            <asp:CheckBox ID="chkStatus" runat="server" Enabled="false" />
                        </td>--%>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblDuration" runat="server" Text="Duration"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtDay" runat="server" CssClass="input_mandatory txtNumber" MaxLength="4"
                                IsInteger="true" />
                            <eluc:Hard ID="ucTime" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                Visible="false" HardTypeCode="151" Enabled="false" />
                            <asp:Literal ID="lblDays" runat="server" Text="Days"></asp:Literal>
                            <eluc:Number ID="txtHour" runat="server" CssClass="input_mandatory txtNumber" MaxLength="3"
                                IsInteger="false" />
                            <asp:Literal ID="lblHours" runat="server" Text="Hours"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblTimings" runat="server" Text="Timings"></asp:Literal>
                        </td>
                        <td>
                            <asp:Literal ID="lblStart" runat="server" Text="Start :"></asp:Literal>
                            <asp:TextBox ID="txtStartTime" runat="server" CssClass="input_mandatory" Width="50px" />
                            <ajaxToolkit:MaskedEditExtender ID="txtStartTimeMaskEdit" runat="server" TargetControlID="txtStartTime"
                                Mask="99:99" MaskType="Time" AcceptAMPM="true" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" />
                            <asp:Literal ID="lblTo" runat="server" Text="To :"></asp:Literal>
                            <asp:TextBox ID="txtEndTime" runat="server" CssClass="input_mandatory" Width="50px" />
                            <ajaxToolkit:MaskedEditExtender ID="txtEndTimeMaskEdit" runat="server" TargetControlID="txtEndTime"
                                Mask="99:99" MaskType="Time" AcceptAMPM="true" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblMinimumParticipants" runat="server" Text="Minimum Participants"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtMinimumParticipant" runat="server" CssClass="input_mandatory txtNumber"
                                MaxLength="4" IsInteger="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblMaximumParticipants" runat="server" Text="Maximum Participants"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtMaximumParticipant" runat="server" CssClass="input_mandatory txtNumber"
                                MaxLength="4" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Panel ID="pnlWritten" runat="server" GroupingText="Written" Width="70%">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="lblWrittenMaxMarks" runat="server" Text="Written Max Marks"></asp:Literal>
                                        </td>
                                        <td>
                                            <eluc:Number ID="txtWrittenPassMarks" runat="server" CssClass="input_mandatory" IsInteger="false" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="lblWrittenMinPass" runat="server" Text="Written Min Pass %"></asp:Literal>
                                        </td>
                                        <td>
                                            <eluc:Number ID="txtWrittenPassPercentage" runat="server" CssClass="input_mandatory"
                                                IsInteger="false" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td colspan="2">
                            <asp:Panel ID="pnlCBT" runat="server" GroupingText="CBT" Width="70%">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Literal ID="lblCBTMaxMarks" runat="server" Text="CBT Max Marks"></asp:Literal>
                                        </td>
                                        <td>
                                            <eluc:Number ID="txtCBTPassMarks" runat="server" CssClass="input_mandatory" IsInteger="false" />
                                        </td>
                                        <td>
                                            <asp:Literal ID="lblCBTMinPass" runat="server" Text="CBT Min Pass %"></asp:Literal>
                                        </td>
                                        <td>
                                            <eluc:Number ID="txtCBTPassPercentage" runat="server" CssClass="input_mandatory"
                                                IsInteger="false" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCostperparticipant" runat="server" Text="Cost per participant"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtCost" runat="server" CssClass="input txtNumber" MaxLength="10"
                                IsInteger="false" Width="80px" />
                            <eluc:Currency runat="server" ID="ucCurrency" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblApprovednumberofcoursesmonth" runat="server" Text="Approved number of courses/month"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtApprovalpermonth" runat="server" CssClass="input" IsInteger="true"
                                AutoPostBack="true" OnTextChangedEvent="CalculatePerYear" />
                        </td>
                        <td>
                            <asp:Literal ID="lblApprovednumberofcoursesyear" runat="server" Text="Approved number of courses/year"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Number ID="txtApprovalperyear" runat="server" CssClass="readonlytextbox" IsInteger="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblApprovalStatus" runat="server" Text="Approval Status"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Quick runat="server" ID="ucApprovalStatus" AppendDataBoundItems="true" QuickTypeCode="65"
                                CssClass="input" />
                        </td>
                        <td>
                            <asp:Literal ID="lblLanguage" runat="server" Text="Language"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Quick runat="server" ID="ucLanguage" AppendDataBoundItems="true" QuickTypeCode="22"
                                CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblInstitution" runat="server" Text="Institution"></asp:Literal>
                        </td>
                        <td>
                            <div runat="server" id="dvRank" class="input_mandatory" style="overflow: auto; width: 80%;
                                height: 140px">
                                <asp:CheckBoxList runat="server" ID="cblInstitution" Height="100%" RepeatColumns="1"
                                    RepeatDirection="Horizontal" RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td>
                            <asp:Literal ID="lblRemarksifanychangesdonetoApprovalpermonthyear" runat="server" Text="Remarks (if any changes done to Approval per month/year)"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNotes" runat="server" CssClass="input" TextMode="MultiLine" Height="60px"
                                Width="320px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
