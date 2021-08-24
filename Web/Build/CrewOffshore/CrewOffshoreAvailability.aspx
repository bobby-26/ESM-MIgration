<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreAvailability.aspx.cs"
    Inherits="CrewOffshoreAvailability" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Query Activity</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      <%--  <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrewSearch.ClientID %>"));
               }, -115);
           }
           window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
           }
        </script>--%>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
                <telerik:RadAjaxPanel ID="panel1" runat="server" Height="100%">

<%--        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>--%>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="CrewQueryMenu" runat="server" OnTabStripCommand="CrewQueryMenu_TabStripCommand"></eluc:TabStrip>


        <%--<asp:GridView ID="gvCrewSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvCrewSearch_RowCommand" OnRowDataBound="gvCrewSearch_RowDataBound"
                        ShowHeader="true" EnableViewState="false" AllowSorting="true" OnSorting="gvCrewSearch_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSearch" runat="server"   AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
            CellSpacing="0" GridLines="None" Height="94%" Width="100%"
            GroupingEnabled="false" EnableHeaderContextMenu="true"
            OnNeedDataSource="gvCrewSearch_NeedDataSource"
            OnItemDataBound="gvCrewSearch_ItemDataBound"
            OnItemCommand="gvCrewSearch_ItemCommand"
            AutoGenerateColumns="false">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" CommandItemDisplay="Top">
                <HeaderStyle Width="102px" />
                <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                <ColumnGroups>
                    <telerik:GridColumnGroup HeaderText="Per Day" Name="day" HeaderStyle-HorizontalAlign="Center">
                    </telerik:GridColumnGroup>
                </ColumnGroups>
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
                    <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="true" HeaderStyle-Width="200px" SortExpression="FLDEMPLOYEENAME">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblRankId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                            <asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>' CommandName="GETEMPLOYEE"></asp:LinkButton>
                            <telerik:RadLabel ID="lblName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Rank" AllowSorting="true" HeaderStyle-Width="75px" SortExpression="FLDRANKCODE">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Nationality" HeaderStyle-Width="100px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblNationality" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONALITYNAME")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="File No" HeaderStyle-Width="80px" SortExpression="FLDFILENO" AllowSorting="true">

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Planned Vessel" HeaderStyle-Width="100px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblPlannedVessel" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPLANNEDVESSELNAME").ToString().Length> 20 ? HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDPLANNEDVESSELNAME").ToString()).ToString().Substring(0, 20)+ "..." : HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDPLANNEDVESSELNAME").ToString()) %>'></telerik:RadLabel>
                            <eluc:Tooltip ID="ucToolTipPlannedVessel" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPLANNEDVESSELNAME") %>' TargetControlId="lblPlannedVessel" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Vessel" HeaderStyle-Width="100px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLastVesselName" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLASTVESSELNAME")%>'
                                runat="server">
                            </telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Last Sign-Off" HeaderStyle-Width="80px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLastSignOffDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTSIGNOFFDATE"))%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn ColumnGroupName="day" HeaderText="Last salary" HeaderStyle-Width="50px">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLastSalary" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDAILYRATEUSD")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn ColumnGroupName="day" HeaderText="DP Allow." HeaderStyle-Width="50px">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLastDP" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFLDDPALLOWANCEUSD")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn ColumnGroupName="day" HeaderText="Expected salary" HeaderStyle-Width="50px">
                        <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblexpectedsalary" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEXPECTEDSALARY")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="DOA Given On" HeaderStyle-Width="80px" SortExpression="FLDDOAGIVENDATE" AllowSorting="true">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDOAGiven" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDOAGIVENDATE"))%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="DOA" HeaderStyle-Width="80px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblDOA" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDOA"))%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Last Contact" HeaderStyle-Width="80px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblLastContact" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTCONTACTDATE"))%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Follow Up Date" HeaderStyle-Width="80px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFollowUpDateItem" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDFOLLOWUPDATE"))%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Comments" HeaderStyle-Width="80px">
                        <ItemStyle Wrap="true" HorizontalAlign="center"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblRemarks" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Length>10 ? DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 10) + "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'></telerik:RadLabel>
                            <asp:LinkButton ID="ImgRemarks" runat="server"
                                CommandArgument='<%# Container.DataSetIndex %>'>
                                                <span class="icon"><i class="fas fa-glasses"></i></span>
                            </asp:LinkButton>
                            <eluc:Tooltip ID="ucToolTipRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' TargetControlId="ImgRemarks" />
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Contact Numbers" HeaderStyle-Width="75px">
                        <ItemStyle Wrap="true" HorizontalAlign="center"></ItemStyle>
                        <ItemTemplate>
                            <%--<eluc:CommonToolTip ID="CommonToolTip1" runat="server" Screen='<%# "CrewOffshore/CrewOffshoreToolTipAvailabilityContactnumber.aspx?employeeid=" + DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID").ToString() %>' />--%>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Email ID" HeaderStyle-Width="200px">
                        <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblemailid" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDEMAILID")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Action">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                CommandName="GETEMPLOYEE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                ToolTip="Edit" Visible="false"></asp:ImageButton>

                            <asp:LinkButton ID="imgSuitableCheck" runat="server" CommandName="SUITABILITYCHECK"
                                CommandArgument="<%# Container.DataSetIndex %>"
                                ToolTip="Suitability">
                                                 <span class="icon"><i class="fas  fa-user-astronaut"></i></span>
                            </asp:LinkButton>

                            <asp:LinkButton ID="imgActivity" runat="server" CommandName="ACTIVITY"
                                ToolTip="Activities">
                                                 <span class="icon"><i class="fa fa-pencil-ruler"></i></span>
                            </asp:LinkButton>

                            <asp:LinkButton runat="server" AlternateText="PD Form"
                                ID="cmdPDForm" CommandName="PDFORM" ToolTip="PD Form">
                                                 <span class="icon"><i class="fas fa-file"></i></span>
                            </asp:LinkButton>
                            <asp:ImageButton runat="server" AlternateText="Reverse Mapping" Visible="false"
                                ID="cmdreverse" CommandName="REVERSE" ToolTip="Reverse Mapping" ImageUrl="<%$ PhoenixTheme:images/cargo-ship.png %>"></asp:ImageButton>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                    PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"    />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
            </telerik:RadAjaxPanel>


    </form>
</body>
</html>
