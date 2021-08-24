<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportMISRecruitmentAnalysisCompany.aspx.cs"
    Inherits="Crew_CrewReportMISRecruitmentAnalysisCompany" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>MIS Recruitment Report Company</title>
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
             <b> <eluc:Title runat="server" ID="ucTitle" Text="" ShowMenu="false"></eluc:Title> </b>
                        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
                              <telerik:RadGrid ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvCrew_ItemCommand"
                OnItemDataBound="gvCrew_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvCrew_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ForeColor="Black"   ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>

                            <telerik:gridtemplatecolumn HeaderText="S.No">
                              
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblSrNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                            <telerik:gridtemplatecolumn HeaderText="Company Name">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblCompanyName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                           <telerik:gridtemplatecolumn HeaderText="Emp No">
                               
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblEmpFileNo" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                           <telerik:gridtemplatecolumn HeaderText="Batch">
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblBatch" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
                        <telerik:gridtemplatecolumn HeaderText="Emp Name">
                                
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblEmpNo" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>' />
                                    <asp:LinkButton ID="lnkEmpName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>' />
                                </ItemTemplate>
                            </telerik:gridtemplatecolumn>
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
                         <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>