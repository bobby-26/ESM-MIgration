<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseOrderLineItemBulkSave.aspx.cs" Inherits="PurchaseOrderLineItemBulkSave" %>

<!DOCTYPE html>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Numeber" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ucUnit" Src="~/UserControls/UserControlPurchaseUnit.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Order Line Bulk Save </title>
    <telerik:RadCodeBlock ID="RadCodeBlock" runat="server">        
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>     
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>        
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/jquery-1.12.4.min.js"></script>

        
    </telerik:RadCodeBlock>
    
</head>
<body>
    <form id="frmOrderLineBulkSave" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgvLine">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" UpdatePanelHeight="88%"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="menuSaveDetails">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="menuSaveDetails"  />
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" UpdatePanelHeight="98%" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
            <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 100%">
                <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="menuSaveDetails" runat="server" TabStrip="false" OnTabStripCommand="menuSaveDetails_TabStripCommand">
                    </eluc:TabStrip>
                    </div>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server" Text="" />
                
                <br clear="all" />
                <font color="blue"><b>Note:</b> Fill up all the mandatory fields marked in <font color="red">red</font> colour and then click 'Save'. Line items which are partially filled up will not be updated.</font>                
                                
                <telerik:RadGrid RenderMode="Lightweight" ID="rgvLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="100%"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="rgvLine_NeedDataSource" OnItemDataBound="rgvLine_ItemDataBound" OnPreRender="rgvLine_PreRender">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false" ></SortingSettings>
                    <MasterTableView EditMode="InPlace" CommandItemDisplay="None" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDORDERID,FLDPARTID,FLDORDERLINEID,FLDFORCOMPID" TableLayout="Fixed">
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="S.No" UniqueName="SNO">
                                <ItemStyle Width="40px" />
                                <HeaderStyle Width="40px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                                      
                            <telerik:GridTemplateColumn HeaderText="Part Number" UniqueName="NUMBER">
                                <ItemStyle Width="120px" />
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblPartNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNUMBER") %>'></telerik:RadLabel>
                                     <input type="hidden" name="hidOrderId" id="hidOrderId" value='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>' />
                                     <input type="hidden" name="hidOrderLineId" id="hidOrderLineId" value='<%# DataBinder.Eval(Container,"DataItem.FLDORDERLINEID") %>' />                                     
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Name" UniqueName="NAME">
                                <ItemTemplate>
                                    <%--<a href="#"><%# DataBinder.Eval(Container,"DataItem.FLDNAME") %></a>--%>
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
                            
                            <telerik:GridTemplateColumn HeaderText="Requested Qty" UniqueName="REQUESTEDQTY">
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <eluc:Numeber ID="txtRequestedQuantityEdit" runat="server" ReadOnly="true" Width="100%"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY","{0:n0}") %>' Mask="99999" InterceptArrowKeys="false" InterceptMouseWheel="false" />
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblRequestedQuantity" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                </ItemTemplate>                               
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Unit" UniqueName="UNIT">
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>                                   
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblunit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblunitid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>'></telerik:RadLabel> 
                                    <telerik:RadDropDownList RenderMode="Lightweight" runat="server" ID="ddlUnit" Width="100%" DataTextField="FLDUNITNAME" DataValueField="FLDUNITID"
                                        ></telerik:RadDropDownList>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Ordered Qty" UniqueName="ORDEREDQTY">
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblOrderQuantity" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                    <eluc:Numeber ID="txtOrderedQuantityEdit" runat="server" CssClass="gridinput_mandatory" Width="100%" InterceptArrowKeys="false" InterceptMouseWheel="false"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>' Mask="99999" />
                                </ItemTemplate>        
                            </telerik:GridTemplateColumn>
                                                       
                            <telerik:GridTemplateColumn HeaderText="Received Qty" UniqueName="RECEIVEDQTY">
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblReceivedQuantity" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                    <eluc:Numeber ID="txtReceivedQuantityEdit" runat="server" MaxLength="8" Width="100%" InterceptArrowKeys="false" InterceptMouseWheel="false"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY","{0:n0}") %>' Mask="99999" />
                                 </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                         <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                            PageSizeLabelText="Items per page:"  AlwaysVisible="true"/>
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
            </div>
    </form>
</body>
</html>
