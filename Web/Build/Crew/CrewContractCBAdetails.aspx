<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewContractCBAdetails.aspx.cs"
    Inherits="CrewContractCBAdetails" ValidateRequest="false" %>

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
                            <eluc:Title runat="server" ID="ucTitle" Text="CBA" ShowMenu="false" />
                        </div>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuCrewContract" runat="server" OnTabStripCommand="CrewContract_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                    <table cellpadding="1" cellspacing="1" width="100%">
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
                                <asp:TextBox ID="txtMiddlename" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                                    Width="97%"></asp:TextBox>
                            </td>
                            <td style="width: 10%;">
                                <asp:Literal ID="lblLastName" runat="server" Text="Last Name"></asp:Literal>
                            </td>
                            <td style="width: 15%;">
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                                    Width="97%"></asp:TextBox>
                            </td>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblRankNationality" runat="server" Text="Rank"></asp:Literal>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRank" runat="server" CssClass="input readonlytextbox" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Label ID="lblUnion" runat="server" Text="Union"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtunion" runat="server" CssClass="input readonlytextbox" ReadOnly="true"
                                        Width="96%"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Literal ID="lblSelecttoViewRevisions" runat="server" Text="Pay Commencement"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Date ID="txtDate" runat="server" CssClass="input readonlytextbox" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCBArevision" runat="server" Text="CBA Revision"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtCBARevision" runat="server" CssClass="input readonlytextbox"
                                        ReadOnly="true" Width="96%"></asp:TextBox>
                                </td>
                            </tr>
                    </table>
                    <table cellpadding="1" cellspacing="1" width="100%">
                        <tr>
                            <td valign="top" colspan="2">
                                <div class="navSelect" style="position: relative; clear: both; width: 15px">
                                    <eluc:TabStrip ID="Menuexport" runat="server" OnTabStripCommand="Menuexport_TabStripCommand"></eluc:TabStrip>
                                </div>

                                <asp:GridView ID="gvCBA" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                                    GridLines="None" Width="100%" CellPadding="3" OnRowCreated="gvCBA_RowCreated" ShowHeader="true"
                                    EnableViewState="false">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="Component">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblComponentName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOMPONENTNAME")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Earnings">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDINCLUDECONTEARDESC")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblDeductions" runat="server" Text="Deductions"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDINCLUDECONTDEDDESC")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblCalculationUnit" runat="server" Text="Unit"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDCALUNITBASISNAME")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblCalculationTime" runat="server" Text="Time"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDCALTIMEBASISNAME")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <HeaderTemplate>
                                                <asp:Literal ID="lblOnboardPayableDeduction" runat="server" Text="Onboard"></asp:Literal>
                                                <br />
                                                <asp:Literal ID="lblPayableDeduction" runat="server" Text="Payable/Deduction"></asp:Literal>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDONBPAYDEDNAME")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Currency">
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE")%>
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
