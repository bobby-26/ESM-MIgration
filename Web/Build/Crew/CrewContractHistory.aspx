<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewContractHistory.aspx.cs"
    Inherits="CrewContractHistory" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UCRank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Contract History</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewContractHistory" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlCrewContractHistory">
            <ContentTemplate>
                <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="Title1" Text="Contract History" ShowMenu="false"></eluc:Title>
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuProcedureDetailList" runat="server" OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                    <div id="divCrewCompanyExperience" style="position: relative; z-index: +1">
                        <table id="tblCrew" runat="server" width="100%">
                            <tr>
                                <td style="width: 10%">First Name
                                </td>
                                <td style="width: 10%">
                                    <asp:TextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 10%">Middle Name
                                </td>
                                <td style="width: 10%">
                                    <asp:TextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 10%">Last Name
                                </td>
                                <td style="width: 10%">
                                    <asp:TextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 10%">File No.
                                </td>
                                <td style="width: 10%">
                                    <asp:TextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                </td>
                                <td style="width: 10%">Current Rank
                                </td>
                                <td style="width: 10%">
                                    <asp:TextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                        <table id="tblvessel" runat="server" width="100%">
                            <tr>
                                <td>Vessel 
                                </td>
                                <td>
                                    <eluc:Vessel ID="ddlVessel" runat="server" CssClass="input" VesselsOnly="true"
                                        AppendDataBoundItems="true" AssignedVessels="true" AutoPostBack="true" />
                                </td>
                                <td>Rank 
                                </td>
                                <td>
                                    <eluc:UCRank ID="ddlRank" runat="server" AppendDataBoundItems="true" CssClass="input" AutoPostBack="true" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="navSelect" style="position: relative; clear: both; width: 15px">
                        <eluc:TabStrip ID="Menuexport" runat="server" OnTabStripCommand="Menuexport_TabStripCommand"></eluc:TabStrip>
                    </div>
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvContractHistory" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowDataBound="gvContractHistory_RowDataBound"
                            ShowHeader="true" EnableViewState="false" AllowSorting="False" GridLines="None"
                            OnRowCommand="gvContractHistory_RowCommand">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                                <%-- <asp:TemplateField HeaderText="S.No.">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblSlNo" runat="server" Text="S.No."></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# ((DataRowView)Container.DataItem)["FLDROWNUMBER"]%>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                                <asp:TemplateField HeaderText="Name">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10px"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"]%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ref.No.">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblRefNo" runat="server" Text="Ref.No."></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRefNumber" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDREFNUMBER"] %>'></asp:Label>
                                        <asp:Label ID="lblContractId" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDCONTRACTID"] %>'
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="lblvesselid" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDVESSELID"] %>'
                                            Visible="false"></asp:Label>
                                        <asp:Label ID="lblrankid" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDRANKID"] %>'
                                            Visible="false"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Rank">
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemRank" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDRANKCODE"] %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Vessel">
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblItemVessel" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDVESSELNAME"] %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Pay Commencement">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPaydate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDPAYDATE", "{0:dd/MMM/yyyy}"))%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Literal ID="lblAction" runat="server" Text="Action"></asp:Literal>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="View Contract" ImageUrl="<%$ PhoenixTheme:images/te_pqtes.png %>"
                                            CommandName="CONTRACT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdGenContract"
                                            ToolTip="View Contract"></asp:ImageButton>
                                        <asp:ImageButton runat="server" AlternateText="Contract Letter" ImageUrl="<%$ PhoenixTheme:images/text-detail.png %>"
                                            CommandName="DOWNLOADPDF" CommandArgument='<%# Container.DataItemIndex %>' ID="cmddownloadpdf"
                                            ToolTip="Contract Letter"></asp:ImageButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divPage" runat="server" style="position: relative;">
                        <table width="100%" border="0" class="datagrid_pagestyle">
                            <tr>
                                <td nowrap="nowrap" align="center">
                                    <asp:Label ID="lblPagenumber" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblPages" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblRecords" runat="server">
                                    </asp:Label>&nbsp;&nbsp;
                                </td>
                                <td nowrap="nowrap" align="left" width="50px">
                                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                                </td>
                                <td width="20px">&nbsp;
                                </td>
                                <td nowrap="nowrap" align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap="nowrap" align="center">
                                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                    </asp:TextBox>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                        Width="40px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
