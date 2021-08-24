<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTMSALIMapping.aspx.cs" Inherits="Dashboard_DashboardTMSALIMapping" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
   <title> TMSA-LI Mapping</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
                  <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
         <eluc:TabStrip ID="Tabkpi" runat="server" OnTabStripCommand="Tabkpi_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="TabstripMenu" runat="server" OnTabStripCommand="TabstripMenu_TabStripCommand" TabStrip="true"></eluc:TabStrip>
          <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvTMSALIlist" AutoGenerateColumns="false"
            AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvTMSALIlist_NeedDataSource" Height="88.5%"
            OnItemDataBound="gvTMSALIlist_ItemDataBound" OnItemCommand="gvTMSALIlist_ItemCommand" >
            <MasterTableView EditMode="InPlace" DataKeyNames="FLDELEMENT2LINKID" AutoGenerateColumns="false"
                TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" EnableColumnsViewState="false"
                InsertItemPageIndexAction="ShowItemOnCurrentPage" ItemStyle-Wrap="true">
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
          <ColumnGroups>
                        <telerik:GridColumnGroup Name="OVMSA/TMSA Element" HeaderText="OVMSA/TMSA Element" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" />
                        <telerik:GridColumnGroup Name="LI" HeaderText="LI" HeaderStyle-Font-Bold="true" HeaderStyle-HorizontalAlign="Center" />

                    </ColumnGroups>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Code" ColumnGroupName="OVMSA/TMSA Element">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Center" />

                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlbltmsacode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTMSACODE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" ColumnGroupName="OVMSA/TMSA Element">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true"/>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlbltmsatitle" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTMSADESCRIPTION")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            
                        </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Code" ColumnGroupName="LI">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Center" />

                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlbllicode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLICODE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" ColumnGroupName="LI">
                            <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlbllititle" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLINAME")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                           
                        </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Action" >
                            <FooterStyle HorizontalAlign="Center" Font-Bold="true"/>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></HeaderStyle>
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
