<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseVendorProductZone.aspx.cs" Inherits="PurchaseVendorProductZone" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Purchase" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VendorZone" Src="~/UserControls/UserControlVendorZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Seaport" Src="~/UserControls/UserControlSeaport.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vendor Product Zone</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {

                TelerikGridResize($find("<%= gvVendorProductZone.ClientID %>"));
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
                fade('statusmessage');
            }
            function rowClick(sender, eventArgs) {
                sender.get_masterTableView().editItem(eventArgs.get_itemIndexHierarchical());
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersVendorProductZone" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlOrderForm" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlVendorProductZone">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuContractItem" Title="Vendor Items" runat="server" OnTabStripCommand="MenuContractItem_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox runat="server" ID="ucVendorZone" AutoPostBack="true" CssClass="input_mandatory"
                            OnSelectedIndexChanged="VendorZone_Changed"
                            DataTextField="FLDZONECODE" DataValueField="FLDVENDORZONEID">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>

                    </td>
                    <td>
                        <telerik:RadComboBox ID="ucPort" runat="server" DataTextField="FLDSEAPORTNAME" DataValueField="FLDSEAPORTID" CssClass="input"
                            OnDataBound="ucSeaport_DataBound" AutoPostBack="true" OnSelectedIndexChanged="ucPort_SelectedIndexChanged">
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblPackage" Text="Package"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard runat="server" ID="ucPackage" AppendDataBoundItems="true" AutoPostBack="true" CssClass="input" HardTypeCode="193" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuRegistersVendorProductZone" runat="server" OnTabStripCommand="RegistersVendorProductZone_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid ID="gvVendorProductZone" runat="server" AutoGenerateColumns="False"
                Font-Size="11px" Width="100%" CellPadding="3" OnItemCommand="gvVendorProductZone_RowCommand"
                OnItemDataBound="gvVendorProductZone_ItemDataBound" OnNeedDataSource="gvVendorProductZone_NeedDataSource"
                OnUpdateCommand="gvVendorProductZone_RowUpdating" AllowPaging="true" AllowCustomPaging="true"
                ShowHeader="true" AllowSorting="true" EnableViewState="false"
                 OnSortCommand="gvVendorProductZone_Sorting">
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDZONEPRODUCTMAPPINGID">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Product Code" SortExpression="FLDPRODUCTCODE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblZoneProductMappingId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONEPRODUCTMAPPINGID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblProductCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTCODE") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkDescription" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItem %>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTCODE") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblVendorProductIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORPRODUCTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblZoneProductMappingIdEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONEPRODUCTMAPPINGID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblProductCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTCODE") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Product Name" SortExpression="FLDPRODUCTNAME">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProductName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblProductNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTNAME") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Product Group" SortExpression="FLDPRODUCTGROUP">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblProductGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTGROUP") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblProductGroupEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTGROUP") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Description">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDescriptionEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Unit">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblUnitEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Price" SortExpression="FLDPRICE">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRICE","{0:n2}") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucPriceEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRICE","{0:n2}") %>' Mask="99.99" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Discount" SortExpression="FLDDISCOUNT">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDiscount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucDiscountEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>' CssClass="input" DefaultZero="true" Mask="99.99" />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument="<%# Container.DataItem %>" ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataItem %>" ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Update" CommandArgument="<%# Container.DataItem %>" ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument="<%# Container.DataItem %>" ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="Add" CommandArgument="<%# Container.DataItem %>" ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" EnablePostBackOnRowClick="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    <ClientEvents OnRowDblClick="rowClick" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
