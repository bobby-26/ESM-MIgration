<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportDataExportPDMSNew.aspx.cs" Inherits="Crew_CrewReportDataExportPDMSNew" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Pool" Src="~/UserControls/UserControlPool.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Data Export to PDMS</title>
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

            <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuReportsFilter" runat="server" OnTabStripCommand="ReportsFilter_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>

            <table>
                <tr>
                    <td>
                        <telerik:radlabel ID="lblPool" Text="Pool" runat="server"></telerik:radlabel>
                    </td>
                    <td>
                        <eluc:Pool ID="ucPool" runat="server" AppendDataBoundItems="true"
                            Enabled="false" />
                    </td>

                </tr>
            </table>

            <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand"></eluc:TabStrip>

              <telerik:RadGrid ID="gvCrew" runat="server" AutoGenerateColumns="False"  OnSortCommand="gvCrew_Sorting"
                CellPadding="1" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" OnItemCommand="gvCrew_ItemCommand"
                OnItemDataBound="gvCrew_ItemDataBound" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvCrew_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Auto">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ForeColor="Black"   ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="70px" Wrap="true" />    
                    <Columns>

                    <telerik:gridtemplatecolumn headertext="Global EmpCode">
                        
                        <ItemTemplate>
                            <telerik:radlabel ID="lblEmployeeCode" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEECODE") %>' />
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="Last Name">
                       
                        <ItemTemplate>
                            <telerik:radlabel ID="lblSurname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLASTNAME") %>' />
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="First Name">
                      
                        <ItemTemplate>
                            <telerik:radlabel ID="lblFirstName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFIRSTNAME") %>' />
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="Birth Date">
                       
                        <ItemTemplate>
                            <telerik:radlabel ID="lblDob" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH") %>'></telerik:radlabel>
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="Middle Name">
                       
                        <ItemTemplate>
                            <telerik:radlabel ID="lblMiddleName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMIDDLENAME") %>' />
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="PreTitleCode">
                        
                        <ItemTemplate>
                            <telerik:radlabel ID="lblPreTitle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTITLE") %>'></telerik:radlabel>
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="Hire Date">
                       
                        <ItemTemplate>
                            <telerik:radlabel ID="lblHireDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFJOINING") %>'></telerik:radlabel>
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="Gender Code">
                        
                        <ItemTemplate>
                            <telerik:radlabel ID="lblGenderCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></telerik:radlabel>
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="Nation Code">
                       
                        <ItemTemplate>
                            <telerik:radlabel ID="lblNationality" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONCODE") %>' />
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="Nation Code2">
                       
                        <ItemTemplate>
                            <telerik:radlabel ID="lblNationCode2" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNATIONCODE2") %>'></telerik:radlabel>
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="Home Mobile">
                       
                        <ItemTemplate>
                            <telerik:radlabel ID="lblHomeMobile" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHOMEMOBILE") %>'></telerik:radlabel>
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="Promoted Date">
                       
                        <ItemTemplate>
                            <telerik:radlabel ID="lblHomeCity1" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROMOTIONDATE") %>'></telerik:radlabel>
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="Rank Experience">
                       
                        <ItemTemplate>
                            <telerik:radlabel ID="lblRankExperience" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKEXPERIANCE") %>'></telerik:radlabel>
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="Total Rank Experience">
                      
                        <ItemTemplate>
                            <telerik:radlabel ID="lblTotalRankExperience" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOTALRANKEXPERIENCE") %>'></telerik:radlabel>
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="Rank">
                       
                        <ItemTemplate>
                            <telerik:radlabel ID="lblRankName" Visible="true" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>' />
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="Inactive">
                       
                        <ItemTemplate>
                            <telerik:radlabel ID="lblInactive" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINACTIVE") %>'></telerik:radlabel>
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="Inactive Date">
                       
                        <ItemTemplate>
                            <telerik:radlabel ID="lblInactiveDste" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINACTIVEDATE") %>'></telerik:radlabel>
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="Source">
                        
                        <ItemTemplate>
                            <telerik:radlabel ID="lblSource" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOURCE") %>'></telerik:radlabel>
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="LeaveEndDate">
                        
                        <ItemTemplate>
                            <telerik:radlabel ID="lblLeaveEndDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEAVEENDDATE") %>'></telerik:radlabel>
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="DueOffDate">
                       
                        <ItemTemplate>
                            <telerik:radlabel ID="lblDueOffDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRELIEFDUEDATE") %>'></telerik:radlabel>
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                    <telerik:gridtemplatecolumn headertext="InactiveReason">
                       
                        <ItemTemplate>
                            <telerik:radlabel ID="lblInactiveReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINACTIVEREASON") %>'></telerik:radlabel>
                        </ItemTemplate>
                    </telerik:gridtemplatecolumn>
                </Columns>
                                    
                </MasterTableView>
                <clientsettings enablerowhoverstyle="true" allowcolumnsreorder="true" reordercolumnsonclient="true" allowcolumnhide="true" columnsreordermethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="false" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </clientsettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
