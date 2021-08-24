<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseGoodsReturnAdd.aspx.cs" Inherits="PurchaseGoodsReturnAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PO List</title>
     <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
       <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmCommonOrder" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

         <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
             
               <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            
             <div id="search">
                <table runat="server" width="100%">
                    <tr>
                        <td>
                             <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        </td>
                        <td>
                              <eluc:Vessel ID="UcVessel" runat="server" OnTextChangedEvent="UcVessel_TextChangedEvent" VesselsOnly="true" Width="150px" AppendDataBoundItems="true" />
                        </td>

                        <td>
                            <telerik:RadLabel ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                        </td>
                        <td>
                            <span id="spnPickListAddress">
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtVendorNumber" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtVenderName" runat="server" Width="180px" CssClass="input"></telerik:RadTextBox>
                                 <asp:LinkButton ID="cmdShowMaker" runat="server" ImageAlign="AbsMiddle" Text=".." 
                                     OnClientClick="return showPickList('spnPickListAddress', 'codehelp1', '', 'Common/CommonPickListAddressOwner.aspx?addresstype=130,131,132&framename=ifMoreInfo', true);">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                </asp:LinkButton>
                        
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtVendorId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                            </span>

                               </td>

                         <td>
                          <telerik:RadLabel ID="lblstocktye" runat="server" Text="Stock Type"></telerik:RadLabel>
                      </td>
                      <td>
                          <telerik:RadDropDownList runat="server" ID="ddlStockType" Width="180px">
                            <Items>
                                <telerik:DropDownListItem Text="--Select--" Value=""  />
                                <telerik:DropDownListItem Text="Spares" Value="SPARE" />
                                <telerik:DropDownListItem Text="Stores" Value="STORE" />
                                <telerik:DropDownListItem Text="Service" Value="SERVICE" />
                            </Items>
                        </telerik:RadDropDownList>
                      </td>
                </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>
                        </td>
                        <td>
                             <telerik:RadTextBox ID="txtTitle" runat="server"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblFromNo" runat="server" Text="Form No"></telerik:RadLabel>
                        </td>
                        <td>
                           <telerik:RadTextBox ID="txtFormNo" runat="server"></telerik:RadTextBox>
                            
                        </td>
                    </tr>
                </table>
            </div>

              <eluc:TabStrip ID="MenuOrder" runat="server" OnTabStripCommand="MenuOrder_TabStripCommand"></eluc:TabStrip>
               <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="gvOrder" DecoratedControls="All" EnableRoundedCorners="true" />
              <telerik:RadGrid RenderMode="Lightweight" ID="gvOrder" runat="server" Height="80%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" EnableViewState="false" GridLines="None"  ShowHeader="true" Width="100%" 
                  OnNeedDataSource="gvOrder_NeedDataSource" OnItemDataBound="gvOrder_ItemDataBound" > 
                   <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDORDERID" >

                       <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>


                     <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Order" UniqueName="ORDER" AllowSorting="true" SortExpression="FLDTITLE" >
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblOrderId" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'
                                    Visible="false"></telerik:RadLabel>
                               <telerik:RadLabel runat="server" ID="lblTitle" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadLabel>
                               <%-- <asp:LinkButton ID="lnkTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>' CommandName="PICKLIST"></asp:LinkButton>--%>
                                <br />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Form No" UniqueName="FORMNO" >
                              <ItemTemplate>
                                <telerik:RadLabel ID="lblFormNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'>   </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="true" SortExpression="FLDVESSELNAME" >
                              <ItemTemplate>
                                <telerik:RadLabel ID="lblvesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' Visible="false">   </telerik:RadLabel>
                                  <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Vendor" AllowSorting="true" SortExpression="FLDNAME"  >
                              <ItemTemplate>
                                <telerik:RadLabel ID="lblVendorId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORID") %>' Visible="false">   </telerik:RadLabel>
                                  <telerik:RadLabel ID="lblVendorName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Stock Type">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStockType" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'> </telerik:RadLabel> 
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Delivery Port">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDeliveryPort" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'> </telerik:RadLabel> 
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                         <telerik:GridTemplateColumn HeaderText="Ordered" >
                              <ItemTemplate>
                                 <telerik:RadLabel ID="lblOrdered" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDORDEREDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                              </ItemTemplate>
                         </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="Received" >
                              <ItemTemplate>
                                    <telerik:RadLabel ID="lblReceived" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDVENDORDELIVERYDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>

                         <telerik:GridTemplateColumn HeaderText="Invoice No" >
                              <ItemTemplate>
                                    <telerik:RadLabel ID="lblInvoiceNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                         </telerik:GridTemplateColumn>

                      <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Wrap="false" HorizontalAlign="Center" />
                            <ItemTemplate>                               
                                 <asp:LinkButton runat="server" AlternateText="Create" CommandName="CREATE" ID="cmdCreate" ToolTip="GRN Create">
                                     <span class="icon"><i class="fa fa-share-square-24"></i></span>
                                </asp:LinkButton>                        
                            </ItemTemplate>                           
                      </telerik:GridTemplateColumn>

                        </Columns>
                     <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" />
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
