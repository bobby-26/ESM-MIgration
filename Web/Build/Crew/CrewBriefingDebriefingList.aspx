<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewBriefingDebriefingList.aspx.cs" Inherits="CrewBriefingDebriefingList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BriefingTopic" Src="~/UserControls/UserControlBriefingTopic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Briefing De-Briefing List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCrewBriefingDebriefingList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
          <eluc:TabStrip ID="MenuCrewBriefingDebriefingList" runat="server" OnTabStripCommand="CrewBriefingDebriefingList_TabStripCommand"></eluc:TabStrip>
         <eluc:TabStrip ID="Csvtow" runat="server" OnTabStripCommand="converttoword_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
          
           
            <table id="tblConfigureCrewBriefingDebriefingList" width="100%">
                <tr>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVessel" VesselsOnly="true" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" Width="30%" Entitytype="VSL" ActiveVesselsOnly="true" AssignedVessels="true" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="Briefing From Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Date runat="server" ID="txtFromDate" CssClass="input_mandatory" Width="30%" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="Briefing To Date"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:Date runat="server" ID="txtToDate" CssClass="input_mandatory" Width="30%"/>
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtSubject" CssClass="input_mandatory" Width="30%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblInstructor" runat="server" Text="Instructor(Office)"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <eluc:User ID="ddlUserAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                            AutoPostBack="true" OnTextChangedEvent="ddlUserAdd_TextChanged" DepartmentType="2" Width="30%" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="Literal1" runat="server" Text="Instructor(Owner)"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtInstructorOwner"  Width="30%"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Location"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company runat="server" ID="ucCompany"  AppendDataBoundItems="true" Width="30%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBriefingMode" runat="server" Text="Briefing Mode"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID ="ddlBriefingMode" runat="server" Width="30%" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="true">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                <telerik:RadComboBoxItem Value="1" Text ="Face to Face" />
                                <telerik:RadComboBoxItem Value="2" Text="Telephonic" />
                                <telerik:RadComboBoxItem Value="3" Text="Skype" />
                                <telerik:RadComboBoxItem Value="4" Text="Video Conference" />
                            </Items>
                        </telerik:RadComboBox>                      
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBrifingTopic" runat="server" Text="Briefing Details"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:BriefingTopic runat="server" ID="ddlBriefingTopic" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="30%"/>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <br />
                        <b>
                            <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks:"></telerik:RadLabel>
                        </b>
                    </td>
                    <td style="width: 30%">                       
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblTopicsdiscussed" runat="server" Text="Topics Discussed"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtTopicsdiscussed" Width="60%" TextMode="MultiLine" Height="50px" ></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblCandidatesfeedback" runat="server" Text="Candidates Feedback"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtCandidatesfeedback" Width="60%" TextMode="MultiLine" Height="50px" ></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblFieldOfficefeedback" runat="server" Text="Field Office Feedback"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtFieldofficefeedback" Width="60%" TextMode="MultiLine" Height="50px" ></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblConclusion" runat="server" Text="Conclusion"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtConclusion" Width="60%" TextMode="MultiLine" Height="50px" ></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

