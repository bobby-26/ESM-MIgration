<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnerVarianceReport.aspx.cs"
    Inherits="OwnerVarianceReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselAccount" Src="~/UserControls/UserControlUserVesselAccount.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Variance Report</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmOwnersAccounts" DecoratedControls="All" />
    <form id="frmOwnersAccounts" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <table width="100%" cellpadding="1" cellspacing="1" style="z-index: 2;">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel Account"></telerik:RadLabel>
                </td>
                <td>
                    <%-- <telerik:RadDropDownList ID="ddlVesselAccount" runat="server" Width="240px" AppendDataBoundItems="true">
                        </telerik:RadDropDownList>--%>
                    <eluc:VesselAccount ID="ucVesselAccount" runat="server" AddressType="128" CssClass="input" AutoPostBack="true"
                        AppendDataBoundItems="true" Width="240px" />
                </td>
                <td>
                    <telerik:RadLabel ID="lblyear1" runat="server" Text="Year"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Year ID="ucYear" runat="server" CssClass="input" AppendDataBoundItems="true"
                        QuickTypeCode="55" Width="200px" />
                </td>

                <td>
                    <telerik:RadLabel ID="lblmonth1" runat="server" Text="Month"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Month ID="ucMonth" runat="server" AppendDataBoundItems="true" CssClass="input"
                        HardTypeCode="55" SortByShortName="true" />
                </td>
            </tr>
        </table>
        <eluc:TabStrip ID="MenuAccountsowner" runat="server" OnTabStripCommand="AccountsownerMenu_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvOwnersAccount" Height="90%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None"
            OnItemCommand="gvOwnersAccount_ItemCommand" OnItemDataBound="gvOwnersAccount_ItemDataBound" EnableViewState="false"
            ShowFooter="false" ShowHeader="true" OnNeedDataSource="gvOwnersAccount_NeedDataSource" GroupingEnabled="false" EnableHeaderContextMenu="true"
            AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" InsertItemPageIndexAction="ShowItemOnCurrentPage" TableLayout="Fixed">
                <HeaderStyle Width="102px" />
                <ColumnGroups>
                    <telerik:GridColumnGroup HeaderText="Variance" Name="Variance"></telerik:GridColumnGroup>
                </ColumnGroups>
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Account">
                        <HeaderStyle Width="15%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblOwnerId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblAccountName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblASonDate" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASONDATE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="SOA Reference">
                        <HeaderStyle Width="10%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblAccountCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblSoaReference" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblSoaReferenceID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBINOTEREFERENCEID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lnkSoaReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Month">
                        <HeaderStyle Width="10%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblMonthId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMONTH") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Year">
                        <HeaderStyle Width="10%" />
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblYearId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDYEAR") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Monthly" UniqueName="MonthlyVariance" ColumnGroupName="Variance">
                        <HeaderStyle Width="10%"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="MonthlyVariance" Text="Monthly" CommandName="MONTHLYVARIANCE"
                                ID="cmdMonthlyVariance" ToolTip="Monthly Variance">
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Yearly" UniqueName="YearlyVariance" ColumnGroupName="Variance">
                        <HeaderStyle Width="10%"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Yearly Variance" Text="Yearly" CommandName="YEARLYVARIANCE"
                                ID="cmdYearlyVariance" ToolTip="Yearly Variance">
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Accumulated" UniqueName="AccumulatedVariance" ColumnGroupName="Variance">
                        <HeaderStyle Width="10%"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Accumulated Variance" Text="Accumulated" CommandName="ACCUMALTEDVARIANCE" ID="cmdAccumaltedVariance" ToolTip="Accumulated Variance">
                            </asp:LinkButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="YTD Details" UniqueName="YTD">
                        <HeaderStyle Width="10%"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="YTD Details" Text="YTD Details" CommandName="YTDDETAILS"
                                ID="cmdYTDDetails" ToolTip="YTD Details" Visible="false" CommandArgument="<%# Container.DataSetIndex %>">
                            </asp:LinkButton>

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
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

    </form>
</body>
</html>
