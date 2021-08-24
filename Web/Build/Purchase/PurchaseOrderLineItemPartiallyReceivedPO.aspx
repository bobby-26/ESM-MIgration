<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseOrderLineItemPartiallyReceivedPO.aspx.cs" Inherits="PurchaseOrderLineItemPartiallyReceivedPO" %>

<!DOCTYPE html>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Partially Received PO</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvPartiallyReceivedPO.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
                Resize();
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>   
    <form id="form1" runat="server">
    <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
    <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Status ID="ucStatus" runat="server" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%">
                <tr>
                    <td width="10%">
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td width="23.33%">
                        <eluc:Vessel ID="UcVesselSearch" runat="server" VesselsOnly="true" Width="100%" AppendDataBoundItems="true" />
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lblTitle" runat="server" Text="PR Description"></telerik:RadLabel>
                    </td>
                    <td width="23.33%">
                        <telerik:RadTextBox ID="txtTitle" runat="server" Width="60%"></telerik:RadTextBox>
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lbl" runat="server" Text="Reqn No."></telerik:RadLabel>
                    </td>
                    <td width="23.33%">
                        <telerik:RadTextBox ID="txtFormNo" runat="server" Width="60%"></telerik:RadTextBox>
                    </td>
                    </tr>
                <tr>
                    <td width="10%">
                        <telerik:RadLabel ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                    </td>
                    <td width="23.33%">
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtVendorNumber" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtVenderName" runat="server" Width="160px" CssClass="input"></telerik:RadTextBox>
                            <asp:LinkButton ID="cmdShowMaker" runat="server" ImageAlign="AbsMiddle" Text=".." OnClientClick="return showPickList('spnPickListMaker', 'codehelp1', '', 'Common/CommonPickListAddressOwner.aspx?addresstype=130,131,132&framename=ifMoreInfo', true);">
                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                            <telerik:RadTextBox RenderMode="Lightweight" ID="txtVendor" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                    </td>
                
                    <td width="10%">
                        <telerik:RadLabel ID="lblFromDate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td width="23.33%">
                        <eluc:Date ID="ucFromDate" runat="server" DatePicker="true" Width="60%" />
                    </td>
                    <td width="10%">
                        <telerik:RadLabel ID="lblToDate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td width="23.33%">
                        <eluc:Date ID="ucToDate" runat="server" DatePicker="true" Width="60%" />
                    </td>
                </tr>
                <tr>
                    <td width="10%">
                        <telerik:RadLabel ID="lblSpare" runat="server" Text="Stock Type"></telerik:RadLabel>
                    </td>
                    <td width="23.33%">
                        <telerik:RadDropDownList runat="server" ID="ddlStockType" Width="60%">
                            <Items>
                                <%--<telerik:DropDownListItem Text="--Select--" Value="Dummy" Selected="true" />--%>
                                <telerik:DropDownListItem Text="Spares" Value="SPARE" />
                                <telerik:DropDownListItem Text="Stores" Value="STORE" />
                                <telerik:DropDownListItem Text="Service" Value="SERVICE" />
                            </Items>
                        </telerik:RadDropDownList>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuList" runat="server" OnTabStripCommand="MenuList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPartiallyReceivedPO" runat="server" AutoGenerateColumns="False" Width="100%" CellSpacing="0" GridLines="None"
                EnableHeaderContextMenu="true" GroupingEnabled="false" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnItemCommand="gvPartiallyReceivedPO_ItemCommand"
                CellPadding="3" ShowHeader="true" ShowFooter="false" EnableViewState="false" OnNeedDataSource="gvPartiallyReceivedPO_NeedDataSource"
                OnSortCommand="gvPartiallyReceivedPO_SortCommand" MasterTableView-AllowCustomSorting="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name" AllowSorting="true" SortExpression="FLDVESSELNAME">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reqn No." AllowSorting="true" SortExpression="FLDFORMNO">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReqNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Priority">
                            <HeaderStyle Width="6%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPriority" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRIORITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PR Description">
                            <HeaderStyle Width="14%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status">
                            <HeaderStyle Width="7%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vendor" AllowSorting="true" SortExpression="FLDVENDORNAME">
                            <HeaderStyle Width="15%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vendor Currency Code">
                            <HeaderStyle Width="8%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCurrencyCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCURRENCYNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Total Cost In Vendor Currency">
                            <HeaderStyle Width="10%" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCostInVendorCur" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORECURTOTALCOST") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO Amount">
                            <HeaderStyle Width="10%" />
                            <ItemStyle HorizontalAlign="Right" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPOAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOCOMMITTEDAMOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PR Creation Date" AllowSorting="true" SortExpression="FLDCREATEDDATE">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCreationDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREATEDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO Date" AllowSorting="true" SortExpression="FLDORDEREDDATE">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurchaseOrderDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASEORDERDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Readiness Date" AllowSorting="true" SortExpression="FLDORDERREADINESSDATE">
                            <HeaderStyle Width="10%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReadinessDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERREADINESSDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
