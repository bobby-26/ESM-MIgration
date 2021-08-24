<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewSignOffAllFlaggedVesselsReport.aspx.cs"
    Inherits="CrewSignOffAllFlaggedVesselsReport" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PoolList" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Sign Off</title>
  <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAppraisalQuestion" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAppraisal">
        <ContentTemplate>
            <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Sign Off" ShowMenu="false">
                        </eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
                </div>
                <table width="80%" cellpadding="2" cellspacing="0">
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Literal ID="lblPeriodFrom" runat="server" Text="Period From"></asp:Literal>
                        </td>
                        <td valign="top">
                            <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" />
                            &nbsp;&nbsp;&nbsp; To &nbsp;&nbsp;&nbsp;
                            <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td valign="top">
                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                        </td>
                        <td valign="top">
                            <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                        </td>
                        <td valign="top">
                            <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                        <td valign="top">
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </td>
                        <td valign="top">
                            <eluc:Vessel ID="ucVessel" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <div style="position: relative; width: 15px;">
                    <eluc:TabStrip ID="MenuCrewSignOff" runat="server" OnTabStripCommand="MenuCrewSignOff_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvSignOff" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" OnRowDataBound="gvSignOff_RowDataBound"
                        AllowSorting="true" OnSorting="gvSignOff_Sorting" OnRowCommand="gvSignOff_RowCommand"
                        EnableViewState="false">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVesselNameHeader" runat="server">Vessel Name</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="EmpName">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="12%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblNameHeader" runat="server">Name</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEmployeeId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDEMPLOYEEID")%>'></asp:Label>
                                    <asp:LinkButton ID="lnkName" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDEMPLOYEENAME")%>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblRank" runat="server">Rank</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDateofEmbarkationHeader" runat="server">Date of Embarkation</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDEMBARKATIONDATE")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDatepfDisEmbarkationHeader" runat="server">Date of DisEmbarkation</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDDISEMBARKATIONDATE")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblDatepfDisEmbarkationqHeader" runat="server">Date of DisEmbarkation</asp:Label>
                                    
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDDISEMBARKATIONDATE")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblFleetManagerHeader" runat="server">Fleet Manager</asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDFLEETMANAGER")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                                  <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="15%"></HeaderStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblSuperintendentHeader" runat="server">Superintendent</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDSUPTNAME")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
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
