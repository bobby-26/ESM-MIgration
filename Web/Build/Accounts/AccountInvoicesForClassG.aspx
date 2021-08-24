<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountInvoicesForClassG.aspx.cs"
    Inherits="AccountsInvoicesForClassG" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStripTelerik" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invoice </title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
      <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAccountsInvoiceSearch" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
     <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" Height="94%">
             
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                   
                            <eluc:TabStrip ID="Menutab" runat="server" TabStrip="false" >
                            </eluc:TabStrip>
                      
                            <eluc:TabStrip ID="MenuSpareItemList" runat="server" TabStrip="false" OnTabStripCommand="MenuSpareItemList_TabStripCommand">
                            </eluc:TabStrip>

           <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvSpareItemList" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="gvSpareItemList" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvSpareItemList_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" OnSortCommand="gvSpareItemList_SortCommand" 
                    OnItemDataBound="gvSpareItemList_ItemDataBound" OnItemCommand="gvSpareItemList_ItemCommand" Height="96%"
                    ShowFooter="false" ShowHeader="true" >
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" >         
                            <Columns>
                              <telerik:GridTemplateColumn HeaderText="Supplier Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBackToBack" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Supplier Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSupplierName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUPPLIERNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText=" Vendor Invoice Number">
                                  
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblInvoiceRef" runat="server" CommandName="Select" CommandArgument='<%# Container.DataSetIndex%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESUPPLIERREFERENCE")  %>'></telerik:RadLabel>
                                 
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Received Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICERECEIVEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Invoice Amount">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblInvoiceAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEAMOUNT","{0:n2}") %>'>                                   
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                          
                            <telerik:GridTemplateColumn HeaderText="Currency Code">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCurrency" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Invoice Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblInvoiceType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICETYPENAME") %>'>                                   
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Invoice Status">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblInvoiceStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESTATUSNAME") %>'>                                                                                                          
                                    </telerik:RadLabel>
                                    <telerik:RadLabel ID="lblStatus" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESTATUSCHANGEINFO") %>'>                                                                                                          
                                    </telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESTATUSCHANGEINFO")%>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Register Number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblInvoiceid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblInvoice" runat="server" CommandName="Select" CommandArgument='<%# Container.DataSetIndex%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER")  %>'></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtInvoiceCode" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDINVOICECODE") %>'
                                        Visible="false"></telerik:RadTextBox>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Invoice Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblActivity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICEDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Invoice PIC">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPIC" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPICUSERNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Remarks">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                             
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Action">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               
                                <ItemTemplate>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="More Info" ImageUrl="<%$ PhoenixTheme:images/te_notes.png %>"
                                        CommandName="INFO" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdMoreInfo"
                                        ToolTip="More Info"></asp:ImageButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            </Columns>
                        <NoRecordsTemplate>
                            <table width="100%" border="0">
                                <tr>
                                    <td align="center">
                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                    </td>
                                </tr>
                            </table>
                        </NoRecordsTemplate>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                        EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
                     
                  </telerik:RadAjaxPanel>

    </form>
</body>
</html>


                 
