<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseZonePortMapping.aspx.cs" Inherits="PurchaseZonePortMapping" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VendorZone" Src="~/UserControls/UserControlVendorZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="SeaPort" Src="~/UserControls/UserControlSeaport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressList" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Contract Zone Mapping</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var browserHeight = $telerik.$(window).height();
            var grid = $find("rgvPort");
            grid._gridDataDiv.style.height = (browserHeight - 220) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmRegistersZonePortMapping" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxManager ID="RadAjaxManager" runat="server" UpdateInitiatorPanelsOnly="true">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="rgvPort">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="rgvPort" />
                        <telerik:AjaxUpdatedControl ControlID="ucError" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>

        </telerik:RadAjaxManager>


        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
         <telerik:RadNotification ID="ucStatus" RenderMode="Lightweight" runat="server" AutoCloseDelay="3500" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"  
            EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="TopCenter" Animation="Fade" ShowTitleMenu="false" ></telerik:RadNotification>
        <eluc:TabStrip ID="MenuPurchaseVendor" runat="server" OnTabStripCommand="MenuPurchaseVendor_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuPurchaseVendorZoneMain" runat="server" OnTabStripCommand="MenuPurchaseVendorZoneMain_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuPurchaseZonePortMapping" runat="server" OnTabStripCommand="PurchaseZonePortMapping_TabStripCommand">
                    </eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="rgvPort" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" 
            CellSpacing="0" GridLines="None" OnNeedDataSource="rgvPort_NeedDataSource" OnItemDataBound="rgvPort_ItemDataBound" OnSortCommand="rgvPort_SortCommand" 
            OnItemCommand="rgvPort_ItemCommand" ShowFooter="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDVENDORZONEPORTID,FLDSEAPORTID,FLDCOUNTRYCODE" TableLayout="Fixed">
                <Columns>
                     <telerik:GridTemplateColumn HeaderText="Country" UniqueName="COUNTRY">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                <HeaderStyle Width="20%" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblCountryCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <eluc:Country ID="ucCountry" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="100%" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Sea Port" UniqueName="PORT" AllowSorting="true" SortExpression="FLDSEAPORT">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                <HeaderStyle Width="20%" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblSeaPortName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>' ></telerik:RadLabel>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <eluc:SeaPort ID="ucSeaPort" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="100%"/>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Key Port" UniqueName="KEYPORT">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                <HeaderStyle Width="20%" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISKEYPORTDESC") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadCheckBox RenderMode="Lightweight" ID="chkKeyPortEdit" runat="server" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadCheckBox RenderMode="Lightweight" ID="chkKeyPortAdd" runat="server" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Port Group" UniqueName="GROUP">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="20%"></ItemStyle>
                                <HeaderStyle Width="20%" HorizontalAlign="Left" />
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTGROUP") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtPortGroupEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPORTGROUP") %>' Width="100%"></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox RenderMode="Lightweight" ID="txtPortGroupAdd" runat="server" Width="100%"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ACTION">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.ItemIndex%>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.ItemIndex%>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="Update" CommandArgument="<%# Container.ItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument="<%# Container.ItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                                <FooterStyle HorizontalAlign="Center" />
                                <FooterTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                        CommandName="Add" CommandArgument="<%# Container.ItemIndex %>" ID="cmdAdd"
                                        ToolTip="Add New"></asp:ImageButton>
                                </FooterTemplate>
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
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" EnablePostBackOnRowClick="false" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="4" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                <%--<ClientEvents OnGridCreated="GridCreated" />--%>
            </ClientSettings>
        </telerik:RadGrid>
    </form>
</body>
</html>
