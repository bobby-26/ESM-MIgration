<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewUnallocatedVesselExpensesEmployee.aspx.cs"
    Inherits="CrewUnallocatedVesselExpensesEmployee" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Unallocated Vessel Expenses</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }

        .checkRtl {
            direction: rtl;
        }

        .fon {
            font-size: small !important;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="CrewExpenseTab" runat="server" Title="Unallocated Vessel Expenses"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%" CssClass="scrolpan ">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFirstName" ReadOnly="True" Enabled="false" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" ReadOnly="True" Enabled="false" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLastName" ReadOnly="True" Enabled="false" Width="180px"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" ReadOnly="True" Enabled="false" Width="180px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRank" ReadOnly="True" Enabled="false" Width="180px"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuExpense" runat="server" OnTabStripCommand="MenuExpense_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvExpense" runat="server" EnableViewState="false" Height="76%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvExpense_NeedDataSource" OnItemCommand="gvExpense_ItemCommand" EnableHeaderContextMenu="true" ShowFooter="false" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Vessel" Name="Vessel" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Voucher" Name="Voucher" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Expense Type" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpenseType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPENSETYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO No." AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPONo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPONUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Ordered On" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSignOnDate" runat="server" Text='<%#General.GetDateTimeToString( DataBinder.Eval(Container,"DataItem.FLDORDERDATE","{0:dd/MMM/yyyy}")) %>'></telerik:RadLabel>
                                &nbsp;
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text='by'></telerik:RadLabel>
                                &nbsp; 
                                 <telerik:RadLabel ID="lblOrderedUser" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOERDEREDUSER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount(USD)" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="7%">
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n2}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Payment Mode" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPaymentMode" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPAYMENTMODENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Joined" ColumnGroupName="Vessel" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblJoinedVessel" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDJOINEDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tentative" ColumnGroupName="Vessel" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTentativeVessel" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDTENTATIVEVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Charged" ColumnGroupName="Vessel" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChargedVessel" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCHARGEDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expense" ColumnGroupName="Voucher" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpenseVoucher" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDEXPENSESVOUCHER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Charging" ColumnGroupName="Voucher" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblChargingVoucher" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCHARGINGVOUCHER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSTATUSNAME") %>'></telerik:RadLabel>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
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
