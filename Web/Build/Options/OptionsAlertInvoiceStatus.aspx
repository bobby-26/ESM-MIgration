<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsAlertInvoiceStatus.aspx.cs" Inherits="OptionsAlertInvoiceStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice Status Alert</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    
</telerik:RadCodeBlock></head>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmFollowUpAlert" runat="server">
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
                            <asp:GridView ID="gvAlertsTask" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" OnRowCommand="gvAlertsTask_RowCommand" OnRowDataBound="gvAlertsTask_ItemDataBound"
                                EnableViewState="false" AllowSorting="true" OnSorting="gvAlertsTask_Sorting">
                                
                                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                <RowStyle Height="10px" />
                                
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblInvoiceNo" runat="server" Text="Invoice No"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <%--<asp:Label ID="lblTaskKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTASKKEY") %>'></asp:Label>
                                            <asp:Label ID="lblTaskType" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDID") %>'></asp:Label>--%>
                                            <asp:Label ID="lblInvoiceNumber" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></asp:Label>
                                            <asp:LinkButton ID="lnkInvoiceNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>' CommandArgument="<%# Container.DataItemIndex %>"></asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblTitle" runat="server" Text="Title"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceCode" runat="server" 
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICETITEL") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblCreatedDate" runat="server" Text="Created Date"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblCreatedDate" runat="server" 
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDATE") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblInvoiceStatus" runat="server" Text="Invoice Status"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblInvoiceStatus" runat="server" 
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESTATUSNAME") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    
                                 <%--   <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblInvoiceClearing" runat="server" Text="Invoice Clearing"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblClearing" runat="server" 
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLEARING") %>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                   
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
            <Triggers>
                <asp:PostBackTrigger ControlID="gvAlertsTask" />
            </Triggers>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
