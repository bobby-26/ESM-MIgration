<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshorePendingWaivingRequest.aspx.cs"
    Inherits="CrewOffshorePendingWaivingRequest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pending Waiving Request</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

                <eluc:TabStrip ID="CrewQueryMenu" runat="server" OnTabStripCommand="CrewQueryMenu_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <%-- <asp:GridView ID="gvCrewSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowDataBound="gvCrewSearch_RowDataBound" OnRowEditing="gvCrewSearch_RowEditing"
                        ShowHeader="true" EnableViewState="false" AllowSorting="true" OnSorting="gvCrewSearch_Sorting"
                        OnRowCommand="gvCrewSearch_RowCommand">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSearch" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvCrewSearch_NeedDataSource"
                        OnItemCommand="gvCrewSearch_ItemCommand"
                        OnItemDataBound="gvCrewSearch_ItemDataBound"
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

                                <telerik:GridTemplateColumn HeaderText="File No">
                                    <HeaderStyle Width="10%" />
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDFILENO"]%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name">
                                    <HeaderStyle Width="30%" />
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                 
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"]%>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lnkEmployeeName" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>' CommandName="GETEMPLOYEE"></asp:LinkButton>
                                    <telerik:RadLabel ID="lblName" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME")%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Planned Vessel">
                                     <HeaderStyle Width="30%" />
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELNAME"]%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblrankid" Visible="false" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDRANKID"]%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblvesselid" Visible="false" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDVESSELID"]%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblmatrixid" Visible="false" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDTRAININGMATRIXID"]%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lbljoiningdate" Visible="false" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDEXPECTEDJOINDATE")) %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblcrewplanid" Visible="false" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCREWPLANID"]%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Planned Rank">
                                     <HeaderStyle Width="10%" />
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                    <headertemplate>
                                    <telerik:RadLabel ID="lblRankHeader" runat="server" Text="Planned Rank"></telerik:RadLabel>
                                </headertemplate>
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblSignOnRank" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDRANKCODE"]%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="No Of Waiver Requested">
                                     <HeaderStyle Width="10%" />
                                    <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                   
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblNoOfwaive" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDCOUNT"]%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                             
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <headerstyle horizontalalign="Center" verticalalign="Middle"></headerstyle>
                                 
                                    <itemstyle wrap="False" horizontalalign="Center" width="100px"></itemstyle>
                                    <itemtemplate>
                                   
                                    <asp:LinkButton runat="server" CommandArgument="<%# Container.DataSetIndex %>"
                                        CommandName="DETAIL" ID="cmdDetail" 
                                        ImageAlign="AbsMiddle" Text=".." ToolTip="Waived detail" >
                                        <span class ="icon"><i class="fas fa-tasks"></i></span>
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
                
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
