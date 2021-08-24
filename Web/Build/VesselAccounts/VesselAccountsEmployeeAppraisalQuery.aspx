<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsEmployeeAppraisalQuery.aspx.cs"
    Inherits="VesselAccountsEmployeeAppraisalQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Vessel Crew Appraisal</title>

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
          <script type="text/javascript">
           function Resize() {
               setTimeout(function () {
                   TelerikGridResize($find("<%= gvCrewSearch.ClientID %>"));
                }, 200);
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
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>      
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />


            <eluc:TabStrip ID="MenuFeedBackHeader" runat="server" OnTabStripCommand="MenuFeedBackHeader_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <br style="clear: both;" />
            <div>
                <div style="color: Blue;">
                    <telerik:RadLabel ID="lbl1Bydefaultitwillshowsonboardcrewmembers" runat="server" Text="1. By default, it will show onboard crew"></telerik:RadLabel>
                    <br />
                    <telerik:RadLabel ID="lbl2Forappraisingsignedoffseafarersuseperiodfilterandclicksearchicon"
                        runat="server" Text="2. For appraising, seafarers who have signed off, use period filter and search">
                    </telerik:RadLabel>
                    <br />
                    <telerik:RadLabel ID="lbl3Forperiodsearchbothfromandtodateismandatory" runat="server"
                        Text=" 3. For period search, both from and to dates are mandatory">
                    </telerik:RadLabel>
                </div>
                <table>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPeriodFrom" runat="server" Text="Period Between"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtFromDate" runat="server" DatePicker="true"></eluc:Date>
                            <eluc:Date ID="txtToDate" runat="server" DatePicker="true"></eluc:Date>
                        </td>
                    </tr>
                </table>
            </div>

            <eluc:TabStrip ID="CrewQueryMenu" runat="server" OnTabStripCommand="CrewQueryMenu_TabStripCommand"></eluc:TabStrip>
      
                <%--  <asp:GridView ID="gvCrewSearch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    GridLines="None" Width="100%" CellPadding="3" OnRowDataBound="gvCrewSearch_RowDataBound"
                    OnRowEditing="gvCrewSearch_RowEditing" ShowHeader="true" EnableViewState="false"
                    AllowSorting="true" OnSorting="gvCrewSearch_Sorting" OnRowCommand="gvCrewSearch_RowCommand">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />--%>
                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSearch" runat="server" Height="400px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        OnNeedDataSource="gvCrewSearch_NeedDataSource"
                        OnSortCommand="gvCrewSearch_SortCommand"
                        OnItemCommand="gvCrewSearch_ItemCommand"
                        OnItemDataBound="gvCrewSearch_ItemDataBound"
                        OnEditCommand="gvCrewSearch_EditCommand"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
                            <HeaderStyle Width="102px" />
                            <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
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
                                <telerik:GridTemplateColumn HeaderText="File No.">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDFILENO"]%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name" AllowFiltering="true" SortExpression="FLDNAME">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"]%>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lnkEployeeName" runat="server" CommandArgument="<%#Container.DataSetIndex%>"
                                            Text='<%# ((DataRowView)Container.DataItem)["FLDNAME"]%>' CommandName="EDIT"></asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rank" SortExpression="FLDSIGNONRANKNAME" AllowFiltering="true">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDSIGNONRANKNAME"]%>
                                        <telerik:RadLabel ID="lblRank" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSIGNONRANKID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblSignonOffId" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSIGNONOFFID") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                        <telerik:RadLabel ID="lblShowxl" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDSHOWXL") %>'
                                            Visible="false">
                                        </telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Sign On" AllowSorting="true" SortExpression="FLDSIGNONDATE">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDSIGNONDATE", "{0:dd/MMM/yyyy}"))%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Relief Due" AllowSorting="true" SortExpression="FLDRELIEFDUEDATE">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDRELIEFDUEDATE", "{0:dd/MMM/yyyy}"))%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                 
                                    <ItemTemplate>
                                        <asp:ImageButton runat="server" AlternateText="Export XL" ImageUrl="<%$ PhoenixTheme:images/document_view.png %>"
                                            CommandName="CREWEXPORT2XL" CommandArgument='<%# Container.DataSetIndex %>'
                                            ID="cmdCrewExport2XL" ToolTip="OC 28 Assess"></asp:ImageButton>
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
                </telerik:RadAjaxPanel>
         

 

    </form>
</body>
</html>
