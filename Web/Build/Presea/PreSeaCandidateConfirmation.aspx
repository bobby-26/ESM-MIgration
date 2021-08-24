<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaCandidateConfirmation.aspx.cs"
    Inherits="PreSeaCandidateConfirmation" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Venue" Src="~/UserControls/UserControlPreSeaExamVenue.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PreSea New Applicant</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">   
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
</telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewDateOfAvailability" runat="server">
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <eluc:Title runat="server" ID="ucTitle" Text="Candidate Selection" ShowMenu="false" />
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
            <eluc:TabStrip ID="MenuPreSea" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlDOA">
            <ContentTemplate>
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td>
                            First Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            Middle Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtMiddleName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            Last Name
                        </td>
                        <td>
                            <asp:TextBox ID="txtLastName" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Course
                        </td>
                        <td>
                            <asp:TextBox ID="txtCourse" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            Batch applied
                        </td>
                        <td>
                            <asp:TextBox ID="txtBatch" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            Venue
                        </td>
                        <td>
                            <eluc:Venue ID="ucExamVenue" runat="server" CssClass="readonlytextbox" AppendDataBoundItems="true"
                                Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Exam Date
                        </td>
                        <td>
                            <eluc:Date ID="ucExamdate" runat="server" CssClass="readonlytextbox" Width="150px" />
                        </td>
                        <td>
                            Interview By
                        </td>
                        <td>
                            <asp:TextBox ID="txtInterviewBy" runat="server" CssClass="readonlytextbox" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            Entrance Roll No
                        </td>
                        <td>
                            <asp:TextBox ID="txtRollNo" runat="server" Width="150px" ToolTip="Entrance Roll No"
                                CssClass="readonlytextbox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Present Status
                        </td>
                        <td>
                            <asp:TextBox ID="txtStatus" runat="server" Width="150px" ToolTip="Present Status"
                                CssClass="readonlytextbox"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <h4>
                                Confirmation Details</h4>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Max Limit
                        </td>
                        <td align="right">
                            <asp:TextBox ID="txtMaxLimit" runat="server" ReadOnly="true" Width="150px" CssClass="readonlytextbox"
                                Style="text-align: right;"></asp:TextBox>
                        </td>
                        <td>
                            Confirmed
                        </td>
                        <td>
                            <asp:TextBox ID="txtConfirmed" runat="server" ReadOnly="true" Width="150px" CssClass="readonlytextbox"
                                Style="text-align: right;"></asp:TextBox>
                        </td>
                        <td>
                            Waitlisted
                        </td>
                        <td>
                            <asp:TextBox ID="txtWaitListed" runat="server" ReadOnly="true" Width="150px" CssClass="readonlytextbox"
                                Style="text-align: right;"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <br />
                <table>
                    <tr>
                        <td>
                            Interview Status
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddlSatus" runat="server" CssClass="input_mandatory" OnSelectedIndexChanged="ddlStatus_Changed"
                                AutoPostBack="true" DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE"
                                Width="150px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Confirmed Batch(Posted)
                        </td>
                        <td colspan="2">                           
                                 <asp:DropDownList ID="ddlBatch" runat="server" DataTextField="FLDBATCH" DataValueField="FLDBATCHID"
                                            CssClass="dropdown_mandatory" OnSelectedIndexChanged ="ddlBatch_Changed">
                                        </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Reason for Rejection
                        </td>
                        <td>
                            <eluc:Quick ID="ddlRejectReason" runat="server" CssClass="input" AppendDataBoundItems="true"
                                Width="300px" QuickTypeCode="100" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Qualified in IMU Entrance
                        </td>
                        <td valign="middle">
                            <asp:RadioButtonList ID="rdoIMUQulify" runat="server" CssClass="input" Width="300px"
                                RepeatDirection="Horizontal">
                                <asp:ListItem Text="Yes" Value="1"></asp:ListItem>
                                <asp:ListItem Text="No" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Not Applicable" Value="-1"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
