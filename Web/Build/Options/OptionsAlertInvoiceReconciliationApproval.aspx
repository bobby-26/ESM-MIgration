<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsAlertInvoiceReconciliationApproval.aspx.cs"
    Inherits="OptionsAlertInvoiceReconciliationApproval" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Invoice Reconciliation Alert</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmInvoiceAlert" runat="server">
    <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <br clear="all" />
                <table width="80%">
                    <tr>
                        <td>
                            <asp:GridView ID="gvAlertsInvoiceReconciliation" runat="server" AutoGenerateColumns="False"
                                Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvAlertsInvoiceReconciliation_RowCommand"
                                OnRowDataBound="gvAlertsInvoiceReconciliation_ItemDataBound" EnableViewState="false"
                                AllowSorting="true" OnSorting="gvAlertsInvoiceReconciliation_Sorting">
                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                <RowStyle Height="10px" />
                                <Columns>
                                    <asp:TemplateField HeaderText=" PO Number">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblPONumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDPURCHASEORDERNUMBER"
                                                ForeColor="White">PO <br />Number&nbsp;</asp:LinkButton>
                                            <img id="FLDPURCHASEORDERNUMBER" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPONumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPONUMBER") %>'>                                   
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Number">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblInvoiceNumberHeader" runat="server" CommandName="Sort" CommandArgument="FLDINVOICESERIALNUMBER"
                                                ForeColor="White">Invoice<br />Number&nbsp;</asp:LinkButton>
                                            <img id="FLDINVOICESERIALNUMBER" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></asp:Label>
                                            <asp:LinkButton ID="lnkInvoice" runat="server"  CommandArgument='<%# Container.DataItemIndex%>'
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER")  %>'></asp:LinkButton>
                                            <asp:TextBox ID="txtInvoiceCode" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDINVOICECODE") %>'
                                                Visible="false"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Supplier Code">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblSupplierCodeHeader" runat="server" CommandName="Sort" CommandArgument="FLDCODE"
                                                ForeColor="White">Supplier<br />Name&nbsp;</asp:LinkButton>
                                            <img id="FLDCODE" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblBackToBack" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Supplier Reference">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblSupplierInvoice" runat="server" Text="SupplierInvoice"></asp:Literal><br />
                                            <asp:Literal ID="lblReference" runat="server" Text="Reference"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblLeave" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESUPPLIERREFERENCE") %>'></asp:Label>
                                            <asp:Label ID="lblAttachmentexists" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENTEXISTS") %>'
                                                Visible="false"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Received Date">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblReceivedDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDINVOICERECEIVEDDATE"
                                                ForeColor="White">Received<br /> Date&nbsp;</asp:LinkButton>
                                            <img id="FLDINVOICERECEIVEDDATE" runat="server" visible="false" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICERECEIVEDDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Vessel">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblVesselHeader" runat="server" CommandName="Sort" CommandArgument="FLDVESSELNAME"
                                                ForeColor="White"> Vessel<br />Name&nbsp;</asp:LinkButton>
                                            <img id="FLDVESSELNAME" runat="server" visible="false" />
                                            <%--                                    
                                    <asp:Label ID="lblVesselHeader" runat="server">
                                    Vessel<br />Name
                                    </asp:Label>--%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Currency">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblCurrencyHeader" runat="server">
                                    Currency<br />Code
                                            </asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Amount">
                                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Label ID="lblInvoiceAmount" runat="server">
                                    Invoice<br />Amount 
                                            </asp:Label>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEAMOUNT","{0:n2}") %>'>                                   
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Type">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblInvoiceType" runat="server" CommandName="Sort" CommandArgument="FLDINVOICETYPE"
                                                ForeColor="White">Invoice<br /> Type&nbsp;</asp:LinkButton>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICETYPENAME") %>'>                                   
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Invoice Status">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:LinkButton ID="lblInvoiceStatus" runat="server" CommandName="Sort" CommandArgument="FLDINVOICESTATUS"
                                                ForeColor="White">  Invoice<br />Status &nbsp;</asp:LinkButton>
                                            <img id="FLDINVOICESTATUS" runat="server" visible="false" />
                                            <%--                                    
                                    <asp:Label ID="lblInvoiceStatus" runat="server">
                                    Invoice<br />Status 
                                    </asp:Label>--%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESTATUSNAME") %>'>                                   
                                            </asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField HeaderText="Invoice Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblInvoiceDateHeader" runat="server" CommandName="Sort" CommandArgument="FLDINVOICEDATE"
                                        ForeColor="White">Entered<br />Date&nbsp;</asp:LinkButton>
                                    <img id="FLDINVOICEDATE" runat="server" visible="false" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="gvAlertsInvoiceReconciliation" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
