<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsTentativeBOW.aspx.cs"
    Inherits="VesselAccountsTentativeBOW" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>BOW</title>
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
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="subHeader" style="position: relative; right: 0px">
                <eluc:Title runat="server" ID="Title1" Text="Tentative BOW (Balance of Wages)" ShowMenu="True"></eluc:Title>
            </div>
            <div class="navSelect" style="width: auto; float: right; margin-top: -26px">
                <eluc:TabStrip ID="MenuBOWMain" runat="server" OnTabStripCommand="MenuBOWMain_TabStripCommand"></eluc:TabStrip>
            </div>
        </div>
        <asp:UpdatePanel runat="server" ID="pnlBOW">
            <ContentTemplate>
                <table width="60%" cellpadding="1" cellspacing="1">
                    <tr>
                        <td width="10%">
                            <asp:Literal ID="lblStaffName" runat="server" Text="Staff Name"></asp:Literal>
                        </td>
                        <td width="40%">
                            <eluc:VesselCrew ID="ddlEmployee" runat="server" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                        <td width="10%">
                            <asp:Literal ID="lblDate" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td width="40%">
                            <eluc:Date ID="txtDate" runat="server" CssClass="input" />
                        </td>
                    </tr>
                </table>
                <br clear="all" />
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvBOW" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        GridLines="None" Width="50%" CellPadding="3" OnRowDataBound="gvBOW_RowDataBound"
                        ShowHeader="true" EnableViewState="false" ShowFooter="true">
                        <FooterStyle CssClass="datagrid_footerstyle" />
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="35%"></ItemStyle>
                                <FooterStyle HorizontalAlign="Right" Width="35%" Font-Bold="true"></FooterStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblWagesName" runat="server" Text="Wages Name"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblWageShort" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTCODE") %>'></asp:Label>
                                    <asp:Label ID="lblWageName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></asp:Label>
                                    <asp:Label ID="lblEarnDeduct" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEARNINGDEDUCTION") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Literal ID="lblTotalAmount" runat="server" Text="Total Amount :"></asp:Literal>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                                <FooterStyle HorizontalAlign="Right" Width="15%" Font-Bold="true"></FooterStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAmount" runat="server" Text="Amount"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblWage" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lblTotalAmountfooter" runat="server" Text=""></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
</html>
