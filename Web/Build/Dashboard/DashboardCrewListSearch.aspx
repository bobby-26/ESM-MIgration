<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardCrewListSearch.aspx.cs" Inherits="DashboardCrewListSearch" %>

<!DOCTYPE html >
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Query Activity</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">

        <telerik:RadScriptManager ID="RadScriptManager1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="skinmanager1" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />
            <telerik:RadGrid RenderMode="LightWeight" GridLines="None" ID="gvCrewSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" Height="100%" CellPadding="3" OnItemCommand="gvCrewSearch_ItemCommand" OnItemDataBound="gvCrewSearch_ItemDataBound"
                ShowHeader="true" ShowFooter="true" EnableViewState="true" AllowSorting="true" OnSortCommand="gvCrewSearch_SortCommand" AllowPaging="true"
                OnNeedDataSource="gvCrewSearch_NeedDataSource" EnableHeaderContextMenu="true" GroupingEnabled="false" AllowCustomPaging="true"
                CellSpacing="0" AllowFilteringByColumn="true" EnableLinqExpressions="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" SortExpression="FLDFIRSTNAME"
                            DataField="FLDFIRSTNAME" AllowFiltering="true" UniqueName="FLDFIRSTNAME" FilterControlWidth="120px" FilterDelay="2000"
                            AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lnkEployeeName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME").ToString()+ " "+ DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME").ToString()+" "+ DataBinder.Eval(Container,"DataItem.FLDLASTNAME").ToString()%>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkName" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME").ToString()+ " "+ DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME").ToString()+" "+ DataBinder.Eval(Container,"DataItem.FLDLASTNAME").ToString()%>' CommandName="GETEMPLOYEE"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="true" SortExpression="FLDRANKPOSTEDNAME"
                            DataField="FLDRANKPOSTEDNAME" AllowFiltering="true" UniqueName="FLDRANKPOSTEDNAME" FilterControlWidth="120px" FilterDelay="2000"
                            AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="EqualTo" >
                            <HeaderStyle Width="35%" />
                             <FilterTemplate>
                                <telerik:RadComboBox ID="ddlRank" OnDataBinding="ddlRank_DataBinding" AppendDataBoundItems="true" Width="100%" EmptyMessage="Type to select" Filter="Contains"
                                    SelectedValue='<%# ViewState["RANK"].ToString() %>' OnClientSelectedIndexChanged="RankIndexChanged" runat="server">
                                </telerik:RadComboBox>
                                <telerik:RadScriptBlock runat="server">
                                    <script type="text/javascript">
                                        function RankIndexChanged(sender, args) {
                                            var tableView = $find("<%# ((GridItem)Container).OwnerTableView.ClientID %>");
                                            tableView.filter("FLDRANKPOSTEDNAME", args.get_item().get_value(), "EqualTo");
                                        }
                                    </script>
                                </telerik:RadScriptBlock>
                            </FilterTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAppliedRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKPOSTEDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No" AllowSorting="true" SortExpression="FLDEMPLOYEECODE"
                             DataField="FLDEMPLOYEECODE" AllowFiltering="true" UniqueName="FLDEMPLOYEECODE" FilterControlWidth="120px" FilterDelay="2000"
                            AutoPostBackOnFilter="false" ShowFilterIcon="false" CurrentFilterFunction="Contains">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last Vessel" AllowSorting="true" SortExpression="FLDLASTVESSELNAME" AllowFiltering="false">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLASTVESSELNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-Off Date" AllowSorting="true" SortExpression="FLDLASTSIGNOFFDATE" AllowFiltering="false">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLASTSIGNOFFDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Present Vessel" AllowSorting="true" SortExpression="FLDPRESENTVESSELNAME" AllowFiltering="false">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPRESENTVESSELNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign-On Date" AllowSorting="true" SortExpression="FLDPRESENTSIGNONDATE" AllowFiltering="false">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPRESENTSIGNONDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="DOA" AllowSorting="true" SortExpression="FLDDOA" AllowFiltering="false">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDOA", "{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="true" SortExpression="FLDSTATUS" AllowFiltering="false">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="false" HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSTATUS") + "/" + DataBinder.Eval(Container, "DataItem.FLDSTATUSNAME")%>'
                                    ToolTip='<%#DataBinder.Eval(Container, "DataItem.FLDSTATUSDESCRIPTION")%>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Zone" AllowSorting="true" SortExpression="FLDZONE" AllowFiltering="false">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDZONE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Batch No" AllowSorting="true" SortExpression="FLDBATCHNO" AllowFiltering="false">
                            <HeaderStyle Width="35%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" Text=' <%# DataBinder.Eval(Container, "DataItem.FLDBATCHNO")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                     <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="415px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
