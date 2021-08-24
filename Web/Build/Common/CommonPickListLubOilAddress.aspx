<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CommonPickListLubOilAddress.aspx.cs"
    Inherits="CommonPickListLubOilAddress" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Address List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />


    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
     <telerik:RadScriptManager  ID="ToolkitScriptManager1"
        runat="server">
    </telerik:RadScriptManager>
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
  
            <eluc:Title runat="server" ID="ucTitle" Text="" ShowMenu="false" />
  
        <eluc:TabStrip ID="MenuAddress" runat="server" OnTabStripCommand="MenuAddress_TabStripCommand" Visible="false">
        </eluc:TabStrip>
    <br clear="all" />
    <telerik:RadAjaxPanel runat="server" ID="pnlAddressEntry" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtNameSearch" CssClass="input" runat="server" Text="" Visible="false"></telerik:RadTextBox>
                        </td>
                        <td>
                            
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtCode" CssClass="input" Text="" Visible="false"></telerik:RadTextBox>
                        </td>
                        <td>
                            
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCountryNameSearch" CssClass="input" Text="" Visible="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                   
                        </td>
                    </tr>
                </table>
                <telerik:RadGrid ID="gvAddress" runat="server" AutoGenerateColumns="False" CellPadding="3" GroupingEnabled="false" EnableHeaderContextMenu="true" 
                    Font-Size="11px" OnItemCommand="gvAddress_ItemCommand" OnItemDataBound="gvAddress_ItemDataBound" OnNeedDataSource="gvAddress_NeedDataSource"
                     OnSortCommand="gvAddress_SortCommand" AllowSorting ="true"  ShowFooter="False" AllowPaging="true" AllowCustomPaging="true" Height="100%"
                    ShowHeader="true" Width="100%" EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                     <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                         <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
                   <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderStyle-Width="110px" HeaderText="Code" >
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:ImageButton runat="server" ID="cmdHiddenSubmit" OnClick="cmdSearch_Click" CommandName="FLDADDRESSCODE"
                                        ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" CommandArgument="1" />
                                <asp:Label ID="lblCodeHeader" runat="server">Code </asp:Label>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderStyle-Width="320px" HeaderText="Name">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <asp:LinkButton ID="lblAddressNameHeader" runat="server" CommandName="Sort" CommandArgument="FLDNAME"
                                    >Name&nbsp;</asp:LinkButton>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblAddressCode" Text='<%# Bind("FLDADDRESSCODE") %>'
                                    Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblEmail" Text='<%# Bind("FLDEMAIL1") %>'  Visible="false"></telerik:RadLabel>                                
                                <asp:LinkButton ID="lnkAddressName" runat="server" CommandArgument='<%# Container.DataItem %>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderStyle-Width="110px" HeaderText="Phone 1">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPhone1Header" runat="server">Phone 1  </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPhone1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPHONE1") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderStyle-Width="120px" HeaderText="Email 1" >
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblEmailHeader" runat="server">Email 1  </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmail1" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMAIL1").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDEMAIL1").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDEMAIL1").ToString() %>'></telerik:RadLabel>                                    
                                <eluc:ToolTip ID="ucEmailTT" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAIL1") %>' />
                                    onmousedown="javascript:closeMoreInformation()" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>                     
                                            
                        <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText=" City">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCityHeader" runat="server"> City </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="State">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblStateHeader" runat="server"> State </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblState" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="Country">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblCountryHeader" runat="server"> Country </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCountry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        
                        <telerik:GridTemplateColumn HeaderStyle-Width="100px" HeaderText="Status">
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblStatusHeader" runat="server"> Status </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn >
                        <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Center" Visible="false" HeaderStyle-Width="100px">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemStyle Wrap="false" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Relation" ImageUrl="<%$ PhoenixTheme:images/annexure.png %>"
                                        CommandName="RELATION" CommandArgument="<%# Container.DataItem %>" ID="cmdRelation"
                                        ToolTip="Related Address"></asp:ImageButton>
                                </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
                              <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                             </MasterTableView>
                       <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="200px"  />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
                </telerik:RadGrid>
         
   
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>
