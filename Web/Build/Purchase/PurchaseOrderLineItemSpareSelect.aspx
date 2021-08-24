<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseOrderLineItemSpareSelect.aspx.cs" Inherits="PurchaseOrderLineItemSpareSelect" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="../UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Component" Src="../UserControls/UserControlDropDownComponentTree.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Stock Item Add</title>

    <telerik:RadCodeBlock ID="RadCodeBlock" runat="server">
         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script language="javascript" type="text/javascript" >
        function CheckPressEnterKey(sender, args) {

                    if (args._domEvent.keyCode == 13) {
                        document.getElementById('<%=cmdHiddenSubmit.ClientID  %>').click();
                    }
            }
       </script>
    </telerik:RadCodeBlock>
    
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgvLine">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" UpdatePanelHeight="80%" />
                        <telerik:AjaxUpdatedControl ControlID="ddlComponent" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="ddlComponent">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="ddlComponent" />
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" UpdatePanelHeight="80%" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                <telerik:AjaxSetting AjaxControlID="MenuStoreItemInOutTransaction">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="MenuStoreItemInOutTransaction" />
                        <telerik:AjaxUpdatedControl ControlID="rgvLine" UpdatePanelHeight="80%" />
                        <telerik:AjaxUpdatedControl ControlID="ddlComponent" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
                
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" runat="server" ></telerik:RadAjaxLoadingPanel>
            <eluc:TabStrip ID="MenuStoreItemInOutTransaction" runat="server" OnTabStripCommand="StoreItemInOutTransaction_TabStripCommand">
            </eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden"/>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <table cellpadding="1" cellspacing="1" style="float: left; width: 100%;">
                <tr>
                    <td>
                        <asp:Literal ID="lblPartNumber" runat="server" Text="Part Number"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPartNumber" runat="server" Width="80%" RenderMode="Lightweight" ClientEvents-OnKeyPress="CheckPressEnterKey"></telerik:RadTextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblItemName" runat="server" Text="Item Name"></asp:Literal>

                    </td>
                    <td>
                        <telerik:RadTextBox RenderMode="Lightweight" ID="txtItemName" runat="server" Width="80%" ClientEvents-OnKeyPress="CheckPressEnterKey"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMakerRef" runat="server" Text="Maker Reference"></asp:Label>
                    </td>
                    <td>
                        <%--<asp:DropDownList ID="ddlComponentClass" runat="server" CssClass="input" Visible="false" DataTextField="FLDCOMPONENTNAME" DataValueField="FLDCOMPONENTID" />--%>
                        <telerik:RadTextBox RenderMode="Lightweight" ClientEvents-OnKeyPress="CheckPressEnterKey" ID="txtMakerReference" runat="server" MaxLength="100" Width="80%"></telerik:RadTextBox>
                    </td>
                    <td>
                        <asp:Label ID="lblClassName" runat="server" Text="Component"> </asp:Label>
                    </td>
                    <td>
                        <eluc:Hard ID="ddlStockClassType" runat="server" AppendDataBoundItems="true" Width="80%"
                            Visible="false" />
                        <%--<eluc:Component ID="ddlComponent" runat="server" />--%>
                        <telerik:RadComboBox ID="ddlComponent" runat="server" DataTextField="FLDCOMPONENTNAME" DataValueField="FLDCOMPONENTID" EnableLoadOnDemand="true" Width="80%" OnClientKeyPressing="CheckPressEnterKey">
                        </telerik:RadComboBox>
                        <asp:TextBox ID="txtComponent" runat="server" CssClass="readonlytextbox" ReadOnly="true" Visible="false"></asp:TextBox>
                    </td>
                </tr>
            </table>
        <br clear="all" />
                <telerik:RadGrid RenderMode="Lightweight" ID="rgvLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="100%"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="rgvLine_NeedDataSource" OnItemDataBound="rgvLine_ItemDataBound" OnUpdateCommand="rgvLine_UpdateCommand" 
                    OnPreRender="rgvLine_PreRender" OnEditCommand="rgvLine_EditCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDVESSELID,FLDITEMID,FLDORDERLINEID,FLDCOMPONENTID" TableLayout="Fixed">    
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Number" UniqueName="NUMBER">
                                <ItemStyle Width="120px" />
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Name" UniqueName="NAME">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblIsInMarket" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISINMARKET") %>'></telerik:RadLabel>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblStoreItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Maker" UniqueName="MAKER">
                                <ItemStyle Width="180px" />
                                <HeaderStyle Width="180px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblMaker" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.MAKER") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Maker Ref" UniqueName="MAKERREF">
                                <ItemStyle Width="120px" />
                                <HeaderStyle Width="120px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblMakerReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKERREFERENCE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="ROB" UniqueName="ROB">
                                <ItemStyle Width="90px" HorizontalAlign="Right"/>
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB","{0:n0}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Quantity" UniqueName="QUANTITY">
                                <ItemStyle Width="90px" HorizontalAlign="Right" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="txtQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadNumericTextBox ID="txtQuantityEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUESTEDQUANTITY","{0:n0}") %>'
                                        CssClass="txtNumber" Width="100%" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="false">
                                    </telerik:RadNumericTextBox>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Unit" UniqueName="UNIT">
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ACTION">
                                <ItemStyle Width="90px" />
                                <HeaderStyle Width="90px" HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItem%>" ID="cmdEdit"
                                        ToolTip="Edit">
                                                                <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Save"
                                        CommandName="Update" CommandArgument="<%# Container.DataItem %>" ID="cmdUpdate"
                                        ToolTip="Update">
                                                                <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" AlternateText="Cancel"
                                        CommandName="Cancel" CommandArgument="<%# Container.DataItem%>" ID="cmdCancel"
                                        ToolTip="Cancel">
                                                                <span class="icon"><i class="fas fa-times"></i></span>
                                    </asp:LinkButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Items matching your search criteria"
                            PageSizeLabelText="Items per page:" />
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
                <table width="100%" border="0" cellpadding="1" cellspacing="1">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Red">Red Line item is not in Market. </asp:Label>
                                                </td>
                                            </tr>
                                        </table>
    </form>
</body>
</html>