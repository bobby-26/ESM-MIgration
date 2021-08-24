<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseContractPriceList.aspx.cs" Inherits="PurchaseContractPriceList" EnableEventValidation="false" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
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

    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function PaneResized(sender, args) {
            var browserHeight = $telerik.$(window).height();
            var grid = $find("rgvLine");           
            grid._gridDataDiv.style.height = (browserHeight - 180) + "px";
        }
        function pageLoad() {
            PaneResized();
        }
    </script>
</head>
<body onresize="PaneResized()" onload="PaneResized()">
    <form id="frmRegistersVendorProductZone" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <telerik:RadNotification ID="ucStatus" RenderMode="Lightweight" runat="server" AutoCloseDelay="3500" ShowCloseButton="false" Title="Status" TitleIcon="none" ContentIcon="none"  
            EnableRoundedCorners="true" Height="80px" Width="300px" OffsetY="30" Position="TopCenter" Animation="Fade" ShowTitleMenu="false" ></telerik:RadNotification>
        <eluc:TabStrip ID="MenuContractItem" runat="server" OnTabStripCommand="MenuContractItem_TabStripCommand"></eluc:TabStrip>
        <table width="100%">
                         <tr>
                            <td>
                               <telerik:RadLabel RenderMode="Lightweight" ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox RenderMode="Lightweight" runat="server" ID="ucVendorZone" AutoPostBack="true" 
                                     OnSelectedIndexChanged="VendorZone_Changed" OnDataBound="ucVendorZone_DataBound" EnableLoadOnDemand="true"
                                     DataTextField="FLDZONECODE" DataValueField="FLDVENDORZONEID" >
                                </telerik:RadComboBox>
                            </td>
                            <td>
                               <telerik:RadLabel RenderMode="Lightweight" ID="lblPort" runat="server" Text="Port"></telerik:RadLabel>
                            </td>
                            <td> 
                                <telerik:RadComboBox RenderMode="Lightweight" ID="ucPort" runat="server" DataTextField="FLDSEAPORTNAME" DataValueField="FLDSEAPORTID"  
                                    OnDataBound="ucSeaport_DataBound" AutoPostBack="true" EnableLoadOnDemand="true" OnSelectedIndexChanged="ucPort_SelectedIndexChanged" >
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadLabel RenderMode="Lightweight" runat="server" ID="lblPackage" Text="Package"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Hard runat="server" ID="ucPackage" AppendDataBoundItems="true" AutoPostBack="true" CssClass="input" HardTypeCode="193" OnTextChangedEvent="ucPackage_TextChangedEvent" />
                            </td>
                        </tr>
                    </table>
        <eluc:TabStrip ID="MenuRegistersVendorProductZone" runat="server" OnTabStripCommand="RegistersVendorProductZone_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="FormDecorator"  runat="server" DecorationZoneID="rgvLine" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadGrid RenderMode="Lightweight" ID="rgvLine" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="rgvLine_NeedDataSource" OnItemDataBound="rgvLine_ItemDataBound" OnSortCommand="rgvLine_SortCommand" OnItemCommand="rgvLine_ItemCommand" ShowFooter="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" DataKeyNames="FLDZONEPRODUCTMAPPINGID,FLDQUOTATIONLINEID" TableLayout="Fixed">
                <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Product Code" UniqueName="PRODUCTCODE" AllowSorting="true" SortExpression="FLDPRODUCTCODE">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblProductCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTCODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lnkDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Product Name" UniqueName="NAME" AllowSorting="true" SortExpression="FLDPRODUCTNAME">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblProductName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRODUCTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Zone" UniqueName="ZONE">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblZoneName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDZONECODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Port" UniqueName="PORT">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblPortName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAPORTNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Contract Unit" UniqueName="UNIT">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Price" UniqueName="PRICE">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRICE","{0:n2}") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Discount" UniqueName="DISCOUNT">
                                <ItemTemplate>
                                    <telerik:RadLabel RenderMode="Lightweight" ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDISCOUNT","{0:n2}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            
                            <telerik:GridTemplateColumn HeaderText="Action" UniqueName="ACTION">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Apply Price" ImageUrl="<%$ PhoenixTheme:images/applied-price.png %>"
                                        CommandName="SAVE" CommandArgument="<%# Container.ItemIndex %>" ID="cmdSave"
                                        ToolTip="Apply Price"></asp:ImageButton>
                                </ItemTemplate>
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
