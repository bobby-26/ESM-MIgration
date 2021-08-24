<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalWorkRestHours.aspx.cs" Inherits="DashboardTechnicalWorkRestHours" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="DashboardMenu" Src="~/UserControls/UserControlDashboardMenu.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>

<%@ Register Assembly="System.Web.DataVisualization, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div2" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixDashboard.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/bootstrap/css/bootstrap.min.css" /> 
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>
        <script  type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/bootstrap/js/bootstrap.min.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/jquery-ui.min.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/jquery-ui.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/DashboardNew.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/echarts.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/chart/bar.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/main.js"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="db_container">
            <div id="dashWrap">
                <div class="icoBlkWrap">
                    <div id="icoTabDiv">
                        <div id="sideExpander" onclick="javascript:ResizeMenu(this);calcW('dataMenu');" style="width: 100px;"><span>◄</span>►</div>
                        <div id="techtab" class="icoTabItem icoTabActive" data-tab="techIcons">Technical</div>
                        <div id="crwid" class="icoTabItem" data-tab="crewIcons" style="width: 60px;" runat="server">Crew</div>
                    </div>
                    <div id="techIcons" class="icoBlkDiv" runat="server">
                    </div>
                </div>
                <div id="popupDiv" class="dialDiv1">
                    <div id="gMain"></div>
                </div>

                <asp:UpdatePanel runat="server" ID="Updp1">
                    <ContentTemplate>
                        <eluc:Error ID="Error1" runat="server" Text="" Visible="false"></eluc:Error>
                        <div id="dataVizWrap">
                            <div id="ucmenu">
                                <eluc:DashboardMenu runat="server" ID="DashboardMenu1" ShowMenu="false" ShowFilter="1" />
                                <div id="dataMenu">
                                    <asp:GridView ID="gvMeasure" GridLines="None" runat="server" AutoGenerateColumns="false"
                                        Width="100%" OnRowDataBound="gvMeasure_RowDataBound" OnRowCommand="gvMeasure_OnRowCommand"
                                        ShowHeader="true" EnableViewState="false" CssClass="table table-striped table-bordered">
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <HeaderStyle CssClass="tableHeadRow" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="center" Width="20px"></ItemStyle>
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ImageUrl="<%$ PhoenixTheme:images/graphIcon.svg %>" AlternateText="Color" ID="cmdGraph"
                                                        CommandName="GRAPH" CommandArgument="<%# Container.DataItemIndex %>" CssClass="customIcon" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="center" Width="20px"></ItemStyle>
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" ImageUrl="<%$ PhoenixTheme:images/settings.svg %>" AlternateText="Color" ID="cmdColor"
                                                        CommandName="COLOR" CommandArgument="<%# Container.DataItemIndex %>" CssClass="customIcon" />
                                                </ItemTemplate>
                                            </asp:TemplateField>

                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="left" Width="40px"></ItemStyle>
                                                <HeaderStyle Wrap="false" />
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblHeaderMeasure" runat="server" Text="Measure Name"></asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMeasureId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMEASUREID")%>'></asp:Label>
                                                    <asp:Label ID="lblMeasureCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.MEASURECODE") %>'></asp:Label>
                                                    <asp:Label ID="lblshowdetail" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSHOWDETAIL")%>'></asp:Label>
                                                    <asp:Label ID="lblMeasureName" runat="server" CssClass="tableMeasureName" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMEASURENAME")%>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <asp:UpdatePanel runat="server" ID="pnl">
                <ContentTemplate>

                    <div class='otisTitleBar' onclick='resizeDashboard()'>
                        <div id='otisDivLbl' class='titleLabel'>
                            <asp:Literal ID="lblResult" runat="server" Text="Result"></asp:Literal>
                            <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                            <b>
                                <asp:Label ID="lblLastScheduleDate" runat="server" Text="Last update on:"></asp:Label></b>
                            <b>
                                <asp:Label ID="lblModifiedDate" runat="server"></asp:Label></b>
                        </div>
                        <div id='chartDivBtn'>
                            <span class='btnExpand'>
                                <svg width='16' height='16' viewbox='0 0 1792 1792' xmlns='" + svgLnk + "'><path d='M1650 288q0 13-10 23l-332 332 144 144q19 19 19 45t-19 45-45 19h-448q-26 0-45-19t-19-45v-448q0-26 19-45t45-19 45 19l144 144 332-332q10-10 23-10t23 10l114 114q10 10 10 23z'/><path d='M896 960v448q0 26-19 45t-45 19-45-19l-144-144-332 332q-10 10-23 10t-23-10l-114-114q-10-10-10-23t10-23l332-332-144-144q-19-19-19-45t19-45 45-19h448q26 0 45 19t19 45z'/></svg>
                            </span>
                        </div>

                    </div>

                    <div id="lwr3rDiv">
                        <div id="DataDiv" style="width: 100%">
                            <asp:GridView GridLines="None" ID="gvMeasureResult" runat="server" AutoGenerateColumns="true"
                                Width="100%" CellPadding="3" OnRowDataBound="gvMeasureResult_RowDataBound" OnRowCommand="gvMeasureResult_OnRowCommand"
                                ShowHeader="true" EnableViewState="false" CssClass="table table-striped table-bordered">
                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                <HeaderStyle CssClass="tableHeadRow" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />

                                <Columns>
                                    <asp:TemplateField Visible="false">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                        </HeaderTemplate>
                                        <ItemTemplate>
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
                                    <td width="20px">&nbsp;
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
            <script type="text/javascript">
                Sys.Application.add_load(function () {
                    FreezeGridColumnScroll("<%=gvMeasure.ClientID%>", 3);
                    calcW('dataMenu');
                });
            </script>
        </div>

        <eluc:Status runat="server" ID="ucStatus" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

    </form>
</body>
<script type="text/javascript">
    var svgLnk = "http://www.w3.org/2000/svg";
    $(document).ready(function () {
        calcH('lwr3rDiv');
    });

    $("#idOTIS").click(function () {
        $("#ucmenu").html("<div></div>")
        $(".otisTitleBar").html("<div id='otisDivLbl' class='titleLabel'>OTIS INFORMATION</div><div id='chartDivBtn'><span class='btnExpand'><svg width='16' height='16' viewBox='0 0 1792 1792' xmlns ='" + svgLnk + "'><path d='M1650 288q0 13-10 23l-332 332 144 144q19 19 19 45t-19 45-45 19h-448q-26 0-45-19t-19-45v-448q0-26 19-45t45-19 45 19l144 144 332-332q10-10 23-10t23 10l114 114q10 10 10 23z'/><path d='M896 960v448q0 26-19 45t-45 19-45-19l-144-144-332 332q-10 10-23 10t-23-10l-114-114q-10-10-10-23t10-23l332-332-144-144q-19-19-19-45t19-45 45-19h448q26 0 45 19t19 45z'/></svg></span></div>");
        $("#lwr3rDiv").html("<iframe id='otisIframe' width='100%' height='600px' src='https://system.stratumfive.com/otis/index.html' frameborder='0'></iframe>");
    });
    // Dashboard tab change click 
    $("#crwid").click(function () {
        location.href = "../Dashboard/DashboardCommon.aspx?ModuleType=" + "2";
    });
    //Resize window in zoom in/out
    $(window).resize(function () {
        calcW('dataMenu');
    });
</script>
</html>
