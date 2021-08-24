<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseVendosPOAnalysis.aspx.cs" Inherits="Purchase_PurchaseVendosPOAnalysis" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvQuery.ClientID %>"));
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
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
          <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
         <table width="100%"> 
                <tr>
                    <td>
                    <telerik:RadLabel runat="server"  Text="Stock Type" />   
                    </td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="radstocktype" AllowCustomText="true" EmptyMessage="Type to select Stock type" Filter="Contains">
                            <Items>
                                <telerik:RadComboBoxItem Text="Spares" Value="SPARE" />
                                <telerik:RadComboBoxItem Text="Stores" Value="STORE" />
                                <telerik:RadComboBoxItem Text="Service" Value="SERVICE" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td> <telerik:RadLabel runat="server" Text="From" /></td>
                    <td>
                        <eluc:UserControlDate runat="server" ID="radfromdate" />
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" Text="To" />  
                    </td>
                    <td>
                         <eluc:UserControlDate runat="server" ID="radtodate" />
                    </td>
                    
                </tr>
             <tr>
                <td>
                    <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="RadLabel1" Text="Port"></telerik:RadLabel>
                </td>
                <td >
                     <eluc:Port ID="ddlmulticolumnport" runat="server"
                                    CssClass="input" Width="250px"  />
                </td>
                <td colspan="4">

                </td>
            </tr>
             </table>
        
                   <eluc:Error runat="server" ID="ucError" Visible="false" />
            <eluc:TabStrip ID="MenuPhoenixQuery" runat="server" OnTabStripCommand="MenuPhoenixQuery_TabStripCommand"></eluc:TabStrip>
             <telerik:RadGrid RenderMode="Lightweight" ID="gvQuery" runat="server"  AllowCustomPaging="false" AllowSorting="true" 
                AllowPaging="false" CellSpacing="0" GridLines="None" EnableViewState="true" GroupingEnabled="false" EnableHeaderContextMenu="true"
                AutoGenerateColumns="false" OnNeedDataSource="gvQuery_NeedDataSource" >
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vendor"  AllowFiltering="false" HeaderStyle-Width="50%">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Left" />
                            
                            <ItemTemplate>
                                <telerik:RadLabel ID="radvendor" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDNAME")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Amount (USD)"  AllowFiltering="false" HeaderStyle-Width="25%">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Right" />
                            
                            <ItemTemplate>
                                <telerik:RadLabel ID="radamount" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="% of Total"  AllowFiltering="false" HeaderStyle-Width="25%">
                            <HeaderStyle HorizontalAlign="Center"  Font-Bold="true" />
                            <ItemStyle HorizontalAlign="Right" />
                            
                            <ItemTemplate>
                                <telerik:RadLabel ID="radpercentage" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPERCENTAGE")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            </telerik:GridTemplateColumn>
                    </Columns>
                  
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
             <table width="100%">
                 <tr>
                     <td width="50%">
                         <telerik:RadLabel runat="server" Text="Total" />
                        
                     </td>
                     <td width ="25%" align="right">
                        <telerik:RadLabel runat="server" ID="radtotal" />
                     </td>
                     <td>

                     </td>
                 </tr>
             </table>
    </form>
</body>
</html>
