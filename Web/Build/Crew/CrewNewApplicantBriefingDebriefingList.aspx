<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantBriefingDebriefingList.aspx.cs" Inherits="CrewNewApplicantBriefingDebriefingList" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BriefingTopic" Src="~/UserControls/UserControlBriefingTopic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Briefing De-Briefing List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCrewBriefingDebriefingList" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel runat="server" ID="AjaxPanel1" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuCrewBriefingDebriefingList" runat="server" OnTabStripCommand="CrewBriefingDebriefingList_TabStripCommand"></eluc:TabStrip>
            <table id="tblConfigureCrewBriefingDebriefingList" width="100%">
                <tr>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVessel" VesselsOnly="true" AppendDataBoundItems="true"
                            CssClass="dropdown_mandatory" Width="165px" Entitytype="VSL" ActiveVesselsOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtFromDate" CssClass="input_mandatory" Width="160px" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtToDate" CssClass="input_mandatory" Width="160px" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 10%">
                        <telerik:RadLabel ID="lblSubject" runat="server" Text="Subject"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtSubject" Width="160px" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblInstructor" runat="server" Text="Instructor"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:User ID="ddlUserAdd" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" Width="165px"
                            AutoPostBack="true" OnTextChangedEvent="ddlUserAdd_TextChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCompany" runat="server" Text="Location"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Company runat="server" ID="ucCompany" CssClass="input" AppendDataBoundItems="true" Width="165px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBriefingMode" runat="server" Text="Briefing Mode"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlBriefingMode" runat="server" CssClass="input"
                            EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="" />
                                <telerik:RadComboBoxItem Text="Face to Face" Value="1" />
                                <telerik:RadComboBoxItem Text="Telephonic" Value="2" />
                                <telerik:RadComboBoxItem Text="Skype" Value="3" />
                                <telerik:RadComboBoxItem Text="Vid Conf" Value="4" />
                            </Items>
                        </telerik:RadComboBox>
                        <%--<asp:DropDownList ID="ddlBriefingMode" runat="server" CssClass="input"  Width="165px">
                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                <asp:ListItem  Text="Face to Face" Value="1"></asp:ListItem>
                                <asp:ListItem  Text="Telephonic" Value="2"></asp:ListItem>
                                <asp:ListItem  Text="Skype" Value="3"></asp:ListItem>
                                <asp:ListItem  Text="Vid Conf" Value="4"></asp:ListItem>
                                </asp:DropDownList>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBrifingTopic" runat="server" Text="Briefing Details"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:BriefingTopic runat="server" ID="ddlBriefingTopic" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="155px" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <br />
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks:"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblTopicsdiscussed" runat="server" Text="Topics discussed"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtTopicsdiscussed" Width="60%" TextMode="MultiLine" Height="50px" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblCandidatesfeedback" runat="server" Text="Candidates feedback"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtCandidatesfeedback" Width="60%" TextMode="MultiLine" Height="50px" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblFieldOfficefeedback" runat="server" Text="Field Office feedback"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtFieldofficefeedback" Width="60%" TextMode="MultiLine" Height="50px" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td align="left" style="width: 10%">
                        <telerik:RadLabel ID="lblConclusion" runat="server" Text="Conclusion"></telerik:RadLabel>
                    </td>
                    <td style="width: 30%">
                        <telerik:RadTextBox runat="server" ID="txtConclusion" Width="60%" TextMode="MultiLine" Height="50px" CssClass="input"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

