<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewQueryActivity.aspx.cs"
    Inherits="CrewQueryActivity" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Query Activity</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function RowContextMenu(sender, eventArgs) {
                var menu = $find("<%=RadMenu1.ClientID %>");
                var evt = eventArgs.get_domEvent();

                if (evt.target.tagName == "INPUT" || evt.target.tagName == "A") {
                    return;
                }

                var index = eventArgs.get_itemIndexHierarchical();
                document.getElementById("radGridClickedRowIndex").value = index;

                sender.get_masterTableView().selectItem(sender.get_masterTableView().get_dataItems()[index].get_element(), true);

                menu.show(evt);

                evt.cancelBubble = true;
                evt.returnValue = false;

                if (evt.stopPropagation) {
                    evt.stopPropagation();
                    evt.preventDefault();
                }
            }
        </script>
        <script type="text/javascript">
            function Resize() {

                TelerikGridResize($find("<%= gvCrewSearch.ClientID %>"));
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
        <telerik:RadSkinManager runat="server" ID="RadSkinManager1"></telerik:RadSkinManager>
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:UserControlStatus ID="ucStatus" runat="server" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecorationZoneID="RadGrid1,table1" DecoratedControls="All" EnableRoundedCorners="true" />
            <eluc:TabStrip ID="CrewQueryMenu" runat="server" OnTabStripCommand="CrewQueryMenu_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSearch" Height="99%" runat="server" EnableViewState="true" OnSortCommand="gvCrewSearch_SortCommand"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvCrewSearch_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvCrewSearch_ItemDataBound"
                OnItemCommand="gvCrewSearch_ItemCommand" AutoGenerateColumns="false" AllowFilteringByColumn="true" EnableLinqGrouping="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="None" AllowMultiColumnSorting="true" AllowFilteringByColumn="true" EnableLinqGrouping="false">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <ColumnGroups>
                        <telerik:GridColumnGroup HeaderText="Vessel" Name="Vessel" HeaderStyle-HorizontalAlign="Center">
                        </telerik:GridColumnGroup>
                    </ColumnGroups>
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowExportToPdfButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Name / File No." AllowSorting="true" ShowSortIcon="true" SortExpression="FLDNAME" AllowFiltering="true">
                            <HeaderStyle Width="18%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbldirectsignon" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDIRECTSIGNON") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblfamlyid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFAMILYID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME").ToString()+ " "+ DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME").ToString()+" "+ DataBinder.Eval(Container,"DataItem.FLDLASTNAME").ToString() %>'
                                    CommandName="GETEMPLOYEE"></asp:LinkButton>
                                <br />
                                <telerik:RadLabel ID="lblEmployeeCode" runat="server" Text='<%# " / "+ DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE").ToString() %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FilterTemplate>
                                <telerik:RadComboBox ID="rcbEmployeename" runat="server" ShowToggleImage="false" EnableLoadOnDemand="true" MinFilterLength="3" EnableVirtualScrolling="true"
                                    OnItemsRequested="rcbEmployeename_ItemsRequested" AutoPostBack="true" OnSelectedIndexChanged="rcbEmployeename_SelectedIndexChanged" EmptyMessage="Type File no. to search"
                                    DataTextField="FLDEMPLOYEECODE" DataValueField="FLDEMPLOYEECODE" OnPreRender="rcbEmployeename_PreRender" Width="99%" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </FilterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="true" ShowSortIcon="true" AllowFiltering="false" SortExpression="FLDRANKPOSTEDNAME">
                            <HeaderStyle Width="5%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAppliedRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKPOSTEDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Last" ColumnGroupName="Vessel" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false">
                            <HeaderStyle Width="4.5%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblLastVesselName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLASTVESSELNAME")%>' CssClass="tooltip" ClientIDMode="AutoID"></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipLastVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTVESSEL") %>' TargetControlId="lblLastVesselName" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Present" ColumnGroupName="Vessel" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false">
                            <HeaderStyle Width="6%" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Left" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPresentVesselName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPRESENTVESSELNAME")%>' CssClass="tooltip" ClientIDMode="AutoID"></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipPresentVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESENTVESSEL") %>' TargetControlId="lblPresentVesselName" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Next" ColumnGroupName="Vessel" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false">
                            <HeaderStyle Width="4.5%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblNextVessel" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPLANNEDVESSELCODES")%>' CssClass="tooltip" ClientIDMode="AutoID"></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipVesselNames" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLANNEDVESSELNAMES") %>' TargetControlId="lblNextVessel" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign Off" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false">
                            <HeaderStyle Width="8.5%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDLASTSIGNOFFDATE", "{0:dd/MMM/yyyy}")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Sign On" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false">
                            <HeaderStyle Width="8.5%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDPRESENTSIGNONDATE", "{0:dd/MMM/yyyy}")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="D.O.A." AllowSorting="false" ShowSortIcon="true" AllowFiltering="false">
                            <HeaderStyle Width="8.5%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDDOA", "{0:dd/MMM/yyyy}")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false">
                            <HeaderStyle Width="10.5%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSTATUS") + "/" + DataBinder.Eval(Container, "DataItem.FLDSTATUSNAME")%>'
                                    CssClass="tooltip" ClientIDMode="AutoID">
                                </telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUSDESCRIPTION") %>' TargetControlId="lblStatus" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Zone" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false">
                            <HeaderStyle Width="5%" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDZONE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Batch" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false">
                            <HeaderStyle Width="7%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDBATCHNO")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Exp(M)" AllowSorting="false" ShowSortIcon="true" AllowFiltering="false">
                            <HeaderStyle Width="6%" HorizontalAlign="Center" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDDECIMALEXPERIENCE")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" UniqueName="Action" AllowFiltering="false">
                            <HeaderStyle Width="15%" HorizontalAlign="Center" />
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="GETEMPLOYEE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="PD Form"
                                    CommandName="PDFORM" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdPDForm"
                                    ToolTip="PD Form">
                                <span class="icon"><i class="fas fa-file-pr"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Suitability Check"
                                    CommandName="SUITABILITYCHECK" CommandArgument="<%# Container.DataSetIndex %>" ID="imgSuitableCheck"
                                    ToolTip="Suitability Check">
                                <span class="icon"><i class="fas  fa-user-astronaut"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Activities"
                                    CommandName="ACTIVITY" CommandArgument="<%# Container.DataSetIndex %>" ID="imgActivity"
                                    ToolTip="Activities">
                                <span class="icon"><i class="fa fa-pencil-ruler"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Send Mail"
                                    CommandName="SENDMAIL" CommandArgument="<%# Container.DataSetIndex %>" ID="imgSendMail"
                                    ToolTip="Send Mail">
                                <span class="icon"><i class="fa fa-envelope"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Unallocated Vessel Expense"
                                    CommandName="UNALLOCATEDVSLEXP" CommandArgument="<%# Container.DataSetIndex %>" ID="imgUnAllocatedVslExp"
                                    ToolTip="Unallocated Vessel Expense">
                                <span class="icon"> <i class="fa fa-coins-ei"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" CommandName="PHOENIXSYNCLOGIN"
                                    CommandArgument="<%# Container.DataSetIndex %>" ID="cmdServiceLogin"
                                    ToolTip="Phoenix Sync">
                                <span class="icon"> <i class="fa fa-share-square-24"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <ClientEvents OnRowContextMenu="RowContextMenu" />
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <input type="hidden" id="radGridClickedRowIndex" name="radGridClickedRowIndex" runat="server" />
            <telerik:RadContextMenu ID="RadMenu1" runat="server" OnItemClick="RadMenu1_ItemClick"
                EnableRoundedCorners="true" EnableShadows="true">
                <Items>
                    <telerik:RadMenuItem Text="Edit" Value="EDIT" ImageUrl="../css/Theme1/images/te_edit.png">
                    </telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="PD Form" Value="PD" ImageUrl="../css/Theme1/images/pr.png">
                    </telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="Suitability Check" Value="SUC" ImageUrl="../css/Theme1/images/crew-suitability-check.png">
                    </telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="Activities" Value="ACT" ImageUrl="../css/Theme1/images/72.png">
                    </telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="Send Mail" Value="MAIL" ImageUrl="../css/Theme1/images/Email.png">
                    </telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="Unallocated Vessel Expenses" Value="UVE" ImageUrl="../css/Theme1/images/edit-info.png">
                    </telerik:RadMenuItem>
                    <telerik:RadMenuItem Text="Phoenix Sync" Value="PHXSYNC" ImageUrl="../css/Theme1/images/24.png">
                    </telerik:RadMenuItem>
                </Items>
            </telerik:RadContextMenu>
        </telerik:RadAjaxPanel>

    </form>

</body>
</html>
