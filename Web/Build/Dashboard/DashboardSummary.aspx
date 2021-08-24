<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardSummary.aspx.cs"
    Inherits="Dashboard_DashboardSummary" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel Summary</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersManningScale" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlManningScaleEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <div class="subHeader">
                    <eluc:Title runat="server" ID="Attachment" Text=""></eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 2px; position: absolute;">
                    <eluc:TabStrip ID="MenuDdashboradVesselParticulrs" runat="server" OnTabStripCommand="MenuDdashboradVesselParticulrs_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <%--<b>Accounts</b>--%>
                <%--<div id="divGrid" style="position:relative; z-index:+1;">               
                    <asp:GridView ID="gvAccounts" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblScannedInvoice" runat="server" Text="Scanned Invoice"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblScannedInvoice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAWAITINGAPP") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAwaitingPO" runat="server" Text="Awaiting PO"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAwaitingPO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAWAITINGPO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblReconciledPO" runat="server" Text="Reconciled PO"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReconcilePO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECONCILEPO") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAccountsChecking" runat="server" Text="Accounts Checking"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReconcilePO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCTCHECK") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblInvoicePosted" runat="server" Text="Invoice Posted"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblReconcilePO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVPOSTED") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblOperationManagerApproval" runat="server" Text="Operation Manager Approval"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOperationManagerApproval" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONMGRAPP") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblAwaitingApproval" runat="server" Text="Awaiting Approval"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAwaitingApproval" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAWAITINGAPP") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                        </Columns>
                    </asp:GridView>
                </div>--%>
                <b>Planned Maintenance</b>
                <div id="divPlannedMaintenance" style="position: relative; z-index: +1;">
                    <asp:GridView GridLines="None" ID="gvPlannedMaintenance" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCancelled" runat="server" Text="Cancelled"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCancelled" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCANCELLED") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="Postponed" runat="server" Text="Postponed"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPostponed" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSTPONED") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblOverDue" runat="server" Text="Over Due"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOverDue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOVERDUE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <%--<b>Purchase</b>--%>
                <%--<div id="divPurchase" style="position:relative; z-index:+1;">               
                    <asp:GridView ID="gvPurchase" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" HorizontalAlign="Center" />
                        
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                     <asp:Literal ID="lblRequestion" runat="server" Text="Requestion"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblRequestion" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITION") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblQuotedNotReceived" runat="server" Text="Quoted Not Received"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuotedNotReceived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTENOTRECEIVED") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblQuoteReceived" runat="server" Text="Quote Received"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuoteReceived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTERECEIVED") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblQuoteAwaitingApproval" runat="server" Text="Quote Awaiting Approval"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuoteAwaitingApproval" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEAWAITINGAPPROVAL") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="POAwaitingtobeIssued" runat="server" Text="PO Awaiting to be Issued"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPoAwaitingIssued" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOWAITINGTOISSUE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPOIssued" runat="server" Text="PO Issued"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPOIssued" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOISSUED") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                        </Columns>
                    </asp:GridView>
                </div>--%>
                <b><asp:Literal ID="lblVesselAccounting" runat="server" Text="Vessel Accounting"></asp:Literal></b>
                <div id="divsummary" style="width: 100%; overflow-x: auto; overflow-y: auto;">
                    <asp:GridView GridLines="None" ID="gvAccountingSummary" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" EnableViewState="false">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblStartDate" runat="server" Text="Start Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblStartDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTARTDATE","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblCTM" runat="server" Text="CTM"></asp:Literal>
                                    <br />
                                    <asp:Literal ID="lblClosingDate" runat="server" Text="Closing Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCpClosingDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPTAINCASHCLOSINGDATE","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblProvision" runat="server" Text="Provision"></asp:Literal>
                                    <br />
                                    <asp:Literal ID="lblClosingDate" runat="server" Text="Closing Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblProvisionCLosing" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROVISIONCLOSINGDATE","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Literal ID="lblPortageBill" runat="server" Text="Portage Bill"></asp:Literal>
                                    <br />
                                    <asp:Literal ID="lblClosingDate" runat="server" Text="Closing Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPbClosing" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTAGEBILLCLOSINGDATE","{0:dd/MM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
