<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationContacts.aspx.cs"
    Inherits="PurchaseQuotationContacts" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contacts</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmQuotationVendor" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
         <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager ID="RadWindowManager" runat="server"></telerik:RadWindowManager>
        <div class="navigation" id="navigation" style="margin-left: 0px; vertical-align: top; border: none; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel RenderMode="Lightweight" ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtVenderCode" runat="server" Width="90px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtVenderName" runat="server" Width="210px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtVenderID" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                </tr>
            </table>
             <telerik:RadGrid RenderMode="Lightweight" ID="gvSupplierContact" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvSupplierContact_NeedDataSource" OnUpdateCommand="gvSupplierContact_UpdateCommand"
                        OnDeleteCommand="gvSupplierContact_DeleteCommand" OnItemDataBound="gvSupplierContact_ItemDataBound1" OnItemCommand="gvSupplierContact_ItemCommand"
                        OnEditCommand="gvSupplierContact_EditCommand" >
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                           CommandItemDisplay="Top"  AutoGenerateColumns="false"  TableLayout="Fixed">
                            <CommandItemSettings ShowPrintButton="false" ShowExportToExcelButton="false" ShowRefreshButton="false" AddNewRecordText="Add Contact" />
                            <Columns>
                                 <telerik:GridTemplateColumn HeaderText="Email Address">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblRelationShipId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSRELATIONSHIPID") %>'></telerik:RadLabel>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblEmailAddress" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAILADDRESS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblAddRelationShipId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDADDRESSRELATIONSHIPID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtEmailAddressEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAILADDRESS") %>'
                                    CssClass="gridinput_mandatory" MaxLength="50" Width="100%"></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Purpose">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblRelation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELATIONSHIP") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox RenderMode="Lightweight" ID="txtRelationEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELATIONSHIP") %>'
                                    CssClass="gridinput_mandatory" Width="100%" ></telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Email Option">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblEmailOption" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAILOPTION") %>'></telerik:RadLabel>
                                <telerik:RadRadioButtonList RenderMode="Lightweight" ID="rblEmailOption" runat="server" Enabled="false" Direction="Horizontal">
                                    <Items>
                                        <telerik:ButtonListItem Text="To" Value="TO" />
                                        <telerik:ButtonListItem Text="CC" Value="CC" />
                                        <telerik:ButtonListItem Text="None" Value="NA" />
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblEmailOptionEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAILOPTION") %>'></telerik:RadLabel>
                                <telerik:RadRadioButtonList RenderMode="Lightweight" ID="rblEmailOptionEdit" runat="server"  Direction="Horizontal">
                                    <Items>
                                        <telerik:ButtonListItem Text="To" Value="TO" />
                                        <telerik:ButtonListItem Text="CC" Value="CC" />
                                        <telerik:ButtonListItem Text="None" Value="NA" />
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                                    <HeaderStyle Width="120px" />
                                    <ItemStyle Width="120px" />
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit" 
                                            CommandName="Edit" ID="cmdEdit"
                                            ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Delete" 
                                            CommandName="Delete" ID="cmdDelete"
                                            ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
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
