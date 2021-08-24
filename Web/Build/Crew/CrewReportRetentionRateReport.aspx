<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportRetentionRateReport.aspx.cs"
    Inherits="CrewReportRetentionRateReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Batch" Src="~/UserControls/UserControlPreSeaBatchList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddressTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlZoneList" Src="../UserControls/UserControlZoneList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Head Count Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

   </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrew">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Retention Rate Report"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <div>
                        <table width="100%">
                            <tr style="color: Blue">
                                <td colspan="4">
                                    Note :&nbsp;To view the Guidelines, put the mouse on the&nbsp;&nbsp;
                                    <img id="imgnotes" runat="server" src="<%$ PhoenixTheme:images/54.png %>" style="vertical-align: top;
                                        cursor: pointer" alt="NOTES" />
                                    &nbsp; button.
                                    <eluc:ToolTip ID="ucToolTipNW" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblFrom" runat="server" Text="From"></asp:Literal>
                                    <br />
                                    <eluc:Date ID="ucDateFrom" runat="server" CssClass="input_mandatory" />
                                    <br />
                                    <asp:Literal ID="lblTo" runat="server" Text="To"></asp:Literal>
                                    <br />
                                    <eluc:Date ID="ucDateTo" runat="server" CssClass="input_mandatory" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                                    <br />
                                    <eluc:Rank ID="ucRank" runat="server" AppendDataBoundItems="true" CssClass="input" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                                    <br />
                                    <eluc:VesselType ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="input" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblBatch" runat="server" Text="Batch"></asp:Literal>
                                    <br />
                                    <eluc:Batch ID="ucBatch" runat="server" AppendDataBoundItems="true" CssClass="input" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblPool" runat="server" Text="Pool"></asp:Literal>
                                    <br />
                                    <eluc:Pool ID="lstPool" runat="server" CssClass="input" AppendDataBoundItems="true" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblZone" runat="server" Text="Zone"></asp:Literal>
                                    </br>
                                    <eluc:UserControlZoneList ID="ucZone" runat="server" CssClass="input" AppendDataBoundItems="true" />
                                </td>
                                <td>
                                    <asp:Panel ID="pnlUser" runat="server" GroupingText="Please select any one">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:RadioButtonList ID="rblPrincipalManager" runat="server" RepeatDirection="Horizontal"
                                                        AutoPostBack="true" OnSelectedIndexChanged="PrincipalManagerClick">
                                                        <asp:ListItem Value="1" Selected="true">Principal</asp:ListItem>
                                                        <asp:ListItem Value="2">Manager</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>
                                                    <br />
                                                    <eluc:Principal ID="ucPrincipal" runat="server" AddressType="128" AppendDataBoundItems="true"
                                                        CssClass="input" />
                                                    <asp:Literal ID="lblManager" runat="server" Text="Manager"></asp:Literal>
                                                    <br />
                                                    <eluc:Address ID="ucManager" runat="server" AddressType="126" AppendDataBoundItems="true"
                                                        CssClass="input" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                <div class="navSelect" id="divTab1" runat="server" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" runat="server" style="position: relative; overflow: auto; z-index: 0">
                    <asp:GridView ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowDataBound="gvCrew_RowDataBound" Width="100%" CellPadding="3" ShowHeader="true"
                        EnableViewState="false" AllowSorting="true">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblRankHeader" runat="server">Rank</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblDismissedHeader" runat="server">Dismissed</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkDismissed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISMISSED") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblResignedHeader" runat="server">Resigned</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkResigned" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRESIGNED") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblTransferToOfficeHeader" runat="server">Transfer To Office</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkTTO" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTTO") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblTotalLeaversHeader" runat="server">Total Leavers</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkTotalLeavers" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALLEAVERS") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblCorrectedLeaversHeader" runat="server">Corrected Leavers</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkCorrectedLeavers" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCORRECTEDLEAVERS") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblHeadCountHeader" runat="server">Head Count</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkHeadCount" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHEADCOUNT") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblAllAttritionHeader" runat="server">All Attrition</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDALLATTRITION")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblCorrectedAttritionHeader" runat="server">Corrected Attrition</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCORRECTEDATTRITION")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblAllRetentionHeader" runat="server">All Retention</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDALLRETENTION")%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblCorrectedRetentionHeader" runat="server">Corrected Retention</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%# DataBinder.Eval(Container, "DataItem.FLDCORRECTEDRETENTION")%>
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
                            <td width="20px">
                                &nbsp;
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
                        <eluc:Status runat="server" ID="ucStatus" />
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
