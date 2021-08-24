<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewPlanReliverMatrix.aspx.cs"
    Inherits="CrewPlanReliverMatrix" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Plan Reliever</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmPlanReliver" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader" style="position: relative">
            <div id="divHeading" style="vertical-align: top">
                <eluc:Title runat="server" ID="ucTitle" Text="Plan Reliever" ShowMenu="false" />
            </div>
        </div>
        <div id="divReliever" style="position: relative; z-index: 0; width: 100%;">
            <b>
                <asp:Literal ID="lblCombinedExp" runat="server" Text="Combined Exp"></asp:Literal>
            </b>
            <asp:GridView ID="gvRelieverMatrix" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnRowDataBound="gvRelieverMatrix_ItemDataBound"
                ShowFooter="true" ShowHeader="true" EnableViewState="false">
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                <RowStyle Height="10px" />
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Left" Width="40%"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblName" runat="server" Text="Name of Seafarer"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="5%"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblRankExp" runat="server" Text="Rank exp"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container,"DataItem.FLDRANKEXP") %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblVesselTypeExp" runat="server" Text="Vsl Type exp"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDVESSELTYPEEXP")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle HorizontalAlign="Right"></ItemStyle>
                        <HeaderTemplate>
                            <asp:Literal ID="lblReliefDate" runat="server" Text="Relief Date"></asp:Literal>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%# DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE")%>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
    </form>
</body>
</html>
