<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseMultipleQuotationVendor.aspx.cs" Inherits="PurchaseMultipleQuotationVendor" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MoreLink" Src="~/UserControls/UserControlMoreLinks.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Multiple Quotation to a Vendor</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
<link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/jquery-1.12.4.min.js"></script>
        
        <script type="text/javascript">
        function PaneResized(sender, args) {
            var splitter = $find('RadSplitter1');
            var browserHeight = $telerik.$(window).height();
            splitter.set_height(browserHeight - 80);
            splitter.set_width("100%");
            var grid = $find("gvQuotationFormDetails");           
            var contentPane = splitter.getPaneById("listPane");
            grid._gridDataDiv.style.height = (contentPane._contentElement.offsetHeight - 120) + "px";
            console.log(grid._gridDataDiv.style.height, contentPane._contentElement.offsetHeight);
        }
        function pageLoad() {
            PaneResized();
        }
    </script>


</telerik:RadCodeBlock></head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmPurchaseMultipleQuotationVendor" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvQuotationFormDetails">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvQuotationFormDetails" /> 
                        <telerik:AjaxUpdatedControl ControlID="ifMoreInfo" /> 
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server"></telerik:RadAjaxLoadingPanel>
            <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>                
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
                <div style="font-weight:600;font-size:12px" runat="server">
                    <eluc:TabStrip ID="MenuMultipleQuotationMain" runat="server" OnTabStripCommand="MenuMultipleQuotationMain_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                </div>
                <table width="50%">
                    <tr>
                        <td style="width:20%">
                            <telerik:RadLabel RenderMode="Lightweight" ID="lblVenor" runat="server" Text="Vendor Name"></telerik:RadLabel>
                        </td>
                        <td style="width:80%">
                            <telerik:RadTextBox RenderMode="Lightweight" runat="server" ID="txtVendorName" ReadOnly="true" CssClass="readonlytextbox" Width="100%"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="hdnvendorid" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnvesselid" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnoid" runat="server"></asp:HiddenField>
                <asp:HiddenField ID="hdnorderId" runat ="server" ></asp:HiddenField>
                <telerik:RadSplitter RenderMode="Lightweight" ID="RadSplitter1" runat="server" Width="100%" Orientation="Horizontal">
                    <telerik:RadPane ID="editPane" runat="server" Scrolling="None">
                        <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 350px; width: 100%; background: white;" frameborder='0'></iframe>
                    </telerik:RadPane>
                    <telerik:RadSplitBar ID="RadSplitbar1" runat="server" CollapseMode="Both"></telerik:RadSplitBar>
                    <telerik:RadPane ID="listPane" runat="server" Scrolling="None" OnClientResized="PaneResized" Height="350">
                        <div style="font-weight: 600; font-size: 12px;">
                            <eluc:TabStrip ID="MenuMultipleQuotation" runat="server" OnTabStripCommand="MenuMultipleQuotation_TabStripCommand"></eluc:TabStrip>
                        </div>

                        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="rgvLine" DecoratedControls="All" EnableRoundedCorners="true" />
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvQuotationFormDetails" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                            CellSpacing="0" GridLines="None" OnDeleteCommand="gvQuotationFormDetails_DeleteCommand" OnSortCommand="gvQuotationFormDetails_SortCommand"
                            OnNeedDataSource="gvQuotationFormDetails_NeedDataSource" OnInsertCommand="gvQuotationFormDetails_InsertCommand" OnEditCommand="gvQuotationFormDetails_EditCommand"
                            OnItemDataBound="gvQuotationFormDetails_ItemDataBound1" OnItemCommand="gvQuotationFormDetails_ItemCommand" OnUpdateCommand="gvQuotationFormDetails_UpdateCommand">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false" DataKeyNames="FLDORDERID,FLDFORMSTATUS,FLDVESSELID,FLDVESSELNAME" TableLayout="Fixed">
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Number">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkFormNumberName" runat="server" CommandName="Select"
                                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Title">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="240px"></ItemStyle>
                                        <HeaderStyle Width="240px" />
                                        <ItemTemplate>
                                            <telerik:RadLabel RenderMode="Lightweight" ID="lblStockItemCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Form Type">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel RenderMode="Lightweight" ID="lblPrefferedVendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPENAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Type">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel RenderMode="Lightweight" ID="lblStockType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Component Class / Store Type">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel RenderMode="Lightweight" ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKCLASS") %>'></telerik:RadLabel>
                                            <telerik:RadLabel RenderMode="Lightweight" ID="lblStockId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKCLASSID") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Vessel">
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel RenderMode="Lightweight" ID="lblVesselCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELCODE") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                                        <HeaderStyle Width="120px" HorizontalAlign="Center" />
                                        <ItemStyle Width="120px" HorizontalAlign="Center" />
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Add"
                                                CommandName="ADD" ID="cmdAdd"
                                                ToolTip="Add Quotation">
                                            <span class="icon"><i class="fas fa-plus"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
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
                    </telerik:RadPane>
                </telerik:RadSplitter>    
            </div>
    </form>
</body>
</html>
