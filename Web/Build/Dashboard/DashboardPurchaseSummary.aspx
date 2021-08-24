<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardPurchaseSummary.aspx.cs"
    Inherits="DashboardPurchaseSummary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Purchase Summary</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
    <asp:UpdatePanel runat="server" ID="pnlPurchase">
        <ContentTemplate>
            <div>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <asp:Literal ID="lblPurchaseSummary" runat="server" Text="Purchase Summary"></asp:Literal>
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPurchaseSummary" runat="server" OnTabStripCommand="PurchaseSummary_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table cellpadding="2" cellspacing="1" width="100%">                   
                    <tr>
                        <td>
                            <asp:GridView GridLines="None" ID="gvPurchaseSummary" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" OnRowDataBound="gvPurchaseSummary_ItemDataBound" EnableViewState="false">
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblFleetCode" runat="server" Text="Fleet Code"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblFleetCode"  runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLEETCODE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"> </ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblRequisition" runat="server" Text="Requisition"></asp:Literal> 
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequisition" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITION") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"> </ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblQuoteNotReceived" runat="server" Text="Quote Not Received"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuoteNotReceived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTENOTRECEIVED") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"> </ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblQuoteReceived" runat="server" Text="Quote  Received"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuoteReceived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTERECEIVED") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"> </ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblQuoteWaitingApproval" runat="server" Text="Quote  Waiting Approval"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuoteWaitingApproval" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEAWAITINGAPPROVAL") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"> </ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblPOWaitingApproval" runat="server" Text="PO  Waiting  Approval"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPOWaitingIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOWAITINGTOISSUE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"> </ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblPOIssued" runat="server" Text="PO Issued"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPOIssued" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOISSUED") %>'></asp:Label>
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
