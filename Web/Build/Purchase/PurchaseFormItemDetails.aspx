<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseFormItemDetails.aspx.cs"
    Inherits="PurchaseFormItemDetails" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Form Line item</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var splitter = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            splitter.set_height(browserHeight - 40);
            splitter.set_width("100%");
            var grid = $find("rgvLine");           
            var contentPane = splitter.getPaneById("listPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 122) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmPurchaseFormItemDetails" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
         <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="MenuLineItem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuLineItem" />
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" LoadingPanelID="RadAjaxLoadingPanel" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="lnkStockItemName">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" LoadingPanelID="RadAjaxLoadingPanel" />
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" LoadingPanelID="RadAjaxLoadingPanel"/>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="rgvLine">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" LoadingPanelID="RadAjaxLoadingPanel" />
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                        <telerik:AjaxUpdatedControl ControlID="ucStatus" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuOrderLineItem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuOrderLineItem" />
                        <%--<telerik:AjaxUpdatedControl ControlID="ifMoreInfo" LoadingPanelID="RadAjaxLoadingPanel" />
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" LoadingPanelID="RadAjaxLoadingPanel"/>--%>
                    </UpdatedControls>
                </telerik:AjaxSetting>
                </AjaxSettings>
            </telerik:RadAjaxManager>
        
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
        <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:TabStrip ID="MenuLineItem" runat="server" OnTabStripCommand="MenuLineItem_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Status runat="server" ID="ucStatus" />
            <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
                <telerik:RadPane ID="editPane" runat="server" Scrolling="None">
                    <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 600px; width: 100%"></iframe>
                </telerik:RadPane>
                <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both"></telerik:RadSplitBar>
                <telerik:RadPane ID="listPane" runat="server" Scrolling="None" OnClientResized="PaneResized" Height="440" >
                    <eluc:TabStrip ID="MenuOrderLineItem" runat="server" OnTabStripCommand="MenuOrderLineItem_TabStripCommand"></eluc:TabStrip>
                    <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="rgvLine" DecoratedControls="All" EnableRoundedCorners="true" />
                    <telerik:RadGrid RenderMode="Lightweight" ID="rgvLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" OnDeleteCommand="rgvLine_DeleteCommand" OnSortCommand="rgvLine_SortCommand"
                        OnNeedDataSource="rgvLine_NeedDataSource" OnInsertCommand="rgvLine_InsertCommand" OnEditCommand="rgvLine_EditCommand"
                        OnItemDataBound="rgvLine_ItemDataBound" OnItemCommand="rgvLine_ItemCommand" OnUpdateCommand="rgvLine_UpdateCommand">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" DataKeyNames="FLDORDERID,FLDVESSELID,FLDORDERLINEID,FLDPARTID,FLDDTKEY" TableLayout="Fixed">
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="" UniqueName="FLAG">
                                    <HeaderStyle Width="30px" />
                                    <ItemStyle Width="30px" />
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgContractFlag" runat="server" Enabled="false" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" />
                                        <asp:ImageButton ID="imgFlag" runat="server" Enabled="false" ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="S.No" UniqueName="SERIALNO">
                                    <HeaderStyle Width="50px" />
                                    <ItemStyle Width="50px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSerialNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Number" UniqueName="NUMBER">
                                    <HeaderStyle Width="90px" />
                                    <ItemStyle Width="90px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblNumber" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPARTNUMBER") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Maker Ref" UniqueName="MAKERREF">
                                    <HeaderStyle Width="120px" />
                                    <ItemStyle Width="120px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblMakerRef" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Drawing No" UniqueName="DRAWINGNO">
                                    <HeaderStyle Width="120px" />
                                    <ItemStyle Width="120px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDrawingNo" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDDRAWINGNO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Stock Item Name" UniqueName="NAME">
                                    <HeaderStyle Width="20%" />
                                <ItemStyle Width="20%" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblComponentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblPartId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPARTID") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkStockItemName" runat="server" CommandName="SELECT"
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton><br />
                                        <telerik:RadLabel ID="lblComponentName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME")%>'></telerik:RadLabel>

                                        <telerik:RadLabel ID="lblIsFormNotes" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOTES") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblIsItemDetails" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILFLAGE") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblIsContract" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTEXISTS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="ROB" UniqueName="ROB">
                                    <HeaderStyle Width="50px" />
                                    <ItemStyle Width="50px" HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblROB" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDROBQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Req Qty" UniqueName="REQUESTEDQ">
                                    <HeaderStyle Width="50px" />
                                    <ItemStyle Width="50px" HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReqQty" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Unit" UniqueName="UNIT">
                                    <HeaderStyle Width="50px" />
                                    <ItemStyle Width="50px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblUnit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="From Order" UniqueName="SPLIT">
                                    <HeaderStyle Width="90px" />
                                    <ItemStyle Width="90px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSplitFrom" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSPLITFORMNO") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Ord Qty" UniqueName="ORDERQTY">
                                    <HeaderStyle Width="90px" />
                                    <ItemStyle Width="90px" HorizontalAlign="Right" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblOrderQty" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Decimal ID="txtOrderQtyEdit" DecimalPlace="0" runat="server" CssClass="input" Text='<%#DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>' Width="70px" />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Receipt Status" UniqueName="RECEIPTSTATUS">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReceiptStatus" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Received Remarks" UniqueName="RREMARKS">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblChkRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHKREMARKS") %>' Visible="false"></telerik:RadLabel>
                                        <asp:LinkButton runat="server" AlternateText="Received Remarks" 
                                            CommandName="ViewRecord" ID="imgReceiptRemarks" ToolTip="View">
                                            <span class="icon"><i class="fas fa-glasses"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                                    <HeaderStyle Width="150px" />
                                    <ItemStyle Width="150px" />
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit" 
                                            CommandName="EDIT" ID="cmdEdit"
                                            ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                        
                                        <asp:LinkButton runat="server" AlternateText="Vendor" 
                                            CommandName="VENDOR" ID="cmdVendor" ToolTip="Vendor">
                                            <span class="icon"><i class="fas fa-user-friends"></i></span>
                                        </asp:LinkButton>
                                        
                                        <asp:LinkButton runat="server" AlternateText="Delete" 
                                            CommandName="DELETE" ID="cmdDelete"
                                            ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                        </asp:LinkButton>
                                        
                                        <asp:LinkButton runat="server" AlternateText="Delete" 
                                            CommandName="Details" ID="cmdDetail" ToolTip="Item Details">
                                            <span class="icon"><i class="fas fa-info"></i></span>
                                        </asp:LinkButton>
                                        
                                        <asp:LinkButton runat="server" AlternateText="Audit Trail" 
                                            CommandName="AUDITTRAIL" ID="cmdAudit" ToolTip="Audit Trail">
                                            <span class="icon"><i class="fas fa-audittrail"></i></span>
                                        </asp:LinkButton>
                                        
                                        <asp:LinkButton runat="server" AlternateText="Critical Spare Search" 
                                            CommandName="SPARESEARCH" ID="cmdCriticalItem" ToolTip="Critical Spare Search">
                                            <span class="icon"><i class="fas fa-cogs"></i></span>
                                        </asp:LinkButton>
                                        
                                        <asp:LinkButton runat="server" AlternateText="remarks" 
                                            CommandName="REMARKS" ID="cmdRemarks" Visible="false" ToolTip="Remarks">
                                            <span class="icon"><i class="fas fa-glasses"></i></span>
                                        </asp:LinkButton>

                                         <asp:LinkButton runat="server" AlternateText="Spare Transfer" 
                                            CommandName="TRANSFER" ID="cmdSpareTransfer" Visible="false" ToolTip="Spare Transfer">
                                            <span class="icon"><i class="fas fa-spare-move"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save" 
                                            CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                        
                                        <asp:LinkButton runat="server" AlternateText="Cancel" 
                                            CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
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
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                                PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" EnablePostBackOnRowClick="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            <%--<ClientEvents OnGridCreated="GridCreated" />--%>
                        </ClientSettings>
                    </telerik:RadGrid>
                    <div id="div2" style="position: relative;">
                        <table width="100%" border="0" cellpadding="1" cellspacing="1">
                            <tr>
                                <td>
                                    <asp:ImageButton ID="imgFlag" runat="server" Enabled="false" ImageUrl="<%$ PhoenixTheme:images/detail-flag.png %>" /><asp:Label
                                        ID="lblMessage" runat="server" ForeColor="Red"> Line item Details. </asp:Label>
                                </td>
                                <td>
                                    <asp:ImageButton ID="imgContractExists" runat="server" Enabled="false" ImageUrl="<%$ PhoenixTheme:images/contract-exist.png %>" /><asp:Label
                                        ID="Label1" runat="server" ForeColor="Red"> Rate Contract exists for the Item. </asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                </telerik:RadPane>
            </telerik:RadSplitter>






            <%--<telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server" RenderMode="Lightweight">
                 </telerik:RadAjaxLoadingPanel>--%>
        </div>
    </form>
</body>
</html>
