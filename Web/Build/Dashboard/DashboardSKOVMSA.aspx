<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardSKOVMSA.aspx.cs" Inherits="Dashboard_DashboardSKOVMSA" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
       <title>Performance Indicators (PI)</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                  <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
            <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
         
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%" Width="100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:TabStrip ID="Tabkpi" runat="server" OnTabStripCommand="Tabkpi_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
                <eluc:TabStrip ID="TabStrip" runat="server" OnTabStripCommand="TabStrip_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                 <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvTMSA" AutoGenerateColumns="false"
            AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvTMSA_NeedDataSource" Height="87%" 
            OnItemDataBound="gvTMSA_ItemDataBound" OnItemCommand="gvTMSA_ItemCommand" ShowFooter="false">
            <MasterTableView EditMode="InPlace" DataKeyNames="FLDTMSAID" AutoGenerateColumns="false" 
                TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" EnableColumnsViewState="false"
                InsertItemPageIndexAction="ShowItemOnCurrentPage" >
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                 <Columns>
                     <telerik:GridTemplateColumn HeaderText="ID">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="100px"/>
                            <ItemStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlbltmsaid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTMSACODE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Short Code">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="100px" />
                            <ItemStyle HorizontalAlign="Center" Wrap="true" />
                           
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlbltmsacode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTMSASHORTCODE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Description">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true"  />
                            <ItemStyle HorizontalAlign="Left" Wrap="true" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlbltmsadescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTMSADESCRIPTION")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75px" Font-Bold="true"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnedit" ToolTip="Edit" >
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                  </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"
                    AlwaysVisible="true" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
        </telerik:RadGrid>
                </telerik:RadAjaxPanel>

    
    </form>
</body>
</html>
