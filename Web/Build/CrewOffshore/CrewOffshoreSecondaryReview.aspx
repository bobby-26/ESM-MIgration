<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreSecondaryReview.aspx.cs"
    Inherits="CrewOffshore_CrewOffshoreSecondaryReview" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Secondary Review</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />


                <eluc:TabStrip ID="CrewQueryMenu" runat="server" OnTabStripCommand="CrewQueryMenu_TabStripCommand"></eluc:TabStrip>


                <%-- <asp:GridView ID="gvCrewSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvCrewSearch_RowCommand" OnRowDataBound="gvCrewSearch_RowDataBound"
                        ShowHeader="true" EnableViewState="false" AllowSorting="true" OnSorting="gvCrewSearch_Sorting">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSearch" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvCrewSearch_NeedDataSource"
                    OnItemCommand="gvCrewSearch_ItemCommand"
                    OnItemDataBound="gvCrewSearch_ItemDataBound"
                    OnSortCommand="gvCrewSearch_SortCommand"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    AutoGenerateColumns="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
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
                        <HeaderStyle Width="102px" />
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                           <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblRankId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKID") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>' CommandName="GETEMPLOYEE"></asp:LinkButton>
                                    <telerik:RadLabel ID="lblName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rank">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                               <HeaderStyle Width="50px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="File No">
                               <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status">
                                <HeaderStyle Width="80px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Planned Vessel">
                                 <HeaderStyle Width="125px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPlannedVessel" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPLANNEDVESSELNAME").ToString().Length> 20 ? HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDPLANNEDVESSELNAME").ToString()).ToString().Substring(0, 20)+ "..." : HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDPLANNEDVESSELNAME").ToString()) %>'></telerik:RadLabel>
                                    <eluc:Tooltip ID="ucToolTipPlannedVessel" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPLANNEDVESSELNAME") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Last Vessel">
                                 <HeaderStyle Width="125px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblLastVesselName" Text='<%# DataBinder.Eval(Container, "DataItem.FLDLASTVESSELNAME")%>'
                                        runat="server">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Present Vessel">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="125px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPresentVesselName" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPRESENTVESSELNAME")%>'
                                        runat="server">
                                    </telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Last Sign-Off Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                              <HeaderStyle Width="75px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblLastSignOffDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTSIGNOFFDATE"))%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Last drawn salary(day)">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                             <HeaderStyle Width="75px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblLastSalary" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDAILYRATEUSD")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="DP Allowance (day)">
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                               <HeaderStyle Width="75px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblLastDP" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDPALLOWANCEUSD")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>

                            <telerik:GridTemplateColumn HeaderText="Last Contact">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                             <HeaderStyle Width="75px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblLastContact" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTCONTACTDATE"))%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                              <HeaderStyle Width="100px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" AlternateText="Edit"
                                        CommandName="GETEMPLOYEE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                        ToolTip="Edit" Visible="false">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                  
                                    <asp:LinkButton ID="imgSuitableCheck" Visible="false" runat="server" CommandName="SUITABILITYCHECK"
                                        CommandArgument="<%# Container.DataSetIndex %>"
                                        ToolTip="Suitability" >
                                        <span class="icon"><i class="fas fa-user-astronaut"></i></span>
                                    </asp:LinkButton>
                                  
                                    <asp:LinkButton ID="imgActivity" runat="server" CommandName="ACTIVITY"
                                        ToolTip="Assess Suitability for re-employment" >
                                        <span class="icon"><i class="fas fa-pencil-ruler"></i></span>
                                    </asp:LinkButton>
                                 
                                    <asp:LinkButton runat="server" AlternateText="PD Form" 
                                        ID="cmdPDForm" CommandName="PDFORM" ToolTip="PD Form" Visible="false">
                                        <span class="icon"><i class="fas fa-file"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>


            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
