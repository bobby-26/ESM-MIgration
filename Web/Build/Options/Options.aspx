<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Options.aspx.cs" Inherits="Options" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserGroup" Src="~/UserControls/UserControlUserGroup.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">

            <eluc:Error runat="server" ID="ucError" Visible="false" />


            <eluc:TabStrip runat="server" ID="PhoenixOptionsForm" OnTabStripCommand="PhoenixOptionsForm_TabStripCommand"></eluc:TabStrip>
            <eluc:Status ID="ucStatus" runat="server" />
            <table width="100%">
                <tr>
                    <td width="25%">
                        <b>
                            <telerik:RadLabel ID="lblChooseTheme" runat="server" Text="Choose Theme"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="true" OnSkinChanged="RadSkinManager1_SkinChanged" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblShow" runat="server" Text="Show"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlShow" runat="server" Width="150px"
                            DefaultMessage="Select a records per page">
                            <Items>
                                <telerik:DropDownListItem Value="2" Text="2" />
                                <telerik:DropDownListItem Value="10" Text="10" />
                                <telerik:DropDownListItem Value="25" Text="25" />
                                <telerik:DropDownListItem Value="50" Text="50" />
                                <telerik:DropDownListItem Value="100" Text="100" />
                            </Items>
                        </telerik:RadDropDownList>
                        &nbsp;<telerik:RadLabel ID="lblRecords" runat="server" Text="Records"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblExport" runat="server" Text="Export"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <eluc:Number ID="txtExcelRecords" runat="server" IsInteger="true" IsPositive="true" />
                        &nbsp;<telerik:RadLabel ID="lblExcelRecords" runat="server" Text="Records"></telerik:RadLabel>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>
                            <telerik:RadLabel ID="lblDefaultVessel" runat="server" Text="Default Vessel"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" AssignedVessels="true" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>
                            <telerik:RadLabel ID="lblDefaultCompany" runat="server" Text="Default Company"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <eluc:Company runat="server" ID="ucCompany" Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblDefaultNationality" runat="server" Text="Default Nationality"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <eluc:Country ID="ddlCountry" runat="server" Width="150px" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="Literal1" runat="server" Text="Date Format"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlDateFormat" runat="server" Width="150px"
                            DefaultMessage="Select a Date Format">
                            <Items>
                                <telerik:DropDownListItem Value="" Text="--Select--" />
                                <telerik:DropDownListItem Value="en-GB:dd/MM/yyyy" Text="dd/MM/yyyy" />
                                <telerik:DropDownListItem Value="en-us:MM/dd/yyyy" Text="MM/dd/yyyy" />
                                <telerik:DropDownListItem Value="en-us:MMM/dd/yyyy" Text="MMM/dd/yyyy" />
                                <telerik:DropDownListItem Value="en-GB:dd/MMM/yyyy" Text="dd/MMM/yyyy" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblDatePicker" runat="server" Text="Date Picker"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkDatePicker" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblDashboard" runat="server" Text="Dashboard"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlDashboard" runat="server" Width="150px"
                            DefaultMessage="Select a Date Format">
                            <Items>
                                <telerik:DropDownListItem Value="" Text="--Select--" />
                                <telerik:DropDownListItem Value="HSEQA" Text="HSEQA" />
                                <telerik:DropDownListItem Value="TECHNICAL" Text="Technical" />
                                <telerik:DropDownListItem Value="ACCOUNTS" Text="Accounts" />
                                <telerik:DropDownListItem Value="CREW" Text="Crew" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>
                            <telerik:RadLabel ID="lblApplications" runat="server" Text="Applications"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <asp:CheckBoxList ID="cblApplication" runat="server" Width="240px" RepeatColumns="2" RepeatDirection="Horizontal" DataTextField="FLDAPPLICATIONNAME" DataValueField="FLDMENUVALUE">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                <%--<tr>
                            <td valign="top">
                                <b>
                                    <Telerik:RadLabel ID="lblAlerts" runat="server" Text="Alerts"></Telerik:RadLabel></b>
                            </td>
                            <td>
                                <asp:CheckBoxList ID="cblAlerts" runat="server" RepeatColumns="2" RepeatDirection="Horizontal" DataTextField="FLDNAME" DataValueField="FLDID" CellPadding="3" CellSpacing="1">
                                </asp:CheckBoxList>
                            </td>
                        </tr>--%>
                <tr>
                    <td valign="top">
                        <b>
                            <telerik:RadLabel ID="lblShowCreditNoteDiscount" runat="server" Text="Show Credit Note Discount"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkShowCreditNote" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>
                            <telerik:RadLabel ID="lblShowSuptFeedback" runat="server" Text="Show Supt Feedback"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkShowSupdtFeedback" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>
                            <telerik:RadLabel ID="lblShowComponentNumber" runat="server" Text="Show Component Number"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <asp:CheckBox ID="ChkShowComponentNumber" runat="server" />
                    </td>
                </tr>
              <%--  <tr>
                    <td valign="top">
                        <b>
                            <telerik:RadLabel ID="lblDashboardModuleList" runat="server" Text="Dashboard Module Configuration"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnModuleConfig" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" runat="server" CommandName="DASHBOARDMODULE"></asp:ImageButton>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>
                            <telerik:RadLabel ID="lblDashboardMeasure" runat="server" Text="Dashboard Measure Configuration"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <asp:ImageButton ID="btnMeasureconfig" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" runat="server" CommandName="DASHBOARDMEASURE"></asp:ImageButton>
                    </td>
                </tr>--%>

                <tr>
                    <td valign="top">
                        <b>
                            <telerik:RadLabel ID="lblDashboardUserGroup" runat="server" Text="Dashboard UserGroup"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <eluc:UserGroup runat="server" ID="ucUserGroup" CssClass="input" Width="220px" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>
                            <telerik:RadLabel ID="lblOwnerReportUserGroup" runat="server" Text="Owner Report UserGroup"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <eluc:UserGroup runat="server" ID="UCOwnerReportUserGroup" CssClass="input" Width="220px" AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <b>
                            <telerik:RadLabel ID="lblownerreporthomepage" runat="server" Text="Owner Report Home Page"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlownerreporthomepage" runat="server" AppendDataBoundItems="true" CssClass="dropdown" Width="270px" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>
                            <telerik:RadLabel ID="lblpreference" runat="server" Text="Dashboard Preference"></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblpreference" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="New Dashboard" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Vessel overview only" Value="1"></asp:ListItem>
                            <asp:ListItem Text="Blank" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
                <tr>
                    <td valign="top">
                        <b>
                            <telerik:RadLabel ID="lblSOAflag" runat="server" Text="SOA Combined pdf Config."></telerik:RadLabel>
                        </b>
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblSOAflag" runat="server" AutoPostBack="true">
                            <asp:ListItem Text="iTextSharp" Value="0" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="PdfMerger" Value="1"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
