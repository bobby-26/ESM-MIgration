<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardSKKPI.aspx.cs" Inherits="Inspection_InspectionShippingKPI" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHardExtn.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  
        <title>Key Performance Indicators (KPI)</title>
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
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="gvKPI" DecoratedControls="All" EnableRoundedCorners="true" />
          <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
        <eluc:TabStrip ID="Tabkpi" runat="server" OnTabStripCommand="KPI_TabStripMenuCommand" TabStrip="true"></eluc:TabStrip>
         <eluc:TabStrip ID="TabstripMenu" runat="server" OnTabStripCommand="KPI1_TabStripMenuCommand" TabStrip="true"></eluc:TabStrip>
           <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvKPI" AutoGenerateColumns="false"
            AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvKPI_NeedDataSource" Height="88.5%" EnableLinqExpressions="false" EnableViewState="true" AllowFilteringByColumn="true"
            OnItemDataBound="gvKPI_ItemDataBound" OnItemCommand="gvKPI_ItemCommand" ShowFooter="false">
            <MasterTableView EditMode="InPlace" DataKeyNames="FLDKPIID" AutoGenerateColumns="false"
                TableLayout="Fixed" CommandItemDisplay="None" ShowHeadersWhenNoRecords="true" EnableColumnsViewState="false" 
                InsertItemPageIndexAction="ShowItemOnCurrentPage">
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
                    <telerik:GridTemplateColumn HeaderText="ID" DataField="FLDKPICODE" UniqueName="FLDKPICODE" FilterControlWidth="80px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="102px"/>
                            <ItemStyle HorizontalAlign="Center" />
                          
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblkpicode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDKPICODE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            
                        </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Name" DataField="FLDKPINAME" UniqueName="FLDKPINAME" FilterControlWidth="150px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="170px"/>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblkpiname" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDKPINAME")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            
                        </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText="Units" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="100px"/>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblkpiunit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDUNIT")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            
                        </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText="Scope" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="100px"/>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblkpiscope" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDSCOPE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>  
                        </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText="Period" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="100px"/>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblkpiperiod" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPERIOD")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    
                        <telerik:GridTemplateColumn HeaderText="Department" AllowFiltering="true" DataField="FLDDEPARTMENT" UniqueName="FLDDEPARTMENT" FilterControlWidth="150px" FilterDelay="2000"
                        AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <FilterTemplate>
                                <telerik:RadComboBox runat="server" ID="radcbdept" CssClass="input_mandatory" Width="130px"
                             AutoPostBack="true" OnTextChanged="radcbdept_TextChanged"
                              />
                            </FilterTemplate>
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="150px"/>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblkpidept" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDEPARTMENT")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Level" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="100px"/>
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblkpilvl" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLEVEL")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Description" AllowFiltering="false">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Left" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblkpidescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDKPIDESCRIPTION")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            
                        </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action" AllowFiltering="false">
                            
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="62px" Font-Bold="true"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnedit" ToolTip="Edit" >
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                  &nbsp&nbsp
                        
                                <asp:LinkButton runat="server" ID="btncolor" ToolTip="Grading Configuration" >
                                            <span class="icon"><i class="fas fa-sliders-h"></i></span>
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
