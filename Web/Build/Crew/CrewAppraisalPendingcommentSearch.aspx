<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewAppraisalPendingcommentSearch.aspx.cs"
    Inherits="Crew_CrewAppraisalPendingcommentSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlNationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register Src="../UserControls/UserControlZoneList.ascx" TagName="UserControlZoneList"
    TagPrefix="eluc" %>
<%@ Register Src="../UserControls/UserControlPoolList.ascx" TagName="UserControlPoolList"
    TagPrefix="eluc" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pending seafarer comments search</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


                <eluc:TabStrip ID="PlanRelieveeFilterMain" runat="server" Title="Filter" OnTabStripCommand="PlanRelieveeFilterMain_TabStripCommand"></eluc:TabStrip>

                <table width="50%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFileNo" runat="server"   MaxLength="50" Width="200px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true"  
                                VesselsOnly="true" AssignedVessels="true"  Width="200px"/>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblaprFrom" runat="server" Text="Appraisal From"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtaprFrom" runat="server"   DatePicker="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblaprTo" runat="server" Text="To"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtaprTo" runat="server"   DatePicker="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:UserControlZoneList ID="ucZone" Width="200px" AppendDataBoundItems="true" runat="server"
                                />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPool" runat="server" Text="Pool"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:UserControlPoolList ID="ucPool" AppendDataBoundItems="true" runat="server"
                                 Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRankHeader" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Rank ID="ucRank" runat="server"  Width="200px"   AppendDataBoundItems="true" />
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
