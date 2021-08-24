<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegisterDrill.aspx.cs" Inherits="Drill_DrillRegister" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Drills</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvDrilllist.ClientID %>"));
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
    <form id="form1" runat="server" >
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="gvDrilllist" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
         <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:TabStrip ID="Tabstripdrillregistermenu" runat="server" OnTabStripCommand="drillregistermenu_TabStripCommand"
                TabStrip="true" />
         <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvDrilllist" AutoGenerateColumns="false"
                AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvDrilllist_NeedDataSource" Height="94.3%" EnableLinqExpressions="false" EnableViewState="true" AllowFilteringByColumn="true"
                OnItemDataBound="gvDrilllist_ItemDataBound" OnItemCommand="gvDrilllist_ItemCommand"   >
                <MasterTableView EditMode="InPlace" DataKeyNames="FLDDRILLID" AutoGenerateColumns="false"
                    EnableColumnsViewState="false" TableLayout="Fixed" 
                    ShowHeadersWhenNoRecords="true" InsertItemPageIndexAction="ShowItemOnCurrentPage">
                 
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
                        <telerik:GridTemplateColumn HeaderText="Name" DataField="FLDDRILLNAME" UniqueName="FLDDRILLNAME" FilterControlWidth="120px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center" Width="140px"  Font-Bold="true"/>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="RadlblDrillName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDRILLNAME")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Interval" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="RadlblDrillFrequency" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFREQUENCY")+" "+DataBinder.Eval(Container, "DataItem.FLDFREQUENCYTYPE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Applies To"  AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnappliesto" ToolTip="Applies To">
                                            <span class="icon"><i class="fa fa-ship" ></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Fixed/Variable"  AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Center" />
                            
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblfixedorvariable" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFIXEDORVARIABLE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                          
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type"  AllowFiltering="false">
                        <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                        <ItemStyle HorizontalAlign="Center" />
                        <ItemTemplate>
                            <telerik:RadLabel ID="Radlbltype" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDTYPE")%>' >
                            </telerik:RadLabel>
                        </ItemTemplate>
                     
                    </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Affected by Crew Change?"  AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblaffectedbycrewchange" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAFFECTEDBYCREWCHANGEYN")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>                 
                        </telerik:GridTemplateColumn> 
                        <telerik:GridTemplateColumn HeaderText=" Crew Change Percentage"  AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblaffectedbycrewchangepercentage" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCREWCHANGEPERCENTAGE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Photo Mandatory"  AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblphotoyn" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPHOTOYN")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Show in Dashboard"  AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true"/>
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="RadlblDashboardyn" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDASHBOARDYN")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action"  AllowFiltering="false">
                            <FooterStyle HorizontalAlign="Center"  />
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="75px"  Font-Bold="true"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnedit" ToolTip="Edit" CommandName="Edit">
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
