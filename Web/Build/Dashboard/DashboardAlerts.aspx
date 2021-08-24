<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardAlerts.aspx.cs"
    Inherits="Dashboard_DashboardAlerts" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript">
        function resize() {

            //alert(screen.availHeight);

            var height = ((document.all ? (document.documentElement && document.documentElement.clientHeight ? document.documentElement.clientHeight : document.body.clientHeight) : window.innerHeight)) - 20;
            var width = ((document.all ? (document.documentElement && document.documentElement.clientWidth ? document.documentElement.clientWidth : document.body.clientWidth) : window.innerWidth));

            var obj1 = document.getElementById("divAlerts");
            obj1.style.height = height - 20 + "px";
            //obj1.style.width = (width - 10) + "px";

            var obj2 = document.getElementById("divDashboard");
            obj2.style.height = height - 20 + "px";
            //obj2.style.width = width / 2 + "px";

            var obj3 = document.getElementById("fraDashboard");
            obj3.style.height = height - 50 + "px";

        }
    </script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDashboard">
        <ContentTemplate>
            <div id="navigation" style="top: 0px; vertical-align: top; width: 100%; postion: absolute;
                margin-bottom: 0px; bottom: 0px;">
                <eluc:Error ID="ucError" runat="server" Visible="false" />
                <div class="subHeader" style="position: relative">
                    <div id="div1">
                        <eluc:Title runat="server" ID="Title1" Text="" ShowMenu="true" />
                    </div>
                </div>
                <table width="100%" style="margin-top: 0px; vertical-align: top;">
                    <tr>
                        <td width="20%" valign="top">
                            <div id="divAlerts" style="width: 100%; overflow-x: hidden; overflow-y: auto; border-width: 1px;
                                border-style: groove; border-color: Navy;">
                                <div class="dashboard_section" style="position: relative">
                                    <div id="divHeading">
                                        <eluc:Title runat="server" ID="ucTitle" Text="Alerts" ShowMenu="false" />
                                    </div>
                                </div>
                                <asp:GridView ID="gvAlert" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                    Width="100%" CellPadding="3" OnRowCommand="gvAlert_RowCommand" OnRowDataBound="gvAlert_RowDataBound"
                                    OnRowEditing="gvAlert_RowEditing" ShowHeader="false" EnableViewState="false"
                                    BorderColor="Transparent">
                                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                    <RowStyle Height="10px" />
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                            <ItemTemplate>
                                                <asp:Label ID="lblTaskType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKTYPE") %>'></asp:Label>
                                                <asp:LinkButton ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                                    CommandName="ALERTTASK" CommandArgument='<%# Container.DataItemIndex %>'></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </div>
                        </td>
                        <td valign="top">
                            <div id="divDashboard" style="width: 100%;
                                border-width: 1px; border-style: groove; border-color: Navy;">
                                 <div class="subHeader" style="position: relative">
                                    <div id="div2">
                                        <eluc:Title runat="server" ID="ucDashboardTitle" Text="Alerts" ShowMenu="false" />
                                    </div>
                                </div>
                                <iframe id="fraDashboard" runat="server" style="overflow-x:auto ; overflow-y: auto; width: 100%;">
                                </iframe>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
