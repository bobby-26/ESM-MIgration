<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseGoodsReturn.aspx.cs" Inherits="Purchase_PurchaseGoodsReturn" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>GoodsReturn</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript">
            function DeleteRecord(args) {
                if (args) {
                    __doPostBack("<%=ucConfirmDelete.UniqueID %>", "");
                }
            }

            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvGoodsReturn.ClientID %>"));
               }, 200);
           }
           window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
           }
        </script>
    </telerik:RadCodeBlock>

</head>
<body>

    <form id="frmGoodsReturn" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <table id="tblSearch" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                        &nbsp;&nbsp;
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
                        <telerik:RadDropDownList runat="server" ID="ddlStockType1" Width="180px">
                            <Items>
                                <telerik:DropDownListItem Text="--Select--" Value="" />
                                <telerik:DropDownListItem Text="Spares" Value="SPARE" />
                                <telerik:DropDownListItem Text="Stores" Value="STORE" />
                                <telerik:DropDownListItem Text="Service" Value="SERVICE" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblGrndateFrom" runat="server" Text="From"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucFromDate" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblGrndateTo" runat="server" Text="To"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlDate ID="ucToDate" runat="server" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCurrencyId" runat="server" Text="Currency"></telerik:RadLabel>

                    </td>
                    <td>
                        <eluc:Currency ID="ucCurrency" AppendDataBoundItems="true" Enabled="true"
                            runat="server" Width="180px" />
                    </td>

                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNumber" runat="server" Text="GRN Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtReferenceNumber" runat="server" Text=""></telerik:RadTextBox>&nbsp;&nbsp;
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblFormNo" runat="server" Text="Form No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtformno" runat="server" Text=""></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblOrderTitle" runat="server" Text="Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtordertitle" runat="server" Text=""></telerik:RadTextBox>
                    </td>
                </tr>

            </table>
            <eluc:TabStrip ID="MenuGoodsReturn" runat="server" OnTabStripCommand="MenuGoodsReturn_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvGoodsReturn" runat="server"
                CellSpacing="0" GridLines="None" Height="78%" EnableHeaderContextMenu="true"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" GroupingEnabled="false" EnableViewState="false"
                OnNeedDataSource="gvGoodsReturn_NeedDataSource" OnItemCommand="gvGoodsReturn_ItemCommand" OnItemDataBound="gvGoodsReturn_ItemDataBound">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDGRNID" TableLayout="Fixed">

                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>

                    <NestedViewSettings>
                        <ParentTableRelation>
                            <telerik:GridRelationFields MasterKeyField="FLDGRNID" DetailKeyField="FLDGRNID" />
                        </ParentTableRelation>
                    </NestedViewSettings>

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel" UniqueName="VESSEL">
                            <HeaderStyle Width="100px" HorizontalAlign="Center" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" Width="100px" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblGrnId" Text='<%#DataBinder.Eval(Container,"DataItem.FLDGRNID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVesselId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Vendor" UniqueName="VENDORID">
                            <HeaderStyle Width="120px" HorizontalAlign="Center" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVendor" runat="server" Text='<% #DataBinder.Eval(Container, "DataItem.FLDVENDORID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVendorNumber" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCODE") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblVendorName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="Stock Type" UniqueName="STOCKTYPE">
                            <HeaderStyle Width="80px" HorizontalAlign="Center" />
                            <ItemStyle Wrap="false" HorizontalAlign="Center" Width="80px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStockType" runat="server" Text='<% #DataBinder.Eval(Container, "DataItem.FLDSTOCKTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Order" UniqueName="ORDER">
                            <HeaderStyle Width="120px" HorizontalAlign="Center" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" Width="120px" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblOrder" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDORDERID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblOrderTitle" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Form No" UniqueName="FORMNO">
                            <HeaderStyle Width="80px" HorizontalAlign="Center" />
                            <ItemStyle Wrap="false" HorizontalAlign="Center" Width="80px" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="lblFormNo" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:LinkButton>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>


                        <telerik:GridTemplateColumn HeaderText="GRN Number" UniqueName="GRNNUMBER">
                            <HeaderStyle Width="80px" HorizontalAlign="Center" />
                            <ItemStyle Wrap="false" HorizontalAlign="Center" Width="80px" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lbGrnId" Text='<%#DataBinder.Eval(Container,"DataItem.FLDGRNID") %>' Visible="false"></telerik:RadLabel>
                                <%--      <telerik:RadLinkbutton ID="lblReferenceNumber" runat="server" Text='<% #DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'> </telerik:RadLinkbutton>--%>
                                <asp:LinkButton ID="lbReferenceNumber" runat="server" Text='<% #DataBinder.Eval(Container, "DataItem.FLDREFERENCENUMBER") %>'></asp:LinkButton>

                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="80px" />
                            <ItemStyle Wrap="false" HorizontalAlign="Center" Width="80px" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>

                    <NestedViewTemplate>
                        <table style="font-size: 11px; width: 60%">
                            <tr>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Text="GRN date"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblGrndate" runat="server" Text='<% # General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDGRNDATE").ToString())  %>'></telerik:RadLabel>

                                </td>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel2" runat="server" Text="Currency"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblCurrencyId" runat="server" Text='<% #DataBinder.Eval(Container, "DataItem.FLDCURRENCYID") %>' Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCurrencyCode" runat="server" Text='<% #DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE") %>'></telerik:RadLabel>
                                </td>

                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel3" runat="server" Text="Amount"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblAmount" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                                </td>

                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel8" runat="server" Text="GRN Status"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblGrnStatus" Text='<%#  DataBinder.Eval(Container,"DataItem.GRNSTATUS") %>'></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel5" runat="server" Text="Invoice No"></telerik:RadLabel>

                                </td>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblInvoiceNumber" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICENUMBER") %>'></telerik:RadLabel>
                                </td>

                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel6" runat="server" Text="Invoice Supplier No"></telerik:RadLabel>

                                </td>
                                <td>
                                    <telerik:RadLabel runat="server" ID="lblSupplierInvoiceNumber" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVOICESUPPLIERREFERENCE") %>'></telerik:RadLabel>

                                </td>

                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel7" runat="server" Text="Credit Note Register No"></telerik:RadLabel>

                                </td>
                                <td>

                                    <telerik:RadLabel runat="server" ID="lblCreditNoteRegisterNo" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDCNREGISTERNO") %>'></telerik:RadLabel>

                                </td>

                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel9" runat="server" Text="Status"></telerik:RadLabel>

                                </td>

                                <td>
                                    <telerik:RadLabel runat="server" ID="lblReturnStatus" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td style="font-weight: 700">
                                    <telerik:RadLabel ID="RadLabel4" runat="server" Text="Comment"></telerik:RadLabel>
                                </td>
                                <td colspan="7">
                                    <telerik:RadLabel runat="server" ID="lblComment" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDCOMMENT") %>'></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NestedViewTemplate>



                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder">

                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>

            </telerik:RadGrid>
            <asp:Button ID="ucConfirmDelete" runat="server" OnClick="ucConfirmDelete_Click" CssClass="hidden" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
