<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportBatchWiseQueryEmployeeList.aspx.cs"
    Inherits="Crew_CrewReportBatchWiseQueryEmployeeList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
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
                    TelerikGridResize($find("<%= gvBatch.ClientID %>"));
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
         
                    <table width="100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCourse" runat="server" Text="Course"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCourse" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                                    Width="300px"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
               
                    <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
             
             <telerik:RadGrid ID="gvBatch" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvBatch_ItemCommand"
                OnItemDataBound="gvBatch_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvBatch_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
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
                    <Columns>
                        
                        <telerik:GridTemplateColumn  HeaderText="Sr.No"  >
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSrNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Batch"  >
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblBatch" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCH") %>' />
                                    <telerik:RadLabel ID="lblBatchId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDBATCHID") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Emp Code"  >
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Emp Name"  >
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblEmpName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                             <telerik:GridTemplateColumn     HeaderText="O'all Rank"  >
                                
                                 <ItemTemplate>
                                    <telerik:RadLabel ID="lblOAllRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOALLRANK") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Cert Issue Date"  >
                           
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblCertIssueDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDCERTDATE")) %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Present Rank"  >

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Status"  >
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblStatusHeader" runat="server">Status</telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="First Join Date"  >
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFstJoinDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDDATEOFJOINING")) %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Last Vessel"  >
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lbllstVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTVESSELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Sign Off Date"  >
                               
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSignOffDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNOFFDATE")) %>' />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Present Vessel"  >
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPresentVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESENTVESSELNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn  HeaderText="Sign On Date"  > 
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblSignOnDate" runat="server" Text='<%# General.GetDateTimeToString(DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE")) %>' />
                                </ItemTemplate>
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