<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceReportSpareStoreRequisition.aspx.cs"
    Inherits="PlannedMaintenanceReportSpareStoreRequisition" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="../UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Requisition</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
         <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRequisition" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
            <%-- <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">--%>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
             
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFormNumberFrom" runat="server" Text="Form Number From"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFormNoFrom" CssClass="input" MaxLength="20"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFormNumberTo" runat="server" Text="Form Number To"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFormNoTo" CssClass="input" MaxLength="20"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFormTitle" runat="server" Text="Form Title"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtNameTitle" CssClass="input" MaxLength="200" Width="240px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVendor" runat="server" Text="Vendor"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListVendor">
                            <telerik:RadTextBox ID="txtVendorNumber" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVenderName" runat="server" Width="180px" CssClass="input"></telerik:RadTextBox>
                            <asp:ImageButton runat="server" ID="cmdShowMaker" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="1px" CssClass="input"></telerik:RadTextBox>
                        </span>&nbsp;
                            <asp:ImageButton ID="cmdClear" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                                ImageAlign="AbsMiddle" Text=".." OnClick="cmdVendorClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDeliverTo" runat="server" Text="Deliver To"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnDLocation">
                            <telerik:RadTextBox ID="txtDeliveryLocationCode" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtDeliveryLocationName" runat="server" Width="180px" CssClass="input"></telerik:RadTextBox>
                            <asp:ImageButton ID="cmdShowLocation" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtDeliveryLocationId" runat="server" Width="0px" CssClass="input"></telerik:RadTextBox>
                        </span>&nbsp;
                            <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                                ImageAlign="AbsMiddle" Text=".." OnClick="cmdLocationClear_Click" />
                    </td>
                    <td>
                        <asp:Literal ID="lblBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                    </td>
                    <td>
                        <span id="spnPickListMainBudget">
                            <telerik:RadTextBox ID="txtBudgetCode" runat="server" Width="60px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetName" runat="server" Width="180px" CssClass="input"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowBudget" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtBudgetId" runat="server" Width="1px" CssClass="input"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtBudgetgroupId" runat="server" Width="1px" CssClass="input"></telerik:RadTextBox>
                        </span>&nbsp;
                            <asp:ImageButton ID="ImageButton2" runat="server" ImageUrl="<%$ PhoenixTheme:images/clear.png %>"
                                ImageAlign="AbsMiddle" Text=".." OnClick="cmdBudgetClear_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFinancialYear" runat="server" Text="Financial Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFinanceYear" CssClass="input" MaxLength="4"></telerik:RadTextBox>
                        <%--  <ajaxtoolkit:maskededitextender id="MaskedEditExtender6" runat="server" targetcontrolid="txtFinanceYear"
                                    mask="9999" masktype="None" inputdirection="LeftToRight" clearmaskonlostfocus="false">
                            </ajaxtoolkit:maskededitextender>--%>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFormStatus" runat="server" Text="Form Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Hard ID="ucFormStatus" AppendDataBoundItems="true" CssClass="input" runat="server" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenugvFormDetails" runat="server" OnTabStripCommand="gvFormDetails_TabStripCommand"></eluc:TabStrip>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="RadGrid1" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="RadGrid1_NeedDataSource" OnPreRender="RadGrid1_PreRender"
                OnItemDataBound="RadGrid1_ItemDataBound" OnItemCommand="RadGrid1_ItemCommand" AutoGenerateColumns="false">

                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">

                    <HeaderStyle Width="102px" />
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Number" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDFORMNO">
                            <HeaderStyle Width="68px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNumber" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbldtkey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="Label1" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMSTATUS") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkFormNumberName" runat="server" CommandName="Report" ToolTip="Generate Report"
                                    CommandArgument='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>' Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMNO") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Form Title" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDTITLE">
                            <HeaderStyle Width="210px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStockItemCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vendor" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDVENDORNAME">
                            <HeaderStyle Width="210px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblVendorName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVENDORNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Form Type" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDFORMTYPENAME">
                            <HeaderStyle Width="60px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblPrefferedVendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMTYPENAME") %>'></asp:Label>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Form Status" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDFORMSTATUSNAME">
                            <HeaderStyle Width="58px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblWanted" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMSTATUSNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Budget Code" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDSUBACCOUNT">
                            <HeaderStyle Width="64px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type">
                            <HeaderStyle Width="55px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStockType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Component Class / Store Type">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKCLASS") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblStockId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKCLASSID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRecivedDate" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <%--<Virtualization EnableVirtualization="true" InitiallyCachedItemsCount="100" LoadingPanelID="RadAjaxLoadingPanel1" ItemsPerView="100" />--%>
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="2" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
                <%-- <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>--%>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
