<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreReliever.aspx.cs" Inherits="CrewOffshoreReliever" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Query Activity</title>
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


                <eluc:TabStrip ID="CrewRelieverTabs" runat="server" Title="Reliever" OnTabStripCommand="CrewRelieverTabs_TabStripCommand"
                    TabStrip="true"></eluc:TabStrip>

                <div id="divTable" runat="server">
                    <table id="tblMatrix" runat="server" width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblMatrix" runat="server" Text="Training Matrix"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox ID="ddlTrainingMatrix" AutoPostBack="true" runat="server" Width="255px"  AppendDataBoundItems="true" Filter="Contains" MarkFirstMatch="true" EmptyMessage="Type to select"></telerik:RadComboBox>
                            </td>
                        </tr>
                    </table>
                </div>

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
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                              <HeaderStyle Width="150px" />
                                <itemtemplate>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>' CommandName="GETEMPLOYEE"></asp:LinkButton>
                                <telerik:RadLabel ID="lblEmpName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Rank">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                             <HeaderStyle Width="100px" />
                                <itemtemplate>
                                <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>'></telerik:RadLabel>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="File No">
                              <HeaderStyle Width="100px" />
                                <itemtemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <HeaderStyle Width="150px" />
                                <itemtemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Planned Vessel">
                                 <HeaderStyle Width="150px" />
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                <telerik:RadLabel ID="lblPlannedVessel" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPLANNEDVESSELNAME").ToString().Length> 20 ? HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDPLANNEDVESSELNAME").ToString()).ToString().Substring(0, 20)+ "..." : HttpContext.Current.Server.HtmlDecode(DataBinder.Eval(Container, "DataItem.FLDPLANNEDVESSELNAME").ToString()) %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipPlannedVessel" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPLANNEDVESSELNAME") %>' />
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Last Vessel">
                                 <HeaderStyle Width="125px" />
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <headertemplate>
                                <telerik:RadLabel ID="lblLastVessel" runat="server" Text="Last Vessel"></telerik:RadLabel>
                            </headertemplate>
                                <itemtemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDLASTVESSELNAME")%>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Present Vessel">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                               <HeaderStyle Width="125px" />
                                <itemtemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDPRESENTVESSELNAME")%>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Last Sign-Off Date">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                               <HeaderStyle Width="75px" />
                                <itemtemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDLASTSIGNOFFDATE"))%>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="DOA">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <HeaderStyle Width="75px" />
                                <itemtemplate>
                                <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDDOA"))%>
                            </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Action">  
                                <headerstyle horizontalalign="Center" verticalalign="Middle"></headerstyle>
                                <HeaderStyle Width="50px" />
                                <itemstyle wrap="False" horizontalalign="Center" width="100px"></itemstyle>
                                <itemtemplate>
                                <asp:LinkButton runat="server" AlternateText="Delete" 
                                    CommandName="GETEMPLOYEE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                    ToolTip="Edit" Visible="false">
                                     <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                               
                                <asp:LinkButton ID="imgActivity" runat="server" CommandName="ACTIVITY"
                                    ToolTip="Activities">
                                    <span class="icon"><i class="fas fa-pencil-ruler"></i></span>
                                </asp:LinkButton>
                             
                                <asp:LinkButton ID="imgSuitableCheck" runat="server" CommandName="SUITABILITYCHECK" CommandArgument="<%# Container.DataSetIndex %>"
                                    ToolTip="Suitability Check" >
                                    <span class="icon"><i class="fas fa-user-astronaut"></i></span>
                                </asp:LinkButton>
                            </itemtemplate>
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
