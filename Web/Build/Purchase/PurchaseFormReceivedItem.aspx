<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFormReceivedItem.aspx.cs"
    Inherits="PurchaseFormReceivedItem" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlPurchaseUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaPort.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Multiport" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Bulk Received</title>
    <telerik:RadCodeBlock ID="RadCodeBlock" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/jquery-1.12.4.min.js"></script>
        <%--<script type="text/javascript" language="javascript">
            var SelectedRow = null;
            var SelectedRowIndex = null;
            var UpperBound = null;
            var LowerBound = null;

            window.onload = function() {
                UpperBound = parseInt('<%= this.gvRecivedItem.Rows.Count %>') - 1;
                LowerBound = 0;
                SelectedRowIndex = -1;
            }

            function SelectRow(CurrentRow, RowIndex, CellIndex) {
                if (SelectedRow == CurrentRow || RowIndex > UpperBound || RowIndex < LowerBound)
                    return;

                if (CellIndex == null) {
                    SelectedRow = CurrentRow;
                    SelectedRowIndex = RowIndex;
                    return;
                }

                if (SelectedRow != null) {
                    try { SelectedRow.cells[CellIndex].getElementsByTagName('INPUT')[0].focus(); } catch (e) { return true; }
                }

                if (CurrentRow != null) {
                    try { CurrentRow.cells[CellIndex].getElementsByTagName('INPUT')[0].focus(); } catch (e) { return true; }
                }

                SelectedRow = CurrentRow;
                SelectedRowIndex = RowIndex;
                //setTimeout("SelectedRow.focus();",0);

            }

            function SelectSibling(e) {
                var eleminscroll;
                e = e ? e : window.event;
                var KeyCode = e.which ? e.which : e.keyCode;
                if (e.target) eleminscroll = e.target;
                if (e.srcElement) eleminscroll = e.srcElement;

                try {
                    if (KeyCode == 40)
                        SelectRow(SelectedRow.nextSibling, SelectedRowIndex + 1, eleminscroll.parentNode.cellIndex);
                    else if (KeyCode == 38)
                        SelectRow(SelectedRow.previousSibling, SelectedRowIndex - 1, eleminscroll.parentNode.cellIndex);
                }
                catch (ex) {
                    return true;
                }
                return true;
            }
        </script>--%>
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
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" UpdatePanelHeight="75%"/>
                        <telerik:AjaxUpdatedControl ControlID="edittbl" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuQuotationLineItem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuQuotationLineItem" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="menuSaveDetails">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="menuSaveDetails" />
                        <telerik:AjaxUpdatedControl ControlID="edittbl" />
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" UpdatePanelHeight="75%" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuRegistersStockItem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuRegistersStockItem" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>

            <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 100%">
                    <eluc:TabStrip ID="MenuQuotationLineItem" runat="server" OnTabStripCommand="MenuQuotationLineItem_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" Visible="false" />
                    <eluc:TabStrip ID="menuSaveDetails" runat="server" TabStrip="false" OnTabStripCommand="menuSaveDetails_TabStripCommand">
                    </eluc:TabStrip>
                <div id="divField" style="position: relative; z-index: 2">
                    <table cellpadding="1" cellspacing="1" width="100%" runat="server" id="edittbl">
                        <tr>
                            <td style="width:15%;">
                               <telerik:RadLabel  RenderMode="Lightweight" ID="lblReceivedDate" runat="server" Text="Received Date"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Date ID="txtRecivedDate" runat="server" />
                            </td>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblReceivedPort" runat="server" Text="Received Port"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Multiport ID="ucPortMulti" runat="server" CssClass="input" Width="400px"/>
                            </td>
                        </tr>
                        <tr>
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblGoodsStatus" runat="server" Text="Quality of Goods received"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadRadioButtonList ID="rblGoodsStatus" runat="server" AppendDataBoundItems="false"
                                RepeatDirection="Horizontal" AutoPostBack="false" RepeatLayout="Table" 
                                Enabled="true" Direction="Horizontal" Width="400px">
                                <DataBindings DataTextField="FLDHARDNAME" DataValueField="FLDHARDCODE"/>
                            </telerik:RadRadioButtonList>
                        </td>
                   
                        <td>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtRemarks" CssClass="input" runat="server"  Width="400px" MaxLength="200" TextMode="MultiLine" Rows="2" ></telerik:RadTextBox>
                        </td>
                        </tr>
                    </table>
                </div>
                
                <br clear="all" />
                
                <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="rgvLine" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="rgvLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="100%"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="rgvLine_NeedDataSource" OnItemDataBound="rgvLine_ItemDataBound" OnPreRender="rgvLine_PreRender">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDORDERID,FLDPARTID,FLDORDERLINEID,FLDFORCOMPID" TableLayout="Fixed">
                        <Columns>
                             <telerik:GridTemplateColumn HeaderStyle-Width="30px">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="MatchingwithOriginalPOIssued" ImageUrl="<%$ PhoenixTheme:images/alert.png %>"
                                    CommandName="MISMATCHING" Visible="false" CommandArgument='<%# Container.DataItem %>'
                                    ID="cmdMisMaching" ToolTip="Shipped Qty and Received Qty is Mismatch."></asp:ImageButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                               
                            <telerik:GridTemplateColumn HeaderText="S.No" UniqueName="SNO">
                                <ItemStyle Width="40px" />
                                <HeaderStyle Width="40px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                                      
                            <telerik:GridTemplateColumn HeaderText="Part Number" UniqueName="NUMBER">
                                <ItemStyle Width="120px" />
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                     <telerik:RadLabel RenderMode="Lightweight" ID="lblPartNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Component" UniqueName="COMPONENT">
                                <ItemTemplate>
                                     <telerik:RadLabel RenderMode="Lightweight" ID="lblComponentNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENT") %>'></telerik:RadLabel><br /> 
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Part Name" UniqueName="NAME">
                                <ItemTemplate>
                                     <telerik:RadLabel RenderMode="Lightweight" ID="lnkStockItemCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel><br /> 
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Maker Reference" UniqueName="MAKERREF">
                                <ItemStyle Width="120px" />
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblMakerReference" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDMAKERREF")%>'></telerik:RadLabel>                                                            
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Quantity" UniqueName="QUANTITY">
                                <ItemStyle Width="120px" />
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblPrice" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRICE","{0:n2}") %>'></telerik:RadLabel>                                
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblOrderQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                             <telerik:GridTemplateColumn HeaderText="Shipped Qty" UniqueName="SHIPPEDQUANTITY">
                                <ItemStyle Width="120px" />
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblShippedQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHIPPEDQUANTITY","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                           
                            <telerik:GridTemplateColumn HeaderText="Unit" UniqueName="UNIT">
                                <ItemStyle Width="120px" />
                                <HeaderStyle Width="120px" />
                                <ItemTemplate> 
                                    <eluc:Unit ID="ucUnit" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" Width="100%"
                                        SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNITID") %>'/>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                                                       
                            <telerik:GridTemplateColumn HeaderText="Received Qty" UniqueName="RECEIVEDQTY">
                                <ItemStyle Width="120px" />
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblQuantityEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                    <eluc:Number runat="server" ID="txtSpareQuantityEdit" Mask="99999999" DecimalSixDigit="0" Visible="false" Width="100%"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY","{0:n0}") %>' InterceptArrowKeys="false" InterceptMouseWheel="false"/>
                                    <eluc:Number runat="server" ID="txtStoreQuantityEdit" Mask="9999999.999" Width="100%" InterceptArrowKeys="false" InterceptMouseWheel="false"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY") %>' Visible="false" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Remarks" UniqueName="REMARKS">
                                <ItemStyle Width="120px" HorizontalAlign="Center" />
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Remarks" 
                                        CommandName="ViewRecord" CommandArgument='<%# Container.DataItem%>' ID="imgReceiptRemarks"
                                        ToolTip="View">
                                        <span class="icon"><i class="fas fa-glasses"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>

                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Requisitions matching your search criteria"
                            PageSizeLabelText="Requisitions per page:" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" AllowKeyboardNavigation="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                    </ClientSettings>
                </telerik:RadGrid>
            </div>
    </form>
</body>
</html>
