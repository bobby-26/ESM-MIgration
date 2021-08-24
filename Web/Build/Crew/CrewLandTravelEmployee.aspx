<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLandTravelEmployee.aspx.cs" Inherits="CrewLandTravelEmployee" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlRankList" Src="~/UserControls/UserControlRankList.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Land Travel</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSeafarersList" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuEmployeeList" runat="server" OnTabStripCommand="MenuEmployeeList_TabStripCommand"></eluc:TabStrip>
            <table>
                <tr>
                    <td>Passenger Type
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rblPassengerType" runat="server" RepeatDirection="Horizontal"
                            RepeatColumns="4" AutoPostBack="true" OnSelectedIndexChanged="rblPassengerType_Changed">
                            <asp:ListItem Text="Crew" Value="1" Selected="True"></asp:ListItem>
                            <asp:ListItem Text="Crew Family" Value="2"></asp:ListItem>
                            <asp:ListItem Text="Office Users" Value="3"></asp:ListItem>
                            <asp:ListItem Text="Freeform" Value="4"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>
            <hr />
            <table cellpadding="1" cellspacing="3" width="80%" id="EmployeeTable" runat="server">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFilenumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFileNo" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlRankList ID="lstRank" runat="server" AppendDataBoundItems="true" Width="100%" />
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvEmployee" runat="server" EnableViewState="false" Height="70%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvEmployee_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvEmployee_ItemDataBound"
                OnItemCommand="gvEmployee_ItemCommand" ShowFooter="false" AutoGenerateColumns="false" OnSortCommand="gvEmployee_SortCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Tick" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkCheck" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmpName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No." AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passport No." AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassportNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table cellpadding="1" cellspacing="3" width="80%" runat="server" id="EmployeeFamily">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmpFamilyName" runat="server" Text="Family Member Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmpFamilyName" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmpFilenumber" runat="server" Text="Employee File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmpFileNo" runat="server"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblEmpRank" runat="server" Text="Employee Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlRankList ID="lstEmpRank" runat="server" AppendDataBoundItems="true" Width="100%" />
                    </td>
                </tr>
            </table>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvEmployeeFamily" runat="server" EnableViewState="false" Height="70%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvEmployeeFamily_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvEmployeeFamily_ItemDataBound"
                OnItemCommand="gvEmployeeFamily_ItemCommand" ShowFooter="false" AutoGenerateColumns="false" OnSortCommand="gvEmployeeFamily_SortCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridColumnGroup HeaderText="Family Member" Name="FamilyMember" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                        <telerik:GridColumnGroup HeaderText="Employee" Name="Employee" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Tick" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkCheck" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" ColumnGroupName="FamilyMember" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmpMemberName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFamilyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passport No." ColumnGroupName="FamilyMember" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassportNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" ColumnGroupName="Employee" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmpName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" ColumnGroupName="Employee" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRankCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No." ColumnGroupName="Employee" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <table id="tblConfigureUser" width="100%" runat="server">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblUserName" runat="server" Text="User Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtSearch" runat="server" MaxLength="100" Width="360px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblFullName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFullName" MaxLength="100" Width="360px"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDepartment" runat="server" Text="Department"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Department runat="server" ID="ucDepartment" AutoPostBack="true"
                            AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvUser" runat="server" EnableViewState="false" Height="70%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvUser_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvUser_ItemDataBound"
                OnItemCommand="gvUser_ItemCommand" ShowFooter="false" AutoGenerateColumns="false" OnSortCommand="gvUser_SortCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Tick" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <telerik:RadCheckBox ID="chkCheckUser" runat="server" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Department" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUser" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Username" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDUSERNAME">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblUserCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERCODE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmail" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMAIL") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblUserName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="First Name" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Middle Name" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMiddleName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Name" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Designation" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDesignation" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table id="otheruser" runat="server">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPassengerNameHeader" runat="server" Text="Passenger Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtPassengerName" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblDesignationHeader" runat="server" Text="Designation"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDesignation" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
