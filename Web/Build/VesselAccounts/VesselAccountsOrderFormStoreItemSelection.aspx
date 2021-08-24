<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsOrderFormStoreItemSelection.aspx.cs"
    Inherits="VesselAccountsOrderFormStoreItemSelection" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlHard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Line Item</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
         <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }

        .fon {
            font-size: small !important;
        }
    </style>
</head>
<body>
    <form id="frmStoreItemList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <%--        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvStoreItem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvStoreItem" UpdatePanelHeight="83%" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>--%>
        <eluc:TabStrip ID="MenuStockItem" runat="server" OnTabStripCommand="MenuStockItem_TabStripCommand"></eluc:TabStrip>        
        <table width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtNumberSearch" CssClass="input_mandatory" MaxLength="20"
                        Width="180px">
                    </telerik:RadTextBox>
                    <span id="Span1" class="icon" runat="server"><i class="fas fa-info-circle" style="align-content: center"></i></span>
                    <telerik:RadToolTip RenderMode="Lightweight" runat="server" ID="RadToolTip1" Width="400px" ShowEvent="onmouseover"
                        RelativeTo="Element" Animation="Fade" TargetControlID="Span1" IsClientID="true" CssClass="fon"
                        HideEvent="ManualClose" Position="TopCenter" EnableRoundedCorners="true" ContentScrolling="Auto"
                        Text="">
                        <font color="blue"><b>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Note:"></telerik:RadLabel>
                        </b>
                            <telerik:RadLabel ID="lblForembeddedsearchusesymbolEgNamexxxx" runat="server" Text="For embedded search, use '%' symbol. (Eg. Name: %xxxx)"></telerik:RadLabel>
                        </font>
                    </telerik:RadToolTip>
                </td>
                <td>
                    <telerik:RadLabel ID="lblStoreItemName" runat="server" Text="Store Item Name"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtStockItemNameSearch" CssClass="input" Width="240px"
                        Text="">
                    </telerik:RadTextBox>
                </td>
                <td>
                    <telerik:RadLabel ID="lblMessage" runat="server" ForeColor="Red">*Red Line item is not in Market. </telerik:RadLabel>
                </td>
            </tr>

        </table>
        <br clear="all" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="83%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvStoreItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="98%"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvStoreItem_NeedDataSource" OnItemDataBound="gvStoreItem_ItemDataBound" OnUpdateCommand="gvStoreItem_UpdateCommand" EnableViewState="true"
                OnPreRender="gvStoreItem_PreRender" OnEditCommand="gvStoreItem_EditCommand">
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDSTOREITEMID" TableLayout="Fixed">
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Number" UniqueName="NUMBER">
                            <ItemStyle Width="120px" />
                            <HeaderStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStockItemNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" UniqueName="NAME">
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblStoreItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit" UniqueName="Unit">
                            <ItemStyle Width="180px" />
                            <HeaderStyle Width="180px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Maker" UniqueName="Maker">
                            <ItemStyle Width="120px" />
                            <HeaderStyle Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblMaker" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ROB" UniqueName="ROB">
                            <ItemStyle Width="90px" HorizontalAlign="Right" />
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblROB" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROB","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Quantity" UniqueName="QUANTITY">
                            <ItemStyle Width="90px" HorizontalAlign="Right" />
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                <telerik:RadLabel RenderMode="Lightweight" ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadNumericTextBox ID="txtQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY","{0:n0}") %>'
                                    CssClass="txtNumber" Width="100%" NumberFormat-DecimalDigits="0" IncrementSettings-InterceptArrowKeys="false">
                                </telerik:RadNumericTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit Price" UniqueName="UNITPrice">
                            <ItemStyle Width="90px" />
                            <HeaderStyle Width="90px" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDUNITPRICE")%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtUnitPrice" runat="server" CssClass="input" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITPRICE") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ACTION">
                            <ItemStyle Width="90px" />
                            <HeaderStyle Width="90px" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItem%>" ID="cmdEdit" ToolTip="Edit"><span class="icon"><i class="fas fa-edit"></i></span></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" CommandArgument="<%# Container.DataItem %>" ID="cmdUpdate" ToolTip="Update">
                             <span class="icon"><i class="fas fa-save"></i></span></asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" CommandArgument="<%# Container.DataItem%>" ID="cmdCancel" ToolTip="Cancel">
                                <span class="icon"><i class="fas fa-times"></i></span></asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
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
                        PageSizeLabelText="Items per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />

                </MasterTableView>
                <ClientSettings AllowKeyboardNavigation="true" EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    <KeyboardNavigationSettings EnableKeyboardShortcuts="true" AllowSubmitOnEnter="true"
                        AllowActiveRowCycle="true" MoveDownKey="DownArrow" MoveUpKey="UpArrow"></KeyboardNavigationSettings>

                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <table width="100%" border="0" cellpadding="1" cellspacing="1">
            <tr>
            </tr>
        </table>

    </form>
</body>
</html>
