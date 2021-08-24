<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPMSManual.ascx.cs" Inherits="UserControls_UserControlPMSManual" %>
<telerik:RadListView ID="lstPMSManual" runat="server" ItemPlaceholderID="pnlManual" 
    DataKeyNames="FLDMANUALID">
    <LayoutTemplate>
        <asp:Panel ID="pnlManual" runat="server">
        </asp:Panel>
    </LayoutTemplate>
    <ItemTemplate>
         <asp:HyperLink runat="server" ID="lnkManualLink" Text='<%#Eval("FLDMANUALNAME")%>' ToolTip='<%#Eval("FLDMANUALNAME")%>'
                NavigateUrl='<%# "../Common/Download.aspx?manualid=" + Eval("FLDMANUALID")%>' Target="_blank" />              
    </ItemTemplate>        
</telerik:RadListView>