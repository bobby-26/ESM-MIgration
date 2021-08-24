<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewInstituteBatchEdit.aspx.cs" Inherits="Crew_CrewInstituteBatchEdit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Institute Batch Edit</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="70%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="MenuTitle" runat="server"></eluc:TabStrip>
            <table cellpadding="1" cellspacing="2px" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInstitute" runat="server" Text="Institute"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtInstituteId" runat="server" Width="0"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtInstituteName" runat="server" Width="350px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtCourse" runat="server" Width="350px" Enabled="False"
                            CssClass="readonlytextbox" ReadOnly="true">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCourseId" runat="server" Width="0" CssClass="hidden"></telerik:RadTextBox>
                        <asp:LinkButton runat="server" AlternateText="Batch Duration" ID="lnkbtnBatchDuration"
                            OnClick="lnkbtnBatchDuration_Click" ToolTip="Batch Duration" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-calendar-alt"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVenue" runat="server" Text="Venue"></telerik:RadLabel>
                    </td>
                    <td colspan="2">
                        <telerik:RadTextBox runat="server" ID="txtVenue" CssClass="readonlytextbox" ReadOnly="true" Width="350px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAddress" runat="server" Text="Address"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVenueAddress" runat="server" CssClass="readonlytextbox" TextMode="MultiLine"
                            Height="60px" Width="320px" ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPrimaryContact" runat="server" Text="Primary Contact"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVenuePrimaryContact" runat="server" CssClass="readonlytextbox"
                            ReadOnly="true" Width="220px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCity" runat="server" Text="City"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVenueCity" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="220px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPhoneNo" runat="server" Text="Phone No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVenuePhoneno" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="220px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblState" runat="server" Text="State"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVenueState" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="220px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEMail" runat="server" Text="E-Mail"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVenueEmail" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="220px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVenueCountry" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="220px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPostalCode" runat="server" Text="Postal Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVenuePostalCode" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="220px">
                        </telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <asp:HiddenField runat="server" ID="hdnbatchcoursemappingId" />
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
