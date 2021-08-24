<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardMaintanenceSummary.aspx.cs"
    Inherits="DashboardMaintanenceSummary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Maintanence Summary</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlMaintanence">
        <ContentTemplate>
            <div>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <%--<div class="subHeader" style="position: relative">
                    Maintenance Summary
                </div>--%>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuMaintanenceSummary" runat="server" OnTabStripCommand="MaintanenceSummary_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table width="50%">
                    <tr>
                        <td>
                            <asp:GridView GridLines="None" ID="gvMaintanenceSummary" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" OnRowDataBound="gvMaintanenceSummary_ItemDataBound" EnableViewState="false">
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"> </ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblCancelled" runat="server" Text="Cancelled"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCancelled" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCANCELLED") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"> </ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblPostponed" runat="server" Text="Postponed"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPostponed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSTPONED") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"> </ItemStyle>
                                        <HeaderTemplate>
                                           <asp:Literal ID="lblOverdue" runat="server" Text="Overdue"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblIssued" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERDUE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                   
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>                   
                    </table>      
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
