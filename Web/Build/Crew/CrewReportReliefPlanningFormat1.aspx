<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportReliefPlanningFormat1.aspx.cs"
    Inherits="CrewReportReliefPlanningFormat1" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vesseltype" Src="~/UserControls/UserControlVesselTypeList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPoolList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZoneList.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Relief Planning Format 1 Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixCrew.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmReliefPlanningReport" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlCrewReliefPlanning">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Relief Planning Format 1 Report"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                        TabStrip="false"></eluc:TabStrip>
                </div>
                <div id="divFind" style="position: relative; z-index: 2">
                    <div>
                        <table width="100%">
                            <tr>
                                <td>
                                    <asp:Literal ID="lblZone" runat="server" Text="Zone"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Zone ID="ucZone" AppendDataBoundItems="true" runat="server" CssClass="input"
                                        Width="80%" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Vesseltype ID="ucVesselType" runat="server" AppendDataBoundItems="true" CssClass="input"
                                        Width="80%" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Rank ID="ucRank" AppendDataBoundItems="true" runat="server" CssClass="input"
                                        Width="80%" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Vessel ID="ucVessel" AppendDataBoundItems="true" runat="server" VesselsOnly="true"
                                        CssClass="input" Width="80%" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblPrincipal" runat="server" Text="Principal"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Principal ID="ucPrincipal" runat="server" AppendDataBoundItems="true" AddressType="128"
                                        CssClass="input" Width="80%" />
                                </td>
                                <td>
                                    <asp:Literal ID="lblPool" runat="server" Text="Pool"></asp:Literal>
                                </td>
                                <td>
                                    <eluc:Pool ID="ucPool" AppendDataBoundItems="true" runat="server" CssClass="input"
                                        Width="80%" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <asp:Panel ID="pnlReliefDue" runat="server" GroupingText="Relief Due Between">
                                        <table>
                                            <tr>
                                                <td>
                                                    <asp:Literal ID="lblFromDate" runat="server" Text="From Date"></asp:Literal>
                                                </td>
                                                <td>
                                                    <eluc:Date ID="ucFromDate" runat="server" CssClass="input_mandatory" />
                                                </td>
                                                <td>
                                                    <asp:Literal ID="lblToDate" runat="server" Text="To Date"></asp:Literal>
                                                </td>
                                                <td>
                                                    <eluc:Date ID="ucToDate" runat="server" CssClass="input_mandatory" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                                <td colspan="1">
                                    <asp:Panel ID="pnlFormat" runat="server" GroupingText="Formats">
                                        <asp:RadioButtonList ID="rblFormats" OnSelectedIndexChanged="rblFormats_SelectedIndexChanged"
                                            RepeatDirection="Horizontal" runat="server" AutoPostBack="true">
                                            <asp:ListItem>Format1</asp:ListItem>
                                            <asp:ListItem>Format2</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                        <table id="note" runat="server" style="color: Blue">
                            <tr>
                                <td>
                                    <asp:Literal ID="lblNote" Text="Note : please select the principal to see the experience with principal."
                                        runat="server"></asp:Literal>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="navSelect" id="divTab1" runat="server" style="position: relative; width: 15px">
                        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                        </eluc:TabStrip>
                    </div>
                    <div id="divGrid" runat="server" style="position: relative; overflow: auto; z-index: 0">
                        <asp:GridView ID="gvCrewReliefPlanning" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" OnRowDataBound="gvCrewReliefPlanning_RowDataBound" Width="100%"
                            CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                            OnSorting="gvCrewReliefPlanning_Sorting" OnPreRender="gvCrewReliefPlanning_PreRender">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <Columns>
                                <%--<asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="60px"></ItemStyle>
                                    <HeaderTemplate>
                                        Emp No
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEmpFileNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="40px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnlRankHeader" runat="server" CommandName="Sort" CommandArgument="FLDRANK"
                                            ForeColor="White">Rank&nbsp;</asp:LinkButton>
                                        <img id="FLDRANK" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblVesselId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' />
                                        <asp:Label ID="lblVesselName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                                        <asp:Label ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERRANK") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                            ForeColor="White">Onboard&nbsp;</asp:LinkButton>
                                        <img id="FLDNAME" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOffsignerID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERID") %>' />
                                        <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNAME") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblSignOnDateHeader" runat="server">SignOn Date</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSignOnDate" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblReliefDateHeader" runat="server">Relief Date</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRelifDue" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEFDUEDATE") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblRelieverHeader" runat="server">Reliever</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOnsignerID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERID") %>' />
                                        <asp:Label ID="lblRelieverName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNAME") %>' />
                                        <asp:LinkButton ID="lnkRelieverName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNAME") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblDOAHeader" runat="server">DOA</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDOA" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOA") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblEOLHeader" runat="server">EOL(Including activities)</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEOL" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEAVECOMPLETIONDATE") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="200px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblRemarksHeader" runat="server">Remarks</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
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
                    <div id="divGrid2" runat="server" style="position: relative; overflow: auto; z-index: 0">
                        <asp:GridView ID="gvCrewReliefPlanningF2" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" OnRowDataBound="gvCrewReliefPlanningF2_RowDataBound" Width="100%"
                            CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                            OnSorting="gvCrewReliefPlanningF2_Sorting" OnPreRender="gvCrewReliefPlanningF2_PreRender">
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="40px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblVesselNameHeader" runat="server">Vessel Name</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top" Width="40px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnlRankHeader" runat="server" CommandName="Sort" CommandArgument="FLDRANK"
                                            ForeColor="White">Rank&nbsp;</asp:LinkButton>
                                        <img id="FLDRANK" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblVesselId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' />
                                        <asp:Label ID="lblVesselName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>' />
                                        <asp:Label ID="lblRank" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERRANK") %>' />
                                        <asp:Label ID="lblRankId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top" Width="200px">
                                    </ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                            ForeColor="White">Name&nbsp;</asp:LinkButton>
                                        <img id="FLDNAME" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOffsignerID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERID") %>' />
                                        <asp:LinkButton ID="lnkName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFSIGNERNAME") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top" Width="40px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTourHeader" runat="server">Tour</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDOFFSIGNERTOURCOUNT")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblReliefDate1Header" runat="server">Relief Date</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRelifDue" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEFDUEDATE") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="100px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblRelieverNameHeader" runat="server">Reliever Name</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOnsignerID" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERID") %>' />
                                        <asp:Label ID="lblRelieverName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNAME") %>' />
                                        <asp:LinkButton ID="lnkRelieverName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEVERNAME") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top" Width="100px">
                                    </ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblDOA1Header" runat="server">DOA</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDOA" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDOA") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" VerticalAlign="Top" Width="100px">
                                    </ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblCOL1Header" runat="server">COL</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblEOL" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEAVECOMPLETIONDATE") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTimeinRankHeader" runat="server">Time In Rank</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDRANKDECIMALEXPERIENCE")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTimeinBPHeader" runat="server">Time In BP</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDPRPLDECIMALEXPERIENCE")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblTour1Header" runat="server">Tour</asp:Label>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDRELIVERTOURCOUNT")%>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <div id="divPage2" runat="server" style="position: relative;">
                        <table width="100%" border="0" class="datagrid_pagestyle">
                            <tr>
                                <td nowrap="nowrap" align="center">
                                    <asp:Label ID="lblPagenumber2" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblPages2" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblRecords2" runat="server">
                                    </asp:Label>&nbsp;&nbsp;
                                </td>
                                <td nowrap="nowrap" align="left" width="50px">
                                    <asp:LinkButton ID="cmdPrevious2" runat="server" OnCommand="PagerButtonClick2" CommandName="prev">Prev << </asp:LinkButton>
                                </td>
                                <td width="20px">
                                    &nbsp;
                                </td>
                                <td nowrap="nowrap" align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext2" OnCommand="PagerButtonClick2" runat="server" CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap="nowrap" align="center">
                                    <asp:TextBox ID="txtnopage2" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                    </asp:TextBox>
                                    <asp:Button ID="btnGo2" runat="server" Text="Go" OnClick="cmdGo2_Click" CssClass="input"
                                        Width="40px"></asp:Button>
                                </td>
                            </tr>
                            <eluc:Status runat="server" ID="Status1" />
                        </table>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
