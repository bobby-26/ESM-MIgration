<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardInvoiceSummary.aspx.cs"
    Inherits="DashboardInvoiceSummary" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title> Invoice Summary</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInvoice">
        <ContentTemplate>
            <div>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <asp:Literal ID="lblInvoiceSummary" runat="server" Text="Invoice Summary "></asp:Literal>                  
                </div>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuInvoiceSummary" runat="server" OnTabStripCommand="InvoiceSummary_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table cellpadding="2" cellspacing="1" width="100%">                   
                    <tr>
                        <td>
                            <asp:GridView GridLines="None" ID="gvInvoiceSummary" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" OnRowDataBound="gvInvoiceSummary_ItemDataBound" EnableViewState="false">
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
                                            <asp:Literal ID="lblAwaitingPO" runat="server" Text="Awaiting PO"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAwaitingPO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAWAITINGPO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"> </ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblReconcilePO" runat="server" Text="Reconcile PO"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblQuoteReceived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECONCILEPO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"> </ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblAcctCheck" runat="server" Text="Acct Check"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAcctCheck" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCTCHECK") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"> </ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblInvoicePosted" runat="server" Text="Invoice Posted"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoicePosted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVPOSTED") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"> </ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblOperationMGRAPP" runat="server" Text="Operation MGRAPP"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="OperationMGRAPP" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONMGRAPP") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"> </ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblAwaitingAPP" runat="server" Text="Awaiting APP"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAwaitingAPP" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAWAITINGAPP") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                     <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"> </ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblScannedINV" runat="server" Text="Scanned INV"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblAScannedINV" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSCANNEDINV") %>'></asp:Label>
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
