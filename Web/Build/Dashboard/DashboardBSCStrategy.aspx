<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardBSCStrategy.aspx.cs" Inherits="Dashboard_DashboardBSCStrategy" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <title>Strategic Perspectives </title>
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
            DecorationZoneID="gvBSSP" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
              <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="Tabbss" runat="server" OnTabStripCommand="Tabbss_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="TabstripMenu" runat="server" OnTabStripCommand="TabstripMenu_TabStripCommand" TabStrip="true"></eluc:TabStrip>
          
        <telerik:RadGrid RenderMode="Lightweight" runat="server" ID="gvBSSP" AutoGenerateColumns="false"
            AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvBSSP_NeedDataSource" Height="88.5%" 
            OnItemDataBound="gvBSSP_ItemDataBound" OnItemCommand="gvBSSP_ItemCommand" ShowFooter="true">
            <MasterTableView EditMode="InPlace" DataKeyNames="FLDBSSTRATEGICPERSPECTIVEID" AutoGenerateColumns="false" 
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
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="150px" />
                            <ItemStyle HorizontalAlign="Center" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblbsspid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBSSPID")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                         <EditItemTemplate>
                             <telerik:RadTextBox ID="radtbbsspidedit" runat="server" width="100px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBSSPID")%>' CssClass="input_mandatory"/>
                         </EditItemTemplate>
                         <FooterTemplate>
                             <telerik:RadTextBox ID="radtbbsspidentry" runat="server" width="100px"  CssClass="input_mandatory"/>
                         </FooterTemplate>
                        </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Code">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true"  Width="150px"  />
                            <ItemStyle HorizontalAlign="Center" Wrap="true" />
                            <FooterStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblbsspcode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBSSHORTCODE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                         <EditItemTemplate>
                             <telerik:RadTextBox ID="radtbbsspcodeedit" runat="server" width="100px" >
                                 </telerik:RadTextBox>
                         </EditItemTemplate>
                         <FooterTemplate>
                             <telerik:RadTextBox ID="radtbbsspcodeentry" runat="server" width="100px"  CssClass="input_mandatory"/>
                         </FooterTemplate>
                        </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Description">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" Width="460px" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="Radlblbsspdescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBSDESCRIPTION")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>  
                       <EditItemTemplate>
                             <telerik:RadTextBox ID="radtbbsspdescriptionedit" runat="server" width="430px" Text='<%# DataBinder.Eval(Container, "DataItem.FLDBSDESCRIPTION")%>' CssClass="input_mandatory"/>
                         </EditItemTemplate>
                           <FooterTemplate>
                             <telerik:RadTextBox ID="radtbbsspdescriptionentry" runat="server" width="430px"  CssClass="input_mandatory"/>
                         </FooterTemplate>
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
                                 <asp:LinkButton runat="server" ID="btnsave" ToolTip="Add"  CommandName="Add">
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
