<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaBatchPlanExamDetails.aspx.cs"
    Inherits="PreSeaBatchPlanExamDetails" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit.HTMLEditor"
    TagPrefix="cc1" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Venue" Src="~/UserControls/UserControlPreSeaExamVenue.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Date Of Availabilty</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewDateOfAvailability" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDOA">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        Batch - Entrance Exam Venue
                    </div>
                    <div style="top: 0px; right: 0px; position: absolute">
                        <eluc:TabStrip ID="MenuBatchPlanner" runat="server" OnTabStripCommand="BatchPlanner_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuPreSea" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table cellpadding="1" cellspacing="1" width="50%">
                    <tr>
                        <td>
                            Course
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtCourse" runat="server" CssClass="readonlytextbox" Enabled="false"
                                Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Batch
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtBatchName" runat="server" CssClass="readonlytextbox" Enabled="false"
                                Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                     <tr>
                        <td>
                            Start Date
                        </td>
                        <td valign="baseline" colspan="3">
                            <eluc:Date ID="txtStartDate" runat="server" CssClass="input_mandatory" Width="120px" />
                            <%--&nbsp;
                            <asp:ImageButton ID="imgbtnTiming" BorderWidth="0" Style="cursor: pointer; vertical-align: top"
                                runat="server" ImageUrl="<%$ PhoenixTheme:images/time-schedule.png %>" ToolTip="Exam day time Schedule" />--%>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            No of days
                        </td>
                        <td colspan="3">
                            <eluc:Number runat="server" ID="txtNoofdays" CssClass="input_mandatory" IsInteger="true"
                                Width="120px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Medical fee
                        </td>
                        <td colspan="3">
                            <eluc:Number runat="server" ID="txtMedfees" CssClass="input_mandatory" IsInteger="false"
                                Width="120px" DecimalPlace="2" IsPositive="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Venue
                        </td>
                        <td colspan="3">
                            <asp:DropDownList ID="ddlExamVenue" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                AutoPostBack="true" OnSelectedIndexChanged="ddlExamVenue_changed" Width="120px">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Zone
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtZoneName" runat="server" CssClass="readonlytextbox" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Address
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtVenueAddress" runat="server" CssClass="readonlytextbox" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                   
                    <tr>
                        <td>
                            Contact Person
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="readonlytextbox" Enabled="false"
                                Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Phone Numbers
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtContactNos" runat="server" CssClass="readonlytextbox" Enabled="false" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td>
                            E-Mail
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtContactMail" runat="server" CssClass="readonlytextbox" Enabled="false"
                                Width="120px"></asp:TextBox>
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
