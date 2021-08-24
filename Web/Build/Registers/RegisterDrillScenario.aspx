<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterDrillScenario.aspx.cs"
    Inherits="Registers_scenarioregister" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Drill-Scenario </title>
      <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvScenariolist.ClientID %>"));
                }, 200);
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
        DecorationZoneID="gvDrillHistorylist,table1" DecoratedControls="All" EnableRoundedCorners="true" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
     <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%"> 
     <eluc:TabStrip ID="Tabstripmenu" runat="server" OnTabStripCommand="scenariomenu_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
   <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>    
        <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvScenariolist" AutoGenerateColumns="false"
            AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvScenariolist_NeedDataSource" Height="94.3%"
            OnItemDataBound="gvScenariolist_ItemDataBound"  ShowFooter="false" EnableLinqExpressions="false" EnableViewState="true" AllowFilteringByColumn="true"
            OnItemCommand="gvScenariolist_ItemCommand">
            <MasterTableView EditMode="InPlace" DataKeyNames="FLDSCENARIOID" AutoGenerateColumns="false"
                TableLayout="Fixed" ShowHeadersWhenNoRecords="true" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                >
                <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true" ></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" 
                        ShowAddNewRecordButton="false" ShowExportToPdfButton="false"  />
                <Columns>
                     <telerik:GridTemplateColumn HeaderText="Drill" DataField="FLDDRILLNAME" UniqueName="FLDDRILLNAME" FilterControlWidth="100px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <HeaderStyle HorizontalAlign="Center" Width="130px" Font-Bold="true" />
                        <ItemStyle HorizontalAlign="Left"  />
                       
                        <ItemTemplate>
                            <telerik:RadLabel ID="RadlblDrillName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDRILLNAME")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Scenario" DataField="FLDSCENARIO" UniqueName="FLDSCENARIO" FilterControlWidth="320px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                        <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <HeaderStyle HorizontalAlign="Center"  Font-Bold="true"/>
                        <ItemStyle HorizontalAlign="Left" />
                        <FooterStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="Radlblscenario" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSCENARIO")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                       
                       
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Description" AllowFiltering="false">
                        <HeaderStyle HorizontalAlign="Center" Font-Bold="true"/>
                        <ItemStyle HorizontalAlign="Left" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="Radlblscenariodescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION")%>'>
                            </telerik:RadLabel>
                        </ItemTemplate>
                       
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Action" AllowFiltering="false">
                        <FooterStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></HeaderStyle>
                        <ItemStyle Wrap="False"   HorizontalAlign="Center"></ItemStyle>
                        <ItemTemplate>
                           <asp:LinkButton runat="server" AlternateText="Edit"  ID="cmdEdit" ToolTip="Edit">
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
