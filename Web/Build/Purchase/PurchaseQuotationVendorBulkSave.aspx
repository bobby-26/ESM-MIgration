<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationVendorBulkSave.aspx.cs" Inherits="Purchase_PurchaseQuotationVendorBulkSave" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Numeber" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ucUnit" Src="~/UserControls/UserControlPurchaseUnit.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Quotation Line Bulk Save </title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
     <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    <script type="text/javascript">
             function PaneResized(sender, args) {
                 
                 var browserHeight = $telerik.$(window).height();
                 var grid = $find("rgvLine");
                 grid._gridDataDiv.style.height = (browserHeight - 130) + "px";
             }
            function pageLoad() {
                PaneResized();
            }
        </script>
</telerik:RadCodeBlock></head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmQuotationLineBulkSave" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnableScriptCombine="false" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgvLine">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="menuSaveDetails">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="menuSaveDetails" />
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
            </AjaxSettings>
        </telerik:RadAjaxManager>
                    <eluc:TabStrip ID="menuSaveDetails" runat="server" TabStrip="false" OnTabStripCommand="menuSaveDetails_TabStripCommand">
                    </eluc:TabStrip>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server" Text="" />
                <telerik:RadGrid RenderMode="Lightweight" ID="rgvLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="rgvLine_NeedDataSource" OnItemDataBound="rgvLine_ItemDataBound" OnBatchEditCommand="rgvLine_BatchEditCommand" OnPreRender="rgvLine_PreRender">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="Batch" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" CommandItemDisplay="Top" DataKeyNames="FLDQUOTATIONLINEID,FLDUNITID,FLDQUOTATIONID,FLDVESSELID,FLDPARTID,FLDORDERLINEID,FLDCOMPONENTID" TableLayout="Fixed">    
                        <BatchEditingSettings EditType="Cell" />
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" SaveChangesText="Save" />
                        <%--<Columns>
                            <telerik:GridTemplateColumn HeaderText="S.No" UniqueName="SNO">
                                <ItemStyle Width="40px" />
                                <HeaderStyle Width="40px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblSerialNo" runat="server"></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>                            
                            <telerik:GridTemplateColumn HeaderText="Part Number" UniqueName="NUMBER">
                                <ItemStyle Width="120px" />
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <asp:Label ID="lblPartNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:Label>
                                     <input type="hidden" name="hidQuotationId" id="hidQuotationId" value='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONID") %>' />
                                     <input type="hidden" name="hidQuotationLineId" id="hidQuotationLineId" value='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTATIONLINEID") %>' />                                     
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Name" UniqueName="NAME">
                                <ItemTemplate>
                                     <telerik:RadLabel RenderMode="Lightweight" ID="lnkStockItemCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel><br /> 
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblComponentName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME")%>'></telerik:RadLabel>                                                            
                                   </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Maker Reference" UniqueName="MAKERREF">
                                <ItemStyle Width="120px" />
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblMakerReference" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE")%>'></telerik:RadLabel>                                                            
                                   </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                             <telerik:GridTemplateColumn HeaderText="ROB" UniqueName="ROB">
                                 <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblROBQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROBQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Quantity" UniqueName="QUANTITY">
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblOrderQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                </ItemTemplate>                               
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn  HeaderText="Unit" UniqueName="UNIT">
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>                                   
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblunit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblunitid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>'></telerik:RadLabel> 
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblItemid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTID") %>'></telerik:RadLabel>                              
                                    <telerik:RadDropDownList runat="server" ID="ddlUnit" DataTextField="FLDUNITNAME" DataValueField="FLDUNITID" Width="100%" >
                                    </telerik:RadDropDownList>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                                       
                            <telerik:GridTemplateColumn HeaderText="Quoted Qty" UniqueName="QUOTEDQTY">
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblQuantity" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                    <eluc:Numeber ID="txtQuantityEdit" runat="server" Width="100%" CssClass="gridinput_mandatory" 
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>' Mask="99999" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Price" UniqueName="PRICE">
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblQuotedPrice" runat="server" Visible="false" Width="90px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE","{0:n2}") %>'></telerik:RadLabel>
                                    <eluc:Numeber ID="txtQuotedPriceEdit" runat="server" Width="100%" CssClass="gridinput_mandatory"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE","{0:n3}") %>' MaxLength="16" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                             <telerik:GridTemplateColumn HeaderText="Discount" UniqueName="DISCOUNT">
                                 <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="txtDiscount" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>'></telerik:RadLabel>
                                    <eluc:Numeber ID="txtDiscountEdit" runat="server" Width="100%" CssClass="gridinput" IsPositive="true"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>' Mask="99.99" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Del.Time" UniqueName="DELTIME">
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <eluc:Numeber ID="txtDeliveryEdit" runat="server" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDELIVERYTIME","{0:n0}") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>--%>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                            PageSizeLabelText="Items per page:" AlwaysVisible="true" />
                    </MasterTableView>
                    <ClientSettings AllowKeyboardNavigation="true" EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        <KeyboardNavigationSettings EnableKeyboardShortcuts="true" AllowSubmitOnEnter="true"
                                 AllowActiveRowCycle="true" MoveDownKey="DownArrow" MoveUpKey="UpArrow"></KeyboardNavigationSettings>
                        <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                    </ClientSettings>
                </telerik:RadGrid>
    </form>
</body>
</html>
