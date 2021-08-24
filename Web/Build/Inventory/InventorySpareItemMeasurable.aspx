<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InventorySpareItemMeasurable.aspx.cs"
    Inherits="InventorySpareItemMeasurable" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlUnit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Spare Item Measurable List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvSpareItem.ClientID %>"));
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
    <form id="frmSpareItemList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="gvSpareItem">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="gvSpareItem" LoadingPanelID="RadAjaxLoadingPanel1" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server"></telerik:RadAjaxLoadingPanel>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:Status ID="status" runat="server" />
        <table width="100%">
            <tr>
                <td width="10%">
                    <asp:Literal ID="lblUnit" runat="server" Text="Unit"></asp:Literal>
                </td>
                <td>
                    <eluc:UserControlUnit ID="ddlunit" runat="server" CssClass="input" Enabled="false"
                        AppendDataBoundItems="true" />
                </td>
            </tr>
        </table>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvSpareItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvSpareItem_NeedDataSource" Height="100%"
            OnItemDataBound="gvSpareItem_ItemDataBound"
            OnItemCommand="gvSpareItem_ItemCommand"
            OnUpdateCommand="gvSpareItem_UpdateCommand"
            OnDeleteCommand="gvSpareItem_DeleteCommand">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDMEASURABLEID" ShowFooter="true">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Unit" HeaderStyle-Width="115px" AllowSorting="true" SortExpression="FLDUNITNAME">
                        <ItemTemplate>
                            <asp:Label ID="lblunit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></asp:Label>
                            <asp:Label ID="lblMeasurableid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEASURABLEID") %>'></asp:Label>
                        </ItemTemplate>
                        <FooterTemplate>
                            <eluc:UserControlUnit ID="ddlSpareUnit" runat="server" AppendDataBoundItems="true"
                                CssClass="input_mandatory" UnitList="<%# PhoenixRegistersUnit.ListUnit() %>" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Conversion Factor" HeaderStyle-Width="70px" AllowSorting="true">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblConversionFactor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONVERSIONFACTOR") %>'></telerik:RadLabel>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <eluc:Number ID="txtConversionFactoredit" runat="server" CssClass="input_mandatory"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONVERSIONFACTOR") %>' />
                        </EditItemTemplate>
                        <FooterTemplate>
                            <eluc:Number ID="txtConversionFactorAdd" runat="server" MaxLength="9" CssClass="input_mandatory" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                        <HeaderStyle Width="40px" />
                        <ItemStyle Width="40px" Wrap="false" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                <span class="icon"><i class="fas fa-trash-alt"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton runat="server" CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                <span class="icon"><i class="fas fa-times"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>
                        <FooterTemplate>
                            <asp:LinkButton runat="server" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                <span class="icon"><i class="fas fa-plus-square"></i></span>
                            </asp:LinkButton>
                        </FooterTemplate>
                        <FooterStyle HorizontalAlign="Center" />
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="true"
                    PageSizeLabelText="Records per page:" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
