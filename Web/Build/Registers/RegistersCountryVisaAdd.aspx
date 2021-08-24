<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersCountryVisaAdd.aspx.cs"
    Inherits="Registers_RegistersCountryVisaAdd" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="../UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Country Visa Add</title>
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
        <%--<eluc:TabStrip ID="MenuTitle" runat="server" Title="Country Visa Add"></eluc:TabStrip>--%>
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="80%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuRegistersCountryAdd" runat="server" OnTabStripCommand="MenuRegistersCountryAdd_TabStripCommand"></eluc:TabStrip>
            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Country ID="ddlCountry" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVisaType" runat="server" Text="Visa Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlVisaTypeAdd" runat="server" CssClass="dropdown_mandatory" HardTypeCode="107"
                            AppendDataBoundItems="true" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblTimeTaken" runat="server" Text="Locations for Submission"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtTimeTakenAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="100" Width="100%"
                            ToolTip="Enter Time Taken">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDaysRequired" runat="server" Text="Days Required"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDaysRequiredAdd" runat="server" CssClass="gridinput_mandatory" Width="100%"
                            MaxLength="100" ToolTip="Enter Days Required For Visa Processing">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUrgentProcedure" runat="server" Text="Urgent Procedure"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtUrgentProcedureAdd" runat="server" CssClass="gridinput_mandatory"
                            Width="100%" ToolTip="Enter Urgent Procedure" MaxLength="500">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPysicalPresenceSpecification" runat="server" Text="Physical Presence Specification"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPhysicalPresenceSpecificationAdd" runat="server" CssClass="input"
                            MaxLength="500" ToolTip="Enter Physical Presence Specification" Width="100%">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOrdinaryAmountUSD" runat="server" Text="Ordinary Amount(USD)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtOrdinaryAmountAdd" runat="server" CssClass="input" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUrgent" runat="server" Text="Urgent Amount(USD)"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Number ID="txtUrgentAmonutAdd" runat="server" CssClass="input" Width="100%" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblRemarks" runat="server" Text="Remarks" Visible="false"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRemarksAdd" runat="server" Width="100%" CssClass="input" Visible="false"
                            ToolTip="Enter Remarks">
                        </telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassport" runat="server" Text="Valid with old PP no"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkPassportYNAdd" runat="server" Visible="true"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPhysicalPresence" runat="server" Text="Physical Presence Y/N"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkPhysicalPresenceYNAdd" runat="server" Visible="true" OnCheckedChanged="chkPhysicalPresenceYNAdd_CheckedChanged"
                            AutoPostBack="true"></asp:CheckBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOnArrival" runat="server" Text="On Arrival"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkOnArrivalAdd" runat="server" />
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
