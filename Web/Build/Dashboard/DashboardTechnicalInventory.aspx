<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalInventory.aspx.cs"
    Inherits="DashboardTechnicalInventory" %>

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
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnl">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="db_container">
                <eluc:DashboardMenu runat="server" ID="ucDashboardMenu" ShowMenu="true" ShowFilter="1"/>
                <div class="db_dashboard">
                    <div class="db_leftpanel">
                        <div align="center">
                            <asp:ImageButton runat="server" ImageUrl="<%$ PhoenixTheme:images/BarChart.png %>" AlternateText="Filter" ID="cmdChart" />
                            <b><asp:Label ID="lblMeasureTitle" runat="server"></asp:Label></b>
                        </div>
                        <asp:Chart ID="ChartMeasure" runat="server" Height="260px" Width="600px">
                        </asp:Chart>
                    </div>
                    <div class="db_rightpanel">
                        <asp:GridView GridLines="None" ID="gvMeasure" runat="server" AutoGenerateColumns="false" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowDataBound="gvMeasure_RowDataBound" OnRowCommand="gvMeasure_OnRowCommand"
                            ShowHeader="true" EnableViewState="false">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>
                            <asp:TemplateField>
                                <Itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <Headertemplate>
                                </Headertemplate>
                                <Itemtemplate>
                                    <asp:ImageButton runat="server" ImageUrl="<%$ PhoenixTheme:images/detail-flag.png %>" AlternateText="Color" ID="cmdColor" 
                                        CommandName="COLOR" CommandArgument="<%# Container.DataItemIndex %>"/>    
                                </Itemtemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <Itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <Headertemplate>
                                    <asp:Label ID="lblHeaderMeasure" runat="server" Text="Measure Name"></asp:Label>
                                </Headertemplate>
                                 <Itemtemplate>
                                    <asp:Label ID="lblMeasureId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMEASUREID")%>'></asp:Label>
                                    <asp:Label ID="lblMeasureName" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMEASURENAME")%>'></asp:Label>
                                </Itemtemplate>
                            </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
                <div class="db_results">
                    <div class="db_title">
                        <asp:Literal ID="lblResult" runat="server" Text="Result"></asp:Literal>
                        <asp:Label ID="lblName" runat="server" Text=""></asp:Label>
                         <b><asp:Label ID ="lblLastScheduleDate" runat = "server" Text="Last updated on:"></asp:Label></b>
                         <b><asp:Label ID="lblModifiedDate" runat= "server"></asp:Label></b>
                    </div>
                    <div id="DataDiv" class="db_resultdatadiv">
                        <asp:GridView GridLines="None" ID="gvMeasureResult" runat="server" AutoGenerateColumns="true" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowDataBound="gvMeasureResult_RowDataBound" OnRowCommand="gvMeasureResult_OnRowCommand"
                            ShowHeader="true" EnableViewState="false">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <Columns>
                            <asp:TemplateField>
                                <Itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <Headertemplate>
                                </Headertemplate>
                                <Itemtemplate>
                                    <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" Screen="Dashboard/DashboardToolTipUnderConstruction.aspx" />                   
                                </Itemtemplate>
                            </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
