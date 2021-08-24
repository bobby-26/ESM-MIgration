<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceReportMaintenanceDone.aspx.cs"
    Inherits="PlannedMaintenanceReportMaintenanceDone" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmMaintenaceDoneFilter" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <telerik:RadCodeBlock ID="Radcodeblock2" runat="server">
            <eluc:TabStrip ID="MenuReportMaintenanceDone" runat="server" OnTabStripCommand="MenuReportMaintenanceDone_TabStripCommand"></eluc:TabStrip>
        </telerik:RadCodeBlock>
        <telerik:RadAjaxPanel runat="server" ID="RadAjaxPanel1">
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblComponentNumber" runat="server" Text="ComponentNumber"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentNumber" runat="server" ReadOnly="false" CssClass="input"
                            MaxLength="20" Width="120px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblComponentName" runat="server" Text="Component Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentName" runat="server" ReadOnly="false" CssClass="input"
                            MaxLength="20" Width="160px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtComponentType" runat="server" CssClass="input" MaxLength="50"
                            Width="264px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDoneDateBetween" runat="server" Text="Done Date Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date runat="server" ID="txtDateFrom" CssClass="input" />
                        -
                        <eluc:Date runat="server" ID="txtDateTo" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td valign="top" class="style1">
                        <telerik:RadLabel ID="lblJobClasses" runat="server" Text="Job Classes"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucJobClass" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td valign="top" colspan="2">&nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <telerik:RadLabel ID="lblMaker" runat="server" Text="Maker"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtMakerCode" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="60px"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtMakerName" runat="server" ReadOnly="false" CssClass="input readonlytextbox"
                                MaxLength="20" Width="180px"></telerik:RadTextBox>
                            <img runat="server" id="imgShowMaker" style="cursor: pointer; vertical-align: top" src="<%$ PhoenixTheme:images/picklist.png %>"  />
                            <telerik:RadTextBox ID="txtMakerId" runat="server" CssClass="input" Width="1px"></telerik:RadTextBox>
                        </span>&nbsp;
                        <asp:ImageButton ID="cmdClear" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdMakerClear_Click" />
                    </td>
                    <td valign="top">
                        <telerik:RadLabel ID="lblVendor" runat="server" Text="Maker"></telerik:RadLabel>
                    </td>
                    <td valign="top">
                        <span id="spnPickListVendor">
                            <telerik:RadTextBox ID="txtVendorNumber" runat="server" MaxLength="20" Width="60px" CssClass="input readonlytextbox"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" MaxLength="20" Width="180px" CssClass="input readonlytextbox"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="cmdShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="1px" CssClass="input"></telerik:RadTextBox>
                        </span>&nbsp;
                        <asp:ImageButton ID="imgVendor" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                            ImageAlign="AbsMiddle" Text=".." OnClick="cmdVendorClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblCalender" runat="server" Text="Calender:"></telerik:RadLabel></b>
                        &nbsp;
                        <telerik:RadLabel ID="lblFrequency" runat="server" Text="Frequency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Decimal runat="server" ID="txtFrequency" Mask="999" CssClass="input" Width="45px" />
                        <eluc:Hard ID="ucFrequency" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblRunHour" runat="server" Text="Run Hour:"></telerik:RadLabel></b>
                        &nbsp;
                        <telerik:RadLabel ID="lblCounterFrequency" runat="server" Text="Frequency"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucCounterType" runat="server" CssClass="input" AppendDataBoundItems="true" />
                        <telerik:RadTextBox runat="server" ID="ucCounterFrequency" CssClass="input" Width="60px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblMaintenanceType" runat="server" Text="Maintenance Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMainType" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUnplannedWork" runat="server" Text="Unplanned Work"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkUnplanned" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblClassCode" runat="server" Text="Class Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtClassCode" runat="server" CssClass="input"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblResponsibility" runat="server" Text="Responsibility"></telerik:RadLabel>
                    </td>
                    <td>
                        <div runat="server" id="Div3" class="input" style="overflow: auto; width: 70%; height: 100px">
                        <%--    <asp:CheckBoxList ID="cblResponsibility" runat="server" DataTextField="FLDDISCIPLINENAME"
                                DataValueField="FLDDISCIPLINEID" RepeatDirection="Vertical" Height="100%">
                            </asp:CheckBoxList>--%>
                            <telerik:RadCheckBoxList ID="cblResponsibility" runat="server" DataBindings-DataTextField="FLDDISCIPLINENAME"
                                DataBindings-DataValueField="FLDDISCIPLINEID" RepeatDirection="Vertical" Height="100%">
                                </telerik:RadCheckBoxList>
                        </div>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
