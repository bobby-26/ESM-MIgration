<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLettersAndForms.aspx.cs"
    Inherits="Crew_CrewLettersAndForms" %>

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
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Letters And Forms</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="form1" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server">
        </telerik:RadSkinManager>

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>
            <eluc:TabStrip ID="MenuLettersAndForms" runat="server" OnTabStripCommand="MenuLettersAndForms_TabStripCommand"></eluc:TabStrip>
            <div id="divFind" runat="server">
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSeafarerFileNumber" runat="server" Text="Seafarer File Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtFileNo" Width="60%" CssClass="input_mandatory"></telerik:RadTextBox>
                            <asp:ImageButton ID="ImgBtnValidFileno" runat="server" ImageAlign="AbsBottom" ImageUrl="<%$ PhoenixTheme:images/tableviewobservation.png %>"
                                ToolTip="Verify Entered File Number" OnClick="ImgBtnValidFileno_Click" />
                            <telerik:RadTextBox ID="txtEmpId" runat="server"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" VesselsOnly="true" />
                        </td>
                    </tr>
                </table>
            </div>

            <table width="100%">
                <tr>
                    <td style="width:13%;">
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td style="width:20%;">
                        <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td style="width:13%;">
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td style="width:20%;">
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td style="width:13%;">
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td style="width:20%;">
                        <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"></telerik:RadTextBox>
                        <asp:Label ID="lblRankid" runat="server" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDateofJoining" runat="server" Text="Date of Joining"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="uDate" runat="server" CssClass="input" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPortofJoining" runat="server" Text="Port of Joining"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Seaport ID="ucSeaport" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFlightSchedule" runat="server" Text="Flight Schedule"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFlightSchedule" runat="server" CssClass="input" TextMode="MultiLine"
                            Width="300px" Height="100px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblAgentAddress" runat="server" Text="Agent Address"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtAgentAddress" runat="server" CssClass="input" TextMode="MultiLine"
                            Width="300px" Height="100px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSeatimeOtherComments" runat="server" Text="Seatime Other Comments"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSeaTimeComments" runat="server" CssClass="input" TextMode="MultiLine"
                            Width="300px" Height="100px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDceAddress" runat="server" Text="Dce Address"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDceAddress" runat="server" CssClass="input" TextMode="MultiLine"
                            Width="300px" Height="100px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCargoDetails" runat="server" Text="Cargo Details"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCargoDetails" runat="server" CssClass="input" TextMode="MultiLine"
                            Width="300px" Height="100px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRequestedDCEType" runat="server" Text="Requested DCE Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Documents ID="ddlLicence" runat="server" DocumentList="<%#PhoenixRegistersDocumentLicence.ListDocumentLicence(null,null,2,null,null)%>"
                            AppendDataBoundItems="true" CssClass="input" DocumentType="LICENCE" />
                    </td>
                </tr>

            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
