<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewJoiningLetters.aspx.cs"
    Inherits="Crew_CrewJoiningLetters" %>

<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="~/UserControls/UserControlDocuments.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Seaport" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Letters And Forms</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
        EnableShadow="true">
    </telerik:RadWindowManager>
    <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="tblConfigureCity"
        runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1"
        Height="100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
        <eluc:TabStrip ID="MenuLettersAndForms" runat="server" OnTabStripCommand="MenuLettersAndForms_TabStripCommand">
        </eluc:TabStrip>
        <table>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSeafarer" runat="server" Text="Seafarer File Number">
                    </telerik:RadLabel>
                </td>
                <td style="width: 15%;">
                    <telerik:RadTextBox runat="server" ID="txtFileNo" CssClass="input_mandatory">
                    </telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtEmpId" runat="server" Width="5%">
                    </telerik:RadTextBox>
                    <asp:LinkButton ID="LinkButton1" runat="server" OnClick="ImgBtnValidFileno_Click"
                        ToolTip="Verify Entered File Number"><span class="icon"><i class="fas fa-search"></i></span></asp:LinkButton>
                </td>
                <td>
                    <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                        VesselsOnly="true" Entitytype="VSL" AssignedVessels="true" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblDateofJoining" runat="server" Text="Date of Joining">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Date ID="uDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox"
                        ReadOnly="true">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="Employee Number">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox"
                        ReadOnly="true">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblRank" runat="server" Text="Rank">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true">
                    </telerik:RadTextBox>
                    <telerik:RadLabel ID="lblRankid" runat="server" Visible="false">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadLabel ID="lblPortofJoining" runat="server" Text="Port of Joining">
                    </telerik:RadLabel>
                </td>
                <td>
                    <eluc:Seaport ID="ucSeaport" runat="server" AppendDataBoundItems="true" Width="200px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblRequestedDCEType" runat="server" Text="Requested DCE Type">
                    </telerik:RadLabel>
                </td>
                <td colspan="5">
                    <eluc:Documents ID="ddlLicence" runat="server" AppendDataBoundItems="true" DocumentType="LICENCE" />
                </td>
            </tr>
        </table>
        <table>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblFlightSchedule" runat="server" Text="Flight Schedule">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtFlightSchedule" runat="server" CssClass="input" TextMode="MultiLine"
                        Width="250px" Height="100px">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblAgentAddress" runat="server" Text="Agent Address">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtAgentAddress" runat="server" CssClass="input" TextMode="MultiLine"
                        Width="250px" Height="100px">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblSeatimeOtherComments" runat="server" Text="Seatime Other Comments">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtSeaTimeComments" runat="server" CssClass="input" TextMode="MultiLine"
                        Width="250px" Height="100px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDceAddress" runat="server" Text="Dce Address">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtDceAddress" runat="server" CssClass="input" TextMode="MultiLine"
                        Width="250px" Height="100px">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblCargoDetails" runat="server" Text="Cargo Details">
                    </telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox ID="txtCargoDetails" runat="server" CssClass="input" TextMode="MultiLine"
                        Width="250px" Height="100px">
                    </telerik:RadTextBox>
                </td>
            </tr>
        </table>
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
