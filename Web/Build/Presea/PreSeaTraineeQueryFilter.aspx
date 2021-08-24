<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaTraineeQueryFilter.aspx.cs" Inherits="PreSeaTraineeQueryFilter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlNationalityList" Src="../UserControls/UserControlNationalityList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="State" Src="~/UserControls/UserControlState.ascx" %>
<%@ Register TagPrefix="eluc" TagName="City" Src="~/UserControls/UserControlCity.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaCourse" Src="~/UserControls/UserControlPreSeaCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaExamVenue" Src="~/UserControls/UserControlPreSeaExamVenue.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaQualificaiton" Src="~/UserControls/UserControlPreSeaQualification.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PreSea Query Filter</title>
    <style type="text/css">
        .style1
        {
            width: 519px;
        }
    </style>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Trainee"></asp:Label>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuActivityFilterMain" runat="server" OnTabStripCommand="NewApplicantFilterMain_TabStripCommand">
        </eluc:TabStrip>
       <%-- <asp:Button ID="btnSubmit" runat="server" OnClick="btnSubmit_Click" />--%>
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlNewApplicantEntry">
        <ContentTemplate>
            <div id="divFind">
                <table cellpadding="2" cellspacing="2" width="100%">
                    <tr>
                        <td colspan="4">
                            <font color="blue"><b>Note: </b>For embeded search, use '%' symbol. (Eg. Name: %xxxx)</font>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="1">
                            Name
                        </td>
                        <td colspan="1">
                            <asp:TextBox ID="txtName" runat="server" CssClass="input" Width="200px" MaxLength="200"></asp:TextBox>
                        </td>
                        <td>
                            Applied Between
                        </td>
                        <td>
                            <eluc:UserControlDate ID="txtAppliedStartDate" CssClass="input" Width="80px" runat="server" />
                            &nbsp;to&nbsp;
                            <eluc:UserControlDate ID="txtAppliedEndDate" CssClass="input" Width="80px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            Nationality
                        </td>
                        <td class="style1">
                            <eluc:UserControlNationalityList ID="lstNationality" runat="server" CssClass="input" />
                            <br />
                            <font color="blue">(Press Control for Multiple Selection)</font>
                        </td>
                        <td>
                            Date of Birth Between
                        </td>
                        <td class="style1">
                            <eluc:UserControlDate ID="txtDOBStartDate" CssClass="input" Width="80px" runat="server" />
                            &nbsp;to&nbsp;
                            <eluc:UserControlDate ID="txtDOBEndDate" CssClass="input" Width="80px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Gender
                        </td>
                        <td>
                            <eluc:UserControlHard ID="ddlSex" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                        <td>
                            Country
                        </td>
                        <td>
                            <eluc:Country ID="ddlCountry" runat="server" AutoPostBack="true" AppendDataBoundItems="true"
                                CssClass="input" OnTextChangedEvent="ddlCountry_TextChanged" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            State
                        </td>
                        <td>
                            <eluc:State ID="ddlState" CssClass="input" runat="server" AppendDataBoundItems="true"
                                AutoPostBack="true" OnTextChangedEvent="ddlState_TextChanged" />
                        </td>
                        <td>
                            City
                        </td>
                        <td>
                            <eluc:City ID="ddlCity" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                            <b>Batch</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Batch
                        </td>
                        <td>
                            <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="input" AppendDataBoundItems="true" IsCalledFromPresea="1"
                                IsOutside="false" />
                        </td>
                        <td>
                            Qualification
                        </td>
                        <td>
                            <eluc:PreSeaQualificaiton ID="ddlQualificaiton" runat="server" CssClass="input" AutoPostBack="true"
                                AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Course
                        </td>
                        <td>
                            <eluc:PreSeaCourse ID="ucPreSeaCourse" runat="server" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <hr />
                            <b>Exam Venue</b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            First Choice
                        </td>
                        <td>
                            <eluc:PreSeaExamVenue ID="ucExamVenue1" runat="server" AppendDataBoundItems="true"
                                CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Second Choice
                        </td>
                        <td>
                            <eluc:PreSeaExamVenue ID="ucExamVenue2" runat="server" AppendDataBoundItems="true"
                                CssClass="input" />
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
