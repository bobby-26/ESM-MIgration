<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardVesselAccountingSummary.aspx.cs" Inherits="Dashboard_DashboardVesselAccountingSummary" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel Accounting Summary</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

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
                    <eluc:TabStrip ID="MenuSummary" runat="server" OnTabStripCommand="MenuSummary_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table cellpadding="2" cellspacing="1" width="50%">
                    <tr>
                        <td>
                             <div id="divsummary" style="
                                width: 100%;overflow-x: auto; overflow-y: auto;">
                            <asp:GridView GridLines="None" ID="gvAccountingSummary" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" OnRowDataBound="gvAccountingSummary_ItemDataBound" EnableViewState="false">
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
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblStartDate" runat="server" Text="Start Date"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblStartDate"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTDATE","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"> </ItemStyle>
                                        <HeaderTemplate>
                                           <asp:Literal ID="lblCTM" runat="server" Text="CTM"></asp:Literal> <br /> <asp:Literal ID="lblClosingDate" runat="server" Text="Closing Date"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCpClosingDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPTAINCASHCLOSINGDATE","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" > </ItemStyle>
                                        <HeaderTemplate>
                                           <asp:Literal ID="lblProvision" runat="server" Text="Provision"></asp:Literal> <br /> <asp:Literal ID="lblClosingDate" runat="server" Text="Closing Date"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblProvisionCLosing" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROVISIONCLOSINGDATE","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"> </ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblPortageBill" runat="server" Text="Portage Bill"></asp:Literal> <br /> <asp:Literal ID="lblClosingDate" runat="server" Text="Closing Date"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPbClosing" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTAGEBILLCLOSINGDATE","{0:dd/MM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                </Columns>
                            </asp:GridView>
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
