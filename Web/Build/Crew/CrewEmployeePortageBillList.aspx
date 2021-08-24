<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewEmployeePortageBillList.aspx.cs"
    Inherits="CrewEmployeePortageBillList" EnableEventValidation="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Company Experience</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewCompanyExperience" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewCompanyExperienceEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <span style="float: left">
                            <eluc:Title runat="server" ID="ucTitle" Text="Portage Bill History" ShowMenu="false" />
                        </span>
                    </div>
                </div>
                <div id="divCrewCompanyExperience" style="position: relative; z-index: +1">
                    <table id="tblCrewCompanyExperience" width="100%">
                        <tr>
                            <td style="width: 10%">
                                First Name
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="width: 10%">
                                Middle Name
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="width: 10%">
                                Last Name
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 10%">
                                File No.
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                            <td style="width: 10%">
                                Current Rank
                            </td>
                            <td style="width: 10%">
                                <asp:TextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <hr />
                <div style="position: relative; width: 15px;">
                    <eluc:TabStrip ID="MenuCrewCompanyExperience" runat="server" OnTabStripCommand="CrewCompanyExperience_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: +1">
                    <asp:GridView ID="gvCrewCompanyExperience" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowDataBound="gvCrewCompanyExperience_RowDataBound"
                        GridLines="None" OnRowCommand="gvCrewCompanyExperience_RowCommand">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Vessel">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVesselHeader" runat="server">Vessel&nbsp;</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                    <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                    <asp:Label ID="lblemployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                                    <asp:Label ID="lblPortagebill" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTAGEBILLID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Rank">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCrewCompanyExperienceHeader" runat="server">Rank&nbsp;</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Closing On">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblClosingDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLOSINGDATE", "{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="From">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblFromDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMDATE", "{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="To">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lbToDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTODATE", "{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Days">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:Label ID="lblDays" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDAYS") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Portage Bill" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png %>"
                                        CommandName="PORTAGEBILL" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdportagebilldetails"
                                        ToolTip="Portage Bill"></asp:ImageButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
