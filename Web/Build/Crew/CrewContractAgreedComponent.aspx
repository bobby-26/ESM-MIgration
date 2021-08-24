<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewContractAgreedComponent.aspx.cs"
    Inherits="CrewContractAgreedComponent" ValidateRequest="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeniorityScale" Src="~/UserControls/UserControlSeniorityScale.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlContractCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CrewComponents" Src="~/UserControls/UserControlContractCrew.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Contract</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmInActive" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlContract">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <div id="divHeading" style="vertical-align: top">
                            <eluc:Title runat="server" ID="ucTitle" Text="Crew Agreed" ShowMenu="false" />
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuCrewContract" runat="server" OnTabStripCommand="CrewContract_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <tr>
                                <td style="width: 10%;">
                                    <asp:Literal ID="lblFirstName" runat="server" Text="First Name"></asp:Literal>
                                </td>
                                <td style="width: 23%;">
                                    <asp:TextBox ID="txtFirstName" runat="server" CssClass="gridinput readonlytextbox"
                                        ReadOnly="true" Width="97%"></asp:TextBox>
                                </td>
                                <td style="width: 9%;">
                                    <asp:Literal ID="lblMiddleName" runat="server" Text="Middle Name"></asp:Literal>
                                </td>
                                <td style="width: 23%;">
                                    <asp:TextBox ID="txtMiddlename" runat="server" CssClass="input readonlytextbox" Width="97%"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 10%;">
                                    <asp:Literal ID="lblLastName" runat="server" Text="Last Name"></asp:Literal>
                                </td>
                                <td style="width: 15%;">
                                    <asp:TextBox ID="txtLastName" runat="server" CssClass="input readonlytextbox" Width="97%"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblRankNationality" runat="server" Text="Rank"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRank" runat="server" CssClass="input readonlytextbox" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="lblSeniorityWageScale" runat="server" Text="Wage Scale"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtSeniority" runat="server" CssClass="input readonlytextbox" Width="97%"
                                        ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblRankExpCaption" runat="server" Text="Months in Rank "></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRankMonth" runat="server" CssClass="input readonlytextbox" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblContractPayCommencementDate" runat="server" Text="Pay Commencement"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Date ID="txtDate" runat="server" CssClass="input readonlytextbox" ReadOnly="true" />
                                </td>
                            </tr>
                    </table>
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td valign="top" colspan="2">
                                <div class="navSelect" style="position: relative; clear: both; width: 15px">
                                    <eluc:TabStrip ID="Menuexport" runat="server" OnTabStripCommand="Menuexport_TabStripCommand"></eluc:TabStrip>
                                </div>
                                <asp:GridView ID="gvCrew" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                                    Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" GridLines="None">
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Component">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Calculation Basis">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDCALCULATIONBASISNAME")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Payable Basis">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDPAYABLEBASISNAME")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Included Onboard">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDINCLUDEDONBOARDYNNAME")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Earning /Deduction">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDEARNINGDEDUCTIONNAME")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                    <eluc:Status ID="ucStatus" runat="server" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
