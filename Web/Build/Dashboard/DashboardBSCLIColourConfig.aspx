<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardBSCLIColourConfig.aspx.cs" Inherits="Dashboard_DashboardBSCLIColourConfig" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>LI Colour Configuration</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server"
            DecorationZoneID="gvLIlist" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table>
                <tr>
                    <td>&nbsp &nbsp     
                    </td>
                    <th>
                        <telerik:RadLabel ID="Radlbllicodetitle" runat="server" Text="Code " />
                    </th>
                    <th>&nbsp
                    </th>
                    <td>
                        <telerik:RadLabel ID="Radlbllicode" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>&nbsp &nbsp 
                    </td>
                    <th>
                        <telerik:RadLabel ID="Radlbllinametitle" runat="server" Text="Name " />
                    </th>
                    <th>&nbsp
                    </th>
                    <td>
                        <telerik:RadLabel ID="Radlblliname" runat="server" />
                    </td>
                </tr>
            </table>

             <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvLIlist" AutoGenerateColumns="false" Height="88.5%"
            AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvLIlist_NeedDataSource"
            OnItemDataBound="gvLIlist_ItemDataBound" OnItemCommand="gvLIlist_ItemCommand" ShowFooter="true">
            <MasterTableView EditMode="InPlace" DataKeyNames="FLDLICOLOURID" AutoGenerateColumns="false"
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
                
                
                <Columns>
                        <telerik:GridTemplateColumn HeaderText="Minimum ">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblspiminimum" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINIMUMVALUE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                         <EditItemTemplate>
                              <eluc:Decimal ID="Radtbminimumedit" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMINIMUMVALUE")%>' runat="server" />
                         </EditItemTemplate>
                            <FooterTemplate>
                            <eluc:Decimal ID="Radtbminimumentry"  runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Maximum ">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true"/>
                            <ItemStyle HorizontalAlign="Right" />
                            <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblspimaximum" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXIMUMVALUE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                             <EditItemTemplate>
                              <eluc:Decimal ID="Radtbmaximumedit" Text='<%# DataBinder.Eval(Container, "DataItem.FLDMAXIMUMVALUE")%>' runat="server"  />
                         </EditItemTemplate>
                            <FooterTemplate>
                            <eluc:Decimal ID="Radtbmaximumentry"  runat="server" />
                            </FooterTemplate>
                           
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Colour">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true"/>
                            
                            
                            <ItemTemplate>
                               
                             <div id="divColor" runat="server" style='<%# "width:80px; height:10px; background-color:" + DataBinder.Eval(Container,"DataItem.FLDCOLOUR") %>'></div>
                            </ItemTemplate>
                            <FooterTemplate >
                                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                                <telerik:RadColorPicker ID="radlicolourpicker" runat="server" ShowIcon="true"  PaletteModes="WebPalette" />
                            </FooterTemplate>
                            <EditItemTemplate>
                                &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                                <telerik:RadColorPicker ID="radlicolourpickeredit" runat="server" ShowIcon="true"  PaletteModes="WebPalette" SelectedColor='<%# System.Drawing.ColorTranslator.FromHtml (DataBinder.Eval(Container, "DataItem.FLDCOLOUR").ToString())%>'/>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                       
                     <telerik:GridTemplateColumn HeaderText="Action" >
                            
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Font-Bold="true"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                         <FooterStyle HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="btnedit" ToolTip="Edit"  CommandName="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                         <EditItemTemplate>
                              <asp:LinkButton runat="server" ID="btnupdate" ToolTip="Update"  CommandName="Update">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                             &nbsp &nbsp
                              <asp:LinkButton runat="server" ID="btncancel" ToolTip="Cancel"  CommandName="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                         </EditItemTemplate>
                            <FooterTemplate>
                                 <asp:LinkButton runat="server" ID="btnsave" ToolTip="Add"  CommandName="Save">
                                            <span class="icon"><i class="fas fa-plus-circle"></i></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
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
