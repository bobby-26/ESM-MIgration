<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseWhatifQuotationItems.aspx.cs"
    Inherits="PurchaseWhatifQuotationItems" MaintainScrollPositionOnPostback="true"
    EnableEventValidation="false" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Numeber" Src="../UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TaxType" Src="~/UserControls/UserControlTaxType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register Src="../UserControls/UserControlCurrency.ascx" TagName="UserControlCurrency"
    TagPrefix="eluc" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/Fonts/fontawesome/css/all.min.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var browserHeight = $telerik.$(window).height();
            var grid = $find("gvVendorItem");
            grid._gridDataDiv.style.height = (browserHeight - 300) + "px";
        }
        function pageLoad() {
            PaneResized();
        }

            </script>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmPurchaseQuotationItems" runat="server" autocomplete="off">
     <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />    
    <div class="navigation" id="navigation" style="position: relative; z-index: 0; width: 100%;">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
        <div style="font-weight:600;font-size:12px">
            <eluc:TabStrip ID="MenuQuotationLineItem" runat="server" OnTabStripCommand="MenuQuotationLineItem_TabStripCommand">
            </eluc:TabStrip>
        </div>
        <br clear="all" />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                          <telerik:RadLabel RenderMode="Lightweight" ID="lblVendorName" runat="server" Text="Vendor Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtVendorName" runat="server" Width="300px" CssClass="input"></telerik:RadTextBox>
                        </td>
                        <td>
                          <telerik:RadLabel RenderMode="Lightweight" ID="lblQuotationValidUntil" runat="server" Text="Quotation Valid Until"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <eluc:Date ID="txtOrderDate" runat="server" CssClass="input" Width="150px"></eluc:Date>
                        </td>
                        <td>
                          <telerik:RadLabel RenderMode="Lightweight" ID="lblExpiryDate" runat="server" Text="Expiry Date"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtExpirationDate" runat="server" CssClass="input" Enabled="false"
                                ReadOnly="true" Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                          <telerik:RadLabel RenderMode="Lightweight" ID="lblAddress" runat="server" Text="Address"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtVendorAddress" runat="server" CssClass="input" TextMode="MultiLine"
                                ReadOnly="true" Height="20px" Width="300px"></telerik:RadTextBox>
                        </td>
                        <td style="vertical-align: top">
                          <telerik:RadLabel RenderMode="Lightweight" ID="lblQuotationRef" runat="server" Text="Quotation Ref."></telerik:RadLabel>
                            
                        </td>
                        <td style="vertical-align: top">
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtVenderReference" runat="server" CssClass="input" Width="150px"></telerik:RadTextBox>
                        </td>
                        <td style="vertical-align: top">
                          <telerik:RadLabel RenderMode="Lightweight" ID="lblPriority" runat="server" Text="Priority"></telerik:RadLabel>
                            
                        </td>
                        <td style="vertical-align: top">
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtPriority" runat="server" CssClass="input" ReadOnly="true" Width="150px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="vertical-align: top">
                          <telerik:RadLabel RenderMode="Lightweight" ID="lblRemarks" runat="server" Text="Remarks"></telerik:RadLabel>
                            
                        </td>
                        <td>
                            <telerik:RadTextBox RenderMode="Lightweight" runat="server" Height="20px" Width="300px" CssClass="input" ID="ucVendorRemarks" TextMode="MultiLine"></telerik:RadTextBox>
                             <%--<Custom:CustomEditor ID="ucVendorRemarks" runat="server"  CssClass="input" Style="width:300px; height:20px;"/>--%>
                            <%--<asp:TextBox ID="txtVendorRemarks" runat="server" CssClass="input" Height="40px"
                                TextMode="MultiLine" Width="300px"></asp:TextBox>--%>
                        </td>
                        <td style="vertical-align: top">
                          <telerik:RadLabel RenderMode="Lightweight" ID="lblDeliveryTerms" runat="server" Text="Delivery Terms"></telerik:RadLabel>
                            
                        </td>
                        <td style="vertical-align: top">
                            <eluc:Quick ID="UCDeliveryTerms" AppendDataBoundItems="true" CssClass="input" runat="server" Width="150px" />
                        </td>
                        <td style="vertical-align: top">
                          <telerik:RadLabel RenderMode="Lightweight" ID="lblPaymentTerms" runat="server" Text="Payment Terms"></telerik:RadLabel>
                          
                        </td>
                        <td style="vertical-align: top">
                            <eluc:Quick ID="UCPaymentTerms" AppendDataBoundItems="true" CssClass="input" runat="server" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                </table>
    
    <br />
        <eluc:TabStrip ID="MenuRegistersStockItem" runat="server" OnTabStripCommand="RegistersStockItem_TabStripCommand">
        </eluc:TabStrip>

        <telerik:RadGrid RenderMode="Lightweight" ID="gvVendorItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvVendorItem_NeedDataSource" OnUpdateCommand="gvVendorItem_UpdateCommand"
            OnItemDataBound="gvVendorItem_ItemDataBound">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                DataKeyNames="FLDQUOTATIONLINEID,FLDORDERLINEID,FLDCOMPONENTID,FLDDTKEY,FLDVESSELID" AutoGenerateColumns="false" TableLayout="Fixed">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="S.No">
                        <ItemStyle Wrap="false" HorizontalAlign="Left" Width="40px"/>
                        <HeaderStyle Width="40px" />
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Part Number">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="100px"></ItemStyle>
                        <HeaderStyle Width="100px" />
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkPartNumber" runat="server" CommandName="EDIT"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Part Name">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblStockItemCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            <br />
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblComponentName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Unit">
                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="60px"></ItemStyle>
                        <HeaderStyle Width="60px" />
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblunit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Order Qty">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="90px"></ItemStyle>
                        <HeaderStyle Width="90px" />
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblOrderQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDEREDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Quoted Qty">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="90px"></ItemStyle>
                        <HeaderStyle Width="90px" />
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Unit Price">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="90px"></ItemStyle>
                        <HeaderStyle Width="90px" />
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblQuotedPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUOTEDPRICE","{0:n2}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Amount">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="90px"></ItemStyle>
                        <HeaderStyle Width="90px" />
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="txtAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TOTALQUOTEDPRICE","{0:n2}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="What If Qty">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="90px"></ItemStyle>
                        <HeaderStyle Width="90px" />
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblWhatIfQty" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWHATIFQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Numeber ID="txtWhatIfQtyEdit" runat="server" Width="100%" CssClass="gridinput_mandatory" DecimalDigits="0"
                                Mask="99,999" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWHATIFQUANTITY","{0:n0}") %>' />
                        </EditItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Review Amount">
                        <ItemStyle Wrap="False" HorizontalAlign="Right" Width="90px"></ItemStyle>
                        <HeaderStyle Width="90px" />
                        <ItemTemplate>
                            <telerik:RadLabel RenderMode="Lightweight" ID="txtReviewAmoun" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.TOTALREVICEDPRICE","{0:n2}") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                        <HeaderStyle Width="60px" HorizontalAlign="Center" />
                        <ItemStyle Width="600px" HorizontalAlign="Center" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit"
                                CommandName="Edit" ID="cmdEdit"
                                ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                    PageSizeLabelText="Items per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
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
