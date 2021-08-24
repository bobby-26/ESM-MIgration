<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewRetentionReport.aspx.cs" Inherits="Crew_CrewRetentionReport" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvCrew.ClientID %>"));
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
        <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div hidden="hidden">
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            </div>
            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>

            <eluc:TabStrip ID="CrewRetentionMenu" runat="server" OnTabStripCommand="CrewRetentionMenu_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvCrew_ItemCommand"
                OnItemDataBound="gvCrew_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvCrew_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnSorting="gvCrew_Sorting">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ForeColor="Black" ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                       <telerik:GridTemplateColumn HeaderText="File No.">
                            <itemtemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>
                                    </itemtemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Name">

                            <itemtemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>
                                    </itemtemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Rank">

                            <itemtemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDRANKCODE") %>
                                    </itemtemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Vessel">

                            <itemtemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>
                                    </itemtemplate>
                        </telerik:GridTemplateColumn>
                          <telerik:GridTemplateColumn HeaderText="Vessel Type">

                            <itemtemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDVESSELTYPENAME") %>
                                    </itemtemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Principal">

                            <itemtemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDPRINCIPALNAME") %>
                                    </itemtemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Sign On">

                            <itemtemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE", "{0:dd/MMM/yyyy}") %>
                                    </itemtemplate>
                        </telerik:GridTemplateColumn>
                      <telerik:GridTemplateColumn HeaderText="Sign Off">

                            <itemtemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE", "{0:dd/MMM/yyyy}") %>
                                    </itemtemplate>
                        </telerik:GridTemplateColumn>
                       <telerik:GridTemplateColumn HeaderText="Contract Days">

                            <itemtemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDCONTRACTEDDAYS") %>
                                    </itemtemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>

                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
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
