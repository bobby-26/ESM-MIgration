<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersMoreInfoBillingParties.aspx.cs" Inherits="RegistersMoreInfoBillingParties" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
</telerik:RadCodeBlock></head>
<body>
    <form id="frmBillingParties" runat="server">
    <div>
        <asp:GridView ID="gvBillingParties" runat="server" AutoGenerateColumns="False" Font-Size="11px"
        Width="100%" CellPadding="3" Style="margin-bottom: 0px" ShowHeader="true">
        
        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
        <RowStyle Height="10px" />
        
        <Columns>
            <asp:TemplateField>
                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                <HeaderTemplate>
                    <asp:Label ID="lblBillingPartiesHeader" runat="server">Billing Parties&nbsp;
                    </asp:Label>
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblBillingParties" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
    </div>
    </form>
</body>
</html>
